<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDentBuckelItems_Ajax.aspx.vb"
    Inherits="Flypal.wfDentBuckelItems_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Items</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table class="clstablelistin" id="tblLedgerList">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Report</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table align="right">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Add"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        ToolTip="Click to go back to the previous page"
                                                                        CausesValidation="False" Text="Back"></asp:Button>
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
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvItemNo" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Enter Item No." ControlToValidate="txtItemNo" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvATA" runat="server" ControlToValidate="cmbATA" ValidateEmptyText="true"
                                                ClientValidationFunction="ValidateATA" Display="None" ErrorMessage="Select ATA from the list"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Enter Description" ControlToValidate="txtDescription" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvMobLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Item No. should not be greater than 100 characters"
                                                Display="None" ControlToValidate="txtItemNo" ClientValidationFunction="validateName"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Reference should not be greater than 500 characters" Display="None"
                                                ControlToValidate="txtReference" ClientValidationFunction="validateName"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Approval Doc. should not be greater than 500 characters" Display="None"
                                                ControlToValidate="txtApprovalDoc" ClientValidationFunction="validateName"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator3" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Remark should not be greater than 1000 characters" Display="None"
                                                ControlToValidate="txtRemark" ClientValidationFunction="validateName"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator4" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Corrective Action Remark should not be greater than 1000 characters"
                                                Display="None" ControlToValidate="txtCorrectiveActionRemark" ClientValidationFunction="validateName">
                                            </asp:CustomValidator>
                                            <asp:CustomValidator ID="cvVendor" runat="server" Display="None" ControlToValidate="cmbItemStatus"
                                                ValidateEmptyText="true" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator5" runat="server" Display="None" ControlToValidate="txtActionTakenByEmployee"
                                                ValidateEmptyText="true" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function ValidateATA(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbATA");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }

                                                function validateName(source, args) {
                                                    //args.IsValid = false;
                                                    var ControlName = source.controltovalidate;
                                                    switch (ControlName) {
                                                        case 'txtItemNo':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 100) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                            break;
                                                        case 'txtReference':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 500) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                            break;
                                                        case 'txtApprovalDoc':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 500) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                            break;
                                                        case 'txtRemark':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 1000) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                            break;
                                                        case 'txtCorrectiveActionRemark':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 1000) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                            break;
                                                    }
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend><b>Report Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td style="width: 150px;">
                                                            <span id="spnSrNo" class="clsLabel">Sr. No.</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtSrNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                                MaxLength="4" ReadOnly="True" Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.SrNo %>"
                                                                ToolTip="Sr. No." Width="36px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStarCharge" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="Span1" class="clsLabel">Item No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtItemNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.ItemNo %>" ToolTip="Enter Item No."></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="Span11" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblChargeName" class="clsLabelAuto">ATA</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbATA" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ATAChapter"
                                                                DataValueField="ID" SelectedValue="<%# mDentBuckle.DentBuckleItems.CurrentItem.ATAID %>"
                                                                Width="185px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="Span8" class="clsLabel">Reported By</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReportedByEmployee" runat="server" AutoComplete="off" AutoPostBack="true"
                                                                ClientIDMode="Static" CssClass="clsTextBoxTagSearch" onChange="SetEmpIdonChange('txtReportedByEmployee')"
                                                                OnTextChanged="txtReportedByEmployee_TextChanged"></asp:TextBox>
                                                            <cc2:AutoCompleteExtender ID="txtReportedByEmployee_Autocomplete" runat="server"
                                                                ClientIDMode="Static" CompletionInterval="1" CompletionListCssClass="ac_results_Main"
                                                                CompletionListHighlightedItemCssClass="ac_over_Main" CompletionListItemCssClass="ac_results_li"
                                                                CompletionSetCount="20" ContextKey="" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0"
                                                                OnClientHiding="ClientHiding" OnClientItemSelected="SetID" OnClientPopulated="ClientPopulated"
                                                                OnClientPopulating="ClientPopulating" OnClientShowing="ClientShowing" OnClientShown="ClientHiding"
                                                                ServiceMethod="GetEmployeeList" ServicePath="" TargetControlID="txtReportedByEmployee"
                                                                UseContextKey="False">
                                                            </cc2:AutoCompleteExtender>
                                                            <asp:HiddenField ID="hdnReportedByEmpId" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                        <td>
                                                        <td>
                                                            <span id="Span28" class="clsLabelAuto">Damage Type</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbDamageTypeID" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mDentBuckle.DentBuckleItems.CurrentItem.DamageTypeID %>"
                                                                Width="185px">
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="btnAddDamageList" runat="server" ImageUrl="~/images/plus1.png"
                                                                CausesValidation="false" Height="22px" Width="24px" Style="vertical-align: top;" ToolTip="Click to Add New"></asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                    </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span15" class="clsLabelStar"></span>
                                                    </td>
                                                    <td>
                                                        <span id="Span18" class="clsLabel">Damage Location(Zone)</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDamageLocation" runat="server"
                                                            CssClass="clsTextBoxTagSearchMultilineNewstyle" TextMode="MultiLine"
                                                            Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.DamageLocation %>"
                                                            ToolTip="Enter Damage Location"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <span id="Span19" class="clsLabel">Dimensions</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDimensions" runat="server"
                                                            CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.Dimensions %>"
                                                            ToolTip="Enter Dimensions" TextMode="MultiLine"
                                                            Width="178px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span12" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="Span2" class="clsLabel">Description</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtDescription" runat="server" Width="465px"
                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.Description %>"
                                                                TextMode="MultiLine" ToolTip="Enter Damage Description">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend><b>Acceptance Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td style="width: 150px;">
                                                            <span id="Span3" class="clsLabel">Reference</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.Reference %>"
                                                                ToolTip="Enter Reference"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="Span14" class="clsLabel">Approval Doc.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtApprovalDoc" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.ApprovalDoc %>"
                                                                ToolTip="Enter Approval Document details"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="Span4" class="clsLabel">Acceptance Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDateofAcceptance" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                onchange="ValidateDateText(this,'DateofAcceptance_watermarkextender','false');"
                                                                ToolTip="Enter Acceptance Date" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtDateofAcceptance_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDateofAcceptance"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="DateofAcceptance_watermarkextender" runat="server"
                                                                TargetControlID="txtDateofAcceptance" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="Span9" class="clsLabel">Acceptance By</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAcceptanceByEmployee" runat="server" AutoComplete="off" AutoPostBack="true"
                                                                ClientIDMode="Static" CssClass="clsTextBoxTagSearch" onChange="SetEmpIdonChange('txtAcceptanceByEmployee')"
                                                                OnTextChanged="AcceptanceByEmployee_Changed"></asp:TextBox>
                                                            <cc2:AutoCompleteExtender ID="txtAcceptanceByEmployee_Autocomplete" runat="server"
                                                                ClientIDMode="Static" CompletionInterval="1" CompletionListCssClass="ac_results_Main"
                                                                CompletionListHighlightedItemCssClass="ac_over_Main" CompletionListItemCssClass="ac_results_li"
                                                                CompletionSetCount="20" ContextKey="" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0"
                                                                OnClientHiding="ClientHiding" OnClientItemSelected="SetID" OnClientPopulated="ClientPopulated"
                                                                OnClientPopulating="ClientPopulating" OnClientShowing="ClientShowing" OnClientShown="ClientHiding"
                                                                ServiceMethod="GetEmployeeList" ServicePath="" TargetControlID="txtAcceptanceByEmployee"
                                                                UseContextKey="False">
                                                            </cc2:AutoCompleteExtender>
                                                            <asp:HiddenField ID="hdnAcceptanceByEmpId" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblType" runat="server" class="clsLabel">Type</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:DropDownList ID="cmbDentBuckleType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mDentBuckle.DentBuckleItems.CurrentItem.DentBuckleTypeID %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="Span5" class="clsLabel">Acceptable Description</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtAcceptableDescription" runat="server"
                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.AcceptableDescription %>"
                                                                TextMode="MultiLine" ToolTip="Enter Acceptable Description" Width="470px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="Span7" class="clsLabel">Remark</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtRemark" runat="server"
                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.Remark %>"
                                                                TextMode="MultiLine" ToolTip="Enter Remark" Width="470px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend><b>Rectification Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td style="width: 150px;">
                                                            <span id="Span17" class="clsLabel">Corrective Action Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCorrectiveActionDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                                onchange="ValidateDateText(this,'CorrectiveActionDate_watermarkextender','false');"
                                                                ToolTip="Enter Corrective Action Date" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCorrectiveActionDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="CorrectiveActionDate_watermarkextender" runat="server"
                                                                TargetControlID="txtCorrectiveActionDate" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblActionTakenBy" runat="server" CssClass="clsLabelStar" Visible="<%# mDentBuckle.DentBuckleItems.CurrentItem.ItemStatusID=3 %>">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="Span10" class="clsLabel">Action Taken By</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtActionTakenByEmployee" runat="server" AutoComplete="off" AutoPostBack="true"
                                                                ClientIDMode="Static" CssClass="clsTextBoxTagSearch" onChange="SetEmpIdonChange('txtActionTakenByEmployee')"
                                                                OnTextChanged="txtActionTakenByEmployee_TextChanged"></asp:TextBox>
                                                            <cc2:AutoCompleteExtender ID="txtActionTakenByEmployee_Autocomplete" runat="server"
                                                                ClientIDMode="Static" CompletionInterval="1" CompletionListCssClass="ac_results_Main"
                                                                CompletionListHighlightedItemCssClass="ac_over_Main" CompletionListItemCssClass="ac_results_li"
                                                                CompletionSetCount="20" ContextKey="" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0"
                                                                OnClientHiding="ClientHiding" OnClientItemSelected="SetID" OnClientPopulated="ClientPopulated"
                                                                OnClientPopulating="ClientPopulating" OnClientShowing="ClientShowing" OnClientShown="ClientHiding"
                                                                ServiceMethod="GetEmployeeList" ServicePath="" TargetControlID="txtActionTakenByEmployee"
                                                                UseContextKey="False">
                                                            </cc2:AutoCompleteExtender>
                                                            <asp:HiddenField ID="hdnActionTakenByEmpId" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTempAction" runat="server" CssClass="clsLabelStar" Visible="<%# mDentBuckle.DentBuckleItems.CurrentItem.ItemStatusID=2 %>">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="Span6" class="clsLabel">Temporary Action</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtTemporaryAction" runat="server"
                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.TemporaryAction %>"
                                                                TextMode="MultiLine" ToolTip="Enter Temporary Action" Width="388px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <%---------------------------%>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="Span25" class="clsLabel">Performance Details</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtPerformanceDetails" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.PerformanceDetails %>"
                                                                ToolTip="Enter Performance Details" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCorrectiveActionRemark" runat="server" CssClass="clsLabelStar"
                                                                Visible="<%# mDentBuckle.DentBuckleItems.CurrentItem.ItemStatusID=3 %>">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="Span13" class="clsLabel">Corrective Action Remark</span>&nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtCorrectiveActionRemark" runat="server"
                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.CorrectiveActionRemark %>"
                                                                TextMode="MultiLine" ToolTip="Enter Corrective Action Remark" Width="388px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Span21" class="clsLabel">W.O. No.</span>&nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtWoNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.WorkOrderNo %>"
                                                                ToolTip="Enter Work Order No." Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Span211" class="clsLabel">Station & Stringer</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtStationAndStringer" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.StationAndStringer %>"
                                                                ToolTip="Enter Station And Stringer" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Span24" class="clsLabel">Interval</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtInterval" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.Interval %>"
                                                                ToolTip="Enter Interval" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Span241" class="clsLabel">Threshold</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtThreshold" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.Threshold %>"
                                                                ToolTip="Enter Threshold" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Span22" class="clsLabel">Done at Hr/Cycles</span>&nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtDoneAtHrCycle" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.DoneAtHrCycles %>"
                                                                ToolTip="Enter Done at Hr/Cycles" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>

                                                        <td>
                                                            <span id="Span26" class="clsLabel">Next Due</span>&nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtNextDue" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.NextDue %>"
                                                                ToolTip="Enter Next Due" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>

                                                        <td>
                                                            <span id="Span23" class="clsLabel">Next Due Remark(if any)</span>&nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtNextDueRemark" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.NextDueRemark %>" ToolTip="Enter Done at Next Due Remark(if any)"
                                                                Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblRemaining" runat="server" class="clsLabel">Remaining</span>&nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtRemaining" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mDentBuckle.DentBuckleItems.CurrentItem.Remaining %>" ToolTip="Enter Remaining"
                                                                Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <fieldset class="clsFieldSetNewStyle" id="Report Status">
                                                <legend><b>Report Status</b></legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="cmbItemStatus" runat="server" AutoPostBack="true"
                                                                CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataTextField="StatusName" DataValueField="StatusID"
                                                                SelectedValue="<%# mDentBuckle.DentBuckleItems.CurrentItem.ItemStatusID %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnDamageTypeList" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
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

        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

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
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid, TobeReset) {

                var datevalue = $(elem).val();
                var resetTodaysDate = TobeReset;
                var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    //        contentType: "application/json",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
                    //                beforeSend: function (xhr, settings) {
                    //                    $("[id$=processing]").dialog();
                    //                },
                    success: onSuccess,
                    error: onError
                });

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
        <%--
    Autocomplete functions to set id--%>
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

                var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
                source.get_element().value = text;

                //Set id to relevent hidden field 
                var textbox;
                if (source._id == "txtActionTakenByEmployee_Autocomplete") {
                    textbox = document.getElementById('hdnActionTakenByEmpId');
                }
                else if (source._id == "txtReportedByEmployee_Autocomplete") {
                    textbox = document.getElementById('hdnReportedByEmpId');
                }
                else if (source._id == "txtAcceptanceByEmployee_Autocomplete") {
                    textbox = document.getElementById('hdnAcceptanceByEmpId');
                }


                textbox.value = value.toString();
            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetEmpIdonChange(elem) {
                switch (elem) {
                    case 'txtActionTakenByEmployee':
                        var popup = $find("txtActionTakenByEmployee_Autocomplete");
                        var complist = popup.get_completionList();
                        var text = $("#txtActionTakenByEmployee").val().toLowerCase();
                        for (var i = 0; i < complist.childNodes.length; i++) {
                            var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                            if (text == texttocompare) {
                                var val = complist.childNodes[i]._value;
                                var textbox = document.getElementById('hdnActionTakenByEmpId');
                                textbox.value = val.toString();
                                return;
                            }
                        }
                        var textbox = document.getElementById('hdnActionTakenByEmpId');
                        textbox.value = '';
                        return;
                    case 'txtReportedByEmployee':
                        var popup = $find("txtReportedByEmployee_Autocomplete");
                        var complist = popup.get_completionList();
                        var text = $("#txtReportedByEmployee").val().toLowerCase();
                        for (var i = 0; i < complist.childNodes.length; i++) {
                            var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                            if (text == texttocompare) {
                                var val = complist.childNodes[i]._value;
                                var textbox = document.getElementById('hdnReportedByEmpId');
                                textbox.value = val.toString();
                                return;
                            }
                        }
                        var textbox = document.getElementById('hdnReportedByEmpId');
                        textbox.value = '';
                        return;
                    case 'txtAcceptanceByEmployee':
                        var popup = $find("txtAcceptanceByEmployee_Autocomplete");
                        var complist = popup.get_completionList();
                        var text = $("#txtAcceptanceByEmployee").val().toLowerCase();
                        for (var i = 0; i < complist.childNodes.length; i++) {
                            var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                            if (text == texttocompare) {
                                var val = complist.childNodes[i]._value;
                                var textbox = document.getElementById('hdnAcceptanceByEmpId');
                                textbox.value = val.toString();
                                return;
                            }
                        }
                        var textbox = document.getElementById('hdnAcceptanceByEmpId');
                        textbox.value = '';
                        return;
                }
            }

        </script>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForItems();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameItemsStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
        <!--PropertyValue Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDamageTypeList" Text="PropertyValue" CausesValidation="true"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlDamageTypeList" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeDamageTypeList" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupPropertyValue" runat="server" TargetControlID="btnDummyDamageTypeList"
            PopupControlID="pnlDamageTypeList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameDamageTypeListStateComplete() {
                $("#btnDummyDamageTypeList").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenDamageTypeListWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeDamageTypeList").attr("src", "wfDamageTypeList.aspx?Type=pup");

                    //  if (!$.browser.msie) {
                    $("#btnDummyDamageTypeList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                    //}
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForDamageTypeList() {
                var DamageTypeListWindow = $find("<%=mdlPopupPropertyValue.ClientID %>");
                //close popup window
                DamageTypeListWindow.hide();
                //release resources
                $("#IframeDamageTypeList").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnDamageTypeList").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
