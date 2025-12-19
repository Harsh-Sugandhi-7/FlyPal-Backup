<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMELList_Ajax.aspx.vb"
	Inherits="Flypal.wfMELList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>MEL List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" id="clientEventHandlersJS">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
		function openFile() {
			str = "wfExportToExcel.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table class="clstablelistin" id="tblLedgerList">
							<tr>
								<td colspan="4" class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<span id="lblMELList" class="clsFormHeader">Master Minimum Equipment List</span>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionButton" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH"
																		ToolTip='<%# IIf(AppSettings("MELSnagNomenclature") = "True", "Click to add ADD", "Click to add MEL") %>'
																		Text="Add New"></asp:Button>
																</td>

                                                                <td>
                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print"
                                                                        Text="Print"></asp:Button>
                                                                </td>
																<td>
																	<asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" runat="server" TabIndex="0"
																		Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report" />
																</td>

                                                                <td>
																	<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
																		Text="Close"></asp:Button>
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
									<table width="100%">
										<tr style="display: block; margin-top: 10px;">
											<td>
												<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="Table2">
															<tr>
																<td>
																	<span id="lblModel" class="clsLabelAuto">Model </span>
																</td>
																<td>
																	<asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBoxTagSearch" onchange="SetModelIdonChange(this,'txtModel_Autocomplete')"
																		ToolTip="Enter Model." AutoPostBack="True"></asp:TextBox>
																	<cc2:AutoCompleteExtender ID="txtModel_Autocomplete" runat="server" DelimiterCharacters=""
																		Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
																		ServicePath="wfMELList_Ajax.aspx" ServiceMethod="GetModelList" TargetControlID="txtModel"
																		UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
																		CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetModelID">
																	</cc2:AutoCompleteExtender>
																</td>
																<td>
																	<span id="lblDescription" class="clsLabelAuto">Description </span>
																</td>
																<td>
																	<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Description."
																		AutoPostBack="True"></asp:TextBox>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
															ToolTip="Click to search as per Criteria." Visible="False"
															CausesValidation="false" class="clsSearch2btn" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td valign="top" colspan="2">
									<asp:UpdatePanel runat="server" ID="clpnlAdvancedSearch" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Panel ID="pnlCollapsible" runat="server" CssClass="clsCollapsePnl">
												<div>
													<div id="divCollapsiblePnl">
														<table width="100%">
															<tr>
																<td>
																	<span id="lblMastersSelection" class="clsLabelHeader">Advance Search
																	</span>
																</td>
																<td align="right">
																	<div id="divCollapsiblePnlImg">
																		<image id="imgMasters" src="images/collapse_blue.jpg"
																			alternatetext="(Show Details...)" />
																	</div>
																</td>
															</tr>
														</table>
													</div>
												</div>
											</asp:Panel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td valign="top">
									<asp:Panel ID="pnlAdvancedSearch" runat="server" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
										<table>
											<tr>
												<td>
													<span id="lblATA" class="clsLabelAuto">ATA</span>
												</td>
												<td>
													<asp:UpdatePanel ID="upnlATA" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																DataTextField="ATAChapter" AutoPostBack="True">
															</asp:DropDownList>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td>
													<span id="lblSubATA" class="clsLabelAuto">Sub ATA</span>
												</td>
												<td>
													<asp:UpdatePanel ID="upnlSubATA" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:DropDownList ID="cmbSubATAList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																AutoPostBack="true" DataTextField="SubATAChapter">
															</asp:DropDownList>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td>
													<span id="lblRectificationInterval" class="clsLabelAuto">Rectification Interval</span>
												</td>
												<td>
													<asp:UpdatePanel ID="upnlRectificationInterval" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:DropDownList ID="cmbMELCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																DataValueField="ID" DataTextField="Name" AutoPostBack="true">
															</asp:DropDownList>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
											<tr>
												<td>
													<span id="lblItemSequenceNo" class="clsLabelAuto">Item Sequence No.</span>
												</td>
												<td>
													<asp:UpdatePanel ID="upnlItemSequenceNo" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:TextBox ID="txtItemSequenceNo" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
																ToolTip="Enter Item Sequence No."></asp:TextBox>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td>
													<span id="lblRevisionNo" class="clsLabelAuto">Issue No./Rev. No.</span>
												</td>
												<td colspan="3">
													<asp:UpdatePanel ID="upnlRevisionNo" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
																ToolTip="Revision No."></asp:TextBox>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</asp:Panel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlMELList" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table1" width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgMELList" runat="server" ShowHeaderWhenEmpty="True"
															AllowPaging="true" PageSize="10" DataKeyNames="ID" AutoGenerateColumns="False"
															AllowSorting="True" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																<asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="MELDescription" SortExpression="MELDescription" HeaderText="Description">
																	<HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="true"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ATACodeSubATACode" SortExpression="ATACodeSubATACode"
																	HeaderText="ATA-SubATA">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="Item Sequence No.">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False" Font-Bold="true"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="PageNo" SortExpression="PageNo" HeaderText="Page No.">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="RevisionNo" SortExpression="RevisionNo" HeaderText="Issue No./Rev. No.">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="RevisionDateFormatted" HeaderText="Revision Date">
																	<HeaderStyle HorizontalAlign="left" Wrap="False"></HeaderStyle>
																	<ItemStyle HorizontalAlign="left" Wrap="False"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="MELCategoryName" SortExpression="MELCategoryName" HeaderText="Rectification Interval">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="MakeMELQty" HeaderText="Number Installed">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="FlyMELQty" HeaderText="No Req. to Dispatch">
																	<HeaderStyle HorizontalAlign="Right" Wrap="true"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="FrequencyInDays" HeaderText="Frequency In Days">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="FrequencyInHours" HeaderText="Frequency In Hours">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="FrequencyInCycles" HeaderText="Frequency In Cycles">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:TemplateField HeaderText="Applicable">
																	<ItemTemplate>
																		<asp:CheckBox ID="IsApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
																			Enabled="False" />
																	</ItemTemplate>
																	<ItemStyle HorizontalAlign="Center" />
																</asp:TemplateField>
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
																								ToolTip="Click to Edit record" CommandName="EditRec"
																								ImageUrl="~/images/edit.png" CausesValidation="false" />
																						</td>

																						<td>
																							<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="Click to Delete record" CommandName="DeleteRec"
																								ImageUrl="~/images/delete.png" CausesValidation="false" />
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
		<cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
			ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="clpnlAdvancedSearch"
			CollapseControlID="clpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
			CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
			ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
			SuppressPostBack="false" />


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

		<%-- Autocomplete functions to set id--%>
		<asp:HiddenField ID="hdnModelId" runat="server" ClientIDMode="Static" />
		<script type="text/javascript">
			function SetModelID(source, e) {
				//get id from autocomplete list
				var node;
				var value = e.get_value();

				if (value) node = e.get_item();
				else {
					value = e.get_item().parentNode._value;
					node = e.get_item().parentNode;
				}
				//Set id to relevent hidden field 
				var textbox;
				if (source._id == "txtModel_Autocomplete") {
					textbox = document.getElementById('hdnModelId');
				}
				textbox.value = value;
			}
			//text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
			function SetModelIdonChange(source, extenderid) {
				var popup = $find(extenderid);
				var complist = popup.get_completionList();
				var text = $(source).val().toLowerCase();
				for (var i = 0; i < complist.childNodes.length; i++) {
					var texttocompare = complist.childNodes[i].innerText.toLowerCase();
					if (text == texttocompare) {
						var val = complist.childNodes[i]._value;

						if (extenderid == "txtModel_Autocomplete") {
							textbox = document.getElementById('hdnModelId');
						}
						textbox.value = val;
						return;
					}

				}

				if (extenderid == "txtModel_Autocomplete") {
					document.getElementById('hdnModelId').value = '';
				}
			}
		</script>

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
