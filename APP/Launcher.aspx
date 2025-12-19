<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Launcher.aspx.vb" Inherits="Flypal.Launcher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">


  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0" />
  <title>Allocation Notification</title>
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

  <link href="modules/materialize/materialize.min.css" type="text/css" rel="stylesheet" media="screen,projection" />
  <link href="modules/fonts/mdi/materialdesignicons.min.css" type="text/css" rel="stylesheet" media="screen,projection" />
  <link href="modules/perfect-scrollbar/perfect-scrollbar.css" type="text/css" rel="stylesheet" media="screen,projection" />


  <!-- CORE CSS FRAMEWORK - END -->

  <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - START -->
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->

  <!-- CORE CSS TEMPLATE - START -->

  
    <link href="assets/css/style.css" type="text/css" rel="stylesheet" media="screen,projection"  /><%--id="main-style"--%>
  <!-- CORE CSS TEMPLATE - END -->

  <style type="text/css">
   
        table {background:transparent !important; -webkit-box-shadow:none !important; box-shadow:none !important;}
        table tr {background:transparent !important; border-width:0px !important; border-style:none !important;}
        table td {padding:15px 15px 0px 15px !important;}

</style>

</head>
<body class="  html"  data-header="light" data-footer="dark"  data-header_align="center"  data-menu_type="left" data-menu="light" data-menu_icons="on" data-footer_type="left" data-site_mode="light" data-footer_menu="show" data-footer_menu_style="light" >
    <form id="form1" runat="server">

  <div class="preloader-background">
    <div class="preloader-wrapper">
      <div id="preloader"></div>
    </div>
  </div>


<div class="container">
  <div class="section">


       <ul class="events">

    <center>
            loading..
    </center>
    </ul>

  </div>
</div>

  

<br /> <br />
 
<div class="backtotop">
  <a class="btn-floating btn primary-bg"><i class="mdi mdi-chevron-up"></i></a>
</div>








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
         
    </script><!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->

    </form>
</body>
</html>
