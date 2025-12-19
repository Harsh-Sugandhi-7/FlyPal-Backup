<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptLogEntryFormat_Ajax.aspx.vb"
    Inherits="Flypal.wfrptLogEntryFormat_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <title></title>
    <script type="text/javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openWordFile() {
            str = "wfExportToWord.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="2000" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td colspan="3" class="clsFormHeader1Newstyle">
                    <span id="lbltitle" class="clsFormHeader">Log Book Entry Report</span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server">
                        <table id="tblInner">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                ErrorMessage="From Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="a"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" OnServerValidate="CustomValidate"
                                                ValidationGroup="a"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Width="60px">From Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                                            AutoPostBack="true" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                            runat="server"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="5">
                                                        <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto" Width="60px">Aircraft </asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True" 
                                                            DataTextField="RegNo" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto" Width="60px">Assembly</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraftAssembly" runat="server"
                                                            DataTextField="ModelSerialNoPostion" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader" Visible="false">Step IV. Selection of Reference Document </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkLogNo" runat="server" Checked="True" CssClass="clsLabel" Text="Log No."
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkLogPageNo" runat="server" CssClass="clsCheckBox" Text="Log Page No."
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkFlightNo" runat="server" CssClass="clsLabelAuto" Text="Flight No."
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkRemark" runat="server" CssClass="clsLabelAuto" Text="Remark"
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkFlightLogClassifications" runat="server" CssClass="clsLabelAuto"
                                                            Visible="false" Text="Flight Log Classifications" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblReportFooter" CssClass="clsLabelHeader" runat="server">Step IV. Enter text to be display at bottom line of report.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewStyleLong" ID="txtBottomLine" runat="server" 
                                                             MaxLength="500" TextMode="MultiLine" ToolTip="Enter Remark"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="Label2" CssClass="clsLabelHeader" runat="server">Step IV. Enter text to be display along with Maintenance Carried Out.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="chkPrintthisline" runat="server" Checked="True" CssClass="clsCheckBox"
                                                            Text="Print this line" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewStyleLong" ID="txtMaintenanceCarriedOut" runat="server" 
                                                            Text="AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE."
                                                             MaxLength="500" TextMode="MultiLine" ToolTip="Enter Remark"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Activities</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowInstRem" runat="server" Checked="True" CssClass="clsCheckBox"
                                                                        Text="Show Install/Removal" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowService" runat="server" Checked="True" CssClass="clsCheckBox"
                                                                        Text="Show Service" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowInsp" runat="server" Checked="True" CssClass="clsCheckBox"
                                                                        Text="Show Inspection" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowDir" runat="server" Checked="True" CssClass="clsCheckBox"
                                                                        Text="Show AD/SB" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowMaintActivity" runat="server" Checked="True" CssClass="clsCheckBox"
                                                                        Text="Show Maintenance Activity" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowPirepsMELSnag" runat="server" Checked="True" CssClass="clsCheckBox"
                                                                        Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Show Pireps/ADD/Defect","Show Pireps/MEL/Snag") %>' />
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
                                <td align="right">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" CssClass="clspanel1">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnTopView" TabIndex="0" runat="server" 
                                                            Text="View" ValidationGroup="a" ToolTip="Click to View"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnTopDisplay" TabIndex="0" runat="server" 
                                                            ValidationGroup="a" Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                    </td>
                                                    <%-- 'Added by Shital on 6-Sep-2016--%>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnTopReportByMail" runat="server" Text="Report By Mail"
                                                            ToolTip="Click to receive Report through mail" ValidationGroup="a"  />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnTopInWord" runat="server"  Text="In Word"
                                                            ValidationGroup="a"  />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnTopClose" runat="server"  Text="Close"
                                                            CausesValidation="False" ToolTip="Click to close the screen"></asp:Button>
                                                    </td>
                                                </tr>
                                                <!--Dummy panel to open modelpopup 6-Sep-2016-->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;" colspan="2" align="right">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                                            <ContentTemplate>
                                                                <asp:Button ID="Button6" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                    Style="display: none;"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--End -->
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gdPartSearch" ShowHeaderWhenEmpty="false" ClientIDMode="Static"
                                                runat="server" AllowSorting="false" DataKeyNames="ID" AllowPaging="false"
                                                GridLines="Horizontal" CellPadding="3" CssClass="clsGridNewStyle"
                                                AutoGenerateColumns="False" PageSize="25">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle  BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <input type="checkbox" name="chkSelectList" class="cbSelectRow" value="<%# Eval("ID") %>"></input>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Defect" HeaderText="WO. No.">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DoneBy" HeaderText="Done By">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LicNo" HeaderText="Lic No.">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CAT" HeaderText="CAT">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td>
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 2px; height: 20px">
                                                    </td>
                                                    <td style="height: 20px">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td style="height: 20px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" CssClass="clspanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnView" TabIndex="0" runat="server"  Text="View"
                                                            ValidationGroup="a" ToolTip="Click to View" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                            ValidationGroup="a" Text="Display" ToolTip="Click to Display Report" Visible="false">
                                                        </asp:Button>
                                                    </td>
                                                    <%-- 'Added by Shital on 6-Sep-2016--%>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Report By Mail"
                                                            ToolTip="Click to receive Report through mail" ValidationGroup="a"
                                                            Visible="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExportToWord" runat="server"  Text="In Word"
                                                            ValidationGroup="a"  Visible="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" Text="Close" CausesValidation="False"
                                                            ToolTip="Click to close the screen" Visible="false"></asp:Button>
                                                    </td>
                                                </tr>
                                                <!--Dummy panel to open modelpopup 6-Sep-2016-->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;" colspan="2" align="right">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--End -->
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
    </div>
    <div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>
    <div>
        <%--Date Validations--%>
        <script type="text/javascript">
            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                var fromdate = $("#txtFromDate").val();
                var todate = $("#txtToDate").val();
                if (!todate) {
                    rfvToDate.isvalid = false;
                    return;
                }
                if (!fromdate) {
                    rfvFromDate.isvalid = false;
                    return;
                }
                var param = { 'FromDate': fromdate, 'ToDate': todate };
                $.ajax({
                    type: "POST",
                    url: "BetweenDateValidationHandler.ashx",
                    cache: false,
                    data: param,
                    async: false,
                    beforeSend: OnBeforeSnd,
                    success: onSuces,
                    error: onErr
                });

                function onSuces(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    if (result == "True") {
                        args.IsValid = true;
                        return;
                    }

                }

                function onErr(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    source.errormessage = result;
                    return;
                }
                function OnBeforeSnd() {
                    $get("AjaxLoader").style.visibility = 'visible';
                }

            }

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
    </div>
    <!-- Popup For Report By Mail 6-Sep-2016-->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
        PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyReceipt1").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgLogBtnSendMail").click();
        }
    </script>
    <!---End-->
    <script type="text/javascript">
        $(document).ready(function () {
            //            debugger;
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#gdPartSearch tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                        SetRow($(this));
                    }
                    else {
                        $(this).removeAttr("checked");
                        SetRow($(this));
                    }

                });
            });
        });

        function SetRow(elem) {
            var status = $(elem).attr("checked");
            if (status == "checked") {
                $(elem).closest("tr").addClass('HighLightRow');
            }
            else {
                $(elem).closest("tr").removeClass('HighLightRow');
            }
        }

        function pageLoad() {
            var status;
            $("#gdPartSearch tr:gt(0)").find(":checkbox").each(function () {
                status = $(this).attr("checked");
                if (status == "checked") {
                    SetRow($(this));
                }
                else {
                    //$(this).removeAttr("checked");
                    SetRow($(this));
                }

            });

        }
    </script>
    </form>
    <script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("gdPartSearch");
            var rowData;
            var regex = /(&nbsp;|<([^>]+)>)/ig
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML.replace(regex, '');
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }    
    </script>
</body>
</html>
