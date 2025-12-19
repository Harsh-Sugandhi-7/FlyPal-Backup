<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfRequisitionItemNew.aspx.vb" Inherits="Flypal.wfRequisitionItemNew" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Requisition Item Details</title>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<meta name="vs_showGrid" content="False">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body bottomMargin="5" leftMargin="5" rightMargin="5" topMargin="5" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table id="tblMain" class="clstablelistout" border="0">
				<tr>
					<td><asp:panel id="pnlMain" Cssclass="clspnl1" Runat="server">
							<TABLE id="tblinner" class="clsTablelistin" border="0">
								<TR>
									<TD colSpan="3">
										<asp:label id="lblTitle" runat="server" Cssclass="clstitle1">Requisition Item [New]</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:ValidationSummary id="Validationsummary2" Runat="server" Cssclass="clsValidationSummary" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
										<asp:RequiredFieldValidator id="rfvPartNo" runat="server" Display="None" CssClass="clsLabelAuto" ControlToValidate="txtPartNo"
											ErrorMessage="Part Required"></asp:RequiredFieldValidator>
										<asp:RequiredFieldValidator id="rfvQuantity" runat="server" Display="None" CssClass="clsLabelAuto" ControlToValidate="txtQty"
											ErrorMessage="Quantity Required"></asp:RequiredFieldValidator>
										<asp:RequiredFieldValidator id="rfvPartDesc" runat="server" Display="None" CssClass="clsLabelAuto" ControlToValidate="txtDescription"
											ErrorMessage="Part can't be saved without Description."></asp:RequiredFieldValidator></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblOrderInfo" runat="server" Cssclass="clsLabelHeader">Requisition Item Information</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblNote1" runat="server" Cssclass="clsLabelAuto">Enter the Details of Items Requested by selecting the Part No. from list and mention the Qty.</asp:label></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblSrNo" runat="server" Cssclass="clsLabelAuto">Sr. No.</asp:label></TD>
									<TD>
										<asp:TextBox id=txtSrNo runat="server" CssClass="clsTextBoxsmall" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.SrNo %>" MaxLength="5" BackColor="#E0E0E0" ReadOnly="True">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblPartNo1" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
									<TD>
										<asp:label id="lblPartNo" runat="server" Cssclass="clsLabelAuto">Part No.</asp:label></TD>
									<TD>
										<TABLE border="0" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:TextBox  id=txtPartNo runat="server" CssClass="clsTextBox" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo %>" Enabled="<%# (mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew) Or (mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty)) %>" ToolTip="Enter Part No.">
													</asp:TextBox></TD>
												<TD>
													<asp:Button id=imgbtnPartNo runat="server" CssClass="clsButtonGrid" Text="..." Enabled="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew %>" ToolTip="Click to Add New Part No." CausesValidation="False">
													</asp:Button></TD>
											</TR>
										</TABLE>
										<asp:customvalidator id="cvQty" runat="server" Display="None" ControlToValidate="txtQty" ErrorMessage="Quantity must be greater than Zero."
											OnServerValidate="customvalidate"></asp:customvalidator></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblDescription1" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
									<TD>
										<asp:label id="lblDesc" runat="server" Cssclass="clsLabelAuto">Description</asp:label></TD>
									<TD>
										<asp:TextBox id=txtDescription runat="server" CssClass="clsTextBoxlong" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Description %>" Enabled="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty) %>" ToolTip="Enter Description" TextMode="MultiLine">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblQuantity1" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
									<TD>
										<asp:label id="lblQuantity" runat="server" Cssclass="clsLabelAuto">Requested Quantity</asp:label></TD>
									<TD>
										<TABLE id="Table4" border="0" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:TextBox id=txtQty runat="server" CssClass="clsTextBoxRightAlign1" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty %>" MaxLength="8" ToolTip="Enter Requested Quantity">
													</asp:TextBox></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblAircraftStar" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
									<TD>
										<asp:label id="lblAircraft" runat="server" Cssclass="clsLabelAuto">Aircraft</asp:label></TD>
									<TD>
										<asp:DropDownList id=cmbMachine runat="server" CssClass="clsComboBox" SelectedValue="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID %>" DataTextField="RegNo" DataValueField="MachineID">
										</asp:DropDownList>
										<asp:CustomValidator id="cvMachine" runat="server" Display="None" ControlToValidate="cmbMachine" ErrorMessage="Aircraft Required"
											OnServerValidate="customvalidate"></asp:CustomValidator></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblWONo" runat="server" Cssclass="clsLabelAuto">WO No.</asp:label></TD>
									<TD>
										<asp:TextBox id=txtWONo runat="server" CssClass="clsTextBox" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo %>" MaxLength="50">
										</asp:TextBox>
										<asp:Button id="btnSelectWONo" runat="server" CssClass="clsButtonLong" Text="Select Work Order"
											CausesValidation="False"></asp:Button></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblNRCNo" runat="server" Cssclass="clsLabelAuto">NRC No.</asp:label></TD>
									<TD>
										<asp:TextBox id=txtNRCNo runat="server" CssClass="clsTextBox" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.NRCNo %>" MaxLength="50" ToolTip="Enter NRC No.">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblIPCReference" runat="server" Cssclass="clsLabelAuto">IPC Reference</asp:label></TD>
									<TD>
										<asp:TextBox id=txtReference runat="server" CssClass="clsTextBox" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference %>" MaxLength="100" ToolTip="Enter IPC Reference">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblJobDescription" runat="server" Cssclass="clsLabelAuto">Reason For Request</asp:label></TD>
									<TD>
										<asp:TextBox id=txtReasonForRequest runat="server" CssClass="clsTextBoxlong" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForRequest %>" MaxLength="1000" ToolTip="Enter Reason For Request" TextMode="MultiLine">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label  id="Label2" runat="server" Cssclass="clsLabelAuto">Reason For Purchase</asp:label></TD>
									<TD>
										<asp:TextBox  id=txtReasonForPurchase runat="server" CssClass="clsTextBoxlong" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForPurchase %>" MaxLength="1000" ToolTip="Enter Reason For Purchase" TextMode="MultiLine">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 14px; HEIGHT: 20px"></TD>
									<TD style="HEIGHT: 20px">
										<asp:label id="lblPriority" runat="server" Cssclass="clsLabelAuto">Priority</asp:label></TD>
									<TD>
										<TABLE id="Table7" border="0" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:DropDownList id=cmbPriority runat="server" CssClass="clsComboBox" SelectedValue="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.PriorityID %>" DataTextField="Name" DataValueField="ID" AutoPostBack="true">
													</asp:DropDownList>
                                                </TD>
                                                <td>
                                                <asp:TextBox ID="txtDays" runat="server" CssClass="clsTextBoxSmall" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Days %>" MaxLength="4" ToolTip="Enter No. Of Days" Enabled="false"></asp:TextBox>
                                                <span id="lblInDays" class="clsLabel">In Days</span>
                                                </td>
                                                
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblRemark" runat="server" Cssclass="clsLabelAuto">Remark</asp:label></TD>
									<TD>
										<asp:TextBox id=txtRemark runat="server" CssClass="clsTextBoxlong" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Remark %>" MaxLength="500" ToolTip="Enter Remark" TextMode="MultiLine">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblNote" runat="server" Cssclass="clsLabelAuto">Note</asp:label></TD>
									<TD>
										<asp:TextBox id=txtNote runat="server" CssClass="clsTextBoxlong" Text="<%# mRequisitionNew.RequisitionItemsNew.CurrentItem.Note %>" MaxLength="500" ToolTip="Enter Note" TextMode="MultiLine">
										</asp:TextBox></TD>
								</TR>
								<TR>
									<TD align="right"></TD>
									<TD colSpan="2" align="right">
										<TABLE id="Table1" border="0">
											<TR>
												<TD>
													<asp:button id="btnSave" runat="server" Cssclass="clsButton" Text="Ok" ToolTip="Click to add Item in Requisition Item List"></asp:button></td>
                                                    <td>
													<asp:button id="btnBack" runat="server" Cssclass="clsButton" Text="Back" ToolTip="Click to go back to the previous page"
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
