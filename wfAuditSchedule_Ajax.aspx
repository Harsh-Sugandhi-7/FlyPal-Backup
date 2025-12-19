<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditSchedule_Ajax.aspx.vb"
    Inherits="Flypal.wfAuditSchedule_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Schedule</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">
        function openFile() {
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
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <table class="clsTablelistin" id="tblinner">
                        <tr>

                            <td colspan="4" class="clsFormHeader1">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Audit Schedule [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionButton" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH" Visible="<%# not mAuditSchedule.IsNew %>"
                                                                    Text="Send Mail" ToolTip="Click to Send Mail" Enabled="<%# Not mAuditSchedule.IsComplied %>"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Enabled="<%# not mAuditSchedule.IsNew %>"
                                                                    Text="Print" ToolTip="Click to Print Audit Schedule"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Audit Schedule"
                                                                    Enabled="<%# Not mAuditSchedule.IsComplied %>"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to Close the Audit Schedule Screen"
                                                                    CausesValidation="false"></asp:Button>
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
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"
                                            runat="server"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Enter Schedule Date." ControlToValidate="txtScheduleDate" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtScheduleDate"
                                            ErrorMessage="Enter Schedule Date."></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvDept" runat="server" Display="None" ErrorMessage="Please select Department"
                                            ControlToValidate="cmbDepartmentList" ClientValidationFunction="ValidateDepartment"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAuditOn" runat="server" Display="None" ErrorMessage="Please select Audit On Center"
                                            ControlToValidate="cmbAuditOnList" ClientValidationFunction="ValidateAuditOnList"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNote" runat="server" ClientValidationFunction="validateNoteLen"
                                            Display="None" ErrorMessage="Note should not be greater than 500 characters"
                                            ControlToValidate="txtNote" CssClass="clsLabelAuto"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvAuditNo" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtAuditNo" ErrorMessage="Audit No Required" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvFrequency" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtFrequency"
                                            Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvToMailIDs" runat="server" Display="None" ErrorMessage="Please Enter Valid To Email-IDs"
                                            ControlToValidate="txtToMailID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCCMailIDs" runat="server" Display="None" ErrorMessage="Please Enter Valid CC Email-IDs"
                                            ControlToValidate="txtCCMailID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAuditOnTypeID" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="cmbAuditOnList" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            //Department
                                            function ValidateDepartment(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbDepartmentList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;

                                                }

                                            }

                                            function ValidateAuditOnList(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbAuditOnList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;

                                                }

                                            }

                                            function validateNoteLen(source, args) {
                                                args.IsValid = false;
                                                var nameLength = $get("txtNote").value.length;
                                                if (nameLength <= 500) {
                                                    args.IsValid = true;
                                                    return;
                                                }

                                            }

                                            function validateEmail(field) {
                                                var regex = /^[a-zA-Z0-9._'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
                                                return (regex.test(field)) ? true : false;
                                            }
                                            function validateMultipleEmailsCommaSeparated(source, args) {
                                                var text = $get(source.controltovalidate).value;
                                                var seperator = ',';
                                                if (text != '') {
                                                    var result = text.split(seperator);
                                                    for (var i = 0; i < result.length; i++) {
                                                        if (result[i] != '') {
                                                            if (!validateEmail(result[i].trim())) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px">
                                    <legend id="lblQualityAuditDetail" style="font-weight: bold"><b>Audit Schedule Detail</b></legend>
                                    <asp:UpdatePanel ID="upnlAuditScheduleDetail" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblAuditStandard" class="clsLabelAuto">Audit Standard</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAuditStandard" runat="server" CssClass="clsTextBoxTagSearch"
                                                            Width="273px" ReadOnly="True" MaxLength="100" BackColor="#E0E0E0" ToolTip="Audit Standard"
                                                            Text="<%# mAuditSchedule.AuditStandardName %>">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Label1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Label2" class="clsLabelAuto">Schedule Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtScheduleDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagDateSearch"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'ScheduleDate_watermarkextender','true');"
                                                            Text=""></asp:TextBox>
                                                        <cc2:CalendarExtender ID="ScheduleDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtScheduleDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="ScheduleDateWatermarkExtender" runat="server" TargetControlID="txtScheduleDate"
                                                            WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblAuditNo" class="clsLabelAuto">Audit No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAuditNo" runat="server" CssClass=" clsTextBoxTagSearch" Text="<%# mAuditSchedule.AuditText %>"
                                                            Enabled="<%# Not mAuditSchedule.IsComplied %>" ToolTip="Enter Audit No." MaxLength="100"
                                                            Width="273px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblReferenceNo" class="clsLabelAuto">Reference No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReferenceNo" runat="server" CssClass=" clsTextBoxTagSearch" MaxLength="500"
                                                            BackColor="#E0E0E0" ToolTip="Reference No." Text="<%# mAuditSchedule.Reference %>"
                                                            ReadOnly="True" Width="275px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblAuditType" class="clsLabelAuto">Audit Type</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbAuditTypeList" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                            DataTextField="Name" DataValueField="ID" SelectedValue="<%# mAuditSchedule.AuditTypeID %>"
                                                            Enabled="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewStyleLong"
                                                            MaxLength="500" Height="34px" BackColor="#E0E0E0" ToolTip="Description" Text="<%# mAuditSchedule.Description %>"
                                                            TextMode="MultiLine" ReadOnly="True" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span2" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td style="width: 140px">
                                                        <span id="lblDepartment" class="clsLabelAuto">Responsible Department</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDepartmentList" runat="server" CssClass=" clsTextBoxTagSearchComboNewstyle"
                                                            Enabled="<%# Not mAuditSchedule.IsComplied %>" ClientIDMode="Static" SelectedValue="<%# mAuditSchedule.DepartmentID %>"
                                                            DataTextField="Name" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblLocation" class="clsLabelAuto">Location</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocation" runat="server" CssClass=" clsTextBoxTagSearch" MaxLength="100"
                                                            Enabled="<%# Not mAuditSchedule.IsComplied %>" ToolTip="Enter Location" Text="<%# mAuditSchedule.Location %>"
                                                            Width="275px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblOtherInformation" class="clsLabelAuto">Other Information</span>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:TextBox ID="txtOtherInformation" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                            Enabled="<%# Not mAuditSchedule.IsComplied %>" MaxLength="1000" ToolTip="Enter Other Information"
                                                            Text="<%# mAuditSchedule.OtherInformation %>" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td colspan="3"></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td style="width: 100px;">
                                                        <span id="lblToMailID" class="clsLabelAuto">To Mail</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToMailID" runat="server" CssClass=" clsTextBoxTagSearch" MaxLength="500"
                                                            Enabled="<%# Not mAuditSchedule.IsComplied %>" ClientIDMode="Static" ToolTip="Enter To Mail ID's"
                                                            Text="<%# mAuditSchedule.ToMailID %>" Width="273px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblCCMailID" class="clsLabelAuto">CC Mail</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCCMailID" runat="server" CssClass=" clsTextBoxTagSearch" MaxLength="500"
                                                            Enabled="<%# Not mAuditSchedule.IsComplied %>" ClientIDMode="Static" ToolTip="Enter CC Mail ID's"
                                                            Text="<%# mAuditSchedule.CCMailID %>" Width="275px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td colspan="2">&nbsp;
                                                    </td>
                                                    <td colspan="2">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlAuditOn" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <fieldset id="Fieldset3" class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                    <legend id="Legend2" runat="server"><b>Audit On&nbsp;&nbsp;
                                                                    <asp:DropDownList ID="cmbAuditOnList" runat="server" AutoPostBack="true" ClientIDMode="Static"
                                                                        CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="ID" Enabled="<%# Not mAuditSchedule.IsComplied %>"
                                                                        SelectedValue="<%# mAuditSchedule.AuditOnID %>">
                                                                    </asp:DropDownList>
                                                                    </b></legend>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass=" clsTextBoxTagSearchComboNewstyle" DataTextField="RegNo"
                                                                                    DataValueField="ID" Enabled="<%# Not mAuditSchedule.IsComplied %>" SelectedValue="<%# mAuditSchedule.AircraftID %>"
                                                                                    Width="277px">
                                                                                </asp:DropDownList>
                                                                                <asp:DropDownList ID="cmbStore" runat="server" CssClass=" clsTextBoxTagSearchComboNewstyle" DataTextField="LocationStore"
                                                                                    DataValueField="ID" Enabled="<%# Not mAuditSchedule.IsComplied %>" SelectedValue="<%# mAuditSchedule.StoreID %>"
                                                                                    Width="277px">
                                                                                </asp:DropDownList>
                                                                                <asp:DropDownList ID="cmbLocation" runat="server" CssClass=" clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                                                    DataValueField="ID" Enabled="<%# Not mAuditSchedule.IsComplied %>" SelectedValue="<%# mAuditSchedule.LocationID %>"
                                                                                    Width="277px">
                                                                                </asp:DropDownList>
                                                                                <asp:DropDownList ID="cmbVendor" runat="server" CssClass=" clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                                                    DataValueField="ID" Enabled="<%# Not mAuditSchedule.IsComplied %>" SelectedValue="<%# mAuditSchedule.VendorID %>"
                                                                                    Width="277px">
                                                                                </asp:DropDownList>
                                                                                <asp:DropDownList ID="cmbAuditOnDepartment" runat="server" CssClass=" clsTextBoxTagSearchComboNewstyle"
                                                                                    DataTextField="Name" DataValueField="ID" Enabled="<%# Not mAuditSchedule.IsComplied %>"
                                                                                    SelectedValue="<%# mAuditSchedule.AuditOnDepartmentID %>" Width="277px">
                                                                                </asp:DropDownList>
                                                                                <asp:TextBox ID="txtAuditOn" runat="server" CssClass=" clsTextBoxTagSearch" Enabled="<%# Not mAuditSchedule.IsComplied %>"
                                                                                    MaxLength="500" Text="<%# mAuditSchedule.AuditOnText %>">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    </b>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlIsNextSchedule" runat="server" UpdateMode="Conditional"
                                                            Visible="false">
                                                            <ContentTemplate>
                                                                <fieldset id="Fieldset2" class="clsFieldSetNewStyle" style="border-width: 1px" visible="false">
                                                                    <legend id="Legend1" runat="server"><b>
                                                                        <asp:CheckBox ID="chkIsScheduleNextAudit" runat="server" Checked="<%# mAuditSchedule.NextSchedule %>"
                                                                            CssClass="clsCheckBox" Enabled="False" Text="Schedule Next Audit" ToolTip="Schedule Next Audit" />
                                                                    </b></legend>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblFrequency" class="clsLabelAuto">Frequency</span>
                                                                            </td>
                                                                            <td>
                                                                                <table id="Table5" border="0" cellpadding="1" cellspacing="1">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtFrequency" runat="server" AutoPostBack="True" BackColor="#E0E0E0"
                                                                                                CssClass="clsTextBoxRightAlignSmall1_Ajax" MaxLength="4" ReadOnly="True" Text="<%# mAuditSchedule.Frequency %>"
                                                                                                ToolTip="Frequency" Width="40px"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblDays" class="clsLabelAuto">Months</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td>&nbsp; &nbsp; <span id="lblNextAuditDate" class="clsLabelAuto">Next Audit Date</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtNextAuditDate" runat="server" CssClass=" clsTextBoxTagSearch" Enabled="<%# Not mAuditSchedule.IsComplied %>"
                                                                                    onchange="ValidateDateText(this,'NextAuditDate_watermarkextender','false');"
                                                                                    Width="90px"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="NextAuditDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtNextAuditDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender ID="NextAuditDate_watermarkextender" runat="server"
                                                                                    ClientIDMode="Static" TargetControlID="txtNextAuditDate" WatermarkCssClass="clsDateTextBox"
                                                                                    WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td colspan="2">&nbsp;
                                                    </td>
                                                    <td colspan="2">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblNote" class="clsLabelAuto">Note</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewStyleLong" ClientIDMode="Static"
                                                            Enabled="<%# Not mAuditSchedule.IsComplied %>" MaxLength="1000" ToolTip="Enter Note"
                                                            Text="<%# mAuditSchedule.Note %>" TextMode="MultiLine" ></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkNotInUse" runat="server" CssClass="clsCheckBox"
                                                        Text="Mark Schedule as Not In Use" Checked="<%# mAuditSchedule.NotInUse %>" Enabled="<%# Not mAuditSchedule.IsComplied %>"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                disabled="<%# mAuditSchedule.IsComplied %>" causesvalidation="false" runat="server"
                                                                                class="clsbtnH" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH" Enabled="False"
                                                                                CausesValidation="false" Text="Remove Attachment" ToolTip="Click to Remove Attachment"
                                                                                Width="140px" />
                                                                        </td>
                                                                        <td style="padding-left: 2px;">
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                                                ImageUrl="icons/CLIP01.ICO" Width="20px" />
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
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlAuditScheduleTasks" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table id="Table3">
                                                        <tr>
                                                            <td>
                                                                <span id="lblAuditScheduleTask" class="clsLabelHeaderItem">Audit Schedule Task(s)</span>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnAddTask" runat="server" CssClass="clsbtnH" Text="Add" ToolTip="Click to add Audit Schedule Task"
                                                                    Enabled="<%# Not mAuditSchedule.IsComplied %>" CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%">
                                                    <asp:GridView ID="dgAuditScheduleTask" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="true"
                                                        AutoGenerateColumns="False" PageSize="3" CellPadding="10" GridLines="Horizontal">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="SrNo" SortExpression="SrNo" HeaderText="Sr. No.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="20px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AuditCategoryName" SortExpression="AuditCategoryName"
                                                                ItemStyle-Wrap="true" HeaderText="Task Category">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="True" Width="300px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField Visible="False" Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <%--<asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="Remove">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="true" Width="30px" />
                                                            </asp:ButtonField>--%>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="Remove" Style="height: 20px; width: 20px"
                                                                        ImageUrl="~/images/delete.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right" colspan="4">
                                <asp:UpdatePanel ID="upnlActionButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSendMail" runat="server" CssClass="clsButton_Ajax" Visible="<%#Not mAuditSchedule.IsNew %>"
                                                        Text="Send Mail" ToolTip="Click to Send Mail" Enabled="<%# Not mAuditSchedule.IsComplied %>"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Enabled="<%#Not mAuditSchedule.IsNew %>"
                                                        Text="Print" ToolTip="Click to Print Audit Schedule"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to Save Audit Schedule"
                                                        Enabled="<%# Not mAuditSchedule.IsComplied %>"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to Close the Audit Schedule Screen"
                                                        CausesValidation="false"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <tr style="height: 0px;">
                            <td colspan="2" style="height: 0px;">
                                <asp:UpdatePanel runat="server" ID="upnlhdnBtn" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnTaskMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
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
        <!-- End -->
        <!-- TaskMaster Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskMaster" Text="Dummy TaskMaster" ClientIDMode="Static"
                CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupTaskMaster" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupTaskMaster" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskMaster" runat="server" TargetControlID="btnDummyTaskMaster"
            PopupControlID="pnlPopupTaskMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskMasterStateComplete() {
                $("#btnDummyTaskMaster").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenTaskWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupTaskMaster").attr("src", "wfTaskListForAuditSchedule_AJAX.aspx?Type=pup&AType=1");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var TaskMasterwindow = $find("<%=mdlPopupTaskMaster.ClientID %>");
                //close TaskMaster popup window
                TaskMasterwindow.hide();
                $("#iPopupTaskMaster").attr("src", "JavaScript:''");
                //call TaskMaster image button
                $("#hdnimgBtnTaskMaster").click();
            }
        </script>
        <!-- End-->
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
    </form>
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddCustomer = document.getElementById("cmbVendor");
            if (ddCustomer != null) {
                if (ddCustomer.disabled == false) {
                    var j = 0;
              <% For Each item2 In mVendorList%>
                <% If item2.NotInUse = "True" Then%>
                    ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                    j = j + 1;
             <% Next%>
                }
            }
        });
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>
