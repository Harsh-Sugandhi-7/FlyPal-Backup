<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditDesignation_Ajax.aspx.vb"
    Inherits="Flypal.wfAuditDesignation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Designation</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlAuditDesignation" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="clstablelistin" id="tblInner">
                                <tr>

                                    <td class="clsFormHeader1">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Designation [New]</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click to add the new Designation"
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save the Designation Information"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                            Text="Close" ToolTip="Click to close Audit Designation screen"></asp:Button>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                            Display="None" ErrorMessage="Designation Name should not be greater than 100 characters."
                                            ClientValidationFunction="validateNameLen"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                            Display="None" ErrorMessage="Designation Name Required"></asp:RequiredFieldValidator>
                                        <script type="text/javascript">
                                            function validateNameLen(source, args) {
                                                args.IsValid = false;
                                                var nameLength = $get("txtName").value.length;
                                                if (nameLength <= 100) {
                                                    args.IsValid = true;
                                                    return;
                                                }

                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset id="fdswodetail" class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="ldwodetail" runat="server"><b>Designation Details</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <%-- <td align="right" colspan="3">
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" Text="New" ToolTip="Click to add the new Designation"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblName1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td style="width: 100px">
                                                        <span id="lblName" class="clsLabelAuto">Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mDesignation.Name %>"
                                                            ToolTip="Enter Designation" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <%-- <td align="right" colspan="2">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save the Designation Information"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Designation List</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgDesignationList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="true" CellPadding="5" GridLines="Horizontal">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                           <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="DepartmentID"></asp:BoundField>
                                                <asp:BoundField DataField="Name" HeaderText="Designation Name" ItemStyle-Width="150px">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
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
                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" CausesValidation="false"/>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false"/>
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
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                    Text="Close" ToolTip="Click to close Audit Designation screen"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
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
                parent.ParentCallBackFunctionForDesignation();
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
                    parent.IFrameDesignationStateComplete();
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
    </form>
</body>
</html>
