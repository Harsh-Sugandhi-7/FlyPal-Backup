<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHangarPlanningHangarMaster.aspx.vb" Inherits="Flypal.wfHangarPlanningHangarMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Hangar</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="wfgroup" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1" 
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlHangarDetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Hangar [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rHangar" runat="server" CssClass="clsLabelAuto" ErrorMessage="Hangar Name Required"
                                                Display="None" ControlToValidate="txtHanger" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ValidationGroup="a" ID="cvHangarTypeID" runat="server" ErrorMessage="Select City from the list."
                                                ControlToValidate="cmbCity" Display="None" ClientValidationFunction="ValidateHangarList"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to add new Hangar in the list"
                                                Text="New" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblManufacturerDetails" runat="server" CssClass="clsLabelHeader">Hangar Details . . .</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="spGMT1" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="spName" class="clsLabelAuto">Hangar</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHanger" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Hanger Name"
                                                Text="<%# mHangerMaster.HHanger %>" MaxLength="20" Height="16px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="spName1" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="spGMT" class="clsLabelAuto">City</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbCity" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                DataTextField="Name" DataValueField="ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="Span1" class="clsLabelAuto">State:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblState" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="Span2" class="clsLabelAuto">Country:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblCountry" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnSave" ValidationGroup="a" runat="server" CssClass="clsButton_Ajax"
                                                ToolTip="Click to save the Hangar Information" Text="Save"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="dgHangerList" runat="server" AllowPaging="True" AutoGenerateColumns="False" AllowSorting ="true"
                                                CssClass="clsGrid" PageSize="10" ShowHeaderWhenEmpty="True">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" Visible="false" HeaderText="ID" />
                                                    <asp:BoundField DataField="HHanger" HeaderText="Hangar" SortExpression="HHanger">
                                                       <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap ="True" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HCity" HeaderText="City" SortExpression="HCity">
                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap ="True"  />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HState" HeaderText="State" SortExpression="HState">
                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HCountry" HeaderText="Country" SortExpression="HCountry">
                                                       <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="DeleteRec"
                                                                Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton"
                                                Text="Close" ToolTip="Click to close Hangar screen" />
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

        function ValidateHangarList(source, args) {
            args.IsValid = false;
            var dd = $get("cmbCity");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
           
    </script>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForHangerMaster();
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
             parent.IFrameHangerMasterStateComplete();
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
