<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionPartListForPurchaseOrder_Ajax.aspx.vb"
    Inherits="Flypal.wfRequisitionPartListForPurchaseOrder_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Requisition Part List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    <form id="Form1" method="post" runat="server">
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
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblPartStockStatusList" class="clsFormHeader">Requisition Part List</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                    Text="Back"></asp:Button>
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
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblDate" class="clsLabelAuto">Order Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                        onchange="ValidateDateText(this,'Date_watermarkextender');" AutoPostBack="true"></asp:TextBox>
                                                    <cc2:CalendarExtender runat="server" ID="txtDateCalendarExtender" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                    </cc2:CalendarExtender>
                                                </td>
                                                <td>
                                                    <span id="Span3" class="clsLabel">Requisition</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="cmbRequisition" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearchComboSmall">
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="65">Engineering</asp:ListItem>
                                                        <asp:ListItem Value="71">Stores</asp:ListItem>
                                                        <asp:ListItem Value="72">WorkShop</asp:ListItem>
                                                        <asp:ListItem Value="77">Planning</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span2" class="clsLabel">Requisition Type</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbType" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="true">
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Part Request</asp:ListItem>
                                                        <asp:ListItem Value="2">Part Purchase</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <span id="lblSearch" class="clsLabel">Part No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Search Criteria"
                                                        AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" Text="Find Now"
                                                                    Style="display: none" ToolTip="Click to Find" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span1" class="clsLabel">Requisition No.</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbRequisitionText" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboSmall"
                                                        DataTextField="Text" DataValueField="Text">
                                                    </asp:DropDownList>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="8"
                                                        Visible="False" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Requisition Part List : Record(s) Found.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:GridView ID="dgPartStockStatusList" runat="server" AllowPaging="True" AllowSorting="True"
                                                        AutoGenerateColumns="False" CssClass="clsGridNewStyle" DataKeyNames="ItemId"
                                                        EnableViewState="false" PageSize="25" ShowHeaderWhenEmpty="true" GridLines="Horizontal"
                                                        CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ItemId" HeaderText="ItemId" Visible="False" />
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ReqItemID" />
                                                            <asp:BoundField Visible="False" DataField="OrderBalQty" HeaderText="OrderBalQty" />
                                                            <asp:BoundField DataField="PartNo" HeaderText="Part No." SortExpression="PartNo">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequisitionNo" HeaderText="Req. No." SortExpression="RequisitionNo">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReqDateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OrderBalQty" HeaderText="Bal. Qty." SortExpression="OrderBalQty">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField CommandName="SelectPart" HeaderText="Select Part" Text="Select Part">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"   />
                                                                <ItemStyle ForeColor="Blue" Wrap="False" />
                                                            </asp:ButtonField>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
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
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
    </script>
</body>
</html>
