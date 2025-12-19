<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAlternatePartListForOrder_Ajax.aspx.vb"
    Inherits="Flypal.wfAlternatePartListForOrder_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Alternate Part List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table id="tblLedgerList" class="clstablelistin">
                                <tr>
                                    <td colspan="2" class="clsFormHeader1">
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Alternate Part [New]</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblSelectedPart" class="clsLabelHeader">Selected Part</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="clsPartNo" class="clsLabel">Part No.</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPartNo" runat="server" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mItem.Name %>"
                                            ToolTip="Enter Part No." CssClass="clsTextBoxTagSearch">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDescription" runat="server" class="clsLabel">Description</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" ReadOnly="True"
                                            Text="<%# mItem.Description %>" ToolTip="Enter Description" CssClass="clsTextBoxTagSearchMultilineNewStyleLong">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="left">
                                        <asp:UpdatePanel runat="server" ID="upnlCreatealternatepart" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table1" class="clstablebutton" align="right">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCreatealternatepart" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                Text="Create alternate part" ToolTip="Click to create alternate part" Visible="False"
                                                                Width="150px" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlAlternatePartList" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Search Resulted: No.of Record Found(s).</asp:Label>
                                                <asp:GridView ID="dgAlternatePartList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                                    EnableViewState="False" AutoGenerateColumns="False" AllowSorting="False" ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PartDescription" SortExpression="PartDescription" HeaderText="Description">
                                                            <HeaderStyle   HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AltTypeName" SortExpression="AltTypeName" HeaderText="Part Type">
                                                            <HeaderStyle  HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table2" class="clstablebutton" align="right">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to close Alternate Part screen"
                                                                CausesValidation="False"></asp:Button>
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
</body>
</html>
