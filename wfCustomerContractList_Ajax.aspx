<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCustomerContractList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfCustomerContractList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contract</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
  
    <script language="javascript" type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }


    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    <script  type="text/javascript"></script>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS"></script>

        <script>
            document.addEventListener('DOMContentLoaded', function () {
                var txtNo = document.getElementById('txtNo');

                txtNo.addEventListener('paste', function (event) {
                    // Get the clipboard data
                    var clipboardData = event.clipboardData || window.clipboardData;
                    var pastedData = clipboardData.getData('Text');

                    // Check if the pasted data is numeric
                    if (!/^\d+$/.test(pastedData)) {
                        // If not numeric, prevent the paste
                        event.preventDefault();
                    }
                });
            });
    </script>


</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnltitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <span id="lbltitle" runat="server" class="clsFormHeader">Contract List</span>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td colspan="2" align="right">
                                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Contract"
                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Contract "
                                                                        Visible="false" Text="Print" CausesValidation="True"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnExcelTop" runat="server" CssClass="clsbtnH clsinfoH" ValidationGroup="1"
                                                                        Visible="false" Width="100px" Text="Export To Excel" ToolTip="Click to Export" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Contract screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <script type="text/javascript">
                                        function showTextField() {


                                            var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                            var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                            var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                            var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");

                                            var DateIndex = $get("cmbDate").selectedIndex;
                                            if (DateIndex == 0) {
                                                txtFromDateobj.style.display = 'none';
                                                txtToDateobj.style.display = 'none';
                                                lblFromDateobj.style.display = 'none';
                                                lblToDateobj.style.display = 'none';

                                            }

                                        }
    </script>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblContractNo" runat="server" CssClass="clsLabel">Contract No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlContract" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:DropDownList ID="cmbContract" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Text"
                                                                                            DataValueField="Text">
                                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlContractNo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="7"
                                                                                             ToolTip="Enter Number">0</asp:TextBox>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDate" runat="server" CssClass="clsLabel" onchange="Disablecontrols();"
                                                                        Text="Date" Visible="false" />
                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabel">Date Range</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="true">
                                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                            <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFromDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                                                          ValidationGroup="a"  CssClass="clsTextBoxTagSearchDate"  onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                                             Width="100px"></asp:TextBox>
                                                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                                        </cc2:CalendarExtender>
                                                                                        <cc2:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                            TargetControlID="txtFromDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                                        </cc2:TextBoxWatermarkExtender>
                                                                                        <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None" ValidationGroup="a"
                                                                                            ClientValidationFunction="BetweenDatesValidation"  ErrorMessage="From Date should not be grater than To Date "></asp:CustomValidator>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        &nbsp;&nbsp;
                                                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" DESIGNTIMEDRAGDROP="19">To Date </asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtToDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                                                            CssClass="clsTextBoxTagSearchDate"  onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                                            ValidationGroup="a" Width="100px"></asp:TextBox>
                                                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                                        </cc2:CalendarExtender>
                                                                                        <cc2:TextBoxWatermarkExtender ID="ToDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                            TargetControlID="txtToDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                                        </cc2:TextBoxWatermarkExtender>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table3" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                                ValidationGroup="a" Text="Find Now" ToolTip="Click to Find records" />--%>

                                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
                                                                              ValidationGroup="a"  CssClass="clsSearch2btn" ToolTip="Click to Find list of Contract's as per searching criteria" />

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
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" Style="border: none; width: 100%"
                                        CssClass="clsCollapsePnl">
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
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Panel ID="pnlAdvancedSearch" runat="server" Style="max-height: 200px; overflow-y: auto;
                                        overflow: auto; overflow-x: hidden;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStatus" class="clsLabel">Status</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                        DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <span id="Span4" class="clsLabelAuto">Customer</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCustomer" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                DataTextField="Name">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Contract as per criteria :  Record(s) found.</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td colspan="2" align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Contract"
                                                                                Text="Add New" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Contract "
                                                                                Visible="false" Text="Print" CausesValidation="True"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnExcelTop" runat="server" CssClass="clsbtnH clsinfoH" ValidationGroup="1"
                                                                                Visible="false" Width="100px" Text="Export To Excel" ToolTip="Click to Export" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Contract screen"
                                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="dgContractList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                     DataKeyNames="ID" EnableViewState="True" ShowHeaderWhenEmpty="True"
                                                                    CssClass="clsGridNewStyle" GridLines="Horizontal"  CellPadding="5" PageSize="25" AllowPaging="true">
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                        <asp:BoundField DataField="ContractDateFormatted" HeaderText="Date">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ContractNumber" HeaderText="Contract No." SortExpression="ContractTextNo">
                                                                            <HeaderStyle Wrap="True" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                            <ItemStyle Wrap="True" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="RegNo" SortExpression="RegNo">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                            <ItemStyle Wrap="True" />
                                                                        </asp:BoundField>
                                                                        <%--  <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>--%>
                                                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="True" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                            <ItemStyle Wrap="True" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ToDateFormatted" HeaderText="To Date">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="True" />
                                                                        </asp:BoundField>
                                                                        <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                                    Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>--%>


                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <%-- <span id="button">Login</span>--%>
                                                                                <div class="dropdown">
                                                                                    <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CausesValidation="false"
                                                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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

                                                                    </Columns>
                                                                </asp:GridView>
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
                                                                            <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Contract"
                                                                              Visible="false"  Text="Add New" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the list of Contract"
                                                                                Visible="false" Text="Print" CausesValidation="True"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnExcel" runat="server" CssClass="clsButton_Ajax" ValidationGroup="1"
                                                                                Visible="false" Width="100px" Text="Export To Excel" ToolTip="Click to Export" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Contract  screen"
                                                                             Visible="false"   Text="Close" CausesValidation="False"></asp:Button>
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
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
            ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
            CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
            CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
            ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
            SuppressPostBack="false" />
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                    background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                    z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <%--Date Validations--%>
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            showTextField();
        });
         $(document).ready(function () {
            Disablecontrols();
        });
        function setattr(elem) {
            var No = $(elem).val();
            if ($(elem).val() == "") {
                $(elem).val('0');
            }
        }
        function Disablecontrols() {
            var index = $get("cmbDate").selectedIndex;
//            if ($("#chkDate").attr('checked') == 'checked') {


//                $("#cmbDate").removeAttr('disabled');

//                if (index == 6) {
//                    $("#cmbDate,#txtFromDate,#txtToDate").removeAttr('disabled');
//                }
//            }
//            else {
//                $("#cmbDate").attr('disabled', 'disabled');

//                if (index == 6) {
//                    $("#cmbDate,#txtFromDate,#txtToDate").attr('disabled', 'disabled');
//                }
//            }
//        }
        $(document).keypress(function (e) {
            if (e.which == 13) {
                $("input[id=btnFindNow]").click();
            }
        });

    </script>
    <script type="text/javascript">
        function FireOnClickButton(e) {
            if (e.keyCode == 13 || e.keyCode == 9) {
                document.getElementById("btnFindNow").click();
            }
        }
    </script>
    </form>
</body>
</html>
