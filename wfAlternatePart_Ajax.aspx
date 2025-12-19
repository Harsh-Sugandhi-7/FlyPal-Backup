<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAlternatePart_Ajax.aspx.vb"
	Inherits="Flypal.wfAlternatePart_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Alternate Part</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}

		//this function takes a value (ltext) and transmits that to the left hand frame
		function tranRight(ltext) {
			parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;
		}
	</script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
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
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table id="tblLedgerList" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Alternate Part [New]</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<table>
													<tr>
														<td>
															<asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table width="100%">
																		<tr>
																			<td align="right">
																				<asp:Button ID="btnSave" runat="server"
																					CssClass="clsbtnH clsinfoH"
																					ToolTip="Click to save current record"
																					Text="Save" ValidationGroup="valGroup1"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table2">
																		<td>
																			<asp:Button ID="btnClose" runat="server"
																				CssClass="clsbtnH clsinfoH"
																				ToolTip="Click to close Alternate Part screen"
																				CausesValidation="False" Text="Close"></asp:Button>
																		</td>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
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
									<asp:UpdatePanel ID="upnlValidationSummary" runat="server">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" ValidationGroup="valGroup1" runat="server"
												CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
											<asp:RequiredFieldValidator ID="rfvName" ValidationGroup="valGroup1" runat="server"
												ErrorMessage="Click on Select Part button to Select Part" ControlToValidate="txtPartNo"
												Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvOptions" runat="server" ValidationGroup="valGroup1" ErrorMessage="Select Option  from the List."
												ControlToValidate="cmbOptions" Display="None" OnServerValidate="CustomValidate"
												CssClass="clsLabelAuto"></asp:CustomValidator>
											<asp:CustomValidator ID="cvPartNo" runat="server" ValidationGroup="valGroup1" ErrorMessage="Click on Select Part button to Select Part"
												ControlToValidate="txtPartNo" Display="None" OnServerValidate="CustomValidate"
												CssClass="clsLabelAuto"></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<span id="lblSelectPart" class="clsLabelHeader">Select Part</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlSelectPart" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td style="width: 10px;">
														<span id="lblSearch1" class="clsLabelStar">*</span>
													</td>
													<td style="width: 70px;">
														<span id="lblSearch" class="clsLabel">Search</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbLookIn" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
															AutoPostBack="True">
															<asp:ListItem Value="0">(All)</asp:ListItem>
															<asp:ListItem Value="1">Part No</asp:ListItem>
															<asp:ListItem Value="2">Description</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<asp:TextBox ID="txtSearchPart" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
															ToolTip="Enter Part No. / Description" Width="240px"></asp:TextBox>
													</td>
													<td align="right">
														<asp:Button ID="btnSelect" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Select Part"
															CausesValidation="False" Text="Select Part" ValidationGroup="valGroup1"></asp:Button>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<span id="lblSelectedPart" class="clsLabelHeader">Selected Part</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlSelectedPart" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td style="width: 10px;"></td>
													<td style="width: 70px;">
														<span id="clsPartNo" class="clsLabel">Part No.</span>
													</td>
													<td>
														<asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" Width="460px"
															ToolTip="Enter Part No." Text="<%# mItem.Name %>" ReadOnly="True" BackColor="#E0E0E0">
														</asp:TextBox>
													</td>
													<td style="width: 95px"></td>
												</tr>
												<tr>
													<td style="width: 10px;"></td>
													<td style="width: 70px;">
														<span id="lblDescription" class="clsLabel">Description</span>
													</td>
													<td>
														<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Width="460px"
															ToolTip="Enter Description" Text="<%# mItem.Description %>" ReadOnly="True" BackColor="#E0E0E0">
														</asp:TextBox>
													</td>
													<td style="width: 95px"></td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<span id="lblAlternatePart" class="clsLabelHeader">Alternate Part</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlAlternatePart" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td style="width: 10px;">
														<span id="lblOptions1" class="clsLabelStar">*</span>
													</td>
													<td style="width: 70px;">
														<span id="lblOptions" class="clsLabel">Options</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbOptions" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True">
															<asp:ListItem Value="0">(SELECT)</asp:ListItem>
															<asp:ListItem Value="1">New</asp:ListItem>
															<asp:ListItem Value="2">Existing</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<asp:Label ID="Label1" runat="server" CssClass="clsLabel" Width="50px">Part No</asp:Label>
													</td>
													<td>
														<asp:TextBox ID="txtNewPart" runat="server" CssClass="clsTextBoxTagSearch" Width="250Px"
															MaxLength="50" ToolTip="Enter New Option" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
													</td>
													<td align="right">
														<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add/Select Alternate Part"
															Text="Add/Select" ValidationGroup="valGroup1"></asp:Button>
													</td>
												</tr>
												<tr>
													<td style="width: 10px;"></td>
													<td style="width: 70px;">
														<span id="lblAltType" class="clsLabel">Part Type</span>
													</td>
													<td colspan="3">
														<asp:DropDownList ID="cmbAltType" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Enabled="False"
															DataTextField="Name" DataValueField="ID">
														</asp:DropDownList>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
                                                            Search Resulted: No.of Record Found(s).
														</asp:Label>
														&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													</td>
												</tr>
												<tr>
													<asp:GridView ID="dgAlternatePartList" runat="server" CssClass="clsGridNewStyle"
														AutoGenerateColumns="False" DataKeyNames="PartName" PageSize="3" AllowSorting="True"
														ShowHeaderWhenEmpty="true" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
														<PagerSettings FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="PartDescription" SortExpression="PartDescription" HeaderText="Description">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="AltTypeName" SortExpression="AltTypeName" HeaderText="Part Type">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
																<ItemTemplate>
																	<asp:ImageButton ID="DeleteRecord" runat="server"
																		CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Center" />
																<ItemStyle HorizontalAlign="Center" />
															</asp:TemplateField>
														</Columns>
														<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
														<SortedAscendingCellStyle BackColor="#F7F7F7" />
														<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
														<SortedDescendingCellStyle BackColor="#E5E5E5" />
														<SortedDescendingHeaderStyle BackColor="#242121" />
													</asp:GridView>
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
			<tr>
				<td colspan="4" align="right">
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

		<%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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

		<!-- Search Part List -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySearchPartList" Text="Dummy Search Part List" />
		</div>
		<asp:Panel runat="server" ID="pnlSearchPartList" Style="display: none">
			<%----%>
			<div>
				<table class="clstablelistout" id="Table1">
					<tr>
						<td>
							<asp:UpdatePanel runat="server" ID="upnlSearchPartList" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="Table3" class="clstablelistin">
										<tr>
											<td colspan="3" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<span id="lblPartList" class="clsFormHeader">Part List</span>
														</td>
														<td align="right" colspan="3">
															<table align="right" class="clstableButton">
																<tr>
																	<td>
																		<asp:Button ID="btnOk" runat="server"
																			CssClass="clsbtnH clsinfoH" Text="Ok"
																			ToolTip="Click to Add the Part In Alternate Part" />
																	</td>
																	<td>
																		<asp:Button ID="btnCloseSearchPartList" runat="server"
																			CssClass="clsbtnH clsinfoH"
																			Text="Close" ToolTip="Click to close Part List screen" />
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="3"></td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtPartNoSearchPartList" runat="server" CssClass="clsTextBoxTagSearch"
													ToolTip="Enter Part No." MaxLength="50"></asp:TextBox>
											</td>
											<td align="right">
												<table id="Table4">
													<tr>
														<td>
															<asp:ImageButton ID="btnFindNow" runat="server"
																ValidationGroup="a" ImageUrl="~/images/Search2.png"
																CssClass="clsSearch2btn" />
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="3">
												<asp:Label ID="lblResultSearchPartList" runat="server" CssClass="clsLabelHeader"> 
                                                    List of Parts : 100 Record(s) found.
												</asp:Label>
											</td>
										</tr>
										<tr>
											<td colspan="3" align="left">
												<div style="max-height: 300px; overflow-y: auto; overflow-x: hidden; width: 621px">
													<asp:GridView ID="dgPartList" runat="server" AllowPaging="True" ShowHeader="true"
														ClientIDMode="Static" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"
														CssClass="clsGridNewStyle" Style="width: 600px; overflow-x: hidden;" PageSize="15"
														CellPadding="5"
														ForeColor="Black" GridLines="Horizontal">
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle HorizontalAlign="Right" CssClass="paging" />
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
														<PagerSettings FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
															<asp:TemplateField HeaderText="Select">
																<ItemTemplate>
																	<asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>' />
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Center" />
																<ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
															</asp:TemplateField>
															<asp:BoundField DataField="Name" HeaderText="Part No" SortExpression="Name">
																<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																<ItemStyle Wrap="true" Width="37%" HorizontalAlign="Left" />
															</asp:BoundField>
															<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="true" Width="37%" HorizontalAlign="Left" />
															</asp:BoundField>
															<asp:BoundField DataField="AlternatePartPresent" HeaderText="Alternate Part Present"
																SortExpression="AlternatePartPresent">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="true" Width="18%" HorizontalAlign="Left" />
															</asp:BoundField>
															<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%" />
															</asp:ButtonField>
														</Columns>
														<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
														<SortedAscendingCellStyle BackColor="#F7F7F7" />
														<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
														<SortedDescendingCellStyle BackColor="#E5E5E5" />
														<SortedDescendingHeaderStyle BackColor="#242121" />
													</asp:GridView>
												</div>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpSearchPartListForAlternatePart" runat="server"
			TargetControlID="btnDummySearchPartList" PopupControlID="pnlSearchPartList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>

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
