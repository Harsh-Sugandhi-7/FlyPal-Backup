<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOtherChargeDocket_Ajax.aspx.vb"
    Inherits="Flypal.wfOtherChargeDocket_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Other Charge Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                        <table id="tblinner" class="clsTablelistin">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="clsFormHeader1Newstyle">
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Other Charge [New]</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                        <asp:CustomValidator ID="cvOtherChargeDate" runat="server" ErrorMessage="Select OtherCharge Date"
                                                            ControlToValidate="txtOtherChargeDate" Display="None" OnServerValidate="CustomValidate"
                                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="rfvOtherChargeDate" runat="server" ErrorMessage="Select OtherCharge Date."
                                                            ControlToValidate="txtOtherChargeDate" Display="None" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvNo" runat="server" ErrorMessage="Number Required." ControlToValidate="txtNo"
                                                            Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upblOtherCharge" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <span id="lblOrderDetails" class="clsLabelHeader">Other Charge</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblDateStar1" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblDate" class="clsLabel">Date</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtOtherChargeDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                    AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                    Text="" Width="100px"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtOtherChargeDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOtherChargeDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender ID="txtOtherChargeDateWatermarkExtender" runat="server"
                                                    TargetControlID="txtOtherChargeDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblNoStar1" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblNo" class="clsLabel">No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Other Charge text "
                                                    Text="<%# mOtherCharge.Text %>" MaxLength="25">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip=" Enter Other Charge No."
                                                    Text="<%# mOtherCharge.No %>" MaxLength="8" Width="60px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <span id="lblDetails" class="clsLabelHeader">Details</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblName" class="clsLabel">Bill Entry No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBillEntryNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Bill Entry No."
                                                    Text="<%# mOtherCharge.BillEntryNo %>" MaxLength="50">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="Label3" class="clsLabel">Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBillEntryDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                    onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                    Text="<%# mOtherCharge.BillEntryDateFormatted %>"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtBillEntryDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtBillEntryDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtBillEntryDate" ID="txtBillEntryDateTextBoxWatermarkExtender"
                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblAddress" class="clsLabel">Master Airway Bill No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMasterAirwayBillNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                    ToolTip="Enter Master Airway Bill No." Text="<%# mOtherCharge.MasterAirwayBillNo %>"
                                                    MaxLength="50">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="Span1" class="clsLabel">Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMasterAirwayBillDate" runat="server" CssClass="clsTextBoxTagSearch"
                                                    Width="100px" onchange="ValidateDateText(this,'Date_watermarkextender','false');"
                                                    ClientIDMode="Static" Text="<%# mOtherCharge.MasterAirwayBillDateFormatted %>"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtMasterAirwayBillDateCalendarExtender" runat="server"
                                                    CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtMasterAirwayBillDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtMasterAirwayBillDate" ID="txtMasterAirwayBillDateTextBoxWatermarkExtender"
                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span2" class="clsLabel">House Airway Bill No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHouseAirwayBillNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                    ToolTip="Enter House Airway Bill No" Text="<%# mOtherCharge.HouseAirwayBillNo %>"
                                                    MaxLength="50">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="Span3" class="clsLabel">Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHouseAirwayBillDate" runat="server" CssClass="clsTextBoxTagSearch"
                                                    Width="100px" onchange="ValidateDateText(this,'Date_watermarkextender','false');"
                                                    ClientIDMode="Static" Text="<%# mOtherCharge.HouseAirwayBillDateFormatted %>"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtHouseAirwayBillDateCalendarExtender" runat="server"
                                                    CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtHouseAirwayBillDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtHouseAirwayBillDate" ID="txtHouseAirwayBillDateTextBoxWatermarkExtender"
                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="Label2" class="clsLabelHeader">Invoice</span>
                                                <asp:GridView ID="dgInvoices" runat="server" CssClass="clsGridNewStyle" EnableViewState="False"
                                                    AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="True" CellPadding="5" GridLines="Horizontal">
                                                   <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"  />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="DateFormatted" HeaderText="Date"></asp:BoundField>
                                                        <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Vendor" HeaderText="Supplier"></asp:BoundField>
                                                        <asp:BoundField DataField="VendorInvNo" HeaderText="Supplier Inv. No."></asp:BoundField>
                                                        <asp:BoundField DataField="VendorInvDateFormatted" HeaderText="Supplier Inv. Date">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Currency" HeaderText="Currency"></asp:BoundField>
                                                        <asp:BoundField DataField="ConversionFactor" HeaderText="Conv. Factor">
                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total ">
                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remark" HeaderText="Remark"></asp:BoundField>
                                                    </Columns>
                                                     <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlReceiptCumInvItems" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblChargeList" class="clsLabelHeader">Charge List</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddCharge" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click To Add Charge"
                                                                        Text="Add"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgCharges" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                             <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"  />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ChargeName" HeaderText="Charge"></asp:BoundField>
                                                                <asp:BoundField DataField="Vendor" HeaderText="Service Provider"></asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Date"></asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice Number">
                                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Currency" HeaderText="Currency"></asp:BoundField>
                                                                <asp:BoundField DataField="ConversionFactor" HeaderText="Conv . Factor">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CServiceCharges" HeaderText="Service Charges">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CGrandTotal" HeaderText="Total Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                               <%-- <asp:ButtonField CommandName="EditView" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left"  />
                                                                </asp:ButtonField>--%>
                                                                  <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditView" runat="server" 
                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" CausesValidation="false"/>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            </Columns>
                                                             <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to save Other Charge"
                                                            Text="Save"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Print Other Charge"
                                                            Text="Print"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                            Text="Close"></asp:Button>
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
</body>
</html>
