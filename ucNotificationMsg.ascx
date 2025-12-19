<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucNotificationMsg.ascx.vb"
	Inherits="Flypal.NotificationMsg" %>

<script type="text/javascript" src="Notification/jQuery/ui.core.js"></script>
<script type="text/javascript" src="Notification/jQuery/ui.notificationmsg.js"></script>
<link id="MainStyle" type="text/css" rel="stylesheet" />
<link href="Notification/style/notificationmsg.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">

	Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

		$('#msg1').notificationmsg({ period: 4000 });
		$('#closebutton').click(function () { $('#msg1').notificationmsg('hide'); });
		$('#closebuttonMsg').click(function () { $('#msg1').notificationmsg('hide'); });
		Center();

	});

	function ShowMessage(Title, Msg) {

		console.log("--- ShowMessage function started ---");
		console.log(`Parameters received: Title = "${Title}", Msg = "${Msg}"`);

		try {

			if(!Title || !Msg) {
				throw new Error("Missing required parameters: Both Title and Msg must be provided.");
			}

			const str = Msg;
			const animStyle = 'slide';

			console.log("Attempting to set notification title and message in the DOM...");

			const titleElement = document.getElementById('<%=lblNotificationTitle.ClientID%>');
			const msgElement = document.getElementById('<%=lblNotification.ClientID%>');

		if (titleElement) {
			titleElement.innerHTML = Title;
			console.log("Successfully set notification title.");
		} else {
			console.warn(`Warning: Could not find DOM element with ID '<%=lblNotificationTitle.ClientID%>'.`);
		}

		if (msgElement) {

			msgElement.innerHTML = str;
			console.log("Successfully set notification message.");

		} else {
			console.warn(`Warning: Could not find DOM element with ID '<%=lblNotification.ClientID%>'.`);
			}

			console.log("Starting AJAX call to 'login.aspx/GetMessages'...");

			$.ajax({
				type: "POST",
				url: "login.aspx/GetMessages",
				data: "{}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",

				success: function (msg) {

					console.log("AJAX Success: Message received.", msg);

					try {

						$("#modalbody").text(`(${msg.d})`);

						console.log(`Updated #modalbody with data: ${msg.d}`);

						$('#msg1').notificationmsg({ animation: animStyle });
						$('#msg1').notificationmsg('show');

						console.log("Notification plugin called to show message ('#msg1').");

					} catch (innerError) {
						console.error("Error during post-AJAX DOM manipulation or plugin call:", innerError);
					}
				},

				error: function (xhr, status, error) {

					console.error("AJAX Error occurred!", {
						status: status,
						errorThrown: error,
						responseText: xhr.responseText,
						readyState: xhr.readyState,
						httpStatus: xhr.status
					});

					const userMessage = `Error fetching additional messages: ${status} - ${error}`;
					console.error(userMessage);
				}
			});

		} catch (e) {
			console.error("Synchronous Error in ShowMessage function:", e.message);
			console.error(e);

		} finally {
			console.log("--- ShowMessage function finished its synchronous execution ---");
		}
	}

	function Center() {

		var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
		var height = document.documentElement.clientHeight + document.documentElement.scrollTop;

		var top = ((height + document.documentElement.scrollTop) / 2) - ($('#centerdiv').height() / 2) + "px";

		var left = (width / 2) - ($('#centerdiv').width() / 2) + "px";
		$('#centerdiv').css({ position: "absolute", top: top, left: left });

	}

</script>

<asp:UpdatePanel ID="upnlNotifier" runat="server" UpdateMode="Conditional">
	<ContentTemplate>
		<div id="msg1" style="height: auto; width: 400px">
			<div id="modal">
				<div class="clsMsgBoxOuter" style="display: none; background-image: url('Notification/img/bg.gif')">
					<div class="clsMsgBoxTitle">
						Message
					</div>
					<span id="closebutton" style="cursor: pointer">
						<img alt="Hide Popup" src="Notification/img/close_vista.gif" border="0" />
					</span>
				</div>
				<div class="modalbody">
					<asp:Panel runat="server" ID="Panel1" Width="400px">
						<div>
							<div class="clsMsgBoxOuter">
								<div class="clsMsgBoxTitle">
									<div style="padding: 3px">
										<table>
											<tr>
												<td style="width: 95%">
													<span class="clsMsgBoxTitle">
														<asp:Label runat="server" ID="lblNotificationTitle" Text="Change Password" />
													</span>
												</td>
												<td style="width: 10%" align="right">
													<span id="closebuttonMsg" style="cursor: pointer">
														<img alt="Hide Popup" src="images/delete.gif" border="0" />
													</span>
												</td>
											</tr>
										</table>
									</div>
								</div>
								<div class="clsMsgBoxBody">
									<div class="clsMsgBoxInnerBody">
										<div style="height: auto; padding: 10px">
											<div class="clsMsgInfoIcon" style="padding: 10px">
											</div>
											<div class="clsMsgContent" style="padding: 3px">
												<asp:Label runat="server" CssClass="clsMsgText" ID="lblNotification" Text="" />
											</div>
										</div>
									</div>
								</div>
								<div class="clsMsgBoxFooterWrap">
									<div class="clsMsgBoxFooter">
									</div>
								</div>
							</div>
						</div>
					</asp:Panel>
				</div>
			</div>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>
