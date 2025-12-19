<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnResourceAllocationForAMOJob.aspx.vb" Inherits="Flypal.wfnResourceAllocationForAMOJob" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Allocation of Resource(s) </title>
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox runat="server" ID="MSGBoxCtrl" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblmain" class="clstablelistout" border="0">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin" border="0">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">
                                                            Resource Allocation
														</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="Right">
												<asp:Button ID="btnSave" runat="server" CausesValidation="true"
													CssClass="clsbtnH clsinfoH" Text="Allocate"
													ToolTip="Click to Allocate Job for selected Employee(s)"
													ValidationGroup="valGroup1" />
												<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH"
													Text="Close" ToolTip="Click to go to the Previous Page" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
												CssClass="clsValidationSummary" ValidationGroup="valGroup1"></asp:ValidationSummary>
											<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Please select at least One Employee from list."
												ControlToValidate="txtWOJobDescription" Display="None" ClientValidationFunction="validateEmp"
												ValidationGroup="valGroup1" CssClass="clsLabelAuto"></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<script type="text/javascript">
									//Training Org Name

									function validateEmp(source, args) {
										args.IsValid = false;
										var NoOfEmp = $("#chkEmployeeList input:checked").length;
										if (NoOfEmp > 0) {
											args.IsValid = true;
											return;
										}
									}

								</script>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlJobDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsJobInfo" class="clsFieldSetNewStyle">
												<legend id="lblJobDetails" runat="server">
													<b>Job Details</b>
												</legend>
												<table id="Table1" border="0" width="100%">
													<tr>
														<td>
															<span id="lblTaskNo" class="clsLabelAuto">Task No. </span>
														</td>
														<td>
															<asp:TextBox ID="txtTaskNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Task No."
																Text="<%# mnWOJob.TaskCardNo %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblWODate" class="clsLabelAuto">Date</span>
														</td>
														<td>
															<asp:TextBox ID="txtDate" runat="server" CssClass="clsTextBoxTagSearchDate" ToolTip="Date"
																Text="<%# mnWOJob.WODateFormatted %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
														</td>
														<td>
															<span id="lblWONo" class="clsLabelAuto">WO No.</span>
														</td>
														<td>
															<asp:TextBox ID="txtWONo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="WO No."
																Text="<%# mnWOJob.WONumber %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblDescription" runat="server" class="clsLabel">Description</span>
														</td>
														<td colspan="3">
															<asp:TextBox ID="txtWOJobDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
																Text="<%# mnWOJob.WOJobDescription %>" BackColor="#E0E0E0" ReadOnly="true"
																MaxLength="500" ToolTip="Enter Description" TextMode="MultiLine"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td><span id="Span3" runat="server" class="clsLabel">Customer Info.</span></td>
														<td>
															<asp:TextBox ID="txtCustomer" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ToolTip="Customer"
																BackColor="#E0E0E0" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
														</td>
														<td>
															<span id="lblSkill" class="clsLabel">Skill</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbSkillcode" runat="server" CssClass="clsTextBoxTagSearchComboSmall" BackColor="#E0E0E0" ReadOnly="true" Enabled="false"
																SelectedValue="<%# mnWOJob.SkillID %>" DataTextField="Code"
																DataValueField="Id" AutoPostBack="false" />
														</td>
													</tr>
													<tr>
														<td><span id="Span6" runat="server" class="clsLabel">Model</span></td>
														<td>
															<asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Model"
																Text="<%# mnWO.ModelName %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
														</td>
														<td>
															<span id="lblSerialno" class="clsLabel">Serial No.</span>
														</td>
														<td>
															<asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Serial No."
																Text="<%# mnWO.SerialNo %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td><span id="Span4" runat="server" class="clsLabel">Zone</span></td>
														<td>
															<asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Zone"
																Text="<%# mnWOJob.Zone %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
														</td>
														<td><span id="Span5" runat="server" class="clsLabel">Area</span></td>
														<td>
															<asp:TextBox ID="txtArea" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Area"
																Text="<%# mnWOJob.Area %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlResourceAllocationInfo" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsRenewalInfo" class="clsFieldSetNewStyle">
												<legend id="ldgRenewalInfo" runat="server">
													<b>Resource Allocation Details</b>
												</legend>
												<table width="100%">
													<tr>
														<td colspan="5" align="right">
															<asp:CheckBox ID="chkShowAllEmp" runat="server"
																AutoPostBack="true" class="clsLabel"
																Text="Show All Technical Crew(s)" />
														</td>
													</tr>
													<tr>
														<td>
															<span id="Span1" class="clsLabelStar" style="color: Red">*</span>
														</td>
														<td>
															<span id="Span2" class="clsLabelAuto">Employees</span>
														</td>
														<td colspan="3">
															<table width="100%">
																<tr>
																	<td colspan="2">
																		<asp:Panel ID="pnlEmpList" runat="server" ClientIDMode="Static" Visible="true">
																			<asp:CheckBoxList ID="chkEmployeeList" runat="server" CssClass="clsFieldSetNewStyle"
																				ClientIDMode="Static" DataValueField="ID" DataTextField="Name" RepeatColumns="4"
																				RepeatDirection="Horizontal" Font-Bold="true" />
																			</asp:CheckBoxList>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td colspan="4">
															<asp:Label runat="server" ID="lblNote">
																<b>Note : </b> 
																Employee marked <span style="color: red;">RED </span> 
																has either Document or Training due.
																Therefore, cannot be allocated.
															</asp:Label>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>

		<div id="divSpinner">

			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader">
					</div>
					<div class="divAjaxLoader">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
									ImageAlign="Middle" CssClass="ajax-loader-gif" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>

		</div>

		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForResourceAllocation();
				return false;
			}
		</script>

		<!--Set page layout when open as popup aspx page-->
		<script type="text/javascript">

            <% Dim mOpenAs As String = Request.QueryString("Type") %>
            <% If Not mOpenAs Is Nothing AndAlso mOpenAs = "pup" Then %>  

			$(document).ready(function () {

				SetPageLayout();
				if ($.browser.msie) {
					parent.IFrameResourceAllocationComplete();
				}

			});

            <% End if %>

			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
			function endRequestHandler() {
				SetPageLayout();
			}

			function SetPageLayout() {

                <% Dim mOpen As String = Request.QueryString("Type") %>
				<% If Not mOpen Is Nothing AndAlso mOpen = "pup" Then %>

				ReSetPageLayout();
				onResize();//for Top bottom link

                <% End if %>

			}

			function ReSetPageLayout() {

				$("body,html").css({ 'background-color': 'transparent' });
				var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
				var windowheight = $(window).height();
				if (tempMargtop >= windowheight) {
					$("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
				}
				else {
					var margintop = (windowheight / 2) - (tempMargtop / 2);
					margintop = margintop;
					$("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
				}

			}

		</script>

		<script type="text/javascript">

			//check all/ uncheck all checkbox of aircraft list
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

				$("#chkSelectAllEmp").click(function () {
					var status = $("#chkSelectAllEmp").attr("checked");
					$("#chkEmployeeList").find(":checkbox").each(function () {
						var enableStatus = $(this).attr("disabled");
						if (!enableStatus) {
							if (status == "checked") {
								$(this).attr("checked", status);
							}
							else {
								$(this).removeAttr("checked");
							}
						}
					});
				});

			});

		</script>
	</form>
</body>
</html>
