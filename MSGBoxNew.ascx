<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MSGBoxNew.ascx.vb" Inherits="Flypal.MSGBoxNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="ucNotificationMsgNew.ascx" TagName="ucNotificationMsgNew" TagPrefix="uc1" %>
<link href="Notification/style/notificationmsg.css" rel="stylesheet" type="text/css" />
<%--Used for Notification--%>
<%--<script type="text/javascript" src="js/jquery.js">    </script>
<script src="js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="Notification/jQuery/ui.core.js"></script>
<script type="text/javascript" src="Notification/jQuery/ui.notificationmsg.js"></script>
<script src="js/jquery.tooltip.js" type="text/javascript"></script>--%>
<style type="text/css">
    .ClsMsgBoxMdlPopup
    {
        z-index: 9999999 !important;
    }
    .clsMsgBoxBG
    {
        background-color: black;
        filter: alpha(opacity=50);
        opacity: 0.5;
        z-index: 9999998 !important;
    }
</style>
<div style="display: none">
    <asp:Button runat="server" ID="btnDummy" Text="Check" />
</div>
<asp:Panel runat="server" ID="pnlMessageBox" ClientIDMode="Static" CssClass="ClsMsgBoxMdlPopup"
    Height="150px" Width="400px" Style="display: none;">
    <div>
        <div class="msgBoxShadow">
            <div class="clsMsgBoxOuter">
                <asp:Panel ID="TitleBar" runat="server">
                    <asp:UpdatePanel ID="upnlMessageBox" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="text-align: center">
                                <asp:Label runat="server" ID="lblMsgTitle" Text="MsgTitle" CssClass="clsMsgBoxTitle"></asp:Label>
                            </div>
                            <div class="clsMsgBoxBody">
                                <div class="clsMsgBoxInnerBody">
                                    <div style="padding: 10px; min-height: 20px;">
                                        <div class="clsMsgInfoIcon">
                                        </div>
                                        <div class="clsMsgContent">
                                            <asp:Label runat="server" CssClass="clsMsgText" ID="lblMsgText" Text="MsgText"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clsMsgBoxFooterWrap">
                                <div class="clsMsgBoxFooter" id="ButtonDiv" clientidmode="Static" runat="server">
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Panel>
<div>
    <asp:UpdatePanel ID="upnlNotificationMsg" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:ucNotificationMsgNew ID="ucNotificationMsgNew" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- Added By Utkarsh to hide message box after button click-->
<script type="text/javascript">
    //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
//        $("#ButtonDiv :submit", "#pnlMessageBox").live('click', function () {
        $(document).on('click', '#ButtonDiv :submit, #pnlMessageBox', function () {
            //alert('i am hit');
            var model = $find('mdlPopupBox');
            model.hide();
            return true;
        });
    });       
</script>
<!-- end -->
<!-- Added By Utkarsh to hide message box after infoOk button click-->
<script type="text/javascript">
    //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
//        $("#ButtonDiv #btnInfoOk", "#pnlMessageBox").live('click', function () {
        $(document).on('click', '#ButtonDiv #btnInfoOk, #pnlMessageBox', function () {
            //alert('i am hit');
            var model = $find('mdlPopupBox');
            model.hide();
            return false;
        });
    });       
</script>
<!-- end -->
<script type="text/javascript" language="javascript">
    //There's a bug in Microsoft's Ajax script that stops the modal popups from working
    //This overrides the the code that causes the error
    Sys.UI.Point = function Sys$UI$Point(x, y) {

        x = Math.round(x);
        y = Math.round(y);

        var e = Function._validateParams(arguments, [
                { name: "x", type: Number, integer: true },
                { name: "y", type: Number, integer: true }
            ]);
        if (e) throw e;
        this.x = x;
        this.y = y;
    }
</script>
<cc2:ModalPopupExtender ID="mdlPopupBox" ClientIDMode="Static" runat="server" TargetControlID="btnDummy"
    PopupControlID="pnlMessageBox" BackgroundCssClass="clsMsgBoxBG" PopupDragHandleControlID="TitleBar">
</cc2:ModalPopupExtender>
