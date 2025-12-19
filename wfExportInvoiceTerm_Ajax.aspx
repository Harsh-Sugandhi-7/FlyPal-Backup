<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExportInvoiceTerm_Ajax.aspx.vb"
    Inherits="Flypal.wfExportInvoiceTerm_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Export Invoice Terms</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
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
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table class="clstablelistin" id="tblLedgerList">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblListExportInvoiceTerms" class="clsFormHeader">List Of Export Invoice Terms</span>
                                            </td>
                                            <td align="right">
                                                <table align="right">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlAddNewTerm" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <span id="lblNewTerm" class="clsLabelAuto">Add New Term : </span>
                                            <%--<asp:Button ID="imgbtnTerm" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                CausesValidation="False" ToolTip="Click to Add New Term"></asp:Button>--%>

                                            <asp:ImageButton ID="imgbtnTerm" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                            ToolTip="Click to Add New Term" CausesValidation="False"></asp:ImageButton>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTerm" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTerm" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="25" ShowHeaderWhenEmpty="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Terms" HeaderText="Terms">
                                                        <ItemStyle Width="500px" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="right">
                                    <table align="right">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" Text="Ok"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <!-- Term Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyTerm" Text="Dummy Term" ClientIDMode="Static" />
        <asp:Button ID="hdnimgBtnTerm" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
            Style="display: none;"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlPopupTerm" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupTerm" frameborder="0" allowtransparency="true" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupTerm" runat="server" TargetControlID="btnDummyTerm"
        PopupControlID="pnlPopupTerm" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameTermStateComplete() {
            $("#btnDummyTerm").click();
        }
        $(document).ready(function () {
            $("#imgbtnTerm").live("click", function () {
                try {
                     $("#iPopupTerm").attr("src", "wfTerm_Ajax.aspx?Type=pup&OpenFrom=10");
                    if (!$.browser.msie) {
                        $("#btnDummyTerm").click();
                     }

                    return false;
                } catch (e) {
                    alert(e);
                }


            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForTerm() {
            var TermWindow = $find("<%=mdlPopupTerm.ClientID %>");
            //close Term popup window
            TermWindow.hide();
            $("#iPopupTerm").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnTerm").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
