<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditReSchedule_Ajax.aspx.vb"
    Inherits="Flypal.wfAuditReSchedule_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Audit Compliance Details</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
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
                <table class="clsTablelistin" id="tblinner">
                    <tr>
                        <td colspan="3" class="clsFormHeader1">
                             <table width="100%">
                                 <tr>
                                     <td>
                                         <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                             <ContentTemplate>
                                                 <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Audit Re-Schedule Dates</asp:Label>
                                             </ContentTemplate>
                                         </asp:UpdatePanel>
                                     </td>

                                     <td align="right" colspan="3">
                                         <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                             <ContentTemplate>
                                                 <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                     <tr>
                                                         <td>
                                                             <asp:Button CssClass="clsbtnH clsinfoH" ID="btnOK" runat="server" ToolTip="Click to Re-Schedule"
                                                                 Text="Re-Schedule"></asp:Button>
                                                         </td>
                                                         <td>
                                                             <asp:Button CssClass="clsbtnH clsinfoH" ID="btnBack" runat="server" ToolTip="Click to Close"
                                                                 Text="Close"></asp:Button>
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
                        <td colspan="3">
                            <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" Display="None"></asp:CustomValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdbScheduleStartDate" runat="server" GroupName="a" Text="From Schedule Date"
                                            Checked="True" CssClass="clsRadioButton" />
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp; <span class="clsLabel" style="text-align: center">(As per Schedule
                                            Date of Current Audit) </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lblScheduleDate" CssClass="clsLabel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdbComplianceEndDate" runat="server" GroupName="a" Text="From Completion Date"
                                            CssClass="clsRadioButton" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp; <span class="clsLabel" style="text-align: center">(As per Completion
                                            Date of Current Audit) </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lblComplianceDate" CssClass="clsLabel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <%--<td align="right" colspan="3">
                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnOK" runat="server" CssClass="clsButton" ToolTip="Click to Save Audit Compliance"
                                                    Text="Re-Schedule"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton" ToolTip="Click to Close the Audit Compliance Screen"
                                                    Text="Close"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                background-color: #000000; top: 0; z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                z-index: 100000;">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div clawftaskss="clsLoad_ajax">
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
            parent.ParentCallBackFunctionForReSchedule();
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
                parent.IFrameReScheduleStateComplete();
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
