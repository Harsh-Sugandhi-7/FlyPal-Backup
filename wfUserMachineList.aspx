<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUserMachineList.aspx.vb" Inherits="Flypal.wfUserMachineList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
	<title>Current Aircraft rights</title>
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<link id="MainStyle" rel="stylesheet" type="text/css">

	<style type="text/css">
		.clsScroll {
			display: none !important;
		}
	</style>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript">

		$(document).ready(function () {

			$("#chkSelectAllAircraft").live("click", function () {

				var status = $("#chkSelectAllAircraft").attr("checked");

				$("#<%=GVMachine.ClientID %>").find(":checkbox").each(function () {

					if (status == "checked") {
						$(this).attr("checked", status);
					}
					else {
						$(this).removeAttr("checked");
					}

				});
			});

			return false;
		});

	</script>
</head>
<body>
	<form id="wfgroup" method="post" runat="server">
		<table id="Table-MaxWidth" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">
													Current Aircraft rights
												</asp:Label>
											</td>
											<td align="right">
												<asp:Button ID="btnSave" runat="server"
													CssClass="clsbtnH clsinfoH" Text="Save"
													ToolTip="Click to save the current record" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:ValidationSummary ID="ValidationSummary1" runat="server"
										CssClass="clsValidationSummary"></asp:ValidationSummary>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Label ID="lblnote" runat="server" CssClass="clsLabelHeader">
										Select user from the list and click on save button to set current aircraft rights
									</asp:Label>
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td>
									<asp:Label ID="lblAircraftList" runat="server" CssClass="clsLabelHeader">
										List of User
									</asp:Label>
								</td>
							</tr>
							<tr>
								<td>
									<asp:GridView ID="GVMachine" runat="server" AutoGenerateColumns="False"
										ToolTip="Click to select Aircraft for the User from List"
										CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
										<AlternatingRowStyle CssClass="clsdgAltItem" />
										<RowStyle CssClass="clsdgItem" />
										<HeaderStyle BackColor="white" CssClass="clsdgHeader"
											Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
										<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
										<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
										<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
										<Columns>
											<%--0--%>
											<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
											<%--1--%>
											<asp:TemplateField>
												<HeaderTemplate>
													<asp:CheckBox ID="chkSelectAllAircraft" ClientIDMode="Static"
														runat="server"></asp:CheckBox>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:CheckBox ID="chkSelect" runat="server"
														Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<%--2--%>
											<asp:BoundField DataField="UserName" HeaderText="User"></asp:BoundField>
											<%--3--%>
											<asp:BoundField DataField="RoleNames" HeaderText="Assigned Roles"></asp:BoundField>
											<%--4--%>
											<asp:BoundField DataField="MachineNames" HeaderText="Aircraft Rights"></asp:BoundField>
										</Columns>
									</asp:GridView>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>
	</form>
</body>
</html>
