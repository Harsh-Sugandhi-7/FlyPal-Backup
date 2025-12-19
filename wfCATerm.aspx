<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCATerm.aspx.vb" Inherits="Flypal.wfCATerm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List of Authorization Terms</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

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
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <td class="clsFormHeader1">
                                                <span id="lblListQuotation" class="clsFormHeader">List of Authorization Terms</span>
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlAddNewTerm" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <span id="lblNewTerm" class="clsLabelAuto">Add New Term : </span>
                                                <%--<asp:Button ID="imgbtnTerm" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                    CausesValidation="False" ToolTip="Click to Add New Term"></asp:Button>--%>

                                                <asp:ImageButton ID="imgTermsAdd" runat="server" CausesValidation="true"
                                                    Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add Authorization Terms"
                                                    Width="24px" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlTerm" UpdateMode="Conditional">
                                            <ContentTemplate>

                                                <asp:GridView ID="dgTerm" runat="server" CellPadding="10" GridLines="Horizontal"
                                                    CssClass="clsGridNewStyle" AllowSorting="True" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                                    DataKeyNames="ID">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                            <%--11--%>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
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
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table align="right">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnOK" runat="server" class="clsbtnH clsinfoH1" Text="Ok"></asp:Button>
                                                            <asp:Button ID="hdnimgBtnTerm" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
                                                                Style="display: none;"></asp:Button>
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
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <!-- Term Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTerm" Text="Dummy Term" ClientIDMode="Static" />

        </div>
        <asp:Panel runat="server" ID="pnlPopupTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
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
                $("#imgTermsAdd").live("click", function () {
                    try {
                        //                    $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupTerm").attr("src", "wfTerm_Ajax.aspx?Type=pup&OpenFrom=11");
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForTerm();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameTermStateComplete();
                }
            });
        <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }



            function SetPageLayout() {
                <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
                <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }
            }
        </script>
        <%--End--%>
    </form>
</body>
</html>
