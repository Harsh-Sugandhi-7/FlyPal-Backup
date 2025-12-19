<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfADSBTechRecording.aspx.vb"
    Inherits="Flypal.wfADSBTechRecording" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AD/SBs</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="StickyNote/css/style.css" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript">

        function WaterMark(txt, evt) {
            var defaultText = "Select your prefix";
            if (txt.value.length == 0 && evt.type == "blur") {
                txt.style.color = "gray";
                txt.value = defaultText;
            }
            if (txt.value == defaultText && evt.type == "focus") {
                txt.style.color = "black";
                txt.value = "";
            }
        }
        $(document).ready(function () {
            var txt = document.getElementById("<%=txtADSBTechRecordingText.ClientID%>");
            var defaultText = "Select your prefix";
            if (txt.value.length == 0) {
                txt.style.color = "gray";
                txt.value = defaultText;
            }
        });
   
     
    </script>
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
                                        <asp:CustomValidator ID="cvIssueDate" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Select Issue Date" ControlToValidate="txtADSBNO"
                                            Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvADSBNo" runat="server" Display="None" ControlToValidate="txtADSBNO"
                                            ValidationGroup="a" ErrorMessage="AD/SB No. Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" Display="None" ControlToValidate="txtSubject"
                                            ValidationGroup="a" ErrorMessage="Subject Required"></asp:RequiredFieldValidator>
                                             <asp:RequiredFieldValidator ID="rfvDesc" runat="server" Display="None" ControlToValidate="txtDescription"
                                            ValidationGroup="a" ErrorMessage="Description Required"></asp:RequiredFieldValidator>
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
                                                    <asp:Label ID="lblStatus" runat="server" Text="<%# mADSBTechRecording.StatusName %>"
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
                                                                    <span id="lblDateStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDate" class="control-label clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBTechRecordingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                                        Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px" onchange="ValidateDateText(this,'txtADSBTechRecordingDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtADSBTechRecordingDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtADSBTechRecordingDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtADSBTechRecordingDateWatermarkExtender" runat="server"
                                                                        TargetControlID="txtADSBTechRecordingDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNoStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="control-label clsLabelAuto">No.</span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtADSBTechRecordingText" runat="server" Text="<%# mADSBTechRecording.Text %>"
                                                                                    CssClass="input-sm clsTextBox_Ajax" Height="25px" onfocus="WaterMark(this, event);"
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
                                                                                    CssClass="input-sm clsTextBoxSmall_Ajax" Height="25px" MaxLength="8"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span1" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblADSBNO" class="control-label clsLabelAuto">AD/SB No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBNO" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="AD/SB No."
                                                                        MaxLength="25" Text="<%# mADSBTechRecording.ADSBNo %>" Style="margin-bottom: 10px;"
                                                                        Width="208px" Height="25px" TextMode="MultiLine"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSubject" class="control-label clsLabelAuto">Subject</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="Subject"
                                                                        MaxLength="25" Text="<%# mADSBTechRecording.ADSBSubject %>" Style="margin-bottom: 10px;"
                                                                        Height="25px" TextMode="MultiLine"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblIssueDate" class="control-label clsLabelAuto">Issue Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtIssueDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                                        Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px" onchange="ValidateDateText(this,'txtIssueDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtIssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtIssueDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtIssueDateWatermarkExtender" runat="server" TargetControlID="txtIssueDate"
                                                                        WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <span id="lblEffectiveDate" class="control-label clsLabelAuto">Effective Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEffectiveDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                                        Style="margin-bottom: 5px;" Text="" Height="25px" Width="110px" onchange="ValidateDateText(this,'txtEffectiveDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtEffectiveDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEffectiveDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtEffectiveDateWatermarkExtender" runat="server"
                                                                        TargetControlID="txtEffectiveDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                  <span id="Span4" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDescription" class="control-label clsLabelAuto">Description</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="input-sm clsTextBoxMultiLine1_Ajax"
                                                                        Width="100%" ToolTip="Description" MaxLength="25" Text="<%# mADSBTechRecording.Description %>"
                                                                        Style="margin-bottom: 10px;" TextMode="MultiLine"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblRevNo" class="control-label clsLabelAuto">Rev. No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRevNo" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="Rev. No."
                                                                        MaxLength="25" Text="<%# mADSBTechRecording.RevNo %>" Style="margin-bottom: 10px;"
                                                                        Height="25px"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblRevDate" class="control-label clsLabelAuto">Rev. Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRevDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                                        Style="margin-bottom: 5px;" Text="" Height="25px" Width="110px" onchange="ValidateDateText(this,'txtRevDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtRevDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRevDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtRevDateWatermarkExtender" runat="server" TargetControlID="txtRevDate"
                                                                        WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCompliance" class="control-label clsLabelAuto">Method of Compliance &nbsp;&nbsp;</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCompliance" runat="server" CssClass="input-sm clsTextBoxMultiLine1_Ajax"
                                                                        ToolTip="Method of Compliance" MaxLength="25" Text="<%# mADSBTechRecording.MethodOfCompliance %>"
                                                                        Style="margin-bottom: 10px;" TextMode="MultiLine"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblRevChange" class="control-label clsLabelAuto">Rev. Change in brief &nbsp;&nbsp;</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRevChange" runat="server" CssClass="input-sm clsTextBoxMultiLine1_Ajax"
                                                                        ToolTip="Rev. Change in brief" MaxLength="25" Text="<%# mADSBTechRecording.RevChangeInBrief %>"
                                                                        Style="margin-bottom: 10px;" TextMode="MultiLine"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%--                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="txtApplicability" class="control-label clsLabelAuto">Applicability</span>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkApplicability" runat="server" TextAlign="Left" CssClass="input-sm"
                                                                        Checked="<%# mADSBTechRecording.Applicability %>" />
                                                                </td>
                                                            </tr>--%>
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
                            <td colspan="1" valign="top">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-right: 5px;"
                                            valign="top;">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;
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
                                                                                        CssClass="table table-striped table-bordered table-hover" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
                                                                                        AllowSorting="True" AllowPaging="False" AutoGenerateColumns="false">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <Columns>
                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-ForeColor="black">
                                                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" Width="60px" ForeColor="black" >
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
                                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View" ToolTip="Click to View attached file"
                                                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center"
                                                                                                HeaderStyle-ForeColor="black">
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                        CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" ToolTip="Click to Delete attached file"
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
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnCancel" runat="server" ClientIDMode="Static" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Cancel" ToolTip="Click to Cancel AD/SB" />
                                       <%-- <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-sm" Visible="false" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Print" ClientIDMode="Static" ToolTip="Click to Print Invoice">
                                        </asp:Button>--%>
                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Authorize" ToolTip="Click to authorize AD/SB" />
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Save" CausesValidation="true" ValidationGroup="a"
                                            ToolTip="Click to Save" />
                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px; margin-right: 5px;" Text="Close" ToolTip="Click to go back to the previous page" />
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
