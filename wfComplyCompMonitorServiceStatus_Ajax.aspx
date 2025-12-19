<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyCompMonitorServiceStatus_Ajax.aspx.vb"
    Inherits="Flypal.wfComplyCompMonitorServiceStatus_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Comply Component Service Status</title>
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
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
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
        .clsCursorStyle {
            cursor: pointer;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblMain" class="clstablelistout">
            <tr>
                <td colspan="3">
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                        <table id="tblinner" class="clsTablelistin">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Comply Component Service Status</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                             <tr>
                                    <td>

                                    </td>
                                    <td align ="right">
                                        <asp:Label ID="lblAMPNo" runat="server" Text="" CssClass ="clsLabel" Font-Bold="true" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'></asp:Label> 
                                    </td>
                                </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlValidationSummary" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                Width="100%" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvCurrentValue" runat="server" CssClass="clsLabelAuto" Display="None"
                                                OnServerValidate="customvalidate1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvExtensionValue" runat="server" OnServerValidate="CustomValidate1"
                                                Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                ControlToValidate="txtRemark" Display="None" ErrorMessage="Remark too long."></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                        <legend id="Legend1"><b>Monitoring Details</b></legend>
                                        <table id="Table7" class="clsTable1" cellpadding="0">
                                            <asp:PlaceHolder ID="phTask" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'>
                                                <tr>

                                                    <td></td>
                                                    <td>
                                                        <span id="lblTaskCardNo" class="clsLabelAuto">Task No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTaskCardNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCompMonitorServiceStatus.PartMonitorService.TaskCardNo %>"
                                                            MaxLength="50" ToolTip="Task No." ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </asp:PlaceHolder>
                                            <tr>
                                                <td style="width: 10px">
                                                    <span id="L1" class="clsLabelAuto"></span>
                                                </td>
                                                <td style="width: 115px">
                                                    <span id="lblMonitorModType" runat="server" class="clsLabelAuto">Service Type </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartMonitorServiceTypeName" runat="server" CssClass="clsTextBox_Ajax"
                                                        BackColor="#E0E0E0" ReadOnly="True" Text="<%# mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName %>"
                                                        ToolTip='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "MPD Type", "Service Type") %>'>
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="lblMonitorType" class="clsLabelAuto">Monitor Type </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMonitorType" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" Text="<%# mCompMonitorServiceStatus.PartMonitorService.MonitorTypeName %>"
                                                        ToolTip="Monitor Type">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="lblATAChapter" class="clsLabelAuto">ATA Chapter </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" Text="<%# mCompMonitorServiceStatus.PartMonitorService.ATAChapter %>"
                                                        ToolTip="ATA Chapter">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="lblReference" class="clsLabelAuto">Reference Doc.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        Width="250px" ReadOnly="True" Text="<%# mCompMonitorServiceStatus.PartMonitorService.Reference %>"
                                                        ToolTip="Reference">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="lblDescription" class="clsLabelAuto">Description </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                        Width="250px" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mCompMonitorServiceStatus.PartMonitorService.Description %>"
                                                        ToolTip="Description" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td valign="top">
                                    <asp:UpdatePanel ID="upnlCurrentValueGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px;">
                                                <legend id="Legend2"><b>Elapsed and Remaining Values</b></legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgCurrentValue" runat="server" CssClass="clsGridLog" ShowHeaderWhenEmpty="true"
                                                                AutoGenerateColumns="False" PageSize="3">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                <Columns>
                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                    <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Threshold">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed">
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
                                <td valign="top">
                                    <asp:UpdatePanel ID="upnlMonitoringStatusDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset3" class="clsFieldSet" style="border-width: 1px;">
                                                <legend id="Legend3"><b>Monitoring Status Details</b></legend>
                                                <table id="Table1" class="clsTable1" cellpadding="0">
                                                    <tr>
                                                        <td colspan="2" width="124px"></td>
                                                        <td colspan="1" align="right">
                                                            <asp:UpdatePanel ID="upnlSelectLog" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnSelectLog" runat="server" CssClass="clsButton_Ajax" Text="Select Log"
                                                                        ToolTip="Click to open Select Log screen"></asp:Button>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;">
                                                            <span id="Label4" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td style="width: 100px">
                                                            <span id="lblDoneOn" class="clsLabelAuto">Date</span>
                                                        </td>
                                                        <td style="padding-left: 2px;">
                                                            <asp:TextBox runat="server" ID="txtDoneOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDate_watermarkextender','true');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtDoneOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDoneOnDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDoneOnDate" ID="DoneOnDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblWorkOrNo" class="clsLabel">Work Order No. </span>
                                                        </td>
                                                        <td style="padding-left: 2px;">
                                                            <asp:TextBox runat="server" ID="txtWorkOrderNo" CssClass="clsTextBox_Ajax" MaxLength="100"
                                                                Text="<%# mCompMonitorServiceStatus.DoneWONo %>" ToolTip="Enter Work Order Number">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblRequiredmanHours" class="clsLabelAuto">Actual Man Hours</span>
                                                        </td>
                                                        <td style="padding-left: 2px;">
                                                            <asp:TextBox ID="txtActualManHours" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                                ToolTip="Enter Actual Man Hours" Text="<%# mCompMonitorServiceStatus.TotalReqManHrs1 %>"
                                                                Enabled="<%# mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count <= 1 %>"
                                                                OnTextChanged="txtActualManHours_TextChanged" AutoPostBack="true" MaxLength="8">
                                                            </asp:TextBox>
                                                            <asp:Label ID="lblEstdManHours" runat="server" ToolTip="Estd. Man Hours" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblDoneByAgency" class="clsLabelAuto">Done By Agency</span>
                                                        </td>
                                                        <td style="padding-left: 2px;">
                                                            <asp:TextBox ID="txtDoneBy" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCompMonitorServiceStatus.DoneBy %>"
                                                                ToolTip="Enter Done By Agency Name" MaxLength="100">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblLicenceNo" class="clsLabelAuto">License No.</span>
                                                        </td>
                                                        <td style="padding-left: 2px;">
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
                                                                                <asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                    ToolTip="<%# mCompMonitorServiceStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblPlace" class="clsLabelAuto">Place</span>
                                                        </td>
                                                        <td style="padding-left: 2px;">
                                                            <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                                Text="<%# mCompMonitorServiceStatus.Place %>" ToolTip="Enter Place">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                    runat="server" class="clsButton_Ajax" />
                                                                            </td>
                                                                            <td style="padding-left: 3px;">
                                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
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
                                                        <td></td>
                                                        <td>
                                                            <span id="lblRemark" class="clsLabelAuto">Remark </span>
                                                        </td>
                                                        <td style="padding-left: 2px;">
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500"
                                                                Text="<%# mCompMonitorServiceStatus.DoneRemark %>" TextMode="MultiLine" ToolTip="Enter Remark"
                                                                Width="250px">
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
                                                <legend id="lblDueValues" style="font-weight: bold;">Component Values at Compliance
                                                of Service</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGridLog" AutoGenerateColumns="False"
                                                                ShowHeaderWhenEmpty="true" PageSize="3">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="PeriodUnitNameForDate" HeaderText="Period">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Compliance At" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCurrentValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                AutoPostBack="true" OnTextChanged="txtCurrentValue_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentValueFormatted") %>'>
                                                                            </asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Extension" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtExtensionValue" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                                AutoPostBack="true" OnTextChanged="txtExtensionValue_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "ExtensionValueFormatted") %>'
                                                                                ToolTip="Enter the Extension Value.">
                                                                            </asp:TextBox>
                                                                        </ItemTemplate>
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
                                                            <asp:Label ID="lblNote1" runat="server" CssClass="clsLabelHeader" Width="505px">Please Note: Started On/Current Values/Due On values for Days/Months/Years will be in Dates.  Extension Value for Calendar period should be entered in Days only.</asp:Label>
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
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                        <asp:UpdatePanel ID="upnlDocument" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="Fieldset5" class="clsFieldSet" style="border-width: 1px;">
                                                    <legend id="Legend5"><b>Document Details</b></legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td></td>
                                                            <td width="115px">
                                                                <span id="lblRevisionNo" class="clsLabelAuto">Revision No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                    Text="<%# mCompMonitorServiceStatus.RevisionNo %>" ToolTip="Enter Revision No."></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <span id="lblPageNo" class="clsLabel">Page No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPageNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                                    Text="<%# mCompMonitorServiceStatus.PageNo %>" ToolTip="Enter Page No.">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <span id="lblBookNo" class="clsLabel">Book No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBookNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                                    Text="<%# mCompMonitorServiceStatus.BookNo %>" ToolTip="Enter Book No.">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <span id="lblSourceDoc" class="clsLabel">Source Doc</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSourceDoc" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                    Width="250px" MaxLength="500" Text="<%# mCompMonitorServiceStatus.SourceDoc %>"
                                                                    TextMode="MultiLine" ToolTip="Enter Source Doc.">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:PlaceHolder>
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
                                                                        <span id="lblExtensionDate" class="clsLabelAuto">Extension Date</span>
                                                                    </td>
                                                                    <td style="padding-left: 2px;">
                                                                        <asp:TextBox ID="txtExtensionDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'ExtensionDate_watermarkextender','false');"
                                                                            Width="100px"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtExtensionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExtensionDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender ID="ExtensionDate_watermarkextender" runat="server"
                                                                            ClientIDMode="Static" TargetControlID="txtExtensionDate" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblApprovalRemark" class="clsLabelAuto">Approval Remark</span>
                                                                    </td>
                                                                    <td style="padding-left: 2px;">
                                                                        <asp:TextBox ID="txtApprovalRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                            MaxLength="500" Text="<%# mCompMonitorServiceStatus.ApprovalRemark %>" TextMode="MultiLine"
                                                                            ToolTip="Enter Approval Remark" Width="250px">
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
                                                                        <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" Text="Applicable"
                                                                            ToolTip="Check to apply applicable" Checked="<%# mCompMonitorServiceStatus.IsApplicable %>"></asp:CheckBox>
                                                                        <span class="clsLabelHeader">(Un-check if not required to be monitored from now onwards..)</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="Span1" class="clsLabel">Do you want to Revise this Activity?</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnRevise" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Revise"
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
                                <td>&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkPrintLogBookEntry" runat="server" CssClass="clsLinkButton" Enabled="<%# Not mCompMonitorServiceStatus.IsNew %>"
                                Font-Italic="true" Font-Size="8pt">View Log Book Entry</asp:LinkButton>
                                    &nbsp;
                                </td>

                                <td valign="top" colspan="1" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print" ToolTip="Click to print" Visible="false" ></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to previous page"
                                                            CausesValidation="False"></asp:Button>
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
        <!-- SeriviceMaster Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySeriviceMaster" Text="Dummy SeriviceMaster"
                ClientIDMode="Static"></asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlSeriviceMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeSeriviceMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSeriviceMaster" runat="server" TargetControlID="btnDummySeriviceMaster"
            PopupControlID="pnlSeriviceMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameServiceMasterStateComplete() {
                $("#btnDummySeriviceMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenSeriviceMasterWindow(GChildPage2, GChildPage4, GChildPage5, GChildPage6) {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    //                    $("#IframeSeriviceMaster").attr("src", "wfPartMonitorService_AJAX.aspx?Type=pup&GChildPage4=wfInstallComp_AJAX.aspx");
                    $("#IframeSeriviceMaster").attr("src", "wfPartMonitorService_AJAX.aspx?Type=pup&GChildPage2=" + GChildPage2 + "&GChildPage4=" + GChildPage4 + "&GChildPage5=" + GChildPage5 + "&GChildPage6=" + GChildPage6);

                    // $("#IframeSeriviceMaster").load(function () {
                    //                    var doc = IframeSeriviceMaster.window;
                    //                    IframeSeriviceMaster.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummySeriviceMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForServiceMaster() {
                var SeriviceMasterwindow = $find("<%=mdlPopupSeriviceMaster.ClientID %>");
                //close SeriviceMaster popup window
                SeriviceMasterwindow.hide();
                //           release resources
                $("#IframeSeriviceMaster").attr("src", "JavaScript:''");
                //call SeriviceMaster image button
                $("#hdnBtnSeriviceMaster").click();
            }
        </script>
        <!-- End-->
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
                    $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=8");

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
