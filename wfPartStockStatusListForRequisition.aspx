<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfPartStockStatusListForRequisition.aspx.vb" Inherits="Flypal.wfPartStockStatusListForRequisition" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Part Stock Status List</title>
		<script language="javascript">
		function openledgersame(FileName)
		{
		window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

		}

		//this function takes a value (ltext) and transmits that to the left hand frame

		function tranRight(ltext)

		{
			parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;
			
		}
		</script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body MS_POSITIONING="GridLayout" bottomMargin="5" leftMargin="5" topMargin="5" rightMargin="5">
		<form id="Form1" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlMain" Runat="server" Cssclass="clsPanel1">
							<TABLE class="clstablelistin" id="tblLedgerList">
								<TR>
									<TD colSpan="5">
										<asp:label id="lblPartStockStatusList" runat="server" Cssclass="clstitle1">Part Stock Status List</asp:label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblSearch" runat="server" Cssclass="clsLabel">Part No</asp:label></TD>
									<TD colSpan="2">
										<asp:TextBox id="txtSearch" runat="server" ToolTip="Enter Search Criteria" CssClass="clsTextBoxLong"></asp:TextBox></TD>
									<TD align="right">
										<asp:button id="btnFindNow" runat="server" Cssclass="clsButton" ToolTip="Click to Find" Text="Find Now"></asp:button></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									</TD>
									<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									</TD>
									<TD></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:label id="lblResult" runat="server" Cssclass="clsLabelHeader">Part Stock Status List : No.of Record Found(s).</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									</TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:datagrid id="dgPartStockStatusList" runat="server" Cssclass="clsGrid" AutoGenerateColumns="False"
											PageSize="3">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ItemId" HeaderText="ItemId"></asp:BoundColumn>
												<asp:BoundColumn DataField="ItemName" HeaderText="Part No">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ItemDescription" HeaderText="Part Description"></asp:BoundColumn>
												<asp:BoundColumn DataField="StockQty" HeaderText="Stock Qty" DataFormatString="{0:#00.00}">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="PendingQty" HeaderText="Pending Qty" DataFormatString="{0:#00.00}">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ReturnableQty" HeaderText="Returnable Qty" DataFormatString="{0:#00.00}">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								<TR>
									<TD align="right" colSpan="5">
										<TABLE class="clstableButton" align="right">
											<TR>
												<TD>
													<asp:button id="btnBack" runat="server" Cssclass="clsButton" ToolTip="Click to go back to the previous page"
														Text="Back"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
