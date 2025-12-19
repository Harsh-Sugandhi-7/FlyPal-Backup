<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHintQuestion.aspx.vb"
    Inherits="Flypal.wfHintQuestion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hint Question</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link id="Link1" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="Styles.css" id="Link2" type="text/css" rel="stylesheet" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
                <table id="tblInner" class="clstablelistin">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTitle" Style="font-size: 18px; font-weight: 100;margin-bottom:15px" CssClass="text-warning clstitle1"
                                            runat="server">Question(s)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                  &nbsp;  &nbsp;    <span id="Span2" class="control-label clsLabelHeader" >List of Questions to help you
                                            with Reviewing/Planning your AD/SB's Meeting :</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset class="input-sm clsFieldSet" style="font-family: Verdana; font-size: 10pt;
                                            font-weight: 500; border-width: 1px;  margin-top: 15px;
                                            margin-right: 5px; margin-left: 5px">
                                            <asp:BulletedList ID="BulletedList1" runat="server" BulletStyle="Numbered">
                                                <asp:ListItem>Does AD require specific parts or articles requiring high lead time? </asp:ListItem>
                                                <asp:ListItem>Does it impact future inspection schedule?</asp:ListItem>
                                                <asp:ListItem>Is the AD applicable with future date ? </asp:ListItem>
                                                <asp:ListItem>Does AD apply to entire fleet?</asp:ListItem>
                                                <asp:ListItem>Does AD applies to appliances or particular product? </asp:ListItem>
                                                <asp:ListItem>Does AD applies to parts/engines in stock?</asp:ListItem>
                                                <asp:ListItem>Does AD applies to parts which is under repair? </asp:ListItem>
                                                <asp:ListItem>Are there repairs or alterations on product or appliances which might require an AMOC?</asp:ListItem>
                                                <asp:ListItem>Determine if AD referenced SB/Service letter instructions already accomplished? </asp:ListItem>
                                                <asp:ListItem>Does AD affects any AMM, IPC, CMM or EMM to ensure continued compliance?</asp:ListItem>
                                                <asp:ListItem>Does AD compliance requires any specific skill such as NDT certifications or use of special equipment like boro or use of any laboratory test or special equipment? </asp:ListItem>
                                                <asp:ListItem>Does AD compliance restricted to any specific shop or locations?</asp:ListItem>
                                                <asp:ListItem>Does AD involves any critical task or double check while compliance in progress? </asp:ListItem>
                                                <asp:ListItem>Does this requires prototyping?</asp:ListItem>
                                                <asp:ListItem>Do you want to set up an audit plan?</asp:ListItem>
                                                <asp:ListItem>Does AD gets covered in warranty?</asp:ListItem>
                                                <asp:ListItem>Name the agencies where compliance needs to be sent?  </asp:ListItem>
                                                <asp:ListItem>Do you require OEM support or Regulator involvement?</asp:ListItem>
                                            </asp:BulletedList>
                                        </fieldset>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                  &nbsp;  &nbsp;    <span id="Span1" class="control-label clsLabelHeader" >If you have any question(s) that we haven't mentioned we would love to hear from you. Please feel free to contact us.</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                            <tr>
                                <td>
                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="btn btn-sm"
                                        Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                        border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                        margin-bottom: 5px; margin-right: 5px;" Text="Close" ToolTip="Click to go back to the previous page">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <!--call parent function after completing subroutine..(when page open as popup)-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForHintQuestion();
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
             parent.IFrameHintQuestionComplete();
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
