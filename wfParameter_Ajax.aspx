<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfParameter_Ajax.aspx.vb"
    Inherits="Flypal.wfParameter_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Parameter</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlParameterDetails" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Parameter [New]</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                        ValidationGroup="1"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLable" ControlToValidate="txtName"
                                        Display="None" ErrorMessage="Name Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDescription"
                                        Display="None" ErrorMessage="Description Should not be greater than 150 chars."
                                        ClientValidationFunction="validateDesc" ValidationGroup="1"></asp:CustomValidator>
                                    <script type="text/javascript">
                                        function validateDesc(source, args) {
                                            var Value = $get("txtDescription").value.length;
                                            if (Value > 150) {
                                                args.IsValid = false;
                                                return
                                            }
                                        }
                                    </script>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Parameter"
                                        Text="New" CausesValidation="False"></asp:Button>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="2">
                                    <span id="lblPartDetails" class="clsLabelHeader">Parameter Details</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <span id="lblStar" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblName" class="clsLabelAuto">Name</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Parameter name"
                                                    Text="<%# mParameter.Name %>" MaxLength="50">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblDescription" class="clsLabelAuto">Description</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                    ToolTip="Enter Description" Text="<%# mParameter.Description %>" MaxLength="150"
                                                    Width="185px" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                          <%--  <tr>
                                <td>
                                    <span id="lblSave" class="clsLabelAuto">Click to Save current record</span>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to save the Parameter Information"
                                        Text="Save" ValidationGroup="1"></asp:Button>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="dgParameter" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="List of parts."
                                     DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True" AutoGenerateColumns="False">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                DataField="ID" HeaderText="Id"></asp:BoundField>
                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                           <%-- <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
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
                                                                        <asp:ImageButton ID="EditView" runat="server" CommandName="EditRec" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"  ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRec" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"  ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
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
                            <tr>
                                <td colspan="2" align="right">
                                    <table>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to add new Parameter"
                                                    Text="New" CausesValidation="False"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to close Parameter screen"
                                                    Text="Close" CausesValidation="False"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to save the Parameter Information"
                                                    Text="Save" ValidationGroup="1"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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
            parent.ParentCallBackFunctionForParameter();
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
                    parent.IFrameParameterStateComplete();
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
