<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateFirstPriorityStatusofItem_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateFirstPriorityStatusofItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Update First Priority Part Status</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSearch">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr class="clsFormHeader1Newstyle">
                                <td colspan="5">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <span id="lblTitle" class="clsFormHeader">Update First Priority Part Status</span>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnUpdate" runat="server" 
                                                    CssClass="clsbtnH clsinfoH" ToolTip="Click to Update First Priority Status"
                                                    ValidationGroup="b" CausesValidation="true" Text="Update">
                                                </asp:Button>
                                                <asp:Button ID="btnBack" runat="server" 
                                                    CssClass="clsbtnH clsinfoH" ToolTip="Click to Close"
                                                    CausesValidation="false" Text="Close">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                Width="100%" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                                ValidationGroup="a" Display="None" ErrorMessage="Enter whole part no. and description"
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlFindNowButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span class="clsLabelHeader">Step I. Enter Part No. and click on Find Now to get Item
                                                        & its Alternate Item(s) list</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <span id="lblPartNos" class="clsLabelAuto">Part No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxSearch_Ajax"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
                                                            CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
                                                            ValidationGroup="1" CausesValidation="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span class="clsLabelHeader">Step II. Please check First Priority Status for One of
                                                        the following Alternate Item and then click on Update button to update the status
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlItemListDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgItemsList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeaderWhenEmpty="true" DataKeyNames="AlternatePartID" AutoGenerateColumns="False" ToolTip="List of Item(s)"
                                                            PageSize="10" AllowSorting="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="LinkID" HeaderText="LinkID"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="First Priority Status" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="clsCheckBox" onclick="CheckBox_Clicked(this)" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartDescription" SortExpression="PartDescription" HeaderText="Description">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
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
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?PartsWithAlternatePartsOnly=True', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });

        });
    </script>
    <script type="text/javascript">
        function CheckBox_Clicked(chk) {
            var grid = chk.parentNode.parentNode.parentNode.parentNode;
            var chks = grid.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && chks[i] != chk) {
                    chks[i].checked = !(chk.checked);
                }
            }
        }
    </script>
</body>
</html>
