<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MSGBox.ascx.vb" Inherits="Flypal.MSGBox" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="ucNotificationMsg.ascx" TagName="ucNotificationMsg" TagPrefix="uc1" %>

<link href="Notification/style/notificationmsg.css" rel="stylesheet" type="text/css" />

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
                                <asp:Label runat="server" ID="lblMsgTitle" Text="MsgTitle" CssClass="clsMsgBoxTitle" />
                            </div>
                            <div class="clsMsgBoxBody">
                                <div class="clsMsgBoxInnerBody">
                                    <div style="padding: 10px; min-height: 20px;">
                                        <div class="clsMsgInfoIcon">
                                        </div>
                                        <div class="clsMsgContent">
                                            <asp:Label runat="server" CssClass="clsMsgText" 
												ID="lblMsgText" Text="MsgText" />
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
            <uc1:ucNotificationMsg ID="ucNotificationMsg1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</div>

<!-- Added By Utkarsh to hide message box after button click-->
<script type="text/javascript">

	Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

		console.log("--- Page Load function started ---");

		try {

			$("#ButtonDiv :submit", "#pnlMessageBox").live('click', function () {
				var model = $find('mdlPopupBox');
				model.hide();
				return true;
			});

		} catch (e) {
			console.error("Error in Page Load function:", e.message);
			console.error(e);

		} finally {
			console.log("--- Page Load function finished ---");
		}

	});
	
</script>
<!-- end -->

<!-- Added By Utkarsh to hide message box after infoOk button click-->
<script type="text/javascript">

	Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

		console.log("--- Page Load function for btnInfoOk started ---");

		try {

			$("#ButtonDiv #btnInfoOk", "#pnlMessageBox").live('click', function () {
				var model = $find('mdlPopupBox');
				model.hide();
				return false;
			});

		} catch (e) {
			console.error("Error in Page Load for btnInfoOk function:", e.message);
			console.error(e);

		} finally {
			console.log("--- Page Load function for btnInfoOk finished ---");
		}

	});

</script>
<!-- end -->

<script type="text/javascript" language="javascript">

	//There's a bug in Microsoft's Ajax script that stops the modal popups from working
    //This overrides the the code that causes the error

	Sys.UI.Point = function Sys$UI$Point(x, y) {

		console.log("--- function Sys$UI$Point started ---");

		try {

			x = Math.round(x);
			y = Math.round(y);

			var e = Function._validateParams(arguments, [
				{ name: "x", type: Number, integer: true },
				{ name: "y", type: Number, integer: true }
			]);

			if (e) throw e;

			this.x = x;
			this.y = y;

		} catch (e) {
			console.error("Error in function Sys$UI$Point:", e.message);
			console.error(e);

		} finally {
			console.log("--- function Sys$UI$Point finished ---");
		}

	}

</script>

<cc2:ModalPopupExtender ID="mdlPopupBox" ClientIDMode="Static" runat="server" TargetControlID="btnDummy"
    PopupControlID="pnlMessageBox" BackgroundCssClass="clsMsgBoxBG" PopupDragHandleControlID="TitleBar">
</cc2:ModalPopupExtender>
