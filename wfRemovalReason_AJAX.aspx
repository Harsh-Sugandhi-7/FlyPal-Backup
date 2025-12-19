<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemovalReason_AJAX.aspx.vb"
    Inherits="Flypal.wfRemovalReason_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Removal Reason</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="wfgroup" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <asp:UpdatePanel ID="upnlRemovalReason" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td class="clsFormHeader1" colspan="4">



                                                <%--<asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Removal Reason [New]</asp:Label>--%>


                                                <table width="100%">
                                                    <tr>
                                                        <td align="Left">
                                                            <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Removal Reason [New]</asp:Label>
                                                        </td>
                                                        <td align="Right">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <%--<asp:Button ID="Button1" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Term"
                                                                                Text="New" CausesValidation="False"></asp:Button>--%>


                                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Removal Reason in the list"
                                                                            Text="New" CausesValidation="False"></asp:Button>


                                                                    </td>
                                                                    <td>


                                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to save the Removal Reason Information"
                                                                            Text="Save"></asp:Button>

                                                                    </td>
                                                                    <td>

                                                                        <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Removal Reason screen"
                                                                            Text="Close" CausesValidation="False"></asp:Button>

                                                                        <%--                                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                                Text="Close" CausesValidation="False"></asp:Button>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>







                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelauto" Display="None"
                                                    ControlToValidate="txtReason" ErrorMessage="Name Required"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="Removal Reason Name should not be greater than 250 Characters"
                                                    ClientValidationFunction="ValidateName" Display="None"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <%--<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Removal Reason in the list"
                                                Text="New" CausesValidation="False"></asp:Button>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblRemovalReasonDetails" runat="server" CssClass="clsLabelHeader1">Removal Reason Details</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Name</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReason" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mRemovalReason.Name %>" ClientIDMode="Static"
                                                    TextMode="MultiLine" ToolTip="Enter Reason" Width="500px" MaxLength="200">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <%--<asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to save the Removal Reason Information"
                                                Text="Save"></asp:Button>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="dgRemovalReason" runat="server" AllowSorting="True" AllowPaging="true" AutoGenerateColumns="False"
                                                    CellPadding="5" GridLines="Horizontal" PageSize="05" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="true"
                                                    DataKeyNames="Id">

                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                        NextPageText="" PreviousPageText="" />
                                                    <%--<PagerStyle HorizontalAlign="Right" CssClass="paging" />--%>

                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />

                                                    <RowStyle CssClass="clsdgItem" />

                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />





                                                    <Columns>
                                                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="False"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Reason" SortExpression="Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="true" />
                                                        </asp:BoundField>


                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <%--11--%>
                                                            <ItemTemplate>
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="EditRec" Style="height: 15px; width: 15px"
                                                                                        ImageUrl="~/images/edit.png" CausesValidation="false" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="DeleteRec" Style="height: 20px; width: 20px"
                                                                                        ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                                </td>
                                                                            </tr>

                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                        Style="cursor: pointer" />
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
                                            <td colspan="4" align="right">
                                                <%--<asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close RemovalReason screen"
                                                Text="Close" CausesValidation="False"></asp:Button>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
        </div>
        <script type="text/javascript">

            function ValidateName(source, args) {
                args.IsValid = false;
                var Nametxt = document.getElementById("txtReason").value;
                var len = Nametxt.length;
                if (len < 250) {
                    args.IsValid = true;
                    return;
                }
            }
        </script>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForRemovalReason();
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
                    parent.IFrameRemovalReasonStateComplete();
                }
       
      
        });
        <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
                    
            }

            function SetPageLayout()
            {
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
