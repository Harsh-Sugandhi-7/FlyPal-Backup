<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSerializedPartStatus_Ajax.aspx.vb"
    Inherits="Flypal.wfrptSerializedPartStatus_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part No. Serial No. Status</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1" EnablePageMethods="true">
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
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lbltitle" class="clsFormHeader">Part No. Serial No. Status</span>
                                            </td>

                           <%--                 <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>

                                                                <td>
                                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH" Text="Display" ToolTip="Click to Display"/>
                                                                </td>

                                                                <td>
                                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                        ToolTip="Click to close Part No. Serial No. Status screen" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary2" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ErrorMessage="Serial No Required"
                                                ControlToValidate="txtSearchSerialNo" Display="None" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="lblStepI" class="clsLabelHeader">Step I. Selection of Part Number/Description</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblPartNos" class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxSearch_Ajax"
                                                            AutoPostBack="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="lblStepII" class="clsLabelHeader">Step II. Selection of Serial No.</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSerialNoStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearchSerialNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                             AutoPostBack="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <%--<td align="right" colspan="7">
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH" Text="Display" />
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSinglePartNo" runat="server" CssClass="clsLabelAuto" Visible="False">Part No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPartNo" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearch"
                                                            ReadOnly="True" Visible="False" ></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSingleDescription" runat="server" CssClass="clsLabelAuto" Visible="False">Description</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" AutoPostBack="False" BackColor="Gainsboro"
                                                            CssClass="clsTextBoxTagSearch" ReadOnly="True" Visible="False" ></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSingleSerialNo" runat="server" CssClass="clsLabelAuto" Visible="False">Serial No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" AutoPostBack="False" BackColor="Gainsboro"
                                                            CssClass="clsTextBoxTagSearch" ReadOnly="True" Visible="False" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="7">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader" Visible="False">Receipt Items</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="7">
                                                        <asp:GridView ID="dgReceipItemList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                             OnRowDataBound="dgReceipItemList_RowDataBound" PageSize="25"
                                                            ShowHeaderWhenEmpty="True" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" >
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ReceiptID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ReceiptID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReceiptItemID" HeaderText="ReceiptItemID">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartName" HeaderText="Part Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartDescription" HeaderText="Description" />
                                                                <asp:BoundField DataField="ReceiptNoDate" HeaderText="Receipt Info." />
                                                                <asp:BoundField DataField="ItemTypeName" HeaderText="Part Type" />
                                                                <asp:BoundField DataField="OrderNoDate" HeaderText="Order Info." />
                                                                <asp:BoundField DataField="ReleaseNoteNoDate" HeaderText="Release Note Info." />
                                                                <asp:BoundField DataField="ReceiptItemQty" HeaderText="Qty." />
                                                                <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." />
                                                                <asp:BoundField DataField="StoreLocation" HeaderText="Store(Location)" />
                                                                <asp:BoundField DataField="CureDateQtrYear" HeaderText="Cure Info." />
                                                                <asp:BoundField DataField="ExpiryDateQtrYear" HeaderText="Expiry Info." />
                                                                <asp:BoundField DataField="BatchNo" HeaderText="Batch No." />
                                                                <asp:ButtonField CommandName="ViewTag" HeaderText="Store Tag View" Text="View" />
                                                                <asp:TemplateField HeaderText="Document View">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Attach"
                                                                            Text="View"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Size" HeaderText="Size">
                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                    <ItemStyle CssClass="hideGridColumn" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="7" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>

                                                    <td align="right">
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1" Text="Display" ToolTip="Click to display"/>
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                            ToolTip="Click to close Part No. Serial No. Status screen" CausesValidation="False">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>

                                                                <td>
                                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1" Text="Display" ToolTip="Click to display"/>
                                                                </td>

                                                                <td>
                                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                                        ToolTip="Click to close Part No. Serial No. Status screen" CausesValidation="False"></asp:Button>

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
    </div>
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?IsSerialisedPartsList=True', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
            $("#<%=txtSearchSerialNo.ClientID %>").autocomplete('wfAutoInventoryList.aspx?Type=SerialNo&LookInType=<%=LookInType%>&PartID=<%=PartID%>', {
                width: 200,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
    <script type="text/javascript">
        function callEvent() {
            document.getElementById("<%= txtSearch.ClientID %>").fireEvent("onchange");

        }
    </script>
</body>
</html>
