<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOtherChargeDocketDetails_Ajax.aspx.vb"
    Inherits="Flypal.wfOtherChargeDocketDetails_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Charge Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
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
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                        <table id="tblinner" class="clsTablelistin">
                            <tr>
                                <td colspan="6">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="clsFormHeader1Newstyle">
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Other Charge Detail</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
                                                            CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                        <asp:CustomValidator ID="cvCha" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbCharge"
                                                            ErrorMessage="Charge Name Required" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator CssClass="clsLabelAuto" ID="rfvChaAm" runat="server"
                                                            ControlToValidate="txtChargeAmount" ErrorMessage="Charge Required" Display="None"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvV" runat="server" ControlToValidate="cmbVendorList" ErrorMessage="Select Vendor from the list."
                                                            Display="None" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvCurr" runat="server" ControlToValidate="cmbCurrencyList"
                                                            ErrorMessage="Select Currency from the list." Display="None" OnServerValidate="customvalidate"
                                                            CssClass="clsLabelAuto"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvF" runat="server" ControlToValidate="txtConversionFactor"
                                                            ErrorMessage="Currency factor must be greater than zero." Display="None" OnServerValidate="customvalidate"
                                                            CssClass="clsLabelAuto"></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="rfvF" runat="server" ControlToValidate="txtConversionFactor"
                                                            ErrorMessage="Currency factor must be greater than zero." Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvCha" runat="server" Display="None" ErrorMessage="Charge Name Required"
                                                            ControlToValidate="cmbCharge" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvA" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtChargeAmount"
                                                            ErrorMessage="Amount should be Greater than 0" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvS" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtCSeriveCharge"
                                                            ErrorMessage="Service Charge Can not be Negative." Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <span id="lblOtherChargeDetails" class="clsLabelHeader">Other Charge Details</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblVendorStar1" class="clsLabelStar">*</span>
                                </td>
                                <td>
                                    <span id="Label1" class="clsLabel">Service Provider</span>
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
                                        DataTextField="Name" SelectedValue="<%# mOtherCharge.OtherChargeDetails.CurrentItem.VendorID %>">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <span id="Label3" class="clsLabelAuto">Invoice No.</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceNo %>"
                                        MaxLength="50" ToolTip="Enter Invoice No.">
                                    </asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <span id="Label4" class="clsLabelAuto">Date</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                        onchange="ValidateDateText(this,'Date_watermarkextender','true');" Text="" Width="100px"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtInvDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInvDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender ID="txtInvDateWatermarkExtender" runat="server" TargetControlID="txtInvDate"
                                        WatermarkText="<%$AppSettings:DateFormat%>">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <span id="Label2" class="clsLabelAuto">Charge Type</span>
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbChargeType" runat="server" DataValueField="ID"
                                        DataTextField="OtherChargeName" SelectedValue="<%# mOtherCharge.OtherChargeDetails.CurrentItem.OtherChargeTypeID %>">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblCurrencyStar1" class="clsLabelStar">*</span>
                                </td>
                                <td>
                                    <span id="Label5" class="clsLabelAuto">Currency</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlCurrency" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCurrencyList" runat="server"
                                                DataValueField="ID" DataTextField="Name" SelectedValue="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CurrencyID %>"
                                                AutoPostBack="True" Enabled="<%# mOtherCharge.OtherChargeDetails.CurrentItem.IsNew %>">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <span id="lblFactorStar1" class="clsLabelStar">*</span>
                                </td>
                                <td>
                                    <span id="Label6" class="clsLabelAuto">Factor</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlConversionFactor" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtConversionFactor" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="60px"
                                                Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.ConversionFactor %>" MaxLength="50"
                                                ToolTip="Enter Conversion Factor" Enabled="<%# mOtherCharge.OtherChargeDetails.CurrentItem.IsNew %>"
                                                ReadOnly='<%# Session("Edit") %>'></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblChargeNameStar1" class="clsLabelStar">*</span>
                                </td>
                                <td>
                                    <span id="lblChargeName" class="clsLabelAuto">Charge Name</span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCharge" runat="server" DataValueField="ID"
                                        DataTextField="Name">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="imgbtnCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                            Width="24px"  
                                                                            ToolTip="Click to Add New Charge" CausesValidation="False">
                                                                        </asp:ImageButton>
                                </td>
                                <td>
                                   <%-- <asp:Button ID="imgbtnCharge" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                        ToolTip="Click to Add New Charge" CausesValidation="False"></asp:Button>--%>
                                </td>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblChargeAmountStar1" class="clsLabelStar">*</span>
                                </td>
                                <td>
                                    <span id="lblChargeAmount" class="clsLabelAuto">Charge Amount </span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtChargeAmount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="190px"
                                        Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CAmount %>" MaxLength="12"
                                        ToolTip="Enter Charge Amount">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <span id="Label7" class="clsLabelAuto">Service Tax</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtCSeriveCharge" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="190px"
                                        Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CServiceCharges %>" MaxLength="12"
                                        ToolTip="Enter Service Charge">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <span id="Label8" class="clsLabelAuto">Total Amount</span>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="190px"
                                        Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CGrandTotal %>" ReadOnly="True"
                                        BackColor="#E0E0E0" ToolTip="Total Amount">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="right">
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH1" Text="Ok" ToolTip="Click to Save the Charge Information">
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
    </div>
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
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddSupplier = document.getElementById("cmbVendorList");
             if  (ddSupplier != null) {
             var i = 0;
              if  (ddSupplier.disabled ==false)
             {
              <% For Each item1 In mVendorList%>
                <% If  item1.NotInUse ="True" Then%>
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
