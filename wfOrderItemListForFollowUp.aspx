<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOrderItemListForFollowUp.aspx.vb"
    Inherits="Flypal.wfOrderItemListForFollowUp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Order Follow Up</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
     <link id="MainStyle" type="text/css" rel="stylesheet" />
     <asp:PlaceHolder runat="server">
      <!-- #include file= "LocalFunctionAjax.htm" -->
     </asp:PlaceHolder>

    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    
    <script id="clientEventHandlersJS" type="text/javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>

    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/close.gif";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/detail.gif";
            }
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
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblMain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPurchaseOrderList" runat="server" CssClass="clsFormHeader">Order Follow Up</asp:Label>
                                        </td>
                                        <td align="right">
                                            <table id="Table2">
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Print " ToolTip="Click to Print Order Item List For Follow Up"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExportTop" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0" Text="Export to Excel"
                                                            ToolTip="Click to Export report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Order Item Follow Up screen."
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
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td width="70px">
                                            <asp:Label ID="lblAsOnDate" runat="server" CssClass="clsLabelauto">As On Date</asp:Label>
                                        </td>
                                        
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBoxTagSearchDateWOList" Height="18px"
                                                CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate"></cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            <asp:CustomValidator ID="cvAsOnDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ClientValidationFunction="BetweenDatesValidation"></asp:CustomValidator>
                                        </td>
                                        
                                        <td colspan="4">
                                            <asp:CheckBox ID="chkReceivedorderitemfollowup" runat="server" CssClass="clsCheckBox"
                                                Text="Select to view all Followup records including Items which have been already received">
                                            </asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelauto">Search</asp:Label>
                                        </td>
                                        
                                        <td width="5%">
                                            <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                <asp:ListItem Value="1">Order</asp:ListItem>
                                                <asp:ListItem Value="2">Part No.</asp:ListItem>
                                                <asp:ListItem Value="3">Priority</asp:ListItem>
                                                <asp:ListItem Value="4">Order Type</asp:ListItem>
                                                <asp:ListItem Value="5">Supplier</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                       <%-- <td>
                                            <asp:Label ID="L1" runat="server" CssClass="clsLabel" Width="20px"></asp:Label>
                                        </td>--%>
                                      
                                            <td width="5%">
                                                <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False">
                                                    <asp:ListItem Value="0">None</asp:ListItem>
                                                    <asp:ListItem Value="1">Low</asp:ListItem>
                                                    <asp:ListItem Value="2">Medium</asp:ListItem>
                                                    <asp:ListItem Value="3">High</asp:ListItem>
                                                    <asp:ListItem Value="4">AOG</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                    Visible="False" DataTextField="Text" DataValueField="Text">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"></asp:TextBox>
                                                <asp:DropDownList ID="cmbOrderTypeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                    <asp:ListItem Value="0" Selected="True">(All)</asp:ListItem>
                                                    <asp:ListItem Value="5">Outright</asp:ListItem>
                                                    <asp:ListItem Value="38">Overhaul / Repair</asp:ListItem>
                                                    <asp:ListItem Value="39">Rental / Lease</asp:ListItem>
                                                    <asp:ListItem Value="31">Exchange</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="2%">
                                                &nbsp;
                                                <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Visible="False">No.</asp:Label>
                                            </td>
                                            <td>
                                                <table id="Table4" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False" MaxLength="6"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxTagSearchSmall" Visible="False"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
                                                                CssClass="clsSearch2btn" ToolTip="Click to find list of Order as per searching criteria" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Order Item(s) as per criteria : Record(s) found</asp:Label>
                            </td>
                            <%--<td align="right">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" 
                                                CssClass="clsSearch2btn" ToolTip="Click to find list of Order as per searching criteria" />
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRed" runat="server" CssClass="clsLabelAuto" BackColor="Red" ForeColor="Red">Green</asp:Label>
                                <asp:Label ID="lblRedInfo" runat="server" CssClass="clsLabelauto">Cross Delivery Limit</asp:Label>
                                <asp:Label ID="lblYellow" runat="server" CssClass="clsLabelauto" BackColor="Yellow"
                                    ForeColor="Yellow">Green</asp:Label>
                                <asp:Label ID="lblYellowInfo" runat="server" CssClass="clsLabelauto">Delivery Limit 0 to 15 Days</asp:Label>
                                <asp:Label ID="lblGreen" runat="server" CssClass="clsLabelauto" BackColor="Green"
                                    ForeColor="Green">Green</asp:Label>
                                <asp:Label ID="lblGreenInfo" runat="server" CssClass="clsLabelauto">Delivery Limit More Than 15 Days</asp:Label>
                            </td>
                            <%--<td align="right">
                                <table id="Table2">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Print " ToolTip="Click to Print Order Item List For Follow Up"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExportTop" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0" Text="Export to Excel"
                                                ToolTip="Click to Export report" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Order Item Follow Up screen."
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                               
                                <asp:GridView ID="dgOrderList" runat="server" AutoGenerateColumns="False"
                                    CellPadding="10" GridLines="Horizontal" CssClass="clsGridNewStyle" PageSize="3" Width="100%" ShowHeaderWhenEmpty="true">
                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <div>
                                                    <a href="javascript:showNestedGridView('ID-<%# Eval("OrderItemID") %>');">
                                                        <img id="imageID-<%# Eval("OrderItemID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif" />
                                                    </a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--0--%>

                                        <asp:BoundField Visible="False" DataField="OrderID" HeaderText="OrderID"></asp:BoundField>
                                        <asp:BoundField Visible="False" DataField="OrderItemID" HeaderText="OrderItemID"></asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblColor" runat="server" Width="4px" Height="19px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date">
                                            <HeaderStyle></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderTextNo" SortExpression="OrderTextNo" HeaderText="Order No.">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IntOrderNo" SortExpression="IntOrderNo" HeaderText="Int.Ord.No.">
                                            <HeaderStyle></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderType" SortExpression="OrderType" HeaderText="Order Type">
                                            <HeaderStyle></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SupplierName" SortExpression="SupplierName" HeaderText="Supplier">
                                            <HeaderStyle></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PartDescription" SortExpression="PartDescription" HeaderText="Description">
                                            <HeaderStyle></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                            <HeaderStyle></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DeliveryInDays" SortExpression="DeliveryInDays" HeaderText="Delivery in Days">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PriorityName" SortExpression="PriorityName" HeaderText="Priority">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RemainingDays" SortExpression="RemainingDays" HeaderText="Remaining Days">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrdQty" HeaderText="Ord.Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RecQty" HeaderText="Rec.Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BalQty" HeaderText="Bal.Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CAmount" HeaderText="Bal.Amount">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                            <HeaderStyle></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Amount" HeaderText="Bal.Amount (Base Currency)">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastFODate" HeaderText="Last F.O. Date">
                                            <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastFORemark" HeaderText="Last F.O. Remark">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:ButtonField Text="Follow Up" HeaderText="Follow Up" CommandName="FollowUp"></asp:ButtonField>
                                        <asp:BoundField Visible="False" DataField="OrderItemFollowCount" HeaderText="OrderItemFollowCount"></asp:BoundField>


                                        <%--13--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <tr>
                                                    <td colspan="95%" bgcolor="White" width="0px">
                                                        <div id="ID-<%# Eval("OrderItemID") %>" style="display: none; position: relative;">
                                                            <panel>

                                                                <table width="100%">
                                                                    <tr>
                                                                        <td  bgcolor="White" width="0px">
                                                                            <asp:GridView ID="dgOrderListchild" runat="server" AutoGenerateColumns="False" 
                                                                                CellPadding="5" GridLines="Horizontal" BorderStyle="Groove" ForeColor="#333333" CssClass="clsGridNewStyle"
                                                                                AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                                SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                                                
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr No.">
                                                                                        <HeaderStyle Wrap="false" />
                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"/>
                                                                                    </asp:BoundField>
                                                                                    <%--0--%>
                                                                                    <asp:BoundField DataField="FollowUpDateFormatted" HeaderText="Date" >
                                                                                        <HeaderStyle  Wrap="false" />
                                                                                        <ItemStyle  Wrap="false" HorizontalAlign="Center"/>
                                                                                    </asp:BoundField>
                                                                                    <%--1--%>

                                                                                    <asp:BoundField DataField="FollowUpTextNo" HeaderText="No."><%--2--%>
                                                                                        <HeaderStyle Wrap="false" />
                                                                                        <ItemStyle Wrap="true" HorizontalAlign="Center"/>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="AWBNo" HeaderText="AWB No.">
                                                                                        <HeaderStyle Wrap="false" />
                                                                                        <ItemStyle Wrap="true" HorizontalAlign="Center"/>
                                                                                    </asp:BoundField>
                                                                                    <%--3--%>

                                                                                    <asp:BoundField DataField="ProformaNo" HeaderText="Proforma No.">
                                                                                        <HeaderStyle Wrap="false" />
                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"/>
                                                                                    </asp:BoundField>
                                                                                    <%--4--%>

                                                                                    <asp:BoundField DataField="ReturnInDays" HeaderText="Return In Days">
                                                                                        <HeaderStyle Wrap="false" />
                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"/>
                                                                                    </asp:BoundField>
                                                                                    <%--5--%>

                                                                                    <asp:BoundField DataField="TD" 
                                                                                        HeaderText="TD">
                                                                                        <HeaderStyle Wrap="false" />
                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center"/>

                                                                                    </asp:BoundField>
                                                                                    <%--6--%>

                                                                                    <asp:BoundField DataField="ShipmentStatus" HeaderText="Shipment Status">
                                                                                         <HeaderStyle  Wrap="false" />
                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Center">
                                                                                        </ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <%--7--%>

                                                                                    <asp:BoundField DataField="FollowUpRemarks" HeaderText="Remark">
                                                                                        <HeaderStyle Wrap="false" />
                                                                                        <ItemStyle Wrap="true" HorizontalAlign="Center"/>
                                                                                    </asp:BoundField>
                                                                                    <%--8--%>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="ControlDark" />
                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                </asp:GridView>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <table id="Table3">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnPrintBottom" runat="server" 	CssClass="clsbtnH clsinfoH" Text="Print "
                                                ToolTip="Click to Print Order Item List For Follow Up" CausesValidation="False" Visible="false">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExportBottom" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                Text="Export to Excel" ToolTip="Click to Export report" Visible="false"/>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCloseBottom" runat="server" 	CssClass="clsbtnH clsinfoH" Text="Close"
                                                ToolTip="Click to close Order Item Follow Up screen." CausesValidation="False" Visible="false">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
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
    </form>
</body>
</html>
