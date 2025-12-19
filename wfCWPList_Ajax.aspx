<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWPList_Ajax.aspx.vb"
    Inherits="Flypal.wfCWPList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="vs_showGrid" content="True">
    <meta http-equiv="x-ua-compatible" content="IE=9">
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
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
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <span id="lbltitle" runat="server" class="clstitle1">Component Work Package List</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
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
                                    <fieldset id="fdsSearchInfoDet" class="clsFieldSet" style="border-width: 1px">
                                        <legend id="lblSearchInfoDet" style="font-weight: bold"><b>Search Criteria</b></legend>
                                        <table width="100%">
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
                                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Width="78px">Date</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="True">
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
                                                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Width="78px">From Date</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                        CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                                        AutoPostBack="True"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                                    </cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                        WatermarkCssClass="clsDateTextBox">
                                                                                    </cc2:TextBoxWatermarkExtender>
                                                                                    <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                                        ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                                                </td>
                                                                                <td align="right">
                                                                                    &nbsp;&nbsp;
                                                                                    <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Width="78px" DESIGNTIMEDRAGDROP="19">To Date </asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                        CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                                        AutoPostBack="True"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                                    </cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                        WatermarkCssClass="clsDateTextBox">
                                                                                    </cc2:TextBoxWatermarkExtender>
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
                                            <tr class="clsCollapsePnl">
                                                <td style="width: 100%">
                                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" Style="border: none; width: 100%">
                                                        <div>
                                                            <div style="float: left; vertical-align: middle;">
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
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblCWPNo" runat="server" CssClass="clsLabel">CWP No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlCWP" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="cmbCWP" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="True"
                                                                                DataTextField="CWPText" DataValueField="CWPText">
                                                                                <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlCWPlblNo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lblNo" runat="server" CssClass="clsLabel">No.</asp:Label>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlCWPNo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="4"
                                                                                ToolTip="Enter Number" AutoPostBack="True">0</asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPart" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlPartNo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBoxDate_Ajax" ToolTip="Enter Part No"
                                                                                AutoPostBack="True"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlSerialNo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxDate_Ajax" ToolTip="Enter Serial No"
                                                                                AutoPostBack="True"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblStatus" class="clsLabel">Status</span>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="cmbCWPStatus" runat="server" AutoPostBack="True" CssClass="clsComboBox1_Ajax"
                                                                                DataTextField="Name" DataValueField="ID">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <td>
                                                                    </td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Barcode No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlBarcodeNo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtBarcodeNo" runat="server" CssClass="clsTextBox_Ajax" AutoPostBack="true"
                                                                                ToolTip="Scan Barcode in this TextBox"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblSchedule" class="clsLabel" style="visibility: hidden">Schedule Type</span>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlSchedule" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="cmbSchedule" runat="server" AutoPostBack="True" CssClass="clsComboBox1_Ajax"
                                                                                DataTextField="Name" DataValueField="ID" Visible="False">
                                                                                <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                <asp:ListItem Value="1">Scheduled</asp:ListItem>
                                                                                <asp:ListItem Value="2">Un-Scheduled</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblNote" class="clsLabelAuto">Select CWP from the list. click on Edit/View
                                        link button to modify the selected CWP. click on Delete link button to delete the
                                        selected CWP. click on View link button to view the attachment click on AddNew button
                                        to add a new CWP.</span>
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
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Component WorkPackage as per criteria :  Record(s) found.</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="1">
                                                                            <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblCWPType" runat="server" CssClass="clsLabel" Visible="false">CWP Type</asp:Label>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:DropDownList ID="cmbCWPType" runat="server" CssClass="clsComboBox_Ajax" Visible="false">
                                                                                <asp:ListItem Value="1">Scheduled</asp:ListItem>
                                                                                <asp:ListItem Value="2">Un-Scheduled</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Work Order"
                                                                                Text="Add New" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the list of Work Order "
                                                                                Text="Print" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Work Order screen"
                                                                                Text="Close" CausesValidation="False"></asp:Button>
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
                                                                <asp:GridView ID="dgCWPList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                    ShowHeaderWhenEmpty="True" CssClass="clsGrid" PageSize="25">
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                        <asp:BoundField DataField="CWPDateFormatted" HeaderText="Date">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CWPTextNo" HeaderText="CWP No." SortExpression="CWPTextNo">
                                                                            <HeaderStyle ForeColor="White" Wrap="False" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CWPStartDateFormatted" HeaderText="Start Date">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CWPEndDateFormatted" HeaderText="End Date">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="WorkShopName" HeaderText="WorkShop" SortExpression="WorkShopName">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="True" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RevStatus" HeaderText="Rev. Status" SortExpression="RevStatus">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CMMOHMUsed" HeaderText="CMM/OHM Used" SortExpression="CMMOHMUsed">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No." SortExpression="RegNo">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Position" HeaderText="Pos." SortExpression="Position">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="PartNoDetails" HeaderText="Part Info" SortExpression="PartNoDetails">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                            <ItemStyle Wrap="True" Width="100px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ScheduleName" HeaderText="Scheduled/Unscheduled" SortExpression="ScheduleName"
                                                                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="History" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                    CommandName="HistoryRec" ImageUrl="~/images/History.png" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="IsHistoryExists" HeaderStyle-CssClass="hideGridColumn"
                                                                            HeaderText="IsHistoryExists" ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle CssClass="hideGridColumn" />
                                                                            <ItemStyle CssClass="hideGridColumn" />
                                                                        </asp:BoundField>
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
                                                                            <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Work Order"
                                                                                Text="Add New" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the list of Work Order "
                                                                                Text="Print" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Work Order  screen"
                                                                                Text="Close" CausesValidation="False"></asp:Button>
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
    </script>
    </form>
</body>
</html>
