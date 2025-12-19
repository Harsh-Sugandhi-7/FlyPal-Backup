<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfVendorApprovalList_Ajax.aspx.vb"
    Inherits="Flypal.wfVendorApprovalList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vendor Document Approval List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <script id="clientEventHandlersJS" language="javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
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
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lblLedgerList" class="clsFormHeader">Vendor Document Approval List</span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlAddTop" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"></asp:Button>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"></asp:Button>
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
                                <td valign="top">
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblVendorSearch " class="clsLabel">Vendor</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50" AutoPostBack="true"></asp:TextBox>
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
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Vendor Document Approval as per criteria :   Record(s) found.</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblVendor" class="clsLabel">Vendor</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataTextField="Name" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgApprovalList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            PageSize="3" ShowHeaderWhenEmpty="true" ToolTip="Vendor Approval List."
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="VendorID">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Vendor">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="VendorsID" SortExpression="VendorsID" HeaderText="ID">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="ApprovalNo" HeaderText="Approval No." SortExpression="ApprovalNo">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="ToDateFormatted" HeaderText="To Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax" dir="ltr">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>

                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                                                Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="IDRenew" runat="server" ToolTip="Click to renew" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="RenewRec" Style="width: 20px" ImageUrl="images/Renew1.png" Visible='<%# Not Eval("IsOneTime")%>' />
                                                                                        </td>
                                                                                    </tr>

                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>



                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            CommandName="HistoryRec" ImageUrl="~/images/History.png" Visible='<%#  Eval("HasHistory")%>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    HeaderText="Size" ItemStyle-CssClass="hideGridColumn" />
                                                                <asp:BoundField DataField="SortNo" HeaderStyle-CssClass="hideGridColumn" HeaderText="SortNo"
                                                                    ItemStyle-CssClass="hideGridColumn" />
                                                                <asp:BoundField DataField="IsOneTime" HeaderStyle-CssClass="hideGridColumn" HeaderText="Size"
                                                                    ItemStyle-CssClass="hideGridColumn" />
                                                                <asp:BoundField DataField="VendorID" HeaderStyle-CssClass="hideGridColumn" HeaderText="VendorID"
                                                                    ItemStyle-CssClass="hideGridColumn" />
                                                                <asp:BoundField DataField="HasHistory" HeaderStyle-CssClass="hideGridColumn" HeaderText="HasHistory"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
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
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnVendorApproval" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server"
                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server"
                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
            runat="server">
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
        <!-- Vendor Approval Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyVendorApproval" Text="VendorApprova" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlVendorApproval" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeVendorApproval" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupVendorApproval" runat="server" TargetControlID="btnDummyVendorApproval"
            PopupControlID="pnlVendorApproval" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameVendorApprovalStateComplete() {
                $("#btnDummyVendorApproval").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenVendorApprovalWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeVendorApproval").attr("src", "wfVendorApproval_Ajax.aspx?Type=pup");
                    //                if (!$.browser.msie) {
                    $("#btnDummyVendorApproval").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                    //                }
                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForVendorApproval() {
                var VendorApprovalwindow = $find("<%=mdlPopupVendorApproval.ClientID %>");
                //close Vendor Approval popup window
                VendorApprovalwindow.hide();
                //release resources
                $("#IframeVendorApproval").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnVendorApproval").click();
            }
        </script>
        <!-- End-->
        <%--Approval History--%>
        <asp:Panel runat="server" ID="pnlApprovalHistory" CssClass="clspanel1">
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyApprovalHistory" Text="Last 10 Purchases" />
            </div>
            <div style="width: 100%">
                <asp:UpdatePanel runat="server" ID="upnlApprovalHistory" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="clstablelistin" id="Table3">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="dgApprovalHistoryList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                        PageSize="3" ShowHeaderWhenEmpty="true"
                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ApprovalNo" HeaderText="Approval No.">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Name">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ToDateFormatted" HeaderText="To Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <%--<asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:ButtonField>--%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                            <asp:BoundField DataField="SortNo" HeaderText="SortNo" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnApprovalHistoryClose" runat="server" CssClass="clsbtnH clsinfoH1"
                                        ToolTip="Click to go back to the previous page" Text="Back" CausesValidation="False"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <cc2:ModalPopupExtender runat="server" ID="mdeApprovalHistory" TargetControlID="btnDummyApprovalHistory"
            PopupControlID="pnlApprovalHistory" BackgroundCssClass="clsModalPopupBGForSecondPage">
        </cc2:ModalPopupExtender>
        <%--End Of Approval History--%>
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
    </form>
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddCustomer = document.getElementById("cmbVendorList");
            if (ddCustomer != null) {
                if (ddCustomer.disabled == false) {
                    var j = 0;
              <% For Each item2 In mVendorListForApprovalList%>
                <% If item2.NotInUse = "True" Then%>
                    ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                    j = j + 1;
             <% Next%>
                }
            }
        });
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>
