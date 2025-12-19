<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="Flypal.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta charset="UTF-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
	<meta name="robots" content="noindex" />
	<title>FlyPal</title>
	<link rel="stylesheet" href="https://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css" />
	<link rel="stylesheet" type="text/css" href="css/compiled/wizard.css" />
	<link href="css/libs/font-awesome.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-growl.css" />
	<link rel="stylesheet" type="text/css" href="bootstrap/bootstrap.min.css" />
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
	<link rel="stylesheet" type="text/css" href="css/libs/nanoscroller.css" />
	<link rel="stylesheet" type="text/css" href="css/compiled/login.css" />
	<link href='//fonts.googleapis.com/css?family=Open+Sans:400,600,700,300|Titillium+Web:200,300,400'
		rel='stylesheet' type='text/css' />
	<script type="text/javascript" src="js Bootstrap/demo-rtl.js"></script>
	<script src="js bootstrap/bootstrap.min.js" type="text/javascript"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

	<style type="text/css">
		div.transbox {
			background-color: rgba(255,255,255,0.88);
			-webkit-box-shadow: rgba(0,0,0,0.2) 0 1px 0 0;
			-moz-box-shadow: rgba(0,0,0,0.2) 0 1px 0 0;
			box-shadow: rgba(0,0,0,0.65) 0 1px 0 0;
			border-radius: 5px;
			-moz-border-radius: 5px;
			-webkit-border-radius: 5px;
			text-shadow: #b2e2f5 0 4px 0;
			height: 100%;
		}

			div.transbox p {
				font-weight: bold;
				color: #000000;
			}

		.modal-dialog {
			min-height: calc(100vh - 60px);
			display: flex;
			flex-direction: column;
			justify-content: center;
			overflow: auto;
		}

		.modal-content {
			-webkit-border-radius: 0px !important;
			-moz-border-radius: 0px !important;
			border-radius: 0px !important;
		}
	</style>

	<script type="text/javascript">

		var a = 0;

		function blinker() {

			$('.blink_me').fadeOut(500);
			$('.blink_me').fadeIn(500);

		}

		setInterval(blinker, 1500);

	</script>
	<script>
		sessionStorage.name = "GeekChamp";
	</script>

	<script type="text/javascript">

		function showmessage() {

			var animStyle = 'slide';

			$.ajax({

				type: "POST",
				url: "login.aspx/GetMessages",
				data: "{}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (msg) {

					$("#modalbody").text("(" + msg.d + ")");
					$('#msg1').notificationmsg({ animation: animStyle });
					$('#msg1').notificationmsg('show');
				},
				error: function (xhr, msg, e) {
					alert(msg);
				}
			});

		}

		function onClick(e) {

			var animStyle = 'slide';
			if (e.srcElement) {
				if (e.srcElement.id === 'btnfade') {
					animStyle = 'fade';
				}
				else if (e.srcElement.id === 'btnslide') {
					animStyle = 'slide';
				}
				else if (e.srcElement.id === 'btnLogin') {
					animStyle = 'slide';
				}
				else {
					animStyle = 'slidethru';
				}
			}
			else {
				if (e.target.id === 'btnfade') {
					animStyle = 'fade';
				}
				else if (e.target.id === 'btnslide') {
					animStyle = 'slide';
				}
				else if (e.target.id === 'btnLogin') {
					animStyle = 'slide';
				}
				else {
					animStyle = 'slidethru';
				}
			}
			$.ajax({
				type: "POST",
				url: "login.aspx/GetMessages",
				data: "{}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (msg) {
					$("#modalbody").text("(" + msg.d + ")");
					$('#msg1').notificationmsg({ animation: animStyle });
					$('#msg1').notificationmsg('show');
				},
				error: function (xhr, msg, e) {
					alert(msg);
				}
			});
		}

		function Center() {
			var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
			var height = document.documentElement.clientHeight + document.documentElement.scrollTop;

			var top = ((height + document.documentElement.scrollTop) / 2) - ($('#centerdiv').height() / 2) + "px";

			var left = (width / 2) - ($('#centerdiv').width() / 2) + "px";
			$('#centerdiv').css({ position: "absolute", top: top, left: left });
		}

		function openChangePasswordModalpopup() {
			$("#ChangePasswordModal").modal('show');
		}

		function hideChangePasswordModalpopup() {
			$("#ChangePasswordModal").modal('hide');
		}

		function openOTPModalpopup() {
			debugger;
			$("#OTPModal").modal('show');
		}

		function hideOTPModalpopup() {
			$("#OTPModal").modal('hide');
		}

		function openConfirmOkModalpopup() {
			$("#ConfirmOkModal").modal('show');
		}

		function hideConfirmOkModalpopup() {
			$("#ConfirmOkModal").modal('hide');
		}

		function opennotificationpopup(Message, Type) {

			// create the notification
			var notification = new NotificationFx({
				message: '<p>' + Message + '</p>',
				layout: 'growl',
				effect: 'scale',
				type: Type, // notice, warning, error or success
				ttl: 2500,
			});

			// show the notification
			notification.show();
		}

		function showAlert() {
			//$('.alert').removeClass('hide');

			$('#ErrorList').show('fade');
		}

		function hideAlert() {
			$('#ErrorList').hide('fade');
		}

		function showAlertChangePassword() {
			$('#ErrorListChangePassword').show('fade');
		}

		function hideAlertChangePassword() {
			$('#ErrorListChangePassword').hide('fade');
		}

		function showAlertOTP() {
			$('#ErrorListOTP').show('fade');
		}

		function hideAlertOTP() {
			$('#ErrorListOTP').hide('fade');
		}

		function showAlertChangePasswordSuccesss() {
			$('#SuccessListChangePassword').show('fade');
		}

		function hideAlertChangePasswordSuccesss() {
			$('#SuccessListChangePassword').hide('fade');
		}

	</script>

	<script>

		function button_click(objTextBox, objBtnID) {
			if (window.event.keyCode == 13) {
				document.getElementById(objBtnID).focus();
				document.getElementById(objBtnID).click();
			}
		}

	</script>
	<style type="text/css">
		.clsAjaxLoader {
			filter: Alpha(Opacity=50);
			opacity: 0.5;
		}

		.vertical-center {
			min-height: 100%;
			min-height: 100vh;
			display: flex;
			align-items: center;
		}

		body {
			height: 100%;
			width: 100%;
			background-color: white;
		}

		#countdown {
			position: relative;
			margin: auto;
			margin-top: -4px;
			height: 40px;
			width: 40px;
			text-align: center;
		}

		#countdown_number {
			color: Red;
			display: inline-block;
			line-height: 40px;
		}

		svg {
			position: absolute;
			top: 0;
			right: 0;
			width: 40px;
			height: 40px;
			transform: rotateY(-180deg) rotateZ(-90deg);
		}

		circle {
			stroke-dasharray: 113px;
			stroke-dashoffset: 0px;
			stroke-linecap: round;
			stroke-width: 2px;
			stroke: red;
			fill: none;
		}

		@keyframes countdown {
			from {
				stroke-dashoffset: 0px;
			}

			to {
				stroke-dashoffset: 113px;
				stroke: green;
			}
		}
	</style>

	<script type="text/javascript">
		if (window !== top) {
			top.location.href = location.href;
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<div id="login-full-wrapper" style="position: fixed; background-image: images\LoginImage.jpg">
			<div style="position: absolute; top: 0px;">
				<img alt="" src="images\LoginImage.jpg" width="1375px" />
			</div>
			<div class="container">
				<div id="Div5" class="row" runat="server" style="color: Black; color: red; font-size: medium;">
					<div class="alert alert-danger" role="alert" id="DivMarquee" runat="server" visible="false">
						<marquee id="lblMessage" scrolldelay="70" runat="server"></marquee>
					</div>
				</div>
			</div>
			<div runat="server" id="Div6" class="vertical-bottom" style="float: right; vertical-align: top; margin-top: -35px; margin-right: 20px">
				<div class="container">
					<div class="row">
						<div class="col-xs-12">
							<div id="login-box">
								<div id="login-box-holder">
									<div class="row">
										<div class="col-xs-12">
											<div id="login-logo" style="border: 1px solid #e1e1e1; border-top-width: 0px; 
													border-radius: 17px 17px 0 0; border-right-width: 5px;">
												<img style="margin-top: -15px" class="clsImage" alt="" src="images/FlyPal_Logo.png" />
											</div>
											<div id="login-box-inner" style="padding: 25px 25px; border-bottom-width: 8px; border-right-width: 5px;">
												<asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<div id="ErrorList" style="-webkit-box-shadow: 3px 4px 6px #999;" 
															class="alert alert-danger collapse">
															<asp:Label ID="lblUserNameError" runat="server" ForeColor="DarkRed" Text="" />
														</div>
													</ContentTemplate>
												</asp:UpdatePanel>
												<asp:UpdatePanel ID="upnlLoginForm" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<div id="Blink" runat="server" style="color: Black; color: red;
																font-size: small; margin-top: -10px; padding-bottom: 10px;">
															<asp:Label ID="lblmsgForOperator" runat="server" />
														</div>
														<asp:Panel ID="Panel1" runat="server" DefaultButton="lnkArrow">
															
															<asp:Panel ID="Panel2" runat="server" DefaultButton="btnLogin">
																
																<div class="input-group" style="background-color: #3498db">
																	
																	<asp:TextBox ID="txtUserName" BackColor="White" class="form-control" runat="server"
																		AutoCompleteType="enabled" placeholder="User Name" TabIndex="1" Font-Bold="true" />
																	
																	<asp:Label ID="lblInvalid" runat="server" ForeColor="DarkRed" CssClass="clsWidth"
																		Style="font-size: 7pt" />
																	
																	<span class="input-group-addon btn btn-primary btn-xs" id="spnUsername" runat="server"
																		style="margin-top: 0px; min-width: 43px;">
																		
																		<asp:LinkButton ID="lnkArrow" runat="server" class="btn btn-link" Font-Bold="True"
																			TabIndex="2">
																			<i class="fa fa-arrow-right"></i>
																		</asp:LinkButton>

																	</span>

																	<asp:CustomValidator ID="cvControlValidator" runat="server" ForeColor="DarkRed" CssClass="clsWidth"
																		ControlToValidate="txtUserName" Display="None" />
																</div>
																<div>

																	<div class="input-group" id="password" runat="server" visible="false" style="background-color: #3498db">
																		
																		<asp:TextBox ID="txtPassword" runat="server" class="form-control" placeholder="Password"
																			autocomplete="new-password" TextMode="Password" TabIndex="3" Font-Bold="true" />
																		
																		<span class="input-group-addon btn btn-primary btn-xs" style="margin-top: 0px; min-width: 43px;">
																			<asp:LinkButton ID="btnLogin" runat="server" class="btn btn-link" Font-Bold="True"
																				TabIndex="4">
																				<i class="fa fa-arrow-right"></i>
																			</asp:LinkButton>
																		</span>	
																		
																	</div>

																</div>
																<div>
																	<span id="capsWarning" style="color: red; display: none;">
																		Caps Lock is ON
																	</span>
																</div>
															</asp:Panel>
														</asp:Panel>
														<div class="row">
															<div class="col-xs-12">
																<asp:Button ID="btnCancel" runat="server" 
																	class="btn btn-primary col-xs-12" Text="Cancel"
																	Visible="false" />
																<asp:CheckBox ID="chkIsLocked" runat="server" Visible="False"
																	Checked="True" />
															</div>
														</div>
														<div class="row" style="display: none">
															<div class="col-xs-12">
																<asp:Label ID="lblReleaseVer_Date" class="social-text" 
																	runat="server" Text="Last Update Date: 19-Oct-2015"
																	Width="100%" Font-Size="X-Small" ForeColor="Gray" />
															</div>
														</div>
														<br />
														<div class="row">
															<div class="col-xs-12 col-sm-8">
																<asp:HyperLink ID="HyperLink1" runat="server" class="btn btn-lg btn-link" Target="_blank" ToolTip="Facebook"
																	NavigateUrl="http://www.facebook.com/bytzsoft" Style="padding-right: 0px;">
																	<i class="fa fa-facebook"></i>
																</asp:HyperLink>

																<asp:HyperLink ID="HyperLink2" runat="server" class="btn btn-lg btn-link" Target="_blank" ToolTip="Twitter"
																	NavigateUrl="http://twitter.com/iFlyPal/" Style="padding-right: 0px;">
																	<i class="fa fa-twitter"></i>
																</asp:HyperLink>

																<asp:HyperLink ID="HyperLink3" runat="server" class="btn btn-lg btn-link" Target="_blank" ToolTip="Linkedin"
																	NavigateUrl="https://www.linkedin.com/company/bytzsoft-technologies-pvt-ltd"
																	Style="padding-right: 0px;">
																	<i class="fa fa-linkedin"></i>
																</asp:HyperLink>

																<asp:HyperLink ID="HyperLink6" runat="server" class="btn btn-lg btn-link" Target="_blank" ToolTip="Instagram"
																	NavigateUrl="https://instagram.com/iflypal?igshid=YmMyMTA2M2Y=" Style="padding-right: 0px;">
																	<i class="fa fa-instagram"></i>
																</asp:HyperLink>

																<asp:HyperLink ID="HyperLink5" runat="server" class="btn btn-lg btn-link" Target="_blank" ToolTip="Youtube"
																	NavigateUrl="https://www.youtube.com/channel/UC4FKw2n8EgFgEuZlkd43yUA" Style="padding-right: 0px;">
																	<i class="fa fa-youtube-play"></i>
																</asp:HyperLink>
															</div>
														</div>
													</ContentTemplate>
												</asp:UpdatePanel>
												<div class="row">
													<div class="col-xs-12 col-sm-12 col-md-12">
														<div class="col-xs-6 col-sm-6 col-md-6" style="font-size: x-small; bottom: -17px;">
															Powered by
                                                        <img src="images/Azure.jpg" height="20px" />
														</div>
														<div class="col-xs-6 col-sm-6 col-md-6">
															<img src="images/Bytz%20logo-lighter.png" id="CompLogo" runat="server" class="pull-right "
																style="height: 50px;" />
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
								<div id="login-box-footer" style="background-color: #fff; border-radius: 10px 10px 10px 10px;">
									<div class="row">
										<div class="col-xs-12">
											<asp:Label ID="Label3" runat="server" Style="color: black;">Copyrights @ 2017 </asp:Label>
											<asp:HyperLink ID="HyperLink4" runat="server" Target="_blank" Style="color: black;"
												NavigateUrl="http://www.bytzsoft.aero">www.bytzsoft.aero</asp:HyperLink>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="divSpinner">

			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader">
					</div>
					<div class="divAjaxLoader">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
									ImageAlign="Middle" CssClass="ajax-loader-gif" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>

		</div>

		<%--Generate OTP--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyGenerateOTP" Text="Dummy GenerateOTP" />
		</div>
		<div id="mdlGenerateOTP" class="modal fade" role="dialog" tabindex="-1" aria-labelledby="myModalLabel"
			data-backdrop="static" data-keyboard="false" aria-hidden="true" style="top: 0px; overflow-y: hidden; display: none;">

			<asp:UpdatePanel runat="server" ID="upnlGenerateOTP" UpdateMode="Conditional">
				<ContentTemplate>
					<div class="modal-dialog modal-lg" style="top: 0px;">
						<div class="modal-content">
							<!-- Modal Header -->
							<div class="modal-header">
								<h4 class="modal-title">User Authentication</h4>
							</div>
							<div class="modal-body">
								<asp:UpdatePanel ID="upnlOTPError" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<div id="ErrorListOTP" role="alert" style="-webkit-box-shadow: 3px 4px 6px #999;"
											class="alert alert-danger collapse">
											<asp:Label ID="lblOTPError" runat="server" ForeColor="DarkRed" Text="" />
											<asp:Label ID="lblOTPErrorMsg" runat="server" class="clsLabelAuto" ForeColor="Red"
												Text="" Visible="false" />
										</div>
									</ContentTemplate>
								</asp:UpdatePanel>
								<div style="display: none">
									<asp:Button runat="server" ID="Button1" Text="Dummy GenerateOTP" />
								</div>
								<div style="width: 100%">
									<asp:UpdatePanel runat="server" ID="upnlOTP" UpdateMode="Conditional">
										<ContentTemplate>
											<div id="Div1">
												<div class="container">
													<div id="Div2">
														<div id="Div3">
															<div class="row">
																<div class="col-xs-6">
																	<div class="input-group">
																		<span class="input-group-addon"><i class="fa fa-user"></i></span>
																		<asp:TextBox ID="txtOTPUserName" BackColor="White" class="form-control" runat="server"
																			placeholder="User Name" ReadOnly="True" />
																	</div>
																</div>
															</div>
															<div class="row">
																<br />
																<div id="myWizard" class="wizard">
																	<div class="wizard-inner">
																		<ul class="steps">
																			<li data-target="#step1" class="active"><span class="badge badge-primary">1</span>Step
                                                                            1<span class="chevron"></span> </li>
																		</ul>
																		<div style="margin-top: 13px; padding-left: 150px;">
																			<asp:Label ID="lblGenerateOTPInfo" runat="server" class="clsLabel" Text="ccsxxx" />
																		</div>
																	</div>
																</div>
																<br />
																<div id="Div4" class="wizard">
																	<div class="wizard-inner">
																		<ul class="steps">
																			<li data-target="#step1" class="active"><span class="badge badge-primary">2</span>Step
                                                                            2<span class="chevron"></span> </li>
																		</ul>
																		<asp:Panel ID="Panel3" runat="server" DefaultButton="btnSubmitOTP">
																			<div style="margin-top: 13px; padding-left: 142px;">
																				<div class="row">
																					<div class="col-xs-4 col-sm-4 col-md-4">
																						Enter One Time Password(OTP):
																					</div>
																					<div class="col-xs-2 col-sm-2 col-md-2" style="margin-top: -4px;">
																						<asp:TextBox ID="txtGenerateOTP" runat="server" Enabled="false" MaxLength="25" Text=""
																							ToolTip="Enter OTP" TextMode="Password" class="form-control input-sm" />
																					</div>
																					<div class="col-xs-1 col-sm-1 col-md-2" style="margin-top: -4px;">
																						<asp:UpdatePanel ID="upnlOTPbuttons" runat="server" UpdateMode="Conditional">
																							<ContentTemplate>
																								<asp:Button ID="btnSubmitOTP" runat="server" CausesValidation="False" class="btn btn-primary btn-xs input-sm "
																									Enabled="false" Text="Verify" ToolTip="Click to Verify OTP" />
																							</ContentTemplate>
																						</asp:UpdatePanel>
																					</div>
																					<%--Added aat 15-4-2019 by Shraddha--%>
																					<div class="col-xs-3 col-sm-3 col-md-2" style="margin-top: -5px;" runat="server"
																						id="divCountdown">
																						<div id="countdown">
																							<div id="countdown_number" runat="server" visible="false">
																							</div>
																							<svg>
																								<circle r="18" cx="20" cy="20" id="svgcircle" runat="server" visible="false">
																								</circle>
																							</svg>
																						</div>
																					</div>
																					<div class="col-xs-2 col-sm-2 col-md-2" style="margin-top: 0px;">
																						<asp:Label ID="lblResendOTP" runat="server" Text="Resend OTP" Style="display: block;" />
																					</div>
																					<div class="col-xs-2 col-sm-2 col-md-2" style="margin-top: -6px;">
																						<asp:LinkButton ID="lnkResendOTP" runat="server" Style="display: none;" CssClass="btn btn-link"> Resend OTP</asp:LinkButton>
																					</div>
																					<%--------------------%>
																				</div>
																			</div>
																		</asp:Panel>
																	</div>
																</div>
																<div class="checkbox-nice">
																</div>
																<div id="Div7">
																	<div class="row">
																		<div class="col-xs-12">
																			<div class="checkbox-nice">
																				<asp:CheckBox ID="chkSafeDevice" runat="server" Font-Bold="true" Text="This is not safe browser." />
																				<%--<label for="terms-cond">
                                                    I accept terms and conditions
                                                </label>--%>
																			</div>
																		</div>
																	</div>
																</div>
															</div>
															<div class="row">
																<table class=" alert alert-info">
																	<tr>
																		<td style="vertical-align: top;">
																			<b>Note:&nbsp;&nbsp;</b>
																		</td>
																		<td>
																			<asp:Label ID="lblNote" runat="server" class="clsLabelAuto" Text="" />
																		</td>
																	</tr>
																</table>
															</div>
															<div class="row">
																<div class="col-xs-4">
																</div>
																<div class="col-xs-4">
																	<asp:Label ID="lblUserInfoOTP" runat="server" class="clsLabelAuto" Text="" ForeColor="Red" />
																</div>
															</div>
															<%--<div class="alert alert-info">
                                                    <i class="fa fa-info-circle fa-fw fa-lg"></i><strong>Note :</strong> Please ensure
                                                    your email id is registered and valid. Having difficulty with OTP generation? Kindly
                                                    contact with our support team at customersupport@bytzsoft.com
                                                </div>--%>
														</div>
													</div>
												</div>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</div>
							</div>
							<!-- Modal body -->
						</div>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>

		</div>
		<%--End of Generate OTP--%>

		<%--Change Password OTP--%>
		<asp:UpdatePanel runat="server" ID="upnlChangePasswordOTP" UpdateMode="Conditional">
			<ContentTemplate>
				<div style="width: 100%">
					<div class="modal fade" id="ChangePasswordModal" role="dialog" tabindex="-1" aria-labelledby="myModalLabel"
						data-backdrop="static" data-keyboard="false" aria-hidden="true" style="top: 0px; overflow-y: hidden;">
						<div class="modal-dialog  modal-sm" style="top: 0px; overflow-y: hidden;">
							<div class="modal-content" style="background-color: snow;">
								<!-- Modal Header -->
								<div class="modal-header">
									<button type="button" class="close" data-dismiss="modal">
										&times;</button>
									<h4 class="modal-title">You are required to change the Password</h4>
								</div>
								<!-- Modal body -->
								<div class="modal-body">
									<asp:UpdatePanel ID="upnlChangePasswordError" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<div id="ErrorListChangePassword" role="alert" style="-webkit-box-shadow: 3px 4px 6px #999;"
												class="alert alert-danger collapse">
												<asp:Label ID="lblChangePasswordError" runat="server" ForeColor="DarkRed" Text="" />
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
									<div style="display: none">
										<asp:Button runat="server" ID="btnDummyChangePassword" Text="Check" />
									</div>
									<%--style="display: none"--%>
									<asp:UpdatePanel ID="upnlChangePassword" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Panel ID="Panel4" runat="server" DefaultButton="btnChangePasswordSave">
												<div class="container">
													<div class="row">
														<div class="col-xs-12">
															<div class="row">
																<div class="col-xs-12">
																	<div class="input-group">
																		<span class="input-group-addon"><i class="fa fa-user"></i></span>
																		<asp:TextBox runat="server" ID="txtResetPasswordUser" Text="" class="form-control"
																			Enabled="False" placeholder="User Name" />
																	</div>
																	<br />
																	<div class="input-group">
																		<span class="input-group-addon"><i class="fa fa-lock"></i></span>
																		<asp:TextBox runat="server" ID="txtResetPasswordOldPassword" Text="" class="form-control"
																			placeholder="Old Password" Enabled="False" />
																	</div>
																	<br />
																	<div class="input-group">
																		<span class="input-group-addon"><i class="fa fa-lock"></i></span>
																		<asp:TextBox ID="txtResetPasswordNewPassword" runat="server" class="form-control"
																			placeholder="New Password" Text="" TextMode="password" />
																	</div>
																	<br />
																	<div class="input-group">
																		<span class="input-group-addon"><i class="fa fa-lock"></i></span>
																		<asp:TextBox ID="txtResetPasswordConfirmPassword" runat="server" class="form-control"
																			placeholder="Confirm Password" Text="" TextMode="Password" />
																	</div>
																	<br />
																	<div class="row">
																		<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<div class="col-xs-6">
																					<asp:Button runat="server" ID="btnChangePasswordSave" Text="Save" class="btn btn-primary col-xs-12 btn-sm " />
																				</div>
																				<div class="col-xs-6">
																					<asp:Button ID="btnChangePasswordCancel0" runat="server" Text="Cancel" class="btn btn-primary col-xs-12 btn-sm" />
																				</div>
																				<div class="col-xs-4">
																					<div style="display: none">
																						<asp:Button runat="server" ID="btnChangePasswordCancel" Text="Cancel" class="btn btn-primary col-xs-12 btn-sm" />
																					</div>
																				</div>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</div>
																</div>
															</div>
														</div>
													</div>
												</div>
											</asp:Panel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</div>
							</div>
						</div>
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
		<%--End of Change Password OTP--%>

	<script type="text/javascript">
		var d = new Date();
		document.getElementById("Label3").innerHTML = 'Copyrights @ ' + d.getFullYear() + ' Bytzsoft Technologies Pvt. Ltd.';
	</script>

	<script type="text/javascript">

		function openOTPModalpopup() {
			$("#mdlGenerateOTP").modal('show');
		}
		function hideOTPModalpopup() {
			$("#mdlGenerateOTP").modal('hide');
		}
		function openChangePasswordModalpopup() {
			$("#ChangePasswordModal").modal('show');
		}
		function hideChangePasswordModalpopup() {
			$("#ChangePasswordModal").modal('hide');
		}
		function showAlertChangePassword() {
			$('#ErrorListChangePassword').show('fade');
		}
		function hideAlertChangePassword() {
			$('#ErrorListChangePassword').hide('fade');
		}

	</script>

	<script type="text/javascript">

		try {

			const pwd = document.getElementById("txtPassword");
			const warn = document.getElementById("capsWarning");

			if (!pwd) console.log("txtPassword not found.");
			if (!warn) console.log("capsWarning not found.");

			pwd.addEventListener("keyup", function (e) {

				try {

					const caps = e.getModifierState && e.getModifierState("CapsLock");
					warn.style.display = caps ? "inline" : "none";

					console.log("KeyUp event fired. CapsLock:", caps);

				} catch (innerErr) {
					console.error("Error inside KeyUp handler:", innerErr);
				}
			});

			console.log("CapsLock script initialized.");

		} catch (err) {
			console.error("Fatal script error:", err);
		}

	</script>

	</form>
</body>
</html>
