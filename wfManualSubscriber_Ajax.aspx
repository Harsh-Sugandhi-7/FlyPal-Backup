<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualSubscriber_Ajax.aspx.vb"
    Inherits="Flypal.wfManualSubscriber_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manual Subscriber</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="Form1" method="post" runat="server">
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
                        <td>
                            <table>
                                <tr>
                                    <td class="clsFormHeader1Newstyle" colspan="4">

                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Subscriber</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>


                                                <td align="right" colspan="4">
                                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" ValidationGroup="1"
                                                                            CausesValidation="true" Text="OK" ToolTip="Click to Add Subscriber"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"></asp:Button>
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
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                    CssClass="clsValidationSummary" ValidationGroup="1"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtEmployeeName"
                                                    ErrorMessage="Employee Name Required." Display="None" ValidationGroup="1" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                    ErrorMessage="Email Required." Display="None" ValidationGroup="1" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvNameLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Employee Name too Long."
                                                    Display="None" ControlToValidate="txtEmployeeName" ClientValidationFunction="validateName"
                                                    ValidateEmptyText="true" ValidationGroup="1"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvEmailLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Email too Long."
                                                    Display="None" ControlToValidate="txtEmail" ClientValidationFunction="validateName"
                                                    ValidationGroup="1"></asp:CustomValidator>
                                                <asp:RegularExpressionValidator ID="rgeEmail" runat="server" Display="None" ControlToValidate="txtEmail"
                                                    ErrorMessage="Please Enter Valid Email-ID." ValidationGroup="1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    CssClass="clsLabelAuto"></asp:RegularExpressionValidator>
                                                <script type="text/javascript">
                                                    function validateName(source, args) {
                                                        var ControlName = source.controltovalidate;
                                                        switch (ControlName) {
                                                            case 'txtEmployeeName':
                                                                var Value = $get(ControlName).value.length;
                                                                if (Value > 50) {
                                                                    args.IsValid = false;
                                                                    return
                                                                }
                                                                break;

                                                            case 'txtEmail':
                                                                var Value = $get(ControlName).value.length;
                                                                if (Value > 200) {
                                                                    args.IsValid = false;
                                                                    return
                                                                }
                                                                break;
                                                        }
                                                    }
                                                </script>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="lblRevisionDetails" class="clsLabelHeader">Subscriber Details</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlSubscriberDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblNo" class="clsLabelAuto">Employee</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataTextField="EmpNoName" DataValueField="ID" AutoPostBack="true" SelectedValue="<%# mManual.ManualSubscribers.CurrentItem.EmployeeID %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="lblRevisionNo" class="clsLabelAuto">Employee Name</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Employee Name"
                                                                Text="<%# mManual.ManualSubscribers.CurrentItem.EmployeeName %>" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFreq" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblEmail" runat="server" CssClass="clsLabelAuto">Email </asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxTagSearch" Text="<%#  mManual.ManualSubscribers.CurrentItem.Email %>"
                                                                ToolTip="Enter Email">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                   <%-- <td align="right" colspan="4">
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" ValidationGroup="1"
                                                                CausesValidation="true" Text="OK"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"></asp:Button>
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
            parent.ParentCallBackFunctionForSubscriber();
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
             parent.IFrameSubscriberStateComplete();
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
