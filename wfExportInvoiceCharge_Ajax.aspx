<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExportInvoiceCharge_Ajax.aspx.vb"
    Inherits="Flypal.wfExportInvoiceCharge_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Export Invoice Charge</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
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
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
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
                                                <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Export Invoice Charge</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table align="right">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
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
                                            <asp:CustomValidator ID="cvCharge" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                Display="None" ErrorMessage="Charge Name Required" ControlToValidate="cmbCharge"></asp:CustomValidator><asp:CustomValidator
                                                    ID="cvPercentage" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                    Display="None" ErrorMessage="Percentage should  be Greater than 0" ControlToValidate="txtPercentage"></asp:CustomValidator><asp:CustomValidator
                                                        ID="cvAmount" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                        Display="None" ErrorMessage="Amount should be Greater than 0" ControlToValidate="txtChargeAmount"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlOtherChargeDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="lblOtherChargeDetails" class="clsLabelHeader">Other Charge Details</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarCharge" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblChargeName" class="clsLabelAuto">Charge Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbCharge" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                            DataTextField="Name" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <%--<asp:Button ID="imgbtnCharge" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Charge"
                                                            CausesValidation="False" Text="..."></asp:Button>--%>
                                                        <asp:ImageButton ID="imgbtnCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                        ToolTip="Click to Add New Charge" CausesValidation="False"></asp:ImageButton>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblPercentage" class="clsLabel">Percentage </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearch" style="text-align:right"
                                                            Text="<%# mExportInvoice.ExportInvoiceCharges.CurrentItem.Percentage %>" ToolTip="Enter Percentage"
                                                            ReadOnly="True" BackColor="#E0E0E0" MaxLength="12"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblChargeAmount" class="clsLabelAuto">Charge Amount </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtChargeAmount" runat="server" CssClass="clsTextBoxTagSearch" style="text-align:right"
                                                            Text="<%# mExportInvoice.ExportInvoiceCharges.CurrentItem.CChargeAmount %>" ToolTip="Enter Charge Amount"
                                                            ReadOnly="True" BackColor="#E0E0E0" MaxLength="12"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table align="right">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                            CausesValidation="False" Text="Back"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
