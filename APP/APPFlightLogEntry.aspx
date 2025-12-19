<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="APPFlightLogEntry.aspx.vb" Inherits="Flypal.APPFlightLogEntry" %>

<!DOCTYPE html>
<%@ Import Namespace="Flypal.LogList" %>
<%@ Import Namespace="Flypal.Log" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0" />
    <title>Aircraft Log Book</title>
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

        iframe {
            display: block; /* iframes are inline by default */
            background: #000;
            border: none; /* Reset default border */
            position: absolute;
            height:100%;
            width:85%;
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
        <!-- START navigation -->
        <nav class="fix_topscroll logo_on_fixed  topbar navigation" role="navigation">
            <div class="nav-wrapper container">
                <%--<a id="logo-container" href="" class=" brand-logo " >Aircraft Log(s)</a>--%>

                <asp:LinkButton ID="lnkHome" runat="server" class="waves-effect waves-circle navicon left sidenav-trigger show-on-large"><i class="mdi mdi-arrow-left"></i></asp:LinkButton>

                <%--    <a href="APPMenu.aspx" class="waves-effect waves-circle navicon left"><i class="mdi mdi-arrow-left"></i></a>

   <a href="#" data-target="slide-nav" class="waves-effect waves-circle navicon sidenav-trigger show-on-large"><i class="mdi mdi-menu"></i></a>--%>



                <a href="#" data-target="slide-settings" class="waves-effect waves-circle navicon right sidenav-trigger show-on-large pulse"><i
                    class="mdi mdi-settings-outline"></i></a>

                <a href="#" data-target="" class="waves-effect waves-circle navicon right nav-site-mode show-on-large"><i
                    class="mdi mdi-invert-colors mdi-transition1"></i></a>
                <!-- <a href="#" data-target="nav-mobile" class="sidenav-trigger"><i class="material-icons">menu</i></a> -->

                <%-- <div style="padding-top:12px;position:absolute;">
    <a href="APPNotificationList.aspx" class="waves-effect waves-circle navicon show-on-large"><i class="mdi mdi-bell-outline" ></i></a>
    <div style=" margin-left:20px; margin-top:-34px; border-radius: 50%; background: red;width: 20px;height: 20px;" ID="divNotificationCount" runat="server">
        0
    </div> 
    </div>--%>
            </div>
        </nav>
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
        <div class="container">
            <div class="section" style="margin-top: -16px;">
                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h5 class="pagetitle" runat="server" id="lblTitle">Aircraft Log Book</h5>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="container">
            <div class="section">
                <div class="row" style="margin-top: -12px;">
                    <div class="col s12">
                        <ul class="tabs z-depth-1 primary-bg colored">
                            <li class="tab col s3 "><a class="white-text"
                                href="#tab-1436457123-0">Log Book</a>
                            </li>
                            <li id="tabFuel" runat="server" class="tab col s3 "><a class="white-text"
                                href="#tab-FuelOil">Fuel Oil</a>
                            </li>
                            <li id="tabMEL" runat="server" class="tab col s3 "><a class="white-text"
                                href="#tab-1436457123-2"><%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect Reporting", "Snag Reporting") %></a>
                            </li>
                            <li id="tabParameter" runat="server" class="tab col s3 "><a class="white-text"
                                href="#tab-1436457123-3">Parameter List</a>
                            </li>
                        </ul>
                    </div>
                    <%--LOG BOOK--%>

                    <div id="tab-1436457123-0" class="col s12">
                        <div>
                            <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel runat="server">
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Remark Can't be greater than 200 chars"
                                            ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAirFrame" runat="server" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvGroundRunTime" runat="server" ErrorMessage="Departure date should be in date time format."
                                            ControlToValidate="txtGroundRunTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAirBornTime" runat="server" ErrorMessage="Not be Nigative."
                                            ControlToValidate="txtAirBorneTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPilot1" runat="server" ErrorMessage="Enter correct Pilot1 name."
                                            ControlToValidate="Pilot1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPilot2" runat="server" ErrorMessage="Enter correct Pilot2 name."
                                            ControlToValidate="Pilot2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPlace1" runat="server" ErrorMessage="Enter correct Source name."
                                            ControlToValidate="Place1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPlace2" runat="server" ErrorMessage="Enter correct Destination name."
                                            ControlToValidate="Place2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-content z-depth-1 z-depth-0 transparent">
                            <div class="row" style="margin-top: -16px;">
                                <div class="input-field col s5">
                                    <asp:TextBox ID="calDateTime" runat="server" class="datepicker datepicker2"></asp:TextBox>
                                    <label for="calDateTime" class="active glyphicon-star">
                                        Date</label>
                                </div>
                                <div class="input-field col s4">
                                    <asp:TextBox ID="txtLogText" runat="server"
                                        ReadOnly="True" ToolTip="Log Number" Text="<%# mLog.LogText %>"></asp:TextBox>
                                    <label for="txtLogText" class="active">Log Text</label>
                                </div>
                                <div class="input-field col s3">
                                    <asp:TextBox ID="txtLogNo" runat="server" Text="<%# mLog.LogNo %>"
                                        ReadOnly="True"></asp:TextBox>
                                    <label for="txtLogNo" class="active">Log No.</label>
                                </div>
                            </div>

                            <div class="row" style="margin-top: -24px;">
                                <div class="input-field col s3">

                                    <asp:TextBox ID="txtLogPageNo" runat="server" MaxLength="9"
                                        Text="<%# mLog.LogPageNoFormatted %>" ToolTip="Enter Log Page No."></asp:TextBox>
                                    <label for="txtLogPageNo" class="active">Page No.</label>
                                </div>
                                <div class="input-field col s3" style="margin-left: 3px;">

                                    <asp:TextBox ID="txtFlightNo" runat="server" MaxLength="10"
                                        Text="<%# mLog.FlightNo %>" ToolTip="Enter Flight No.">

                                    </asp:TextBox>
                                    <label for="txtFlightNo" class="active">Flight No.</label>
                                </div>
                                <div class="input-field col s5" style="margin-left: 6px;">
                                    <asp:DropDownList ID="cmbFlightLogClassification" runat="server"
                                        DataTextField="Name" DataValueField="ID">
                                    </asp:DropDownList>
                                    <label for="cmbFlightLogClassification">Classification</label>
                                </div>
                            </div>

                            <div class="row" style="margin-top: -24px;">
                                <div class="dropdown-trigger input-field col s4">
                                    <asp:TextBox ID="Pilot1" runat="server" Text="<%# mLog.Pilot1Name %>" data-target='dropdown1846989425'></asp:TextBox>
                                    <label for="Pilot1" class="active">
                                        Pilot</label>
                                    <cc2:AutoCompleteExtender runat="server" ID="Pilot1_AutoCompleteExtender" TargetControlID="Pilot1"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                        CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                    </cc2:AutoCompleteExtender>
                                </div>


                                <div class="input-field col s4" style="margin-left: 6px;">

                                    <asp:TextBox ID="Pilot2" runat="server"
                                        Text="<%# mLog.Pilot2Name %>" ToolTip="Pilot #2 Name"></asp:TextBox>
                                    <label for="Pilot1" class="active">
                                        Co-Pilot</label>
                                    <cc2:AutoCompleteExtender runat="server" ID="Pilot2_AutoCompleteExtender" TargetControlID="Pilot2"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                        CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                    </cc2:AutoCompleteExtender>
                                </div>

                                <%--   <div class="col s1" style="margin-top: 25px;">

                                    <asp:LinkButton runat="server" ID="imgbtnPilot2" class="btn-floating btn-small waves-effect waves-light bg-primary" ToolTip="Click to Add new pilot">  <i class="mdi mdi-plus"></i></asp:LinkButton>

                                </div>--%>
                            </div>
                            <div class="divider">
                            </div>
                            <div class="divider">
                            </div>
                            <div class="divider">
                            </div>

                            <%--Departure/Arrival details--%>
                            <div class="row">
                                <h7 id="lblDepHeader" runat="server" class="bot" style="margin-top: 4px; font-weight: bold;">Departure</h7>
                            </div>
                            <div class="row">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlPlace1">
                                    <ContentTemplate>
                                        <div class="input-field col s12" id="divPlace1" runat="server" style="margin-top: -11px;">
                                            <asp:TextBox ID="Place1" runat="server" Text="<%# mLog.SourceName %>"></asp:TextBox>
                                            <label for="Place1" class="active">
                                                Place</label>

                                            <cc2:AutoCompleteExtender runat="server" ID="Place1_AutoCompleteExtender" TargetControlID="Place1"
                                                ServiceMethod="GetPlaceCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                                CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                                CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                            </cc2:AutoCompleteExtender>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="row">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlDepartureDet">
                                    <ContentTemplate>
                                        <div class="col s1" style="margin-top: 25px;"></div>
                                        <div id="divDepartureDate" runat="server" class="input-field col s6 pad-0" style="margin-top: 5px; margin-left: 5px;">
                                            <asp:TextBox ID="calDeparture" runat="server" class="datepicker datepicker2"></asp:TextBox>
                                            <label id="lblDepartureDate" runat="server" for="calDeparture" class="active glyphicon-star">
                                                Date</label>
                                            <asp:TextBox runat="server" ID="CalUTCDateTime" class="datepicker datepicker2" Enabled="false"
                                                CausesValidation="True"></asp:TextBox>
                                            <label id="lblUTCDepartureDate" runat="server" for="CalUTCDateTime" class="active glyphicon-star">
                                                UTC Date</label>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="input-field col s4 pad-0" style="margin-top: 5px; margin-left: 4px;">

                                    <asp:TextBox ID="txtDepartureTime" runat="server" class="timepicker timepicker2"></asp:TextBox>
                                    <label id="lblDepartureTime" runat="server" for="txtDepartureTime" class="active glyphicon-star">
                                        Time</label>

                                    <asp:TextBox ID="txtUTCDepartureTime" runat="server" AutoPostBack="True" class="timepicker timepicker2"
                                        MaxLength="10" ToolTip="Enter UTC Departure Time."></asp:TextBox>
                                    <label id="lblUTCDepartureTime" runat="server" for="txtUTCDepartureTime" class="active glyphicon-star">
                                        UTC Time</label>

                                </div>
                            </div>

                            <div class="row" style="margin-top: -14px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlTakeOffDet">
                                    <ContentTemplate>
                                        <div class="col s1" style="margin-top: 25px;">
                                            <label>
                                                <input type="checkbox" id="chkTakeOff" runat="server" class="filled-in" onclick='OnTakeOffClick(this);' />
                                                <span></span>
                                            </label>

                                        </div>
                                        <div id="divTakeOffDate" runat="server" class="input-field col s6 pad-0" style="margin-top: 5px; margin-left: 5px;">
                                            <asp:TextBox ID="calTakeOffLocalDateTime" runat="server" class="datepicker datepicker2"></asp:TextBox>
                                            <label id="lblTakeOffLocalDateTime" runat="server" for="calTakeOffLocalDateTime" class="active glyphicon-star">
                                                Take Off Date</label>

                                            <asp:TextBox runat="server" ID="calUTCTakeOffDateTime" class="datepicker datepicker2"
                                                Enabled="false" AutoPostBack="True" CausesValidation="True"></asp:TextBox>
                                            <label id="lblUTCTakeOffDateTime" runat="server" for="CalUTCTakeOff" class="active glyphicon-star">
                                                UTC Take Off Date</label>
                                        </div>
                                        <div class="input-field col s4 pad-0" style="margin-top: 5px; margin-left: 4px;">
                                            <asp:TextBox ID="txtTakeOffLocalTime" runat="server" AutoPostBack="True" class="timepicker timepicker2"
                                                MaxLength="10" ToolTip="Enter TakeOff Time."></asp:TextBox>
                                            <label id="lblTakeOffTime" runat="server" for="txtTakeOffLocalTime" class="active glyphicon-star">
                                                Time
                                            </label>
                                            <asp:TextBox ID="txtUTCTakeOffTime" runat="server" AutoPostBack="True" class="timepicker timepicker2"
                                                MaxLength="10" ToolTip="Enter UTC TakeOff Time."></asp:TextBox>
                                            <label id="lblUTCTakeOffTime" runat="server" for="txtUTCTakeOffTime" class="active glyphicon-star">
                                                UTC Time
                                            </label>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="row">
                                <h7 id="lblArrHeader" runat="server" class="bot" style="margin-top: 4px; font-weight: bold;">Arrival</h7>
                            </div>
                            <div class="row">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlPlace2">
                                    <ContentTemplate>
                                        <div class="input-field col s12 pad-0" id="divPlace2" runat="server" style="margin-top: -14px;">
                                            <asp:TextBox ID="Place2" runat="server" Text="<%# mLog.DestinationName %>"></asp:TextBox>
                                            <label for="Place2">
                                                Place</label>

                                            <cc2:AutoCompleteExtender runat="server" ID="Place2_AutoCompleteExtender" TargetControlID="Place2"
                                                ServiceMethod="GetPlaceCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                                CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                                CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                            </cc2:AutoCompleteExtender>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="row" style="margin-top: -10px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlTouchDownDet">
                                    <ContentTemplate>
                                        <div class="col s1" style="margin-top: 25px;">
                                            <label>
                                                <input type="checkbox" id="chkTouchDown" runat="server" class="filled-in" onclick='OnTouchDownClick(this);' />
                                                <span></span>
                                            </label>

                                        </div>
                                        <div id="divTouchDownDate" runat="server" class="input-field col s6 pad-0" style="margin-top: 5px; margin-left: 13px;">
                                            <asp:TextBox ID="calTouchDownLocalDateTime" runat="server" class="datepicker datepicker2"></asp:TextBox>
                                            <label id="lblTouchDownLocalDateTime" runat="server" for="calTouchDownLocalDateTime" class="glyphicon-star">
                                                Touch Down Date</label>

                                            <asp:TextBox runat="server" ID="calUTCTouchDownDateTime" class="datepicker datepicker2"
                                                Enabled="false" AutoPostBack="True" CausesValidation="True"></asp:TextBox>
                                            <label id="lblUTCTouchDownDateTime" runat="server" for="CalUTCTouchDownOff" class="active glyphicon-star">
                                                UTC Touch Down Date</label>
                                        </div>
                                        <div class="input-field col s4 pad-0" style="margin-top: 5px;">

                                            <asp:TextBox ID="txtTouchDownLocalTime" runat="server" AutoPostBack="True" class="timepicker timepicker2"
                                                MaxLength="10" ToolTip="Enter TouchDownOff Time."></asp:TextBox>
                                            <label id="lblTouchDownOffTime" runat="server" for="txtTouchDownOffTime" class="active glyphicon-star">
                                                Time 
                                            </label>
                                            <asp:TextBox ID="txtUTCTouchDownTime" runat="server" AutoPostBack="True" class="timepicker timepicker2"
                                                MaxLength="10" ToolTip="Enter UTC TouchDownOff Time."></asp:TextBox>
                                            <label id="lblUTCTouchDownOffTime" runat="server" for="txtUTCTouchDownOffTime" class="active glyphicon-star">
                                                UTC Time</label>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                            <div class="row" style="margin-top: -10px;">

                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlArrivalDet">
                                    <ContentTemplate>
                                        <div class="col s1" style="margin-top: 25px;">
                                            <label>
                                                <input type="checkbox" id="chkArrival" runat="server" class="filled-in" onclick='OnArrivalClick(this);' />
                                                <span></span>
                                            </label>
                                        </div>

                                        <div id="divArrivalDate" runat="server" class="input-field col s6 pad-0" style="margin-top: 5px; margin-left: 13px;">
                                            <asp:TextBox ID="calArrival" runat="server" class="datepicker datepicker2"></asp:TextBox>
                                            <label id="lblArrivalDate" runat="server" for="calArrival" class="glyphicon-star">
                                                Date</label>

                                            <asp:TextBox runat="server" ID="CalUTCArrival" class="datepicker datepicker2" Enabled="false"
                                                AutoPostBack="True" CausesValidation="True"></asp:TextBox>
                                            <label id="lblUTCArrivalDate" runat="server" for="CalUTCArrival" class="active glyphicon-star">
                                                UTC Date</label>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="input-field col s4 pad-0" style="margin-top: 5px;">

                                    <asp:TextBox ID="txtArrivalTime" runat="server" AutoPostBack="True" class="timepicker timepicker2"
                                        MaxLength="10" ToolTip="Enter Arrival Time."></asp:TextBox>
                                    <label id="lblArrivalTime" runat="server" for="txtArrivalTime" class="active glyphicon-star">
                                        Time  
                                    </label>
                                    <asp:TextBox ID="txtUTCArrivalTime" runat="server" AutoPostBack="True" class="timepicker timepicker2"
                                        MaxLength="10" ToolTip="Enter UTC Arrival Time."></asp:TextBox>
                                    <label id="lblUTCArrivalTime" runat="server" for="txtUTCArrivalTime" class="active glyphicon-star">
                                        UTC Time
                                    </label>

                                </div>

                            </div>
                            <div class="divider">
                            </div>
                            <div class="divider">
                            </div>
                            <div class="divider">
                            </div>
                            <div class="row">
                                <div class="col s12">
                                    <h7 id="H1" runat="server" class="bot" style="margin-top: 4px; font-weight: bold;">Aircraft Flying Hours as per Flight Log book</h7>

                                </div>

                            </div>
                            <div class="row" style="margin-top: -21px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBlockAirborneTime">
                                    <ContentTemplate>
                                        <div class="input-field col s5" runat="server" id="divBlockTime">
                                            <asp:TextBox ID="txtBlockTime" runat="server"
                                                Enabled='<%#IIf(AppSettings("SetBlockTime") = "True", True, False) %>' Text="<%# mLog.DiffTime %>"></asp:TextBox>
                                            <label for="txtBlockTime" class="active">Block Time</label>
                                        </div>
                                        <div class="input-field col s5" runat="server" id="divAirBorneTime" style="margin-left: 6px;">
                                            <asp:TextBox ID="txtAirBorneTime" runat="server"
                                                AutoPostBack="true" ReadOnly="<%# mLog.ShowTimeTextBoxes Or Not mLog.IsNew %>"
                                                Text="<%# mLog.TimeInAir %>"></asp:TextBox>
                                            <label for="txtLogNo" class="active">Airborne Time</label>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                            <div class="row" style="margin-top: -21px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlGroundWithTotalTime">
                                    <ContentTemplate>
                                        <div class="input-field col s4" runat="server" id="divGroundRunTime">
                                            <asp:TextBox ID="txtGroundRunTime" runat="server" ReadOnly="<%# mLog.ShowTimeOnGround Or Not mLog.IsNew %>" Text="<%# mLog.TimeOnGround %>" Enabled='<%# iif(AppSettings("SetBlockTime") = "True", False, True) %>'
                                                AutoPostBack="True"></asp:TextBox>
                                            <label for="txtGroundRunTime" class="active">Ground Run Time</label>
                                        </div>
                                        <div class="input-field col s4" runat="server" id="divPercentTimeOnGround" style="margin-left: 6px;">
                                            <asp:TextBox ID="txtPercentTimeOnGround" runat="server"
                                                ReadOnly="<%# Not mLog.IsNew %>" Text="<%# mLog.PercentTimeOnGround %>"
                                                AutoPostBack="True" onfocus="onTextFocus();"></asp:TextBox>
                                            <label for="txtPercentTimeOnGround" class="active">%Ground Run Time </label>
                                        </div>
                                        <div class="input-field col s3">
                                            <asp:TextBox ID="txtTotalTime" runat="server" Text="<%# mLog.TotalTime %>" Enabled="false"></asp:TextBox>
                                            <label for="txtTotalTime" class="active">Total Time </label>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row" style="margin-top: -21px;">
                                <div class="input-field col s3">
                                    <asp:TextBox ID="txtLandings" runat="server"></asp:TextBox>
                                    <label for="txtLandings" class="active">Landings </label>
                                </div>
                                <div class="input-field col s3">
                                    <asp:TextBox ID="txtCycles" runat="server"></asp:TextBox>
                                    <label for="txtCycles" class="active">Cycles </label>
                                </div>
                            </div>
                            <div class="divider">
                            </div>
                            <div class="divider">
                            </div>
                            <div class="divider">
                            </div>
                            <div class="row">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlRemark">
                                    <ContentTemplate>
                                        <div class="input-field col s12">
                                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Style="border: none;"
                                                MaxLength="500" Text="<%# mLog.Remark %>" ToolTip="Enter Remark"></asp:TextBox>
                                            <label for="txtRemark" class="active">Remark</label>
                                            <%-- <textarea id="txtRemark" class="materialize-textarea"></textarea>--%>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row" style="margin-top: 10px; text-align: right;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlSave">
                                    <ContentTemplate>
                                        <asp:LinkButton runat="server" ID="btnSave" class="btn btn-link"><span class="fa fa-save" ></span> Save</asp:LinkButton>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <%--FUEL OIL--%>
                    <div id="tab-FuelOil" class="col s12" hidden="<%#  mLog.IsNew %>">
                         <div class="tab-content z-depth-1  z-depth-0 transparent ">
                            <%--<asp:checkbox id="ss" runat="server" text="dasdassa" />--%>
                            <iframe id="IframeFuelOil" scrolling="no" marginheight="0" src='APPFlightLogFuelOil.aspx' style="margin-left: -5px;"
                                frameborder="0" allowfullscreen></iframe>
                         </div>
                    </div>

                    <%-- Defect Reporting--%>
                    <div id="tab-1436457123-2" class="col s12" hidden="<%#  mLog.IsNew %>">
                        <div class="tab-content z-depth-1  z-depth-0 transparent ">
                            <p>
                                <img class="img-wrap z-depth-1 round" style="width: 100%;" src="assets/images/blog-masonry-101.jpg">
                            </p>
                            <p>I haven't bailed on writing. Look, I'm generating a random paragraph at this very moment in an attempt to get my writing back on track. I am making an effort. I will start writing consistently again! she'll prove she can again. We all already know this and you will too.</p>
                        </div>
                    </div>

                    <%--Parameter List--%>
                    <div id="tab-1436457123-3" class="col s12" hidden="<%#  mLog.IsNew %>">
                        <div class="tab-content z-depth-1 z-depth-0 transparent ">
                            <p>
                                <img class="img-wrap z-depth-1 round" style="width: 100%;" src="assets/images/blog-masonry-116.jpg">
                            </p>
                            <p>He sat across from her trying to imagine it was the first time. It wasn't. Had it been a hundred? It quite possibly could have been. Two hundred? Probably not. His mind wandered until he caught himself and again tried to imagine it was the first time. she'll prove she can again. We all already know this and you will too.</p>
                        </div>
                    </div>
                </div>

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
                    runat="server" class="mdi mdi-chart-timeline" style="color: var(--primary-color);"></i><span>Aircraft Current</span></a></li>
                <li><a href="APPAircraftLogBook.aspx" id="hrefFlights" runat="server"><i id="iFlights"
                    runat="server" class="mdi mdi-airplane" style="color: var(--primary-color);"></i>
                    <span>Flights</span></a></li>
                <li><a href="javascript:return(0)" id="hrefAvailability" runat="server"><i id="iAvailability"
                    runat="server" class="mdi mdi-account-check1" style="color: var(--primary-color);"></i><span>&nbsp;</span></a></li>
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
        <!-- LOAD FILES AT PAGE END FOR FASTER LOADING -->
        <!-- CORE JS FRAMEWORK - START -->
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
        <!-- OTHER SCRIPTS IN   CLUDED ON THIS PAGE - START -->
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
        <script type="text/javascript">
            function CallFuelOil() {
                alert("aaa");
                document.getElementById('IframeFuelOil').src = 'APPFlightLogFuelOil.aspx'
            }
            function autoResizeFuelOil() {
                alert("aaa");
            }
        </script>
    </form>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            $('.preloader-background').delay(10).fadeOut('slow');
        });

    </script>

</body>
</html>
