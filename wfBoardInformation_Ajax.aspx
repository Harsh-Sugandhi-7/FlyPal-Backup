<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfBoardInformation_Ajax.aspx.vb"
    Inherits="Flypal.wfBoardInformation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Board Information</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout" class="formBGColor">
    <form id="form1" runat="server" >
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="3">
                                    <fieldset id="fdsAircraftRegInfo" class="clsFieldSet" style="border-width: 1px">
                                        <legend id="lblBoardListInfo" style="font-weight: bold"><b>Board Information Details</b></legend>
                                        <table id="Table4">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblDocumentStar1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblBoardType" class="clsLabelAuto">Board Type</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbBoardType" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                                    DataValueField="ID">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlAddInfo" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" Text="Add" ToolTip="Click to Add">
                                                            </asp:Button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgBoardInfoList" runat="server" CssClass="clsGrid" ToolTip="Board Information List"
                                                            ShowHeaderWhenEmpty="true" AllowSorting="True" AutoGenerateColumns="False">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <RowStyle CssClass="clsdgAltItem TextBreak"></RowStyle>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No." SortExpression="SrNo">
                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="BoardTypeName" SortExpression="BoardTypeName" HeaderText="Board Type">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DueOnValue" HeaderText="Due At" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="false" />
                                                                    <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
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
                            <tr>
                                <td colspan="4" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save the current record.">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to the previous page"
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    </form>
    <%--  Call parent AutoResize function to resize the form--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeBoardInfoList();
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
    </script>
    <%--Called parent function to open Select Info Board page--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentSelectInfoBoardFunction() {
            window.parent.OpenSelectInfoBoardWindow();
        }
    </script>
</body>
</html>
