<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskCard_AJAX.aspx.vb"
    Inherits="Flypal.wfTaskCard_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Task Card Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script src="jquery.tablednd_0_5.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <table id="Table2" class="clstablelistin">
                            <tr>

                                <td class="clsFormHeader1" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Task Card [New]</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table5" border="0" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Task Card"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSaveNew" CssClass="clsbtnH clsinfoH" runat="server" Text="Save &amp; New"
                                                                        ToolTip="Click to save the Task Card &amp; refresh the screen"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to go back to the previous page"
                                                                        CausesValidation="False"></asp:Button>
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
                            <tr>
                                <td></td>
                                <td colspan="1">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlMachineDet" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset>
                                                <legend class="clsFieldSet1"><b>Task Card Details </b></legend>
                                                <table id="Table11" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:UpdatePanel ID="UpnlTaskCardDetail" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table3" border="0" cellspacing="3" cellpadding="1">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblCardNo" runat="server" CssClass="clsLabel">Task Card No.</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtCardNo" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                        Text="<%# mTaskCard.TaskCardNo %>" ToolTip="Enter Task Card No." MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td style="height: 30px">
                                                                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbModel" runat="server" SelectedValue="<%# mTaskCard.ModelID %>"
                                                                                        DataValueField="ID" DataTextField="ModelName">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td style="height: 30px">
                                                                                    <asp:Label ID="lblSubject" runat="server" CssClass="clsLabel">Subject</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtSubject" runat="server" Width="320px"
                                                                                        Text="<%# mTaskCard.TaskSubject %>" ToolTip="Enter Task Card Subject." MaxLength="1000"
                                                                                        TextMode="MultiLine"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td style="height: 30px">
                                                                                    <asp:Label ID="lblInspCode" runat="server" CssClass="clsLabelAuto">Task Type</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtInspCode" runat="server" Width="320px"
                                                                                        Text="<%# mTaskCard.InspCode %>" ToolTip="Enter Task Type" MaxLength="50"
                                                                                        TextMode="MultiLine"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td style="width: 98px">
                                                                                    <asp:Label ID="lblATAChapter" runat="server" CssClass="clsLabelAuto">ATA Chapter</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbATAChapter" runat="server"
                                                                                        SelectedValue="<%# mTaskCard.ATAChapterID %>" DataTextField="ATAChapter" DataValueField="ID">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 26px"></td>
                                                                                <td style="width: 98px; height: 26px">
                                                                                    <asp:Label ID="lblZone" runat="server" CssClass="clsLabel">Zone </asp:Label>
                                                                                </td>
                                                                                <td style="height: 26px">
                                                                                    <asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                        Text="<%# mTaskCard.Zone %>" ToolTip="Enter Task Card Zone" MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td style="width: 98px">
                                                                                    <asp:Label ID="lblSkill" runat="server" CssClass="clsLabelAuto">Skill</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtSkill" runat="server" Width="320px"
                                                                                        Text="<%# mTaskCard.Skill %>" ToolTip="Enter Skill" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td style="width: 98px">
                                                                                    <asp:Label ID="lblEstimatedHr" runat="server" CssClass="clsLabel">Estimated Hr.</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtEstimatedHr" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTaskCard.EstimatedHours %>"
                                                                                                    ToolTip="Enter Estimated Hr." MaxLength="8" Width="65px"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="width: 98px">
                                                                                                <asp:Label ID="lblAccessHr" runat="server" CssClass="clsLabel">Access Hr.</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtAccessHr" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTaskCard.AccessHours %>"
                                                                                                    ToolTip="Enter Access Hr." MaxLength="8" Width="65px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td valign="top">
                                                            <asp:UpdatePanel ID="upnlTaskCardHeader" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table4" class="clsTable1" border="0" cellspacing="3" cellpadding="1">
                                                                        <tr>
                                                                            <td style="height: 51px"></td>
                                                                            <td style="height: 51px">
                                                                                <asp:Label ID="lblHeading" runat="server" CssClass="clsLabelAuto"> Heading</asp:Label>
                                                                            </td>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtHeading" runat="server" Width="320px"
                                                                                    Text="<%# mTaskCard.TaskHeading %>" ToolTip="Enter Task Card Heading." MaxLength="500"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="height: 51px">&nbsp;&nbsp;
                                                                            <asp:Label ID="lblStar3" runat="server" CssClass="clsLabelStar" DESIGNTIMEDRAGDROP="275">*</asp:Label>
                                                                            </td>
                                                                            <td style="height: 51px">
                                                                                <asp:Label ID="lblDesc" runat="server" CssClass="clsLabel">Description</asp:Label>
                                                                            </td>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtDescription" runat="server" Width="320px"
                                                                                    Text="<%# mTaskCard.TaskDesc %>" ToolTip="Enter Description of Task Card" MaxLength="1000"
                                                                                    TextMode="MultiLine">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:Label ID="lblReference" runat="server" CssClass="clsLabelAuto">MPD Reference</asp:Label>
                                                                            </td>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtReference" runat="server" Width="320px"
                                                                                    Text="<%# mTaskCard.Reference %>" ToolTip="Enter Reference" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:Label ID="mINSPTypeInterval" runat="server" CssClass="clsLabelAuto">INSP Type / Interval</asp:Label>
                                                                            </td>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtINSPTypeInterval" runat="server" Width="320px"
                                                                                    Text="<%# mTaskCard.INSPTypeInterval %>" ToolTip="Enter INSP Type / Interval"
                                                                                    MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:Label ID="lblArea" runat="server" CssClass="clsLabelAuto">Area</asp:Label>
                                                                            </td>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtArea" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                    Text="<%# mTaskCard.Area %>" ToolTip="Enter Task Card Area" MaxLength="50"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="lblPanels" runat="server" CssClass="clsLabelAuto">Panels</asp:Label>
                                                                            </td>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtPanels" runat="server" Width="320px"
                                                                                    Text="<%# mTaskCard.Panels %>" ToolTip="Enter Panels" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:Label ID="lblRII" runat="server" CssClass="clsLabelAuto">RII</asp:Label>
                                                                            </td>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkIsRII" runat="server" CssClass="clsCheckBox" Checked="<%# mTaskCard.IsRII %>"></asp:CheckBox><asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">(Check if RII)</asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table id="Table13" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="UpnlTargetOtherDet" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:Panel ID="pnlTargetOtherDet" runat="server">
                                                                                    <div style="vertical-align: middle;" class="clsCollapsePnl">
                                                                                        <div style="float: left;">
                                                                                            <asp:Label runat="server" ID="lblOtherDetRecCount" CssClass="clsLabelHeader">Other Details</asp:Label>
                                                                                        </div>
                                                                                        <div style="float: right;">
                                                                                            <span id="lblMessageOtherDet" class="clsLabelHeader"></span>
                                                                                            <asp:Image ID="imgArrowsOtherDet" Style="vertical-align: middle;" runat="server" />
                                                                                        </div>
                                                                                        <div style="clear: both">
                                                                                        </div>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandOtherDet" runat="server" CssClass="clsExpandiblePnl">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table id="Table12" border="0" cellspacing="3" cellpadding="1">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 10px"></td>
                                                                                                    <td style="width: 99px">
                                                                                                        <asp:Label ID="lblPublication" runat="server" CssClass="clsLabelAuto">Publication</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtPublication" runat="server" Width="320px"
                                                                                                            Text="<%# mTaskCard.Publication %>" ToolTip="Enter Publication." MaxLength="150"
                                                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 99px">
                                                                                                        <asp:Label ID="IssueDate" runat="server" CssClass="clsLabel" DESIGNTIMEDRAGDROP="287">Issue Date</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtIssueDate" runat="server" CssClass="clsTextBoxTagSearch" onchange="ValidateDateText(this,'txtIssueDate_CalendarExtender');"
                                                                                                            Width="100px"></asp:TextBox>
                                                                                                        <cc2:CalendarExtender ID="txtIssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtIssueDate"></cc2:CalendarExtender>
                                                                                                        <cc2:TextBoxWatermarkExtender ID="TBWE1" runat="server" TargetControlID="txtIssueDate"
                                                                                                            WatermarkText="<%$AppSettings:DateFormat%>" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 99px">
                                                                                                        <asp:Label ID="lblRevNo" runat="server" CssClass="clsLabel">Revision No.</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtRevNo" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                                            Text="<%# mTaskCard.RevNo %>" ToolTip="Enter Revision No" MaxLength="50"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 99px">
                                                                                                        <asp:Label ID="RevDate" runat="server" CssClass="clsLabel">Revision Date</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtRevDate" runat="server" CssClass="clsTextBoxTagSearch" onchange="ValidateDateText(this,'txtRevDate_CalendarExtender');"
                                                                                                            Width="100px"></asp:TextBox>
                                                                                                        <cc2:CalendarExtender ID="txtRevDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRevDate"></cc2:CalendarExtender>
                                                                                                        <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtRevDate"
                                                                                                            WatermarkText="<%$AppSettings:DateFormat%>" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 99px">
                                                                                                        <asp:Label ID="lblMaterial" runat="server" CssClass="clsLabel">Material</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtMaterial" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                                            Text="<%# mTaskCard.Material %>" ToolTip="Enter Material" MaxLength="150"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 10px"></td>
                                                                                                    <td style="width: 99px">
                                                                                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabel">Tally Sequence No</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtTallySequenceNo" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                                            Text="<%# mTaskCard.TallySequenceNo %>" ToolTip="Enter Tally Sequence No." MaxLength="50"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td>
                                                                                        <table id="Table14" border="0" cellspacing="3" cellpadding="1">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 98px">
                                                                                                        <asp:Label ID="lblAMPIssueRev" runat="server" CssClass="clsLabel">AMP Issue/Rev </asp:Label>
                                                                                                    </td>
                                                                                                    <td>&nbsp;
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtAMPIssueRev" runat="server" Width="320px"
                                                                                                            Text="<%# mTaskCard.AMPIssueRev %>" ToolTip="Enter AMP Issue/Rev " MaxLength="150"
                                                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblCheck" runat="server" CssClass="clsLabel">Check</asp:Label>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtCheck" runat="server" Width="320px"
                                                                                                            Text="<%# mTaskCard.Check %>" ToolTip="Enter Check" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 22px"></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblRelatedTaskCardNo" runat="server" CssClass="clsLabelAuto">Related Task Card No.</asp:Label>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtRelatedTaskCardNo" runat="server"
                                                                                                            Width="320px" Text="<%# mTaskCard.RelatedTaskCardsNo %>" ToolTip="Enter Related Task Card No."
                                                                                                            MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 22px"></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblEquipment" runat="server" CssClass="clsLabel">Equipment</asp:Label>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtEquipment" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                                            Text="<%# mTaskCard.Equipment %>" ToolTip="Enter Equipment" MaxLength="100"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 22px"></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblCategory" runat="server" CssClass="clsLabel">Category</asp:Label>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtCategory" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                                                            Text="<%# mTaskCard.Category %>" ToolTip="Enter Task Card Category" MaxLength="50"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 22px">&nbsp;
                                                                                                    </td>
                                                                                                    <td style="width: 120px">
                                                                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabel">Remark</asp:Label>
                                                                                                    </td>
                                                                                                    <td>&nbsp;
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtRemark" runat="server" Width="320px"
                                                                                                            Text="<%# mTaskCard.Remark %>" ToolTip="Enter Task Card Remark." MaxLength="1000"
                                                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <cc2:CollapsiblePanelExtender ID="cpeOtherDet" runat="Server" TargetControlID="pnlExpandOtherDet"
                                                                                            Collapsed="true" ExpandControlID="pnlTargetOtherDet" CollapseControlID="pnlTargetOtherDet"
                                                                                            AutoCollapse="False" AutoExpand="False" ScrollContents="false" TextLabelID="lblMessageOtherDet"
                                                                                            CollapsedText="Show..." ExpandedText="Hide" ImageControlID="imgArrowsOtherDet"
                                                                                            ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                                                            ExpandDirection="Vertical" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:UpdatePanel ID="upnlEnclosure" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label5" runat="server" CssClass="clsLabelHeader">Enclosure</asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkCMR" runat="server" CssClass="clsCheckBox" Checked="<%# mTaskCard.CMR %>"
                                                                                                                Text="CMR" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkCPCP" runat="server" CssClass="clsCheckBox" Checked="<%# mTaskCard.CPCP %>"
                                                                                                                Text="CPCP" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkCDCCL" runat="server" CssClass="clsCheckBox" Checked="<%# mTaskCard.CDCCL %>"
                                                                                                                Text="CDCCL" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkAD" runat="server" CssClass="clsCheckBox" Checked="<%# mTaskCard.AD %>"
                                                                                                                Text="AD" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkALI" runat="server" CssClass="clsCheckBox" Checked="<%# mTaskCard.ALI  %>"
                                                                                                                Text="ALI" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkETO" runat="server" CssClass="clsCheckBox" Checked="<%# mTaskCard.ETO %>"
                                                                                                                Text="ETO" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--ExpandedSize="137"--%>
                                                                            </table>
                                                                        </asp:Panel>
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
                                <td colspan="2">
                                    <asp:Label ID="Label7" runat="server" CssClass="clsLabelHeader">* : Indicates Part no. does not exist and need to be added in the Part Master.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="UpnlAddTaskCardSpare" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table9" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTaskCardSpares" runat="server" CssClass="clsLabelHeader" Width="188px">Task Card Spare(s)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddTaskCardSpare" CssClass="clsbtnH" runat="server" Text="Add"
                                                            CausesValidation="False" ToolTip="Click to Add Task Card Spare"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnldgTaskCardSpares" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskCardSpares" runat="server" CssClass="clsGridNewStyle" DataKeyNames="ID"
                                                ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="false" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="7"
                                                PageSize="5">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="IsNewPart" HeaderText="New Part">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" Width="20px"></HeaderStyle>
                                                        <ItemStyle Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="True" Width="20px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OffSerialNo" HeaderText="Off Serial No.">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
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
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnlAddTaskTools" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table9" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRequiredToolList" runat="server" CssClass="clsLabelHeader" Width="188px">Task Card Tool(s)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddTaskTools" CssClass="clsbtnH" runat="server" Text="Add"
                                                            ToolTip="Click to Add Task Card Tool"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnldgTaskCardTools" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskCardTools" ToolTip="List of Task Card Tool(s)" runat="server"
                                                CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True" GridLines="Horizontal" CellPadding="7"
                                                AllowPaging="false" AutoGenerateColumns="False" PageSize="5">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="IsNewPart" HeaderText="New Part">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="True" HorizontalAlign="Left" Width="20px"></HeaderStyle>
                                                        <ItemStyle Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="True" Width="20px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
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
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table8" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader" Width="188px">Task Card Attachment(s)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <input type="button" runat="server" id="btnSelectFile" value="Select File" class="clsbtnH" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup for FileUpload-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnldgTaskCardAttachment" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskCardAttachment" ToolTip="List of Task Card Attachment(s)"
                                                runat="server" CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="7"
                                                AllowSorting="True" AllowPaging="False" AutoGenerateColumns="False">
                                                <%--<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />--%>
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="White" CssClass="clsdgHeader nodrag nodrop" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="TaskCardID" HeaderText="TaskCardID"></asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr No">
                                                        <HeaderStyle Wrap="True" HorizontalAlign="Left" Width="20px"></HeaderStyle>
                                                        <ItemStyle Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="File Name">
                                                        <HeaderStyle Width="350px" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
                                                                ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'
                                                                Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="Remove">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <div id="T1" class="clsGridNew_Ajax">
                                                                        <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                            CommandName="View" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" CssClass="clsverticalmargintop clsverticalalignmiddle " />

                                                                        <asp:ImageButton ID="Remove" runat="server" CommandName="Remove" Style="height: 20px; width: 20px"
                                                                            ImageUrl="~/images/delete.png" CommandArgument='<%# Eval("SrNo") %>' CssClass="clsverticalalignmiddle" />
                                                                    </div>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer;" />
                                                            </div>
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
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnRefresh" runat="server" ClientIDMode="Static" CssClass="clsbtnH"
                                                            Text="Refresh" ToolTip="Click to Refresh Task Card Attachment" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnlAddStep" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table7" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSteps" runat="server" CssClass="clsLabelHeader" Width="188px">Additional Work(s)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddStep" CssClass="clsbtnH" runat="server" Text="Add" ToolTip="Click to Add Additional Work"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnldgTaskCardSteps" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskCardSteps" ToolTip="List of Additional Work(s)" runat="server"
                                                CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True" GridLines="Horizontal" CellPadding="7"
                                                AllowPaging="false" AutoGenerateColumns="False" PageSize="5">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="True" Width="20px" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MPDNo" HeaderText="MPD No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AMMNo" HeaderText="AMM No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WebDescription" HeaderText="Description">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Zone" HeaderText="Zone">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
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
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnlAddWorkSpares" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table10" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td style="margin-left: 40px">
                                                        <asp:Label ID="lblTaskCardWorkSpares" runat="server" CssClass="clsLabelHeader" Width="188px">Additional Work Spare(s)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddWorkSpares" CssClass="clsbtnH" runat="server" Text="Add"
                                                            ToolTip="Click to Add Additional Work Spare"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnldgTaskCardWorkSpares" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskCardWorkSpares" ToolTip="List of Additional Work Spare(s)"
                                                runat="server" CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="7"
                                                AllowSorting="True" AllowPaging="false" PageSize="5" AutoGenerateColumns="False">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="IsNewPart" HeaderText="New Part">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="True" Width="20px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OffSerialNo" HeaderText="Off Serial No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OnSerialNo" HeaderText="OnSerialNo" Visible="False">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
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
                            <%-- 'Added by Shital on 18-Aug-2016--%>
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnlAddSkill" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td style="margin-left: 40px">
                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader" Width="188px">Skill(s)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddSkill" CssClass="clsbtnH" runat="server" Text="Add"
                                                            ToolTip="Click to Add New Skill"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnldgSkillList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gdSkillList" ToolTip="List of Skill(s)" runat="server" CssClass="clsGridNewStyle"
                                                DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="false" GridLines="Horizontal" CellPadding="7"
                                                PageSize="5" AutoGenerateColumns="False">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SkillCode" HeaderText="Skill Code">
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SkillName" HeaderText="Skill Name">
                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:BoundField>
                                                    <%-- <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="DeleteRec"
                                                                Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                            <tr>
                                <td style="height: 15px" colspan="2">
                                    <asp:UpdatePanel ID="upnlAddPartRemovals" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table6" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td style="margin-left: 40px">
                                                        <asp:Label ID="lblTaskCardPartRemovals" runat="server" CssClass="clsLabelHeader"
                                                            Width="188px">Part Removal(s)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddPartRemovals" CssClass="clsbtnH" runat="server" Text="Add"
                                                            ToolTip="Click to Add Part Removals"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnldgTaskCardPartRemovals" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskCardPartRemovals" ToolTip="List of Part Removal(s)" runat="server"
                                                CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True" GridLines="Horizontal" CellPadding="7"
                                                AllowPaging="false" PageSize="5" AutoGenerateColumns="False">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="IsNewPart" HeaderText="New Part">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="True" Width="20px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="True" Width="20px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OffSerialNo" HeaderText="Serial No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Position" HeaderText="Position">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
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
                            <%--  --%>
                            <tr>
                                <%--<td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table5" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Task Card">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSaveNew" CssClass="clsbtnH clsinfoH" runat="server" Text="Save &amp; New"
                                                        ToolTip="Click to save the Task Card &amp; refresh the screen"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnTaskCardSpare" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnTaskCardTool" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnTaskCardStep" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnTaskCardStepSpares" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <%-- 'Added by Shital on 18-Aug-2016--%>
                                            <asp:Button ID="hdnBtnTaskCardSkill" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnTaskCardPartRemoval" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--End -->
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>
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
        </div>
        <div>
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
        </div>
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IFileUpload" frameborder="0" height="100%" width="100%" allowtransparency="true"
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
                        $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                        $("#IFileUpload").ready(function () {
                            $("#btnDummyFileUpload").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        });

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
        <!-- End File Upload Modal Dialog-->
        <!-- Task Card Spare Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskCardSpare" Text="TaskCard Spare" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlTaskCardSpare" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeTaskCardSpare" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardSpare" runat="server" TargetControlID="btnDummyTaskCardSpare"
            PopupControlID="pnlTaskCardSpare" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskCardSpareStateComplete() {
                $("#btnDummyTaskCardSpare").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenTaskCardSpareWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeTaskCardSpare").attr("src", "wfTaskCardSpares_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskCardSpare").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForTaskCardSpare() {
                var TaskCardSparewindow = $find("<%=mdlPopupTaskCardSpare.ClientID %>");
                //close Task Card Spare popup window
                TaskCardSparewindow.hide();
                //           release resources
                $("#IframeTaskCardSpare").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnTaskCardSpare").click();
            }
        </script>
        <!-- End-->
        <!-- Task Card Tool Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskCardTool" Text="TaskCard Tool" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlTaskCardTool" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeTaskCardTool" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardTool" runat="server" TargetControlID="btnDummyTaskCardTool"
            PopupControlID="pnlTaskCardTool" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskCardToolStateComplete() {
                $("#btnDummyTaskCardTool").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenTaskCardToolWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeTaskCardTool").attr("src", "wfTaskCardTools_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskCardTool").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForTaskCardTool() {
                var TaskCardToolwindow = $find("<%=mdlPopupTaskCardTool.ClientID %>");
                //close Task Card Tool popup window
                TaskCardToolwindow.hide();
                //           release resources
                $("#IframeTaskCardTool").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnTaskCardTool").click();
            }
        </script>
        <!-- End-->
        <%-- 'Added by Shital on 18-Aug-2016--%>
        <!-- Task Card Skill Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskCardSkill" Text="TaskCard Skill" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlTaskCardskill" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeTaskCardSkill" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardSkill" runat="server" TargetControlID="btnDummyTaskCardSkill"
            PopupControlID="pnlTaskCardSkill" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskCardSkillStateComplete() {
                $("#btnDummyTaskCardSkill").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenTaskCardSkillWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeTaskCardSkill").attr("src", "wfTaskCardSkill_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskCardSkill").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForTaskCardSkill() {
                var TaskCardSkillwindow = $find("<%=mdlPopupTaskCardSkill.ClientID %>");
                //close Task Card Tool popup window
                TaskCardSkillwindow.hide();
                //           release resources
                $("#IframeTaskCardskill").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnTaskCardSkill").click();
            }
        </script>
        <!-- End-->
        <!-- Task Card Step Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskCardStep" Text="TaskCard Step" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlTaskCardStep" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeTaskCardStep" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardStep" runat="server" TargetControlID="btnDummyTaskCardStep"
            PopupControlID="pnlTaskCardStep" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskCardStepStateComplete() {
                $("#btnDummyTaskCardStep").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenTaskCardStepWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeTaskCardStep").attr("src", "wfTaskCardStep_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskCardStep").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForTaskCardStep() {
                var TaskCardStepwindow = $find("<%=mdlPopupTaskCardStep.ClientID %>");
                //close Task Card Step popup window
                TaskCardStepwindow.hide();
                //           release resources
                $("#IframeTaskCardStep").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnTaskCardStep").click();
            }
        </script>
        <!-- End-->
        <!-- Task Card Step Spares Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskCardStepSpares" Text="TaskCard Step Spares"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlTaskCardStepSpares" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeTaskCardStepSpares" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardStepSpares" runat="server" TargetControlID="btnDummyTaskCardStepSpares"
            PopupControlID="pnlTaskCardStepSpares" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskCardStepsSpareStateComplete() {
                $("#btnDummyTaskCardStepSpares").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenTaskCardStepSparesWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeTaskCardStepSpares").attr("src", "wfTaskCardStepSpares_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskCardStepSpares").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForTaskCardStepsSpare() {
                var TaskCardStepSpareswindow = $find("<%=mdlPopupTaskCardStepSpares.ClientID %>");
                //close Task Card Step popup window
                TaskCardStepSpareswindow.hide();
                //           release resources
                $("#IframeTaskCardStepSpares").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnTaskCardStepSpares").click();
            }
        </script>
        <!-- End-->
        <!-- Task Card Part Removal Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskCardPartRemoval" Text="TaskCard PartRemoval"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlTaskCardPartRemoval" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeTaskCardPartRemoval" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardPartRemoval" runat="server" TargetControlID="btnDummyTaskCardPartRemoval"
            PopupControlID="pnlTaskCardPartRemoval" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskCardPartRemovalStateComplete() {
                $("#btnDummyTaskCardPartRemoval").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenTaskCardPartRemovalWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeTaskCardPartRemoval").attr("src", "wfTaskCardPartRemoval_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskCardPartRemoval").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForTaskCardPartRemoval() {
                var TaskCardPartRemovalwindow = $find("<%=mdlPopupTaskCardPartRemoval.ClientID %>");
                //close Task Card Part Removal popup window
                TaskCardPartRemovalwindow.hide();
                //           release resources
                $("#IframeTaskCardPartRemoval").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnTaskCardPartRemoval").click();
            }
        </script>
        <!-- End-->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForTaskMaster();
                return false;
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameTaskMasterStateComplete();
                }
            });

    <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
           <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
    </form>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#btnRefresh").live("click", function () {
                var index = new Array();
                var srno = new Array();
                $("#<%= dgTaskCardAttachment.ClientID %> tr:not(:first)").each(function (i) {
                    index[i] = i;
                    srno[i] = $(this).find("td:first").html();
                });
                var myobj = new Object();
                myobj.SrNo = srno;
                myobj.index = index;
                var myData = "{Ids:" + JSON.stringify(myobj) + "}";
                $.ajax({
                    url: "wfTaskCard_AJAX.aspx/GetTableIDs",
                    data: myData,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //$("#" + ID).html(data.d).slideDown("medium");
                        //alert(data);
                    },
                    error: function (data, status, jqXHR) {// $("#" + ID).html(status);
                    }
                });
                return true;
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%= dgTaskCardAttachment.ClientID %>").tableDnD({
                scrollAmount: 5,
                onDragClass: "GbiHighlight",
                onDrop: function (table, row) {
                    var rows = table.tBodies[0].rows;
                    var myobj = new Object();
                    myobj.SrNo = "1";
                    myobj.index = "0";

                    var myData = "{Ids:" + JSON.stringify(myobj) + "}";
                    var data = $.tableDnD.serialize();
                },
                onDragStart: function (table, row) {
                    $("#debugArea").html("Started dragging row " + row.id);
                }
            });
        });
    </script>
</body>
</html>
