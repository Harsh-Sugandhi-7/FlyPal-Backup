<%@ Page CodeBehind="index.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="Flypal.index" %>

<html>
<head runat="server">
    <title>FlyPal System</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft FrontPage 6.0" name="GENERATOR">
    <meta content="FrontPage.Editor.Document" name="ProgId">
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<frameset rows="85,81%,25" border="0" framespacing="0" frameborder="0">
		<frame id="FrameTop" name="top" noResize scrolling="no" target="top" src="TopHeader.aspx">
		<frameset id="mainframeset" cols="180,84%">
			<frame id="FrameLeft" name="contents" src="MainMenu.aspx" scrolling="auto">
			<frame id="FrameCentre" name="main" src="Dashboard.aspx" scrolling="auto">
		</frameset>
		<frame id="FrameBottom" name="bottom" src="footer.aspx" noResize scrolling="no">
		<noframes>
			<body onload="javascript:fullWin()">
				<p>This page uses frames, but your browser doesn't support them.</p>
			    <p>
                    <a href="GetCountList.vb">GetCountList.vb</a></p>
			</body>
		</noframes>
	</frameset>
</html>
