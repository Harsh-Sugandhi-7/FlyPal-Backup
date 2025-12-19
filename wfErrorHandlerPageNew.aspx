<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfErrorHandlerPageNew.aspx.vb"
    Inherits="Flypal.wfErrorHandlerPageNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
    <title>Error</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <link href="404-Page-Not-Found-AJAX/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="404-Page-Not-Found-AJAX/css/styles.css" rel="stylesheet" type="text/css" />
    <link href="404-Page-Not-Found-AJAX/css/ajax-pages.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
    <script type="text/javascript" src="404-Page-Not-Found-AJAX/js/jquery.tools.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#overlay").overlay({ api: true, finish: { top: 0, left: "center" } }).load();
        });
    </script>
    <script type="text/javascript">

        function goTopFrame() {
            if (window != window.top) {
                window.top.location.href = window.location.href;
            }
        }

    </script>

</head>
<body onload="goTopFrame()">
    <div class="overlay" id="overlay">
        <div id="result" class="wrap">
            <div id="ajaxWrap">
                <div id="wrapper">
                    <div id="bg-top">
                    </div>
                    <div id="contentWrap">
                        <div id="content">
                            <!-- BEGIN LEFT COLUMN -->
                            <div id="leftColumn">
                                <!-- YOUR LOGO GOES HERE -->
                                <div id="img">
                                    <img id="logo" src="404-Page-Not-Found-AJAX/images/template/logo.png" alt="Logo Sample" />
                                </div>
                                <!-- BEGIN MENU -->
                                <div id="nav">
                                    <span>menu</span>
                                    <ul>
                                        <li class="home"><a href="#">Home</a></li>
                                        <li class="about"><a href="#">About Us</a></li>
                                        <li class="contact"><a href="#">Conact Us</a></li>
                                    </ul>
                                </div>
                                <!-- end div #nav -->
                                <!-- ERROR CODE HERE -->
                            </div>
                            <!-- end div #leftColumn -->
                            <!-- END LEFT COLUMN -->
                            <!-- BEGIN RIGHT COLUMN -->
                            <div id="rightColumn">
                                <h2>Oops! An error occured...</h2>
                                <p>
                                    Sorry, Evidently the document you were looking for has either been moved or no longer
                                    exists. Please use the navigational links to the right to locate additional resources
                                    and information.
                                </p>
                                <h4 class="regular">
                                    <strong>This error has occured for one of the following reasons..</strong></h4>
                                <ol>
                                    <li><span>You have used Back/Forward/Refresh button.</span></li>
                                    <li><span>You have clicked twice on any button.</span></li>
                                    <li><span>You have kept browser idle for long time.</span></li>
                                    <li><span>You have requested a page which does not exist</span></li>
                                </ol>
                                <!-- BEGIN SEARCH FORM - EDIT YOUR DOMAIN BELOW -->
                                <form id="Form1" method="post" runat="server">
                                    <div>
                                        Go back to
                                    <asp:LinkButton ID="bntLogin" runat="server">Login</asp:LinkButton>
                                        &nbsp;page 
                                        <asp:LinkButton ID="close" runat="server"></asp:LinkButton>
                                    </div>
                                </form>
                            </div>
                            <!-- end div #rightColumn -->
                            <!-- END RIGHT COLUMN -->
                        </div>
                        <!-- end div #content -->
                    </div>
                    <div id="bg-bottom">
                    </div>
                    <!-- end div #contentWrap -->
                </div>
                <!-- end div #wrapper -->
            </div>
            <!-- end div #ajaxWrap -->
            <script type="text/javascript" src="404-Page-Not-Found-AJAX/js/ajax-pages.js"></script>
        </div>
    </div>
</body>
</html>
