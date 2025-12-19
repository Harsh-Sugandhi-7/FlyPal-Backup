<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFileUpload.aspx.vb"
	Inherits="Flypal.wfFileUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>File Upload</title>

	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="form1" runat="server">
		<div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
							<table border="0" class="clsTablelistin" width="100%">
								<tr>
									<td colspan="3" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<span class="clsFormHeader">File Upload</span>
												</td>
												<td align="right">
													<asp:Button ID="btnupload" runat="server" Text="Attach"
														ToolTip="Click to Attach selected File(s)"
														CssClass="clsbtnH clsinfoH" />

													<input id="btnClose" type="button" value="Close"
														title="Click to Close current window without Attaching File(s)"
														class="clsbtnH clsinfoH" onclick="onUploadComplete(false);" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td colspan="3" align="left">
						<div id="FileContentShow">
							<div id="filepath">
								No File selected
							</div>
						</div>
					</td>
				</tr>
				<tr>
					<td style="width: 125px;">
						<div class="fileUpload1 uploadbtn">
							<span>Browse...</span>
							<asp:FileUpload ID="FileUpload" 
								CssClass="clsbtnH clsinfoH1" 
								runat="server" onchange="showfilepath(this);" />
						</div>
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<asp:Label ID="lblMessage" runat="server" CssClass="clsLabelHeader" Visible="false" Text="* Click Close if Attachment is not Required for BER Issue."></asp:Label>
					</td>
				</tr>
			</table>

			<script type="text/javascript">        
</script>

			<script type="text/javascript">
				$(document).ready(function () {
					$("#<%=btnupload.ClientID %>").live("click", function () {
						var tempval = document.getElementById("FileUpload1").value;


						if (tempval) {

							// parent.submitChildForm();
							$('#fileuploadform').submit();
							document.getElementById("FileUpload1").value = tempval;
							return true;
						}
						else {
							return false;
						}
					});
				});

				function onUploadComplete(fileattached) {
					parent.ParentCallBackFunctionForFileUpload(fileattached);
					return false;
				}
				var timeout;
				var duration;
				var marginleft;
				function showfilepath(elem) {
					$("#<%=btnupload.ClientID %>").removeAttr('disabled');
					$("#filepath").clearQueue().stop();
					$("div:animated").stop(true, true);
					$("#filepath").html('');
					$("#filepath").html(elem.value);
					$("#filepath").attr("title", elem.value);
					$("#filepath").css({ 'left': '0', 'font-style': 'normal', 'color': '#1C1F24' });
					//var marginleft = $("body #tblmain:eq(0)").css('margin-left');
					marginleft = $("#filepath").parent().width() - $("#filepath").width();
					if (marginleft < 0) {
						duration = ((-1 * marginleft) / 100) * 2000;
						Marquee(marginleft, duration);
					}
					//$("#filepath")

				}
				function Marquee(margin, dur) {
					$("#filepath").delay(2000).animate({ 'left': margin }, dur, 'linear', function () {
						$("#filepath").delay(2000).animate({ 'left': 0 }, 0, 'linear');
						Marquee(marginleft, duration);
					});

				}
			</script>

		</div>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">
			$(document).ready(function () {
				$("#<%=btnupload.ClientID %>").attr('disabled', 'disabled');
				ReSetPageLayout();

			});

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

			function OnPageLoad() {
				$("#<%=btnupload.ClientID %>").attr('disabled', 'disabled');
				ReSetPageLayout();
				parent.IFrameFileUploadStateComplete();
			}
		</script>
		<%--End--%>
	</form>
</body>
</html>
