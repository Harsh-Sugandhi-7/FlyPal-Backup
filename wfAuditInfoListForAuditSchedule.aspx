<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfAuditInfoListForAuditSchedule.aspx.vb" Inherits="Flypal.wfAuditInfoListForAuditSchedule" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Audit List</title>
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
		<script language="javascript">
                function OpenLocation(FileName)
                   {
                     window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 
                   }
		</script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body bottomMargin="5" leftMargin="5" topMargin="5" rightMargin="5" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
         <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE id="tblInner" class="clstablelistin">
								<TR>
									<TD colSpan="3">
										<asp:Label id="lblTitle" CssClass="clstitle1" Runat="server">Audit List</asp:Label></TD>
								</TR>
								<TR>
									<TD>
										<TABLE id="Table4" cellSpacing="0">
											<TR>
												<TD>
													<asp:Label id="lblSearch" runat="server" CssClass="clsLabel" Width="48px" Height="10px">Search</asp:Label></TD>
												<TD>
													<TABLE id="Table5">
														<TR>
															<TD>
																<asp:dropdownlist id="cmbSearch" runat="server" CssClass="clsComboBox" Width="170px" AutoPostBack="True">
																	<asp:ListItem Value="0">All</asp:ListItem>
																	<asp:ListItem Value="2">Text</asp:ListItem>
																	<asp:ListItem Value="3">Audit Type</asp:ListItem>
																</asp:dropdownlist></TD>
															<TD>
																<P>
																	<asp:DropDownList id="cmbAuditType" runat="server" CssClass="clsComboBox1" DataValueField="ID" DataTextField="Name"
																		Visible="False"></asp:DropDownList>
																	<asp:textbox id="txtAuditSearchText" runat="server" CssClass="clsTextBox2" Visible="False" BackColor="White"
																		ToolTip="Enter Search Text"></asp:textbox></P>
															</TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
									</TD>
									<TD colSpan="2" align="right">
										<TABLE id="Table2" border="0">
											<TR>
												<TD>
													<asp:button id="btnFindNow" runat="server" ToolTip="Click to find Audit List as per searching criteria"
														Cssclass="clsButton" Text="Find Now"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:Label id="lblInfo" runat="server" CssClass="clsLabelAuto">Select Audit from the list. Click On Edit/View link To Modify The Selected Audit. Click On Delete link To Delete the Selected Audit. Click On Add  New button To Add A New Audit.Click on View link to view the attachment. Click on Select link to select the audit.</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:label id="lblResult" runat="server" Cssclass="clsLabelHeader"></asp:label></TD>
									<TD align="right">
										<TABLE id="Table3" border="0">
											<TR>
												<TD>
													<asp:button id="btnAddTop" runat="server" ToolTip="Click to Add New Audit" Cssclass="clsButton"
														Text="Add New"></asp:button></TD>
												<TD>
													<asp:button id="btnBackTop" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous page"
														Text="Close" CausesValidation="False"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgPendingList" runat="server" CssClass="clsGrid" AllowSorting="True" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="AuditNo" SortExpression="AuditNo" HeaderText="Audit No.">
													<HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="AuditTypeName" SortExpression="AuditTypeName" HeaderText="Audit Type">
													<HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Reference No.">
													<HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="NextScheduleText" SortExpression="NextScheduleText" HeaderText="Next Schedule">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
												<asp:TemplateColumn HeaderText="Attach">
													<ItemTemplate>
														<asp:LinkButton ID="LinkButton1" runat="server" Text="View" CommandName="View" CausesValidation="false"></asp:LinkButton>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="ImageSize" HeaderText="ImageSize"></asp:BoundColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD colSpan="3" align="right">
										<TABLE id="Table1" border="0">
											<TR>
												<TD>
													<asp:button id="btnAdd" runat="server" ToolTip="Click to Add New Audit" Cssclass="clsButton"
														Text="Add New"></asp:button></TD>
												<TD>
													<asp:button id="btnBack" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous page"
														Text="Close" CausesValidation="False"></asp:button></TD>
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
