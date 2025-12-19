<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfRequisitionItemListNewForPurchaseApproval.aspx.vb" Inherits="Flypal.wfRequisitionItemListNewForPurchaseApproval" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Engineering Purchase Approval List</title>
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
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body bottomMargin="5" leftMargin="0" rightMargin="0" topMargin="5" MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="tblMain" class="clstablelistout">
				<TR>
					<TD><asp:panel id="pnlMain" CssClass="clsPanel1" Runat="server">
							<TABLE id="tblInner" class="clstablelistin">
								<TR>
									<TD colSpan="3" noWrap>
										<DIV noWrap>
											<asp:label id="LblTitle" runat="server" CssClass="clstitle1">Requisition Item List For Purchase Approval </asp:label></DIV>
									</TD>
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
													<asp:Label id="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label></TD>
												<TD>
													<TABLE>
														<TR>
															<TD>
																<asp:dropdownlist id="cmbSearch" runat="server" CssClass="clsComboBox" AutoPostBack="True">
																	<asp:ListItem Value="0" Selected="True">All</asp:ListItem>
																	<asp:ListItem Value="1">Date</asp:ListItem>
																	<asp:ListItem Value="2">Part No.</asp:ListItem>
																</asp:dropdownlist></TD>
															<TD>
																<asp:Label id="L1" runat="server" CssClass="clsLabel" Width="20px"></asp:Label></TD>
															<TD>
																<P>
																	<asp:DropDownList id="cmbDate" runat="server" CssClass="clsComboBox1" AutoPostBack="True" Visible="False">
																		<asp:ListItem Value="0">(All)</asp:ListItem>
																		<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
																		<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
																		<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
																		<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
																		<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
																		<asp:ListItem Value="6">Between Dates</asp:ListItem>
																	</asp:DropDownList></P>
															</TD>
															<TD align="left">
																<asp:TextBox id="txtName" runat="server" CssClass="clsTextBox" Visible="False" MaxLength="50"></asp:TextBox></TD>
														</TR>
													</TABLE>
												</TD>
												<TD align="right">
													<asp:Label id="lblFromDate" Runat="server" CssClass="clsLabel" Visible="False">From Date </asp:Label></TD>
												<TD>
													<TABLE>
														<TR>
															<TD align="left">
																<uc1:sicalendar id="txtFromDate" runat="server"></uc1:sicalendar></TD>
														</TR>
													</TABLE>
												</TD>
												<TD align="right">&nbsp;&nbsp;
													<asp:Label id="lblToDate" Runat="server" CssClass="clsLabel" Width="78px" Visible="False" DESIGNTIMEDRAGDROP="19">To Date </asp:Label></TD>
												<TD>
													<TABLE>
														<TR>
															<TD align="left">
																<uc1:sicalendar id="txtToDate" runat="server"></uc1:sicalendar></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
										<asp:Label id="lblInfo" runat="server" CssClass="clsLabelAuto">Select Requisition Part from the list. Click on Detail link to see the Requisition(s). </asp:Label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Requisition as per criteria : Record(s) found</asp:label></TD>
									<TD colSpan="2" align="right">
										<TABLE>
											<TR>
												<TD align="right">
													<asp:Button id="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now" ToolTip="Click to find requisition items as per search criteria"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="left">
										<TABLE id="Table1">
											<TR>
												<TD>
													<asp:button id="Button2" runat="server" CssClass="clsButtonLong" Visible="False" Text="Processed Requisitions"
														ToolTip="Click to Print Requisition Register" CausesValidation="False"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
									<TD colSpan="2" align="right">
										<TABLE>
											<TR>
												<TD>
													<asp:button id="btnPrintTop" runat="server" CssClass="clsButton" Visible="False" Text="Print"
														ToolTip="Click to Print" CausesValidation="False"></asp:button></TD>
												<TD>
													<asp:button id="btnCloseTop" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to Close Requisition Item List For Purchase Approval screen"
														CausesValidation="False"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="3" align="left">
										<asp:datagrid id="dgApprovalList" runat="server" CssClass="clsGrid" DESIGNTIMEDRAGDROP="139" AutoGenerateColumns="False"
											AllowPaging="True" PageSize="25" AllowSorting="True">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ItemID" HeaderText="PartID"></asp:BoundColumn>
												<asp:BoundColumn DataField="DateFormatted" HeaderText="Date"></asp:BoundColumn>
												<asp:BoundColumn DataField="ReqNo" SortExpression="ReqNo" HeaderText="Requisition No.">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="PartNo" SortExpression="PartNo" HeaderText="Part No.">
													<HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="RequestedQty" SortExpression="RequestedQty" HeaderText="Req. Qty.">
													<HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="Detail" HeaderText="Detail" CommandName="Detail"></asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="ReqItemID" HeaderText="ReqItemID"></asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD colSpan="3" align="right">
										<TABLE>
											<TR>
												<TD>
													<asp:button id="BtnPrint" runat="server" CssClass="clsButton" Visible="False" Text="Print" ToolTip="Click to Print"
														CausesValidation="False"></asp:button></TD>
												<TD>
													<asp:button id="btnClose" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to Close Requisition Item List For Purchase Approval screen"
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
