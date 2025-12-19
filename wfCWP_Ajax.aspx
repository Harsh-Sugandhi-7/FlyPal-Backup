<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWP_Ajax.aspx.vb" Inherits="Flypal.wfCWP_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>CWP Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="5400">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server"></uc2:MSGBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        window.onload = blinknow;
        function blinknow() {
            var e = document.getElementById("<%=lblStatus.ClientID%>");

            e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
            setTimeout("blinknow();", 750);
        }
        
    </script>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">CWP Detail</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                            Width="100%" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtVisitNo" OnServerValidate="CustomValidate" Display="None"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlCWPDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td valign="top">
                                                                <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                    <legend><b>Detail</b></legend>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Label4" class="clsLabelStar">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblCWPDate" class="clsLabelAuto">Date</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCWPDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'txtCWPDate_CalendarExtender','true');"
                                                                                    Width="100px"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtCWPDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCWPDate">
                                                                                </cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender ID="txtCWPDate_Watermarkextender" runat="server" TargetControlID="txtCWPDate"
                                                                                    WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                                </cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span class="clsLabelStar">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <span class="clsLabelAuto">Visit No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtVisitNo" runat="server" CssClass="clsTextBoxSmall_Ajax" Text="<%# mCWP.VisitNo %>"
                                                                                                Enabled="<%# mCWP.VisitNo = 0 %>" Width="40px" ToolTip="Enter No." MaxLength="4"
                                                                                                ClientIDMode="Static"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="Span6" class="clsLabelAuto">Rev. Status</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRevStatus" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.RevStatus %>"
                                                                                                Width="140px" ToolTip="Enter Rev. Status"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right" colspan="2">
                                                                                <asp:UpdatePanel ID="upnlStatusHeader" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblStatus" runat="server" Font-Italic="true" Font-Size="11pt" CssClass="clsLabelHeader"
                                                                                            Text="<%# mCWP.StatusName %>"></asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Span2" class="clsLabelStar">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span1" class="clsLabelAuto">Text</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtText" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                                    Enabled="<%# mCWP.IsNew %>" CssClass="clsTextBox_Ajax" Width="140px" Text="<%# mCWP.CWPText %>"
                                                                                    ToolTip="Enter Text"></asp:TextBox>
                                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxSmall_Ajax" Text="<%# mCWP.CWPNo %>"
                                                                                    Enabled="<%# mCWP.IsNew %>" Width="40px" ToolTip="Enter No." MaxLength="4"></asp:TextBox>
                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" CompletionInterval="1"
                                                                                    ServicePath="wfCWP_Ajax.aspx" ServiceMethod="GetTextList" TargetControlID="txtText"
                                                                                    UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                    OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                    OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                </cc2:AutoCompleteExtender>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span7" class="clsLabelAuto">Shop W/O No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtShopWONo" runat="server" CssClass="clsTextBox_Ajax" Width="140px"
                                                                                                Text="<%# mCWP.ShopWONo %>" ToolTip="Enter Shop W/O No."></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtShopWODate" runat="server" ReadOnly="true" CssClass="clsTextBoxDate_Ajax"
                                                                                                BackColor="Gainsboro" Enabled="false" Text="<%# mCWP.ShopWODate %>" ToolTip="Shop W/O Date"
                                                                                                Width="100px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span12" class="clsLabelAuto">Turn Around Time</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTurnAroundTime" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                                    MaxLength="4" ClientIDMode="Static" Text="<%# mCWP.TurnAroundTime %>" ToolTip="Enter Turn Around Time"></asp:TextBox>
                                                                                <span id="Span23" class="clsLabelHeader">In Days</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Span3" class="clsLabelStar">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span4" class="clsLabelAuto">WorkShop</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbWorkShop" runat="server" CssClass="clsComboBox2_Ajax" Width="250px"
                                                                                    AutoPostBack="true" DataTextField="LocationWorkShop" DataValueField="ID" SelectedValue="<%# mCWP.WorkShopID %>">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span5" class="clsLabelAuto">CMM/OHM Used</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCMMOHMUsed" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.CMMOHMUsed %>"
                                                                                    ToolTip="Enter CHM/OHM Reference"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                                <legend><b>Removal Data</b></legend>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span13" class="clsLabelAuto">A/C Regn.</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.RegNo %>"
                                                                                                Width="100px" ToolTip="Enter Aircraft Reg No."></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span15" class="clsLabelAuto">A/F Serial No.</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtAirframeSrNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.NHASerialNo %>"
                                                                                                Width="100px" ToolTip="Enter Airframe Serial No."></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span16" class="clsLabelAuto">Station</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:UpdatePanel ID="upnlStation" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:TextBox ID="txtStation" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.Station %>"
                                                                                                        Width="100px" ToolTip="Enter Station"></asp:TextBox>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span17" class="clsLabelAuto">Rem. Date</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRemDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'txtRemDate_CalendarExtender','true');"
                                                                                                Width="100px" ToolTip="Enter Comp. Removal Date"></asp:TextBox>
                                                                                            <cc2:CalendarExtender ID="txtRemDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRemDate">
                                                                                            </cc2:CalendarExtender>
                                                                                            <cc2:TextBoxWatermarkExtender ID="txtRemDate_Watermarkextender" runat="server" TargetControlID="txtRemDate"
                                                                                                WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                                            </cc2:TextBoxWatermarkExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span11" class="clsLabelAuto">Removal Reason</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRemovalReason" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.RemovalReason %>"
                                                                                                ToolTip="Enter Removal Reason"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                                <legend><b>Component Data</b></legend>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span18" class="clsLabelAuto">Part No.</span>
                                                                                        </td>
                                                                                        <td colspan="3">
                                                                                            <asp:TextBox ID="txtPartName" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.PartNo %>"
                                                                                                ReadOnly="true" BackColor="Gainsboro" ToolTip="Enter Part Name"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span19" class="clsLabelAuto">Description</span>
                                                                                        </td>
                                                                                        <td colspan="3">
                                                                                            <asp:TextBox ID="txtPartDescription" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.PartDescription %>"
                                                                                                ReadOnly="true" BackColor="Gainsboro" TextMode="MultiLine" ToolTip="Enter Part Description"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span20" class="clsLabelAuto">Serial No.</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtCompSerialNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.SerialNo %>"
                                                                                                ReadOnly="true" BackColor="Gainsboro" Width="100px" ToolTip="Enter Comp. Serial No."></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="Span14" class="clsLabelAuto">Posi.</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPartPosition" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.Position %>"
                                                                                                Width="33px" ToolTip="Enter Part Position"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span21" class="clsLabelAuto">TSO/CSO/LSO</span>
                                                                                        </td>
                                                                                        <td colspan="3">
                                                                                            <asp:TextBox ID="txtTSOCSOLSO" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.TSOCSOLSO %>"
                                                                                                Width="100px" ToolTip="Enter TSO/CSO/LSO Values"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span22" class="clsLabelAuto">TSC/CSC/LSC</span>
                                                                                        </td>
                                                                                        <td colspan="3">
                                                                                            <asp:TextBox ID="txtTSCCSCLSC" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.TSCCSCLSC %>"
                                                                                                Width="100px" ToolTip="Enter TSC/CSC/LSC Values"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                                <legend><b>Customer Info.</b></legend>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span8" class="clsLabelAuto">Customer</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mCWP.CustomerID %>">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span9" class="clsLabelAuto">Cust. W/O No.</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtCustWONo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.CustomerWONo %>"
                                                                                                Width="100px" ToolTip="Enter Customer W/O No."></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="Span10" class="clsLabelAuto">Tag No.</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtTagNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCWP.TagNo %>"
                                                                                                Width="100px" ToolTip="Enter Tag No."></asp:TextBox>
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
                                                            <td valign="top">
                                                                <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                    <legend><b>Bill Of Work</b></legend>
                                                                    <table style="margin-bottom: 0px">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span class="clsLabelHeader">Visual Inspection</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span class="clsLabelHeader">Initial Test(If Required)</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span class="clsLabelHeader">Start Date</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtVisualInspection" runat="server" CssClass="clsTextBox1_Ajax"
                                                                                                Text="<%# mCWP.VisualInspectionDesc %>" ToolTip="Enter Visual Inspection description"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtInitialTest" runat="server" CssClass="clsTextBox1_Ajax" Text="<%# mCWP.PerformInitialTestDesc %>"
                                                                                                ToolTip="Enter Initial Test description"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtCWPStartDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'txtCWPStartDate_CalendarExtender','false');"
                                                                                                Width="100px" ToolTip="Enter CWP Start Date"></asp:TextBox>
                                                                                            <cc2:CalendarExtender ID="txtCWPStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCWPStartDate">
                                                                                            </cc2:CalendarExtender>
                                                                                            <cc2:TextBoxWatermarkExtender ID="txtCWPStartDate_Watermarkextender" runat="server"
                                                                                                TargetControlID="txtCWPStartDate" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                                WatermarkCssClass="clsDateTextBox">
                                                                                            </cc2:TextBoxWatermarkExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span class="clsLabelHeader">Shop Findings</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span class="clsLabelHeader">Autho.</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtShopFindings" runat="server" CssClass="clsTextBoxLong1_Ajax"
                                                                                                Width="747px" Height="35px" Text="<%# mCWP.ShopFindings %>" ToolTip="Enter Shop Findings"
                                                                                                TextMode="MultiLine"></asp:TextBox>
                                                                                        </td>
                                                                                        <td valign="top">
                                                                                            <asp:TextBox ID="txtBillOfWorkLicenceNo" runat="server" CssClass="clsTextBox_Ajax"
                                                                                                OnTextChanged="txtBillOfWorkLicenceNo_TextChanged" AutoPostBack="true" MaxLength="200"
                                                                                                ToolTip="Enter Bill Of Work License No."></asp:TextBox>
                                                                                            <cc2:AutoCompleteExtender ID="txtBillOfWorkLicenceNo_Autocomplete" runat="server"
                                                                                                CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                                EnableCaching="true" TargetControlID="txtBillOfWorkLicenceNo">
                                                                                            </cc2:AutoCompleteExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                    <legend><b>Recommendation</b></legend>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <span class="clsLabelHeader">Autho.</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtRecommendation" runat="server" CssClass="clsTextBox1_Ajax" ToolTip="Enter Recommendation"
                                                                                    Width="747px" Text="<%# mCWP.Recommendation %>" Height="35px" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtRecommendationLicenceNo" runat="server" CssClass="clsTextBox_Ajax"
                                                                                    OnTextChanged="txtRecommendationLicenceNo_TextChanged" AutoPostBack="true" MaxLength="200"
                                                                                    ToolTip="Enter Recommendation License No."></asp:TextBox>
                                                                                <cc2:AutoCompleteExtender ID="txtRecommendationLicenceNo_Autocomplete" runat="server"
                                                                                    CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                    CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                    EnableCaching="true" TargetControlID="txtRecommendationLicenceNo">
                                                                                </cc2:AutoCompleteExtender>
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
                                                <td valign="top">
                                                    <asp:UpdatePanel ID="upnlCWPTaskSheet" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                <legend><b>Task Sheet</b></legend>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="dgCWPTaskSheet" runat="server" CssClass="clsGrid" ShowHeaderWhenEmpty="true"
                                                                                AllowPaging="true" PageSize="6" AutoGenerateColumns="False">
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle HorizontalAlign="Right" />
                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                <AlternatingRowStyle CssClass="alt" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="CWPFunctionName" HeaderText="Function">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Tech.License No." HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtTechLicenceNo" runat="server" CssClass="clsTextBoxTextSearch_Ajax"
                                                                                                OnTextChanged="txtTechLicenceNo_TextChanged" AutoPostBack="true" MaxLength="200"
                                                                                                ToolTip="Enter Tech. License No."></asp:TextBox>
                                                                                            <cc2:AutoCompleteExtender ID="txtTechLicenceNo_Autocomplete" runat="server" CompletionInterval="1"
                                                                                                CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                                EnableCaching="true" TargetControlID="txtTechLicenceNo">
                                                                                            </cc2:AutoCompleteExtender>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="TechEmpName" HeaderText="Tech.Emp.">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="TechLicenseNo" HeaderText="Tech.License No.">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Eng.License No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtEngLicenceNo" runat="server" CssClass="clsTextBoxTextSearch_Ajax"
                                                                                                OnTextChanged="txtEngLicenceNo_TextChanged" AutoPostBack="true" MaxLength="200"
                                                                                                ToolTip="Enter Eng. License No."></asp:TextBox>
                                                                                            <cc2:AutoCompleteExtender ID="txtEngLicenceNo_Autocomplete" runat="server" CompletionInterval="1"
                                                                                                CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                                EnableCaching="true" TargetControlID="txtEngLicenceNo">
                                                                                            </cc2:AutoCompleteExtender>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="EngEmpName" HeaderText="Eng.Emp.">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="EngLicenseNo" HeaderText="Eng.License No.">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                CausesValidation="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                CausesValidation="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:ImageButton ID="imgAddTaskSheet" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Visible='<%# not AppSettings("ClientCode")="BA" %>' Height="22px" Width="24px"
                                                                                ToolTip="Click to Add New TaskSheet" CausesValidation="true"></asp:ImageButton>
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
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Task Performed As Per Recommendation</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtTaskPerformed" runat="server" CssClass="clsTextBox1_Ajax" ToolTip="Enter Task Performed as per Recommendation"
                                                                        Width="747px" Text="<%# mCWP.TaskPerformed %>" Height="45px" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="clsLabelHeader">Tech.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTaskPerformedTechLicenceNo" runat="server" CssClass="clsTextBox_Ajax"
                                                                                    OnTextChanged="txtTaskPerformedTechLicenceNo_TextChanged" AutoPostBack="true"
                                                                                    MaxLength="200" ToolTip="Enter Task Performed Tech License No."></asp:TextBox>
                                                                                <cc2:AutoCompleteExtender ID="txtTaskPerformedTechLicenceNo_Autocomplete" runat="server"
                                                                                    CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                    CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                    EnableCaching="true" TargetControlID="txtTaskPerformedTechLicenceNo">
                                                                                </cc2:AutoCompleteExtender>
                                                                            </td>
                                                                            <tr>
                                                                                <td>
                                                                                    <span class="clsLabelHeader">Eng.</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtTaskPerformedEngLicenceNo" runat="server" CssClass="clsTextBox_Ajax"
                                                                                        OnTextChanged="txtTaskPerformedEngLicenceNo_TextChanged" AutoPostBack="true"
                                                                                        MaxLength="200" ToolTip="Enter Bill Of Work License No."></asp:TextBox>
                                                                                    <cc2:AutoCompleteExtender ID="txtTaskPerformedEngLicenceNo_Autocomplete" runat="server"
                                                                                        CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                        CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                        EnableCaching="true" TargetControlID="txtTaskPerformedEngLicenceNo">
                                                                                    </cc2:AutoCompleteExtender>
                                                                                </td>
                                                                            </tr>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:UpdatePanel ID="upnlCWPInspection" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                <legend><b>Inspection Sheet</b></legend>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="dgCWPInspection" runat="server" CssClass="clsGrid" ShowHeaderWhenEmpty="true"
                                                                                AllowPaging="true" PageSize="5" AutoGenerateColumns="False">
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle HorizontalAlign="Right" />
                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                <AlternatingRowStyle CssClass="alt" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Defect" HeaderText="Defect/Work Required">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" Width="250px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="InspSheetDefectEngEmpLicenseNo" HeaderText="Eng.">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="WorkDone" HeaderText="Work Done">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" Width="250px"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="TechEmpLicenseNo" HeaderText="Tech.">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="EngEmpLicenseNo" HeaderText="Eng.">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                CausesValidation="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                CausesValidation="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:ImageButton ID="imgAddInspection" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Inspection" CausesValidation="true">
                                                                            </asp:ImageButton>
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
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                <legend><b>Final Test Report</b></legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFinalTestReport" runat="server" CssClass="clsTextBox1_Ajax" ToolTip="Enter Final Test Report"
                                                                                Width="747px" Text="<%# mCWP.FinalTestReport %>" TextMode="MultiLine" Height="45px"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="clsLabelHeader">Tech.</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFinalTestReportTechLicenceNo" runat="server" CssClass="clsTextBox_Ajax"
                                                                                            OnTextChanged="txtFinalTestReportTechLicenceNo_TextChanged" AutoPostBack="true"
                                                                                            MaxLength="200" ToolTip="Enter Final Test Report Tech. License No."></asp:TextBox>
                                                                                        <cc2:AutoCompleteExtender ID="txtFinalTestReportTechLicenceNo_Autocomplete" runat="server"
                                                                                            CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                            CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                            EnableCaching="true" TargetControlID="txtFinalTestReportTechLicenceNo">
                                                                                        </cc2:AutoCompleteExtender>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="clsLabelHeader">Eng.</span>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtFinalTestReportEngLicenceNo" runat="server" CssClass="clsTextBox_Ajax"
                                                                                                OnTextChanged="txtFinalTestReportEngLicenceNo_TextChanged" AutoPostBack="true"
                                                                                                MaxLength="200" ToolTip="Enter Final Test Report Eng. License No."></asp:TextBox>
                                                                                            <cc2:AutoCompleteExtender ID="txtFinalTestReportEngLicenceNo_Autocomplete" runat="server"
                                                                                                CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                                EnableCaching="true" TargetControlID="txtFinalTestReportEngLicenceNo">
                                                                                            </cc2:AutoCompleteExtender>
                                                                                        </td>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Sub-Assemblies/Parts Replaced</b></legend>
                                                        <asp:UpdatePanel ID="upnlCWPComponent" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="dgCWPComponent" runat="server" CssClass="clsGrid" ShowHeaderWhenEmpty="true"
                                                                                AllowPaging="true" PageSize="5" AutoGenerateColumns="false">
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle HorizontalAlign="Right" />
                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                <AlternatingRowStyle CssClass="alt" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Description" HeaderText="Part Description">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="OffSerialNo" HeaderText="Off. S. No.">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="OnSerialNo" HeaderText="On S. No.">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N.No.">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N.Date">
                                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="TechEmpLicenseNo" HeaderText="Tech.">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="EngEmpLicenseNo" HeaderText="Eng.">
                                                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                CausesValidation="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                CausesValidation="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:ImageButton ID="imgAddComponent" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Sub-Assemblies/Parts" CausesValidation="true">
                                                                            </asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 50%">
                                            <asp:UpdatePanel ID="upnlInMod" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Incoming Mod Status</b></legend>
                                                        <asp:TextBox ID="txtIncomingModStatus" runat="server" CssClass="clsTextBox1_Ajax"
                                                            Text="<%# mCWP.IncomingModStatus %>" ToolTip="Enter Incoming Mod Status"></asp:TextBox>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="width: 50%">
                                            <asp:UpdatePanel ID="upnlOutMod" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Outgoing Mod Status</b></legend>
                                                        <asp:TextBox ID="txtOutgoingModStatus" runat="server" CssClass="clsTextBox1_Ajax"
                                                            Text="<%# mCWP.OutgoingModStatus %>" ToolTip="Enter Outgoing Mod Status"></asp:TextBox>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 50%">
                                            <asp:UpdatePanel ID="upnlOutgoing" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Out Going Mod Information</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="clsLabelAuto">LRU Control No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLRUControlNo" runat="server" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        Text="<%# mCWP.LRUControlNo %>" ToolTip="Enter LRU Control No."></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span class="clsLabelAuto">R.N.No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRNNo" runat="server" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        Text="<%# mCWP.RNNo %>" ToolTip="Enter Release Note No."></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="clsLabelAuto">Form 1 No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtForm1No" runat="server" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        Text="<%# mCWP.Form1No %>" ToolTip="Enter Form 1 No."></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span24" class="clsLabelAuto">Part No.</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtPartNoCopy" runat="server" CssClass="clsTextBox_Ajax" Text='<%# iif( mCWP.IsNew, mCWP.PartNo,mCWP.CompPartNo) %>'
                                                                        ToolTip="Enter Part Name"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td valign="top" style="width: 50%">
                                            <asp:UpdatePanel ID="upnlCompletion" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Completion Info.</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="clsLabelAuto">Completion Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCWPEndDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'txtCWPEndDate_CalendarExtender','false');"
                                                                        Width="100px" ToolTip="Enter Completion Date" AutoPostBack="true"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtCWPEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCWPEndDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtCWPEndDate_Watermarkextender" runat="server"
                                                                        TargetControlID="txtCWPEndDate" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="clsLabelAuto">Staff Name</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCRSEmployeeList" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                        AutoPostBack="true" DataTextField="EmpNoName" DataValueField="ID" SelectedValue="<%# mCWP.CRSEmployeeID %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="clsLabelAuto">License No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCRSLicenseNo" runat="server" CssClass="clsComboBox_Ajax"
                                                                        AutoPostBack="true" DataTextField="LicenseNo" DataValueField="LicenseNo">
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
                                        <td valign="top" style="width: 50%">
                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                <legend><b>File Attachments</b></legend>
                                                <asp:UpdatePanel ID="upnlCWPAttachment" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="height: 15px">
                                                                    <asp:UpdatePanel ID="upnldgCWPAttachment" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="dgCWPAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                                CssClass="clsGrid" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                                AllowPaging="False" AutoGenerateColumns="false">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                <Columns>
                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                    <asp:BoundField Visible="False" DataField="CWPID" HeaderText="CWPID"></asp:BoundField>
                                                                                    <asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="File Name">
                                                                                        <HeaderStyle Width="350px" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
                                                                                                ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>'
                                                                                                Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                                Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                CausesValidation="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
                                                                        Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="true">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </fieldset>
                                        </td>
                                        <td valign="top" style="width: 50%">
                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                <legend><b>Status</b></legend>
                                                <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgStatusList" runat="server" CssClass="clsGrid" ShowHeaderWhenEmpty="true"
                                                            AllowPaging="true" PageSize="5" AutoGenerateColumns="false">
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="StatusDateFormatted" HeaderText="Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="StatusName" HeaderText="Status">
                                                                    <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UserName" HeaderText="User">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Width="250px"></ItemStyle>
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to Save Transaction"
                                                        Visible="<%# mCWP.StatusID <> 5 %>" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSubmit" runat="server" CssClass="clsButton_Ajax" Text="Submit"
                                                        Visible="<%# mCWP.StatusID = 1 %>" ToolTip="Click to Submit/Authorize Transaction" />
                                                    <asp:Button ID="btnStart" runat="server" CssClass="clsButton_Ajax" Text="Start" ToolTip="Click to Start"
                                                        Visible="<%# mCWP.StatusID = 2 %>" />
                                                    <asp:Button ID="btnOnHold" runat="server" CssClass="clsButton_Ajax" Text="On Hold"
                                                        Visible="<%# mCWP.StatusID = 3 %>" ToolTip="Click to hold" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnComplete" runat="server" CssClass="clsButton_Ajax" Text="Complete"
                                                        Visible="<%# mCWP.StatusID = 3 %>" ToolTip="Click to Complete CWP" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the CWP"
                                                        Enabled="<%# not mCWP.IsNew %>" Text="Print Package" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrintForm" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the CA FORM 1"
                                                        Enabled="<%# mCWP.StatusID = 5 %>" Text="Print CA FORM 1" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to Close screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnInspection" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnCWPComp" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;" />
                                        <asp:Button ID="hdnimgBtnCWPStatusChild" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;" />
                                        <asp:Button ID="hdnimgBtnTaskSheet" ClientIDMode="Static" runat="server" Text="----"
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" DynamicLayout="false" runat="server">
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
            $("#btnSelectFiles").live("click", function () {
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
    <!-- Inspection Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyInspection" Text="Inspection" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlInspection" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeInspection" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupInspection" runat="server" TargetControlID="btnDummyInspection"
        PopupControlID="pnlInspection" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameInspectionStateComplete() {
            $("#btnDummyInspection").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenInspectionWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeInspection").attr("src", "wfCWPInspection_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyInspection").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForInspection() {
            var Inspectionwindow = $find("<%=mdlPopupInspection.ClientID %>");
            //close Inspection popup window
            Inspectionwindow.hide();
            //           release resources
            $("#IframeInspection").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnInspection").click();
        }
    </script>
    <!-- End-->
    <!-- CWP Comp Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCWPComp" Text="CWP Comp" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCWPComp" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCWPComp" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCWPComp" runat="server" TargetControlID="btnDummyCWPComp"
        PopupControlID="pnlCWPComp" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCWPCompStateComplete() {
            $("#btnDummyCWPComp").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCWPCompWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCWPComp").attr("src", "wfCWPComp_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCWPComp").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCWPComp() {
            var CWPCompwindow = $find("<%=mdlPopupCWPComp.ClientID %>");
            //close CWP Comp popup window
            CWPCompwindow.hide();
            //           release resources
            $("#IframeCWPComp").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnCWPComp").click();
        }
    </script>
    <!-- End-->
    <!-- CWP OnHold Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCWPOnHold" Text="CWP OnHold" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCWPOnHold" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCWPOnHold" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCWPOnHold" runat="server" TargetControlID="btnDummyCWPOnHold"
        PopupControlID="pnlCWPOnHold" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCWPOnHoldStateComplete() {
            $("#btnDummyCWPOnHold").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCWPOnHoldWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCWPOnHold").attr("src", "wfCWPOnHold_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCWPOnHold").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCWPOnHold() {
            var CWPOnHoldwindow = $find("<%=mdlPopupCWPOnHold.ClientID %>");
            //close CWP OnHold popup window
            CWPOnHoldwindow.hide();
            //           release resources
            $("#IframeCWPOnHold").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnCWPStatusChild").click();
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
    <!-- TaskSheet Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyTaskSheet" Text="TaskSheet" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlTaskSheet" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeTaskSheet" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupTaskSheet" runat="server" TargetControlID="btnDummyTaskSheet"
        PopupControlID="pnlTaskSheet" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameTaskSheetStateComplete() {
            $("#btnDummyTaskSheet").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenTaskSheetWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeTaskSheet").attr("src", "wfCWPTaskSheet_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyTaskSheet").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForTaskSheet() {
            var TaskSheetwindow = $find("<%=mdlPopupTaskSheet.ClientID %>");
            //close TaskSheet popup window
            TaskSheetwindow.hide();
            //           release resources
            $("#IframeTaskSheet").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnTaskSheet").click();
        }
    </script>
    <!-- End-->
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, DefaultValue) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': DefaultValue };
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
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
           var ddCustomer = document.getElementById("cmbVendorList");
            if  (ddCustomer != null) {
             if  (ddCustomer.disabled ==false)
             {
              var j = 0;
              <% For Each item2 In mVendorList%>
                <% If  item2.NotInUse ="True" Then%>
                ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                j = j + 1;
             <% Next%>
             }
             }
            });    
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>
