<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFlypalVideoHelp.aspx.vb"
    Inherits="Flypal.wfFlypalVideoHelp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="GridviewStyle.css" />
    <style>
        .gridview
        {
            background-color: #fff;
            padding: 2px;
            margin: 2% auto;
        }
        .gridview a
        {
            margin: auto 1%;
            border-radius: 50%;
            background-color: #444;
            padding: 5px 10px 5px 10px;
            color: #fff;
            text-decoration: none;
            -o-box-shadow: 1px 1px 1px #111;
            -moz-box-shadow: 1px 1px 1px #111;
            -webkit-box-shadow: 1px 1px 1px #111;
            box-shadow: 1px 1px 1px #111;
        }
        .gridview a:hover
        {
            background-color: #1e8d12;
            color: #fff;
        }
        .gridview span
        {
            background-color: #ae2676;
            color: #fff;
            -o-box-shadow: 1px 1px 1px #111;
            -moz-box-shadow: 1px 1px 1px #111;
            -webkit-box-shadow: 1px 1px 1px #111;
            box-shadow: 1px 1px 1px #111;
            border-radius: 50%;
            padding: 5px 10px 5px 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <table width="100%">
        <tr>
            <td>
                <table id="tblInner" width="100%">
                    <tr>
                        <td colspan="2" bgcolor="#2EA4A4" height="40px" class="Header">
                            <%--<asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Flypal Video Help</asp:Label>--%>
                            <table width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:Label ID="lblFlypalVideoHelp" runat="server" Text="Flypal Video Help" Style="color: #FFFFFF;
                                            font-weight: 700; font-size: large;"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtSearch" runat="server" Height="20px" class="icon-rtl" placeholder="Search"
                                                    AutoPostBack="true"> </asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="dgGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        CssClass="clsGrid" PageSize="25" ShowHeaderWhenEmpty="True" ShowHeader="false"
                                        DataKeyNames="ID,VideoPath,SrNo">
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                        <RowStyle CssClass="clsdgItem" BackColor="#E2ECE6" ForeColor="#333333"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="white" Font-Bold="True" ForeColor="black" />
                                        <AlternatingRowStyle CssClass="clsdgAltItem" BackColor="White"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="EditView" runat="server" CommandName="VideoView" Style="height: 50px;
                                                        width: 70px" ImageUrl='<%# Eval("ThumbnailPath") %>' CommandArgument='<%# Container.DataItemIndex %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VideoName" HeaderStyle-CssClass="hideGridColumn" HeaderText="Video">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VideoPath" HeaderStyle-CssClass="hideGridColumn" HeaderText="VideoPath"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SrNo" HeaderStyle-CssClass="hideGridColumn" HeaderText="SrNo"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <asp:UpdatePanel runat="server" ID="upnlVideo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <video width="1000" controls controlslist="nodownload" id="Vediosource" runat="server">
                                    <source type="video/mp4" >
                                   </video>
                                    <asp:GridView ID="dgGridView1" runat="server" AutoGenerateColumns="False" Width="400px"
                                        Height="80px" DataKeyNames="id" PageSize="1" AllowPaging="True" ShowHeader="false"
                                        BorderStyle="None" GridLines="None">
                                        <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Previous" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <video width="1000" controls controlslist="nodownload" src="<%# Eval("VideoPath") %>">
                          
                          </video>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Previous" />
                                        <PagerStyle CssClass="gridview" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        document.addEventListener("contextmenu", function (e) {
            e.preventDefault();
        }, false);
 
    </script>
    </form>
</body>
</html>
