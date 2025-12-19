<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfAttachFiles.aspx.vb" Inherits="Flypal.wfAttachFiles" %>
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
      <TABLE class=clstablelistin id=tblInner>
        <TR>
          <TD colSpan=4>
<asp:Label id=lbltitle CssClass="clstitle1" Runat="server">Attach Files [New]</asp:Label></TD></TR>
        <TR>
          <TD colSpan=4>
<asp:ValidationSummary id=ValidationSummary1 runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>&nbsp; 
<asp:RequiredFieldValidator id=rfvName runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName" Display="None" ErrorMessage="Name Required"></asp:RequiredFieldValidator>
<asp:customvalidator id=cvDocumentType runat="server" ControlToValidate="cmbDocumentType" Display="None" ErrorMessage="Select Document Type from the list." OnServerValidate="CustomValidate"></asp:customvalidator></TD></TR>
        <TR>
          <TD colSpan=3>
<asp:label id=lblAdd runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:label></TD>
          <TD align=right colSpan=1>
<asp:button id=btnAdd runat="server" CssClass="clsButton" Text="New" ToolTip="Click to Add New File." CausesValidation="False"></asp:button></TD></TR>
        <TR>
          <TD colSpan=4>
<asp:label id=lblAttachFileDetails runat="server" CssClass="clsLabelHeader">Attach File Details</asp:label></TD></TR>
        <TR>
          <TD>
<asp:Label id=lblNameStar1 runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
          <TD>
<asp:label id=lblName runat="server" CssClass="clsLabelAuto">Name</asp:label></TD>
          <TD>
            <TABLE id=Table2>
              <TR>
                <TD>
<asp:TextBox id=txtName runat="server" CssClass="clsTextBox" Text="<%# mAttachFileDetail.Name %>" ToolTip="Enter Document Type Name" MaxLength="50">
													</asp:TextBox></TD></TR></TABLE></TD>
          <TD align=right></TD></TR>
        <TR>
          <TD></TD>
          <TD>
<asp:label id=lblPath runat="server" CssClass="clsLabelAuto">Path</asp:label></TD>
          <TD>
            <TABLE id=Table3>
              <TR>
                <TD>
<asp:TextBox id=txtPath runat="server" CssClass="clsTextBoxAuto" Text="<%# mAttachFileDetail.Path %>" MaxLength="50" ReadOnly="True" BackColor="LightGray">
													</asp:TextBox><INPUT id=MyFile type=file size=40 
                  name=MyFile RunAt="Server"> 
<asp:DropDownList id=cmbAttach runat="server" CssClass="clsComboBoxSmall" Height="27px" AutoPostBack="True">
														<asp:ListItem Value="Existing">Existing</asp:ListItem>
														<asp:ListItem Value="Browse">Browse</asp:ListItem>
													</asp:DropDownList>
<asp:Button id=btnAttach runat="server" CssClass="clsButton" Text="Attach" ToolTip="Click to attach a File" CausesValidation="False"></asp:Button></TD></TR></TABLE></TD>
          <TD></TD></TR>
        <TR>
          <TD style="HEIGHT: 13px">
<asp:Label id=lblDocumentStar1 runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
          <TD style="HEIGHT: 13px">
<asp:label id=lblDocument runat="server" CssClass="clsLabelAuto">Document</asp:label></TD>
          <TD style="HEIGHT: 13px">
            <TABLE id=Table4>
              <TR>
                <TD>
<asp:DropDownList id=cmbDocumentType runat="server" CssClass="clsComboBox" SelectedValue="<%# mAttachFileDetail.DocumentTypeID %>" DataTextField="Name" DataValueField="ID">
													</asp:DropDownList>
<asp:Button id=imgbtnDocumentType runat="server" CssClass="clsButtonGrid" Text="..." ToolTip="Click to Add New Document." CausesValidation="False"></asp:Button></TD></TR></TABLE></TD>
          <TD style="HEIGHT: 13px"></TD></TR>
        <TR>
          <TD></TD>
          <TD>
<asp:label id=lblRemark runat="server" CssClass="clsLabelAuto">Remark</asp:label></TD>
          <TD>
            <TABLE id=Table5>
              <TR>
                <TD>
<asp:TextBox id=txtRemark runat="server" CssClass="clsTextBoxAuto" Text="<%# mAttachFileDetail.Remark %>" MaxLength="50">
													</asp:TextBox></TD></TR></TABLE></TD>
          <TD></TD></TR>
        <TR>
          <TD colSpan=3>
<asp:label id=lblSave runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:label></TD>
          <TD align=right>
<asp:Button id=btnSave CssClass="clsButton" Runat="server" Text="Save" ToolTip="Click to save the current record."></asp:Button></TD></TR>
        <TR>
          <TD colSpan=4>
<asp:label id=lblResult runat="server" CssClass="clsLabelHeader"></asp:label></TD></TR>
        <TR>
          <TD colSpan=3>
<asp:datagrid id=dgAttachFileList runat="server" CssClass="clsGrid" ToolTip="Attached Files List" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
												<asp:BoundColumn DataField="DocumentTypeName" HeaderText="Document Type"></asp:BoundColumn>
												<asp:BoundColumn DataField="Remark" HeaderText="Remark"></asp:BoundColumn>
												<asp:ButtonColumn Text="View" HeaderText="View" CommandName="View"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Edit" HeaderText="Edit" CommandName="Edit"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
											</Columns>
										</asp:datagrid></TD>
          <TD align=right colSpan=1>
            <TABLE id=Table1 height="100%" cellSpacing=0 cellPadding=0 
            align=right border=0>
              <TR>
                <TD vAlign=top align=right></TD></TR>
              <TR>
                <TD></TD></TR>
              <TR>
                <TD vAlign=bottom align=right>
<asp:Button id=btnClose tabIndex=0 CssClass="clsButton" Runat="server" Text="Close" ToolTip="Click to go back to the previous page" CausesValidation="False"></asp:Button></TD></TR></TABLE></TD></TR></TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
