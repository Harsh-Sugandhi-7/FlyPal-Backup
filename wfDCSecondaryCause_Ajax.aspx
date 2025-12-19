<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDCSecondaryCause_Ajax.aspx.vb"
    Inherits="Flypal.wfDCSecondaryCause_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Delay/Cancellation Secondary Cause</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="1" />
                                                    <asp:RequiredFieldValidator ID="rfvShortcode" runat="server" ControlToValidate="txtshortcode"
                                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Short Code Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Description Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvDesc" runat="server" ClientValidationFunction="validateDescription"
                                                        ControlToValidate="txtDescription" CssClass="clsLabelAuto" Display="None" ErrorMessage="Description should not be greater than 200 characters."
                                                        ValidationGroup="1"></asp:CustomValidator>
                                                    <script type="text/javascript">
                                                        function validateDescription(source, args) {
                                                            var len = $("#txtDescription").val().length;
                                                            if (len > 200) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                        }
                                                    </script>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" class="clsFormHeader1Newstyle">
                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Delay/Cancellation Secondary Cause [New]</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblCityDetails" class="clsLabelHeader">Secondary Cause Details</span>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td colspan="3">
                                                    <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="New" ToolTip="Click to add the new D/C Secondary Cause" />
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td>
                                                    <span id="lblName1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblShortCode" class="clsLabelAuto">Short Code</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtshortcode" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                        Height="36px" MaxLength="15" Text="<%# mDCSecondaryCause.ShortCode %>">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblGMT1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mDCSecondaryCause.SecondaryCause %>"
                                                        ClientIDMode="Static" Height="48px" MaxLength="200" TextMode="MultiLine" ToolTip="Enter Description"
                                                        Width="348px">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td colspan="3">
                                                    <span id="lblSave" class="clsLabelAuto">Click to Save current record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save the D/C Secondary Cause Information"
                                                        ValidationGroup="1" />
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="dgSecondaryCauseList" runat="server" AllowPaging="True" AllowSorting="True"
                                                        AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID" PageSize="5"
                                                        ToolTip="Secondary Cause  List">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                            <asp:BoundField DataField="ShortCode" HeaderText="Short Code" SortExpression="ShortCode">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SecondaryCause" HeaderText="Description" SortExpression="SecondaryCause">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                           <%-- <asp:ButtonField CommandName="EditRec" HeaderText="Edit/View" Text="Edit/View">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditViewRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                    </td>
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
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                        Text="New" ToolTip="Click to add the new D/C Secondary Cause" />
                                                </td>
                                                <td>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to save the D/C Secondary Cause Information"
                                                            ValidationGroup="1" />
                                                    </td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" TabIndex="0" CssClass="clsbtnH clsinfoH1" runat="server" CausesValidation="False"
                                                        ToolTip="Click to close D/C Secondary Cause screen" Text="Close"></asp:Button>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForSecCauseMaster();
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
                parent.IFrameSecCauseMasterStateComplete();
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
        function ReSetPageLayout()
        {
        $("body,html").css({ 'background-color': 'transparent' });
            var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
            var windowheight=$(window).height();
            if (tempMargtop>=windowheight)
            {
            $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
            }
            else
            {
            var margintop=(windowheight/2)-(tempMargtop/2);
            $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
            }
       
        }
    </script>
    <%--End--%>
    </form>
</body>
</html>
