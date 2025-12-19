<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOpeningBalance_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfOpeningBalance_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Opening Balance</title>
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
                                        <td class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Opening Stock [New]</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table1" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add Item in Receipt cum Invoice Item List"
                                                                                Text="OK"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTag" runat="server" Text="Print Acceptance Tag" CssClass="clsbtnH clsinfoH"
                                                                                ToolTip="Click to Print Acceptance Tag " CausesValidation="False" Visible='<%# iif(mItem.OpeningBalances.CurrentItem.IsNew=True  ,False,True) %>'></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                                Text="Back" CausesValidation="False"></asp:Button>
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
                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvReceiptDate" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="txtReceiptDate" ErrorMessage="Receipt date must be prior or equals to As On Date."
                                                Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <%--  <asp:RequiredFieldValidator ID="rfvRelaseNoteNo" runat="server" ControlToValidate="txtReleaseNoteNo"
                                            ErrorMessage="Enter the Release Note  No." Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>--%>
                                            <asp:CustomValidator ID="cvQty" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="txtQty" ErrorMessage="Quantity Must be greater than zero."
                                                Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQty"
                                                ErrorMessage="Quantity Required" Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvSelectDetails" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="cmbVendor" ErrorMessage="Vendor Required" Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvAircraft" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="cmbAircraft" ErrorMessage="Aircraft Required" Display="None"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvStore" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="cmbFromStore" ErrorMessage="Store Required" Display="None"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvWorkShop" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="cmbWorkShop" ErrorMessage="WorkShop Required" Display="None"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation"
                                                ErrorMessage="Bin Location Required." Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvIntrectNo" runat="server" ControlToValidate="txtInternalReceiptNo"
                                                Display="None" ErrorMessage="Max Length of Internal Receipt No should be 50."
                                                OnServerValidate="CustomValidate" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvStoreList" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="cmbStoreList" ErrorMessage="Select Store Name from the list."
                                                Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCurrency" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="cmbCurrencyList" CssClass="clsLabelAuto" ErrorMessage="Select Currency from the list."
                                                Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvFactor" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="txtConversionFactor" CssClass="clsLabelAuto" ErrorMessage="Currency factor must be greater than zero."
                                                Display="None"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFactor" runat="server" ControlToValidate="txtConversionFactor"
                                                ErrorMessage="Currency factor must be greater than zero." CssClass="clsLabelAuto"
                                                Display="None"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvReceiptdate" runat="server" ControlToValidate="txtReceiptDate"
                                                ErrorMessage="Receipt Date Required" Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvPartType" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="cmbPartType" ErrorMessage="Part Type Required" Display="None"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvPartType" runat="server" ControlToValidate="cmbPartType"
                                                ErrorMessage="Select Part Type From List." Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCaliDoneOnDate" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="txtOtherCharge" ErrorMessage="Expiry Date should be Later to Start Date."
                                                Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCodeNo" runat="server" ControlToValidate="txtCodeNo" Display="None"
                                                ErrorMessage="Code No. Required" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"
                                                ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCondCheck" runat="server" OnServerValidate="customvalidate"
                                                ControlToValidate="txtInvoiceNo" ErrorMessage="Part is Condition Check so Start Date required"
                                                Display="None" CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <%--  <tr>
                    <td>
                        <span id="spnPartInformation" class="clsLabelHeader"></span>
                    </td>
                    <td align="right">
                       <asp:Label ID="lblSerializedStatus" runat="server" CssClass="clsLabelAuto" Font-Bold="True"
                        Visible="False">Receiving Serialized Part</asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td valign="top" colspan="2">
                        <asp:Panel runat="server" ID="Panel3" Style="width: auto;">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset9" style="padding: 0px 4px 0px 0px; width: auto;" class="clsFieldSetNewStyle">
                                        <legend class="clsFieldSet1"><b>Receipt Info.</b></legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblStarReceiptDate" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblReceiptDate" class="clsLabel">Receipt Date</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtReceiptDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                                    Text="<%# mItem.OpeningBalances.CurrentItem.InvoiceDateFormatted %>" ReadOnly="true"
                                                                    Width="100px"></asp:TextBox>
                                                                <%-- <cc2:CalendarExtender ID="txtReceiptDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReceiptDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender ID="txtReceiptDateTextBoxWatermarkExtender" runat="server"
                                                    TargetControlID="txtReceiptDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblStarReceiptNo" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblReceiptNo" class="clsLabel">Receipt No.</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtReceiptText" runat="server" CssClass="clsTextBoxTagSearch" Enabled='<%# not Session("EditItem") %>'
                                                                    MaxLength="50" Text="<%# mItem.OpeningBalances.CurrentItem.InvoiceText %>" ToolTip="Enter Receipt text">
                                                                </asp:TextBox>
                                                                <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="clsTextBoxMedium_Ajax" Enabled='<%# Not Session("EditItem") %>'
                                                                    MaxLength="8" Text="<%# mItem.OpeningBalances.CurrentItem.InvoiceNo %>" ToolTip="Enter Receipt No.">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblStarQty" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblQty" class="clsLabel">Quantity</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Enabled="<%# Not mItem.SerialisedStatus %>"
                                                                    MaxLength="8" Text="<%# mItem.OpeningBalances.CurrentItem.DisplayQty %>" ToolTip="Enter Quantity">
                                                
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--<span id="lblStarRelnoteno" class="clsLabelStar">*</span>--%>
                                                            </td>
                                                            <td>
                                                                <span id="spnDescription" class="clsLabel">Release Note No.</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtReleaseNoteNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mItem.OpeningBalances.CurrentItem.ReleaseNoteNo %>"
                                                                    ToolTip="Enter Release Note No"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblStarStore" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblStore" class="clsLabel">Store</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="cmbStoreList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="LocationStore"
                                                                    DataValueField="ID" SelectedValue="<%# mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblStarLocation" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblLocation" class="clsLabel">Bin Location</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtLocation" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="12"
                                                                    ToolTip="Enter Location" Text="<%# mItem.OpeningBalances.CurrentItem.Location %>"
                                                                    DESIGNTIMEDRAGDROP="259">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="spnPartStatus" class="clsLabelAuto">Part Status</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbPartType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                    SelectedValue="<%# mItem.OpeningBalances.CurrentItem.ItemTypeID %>" DataValueField="ID"
                                                                    DataTextField="Name">
                                                                </asp:DropDownList>
                                                                <%--  <asp:Button ID="ImgbtnPartType" runat="server" CausesValidation="False" CssClass="clsButtonGrid_Ajax"
                                                                    Text="..." ToolTip="Click to Add New Part Type" />--%>
                                                               
                                                            </td>
                                                            <td colspn="3">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImgbtnPartTypeNew" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to Add Part Type" CausesValidation="False"></asp:ImageButton>
                                                                        </td>
                                                                        <td valign="middle">
                                                                            <asp:Label ID="lblColor" runat="server" CssClass="clsColorLabel" Style="margin-top: -1px"></asp:Label>
                                                                        </td>
                                                                        <td style="margin-top: 5px;">
                                                                            <asp:Label ID="lblPartStatus" runat="server" CssClass="clsLabelHeader" Width="75px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <span id="lblInvoiceNo" class="clsLabelAuto">Supp. Inv. No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                    Text="<%# mItem.OpeningBalances.CurrentItem.VendorInvoiceNo %>" ToolTip="Enter Vendor Invoice No.">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                                <td valign="top">
                                                    <asp:Panel runat="server" ID="Panel4" Style="width: auto;">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <%--  <fieldset id="Fieldset10" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                                                                    class="clsFieldSetNewStyle">--%>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblReceivedFrom" class="clsLabel">Source</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbReceivedFrom" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                SelectedValue="<%# mItem.OpeningBalances.CurrentItem.TypeID %>" DataValueField="ID"
                                                                                DataTextField="Type" AutoPostBack="True">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnSelectDetails" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                                                Text="Select Details" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbFromStore" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mItem.OpeningBalances.CurrentItem.FromStoreID %>"
                                                                                DataValueField="ID" DataTextField="LocationStore" Visible="False">
                                                                            </asp:DropDownList>
                                                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="RegNo"
                                                                                DataValueField="ID" DESIGNTIMEDRAGDROP="452" SelectedValue="<%# mItem.OpeningBalances.CurrentItem.MachineID %>"
                                                                                Visible="False">
                                                                            </asp:DropDownList>
                                                                            <asp:DropDownList ID="cmbVendor" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                                                DataValueField="ID" SelectedValue="<%# mItem.OpeningBalances.CurrentItem.VendorID %>"
                                                                                Visible="False">
                                                                            </asp:DropDownList>
                                                                            <asp:DropDownList ID="cmbWorkShop" runat="server" CssClass="clsTextBoxTagSearchCombo" DataTextField="LocationWorkShop"
                                                                                DataValueField="ID" DESIGNTIMEDRAGDROP="452" SelectedValue="<%# mitem.OpeningBalances.CurrentItem.WorkShopID %>"
                                                                                Visible="False">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblSerialNo" class="clsLabel">Serial No</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                Text="<%# mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.SerialNo %>"
                                                                                ToolTip="Enter Serial No.">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblReleaseNoteDate" class="clsLabel">Release Note Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtReleaseNoteDate" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                                                Text="<%# mItem.OpeningBalances.CurrentItem.ReleaseNoteDateFormatted %>" ClientIDMode="Static" autocomplete="off"
                                                                                Width="100px"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtReleaseNoteDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReleaseNoteDate"></cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender ID="txtReleaseNoteDateWatermarkExtender" runat="server"
                                                                                TargetControlID="txtReleaseNoteDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblBatchNo" runat="server" CssClass="clsLabel">Batch No.</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                ToolTip="Enter Batch No. for an Item" Text="<%# mItem.OpeningBalances.CurrentItem.BatchNo %>">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="Label7" class="clsLabel">Int. Rec. No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInternalReceiptNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                MaxLength="50" Text="<%# mItem.OpeningBalances.CurrentItem.Receipt.IntReceiptNo %>"
                                                                                ToolTip="Enter Internal Receipt No.">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblLoanRental" class="clsLabel">Loan/Rental</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkReturnable" runat="server" Checked="<%# mItem.OpeningBalances.CurrentItem.Returnable %>"
                                                                                CssClass="clsLabelAuto" ToolTip="Select Returnable" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblInvoiceDate" class="clsLabelAuto">Supp. Inv. Date </span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInvoiceDate" runat="server" AutoPostBack="true" Text="<%# mItem.OpeningBalances.CurrentItem.VendorInvoiceDateFormatted %>"
                                                                                ClientIDMode="Static" CssClass="clsTextBoxTagSearch" Width="100px" autocomplete="off"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInvoiceDate"></cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender ID="txtInvoiceDateTextBoxWatermarkExtender" runat="server"
                                                                                TargetControlID="txtInvoiceDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblCodeNo" runat="server" CssClass="clsLabel" Visible="false">Code No.</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="20"
                                                                                Visible="false" ToolTip="Code No." Text="<%# mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.CodeNo %>">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <%-- </fieldset>--%>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Panel>
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
                    <td valign="top" colspan="2">
                        <asp:UpdatePanel runat="server" ID="upnlRateValues" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="Fieldset2" style="padding: 0px 4px 0px 0px; width: auto;" class="clsFieldSetNewStyle">
                                    <legend class="clsFieldSet1"><b>Values</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStarCurrency" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblCurrency" class="clsLabel">Currency</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                AutoPostBack="True" SelectedValue="<%# mItem.OpeningBalances.CurrentItem.CurrencyId %>"
                                                                DataValueField="ID" DataTextField="Name">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <span id="lblStarFactor" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblConversionFactor" class="clsLabel">Conv. Factor</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtConversionFactor" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
                                                                MaxLength="9" ToolTip="currency conversion factor" Text="<%# mItem.OpeningBalances.CurrentItem.ConversionFactor %>">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblLandingRates" class="clsLabel">Rate</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLandingRates" runat="server" Width="150px" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                MaxLength="12" ToolTip="Enter Landing Rates" Text="<%# mItem.OpeningBalances.CurrentItem.CRate %>"
                                                                AutoPostBack="True">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblOtheCharge" runat="server" CssClass="clsLabelAuto" Visible="False">Oth. Charges</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOtherCharge" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                MaxLength="12" Text="<%# mItem.OpeningBalances.CurrentItem.COtherCharges %>"
                                                                ToolTip="Enter Other Charges" Visible="False" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblAmount" class="clsLabel">Amount</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtAmount" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                MaxLength="10" ReadOnly="True" Text="<%# mItem.OpeningBalances.CurrentItem.CAmount %>"
                                                                ToolTip="Amount" Width="150px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                            <td valign="top">
                                                <asp:UpdatePanel runat="server" ID="upnlEffectiveRate" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <%--   <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: auto;" class="clsFieldSetNewStyle">--%>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Label2" class="clsLabel">Landing Cost</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLandingCost" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Width="150px" MaxLength="12" ToolTip="Enter Landing Rates" Text="<%# mItem.OpeningBalances.CurrentItem.LandingCost %>"
                                                                        AutoPostBack="True">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="clsLabelAuto">Commercial Rate</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCommercialRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        MaxLength="12" Text="<%# mItem.OpeningBalances.CurrentItem.CCommercialRate %>"
                                                                        Width="150px" ToolTip="Enter Commercial Rate"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <%--   </fieldset>--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upnlTabDetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <cc2:TabContainer ID="tabReceiptDetailsContainer" runat="server" class="clstablelistin"
                                    Visible="true">


                                    <cc2:TabPanel ID="tabExpiryDetails" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Expiry(s)" ID="lblExpiry"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="Panel1" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="upnlExpiryInformation" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <%--  <fieldset id="Fieldset7" style="padding: 0px 4px 0px 0px; width: auto; z-index: 9000;"
                                                            class="clsFieldSetNewStyle">--%>

                                                        <table>
                                                            <tr>
                                                                <td valign="top" colspan="4">
                                                                    <td valign="top" colspan="2">
                                                                        <asp:CustomValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate"
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
                                                                            CssClass="clsLabel" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblExpPeriod" runat="server" CssClass="clsLabelAuto" Text="<%# mItem.OpeningBalances.CurrentItem.ExpiryPeriod %>">
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table id="Table4" border="0" cellpadding="0" cellspacing="0" runat="server" visible='<%# iif(mItem.OpeningBalances.CurrentItem.ExpiryMonth=0 or AppSettings("ClientCode") = "IND",True,False) %>'>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblOthers" class="clsLabel">Others</span>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkIsExpiryNA" runat="server" AutoPostBack="True" Checked="<%# mItem.OpeningBalances.CurrentItem.IsExpiryNA %>"
                                                                                    CssClass="clsCheckBox" Text="N/A" />
                                                                                <asp:CheckBox ID="chkIsExpiryUnlimited" runat="server" AutoPostBack="True" Checked="<%# mItem.OpeningBalances.CurrentItem.IsExpiryUnlimited %>"
                                                                                    CssClass="clsCheckBox" Text="Unlimited" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table id="Table2" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblStartDate" class="clsLabel">Cure Date</span>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px" autocomplete="off"
                                                                                    Text="<%# mItem.OpeningBalances.CurrentItem.StartDateFormatted %>" AutoPostBack="true"
                                                                                    ClientIDMode="Static"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtStartDate" ID="txtStartDateWatermarkExtender"
                                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lstExpiryDate" class="clsLabel">Expiry Date</span> &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px" autocomplete="off"
                                                                                    Text="<%# mItem.OpeningBalances.CurrentItem.ExpiryDateFormatted %>" AutoPostBack="true"
                                                                                    ClientIDMode="Static"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtExpiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExpiryDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtExpiryDate" ID="txtExpiryDateWatermarkExtender"
                                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table id="Table5" border="0" cellpadding="0" cellspacing="0" runat="server" visible="<%# ((mItem.OpeningBalances.CurrentItem.ExpiryMonth<>0 And mItem.OpeningBalances.CurrentItem.ExpiryQuarter<>0) Or (mItem.OpeningBalances.CurrentItem.ExpiryMonth=0 And mItem.OpeningBalances.CurrentItem.ExpiryQuarter=0) Or (mItem.IsExpiryItem))%>">
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Label3" class="clsLabel">Cure Quarter</span>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCureQtrs" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    MaxLength="1" ToolTip="Enter Quarter." Text="<%# mItem.OpeningBalances.CurrentItem.CureQtrs %>"
                                                                                    Enabled="<%# (mItem.OpeningBalances.CurrentItem.ExpiryQuarter>0) OR (mItem.OpeningBalances.CurrentItem.ExpiryMonth=0 and mItem.OpeningBalances.CurrentItem.ExpiryQuarter=0) %>"
                                                                                    AutoPostBack="True" Width="24px">
                                                                                </asp:TextBox>
                                                                                <asp:Label ID="Label5" runat="server">/</asp:Label>
                                                                                <asp:TextBox ID="txtCureYear" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    MaxLength="4" ToolTip="Enter Cure Year." Text="<%# mItem.OpeningBalances.CurrentItem.CureYear %>"
                                                                                    Enabled="<%# (mItem.OpeningBalances.CurrentItem.ExpiryQuarter>0) OR (mItem.OpeningBalances.CurrentItem.ExpiryMonth=0 and mItem.OpeningBalances.CurrentItem.ExpiryQuarter=0) %>"
                                                                                    AutoPostBack="True" Width="56px">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Label4" class="clsLabel">Expiry Quarter</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtExpQrts" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    MaxLength="1" ToolTip="Enter Expiry Quarter." Text="<%# mItem.OpeningBalances.CurrentItem.ExpQtrs %>"
                                                                                    AutoPostBack="True" Width="24px">
                                                                                </asp:TextBox>
                                                                                <asp:Label ID="Label6" runat="server">/</asp:Label>
                                                                                <asp:TextBox ID="txtExpYear" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    MaxLength="4" ToolTip="Enter Expiry Year." Text="<%# mItem.OpeningBalances.CurrentItem.ExpYear %>"
                                                                                    AutoPostBack="True" Width="56px">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <%-- </fieldset>--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>

                                    </cc2:TabPanel>


                                    <cc2:TabPanel ID="tabBenchCheck" runat="server" CssClass="clsPanel1">

                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Benchcheck/Calibration Info."
                                                ID="Label1"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="Panel5" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <%--  <fieldset id="Fieldset11" style="padding: 0px 4px 0px 0px; width: auto; z-index: 8000;"
                                                            class="clsFieldSetNewStyle">--%>

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span4" class="clsLabel">Calibration Start Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCalibrationDoneOnDate" runat="server" CssClass="clsTextBoxTagSearch"
                                                                        Text="<%# mItem.OpeningBalances.CurrentItem.CalibrationDoneOnDateFormatted %>"
                                                                        Width="100px" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtCalibrationDoneOnDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCalibrationDoneOnDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtCalibrationDoneOnDate" ID="txtCalibrationDoneOnDateWatermarkExtender"
                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <%--   </fieldset>--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>

                                    <cc2:TabPanel ID="tabRemark" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Remark/Note" ID="lblRemark"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="Panel6" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <%-- <fieldset id="Fieldset12" style="padding: 0px 4px 0px 0px; width: auto;" class="clsFieldSetNewStyle">
                                                            <legend class="clsFieldSet1"><b>Remark/Note</b></legend>--%>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="spnRemark" class="clsLabel">Remark<asp:CustomValidator ID="cvRemark" runat="server"
                                                                        ControlToValidate="txtRemark" CssClass="clsLabelAuto" Display="None" ErrorMessage="Max. Length should be 100."
                                                                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Height="36px" MaxLength="250"
                                                                        Text="<%# mItem.OpeningBalances.CurrentItem.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark."
                                                                        Width="350px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span6" class="clsLabelAuto">Note<asp:CustomValidator ID="cvNote" runat="server"
                                                                        ControlToValidate="txtNote" CssClass="clsLabelAuto" Display="None" ErrorMessage="Max. Length should be 150."
                                                                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Height="36px" MaxLength="250"
                                                                        Text="<%# mItem.OpeningBalances.CurrentItem.Note %>" TextMode="MultiLine" ToolTip="Enter Note."
                                                                        Width="350px"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                        <%-- </fieldset>--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                </cc2:TabContainer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td valign="top"></td>
                    <td valign="top">

                        <%--<asp:Panel runat="server" ID="Panel2" Style="width: auto;">
                        <asp:UpdatePanel runat="server" ID="upnlConditionCheck" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="Fieldset3" style="padding: 0px 4px 0px 0px; width: auto; z-index: 7000;"
                                    class="clsLabelHeader">
                                    <legend><b>Condition Check/Serviced Inspected Info.</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <span id="Span1" class="clsLabel">Start Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConditionCheckDoneOnDate" runat="server" CssClass="clsTextBoxTagSearch"
                                                    Text="<%# mItem.OpeningBalances.CurrentItem.ConditionCheckDoneOnDateFormatted %>"
                                                    Width="100px" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtConditionCheckDoneOnDate_CalendarExtender" runat="server"
                                                    CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtConditionCheckDoneOnDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtConditionCheckDoneOnDate" ID="txtConditionCheckDoneOnDateWatermarkExtender"
                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>--%>
                      
                    </td>
                </tr>              
            </table>
        </div>
        <%-- <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
        </asp:UpdateProgress>--%>
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IFileUpload" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto"></iframe>
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
                        $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                        $("#IFileUpload").ready(function () {
                            $("#btnDummyFileUpload").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        });

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
        <!-- End File Upload Modal Dialog-->
    </form>
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddSupplier = document.getElementById("cmbVendor");
            if (ddSupplier != null) {
                var i = 0;
                if (ddSupplier.disabled == false) {
              <% For Each item1 In mVendorList%>
                <% If item1.NotInUse = "True" Then%>
                    ddSupplier[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                    i = i + 1;
             <% Next%>
                }
            }
        });
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>
