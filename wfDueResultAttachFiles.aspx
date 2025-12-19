<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfDueResultAttachFiles.aspx.vb" Inherits="Flypal.wfDueResultAttachFiles" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Attach Files</title>
		<SCRIPT language="javascript">
			function openledgersame(FileName)
               {
                  window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

               }
		</SCRIPT>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
		
		<script language="javascript" id="clientEventHandlersJS">
			function openTranDetail()
			{
				str = "wfReports.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openTranDetail1()
			{
				str = "webform1.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openFile()
			{
				str = "wfFileView.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openDetail()
			{
				str = "wfDetail.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
		</script>
	</HEAD>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="4">
										<asp:Label id="lbltitle" CssClass="clstitle1" Runat="server">Attach Files [New]</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgAttachFileList" runat="server" CssClass="clsGrid" ToolTip="Attached Files List"
											AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
												<asp:BoundColumn DataField="DocumentTypeName" HeaderText="Document Type"></asp:BoundColumn>
												<asp:BoundColumn DataField="Remark" HeaderText="Remark"></asp:BoundColumn>
												<asp:ButtonColumn Text="View" HeaderText="View" CommandName="View"></asp:ButtonColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="3">
										<asp:Button id="btnClose" tabIndex="0" CssClass="clsButton" Runat="server" ToolTip="Click to go back to the previous page"
											Text="Close" CausesValidation="False" Visible="False"></asp:Button></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
