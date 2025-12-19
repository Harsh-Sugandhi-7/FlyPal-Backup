<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAMPDetail.aspx.vb" Inherits="Flypal.wfAMPDetail" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMP Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script type="text/javascript">
        function IFrameFileUploadStateComplete() {
            $("#btnDummyFileUpload").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenFileUploadWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                //                if (!$.browser.msie) {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = "hidden";
                //                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <table id="Table1" class="clstablelistout" border="0" cellspacing="1" cellpadding="1"
            width="100%">

            <tr>

                <td valign="top" colspan="3">

                    <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">

                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <table id="Table13" valign="top" border="0" cellspacing="1" cellpadding="1" width="100%">

                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">MPD Configuration</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="clsbtnH clsinfoH" ToolTip="Click to save"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" Text="Close" class="clsbtnH clsinfoH" ToolTip="Click to close" CausesValidation="false"></asp:Button>
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
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvAMPTaskNo" runat="server" Display="None" ControlToValidate="txtAMPNo"
                                            ErrorMessage="AMP Task No. Required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvATAChapter" runat="server" Display="None" ControlToValidate="cmbType"
                                            ErrorMessage="Select ATA Chapter From List" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="1">
                                <asp:Label ID="Label16" runat="server" CssClass="clsLabelAuto" Font-Size="12pt">Is This MPD Applicable to this Aircraft?</asp:Label>
                                <asp:CheckBox ID="chkIsApplicable" runat="server" CssClass="clsCheckBoxNewStyle" AutoPostBack="true" CausesValidation="false" />
                                <asp:Label ID="Label161" runat="server" CssClass="clsLabelAuto" Font-Size="10pt">(Click here if applicable else enter Note for justification)</asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlMPDConfig" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>

                                                    <asp:Label ID="lblText" runat="server" CssClass="clsLabelAuto">MPD Task No.</asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMPDTaskNo" runat="server" AutoComplete="off" ClientIDMode="Static" Text="<%# mMPDMaster.MPDTaskNumber %>"
                                                        CssClass="clsTextBoxTagSearch" ToolTip="Enter Text" Enabled="fALSE"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">AMP Task No.</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAMPNo" runat="server" AutoComplete="off" ClientIDMode="Static" MaxLength="150"
                                                        CssClass="clsTextBoxTagSearch" ToolTip="Enter Text"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>

                                                    <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>

                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                        ClientIDMode="Static" Text="<%# mMPDMaster.Description %>" ToolTip="Enter Description" Enabled="false"
                                                        TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--<asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>--%>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTaskTimings" runat="server" CssClass="clsLabelAuto">Task Description</asp:Label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtTaskTimings" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Text="<%# mMPDMaster.TaskIntervalDescription %>"
                                                        ToolTip="Task Timings" MaxLength="500" TextMode="MultiLine" Enabled="<%# (mMPDMaster.IsNew)   %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Label ID="Label15" runat="server" CssClass="clsLabelAuto">ATA</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mMPDMaster.ATAChapter %>"
                                                        ToolTip="ATA" MaxLength="500" Enabled="<%# (mMPDMaster.IsNew)   %>">
                                                    </asp:TextBox>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" CssClass="clsLabelAuto">Task Type</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" SelectedValue="<%# mMPDMaster.ServiceTypeID %>" Enabled="false"
                                                        DataValueField="ID" DataTextField="Name">
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto">Applicability</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApplicability" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Enabled="false"
                                                        ClientIDMode="Static" Text="<%# mMPDMaster.Applicability %>" ToolTip="Enter Applicability"
                                                        TextMode="MultiLine" Width="350px">
                                                    </asp:TextBox>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <span id="lblCurrentValues" class="clsLabel">Current Values</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCurrentValues" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="1000"
                                                        ClientIDMode="Static" Width="150px" TextMode="MultiLine" Enabled="false"
                                                        ToolTip="Enter Note">
                                                    </asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label7" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label8" runat="server" CssClass="clsLabelAuto">Applicable To</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApplicableTo" runat="server" CssClass="clsTextBoxTagSearch"
                                                        ClientIDMode="Static" ToolTip="Enter Applicability" Enabled ="false"
                                                        Width="150px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblReference" CssClass="clsLabel">Reference</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                        ToolTip="Enter Reference" MaxLength="500"
                                                        TextMode="MultiLine" Width="350px">
                                                    </asp:TextBox>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <span id="lblNote" class="clsLabel">Note</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="1000"
                                                        ClientIDMode="Static" Width="350px" TextMode="MultiLine"
                                                        ToolTip="Enter Note">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>

                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>

                        <asp:PlaceHolder ID="phLine" runat="server" Visible="false">

                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td colspan="2" valign="top">
                                <asp:PlaceHolder ID="phCompliance" runat="server" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top" width="50%">
                                                <asp:Panel ID="pnlThreshold" runat="server">
                                                    <asp:UpdatePanel ID="upnlThreshold" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="2" align="center" style="text-decoration: underline;">
                                                                        <asp:Label ID="Label9" runat="server" CssClass="clsLabelAuto" Font-Size="14pt">Threshold</asp:Label>
                                                                        <asp:CheckBox ID="chkIsThreshold" runat="server" CssClass="clsCheckBoxNewStyle" CausesValidation="true" AutoPostBack="true" />

                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="upnlPeriodsThreshold" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <fieldset id="fdsFrequencyofMonitorServiceThreshold" class="clsFieldSet" style="border-width: 1px; width: auto">
                                                                                    <%-- <legend id="lgdFrequencyofMonitorServiceThreshold"><b>Threshold</b></legend>--%>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <asp:GridView ID="dgPeriodsThreshold" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                                                                    ShowHeaderWhenEmpty="true"  AllowPaging="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                                                    <Columns>
                                                                                                        <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:TemplateField HeaderText="Threshold">
                                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:TextBox ID="txtFrequencyValueThreshold" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax" AutoPostBack="true" OnTextChanged="txtFrequencyValueThreshold_TextChanged"
                                                                                                                    MaxLength="8" ReadOnly="<%#IIf(mModelMonitorServiceThreshold.ReadOnlyFrequencyColumn = True, True, False) %>"
                                                                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "FrequencyValueFormatted") %>'>
                                                                                                                </asp:TextBox>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </td>
                                                                                            <td valign="top" align="right">
                                                                                                <asp:ImageButton ID="btnAddPeriodUnitThreshold" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                    Height="22px" Width="24px" ToolTip="Click to Add New period" CausesValidation="False"></asp:ImageButton>
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
                                                                        <asp:Label ID="Label11" runat="server" CssClass="clsLabelAuto" Font-Size="12pt">Compliance Details Available?</asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:UpdatePanel ID="upnlIsComplianceThreshold" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%--<asp:CheckBox ID="chkIsComplianceThreshold" runat="server" CssClass="clsCheckBoxNewStyle" AutoPostBack="true" groupname="a1" checked="true" text="Yes" />--%>
                                                                                            <asp:RadioButton ID="rdbIsComplianceThresholdYes" runat="server" CssClass="clsRadioButtonNewStyle" AutoPostBack="true" GroupName="a1"  Text="Yes" />
                                                                                       &nbsp;&nbsp;
                                                                                            </td>
                                                                                        <td>
                                                                                            <%-- <asp:CheckBox ID="chkIsComplianceThresholdNO" runat="server" CssClass="clsCheckBoxNewStyle" AutoPostBack="true" groupname="a1" Text="No" />--%>
                                                                                            <asp:RadioButton ID="rdbIsComplianceThresholdNo" runat="server" CssClass="clsRadioButtonNewStyle" AutoPostBack="true" GroupName="a1"  Text="No" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>

                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="upnlMonitoringStatusDetailsThreshold" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:PlaceHolder ID="phThresholdDoneDetails" runat="server" Visible="false">
                                                                                    <fieldset id="fdsThresholdDone" class="clsFieldSet" style="border-width: 1px; width: auto">

                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblDoneOnThreshold" class="clsLabel">Date</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox runat="server" ID="txtDoneOnDateThreshold" CssClass="clsTextBoxTagSearchDate" Width="100px" autocomplete="off"
                                                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDateThreshold_watermarkextender','false');"></asp:TextBox>
                                                                                                    <cc2:CalendarExtender ID="txtDoneOnDateThreshold_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDoneOnDateThreshold"></cc2:CalendarExtender>
                                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDoneOnDateThreshold" ID="DoneOnDateThreshold_watermarkextender"
                                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblWorkOrNoThreshold" class="clsLabel">Work Order No. </span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtWorkOrNoThreshold" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Work Order No."
                                                                                                        Text="<%# mAssemblyMonitorServiceStatusThreshold.DoneWONo %>" ReadOnly="<%# mAssemblyMonitorServiceStatusThreshold.ModelMonitorService.ID.Equals(Guid.Empty) %>"
                                                                                                        MaxLength="100">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblLicenceNoThreshold" class="clsLabelAuto">License No.</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtLicenceNoThreshold" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter License No."
                                                                                                                            AutoComplete="off" ClientIDMode="Static" OnTextChanged="txtLicenceNoThreshold_TextChanged"
                                                                                                                            AutoPostBack="true" MaxLength="200"></asp:TextBox>
                                                                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNoThreshold_Autocomplete" runat="server"
                                                                                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                                                            CompletionInterval="1" ServicePath="wfAssemblyMonitorServiceStatus_Ajax.aspx"
                                                                                                                            ServiceMethod="GetLicenseNoList" TargetControlID="txtLicenceNoThreshold" UseContextKey="False"
                                                                                                                            ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                                                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                                                                            OnClientShowing="ClientShowing">
                                                                                                                        </cc2:AutoCompleteExtender>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:ImageButton ID="imgbtnEmployeeLicenceT" runat="server" ImageUrl="~/images/plus1.png" Visible ="false" 
                                                                                                                            Height="22px" Width="24px" ToolTip="Click to select multiple Licence No." CausesValidation="true" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2">
                                                                                                                        <asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                                                            ToolTip="<%# mAssemblyMonitorServiceStatusThreshold.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblPlaceThreshold" class="clsLabelAuto">Place</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtPlaceThreshold" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                                                        Text="<%# mAssemblyMonitorServiceStatusThreshold.Place %>" ToolTip="Enter Place">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblRequiredmanHoursThreshold" class="clsLabelAuto">Actual Man Hours</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtRequiredManHoursThreshold" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                                                                        Text="<%# mAssemblyMonitorServiceStatusThreshold.TotalReqManHrs1 %>" Enabled="<%# mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count <= 1 %>"
                                                                                                        OnTextChanged="txtRequiredManHoursThreshold_TextChanged" AutoPostBack="true" MaxLength="8"
                                                                                                        ToolTip="Enter Actual Man Hours">
                                                                                                    </asp:TextBox>
                                                                                                    <asp:Label ID="lblEstdManHours" runat="server" CssClass="clsLabelHeader" ToolTip="Estd. Man Hours">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblRemark" class="clsLabel">Remark </span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtRemarkThreshold" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" Width="250px"
                                                                                                        ToolTip="Enter the Remark" Text="<%# mAssemblyMonitorServiceStatusThreshold.DoneRemark %>"
                                                                                                        ReadOnly="<%# mAssemblyMonitorServiceStatusThreshold.ModelMonitorService.ID.Equals(Guid.Empty) %>"
                                                                                                        MaxLength="500" TextMode="MultiLine">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <asp:UpdatePanel ID="upnlThresholdValues" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:GridView ID="dgThresholdValues" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                                                                                ShowHeaderWhenEmpty="true" PageSize="3" AllowPaging="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Threshold">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:TemplateField HeaderText="Done On/Starts">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtDoneOnValueThreshold" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txtDoneOnValueThreshold_TextChanged"
                                                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DoneOnValueFormatted") %>' ToolTip="Enter the DoneOn Value.">
                                                                                                                            </asp:TextBox>
                                                                                                                            <asp:CustomValidator ID="cvDoneOnValue" runat="server" Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                    </asp:BoundField>
                                                                                                                    <%--4--%>
                                                                                                                    <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                    </asp:BoundField>
                                                                                                                    <%--5--%>
                                                                                                                    <asp:TemplateField HeaderText="Extn">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtExtensionValueThreshold" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                                                                                AutoPostBack="true" OnTextChanged="txtExtensionValueThreshold_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "ExtensionValueFormatted") %>'
                                                                                                                                ToolTip="Enter the Extension Value." Enabled="<%# iif(mAssemblyMonitorServiceStatusThreshold.ModelMonitorService.MonitorTypeID = 3, False, True) %>">
                                                                                                                            </asp:TextBox>
                                                                                                                            <asp:CustomValidator ID="cvExtensionValue" runat="server" Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <%--6--%>
                                                                                                                    <asp:TemplateField HeaderText="Due At">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtDueOnValueThreshold" runat="server" ToolTip="Enter the Due at Value."
                                                                                                                                AutoPostBack="true" OnTextChanged="txtDueOnValueThreshold_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "DueOnValueFormatted") %>'
                                                                                                                                CssClass="clsTextBoxRightAlignSmall_Ajax" ReadOnly="True">
                                                                                                                            </asp:TextBox>
                                                                                                                            <asp:CustomValidator ID="cvDueOnValue" runat="server" Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <%--7--%>
                                                                                                                    <asp:BoundField DataField="AssemblyDueOnValueFormattedByAirFrame" HeaderText="Due At Airframe">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <%--8--%>
                                                                                                                    <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                    </asp:BoundField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" colspan="2">
                                                                                                    <asp:UpdatePanel ID="upnlRedLabel" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Label ID="lblRed" runat="server" CssClass="clsLabelAuto" BackColor="Red" ForeColor="Red"
                                                                                                                Visible="false">Green</asp:Label>
                                                                                                            <asp:Label ID="lblInfo" runat="server" Text="Complied one time service record" CssClass="clsLabelAuto"
                                                                                                                Visible="false"></asp:Label>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </fieldset>
                                                                                </asp:PlaceHolder>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                            <td valign="top" width="50%">
                                                <asp:Panel ID="pnlInterval" runat="server">
                                                    <asp:UpdatePanel ID="upnlInterval" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="center" style="text-decoration: underline;">
                                                                        <asp:UpdatePanel ID="upnlIsInterval" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:Label ID="Label10" runat="server" CssClass="clsLabelAuto" Font-Size="14pt">Interval</asp:Label>
                                                                                <asp:CheckBox ID="chkIsInterval" runat="server" CssClass="clsCheckBoxNewStyle" CausesValidation="true" AutoPostBack="true" />
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="upnlPeriodsInterval" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <fieldset id="fdsFrequencyofMonitorServiceInterval" class="clsFieldSet" style="border-width: 1px; width: auto">
                                                                                    <%--<legend id="lgdFrequencyofMonitorServiceInterval"><b>Interval</b></legend>--%>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <asp:GridView ID="dgPeriodsInterval" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                                                                    ShowHeaderWhenEmpty="true"  AllowPaging="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                                                    <Columns>
                                                                                                        <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:TemplateField HeaderText="Interval">
                                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:TextBox ID="txtFrequencyValueInterval" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax" AutoPostBack="true" OnTextChanged="txtFrequencyValueInterval_TextChanged"
                                                                                                                    MaxLength="8" ReadOnly="<%#IIf(mModelMonitorServiceInterval.ReadOnlyFrequencyColumn = True, True, False) %>"
                                                                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "FrequencyValueFormatted") %>'>
                                                                                                                </asp:TextBox>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </td>
                                                                                            <td valign="top" align="right">
                                                                                                <asp:ImageButton ID="btnAddPeriodUnitInterval" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                    Height="22px" Width="24px" ToolTip="Click to Add New period" CausesValidation="False"></asp:ImageButton>
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
                                                                        <asp:Label ID="Label12" runat="server" CssClass="clsLabelAuto" Font-Size="12pt">Last Compliance Details Available?</asp:Label>
                                                                    </td>
                                                                    <td align="right">

                                                                        <asp:UpdatePanel ID="upnlIsComplianceInterval" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%--<asp:CheckBox ID="chkIsComplianceInterval" runat="server" CssClass="clsCheckBoxNewStyle" AutoPostBack="true" />--%>
                                                                                            <asp:RadioButton ID="rdbIsComplianceIntervalYes" runat="server" CssClass="clsRadioButtonNewStyle" AutoPostBack="true" groupname="b1" text="Yes" />
                                                                                     &nbsp;&nbsp;
                                                                                            </td>
                                                                                        <td>
                                                                                              <asp:RadioButton ID="rdbIsComplianceIntervalNo" runat="server" CssClass="clsRadioButtonNewStyle" AutoPostBack="true" groupname="b1" text="No"  />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <%-- <td>
                                                                    <asp:Label ID="Label17" runat="server" CssClass="clsLabelAuto" Font-Size="8pt">Mark Threshold Not Applicable and start this MPD ?</asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:CheckBox ID="chkstartInterval" runat="server" CssClass="clsCheckBoxNewStyle" AutoPostBack="true" />
                                                                </td>--%>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="upnlLinkActivity" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:PlaceHolder ID="phNAStart" runat="server">
                                                                                    <asp:Panel ID="pnlLinkActivity" runat="server" Enabled="<%# (chkIsInterval.Checked)   %>">
                                                                                        <fieldset id="fdsLinkActivity" class="clsFieldSet" style="border-width: 1px; width: auto">
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="1">
                                                                                                        <span id="lblLinking" class="clsLabel">Following decisions to be marked as no compliance details</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="1">
                                                                                                        <asp:RadioButton ID="rdbMakeApplicable" Font-Size="10pt" runat="server" Text="Make Applicable" Checked="true" GroupName="radio" CssClass="clsCheckBoxNewStyle" />
                                                                                                    </td>

                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="1">
                                                                                                        <asp:RadioButton ID="rdbMakeApplicableAndStart" Font-Size="10pt" runat="server" Text="Make Applicable And Start" GroupName="radio" CssClass="clsCheckBoxNewStyle" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="1">
                                                                                                        <asp:RadioButton ID="rdbMakeNotApplicable" Font-Size="10pt" runat="server" Text="Make Not Applicable" GroupName="radio" CssClass="clsCheckBoxNewStyle" />
                                                                                                    </td>

                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="1">
                                                                                                        <asp:RadioButton ID="rdbComply" runat="server" Font-Size="10pt" Text="Comply" GroupName="radio" CssClass="clsCheckBoxNewStyle" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="1">
                                                                                                        <asp:RadioButton ID="rdbDoNothing" runat="server" Font-Size="10pt" Text="Do Nothing" GroupName="radio" CssClass="clsCheckBoxNewStyle" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </fieldset>
                                                                                    </asp:Panel>
                                                                                </asp:PlaceHolder>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="upnlMonitoringStatusDetailsInterval" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:PlaceHolder ID="phIntervalDoneDetails" runat="server" Visible="false">
                                                                                    <fieldset id="fdsIntervalDone" class="clsFieldSet" style="border-width: 1px; width: auto">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblDoneOnInterval" class="clsLabel">Date</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox runat="server" ID="txtDoneOnDateInterval" CssClass="clsTextBoxTagSearchDate" Width="100px" autocomplete="off"
                                                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDateInterval_watermarkextender','false');"></asp:TextBox>
                                                                                                    <cc2:CalendarExtender ID="txtDoneOnDateInterval_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDoneOnDateInterval"></cc2:CalendarExtender>
                                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDoneOnDateInterval" ID="DoneOnDateInterval_watermarkextender"
                                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblWorkOrNoInterval" class="clsLabel">Work Order No. </span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtWorkOrNoInterval" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Work Order No."
                                                                                                        Text="<%# mAssemblyMonitorServiceStatusInterval.DoneWONo %>" ReadOnly="<%# mAssemblyMonitorServiceStatusInterval.ModelMonitorService.ID.Equals(Guid.Empty) %>"
                                                                                                        MaxLength="100">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblLicenceNoInterval" class="clsLabelAuto">License No.</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtLicenceNoInterval" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter License No."
                                                                                                                            AutoComplete="off" ClientIDMode="Static" OnTextChanged="txtLicenceNoInterval_TextChanged"
                                                                                                                            AutoPostBack="true" MaxLength="200"></asp:TextBox>
                                                                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNoInterval_Autocomplete" runat="server"
                                                                                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                                                            CompletionInterval="1" ServicePath="wfAssemblyMonitorServiceStatus_Ajax.aspx"
                                                                                                                            ServiceMethod="GetLicenseNoList" TargetControlID="txtLicenceNoInterval" UseContextKey="False"
                                                                                                                            ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                                                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                                                                            OnClientShowing="ClientShowing">
                                                                                                                        </cc2:AutoCompleteExtender>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:ImageButton ID="imgbtnEmployeeLicenceI" runat="server" ImageUrl="~/images/plus1.png" Visible ="false" 
                                                                                                                            Height="22px" Width="24px" ToolTip="Click to select multiple Licence No." CausesValidation="true" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2">
                                                                                                                        <asp:Label ID="Label13" runat="server" Visible="<%# mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                                                            ToolTip="<%# mAssemblyMonitorServiceStatusInterval.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblPlaceInterval" class="clsLabelAuto">Place</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtPlaceInterval" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                                                        Text="<%# mAssemblyMonitorServiceStatusInterval.Place %>" ToolTip="Enter Place">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblRequiredmanHoursInterval" class="clsLabelAuto">Actual Man Hours</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtRequiredManHoursInterval" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                                                                        Text="<%# mAssemblyMonitorServiceStatusInterval.TotalReqManHrs1 %>" Enabled="<%# mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count <= 1 %>"
                                                                                                        OnTextChanged="txtRequiredManHoursInterval_TextChanged" AutoPostBack="true" MaxLength="8"
                                                                                                        ToolTip="Enter Actual Man Hours">
                                                                                                    </asp:TextBox>
                                                                                                    <asp:Label ID="Label14" runat="server" CssClass="clsLabelHeader" ToolTip="Estd. Man Hours">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblRemarkInterval" class="clsLabel">Remark </span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtRemarkInterval" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" Width="250px"
                                                                                                        ToolTip="Enter the Remark" Text="<%# mAssemblyMonitorServiceStatusInterval.DoneRemark %>"
                                                                                                        ReadOnly="<%# mAssemblyMonitorServiceStatusInterval.ModelMonitorService.ID.Equals(Guid.Empty) %>"
                                                                                                        MaxLength="500" TextMode="MultiLine">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <asp:UpdatePanel ID="upnlIntervalValues" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:GridView ID="dgIntervalValues" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                                                                                ShowHeaderWhenEmpty="true" PageSize="3" AllowPaging="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Interval">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:TemplateField HeaderText="Done On/Starts">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtDoneOnValueInterval" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txtDoneOnValueInterval_TextChanged"
                                                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DoneOnValueFormatted") %>' ToolTip="Enter the DoneOn Value.">
                                                                                                                            </asp:TextBox>
                                                                                                                            <asp:CustomValidator ID="cvDoneOnValue" runat="server" Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:TemplateField HeaderText="Extn">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtExtensionValueInterval" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                                                                                AutoPostBack="true" OnTextChanged="txtExtensionValueInterval_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "ExtensionValueFormatted") %>'
                                                                                                                                ToolTip="Enter the Extension Value." Enabled="<%#IIf(mAssemblyMonitorServiceStatusInterval.ModelMonitorService.MonitorTypeID = 3, False, True) %>">
                                                                                                                            </asp:TextBox>
                                                                                                                            <asp:CustomValidator ID="cvExtensionValue" runat="server" Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Due At">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtDueOnValueInterval" runat="server" ToolTip="Enter the Due at Value."
                                                                                                                                AutoPostBack="true" OnTextChanged="txtDueOnValueInterval_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "DueOnValueFormatted") %>'
                                                                                                                                CssClass="clsTextBoxRightAlignSmall_Ajax" ReadOnly="True">
                                                                                                                            </asp:TextBox>
                                                                                                                            <asp:CustomValidator ID="cvDueOnValue" runat="server" Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:BoundField DataField="AssemblyDueOnValueFormattedByAirFrame" HeaderText="Due At Airframe">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining">
                                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                                                    </asp:BoundField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </fieldset>
                                                                                </asp:PlaceHolder>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>

                                    </table>
                                </asp:PlaceHolder>
                            </td>

                        </tr>
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnATAChapter" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnPeriodUnit" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnTools" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>


                </td>
            </tr>

        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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

            function OpenFileUploadWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                    //                if (!$.browser.msie) {
                    $("#btnDummyFileUpload").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                    //                }
                    return false;
                } catch (e) {
                    alert(e);
                }

            }
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
        <!-- Period Unit popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyPeriodUnit" Text="Period Unit" ClientIDMode="Static"
                CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPeriodUnit" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframePeriodUnit" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupPeriodUnit" runat="server" TargetControlID="btnDummyPeriodUnit"
            PopupControlID="pnlPeriodUnit" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFramePeriodUnitStateComplete() {
                $("#btnDummyPeriodUnit").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenPeriodUnitWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframePeriodUnit").attr("src", "wfSelectPeriodUnit_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyPeriodUnit").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForPeriodUnit() {
                var PeriodUnitwindow = $find("<%=mdlPopupPeriodUnit.ClientID %>");
                //close Period Unit popup window
                PeriodUnitwindow.hide();
                //           release resources
                $("#IframePeriodUnit").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnPeriodUnit").click();
            }
        </script>
        <!-- End-->
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
</body>
</html>
