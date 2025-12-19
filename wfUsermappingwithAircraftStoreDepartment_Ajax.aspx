<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUsermappingwithAircraftStoreDepartment_Ajax.aspx.vb"
    Inherits="Flypal.wfUsermappingwithAircraftStoreDepartment_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <title>User Mapping with Aircraft Store Department</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblTitle" class="clsFormHeader">User Mapping with Aircraft, Store, Department</span>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlTopButton">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSaveTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save"></asp:Button>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to Close"
                                                                    CausesValidation="false"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSaveBottom" EventName="click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCloseBottom" EventName="click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbAircraft" runat="server" Checked="True" CssClass="clsRadioButton"
                                                        GroupName="c" AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <span id="lblCategory" class="clsLabel">Aircraft</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                        DataTextField="RegNo" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbStore" runat="server" CssClass="clsRadioButton" GroupName="c"
                                                        AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <span id="lblStore" class="clsLabel">Store</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="LocationStore"
                                                        DataValueField="ID" AutoPostBack="True" Width="250px" Enabled="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbDepartment" runat="server" CssClass="clsRadioButton" GroupName="c"
                                                        AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <span id="lblDepartment" class="clsLabel">Department</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbDepartmentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                        AutoPostBack="True" Width="250px" DataTextField="Name" DataValueField="ID" Enabled="false">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlTopButton">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Save">
                                                    </asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                        CausesValidation="false"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveBottom" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCloseBottom" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel runat="server" ID="upnlMachine" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div style="width: 100%">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of User</asp:Label>
                                        </div>
                                        <div style="width: 100%">
                                            <asp:GridView ID="dgUserList" runat="server"  Width="100%" AutoGenerateColumns="False"
                                                ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="100" AllowSorting="True"
                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectUser" ClientIDMode="Static" runat="server"
                                                                Text="Select" onclick="CheckUncheck(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" ClientIDMode="Static" CssClass="clsCheckBox"
                                                                Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelected") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="UserName" HeaderText="User">
                                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UserID" HeaderText="UserID" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <!--End-->
                        </tr>
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSaveBottom" runat="server" CssClass="clsButton_Ajax" Text="Save" Visible="false">
                                            </asp:Button>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnCloseBottom" runat="server" CausesValidation="false" CssClass="clsButton_Ajax" Visible="false"
                                                Text="Close"></asp:Button>
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
    <script type="text/javascript">
        function CheckUncheck(chkBoxAll) {
            var str = chkBoxAll.id;
            var status = $("#chkSelectUser").attr("checked");
            $("#dgUserList tr:gt(0)").find(":checkbox").each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                }
                else {
                    $(this).removeAttr("checked");
                }
            });
        }
    </script>
</body>
</html>
