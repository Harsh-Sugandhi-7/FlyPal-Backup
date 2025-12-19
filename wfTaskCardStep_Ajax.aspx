<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskCardStep_Ajax.aspx.vb"
    Inherits="Flypal.wfTaskCardStep_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Additional Work Detail</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
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
            <table class="clstablelistout" id="tblmain" cellspacing="1" cellpadding="1" border="0">
                <tr>
                    <td>
                        <table class="clstablelistin" id="Table2" cellspacing="1" cellpadding="1" border="0">
                            <tr>

                               <td class="clsFormHeader1" colspan="5">
                                <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Additional Work Detail</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnOK" CssClass="clsbtnH clsinfoH" runat="server" Text="OK" ToolTip="Click to save Additional Work Detail &amp; go to Previous page"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" CssClass="clsbtnH clsinfoH" runat="server" Text="Back" ToolTip="Click to go Previous page"></asp:Button>
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
                                <td></td>
                                <td colspan="4">
                                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel ID="upnlStepDetail" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdswodetail" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                                <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Additional Work Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td style="width: 1px"></td>
                                                        <td>
                                                            <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto">MPD No. </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMPDNo" runat="server" CssClass="clsTextBoxTagSearch" Width="184px"
                                                                ToolTip="Enter MPD No." Text="<%# mTaskCard.TaskSteps.CurrentItem.MPDNo %>" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 1px"></td>
                                                        <td>
                                                            <asp:Label ID="lblRevisionNo" runat="server" CssClass="clsLabelAuto">AMM No. </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAMMNo" runat="server" CssClass="clsTextBoxTagSearch" Width="184px"
                                                                ToolTip="Enter AMM No." Text="<%# mTaskCard.TaskSteps.CurrentItem.AMMNo %>" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 1px">
                                                            <asp:Label ID="lblPartNo1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td style="width: 109px">
                                                            <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto" Width="64px"> Description </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStepDesc" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="1000"
                                                                Text="<%# mTaskCard.TaskSteps.CurrentItem.Description %>" ToolTip="Enter Description"
                                                                Width="360px" Height="136px" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 1px; height: 12px"></td>
                                                        <td style="width: 109px; height: 12px">
                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Zone/Work Area</asp:Label>
                                                        </td>
                                                        <td style="height: 12px">
                                                            <asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                Text="<%# mTaskCard.TaskSteps.CurrentItem.Zone %>" ToolTip="Enter Zone/Work Area"
                                                                Width="360px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right" colspan="5">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOK" CssClass="clsButton" runat="server" Text="OK" ToolTip="Click to save Additional Work Detail &amp; go to Previous page">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" CssClass="clsButton" runat="server" Text="Back" ToolTip="Click to go Previous page">
                                                    </asp:Button>
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
        </div>
        <div>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
                runat="server">
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
        </div>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForTaskCardStep();
                return false;
            }
        </script>
        <%--End--%>
        <div>
            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameTaskCardStepStateComplete();
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
            <%--End--%>
        </div>
    </form>
</body>
</html>
