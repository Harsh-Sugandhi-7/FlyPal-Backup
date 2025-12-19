<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExportInvoiceItem_Ajax.aspx.vb"
    Inherits="Flypal.wfExportInvoiceItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Export Invoice Item Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
                <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                    <table id="tblinner" class="clsTablelistin">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Export Invoice Item [New]</asp:Label>
                                        </td>
                                        <%--<td align="right">
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add Item in Export Invoice Item List"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span id="lblOrderInfo" class="clsLabelHeader">Export Invoice Item Information</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblSrNo" class="clsLabel">Sr. No.</span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ReadOnly="True"
                                    BackColor="#E0E0E0" MaxLength="5" Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.SrNo %>">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label1" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblPartNo" class="clsLabel">Part No.</span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                    BackColor="#E0E0E0" Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.PartNo %>"
                                    ToolTip="Enter Part No.">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label3" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblDescription" class="clsLabel">Description</span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxSearch_Ajax" BackColor="#E0E0E0"
                                    Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.Description %>" ToolTip="Enter Description"
                                    TextMode="MultiLine">
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
                                <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.Qty %>"
                                  style="text-align:right"  ToolTip="Enter Quantity" ReadOnly="True" BackColor="#E0E0E0">
                                </asp:TextBox>
                            </td>
                            <td>
                                <span id="lblUnit" class="clsLabelAuto">Unit</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnit" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.UnitName %>"
                                    ToolTip="Enter Unit" ReadOnly="True" BackColor="#E0E0E0">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label9" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblRate" runat="server" class="clsLabel">Rate</span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.CRate %>"
                                  style="text-align:right"  MaxLength="12" ToolTip="Enter Rate" Enabled="<%# mExportInvoice.StatusID = 1 %>">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblAmount" class="clsLabel">Amount</span>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="clsTextBoxTagSearch"
                                    Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.CAmount %>" BackColor="#E0E0E0" style="text-align:right"
                                    ReadOnly="True">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblNote" class="clsLabel">Note</span>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mExportInvoice.ExportInvoiceItems.CurrentItem.Note %>"
                                    MaxLength="500" TextMode="MultiLine" ToolTip="Enter Note" Enabled="<%# mExportInvoice.StatusID = 1 %>">
                                </asp:TextBox>
                                <asp:CustomValidator ID="cvNote" runat="server" Display="None" ControlToValidate="txtNote"
                                    OnServerValidate="customvalidate" CssClass="clslabel"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td colspan="5" align="right">
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Ok" ToolTip="Click to add Item in Export Invoice Item List" Enabled="<%# mExportInvoice.StatusID = 1 %>">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back" ToolTip="Click to go back to the previous page"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
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
