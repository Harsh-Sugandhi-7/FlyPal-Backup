<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemoveSelectedOrderFromRequisitionItem_Ajax.aspx.vb"
	Inherits="Flypal.wfRemoveSelectedOrderFromRequisitionItem_Ajax" %>

<%@ Import Namespace="FlyPal" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>To Remove Selected Order From Requisition Item</title>
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<style type="text/css">
		#td-header {
			width: 500px;
		}

		#trDropdown {
			margin-top: 10px;
			display: block;
		}
	</style>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblmain" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle" id="td-header">
									<table width="100%">
										<tr>
											<td>
												<span id="lblPendingReceiptItemListTitle" class="clsFormHeader">To Remove Selected Order From Requisition Item
												</span>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnClose" runat="server"
																		CssClass="clsbtnH clsinfoH" Text="Close"
																		CausesValidation="False"
																		ToolTip="Click to close">
																	</asp:Button>
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
							<tr id="trDropdown">
								<td>
									<asp:UpdatePanel runat="server" ID="upnlOrderNo" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblOrder" runat="server" CssClass="clsLabel">Order</asp:Label>
													</td>
													<td>
														<asp:DropDownList ID="cmbOrder" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
															DataValueField="ID" DataTextField="OrderNo" AutoPostBack="true">
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
									<br />
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:GridView ID="dgRequisitionItemList" runat="server"
												ClientIDMode="Static" PageSize="10"
												AutoGenerateColumns="False" AllowPaging="True"
												AllowSorting="True" DataKeyNames="ID,RequisitionNo"
												CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
													<asp:BoundField DataField="RequisitionNo" HeaderText="Requisition No.">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:ButtonField CommandName="Remove" HeaderText="Remove" Text="Remove">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:ButtonField>
												</Columns>
											</asp:GridView>
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
		<asp:HiddenField ID="hdnOrderId" runat="server" ClientIDMode="Static" />

		<%--Autocomplete functions to set id--%>
		<script type="text/javascript">

			function SetID(source, e) {
				//get id from autocomplete list
				var node;
				var value = e.get_value();

				if (value) node = e.get_item();
				else {
					value = e.get_item().parentNode._value;
					node = e.get_item().parentNode;
				}

				var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml; //Boolean Expression ? First Statement : Second Statement Ternary operator ?:
				source.get_element().value = text;

				//Set id to relevent hidden field 
				var textbox;
				if (source._id == "txtSearch_Autocomplete") {
					textbox = document.getElementById('hdnOrderId');
				}

				textbox.value = value.toString();
			}

			function SetPartIdonChange() {
				var popup = $find("txtSearch_Autocomplete");
				var complist = popup.get_completionList();
				var text = $("#txtOrderList").val().toLowerCase();
				for (var i = 0; i < complist.childNodes.length; i++) {
					var texttocompare = complist.childNodes[i].innerText.toLowerCase();
					if (text == texttocompare) {
						var val = complist.childNodes[i]._value;
						var textbox = document.getElementById('hdnOrderId');
						textbox.value = val.toString();
						return;
					}

				}
			}

			<%--ReleaseNote No autocomplete--%>
			function GetPartID() {
				var partid = document.getElementById('hdnOrderId').value.toString();
				if (partid) {
					return partid;
				}
				else {
					return '{00000000-0000-0000-0000-000000000000}';
				}

			}
			function SetContextKeyForRelNoteNo() {
				var autoComplete = $find('txtRelNoteNo_AutoComplete');
				var str = 'PartID=' + GetPartID();
				autoComplete.set_contextKey(str);
			}
			function SetContextKeyForSerialNo() {
				var autoComplete = $find('txtSerialNo_AutoCompleteExtender');
				var str = 'PartID=' + GetPartID();
				autoComplete.set_contextKey(str);
			}

			<%--autocomplete css functions--%>

									 //bold input value in list...
									 function ClientPopulated(source, eventArgs) {
										 $("#" + source._element.id).removeClass("ac_loading");
									 }

									 //Alternate item style
									 function ClientShowing(source, eventArgs) {
										 $.elements = $(source.get_completionList());
										 $.elements.find(".ac_results_li").each(function (i) {
											 if (i % 2 == 0) {
												 //$(this).addClass("ac_even");
											 }
											 else {
												 $(this).addClass("ac_odd");
											 }
										 });
									 }

									 //add loader to textbox
									 function ClientPopulating(source, e) {
										 $("#" + source._element.id).addClass("ac_loading");
									 }
									 //remove loader from textbox
									 function ClientHiding(source, eventArgs) {
										 $("#" + source._element.id).removeClass("ac_loading");
									 }
		</script>
		<%--End--%>
	</form>
</body>
</html>
