<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfModelList_Ajax.aspx.vb"
	Inherits="Flypal.wfModelList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Model List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script language="javascript" type="text/javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>

</head>
<body>
	<form id="form1" runat="server">
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
		<div>
			<table class="clstablelistout" id="tblMain">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="4" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Label ID="lblList" runat="server" CssClass="clsFormHeader">Model List</asp:Label>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td align="right">
													<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<table>
																<tr>
																	<td>
																		<asp:Button ID="btnAddNew" runat="server" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to add new Model"
																			Text="Add New" CausesValidation="False"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to close Model List screen"
																			Text="Close" CausesValidation="False"></asp:Button>
																	</td>
																</tr>
															</table>
														</ContentTemplate>
													</asp:UpdatePanel>
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
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fdswodetail" class="clsFieldSetNewStyle" style="border-width: 1px">
													<legend id="ldwodetail" runat="server"><b>Search Information</b></legend>
													<table id="Table1">
														<tr>
															<td>
																<span id="lblSearchModel" runat="server" class="clsLabelAuto">Model</span>
															</td>
															<td>
																<table>
																	<tr>
																		<td>
																			<asp:TextBox ID="txtFor" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
																				MaxLength="50"></asp:TextBox>
																		</td>
																	</tr>
																</table>
															</td>
															<td align="right">
																<table id="Table3">
																	<tr>
																		<td align="right">
																			<asp:UpdatePanel runat="server" ID="upnlFindnow" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Find the list of Model as per searching criteria"
																						Text="Find Now" CausesValidation="False" Visible="False"></asp:Button>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																</table>
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
										<asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Model as per criteria:  Record(s) found.</asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>

								</tr>
								<tr>
									<td colspan="2" align="left">
										<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:GridView ID="dgModelList" runat="server" ToolTip="Model List" AutoGenerateColumns="False"
													CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
														<asp:BoundField DataField="PrimaryModelName" HeaderText="Primary Model">
															<HeaderStyle ForeColor="black" HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="ManufacturerName" HeaderText="Manufacturer" SortExpression="ManufacturerName">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="AssemblyTypeName" HeaderText="Assembly" SortExpression="AssemblyTypeName">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:ButtonField CommandName="ModelMonitorServiceClick" DataTextField="ModelMonitorServiceCount"
															HeaderText="Service Capability">
															<ItemStyle HorizontalAlign="Center" />
														</asp:ButtonField>
														<asp:ButtonField CommandName="ModelMonitorInspClick" DataTextField="ModelMonitorInspCount"
															HeaderText="Inspection Capability">
															<ItemStyle HorizontalAlign="Center" />
														</asp:ButtonField>
														<asp:ButtonField CommandName="ModelMonitorModClick" DataTextField="ModelMonitorModCount"
															HeaderText="Directive Capability">
															<ItemStyle HorizontalAlign="Center" />
														</asp:ButtonField>
														<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
															<HeaderStyle HorizontalAlign="Center" />
															<ItemStyle HorizontalAlign="Center" />
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
																						ToolTip="Click to Delete record" CausesValidation="false"
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
								<tr style="height: 0px;">
									<td style="height: 0px;">
										<asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Button ID="hdnBtnModelMonitorServiceList" ClientIDMode="Static" runat="server"
													Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
												<asp:Button ID="hdnBtnModelMonitorInspList" ClientIDMode="Static" runat="server"
													Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
												<asp:Button ID="hdnBtnModelMonitorModList" ClientIDMode="Static" runat="server" Text="..."
													CausesValidation="False" Style="display: none;"></asp:Button>
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

		</div>
		<!--Model Monitor Service List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModelMonitorServiceList" Text="Model Service Master"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlModelMonitorServiceList" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModelMonitorServiceList" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupModelMonitorServiceList" runat="server" TargetControlID="btnDummyModelMonitorServiceList"
			PopupControlID="pnlModelMonitorServiceList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameModelMonitorServiceListStateComplete() {
				$("#btnDummyModelMonitorServiceList").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}
			function OpenModelMonitorServiceListWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelMonitorServiceList").attr("src", "wfModelMonitorServiceList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyModelMonitorServiceList").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					//});
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForModelMonitorServiceList() {
				var ModelMonitorServiceListwindow = $find("<%=mdlPopupModelMonitorServiceList.ClientID %>");
				//close Model Monitor Service List popup window
				ModelMonitorServiceListwindow.hide();
				//           release resources
				$("#IframeModelMonitorServiceList").attr("src", "JavaScript:''");
				//call Model Monitor Service List image button
				$("#hdnBtnModelMonitorServiceList").click();
			}
		</script>
		<!-- Model Monitor Service List Popup Window End -->
		<!--Model Monitor Insp List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModelMonitorInspList" Text="Model Service Master"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlModelMonitorInspList" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModelMonitorInspList" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupModelMonitorInspList" runat="server" TargetControlID="btnDummyModelMonitorInspList"
			PopupControlID="pnlModelMonitorInspList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameModelMonitorInspListStateComplete() {
				$("#btnDummyModelMonitorInspList").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModelMonitorInspListWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelMonitorInspList").attr("src", "wfModelMonitorInspList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyModelMonitorInspList").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					//});
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForModelMonitorInspList() {
				var ModelMonitorInspListwindow = $find("<%=mdlPopupModelMonitorInspList.ClientID %>");
				//close Model Monitor Insp List popup window
				ModelMonitorInspListwindow.hide();
				//           release resources
				$("#IframeModelMonitorInspList").attr("src", "JavaScript:''");
				//call Model Monitor Insp List image button
				$("#hdnBtnModelMonitorInspList").click();
			}
		</script>
		<!-- Model Monitor Insp List Popup Window End -->
		<!--Model Monitor Mod List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModelMonitorModList" Text="Model Service Master"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlModelMonitorModList" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModelMonitorModList" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupModelMonitorModList" runat="server" TargetControlID="btnDummyModelMonitorModList"
			PopupControlID="pnlModelMonitorModList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameModelMonitorModListStateComplete() {
				$("#btnDummyModelMonitorModList").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModelMonitorModListWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelMonitorModList").attr("src", "wfModelMonitorModList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyModelMonitorModList").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					//});
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForModelMonitorModList() {
				var ModelMonitorModListwindow = $find("<%=mdlPopupModelMonitorModList.ClientID %>");
				//close Monitor Mod List popup window
				ModelMonitorModListwindow.hide();
				//           release resources
				$("#IframeModelMonitorModList").attr("src", "JavaScript:''");
				//call Monitor Mod List image button
				$("#hdnBtnModelMonitorModList").click();
			}
		</script>
		<!-- Model Monitor Mod List Popup Window End -->

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
