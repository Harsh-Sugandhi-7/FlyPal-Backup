<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfAlternateEnquiryPartList.aspx.vb" Inherits="Flypal.wfAlternateEnquiryPartList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Alternate Part</title>
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
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlMain" Runat="server" Cssclass="clsPanel1">
							<TABLE class="clstablelistin" id="tblLedgerList">
								<TR>
									<TD colSpan="5">
										<asp:label id="lblTitle" runat="server" Cssclass="clstitle1">Alternate Part [New]</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:ValidationSummary id="Validationsummary2" Cssclass="clsValidationSummary" Runat="server" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:label id="lblSelectedPart" runat="server" Cssclass="clsLabelHeader">Selected Part</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:label id="clsPartNo" runat="server" Cssclass="clsLabel">Part No.</asp:label></TD>
									<TD colSpan="2">
										<asp:TextBox id=txtPartNo runat="server" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mItem.Name %>" ToolTip="Enter Part No." CssClass="clsTextBoxLong">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:label id="lblDescription" runat="server" Cssclass="clsLabel">Description</asp:label></TD>
									<TD colSpan="2">
										<asp:TextBox id=txtDescription runat="server" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mItem.Description %>" ToolTip="Enter Description" CssClass="clsTextBoxLong">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:label id="lblResult" runat="server" Cssclass="clsLabelHeader">Search Resulted: No.of Record Found(s).</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									</TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:datagrid id="dgAlternatePartList" runat="server" Cssclass="clsGrid" PageSize="3" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="PartName" HeaderText="Part No">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="PartDescription" HeaderText="Description"></asp:BoundColumn>
												<asp:BoundColumn DataField="AltTypeName" HeaderText="Part Type"></asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="5">
										<TABLE class="clstablebutton" id="Table2" align="right">
											<TR>
												<TD>
													<asp:button id="btnClose" runat="server" Cssclass="clsButton" Text="Close" ToolTip="Click to close Alternate Part screen"
														CausesValidation="False"></asp:button></TD>
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
