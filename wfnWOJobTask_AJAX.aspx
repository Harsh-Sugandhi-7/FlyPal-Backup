<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobTask_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOJobTask_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Task Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .style1 {
            height: 26px;
        }
    </style>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain" cellspacing="1" cellpadding="1" border="0">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="clstablelistin" id="InnerTable" border="0">
                                <tr>


                                    <td class="clsFormHeader1Newstyle" colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">W.O. JOB Task Details</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnPrint" CssClass="clsbtnH clsinfoH" runat="server" Text="Print" ToolTip="Click to Print Job Task"
                                                                            Enabled="False" Visible="False" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnOK" CssClass="clsbtnH clsinfoH" runat="server" Text="OK" ToolTip="Click to Add Job Task"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCancel" CssClass="clsbtnH clsinfoH" runat="server" Text="Back" ToolTip="Click to go back to the previous page"
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
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"
                                                    Display="None"></asp:CustomValidator><asp:CustomValidator ID="cvActualTime" runat="server"
                                                        CssClass="clsValidationSummary" OnServerValidate="CustomVailidity" ControlToValidate="txtTime"
                                                        Display="Static"></asp:CustomValidator><asp:RequiredFieldValidator ID="rfvDesc" runat="server"
                                                            CssClass="clsValidationSummary" ControlToValidate="txtDescription" Display="None"
                                                            ErrorMessage="Description required"></asp:RequiredFieldValidator><asp:CustomValidator
                                                                ID="cvDesc" runat="server" CssClass="clsValidationSummary" OnServerValidate="CustomVailidity"
                                                                ControlToValidate="txtDescription" Display="None"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" style="width: 49%;">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; height: 75px;">
                                            <legend><b>Task Details </b></legend>
                                            <table id="Table4">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWO" runat="server" CssClass="clsLabel">W.O. No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWOLabel" runat="server" CssClass="clsLabelAuto" Text="<%# mnWO.WONumber %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWODate" runat="server" CssClass="clsLabel">W.O. Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWODate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagDateSearch"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtWODate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWODate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtWODate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td ></td>
                                                    <td>
                                                        <asp:Label ID="lblJob" runat="server" CssClass="clsLabelAuto">Job # </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblJobLabel" runat="server" CssClass="clsLabel"></asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblTask" runat="server" CssClass="clsLabelAuto">Task # </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTaskLabel" runat="server" CssClass="clsLabel" Text="<%# mnWOJob.WOJobTasks.CurrentItem.SrNo %>"></asp:Label>
                                                    </td>

                                                </tr>

                                            </table>
                                        </fieldset>
                                    </td>
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; height: 75px;">
                                            <legend><b>Actual </b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStartDate" runat="server" CssClass="clsLabelAuto">Start Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                            onchange="ValidateDateText(this,'txtStartDate_CalendarExtender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TBWEStartDate" runat="server" TargetControlID="txtStartDate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsTextBoxDate_Ajax" />
                                                        <asp:TextBox ID="txtStartDateTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                            MaxLength="10" ToolTip="Enter Time"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblTime" runat="server" CssClass="clsLabel">Time</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTime" runat="server" CssClass="clsTextBoxTagSearchRightAlign1" Text="<%# mnWOJob.WOJobTasks.CurrentItem.ActualTime %>"
                                                            ToolTip="Enter Actual Time" MaxLength="5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEndDate" runat="server" CssClass="clsLabel">End Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="clsTextBoxTagDateSearch"
                                                            onchange="ValidateDateText(this,'txtEndDate_CalendarExtender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEndDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TBWEEndDate" runat="server" TargetControlID="txtEndDate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsTextBoxDate_Ajax" />
                                                        <asp:TextBox ID="txtEndDateTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                            MaxLength="10" ToolTip="Enter Time"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblTaskDone" runat="server" CssClass="clsLabelAuto">Task Done ?</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsDone" TabIndex="3" runat="server" CssClass="clsCheckBox" ToolTip="Check if this task is over"
                                                            Checked="<%# mnWOJob.WOJobTasks.CurrentItem.IsDone %>"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend><b>Task Card Details </b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCardNo" runat="server" CssClass="clsLabel">Task Card No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCardNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWOJob.WOJobTasks.CurrentItem.TaskCardNo %>"
                                                            ToolTip="Task Card No." MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblReference" runat="server" CssClass="clsLabel">AMP Task Ref. No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWOJob.WOJobTasks.CurrentItem.Reference %>"
                                                            MaxLength="150" ToolTip="Reference"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAMPIssueRev" runat="server" CssClass="clsLabel">AMP Issue/Rev </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAMPIssueRev" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="AMP Issue/Rev "
                                                            ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblHeading" runat="server" CssClass="clsLabel">Heading</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtHeading" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Task Card Heading"
                                                            Height="25px" TextMode="MultiLine" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSubject" runat="server" CssClass="clsLabel">Subject</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Task Card Subject"
                                                            Height="25px" TextMode="MultiLine" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblATAChapter" runat="server" CssClass="clsLabelAuto">ATA Chapter</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboSmall" BackColor="#E0E0E0"
                                                            Enabled="False" DataTextField="ATAChapter" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblZone" runat="server" CssClass="clsLabel">Zone </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="150"
                                                            ToolTip="Task Card Zone" BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblArea" runat="server" CssClass="clsLabelAuto">Area</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="150" ToolTip="Task Card Area"
                                                            BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPublication" runat="server" CssClass="clsLabelAuto">Publication</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPublication" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="150"
                                                            ToolTip="Publication." BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="mINSPTypeInterval" runat="server" CssClass="clsLabelAuto">INSP Type / Interval</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtINSPTypeInterval" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="150"
                                                            ToolTip="INSP Type / Interval" ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInspCode" runat="server" CssClass="clsLabelAuto">Inspection Code</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInspCode" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                            ToolTip="Inspection Code" BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSkill" runat="server" CssClass="clsLabelAuto">Skill</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSkill" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="150" ToolTip="Skill"
                                                            BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRevNo" runat="server" CssClass="clsLabel">Revision No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRevNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWOJob.WOJobTasks.CurrentItem.RevNo %>"
                                                            MaxLength="50" ToolTip="Revision No."></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="RevDate" runat="server" CssClass="clsLabel">Revision Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRevDate" runat="server" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'CalendarExtender1');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRevDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtRevDate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="IssueDate" runat="server" CssClass="clsLabel">Issue Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtIssueDate" runat="server" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'CalendarExtender2');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtIssueDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtIssueDate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPanels" runat="server" CssClass="clsLabelAuto">Panels</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPanels" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="150"
                                                            ToolTip="Panels" BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEquipment" runat="server" CssClass="clsLabel">Equipment</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEquipment" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWOJob.WOJobTasks.CurrentItem.Equipment %>"
                                                            ToolTip="Equipment" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMaterial" runat="server" CssClass="clsLabel">Material</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMaterial" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWOJob.WOJobTasks.CurrentItem.Material %>"
                                                            ToolTip="Material" MaxLength="150"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCategory" runat="server" CssClass="clsLabel">Category</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCategory" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="150"
                                                            ToolTip="Task Card Category" ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEstimatedHr" runat="server" CssClass="clsLabel">Estimated Hr.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEstimatedHr" runat="server" CssClass="clsTextBoxTagSearchRightAlign1" Text="<%# mnWOJob.WOJobTasks.CurrentItem.EstimatedHours %>"
                                                            MaxLength="5" ToolTip="Estimated Hours"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabel">Remark</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle1" ToolTip="Task Card Remark"
                                                            TextMode="MultiLine" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCheck" runat="server" CssClass="clsLabel">Check</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCheck" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                            Text="<%# mnWOJob.WOJobTasks.CurrentItem.checks %>" ToolTip="Check"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRelatedTaskCardNo" runat="server" CssClass="clsLabel">Related Task Card No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRelatedTaskCardNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                            MaxLength="50" Text="<%# mnWOJob.WOJobTasks.CurrentItem.RelatedTaskCardsNo %>"
                                                            ToolTip="Related Task Card No."></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRII" runat="server" CssClass="clsLabelAuto">RII</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsRII" runat="server" CssClass="clsCheckBox" Enabled="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblAttach" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table8">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" runat="server" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                class="clsbtnH clsinfoH" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Remove Attachment"
                                                                                Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Height="20px" Width="20px"></asp:ImageButton>
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
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>

                                <tr>
                                        <td>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelHeader">Description/Subject</asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </legend>
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                    Text="<%# mnWOJob.WOJobTasks.CurrentItem.TaskDescription %>" ToolTip="Description "
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend>Action
                                                </legend>
                                                <asp:TextBox ID="txtTaskAction" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                    Text="<%# mnWOJob.WOJobTasks.CurrentItem.TaskAction %>" MaxLength="500"
                                                    ToolTip="Enter Action" TextMode="MultiLine"></asp:TextBox>
                                            </fieldset>
                                        </td>
                                 
                                </tr>
                                <asp:PlaceHolder ID="HideOnManaulTask" runat="server" >
                                <tr>
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                            <legend>
                                                <asp:Label ID="lblSpares" runat="server" CssClass="clsLabelHeader">Spares</asp:Label></legend>
                                            <asp:GridView ID="dgTaskCardSpares" runat="server" CssClass="clsGridNewStyle" Width="100%"
                                                ShowHeaderWhenEmpty="true" ToolTip="List of Task Card Spares" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No." HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Off Serial No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtOffSerialNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text='<%# DataBinder.Eval(Container.DataItem, "OffSerialNo") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings PreviousPageText="Prev" NextPageText="Next" />
                                                <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                            </asp:GridView>
                                        </fieldset>
                                    </td>
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                            <legend>
                                                <asp:Label ID="lblTools" runat="server" CssClass="clsLabelHeader">Tools</asp:Label></legend>
                                            <asp:GridView ID="dgTaskCardTools" runat="server" CssClass="clsGridNewStyle" ToolTip="List of Task Card Tools" GridLines="Horizontal" CellPadding="5"
                                                ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" Width="100%">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" Width="200px"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings PreviousPageText="Prev" NextPageText="Next" />
                                                <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                            </asp:GridView>
                                        </fieldset>
                                    </td>
                                </tr>

                                <tr>
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                            <legend>
                                                <asp:Label ID="lblSteps" runat="server" CssClass="clsLabelHeader">Additional Work</asp:Label></legend>

                                            <asp:GridView ID="dgTaskSteps" runat="server" CssClass="clsGridNewStyle" Width="100%" ToolTip="List of Additional Works" GridLines="Horizontal" CellPadding="5"
                                                ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MPDNo" HeaderText="MPD. No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AMMNo" HeaderText="AMM. No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Zone" HeaderText="Zone">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings PreviousPageText="Prev" NextPageText="Next" />
                                                <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                            </asp:GridView>
                                        </fieldset>
                                    </td>
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                            <legend>
                                                <asp:Label ID="lblAdditionalWorkSpares" runat="server" CssClass="clsLabelHeader">Additional Work Spares</asp:Label></legend>
                                            <asp:GridView ID="dgWOJobTaskSpares" runat="server" CssClass="clsGridNewStyle" Width="100%"
                                                ShowHeaderWhenEmpty="true" ToolTip="List of Additional Work Spares" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Off Serial No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAdditionalSparesOffSerialNo" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "OffSerialNo") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings PreviousPageText="Prev" NextPageText="Next" />
                                                <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                            </asp:GridView>
                                        </fieldset>
                                    </td>
                                </tr>

                                <tr>
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                            <legend>
                                                <asp:Label ID="lblPartRemovals" runat="server" CssClass="clsLabelHeader">Part Removals</asp:Label></legend>
                                            <asp:GridView ID="dgPartRemovals" runat="server" CssClass="clsGridNewStyle" Width="100%"
                                                ShowHeaderWhenEmpty="true" ToolTip="List of Part Removals" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No." HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Off Serial No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtOffSerialNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text='<%# DataBinder.Eval(Container.DataItem, "OffSerialNo") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Position" HeaderText="Position">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings PreviousPageText="Prev" NextPageText="Next" />
                                                <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                            </asp:GridView>
                                        </fieldset>
                                    </td>
                                    <td valign="top"></td>
                                </tr>
                                    </asp:PlaceHolder>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForJobTaskDetail();
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
                    parent.IFrameJobTaskDetailStateComplete();
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
</body>
</html>
