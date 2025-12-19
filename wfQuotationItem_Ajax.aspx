<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfQuotationItem_Ajax.aspx.vb"
    Inherits="Flypal.wfQuotationItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Quotation Item Details</title>
    <script type="text/jscript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout">
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td class="clsFormHeader1">
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Quotation Item [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ErrorMessage="Part Required"
                                                ControlToValidate="txtPartNo" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ErrorMessage="Quantity Required"
                                                ControlToValidate="txtQty" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" ErrorMessage="Part Description Required."
                                                ControlToValidate="txtDescription" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvRate" runat="server" ErrorMessage="Rate Required"
                                                ControlToValidate="txtRate" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvOtherCharge" runat="server" ErrorMessage="Other Charge Must be greater than Zero."
                                                ControlToValidate="txtOtherCharges" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvQty" runat="server" ErrorMessage="Quantity must be greater than Zero."
                                                ControlToValidate="txtQty" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCRate" runat="server" ErrorMessage="Rate Must be greater than Zero."
                                                ControlToValidate="txtRate" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="lblNote1" class="clsLabelAuto">Enter the Details of Items by selecting the
                        Part No. from list and mention the Qty and the Rate</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="Span8" class="clsLabelAuto">Select Quotation Item </span>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel runat="server" ID="Panel3" Style="width: auto;">
                            <asp:UpdatePanel runat="server" ID="upnlQuotationItem" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset9" style="padding: 0px 4px 0px 0px; width: auto;" class="clsFieldSetNewStyle">
                                        <%-- <legend><b>Select Quotation Item </b></legend>--%>
                                        <table>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="spnSrNo" class="clsLabel">Sr. No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSrNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                                        MaxLength="4" ReadOnly="True" Text="<%# mQuotation.QuotationItems.CurrentItem.SrNo %>"
                                                        ToolTip="Sr. No." ></asp:TextBox>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="spnPartNoStar" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="spnPartNo" class="clsLabel">Part No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                        ReadOnly='<%# Session("Edit") %>' Text="<%# mQuotation.QuotationItems.CurrentItem.ItemName %>"
                                                        ToolTip="Enter Part No.">
                                                    </asp:TextBox>
                                                    <asp:ImageButton ID="imgbtnPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                        Enabled='<%# Not Session("Edit") %>' CausesValidation="False"
                                                        Width="24px" ToolTip="Click to Select New Part No."></asp:ImageButton>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAlternatePart" runat="server" CssClass="clsbtnH clsinfoH1" Text="Alternate Part"
                                                        ToolTip="Click to add Alternate Part" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="spnDescription" class="clsLabel">Description</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxLong_Ajax"
                                                        MaxLength="50" ReadOnly="True" Text="<%# mQuotation.QuotationItems.CurrentItem.ItemDescription %>"
                                                        ToolTip="Part Description"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel runat="server" ID="Panel4" Style="width: auto;">
                            <asp:UpdatePanel runat="server" ID="upnlPartType" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset10" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                                        class="clsFieldSetNewStyle">
                                        <table>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="Span7" class="clsLabel">Part Type</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbPartTypeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                        DataTextField="Name" DataValueField="ID" SelectedValue="<%# mQuotation.QuotationItems.CurrentItem.ItemTypeID %>"
                                                         >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="lblAltPartNo" class="clsLabel">Alt. Part No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAltPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                        Text="<%# mQuotation.QuotationItems.CurrentItem.AltPartNo %>" ToolTip="Enter Alternate Part No.">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <span id="lblIPCReference" class="clsLabel">IPC Reference</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIPCReference" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                        Text="<%# mQuotation.QuotationItems.CurrentItem.IPCReference %>" ToolTip="Enter IPC Reference No.">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="lblValues" class="clsLabelAuto">Values </span>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%-- <asp:UpdatePanel runat="server" ID="upnlQuotationItemsRateInfo" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                        <fieldset id="Fieldset5" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                            class="clsFieldSetNewStyle">
                            <%--<legend><b>Receiving Information </b></legend>--%>
                            <table>
                                <tr>
                                    <td>
                                        <span id="spnQtyStar" class="clsLabelStar">*</span>
                                    </td>
                                    <td>
                                        <span id="lblQuantity" class="clsLabel">Qty.</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchMegaSmallRightAlign" MaxLength="9"
                                            Text="<%# mQuotation.QuotationItems.CurrentItem.Qty %>" ToolTip="Enter Quantity."
                                            Enabled="<%# (mQuotation.QuotationItems.CurrentItem.RequisitionItemQuotationItemsNew.Count = 0) %>"
                                             ></asp:TextBox>
                                        <asp:DropDownList ID="cmbUnitConverterList" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                            DataTextField="ConvertUnitName" DataValueField="ConvertUnitID" SelectedValue="<%# mQuotation.QuotationItems.CurrentItem.UnitID %>"
                                           Enabled="False">
                                        </asp:DropDownList>
                                        <%-- <asp:TextBox ID="txtUnit" runat="server" CssClass="clsTextBoxTagSearchSmall" ReadOnly="True"
                                        BackColor="#E0E0E0" Text="<%# mQuotation.QuotationItems.CurrentItem.UnitName %>">
                                    </asp:TextBox>--%>
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
                                        <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right" MaxLength="12"
                                            Text="<%# mQuotation.QuotationItems.CurrentItem.CRate %>" ToolTip="Enter Rate"
                                              ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCurrency" runat="server" CssClass="clsTextBoxTagSearchSmall" ReadOnly="True"
                                            BackColor="#E0E0E0" Text="<%# mQuotation.QuotationItems.CurrentItem.Currency %>" >
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <span id="lblOtherCharges" class="clsLabelAuto">Oth. Charges</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOtherCharges" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                            MaxLength="12" Text="<%# mQuotation.QuotationItems.CurrentItem.COtherCharges %>"
                                            ToolTip="Enter Other Charge"  ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <span id="lblAmount" class="clsLabelAuto">Amount</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                            MaxLength="12" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mQuotation.QuotationItems.CurrentItem.CAmount %>"
                                            ToolTip="Amount" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <span id="lblEffRate" class="clsLabelAuto">Effective Rate</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCEffRate" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                            MaxLength="12" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mQuotation.QuotationItems.CurrentItem.CEffRate %>"
                                            ToolTip="Effective Rate" ></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                        <%--</asp:Panel>--%>
                    </td>
                    <td valign="top">
                        <%-- <asp:UpdatePanel runat="server" ID="upnlEconomicalInfo" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                        <fieldset id="Fieldset6" style="padding: 0px 4px 0px 0px; width: auto;" class="clsFieldSetNewStyle">
                            <table>
                                <tr>
                                    <td>
                                        <span id="lblEOQ" class="clsLabel">Economical Order Qty.</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEOQ" runat="server" CssClass="clsTextBoxTagSearchMegaSmallRightAlign" MaxLength="9"
                                            Text="<%# mQuotation.QuotationItems.CurrentItem.EOQ %>" ToolTip="Enter Economical Order Quantity"
                                             ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblEOQCRate" class="clsLabel">Economical Order Rate</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEOQCRate" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                            MaxLength="12" Text="<%# mQuotation.QuotationItems.CurrentItem.EOQCRate %>" ToolTip="Enter Economical Order Rate"
                                             ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblCBillBackRate" class="clsLabel">Bill Back Rate</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCBillBackRate" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                            MaxLength="12" Text="<%# mQuotation.QuotationItems.CurrentItem.CBillBackRate %>"
                                            ToolTip="Enter Bill Back Rate"  ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDeliveryInDays" class="clsLabel">Lead Time (Days)</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDeliveryInDays" runat="server" CssClass="clsTextBoxTagSearchMegaSmallRightAlign"
                                            MaxLength="4" Text="<%# mQuotation.QuotationItems.CurrentItem.DeliveryInDays %>"
                                            ToolTip="Enter Delivery In Days"  ></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                        <%--</asp:Panel>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel runat="server" ID="pnlApplicableTo" Style="width: auto;">
                            <asp:UpdatePanel runat="server" ID="upnlApplicableTo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: auto; z-index: 8000;"
                                        class="clsFieldSetNewStyle">
                                        <legend><b>Applicable To</b></legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblApplicable" class="clsLabel">Applicable To</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbApplicable" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mQuotation.QuotationItems.CurrentItem.ModelID %>"
                                                        DataTextField="ModelAndTypeName" DataValueField="ID"  >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblPriority" class="clsLabel">Priority</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mQuotation.QuotationItems.CurrentItem.PriorityID %>"
                                                        DataTextField="Name" DataValueField="ID"  >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span1" class="clsLabel">Payment Terms</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPaymentTerms" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="15"
                                                        Text="<%# mQuotation.QuotationItems.CurrentItem.PaymentTerm %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel runat="server" ID="pnlRemarkNote" Style="width: auto;">
                            <%--  <asp:UpdatePanel runat="server" ID="upnlRemarkNote" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                            <fieldset id="Fieldset12" style="padding: 0px 4px 0px 0px; width: auto;" class="clsFieldSetNewStyle">
                                <legend><b>Remark/Note</b></legend>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="spnRemark" class="clsLabel">Remark<asp:CustomValidator ID="cvRemark" runat="server"
                                                ControlToValidate="txtRemark" CssClass="clsLabelAuto" Display="None" ErrorMessage="Max. Length should be 100."
                                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            </span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"  MaxLength="250"
                                                Text="<%# mQuotation.QuotationItems.CurrentItem.Remark %>" TextMode="MultiLine"
                                                ToolTip="Enter Remark." ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Span6" class="clsLabelAuto">Note<asp:CustomValidator ID="cvNote" runat="server"
                                                ControlToValidate="txtNote" CssClass="clsLabelAuto" Display="None" ErrorMessage="Max. Length should be 150."
                                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            </span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"  MaxLength="250"
                                                Text="<%# mQuotation.QuotationItems.CurrentItem.Note %>" TextMode="MultiLine"
                                                ToolTip="Enter Note."  ></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <asp:Panel runat="server" ID="pnlEnquiryItemInformation" Style="width: auto;">
                            <%-- <asp:UpdatePanel runat="server" ID="upnlEnquiryItemInformation" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                            <fieldset id="Fieldset11" style="padding: 0px 4px 0px 0px; width: auto; z-index: 8000;"
                                class="clsFieldSetNewStyle">
                                <legend><b>Enquiry Item Information</b></legend>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lblEnquiryNo" class="clsLabel">Enquiry No.</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEnquiryNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="12"
                                                ReadOnly="True" BackColor="#E0E0E0" Text="<%# mQuotation.QuotationItems.CurrentItem.EnquiryNo %>"
                                                ToolTip="Enquiry No"  ></asp:TextBox>
                                        </td>
                                        <td>
                                            <span id="lblEnquirydate" class="clsLabel">Enquiry Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEnquiryDate" runat="server" CssClass="clsTextBoxTagSearch"  
                                                ReadOnly="true" ClientIDMode="Static" BackColor="#E0E0E0" MaxLength="8" Text="<%# mQuotation.QuotationItems.CurrentItem.EnquiryDateFormatted %>"
                                                ToolTip="Enquiry Date"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <%--   <asp:CustomValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate"
                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Expiry Date should be Later to Start Date."
                        OnServerValidate="customvalidate"></asp:CustomValidator>
                    <asp:CustomValidator ID="cvExpiryDate" runat="server" ControlToValidate="txtExpiryDate"
                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Expiry Date Should be Later to Start Date."
                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                    <asp:CustomValidator ID="cvcureqtrs" runat="server" ControlToValidate="txtCureQtrs"
                        CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                    <asp:CustomValidator ID="cvCureYrs" runat="server" ControlToValidate="txtCureYear"
                        CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                    <asp:CustomValidator ID="cvExpQtrs" runat="server" ControlToValidate="txtExpQrts"
                        CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                    <asp:CustomValidator ID="cvExpYrs" runat="server" ControlToValidate="txtExpYear"
                        CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <asp:UpdatePanel runat="server" ID="upnlRequisitionItems" UpdateMode="Conditional"
                            Visible="false">
                            <ContentTemplate>
                                <fieldset id="fsRequisitionItems" style="padding: 0px 4px 0px 0px; width: auto;"
                                    class="clsFieldSetNewStyle">
                                    <legend><b>List of selected Requisition Items</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="dgRequisitionItemList" runat="server" AutoGenerateColumns="False"
                                                     BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                                    AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
                                                    SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
                                                    PagerSettings-Mode="NextPreviousFirstLast">
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                            ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr. No."></asp:BoundField>
                                                        <asp:BoundField DataField="RequisitionDateFormatted" HeaderText="Date">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RequisitionNo" HeaderText="Number">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Qty.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReqQty" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                    ToolTip="Enter corresponding Period Value." Text='<%# DataBinder.Eval(Container.DataItem,"Qty") %>'>
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Approved By Logistic">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkLog" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApprovedByLog") %>'></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Approved By Eng.">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkEng" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApprovedByEng") %>'></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Approved By Mgt.">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkMgt" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApprovedByMgt") %>'></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField CommandName="ForDelete" HeaderText="Remove" Text="Remove" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                            <td valign="top">
                                                <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                    Height="20px" Text="Add" ToolTip="Click to Add Store Appoval Item" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- </asp:Panel>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="Table1" border="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add Quotation Item"
                                                Text="OK"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                Text="Back" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
        <%--Alternate Part List--%>
        <asp:Panel runat="server" ID="pnlAlternatePartList" CssClass="clspanel1">
            <div style="display: none">
                <asp:Button runat="server" ID="btnAlternatePartList" Text="Alternate Part List" />
                <asp:Button ID="btnBackAlternatePartList" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                    Text="Back"></asp:Button>
            </div>
            <div>
                <asp:UpdatePanel runat="server" ID="upnlAlternatePartList" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="clstablelistout" id="tblInner">
                            <tr>
                                <td colspan="2">
                                    <span id="lblAlternatePart" class="clstitle1">Alternate Part List</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblSelectedPart" class="clsFieldSetNewStyle">Selected Part</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblPartNo" class="clsLabel">Part No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAlternatePartNo" runat="server" BackColor="#E0E0E0" ReadOnly="True"
                                        CssClass="clsTextBoxTagSearch">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblDescription" class="clsLabel">Description</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAlternateDescription" runat="server" BackColor="#E0E0E0" ReadOnly="True"
                                        CssClass="clsTextBoxTagSearch">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsFieldSetNewStyle"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="dgAlternatePartList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                        AllowSorting="True">
                                        <PagerSettings Mode="NextPreviousFirstLast" />
                                        <RowStyle CssClass="clsdgItem" />
                                        <HeaderStyle CssClass="clsdgHeader" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:BoundField DataField="PartName" HeaderText="Part No">
                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PartDescription" HeaderText="Description">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AltTypeName" HeaderText="Part Type">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:ButtonField CommandName="SelectPart" HeaderText="Select" Text="Select">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                        </Columns>
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Alternate Part screen"
                                        CausesValidation="False"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <cc2:ModalPopupExtender runat="server" ID="mdeAlternatePartList" TargetControlID="btnAlternatePartList"
            PopupControlID="pnlAlternatePartList" BackgroundCssClass="clsModalPopupBGForSecondPage">
        </cc2:ModalPopupExtender>
        <%--End Of Alternate Part List--%>
    </form>
</body>
</html>
