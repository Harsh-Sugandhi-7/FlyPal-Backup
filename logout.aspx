<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="logout.aspx.vb" Inherits="Flypal.logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    <script type="text/javascript">
        function delete_cookie() {
            document.cookie('noShowInvStickynote', null);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
