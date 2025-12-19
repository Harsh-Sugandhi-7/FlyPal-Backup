<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualLastRevAttachmentList_Ajax.aspx.vb"
    Inherits="Flypal.wfManualLastRevAttachmentList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Revision Attachments</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
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
                <table id="tblInner" class="clstablelistin">
                    <tr>
                        <td class="clsFormHeader1Newstyle">
                            <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Revision Attachments</asp:Label>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button CssClass="clsbtnH clsinfoH" ID="btnBack" runat="server" Text="Close"></asp:Button>
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
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                        DisplayMode="BulletList" ValidationGroup="a" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span id="lblRevisionDetails" class="clsLabelHeader">Revision Details</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlRevisionDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblNo" class="clsLabelAuto">No.</span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNo" runat="server" Text="<%# mRevision.No %>" CssClass="clsLabelHeader"> </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td width="70px">
                                                <span id="lblRevisionNo" class="clsLabelAuto">Revision No.</span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRevNo" runat="server" Text="<%# mRevision.RevNo %>" CssClass="clsLabelHeader"> </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                    <legend class="clsFieldSet1"><b>File Attachments</b></legend>
                                                    <asp:UpdatePanel ID="upnlManRevisionAttachment" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="dgManRevisionAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                            AllowPaging="False" AutoGenerateColumns="false">
                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                <asp:BoundField DataField="FileName" HeaderText="File Name">
                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
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
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <%--<td align="right">
                            <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnBack" runat="server" Text="Close"></asp:Button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>--%>
                    </tr>
                </table>
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
    <!--call parent function after completing subroutine..(when page open as popup)-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForManualRevisionAttach();
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
             parent.IFrameManualRevisionAttachStateComplete();
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
