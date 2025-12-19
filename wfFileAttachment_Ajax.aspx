<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFileAttachment_Ajax.aspx.vb"
    Inherits="Flypal.wfFileAttachment_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>File Attachment</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
    <div>
        <table id="Table1" class="clstablelistout" border="0" cellspacing="1" cellpadding="1">
            <tr>
                <td colspan="3" class="clsFormHeader1Newstyle">
                    <table>
                        <tr>
                            <td>
                                <span class="clsFormHeader">Logo Attachment</span>
                            </td>
                            <%--<td colspan="3" align="right" style="padding-left: 3px;">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnSave" runat="server" ToolTip="Click To Save Attachment"
                                            Text="Save"></asp:Button>
                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" ToolTip="Click To Close"
                                            Text="Close"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" class="clsbtnH clsinfoH" id="btnSelectFile" value="Select File"
                        runat="server" />
                </td>
                <td style="padding-left: 3px;">
                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDelAttach" runat="server"  ToolTip="Click to Remove Attachment"
                        Text="Remove Attachment" Enabled="False"></asp:Button>
                </td>
                <td style="padding-left: 3px;">
                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                        onmouseout="BalloonPopupControlBehavior.hidePopup();" Height="20px" Width="20px">
                    </asp:ImageButton>
                    <asp:Panel ID="Panel2" runat="server">
                        Click to view attachment
                    </asp:Panel>
                    <cc2:BalloonPopupExtender ID="PopupControlExtender2" runat="server" TargetControlID="ImageButton1"
                        BalloonPopupControlID="Panel2" Position="BottomRight" BalloonStyle="Rectangle"
                        BalloonSize="Small" CustomCssUrl="CustomStyle/BalloonPopupOvalStyle.css" CustomClassName="oval"
                        UseShadow="true" ScrollBars="Auto" DisplayOnClick="false" DisplayOnFocus="false"
                        DisplayOnMouseOver="true" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right" style="padding-left: 3px;">
                    <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnSave" runat="server" ToolTip="Click To Save Attachment"
                                Text="Save"></asp:Button>
                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" ToolTip="Click To Close"
                                Text="Close"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr style="height: 0px;">
                <td style="height: 0px;">
                    <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                        CausesValidation="False" Style="display: none;"></asp:Button>
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
    </div>
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
        PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameFileUploadStateComplete() {
            $("#btnDummyFileUpload").click();
        }

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    $("#IFileUpload").attr("src", "wfFileUpload.aspx");

                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }
            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForFileUpload(fileattached) {
            var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
            //close File Upload popup window
            FileUpwindow.hide();
            //Free resources
            $("#IFileUpload").attr("src", "JavaScript:''");
            if (fileattached) {
                //call hidden button to set file upload content to object
                $("#hdnBtnFileUpload").click();
            }
        }
    </script>
    <!-- End -->
    </form>
</body>
</html>
