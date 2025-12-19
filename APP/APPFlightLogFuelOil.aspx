<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="APPFlightLogFuelOil.aspx.vb" Inherits="Flypal.APPFlightLogFuelOil" %>

<!DOCTYPE html>
<%@ Import Namespace="Flypal.LogList" %>
<%@ Import Namespace="Flypal.Log" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0" />
    <title>Aircraft Log Fuel Oil</title>
    <meta content="Alix Mobile App" name="description" />
    <meta content="themepassion" name="author" />
    <link href="../AutoComplete/jquery.autocomplete.css" rel="stylesheet" />

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
        table {
            background: transparent !important;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }

            table tr {
                background: transparent !important;
                border-width: 0px !important;
                border-style: none !important;
            }

            table td {
                padding: 15px 15px 0px 15px !important;
            }
    </style>

    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
</head>
<body class="html" data-header="light" data-footer="dark" data-header_align="center"
    data-menu_type="left" data-menu="light" data-menu_icons="on" data-footer_type="left"
    data-site_mode="light" data-footer_menu="show" data-footer_menu_style="light">
    <form id="form1" runat="server">
        <div class="preloader-background">
            <div class="preloader-wrapper">
                <div id="preloader">
                </div>
            </div>
        </div>

           <ul id="slide-settings" class="sidenav sidesettings right fixed">
       <li class="menulinks">
           <ul class="collapsible">
               <!-- SIDEBAR - START -->
               <!-- MAIN MENU - START -->
               <li class="sh-wrap">
                   <div class="subheader">
                       Themes
                   </div>
               </li>
               <li class="lvl1  theme">
                   <div class="waves-effect appsettings " data-type="theme" data-value="red">
                       <a href="#!"><i class="mdi mdi-checkbox-intermediate red-text text-lighten-2"></i><span
                           class="title">Red</span> </a>
                   </div>
               </li>
               <li class="lvl1  theme">
                   <div class="waves-effect appsettings " data-type="theme" data-value="orange">
                       <a href="#!"><i class="mdi mdi-checkbox-blank-outline deep-orange-text text-lighten-2"></i><span class="title">Orange</span> </a>
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
                       <a href="#!"><i class="mdi mdi-checkbox-blank-outline light-green-text text-lighten-2"></i><span class="title">Light Green</span> </a>
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
                       <a href="#!"><i class="mdi mdi-checkbox-blank-outline deep-purple-text text-lighten-2"></i><span class="title">Purple</span> </a>
                   </div>
               </li>
               <li class="lvl1  theme">
                   <div class="waves-effect appsettings " data-type="theme" data-value="amber">
                       <a href="#!"><i class="mdi mdi-checkbox-blank-outline amber-text"></i><span class="title">Yellow</span> </a>
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
                       <a href="#!"><i class="mdi mdi-checkbox-blank-outline blue-grey-text text-lighten-2"></i><span class="title">Blue Grey</span> </a>
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
                       Site Mode
                   </div>
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
                       Header Style
                   </div>
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
                       Header Alignment
                   </div>
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
                       Menu Style
                   </div>
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
                       Menu Icons
                   </div>
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
               <%--  <li class="sep-wrap">
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
                       Fixed Footer Menu
                   </div>
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
                       Fixed Footer Menu Style
                   </div>
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

        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" EnablePartialRendering="true"
            AsyncPostBackTimeout="1500">
        </asp:ScriptManager>


        <div class="container" style="margin-left:-2px">
            <div class="row col s12">
                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h5 runat="server" id="lblTitle">Aircraft Log Book</h5>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>



            <div class="row">
                <div class="input-field col s6">
                    <asp:DropDownList ID="cmbFuelType" runat="server"
                        DataTextField="Name" DataValueField="Id" SelectedValue="<%# mLog.FuelUpLifts.CurrentItem.FuelTypeID %>">
                    </asp:DropDownList>
                    <label for="cmbFuelType" class="active glyphicon-star">
                        Fuel Type</label>
                </div>
            </div>

            <div class="row" style="margin-top: 10px; text-align: right;">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlSave">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="btnSave" class="btn btn-link"><span class="fa fa-save" ></span> Save</asp:LinkButton>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <br />
        <br />
        <div class="backtotop">
            <a class="btn-floating btn primary-bg"><i class="mdi mdi-chevron-up"></i></a>
        </div>
        <!-- Modal pop up -->
        <div id="divShowModelPopup" style="display: none;">
            <a id="AlertModelpopup" class="waves-effect waves-light btn modal-trigger accent-bg"
                href="#modal1533064022">Daily Deal Message123</a>
        </div>
        <div id="modal1533064022" class="modal accent-bg">
            <div class="modal-content white-text">
                <h4 id="h4popuptitle">Session expired11..</h4>
                <p id="popupmsg">
                    Session expired ! Please click on Home button on menu11.
                </p>
            </div>
        </div>

        <div id="divShowAgreeModelPopup" style="display: none;">
            <a id="AlertAgreeModelpopup" class="waves-effect waves-light btn modal-trigger accent-bg"
                href="#modal1533064023">Daily Deal Message123</a>
        </div>
        <div style="display: none">
            <asp:HiddenField runat="server" ID="hdnDummyAgreeString" />
        </div>
        <div id="modal1533064023" class="modal accent-bg">
            <div class="modal-content white-text">
                <h4 id="h4Agreepopuptitle">Session expired11..</h4>
                <p id="agreepopupmsg">
                    Session expired ! Please click on Home button on menu11.
                </p>
            </div>
            <div class="modal-footer pink lighten-2 white-text">
                <%--<a id="btnDisagree" href="#!" class="modal-close waves-effect waves-red btn-flat">Disagree</a>
          <a id="btnAgree" href="#!" class="modal-close waves-effect waves-green btn-flat">Agree</a>--%>
                <asp:LinkButton ID="btnDisagree" runat="server" class="modal-close waves-effect waves-red btn-flat">Disagree </asp:LinkButton>
                <asp:LinkButton ID="btnAgree" runat="server" class="modal-close waves-effect waves-green btn-flat">Agree </asp:LinkButton>
            </div>
        </div>

        <script type="text/javascript">

            function opennotificationpopup(Message, Title) {

                document.getElementById('h4popuptitle').innerHTML = Title;
                document.getElementById('popupmsg').innerHTML = Message;

                document.getElementById('AlertModelpopup').click();
            };

            function openAgreenotificationpopup(Message, Title, AgreeString) {

                document.getElementById('h4Agreepopuptitle').innerHTML = Title;
                document.getElementById('agreepopupmsg').innerHTML = Message;
                document.getElementById('hdnDummyAgreeString').value = AgreeString;
                document.getElementById('AlertAgreeModelpopup').click();
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
        <script src="modules/jquery/jquery-2.2.4.min.js"></script>
        <script src="modules/materialize/materialize.js"></script>
        <script src="modules/materialize/select.js" type="text/javascript"></script>
        <script src="modules/perfect-scrollbar/perfect-scrollbar.min.js"></script>
        <script src="assets/js/variables.js"></script>
        <!-- CORE JS FRAMEWORK - END -->
        <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - START -->
        <script type="text/javascript">
            $(".tabs").tabs();
            $("#tabs-swipe-demo").tabs({ swipeable: true });
        </script>
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
                format: "dd-mmm-yyyy",
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
        <script type="text/javascript">
            $(".timepicker").timepicker();
            $(".timepicker.timepicker1").timepicker({
                autoClose: true
            });

            $(".timepicker.timepicker2").timepicker({
                twelveHour: false,
                autoClose: true
            });

            $(".timepicker.timepicker3").timepicker({
                vibrate: false,
                autoClose: true
            });
            $(".timepicker.timepicker4").timepicker({
                autoClose: true,
                showClearBtn: true
            });
            function OnArrivalClick(cb) {
                $("#hdnArrivalClick").click();
            }
            function OnTakeOffClick(cb) {
                $("#hdnTakeOffClick").click();
            }
            function OnTouchDownClick(cb) {
                $("#hdnTouchDownClick").click();
            }
        </script>

        <asp:Button ID="hdnArrivalClick" ClientIDMode="Static" runat="server" Text="----"
            CausesValidation="False" Style="display: none;"></asp:Button>
        <asp:Button ID="hdnTakeOffClick" ClientIDMode="Static" runat="server" Text="----"
            CausesValidation="False" Style="display: none;"></asp:Button>
        <asp:Button ID="hdnTouchDownClick" ClientIDMode="Static" runat="server" Text="----"
            CausesValidation="False" Style="display: none;"></asp:Button>

        <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->

    </form>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            $('.preloader-background').delay(10).fadeOut('slow');
        });

    </script>

</body>
</html>
