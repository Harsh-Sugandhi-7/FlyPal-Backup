<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TopHeader.aspx.vb" Inherits="Flypal.TopHeader" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Import Namespace="Flypal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
	<title>TopHeader1</title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	<link rel="stylesheet" type="text/css" href="JQGridReq/jqueryui/1.8.23/jquery-ui.css" />
	<link id="MainStyle" type="text/css" rel="stylesheet">	
	<script src="json2.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery/1.8.1/jquery.js" type="text/javascript"></script>
	<script src="JQGridReq/jqueryui/1.8.23/jquery-ui.js" type="text/javascript"></script>
	<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {

			$("#menu li > label").click(function (e) {

				$parent = $(this).parent("li");
				$children = $parent.children("Div");

				if ($parent.hasClass('Active')) {
					$children.slideUp('medium');
					$parent.removeClass('Active');
				}
				else {
					$("#menu li > div").slideUp('medium');
					$("#menu li").removeClass('Active');
					$children.slideDown('medium');
					$parent.addClass('Active');
				}

			});

		});
	</script>
	<script type="text/javascript">
		if ("<%= not HttpContext.Current.Session("StyleSheet") is nothing %>" == "True") {
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
		}
	</script>
	<script type="text/javascript">
		function delete_cookie() {
			document.cookie('noShowInvStickynote', null);
		}
		function openHelp() {
			window.open("FLYPAL HELP/hh_start.htm", "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openledgersame1(FileName) {
			window.open(FileName, "main", "width=auto,height=auto");
		}
	</script>
	<script type="text/javascript">
		$(document).ready(function () {
			$("#toggleSwitch_j").hover(
				function () {
					$("#theBox_3").slideDown(500);
				}, function () {
					$("#theBox_3").slideUp(500);
				});


			$("#StayOpen").hover(
				function () {
					$("#theBox_2").slideDown(500);
				}, function () {
					$("#theBox_2").slideUp(500);
				});
		});

	</script>
	<style type="text/css">
		#theBox_3, #theBox_2 {
			display: none;
			width: 145px;
			height: auto;
		}
	</style>
</head>
<body ms_positioning="GridLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="5">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table border="0" class="clsHeaderTableOut">
			<tr>
				<td>
					<table>
						<tr>
							<td>
								<img class="clsImage" alt="" src="images\FlyPal_Logo.png" height="70">&nbsp;
                            <asp:Image ID="FlagImage" runat="server" ImageUrl="images\indian-flag.gif" />
								<asp:CheckBox ID="chkIsLocked" runat="server" Visible="False" Checked="True" />
							</td>
						</tr>
					</table>
				</td>
				<td class="clsHyperlink" valign="top" style="margin-top: 0px; padding-top: 0px" align="center">
					<asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="Red" CssClass="clsLabelAuto" />
					<asp:Label ID="lblLeaseNotification" runat="server" Visible="False" ForeColor="Red"
						CssClass="clsLabelAuto" />
					<asp:Label ID="lblPassExpiryInfo" runat="server" Visible="False" ForeColor="Red"
						CssClass="clsLabelAuto" />
					<table border="0" id="Table1">
						<tr>
							<td align="center" class="clstablecell" style="height: 5px">
								<asp:ImageButton ID="lnkProfilesbtn" ToolTip="Profiles" ImageUrl="icons/profile.png"
									runat="server" Class="clsHyperlink1" />
							</td>
							<td align="center" class="clstablecell" style="height: 5px">
								<asp:ImageButton ID="lnkHelpbtn" ToolTip="Help" ImageUrl="icons/help.png" runat="server"
									Class="clsHyperlink1" />
							</td>
							<td align="center" class="clstablecell" style="height: 5px">
								<asp:ImageButton ID="lnkAboutFlyPalbtn" ToolTip="About FlyPal" ImageUrl="icons/about.png"
									runat="server" Class="clsHyperlink1" />
							</td>
							<td align="center" class="clstablecell" style="height: 5px">
								<asp:ImageButton ID="lnkLogoutbtn" ToolTip="Logout" ImageUrl="icons/logout.png" runat="server"
									Class="clsHyperlink1" />
							</td>
							<td align="center" class="clstablecell" style="border-color: White; height: 5px">
								<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="DashboardForInventory.aspx"
									ImageUrl="icons/search.png" Target="main" ClientIDMode="Static" EnableTheming="True" />
							</td>
						</tr>
					</table>
				</td>
				<td align="right" valign="center">
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
						<ContentTemplate>
							<table>
								<tr>
									<td align="left" valign="center">
										<asp:UpdatePanel ID="UpdatePanel4" runat="server">
											<ContentTemplate>
												<img class="clsImage" runat="server" id="imgClientLogo" src="" alt="" height="65" />&nbsp;
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									</td>
									<td align="Left" class="clstablecell" valign="top">
										<table width="100%">
											<tr>
												<td class="clstablecell">
													<asp:HyperLink ID="hylnktDashBoardforTodoList" runat="server"
														NavigateUrl="DashBoardForTodoList.aspx"
														ToolTip="To Do List" ImageUrl="icons/ToDoList.jpg" Target="main"
														Visible='<%# CBool(AppSettings("ShowDashBoard")) %>' />
												</td>
												<td class="clstablecell">
													<asp:HyperLink ID="hylnktDashBoard" runat="server"
														NavigateUrl="DashBoard.aspx?IsFromTopHeaderID=1"
														ToolTip="DASHBOARD" ImageUrl="icons/Dashboard.png" Target="main" />
												</td>
												<td class="clstablecell">
													<asp:HyperLink ID="hylnktWODashBoard" runat="server"
														NavigateUrl="DashBoardWO.aspx"
														ToolTip="WORK ORDER DASHBOARD" ImageUrl="icons/Pie.jpg" Target="main" />
												</td>
												<td class="clstablecell">
													<asp:HyperLink ID="hylnkStickyNote" runat="server" NavigateUrl="StickyNote.aspx"
														ToolTip="StickyNote" ImageUrl="icons/post-it.png" Target="main" />
												</td>
											</tr>
										</table>
									</td>
									<td valign="top" align="right">
										<ul id="menu" class="accordion" style="margin-top: 0px; padding-top: 0px;">
											<li>
												<div id="StayOpen">
													<label id="toggleSwitch_2" class="menuHeading" style="width: 163px">
														&#9992; Recent Report(s) &#x25BC;</label>
													<div id="theBox_2">
														<ul class="menu" style="height: 48px; width: 165px">
															<% Dim Child3 As RecentlyUsedMenuItemList.RecentlyUsedMenuItemListInfo%>
															<% For Each Child3 In objModuleList%>

																<% If Child3.MainMenu <> "" Then%>

																	<li class="listItem">

																		<a href="<%= Child3.URL  %>" target="main" class="menulink">
																			<%= Child3.SubMenu %>
																		</a>

																	</li>

																<% End If%>

															<% Next%>
														</ul>
													</div>
												</div>
											</li>
										</ul>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
		</table>
	</form>
	<script type="text/javascript" language="javascript">

		//this function takes a value (ltext) and transmits that to the left hand frame
		function tranRight(ltext) {
			parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;
		}

		//this takes two values from textboxes and opens a new web form with query strings attached
		function openUC() {
			msgWindow = window.open("underC.htm", "", "fullscreen=no,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=500,height=400");
		}

		function openCL() {
			window.open("wfCompanieslist.aspx", "_top", "fullscreen=no,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto");
		}

	</script>
	<script type="text/javascript">
		function OpenProfile() {
			window.open("Profile.aspx", "_top", "fullscreen=no,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto");
		}
	</script>
</body>
</html>
