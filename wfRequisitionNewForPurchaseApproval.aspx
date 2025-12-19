<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionNewForPurchaseApproval.aspx.vb"
    Inherits="Flypal.wfRequisitionNewForPurchaseApproval" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Requisition New For Purchase Approval</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblLedgerList">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Requisition For Purchase Approval</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:CustomValidator  ID="cvcp" runat="server" ErrorMessage="Select "
                                    Display="None" ControlToValidate="txtPartNo" OnServerValidate="customvalidate"
                                    CssClass="cslLabelAuto"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td>
                                            <asp:Label  ID="lblPartNoc" runat="server" CssClass="clsLabel"
                                                Font-Bold="True">Part No.</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox  ID="txtPartNo" runat="server" CssClass="clsTextBox"
                                                Font-Bold="True" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label  ID="lblDescription" runat="server" CssClass="clsLabel"
                                                Font-Bold="True">Description</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox  ID="txtDescription" runat="server" CssClass="clsTextBoxLong"
                                                Font-Bold="True" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label  ID="lblTotReqQtyC" runat="server" CssClass="clsLabelAuto">Total Req. Qty.</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox  ID="txtTotReqQty" runat="server" CssClass="clsTextBoxRightAlign1"
                                                BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Requisition(s) </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:DataGrid ID="dgRequisitionList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="SrNo" HeaderText="Sr.No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="DateFormatted" HeaderText="Date">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Number" HeaderText="Number"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="RequestedBy" HeaderText="Requested By"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="RequestedQty" HeaderText="Req. Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Location" HeaderText="Location"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Quotations">
                                            <ItemTemplate>
                                                <asp:DropDownList  ID="cmbAppQuotation" runat="server" CssClass="clsComboBox1"
                                                    DataTextField="QuotationDetail" DataValueField="ID" DataSource="<%# mQuotationItems %>">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                            <tr>
                                <td>
                                    <asp:Label ID="lblQuotation" runat="server" CssClass="clsLabelHeader">All Quotation(s)</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblPurchase" runat="server" CssClass="clsLabelHeader">Last 10 Purchase</asp:Label>
                                </td>
                            </tr>
                        <tr>
                            <td valign="top">
                                <asp:DataGrid ID="dgReqQuotes" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="3">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="QuotationDateFormatted" HeaderText="Date">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="QuotationNo" HeaderText="Number">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="WebRequisitionsNew" HeaderText="Requisition(s)"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="VendorName" HeaderText="Supplier"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Currency" HeaderText="Currency"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ConversionFactor" HeaderText="Conv. Factor">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Qty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CRate" HeaderText="Rate" DataFormatString="{0:#00.00}">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PriorityName" HeaderText="Priority"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="DeliveryInDays" HeaderText="Del. In Days">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PaymentTerm" HeaderText="Payment Term"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Note" HeaderText="Note"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Attach">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" Text="View" ID="LinkButton1" CommandName="Select"
                                                    CausesValidation="false"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="Size" HeaderText="Size"></asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                            <td valign="top">
                                <asp:DataGrid ID="dgInvoiceItemList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="3">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="InvoiceDateFormatted" HeaderText="Invoice Date">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="InvoiceNumber" HeaderText="Invoice No.">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="OrderDateFormatted" HeaderText="Order Date">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="OrderNumber" HeaderText="Order No.">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="VendorName" HeaderText="Supplier"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CurrencyName" HeaderText="Currency"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ConversionFactor" HeaderText="Conv. Factor">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReleaseNoteNo" HeaderText="Release Note No.">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Qty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CRate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CCommercialRate" HeaderText="Commercial Rate">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <table class="clstableButton" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="clsButton" Text="Ok" ToolTip="Click to Accept the Requisition">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to go back to the previous page">
                                            </asp:Button>
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
