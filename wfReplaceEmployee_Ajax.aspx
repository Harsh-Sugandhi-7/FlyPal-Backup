<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReplaceEmployee_Ajax.aspx.vb"
    Inherits="Flypal.wfReplaceEmployee_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Replace Employee</title>
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
                            <tr class="clsFormHeader1Newstyle">
                                <td>
                                    <span id="lblTitle" class="clsFormHeader">Replace Employee</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvEmployee" runat="server" ErrorMessage="Select Employee."
                                                ControlToValidate="cmbEmployee" Display="None" ClientValidationFunction="ValidationEmployee"
                                                CssClass="clsValidationSummary"></asp:CustomValidator><asp:CustomValidator ID="cvReplaceWithEmployee"
                                                    runat="server" ErrorMessage="Select Replace With Employee." ControlToValidate="cmbReplaceWithEmployee"
                                                    Display="None" ClientValidationFunction="ValidationReplaceWithEmployee" CssClass="clsValidationSummary"></asp:CustomValidator>
                                                      <asp:CustomValidator ID="cv" runat="server" 
                                                ClientValidationFunction="ValidateBothCategories" 
                                                ControlToValidate="cmbReplaceWithEmployee" CssClass="clsValidationSummary" 
                                                Display="None" ErrorMessage="Please Select Different Replace With Employee."></asp:CustomValidator>
                                                    <script type="text/javascript">
                                                        function ValidationEmployee(source, args) {
                                                            var dd = $get("cmbEmployee");
                                                            args.IsValid = true;
                                                            if (dd.selectedIndex == 0) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                        }
                                                    </script>
                                                     <script type="text/javascript">
                                                         function ValidationReplaceWithEmployee(source, args) {
                                                             var dd = $get("cmbReplaceWithEmployee");
                                                             args.IsValid = true;
                                                             if (dd.selectedIndex == 0) {
                                                                 args.IsValid = false;
                                                                 return;
                                                             }
                                                         }
                                                    </script>
                                                    <script type="text/javascript">
                                                        function ValidateBothCategories(source, args) {
                                                            var e = document.getElementById("cmbEmployee");
                                                            var EmployeeID = e.options[e.selectedIndex].value;
                                                            var e1 = document.getElementById("cmbReplaceWithEmployee");
                                                            var ReplaceWithEmployeeID = e1.options[e1.selectedIndex].value;
                                                            args.IsValid = true;

                                                            if (EmployeeID == ReplaceWithEmployeeID) {
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
                                                        <span id="lblEmployeeStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblEmployee" class="clsLabelAuto">Employee</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbEmployee" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                            DataTextField="NameEmpNo">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblReplaceWithEmployeeStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblReplaceWithEmployee" class="clsLabelAuto">Replace With Employee</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbReplaceWithEmployee" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataValueField="ID" DataTextField="NameEmpNo">
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
                                                        <asp:Button ID="btnReplace" runat="server" CssClass="clsbtnH clsinfoH1" Text="Replace" ToolTip="Click to Replace Employee.">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnReplaceNDelete" runat="server" CssClass="clsbtnH clsinfoH1" Text="Replace &amp; Delete"
                                                            ToolTip="Click to Replace &amp;  Delete Old Employee." Width="120px" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" 
                                                            ToolTip="Click to close Replace Employee screen"
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
