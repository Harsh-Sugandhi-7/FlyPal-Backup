<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForModel.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForModel" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Search Criteria For Model</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblPartList" runat="server" CssClass="clstitle1">Model & Serial No. of Assembly</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto">Total Records Found</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Step - I.  Selection Of Model And Serial No</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <asp:DataGrid ID="dgPartList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                    PageSize="3" AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                            <tr>
                                <td align="right" colspan="5">
                                    <table class="clstableButton" align="right">
                                        <tr>
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
                <asp:Panel ID="pnlMessageBox" Style="z-index: 111" runat="server">
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
