<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCopyModification_Ajax.aspx.vb"
	Inherits="Flypal.wfCopyModification_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
	<title>Invoice Payment Details</title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript" id="clientEventHandlersJS">
		function openTranDetail() {
			str = "wfReports.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfFileView.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}

		//this function takes a value (ltext) and transmits that to the left hand frame

		function tranRight(ltext) {
			parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

		}
	</script>
</head>
<body>
	<form id="wfgroup" method="post" runat="server">
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				$('.cbSelectRow').change(function () {
					// detect if the checkbox is checked
					var checked = $(this).prop('checked');
					// gets the table row indiect parent
					var trParent = $(this).closest('tr');
					// add or remove the css class according to the check state
					if (checked == true)
						trParent.addClass('clslightColor')
					else
						trParent.removeClass('clslightColor');
				})
					// the each is used when postback is triggered with checked rows
					.each(function (index, element) {
						var checked = $(element).prop('checked');
						if (checked == true)
							$(element).closest('tr').addClass('clslightColor');
						else
							$(element).closest('tr').removeClass('clslightColor');
					});
				// select all click
				$("#chkSelectAll").change(function () {
					var checked = $(this).prop('checked');
					$('.cbSelectRow').prop('checked', checked).trigger('change');
				});


			});

		</script>
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<asp:UpdatePanel ID="upnlCopyDirectives" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="tblInner" class="clstablelistin">
										<tr>
											<td colspan="6" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblTitle" TabIndex="1" CssClass="clsFormHeader" runat="server">Copy Directives</asp:Label>
														</td>
														<td align="right">
															<asp:Button ID="btnNewModel" runat="server" CssClass="clsbtnH clsinfoH" Text="New Model"
																ToolTip="Click to Add New Model" Enabled="<%# cmbAssemblyType.SelectedIndex%>"
																CausesValidation="False"></asp:Button>
															<asp:Button ID="btnSaveLog" runat="server" CssClass="clsbtnH clsinfoH" Text="Save Log File"
																ToolTip="Click to Save Details" CausesValidation="False" Visible="False"></asp:Button>
															<asp:Button ID="btnCopyTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Copy"
																ToolTip="Click to Copy Directive" Enabled="<%# cmbAssemblyType.SelectedIndex%>"></asp:Button>
															<asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
																ToolTip="Click to Close" CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="6">
												<asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
													CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvSourceModel" runat="server" ErrorMessage="Select Source Model from the list."
													ControlToValidate="cmbSourceModel" Display="None" ClientValidationFunction="ValidateSourceModel"></asp:CustomValidator>
												<asp:CustomValidator ID="cvDestinationModel" runat="server" ErrorMessage="Select Destination Model from the list."
													ControlToValidate="cmbDestinationModel" Display="None" ClientValidationFunction="ValidateDestinationModel"></asp:CustomValidator>
											</td>
										</tr>
										<tr>
											<td colspan="5">
												<asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader" DESIGNTIMEDRAGDROP="13">Copy Directives Details</asp:Label>
											</td>
										</tr>
										<tr>
											<td style="height: 14px">
												<asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
											</td>
											<td style="height: 14px">
												<asp:Label ID="lblAssemblyType" runat="server" CssClass="clsLabel">Assembly Type</asp:Label>
											</td>
											<td style="height: 14px">
												<asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
													DataValueField="ID" DataTextField="Name" AutoPostBack="True">
												</asp:DropDownList>
											</td>
											<td style="height: 14px"></td>
										</tr>
										<tr>
											<td style="height: 3px">
												<asp:Label ID="lblPartNoStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
											</td>
											<td style="height: 3px">
												<asp:Label ID="lblSourceModel" runat="server" CssClass="clsLabel">Source Model</asp:Label>
											</td>
											<td style="height: 3px">
												<asp:DropDownList ID="cmbSourceModel" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearchComboNewstyle"
													DataValueField="ID" DataTextField="ModelName">
												</asp:DropDownList>
											</td>
											<td style="height: 3px">
												<asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
											</td>
											<td style="height: 3px">
												<asp:Label ID="lblDestinationModel" runat="server" CssClass="clsLabelAuto" Width="104px">Destination Model</asp:Label>
											</td>
											<td style="height: 3px">
												<asp:DropDownList ID="cmbDestinationModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
													DataValueField="ID" DataTextField="ModelName">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<asp:Label ID="lblModificationType" runat="server" CssClass="clsLabelAuto">Directive Type</asp:Label>
											</td>
											<td colspan="4">
												<asp:DropDownList ID="cmbModificationType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" AutoPostBack="true"
													DataValueField="ID" DataTextField="CodeType">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td colspan="6">
												<asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">List of Duplicate Directives</asp:Label>
											</td>
										</tr>
										<tr>
											<td colspan="6">
												<asp:TextBox ID="txtListError" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
													ReadOnly="True" TextMode="MultiLine" Style="width: 702px; height: 62px;"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<br />
											</td>
										</tr>
										<tr>
											<td colspan="3">
												<asp:Label ID="lblModelModList" runat="server" CssClass="clsLabelHeader"></asp:Label>
											</td>
										</tr>
										<tr>
											<td colspan="6">
												<asp:GridView ID="dgModelModList" runat="server" AutoGenerateColumns="False"
													AllowSorting="true" EmptyDataText="No Records Found..." DataKeyNames="ID" ShowHeaderWhenEmpty="false"
													PageSize="10" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="true">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
														<asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
															<HeaderTemplate>
																<asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
															</HeaderTemplate>
															<ItemTemplate>
																<input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
																	<%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Left" />
														</asp:TemplateField>
														<asp:BoundField DataField="CodeNumber" SortExpression="CodeNumber" HeaderText="Code/Form No.">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														</asp:BoundField>
														<asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														</asp:BoundField>
														<asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														</asp:BoundField>
														<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
														</asp:BoundField>
														<asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														</asp:BoundField>
														<asp:BoundField DataField="Number" SortExpression="Number" HeaderText="Directive No.">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														</asp:BoundField>
														<asp:TemplateField HeaderText="Show In C of A">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox ID="chkCOfA" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "ShowInCofA") %>'></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" />
														</asp:BoundField>
														<asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
														</asp:BoundField>
														<asp:BoundField DataField="Applicability" HeaderText="Applicability">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" />
														</asp:BoundField>
														<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
															DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
													</Columns>
												</asp:GridView>
											</td>
										</tr>
										<tr>
											<td colspan="6" align="right">
												<table>
													<tr>
														<td>
															<asp:Button ID="btnCopy" runat="server" Visible="false" CssClass="clsbtnH clsinfoH" Text="Copy" ToolTip="Click to Copy Directive"
																Enabled="<%# cmbAssemblyType.SelectedIndex>0 %>"></asp:Button>
														</td>
														<td></td>
														<td>
															<asp:Button ID="btnClose" runat="server" Visible="false" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go back to the previous page"
																CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
						</asp:Panel>
					</td>
				</tr>
			</table>
			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
				runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
					</div>
					<div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
									Height="48px" Width="48px" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>
		</div>
		<script type="text/javascript">

			function ValidateSourceModel(source, args) {
				args.IsValid = false;
				var dd = $get("cmbSourceModel");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;
				}
			}
		</script>
		<script type="text/javascript">

			function ValidateDestinationModel(source, args) {
				args.IsValid = false;
				var dd = $get("cmbDestinationModel");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;
				}
			}
		</script>
	</form>
</body>
</html>
