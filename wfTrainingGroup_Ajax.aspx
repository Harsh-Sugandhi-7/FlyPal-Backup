<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTrainingGroup_Ajax.aspx.vb"
	EnableViewState="True" Inherits="Flypal.wfTrainingGroup_Ajax" %>

<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Group Training</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" language="javascript">

		function OpenLocation(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}

	</script>

</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<%--AJAX- Add MSGBox Control--%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox runat="server" ID="MSGBoxCtrl" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td colspan="4" class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Training Group [New]</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:Button ID="btnAdd" runat="server" CausesValidation="False"
													CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click to add the new Group" />
												<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
													Text="Save" ToolTip="Click to save the Group" />
												<asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
													Text="Close" ToolTip="Click to close Group Training" />
											</td>
										</tr>
									</table>
								</td>
								<%--Added by Harsh on 15th July 2024 for FLYPAL 1757--%>
								<td id="tdFavICN" align="center">
									<span id="spFavICN">
										<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
											class="fa fa-star fa-spin fa-5x circle-icon"></i>
									</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
											<asp:CustomValidator ID="cvLocation" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbDesignation"
												Display="None" ErrorMessage="Designation Required" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtGroupName"
												Display="None" ErrorMessage="Group Name Required " OnServerValidate="customvalidate"
												ValidateEmptyText="true"></asp:CustomValidator>

											<asp:CustomValidator ID="cvDate" runat="server" CssClass="clsLabelAuto" Display="None"
												ErrorMessage=""></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlGroupDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td colspan="2">
														<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
															<table>
																<tr>
																	<td align="left">
																		<span id="lblName1" class="clsLabelStar">*</span>
																	</td>
																	<td align="left">
																		<asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto" Width="61px">Name</asp:Label>
																	</td>
																	<td>
																		<table id="Table2">
																			<tr>
																				<td>
																					<asp:TextBox ID="txtGroupName" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="50"
																						ToolTip="Enter Group Name" Width="272px"></asp:TextBox>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td align="left">
																		<span id="lblLocation1" class="clsLabelStar">*</span>
																	</td>
																	<td align="left">
																		<span id="lblLocation" class="clsLabelAuto">Designation</span>
																	</td>
																	<td>
																		<table>
																			<tr>
																				<td>
																					<asp:DropDownList ID="cmbDesignation" runat="server"
																						CssClass="clsTextBoxTagSearchComboNewstyleLong"
																						DataTextField="Name" DataValueField="ID" Width="275px">
																					</asp:DropDownList>
																				</td>
																				<td></td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td align="left">
																		<span id="Span2" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="Span1" class="clsLabelAuto">Trainings</span>
																	</td>
																	<td>
																		<asp:Panel ID="pnlTrainingList" runat="server" ClientIDMode="Static" Visible="true">
																			<asp:CheckBoxList ID="chkTrainingList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
																				ClientIDMode="Static" DataValueField="ID" DataTextField="Name" RepeatColumns="10"
																				RepeatDirection="Horizontal">
																			</asp:CheckBoxList>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</fieldset>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table width="100%">
															<tr>
																<td>
																	<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Training Group List</asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	<asp:GridView ID="dgTrainingGroupList" runat="server" AutoGenerateColumns="False"
																		AllowSorting="True" ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle"
																		GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10">
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<RowStyle CssClass="clsdgItem" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																		<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																		<Columns>
																			<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																			<asp:BoundField Visible="False" DataField="mTrainingID" SortExpression="mTrainingID"
																				HeaderText="mTrainingID">
																				<HeaderStyle></HeaderStyle>
																			</asp:BoundField>
																			<asp:BoundField DataField="GroupName" HeaderText="Group Name">
																				<HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px"></HeaderStyle>
																				<ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
																			</asp:BoundField>
																			<asp:BoundField DataField="DesignationName" HeaderText="Designation Name">
																				<HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px"></HeaderStyle>
																				<ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
																			</asp:BoundField>
																			<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																				<HeaderStyle HorizontalAlign="Center" CssClass="TextBreak" />
																				<ItemStyle HorizontalAlign="Center" Wrap="true" />
																				<ItemTemplate>
																					<div id="dropDownImg" class="dropdown">
																						<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																						<div id="dropdownICN-content" class="dropdownbtn-content">
																							<table id="dropdown-content" class="clsGridNew_Ajax">
																								<tr>
																									<td>
																										<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																											CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																											ToolTip="Click to Edit record" CausesValidation="false"
																											CommandName="EditRec" ImageUrl="~/images/edit.png" />
																									</td>

																									<td>
																										<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																											CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																											ToolTip="Click to Delete record"
																											CommandName="DeleteRec" ImageUrl="~/images/delete.png" CausesValidation="false" />
																									</td>
																								</tr>
																							</table>
																						</div>
																					</div>
																				</ItemTemplate>
																			</asp:TemplateField>
																		</Columns>
																	</asp:GridView>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="2" align="right">
									<asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<%--Added by Harsh on 15th July 2024 for FLYPAL 1757--%>
													<td>
														<asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
															Style="display: none;"></asp:Button>
														<asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">
			 <% Dim mopen As String = Request.QueryString("Type") %>
			 <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

					$(document).ready(function () {
						SetPageLayout();
						if ($.browser.msie) {
							parent.IFrameATAStateComplete();
						}
					});

			 <% End if %>
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
			function endRequestHandler() {
				SetPageLayout();
			}

			function SetPageLayout() {
				<% Dim mopenas As String = Request.QueryString("Type") %>
				<% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
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
					$("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
				}

			}
		</script>
		<%--End--%>

		<%--Added by Harsh on 15th July 2024 for FLYPAL 1757--%>
		<script type="text/javascript">
			function fnMarkFavouriteUnFavourite(x) {
				if (x.classList.contains("fa-star")) {
					x.classList.remove("fa-star");
					x.classList.add("fa-star-o");
					x.style.color = 'black';
					x.style.border = 'black';
					$("#hdnBtnRemoveFavourite").click();
				}
				else {
					x.classList.remove("fa-star-o");
					x.classList.add("fa-star");
					x.style.color = '#fff';
					x.style.border = 'black';
					$("#hdnBtnMarkFavourite").click();
				}
			}
			function MarkAsFavourite() {
				var redstar = document.getElementById("<%=favICN.ClientID%>");
				redstar.classList.add("fa-star");
				redstar.classList.remove("fa-star-o");
				redstar.style.color = '#fff';
				redstar.style.border = 'black';

			}
			function RemoveFromFavourite() {
				var redstar = document.getElementById("<%=favICN.ClientID%>");
				redstar.classList.add("fa-star-o");
				redstar.classList.remove("fa-star");
				redstar.style.border = 'black';
			}
		</script>

	</form>
</body>
</html>
