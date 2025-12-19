<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StickyNotejs.aspx.vb" Inherits="Flypal.StickyNotejs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>jQuery StickyNote Plugin</title>
    <link href="css/style.css" id="MainStyle" type="text/css" rel="stylesheet">
    <style>
        .content
        {
            position: absolute;
            top: 75px;
            right: 250px;
            padding: 0px;
            margin: 0px;
            height: 700px;
        }
    </style>
    <script src="js/jquery-1.3.2.js" type="text/javascript"></script>
    <script src="js/jquery.stickynote.js" type="text/javascript"></script>
    <script src="js/ui.core.js" type="text/javascript"></script>
    <script src="js/ui.draggable.js" type="text/javascript"></script>
    <script type="text/javascript">
        function displyStickyNote() {
            jQuery("#FlyPalstickynote").stickynote({
                size: 'large',
                ontop: false,
                text: document.getElementById("lnkPendingOrder").innerText + '<BR/><BR/>' +
                      document.getElementById("lnkCalibrationDueReport").innerText + '<BR/><BR/>' +
                      document.getElementById("lnkExpiredItems").innerText + '<BR/><BR/>' +
                      document.getElementById("lnkItemsToExpire").innerText + '<BR/><BR/>' +
                      document.getElementById("lnkCoreUnitDue").innerText
            });

            jQuery("#FlyPalstickynote").trigger('click');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div id="FlyPalstickynote" style="display: none;">
        <asp:Label runat="server" ID="lnkPendingOrder" Text=""></asp:Label>
        <asp:Label runat="server" ID="lnkCalibrationDueReport" Text=""></asp:Label>
        <asp:Label runat="server" ID="lnkExpiredItems" Text=""></asp:Label>
        <asp:Label runat="server" ID="lnkItemsToExpire" Text=""></asp:Label>
        <asp:Label runat="server" ID="lnkCoreUnitDue" Text=""></asp:Label>
        
    </div>
    <div id="content" class="content">
    </div>
    </form>
</body>
</html>