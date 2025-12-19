<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfADSBMonitoring.aspx.vb"
    Inherits="Flypal.wfADSBMonitoring" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AD/SB Monitoring</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <%-- <script type="text/javascript" src="jquery-1.6.1.min.js"></script>--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="StickyNote/css/style.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="Styles.css" id="Link1" type="text/css" rel="stylesheet" />
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
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
    <script type="text/javascript">
        function resizeTextBox(txt) {
            txt.style.height = "25px";
            //   txt.style.height = (1 + txt.scrollHeight) + "px";

        }
        function OnResize(txt) {
            $(txt).animate({ width: 185, height: txt.scrollHeight }, "fast");
        }
        function OnLostResize(txt) {
            $(txt).animate({ width: 185, height: 25 }, "fast");
        }
    </script>
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
                                        <span id="lblTitle" style="font-size: 18px; font-weight: 100" class="text-text-primary clstitle1"
                                            runat="server">List Of AD/SB(s)</span>
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
                                            ValidationGroup="a" ErrorMessage="Select Date" ControlToValidate="txtADSBTechRecordingDate"
                                            Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvADSBNo" runat="server" Display="None" ControlToValidate="txtADSBNO"
                                            ValidationGroup="a" ErrorMessage="AD/SB No. Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" Display="None" ControlToValidate="txtSubject"
                                            ValidationGroup="a" ErrorMessage="Subject Required"></asp:RequiredFieldValidator>
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
                                                    <asp:Label ID="lblStatus" runat="server" Text="<%# mADSBTechRecording.StatusNameWithConfigured %>"
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
                                                        <table width="99%" style="margin-top: -22px">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDateStar" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDate" class="control-label clsLabelAuto">Date</span>
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
                                                                    <span id="Span1" class="control-label clsLabelStar"></span>&nbsp;&nbsp;
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
                                                                    &nbsp; <span id="Span2" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSubject" class="control-label clsLabelAuto">Subject</span> &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="input-sm clsTextBox3_Ajax"
                                                                        Height="25px" ToolTip="Subject" Enabled="false" MaxLength="25" Text="<%# mADSBTechRecording.ADSBSubject %>"
                                                                        Style="margin-bottom: 10px;"> </asp:TextBox>
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
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upnlADSBMonitoring" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-right: 5px;"
                                            valign="top;">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend>Compliance </legend>
                                                        <table width="99%" valign="top" style="margin-top: -16px">
                                                            <tr>
                                                                <td>
                                                                    &nbsp; <span id="Span7" class="control-label clsLabelHeader" runat="server">Effectivity
                                                                        Detail(s)</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="dgEffectivityDetails" runat="server" AutoGenerateColumns="False"
                                                                        CssClass="table table-striped table-bordered" ShowHeaderWhenEmpty="True">
                                                                        <PagerSettings Mode="NextPreviousFirstLast" />
                                                                        <RowStyle CssClass="table table-striped table-bordered table-hover" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <AlternatingRowStyle CssClass="table table-striped table-bordered table-hover" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="PartName" HeaderText="Part" SortExpression="PartName">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="EffectiveDateFormatted" HeaderText="Effective Date">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="CompliancePeriodInMeeting" HeaderText="Compliance Period">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Comply" ItemStyle-HorizontalAlign="Center"
                                                                                Visible="false" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="Comply" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                        CommandName="ComplyRec" ImageUrl="~/images/Comply.jpg" Style="height: 20px; width: 20px" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Configure" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="lnkConfigure" CommandArgument='<%# Eval("SrNo") %>'
                                                                                        ToolTip="Let's Configure" CommandName="ConfigureRec" Text="Config" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                DataField="IsConfigured" HeaderText="IsConfigured"></asp:BoundField>
                                                                            <asp:ButtonField CommandName="History" HeaderText="History" Text="History" >
                                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                                            </asp:ButtonField>
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
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px; margin-right: 5px;" Text="Close" ToolTip="Click to Close" />
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnModelModMaster" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="false" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnModelMonitorModList" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnModMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <!-- Change Monitoring -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyMonitoring" Text="Dummy Monitoring" />
    </div>
    <asp:Panel runat="server" ID="pnlChangeMonitoring">
        <div>
            <fieldset class="clsFieldSet" style="border-width: 1px; margin: -2px">
                <table class="clstablelistout" id="Table2">
                    <tr>
                        <td align="left" colspan="1">
                            <span id="Span3" class="clstitle1">Monitoring </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            <asp:UpdatePanel runat="server" ID="upnlEffectivityDet" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset class="clsFieldSet" style="border-width: 1px;">
                                        <legend>Effectivity Detail(s) </legend>
                                        <table class="clstablelistin" id="Table1" style="margin-top: -20px;">
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblPartModelNo" runat="server" class="control-label">Part No. </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartModelNo" runat="server" CssClass="input-sm clsTextBox_Ajax"
                                                        ToolTip="Part No. /Model No." Enabled="false" MaxLength="25" Style="margin-bottom: 10px;"
                                                        Width="208px"> </asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp; <span id="lblSerialNo" class="control-label">Serial No. </span>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="input-sm clsTextBox_Ajax"
                                                        ToolTip="Serial No." Enabled="false" MaxLength="25" Style="margin-bottom: 10px;"
                                                        Width="208px"> </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            <asp:UpdatePanel runat="server" ID="upnlMonitoring" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset class="clsFieldSet" style="border-width: 1px;">
                                        <legend>Detail(s) </legend>
                                        <table class="clstablelistin" id="Table3" style="margin-top: -20px;">
                                            <tr>
                                                <td colspan="3" align="left">
                                                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="2" runat="server"
                                                        CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true"
                                                        ErrorMessage="Enter EO. No." CssClass="clsLabelAuto" ControlToValidate="txtEONo"
                                                        Display="None" ValidationGroup="2" runat="server" />
                                                    <asp:CustomValidator ID="cvIssueDate" runat="server" OnServerValidate="CustomValidate"
                                                        ValidationGroup="a" ErrorMessage="Select EO Date" ControlToValidate="txtEONo"
                                                        Display="None"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <span id="Span8" class="control-label"><b>Scheduling :-</b> </span>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <span id="Span18" class="control-label clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblEONo" class="control-label">EO No. </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEONo" runat="server" CssClass="input-sm clsTextBox_Ajax" Height="25px"
                                                        Text="<%# mADSBMonitoring.EONO %>"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp; <span id="Span4" class="control-label">EO Date </span>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEODate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                        Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px" onchange="ValidateDateText(this,'txtEODateWatermarkExtender','true');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtEODate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEODate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="txtEODateWatermarkExtender" runat="server" TargetControlID="txtEODate"
                                                        WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblFacility" class="control-label">Facility Name</span> &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFacility" runat="server" CssClass="input-sm clsTextBox_Ajax"
                                                        Style="margin-top: 5px" Text="<%# mADSBMonitoring.FacilityName %>" Height="25px"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp; <span id="Span5" class="control-label">Location</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="input-sm clsTextBox_Ajax"
                                                        Style="margin-top: 5px" Text="<%# mADSBMonitoring.Location %>" Height="25px"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span6" class="control-label">Planned Date</span> &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPlannedDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                        Style="margin-bottom: 5px; margin-top: 5px" Height="25px" Text="" Width="110px"
                                                        onchange="ValidateDateText(this,'txtPlannedDateWatermarkExtender','true');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtPlannedDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtPlannedDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="txtPlannedDateWatermarkExtender" runat="server"
                                                        TargetControlID="txtPlannedDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <span id="Span9" class="control-label"><b>Compliance :-</b> </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span10" class="control-label">Date of Compliance </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtComplianceDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                        Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px" onchange="ValidateDateText(this,'txtComplianceDateWatermarkExtender','true');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtComplianceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtComplianceDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="txtComplianceDateWatermarkExtender" runat="server"
                                                        TargetControlID="txtComplianceDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    &nbsp; <span id="Span11" class="control-label">Complaince Fully/Part </span>&nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton runat="server" ID="rdbPartially" GroupName="app" Checked="<%# mADSBMonitoring.IsPartially %>"
                                                                    AutoPostBack="true" Text="Partially" />
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbFully" runat="server" AutoPostBack="true" Checked="<%# not mADSBMonitoring.IsPartially %>"
                                                                    GroupName="app" Text="Fully" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span12" class="control-label">Hrs. </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHours" runat="server" CssClass="input-sm clsTextBoxSmall_Ajax"
                                                        Height="25px" MaxLength="8" Text="<%# mADSBMonitoring.AircraftHoursFormatted %>"
                                                        ToolTip="Enter Hours" Style="border-top-left-radius: 4px; border-top-right-radius: 4px;
                                                        border-bottom-left-radius: 4px; border-bottom-right-radius: 4px;">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp; <span id="Span13" class="control-label">Landings </span>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLandings" runat="server" CssClass="input-sm clsTextBoxSmall_Ajax"
                                                        Height="25px" MaxLength="8" Text="<%# mADSBMonitoring.Landings %>" ToolTip="Enter Landings"
                                                        Style="border-top-left-radius: 4px; border-top-right-radius: 4px; border-bottom-left-radius: 4px;
                                                        border-bottom-right-radius: 4px;">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span14" class="control-label">Cycles</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCycles" runat="server" CssClass="input-sm clsTextBoxSmall_Ajax"
                                                        Height="25px" MaxLength="8" Text="<%# mADSBMonitoring.Cycles %>" ToolTip="Enter Cycles"
                                                        Style="border-top-left-radius: 4px; border-top-right-radius: 4px; border-bottom-left-radius: 4px;
                                                        border-bottom-right-radius: 4px; margin-top: 5px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span16" class="control-label">Audit Required?</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkAuditRequired" class="control-label" Text=""
                                                        Checked="<%# mADSBMonitoring.IsAuditRequired %>" />
                                                </td>
                                                <td>
                                                    &nbsp; <span id="Span15" class="control-label">Audit Due Date </span>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAuditDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                        Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px" onchange="ValidateDateText(this,'txtAuditDateWatermarkExtender','true');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtAuditDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAuditDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="txtAuditDateWatermarkExtender" runat="server" TargetControlID="txtAuditDate"
                                                        WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span17" class="control-label">Re-Inspection/Re-Current Check </span>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkReInspection" TextAlign="left" class="control-label"
                                                        Text="" Checked="<%# mADSBMonitoring.ReInspection %>" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" valign="top">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="99%" style="border-width: 1px; margin-left: 5px; margin-right: 5px;"
                                        valign="top;">
                                        <tr>
                                            <td>
                                                <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 5px; margin-left: -2px;
                                                    margin-right: 5px;">
                                                    <legend>File Attachments</legend>
                                                    <asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="99%" style="border-width: 1px; margin-left: 5px">
                                                                <tr>
                                                                    <td style="height: 15px">
                                                                        <asp:UpdatePanel ID="upnldgAttachment" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="dgAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                                    Visible="true" CssClass="table table-striped table-bordered table-hover" DataKeyNames="ID"
                                                                                    ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="False" AutoGenerateColumns="false">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                    <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-ForeColor="black">
                                                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" Width="10px" ForeColor="black">
                                                                                            </HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
                                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="File Name" HeaderStyle-ForeColor="black">
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtFileName" runat="server" CssClass="input-sm clsTextBox3_Ajax"
                                                                                                    MaxLength="100" ClientIDMode="Static" Height="25px" ToolTip="Enter File Name To Be Attached"
                                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>' Width="300px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
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
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">
                            <table id="Table4" cellspacing="1" cellpadding="1" style="margin-top: 5px; margin-bottom: 5px">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnMonitoringOk" runat="server" CausesValidation="True" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px; margin-right: 5px;" Text="Save" ToolTip="Click to add new Monitoring"
                                            ValidationGroup="2"></asp:Button>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="btnMonitoringClose" runat="server" CausesValidation="False" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px; margin-right: 5px;" Text="Close" ToolTip="Click to go back to the previous page">
                                        </asp:Button>
                                        &nbsp;
                                        <asp:Button ID="Button1" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpChangeMonitoring" runat="server" TargetControlID="btnDummyMonitoring"
        PopupControlID="pnlChangeMonitoring" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End Change Monitoring -->
    <!--Model Mod Master Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyModelModMaster" Text="Model Mod Master" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlModelModMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeModelModMaster" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupModelModMaster" runat="server" TargetControlID="btnDummyModelModMaster"
        PopupControlID="pnlModelModMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameModelModMasterStateComplete() {
            $("#btnDummyModelModMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenModelModMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeModelModMaster").attr("src", "wfModelMonitorMod_Ajax.aspx?Type=pup&GChildPage4=wfADSBMonitoring.aspx");

                if (!$.browser.msie) {
                    $("#btnDummyModelModMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForModelModMaster() {
            var ModelModMasterwindow = $find("<%=mdlPopupModelModMaster.ClientID %>");
            //close Model Mod Master popup window
            ModelModMasterwindow.hide();
            //           release resources
            $("#IframeModelModMaster").attr("src", "JavaScript:''");
            //call Model Mod Master image button
            $("#hdnBtnModelModMaster").click();
        }
    </script>
    <!-- End-->
    <!--Model Monitor Mod List Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyModelMonitorModList" Text="Model Service Master"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlModelMonitorModList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeModelMonitorModList" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupModelMonitorModList" runat="server" TargetControlID="btnDummyModelMonitorModList"
        PopupControlID="pnlModelMonitorModList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameModelMonitorModListStateComplete() {
            $("#btnDummyModelMonitorModList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenModelMonitorModListWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeModelMonitorModList").attr("src", "wfModelMonitorModList_Ajax.aspx?Type=pup&GChildPage2=wfADSBMonitoring.aspx");

                if (!$.browser.msie) {
                    $("#btnDummyModelMonitorModList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                //});
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForModelMonitorModList() {
            var ModelMonitorModListwindow = $find("<%=mdlPopupModelMonitorModList.ClientID %>");
            //close Monitor Mod List popup window
            ModelMonitorModListwindow.hide();
            //           release resources
            $("#IframeModelMonitorModList").attr("src", "JavaScript:''");
            //call Monitor Mod List image button
            $("#hdnBtnModelMonitorModList").click();
        }
    </script>
    <!-- Model Monitor Mod List Popup Window End -->
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

        //        $(document).ready(function () {
        //            $("#btnSelectFiles").live("click", function () {
        //                try {
        //                    $get("AjaxLoader").style.visibility = 'visible';
        //                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
        //                    //                        $("#IFileUpload").ready(function () {
        //                    //                            $("#btnDummyFileUpload").click();
        //                    //                            $get("AjaxLoader").style.visibility = 'hidden';
        //                    //                        });
        //                    if (!$.browser.msie) {
        //                        $("#btnDummyFileUpload").click();
        //                        $get("AjaxLoader").style.visibility = 'hidden';
        //                    }

        //                    return false;
        //                } catch (e) {
        //                    alert(e);
        //                }


        //            });
        //                }); 
        function OpenFileUploadWindow() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                //                        $("#IFileUpload").ready(function () {
                //                            $("#btnDummyFileUpload").click();
                //                            $get("AjaxLoader").style.visibility = 'hidden';
                //                        });
                //                if (!$.browser.msie) {
                //                    $("#btnDummyFileUpload").click();
                //                    $get("AjaxLoader").style.visibility = 'hidden';
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
    <!-- End -->
    <!-- End File Upload Modal Dialog-->
    <!--Part ModMaster Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyModMaster" Text="Dummy ModMaster" ClientIDMode="Static">
        </asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlModMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeModMaster" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupModMaster" runat="server" TargetControlID="btnDummyModMaster"
        PopupControlID="pnlModMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameModMasterStateComplete() {
            $("#btnDummyModMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenModMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                //var GChildPage2 = window.location.search.split('GChildPage2')[1].split('=')[1];
                $("#IframeModMaster").attr("src", "wfPartMonitorMod_AJAX.aspx?Type=pup&GChildPage4=wfADSBMonitoring.aspx&GChildPage2=wfADSBMonitoring.aspx");
                // $("#IframeModMaster").load(function () {
                //                    var doc = IframeModMaster.window;
                //                    IframeModMaster.SetPageLayout();

                if (!$.browser.msie) {
                    $("#btnDummyModMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForModMaster() {
            var ModMasterwindow = $find("<%=mdlPopupModMaster.ClientID %>");
            //close ModMaster popup window
            ModMasterwindow.hide();
            //           release resources
            $("#IframeModMaster").attr("src", "JavaScript:''");
            //call ModMaster image button
            $("#hdnBtnModMaster").click();
        }
    </script>
    <!-- End-->
    <!--Part Monitor Mod List Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPartMonitorModList" Text="Part Service Master"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPartMonitorModList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframePartMonitorModList" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupPartMonitorModList" runat="server" TargetControlID="btnDummyPartMonitorModList"
        PopupControlID="pnlPartMonitorModList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFramePartMonitorModListStateComplete() {
            $("#btnDummyPartMonitorModList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenPartMonitorModListWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframePartMonitorModList").attr("src", "wfPartMonitorModList_Ajax.aspx?Type=pup&GChildPage4=wfADSBMonitoring.aspx");

                if (!$.browser.msie) {
                    $("#btnDummyPartMonitorModList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                //});
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForPartMonitorModList() {
            var PartMonitorModListwindow = $find("<%=mdlPopupPartMonitorModList.ClientID %>");
            //close Monitor Mod List popup window
            PartMonitorModListwindow.hide();
            //           release resources
            $("#IframePartMonitorModList").attr("src", "JavaScript:''");
            //call Monitor Mod List image button
            $("#hdnBtnPartMonitorModList").click();
        }
    </script>
    <!-- Part Monitor Mod List Popup Window End -->
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

      <!--Assembly Directive History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyModHistory" Text="Mod History" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlModHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeModHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupModHistory" runat="server" TargetControlID="btnDummyModHistory"
        PopupControlID="pnlModHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        //function IFrameDirectiveHistoryStateComplete() {
        function IFrameDirectiveHistoryStateComplete() {
            $("#btnDummyModHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenModHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeModHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorModStatusList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyModHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        //function ParentCallBackFunctionForDirectiveHistory() {
        function ParentCallBackFunctionForDirectiveHistory() {
            var ModHistorywindow = $find("<%=mdlPopupModHistory.ClientID %>");
            //close Mod History popup window
            ModHistorywindow.hide();
            //           release resources
            $("#IframeModHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnModHistory").click();
        }
    </script>
    <!-- End-->

    <!-- Component Comply History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemHistory" Text="TaskCard Tool" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemHistory" runat="server" TargetControlID="btnDummyRemHistory"
        PopupControlID="pnlRemHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        //function IFrameRemHistoryStateComplete() {
        function IFrameCompDirectiveHistoryStateComplete() {

            $("#btnDummyRemHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenDirectiveHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemHistory").attr("src", "wfUpdateComplyHistoryCompMonitorModStatusList_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        //  function ParentCallBackFunctionForRemHistory() {
        function ParentCallBackFunctionForCompDirectiveHistory() {
            var RemHistorywindow = $find("<%=mdlPopupRemHistory.ClientID %>");
            //close Removal History popup window
            RemHistorywindow.hide();
            //           release resources
            $("#IframeRemHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnModHistory").click();
        }
    </script>
    <!-- End-->
    </form>
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
</body>
</html>
