<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUserList_Ajax.aspx.vb"
    Inherits="Flypal.wfUserList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
   
    <script type="text/javascript" id="clientEventHandlersJS">

        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
</head>
<body>
    <form id="frmUserList" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlUserList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="3" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblUserList" runat="server" 
                                                            CssClass="clsFormHeader">User List</asp:Label>
                                                    </td>

                                                    <td align="right">
                                                        <table id="Table2" class="clstableButton" align="right">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" CssClass="clsbtnH clsinfoH" 
                                                                        runat="server" ToolTip="Create a New User."
                                                                        Text="Add New" Enabled="<%# mUserList.count<30 %>" />
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnPrintBottom" CssClass="clsbtnH clsinfoH"
                                                                        runat="server" ToolTip="Print List of User."
                                                                        CausesValidation="False" Text="Print" />
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH"
                                                                        runat="server" ToolTip="Close User List screen."
                                                                        CausesValidation="False" Text="Close" />
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
                                            <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Width="648px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">User</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" CssClass="clsTextBoxTagSearch" 
                                                runat="server" ToolTip="Enter User Name "
                                                MaxLength="25"></asp:TextBox>
                                        </td>
                                        <td align="right"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRole" runat="server" CssClass="clsLabelAuto">Role</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbRoleList"
                                                CssClass="clsTextBoxTagSearchComboNewstyle"
                                                runat="server" DataValueField="RoleID"
                                                DataTextField="Name">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right">
                                            <table id="Table4" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="btnSearch" runat="server" 
                                                            ImageUrl="~/images/Search2.png" 
                                                            CssClass="clsSearch2btn" ToolTip="Search as per Criteria." />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto"
                                                Width="360px" Font-Bold="True">
                                                List of Users as per criteria: 0 Record(s) found.</asp:Label>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="left">
                                            <asp:GridView ID="dgUser" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CellPadding="5"
                                                CssClass="clsGridNewStyle" PageSize="25" ForeColor="Black" 
                                                ShowHeaderWhenEmpty="true" GridLines="Horizontal">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" Wrap="False" />
                                                <RowStyle CssClass="clsdgItem" Wrap="False" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="UserID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText="User Name">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RoleName" HeaderText="Role">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Create Copy" HeaderText="Create Copy"
                                                        CommandName="CreateCopy" ControlStyle-ForeColor="Blue" 
                                                        ItemStyle-HorizontalAlign="Center" >
                                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" 
                                                        HeaderText="Action" ItemStyle-HorizontalAlign="Center" 
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" 
                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                                                                    CommandName="EditView" ImageUrl="~/images/edit.png"
                                                                                    Style="height: 15px; width: 15px" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" 
                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                                                                    CommandName="Remove" ImageUrl="~/images/delete.png" 
                                                                                    Style="height: 20px; width: 20px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" 
                                                                    ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right"></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>
    </form>
</body>
</html>
