<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="APPNotificationFlightDet.aspx.vb"
    Inherits="Flypal.APPNotificationFlightDet" %>

<%@ Import Namespace="Flypal.Log" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0" />
    <title>Flight Notification</title>
    <meta content="Alix Mobile App" name="description" />
    <meta content="themepassion" name="author" />
    <!-- App Icons -->
    <link rel="apple-touch-icon" sizes="57x57" href="assets/images/icons/apple-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="assets/images/icons/apple-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="assets/images/icons/apple-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="assets/images/icons/apple-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="assets/images/icons/apple-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="assets/images/icons/apple-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="assets/images/icons/apple-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="assets/images/icons/apple-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="assets/images/icons/apple-icon-180x180.png">
    <link rel="icon" type="image/png" sizes="192x192" href="assets/images/icons/android-icon-192x192.png">
    <link rel="icon" type="image/png" sizes="32x32" href="assets/images/icons/favicon-32x32.png">
    <link rel="icon" type="image/png" sizes="96x96" href="assets/images/icons/favicon-96x96.png">
    <link rel="icon" type="image/png" sizes="16x16" href="assets/images/icons/favicon-16x16.png">
    <link rel="manifest" href="assets/images/icons/manifest.json">
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="msapplication-TileImage" content="assets/images/icons/ms-icon-144x144.png">
    <meta name="theme-color" content="#ffffff">
    <!-- CORE CSS FRAMEWORK - START -->
    <link href="assets/css/preloader.css" type="text/css" rel="stylesheet" media="screen,projection" />
    <link href="modules/materialize/materialize.min.css" type="text/css" rel="stylesheet"
        media="screen,projection" />
    <link href="modules/fonts/mdi/materialdesignicons.min.css" type="text/css" rel="stylesheet"
        media="screen,projection" />
    <link href="modules/perfect-scrollbar/perfect-scrollbar.css" type="text/css" rel="stylesheet"
        media="screen,projection" />
    <!-- CORE CSS FRAMEWORK - END -->
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - START -->
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->
    <!-- CORE CSS TEMPLATE - START -->
    <link href="assets/css/style.css" type="text/css" rel="stylesheet" media="screen,projection" />
    <%--id="main-style"--%>
    <!-- CORE CSS TEMPLATE - END -->
    <style type="text/css">
        table
        {
            background: transparent !important;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }
        table tr
        {
            background: transparent !important;
            border-width: 0px !important;
            border-style: none !important;
        }
        table td
        {
            padding: 15px 15px 0px 15px !important;
        }
    </style>
</head>
<body class="  html" data-header="light" data-footer="dark" data-header_align="center"
    data-menu_type="left" data-menu="light" data-menu_icons="on" data-footer_type="left"
    data-site_mode="light" data-footer_menu="show" data-footer_menu_style="light">
    <form id="form1" runat="server">
    <div class="preloader-background">
        <div class="preloader-wrapper">
            <div id="preloader">
            </div>
        </div>
    </div>
    <!-- START navigation -->
    <nav class="fix_topscroll logo_on_fixed  topbar navigation" role="navigation">
  <div class="nav-wrapper container">
    <%--<a id="logo-container" href="" class=" brand-logo " >BytzSoft</a>--%>    

     <asp:LinkButton ID="lnkNotificationList" runat="server" class="waves-effect waves-circle navicon left sidenav-trigger show-on-large" ><i class="mdi mdi-arrow-left"></i></asp:LinkButton>
    <%--<a href="APPNotificationList.aspx" class="waves-effect waves-circle navicon left"><i class="mdi mdi-arrow-left"></i></a>--%>


    <a href="#" data-target="slide-settings" class="waves-effect waves-circle navicon right sidenav-trigger show-on-large pulse"><i
        class="mdi mdi-settings-outline"></i></a>

    <a href="APPNotificationList.aspx" data-target="" class="waves-effect waves-circle navicon right nav-site-mode show-on-large"><i
        class="mdi mdi-invert-colors mdi-transition1"></i></a>

  </div>
