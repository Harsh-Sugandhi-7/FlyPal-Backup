<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartMonitorMod_AJAX.aspx.vb"
    Inherits="Flypal.wfPartMonitorMod_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part Mod Master</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table id="tblinner" class="clsTablelistin">
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Part Mod [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvATAChapter" runat="server" Display="None" ControlToValidate="cmbATAChapter"
                                            ErrorMessage="Select ATA Chapter From List" CssClass="clsLabelAuto" ClientValidationFunction="validateSelection"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Description Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvModificationNo" runat="server" ControlToValidate="txtModificationNo"
                                            CssClass="clsLabel" Display="None" ErrorMessage="Modification No Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvIssueDate" runat="server" ControlToValidate="calIssueDate"
                                            CssClass="clsLabel" Display="None" ErrorMessage="Issue Date Required"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvDescription" runat="server" OnServerValidate="CustomValidate"
                                            ControlToValidate="txtDescription" CssClass="clsLabelAuto" Display="None" ErrorMessage="Description can not be more than 1000 chars"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvReference" runat="server" ControlToValidate="txtReference"
                                            CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvMonitorSerType" runat="server" Display="None" ControlToValidate="cmbMonitorModType"
                                            ErrorMessage="Select Mod Type from List" CssClass="clsLabelAuto" ClientValidationFunction="validateSelection"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvFrequencyValue" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ErrorMessage="Enter valid Frequency value." OnServerValidate="customvalidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNote" runat="server" ClientValidationFunction="validateName"
                                            ControlToValidate="txtNote" CssClass="clsLabelAuto" Display="None" ErrorMessage="Note can not be more than 1000 chars"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateSelection(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'cmbATAChapter':
                                                        var Value = $get(ControlName).selectedIndex;
                                                        if (Value == 0) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'cmbMonitorModType':
                                                        var Value = $get(ControlName).selectedIndex;
                                                        if (Value == 0) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                }

                                            }

                                            function validateName(source, args) {
                                                //args.IsValid = false;
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {

                                                    case 'txtNote':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 1000) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                }
                                            }
                                        </script>
                                         <asp:CustomValidator ID="cvCc" runat="server" Display="None" ControlToValidate="txtRefAttachLink"
                                                ErrorMessage="Please Enter Valid Reference link" CssClass="" ClientValidationFunction="validURL"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validURL(source, args) {
                                                    var text = $("#txtRefAttachLink").val();
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
                                                        return;
                                                    }
                                                    else {
                                                        args.IsValid = true;
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
                                <asp:UpdatePanel ID="upnlMonitorDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsMonitorModingDetails" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="lblMonitorModingDetails"><b>Modification Details</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblCode" class="clsLabelAuto">Code/Form No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mPartMonitorMod.Code %>"
                                                            ToolTip="Enter Code"  Width="252px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span id="lblStarATA" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblATAChapter" class="clsLabelAuto">ATA Chapter</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlATAMaster" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox2_Ajax"
                                                                                SelectedValue="<%# mPartMonitorMod.ATAID %>" DataTextField="ATAChapter" DataValueField="ID">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imgbtnATAChapter" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to add new ATA chapter." CausesValidation="False">
                                                                            </asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblReference" CssClass="clsLabel">Reference</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            Text="<%# mPartMonitorMod.Reference %>" ToolTip="Enter Reference" MaxLength="500"
                                                            TextMode="MultiLine" Width="250px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span id="lblStarDesc" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDescription" class="clsLabel">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            ClientIDMode="Static" Text="<%# mPartMonitorMod.Description %>" ToolTip="Enter Description"
                                                            TextMode="MultiLine" Width="250px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span id="lblStarMonitor" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblMonitorModType" class="clsLabelAuto">Mod Type</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlMonitorModType" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbMonitorModType" runat="server" CssClass="clsComboBox2_Ajax"
                                                                    SelectedValue="<%# mPartMonitorMod.PartMonitorModTypeID %>" DataTextField="CodeType"
                                                                    DataValueField="Id" AutoPostBack="True" Width="257px">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblStarModificationNo" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblModNo" runat="server" CssClass="clsLabelAuto">Modification No. </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtModificationNo" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            Text="<%# mPartMonitorMod.Number %>" ToolTip="Enter Modification Number" MaxLength="150"
                                                            TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblStarIssueDate" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblIssueDate" runat="server" CssClass="clsLabelAuto">Effective Date </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%--                                                        <uc1:sicalendar id="calIssueDate" runat="server"></uc1:sicalendar>--%>
                                                        <asp:TextBox runat="server" ID="calIssueDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calIssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calIssueDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="calIssueDate" ID="DoneOnDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" Text="Applicable"
                                                            ToolTip="Check if applicable" Enabled="<%# Not (mPartMonitorMod.ReadOnlyFrequencyColumn) %>"
                                                            Checked="<%# mPartMonitorMod.IsApplicable %>" TextAlign="Left"></asp:CheckBox>
                                                    </td>
                                                </tr>
												<tr>
													<td></td>
													<td>
														<span id="lblIssuingAuthority" class="clsLabel">Issuing Authority</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbIssuingAuthority" runat="server" CssClass="clsComboBox2_Ajax"
															SelectedValue="<%# mPartMonitorMod.IssuingAuthorityID %>" DataTextField="Name"
															DataValueField="ID" AutoPostBack="True" Width="250px">
														</asp:DropDownList>
													</td>
												</tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblApplicability" runat="server" CssClass="clsLabelAuto">Applicability</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtApplicability" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            Text="<%# mPartMonitorMod.Applicability %>" ToolTip="Enter Applicability" MaxLength="1000"
                                                            TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblComplianceRequirement" runat="server" CssClass="clsLabelAuto">Compliance Requirement</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtComplianceRequirement" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            Text="<%# mPartMonitorMod.ComplianceRequirement %>" ToolTip="Enter Compliance Requirement"
                                                            MaxLength="1000" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblZone" runat="server" class="clsLabel">Zone </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                            Text="<%# mPartMonitorMod.Zone %>" ToolTip="Enter Zone" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblArea" class="clsLabelAuto">Area</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                            Text="<%# mPartMonitorMod.Area %>" ToolTip="Enter Area" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRII" class="clsLabelAuto">RII</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsRII" runat="server" Checked="<%# mPartMonitorMod.IsRII %>"
                                                            Text="(Check if RII)" CssClass="clsCheckBox" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblNote" class="clsLabel">Note</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="1000"
                                                            ClientIDMode="Static" Width="250px" Text="<%# mPartMonitorMod.Note %>" TextMode="MultiLine"
                                                            ToolTip="Enter Note">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblShowInCofA" class="clsLabelAuto">Show In C of A</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkShowInCofA" runat="server" Checked="<%# mPartMonitorMod.ShowInCofA %>"
                                                            ToolTip="Check if want to display in C Of A." />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRequiredmanHours" class="clsLabelAuto">Estd. Man Hours</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRequiredManHours" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                            MaxLength="8" Text="<%# mPartMonitorMod.RequiredManHours %>" ToolTip="Enter Required Man Hours">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>
                                                        <table id="Table12" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                            runat="server" class="clsButton_Ajax" causesvalidation="False" />
                                                                                    </td>
                                                                                    <td style="padding-left: 3px;">
                                                                                        <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                            CausesValidation="false" Text="Remove Attachment" Enabled="False" Width="120px">
                                                                                        </asp:Button>
                                                                                    </td>
                                                                                    <td style="padding-left: 2px;">
                                                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                            Height="20px" Width="20px"></asp:ImageButton>
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
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlPeriods" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdsTotWtAndCapacity" class="clsFieldSet" style="border-width: 1px;">
                                                        <legend id="lblTotWtAndCapacity"><b>Frequency of Mod</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:GridView ID="dgPeriods" runat="server" CssClass="clsGrid" PageSize="3" AutoGenerateColumns="False"
                                                                        ShowHeaderWhenEmpty="true">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Frequency">
                                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFrequencyValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                        ReadOnly="<%# mPartMonitorMod.ReadOnlyFrequencyColumn %>" Text='<%# DataBinder.Eval(Container.DataItem, "FrequencyValueFormatted") %>'>
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left">
                                                                            </asp:ButtonField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                                <td valign="top" align="right">
                                                                    <asp:ImageButton ID="btnAddPeriodUnit" runat="server" ImageUrl="~/images/plus1.png"
                                                                        Height="22px" Width="24px" ToolTip="Click to Add New period" CausesValidation="False">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 50px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlOtherDetails" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdsOtherDetails" class="clsFieldSet" style="border-width: 1px;">
                                                        <legend id="Legend1"><b>Other Details</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <table cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <input id="imgTools" type="image" src="images/Tool.png" disabled="disabled" style="height: 22px;
                                                                                    width: 24px" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkTools" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                    Enabled="<%# Not mPartMonitorMod.IsNew %>" ToolTip="Click to add Tools" Text="Tools (0 records)"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <table cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <input id="imgSpares" type="image" src="images/Spare.png" disabled="disabled" style="height: 22px;
                                                                                    width: 24px" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkSpares" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                    Enabled="<%# Not mPartMonitorMod.IsNew %>" ToolTip="Click to add Spares" Text="Spares (0 records)"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <table cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <input id="imgTaskCard" type="image" src="images/TaskCard.png" disabled="disabled"
                                                                                    style="height: 22px; width: 24px" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkTaskCards" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                    Enabled="<%# Not mPartMonitorMod.IsNew %>" ToolTip="Click to add Task Cards"
                                                                                    Text="Task Cards (0 records)"></asp:LinkButton>
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
                                        <td style="height: 50px">
                                        </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset id="Fieldset1" class="clsFieldSet" 
                                                            style="border-width: 1px; width: auto">
                                                            <legend id="Legend2"><b>Reference HyperLink</b></legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRefAttachLink" runat="server" 
                                                                            CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500" 
                                                                            Text="<%# mPartMonitorMod.RefAttachlink %>" TextMode="MultiLine" 
                                                                            ToolTip="Enter Reference Link" Width="250px">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblHyperlink" runat="server" 
                                                                            Text="Website Reference link for AD/SB"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveSelect" runat="server" CssClass="clsButton_Ajax" Text="Save &amp; Select"
                                                        ToolTip="Click to Save and Select Part Mod" Visible='<%# not Session("EditMasterRecord") = "True" %>'>
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to Save Part Mod">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print" ToolTip="Click to Print Part Mod">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to previous page"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup for category/nomenclature-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnATAChapter" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnPeriodUnit" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnTools" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
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

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                    //                        $("#IFileUpload").ready(function () {
                    //                            $("#btnDummyFileUpload").click();
                    //                            $get("AjaxLoader").style.visibility = 'hidden';
                    //                        });
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

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
    <!-- End -->
    <!-- ATA Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyATA" Text="Dummy ATA" ClientIDMode="Static"
            CausesValidation="False" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupATA" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupATA" frameborder="0" allowtransparency="true" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupATA" runat="server" TargetControlID="btnDummyATA"
        PopupControlID="pnlPopupATA" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameATAStateComplete() {
            $("#btnDummyATA").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#imgbtnATAChapter").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupATA").attr("src", "wfATA_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyATA").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunction() {
            var atawindow = $find("<%=mdlPopupATA.ClientID %>");
            //close ata popup window
            atawindow.hide();
            $("#iPopupATA").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnATAChapter").click();
        }
    </script>
    <!-- End-->
    <!-- Period Unit popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPeriodUnit" Text="Period Unit" ClientIDMode="Static"
            CausesValidation="False" />
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
    <!-- Tools Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyTools" Text="Tools" ClientIDMode="Static"
            CausesValidation="False" />
    </div>
    <asp:Panel runat="server" ID="pnlTools" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeTools" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupTools" runat="server" TargetControlID="btnDummyTools"
        PopupControlID="pnlTools" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameToolsStateComplete() {
            $("#btnDummyTools").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenToolsWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeTools").attr("src", "wfMaintenanceKitandTask_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyTools").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForTools() {
            var Toolswindow = $find("<%=mdlPopupTools.ClientID %>");
            //close TTools popup window
            Toolswindow.hide();
            //           release resources
            $("#IframeTools").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnTools").click();
        }
    </script>
    <!-- End-->
    <!-- ModMaster Popup Window -->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForModMaster();
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
             parent.IFrameModMasterStateComplete();
         }
    });

    <% End if %>
       Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            SetPageLayout();
        }

       function SetPageLayout()
       {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
          ReSetPageLayout();
          onResize();//for Top bottom link
           <% End if %>
       }
       function ReSetPageLayout()
       {
       $("body,html").css({ 'background-color': 'transparent' });
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <%--End--%>
    </form>
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var resetTodaysDate = 'true';
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
</body>
</html>
