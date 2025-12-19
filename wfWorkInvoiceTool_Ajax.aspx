<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfWorkInvoiceTool_Ajax.aspx.vb"
    Inherits="Flypal.wfWorkInvoiceTool_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Invoice Tool Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
    <table class="clstablelistout" id="tblMain" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table class="clsTablelistin" id="tblinner" border="0">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Work Invoice Tool [New]</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ErrorMessage="Quantity Required"
                                            ControlToValidate="txtQty" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" ErrorMessage="Tool can't be saved without Description."
                                            ControlToValidate="txtDescription" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvRate" runat="server" ErrorMessage="Rate Required"
                                            ControlToValidate="txtRate" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvQty" runat="server" ErrorMessage="Quantity must be greater than Zero."
                                            ControlToValidate="txtQty" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCRate" runat="server" ErrorMessage="Rate Must be greater than Zero."
                                            ControlToValidate="txtRate" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvDesc" runat="server" ErrorMessage="Quantity must be greater than Zero."
                                            ControlToValidate="txtDescription" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Quantity must be greater than Zero."
                                            ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblDescriptionStar1" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblDesc" class="clsLabel">Description</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox1_Ajax" BackColor="White"
                                    MaxLength="2000" Text="<%# mWorkInvoice.WorkInvoiceTools.CurrentItem.ToolDescription %>"
                                    Height="39px" Width="382px" ToolTip="Enter Description" TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label1" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblQuantity" class="clsLabel">Quantity</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxRightAlign1_Ajax" MaxLength="4"
                                    Text="<%# mWorkInvoice.WorkInvoiceTools.CurrentItem.Qty %>" ToolTip="Enter Quantity">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblStarRate" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblRate" class="clsLabel">Rate</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxRightAlign1_Ajax" MaxLength="12"
                                    Text="<%# mWorkInvoice.WorkInvoiceTools.CurrentItem.CRate %>" ToolTip="Enter Rate">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblRemark" class="clsLabel">Remark</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxLong_Ajax" Height="39px"
                                    MaxLength="250" Text="<%# mWorkInvoice.WorkInvoiceTools.CurrentItem.Remark %>"
                                    ToolTip="Enter Remark" TextMode="MultiLine" Width="382px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblNote" class="clsLabel">Note</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxLong_Ajax" MaxLength="250"
                                    Text="<%# mWorkInvoice.WorkInvoiceTools.CurrentItem.Note %>" Height="39px" Width="382px"
                                    ToolTip="Enter Note" TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to Add Work Invoice Tool" />
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Back" ToolTip="Click to go back to the previous page" />
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
    </form>
</body>
</html>
