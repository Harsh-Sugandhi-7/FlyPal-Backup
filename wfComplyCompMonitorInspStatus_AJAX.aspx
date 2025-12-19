<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyCompMonitorInspStatus_AJAX.aspx.vb"
    Inherits="Flypal.wfComplyCompMonitorInspStatus_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Comply Comp Inspection Status</title>
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
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout" border="0">
        <tr>
            <td>
                <table id="tblinner" class="clstablelistin" border="0">
                    <tr>
                        <td colspan="3">
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Comply Component Inspection Status</asp:Label>
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
                                    <asp:CustomValidator ID="cvCurrentValue" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtATAChapter"
                                        Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvExtensionValue" runat="server" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Done on required"
                                        Display="None" ControlToValidate="txtDoneOnDate"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" ErrorMessage="Remark too long."
                                        Display="None" ControlToValidate="txtRemark" OnServerValidate="customvalidate"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvLicenseNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="Enter correct License No"
                                        Display="None" ControlToValidate="txtLicenceNo" OnServerValidate="customvalidate"></asp:CustomValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <asp:UpdatePanel ID="upnlMonitoringDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                        <legend id="ldgMonitoringDetails" runat="server"><b>Monitoring Details</b></legend>
                                        <table id="Table3" class="clsTable1" border="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 10px;">
                                                </td>
                                                <td style="width: 115px;">
                                                    <span id="lblMonitorInspType" class="clsLabelAuto">Insp Type </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMonitorInspType" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" ToolTip="Inspection Type" Text="<%# mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName %>"
                                                        TabIndex="18"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblMonitorType" class="clsLabel">Monitor Type </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMonitorType" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" ToolTip="Monitor Type" Text="<%# mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeName %>"
                                                        TabIndex="19"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblATAChapter" class="clsLabel">ATA Chapter </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" ToolTip="ATA Chapter" Text="<%# mCompMonitorInspStatus.PartMonitorInsp.ATAChapter %>"
                                                        TabIndex="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblReference" class="clsLabel">Reference </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" ToolTip="Reference" Text="<%# mCompMonitorInspStatus.PartMonitorInsp.Reference %>"
                                                        TabIndex="21" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblDescription" class="clsLabel">Description </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                        BackColor="#E0E0E0" ReadOnly="True" ToolTip="Description" Text="<%# mCompMonitorInspStatus.PartMonitorInsp.Description %>"
                                                        Width="250px" Height="32px" TextMode="MultiLine" TabIndex="22"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <asp:UpdatePanel ID="upnlCurrentValueGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px;">
                                        <legend id="ldgElapsedRemValues" runat="server"><b>Elapsed and Remaining Values</b></legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgCurrentValue" runat="server" CssClass="clsGridLog" PageSize="3"
                                                        ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="Id"></asp:BoundField>
                                                            <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed Value">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">Please Note: Elapsed and Remaining Values for Days/Months/Years will be in Days</asp:Label>
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
                            <asp:UpdatePanel ID="upnlMonitoringStatusDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset3" class="clsFieldSet" style="border-width: 1px;">
                                        <legend id="ldgMonitoringStatusDet" runat="server"><b>Done On Details</b></legend>
                                        <table id="Table2" class="clsTable1" cellpadding="0">
                                            <tr>
                                                <td colspan="2" width="124px">
                                                </td>
                                                <td colspan="1" align="right">
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
                                                <td style="width: 05px;">
                                                    <span id="lblDoneOnStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDoneOn" class="clsLabelAuto">Done On </span>
                                                </td>
                                                <td>
                                                    <table id="Table9">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDoneOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                    TabIndex="1" AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDate_watermarkextender','true');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtDoneOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDoneOnDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDoneOnDate" ID="DoneOnDate_watermarkextender"
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
                                                </td>
                                                <td>
                                                    <span id="lblWorkOrNo" class="clsLabelAuto">Work Order No. </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Work Order No."
                                                        Text="<%# mCompMonitorInspStatus.DoneWONo %>" ReadOnly="<%# mCompMonitorInspStatus.PartMonitorInsp.ID.Equals(Guid.Empty) %>"
                                                        MaxLength="100">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblDoneByAgency" class="clsLabelLong">Done By Agency</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDoneBy" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCompMonitorInspStatus.DoneBy %>"
                                                        ToolTip="Enter Done By Agency Name" MaxLength="100">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
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
                                                </td>
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
                                                </td>
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
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabel">Remark </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" ToolTip="Enter the Remark for the Comp Insp Status"
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
                                    <fieldset id="Fieldset4" class="clsFieldSet" style="border-width: 1px;">
                                        <legend id="lblCompValue" runat="server" class="clsLabelHeader">Component Values at
                                            Compliance of Inspection</legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGridLog" PageSize="3"
                                                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" TabIndex="10">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <Columns>
                                                            <asp:BoundField DataField="PeriodUnitNameForDate" HeaderText="Period">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Done On" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCurrentValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CurrentValueFormatted") %>' AutoPostBack="true"
                                                                        OnTextChanged="txtCurrentValue_TextChanged" ClientIDMode="Static" TabIndex="11"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Extension" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtExtensionValue" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                        ToolTip="Enter the Extension Value." AutoPostBack="true" OnTextChanged="txtExtensionValue_TextChanged"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"ExtensionValueFormatted") %>' TabIndex="12"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
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
                                                    <asp:Label ID="lblNote1" runat="server" Width="505px" CssClass="clsLabelHeader">Please Note: Started On/Current Values/Due On values for Days/Months /Years will be in Dates. Extension Value for Calendar period should be entered in Days only.</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <asp:UpdatePanel ID="upnlDocument" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset5" class="clsFieldSet" style="border-width: 1px;">
                                        <legend id="Legend5"><b>Document Details</b></legend>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                </td>
                                                <td width="118px">
                                                    <span id="lblRevisionNo" class="clsLabelAuto">Revision No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                        Text="<%# mCompMonitorInspStatus.RevisionNo %>" ToolTip="Enter Revision No."></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
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
                                                </td>
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
                                                </td>
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
                                                </td>
                                                <td>
                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                </td>
                                                <td>
                                                    <table id="Table12" border="0">
                                                        <tr>
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
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
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
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlRevisedDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsRevisedDetails" class="clsFieldSet" style="border-width: 1px;">
                                                    <legend id="Legend6"><b>Revised Details</b></legend>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip="Check to apply applicable"
                                                                    Text="Applicable" Checked="<%# mCompMonitorInspStatus.IsApplicable %>" TabIndex="1">
                                                                </asp:CheckBox>
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
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="lnkPrintLogBookEntry" runat="server" CssClass="clsLinkButton" Enabled="<%# Not mCompMonitorInspStatus.IsNew %>"
                                Font-Italic="true" Font-Size="8pt">View Log Book Entry</asp:LinkButton>
                            &nbsp;
                        </td>
                        <td valign="top" align="right" colspan="2">
                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" border="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to save Comp Inspection"
                                                    Text="Save" TabIndex="15"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print Comp Inspection"
                                                    Text="Print" TabIndex="16"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                    Text="Back" CausesValidation="False" TabIndex="17"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <!--Dummy panel to open modelpopup-->
                    <tr style="height: 0px;">
                        <td style="height: 0px;">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <!--End -->
                </table>
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
