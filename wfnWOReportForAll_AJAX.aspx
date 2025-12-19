<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOReportForAll_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOReportForAll_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WO Report For All</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" rel="stylesheet" type="text/css"    />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlmain" CssClass="clspanel1" runat="server">
                        <ContentTemplate>
                            <table class="clstablelistin" id="tblInner">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">W. O. Report For All</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="4">
                                        <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Option Selection</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:RadioButton ID="rbAll" runat="server" CssClass="clsRadioButton" Checked="True"
                                            Text="All" GroupName="a"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:RadioButton ID="rbWOSummary" runat="server" CssClass="clsRadioButton" Text="W. O. Summary"
                                            GroupName="a"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:RadioButton ID="rbWOJobSummary" runat="server" CssClass="clsRadioButton" Text="W. O. Job Summary"
                                            GroupName="a"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:RadioButton ID="rbTaskCard" runat="server" CssClass="clsRadioButton" Text="Task Card"
                                            GroupName="a" Visible="False"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkAMMNo" runat="server" CssClass="clsCheckBox" Checked="True"
                                            Text="AMM. No." Visible="False"></asp:CheckBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkMPDNo" runat="server" CssClass="clsCheckBox" Text="MPD. No."
                                            Visible="False"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblIssueTo" runat="server" CssClass="clsLabelAuto" Visible="False">Number</asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox" MaxLength="10" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        <asp:UpdatePanel ID="upnlButton" runat="server" CssClass="clspanel1" Width="300px">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton" Text="Display"
                                                                ToolTip="Click to Display Report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" Text="Close"
                                                                ToolTip="Click to close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
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
    </div>
    <div>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForReportAll();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        $(document).ready(function () {
       SetPageLayout();
         if ($.browser.msie) {
             parent.IFrameStateComplete();
         }
       
      
    });
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
          var tempMargtop=$("body #tblmain:eq(0)").outerHeight(true);
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
    </div>
    </form>
</body>
</html>
