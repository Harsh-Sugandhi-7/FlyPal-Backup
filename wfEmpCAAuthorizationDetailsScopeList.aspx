<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmpCAAuthorizationDetailsScopeList.aspx.vb" Inherits="Flypal.wfEmpCAAuthorizationDetailsScopeList" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">

    <form id="wfgroup" method="post" runat="server">
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td class="clsFormHeader1 clsFormHeaderTD">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">SCOPE OF AUTH./CODE</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right">
                                                        <%--<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="New"
                                                            ToolTip="Click to add the new Designation" CausesValidation="true"></asp:Button>--%>
                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Done" ToolTip="Click to add the Scope of Authorization Code"
                                                            ValidationGroup="1"></asp:Button>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                            TabIndex="0" Text="Close" ToolTip="Click to Close Scope of Authorization Code Screen" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" Width="440px" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlScope" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                              
                                                <tr>
                                                    <td>
                                                        <div style="height:100vh; overflow: auto;">
                                                            <asp:GridView ID="dgScopeList" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" DataKeyNames="ID"
                                                                AutoGenerateColumns="False" CellPadding="10" Style="overflow: auto;" ForeColor="Black" GridLines="Horizontal">

                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                <Columns>
                                                                    <%--0--%>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" id="chkSelectAll" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                                <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <%--1--%>
                                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                    <%--2--%>
                                                                    <asp:BoundField DataField="AuthorizationCode" HeaderText="Authorization Code">
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <%--3--%>
                                                                    <asp:BoundField DataField="Scope" HeaderText="Scope">
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundField>

                                                                </Columns>
                                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                                            </asp:GridView>

                                                        </div>
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



        <div>
            <%--call parent function after completing subroutine..(when page open as popup)--%>
            <script type="text/javascript">
                function CallParentCallback() {
                    parent.ParentCallBackFunctionForAuthorizationDetail();
                }
            </script>
            <%--End--%>
            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameStateComplete();
                    }


                });
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
                function endRequestHandler() {
                    SetPageLayout();
                }

                function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                    ReSetPageLayout();
                 //  onResize();//for Top bottom link
           <% End if %>
                }
                function ReSetPageLayout() {
                    $("body,html").css({ 'background-color': 'transparent' });
                    var tempMargtop = $("body #tblmain:eq(0)").outerHeight(true);
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
        </div>
        <script type="text/javascript">


            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.cbSelectRow').change(function () {
                    // detect if the checkbox is checked
                    var checked = $(this).prop('checked');
                    // gets the table row indiect parent
                    var trParent = $(this).closest('tr');
                    // add or remove the css class according to the check state
                    if (checked == true)
                        trParent.addClass('clslightColor')
                    else
                        trParent.removeClass('clslightColor');
                })
                    // the each is used when postback is triggered with checked rows
                    .each(function (index, element) {
                        var checked = $(element).prop('checked');
                        if (checked == true)
                            $(element).closest('tr').addClass('clslightColor');
                        else
                            $(element).closest('tr').removeClass('clslightColor');
                    });
                // select all click
                $("#chkSelectAll").change(function () {
                    var checked = $(this).prop('checked');
                    $('.cbSelectRow').prop('checked', checked).trigger('change');
                });

            });

        </script>
        <!-- End-->
    </form>
</body>
</html>
