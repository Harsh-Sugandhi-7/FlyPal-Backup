<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManufacturer_Ajax.aspx.vb" EnableEventValidation="false"
    Inherits="Flypal.wfManufacturer_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Manufacturer</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

</head>
<body>
    <form id="wfgroup" runat="server">
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
                            <asp:UpdatePanel ID="upnlManufacturer" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td colspan="3" class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbltitle" CssClass="clsFormHeader displayBlock" runat="server">
																Manufacturer [New]
                                                            </asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server"
                                                                            CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Manufacturer in the list"
                                                                            Text="New" CausesValidation="False"></asp:Button></td>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server"
                                                                            ToolTip="Click to save the Manufacturer Information"
                                                                            Text="Save"></asp:Button></td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH"
                                                                            runat="server" ToolTip="Click to close Manufacturer screen"
                                                                            Text="Close" CausesValidation="False"></asp:Button></td>
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
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                                    CssClass="clsLabelauto" Display="None"
                                                    ControlToValidate="txtName" ErrorMessage="Name Required">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Manufaturer Name should not be greater than 50 characters."
                                                    ClientValidationFunction="ValidateName" Display="None">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblManufacturerDetails" runat="server" CssClass="clsLabelHeader1">Manufacturer Details</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                    ToolTip="Enter Manufacturer's Name" Text="" MaxLength="50" TextMode="MultiLine"
                                                    Width="278px" Height="45px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">

                                                <asp:GridView ID="dgManufacturer" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    AllowPaging="True" PageSize="10">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="False"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Manufacturer" SortExpression="Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
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
                                                                                        CommandName="ViewRec" ImageUrl="~/images/edit.png" />
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
        <script type="text/javascript">

            function ValidateName(source, args) {
                args.IsValid = false;
                var Nametxt = document.getElementById("txtName").value;
                var len = Nametxt.length;
                if (len < 50) {
                    args.IsValid = true;
                    return;
                }
            }
        </script>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForManufacturer();
                return false;
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
     <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameManufacturerStateComplete();
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
