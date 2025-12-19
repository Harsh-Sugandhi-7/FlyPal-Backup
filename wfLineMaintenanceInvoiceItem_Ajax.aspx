<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLineMaintenanceInvoiceItem_Ajax.aspx.vb"
    Inherits="Flypal.wfLineMaintenanceInvoiceItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Service Invoice Item Details</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                        <table id="tblinner" class="clsTablelistin">
                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="5" class="clsFormHeader1Newstyle">
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Service Invoice Item [New]</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                        <asp:RequiredFieldValidator ID="rfvJobDetails" runat="server" Display="None" CssClass="clsLabelAuto"
                                                            ControlToValidate="txtJobDetails" ErrorMessage="Job Details Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" Display="None" CssClass="clsLabelAuto"
                                                            ControlToValidate="txtQty" ErrorMessage="Quantity Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvRate" runat="server" Display="None" CssClass="clslabel"
                                                            ControlToValidate="txtRate" ErrorMessage="Rate Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvNote" runat="server" Display="None" ControlToValidate="txtNote"
                                                            ErrorMessage="Note must not be greater than 250 Char." ClientValidationFunction="validateNameLength"
                                                            ValidationGroup="1"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvRemark" runat="server" Display="None" ControlToValidate="txtRemark"
                                                            ErrorMessage="Remark must not be greater than 250 Char." ClientValidationFunction="validateNameLength"
                                                            ValidationGroup="1"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvcustom1" runat="server" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvCRate" runat="server" Display="None" ControlToValidate="txtRate"
                                                            ErrorMessage="Rate must be greater than zero." ClientValidationFunction="validateForZeroValue"
                                                            ValidationGroup="1"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvQty" runat="server" Display="None" ControlToValidate="txtQty"
                                                            ErrorMessage="Quantity must be greater than zero." ClientValidationFunction="validateForZeroValue"
                                                            ValidationGroup="1"></asp:CustomValidator>
                                                        <script type="text/javascript">

                                                            function validateForZeroValue(source, args) {
                                                                args.IsValid = false;
                                                                var TextBoxValue = parseInt(document.getElementById(source.controltovalidate).value);
                                                                if (TextBoxValue > 0) {
                                                                    args.IsValid = true;
                                                                    return
                                                                }
                                                            }
                                                            function validateNameLength(source, args) {
                                                                //args.IsValid = false;
                                                                var ControlName = source.controltovalidate;
                                                                switch (ControlName) {
                                                                    case 'txtNote':
                                                                        var Value = $get(ControlName).value.length;
                                                                        if (Value > 250) {
                                                                            args.IsValid = false;
                                                                            return
                                                                        }
                                                                        break;
                                                                    case 'txtRemark':
                                                                        var Value = $get(ControlName).value.length;
                                                                        if (Value > 250) {
                                                                            args.IsValid = false;
                                                                            return
                                                                        }
                                                                        break;
                                                                }
                                                            }
                                                        </script>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <span id="lblOrderInfo" class="clsLabelHeader">Service Invoice Item Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblSrNo" class="clsLabel">Sr. No.</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.SrNo %>"
                                                            MaxLength="5" BackColor="#E0E0E0" ReadOnly="True">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarDesc" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblJobDetails" class="clsLabel">Job Details</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtJobDetails" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.JobDetails %>"
                                                            MaxLength="500" BackColor="White" TextMode="MultiLine" ToolTip="Enter Job Details"
                                                            Style="width: 350px;">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Label2" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblQuant" class="clsLabel">Quantity</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Qty %>"
                                                                        MaxLength="8" ToolTip="Enter Quantity">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblUnit" class="clsLabelAuto">Unit</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtUnit" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Unit %>"
                                                                        MaxLength="10" ToolTip="Enter Unit">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Label9" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblRate" class="clsLabel">Rate</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.CRate %>"
                                                            MaxLength="12" ToolTip="Enter Rate" Width="150px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblAmount" class="clsLabel">Amount</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.CAmount %>"
                                                            BackColor="#E0E0E0" ReadOnly="True" MaxLength="12" Width="150px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblRemark" class="clsLabel">Remark</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Remark %>"
                                                            MaxLength="250" TextMode="MultiLine" ToolTip="Enter Remark" Style="width: 350px;">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblNote" class="clsLabel">Note</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Note %>"
                                                            MaxLength="250" TextMode="MultiLine" ToolTip="Enter Note" Style="width: 350px;">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ValidationGroup="1" ToolTip="Click to add Item in Service Invoice Item List"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
