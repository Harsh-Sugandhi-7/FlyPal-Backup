<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRoleList_Ajax.aspx.vb"
    Inherits="Flypal.wfRoleList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Role List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server" method="post">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:msgbox id="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="3" class="clsFormHeader1Newstyle">

                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Role List</asp:Label>
                                                                </td>


                                                                <td align="right">
                                                                    <table id="Table2" border="0" cellpadding="1">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAdd" CssClass="clsbtnH clsinfoH" TabIndex="0" runat="server" ToolTip="Click to add the new role."
                                                                                    Text="Add New" CausesValidation="False"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Role List screen"
                                                                                    Text="Close" CausesValidation="False"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                            </tr>
                                                        </table>

                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto" Width="544px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRole" runat="server" CssClass="clsLabel" Width="24px">Role</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFind" CssClass="clsTextBoxTagSearch" runat="server" ToolTip="Enter Role"
                                                            MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td align="right">
                                                        <table id="Table3" border="0" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Find the list of Role."
                                                                        Text="Find Now"></asp:Button>--%>
                                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlRoleList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbllistroles" runat="server" CssClass="clsLabelHeader">List of Roles as per Criteria: 0 Record(s) found</asp:Label>
                                                    </td>
                                                    <%--<td align="right">
                                                        <table id="Table2" border="0" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" CssClass="clsbtnH clsinfoH" TabIndex="0" runat="server" ToolTip="Click to add the new role."
                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Role List screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgRoleList" runat="server" ToolTip="Role List."
                                                           GridLines="Horizontal" CellPadding="5" CssClass="clsGridNewStyle" AutoGenerateColumns="False" PageSize="25" AllowPaging="True" ShowHeaderWhenEmpty="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" Wrap="False" />
                                                            <RowStyle CssClass="clsdgItem" Wrap="False" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Wrap="False" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="RoleID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="Name" HeaderText="Role Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Create Copy" HeaderText="Create  Copy" CommandName="CreateCopyLnk">
                                                                </asp:ButtonField>
                                                               <%-- <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditLnk">
                                                                    <HeaderStyle Width="20px" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteLnk">
                                                                    <HeaderStyle Width="20px" />
                                                                </asp:ButtonField>--%>


                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditLnk" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteLnk" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                        </td>
                                                                                        <%--<td>
                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditView" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                        </td>--%>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>


                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <table id="Table1" border="0" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add the new role."
                                                                        Text="Add New" CausesValidation="False" Visible="false"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCl" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Role List screen"
                                                                        Text="Close" CausesValidation="False" Visible="false"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="dgRoleList" EventName="RowCommand" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
            runat="server">
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
    </div>
    </form>
</body>
</html>
