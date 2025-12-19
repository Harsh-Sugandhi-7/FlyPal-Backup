<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfStoreApprovalList.aspx.vb"
    Inherits="Flypal.wfStoreApprovalList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Store Approval Requisition Part list</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Store Approved Requisition Part List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPartNumber" runat="server" CssClass="clsLabel">Part Number</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartNumber" runat="server" CssClass="clsTextBox1" MaxLength="50"></asp:TextBox>
                            </td>
                            <td align="right" colspan="1">
                                <table id="Table2">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" ToolTip="Click to find the list of records as per searching criteria."
                                                Text="Find Now" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto">Click on Check Box to Select Part Information or click on Close button to Close the screen.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:DataGrid ID="dgRequisitionItemList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                    AllowPaging="True" PageSize="20" AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>'
                                                    AutoPostBack="True"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Description">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="IPCReference" SortExpression="IPCReference" HeaderText="IPC Reference">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="DateFormatted" HeaderText="Date">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="RequisitionNo" SortExpression="RequisitionNo" HeaderText="Number">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="RequestedQty" SortExpression="RequestedQty" HeaderText="Engineer Requested Qty.">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PurchaseQty" SortExpression="PurchaseQty" HeaderText="Store Requested Qty.">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="EngApprovedIssueQty" SortExpression="EngApprovedIssueQty"
                                            HeaderText="Engg. Issue Requested Qty.">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="EnquiryBalQty" SortExpression="EnquiryBalQty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PurchaseQty" SortExpression="PurchaseQty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="3">
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnOk" runat="server" ToolTip="Click to add the selected Requisition Item"
                                                Text="Ok" CssClass="clsButton"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to close Store Approved Requisition Part List screen"
                                                Text="Close" CausesValidation="False"></asp:Button>
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
