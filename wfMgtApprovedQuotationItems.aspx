<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfMgtApprovedQuotationItems.aspx.vb" Inherits="Flypal.wfMgtApprovedQuotationItems" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Management Approved Quotation Item</title>
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
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body bottomMargin="5" leftMargin="5" topMargin="5" rightMargin="5" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlMain" Cssclass="clsPanel1" Runat="server">
							<TABLE id="tblLedgerList" class="clstablelistin">
								<TR>
									<TD colSpan="3">
										<asp:label id="lblLedgerList" runat="server" Cssclass="clstitle1">Mgt. Approved Quotation Items</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="Label2" runat="server" Cssclass="clsLabelAuto" Font-Bold="True">Select Order Date and select the Part you want to Add in Order.</asp:label></TD>
								</TR>
								<TR>
									<TD>
										<TABLE id="Table2">
											<TR>
												<TD>
													<asp:label id="lblOrderDate" runat="server" Cssclass="clsLabelAuto">Order Date</asp:label></TD>
												<TD>
													<uc1:sicalendar id="calOrderDate" runat="server"></uc1:sicalendar></TD>
											</TR>
										</TABLE>
									</TD>
									<TD>
										<TABLE>
											<TR>
												<TD>
													<asp:label id="lblPartNo" runat="server" Cssclass="clsLabelAuto">Part No</asp:label></TD>
												<TD>
													<asp:TextBox id="txtSearch" runat="server" CssClass="clsTextBox" ToolTip="Enter Part No"></asp:TextBox></TD>
											</TR>
										</TABLE>
									</TD>
									<TD align="right">
										<TABLE id="Table1">
											<TR>
												<TD>
													<asp:button id="btnFindNow" runat="server" Cssclass="clsButton" ToolTip="Click To Find " Text="Find Now"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 14px" colSpan="3">
										<asp:label id="Label1" runat="server" Cssclass="clsLabelAuto">Select Part from the list and check to select the Part Information or click on Back button to go back to previous page.</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblResult" runat="server" Cssclass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgPartList" runat="server" Cssclass="clsGrid" AllowPaging="True" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ItemID" HeaderText="Item ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="PartNo" HeaderText="Part No.">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="PartDescription" HeaderText="Part Description"></asp:BoundColumn>
												<asp:BoundColumn DataField="StockQTY" HeaderText="Stock Qty.">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="TotalPendingQTY" HeaderText="Total Req. Qty.">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblResult1" runat="server" Cssclass="clsLabelHeader" Visible="False">lblResult</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3" align="left">
										<asp:datagrid id="dgQuotationItems" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
											PageSize="5" OnPageIndexChanged="NewPageofQuotationItems">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="Select">
													<ItemTemplate>
														<asp:CheckBox id=chkSelect Runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>' AutoPostBack="True">
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="QuotationDateFormatted" HeaderText="Date"></asp:BoundColumn>
												<asp:BoundColumn DataField="QuotationNo" HeaderText="Quotation No."></asp:BoundColumn>
												<asp:BoundColumn DataField="VendorName" HeaderText="Supplier"></asp:BoundColumn>
												<asp:BoundColumn DataField="Qty" HeaderText="Quotation Qty.">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="PurchaseBalQty" HeaderText="Pending Order Qty.">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:TemplateColumn HeaderText="Order Qty.">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:TextBox id=txtOrderQty runat="server" CssClass="clsTextBoxRightAlignSmall" ToolTip="Enter Order Qty" Text='<%# DataBinder.Eval(Container.DataItem,"OrderQty") %>' Width="47px">
														</asp:TextBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="CRate" HeaderText="Rate"></asp:BoundColumn>
												<asp:BoundColumn DataField="Currency" HeaderText="Currency">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Visible="False" Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD colSpan="3" align="right">
										<TABLE class="clstableButton" align="right">
											<TR>
												<TD>
													<asp:button id="btnOk" runat="server" Cssclass="clsButton" ToolTip="Click To  Add the Item In Order"
														Text="OK"></asp:button></TD>
												<TD>
													<asp:button id="btnBack" runat="server" Cssclass="clsButton" ToolTip="Click To Go Back To Order Detail"
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
