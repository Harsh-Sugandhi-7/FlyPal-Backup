<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueItem_Ajax.aspx.vb"
    Inherits="Flypal.wfIssueItem_Ajax" %>

<%@ Import Namespace="FlyPal" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Issue Item Details</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <script type="text/javascript">
        window.onload = blinknow;
        function blinknow() {
            var e = document.getElementById("<%=ImgID.ClientID%>");
            if (e != null) {
                e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
                setTimeout("blinknow();", 750);
            }
        }
    </script>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                    <table id="tblinner" class="clsTablelistin" border="0">
                        <tr>
                            <td class="clsFormHeader1">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Issue Item [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlvalidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ErrorMessage="Quantity must be greater than zero." ControlToValidate="txtQty"
                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ErrorMessage="Select Part from Pending Part list." ControlToValidate="txtPartNo"
                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ErrorMessage="Part can't be saved without Description." ControlToValidate="txtDescription"
                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvQty" runat="server" Display="None" ErrorMessage="Quantity must be greater than zero."
                                            CssClass="clsLabelAuto" ControlToValidate="txtQty" OnServerValidate="customvalidate"
                                            ValidationGroup="1"></asp:CustomValidator>
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlEnqItemDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="6">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblIssueItem" class="clsLabelHeader">Issue Item</span>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="ImgID" runat="server" ImageUrl="~/images/Attention.ico" Visible="<%# mIssue.IssueItems.CurrentItem.ItemTagID > 0 %>" />
                                                                <asp:Label ID="lblImageTagName" runat="server" CssClass="clsLabel" Text='<%# " ATTENTION! " + mIssue.IssueItems.CurrentItem.ItemTagName + " OBSERVE PRECAUTIONS FOR HANDLING." %>'
                                                                    Visible="<%# mIssue.IssueItems.CurrentItem.ItemTagID > 0 %>" ForeColor="Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblPartNoStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblPartNo" class="clsLabel">Part No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table1" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    ReadOnly='<%# Session("Edit") %>' Text="<%# mIssue.IssueItems.CurrentItem.ItemName %>"
                                                                    ToolTip="Enter Part No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="3">
                                                    <asp:UpdatePanel ID="upnlimgPartBtn" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table10" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
                                                                            <asp:ListItem Value="0">Part List</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnAddCombo" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                                            Text="Add " ToolTip="Click to Add the Part"/>
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                                                <td colspan="4">
                                                    <table id="Table2" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" 
                                                                    MaxLength="100" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ItemDesc %>"
                                                                    ToolTip="Description">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblPartType" class="clsLabel">Part Type</span>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table11" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPartType" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ItemTypeName %>"
                                                                    ToolTip="Part Type">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table111" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsReturnableFromAircraft" runat="server" Checked="<%# mIssue.IssueItems.CurrentItem.IsReturnableFromAircraft %>"
                                                                    CssClass="clsCheckBox" Enabled="<%# mIssue.StatusID=1 %>" Text="Is Unserviceable/Serviceable Part Expected Back"
                                                                    Visible="<%# mIssue.TransTypeID=14 or mIssue.TransTypeID=25 or mIssue.TransTypeID=44 %>" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblMaintenanceType" class="clsLabel">Maintenance Type</span>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table16" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbRequisitionItemTypeList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                    DataTextField="Name" DataValueField="ID" Enabled="<%# mIssue.StatusID=1 %>" SelectedValue="<%# mIssue.IssueItems.CurrentItem.RequisitionItemTypeID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table18" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsCapitalize" runat="server" Checked="<%# mIssue.IssueItems.CurrentItem.IsCapitalize %>"
                                                                    CssClass="clsCheckBox" Enabled="<%# mIssue.StatusID=1 %>" Text="Is Capitalize"
                                                                    Visible="<%# mIssue.TransTypeID=14 or mIssue.TransTypeID=44 %>" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <span id="lblIssueItemInfo" class="clsLabelHeader">Stock Item Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label7" class="clsLabel">Original Receipt No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table15" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtOriginalReceiptText" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.OriginalReceiptTextNo %>"
                                                                    ToolTip="Receipt  No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label6" class="clsLabel">Original Receipt Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOriginalReceiptDate" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchDate"
                                                        MaxLength="12" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.OriginalReceiptDateFormatted %>">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblReceiptNo" class="clsLabel">Receipt No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table3" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtReceiptText" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ReceiptTextNo %>"
                                                                    ToolTip="Receipt  No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReceiptNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                        MaxLength="8" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ReceiptNo %>"
                                                        ToolTip="Receipt No." Visible="False">

                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblReceiptDate" class="clsLabel">Receipt Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReceiptDate" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchDate"
                                                        MaxLength="12" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ReceiptDateFormatted %>">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblInvoiceNo" class="clsLabel">Supp. Invoice No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table14" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtInvoiceNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.VendorInvoiceNo %>"
                                                                    ToolTip="Receipt  No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblInvoiceDate" class="clsLabel">Supp. Invoice Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInvoiceDate" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                        MaxLength="12" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.VendorInvoiceDateFormatted %>">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label4" class="clsLabel">Int. Rec. No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table13" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtIntReceiptNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.IntReceiptNo %>"
                                                                    ToolTip="Internal Receipt  No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBatchNo" runat="server" CssClass="clsLabel">Batch No.</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBatchNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                        MaxLength="50" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.BatchNo %>"
                                                        ToolTip="Enter Batch No. for an Item">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRelNoteNo" class="clsLabel">Rel. Note No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table4" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRelNoteNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="200" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ReleaseNoteNo %>"
                                                                    ToolTip="Release Note No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRelNoteDate" class="clsLabel">R. Note Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRelNoteDate" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchDate"
                                                        MaxLength="12" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ReleaseNoteDateFormatted %>">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblReeiptItemView" CssClass="clsLabel">Receipt item Attachment</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                        Height="20px" Width="20px"></asp:ImageButton>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    <div class="dropdown">
                                                        <% If mIssue.IssueItems.CurrentItem.StatusKit Then%>
                                                        <span id="Span1" class="dropbtn">Kit Item List</span>
                                                        <div class="dropdown-content">
                                                            <table id="T1" class="clsGrid" width="100%" border="0">
                                                                <tr>
                                                                    <td class="clsdgHeader">
                                                                        Part No.
                                                                    </td>
                                                                    <td class="clsdgHeader">
                                                                        Description
                                                                    </td>
                                                                    <td class="clsdgHeader" align="right">
                                                                        Qty.
                                                                    </td>
                                                                    <td class="clsdgHeader">
                                                                        Serial No.
                                                                    </td>
                                                                    <td class="clsdgHeader">
                                                                        Remark
                                                                    </td>
                                                                </tr>
                                                                <% Dim Child3 As ReceiptItemKitItem%>
                                                                <% If Not mIssue.IssueItems.CurrentItem.ReceiptItemKitItems Is Nothing Then%>
                                                                <% For Each Child3 In mIssue.IssueItems.CurrentItem.ReceiptItemKitItems%>
                                                                <tr class="clsdgItem">
                                                                    <td>
                                                                        <%= Child3.ItemName%>
                                                                    </td>
                                                                    <td>
                                                                        <%= Child3.ItemDescription%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%= Child3.KitItemQty%>
                                                                    </td>
                                                                    <td>
                                                                        <%= Child3.SerialNoForItemIDOfKitItem%>
                                                                    </td>
                                                                    <td>
                                                                        <%= Child3.Remark%>
                                                                    </td>
                                                                </tr>
                                                                <% Next%>
                                                                <% End If%>
                                                            </table>
                                                        </div>
                                                        <% End If%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <span id="lblReceiptItemInfo" class="clsLabelHeader">Values</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblQuantityStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblQuantity" class="clsLabel">Quantity</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"  Width="183px" Enabled="<%# (((CType(mIssue.TransTypeID, FlyPal.Util.Trans) <> FlyPal.Util.Trans.IssueToAircraft) Or (mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.Count = 0)) AND mIssue.StatusID = 1 ) %>"
                                                        MaxLength="8" Text="<%# mIssue.IssueItems.CurrentItem.DisplayQty %>" ToolTip="Enter Quantity">

                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbUnitConverterList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        DataTextField="ConvertUnitName" DataValueField="ConvertUnitID" Enabled="False"
                                                        SelectedValue="<%# mIssue.IssueItems.CurrentItem.DisplayUnitID %>" Width="100px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblRate" runat="server" CssClass="clsLabel" Visible="<%# (((CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart))) %>">Rate </asp:Label>
                                                    <asp:Label ID="lblRateInBase" runat="server" CssClass="clsLabel" Text="<%# mIssue.IssueItems.CurrentItem.EffRate %>"
                                                        Visible="<%# (((CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart))) %>"></asp:Label>
                                                    <asp:Label ID="lblInBaseCurrency" runat="server" CssClass="clsLabel" Visible="<%# (((CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart))) %>"> In Base Currency</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDiscardAmt" runat="server" CssClass="clsLabel" Visible="<%# (((CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart))) %>">Discard Amount</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table17" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDiscardAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"  Width="183px"
                                                                    Enabled="false" MaxLength="12" Text="<%# mIssue.IssueItems.CurrentItem.DiscardAmt %>"
                                                                    ToolTip="Enter Discard Amount" Visible="<%# (((CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart))) %>">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBarcodeNo" runat="server" CssClass="clsLabel" Visible="<%$AppSettings:Barcode%>">Barcode No.</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBarcodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                        ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.BarcodeNo %>" Visible="False">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table5" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSerialNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.SerialNo %>"
                                                                    ToolTip="Serial No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lstExpiryDate" class="clsLabel">Expiry Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExpiryDate" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchDate"
                                                        MaxLength="12" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ExpiryDateFormatted %>">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblOutGoingReleaseNoteNoStar" CssClass="clsLabelStar"
                                                        Visible="<%$AppSettings:ReleaseNoteNoRequire%>">*</asp:Label>
                                                </td>
                                                <td>
                                                    <span id="Label1" class="clsLabel">Outgoing Rel. Note No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table12" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtOGReleaseNoteNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="<%# ((CType(mIssue.TransTypeID, FlyPal.Util.Trans) <> FlyPal.Util.Trans.IssueToAircraft) Or (mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.Count = 0)) and mIssue.StatusID = 1 %>"
                                                                    MaxLength="200" Text="<%# mIssue.IssueItems.CurrentItem.OutGoingReleaseNoteNo %>"
                                                                    ToolTip="Enter Outgoing Release Note No.">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label3" class="clsLabel">Expiry Qtrs.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExpQtrs" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                        MaxLength="12" ReadOnly="True" Text="<%# mIssue.IssueItems.CurrentItem.ExpiryQtrs %>">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <span id="lblRemarkInfo" class="clsLabelHeader">Remark / Note</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabel">Remark</span>
                                                </td>
                                                <td colspan="4">
                                                    <table id="Table7" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"   Enabled="<%# mIssue.StatusID = 1 %>"
                                                                    MaxLength="250" Text="<%# mIssue.IssueItems.CurrentItem.Remark %>" TextMode="MultiLine"
                                                                    ToolTip="Enter Remark">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblNote" class="clsLabel">Note</span>
                                                </td>
                                                <td colspan="4">
                                                    <table id="Table8" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Enabled="<%# mIssue.StatusID = 1 %>"
                                                                    MaxLength="250" Text="<%# mIssue.IssueItems.CurrentItem.Note %>" TextMode="MultiLine"
                                                                    ToolTip="Enter Note">

                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="right" colspan="4">
                                                    <table id="Table9" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                    Enabled="<%# CType(mIssue.TransTypeID,FlyPal.Util.Trans) = FlyPal.Util.Trans.IssueToAircraft And (Not mIssue.IssueItems.CurrentItem.ItemID.Equals(Guid.Empty)) %>"
                                                                    Text="Add" ToolTip="Click to Add Store Appoval Item" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="lblGridHeader" runat="server" CssClass="clsLabelHeader">List of Selected Requisition Items</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:GridView ID="dgRequisitionItemList" runat="server" AutoGenerateColumns="False"
                                                        CssClass="clsGrid" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No." />
                                                            <asp:BoundField DataField="RequisitionDateFormatted" HeaderText="Date">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequisitionNo" HeaderText="Number" />
                                                            <asp:TemplateField HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtReqQty" runat="server" CssClass="clsTextBoxRightAlignQty" MaxLength="8"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"Qty") %>'>
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:ButtonField CommandName="Remove" HeaderText="Remove" Text="Remove" />
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
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="tblButton" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="OK" ToolTip="Click to add Item in Issue Item List"
                                                        ValidationGroup="1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
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

       <!--ReceiptCumInvoiceAttach Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:modalpopupextender id="mdlAttach" runat="server" targetcontrolid="btnDummyAttach"
        popupcontrolid="pnlAttach" backgroundcssclass="clsModalPopupBG">
    </cc2:modalpopupextender>
    <script type="text/javascript">
        function IFrameAttachStateComplete() {
            $("#btnDummyAttach").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenAttachWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                //if (!$.browser.msie) {
                    $("#btnDummyAttach").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                //}
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForAttach() {
            var Attachwindow = $find("<%=mdlAttach.ClientID %>");
            //close popup window
            Attachwindow.hide();
            //release resources
            $("#IframeAttach").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnAttach").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
