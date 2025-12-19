<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfKitList_Ajax.aspx.vb"
	EnableViewState="True" Inherits="Flypal.wfKitList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Inspection Kit List</title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>	

	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>

</head>
<body bottommargin="5" ms_positioning="GridLayout" leftmargin="0" topmargin="0" rightmargin="0">
	<form id="wfgroup" method="post" runat="server">

		<%--AJAX- ScriptManager Added--%>
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true" runat="server">
		</asp:ScriptManager>
		<%--AJAX- Add MSGBox Control--%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>

		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
						<table id="tblInner" class="clsFormHeader">
							<tr>								
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:Label runat="server" class="clsFormHeader displayBlock">
													Inspection Kit List
												</asp:Label>
											</td>										
											<td align="right">
												<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAdd" runat="server" CausesValidation="False"
																		CssClass="clsbtnH clsinfoH" Text="Add New"
																		ToolTip="Click to Add New Inspection Kit" />
																</td>
																<td>
																	<asp:Button ID="btnClose" runat="server" CausesValidation="False"
																		CssClass="clsbtnH clsinfoH" Text="Close"
																		ToolTip="Click to close Inspection Kit List screen" />
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
								<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
								<td id="tdFavICN" align="center">
									<span id="spFavICN">
										<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
											class="fa fa-star fa-spin fa-5x circle-icon"></i>
									</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlInspectionKitDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<span id="lblSearch" class="clsLabel">Search</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbLookIn" runat="server" AutoPostBack="True"
															CssClass="clsTextBoxTagSearchComboNewstyle">
															<asp:ListItem Value="0">All</asp:ListItem>
															<asp:ListItem Value="1">Name</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td align="right"></td>
												</tr>
												<tr>
													<td>
														<span id="lblFor" class="clsLabel">For</span>
													</td>
													<td>
														<asp:TextBox ID="txtFor" runat="server" Width="180px"
															CssClass="clsTextBoxTagSearch" MaxLength="50"
															BackColor="#E0E0E0" ReadOnly="True" ToolTip="Enter search Name">
														</asp:TextBox>
													</td>
													<td align="right">
														<asp:ImageButton ID="SearchButton" runat="server"
															ImageUrl="~/images/Search2.png"
															ToolTip="Click to search as per Criteria."
															CausesValidation="false" class="clsSearch2btn" />
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlGridViewTitle" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
												List of Inspection Kit as per criteria : Record(s) found.
											</asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:GridView ID="dgKitList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
												CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="8" 
												ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" 
												PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
															 ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="ID" HeaderText="Id" Visible="False"></asp:BoundField>
													<asp:BoundField DataField="KitName" HeaderText="Inspection Kit Name" SortExpression="KitName">
														<HeaderStyle HorizontalAlign="Left" />
														<ItemStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
														<HeaderStyle HorizontalAlign="Center" />
														<ItemStyle HorizontalAlign="Center" />
														<ItemTemplate>
															<div id="dropDownImg" class="dropdown">
																<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" 
																	 runat="server" CssClass="clsActionbtn" />
																<div id="dropdownICN-content" class="dropdownbtn-content">
																	<table id="dropdown-content" class="clsGridNew_Ajax">
																		<tr>
																			<td>
																				<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																					CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																					ToolTip="Click to Edit record"
																					CommandName="EditRec" ImageUrl="~/images/edit.png" />
																			</td>

																			<td>
																				<asp:ImageButton ID="DeleteRec" class="actionICNS largerActionICNS" runat="server"
																					CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																					ToolTip="Click to Delete record"
																					CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
																			</td>
																		</tr>
																	</table>
																</div>
															</div>
														</ItemTemplate>
													</asp:TemplateField>													
												</Columns>
											</asp:GridView>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2" align="right">
									<asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
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

		<!-- Ajax Loader -->
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

		<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
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
