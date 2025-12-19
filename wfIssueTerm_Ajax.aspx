<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueTerm_Ajax.aspx.vb"
    Inherits="Flypal.wfIssueTerm_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Issue Terms</title>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlTermDetails" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table class="clstablelistin" id="tblLedgerList">
                                <tr>
                                    <td colspan="5" class="clsFormHeader1Newstyle">
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">List Of Issue Terms</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" >
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblNewTerm" class="clsLabelAuto">Add New Term : </span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlAddNewTerm" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                          
                                                                <asp:ImageButton ID="imgbtnTerm" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                    Width="24px" ToolTip="Click to Add New Term" CausesValidation="False"></asp:ImageButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:GridView ID="dgTerm" runat="server"  CellPadding="5" GridLines="Horizontal" CssClass="clsGridNewStyle" PageSize="25" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="true">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Terms" HeaderText="Term">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="500px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="5">
                                        <table align="right">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH1" Text="Ok"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                        Text="Back"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
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
            //            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#imgbtnTerm").live("click", function () {
                try {
                    //                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupTerm").attr("src", "wfTerm_Ajax.aspx?Type=pup&OpenFrom=2");
                    if (!$.browser.msie) {
                        $("#btnDummyTerm").click();
                        //                        $get("AjaxLoader").style.visibility = "hidden";
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