</nav>
    <ul id="slide-settings" class="sidenav sidesettings right fixed">
        <li class="menulinks">
            <ul class="collapsible">
                <!-- SIDEBAR - START -->
                <!-- MAIN MENU - START -->
                <li class="sh-wrap">
                    <div class="subheader">
                        Themes</div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="red">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate red-text text-lighten-2"></i><span
                            class="title">Red</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="orange">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline deep-orange-text text-lighten-2">
                        </i><span class="title">Orange</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="blue">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline blue-text text-lighten-2"></i>
                            <span class="title">Blue</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="teal">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline teal-text text-lighten-2"></i>
                            <span class="title">Teal</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="pink">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline pink-text text-lighten-2"></i>
                            <span class="title">Pink</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="light-green">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline light-green-text text-lighten-2">
                        </i><span class="title">Light Green</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="purple">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline purple-text text-lighten-2"></i>
                            <span class="title">Violet</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="green">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline green-text text-lighten-2"></i>
                            <span class="title">Green</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings active" data-type="theme" data-value="deep-purple">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline deep-purple-text text-lighten-2">
                        </i><span class="title">Purple</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="amber">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline amber-text"></i><span class="title">
                            Yellow</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="indigo">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline indigo-text text-lighten-2"></i>
                            <span class="title">Indigo</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="blue-grey">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline blue-grey-text text-lighten-2">
                        </i><span class="title">Blue Grey</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="brown">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline brown-text text-lighten-2"></i>
                            <span class="title">Brown</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="cyan">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline cyan-text text-lighten-2"></i>
                            <span class="title">Cyan</span> </a>
                    </div>
                </li>
                <li class="lvl1  theme">
                    <div class="waves-effect appsettings " data-type="theme" data-value="grey">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline grey-text text-darken-2"></i><span
                            class="title">Black</span> </a>
                    </div>
                </li>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Site Mode</div>
                </li>
                <li class="lvl1  site_mode">
                    <div class="waves-effect appsettings active" data-type="site_mode" data-value="light">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Light
                            Mode</span> </a>
                    </div>
                </li>
                <li class="lvl1  site_mode">
                    <div class="waves-effect appsettings " data-type="site_mode" data-value="dark">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Dark
                            Mode</span> </a>
                    </div>
                </li>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Header Style</div>
                </li>
                <li class="lvl1  header">
                    <div class="waves-effect appsettings active" data-type="header" data-value="light">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Light
                            Header</span> </a>
                    </div>
                </li>
                <li class="lvl1  header">
                    <div class="waves-effect appsettings " data-type="header" data-value="dark">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Dark
                            Header</span> </a>
                    </div>
                </li>
                <li class="lvl1  header">
                    <div class="waves-effect appsettings " data-type="header" data-value="colored">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Colored
                            Header</span> </a>
                    </div>
                </li>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Header Alignment</div>
                </li>
                <li class="lvl1  header_align">
                    <div class="waves-effect appsettings " data-type="header_align" data-value="left">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Left Align
                            Header</span> </a>
                    </div>
                </li>
                <li class="lvl1  header_align">
                    <div class="waves-effect appsettings active" data-type="header_align" data-value="center">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Center
                            Align Header</span> </a>
                    </div>
                </li>
                <li class="lvl1  header_align">
                    <div class="waves-effect appsettings " data-type="header_align" data-value="right">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Right
                            Align Header</span> </a>
                    </div>
                </li>
                <li class="lvl1  header_align">
                    <div class="waves-effect appsettings " data-type="header_align" data-value="app">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">App Based
                            Align Header</span> </a>
                    </div>
                </li>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Menu Style</div>
                </li>
                <li class="lvl1  menu">
                    <div class="waves-effect appsettings active" data-type="menu" data-value="light">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Light
                            Menu</span> </a>
                    </div>
                </li>
                <li class="lvl1  menu">
                    <div class="waves-effect appsettings " data-type="menu" data-value="dark">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Dark
                            Menu</span> </a>
                    </div>
                </li>
                <li class="lvl1  menu">
                    <div class="waves-effect appsettings " data-type="menu" data-value="colored">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Colored
                            Menu</span> </a>
                    </div>
                </li>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Menu Icons</div>
                </li>
                <li class="lvl1  menu_icons">
                    <div class="waves-effect appsettings active" data-type="menu_icons" data-value="on">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Menu Icons
                            Show</span> </a>
                    </div>
                </li>
                <li class="lvl1  menu_icons">
                    <div class="waves-effect appsettings " data-type="menu_icons" data-value="off">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Menu
                            Icons Hide</span> </a>
                    </div>
                </li>
               <%-- <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Page Footer Style</div>
                </li>
                <li class="lvl1  footer">
                    <div class="waves-effect appsettings " data-type="footer" data-value="light">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Light
                            Footer</span> </a>
                    </div>
                </li>
                <li class="lvl1  footer">
                    <div class="waves-effect appsettings active" data-type="footer" data-value="dark">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Dark
                            Footer</span> </a>
                    </div>
                </li>
                <li class="lvl1  footer">
                    <div class="waves-effect appsettings " data-type="footer" data-value="colored">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Colored
                            Footer</span> </a>
                    </div>
                </li>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Page Footer Type</div>
                </li>
                <li class="lvl1  footer_type">
                    <div class="waves-effect appsettings " data-type="footer_type" data-value="minimal">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Minimal
                            Footer</span> </a>
                    </div>
                </li>
                <li class="lvl1  footer_type">
                    <div class="waves-effect appsettings active" data-type="footer_type" data-value="left">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Left
                            Aligned Footer</span> </a>
                    </div>
                </li>
                <li class="lvl1  footer_type">
                    <div class="waves-effect appsettings " data-type="footer_type" data-value="center">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Centered
                            Footer</span> </a>
                    </div>
                </li>--%>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Fixed Footer Menu</div>
                </li>
                <li class="lvl1  footer_menu">
                    <div class="waves-effect appsettings active" data-type="footer_menu" data-value="show">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Show Fixed
                            Footer Menu</span> </a>
                    </div>
                </li>
                <li class="lvl1  footer_menu">
                    <div class="waves-effect appsettings " data-type="footer_menu" data-value="hide">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Hide
                            Fixed Footer Menu</span> </a>
                    </div>
                </li>
                <li class="sep-wrap">
                    <div class="divider">
                    </div>
                </li>
                <li class="sh-wrap">
                    <div class="subheader">
                        Fixed Footer Menu Style</div>
                </li>
                <li class="lvl1  footer_menu_style">
                    <div class="waves-effect appsettings active" data-type="footer_menu_style" data-value="light">
                        <a href="#!"><i class="mdi mdi-checkbox-intermediate"></i><span class="title">Light
                            Fixed Menu</span> </a>
                    </div>
                </li>
                <li class="lvl1  footer_menu_style">
                    <div class="waves-effect appsettings " data-type="footer_menu_style" data-value="dark">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Dark
                            Fixed Menu</span> </a>
                    </div>
                </li>
                <%--<li class="lvl1  footer_menu_style">
                    <div class="waves-effect appsettings " data-type="footer_menu_style" data-value="colored">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Colored
                            Fixed Menu</span> </a>
                    </div>
                </li>--%>
                <!-- MAIN MENU - END -->
                <!--  SIDEBAR - END -->
            </ul>
        </li>
    </ul>
    <div class="container">
        <div class="section">
            <h5 class="pagetitle">
                Flight Notification</h5>
        </div>
    </div>
    <div class="container">
        <div class="section">
            <div class="divider">
            </div>
            <div class="title">
                <asp:Label ID="lblTotalrecordCount" runat="server" Text=""></asp:Label></div>
            <br />
            <ul class="events">
                <li class="event-item card" style="margin: -14px -14px -14px -14px; -webkit-box-shadow: 0 2px 2px 0 rgb(0 0 0 / 14%), 0 3px 1px -2px rgb(0 0 0 / 12%), 0 1px 5px 0 rgb(0 0 0 / 20%) !important;
                    box-shadow: 0 2px 2px 0 rgb(0 0 0 / 14%), 0 3px 1px -2px rgb(0 0 0 / 12%), 0 1px 5px 0 rgb(0 0 0 / 20%) !important;">
                    <div class="info">
                        <div class="row settings-row">
                            <div class="col s4">
                                <div class="title">
                                    Aircraft
                                </div>
                                <div class="time">
                                    <span id="spnRegNo" runat="server"></span>
                                </div>
                                <div class="title">
                                    Departure</div>
                                <div class="time">
                                  <span id="spnSourceName" runat="server"></span>  
                                </div>
                                <div class="time">
                                  <span id="spnSouDate" runat="server"></span>   
                                </div>
                            </div>
                            <div class="col s4">
                                <div class="title">
                                    Log No
                                </div>
                                <div class="time">
                                   <span id="spnLogTextNo" runat="server"></span> 
                                </div>
                                <div class="title">
                                    Arrival</div>
                                <div class="time">
                                   <span id="spnDestinationName" runat="server"></span>   
                                </div>
                                <div class="time">
                                    <span id="spnDesDate" runat="server"></span>   
                                </div>
                            </div>
                              <div class="col s4">
                               <div class="title">
                                    Log Page No
                                </div>
                                <div class="time">
                                   <span id="spnLogPageNo" runat="server"></span> 
                                </div>
                              </div>
                        </div>
                        <div class="row settings-row">
                            <div class="col s4">
                                <div class="title">
                                    Time in Air:</div>
                                <div class="time">
                                <span id="spnTimeInAir" runat="server"></span>     
                                </div>
                            </div>
                            <div class="col s4">
                                <div class="title">
                                    Pilot
                                </div>
                                <div class="time">
                                  <span id="spnPilot1Name" runat="server"></span>   
                            </div>
                        </div>
                    </div>
                </li>
                <%--<asp:LinkButton ID="lnkMarkReadNotification" runat="server" class='view-btn btn-small'>Ok, Got it !</asp:LinkButton>--%>
            </ul>
        </div>
    </div>
    <br />
    <br />
    <div class="backtotop">
        <a class="btn-floating btn primary-bg"><i class="mdi mdi-chevron-up"></i></a>
    </div>
    <div class="footer-menu circular">
        <ul>
           <li><a href="APPAircraftCurrentStatus.aspx" id="hrefTimeline" runat="server"><i id="iTimeline"
                runat="server" class="mdi mdi-chart-timeline" style="color: var(--primary-color);">
            </i><span>Aircraft Current</span></a></li>
            <li><a href="APPAircraftLogBook.aspx" id="hrefFlights" runat="server"><i id="iFlights"
                runat="server" class="mdi mdi-airplane" style="color: var(--primary-color);"></i>
                <span>Flights</span></a></li>
            <li><a href="javascript:return(0)" id="hrefAvailability" runat="server"><i id="iAvailability"
                runat="server" class="mdi mdi-account-check1" style="color: var(--primary-color);">
            </i><span>&nbsp;</span></a></li>
            <li><a href="APPProfile.aspx" id="hrefProfile" runat="server"><i id="iProfile" runat="server"
                class="mdi mdi-account" style="color: var(--primary-color);"></i><span>Profile</span></a></li>
            <li><a href="APPMenu.aspx" id="hrefHome" runat="server"><i id="iHome" runat="server"
                class="mdi mdi-home" style="color: var(--primary-color);"></i><span>Home</span></a></li>
        </ul>
    </div>
    <!-- Modal pop up -->
    <div id="divShowModelPopup" style="display: none;">
        <a id="AlertModelpopup" class="waves-effect waves-light btn modal-trigger accent-bg"
            href="#modal1533064022">Daily Deal Message123</a>
    </div>
    <div id="modal1533064022" class="modal accent-bg ">
        <div class="modal-content white-text">
            <h4 id="h4popuptitle">
                Session expired11..</h4>
            <p id="popupmsg">
                Session expired ! Please click on Home button on menu11.</p>
        </div>
    </div>
    <script type="text/javascript">

        function opennotificationpopup(Message, Title) {

            var mdlpopuptitle = document.getElementById('h4popuptitle').innerHTML = Title;
            var mdlpopupMsg = document.getElementById('popupmsg').innerHTML = Message;

            var mdl = document.getElementById('AlertModelpopup').click();
        };

    </script>
    <!-- PWA Service Worker Code -->
    <script type="text/javascript">
        // This is the "Offline copy of pages" service worker

        // Add this below content to your HTML page, or add the js file to your page at the very top to register service worker

        // Check compatibility for the browser we're running this in
        if ("serviceWorker" in navigator) {
            if (navigator.serviceWorker.controller) {
                console.log("[PWA Builder] active service worker found, no need to register");
            } else {
                // Register the service worker
                navigator.serviceWorker
      .register("pwabuilder-sw.js", {
          scope: "./"
      })
      .then(function (reg) {
          console.log("[PWA Builder] Service worker has been registered for scope: " + reg.scope);
      });
            }
        }
    </script>
    <!-- LOAD FILES AT PAGE END FOR FASTER LOADING -->
    <!-- CORE JS FRAMEWORK - START -->
    <script src="modules/jquery/jquery-2.2.4.min.js"></script>
    <script src="modules/materialize/materialize.js"></script>
    <script src="modules/materialize/select.js" type="text/javascript"></script>
    <script src="modules/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script src="assets/js/variables.js"></script>
    <!-- CORE JS FRAMEWORK - END -->
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - START -->
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->
    <script type="text/javascript">
        $(".modal").modal();
        //  document.addEventListener("DOMContentLoaded", function () {
        //   var Modalelem = document.querySelector(".modal");
        //   var instance = M.Modal.init(Modalelem);
        //   instance.open();
        // });
    </script>
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->
    <!-- CORE TEMPLATE JS - START -->
    <script src="modules/app/init.js"></script>
    <script src="modules/app/settings.js"></script>
    <script src="modules/app/scripts.js"></script>
    <!-- END CORE TEMPLATE JS - END -->
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            $('.preloader-background').delay(10).fadeOut('slow');
        });
    </script>
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - START -->
    <script type="text/javascript">
        $("select").formSelect();
    </script>
    <script type="text/javascript">
        $(".datepicker").datepicker();
        $(".datepicker.datepicker1").datepicker({
            autoClose: true
        });
        $(".datepicker.datepicker2").datepicker({
            format: "dddd, dd mmmm yyyy",
            autoClose: true
        });

        $(".datepicker.datepicker3").datepicker({
            isRTL: true,
            autoClose: true
        });
        $(".datepicker.datepicker4").datepicker({
            autoClose: true,
            firstDay: 1
        });

        $(".datepicker.datepicker5").datepicker({
            autoClose: true,
            showDaysInNextAndPreviousMonths: true
        });

        $(".datepicker.datepicker6").datepicker({
            autoClose: true,
            showClearBtn: true
        });
        $(".datepicker.datepicker7").datepicker({
            autoClose: true,
            format: "mm/dd/yyyy",
            autoClose: true
        });

        $(".datepicker.datepicker8").datepicker({
            autoClose: true,
            disableWeekends: true,
            firstDay: 1
        });
         
    </script>
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->
    </form>
</body>
</html>
