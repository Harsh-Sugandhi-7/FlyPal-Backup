<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfInvoiceItem_Ajax.aspx.vb"
    Inherits="Flypal.wfInvoiceItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Purchase Invoice Item</title>
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
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" AsyncPostBackTimeout="600">
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
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Invoice Item [New]</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table1" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add Item in Invoice List">
                                                                        </asp:Button>
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
                                        <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" Display="None" ErrorMessage="Part No. Required"
                                            ControlToValidate="txtPartNo" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" Display="None" ErrorMessage="Quantity Required"
                                            ControlToValidate="txtQty" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" Display="None" ErrorMessage="Part can not be save without description."
                                            ControlToValidate="txtDescription" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvQty" runat="server" Display="None" ErrorMessage="Quantity must be greater than Zero."
                                            ControlToValidate="txtQty" OnServerValidate="customvalidate" CssClass="clsLabel"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCRate" runat="server" Display="None" ErrorMessage="Rate Must be greater than Zero."
                                            ControlToValidate="txtRate" OnServerValidate="customvalidate" CssClass="clsLabel"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNote" runat="server" ControlToValidate="txtNote" CssClass="clsLabelAuto"
                                            Display="None" ErrorMessage="Max. Length should be 150." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" CssClass="clsLabelAuto"
                                            Display="None" ErrorMessage="Max. Length should be 100." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <span id="spnPartInformation" class="clsLabelHeader">Invoice Item Status</span>
                </td>
                <td align="right">
                    <asp:UpdatePanel runat="server" ID="upnlInvoiceItemStatus" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" CssClass="clsLabelHeader">Invoicing Part : </asp:Label>
                            <asp:Label ID="lbliRecNo" runat="server" CssClass="clsLabelHeader"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <!--**********************************************************-->
            <tr>
                <td colspan="2">
                    <span id="lblDetail" class="clsLabelAuto">Enter the Details of Items Invoiced by selecting
                        the Part No. from list and mention the Qty and the Rate</span>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="2">
                    <asp:Panel runat="server" ID="Panel3" Style="width: auto;">
                        <asp:UpdatePanel runat="server" ID="upnlItemIformation" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="Fieldset9" style="padding: 0px 4px 0px 0px; width: auto;" class="clsLabelHeader">
                                    <%--<legend><b>Receiving Information </b></legend>--%>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="spnSrNo" class="clsLabel">Sr. No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSrNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                    MaxLength="4" ReadOnly="True" Text="<%# mInvoice.InvoiceItems.CurrentItem.SrNo %>"
                                                    ToolTip="Sr. No." Width="36px"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="spnPartNoStar" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="spnPartNo" class="clsLabel">Part No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                    ReadOnly='<%# Session("Edit") %>' Text="<%# mInvoice.InvoiceItems.CurrentItem.ItemName %>"
                                                    ToolTip='<%# iif(Session("Edit"),"Part No." , "Enter Part No.") %>'>
                                                </asp:TextBox>
                                                <%-- <asp:Button ID="imgbtnPartNo" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                    Enabled="False" Height="22px" Text="..." ToolTip="Click to Select New Part No." />--%>
                                                <asp:ImageButton ID="imgbtnPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                     Enabled="False" CausesValidation="False" Style="margin-top: 6px"
                                                    Width="24px" ToolTip="Click to Select New Part No."></asp:ImageButton>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="spnDescription" class="clsLabel">Description</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchLong"
                                                    ReadOnly="True" Text="<%# mInvoice.InvoiceItems.CurrentItem.ItemDescription %>"
                                                    ToolTip="Part Description"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <span id="lblPartType" class="clsLabel">Part Type</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPartType" runat="server" ReadOnly="True" BackColor="#E0E0E0"
                                                    Text="<%# mInvoice.InvoiceItems.CurrentItem.ItemTypeName %>" CssClass="clsTextBoxTagSearch">
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
                <td valign="top" colspan="2">
                    <asp:Panel runat="server" ID="Panel4" Style="width: auto;">
                        <asp:UpdatePanel runat="server" ID="upnlOrderIssueInformation" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="Fieldset10" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                                    class="clsLabelHeader">
                                    <legend><b>Invoice Item Information</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOrderIssueNo" runat="server" Text='<%# IIf(mInvoice.InvoiceItems.CurrentItem.IssueNumber <> "", "Issue No.", IIf(mInvoice.InvoiceItems.CurrentItem.OrderNumber <> "", "Order No.", "Ord./Iss.No.")) %>'
                                                    CssClass="clsLabel">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOrderIssueNo" runat="server" ReadOnly="True" BackColor="#E0E0E0"
                                                    Text='<%# IIf(mInvoice.InvoiceItems.CurrentItem.IssueNumber <> "", mInvoice.InvoiceItems.CurrentItem.IssueNumber, mInvoice.InvoiceItems.CurrentItem.OrderNumber) %>'
                                                    CssClass="clsTextBoxTagSearch">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOrderIssueDate" runat="server" Text='<%# IIf(mInvoice.InvoiceItems.CurrentItem.IssueNumber <> "", "Issue Date", IIf(mInvoice.InvoiceItems.CurrentItem.OrderNumber <> "", "Order Date", "Ord./Iss.Date")) %>'
                                                    CssClass="clsLabel">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOrderIssueDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                     BackColor="#E0E0E0" ReadOnly="true" ClientIDMode="Static" Enabled="False"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtOrderIssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOrderIssueDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtOrderIssueDate" ID="txtOrderIssueDateWatermarkExtender"
                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblReceiptNo" class="clsLabel">Receipt No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReceiptNo" runat="server" ReadOnly="True" BackColor="#E0E0E0"
                                                    Text="<%# mInvoice.InvoiceItems.CurrentItem.ReceiptNumber %>" CssClass="clsTextBoxTagSearch">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="lblReceiptDate" runat="server" class="clsLabel">Receipt Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReceiptDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                    BackColor="#E0E0E0" ReadOnly="true" ClientIDMode="Static" Text="<%# mInvoice.InvoiceItems.CurrentItem.ReceiptDateFormatted %>" Enabled="False"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtReceiptDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReceiptDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtReceiptDate" ID="txtReceiptDateTextBoxWatermarkExtender"
                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblReleaseNote" class="clsLabel">Rel. Note No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRelNoteNo" runat="server" ReadOnly="True" BackColor="#E0E0E0"
                                                    Text="<%# mInvoice.InvoiceItems.CurrentItem.ReleaseNoteNo %>" CssClass="clsTextBoxTagSearch">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="lblRelNoteDate" class="clsLabel">R. Note Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRelNoteDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                    BackColor="#E0E0E0" ReadOnly="true" ClientIDMode="Static" Text="<%# mInvoice.InvoiceItems.CurrentItem.ReleaseNoteDateFormatted %>" Enabled="False"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtRelNoteDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRelNoteDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtRelNoteDate" ID="txtRelNoteDateTextBoxWatermarkExtender"
                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
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
                            <fieldset id="Fieldset2" style="padding: 0px 4px 0px 0px; width: auto;" class="clsLabelHeader">
                                <legend><b>Values</b></legend>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="spnQtyStar" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="spnQty" class="clsLabel">Qty.</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="9" Enabled="false" Text="<%# mInvoice.InvoiceItems.CurrentItem.DisplayQtyForForFourDigit %>"
                                                ToolTip="Enter Quantity." Width="150px"></asp:TextBox>
                                            <%-- <asp:TextBox ID="txtDisplayQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="9" Enabled="false" Text="<%# mInvoice.InvoiceItems.CurrentItem.DisplayQty %>"
                                                ToolTip="Enter Quantity." Width="150px"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            <%-- <asp:TextBox ID="txtQtyUnit" runat="server" Width="96px" ReadOnly="True" BackColor="#E0E0E0"
                                                Text="<%# mInvoice.InvoiceItems.CurrentItem.unit %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax">
                                            </asp:TextBox>--%>
                                            <asp:TextBox ID="txtQtyDisplayUnitName" runat="server" Width="96px" ReadOnly="True"
                                                BackColor="#E0E0E0" Text="<%# mInvoice.InvoiceItems.CurrentItem.DisplayUnitName %>"
                                                CssClass="clsTextBoxTagSearchSmall">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="lblRate" class="clsLabelAuto">Rate</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# IIf(mInvoice.TransTypeID = 10, mInvoice.InvoiceItems.CurrentItem.GROCRate, mInvoice.InvoiceItems.CurrentItem.DisplayCRateForFourDigit) %>"
                                                ToolTip="Enter Rate" Width="150px"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtDisplayRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# IIf(mInvoice.TransTypeID = 10, mInvoice.InvoiceItems.CurrentItem.GROCRate, mInvoice.InvoiceItems.CurrentItem.DisplayCRate) %>"
                                                ToolTip="Enter Rate" Width="150px" AutoPostBack='<%# AppSettings("ClientCode") <> "BA" %>'></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRateCurrency" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                Width="96px" ReadOnly="True" Text="<%# mInvoice.InvoiceItems.CurrentItem.Currency %>">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOtherCharges" runat="server" CssClass="clsLabelAuto" Visible="False">Oth. Charges</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherCharges" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# mInvoice.InvoiceItems.CurrentItem.COtherCharges %>"
                                                ToolTip="Enter Other Charges" Visible="False" Width="150px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="lblAmount" class="clsLabelAuto">Amount</span>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtAmount" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# mInvoice.InvoiceItems.CurrentItem.DisplayCAmountForFourDigit %>"
                                                Width="150px"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtDisplayCAmount" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# mInvoice.InvoiceItems.CurrentItem.DisplayCAmount %>"
                                                Width="150px" AutoPostBack='<%# AppSettings("ClientCode")="BA" %>'></asp:TextBox>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="Span2" class="clsLabelAuto">Effective Rate</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEffectiveRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# IIf(mInvoice.TransTypeID = 10, mInvoice.InvoiceItems.CurrentItem.GROCEffRate, mInvoice.InvoiceItems.CurrentItem.DisplayCEffRateForFourDigit) %>"
                                                Width="150px" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                            <%-- <asp:TextBox ID="txtDisplayEffectiveRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="12"
                                                Text="<%# IIf(mInvoice.TransTypeID = 10, mInvoice.InvoiceItems.CurrentItem.GROCEffRate, mInvoice.InvoiceItems.CurrentItem.DisplayCEffRate) %>"
                                                Width="150px" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="Span3" class="clsLabelAuto">Commercial Rate</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCommercialRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# mInvoice.InvoiceItems.CurrentItem.DisplayCCommercialRateForFourDigit %>"
                                                Width="150px" ToolTip="Enter Commercial Rate"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtDisplayCommercialRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# mInvoice.InvoiceItems.CurrentItem.DisplayCCommercialRate %>"
                                                Width="150px" ToolTip="Enter Commercial Rate" AutoPostBack="true"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="2">
                    <asp:Panel runat="server" ID="Panel6" Style="width: auto;">
                        <asp:UpdatePanel runat="server" ID="upnlRemarkNote" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="Fieldset12" style="padding: 0px 4px 0px 0px; width: auto;" class="clsLabelHeader">
                                    <legend><b>Remark/Note</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <span id="spnRemark" class="clsLabel">Remark </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBox1" Height="36px" MaxLength="250"
                                                    Text="<%# mInvoice.InvoiceItems.CurrentItem.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark."
                                                    Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="Span6" class="clsLabelAuto">Note </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBox1" Height="36px" MaxLength="250"
                                                    Text="<%# mInvoice.InvoiceItems.CurrentItem.Note %>" TextMode="MultiLine" ToolTip="Enter Note."
                                                    Width="250px"></asp:TextBox>
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
                <td colspan="2" align="right">
                </td>
            </tr>
        </table>
    </div>
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
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
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
</body>
</html>
