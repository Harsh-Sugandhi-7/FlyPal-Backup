<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectPartStatusForPartType.aspx.vb"
    Inherits="Flypal.wfSelectPartStatusForPartType" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
<head runat="server">
    <title>Part Type Status List</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" src="DATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="tblMain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tbody>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblPartsList" runat="server" CssClass="clstitle1">Part Type Status List</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                </td>
                            </tr>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:CustomValidator ID="cvcp" runat="server" CssClass="cslLabelAuto" Display="None"
                    OnServerValidate="customvalidate" ErrorMessage="Select "></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:Label ID="lblHeader" runat="server" CssClass="clsLabelHeader" Width="550px">Please select Part Status for each Part Type.Please make sure that at least one Part Type must be Unservicable(This is one time selection)</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True"> Part Type List</asp:Label>
            </td>
            <td colspan="2" align="right">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" CssClass="clsButton" Text="Update" ToolTip="Click to Update Part Status">
                            </asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to close Part Type List screen"
                                CausesValidation="False"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:DataGrid ID="dgItemTypeList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                    PageSize="25">
                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Part Type">
                            <HeaderStyle ForeColor="White"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Code" SortExpression="Code" HeaderText="Code"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Color">
                            <ItemTemplate>
                                <asp:Label ID="lblColor" runat="server" CssClass="clsColorLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Part Status">
                            <ItemTemplate>
                                <asp:DropDownList ID="cmbPartStatusList" runat="server" CssClass="clsCombobox1" OnSelectedIndexChanged="cmbPartStatusList_SelectedIndexChanged"
                                    AutoPostBack="True" DataTextField="PartStatusName" DataValueField="PartStatusID"
                                    DataSource="<%# mPartStatusList %>" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"PartStatusID") %>'>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td colspan="2" align="right">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnUpdateBottom" runat="server" CssClass="clsButton" Text="Update"
                                ToolTip="Click to Update Part Status"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to close Part Type List screen"
                                CausesValidation="False"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:panel></TD></TR></TBODY></TABLE></form>
</body>
</html>
