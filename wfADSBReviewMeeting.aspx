<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfADSBReviewMeeting.aspx.vb"
    Inherits="Flypal.wfADSBReviewMeeting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Review Board Meeting</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
                                            runat="server">Set Board Meeting</span>
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
                                            ValidationGroup="a" ErrorMessage="Select Plan Date" ControlToValidate="txtPlannedMeetingDateTime"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvLocation" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Location Required" ControlToValidate="txtLocation"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvMeetingLink" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Meeting Link Required" ControlToValidate="txtMeetingLink"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvEmployee" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Add Participants" ControlToValidate="cmbEmployeeList"
                                            Display="None"></asp:CustomValidator>
                                        <%--<asp:RequiredFieldValidator ID="rfvADSBNo" runat="server" Display="None" ControlToValidate="txtMeetingLink"
                                            ValidationGroup="a" ErrorMessage="Meeting Link Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" Display="None" ControlToValidate="txtLocation"
                                            ValidationGroup="a" ErrorMessage="Location Required"></asp:RequiredFieldValidator>--%>
                                        <asp:CustomValidator ID="cvCc" runat="server" Display="None" ControlToValidate="txtMeetingLink"
                                            ErrorMessage="Please Enter Valid Metting link" CssClass="" ClientValidationFunction="validURL"
                                            ValidationGroup="a"></asp:CustomValidator>
                                            <asp:HiddenField ID="hdnValue" runat="server" ClientIDMode="Static" />
                                        <script type="text/javascript">
                                            function validURL(source, args) {
                                                var text = $("#txtMeetingLink").val();
                                                var pattern = new RegExp('^((https?:)?\\/\\/)?' + // protocol
                                                                            '(?:\\S+(?::\\S*)?@)?' + // authentication
                                                                            '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
                                                                            '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
                                                                            '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
                                                                            '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
                                                                            '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locater
                                                var seperator = ',';
                                                if (!pattern.test(text)) {
                                                    args.IsValid = false;
                                                    $("#hdnValue").val(args.IsValid);
                                                    return;
                                                }
                                                else {
                                                    args.IsValid = true;
                                                    $("#hdnValue").val(args.IsValid);
                                                    return;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table style="border-width: 1px; margin-left: 5px" valign="top" width="99%">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;
                                                        margin-right: 5px">
                                                        <legend>AD/SB Detail </legend>
                                                        <table style="margin-top: -22px">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDate" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
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
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                                        No.</span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtADSBTechRecordingText" runat="server" Text="<%# mADSBTechRecording.ADSBRecordingText %>"
                                                                                    CssClass="input-sm clsTextBox_Ajax" Height="25px" onfocus="WaterMark(this, event);"
                                                                                    onblur="WaterMark(this, event);" ToolTip="Enter No." MaxLength="25" Width="208px"
                                                                                    Enabled="false"> </asp:TextBox>
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
                                                                    <span id="Span6" class="control-label clsLabelAuto" style="margin-left: 5px; margin-right: 5px;">
                                                                        AD/SB No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBNO" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="AD/SB No."
                                                                        MaxLength="25" Text="<%# mADSBTechRecording.ADSBNo %>" Style="margin-bottom: 10px;"
                                                                        Width="208px" Height="25px" Enabled="false"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSubject" class="control-label clsLabelAuto" style="margin-left: 5px;
                                                                        margin-right: 5px;">Subject</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="Subject"
                                                                        MaxLength="500" Text="<%# mADSBTechRecording.ADSBSubject %>" Style="margin-bottom: 10px;"
                                                                        Height="25px" Width="208px" Enabled="false" TextMode="MultiLine"> </asp:TextBox>
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
                                <asp:UpdatePanel runat="server" ID="upnlADSBTechRecordingDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table style="border-width: 1px; margin-left: 5px; margin-right: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend>Review Board Meeting Detail </legend>
                                                        <table style="margin-top: -22px">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblOffline" class="control-label" style="margin-left: 5px; margin-right: 5px;">
                                                                        Offline</span>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="optOffline" runat="server" CssClass="clsRadioButton" GroupName="x"
                                                                        AutoPostBack="True" Checked='<%#iif(mADSBReviewMeeting.IsOnLine = False,true,false)%>'
                                                                        Style="margin-bottom: 50px;" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=3  %>'>
                                                                    </asp:RadioButton>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblOnline" class="control-label" style="margin-left: 5px; margin-right: 2px;">
                                                                        Online</span>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="optOnline" runat="server" CssClass="clsRadioButton" GroupName="x"
                                                                        Checked='<%#iif(mADSBReviewMeeting.IsOnLine = True,true,false)%>' AutoPostBack="True"
                                                                        Style="margin-bottom: 50px;" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=3  %>'>
                                                                    </asp:RadioButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span2" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblLocation" class="control-label" style="margin-left: 5px; margin-right: 2px;">
                                                                        Location</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="Location"
                                                                        MaxLength="200" Text="<%# mADSBReviewMeeting.MeetingLocation %>" Style="margin-bottom: 10px;"
                                                                        Height="25px" TextMode="MultiLine" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=3  %>'> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblADSBNO" class="control-label" style="margin-left: 5px; margin-right: 2px;">
                                                                        Meeting Link</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMeetingLink" runat="server" CssClass="input-sm clsTextBox_Ajax"
                                                                        ToolTip="Meeting Link" Text="<%# mADSBReviewMeeting.MeetingLink %>" Style="margin-bottom: 10px;"
                                                                        Width="208px" Height="25px" TextMode="MultiLine" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=3  %>'> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblPlanningDateStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblPlanningDate" class="control-label" style="margin-left: 5px; margin-right: 2px;">
                                                                        Planning Date</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:TextBox ID="txtPlannedMeetingDateTime" runat="server" ClientIDMode="Static"
                                                                        CssClass="input-sm clsTextBox_Ajax" Style="margin-bottom: 5px;" Text="" Height="25px"
                                                                        Width="110px" onchange="ValidateDateText(this,'txtPlannedMeetingDateTimeWatermarkExtender','true');"
                                                                        Enabled='<%#  mADSBTechRecording.ADSBStepsID<=3  %>'></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtPlannedMeetingDateTime_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtPlannedMeetingDateTime">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtPlannedMeetingDateTimeWatermarkExtender" runat="server"
                                                                        TargetControlID="txtPlannedMeetingDateTime" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
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
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table style="border-width: 1px; margin-left: 5px" valign="top" width="99%">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;
                                                        margin-right: 5px">
                                                        <legend>Participants </legend>
                                                        <table style="margin-top: -22px">
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="control-label clsLabelStar" style="margin-left: 5px; margin-right: 2px;">
                                                                        *</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span4" class="control-label" style="margin-left: 5px; margin-right: 2px;">
                                                                        Participants</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsComboBox_Ajax"
                                                                        AutoPostBack="True" DataTextField="EmpNoName" DataValueField="ID" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=3  %>'>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="dgMeetingParticipantsList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                GridLines="Both" DataKeyNames="ID" ShowHeaderWhenEmpty="True" CssClass="table table-striped table-bordered table-hover"
                                                                                PageSize="25" AllowPaging="true" Style="margin-right: 8px; margin-top: 5px;"
                                                                                Width="100%">
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                <Columns>
                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                    <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                                                    <asp:BoundField DataField="EmployeeName" HeaderText="Participant Name">
                                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                            Font-Underline="False" Wrap="False" />
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="EmployeeEmail" HeaderText="Email">
                                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                            Font-Underline="False" Wrap="False" />
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="MailSendDateTime" HeaderText="Mail Sent On">
                                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                            Font-Underline="False" Wrap="False" />
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
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
                                        <asp:Button ID="btnSetMeeting" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Set Meeting" CausesValidation="true" ValidationGroup="a"
                                            ToolTip="Click to Set Meeting" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=3  %>' />
                                        <asp:Button ID="btnSendMail" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Send Mail" ToolTip="Send Mail" Enabled="<%# Not mADSBReviewMeeting.IsNew and mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count > 0 and  mADSBTechRecording.ADSBStepsID<=3 %>" />
                                        <%--<asp:Button ID="btnAuthorized" runat="server" CssClass="btn" Style="border-color: black;
                                            border-top-left-radius: 4px; border-top-right-radius: 4px; margin-bottom: 3px;"
                                            Text="Authorize" ToolTip="Click to authorize WO Invoice" />
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn" Style="border-color: black;
                                            border-top-left-radius: 4px; border-top-right-radius: 4px; margin-bottom: 3px;"
                                            Text="Save" CausesValidation="true" ValidationGroup="a" ToolTip="Click to Save WO Invoice" />--%>
                                        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Close" ToolTip="Click to close" />
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
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
    <div>
    </div>
    </form>
</body>
</html>
