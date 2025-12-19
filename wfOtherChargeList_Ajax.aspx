<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOtherChargeList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfOtherChargeList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Other Charge List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblotherchargelist" runat="server" CssClass="clsFormHeader">List of Other Charge</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                                    ToolTip="Click to Add New Other Charge" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"
                                                                    ToolTip="Click to print list of Other Charge" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                    ToolTip="Click to close List Of Charge screen" CausesValidation="False"></asp:Button>
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
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="Span8" class="clsLabel">Range</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                                <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                                <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                                <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                                <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                                <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                                <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server">From Date </asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
                                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server">To Date </asp:Label>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
                                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblPartNoSearch" class="clsLabel">Part No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPartNoSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span1" class="clsLabel">Other Charge No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbOtherChargeText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right" MaxLength="8"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblSupplier" CssClass="clsLabel" runat="server">Supplier</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSupplier" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" valign="top">
                                                    <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                ToolTip="Click to find the list of Other Charge as per searching criteria" ValidationGroup="a">
                                                            </asp:Button>--%>


                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                ToolTip="Click to find list of Other Charge as  per searching criteria" ValidationGroup="a"/>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                <div>
                                                                    <div style="float: left; vertical-align: middle; width: 100%">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <span style="vertical-align: middle; margin-left: 2px; width: 100%" id="lblMastersSelection"
                                                                                        class="clsLabelHeader">Advance Search</span>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                                        <image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="2">
                                                    <asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="btnFindNow" Style="max-height: 200px;
                                                                overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblInvoiceNo" class="clsLabel">Invoice No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbInvoiceText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                                                                MaxLength="6"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblReceiptNo" class="clsLabel">Receipt No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbReceiptText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtReceipNo" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                                                                MaxLength="6"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span3" class="clsLabel">Order No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                                DataValueField="Text" DataTextField="Text">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOrderNo" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right" MaxLength="6"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
                                                                ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
                                                                CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
                                                                CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                                                ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                                SuppressPostBack="false" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                        ToolTip="Click to Add New Other Charge" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"
                                                        ToolTip="Click to print list of Other Charge" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                        ToolTip="Click to close List Of Charge screen" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Other Charge as per criteria : Record(s) found</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgOtherChargeList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="25"
                                                        ShowHeaderWhenEmpty="true" AllowPaging="true" AutoGenerateColumns="False" AllowSorting="True">
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OtherChargeNumber" SortExpression="OtherChargeNumber"
                                                                HeaderText="Number">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BillEntryNo" SortExpression="BillEntryNo" HeaderText="Bill of Entry No.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BillEntryDateFormatted" HeaderText="Bill Entry Date">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MasterAirwayBillNo" SortExpression="MasterAirwayBillNo"
                                                                HeaderText="Master Airway Bill No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MasterAirwayBillDateFormatted" HeaderText="Master Airway Bill Date">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="HouseAirwayBillNo" SortExpression="HouseAirwayBillNo"
                                                                HeaderText="House Airway Bill No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="HouseAirwayBillDateFormatted" HeaderText="House Airway Bill Date">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
                                                            <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>
                                                           <%-- <asp:TemplateField HeaderText="Edit/View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                          <%--  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                         <%--   <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                        Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                            CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                            CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                                            Visible='<%#  Eval("IsAttachmentAdded")%>' />
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

                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <asp:Panel ID="PnlPaging" runat="server">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <table border="0" cellpadding="2" cellspacing="1" align="right">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label Text="" EnableViewState="false" runat="server" ClientIDMode="Static" ID="valuetodisplay"
                                                                                class="letterbox" />
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnfirstpage" class="first" onclick="setValue(0);" title="Move First">
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnprevpage" onclick="setValue(1);" class="prev" title="Move Previous">
                                                                            </span>
                                                                        </td>
                                                                        <td align="center">
                                                                            <div align="center">
                                                                                <asp:TextBox runat="server" Text="" ID="Slidercontrol">
                                                                                </asp:TextBox>
                                                                                <cc2:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="Slidercontrol"
                                                                                    Minimum="-100" Maximum="100" BoundControlID="txtPageDisplay" EnableHandleAnimation="true"
                                                                                    Length="300" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnnextvpage" onclick="setValue(2);" class="next" title="Move Next"></span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnlastpage" onclick="setValue(3);" class="last" title="Move Last"></span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtPageDisplay" ToolTip="Enter page no." CssClass="clsTextBoxMegaSmall_Ajax" />
                                                                        </td>
                                                                        <td>
                                                                            <span>of </span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label Text="" ID="lblpagecount" CssClass="clsLabelHeader" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <div>
                                                                                <asp:Button ID="btnGridPaging" CssClass="clsButtonPlus_Ajax" runat="server" Text="Go" />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                        ToolTip="Click to Add New Other Charge" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Print" ToolTip="Click to print list of Other Charge"
                                                        CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close List Of Charge screen"
                                                        CausesValidation="False" Visible="false"></asp:Button>
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
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#txtFromDate").val();
            var todate = $("#txtToDate").val();
            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate };
            $.ajax({
                type: "POST",
                url: "BetweenDateValidationHandler.ashx",
                cache: false,
                data: param,
                async: false,
                beforeSend: OnBeforeSnd,
                success: onSuces,
                error: onErr
            });

            function onSuces(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                if (result == "True") {
                    args.IsValid = true;
                    return;
                }

            }

            function onErr(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                source.errormessage = result;
                return;
            }
            function OnBeforeSnd() {
                $get("AjaxLoader").style.visibility = 'visible';
            }

        }

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
    <!-- Slider control events  -->
    <script type="text/javascript">
        //initialize slider control and attach events
        function pageLoad(sender, e) {
            var slider = $find('<%=SliderExtender1.ClientID %>');
            if (slider) {
                slider.add_slideStart(sliderStart);
                slider.add_slideEnd(sliderEnd);
                slider.add_valueChanged(valChanged);
            }
        }

            
    </script>
    <script type="text/javascript">
        function valChanged() {
            var showval = $('#valuetodisplay');
            var curval = $('#<%=Slidercontrol.ClientID %>');
            showval.html(curval.val());
        }
       
        
    </script>
    <script type="text/javascript">

        function sliderStart() {
            $('#valuetodisplay').css('display', 'inline-block');
        }
    </script>
    <script type="text/javascript">
        function sliderEnd() {
            $('#valuetodisplay').css('display', 'none');

        }
    </script>
    <script type="text/javascript">
        function setValue(val) {
            if (val === 0) {//first
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var minval = slider.get_Minimum();
                $('#<%=txtPageDisplay.ClientID %>').val(minval);
                $('#<%=Slidercontrol.ClientID %>').val(minval);
                slider.set_Value(minval);


            }
            else if (val === 1) {//prev
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval - 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);


            }
            else if (val === 2) {//next
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval + 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);
                //                            sliderStart();
                //                            valChanged();
                //                            sliderEnd();

            }
            else if (val === 3) {//last
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var maxval = slider.get_Maximum();
                $('#<%=txtPageDisplay.ClientID %>').val(maxval);
                $('#<%=Slidercontrol.ClientID %>').val(maxval);
                slider.set_Value(maxval);
            }
        }
    </script>
    <!-- End  -->
    </form>
</body>
</html>
