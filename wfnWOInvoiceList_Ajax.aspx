<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOInvoiceList_Ajax.aspx.vb"
    Inherits="Flypal.wfnWOInvoiceList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="FlyPal" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work Order Invoice</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script language="javascript" type="text/javascript">
        function openLedgerSame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>

    <style type="text/css">
        #lblNo,
        #lblFromDate {
            margin-left: 40px;
        }

        #lblFromDate,
        #lblToDate {
            display: inline-block;
        }

        #lblFromDate {
            width: 78px;
        }

        #lblToDate {
            width: 55px;
        }
    </style>
</head>
<body>
    <form id="WOInvoiceForm" runat="server">
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
                                                            <span id="lbltitle" runat="server" class="clsFormHeader">Work Order Invoice List
                                                            </span>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="2" align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNew" runat="server" Text="Add New"
                                                                            ToolTip="Click to Add New" CssClass="clsbtnH clsinfoH"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            ToolTip="Click to close List of Invoice screen"
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
                                    <%--Added by Harsh on 29th March 2024--%>
                                    <td id="tdFavICN" align="center">
                                        <span id="spFavICN">
                                            <i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
                                                class="fa fa-star fa-spin fa-5x circle-icon"  title="Mark As Favourites"> </i>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="text-danger clsValidationSummary"
                                                    ValidationGroup="a" HeaderText="Fill Up The Following Information."></asp:ValidationSummary>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblInvoiceNo" runat="server" CssClass="clsLabelAuto">Invoice No.</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel ID="upnlInvoice" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:DropDownList ID="cmbInvoice" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Text"
                                                                                                DataValueField="Text">
                                                                                                <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:UpdatePanel ID="upnlInvoicelblNo" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto">No.</asp:Label>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:UpdatePanel ID="upnlInvoiceNo" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="7" onchange="setattr(this);"
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
                                                                        <asp:CheckBox ID="chkDate" runat="server" Visible="false"
                                                                            Style="margin-top: 5px;" onchange="Disablecontrols();" Text="Date" Width="78px" />
                                                                        <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                                    </td>
                                                                    <td colspan="6">
                                                                        <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table style="margin-top: 5px;">
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
                                                                                                CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                                                ValidationGroup="a" Width="100px"></asp:TextBox>
                                                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                                            <cc2:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                                TargetControlID="txtFromDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be grater than To Date "></asp:CustomValidator>
                                                                                        </td>
                                                                                        <td align="right">&nbsp;&nbsp;
                                                                                        <asp:Label ID="lblToDate" runat="server" DESIGNTIMEDRAGDROP="19" CssClass="clsLabelAuto">
																							To Date 
                                                                                        </asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtToDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                                                                Style="margin-left: 5px;" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                                                ValidationGroup="a" Width="100px"></asp:TextBox>
                                                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                                            <cc2:TextBoxWatermarkExtender ID="ToDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                                TargetControlID="txtToDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
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
                                                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
																					ToolTip="Click to search as per Criteria."
																					ValidationGroup="a" CausesValidation="false" CssClass="clsSearch2btn" />
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
                                    <td valign="top" colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="clpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
                                                    <div>
                                                        <div id="divCollapsiblePnl">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblMastersSelection" class="clsLabelHeader">More Search
                                                                        </span>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div id="divCollapsiblePnlImg">
                                                                            <image id="imgMasters" src="images/collapse_blue.jpg"
                                                                                alternatetext="(Show Details...)" />
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
                                    <td valign="top">
                                        <asp:Panel ID="pnlAdvancedSearchContent" runat="server" Style="max-height: 200px; overflow-y: auto; margin-top: 5px; overflow: auto; overflow-x: hidden;">
                                            <table style="width: 100%; margin-left: 5px">
                                                <tr>
                                                    <td>
                                                        <span id="lblStatus" class="clsLabelAuto">Status</span>
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
                                                        <span id="Span4" class="clsLabelAuto">Work Order</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbWorkOrder" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="WOText"
                                                            DataTextField="WOText">
                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWONo" runat="server" CssClass="clsLabelAuto">No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWONo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="6" onchange="setattr(this);" ToolTip="Enter Number"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%" style="margin-top: 15px; margin-left: 5px">
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional" style="margin-bottom: 10px;">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="true">
																		List of Invoice as per criteria :  Record(s) found.
                                                                    </asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="table-responsive">
                                                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="dgInvoiceList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                    DataKeyNames="ID" EnableViewState="True" ShowHeaderWhenEmpty="True"
                                                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                                    <Columns>
                                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                        <asp:BoundField DataField="WOInvoiceDateFormatted" HeaderText="Date">
                                                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                Font-Underline="False" Wrap="False" />
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="InvoiceText" HeaderText="Invoice" SortExpression="InvoiceText">
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="WOTextNo" HeaderText="Work Order No." SortExpression="WOTextNo">
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CGrandTotal" HeaderText="Grand Total">
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                Font-Underline="False" Wrap="False" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="UserName" HeaderText="Created By" SortExpression="UserName">
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                Font-Underline="False" Wrap="False" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
                                                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                Font-Underline="False" Wrap="False" />
                                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
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
                                                                                                                    <asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
                                                                                                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                        ToolTip="Click to Edit record"
                                                                                                                        CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:ImageButton ID="deleteICN" class="largerActionICNS" runat="server"
                                                                                                                        CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                        ToolTip="Click to Delete record"
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
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <%--Added by Harsh on 29th March 2024--%>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static" runat="server"
                                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static" runat="server"
                                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
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
                ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearchContent" ExpandControlID="clpnlAdvancedSearch"
                CollapseControlID="clpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
                CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                SuppressPostBack="false" />

        </div>
        <div>
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
                if ($("#chkDate").attr('checked') == 'checked') {
                    if (index == 6) {
                        $("#cmbDate,#txtFromDate,#txtToDate").removeAttr('disabled');
                    }
                }
                else {
                    if (index == 6) {
                        $("#cmbDate,#txtFromDate,#txtToDate").attr('disabled', 'disabled');
                    }
                }
            }
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

        <%--Added by Harsh on 29th March 2024--%>
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
