<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFileUploadForIssueToBERPart.aspx.vb"
	Inherits="Flypal.wfFileUploadForIssueToBERPart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>File Upload / Discard Value</title>

	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<script type="text/jscript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
						<asp:Panel ID="pnlMain" runat="server">
							<table border="0" id="" class="clstablelistin">
								<tr>
									<td colspan="3" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<span class="clsFormHeader" style="display: block; width: 300px">File Upload / Discard Value
													</span>
												</td>
												<td align="right">
													<asp:Button ID="btnupload" runat="server"
														Text="Attach" ToolTip="click to attach selected file"
														ClientIDMode="Static" CssClass="clsbtnH clsinfoH" />

													<asp:Button ID="btnSave" runat="server" Text="Save"
														ClientIDMode="Static" ToolTip="click to save"
														CssClass="clsbtnH clsinfoH" />

													<input id="btnClose" type="button" value="Close"
														title="click to close current window without saving"
														class="clsbtnH clsinfoH" onclick="onuploadclose();" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:Label ID="lblEnterDiscardAmountInBaseCurrency" Style="display: block; height: 25px"
											CssClass="clsLabelHeader" runat="server">Enter Discard Amount In Base Currency
										</asp:Label>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblDiscardRate" class="clsLabel">Enter Discard Amount</span>
									</td>
									<td>
										<asp:TextBox ID="txtDiscardAmt" runat="server" Width="185px"
											CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
											MaxLength="12" ToolTip="Enter Discard Amount">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:Label ID="lblMaxEffectiveRateValue"
											CssClass="clsLabelHeader" runat="server">
										</asp:Label>
									</td>
								</tr>
								<tr>
									<td colspan="3" align="left">
										<div id="FileContentShow">
											<asp:Label runat="server" ID="filepath">
                                            No File Selected
											</asp:Label>
										</div>
									</td>
								</tr>
								<tr>
									<td style="width: 125px;">
										<div class="fileUpload1 uploadbtn">
											<span>Browse...</span>
											<asp:FileUpload ID="FileUpload1" CssClass="clsbtnH clsinfoH1"
												runat="server" onchange="showfilepath(this);" />
										</div>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:Label ID="lblMessage" runat="server" CssClass="clsLabelHeader" Visible="false"
											Text="* Click Close if Attachment is not Required for BER Issue."></asp:Label>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>

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
				//**********************************************************************
				function onuploadcomplete(fileattached) {
					parent.ParentCallBackFunctionForFileUpload(fileattached);
					return false;
				}
				//**********************************************************************
				function onuploadclose() {
					parent.ParentCallBackFunctionForClose();
					return false;
				}
				//**********************************************************************
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
