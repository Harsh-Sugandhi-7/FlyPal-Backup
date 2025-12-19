<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWPFunctionMaster_AJAX.aspx.vb"
    Inherits="Flypal.wfCWPFunctionMaster_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title></title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlFunction" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Function [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelauto" Display="None"
                                                ControlToValidate="txtName" ErrorMessage="Name Required"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="Function Name should not be greater than 50 characters."
                                                ClientValidationFunction="ValidateName" Display="None"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelAuto" Text="Click To Add New Record"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to add new Function in the list"
                                                Text="New" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblFunctionDetails" runat="server" CssClass="clsLabelHeader1">Function Details</asp:Label>
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
                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax"
                                                ToolTip="Enter Function's Name" Text="" MaxLength="50" TextMode="MultiLine">
                                            </asp:TextBox><%--"<%# mFunction.Name %>"--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSave" CssClass="clsButton" runat="server" ToolTip="Click to save the Function Information"
                                                Text="Save"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="dgFunction" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="clsGrid" PageSize="25" ShowHeaderWhenEmpty="true">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                    NextPageText="" PreviousPageText="" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText="Function" SortExpression="Name">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                CommandName="ViewRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                CausesValidation="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                CausesValidation="false" />
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
                                            <asp:Button ID="btnClose" CssClass="clsButton" runat="server" ToolTip="Click to close Function screen"
                                                Text="Close" CausesValidation="False"></asp:Button>
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
    <script type="text/javascript">

        function ValidateName(source, args) {
            args.IsValid = false;
            var Nametxt = document.getElementById("txtName").value;
            var len = Nametxt.length;
            if (len <= 50) {
                args.IsValid = true;
                return;
            }
        }
    </script>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunction();
            return false;
        }
    </script>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameFunctionStateComplete();
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <%--End--%>
    </form>
</body>
</html>
