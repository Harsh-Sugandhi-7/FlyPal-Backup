<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesOrderItem.aspx.vb"
    Inherits="Flypal.wfSalesOrderItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Sales Order Item Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta content="False" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
                        <table class="clsTablelistin" id="tblinner">
                            <tr>
                                <td nowrap colspan="5" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Sales Order Item [New]</asp:Label>
                                            </td>
                                            <td align="right" colspan="5">
                                                <table id="Table1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add Item in Sales Order."></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ErrorMessage="Part Required"
                                        ControlToValidate="txtPartNo" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ErrorMessage="Quantity Required"
                                        ControlToValidate="txtQty" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" ErrorMessage="Part can't be saved without Description."
                                        ControlToValidate="txtDescription" CssClass="clslabel" Display="None" Width="72px"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rvfRate" runat="server" ErrorMessage="Rate Required"
                                        ControlToValidate="txtRate" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <%--  <tr>
                            <td colspan="5">
                                <asp:Label ID="lblNote1" runat="server" CssClass="clsLabelAuto" Width="520px">Enter the Details of Items by selecting the Part No. from list and mention the Qty and the Rate</asp:Label>
                            </td>
                        </tr>--%>

                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel runat="server" ID="upnlPart" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsPartDet" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                <legend id="ledOrderTotal"><b>Part Details</b>
                                                </legend>
                                                <table>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblSrNo" runat="server" CssClass="clsLabel">Sr. No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ReadOnly="True"
                                                                Height="25px" BackColor="#E0E0E0" MaxLength="5" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.SrNo %>">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td align="right" colspan="2"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblPartNo1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <table cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly='<%# Session("Edit") %>'
                                                                            Height="25px" MaxLength="50" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.ItemName %>"
                                                                            ToolTip="Enter Part No.">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <%--                                            <asp:Button ID="imgbtnPartNo" runat="server" CssClass="clsButtonGrid" Text="..."
                     ToolTip="Click to Add New Part No." CausesValidation="False" Enabled='<%# Not Session("Edit") %>'></asp:Button>--%>

                                                                        <asp:ImageButton ID="imgbtnPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                            ToolTip="Click to Add New Part No." CausesValidation="False" Enabled='<%# Not Session("Edit") %>'></asp:ImageButton>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblDescription1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDesc" runat="server" CssClass="clsLabel">Description</asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ReadOnly="True"
                                                                BackColor="#E0E0E0" MaxLength="100" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.ItemDescription %>"
                                                                Width="300px" TextMode="MultiLine">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Label ID="lblQuoItemInformation" runat="server" CssClass="clsLabelHeader">Quotation Item Information</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblQuoNo" runat="server" CssClass="clsLabel">Quotation No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtQuotationNo" runat="server" CssClass="clsTextBox" ReadOnly="True"
                                                                BackColor="#E0E0E0" MaxLength="8" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.QuotationNo %>">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblQuodate" runat="server" CssClass="clsLabelAuto">Quotation Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtQuotaionDate" runat="server" CssClass="clsTextBoxDate" ReadOnly="True"
                                                                BackColor="#E0E0E0" MaxLength="8" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.QuotationDateFormatted %>">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>


                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>

                            </tr>



                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel runat="server" ID="upnlValues" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset1" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                <legend id="ledValues"><b>Values</b>
                                                </legend>
                                                <table>

                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblQuantity1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblQuantity" runat="server" CssClass="clsLabel">Quantity</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="4"
                                                                Height="25px" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.Qty %>" ToolTip="Enter Quantity">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtUnit" runat="server" ReadOnly="True" BackColor="#E0E0E0" Height="25px" Width="100px"
                                                                CssClass="clsTextBoxTagSearch" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.UnitName %>">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblRate1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRate" runat="server" CssClass="clsLabel">Rate</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right; height: 25px" MaxLength="12"
                                                                Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.CRate %>" ToolTip="Enter Rate">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtCurrency" runat="server" ReadOnly="True" Height="25px" Width="100px"
                                                                CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.Currency %>">
                                                            </asp:TextBox>
                                                            <asp:CustomValidator ID="cvQty" runat="server" ErrorMessage="Quantity must be greater than Zero."
                                                                ControlToValidate="txtQty" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="cvCRate" runat="server" ErrorMessage="Rate Must be greater than Zero."
                                                                ControlToValidate="txtRate" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblOtherCharges" runat="server" CssClass="clsLabel">Oth. Charges</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOtherCharges" runat="server" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right; height: 25px"
                                                                MaxLength="12" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.COtherCharges %>"
                                                                ToolTip="Enter Other Charges">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:CustomValidator ID="cvOtherCharge" runat="server" ErrorMessage="Other Charge must be Equal or Greater than Zero."
                                                                ControlToValidate="txtOtherCharges" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblAmount" runat="server" CssClass="clsLabel">Amount</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right; height: 25px" ReadOnly="True"
                                                                BackColor="#E0E0E0" MaxLength="12" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.CAmount %>">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td colspan="2"></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblEffRate" runat="server" CssClass="clsLabel">Effective Rate</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCEffRate" runat="server" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right; height: 25px" ReadOnly="True"
                                                                BackColor="#E0E0E0" MaxLength="12" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.CEffRate %>">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td colspan="2"></td>
                                                    </tr>
                                                </table>


                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel runat="server" ID="upnlRemarkNote" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset2" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                <legend id="ledRemarkNote"><b>Remark/Note</b>
                                                </legend>
                                                <table>

                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblApplicable" runat="server" CssClass="clsLabel">Applicable To</asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:DropDownList ID="cmbApplicable" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataValueField="ID"
                                                                DataTextField="ModelAndTypeName" SelectedValue="<%# mSalesOrder.SalesOrderItems.CurrentItem.ModelID %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:CustomValidator ID="cvModelName" runat="server" ErrorMessage="Select Applicable Model From the List."
                                                                ControlToValidate="cmbApplicable" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabel">Remark</asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtRemark" runat="server" MaxLength="250"
                                                                Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.Remark %>" ToolTip="Enter Remark"
                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="300px" TextMode="MultiLine">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Max. Length should be 100."
                                                                ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblNote" runat="server" CssClass="clsLabel">Note</asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtNote" runat="server" MaxLength="250" Text="<%# mSalesOrder.SalesOrderItems.CurrentItem.Note %>"
                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="300px" ToolTip="Enter Note" TextMode="MultiLine">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CustomValidator ID="cvNote" runat="server" ErrorMessage="Max. Length should be 100."
                                                                ControlToValidate="txtNote" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>


                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <%--<tr>
                            <td align="right" colspan="5">
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add Item in Sales Order.">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
