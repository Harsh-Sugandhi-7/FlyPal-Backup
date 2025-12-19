<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingLoanToRecover_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingLoanToRecover_Ajax" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Pending Loan to Receive from Store</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="wfgroup" method="post" runat="server">
    <%-- AJAX ScriptManager --%>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblTitle" class="clsFormHeader">Pending Loan to Receive from Store</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
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
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblPartNo" class="clsLabelAuto">Part No</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part Number"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Search the Record"
                                                        Text="Find Now"></asp:Button>--%>
                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to Search the Record"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlPendingList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgPendingList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                                        EnableViewState="False" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                                                        GridLines="Horizontal" CellPadding="7">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="IssueItemID" HeaderText="IssueItemID">
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IssueText" HeaderText="Issue Text" SortExpression="IssueText">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IssueNo" HeaderText="Issue No." SortExpression="IssueNo">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" ForeColor="Black"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" ForeColor="Black"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="Part Desc." SortExpression="Description">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FromStoreName" HeaderText="From Store" SortExpression="FromStoreName">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ToStoreName" HeaderText="To Store" SortExpression="ToStoreName">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Qty" HeaderText="Issued Qty." SortExpression="Qty">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" ForeColor="Black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanQty" HeaderText="Pending Qty." SortExpression="LoanQty">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" ForeColor="Black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Blue" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False"/>
                                                            </asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
</body>
</html>
