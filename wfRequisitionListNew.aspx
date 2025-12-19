<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfRequisitionListNew.aspx.vb" Inherits="Flypal.wfRequisitionListNew" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<TITLE>Requisition List</TITLE>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<SCRIPT language="javascript">
			function openledgersame(FileName)
               {
                  window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

               }
		</SCRIPT>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body bottomMargin="5" leftMargin="0" rightMargin="0" topMargin="5" MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="tblMain" class="clstablelistout" border="0">
				<TR>
					<TD><asp:panel id="pnlMain" CssClass="clsPanel1" Runat="server">
							<TABLE id="tblInner" class="clstablelistin" border="0">
								<TR>
									<TD colSpan="3" noWrap>
                                    <asp:label id="LblTitle" runat="server" CssClass="clstitle1">List of Requisition(s)
										<asp:label id="lblTotal" runat="server" CssClass="clstitle1"></asp:label></asp:label>
										
									</TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:ValidationSummary id="Validationsummary" Runat="server" HeaderText="Fill Up The Following Information"
											Cssclass="clsValidationSummary"></asp:ValidationSummary></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<TABLE border="0">
											<TR>
												<TD>
													<asp:Label id="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label></TD>
												<TD>
													<TABLE border="0">
														<TR>
															<TD>
																<asp:dropdownlist id="cmbSearch" runat="server" CssClass="clsComboBox" AutoPostBack="True">
																	<asp:ListItem Value="0" Selected="True">All</asp:ListItem>
																	<asp:ListItem Value="1">Date</asp:ListItem>
																	<asp:ListItem Value="2">Requisition</asp:ListItem>
																	<asp:ListItem Value="3">Requesting Location</asp:ListItem>
																	<asp:ListItem Value="4">Status</asp:ListItem>
																	<asp:ListItem Value="5">Requisition Type </asp:ListItem>
																	<asp:ListItem Value="6">Part No</asp:ListItem>
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
																	</asp:DropDownList>
																	<asp:DropDownList id="cmbStatus" runat="server" CssClass="clsComboBox1" Width="160px" Visible="False">
																		<asp:ListItem Value="0">(All)</asp:ListItem>
																		<asp:ListItem Value="1">Open</asp:ListItem>
																		<asp:ListItem Value="2">Authorized</asp:ListItem>
																	</asp:DropDownList>
																	<asp:DropDownList id="cmbRequisitionText" runat="server" CssClass="clsComboBox1" AutoPostBack="True"
																		Visible="False" DataTextField="Text" DataValueField="Text"></asp:DropDownList>
																	<asp:DropDownList id="cmbRequisitionLocation" runat="server" CssClass="clsComboBox1" Visible="False"
																		DataTextField="Name" DataValueField="ID"></asp:DropDownList>
																	<asp:TextBox id="txtName" runat="server" CssClass="clsTextBox" Visible="False" MaxLength="50"></asp:TextBox>
																	<asp:DropDownList  id="cmbRequisitionType" runat="server" CssClass="clsComboBox1"
																		Width="160px" Visible="False">
																		<asp:ListItem Value="0">(All)</asp:ListItem>
																		<asp:ListItem Value="1">Engineering</asp:ListItem>
																		<asp:ListItem Value="2">Stores</asp:ListItem>
																	</asp:DropDownList></P>
															</TD>
															<TD>
																<asp:Label id="lblNo" runat="server" CssClass="clsLabelAuto" Width="24px" Visible="False">No.</asp:Label></TD>
															<TD align="left">
																<asp:TextBox id="txtNo" runat="server" CssClass="clsTextBox" Visible="False" MaxLength="8"></asp:TextBox></TD>
														</TR>
													</TABLE>
												</TD>
												<TD align="right">
													<asp:Label id="lblFromDate" Runat="server" CssClass="clsLabel" Visible="False">From Date </asp:Label></TD>
												<TD>
													<TABLE border="0">
														<TR>
															<TD align="left">
																<UC1:SICALENDAR id="txtFromDate" runat="server"></UC1:SICALENDAR></TD>
														</TR>
													</TABLE>
												</TD>
												<TD align="right">&nbsp;&nbsp;
													<asp:Label id="lblToDate" Runat="server" CssClass="clsLabel" Width="78px" Visible="False" DESIGNTIMEDRAGDROP="19">To Date </asp:Label></TD>
												<TD>
													<TABLE border="0">
														<TR>
															<TD align="left">
																<UC1:SICALENDAR id="txtToDate" runat="server"></UC1:SICALENDAR></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
											<TR>
												<TD></TD>
												<TD></TD>
												<TD align="right"></TD>
												<TD></TD>
												<TD align="right"></TD>
												<TD></TD>
											</TR>
										</TABLE>
										<asp:Label id="lblInfo" runat="server" CssClass="clsLabelAuto">Select Requisition from the list. Click On Edit Link To Modify The Selected Requisition. Click On Delete link To Delete The Selected Requisition. Click On Add New button To Add A New Requisition.</asp:Label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Requisition as per criteria : Record(s) found</asp:label></TD>
									<TD colSpan="2" align="right">
										<TABLE border="0">
											<TR>
												<TD align="right">
													<asp:Button id="btnFindNow" runat="server" CssClass="clsButton" ToolTip="Click to find list of Requisition as per searching criteria"
														Text="Find Now"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="3" align="right">
										<TABLE border="0">
											<TR>
												<TD>
													<asp:button id="btnAddNewTop" runat="server" CssClass="clsButton" ToolTip="Click to Add New Requisition"
														Text="Add New" CausesValidation="False"></asp:button></TD>
												<TD>
													<asp:button  id="btnPrintTop" runat="server" CssClass="clsButton" ToolTip="Click to print list of Requisition"
														Text="Print" CausesValidation="False"></asp:button></TD>
												<TD>
													<asp:button id="btnCloseTop" runat="server" CssClass="clsButton" ToolTip="Click to close List of Requisition Raised by Engineer screen"
														Text="Close" CausesValidation="False"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="3" align="left">
										<asp:datagrid id="dgRequisitionList" runat="server" CssClass="clsGrid" DESIGNTIMEDRAGDROP="139"
											PageSize="25" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="DateFormatted" HeaderText="Date">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="RequisitionTextNo" SortExpression="RequisitionTextNo" HeaderText="Requisition No.">
													<HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ReqTypeName" SortExpression="ReqTypeName" HeaderText="Requisition Type">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="RequisitionEngineeringBranch" HeaderText="Branch" 
                                                    SortExpression="RequisitionEngineeringBranch">
                                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                                        Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                                                </asp:BoundColumn>
												<asp:BoundColumn DataField="LocationName" SortExpression="Location" HeaderText="Location">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="EmployeeName" SortExpression="EmployeeName" HeaderText="Requested By">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
													<HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD colSpan="3" align="right">
										<TABLE border="0">
											<TR>
												<TD>
													<asp:button id="btnAddNew" runat="server" CssClass="clsButton" ToolTip="Click to Add New Requisition"
														Text="Add New" CausesValidation="False"></asp:button></TD>
												<TD>
													<asp:button  id="BtnPrint" runat="server" CssClass="clsButton" ToolTip="Click to print list of Issues"
														Text="Print" CausesValidation="False"></asp:button></TD>
												<TD>
													<asp:button id="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to close List of Requisition Raised by Engineer screen"
														Text="Close" CausesValidation="False"></asp:button></TD>
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
