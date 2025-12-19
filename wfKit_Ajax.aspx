<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfKit_Ajax.aspx.vb" EnableViewState="True"
    Inherits="Flypal.wfKit_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Kit Information</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
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
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle clsFormHeaderTD">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblInspection" CssClass="clsFormHeader" runat="server">Inspection [New]</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlSaveClose" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" CssClass="clsbtnH clsinfoH" runat="server" Text="Add New" ValidationGroup="valGroupParent"
                                                                        CausesValidation="true"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save the Kit Information"
                                                                        ValidationGroup="valGroupParent"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Enabled="False"
                                                                        Text="Print" ToolTip="Click to Print the Kit Information"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go back to the previous page"
                                                                        CausesValidation="False"></asp:Button>
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
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="valGroupParent"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ErrorMessage="Name Required" ControlToValidate="txtInspectionKit" ValidateEmptyText="true"
                                                OnServerValidate="customvalidate" ValidationGroup="valGroupParent"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlKit" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <span id="lblInspectionKitStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblInspectionKit" Class="clsLabel">Inspection Kit</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInspectionKit" runat="server" CssClass="clsTextBoxTagSearch" Width="293px"
                                                            Enabled="<%# mKit.ItemID.Equals(Guid.Empty) %>" Text="<%# mKit.KitName %>" ToolTip="Enter Inspection Kit Name"
                                                            MaxLength="50">
                                                        </asp:TextBox>
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
                                                    <td align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Kit Item List : Record(s).</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgKit" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                            DataKeyNames="ID" EnableViewState="True" ForeColor="Black" GridLines="Horizontal" PageSize="8" ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                NextPageText="" PreviousPageText="" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                                    <HeaderStyle Wrap="True" HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                    <HeaderStyle Wrap="True" HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
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

        <!-- Kit Item --ModalPopUp -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyKitItem" Text="Dummy Kit Item" />
        </div>
        <asp:Panel runat="server" ID="pnlKitItem" Style="display: none">
            <div>
                <table class="clstablelistout" id="TABLE2">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upnlKitItem" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="clstablelistin" id="TABLE3">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Inspection Item [New]</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Information"
                                                    CssClass="clsValidationSummary" ValidationGroup="valGroupPopUp"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvQty" runat="server" ErrorMessage="Quantity must be greater than zero."
                                                    ControlToValidate="txtQuantity" Display="None" ValidateEmptyText="true" OnServerValidate="customvalidate1"
                                                    CssClass="clsLabelAuto" ValidationGroup="valGroupPopUp"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvPartNo" runat="server" Display="None" ControlToValidate="cmbPartNo"
                                                    ErrorMessage="Select Part No from the List." ClientValidationFunction="ValidateItemList"
                                                    CssClass="clsLabelAuto" ValidationGroup="valGroupPopUp"></asp:CustomValidator>
                                                <script type="text/javascript">
                                                    function ValidateItemList(source, args) {
                                                        args.IsValid = false;
                                                        var dd = $get("cmbPartNo");
                                                        if (dd.selectedIndex != 0) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }
                                                </script>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="10px"></td>
                                                        <td width="50px">
                                                            <span id="lblSrNo" class="clsLabel">Sr.No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Sr. No." width="50px"
                                                                Enabled="False" BorderColor="#E0E0E0" MaxLength="10"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblInfo" class="clsLabelHeader">Enter Part Number to Search</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td width="10px"></td>
                                                        <td width="50px">
                                                            <span id="lblSearch" class="clsLabel">Search</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                ToolTip="Enter Search" Width="180px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <table id="Table4">
                                                                <tr>
                                                                    <td>
                                                                        <%-- <asp:Button ID="btnSearch" CssClass="clsbtnH clsinfoH1" runat="server" Text="Find Now"
                                                                            CausesValidation="False"></asp:Button>--%>
                                                                        <asp:ImageButton ID="btnSearchNew" runat="server" CssClass="clsSearch2btn" 
                                                                            ImageUrl="~/images/Search2.png"  />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblInfo1" class="clsLabelHeader">Select Part to Add into Kit</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="10px">
                                                            <span id="lblStarParNo" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td width="50px">
                                                            <span id="lblPartNo" class="clsLabelAuto">Part No. </span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbPartNo" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ItemWithDescription"
                                                                DataValueField="ID" EnableViewState="false" onChange="SetItemValues()" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="ItemIDValue" runat="server" ClientIDMode="Static" />
                                                            <asp:HiddenField ID="ItemNameValue" runat="server" ClientIDMode="Static" />
                                                            <script type="text/javascript">
                                                                function SetItemValues() {
                                                                    var dd = $get("cmbPartNo");
                                                                    $get('ItemIDValue').value = dd.options[dd.selectedIndex].value;
                                                                    $get('ItemNameValue').value = dd.options[dd.selectedIndex].text;
                                                                }
                                                            </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStarQty" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblQuality" class="clsLabelAuto">Quantity</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                ToolTip="Enter Quantity" MaxLength="8" >
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <table id="Table5">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAddKitItem" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to add Kit Item"
                                                                Text="Ok" ValidationGroup="valGroupPopUp"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCloseKitItem" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                                Text="Back" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
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
        <cc2:ModalPopupExtender ID="mdlPopUpKitItem" runat="server" TargetControlID="btnDummyKitItem"
            PopupControlID="pnlKitItem" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForKit();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
     <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameKitStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
    </form>
</body>
</html>
