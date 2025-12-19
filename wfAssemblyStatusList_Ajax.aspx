<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAssemblyStatusList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfAssemblyStatusList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="MSGBox.ascx" TagPrefix="uc2" TagName="MSGBox" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Assembly List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

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
<body class="formBGColor">
    <form id="form2" runat="server" >
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:Label ID="lblAssemblyListInfo" runat="server" CssClass="clsLabelAuto">List of all the  Assemblies on the Aircraft as of Date : [As of Date] . The Time Since New values of all the Assemblies will be as of Date : [As On Date]</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsComboBox2_Ajax" DataValueField="ID"
                                                            DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" Text="Add New" ToolTip="Click to Add the Assembly"
                                                            CssClass="clsButton_Ajax"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAssemblyStatusDetails" runat="server" CssClass="clsLabelHeader">List of Assemblies.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgAssemblyStatusList" runat="server" AllowPaging="False" AllowSorting="True"
                                                            ToolTip="Assembly Status List" AutoGenerateColumns="False" CssClass="clsGrid"
                                                            EnableViewState="True" ShowHeaderWhenEmpty="True">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AssemblyTypeName" SortExpression="AssemblyTypeName" HeaderText="Type">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ManufacturerName" SortExpression="ManufacturerName" HeaderText="Manufacturer" HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle ForeColor="White" Width="5px" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Position" SortExpression="Position" HeaderText="Pos.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HeaderText="Period"
                                                                    HtmlEncode="False">
                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrentValueFormatted" SortExpression="CurrentValueFormatted"
                                                                    HeaderText="Current" HtmlEncode="False">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InstallationValueFormatted" SortExpression="InstallationValueFormatted"
                                                                    HeaderText="At Inst." HtmlEncode="False">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>' 
                                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                    <ItemStyle CssClass="hideGridColumn" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPrint" TabIndex="0" runat="server" Text="Print" ToolTip="Click to Print the list of Assemblies"
                                                            CssClass="clsButton_Ajax"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" Text="Back" ToolTip="Click to go back to the previous page"
                                                            CssClass="clsButton_Ajax"></asp:Button>
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
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
    <%--  Call parent AutoResize function to resize the form--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeAssemblyList();
        }
        function OpenOverFrame() {
            window.open('wfAssemblyStatus_Ajax.aspx', '_parent');
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
    </script>
</body>
</html>
