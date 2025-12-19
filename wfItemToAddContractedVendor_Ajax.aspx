<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfItemToAddContractedVendor_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfItemToAddContractedVendor_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Update Min./Max. Stock Level and Re-Order Qty. Screen</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
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
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle" class="clsFormHeader">Update Contracted Vendor For Item(s)</span>
                                            </td>

                                            <td align="right">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAddClose">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnUpdateTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                                                        ToolTip="Click to Update"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                        CausesValidation="false" ToolTip="Click to close"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>

                                        </tr>
                                    </table>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" runat="server" ValidationGroup="a"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblPartNo" class="clsLabel">Part No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCategory" class="clsLabel">Category</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                        DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td valign="top" align="right">
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                    ToolTip="Click to find the list of Part as per searching criteria" ValidationGroup="a">
                                                </asp:Button>--%>

                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                    ToolTip="Click to find the Part as per searching criteria" ValidationGroup="a"/>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <%--<td align="right">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAddClose">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnUpdateTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                                            ToolTip="Click to Add New Part"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                            CausesValidation="false" ToolTip="Click to close Part List screen"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 100%">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Part as per criteria : Record(s) found.</asp:Label>
                                            </div>
                                            <div style="width: 100%">
                                                <asp:GridView ID="gdvItem" runat="server"  Width="100%" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="True" AllowPaging="False" AllowSorting="True"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" ></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CategoryName" HeaderText="Category">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Vendor">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    DataTextField="Name" DataValueField="ID" ClientIDMode="Static" DataSource="<%# mContractVendorList %>">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <!--End-->
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <table class="clstableButton" align="right">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnUpdate" runat="server" CssClass="clsButton_Ajax" Text="Update"
                                                    ToolTip="Click to Add New Part" Visible="false"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnClose" runat="server" CausesValidation="false" CssClass="clsButton_Ajax"
                                                    Text="Close" ToolTip="Click to close Part List screen" Visible="false"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
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
    </div>
    <asp:HiddenField ID="hdnVendorIDList" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnUpdate,#btnUpdateTop").live('click', function () {
                try {
                    setIDs();
                } catch (e) {
                    alert(e.Message);
                }
                return true;
            });
            function setIDs() {
                var VendorIDList = new Array();
                var VendorList = new Array();
                $('#<%=gdvItem.ClientID %>').find("[id*=cmbVendorList]").each(function () {
                    var ID = $(":selected", this).val();
                    var Text = $(":selected", this).text();
                    if (ID != "00000000-0000-0000-0000-000000000000") {
                        VendorIDList.push(ID);
                        VendorList.push(Text);
                    }
                });
                $("#hdnVendorIDList").val('');
                $("#hdnVendorIDList").val(VendorIDList);
            }
        });
    </script>
    </form>
</body>
</html>
