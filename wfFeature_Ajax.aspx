<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFeature_Ajax.aspx.vb"
    Inherits="Flypal.wfFeature_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Feature</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
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
                        <asp:UpdatePanel runat="server" ID="pnlValidationSummary" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Feature [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Feature Required"
                                                Display="None" ControlToValidate="txtFeatureName" ValidationGroup="a"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="New"
                                                ToolTip="Click to add the new Feature" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="upnlFeatureDetails" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblFeatureDetails" class="clsLabelHeader">Feature Details</span>
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
                                                        <asp:TextBox ID="txtFeatureName" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mFeature.Name %>"
                                                            ToolTip="Enter Feature Name" MaxLength="50" >
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
                                            <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" Text="Save" ToolTip="Click to save the Feature Information"
                                                ValidationGroup="a"></asp:Button>
                                        </td>
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
                                            <asp:GridView ID="dgFeature" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                ShowHeader="true" CssClass="clsGrid" EnableViewState="False" PagerSettings-Mode="NumericFirstLast"
                                                PageSize="10">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle HorizontalAlign="Right" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                    <asp:BoundField DataField="Name" HeaderText="Feature" SortExpression="Name">
                                                        <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="205px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="left" Width="205px" Wrap="true" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="EditView" HeaderText="Edit/View" Text="Edit/View">
                                                        <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="left" Width="70px" Wrap="true" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField CommandName="Remove" CausesValidation="false" HeaderText="Delete"
                                                        Text="Delete">
                                                        <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="left" Width="50px" Wrap="true" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnClose" CssClass="clsButton" runat="server" Text="Close" ToolTip="Click to close Feature screen"
                                                CausesValidation="False"></asp:Button>
                                        </td>
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

     <div>
        <!-- FeatureMaster Popup Window -->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForFeature();
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
             parent.IFrameFeatureMasterStateComplete();
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
    </div>
    </form>
</body>
</html>
