<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualProperty_Ajax.aspx.vb"
    Inherits="Flypal.wfManualProperty_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manual Property</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
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
                        <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                               <tr>
                                                   <td>
                                                       <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Manual Property [New]</asp:Label>
                                                   </td>
                                                   <td align="right">
                                                          <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>

                                                             <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnAdd" TabIndex="0" runat="server" Text="New"
                                                                         ToolTip="Click to add new Manual Property" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnSave" runat="server" Text="Save" ToolTip="Click to save the Manual Property Information"
                                                                            ValidationGroup="a"></asp:Button>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close" ToolTip="Click to close Manual Property screen"
                                                                            CausesValidation="False"></asp:Button>
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
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                ErrorMessage="Name Required." Display="None" ValidationGroup="a" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblAdd" class="clsLabelAuto"></span>
                                        </td>
                                        <%--<td align="right">
                                            <asp:Button CssClass="clsbtnH clsinfoH" ID="btnAdd" TabIndex="0" runat="server" Text="New"
                                                ToolTip="Click to add new Manual Property" CausesValidation="False"></asp:Button>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="upnlManualPropertyDetails" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblManualPropertyDetails" class="clsLabelHeader">Manual Property Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblNameStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblName" class="clsLabel">Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtName" runat="server"  ToolTip="Enter Manual Property Name"
                                                            MaxLength="50" AutoComplete="off">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span id="lblSave" class="clsLabelAuto"></span>
                                        </td>
                                        <%--<td align="right">
                                            <asp:Button CssClass="clsbtnH clsinfoH" ID="btnSave" runat="server" Text="Save" ToolTip="Click to save the Manual Property Information"
                                                ValidationGroup="a"></asp:Button>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="dgManualPropertyList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                PagerSettings-Mode="NumericFirstLast"
                                                PageSize="25">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField CommandName="EditView" HeaderText="Edit/View" Text="Edit/View">
                                                        <HeaderStyle HorizontalAlign="Left" Width="10px" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField CommandName="Remove" HeaderText="Delete" Text="Delete">
                                                        <HeaderStyle HorizontalAlign="Left" Width="10px" />
                                                    </asp:ButtonField>--%>



                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandName="EditView" ToolTip="Click to edit"
                                                                                CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="Remove" ToolTip="Click to delete"
                                                                                CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                        <%--<td align="right">
                                            <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close" ToolTip="Click to close Manual Property screen"
                                                CausesValidation="False"></asp:Button>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <!--call parent function after completing subroutine..(when page open as popup)-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForManualProperty();
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
             parent.IFrameManualPropertyStateComplete();
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
