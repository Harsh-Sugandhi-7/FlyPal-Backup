<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfADSBPlanningSupport.aspx.vb"
    Inherits="Flypal.wfADSBPlanningSupport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html>
<head runat="server">
    <title>AD/SB Planning</title>
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
        function openTranDetail() {
            str = "wfReports.aspx"
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
                                        <span id="lblTitle" style="font-size: 18px; font-weight: 100" class="text-warning clstitle1"
                                            runat="server">AD/SB Planning</span>
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
                                            ValidationGroup="a" ErrorMessage="Select Actual Meeting Date Time." ControlToValidate="txtADSBPlanningSupportDate"
                                            Display="None" ValidateEmptyText="true"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRiskIdentification" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Risk Identification Description is too long"
                                            ControlToValidate="txtRiskIdentification" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Remark is too long" ControlToValidate="txtComplianceDuringOthersRemark"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAreaOfConcern" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Area Of Concern is too long" ControlToValidate="txtAreaOfConcern"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAuditDescription" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Audit Description is too long" ControlToValidate="txtAuditDescription"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAMOCDescription" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="AMOC Description is too long" ControlToValidate="txtAMOCDescription"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                            Display="None" ErrorMessage="Enter whole part no. and description" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvSelectPart" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtSearch" Display="None" ErrorMessage="Enter part no."></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvComplianceDuringOthersRemark" runat="server" Display="None"
                                            ControlToValidate="txtComplianceDuringOthersRemark" ValidationGroup="a" ErrorMessage="Remark Required"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblStatus" runat="server" Text="<%# mADSBPlanningSupport.StatusName %>"
                                                        Style="margin-right: 5px" CssClass="control-label clsLabelAuto" Font-Bold="true"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upnlADSBTechRecordingDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend>Detail </legend>
                                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: -18px">
                                                            <tr>
                                                                <td>
                                                                    <span id="Span1" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" class="control-label clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBTechRecordingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                                        Enabled="false" Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px"
                                                                        onchange="ValidateDateText(this,'txtADSBTechRecordingDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtADSBTechRecordingDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtADSBTechRecordingDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtADSBTechRecordingDateWatermarkExtender" runat="server"
                                                                        TargetControlID="txtADSBTechRecordingDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNoStar" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="control-label clsLabelAuto">No.</span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtADSBTechRecordingText" runat="server" Text="<%# mADSBTechRecording.Text %>"
                                                                                    Enabled="false" CssClass="input-sm clsTextBox_Ajax" Height="25px" onfocus="WaterMark(this, event);"
                                                                                    onblur="WaterMark(this, event);" ToolTip="Enter No." MaxLength="25" Width="208px"> </asp:TextBox>
                                                                                <%--   <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                CompletionInterval="1" ServicePath="wfADSBTechRecording_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                TargetControlID="txtADSBTechRecordingText" UseContextKey="False">
                                                            </cc2:AutoCompleteExtender>--%>
                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtADSBTechRecordingText_Autocomplete"
                                                                                    runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0"
                                                                                    CompletionInterval="1" ServicePath="wfADSBTechRecording.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                                    CompletionSetCount="0" TargetControlID="txtADSBTechRecordingText" UseContextKey="False"
                                                                                    ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                                    OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                                    OnClientShowing="ClientShowing">
                                                                                </cc2:AutoCompleteExtender>
                                                                                <script type="text/jscript">
                                                                                    function SetContextKey() {
                                                                                        var autoComplete = $find('txtText_Autocomplete');
                                                                                        var TransTypeID = 'TransTypeID=<%=mADSBTechRecording.TransTypeID%>¿Date=<%=mADSBTechRecording.Date%>';
                                                                                        autoComplete.set_contextKey(TransTypeID);
                                                                                    }
                                                                                </script>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtADSBTechRecordingNo" runat="server" Text="<%# mADSBTechRecording.No %>"
                                                                                    Enabled="false" CssClass="input-sm clsTextBoxSmall_Ajax" Height="25px" MaxLength="8"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span6" class="control-label clsLabelStar"></span>&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <span id="lblADSBNO" class="control-label clsLabelAuto">AD/SB No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBNO" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="AD/SB No."
                                                                        Enabled="false" MaxLength="25" Text="<%# mADSBTechRecording.ADSBNo %>" Style="margin-bottom: 10px;"
                                                                        Width="208px" Height="25px"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp; <span id="Span9" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSubject" class="control-label clsLabelAuto">Subject</span> &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="input-sm clsTextBox3_Ajax"
                                                                        ToolTip="Subject" Enabled="false" MaxLength="25" Text="<%# mADSBTechRecording.ADSBSubject %>"
                                                                        Style="margin-bottom: 10px;" Height="25px"> </asp:TextBox>
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlADSBPlanningSupportDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend>Meeting Planning Details </legend>
                                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: -18px">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDate" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                                        Meeting Date</span> <span id="lblDateStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBPlanningSupportDate" runat="server" ClientIDMode="Static"
                                                                        CssClass="input-sm clsTextBox_Ajax" Style="margin-bottom: 5px; margin-top: 5px;"
                                                                        Height="25px" Text="" Width="110px" onchange="ValidateDateText(this,'txtADSBPlanningSupportDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtADSBPlanningSupportDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtADSBPlanningSupportDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtADSBPlanningSupportDateWatermarkExtender" runat="server"
                                                                        TargetControlID="txtADSBPlanningSupportDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td colspan="2" align="right">
                                                                    <asp:LinkButton ID="lnkHintQuestion" runat="server" ClientIDMode="Static" CssClass="clsLinkButton"
                                                                        CausesValidation="false">Hint Question(s)</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblRiskIdentification" class="control-label clsLabel" style="margin-left: 5px;
                                                                        margin-right: 5px;">Risk Identification</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRiskIdentification" runat="server" Text="<%# mADSBPlanningSupport.RiskIdentification %>"
                                                                        CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine" Style="margin-top: 5px;
                                                                        margin-bottom: 5px;"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblComplianceDuringOthersRemark" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        Remark</span><span id="lblComplianceDuringOthersRemarkStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtComplianceDuringOthersRemark" runat="server" Text="<%# mADSBPlanningSupport.ComplianceDuringOthersRemark %>"
                                                                        CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine" Style="margin-bottom: 5px;"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblComplianceDuring" class="control-label clsLabel" style="margin-left: 5px;
                                                                        margin-right: 5px;">Compliance During</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbComplianceDuring" runat="server" CssClass="clsComboBox_Ajax"
                                                                        Style="margin-bottom: 5px;" AutoPostBack="True" DataTextField="Name" DataValueField="ID"
                                                                        SelectedValue="<%# mADSBPlanningSupport.ComplianceDuring %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblVerification" class="control-label clsLabelAuto" style="margin-left: 5px;
                                                                        margin-right: 5px;">Verification</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbVerification" runat="server" CssClass="clsComboBox_Ajax"
                                                                        Style="margin-bottom: 5px;" AutoPostBack="True" DataTextField="Name" DataValueField="ID"
                                                                        SelectedValue="<%# mADSBPlanningSupport.VerificationID %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblFacility" runat="server" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        Facility</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbFacility" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                                        DataTextField="Name" DataValueField="ID" SelectedValue="<%# mADSBPlanningSupport.FacilityID %>"
                                                                        Style="margin-bottom: 5px;">
                                                                    </asp:DropDownList>
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlEffectivityDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend>Reconfirmation of AD/SB Effectivity </legend>
                                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: -18px">
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="dgEffectivityDetails" runat="server" AutoGenerateColumns="False"
                                                                        DataKeyNames="ID" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="True">
                                                                        <PagerSettings Mode="NextPreviousFirstLast" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ModelName" HeaderText="Applicable Model" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="PartName" HeaderText="Applicable Part" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="EffectiveDateFormatted" HeaderText="Effective Date" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="CompliancePeriod" HeaderText="Compliance Period" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:TemplateField HeaderText="Compliance Period while Planning" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtCompliancePeriodInMeeting" runat="server" CssClass="input-sm clsTextBoxMultiLine"
                                                                                        TextMode="MultiLine" Height="25px" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem,"CompliancePeriodInMeeting") %>'> </asp:TextBox>
                                                                                    <asp:CustomValidator ID="cvBrokenRules1" runat="server" ControlToValidate="txtCompliancePeriodInMeeting"
                                                                                        Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="left" />
                                                                                <ItemStyle HorizontalAlign="left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Remark while Planning" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtRemarkInMeeting" runat="server" CssClass="input-sm clsTextBoxMultiLine1_Ajax"
                                                                                        TextMode="MultiLine" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem,"RemarkInMeeting") %>'
                                                                                        MaxLength="400"> </asp:TextBox>
                                                                                    <asp:CustomValidator ID="cvBrokenRules2" runat="server" ControlToValidate="txtRemarkInMeeting"
                                                                                        Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="left" />
                                                                                <ItemStyle HorizontalAlign="left" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                                    </asp:GridView>
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlAMOCInvoking" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">AMOC Invoking
                                                            <asp:CheckBox ID="chkIsAMOCInvokingRequired" runat="server" TextAlign="Left" CssClass="input-sm"
                                                                Checked="<%# mADSBPlanningSupport.IsAMOCInvokingRequired %>" />
                                                        </legend>
                                                        <table width="99%" style="margin-top: -18px; margin-right: 10px">
                                                            <tr>
                                                                <td>
                                                                    <span id="Span5" class="control-label clsLabel" style="margin-left: 5px;">Description</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAMOCDescription" runat="server" Text="<%# mADSBPlanningSupport.AMOCDescription %>"
                                                                        CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine" Style="margin-bottom: 5px;"> </asp:TextBox>
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDesignateMemberResponsibility" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">Designate Member Responsibility </legend>
                                                        <table width="99%" style="margin-top: -18px; margin-right: 10px">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblOEMDesignateMember" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        OEM</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbOEMDesignateMember" runat="server" CssClass="input-sm clsComboBox_Ajax"
                                                                        Height="25px" Style="margin-bottom: 5px;" AutoPostBack="True" DataTextField="EmpNoName"
                                                                        DataValueField="ID" SelectedValue="<%# mADSBPlanningSupport.OEMDesignateMemberID %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNAADesignateMember" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        NAA</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbNAADesignateMember" runat="server" CssClass="input-sm clsComboBox_Ajax"
                                                                        Height="25px" AutoPostBack="True" DataTextField="EmpNoName" DataValueField="ID"
                                                                        SelectedValue="<%# mADSBPlanningSupport.NAADesignateMemberID %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblOtherDesignateMemberData" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        Other(s) </span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtOtherDesignateMemberData" runat="server" Text="<%# mADSBPlanningSupport.OtherDesignateMemberData %>"
                                                                                    CssClass="input-sm clsTextBox_Ajax" Height="25px" MaxLength="500" Width="208px"
                                                                                    Style="margin-right: 5px;" TextMode="MultiLine"> </asp:TextBox>
                                                                            </td>
                                                                            <%--   <td>
                                                                                <span id="lblOtherDesignateMember" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                                    Other Member</span>
                                                                            </td>--%>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbOtherDesignateMember" runat="server" CssClass="input-sm clsComboBox_Ajax"
                                                                                    Height="25px" AutoPostBack="True" DataTextField="EmpNoName" DataValueField="ID"
                                                                                    SelectedValue="<%# mADSBPlanningSupport.OtherDesignateMemberID %>">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTechRecordImplications" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">Tech Record Implications </legend>&nbsp;
                                                        <asp:TextBox ID="txtTechPubRecordImplications" runat="server" Text="<%# mADSBPlanningSupport.TechPubRecordImplications %>"
                                                            CssClass="input-sm clsTextBoxMultiLine1_Ajax" Height="50px" MaxLength="500" TextMode="MultiLine"
                                                            Style="margin-bottom: 5px; margin-top: -18px;"> </asp:TextBox>
                                                    </fieldset>
                                                </td>
                                                <td valign="top">
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">Audit Require ?
                                                            <asp:CheckBox ID="chkIsAuditRequired" runat="server" TextAlign="Left" CssClass="input-sm"
                                                                Checked="<%# mADSBPlanningSupport.IsAuditRequired %>" /></legend>&nbsp;
                                                        <asp:TextBox ID="txtAuditDescription" runat="server" Text="<%# mADSBPlanningSupport.AuditDescription %>"
                                                            Height="50px" CssClass="input-sm  clsTextBoxMultiLine1_Ajax" MaxLength="500"
                                                            TextMode="MultiLine" Style="margin-bottom: 5px; margin-top: -18px;"> </asp:TextBox>
                                                    </fieldset>
                                                </td>
                                                <td valign="top">
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">Area Of Concern </legend>&nbsp;
                                                        <asp:TextBox ID="txtAreaOfConcern" runat="server" Text="<%# mADSBPlanningSupport.AreaOfConcern %>"
                                                            CssClass="input-sm  clsTextBoxMultiLine1_Ajax" MaxLength="500" TextMode="MultiLine"
                                                            Style="margin-bottom: 5px; margin-top: -18px;" Height="50px"> </asp:TextBox>
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
                                <asp:UpdatePanel runat="server" ID="upnlMaterialRequirement" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet " style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">Material Requirement </legend>
                                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: -18px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            &nbsp;&nbsp;
                                                                            <asp:TextBox ID="txtSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                                CssClass="input-sm  clsTextBox_Ajax" onChange="SetPartIdonChange()" AutoPostBack="true"
                                                                                Style="margin-bottom: 5px; margin-top: -15px;" Height="25px" Width="208px"></asp:TextBox>
                                                                            <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                CompletionInterval="1" ServicePath="wfADSBPlanningSupport.aspx" ServiceMethod="GetPartNoDescriptionList"
                                                                                TargetControlID="txtSearch" OnClientItemSelected="SetID" UseContextKey="False"
                                                                                ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                                OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                                OnClientShowing="ClientShowing">
                                                                            </cc2:AutoCompleteExtender>
                                                                            <cc2:TextBoxWatermarkExtender ID="txtSearchWatermarkExtender" runat="server" TargetControlID="txtSearch"
                                                                                WatermarkCssClass="clsTextBox1_Ajax" WatermarkText="Select Material Here To Add">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                            <span id="lblInstr" class="clsLabelAuto" style="color: Brown; font-size: 9px; font-weight: bold;
                                                                                margin-right: 5px; font-style: italic" runat="server" visible="false">Enter whole
                                                                                part no. and description </span>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="99%" style="border-width: 1px; margin-left: 5px">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="dgItemList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                            DataKeyNames="ID" ShowHeaderWhenEmpty="True" CssClass="table table-striped table-bordered table-hover"
                                                                                            PageSize="25" AllowPaging="True" Style="margin-right: 8px; margin-top: 5px;">
                                                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                            <Columns>
                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="ItemID"></asp:BoundField>
                                                                                                <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                        Font-Underline="False" Wrap="true" />
                                                                                                    <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                        Font-Underline="False" Wrap="true" CssClass="TextBreak" />
                                                                                                    <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:TemplateField HeaderText="Qty.">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax" MaxLength="8"
                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"Qty") %>' Height="25px"    OnTextChanged="AddAttributesForGridControls" CausesValidation="false"></asp:TextBox>
                                                                                                        <asp:CustomValidator ID="cvBrokenRules" runat="server" ControlToValidate="txtQty"
                                                                                                            Display="None"></asp:CustomValidator>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Lead Time in Days">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtLeadTime" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                                                            MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"LeadTime") %>' Height="25px" OnTextChanged="AddAttributesForGridControls" CausesValidation="false"></asp:TextBox>
                                                                                                        <asp:CustomValidator ID="cvLeadTime" runat="server" ControlToValidate="txtLeadTime"
                                                                                                            Display="None"></asp:CustomValidator>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlMeetingParticipantsList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td valign="top">
                                                    <fieldset class="clsFieldSet " style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">Present Participant(s) </legend>
                                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: -18px">
                                                            <tr>
                                                                <td style="height: 15px">
                                                                    <asp:GridView ID="dgMeetingParticipantsList" runat="server" AllowPaging="true" AllowSorting="True"
                                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover"
                                                                        DataKeyNames="ID" GridLines="Both" PageSize="25" ShowHeaderWhenEmpty="True" Style="margin-right: 8px;
                                                                        margin-top: 5px;">
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                        <Columns>
                                                                            <%--0--%>
                                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                            <%--1--%>
                                                                            <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" Visible="False" />
                                                                            <%--2--%>
                                                                            <asp:BoundField DataField="EmployeeName" HeaderText="Participant Name">
                                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                    Font-Underline="False" Wrap="False" />
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--3--%>
                                                                            <asp:BoundField DataField="EmployeeEmail" HeaderText="Email">
                                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                    Font-Underline="False" Wrap="False" />
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--4--%>
                                                                            <asp:BoundField DataField="MailSendDateTime" HeaderText="Mail Sent On">
                                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                    Font-Underline="False" Wrap="False" />
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--5--%>
                                                                            <asp:TemplateField HeaderText="Approved/Not Approved">
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdbApproved" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Approved") %>'
                                                                                        CssClass="clsRadioButton" Enabled='<%#  Eval("EnabledDisabled")%>' GroupName="a" />
                                                                                    <asp:Label ID="lblApproved" runat="server" class="control-label" Style="margin-left: 5px;
                                                                                        margin-right: 5px;">
                                                                                                            Approved</asp:Label>
                                                                                    <%--<asp:ImageButton ID="ApprovedStatus" runat="server" Visible="false" Style="height: 30px;
                                                                                                        width: 30px" ImageUrl="~/images/yes.png" />--%>&nbsp;&nbsp;<asp:RadioButton ID="rdbNotApproved"
                                                                                                            runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "NotApproved") %>'
                                                                                                            CssClass="clsRadioButton" Enabled='<%#  Eval("EnabledDisabled")%>' GroupName="a" />
                                                                                    <asp:Label ID="lblNotApproved" runat="server" class="control-label" Style="margin-left: 5px;
                                                                                        margin-right: 5px;">
                                                                                                                    Not Approved</asp:Label>
                                                                                    <%--<asp:ImageButton ID="lblNotApprovedImageButton" runat="server" Visible="false" Style="height: 30px;
                                                                                                        width: 30px" ImageUrl="~/images/no.png" />--%>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <%--6--%>
                                                                            <asp:TemplateField HeaderStyle-ForeColor="black" HeaderText="Remark">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtApprovedRemark" runat="server" AutoPostBack="true" CssClass="input-sm clsTextBoxMultiLine"
                                                                                        Enabled='<%#  Eval("EnabledDisabled")%>' Height="25px" MaxLength="400" Text='<%# DataBinder.Eval(Container.DataItem,"ApprovedRemark") %>'
                                                                                        TextMode="MultiLine"> </asp:TextBox>
                                                                                    <asp:CustomValidator ID="cvBrokenRules3" runat="server" ControlToValidate="txtApprovedRemark"
                                                                                        Display="None" OnServerValidate="CustomValidate2"></asp:CustomValidator>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--7--%>
                                                                            <asp:BoundField DataField="EnabledDisabled" HeaderStyle-CssClass="hideGridColumn"
                                                                                HeaderText="EnabledDisabled" ItemStyle-CssClass="hideGridColumn" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td valign="top">
                                                    <fieldset class="clsFieldSet " style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend class="control-label clsLabel">Budgetary Cost Implications </legend>
                                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: -18px">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblLabourCost" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        Labour Cost</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLabourCost" runat="server" CssClass="input-sm clsTextBoxRightAlignSmall_Ajax"
                                                                        Height="25px" MaxLength="12" Text="<%# mADSBPlanningSupport.LabourCost %>" Style="margin-bottom: 10px;
                                                                        margin-left: 3px;"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblMaterialCost" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        Material Cost</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMaterialCost" runat="server" CssClass="input-sm clsTextBoxRightAlignSmall_Ajax"
                                                                        Height="25px" MaxLength="12" Text="<%# mADSBPlanningSupport.MaterialCost %>"
                                                                        Style="margin-bottom: 10px; margin-left: 3px;"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblFacilityCost" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        Facility Cost</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFacilityCost" runat="server" CssClass="input-sm clsTextBoxRightAlignSmall_Ajax"
                                                                        Height="25px" MaxLength="12" Text="<%# mADSBPlanningSupport.FacilityCost %>"
                                                                        Style="margin-bottom: 10px; margin-left: 3px;"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSubContratctedCost" class="control-label clsLabelAuto" style="margin-left: 5px;">
                                                                        Sub Cost</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSubContratctedCost" runat="server" CssClass="input-sm clsTextBoxRightAlignSmall_Ajax"
                                                                        Height="25px" MaxLength="12" Text="<%# mADSBPlanningSupport.SubContratctedCost %>"
                                                                        Style="margin-bottom: 10px; margin-left: 3px;"> </asp:TextBox>
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
                            <td>
                                <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: 5px">
                                    <tr>
                                        <td valign="top">
                                            <fieldset class="clsFieldSet " style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                <legend class="control-label clsLabel">File Attachment(s) </legend>
                                                <table width="99%" style="border-width: 1px; margin-left: 5px; margin-top: -18px">
                                                    <tr>
                                                        <td>
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
                                                                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" CausesValidation="false"/>
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
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-sm"   Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Print" ClientIDMode="Static" ToolTip="Click to Print" CausesValidation="false" Enabled="<%# Not mADSBPlanningSupport.IsNew %>">
                                        </asp:Button>
                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Authorize" ToolTip="Click to authorize" CausesValidation="true"
                                            ValidationGroup="a" />
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Save" CausesValidation="true" ValidationGroup="a"
                                            ToolTip="Click to Save" />
                                        <asp:Button ID="btnSaveAndClose" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Save And Close" CausesValidation="true" ValidationGroup="a"
                                            ToolTip="Click to Save" Visible="false" />
                                        <asp:Button ID="btnSendMail" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Send Mail" ToolTip="Send Mail" Enabled="<%# Not mADSBPlanningSupport.IsNew and mADSBPlanningSupport.StatusID >= 2 %>"
                                            CausesValidation="false" />
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
    <%--Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }
            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml; //Boolean Expression ? First Statement : Second Statement Ternary operator ?:
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtSearch_Autocomplete") {
                textbox = document.getElementById('hdnpartId');
            }
            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetPartIdonChange() {
            var popup = $find("txtSearch_Autocomplete");
            var complist = popup.get_completionList();
            var text = $("#txtSearch").val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    var textbox = document.getElementById('hdnpartId');
                    textbox.value = val.toString();
                    return;
                }
            }
        }
    </script>
    <!-- End Autocomplete functions to set id-->
    <!--Hint Question  Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyHintQuestion" Text="HintQuestion" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlHintQuestion" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeHintQuestion" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlHintQuestion" runat="server" TargetControlID="btnDummyHintQuestion"
        PopupControlID="pnlHintQuestion" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameHintQuestionComplete() {
            $("#btnDummyHintQuestion").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenHintQuestionWindow() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeHintQuestion").attr("src", "wfHintQuestion.aspx?Type=pup");

                //                var windowheight = $(window).height();
                //                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                //                var margintopm = (windowheight / 2) - (tempMargtop / 2);
                $pos = $("#<%=lblFacility.ClientID%>").position();
                var top = $pos.top;
                var left = $pos.left - 400;
                var searchHeight = $("#<%=lblFacility.ClientID%>").height();
                var margin = top + searchHeight;

                var height = $("#tblmain").outerHeight();
                var h = margin - height;
                $("#mdlHintQuestion").animate({ marginTop: h, marginLeft: left + 5 }, 100, 'swing', function () {
                    $("#mdlHintQuestion").delay(9000).fadeOut();

                });
                $("#btnDummyHintQuestion").click();
                $get("AjaxLoader").style.visibility = 'hidden';
                //                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForHintQuestion() {
            var HintQuestionwindow = $find("<%=mdlHintQuestion.ClientID %>");
            //close popup window
            HintQuestionwindow.hide();
            //release resources
            $("#IframeHintQuestion").attr("src", "JavaScript:''");
            //call button click
            $("#hdnHintQuestion").click();
        }
    </script>
    <!-- End-->
    </form>
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
