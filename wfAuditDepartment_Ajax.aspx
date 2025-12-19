<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditDepartment_Ajax.aspx.vb"
    Inherits="Flypal.wfAuditDepartment_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Department</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet">
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
                <asp:UpdatePanel ID="upnlAuditDepartment" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Department [New]</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ErrorMessage="Department Name Required" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ErrorMessage="Department Name should not be greater than 50 characters" ControlToValidate="txtName"
                                        ClientValidationFunction="validateNameLen"></asp:CustomValidator>
                                    <script type="text/javascript">
                                        function validateNameLen(source, args) {
                                            args.IsValid = false;
                                            var nameLength = $get("txtName").value.length;
                                            if (nameLength <= 50) {
                                                args.IsValid = true;
                                                return;
                                            }

                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlDepartmentDet" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsDepartmentdetail" class="clsFieldSet" style="border-width: 1px">
                                                <legend id="ldDepartmentdetail" runat="server"><b>Audit Department Details</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" Text="New" ToolTip="Click to add the new Department"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table id="Table2" border="0" cellspacing="1" cellpadding="1">
                                                                <tr>
                                                                    <td>
                                                                        <span id="Label2" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td style="width: 90px">
                                                                        <span id="lblName" class="clsLabelAuto">Name</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mDepartment.Name %>"
                                                                            ClientIDMode="Static" ToolTip="Enter Department" MaxLength="50">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save the Department Information">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Department List</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="dgDepartmentList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                        AllowSorting="true" ShowHeaderWhenEmpty="true">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="DepartmentID"></asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Department Name" SortExpression="Name"
                                                ItemStyle-Width="150px">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                Text="Close" ToolTip="Click to close Audit Department screen"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForAuditDept();
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
                    parent.IFrameAuditDeptStateComplete();
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
    </form>
</body>
</html>
