<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOrderItemForFollowUp.aspx.vb" Inherits="Flypal.wfOrderItemForFollowUp" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Other Charge Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>


    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
                // $find(extenderid).set_Text(result);
                __doPostBack($(elem).id, "TextChanged");
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


</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
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
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                    <table id="tblinner" class="clsTablelistin">
                        <tbody>
                            <tr>
                                <td colspan="7" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Order Follow Up [New]</asp:Label>
                                            </td>
                                            <%--<td colspan="6" align="right">
                                                <table id="Table1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to save Export Invoice"
                                                                Text="Save"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                Text="Close"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>--%>
                                        </tr>
                                    </table>

                                </td>
                            </tr>

                            <tr>
                                <td colspan="7">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvCommon" runat="server" OnServerValidate="customvalidate"
                                        Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7" align="left">
                                    <asp:Label ID="lblOrderDetails" runat="server" CssClass="clsLabelHeader">Order Details</asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td align="left"></td>
                                <td align="left">
                                    <asp:Label ID="lblOrderDate" runat="server" CssClass="clsLabel">Order Date</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="clsTextBoxTagSearchDate" BackColor="Gainsboro"
                                        ReadOnly="True" ></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabel">Order No.</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtOrderNo" runat="server" CssClass="clsTextBoxTagSearch" BackColor="Gainsboro"
                                        ReadOnly="True" ></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblSupplier" runat="server" CssClass="clsLabel">Supplier</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSupplier" runat="server" CssClass="clsTextBoxTagSearch" BackColor="Gainsboro"
                                        ReadOnly="True" ></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td align="left" colspan="7">
                                    <asp:Label ID="lblFollowUpDetail" runat="server" CssClass="clsLabelHeader">Follow Up Details</asp:Label>
                                </td>
                            </tr>

                            <tr>
                                
                                            <td align="left">
                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" Width="8px">*</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblFolloUpDate" runat="server" CssClass="clsLabel"> Date</asp:Label>
                                            </td>

                                            <td>
                                                <asp:TextBox runat="server" ID="txtFollowUpDate" CssClass="clsTextBoxTagSearchDateWOList" Height="18px"
                                                    CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FollowUpDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtFollowUpDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFollowUpDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFollowUpDate" ID="FollowUpDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                <asp:CustomValidator ID="cvFollowUpDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                    ClientValidationFunction="BetweenDatesValidation"></asp:CustomValidator>
                                            </td>

                                            <td align="left">
                                                <asp:Label ID="lblAWBNo" runat="server" CssClass="clsLabel">AWB No.</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtAWBNo" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                            </td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                            </tr>


                            <tr>
                                
                                            <td align="left">
                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar" Width="8px">*</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblNo" runat="server" CssClass="clsLabel">Follow Up No.</asp:Label>
                                            </td>
                                            <td align="left">
                                                <table id="Table4" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" BackColor="Gainsboro"
                                                                ReadOnly="True" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNo" onkeypress="javascript:validate('No');" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                                BackColor="Gainsboro" ReadOnly="True" MaxLength="8" ToolTip="Enter Goods Receipt No." Height="18px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblProformaNo" runat="server" CssClass="clsLabel">Proforma No. </asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtProformaNo" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                            </td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                      
                            </tr>

                            <tr>
                                            <td align="left"></td>
                                            <td align="left">
                                                <asp:Label ID="lblTD" runat="server" CssClass="clsLabel">TD</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtTD" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblShipmentStatus" runat="server" CssClass="clsLabel">Shipment Status</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtShipmentStatus" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                            </td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                       
                            </tr>

                            <tr>
                                
                                            <td align="left"></td>
                                            <td align="left">
                                                <asp:Label ID="lblRetrunInDays" runat="server" CssClass="clsLabelAuto">Retrurn In Days</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtReturnInDays" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" MaxLength="8"
                                                    onKeyPress="javascript:validate('days');"></asp:TextBox>
                                            </td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                     
                            </tr>




                            <tr>
                                            <td align="left"></td>
                                            <td align="left">
                                                <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                            </td>
                                            <td colspan="5" align="left">
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="600px"
                                                    ToolTip="Enter Remark" Rows="5" Height="62px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                            </tr>

                            <tr>

                                <td align="left" colspan="7">
                                    <asp:CheckBox ID="chkAddOrderItem" runat="server" CssClass="clsLabelAuto" Text="check to add current follow up record to other Ordered item(s) of same order"
                                        AutoPostBack="True" />
                                </td>
                            </tr>

                            <tr>
                                <td align="left" colspan="7">

                                    <asp:DataGrid ID="dgOrderList" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                        <AlternatingItemStyle CssClass="clsdgAltItem" />
                                        <ItemStyle CssClass="clsdgItem" />
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Select">
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static"></asp:CheckBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="OrderID" HeaderText="OrderID" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="OrderItemID" HeaderText="OrderItemID" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="OrderDate" HeaderText="Order Date">
                                                <HeaderStyle />
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="OrderTextNo" HeaderText="Order No." SortExpression="OrderTextNo">
                                                <HeaderStyle Wrap="False" />
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="IntOrderNo" HeaderText="Int.Ord.No." SortExpression="IntOrderNo">
                                                <HeaderStyle />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="OrderType" HeaderText="Order Type" SortExpression="OrderType">
                                                <HeaderStyle />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="SupplierName" HeaderText="Supplier" SortExpression="SupplierName">
                                                <HeaderStyle />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="PartName" HeaderText="Part No." SortExpression="PartName">
                                                <HeaderStyle Wrap="False" />
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="PartDescription" HeaderText="Description" SortExpression="PartDescription">
                                                <HeaderStyle />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                <HeaderStyle />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="DeliveryInDays" HeaderText="Delivery in Days" SortExpression="DeliveryInDays">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="PriorityName" HeaderText="Priority" SortExpression="PriorityName">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="RemainingDays" HeaderText="Remaining Days" SortExpression="RemainingDays">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="OrdQty" HeaderText="Ord.Qty.">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="RecQty" HeaderText="Rec.Qty.">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="BalQty" HeaderText="Bal.Qty.">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CAmount" HeaderText="Bal.Amount">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                <HeaderStyle />
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Amount" HeaderText="Bal.Amount (Base Currency)">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <script type="text/javascript">
                                        $(document).ready(function () {
                                            $("#chkSelectAll").click(function () {
                                                var status = $("#chkSelectAll").attr("checked");
                                                $("#<%=dgOrderList.ClientID %>").find(":checkbox").each(function () {
                                                if (status == "checked") {
                                                    $(this).attr("checked", status);
                                                }
                                                else {
                                                    $(this).removeAttr("checked");
                                                }

                                            });
                                        });
                                        return false;
                                    });

                                    </script>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="6" align="left">
                                    <asp:Label ID="lblOrderItemFollowUp" runat="server" CssClass="clsLabelHeaderItem">Order Follow Up Item(s)</asp:Label>
                                </td>
                                <td align="right">
                                    <table id="Table2">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add" CausesValidation="true"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="7">
                                    <asp:GridView ID="dgOrderItemFollowUp" runat="server" CssClass="clsGridNewStyle" ToolTip="List of parts"
                                        AllowSorting="True" AutoGenerateColumns="False" Width="100%" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5"
                                        OnRowCommand="dgOrderItemFollowUp_RowCommand1">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                            <asp:BoundField DataField="OrderItemID" HeaderText="OrderItemID" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No."></asp:BoundField>
                                            <asp:BoundField DataField="FollowUpDateFormatted" HeaderText="Date">
                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                                <FooterStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FollowUpTextNo" SortExpression="FollowUpTextNo" HeaderText="No.">
                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                                <FooterStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AWBNo" HeaderText="AWB No.">
                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                                <FooterStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProformaNo" HeaderText="Proforma No."></asp:BoundField>
                                            <asp:BoundField DataField="ReturnInDays" HeaderText="Return In Days">
                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                                <FooterStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TD" HeaderText="TD"></asp:BoundField>
                                            <asp:BoundField DataField="ShipmentStatus" HeaderText="Shipment Status"></asp:BoundField>
                                            <asp:BoundField DataField="FollowUpRemarks" HeaderText="Remark"></asp:BoundField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>

                                                    <div class="dropdown">
                                                        <div class="dropdownbtn-content">
                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                            CommandName="Edit" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                            CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </div>

                                                        <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                            Style="cursor: pointer" />
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="clsdgHeader" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td colspan="6" align="right">
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Save Order Follow Up"
                                                    Text="Save" Enabled="<%# mOrderItemFollowUps.Count > 0 %>"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                    Text="Close"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
   
    </form>
</body>
</html>

