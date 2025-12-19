<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReplaceCategory_Ajax.aspx.vb"
    Inherits="Flypal.wfReplaceCategory_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Replace Category</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1" EnablePageMethods="true">
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
                        <table width="100%">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle" class="clsFormHeader">Replace Category</span>
                                            </td>
                                          <%--  <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnReplace" runat="server" CssClass="clsbtnH clsinfoH" Text="Replace" ToolTip="Click to Replace Category."></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnReplaceNDelete" runat="server" CssClass="clsbtnH clsinfoH" Text="Replace &amp; Delete"
                                                                        ToolTip="Click to Replace &amp;  Delete Old Category." Width="120px"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Replace Category screen"
                                                                        CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvCategory" runat="server" ErrorMessage="Select Category."
                                                ControlToValidate="cmbCategory" Display="None" ClientValidationFunction="ValidationCategory"
                                                CssClass="clsValidationSummary"></asp:CustomValidator><asp:CustomValidator ID="cvReplaceWithCategory"
                                                    runat="server" ErrorMessage="Select Replace With Category." ControlToValidate="cmbReplaceWithCategory"
                                                    Display="None" ClientValidationFunction="ValidationReplaceWithCategory" CssClass="clsValidationSummary"></asp:CustomValidator>
                                                      <asp:CustomValidator ID="cv" runat="server" 
                                                ClientValidationFunction="ValidateBothCategories" 
                                                ControlToValidate="cmbReplaceWithCategory" CssClass="clsValidationSummary" 
                                                Display="None" ErrorMessage="Please Select Different Replace With Category."></asp:CustomValidator>
                                                    <script type="text/javascript">
                                                        function ValidationCategory(source, args) {
                                                            var dd = $get("cmbCategory");
                                                            args.IsValid = true;
                                                            if (dd.selectedIndex == 0) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                        }
                                                    </script>
                                                     <script type="text/javascript">
                                                         function ValidationReplaceWithCategory(source, args) {
                                                             var dd = $get("cmbReplaceWithCategory");
                                                             args.IsValid = true;
                                                             if (dd.selectedIndex == 0) {
                                                                 args.IsValid = false;
                                                                 return;
                                                             }
                                                         }
                                                    </script>
                                                    <script type="text/javascript">
                                                        function ValidateBothCategories(source, args) {
                                                            var e = document.getElementById("cmbCategory");
                                                            var CategoryID = e.options[e.selectedIndex].value;
                                                            var e1 = document.getElementById("cmbReplaceWithCategory");
                                                            var ReplaceWithCategoryID = e1.options[e1.selectedIndex].value;
                                                            args.IsValid = true;

                                                            if (CategoryID == ReplaceWithCategoryID) {
                                                                args.IsValid = false;
                                                            }
                                                        }
                                                    </script>
                                          
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblCategoryStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblCategory" class="clsLabelAuto">Category</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                            DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblReplaceWithCategoryStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblReplaceWithCategory" class="clsLabelAuto">Replace With Category</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbReplaceWithCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataValueField="ID" DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnReplace" runat="server" CssClass="clsbtnH clsinfoH1" Text="Replace" ToolTip="Click to Replace Category.">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnReplaceNDelete" runat="server" CssClass="clsbtnH clsinfoH1" Text="Replace &amp; Delete"
                                                            ToolTip="Click to Replace &amp;  Delete Old Category." Width="120px"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to close Replace Category screen"
                                                            CausesValidation="False"></asp:Button>
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
    </div>
    </form>
</body>
</html>
