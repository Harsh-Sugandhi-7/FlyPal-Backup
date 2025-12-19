<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionItem_Ajax.aspx.vb"
    Inherits="Flypal.wfRequisitionItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Requisition Item Details</title>
    <%--  <link id="MainStyle" type="text/css" rel="stylesheet" />--%>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
        <table id="tblMain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                        <table id="tblinner" class="clsTablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Requisition Item [New]</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" Display="None" CssClass="clsLabelAuto"
                                                ValidationGroup="a" ControlToValidate="txtPartNo" ErrorMessage="Part No. Required"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" Display="None" CssClass="clsLabelAuto"
                                                ValidationGroup="a" ControlToValidate="txtQty" ErrorMessage="Quantity Required"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" Display="None" CssClass="clsLabelAuto"
                                                ValidationGroup="a" ControlToValidate="txtDescription" ErrorMessage="Part Description Required"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvQty" runat="server" Display="None" ControlToValidate="txtQty"
                                                ValidationGroup="a" ErrorMessage="Quantity must be greater than Zero." OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvMachine" runat="server" Display="None" ControlToValidate="cmbMachine"
                                                ValidationGroup="a" ErrorMessage="Aircraft Required" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvWorkShop" runat="server" Display="None" ControlToValidate="cmbWorkShop"
                                                ValidationGroup="a" ErrorMessage="WorkShop Required" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvQty1" runat="server" Display="None" ValidationGroup="a"
                                                ValidateEmptyText="true" ControlToValidate="txtMinStockLevel" ErrorMessage="Either mark Requisition Item as One Time Purchase or enter either of the Min. Stock Level,Max. Stock Level,Min. Re-Order Level Quantities."
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvMax" runat="server" Display="None" ValidationGroup="a"
                                                ValidateEmptyText="true" ControlToValidate="txtMaxStockLevel" ErrorMessage="Max Stock Level quantity should be greater than Min Stock Level quantity."
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblOrderInfo" class="clsLabelHeader">Requisition Item Information</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlReqItemDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblSrNo" class="clsLabelAuto">Sr. No.</span>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.SrNo %>"
                                                            MaxLength="5" BackColor="#E0E0E0" ReadOnly="True" Width="60px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblPartNo1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo %>"
                                                                        Enabled="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty) %>"
                                                                        ToolTip="Enter Part No." ReadOnly="<%# (mRequisitionNew.TransTypeID=65 and mRequisitionNew.ReqTypeID=1) Or (mRequisitionNew.TransTypeID=71) or (mRequisitionNew.TransTypeID=72 and mRequisitionNew.ReqTypeID=1)    %>">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlAddReqItem" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <%-- <asp:Button ID="imgbtnPartNo" runat="server" CssClass="clsbtnH clsinfoH"  Height="30px" Text="..."
                                                                            Enabled="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew %>" ToolTip="Click to Add New Part No."
                                                                            CausesValidation="False" ClientIDMode="Static"></asp:Button>--%>
                                                                            <asp:ImageButton ID="imgbtnPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                Width="24px" Enabled="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew %>"
                                                                                ToolTip="Click to Add New Part No." CausesValidation="False" ClientIDMode="Static"></asp:ImageButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblDescription1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDesc" class="clsLabelAuto">Description</span>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Description %>"
                                                            Enabled="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty) %>"
                                                            ReadOnly="<%# (mRequisitionNew.TransTypeID=65 and mRequisitionNew.ReqTypeID=1) Or (mRequisitionNew.TransTypeID=71)  or (mRequisitionNew.TransTypeID=72 and mRequisitionNew.ReqTypeID=1)    %>"
                                                            ToolTip="Enter Description" TextMode="MultiLine">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblQuantity1" runat="server" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblQuantity" class="clsLabelAuto">Requested Quantity</span>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty %>" MaxLength="8"
                                                            ToolTip="Enter Requested Quantity" Width="100px">
                                                        </asp:TextBox>
                                                        <asp:DropDownList ID="cmbUnitConverterList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                            DataTextField="ConvertUnitName" DataValueField="ConvertUnitID" Width="140px">
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="chkOneTimePurchase" runat="server" Text="One Time Purchase" CssClass="clsCheckBox"
                                                            Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>" ClientIDMode="Static" onchange="ControlEnabilityForQuantity();"
                                                            Checked="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase %>" />
                                                        <asp:CheckBox ID="chkExchangePurchase" runat="server" Text="Exchange/Repair/Overhaul Purchase"
                                                            CssClass="clsCheckBox" AutoPostBack="true" ClientIDMode="Static" Checked="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IsExchangePurchase %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td colspan="2" style="padding-left: 7px;">
                                                        <asp:Label CssClass="clsLabelAuto" ID="lblMaxStock" runat="server" Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>">Max. Stock Level</asp:Label>
                                                        <asp:TextBox ID="txtMaxStockLevel" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            AutoPostBack="true" Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>" Enabled="<%# not mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase %>"
                                                            onchange="ControlEnabilityForOneTimePurchase(this);" ClientIDMode="Static" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel %>"
                                                            MaxLength="8" ToolTip="Enter Max. Stock Level" Width="60px">
                                                        </asp:TextBox>
                                                        <asp:Label CssClass="clsLabelAuto" ID="lblMinStock" runat="server" Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>">Min. Stock Level</asp:Label>
                                                        <asp:TextBox ID="txtMinStockLevel" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            AutoPostBack="true" Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>" Enabled="<%# not mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase %>"
                                                            onchange="ControlEnabilityForOneTimePurchase(this);" ClientIDMode="Static" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel %>"
                                                            MaxLength="8" ToolTip="Enter Min. Stock Level" Width="60px">
                                                        </asp:TextBox>
                                                        <asp:Label ID="lblReOrd" runat="server" CssClass="clsLabelAuto" Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>">Re-Order Level</asp:Label>
                                                        <asp:TextBox ID="txtMinReOrderLevel" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>" Enabled='<%# IIf((mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase Or AppSettings("ClientCode") = "BA"), False, True) %>'
                                                            onchange="ControlEnabilityForOneTimePurchase(this);" ClientIDMode="Static" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel %>"
                                                            MaxLength="8" ToolTip="Enter Min. Re-Order Level" Width="60px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" Visible='<%# IIf((mRequisitionNew.TransTypeID = 65 And
                                                                                                                                    (AppSettings("ClientCode") = "APFT" Or
                                                                                                                                     AppSettings("ClientCode") = "Heligo" Or
                                                                                                                                     AppSettings("ClientCode") = "KAS" Or
                                                                                                                                     AppSettings("ClientCode") = "CE" Or
                                                                                                                                     AppSettings("ClientCode") = "AAP")) Or
                                                                                                                                    (mRequisitionNew.TransTypeID = 77 And
                                                                                                                                    (AppSettings("ClientCode") = "KAS" Or
                                                                                                                                        AppSettings("ClientCode") = "CE")), True, False) %>'>*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto" Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>">Aircraft</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 2px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbMachine" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>"
                                                                        onChange="ControlEnability()" SelectedValue="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID %>"
                                                                        DataTextField="RegNo" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCostCenter" runat="server" CssClass="clsLabelAuto" Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>">Cost Center</asp:Label>
                                                                </td>
                                                                <td></td>
                                                                <td style="padding-left: 4px;">
                                                                    <asp:TextBox ID="txtCostCenter" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo %>"
                                                                        ClientIDMode="Static" Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>"
                                                                        MaxLength="25"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblWorkShopStar" runat="server" CssClass="clsLabelStar" Visible="<%# mRequisitionNew.TransTypeID=72  %>">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWorkShop" runat="server" CssClass="clsLabelAuto" Visible="<%# mRequisitionNew.TransTypeID=72 %>">WorkShop</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:DropDownList ID="cmbWorkShop" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                            DataTextField="LocationWorkShop" DataValueField="ID" SelectedValue="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.WorkShopID %>"
                                                            Enabled="false" Visible="<%# mRequisitionNew.TransTypeID=72 %>">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblWONo" runat="server" CssClass="clsLabelAuto" Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>">WO No.</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtWONo" runat="server" CssClass="clsTextBoxTagSearch" Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>"
                                                                        Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo %>" MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSelectWONo" runat="server" CssClass="clsbtnH clsinfoH1" Text="Select Work Order"
                                                                        Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>"
                                                                        CausesValidation="False" ClientIDMode="Static"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblOrder" runat="server" CssClass="clsLabelAuto" Visible='<%# IIf((mRequisitionNew.TransTypeID = 72 And AppSettings("ClientCode") = "BA"), True, False) %>'>Order</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:DropDownList ID="cmbOrder" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="OrderNo"
                                                            Visible='<%# IIf((mRequisitionNew.TransTypeID = 72 And AppSettings("ClientCode") = "BA"), True, False) %>'
                                                            SelectedValue="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.OrderID %>"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblNRCNo" runat="server" CssClass="clsLabelAuto">NRC No.</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 2px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtNRCNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.NRCNo %>"
                                                                        MaxLength="50" ToolTip="Enter NRC No.">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblIPCReference" class="clsLabelAuto">IPC Reference</span>
                                                                </td>
                                                                <td></td>
                                                                <td style="padding-left: 4px;">
                                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference %>"
                                                                        MaxLength="100" ToolTip="Enter IPC Reference">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <%--<td>
                                                    <span id="lblIPCReference" class="clsLabelAuto">IPC Reference</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td style="padding-left: 4px;">
                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference %>"
                                                        MaxLength="100" ToolTip="Enter IPC Reference">
                                                    </asp:TextBox>
                                                </td>--%>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span class="clsLabelAuto" id="lblMainType">Main.Type</span>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: -1px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbRequisitionItemTypeList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                            DataTextField="Name" DataValueField="ID" SelectedValue="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.RequisitionItemTypeID %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Span1" runat="server" CssClass="clsLabelAuto" Visible="<%#(mRequisitionNew.TransTypeID = 77)%>">Manual Reference</asp:Label>
                                                                </td>
                                                                <td>&nbsp;
                                                                <asp:TextBox ID="txtManualRef" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ManualReference %>"
                                                                    Visible="<%#(mRequisitionNew.TransTypeID = 77)%>" MaxLength="200" ToolTip="Enter Manual Reference">
                                                                </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblJobDescription" class="clsLabelAuto">Reason For Request</span>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtReasonForRequest" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                            Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForRequest %>"
                                                            MaxLength="1000" ToolTip="Enter Reason For Request" TextMode="MultiLine">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="Label2" CssClass="clsLabelAuto" runat="server" Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>">Reason For Purchase</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:TextBox ID="txtReasonForPurchase" runat="server" CssClass="clsTextBoxTagSearch"
                                                            Visible="<%#(mRequisitionNew.TransTypeID <> 77)%>" MaxLength="1000" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForPurchase %>"
                                                            TextMode="MultiLine" ToolTip="Enter Reason For Purchase" Width="700Px" Height="20px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblPriority" class="clsLabelAuto">Priority</span>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlPriority" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table7">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbPriority" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.PriorityID %>">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDays" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
                                                                                Enabled="false" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Days %>"
                                                                                ToolTip="Enter No. Of Days"></asp:TextBox>
                                                                            <asp:Label ID="lblInDays" runat="server" CssClass="clsLabel">In Days</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Remark %>"
                                                            MaxLength="500" ToolTip="Enter Remark" TextMode="MultiLine">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblNote" class="clsLabelAuto">Note</span>
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 4px;">
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" MaxLength="500"
                                                            Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Note %>" TextMode="MultiLine"
                                                            ToolTip="Enter Note">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <placeholder id="plTSO" runat="server" visible="false">
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Span2" class="clsLabelAuto">TSN/TSO</span>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td style="padding-left: 4px;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtTSN" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.TSNValueFormatted %>"
                                                                            ToolTip="Enter TSN/TSO" />
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span3" class="clsLabelAuto">CSN/CSO</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCSN" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.CSNValueFormatted %>"
                                                                            ToolTip="Enter CSN/CSO" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </placeholder>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Ok" ToolTip="Click to add Item in Requisition Item List"
                                                            ValidationGroup="a"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back"
                                                            ToolTip="Click to go back to the previous page" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnWOList" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnbtnSelectWONo" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnimgBtnPartNo" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnimgBtnRequisitionItemSearch" ClientIDMode="Static" runat="server"
                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
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
        <!-- WO List Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyWOList" Text="Dummy WO List" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupWOList" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupWOList" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupWOList" runat="server" TargetControlID="btnDummyWOList"
            PopupControlID="pnlPopupWOList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameWOListStateComplete() {
                $("#btnDummyWOList").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            $(document).ready(function () {
                $("#btnSelectWONo").live("click", function () {
                    try {
                        $("#hdnbtnSelectWONo").click();
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupWOList").attr("src", "wfSelectListForNewRequisition_Ajax.aspx?Type=pup");
                        if (!$.browser.msie) {
                            $("#btnDummyWOList").click();
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
            function ParentCallBackFunctionForWOList() {
                var WOListWindow = $find("<%=mdlPopupWOList.ClientID %>");
                //close WO List popup window
                WOListWindow.hide();
                $("#iPopupWOList").attr("src", "JavaScript:''");
                //call WO List image button
                $("#hdnimgBtnWOList").click();
            }
        </script>
        <script type="text/javascript">
            function ControlEnability() {
                var AircraftIndex = $get("cmbMachine").selectedIndex;
                var Aircraft = $get("cmbMachine").options[AircraftIndex].text;
                if (AircraftIndex > 0) {
                    $('#txtCostCenter').val(Aircraft);
                    $('#txtCostCenter').attr('disabled', true);
                }
                else {
                    $('#txtCostCenter').val('');
                    $('#txtCostCenter').removeAttr('disabled');
                }
            }

            function ControlEnabilityForQuantity() {
                var IsOneTimePurchase = $("#chkOneTimePurchase").attr("checked");
                if (IsOneTimePurchase) {
                    $("#txtMaxStockLevel").attr('disabled', true);
                    $("#txtMinReOrderLevel").attr('disabled', true);
                    $("#txtMinStockLevel").attr('disabled', true);
                    $("#txtMaxStockLevel").val('');
                    $("#txtMinReOrderLevel").val('');
                    $("#txtMinStockLevel").val('');
                }
                else {
                    $('#txtMaxStockLevel').removeAttr('disabled');
                    $('#txtMinReOrderLevel').removeAttr('disabled');
                    $('#txtMinStockLevel').removeAttr('disabled');
                }
            }

            function ControlEnabilityForOneTimePurchase(element) {
                var textbox1value = $("#txtMaxStockLevel").val();
                var textbox2value = $("#txtMinReOrderLevel").val();
                var textbox3value = $("#txtMinStockLevel").val();
                if ((textbox1value == "" || textbox1value == "0") && (textbox2value == "" || textbox2value == "0") && (textbox3value == "" || textbox3value == "0")) {
                    $('#chkOneTimePurchase').removeAttr('disabled');
                }
                else {
                    $("#chkOneTimePurchase").attr('disabled', true);
                }

            }
        </script>
        <!-- End-->
        <%--<!-- Requisition Item Search Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRequisitionItemSearch" Text="Dummy Requisition Item Search"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupRequisitionItemSearch" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="iPopupRequisitionItemSearch" frameborder="0" allowtransparency="true"
            height="100%" width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRequisitionItemSearch" runat="server" TargetControlID="btnDummyRequisitionItemSearch"
        PopupControlID="pnlPopupRequisitionItemSearch" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRequisitionItemSearchStateComplete() {
            $("#btnDummyRequisitionItemSearch").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#imgbtnPartNo").live("click", function () {
                try {
                    $("#hdnimgBtnPartNo").click();
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupRequisitionItemSearch").attr("src", "wfRequisitionItemSearch_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyRequisitionItemSearch").click();
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
            var RequisitionItemSearchWindow = $find("<%=mdlPopupRequisitionItemSearch.ClientID %>");
            //close Requisition Item Search popup window
            RequisitionItemSearchWindow.hide();
            $("#iPopupRequisitionItemSearch").attr("src", "JavaScript:''");
            //call Requisition Item Search image button
            $("#hdnimgBtnRequisitionItemSearch").click();
        }
    </script>
    <!-- End-->--%>
    </form>
</body>
</html>
