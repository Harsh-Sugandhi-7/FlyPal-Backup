<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLinkMaintenanceActivity_Ajax.aspx.vb"
    Inherits="Flypal.wfLinkMaintenanceActivity_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Link Maintenance Activity</title>
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
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table id="tblinner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Link Maintenace Activity</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvAction" runat="server" Display="None" ControlToValidate=""
                                                CssClass="clsLabelAuto" OnServerValidate="CustomValidate3"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlLinkedMaintenanceList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlinner" runat="server" CssClass="clsCalPanel" Wrap="true">
                                            <table id="Table4" width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblActivityType" class="clsLabelAuto">Select Type </span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBox2_Ajax">
                                                           <%-- <asp:ListItem Value="1">Service</asp:ListItem>
                                                            <asp:ListItem Value="2">Inspection</asp:ListItem>
                                                            <asp:ListItem Value="3">Directive</asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnAddNewLinkMaintenance" runat="server" CssClass="clsButton_Ajax"
                                                            ToolTip="Click to add new Link Maintenance Activity" Text="Add New" CausesValidation="False"
                                                            Visible="True"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:GridView ID="dgLinkedMaintenanceList" runat="server" CssClass="clsGridLog" AutoGenerateColumns="False"
                                                            AllowSorting="True" ShowHeaderWhenEmpty="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="LinkedMaintenanceTypeName" HeaderText="Linked with">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Task No./Code/Form No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="MonitorType" SortExpression="MonitorType"
                                                                    HeaderText="Monitor Type">
                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA Chapter">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DirectiveNo" SortExpression="DirectiveNo" HeaderText="Directive Number">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="cmbLinkMaintActionlist" runat="server" CssClass="clsComboBox_Ajax"
                                                                            DataTextField="ActionType" DataValueField="ID" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"MaintenanceActionID") %>'
                                                                            DataSource="<%# mLinkMaintenanceActionList %>">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remark">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemark" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Remark") %>'
                                                                            TextMode="MultiLine" CssClass="clsTextBoxMultiLine1_Ajax" Width="250px">
                                                                        </asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
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
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Link Maintenace Activity"
                                                        Text="Save" CausesValidation="true"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                        Text="Back" CausesValidation="False"></asp:Button>
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
    <!--call parent function after completing subroutine..(when page open as popup)-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForLinkMaintActivity();
            return false;
        }
    </script>
    <!--Set page layout when open as popup aspx page-->
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameLinkMaintActivityStateComplete();
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
    <!-- End-->
    </form>
</body>
</html>
