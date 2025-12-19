<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainMenu.aspx.vb" Inherits="Flypal.MainMenu" %>

<%@ Import Namespace="SI.UTILITY" %>
<!DOCTYPE HTML  PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
	<title>MainMenu</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<meta content="JavaScript" name="vs_defaultClientScript" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />	
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
	<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" rel="stylesheet" />
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>

	<script type="text/javascript">

		$(document).ready(function () {
			$("#menu  li > div").hide();
			$("#menu1  li > div").hide();
		});

		$(document).ready(function () {
			$("#menu li > label").click(function (e) {
            <%Session("ShowDashboardOnLogin") = "" %>
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

		$(document).ready(function () {
			$("#menu1 li > label").click(function (e) {
            <%Session("ShowDashboardOnLogin") = "" %>
				$parent = $(this).parent("li");
				$children = $parent.children("Div");

				if ($parent.hasClass('Active')) {
					$children.slideUp('medium');
					$parent.removeClass('Active');
				}
				else {
					$("#menu1 li > div").slideUp('medium');
					$("#menu1 li").removeClass('Active');
					$children.slideDown('medium');
					$parent.addClass('Active');
				}

			});
		});

		if ("<%= not HttpContext.Current.Session("StyleSheet") is nothing %>" == "True") {
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
		}

	</script>
	<style type="text/css" id="searchBoxImage">

        .clsTextBoxTagSearch {
            font-family: 'Inter', sans-serif;
            font-size: 12px;
            width: 115px;
            z-index: 0;
            border: 0px none;
            background: #F1F1F1 url(images/MenuSearch.png) no-repeat;
            background-position: 3px -8px !important;
            padding: 2px 4px 5px 25px; 
            -moz-box-shadow: 0 1px 1px #ccc inset, 0 1px 0 #fff;
            -webkit-box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
            box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
            background-color: White;
        }

	</style>
	<style>
    .raise-ticket-btn {
        background:linear-gradient(90deg, #8e2de2, #ff6a00);/*linear-gradient(90deg, #8e2de2, #ff6a00);*/
        color: #fff !important;
        font-weight: 600;
        padding: 8px 14px;
        border-radius: 6px;
        text-decoration: none;
        display: inline-flex;
        align-items: center;
        gap: 6px;
        box-shadow: 0 3px 8px rgba(0,0,0,.2);
        animation: pulse 1.8s infinite;
    }

        .raise-ticket-btn i {
            font-size: 1rem;
        }

    @keyframes pulse {
        0% {
            transform: scale(1);
            box-shadow: 0 0 0 rgba(238,9,121, 0.4);
        }

        50% {
            transform: scale(1.05);
            box-shadow: 0 0 12px rgba(238,9,121, 0.6);
        }

        100% {
            transform: scale(1);
            box-shadow: 0 0 0 rgba(238,9,121, 0.4);
        }
    }
</style>
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="0" topmargin="0">
	<form id="Form1" method="post" runat="server">
		<table id="Table1">
			<tr>
				<td style="width: 100%" align="center">
					<table style="width: 100%; background-color: #4A63A0; border-top-left-radius: 5px;
									border-top-right-radius: 5px; 
									border-bottom-left-radius: 5px;
									border-bottom-right-radius: 5px;">
						<tr>
							<td align="center">
								<div>
									<asp:Image ID="MyImage1" runat="server" Style="border-radius: 50%;" CssClass="clsRoundedImage"
										Width="43px" Height="43px"></asp:Image>
								</div>
							</td>
							<td valign="top" align="center">
								<table style="width: 100%;">
									<tr>
										<td>
											<div>
												<span class="clsLabelauto" style="font-weight: 500; color: white; font-size: smaller">
													<%=  HttpContext.Current.User.Identity.Name %>
												</span>
											</div>
											<div>
												<span class="clsLabelauto" style="color: white; font-size: xx-small">
													<%= mEventLog.LogInTimeFormatted  %> (UTC)</span>
											</div>
										</td>
									</tr>
								</table>
							</td>
							<td valign="top">
								<div id="divHideMenu" style="font-size: 28px; padding: 2px; cursor: pointer; text-align: right; color: white; overflow: hidden;"
									title="click to hide menu" onclick="setfullscreen();">
									&#8810;
								</div>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td width="100%">
					<a target="main" href="wfCreateTicket.aspx" style="width: 118px;" class="raise-ticket-btn">
						<i class="fa-solid fa-bolt"></i>Raise Ticket          
					</a>
				</td>
			</tr>
			<tr>
				<td width="100%">
					<span id="Label1" class="clsdgHeader" style="display: none;"></span>
				</td>
			</tr>
			<tr>
				<td width="100%">
					<div class="divSeacrhBox clsTextBoxTagSearch">
						<div style="width: 118px;">
							<input type="text" id="txtsearch" autocomplete="off" style="border: 0px none; width: 118px;" placeholder="Search">
						</div>
					</div>
					<div style="padding-left: 2px; padding-top: 3px;">
						<ul id="SearchList" style="display: none; overflow-x: hidden; width: 150px;">
						</ul>
					</div>
				</td>
			</tr>
			<tr>
				<td valign="top" align="left">
					<ul id="menu1" class="accordion">
						<li>
							<label class="menuHeading">
								<span class="fa fa-star circle-icon fa-spin fa-5x" style="font-size: 14px; color: white"></span>FAVOURITES
							</label>
							<% For Each item In mUserFavouritesListLinq%>
							<div>
								<ul class="menu">
									<% For Each item1 In item.SubMenuCollection%>
									<li class="listItem"><a href="<%= item1.URL  %>" target="main" class="menulink"><span
										class="fa fa-star circle-icon  fa-5x" style="font-size: 10px; color: white"></span>
										<%= item1.SubMenu%></a></li>
									<% Next%>
								</ul>
							</div>
							<% Next%>
						</li>
					</ul>
					<ul id="menu" class="accordion" style="margin-top: -17px">
						<% For Each item In objModuleListLinq%>
						<li>
							<label class="menuHeading">
								&#9992;
                            <%= item.MainMenu%></label>
							<div>
								<ul class="menu">
									<% For Each item1 In item.SubMenuCollection%>
									<li class="listItem"><a href="<%= item1.URL  %>" target="main" class="menulink">&#x25B6;
                                    <%= item1.SubMenu%></a> </li>
									<% Next%>
								</ul>
							</div>
						</li>
						<% Next%>
					</ul>
				</td>
			</tr>
		</table>
	</form>
	<script type="text/javascript">

		function setfullscreen() {

			var doc = parent.document;
			$('#mainframeset', doc).attr('cols', '18,84%');
			$('html').css('overflow-y', 'hidden');
			$('html').css('overflow-x', 'hidden');
			var elem = $('<label>');
			elem.attr('id', 'collapselabel');
			elem.attr('title', 'click to Show Menu');
			var left = $(window).scrollLeft();
			elem.css({ 'width': '17px', 'height': '100%', 'text-align': 'center', 'font-family': 'Segoe UI , Open Sans , Verdana, Arial, Helvetica, sans-serif', 'font-weight': '600', 'z-index': '9999', 'font-size': '18px', 'vertical-align': 'middle', 'position': 'absolute', 'left': left, 'top': '0px', 'border': '1px solid #ccc', 'background-color': '#4A63A0', 'display': 'inline-block', 'cursor': 'pointer', 'color': 'white' });
			elem.html('&#8811;');
			elem.appendTo('body');

		}

		$(document).ready(function () {
			$("#collapselabel").live('click', function () {
				var doc = parent.document;
				$('#mainframeset', doc).attr('cols', '180,84%');
				$('html').css('overflow-y', 'auto');
				$('html').css('overflow-x', 'auto');
				$(this).remove();

			});
		});

		/*Menu Search*/

		$(document).ready(function () {

			$("#menu a").each(function () {
				//Adding each link name in data(cache) to use it in later stage...
				$(this).data("search", $(this).html());
			});

		});

		/*End*/

		$(document).ready(function () {

			$("#txtsearch").keyup(function () {

				$("#SearchList").empty();
				var val = $.trim($(this).val());
				if (val != "") {

					$("#menu li a").each(function () {

						var data = $(this).data("search");

						if (typeof (data) !== 'undefined') {

							var tempval = data;
							if (tempval.toLowerCase().indexOf(val.toLowerCase()) >= 0) {

								//Highlight the input text
								tempval = tempval.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + val.replace(/([\^\$\(\)\[\]\{\}\*\.\+\?\|\\])/gi, "\\$1") + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
								//creating Div element to show site map of menu
								var element = $("<Div/>");
								element.css({ 'margin': '0 0 0 0' });
								var childelem = $("<ul/>");
								childelem.css({ 'list-style': 'none', 'display': 'block', 'margin-left': '-1px', 'padding-left': '0px', 'overflow-x': 'hidden' });
								var navigationpath = ''; //to show site map of menu
								var divid = '';
								//find the menu label and create site map with text of label
								$(this).parents('div').prev().each(function (index) {

									if (index == 0) {
										navigationpath = $(this).text();
										divid = navigationpath.replace(/\s/g, '').replace('(', '').replace(')', '').replace('/', '');
									}
									else {
										divid = divid + $(this).text();
										navigationpath += ' > ' + $(this).text();
										divid = navigationpath.replace(/\s/g, '').replace('(', '').replace(')', '').replace('/', '');
									}

								});

								var pathexists = false; // to add items in related menu first check if menu exist ...
								if ($("#SearchList").children('#' + divid).length == 1) {
									pathexists = true;
								}
								else {//add newly created div and ul to site map list.

									element.addClass('menuHeading');
									//add navigation path to current div's title attribute , so can be checked for uniqueness in later stage...
									element.attr('title', navigationpath);
									element.html(navigationpath);
									element.appendTo($("#SearchList"));
									element.attr('id', divid);
									pathexists = false;

								}
								var listitem = $(this).parent("li").clone(true).attr('class', 'listItem'); //.css({ 'margin': '0 0 0 0', 'padding-left': '0px'});
								listitem.children('a').html(tempval); //.addClass("sublistItem");

								if (pathexists) {
									//add item to existing menu div
									listitem.appendTo($("#SearchList").children('#' + divid).children('ul'));
								}
								else {
									//add ul to new menu div
									childelem.appendTo(element);
									//add item to new ul
									listitem.appendTo($(element).children('ul'));
								}
							}
						}
					});
				}

				if ($("#SearchList").children().length > 0) {
					$("#SearchList").addClass("menu");
					$("#SearchList").css({ 'maxHeight': '200px', 'overflow': 'auto', 'overflow-x': 'hidden' });
					$("#SearchList").slideDown();
				}
				else {
					$("#SearchList").removeClass("menu");
					$("#SearchList").slideUp();
				}

			});
		});

		/*Hide Menu Animation*/

		$(document).ready(function () {

			//Added by Saylee on 30-Oct-2023, as per YA/TA requirement
			var ClientCode = "<%=System.Configuration.ConfigurationManager.AppSettings("ClientCode").ToString() %>";
			if ((ClientCode != 'YA' & ClientCode != 'TA')) {
				$("#txtsearch").focus();
			}

			setTimeout(animatefunction, 1000);
		});

		function animatefunction() {
			var childer = $("#divHideMenu").children();
			var len = childer.length - 1;
			var templen = len;
			setInterval(function () {
				var i = templen;
				var csscolor = $(childer[i]).css('border-right-color');
				var color = $("#Label1").css('background-color');
				if (csscolor == color) {
					$(childer[i]).css('border-right', '7px solid #ddd');
				}
				else {
					$(childer[i]).css('border-right', '7px solid ' + color);
				}
				if (templen === 0) {
					templen = len;
				}
				else {
					templen = templen - 1;
				}

			}, 80);
		}

	</script>
</body>
</html>
