<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEvidence.aspx.vb" Inherits="Flypal.wfEvidence" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0" />
    <title>Evidence Details</title>
    <meta content="Alix Mobile App" name="description" />
    <meta content="themepassion" name="author" />
    <!-- App Icons -->
    <link rel="apple-touch-icon" sizes="57x57" href="APP/assets/images/icons/apple-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="APP/assets/images/icons/apple-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="APP/assets/images/icons/apple-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="APP/assets/images/icons/apple-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="APP/assets/images/icons/apple-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="APP/assets/images/icons/apple-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="APP/assets/images/icons/apple-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="APP/assets/images/icons/apple-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="APP/assets/images/icons/apple-icon-180x180.png">
    <link rel="icon" type="image/png" sizes="192x192" href="APP/assets/images/icons/android-icon-192x192.png">
    <link rel="icon" type="image/png" sizes="32x32" href="APP/assets/images/icons/favicon-32x32.png">
    <link rel="icon" type="image/png" sizes="96x96" href="APP/assets/images/icons/favicon-96x96.png">
    <link rel="icon" type="image/png" sizes="16x16" href="APP/assets/images/icons/favicon-16x16.png">
    <link rel="manifest" href="APP/assets/images/icons/manifest.json">
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="msapplication-TileImage" content="APP/assets/images/icons/ms-icon-144x144.png">
    <meta name="theme-color" content="#ffffff">
    <!-- CORE CSS FRAMEWORK - START -->
    <link href="APP/assets/css/_preloader.css" type="text/css" rel="stylesheet" media="screen,projection" />
    <link href="APP/modules/materialize/materialize.min.css" type="text/css" rel="stylesheet"
        media="screen,projection" />
    <link href="APP/modules/fonts/mdi/materialdesignicons.min.css" type="text/css" rel="stylesheet"
        media="screen,projection" />
    <link href="APP/modules/perfect-scrollbar/perfect-scrollbar.css" type="text/css"
        rel="stylesheet" media="screen,projection" />
    <!-- CORE CSS FRAMEWORK - END -->
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - START -->
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->
    <!-- CORE CSS TEMPLATE - START -->
    <link href="APP/assets/css/style.css" type="text/css" rel="stylesheet" media="screen,projection" />
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
    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/close.gif";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/detail.gif";
            }
        }
    </script>
    <style type="text/css">
        *
        {
            box-sizing: border-box;
        }
        
        #myInput
        {
            background-image: url('/css/searchicon.png');
            background-position: 10px 10px;
            background-repeat: no-repeat;
            width: 100%;
            font-size: 16px;
            padding: 12px 20px 12px 40px;
            border: 1px solid #ddd;
            margin-bottom: 12px;
        }
        
        #myTable
        {
            border-collapse: collapse;
            width: 100%;
            border: 1px solid #ddd;
            font-size: 18px;
        }
        
        #myTable th, #myTable td
        {
            text-align: left;
            padding: 12px;
        }
        
        #myTable tr
        {
            border-bottom: 1px solid #ddd;
        }
        
        #myTable tr.header, #myTable tr:hover
        {
            background-color: #f1f1f1;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <%--<script type="text/javascript" src="quicksearch.js"></script>--%>
</head>
<body class="  html" data-header="light" data-footer="dark" data-header_align="center"
    data-menu_type="left" data-menu="light" data-menu_icons="on" data-footer_type="left"
    data-site_mode="light" data-footer_menu="show" data-footer_menu_style="light">
    <form id="form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <!-- START navigation -->
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
                <li class="sep-wrap">
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
                </li>
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
                <li class="lvl1  footer_menu_style">
                    <div class="waves-effect appsettings " data-type="footer_menu_style" data-value="colored">
                        <a href="#!"><i class="mdi mdi-checkbox-blank-outline"></i><span class="title">Colored
                            Fixed Menu</span> </a>
                    </div>
                </li>
                <!-- MAIN MENU - END -->
                <!--  SIDEBAR - END -->
            </ul>
        </li>
    </ul>
    <div class="container">
        <div class="section">
            <h5 class="pagetitle">
                Evidence Details</h5>
            <asp:LinkButton runat="server" class="btn btn-link pull-right" ID="btnClose"><span class="fa  fa-times"></span> Close</asp:LinkButton>
        </div>
    </div>
    <div class="container">
        <div class="section">
            <div class="divider">
            </div>
            <%--<input type="text" id="myInput" placeholder="Search for names.." title="Type in a name"
                    onkeyup="Search_Gridview(this,dgEvidenceDetailsList)" />--%>
            <div class="title">
                <asp:UpdatePanel ID="upnlRosterList" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="title">
                            <%-- Total records found : 2--%>
                            <asp:Label ID="lblTotalrecordCount" runat="server" Text=""></asp:Label></div>
                        <br />
                        <ul class="events">
                            <asp:GridView ID="dgEvidenceDetailsList" runat="server" AutoGenerateColumns="False"
                                EmptyDataText="No data found." ShowHeader="False" GridLines="None" DataKeyNames="KeyfieldID,LogTextNo,UserName,DateTimeStampFormatted"
                                PagerSettings-Mode="NumericFirstLast">
                                <Columns>
                                
                                    <asp:BoundField DataField="KeyfieldID" HeaderText="KeyfieldID" Visible="False" />
                                    <asp:BoundField DataField="LogTextNo" HeaderText="LogTextNo">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="User">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateTimeStampFormatted" HeaderText="Updated Date">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("KeyfieldID") %>'
                                                CommandName="EditRec" ImageUrl="~/images/ReadMore.png" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--  <asp:TemplateField>
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="100%" bgcolor="White" width="0px">
                                                    <div id="ID-<%# Eval("KeyfieldID") %>" style="display: none; position: relative;
                                                        left: 25px;">
                                                        <asp:GridView ID="dgTransactionDetails" runat="server" AutoGenerateColumns="False"
                                                            Width="60%" BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                                            AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
                                                            SelectedRowStyle-BackColor="ButtonShadow" DataKeyNames="ID" ShowHeaderWhenEmpty="True"
                                                            PageSize="5">
                                                            <HeaderStyle CssClass="title" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false"></asp:BoundField>
                                                                <asp:BoundField DataField="FieldName" HeaderText="Field Name">
                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="title" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OldValue" HeaderText="Before Change">
                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="title" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="NewValue" HeaderText="After Change">
                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="title" />
                                                                </asp:BoundField>
                                                                
                                                             
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                            </asp:GridView>
                        </ul>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <br />
    <br />
    <%-- <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                background-color: #000000; top: 0; z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                z-index: 100000;">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div class="clsLoad_ajax">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                            Height="48px" Width="48px" />
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
        <ContentTemplate>
            <asp:Button ID="hdnBtnEvidenceDetails" ClientIDMode="Static" runat="server" Text="Add"
                CausesValidation="False" Style="display: none;"></asp:Button>
        </ContentTemplate>
    </asp:UpdatePanel>
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
    <!-- LOAD FILES AT PAGE END FOR FASTER LOADING -->
    <!-- CORE JS FRAMEWORK - START -->
    <script src="APP/modules/jquery/jquery-2.2.4.min.js"></script>
    <script src="APP/modules/materialize/materialize.js"></script>
    <script src="APP/modules/materialize/select.js" type="text/javascript"></script>
    <script src="APP/modules/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script src="APP/assets/js/variables.js"></script>
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
    <script src="APP/modules/app/init.js" type="text/javascript"></script>
    <script src="APP/modules/app/settings.js" type="text/javascript"></script>
    <script src="APP/modules/app/scripts.js" type="text/javascript"></script>
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
    <!-- Evidence Details Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEvidenceDetails" Text="Employee Equipment"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEvidenceDetails" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEvidenceDetails" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEvidenceDetails" runat="server" TargetControlID="btnDummyEvidenceDetails"
        PopupControlID="pnlEvidenceDetails" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenEvidenceWindow() {
            try {

                //       $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEvidenceDetails").attr("src", "wfEvidenceDetail.aspx?Type=pup");

                //                if (!$.browser.msie) {
                $("#btnDummyEvidenceDetails").click();
                //                    $get("AjaxLoader").style.visibility = 'hidden';
                //                }

                return false;
            } catch (e) {
                alert(e);
            }
        }
    </script>
    <script type="text/javascript">
        function IFrameEvidenceDetailsStateComplete() {
            $("#btnDummyEvidenceDetails").click();
            // $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEvidenceDetailsWindow() {


        }
        function ParentCallBackFunctionForEvidenceDetails() {
            var EvidenceDetailswindow = $find("<%=mdlPopupEvidenceDetails.ClientID %>");
            //close Equipment popup window
            EvidenceDetailswindow.hide();
            //           release resources
            $("#IframeEvidenceDetails").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEvidenceDetails").click();
        }
    </script>
    <!-- End-->
    </form>
    <script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("dgEvidenceDetailsList");
            var rowData;
            var regex = /(&nbsp;|<([^>]+)>)/ig
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML.replace(regex, '');
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }    
    </script>
</body>
</html>
