<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar" Assembly="obout_Calendar_Pro_Net" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfCompCurrentStatusList.aspx.vb" Inherits="Flypal.wfCompCurrentStatusList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<TITLE>Purchase Order List</TITLE>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<SCRIPT language="javascript">
			function openledgersame(FileName)
               {
                  window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

               }
               function openTranDetail()
			{
				str1 = "wfReports.aspx"
				window.open(str1,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
		</SCRIPT>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body MS_POSITIONING="GridLayout" bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0">
		<FORM id="Form1" method="post" runat="server">
			<TABLE class="clstablelistout" id="tblMain">
				<TR>
					<TD><asp:panel id="pnlMain" Runat="server" CssClass="clsPanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="3">
										<asp:label id="lblCompList" runat="server" CssClass="clstitle1">Component List</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:ValidationSummary id="Validationsummary" Runat="server" HeaderText="Fill Up The Following Information"
											Cssclass="clsValidationSummary"></asp:ValidationSummary></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<TABLE>
											<TR>
												<TD>
													<asp:Label id="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label></TD>
												<TD>
													<TABLE>
														<TR>
															<TD>
																<asp:DropDownList id="cmbAircraft" runat="server" CssClass="clsComboBox1" AutoPostBack="True" DataValueField="MachineID"
																	DataTextField="RegNo"></asp:DropDownList></TD>
															<TD>
																<asp:Label id="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label></TD>
															<TD>
																<P>
																	<asp:DropDownList id="cmbAssembly" runat="server" CssClass="clsComboBox1" DataValueField="ID" DataTextField="Description"></asp:DropDownList></P>
															</TD>
														</TR>
													</TABLE>
												</TD>
												<TD align="right">
													<asp:Button id="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now" ToolTip="Click to Find"></asp:Button></TD>
											</TR>
											<TR>
												<TD>
													<asp:Label id="lblDate" CssClass="clsLabelAuto" Runat="server" DESIGNTIMEDRAGDROP="19">Date </asp:Label></TD>
												<TD>
													<TABLE id="Table1">
														<TR>
															<TD align="left">
																<uc1:SICalendar id="SICalendar1" runat="server"></uc1:SICalendar></TD>
														</TR>
													</TABLE>
												</TD>
												<TD></TD>
											</TR>
											<TR>
												<TD colSpan="2">
													<asp:label id="lblPrint" runat="server" CssClass="clsLabelAuto">Click To Print the Records</asp:label></TD>
												<TD align="right" colSpan="1">
													<asp:button id="btnPrintTop" runat="server" CssClass="clsButton" Text="Print" ToolTip="Click to Print Purchase Order Register"
														CausesValidation="False"></asp:button></TD>
											</TR>
											<TR>
												<TD colSpan="2"></TD>
												<TD align="right"></TD>
											</TR>
											<TR>
												<TD vAlign="top" colSpan="2">
													<asp:datagrid id="dgCompList" runat="server" CssClass="clsGrid" DESIGNTIMEDRAGDROP="139" AutoGenerateColumns="False">
														<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
														<ItemStyle CssClass="clsdgItem"></ItemStyle>
														<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
														<Columns>
															<asp:BoundColumn DataField="ATAChapter" HeaderText="ATA Chapter"></asp:BoundColumn>
															<asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
															<asp:BoundColumn DataField="PartNo" HeaderText="Part No">
																<HeaderStyle Wrap="False"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="SerialNo" HeaderText="Serial No"></asp:BoundColumn>
															<asp:BoundColumn DataField="Hours" HeaderText="Hours">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
																<FooterStyle HorizontalAlign="Right"></FooterStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="Landings" HeaderText="Landings">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="Cycles" HeaderText="Cycles">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="RINS" HeaderText="RINS">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
														</Columns>
														<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
													</asp:datagrid></TD>
												<TD vAlign="bottom" align="right">
													<asp:button id="btnCloseTop" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to go back to the previous page"
														CausesValidation="False"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</asp:panel></TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
