<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMaintenanceKitDetailMultipleItems_Ajax.aspx.vb"
    Inherits="Flypal.wfMaintenanceKitDetailMultipleItems_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <table class="clstablelistin" id="tblInner">
                    <tr>
                        <td colspan="4"  class="clsFormHeader1Newstyle">
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Maintenance Kit Item [New]</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
                                        CssClass="clsValidationSummary"></asp:ValidationSummary>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset id="fdsInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                <legend id="lblInfo" runat="server" style="font-weight: bold"><b>Enter Part No./Description
                                    to Search</b></legend>
                                <asp:UpdatePanel ID="upnlInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel"> Part No./Description </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
                                                                    ToolTip="Enter Part No. or Description to Search"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCat" runat="server" CssClass="clsLabel"> Category </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                                                    DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSearch" CssClass="clsbtnH clsinfoH1" runat="server" Text="Find Now"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlgrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblresult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtonsTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnSaveTop" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                        ToolTip="Click to add Kit Item(s)" Text="Save"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBackTop" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                        ToolTip="Click to go back to the previous page" Text="Back" CausesValidation="False">
                                                                    </asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                    <asp:GridView ID="dgItemsList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
                                                        ClientIDMode="Static" AllowSorting="true" DataKeyNames="ID" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server"></asp:CheckBox>
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server" onclick="CheckUncheck()">
                                                                    </asp:CheckBox>
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Qty." HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                <HeaderTemplate>
                                                                    <span class="clsdgHeader">Qty.</span>
                                                                    <asp:Button ID="imgRefreshQty" runat="server" CssClass="clsButtonGrid" Text="..."
                                                                        OnClick="txtQuantity_TextChanged" ToolTip="Click to Copy First Item Qty. to following row(s)"
                                                                        CausesValidation="False" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                        Text="0" ToolTip="Enter Quantity" MaxLength="8"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Note" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <span class="clsdgHeader">Note</span>
                                                                    <asp:Button ID="imgRefreshNote" runat="server" CssClass="clsButtonGrid" Text="..."
                                                                        OnClick="txtNote_TextChanged" ToolTip="Click to Copy First Item Note to following row(s)"
                                                                        CausesValidation="False" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="500"
                                                                        Width="220px" ToolTip="Enter Note" TextMode="MultiLine"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <span class="clsdgHeader">Remark</span>
                                                                    <asp:Button ID="imgRefreshRemark" runat="server" CssClass="clsButtonGrid" Text="..."
                                                                        OnClick="txtRemark_TextChanged" ToolTip="Click to Copy First Item Remark to following row(s)"
                                                                        CausesValidation="False" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="500"
                                                                        Width="220px" ToolTip="Enter Remark" TextMode="MultiLine"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <span id="lblSpareList" class="clsLabelHeader" runat="server">Spare's List for Maintenance
                                Activity</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlKitList" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                        <asp:GridView ID="dgKitList" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                            PageSize="3" ShowHeaderWhenEmpty="True">
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle HorizontalAlign="Right" />
                                            <RowStyle CssClass="clsdgItem" />
                                             <HeaderStyle CssClass="clsdgHeader nodrag nodrop" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Name" HeaderText="Part No.">
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to add Kit Item(s)"
                                                    Text="Save"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                    Text="Back" CausesValidation="False"></asp:Button>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForMaintenanceKit();
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
             parent.IFrameMaintenanceKitStateComplete();
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
    <script type="text/javascript">

        //        $("#chkSelectAll").click(function () {
        function CheckUncheck() {
            var status = $("#chkSelectAll").attr("checked");
            $("#dgItemsList tr:gt(0)").find(":checkbox").each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                    SetRow($(this));
                }
                else {
                    $(this).removeAttr("checked");
                    SetRow($(this));
                }

            });
        }

        //        });

        //        $("#chkSelectAll").change(function () {
        //            var checked = $(this).prop('checked');
        //            $('.cbSelectRow').prop('checked', checked).trigger('change');
        //        });


        function SetRow(elem) {
            var status = $(elem).attr("checked");
            if (status == "checked") {
                $(elem).closest("tr").addClass('HighLightRow');
            }
            else {
                $(elem).closest("tr").removeClass('HighLightRow');
            }
        }

    </script>
</body>
</html>
