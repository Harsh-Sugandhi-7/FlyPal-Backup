<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobCompRemInstList.aspx.vb" Inherits="Flypal.wfnWOJobCompRemInstList" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Removal / Installation List</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script language="javascript" id="clientEventHandlersJS">

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
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        #theBox_3, #theBox_2 {
            display: none;
            width: 145px;
            height: auto;
        }

        a:active, a:focus {
            outline: none;
            ie-dummy: expression(this.hideFocus=true);
        }
    </style>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <table class="clstablelistout" id="tblMain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2" class="clsFormHeader1">

                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">List Of Removal / Installations from Work Order</asp:Label>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>

                                                            </td>
                                                            <td align="right">
                                                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add"  Visible ="false" 
                                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" Visible="False"
                                                                                        ToolTip="Click to Print" Text="Print" CausesValidation="False"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
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
                                                <td style="width: 1%" align="center">
                                                    <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                        class="fa fa-star fa-spin fa-5x circle-icon"
                                                        title="Mark As Favourites"></i>
                                                        <%--Ajay 28-Dec-2022--%>
                                                    </span>
                                                </td>

                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <asp:PlaceHolder id="phHideBarcode" runat="server" visible="false">
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:TextBox ID="txtBarcode" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearchBarCodeOnWOList"
                                                                ClientIDMode="Static" placeholder="Scan your Barcode Here"> </asp:TextBox>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtBarcode" ID="TextBoxWatermarkExtender1"
                                                                ClientIDMode="Static" runat="server" WatermarkText="Enter Barcode No. to search"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            <!--ALL27072020-->
                                                        </td>
                                                        <td align="right">
                                                            <asp:HyperLink ID="hylnktWODashBoard" runat="server" NavigateUrl="DashBoardWO.aspx"
                                                                ToolTip="WORK ORDER DASHBOARD" ImageUrl="icons/Pie.jpg" Target="main" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                            ValidationGroup="a"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table1" width="100%">
                                                    <tr>
                                                        <td>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel">From Date</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDateWOList"
                                                                            CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                        <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                            ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" DESIGNTIMEDRAGDROP="19">To Date </asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDateWOList"
                                                                            CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                    <td>
                                                                        <span>W.O.</span>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbWO" runat="server" DataTextField="WOText"
                                                                                        DataValueField="WOText">
                                                                                        <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblNo" runat="server" CssClass="clsLabel">No.</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmallWOList" MaxLength="6"
                                                                                        ToolTip="Enter Number"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>

                                                                </tr>

                                                            </table>
                                                        </td>
                                                        <td align="right" valign="top">
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to find list of Work Order as  per searching criteria"
                                                          CausesValidation="true" ValidationGroup="a" Text="Find Now"></asp:Button>--%>
                                                                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Removal / Installations from Work Order as  per searching criteria" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </td>
                                                    </tr>
                                                    <asp:PlaceHolder ID="phhide" runat="server" Visible="false">

                                                        <tr>
                                                            <td colspan="3" valign="top">
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
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
                                                                        <asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="btnSearch" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span>Status</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStatus" runat="server" DataTextField="Name"
                                                                                                        DataValueField="ID">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabel">Aircraft</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" DataTextField="RegNo"
                                                                                                        DataValueField="RegNo" AutoPostBack="true">
                                                                                                        <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabel">Model</asp:Label>
                                                                                                </td>
                                                                                                <td colspan="5">
                                                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbModel" runat="server" DataTextField="Name"
                                                                                                        DataValueField="Name">
                                                                                                    </asp:DropDownList>
                                                                                                </td>


                                                                                            </tr>

                                                                                            <tr>

                                                                                                <td style="width: 69px">
                                                                                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                                                </td>
                                                                                                <td colspan="5">
                                                                                                    <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="AssemblyStatusID"
                                                                                                        DataTextField="ModelSerialNo">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td style="width: 69px">
                                                                                                    <asp:Label ID="lblComponent" runat="server" CssClass="clsLabelAuto">Component</asp:Label>
                                                                                                </td>
                                                                                                <td colspan="7">
                                                                                                    <asp:DropDownList ID="cmbComponent" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="CompStatusID"
                                                                                                        DataTextField="PartSerialNo">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
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
                                                        <tr>
                                                            <td style="padding-left: 4px" colspan="2">
                                                                <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                    Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </asp:PlaceHolder>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Removal / Installations from Work Order as per criteria : Record(s) found.</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="Label2" runat="server" Text="Show Entries"></asp:Label>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall" ID="cmbShowE" runat="server" Width="55px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                                    <asp:ListItem Value="0">5</asp:ListItem>
                                                    <asp:ListItem Value="1">10</asp:ListItem>
                                                    <asp:ListItem Value="2">15</asp:ListItem>
                                                    <asp:ListItem Value="3">20</asp:ListItem>
                                                    <asp:ListItem Value="4" Selected="True">25</asp:ListItem>
                                                    <asp:ListItem Value="5">30</asp:ListItem>
                                                    <asp:ListItem Value="6">40</asp:ListItem>
                                                    <asp:ListItem Value="7">45</asp:ListItem>
                                                    <asp:ListItem Value="8">50</asp:ListItem>
                                                    <asp:ListItem Value="9">55</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>

                                    <td align="left" colspan="2">
                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgnWOJobCompRemInstList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle" DataKeyNames="ID" EnableViewState="True" ForeColor="Black" GridLines="Horizontal" PageSize="25" ShowHeaderWhenEmpty="true">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="White" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="WODateFormatted" HeaderText="Date">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WONumber" HeaderText="W.O.No.">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg.No.">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OffPartNo" HeaderText="Off Part No.">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OffDescription" HeaderText="Off Part Description">
                                                            <ItemStyle Wrap="True" Width="170px"></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" ></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OffSerialNo" HeaderText="Off Serial No.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                             <ItemStyle Wrap="True" ></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OnPartNo" HeaderText="On Part No.">
                                                            <ItemStyle Wrap="True" ></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OnDescription" HeaderText="On Part Description">
															<ItemStyle Wrap="True" Width="170px"></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Width="170px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OnSerialNo" HeaderText="On Serial No.">
                                                            <ItemStyle Wrap="True" ></ItemStyle>
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" wrap="false" ></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>

                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="RemInst" ImageUrl="~/images/maintenance.png" Style="height: 35px; width: 35px" />
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
                                                        <td><%--Ajay 27-Dec-2022--%>
                                                            <asp:Button ID="hdnBtnMarkFav" runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="----" />
                                                            <asp:Button ID="hdnBtnRemoveFav" runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="----" />
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

        </div>
        <!--WorkOrderAttach Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
            PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
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
        <!--Ajay E -->

        <%-- 'Added by Saylee on 29-May-2019--%>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyJobCompDetail" Text="Dummy JobCompDetail"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupJobCompDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupJobCompDetail" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupJobCompDetail" runat="server" TargetControlID="btnDummyJobCompDetail"
            PopupControlID="pnlPopupJobCompDetail" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameJobCompDetailStateComplete() {
                $("#btnDummyJobCompDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenRemInstDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupJobCompDetail").attr("src", "wfnWOJobComp_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyJobCompDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForJobCompDetail() {
                var JobCompDetailWindow = $find("<%=mdlPopupJobCompDetail.ClientID %>");
                //close JobCompDetail popup window
                JobCompDetailWindow.hide();
                $("#iPopupJobCompDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddJobCompDetail").click();
            }
        </script>

    </form>
    <script type="text/javascript">
        function DisableValidators() {
            var SearchIndex = '1';
            if (SearchIndex == 1) {
                var DateIndex = $get("cmbDateRange").selectedIndex;
                if (DateIndex == 6) {
                    return true;
                }
            }
            ToDo:
            {
                for (i = 0; i < Page_Validators.length; i++) {
                    if (Page_Validators[i].validationGroup == "a") {
                        ValidatorEnable(Page_Validators[i], false);
                    }
                }
                document.getElementById("<%= Validationsummary2.ClientID %>").style.display = 'none';


            }
        }
    </script>
</body>
</html>
