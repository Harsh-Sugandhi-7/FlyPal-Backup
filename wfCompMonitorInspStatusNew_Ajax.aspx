<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCompMonitorInspStatusNew_Ajax.aspx.vb"
    Inherits="Flypal.wfCompMonitorInspStatusNew_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Comply Component Inspection Status</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
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
    <%--<div>--%>
    <table border="0" id="tblMain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblinner" class="clstablelistin">
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Component Inspection Status [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvLicenseNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="Enter correct License No"
                                            ControlToValidate="txtLicenceNo" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                            ErrorMessage="Remark too long." Display="None" ControlToValidate="txtRemark"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvExtensionValue" runat="server" OnServerValidate="CustomValidate1"
                                            Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <fieldset id="fdsMonitoringDetails" class="clsFieldSet" style="border-width: 1px;">
                                    <legend id="ledMonitoringDetails"><b>Monitoring Details</b></legend>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <asp:UpdatePanel runat="server" ID="upnlSelectMonitoringInspection" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnSelect" runat="server" CssClass="clsButtonVeryLong" ToolTip="Click to open Part Inspection List screen"
                                                            Width="170px" Text="Select Monitoring Inspection" CausesValidation="False"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="121px">
                                                <span id="lblMonitorInspectionType" class="clsLabelAuto">Insp. Type</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPartMonitorInspectionTypeName" runat="server" CssClass="clsTextBox_Ajax"
                                                    ToolTip="Inspection Type" Text="<%# mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName %>"
                                                    ReadOnly="True" BackColor="#E0E0E0">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblATAChapter" class="clsLabelAuto">ATA Chapter </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBox_Ajax" ToolTip="ATA Chapter"
                                                    Text="<%# mCompMonitorInspStatus.PartMonitorInsp.ATAChapter %>" ReadOnly="True"
                                                    BackColor="#E0E0E0">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblReference" class="clsLabelAuto">Reference </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Reference"
                                                    Text="<%# mCompMonitorInspStatus.PartMonitorInsp.Reference %>" ReadOnly="True"
                                                    BackColor="#E0E0E0" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblDescription" class="clsLabelAuto">Description </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                    Width="250px" BackColor="#E0E0E0" Text="<%# mCompMonitorInspStatus.PartMonitorInsp.Description %>"
                                                    ReadOnly="True" ToolTip="Description" MaxLength="200" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td valign="top">
                                <asp:UpdatePanel ID="upnlCurrentValueGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsElapsedRemainingValues" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend2"><b>Elapsed and Remaining Values</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgCurrentValue" runat="server" CssClass="clsGridLog" PageSize="3"
                                                            ToolTip="Elapsed And Remaining Values of the Comp" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Elapsed" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtElapsedValue" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                            AutoPostBack="true" OnTextChanged="txtElapsedValue_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"ElapsedValueFormatted") %>'
                                                                            ReadOnly="<%# mCompMonitorInspStatus.PartMonitorInspID.Equals(Guid.Empty) %>"
                                                                            Width="80px" ToolTip="Enter the Elapsed Value."></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remaining" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemainingValue" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                            AutoPostBack="true" OnTextChanged="txtRemaining_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"RemainingValueFormatted") %>'
                                                                            ReadOnly="<%# mCompMonitorInspStatus.PartMonitorInspID.Equals(Guid.Empty) %>"
                                                                            Width="80px" ToolTip="Enter the Remaining Value."></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">Please Note : Elapsed / Remaining values for Days/Months/Years will be in days.</asp:Label>
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
                                <asp:UpdatePanel ID="upnlMonitoringStatusDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset3" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend3"><b>Done On Details</b></legend>
                                            <table border="0" id="Table2" cellpadding="0" width="100%">
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:UpdatePanel ID="upnlSelectLog" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnSelectLog" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to open Select Log screen"
                                                                                Text="Select Log" TabIndex="2"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="121px">
                                                        <span id="lblDoneOn" class="clsLabel">Done On </span>
                                                    </td>
                                                    <td>
                                                        <table border="0" id="Table9" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="calDoneOn" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDate_watermarkextender','false');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calDoneOn_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDoneOn">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calDoneOn" ID="DoneOnDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblWorkOrNo" class="clsLabel">Work Order No. </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWorkOrdNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Work Order No."
                                                            Text="<%# mCompMonitorInspStatus.DoneWONo %>" ReadOnly="<%# mCompMonitorInspStatus.PartMonitorInsp.ID.Equals(Guid.Empty) %>"
                                                            MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblDoneByAgency" class="clsLabelAuto">Done By Agency</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDoneBy" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCompMonitorInspStatus.DoneBy %>"
                                                            ToolTip="Enter Done By Agency Name" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblLicenceNo" class="clsLabelAuto">License No.</span>
                                                    </td>
                                                    <td>
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
                                                                            <asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                ToolTip="<%# mCompMonitorInspStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblPlace" class="clsLabelAuto">Place</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                            Text="<%# mCompMonitorInspStatus.Place %>" ToolTip="Enter Place">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblRequiredmanHours0" class="clsLabelAuto">Actual Man Hours</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtActualManHours" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                            ToolTip="Enter Actual Man Hours" Text="<%# mCompMonitorInspStatus.TotalReqManHrs1 %>"
                                                            Enabled="<%# mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count <= 1 %>"
                                                            OnTextChanged="txtActualManHours_TextChanged" AutoPostBack="true" MaxLength="8">
                                                        </asp:TextBox>
                                                        <asp:Label ID="lblEstdManHours" runat="server" ToolTip="Estd. Man Hours" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblRemark" class="clsLabel">Remark </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" ToolTip="Enter the Remark for the Comp Inspection Status"
                                                            Width="250px" Text="<%# mCompMonitorInspStatus.DoneRemark %>" ReadOnly="<%# mCompMonitorInspStatus.PartMonitorInsp.ID.Equals(Guid.Empty) %>"
                                                            MaxLength="500" TextMode="MultiLine">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td valign="top">
                                <asp:UpdatePanel ID="upnlDoneOnValueGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsDoneOnValueGrid" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="lblCompValues" runat="server" style="font-weight: bold;">Component Values</legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsLater" runat="server" CssClass="clsLabelAuto" Text="Calculate Due For Period Whichever Later"
                                                            Enabled="False" Checked="<%# mCompMonitorInspStatus.IsLater %>"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGridLog" PageSize="3"
                                                            AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="PeriodID" HeaderText="PeriodID"></asp:BoundField>
                                                                <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Done On/Start">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDoneOnValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DoneOnValueFormatted") %>'
                                                                            BackColor="Gainsboro" ReadOnly="true" AutoPostBack="true" OnTextChanged="txtDoneOnValue_TextChanged"
                                                                            CssClass="clsTextBoxRightAlignSmall_Ajax">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvCurrentValue" runat="server" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                  <%-- 'Added By Shital on 25-Jan-2021--%>
                                                                 <asp:TemplateField HeaderText="Extension" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtExtensionValue" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                            AutoPostBack="true" OnTextChanged="txtExtensionValue_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"ExtensionValueFormatted") %>'
                                                                            ToolTip="Enter the Extension Value.">
                                                                        </asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                              <%--  End--%>
                                                                <asp:TemplateField HeaderText="Due At">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDueOnValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DueOnValueFormatted") %>'
                                                                            BackColor="Gainsboro" ReadOnly="true" AutoPostBack="true" OnTextChanged="txtDueOnValue_TextChanged"
                                                                            CssClass="clsTextBoxRightAlignSmall_Ajax">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvDueOnValue" runat="server" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="AssemblyDueOnValueFormatted" HeaderText="Due At Assembly">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AssemblyDueOnValueFormattedByAirFrame" HeaderText="Due At Airframe">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNote1" runat="server" CssClass="clsLabelHeader" Width="500">Please Note: Done On/Current/Due At values for Days/Months/Years will be in dates.</asp:Label>
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
                                <asp:UpdatePanel ID="upnlDocument" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend5"><b>Document Details</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td width="121px">
                                                        <span id="lblRevisionNo" class="clsLabelAuto">Revision No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                            Text="<%# mCompMonitorInspStatus.RevisionNo %>" ToolTip="Enter Revision No."></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblPageNo" class="clsLabel">Page No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPageNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                            Text="<%# mCompMonitorInspStatus.PageNo %>" ToolTip="Enter Page No.">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblBookNo" class="clsLabel">Book No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBookNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                            Text="<%# mCompMonitorInspStatus.BookNo %>" ToolTip="Enter Book No.">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSourceDoc" class="clsLabel">Source Doc</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSourceDoc" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            Width="250px" MaxLength="500" Text="<%# mCompMonitorInspStatus.SourceDoc %>"
                                                            TextMode="MultiLine" ToolTip="Enter Source Doc.">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                runat="server" class="clsButton_Ajax" causesvalidation="False" tabindex="13" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
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
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td valign="top">
                                <asp:UpdatePanel ID="upnlExtensionDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsExtensionDetails" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend4"><b>Extension Details</b></legend>
                                            <table id="Table4" border="0" cellpadding="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblExtensionDate" class="clsLabel">Extension Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtExtensionDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'ExtensionDate_watermarkextender','false');"
                                                            Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtExtensionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExtensionDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="ExtensionDate_watermarkextender" runat="server"
                                                            ClientIDMode="Static" TargetControlID="txtExtensionDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblApprovalRemark" class="clsLabel">Approval Remark</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtApprovalRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            MaxLength="500" Text="<%# mCompMonitorInspStatus.ApprovalRemark %>" TextMode="MultiLine"
                                                            ToolTip="Enter Approval Remark">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlRevisedDetails" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <fieldset id="fdsRevisedDetails" class="clsFieldSet" style="border-width: 1px;">
                                                                    <legend id="Legend6"><b>Revised Details</b></legend>
                                                                    <table>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip="Check to apply applicable"
                                                                                    Text="Applicable" Checked="<%# mCompMonitorInspStatus.IsApplicable %>"></asp:CheckBox>
                                                                                <span class="clsLabelHeader">(Un-check if not required to be monitored from now onwards..)</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Span1" class="clsLabel">Do you want to Revise this Activity?</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnRevise" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Revise Component Inspection"
                                                                                    Text="Yes"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlRedLabel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRed" runat="server" CssClass="clsLabelAuto" BackColor="Red" ForeColor="Red"
                                            Visible="false">Green</asp:Label>
                                        <asp:Label ID="lblInfo" runat="server" Text="Complied One Time Inspection record"
                                            CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right" colspan="1">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to save Comp Inspection"
                                                        Text="Save"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print Comp Inspection"
                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                        Text="Back" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open Partpopup-->
                        <tr style="height: 0px;">
                            <td>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
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
    <%--</div>--%>
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
    <!-- InspMaster Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyInspMaster" Text="Dummy InspMaster" ClientIDMode="Static">
        </asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlInspMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeInspMaster" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupInspMaster" runat="server" TargetControlID="btnDummyInspMaster"
        PopupControlID="pnlInspMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameInspMasterStateComplete() {
            $("#btnDummyInspMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenInspMasterWindow(GChildPage2, GChildPage4, GChildPage5, GChildPage6) {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                //$("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage4=wfInstallComp_AJAX.aspx");
                //$("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage4=" + GChildPageTmp);
                $("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage2=" + GChildPage2 + "&GChildPage4=" + GChildPage4 + "&GChildPage5=" + GChildPage5 + "&GChildPage6=" + GChildPage6);
                // $("#IframeInspMaster").load(function () {
                //                    var doc = IframeInspMaster.window;
                //                    IframeInspMaster.SetPageLayout();

                if (!$.browser.msie) {
                    $("#btnDummyInspMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForInspMaster() {
            var InspMasterwindow = $find("<%=mdlPopupInspMaster.ClientID %>");
            //close InspMaster popup window
            InspMasterwindow.hide();
            //           release resources
            $("#IframeInspMaster").attr("src", "JavaScript:''");
            //call InspMaster image button
            $("#hdnBtnInspMaster").click();
        }
    </script>
    <!-- End-->
    <!-- Select Log popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySelectLog" Text="Select Log" ClientIDMode="Static" />
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
            //close Select Log  popup window
            SelectLogwindow.hide();
            //           release resources
            $("#IframeSelectLog").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSelectLog").click();
        }
    </script>
    <!-- End-->
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
                $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=9");

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
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
