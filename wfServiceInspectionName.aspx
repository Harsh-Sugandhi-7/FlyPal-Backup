<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfServiceInspectionName.aspx.vb" Inherits="Flypal.wfServiceInspectionName" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Service Inspection List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" ms_positioning="GridLayout" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="wfserviceinspection" method="post" runat="server">
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">

                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span class="clsFormHeader" width="660px">Service Inspection Name List</span>

                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlAddBottom" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddNew" runat="server" CausesValidation="true" ValidationGroup="1" CssClass="clsbtnH clsinfoH"
                                                                        Text="Add" ToolTip="Click to Add New Service Inspection" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                        Text="Close" ToolTip="Click to close Service Inspection List screen" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                            <asp:CustomValidator ValidationGroup="1" ID="cvDescription" runat="server" Display="None"
                                                ErrorMessage="Service Inspection Name Required." ControlToValidate="txtserviceInspection"
                                                OnServerValidate="customvalidate" CssClass="clsLabel"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ValidationGroup="1" ID="rfvDescription" runat="server"
                                                CssClass="clsLabel" Display="None" ErrorMessage="Service Inspection Name Required"
                                                ControlToValidate="txtserviceInspection"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlServiceInspectionNameDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">

                                                <tr>
                                                    <td>
                                                        <span id="lblFor" class="clsLabel">Service/Inspection Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtserviceInspection" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                            ToolTip="Enter service Inspection Name" Width="180px" CausesValidation="true"></asp:TextBox>
                                                    </td>
                                                    <%--<td align="right">
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                            <asp:Button ID="btnAddNewTop" runat="server"   ValidationGroup="1" CausesValidation="true" CssClass="clsButton_Ajax"
                                                              Text="Save" ToolTip="Click to Add New Service Inspection" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>--%>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGridViewTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Service Inspection as per criteria : Record(s) found.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgServiceInspectionNameList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                DataKeyNames="ID" ForeColor="Black" GridLines="Horizontal" PageSize="10" ShowHeaderWhenEmpty="true"
                                                PagerSettings-Mode="NumericFirstLast" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="white" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="Id" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="ServiceInspectionName" HeaderText="Service Inspection Name" SortExpression="ServiceInspectionName">
                                                        <HeaderStyle HorizontalAlign="Left" ForeColor="black" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--   <asp:ButtonField CommandName="EditRec" HeaderText="Edit/View" Text="Edit/View">
                                                        <HeaderStyle Width="10px" HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                        <HeaderStyle Width="10px" HorizontalAlign="Left" />
                                                    </asp:ButtonField>--%>

                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditRec" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                    CausesValidation="false" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="Delete" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                    CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                    CausesValidation="false" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="white" ForeColor="Black" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForServiceInspactionName();
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
                    parent.IFrameServiceInspactionNameStateComplete();
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
