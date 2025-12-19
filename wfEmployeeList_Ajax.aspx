<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeList_Ajax.aspx.vb"
	Inherits="Flypal.wfEmployeeList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Employee List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js">
	</script>
	<script type="text/javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function openTranDetail() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="formEmployeeList" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="Table-MaxWidth">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table class="clstablelistin" id="tblLedgerList">
							<tr>
								<td colspan="2">
									<table width="100%">
										<tr>
											<td colspan="2" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<span id="lblEmployeeList" class="clsFormHeader">Employee List</span>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlAddTop" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table>
																		<tr>
																			<td>
																				<asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Employee"
																					Text="Add New"></asp:Button>
																			</td>
																			<td align="right">
																				<asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Employee List screen"
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
											<td style="width: 1%" align="center">
												<span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 17px; color: black; border: black; cursor: pointer"
													class="fa fa-star fa-spin fa-5x circle-icon"
													title="Mark As Favourites"></i></span>
											</td>

										</tr>
									</table>
								</td>



							</tr>
							<tr>
								<td colspan="2"></td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upblSearch" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table cellspacing="0" cellpadding="0">
												<tr>
													<td>
														<span id="lblSearchIn" class="clsLabel">Search</span>
													</td>
													<td>&nbsp;&nbsp;
                                                    <asp:DropDownList ID="cmbLookIn" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Emp No.</asp:ListItem>
														<asp:ListItem Value="2">Employee</asp:ListItem>
														<asp:ListItem Value="3">Designation</asp:ListItem>
														<asp:ListItem Value="4">Contractor</asp:ListItem>
														<%--Other ListItems are add from code behind--%>
													</asp:DropDownList>
													</td>
													<td>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblFor" runat="server" CssClass="clsLabelMedium" Visible="False">For</asp:Label>
													</td>
													<td>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
														MaxLength="25" ToolTip="Enter Search Criteria"></asp:TextBox>
													</td>
													<td>
														<asp:CheckBox ID="chkShownotworkingemployee" runat="server" CssClass="clsCheckBox" Text="Show not working employee"></asp:CheckBox>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td align="right">
									<table>
										<tr>
											<td align="right">
												<%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find the list of Employee as per searching criteria"
                                                Text="Find Now"></asp:Button>--%>

												<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
													ToolTip="Click to find the list of Employee as per searching criteria" />
											</td>

										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<span id="lblInfo" class="clsLabelAuto" style="display: none">Select Employee from the
                                    list. Click On Edit Link To Modify The Selected Employee. Click On Delete link To
                                    Delete The Selected Employee. Click On Add New button To Add A New Employee.</span>
								</td>

							</tr>
							<tr>
								<td colspan="2"></td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlGridTitle" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<td>
													<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Employee as per criteria :   Record(s) found.</asp:Label>
												</td>
											</table>
										</ContentTemplate>
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
										</Triggers>
									</asp:UpdatePanel>
								</td>
								<td align="right">
									<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="Label2" runat="server" Text="Show Entries"></asp:Label>
											<asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall" ID="cmbShowE" runat="server" Width="55px"
												AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
												<asp:ListItem Value="0">5</asp:ListItem>
												<asp:ListItem Value="1">10</asp:ListItem>
												<asp:ListItem Value="2">15</asp:ListItem>
												<asp:ListItem Value="3">20</asp:ListItem>
												<asp:ListItem Value="4" Selected="True">25</asp:ListItem>
												<asp:ListItem Value="5">30</asp:ListItem>
												<asp:ListItem Value="6">40</asp:ListItem>
												<asp:ListItem Value="7">45</asp:ListItem>
												<asp:ListItem Value="8">50</asp:ListItem>
												<asp:ListItem Value="9">55</asp:ListItem>
											</asp:DropDownList>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:GridView ID="dgEmployeeList" runat="server" AutoGenerateColumns="False"
												DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowPaging="True"
												PageSize="25" AllowSorting="True"
												CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
												<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
												<RowStyle CssClass="clsdgItem"></RowStyle>
												<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
												<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
												<PagerStyle HorizontalAlign="Right" CssClass="paging" />
												<Columns>
													<%--0--%>
													<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
													<%--1--%>
													<asp:BoundField DataField="EmpNo" SortExpression="EmpNo" HeaderText="Emp No.">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													</asp:BoundField>
													<%--2--%>
													<asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Employee">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													</asp:BoundField>
													<%--3--%>
													<asp:BoundField DataField="DesignationName" SortExpression="DesignationName" HeaderText="Designation">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													</asp:BoundField>
													<%--4--%>
													<asp:BoundField DataField="GenderName" SortExpression="GenderName" HeaderText="Gender">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													</asp:BoundField>
													<%--5--%>
													<asp:BoundField DataField="DateofBirth" HeaderText="Date of Birth">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
														<FooterStyle Wrap="False"></FooterStyle>
													</asp:BoundField>
													<%--6--%>
													<asp:BoundField DataField="ContractorName" SortExpression="ContractorName" HeaderText="Contractor">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													</asp:BoundField>
													<%--7--%>
													<asp:BoundField DataField="DepartmentName" SortExpression="DepartmentName" HeaderText="Department">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													</asp:BoundField>
													<%--Added By Harsh Sugandhi on 5th July 2024 for FLYPAL-1728--%>
													<%--8--%>
													<asp:TemplateField HeaderText="Technical Crew">
														<ItemTemplate>
															<asp:CheckBox ID="IsTechnicalCrew" runat="server"
																Enabled="false" 
																Checked='<%# DataBinder.Eval(Container.DataItem, "IsTechnicalCrew") %>' />
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Center" />
														<ItemStyle HorizontalAlign="Center" />
													</asp:TemplateField>
													<%--9--%>
													<asp:TemplateField HeaderText="Flying Crew">
														<ItemTemplate>
															<asp:CheckBox ID="IsFlyingCrew" runat="server"
																Enabled="false" 
																Checked='<%# DataBinder.Eval(Container.DataItem, "IsUseInFlightLog") %>' />
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Center" />
														<ItemStyle HorizontalAlign="Center" />
													</asp:TemplateField>
													<%--10--%>
													<asp:ButtonField CommandName="DocsAddRemove" DataTextField="EmployeeDocumentCountForLink"
														HeaderText="Document">
														<ItemStyle HorizontalAlign="Center" />
													</asp:ButtonField>
													<%--11--%>
													<asp:ButtonField CommandName="TrainingAddRemove" DataTextField="EmployeeTrainingCountForLink"
														HeaderText="Training">
														<ItemStyle HorizontalAlign="Center" />
													</asp:ButtonField>
													<%--12--%>
													<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
														<ItemTemplate>
															<div class="dropdown">
																<div class="dropdownbtn-content">
																	<table id="T1" class="clsGridNew_Ajax">
																		<tr>
																			<td>
																				<asp:ImageButton ID="EditView" runat="server" 
																					CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
																					CommandName="EditRec" ImageUrl="~/images/edit.png" 
																					Style="height: 15px; width: 15px" />
																			</td>
																			<asp:PlaceHolder ID="phdelete" runat="server" Visible='<%# Not Eval("IsSyncFromCRS")%>'>
																			<td>
																				<asp:ImageButton ID="DeleteRecord" runat="server" 
																					CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
																					CommandName="DeleteRec" ImageUrl="~/images/delete.png" 
																					Style="height: 20px; width: 20px" />
																			</td>
																				</asp:PlaceHolder>
																			<asp:PlaceHolder ID="phView" runat="server" Visible='<%#  Eval("ImageSize")>0 %>'>
																			<td>
																				<asp:ImageButton ID="View" runat="server" 
																					CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
																					CommandName="View" ImageUrl="icons/CLIP01.ICO" 
																					Style="height: 20px; width: 13px" 
																					Visible='<%#  Eval("ImageSize")>0 %>' />
																			</td>
																				</asp:PlaceHolder>
																			<td>
																				<asp:ImageButton ID="printICN" class="actionICNS" runat="server"
																					CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																					ToolTip="Click to Print record"
																					CommandName="PrintRec" ImageUrl="~/images/print.png" />
																			</td>

																		</tr>
																	</table>
																</div>
																<asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
															</div>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Center" />
														<ItemStyle HorizontalAlign="Center" />
													</asp:TemplateField>
													<%--13--%>
													<asp:TemplateField HeaderText="View" Visible="false">
														<ItemTemplate>
															<asp:LinkButton ID="LinkButton1" runat="server" Text="View" CommandName="View"></asp:LinkButton>
														</ItemTemplate>
													</asp:TemplateField>
													<%--14--%>
													<asp:ButtonField Text="Print" HeaderText="Print" CommandName="PrintRec" Visible="false"></asp:ButtonField>
													<%--15--%>
													<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
														DataField="ImageSize" HeaderText="Size"></asp:BoundField>
													<%--16--%>
													<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
														DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
												</Columns>
											</asp:GridView>
										</ContentTemplate>
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
										</Triggers>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="right" colspan="2">
									<asp:UpdatePanel ID="upnlAddBottom" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table class="clstableButton" align="right">
												<tr>
													<td>
														<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Employee"
															Text="Add New" Visible="false"></asp:Button>
													</td>
													<td align="right">
														<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Employee List screen"
															Text="Close" Visible="false"></asp:Button>
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
													<td>
														<asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server"
															Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
														<asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server"
															Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr style="height: 0px;">
								<td colspan="2" style="height: 0px;">
									<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel3">
										<ContentTemplate>
											<asp:Button ID="hdnBtnAddDocDetail" ClientIDMode="Static" runat="server" Text="..."
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnBtnAddTrainingDetail" ClientIDMode="Static" runat="server" Text="..."
												CausesValidation="False" Style="display: none;"></asp:Button>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
		<%--NEW--%>
		<!-- DocDetail Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyDocDetail" Text="Dummy DocDetail" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupDocDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupDocDetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupDocDetail" runat="server" TargetControlID="btnDummyDocDetail"
			PopupControlID="pnlPopupDocDetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
            function IFrameDocDetailStateComplete() {
                $("#btnDummyDocDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenToAddDocDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupDocDetail").attr("src", "wfEmployeeDocumentList_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyDocDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
        </script>
		<script type="text/javascript">
            function ParentCallBackFunctionForDocDetail() {
                var DocDetailWindow = $find("<%=mdlPopupDocDetail.ClientID %>");
                //close DocDetail popup window
                DocDetailWindow.hide();
                $("#iPopupDocDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddDocDetail").click();
            }
        </script>
		<!-- End-->
		<!-- Training Detail Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyTrainingDetail" Text="Dummy Training Detail"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupTrainingDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupTrainingDetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupTrainingDetail" runat="server" TargetControlID="btnDummyTrainingDetail"
			PopupControlID="pnlPopupTrainingDetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
            function IFrameTrainingDetailStateComplete() {
                $("#btnDummyTrainingDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenToAddTrainingDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupTrainingDetail").attr("src", "wfEmployeeTrainingList_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyTrainingDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }
            }
        </script>
		<script type="text/javascript">
            function ParentCallBackFunctionForTrainingDetail() {
                var TrainingDetailWindow = $find("<%=mdlPopupTrainingDetail.ClientID %>");
                //close Training Detail popup window
                TrainingDetailWindow.hide();
                $("#iPopupTrainingDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddTrainingDetail").click();
            }
        </script>

		<%--Added By Sachin on 13-Feb-2024--%>

		<script type="text/javascript">
            function FunctionFav(x) {
                if (x.classList.contains("fa-star")) {
                    x.classList.remove("fa-star");
                    x.classList.add("fa-star-o");
                    x.style.color = 'black';
                    x.style.border = 'black';
                    $("#hdnBtnRemoveFav").click();
                }
                else {
                    x.classList.remove("fa-star-o");
                    x.classList.add("fa-star");
                    x.style.color = '#fff';
                    x.style.border = 'black';
                    $("#hdnBtnMarkFav").click();
                }
            }
            function MarkFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star");
                redstar.classList.remove("fa-star-o");
                redstar.style.color = '#fff';
                redstar.style.border = 'black';

            }
            function RemoveFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star-o");
                redstar.classList.remove("fa-star");
                redstar.style.border = 'black';
            }
        </script>

		<!-- End-->
	</form>
</body>
</html>
