<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfADSBSamplingClosure.aspx.vb"
    Inherits="Flypal.wfADSBSamplingClosure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html>
<head runat="server">
    <title>AD/SB Sampling Closure</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="Styles.css" id="Link1" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .clsFieldSet legend
        {
            font-family: Verdana;
            font-size: 13px;
            color: Black;
            font-weight: 500;
            border-style: solid;
            padding: 2 2 2 2;
            margin: 2 2 2 2;
            width: auto; /*   height: auto; vertical-align:middle;*/
            text-align: left;
            margin-left: 10px;
            background-color: WhiteSmoke;
            border-width: 1.8;
        }
    </style>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
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
    <table class="clstablelistout" id="tblmain" style="margin-top: 5px; margin-left: 5px;">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <span id="lblTitle" style="font-size: 18px; font-weight: 100;" class="text-warning clstitle1"
                                            runat="server">AD/SB Sampling Closure</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Select Date" ControlToValidate="txtAircraftEngineAPU"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAircraftEngineAPU" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Aircraft Engine APU is too long" ControlToValidate="txtAircraftEngineAPU"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAuditConformance" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Audit Conformance is too long" ControlToValidate="txtAuditConformance"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAreaOfConcern" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Observation is too long" ControlToValidate="txtObservation"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAudit" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Notices For Rectification is too long" ControlToValidate="txtNoticesForRectification"
                                            Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvAuditConformance" runat="server" Display="None"
                                            ControlToValidate="txtAuditConformance" ValidationGroup="a" ErrorMessage="Audit Conformance Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvObservation" runat="server" Display="None" ControlToValidate="txtObservation"
                                            ValidationGroup="a" ErrorMessage="Observation Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvNoticesForRectification" runat="server" Display="None"
                                            ControlToValidate="txtNoticesForRectification" ValidationGroup="a" ErrorMessage="Notices For Rectification Required"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="border-width: 1px; margin-left: 5px; margin-right: 5px;">
                                            <legend style="font-family: Verdana; font-size: 8pt; font-weight: 500;"><b>AD/SB Detail</b></legend>
                                            <table style="margin-top: -22px">
                                                <tr>
                                                    <td>
                                                        <span id="Span1" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtADSBTechRecordingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                            Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px" Enabled="false"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtADSBTechRecordingDate_CalendarExtender" runat="server"
                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtADSBTechRecordingDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtADSBTechRecordingDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtADSBTechRecordingDate" WatermarkCssClass="clsDateTextBox"
                                                            WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="lblNo" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtADSBTechRecordingText" runat="server" Text="<%# mADSBTechRecording.ADSBRecordingText %>"
                                                            CssClass="input-sm clsTextBox_Ajax" Height="25px" ToolTip="Enter No." MaxLength="25"
                                                            Width="208px" Enabled="false" Style="margin-bottom: 5px;"> </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="Span6" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            AD/SB No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtADSBNO" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="AD/SB No."
                                                            MaxLength="25" Text="<%# mADSBTechRecording.ADSBNo %>" Width="208px" Height="25px"
                                                            Enabled="false" Style="margin-bottom: 5px;"> </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblSubject" class="control-label clsLabelAuto" style="margin-left: 5px;
                                                            margin-right: 5px;">Subject</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="Subject"
                                                            MaxLength="500" Text="<%# mADSBTechRecording.ADSBSubject %>" Style="margin-bottom: 5px;"
                                                            Height="25px" Width="208px" Enabled="false" TextMode="MultiLine"> </asp:TextBox>
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
                                <asp:UpdatePanel runat="server" ID="upnlADSBPlanningSupportDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="font-family: Verdana; font-size: 10pt; font-weight: 500;
                                            border-width: 1px; margin-left: 5px; margin-right: 5px;">
                                            <legend style="font-family: Verdana; font-size: 8pt; font-weight: 500;"><b>Sampling
                                                Closure</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="Span2" class="control-label clsLabel" style="margin-left: 5px; margin-right: 5px;">
                                                            Engine APU</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAircraftEngineAPU" runat="server" Text="<%# mADSBSamplingClosure.AircraftEngineAPU %>"
                                                            CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine" Style="margin-top: 5px;
                                                            margin-bottom: 5px;"> </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblAuditConformance" class="control-label clsLabel" style="margin-left: 5px;
                                                            margin-right: 5px;">Audit Conformance</span> <span id="lblAuditConformanceStar" class="control-label clsLabelStar">
                                                                *</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAuditConformance" runat="server" Text="<%# mADSBSamplingClosure.AuditConformance %>"
                                                            CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine" Style="margin-top: 5px;
                                                            margin-bottom: 5px;"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblObservation" class="control-label clsLabel" style="margin-left: 5px;
                                                            margin-right: 5px;">Observation</span><span id="lblObservationStar" class="control-label clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtObservation" runat="server" Text="<%# mADSBSamplingClosure.Observation %>"
                                                            CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine" Style="margin-top: 5px;
                                                            margin-bottom: 5px;"> </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblNoticesForRectification" class="control-label clsLabel" style="margin-left: 5px;
                                                            margin-right: 5px;">Notices For Rectification</span><span id="lblNoticesForRectificationStar"
                                                                class="control-label clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNoticesForRectification" runat="server" Text="<%# mADSBSamplingClosure.NoticesForRectification %>"
                                                            CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine" Style="margin-top: 5px;
                                                            margin-bottom: 5px;"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span3" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            Audit Compliance Date</span>
                                                        <%--<span id="lblAuditComplianceDateStar" class="control-label clsLabelStar">
                                                                *</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAuditComplianceDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                            Style="margin-bottom: 5px; margin-top: 5px;" Height="25px" Text="" Width="110px"
                                                            onchange="ValidateDateText(this,'txtAuditComplianceDateWatermarkExtender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtAuditComplianceDate_CalendarExtender" runat="server"
                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAuditComplianceDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtAuditComplianceDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtAuditComplianceDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="Span13" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            Audit Sampling Date</span>
                                                        <%--<span id="Span14" class="control-label clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAuditSamplingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                            Style="margin-bottom: 5px; margin-top: 5px;" Height="25px" Text="" Width="110px"
                                                            onchange="ValidateDateText(this,'txtAuditSamplingDateWatermarkExtender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtAuditSamplingDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAuditSamplingDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtAuditSamplingDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtAuditSamplingDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span4" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            OEM Reporting Date</span>
                                                        <%--<span id="lblOEMReportingDateStar" class="control-label clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOEMReportingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                            Style="margin-bottom: 5px; margin-top: 5px;" Height="25px" Text="" Width="110px"
                                                            onchange="ValidateDateText(this,'txtOEMReportingDateWatermarkExtender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtOEMReportingDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOEMReportingDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtOEMReportingDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtOEMReportingDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="Span5" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            Lessor Reporting Date</span>
                                                        <%--<span id="Span15" class="control-label clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLessorReportingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                            Style="margin-bottom: 5px; margin-top: 5px;" Height="25px" Text="" Width="110px"
                                                            onchange="ValidateDateText(this,'txtLessorReportingDateWatermarkExtender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtLessorReportingDate_CalendarExtender" runat="server"
                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtLessorReportingDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtLessorReportingDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtLessorReportingDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span16" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            Re-Current Monitoring Date</span>
                                                        <%--<span id="lblReCurrentCheckMonitoringDateStar" class="control-label clsLabelStar">
                                                                *</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReCurrentCheckMonitoringDate" runat="server" ClientIDMode="Static"
                                                            CssClass="input-sm clsTextBox_Ajax" Style="margin-bottom: 5px; margin-top: 5px;"
                                                            Height="25px" Text="" Width="110px" onchange="ValidateDateText(this,'txtReCurrentCheckMonitoringDateWatermarkExtender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtReCurrentCheckMonitoringDate_CalendarExtender" runat="server"
                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReCurrentCheckMonitoringDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtReCurrentCheckMonitoringDateWatermarkExtender"
                                                            runat="server" TargetControlID="txtReCurrentCheckMonitoringDate" WatermarkCssClass="clsDateTextBox"
                                                            WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="Span17" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            Record Updating Date</span>
                                                        <%--<span id="Span18" class="control-label clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRecordUpdatingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                            Style="margin-bottom: 5px; margin-top: 5px;" Height="25px" Text="" Width="110px"
                                                            onchange="ValidateDateText(this,'txtRecordUpdatingDateWatermarkExtender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtRecordUpdatingDate_CalendarExtender" runat="server"
                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRecordUpdatingDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtRecordUpdatingDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtRecordUpdatingDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span19" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                            NAA Reporting Date</span>
                                                        <%--<span id="lblNAAReportingStar" class="control-label clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNAAReporting" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                            Style="margin-bottom: 5px; margin-top: 5px;" Height="25px" Text="" Width="110px"
                                                            onchange="ValidateDateText(this,'txtNAAReportingWatermarkExtender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtNAAReporting_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtNAAReporting">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtNAAReportingWatermarkExtender" runat="server"
                                                            TargetControlID="txtNAAReporting" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="Span8" class="control-label clsLabelAuto" style="margin-left: 5px;">File Attachments</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="99%" style="border-width: 1px; margin-left: 5px">
                                                                    <tr>
                                                                        <td style="height: 15px">
                                                                            <asp:UpdatePanel ID="upnldgAttachment" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:GridView ID="dgAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                                        CssClass="table table-striped table-bordered table-hover" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
                                                                                        AllowSorting="True" AllowPaging="False" AutoGenerateColumns="false">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <Columns>
                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-ForeColor="black">
                                                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" Width="60px" ForeColor="black">
                                                                                                </HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
                                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:TemplateField HeaderText="File Name" HeaderStyle-ForeColor="black">
                                                                                                <HeaderStyle Width="700px" HorizontalAlign="Left"></HeaderStyle>
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtFileName" runat="server" CssClass="input-sm clsTextBox3_Ajax"
                                                                                                        MaxLength="100" ClientIDMode="Static" Height="25px" ToolTip="Enter File Name To Be Attached"
                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>' Width="700px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center"
                                                                                                HeaderStyle-ForeColor="black">
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center"
                                                                                                HeaderStyle-ForeColor="black">
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
                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="false">
                                                                            </asp:ImageButton>
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
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnCancel" runat="server" ClientIDMode="Static" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Cancel" ToolTip="Click to Cancel the WO Invoice" Visible="false" />
                                        <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-sm" Visible="false" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Print" ClientIDMode="Static" ToolTip="Click to Print Invoice">
                                        </asp:Button>
                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Authorize" ToolTip="Click to authorize WO Invoice"
                                            Visible="false" />
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Save" CausesValidation="true" ValidationGroup="a"
                                            ToolTip="Click to Save" />
                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px; margin-right: 5px;" Text="Close" ToolTip="Click to close" />
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
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
    <asp:HiddenField ID="hdnpartId" runat="server" ClientIDMode="Static" />
    <%--Date Validations--%>
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
    <div>
    </div>
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
        function OpenFileUploadWindow() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
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
    <!-- End -->
    <!-- End File Upload Modal Dialog-->
    </form>
</body>
</html>
