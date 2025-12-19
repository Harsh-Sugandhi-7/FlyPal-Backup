<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfItemStockDetailList_Ajax.aspx.vb"
    EnableEventValidation="true" Inherits="Flypal.wfItemStockDetailList_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>ItemStockDetailListItemStockDetailList</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Table1" class="clstablelistout" border="0">
        <tr>
            <td>
                <table id="Table2" class="clsTablelistin" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left" class="clsFormHeader1Newstyle">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <span id="lblTitle" class="clsFormHeader">Stock Detail</span>
                                    </td>
                                    <td align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                Text="Close" />
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
                         <fieldset class="clsFieldSet" style="border-width: 1px" id="fdsLast3Purchasesdetails"
                                                runat="server">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto">Part No. :</asp:Label>
                                        <asp:Label ID="lblPartNoInfo" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblTotalStockQty" runat="server" CssClass="clsLabelAuto">Total Stock Qty. :</asp:Label>
                                        <asp:Label ID="lblStockQty" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description :</asp:Label>
                                        <asp:Label ID="lblDescriptionInfo" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Total Bal Amount. :</asp:Label>
                                        <asp:Label ID="lblTotalBalAmt" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <fieldset class="clsFieldSet" style="border-width: 1px" id="Fieldset1"
                                                runat="server">
                                                <legend> <asp:Label ID="lblItemStockDetailList" runat="server" CssClass="clsLabelHeader">List Of</asp:Label></legend>
                            <asp:UpdatePanel runat="server" ID="upnlItemStockDetailList" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="dgItemStockDetailList" runat="server" ShowHeaderWhenEmpty="True"
                                        CssClass="clsGridNewStyle"  AutoGenerateColumns="False"
                                        AllowSorting="True" DataKeyNames="ReciptItemID,ReceiptNumber,ReceiptDate" CellPadding="5" GridLines="Horizontal">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                        <Columns>
                                            <%--0--%>
                                            <asp:BoundField DataField="Folio" HeaderText="Sr. No.">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--1--%>
                                            <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"/>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--2--%>
                                            <asp:BoundField DataField="ReceiptNumber" HeaderText="Receipt No.">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                            </asp:BoundField>
                                            <%--3--%>
                                            <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Release Note Info." HtmlEncode="false">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                            </asp:BoundField>
                                             
                                            <%--4--%>
                                            <asp:BoundField DataField="BalQty" HeaderText="Stock Qty.">
                                                <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--5--%>
                                            <asp:BoundField DataField="Unit" HeaderText="Unit">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--6--%>
                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--7--%>
                                            <asp:BoundField DataField="StoreLocation" HeaderText="Store </br> Location" HtmlEncode="false">
                                                <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                            </asp:BoundField>
                                            
                                            <%--8--%>
                                            <asp:BoundField DataField="BatchNo" HeaderText="Batch No.">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                 <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--9--%>
                                            <asp:BoundField DataField="ExpiryDate" HeaderText="Exp. Date">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--10--%>
                                            <asp:BoundField DataField="BalAmountForGrid" HeaderText="Bal. Amount">
                                                <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"/>
                                            </asp:BoundField>
                                            <%--11--%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                        CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                        Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--12--%>
                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--13--%>
                                            <asp:BoundField DataField="ReciptItemID" HeaderText="ReciptItemID" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                background-color: #000000; top: 0; z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                z-index: 100000;">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div class="clsLoad_ajax">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                            Height="48px" Width="48px" />
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <%-- Ajay 28-Des-2022--%>
    <!--ReceiptCumInvoiceAttach Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
        PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
    </asp:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameAttachStateComplete() {
            $("#btnDummyAttach").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenAttachWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyAttach").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForAttach() {
            var Attachwindow = $find("<%=mdlAttach.ClientID %>");
            //close popup window
            Attachwindow.hide();
            //release resources
            $("#IframeAttach").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnAttach").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
