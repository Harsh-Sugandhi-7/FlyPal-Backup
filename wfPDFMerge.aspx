<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPDFMerge.aspx.vb" Inherits="Flypal.wfPDFMerge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
	<HEAD id="HEAD1" runat ="server" >
		<title>Receipt against Purchase Order Details</title>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<SCRIPT language="javascript">
		    function openledgersame(FileName) {
		        window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		    }
		</SCRIPT>
		<meta name="vs_showGrid" content="False">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		<script id="clientEventHandlersJS" language="javascript">
		    function openTranDetail() {
		        str = "wfReports.aspx"
		        window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		    }
		    function openTranDetail1() {
		        str = "webform1.aspx"
		        window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		    }
		    function openFile() {
		        str = "wfFileView.aspx"
		        window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		    }
		    function openDetail() {
		        str = "wfDetail.aspx"
		        window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		    }
		</script>
		<!--Added by Prashant 23-May-2012  23052012-->
		<LINK rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
		<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
		<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
		<script type="text/javascript" src="jquery.textchange.min.js"></script>
		<!------------------------------------------->
	</HEAD>
<body>
    <form id="form1" runat="server">
    <div>
    
												<asp:button id=btnPrintPDF Runat="server" CssClass="clsButton" Text="Print PDF" ToolTip="Click to Print the Receipt" CausesValidation="False">
													</asp:button>
    
    </div>
    <asp:TextBox ID="pdftext" runat="server" Height="456px" TextMode="MultiLine" 
        Width="879px"></asp:TextBox>
    </form>
</body>
</html>
