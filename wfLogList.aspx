<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogList.aspx.vb" Inherits="Flypal.wfLogList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Flight Log List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
   <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function delete_cookie() {
            /* $.cookie('HideInfoMessagepanel', null);*/
        }
    </script>

    <script type="text/javascript">
        function openLedgerInSameWindow(FileName) {
           window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>

    <script id="clientEventHandlersJS" type="text/javascript" >

        function openReport() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>

</head>
<body>
    <form id="frmgroup" method="post" runat="server">

        <script src="js/query-1.7.1.js" type="text/javascript"></script>

        <%--Modified by Harsh Sugandhi on 5th Feb 2025 => Resolved Multiple Header Column Issue of GridView--%>
        <%--<script type="text/javascript" >

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                console.log("Page loaded. Starting the header fix process.");

                // Check if the header is already added to avoid repeating
                if ($('#GHead').children().length === 0) {
                    console.log("Header is not found. Cloning the GridView header.");

                    var gridHeader = $('#<%=gdvLogList.ClientID%>').clone(true); // Clone the GridView
                    console.log("GridView Header Cloned.");

                    $(gridHeader).find("tr:gt(0)").remove(); // Remove all rows except the first one (header row)
                    console.log("Removed all rows except the Header.");

                    $('#<%=gdvLogList.ClientID%> tr th').each(function (i) {
                        console.log("Setting width for header column " + (i + 1));
                        // Set the width of each th in the cloned header
                        $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width() + 1).toString() + "px");
                    });

                    // Append the header only if it hasn't been added before
                    console.log("Appending the header to #GHead.");
                    $("#GHead").append(gridHeader);

                    // Set CSS styles for the header
                    console.log("Setting position and top for the header.");
                    $('#GHead').css('position', 'absolute');
                    $('#GHead').css('top', $('#<%=gdvLogList.ClientID%>').offset().top);

                    console.log("Header fix process completed.");
                } else {
                    console.log("Header already exists. No need to clone again.");
                }
            });

        </script>--%>

        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="Table-MaxWidth" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" CssClass="clsPanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 99%" valign="middle">
                                                            <span id="lbltitle" class="clsFormHeader"
                                                                style="width: 100%">Flight Log List</span>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnAddNew" runat="server"
                                                                CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                                Text="Add New" ToolTip="Add new Flight Log." />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPrint" runat="server"
                                                                CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                Enabled="False" TabIndex="0" Text="Print"
                                                                ToolTip="Print Flight Log List report." />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server"
                                                                CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                TabIndex="0" Text="Close"
                                                                ToolTip="Close Flight Log List screen." />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%--Added by Harsh Sugandhi on 5th Feb 2025--%>
                                    <td id="tdFavICN" align="center">
                                        <span id="spFavICN">
                                            <i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
                                                class="fa fa-star fa-spin fa-5x circle-icon">
                                            </i>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Information." />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlpbh" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label runat="server" ID="lblPBH"
                                                                Style="font-size: 10pt; font-weight: bold;"
                                                                class="clsLabelAuto" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%" style="margin-top: -5px;">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td width="105px">
                                                                        <span id="lblAircraft" class="clsLabel">Aircraft</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAircraft" runat="server"
                                                                            CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
                                                                            DataTextField="RegNo" DataValueField="ID" />
                                                                        <asp:CustomValidator ID="cvAircraftList" runat="server"
                                                                            CssClass="clsLabelAuto" Display="None"
                                                                            ControlToValidate="cmbAircraft" ErrorMessage="Select Aircraft From The List."
                                                                            OnServerValidate="customvalidate" />
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblStartDate" class="clsLabel" style="width: 64px;">Start Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtStartDate" CssClass="clsTextBoxTagDateSearch"
                                                                            Width="100px" AutoComplete="off"
                                                                            onchange="ValidateDateText(this,'txtStartDate_watermarkextender');" />
                                                                        <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server"
                                                                            CssClass="cal_Theme1" Enabled="true"
                                                                            Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate" />
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtStartDate"
                                                                            ID="txtStartDate_watermarkextender" ClientIDMode="Static"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" />
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblEndDate" class="clsLabelAuto">End Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtEndDate" CssClass="clsTextBoxTagDateSearch"
                                                                            Width="100px" AutoComplete="off"
                                                                            onchange="ValidateDateText(this,'txtEndDate_watermarkextender');" />
                                                                        <cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server"
                                                                            CssClass="cal_Theme1" Enabled="true"
                                                                            Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEndDate" />
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtEndDate"
                                                                            ID="txtEndDate_watermarkextender" ClientIDMode="Static"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" />
                                                                    </td>
                                                                    <td width="105px">
                                                                        <asp:Label ID="lblLogPageNo" runat="server"
                                                                            CssClass="clsLabel" Text="TLP / Log Page No." />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogPageNo" runat="server"
                                                                            CssClass="clsTextBoxTagSearchSmall" 
                                                                            ToolTip="Enter Log Page No." MaxLength="9" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <table id="Table2">
                                                                <tr>
                                                                    <td>
                                                                        <asp:LinkButton ID="lnkImportFromAPI" runat="server"
                                                                            CssClass="clsLinkButton" Visible="false"
                                                                            Text="Import Log from API">ss</asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <span id="spnImportFromCRS" title="Import Log(s)" runat="server">
                                                                            <i id="iImportFromCRS" runat="server" onclick="ImportFromCRS(this)"
                                                                                style="font-size: 18px; color: white; border: black; cursor: pointer"
                                                                                class="fa fa-refresh fa-spin fa-5x circle-iconGreen">
                                                                            </i>
                                                                        </span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkShowAll" runat="server" CssClass="clsLabel"
                                                                            ToolTip='Check to see "ALL" records'
                                                                            Text="ALL Records" Visible="true" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="btnSearch" runat="server"
                                                                            ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                            ToolTip="Click to find list of Flight Logs as per searching criteria" />
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
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%" align="top" style="margin-top: -15px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1" >
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblLogType" CssClass="clsLabelAuto" runat="server">Log Type</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbLogType" runat="server"
                                                                                CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
                                                                                DataValueField="ID" AutoPostBack="true" />
                                                                        </td>
                                                                        <td align="left"></td>
                                                                        <td align="left"></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <input type="text" id="fname" runat="server" class="clsTextBoxTagSearch"
                                                                        placeholder="Search here" onkeyup="myFunction();" style="display: none;" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:UpdatePanel ID="upnlLogGrid" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div id="GHead" style="overflow: auto; z-index: 5; position: relative;">
                                                                    </div>
                                                                    <div style="height: 420px; overflow: auto;">
                                                                        <asp:GridView ID="gdvLogList" runat="server" AllowSorting="True"
                                                                            AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="clsGridNewStyle"
                                                                            PageSize="25" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                            <RowStyle CssClass="clsdgItem" />
                                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px" />
                                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                            <Columns>
                                                                                <%--0--%>
                                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                                <%--1--%>
                                                                                <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="black" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <%--2--%>
                                                                                <asp:BoundField DataField="LogTextNo" HeaderText="Log No."
                                                                                    SortExpression="LogTextNo">
                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <%--3--%>
                                                                                <asp:BoundField DataField="LogPageNoFormatted" HeaderText="Log Page No."
                                                                                    SortExpression="LogPageNo">
                                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <%--4--%>
                                                                                <asp:BoundField DataField="FlightNo" HeaderText="Flight No."
                                                                                    SortExpression="FlightNo">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <%--5--%>
                                                                                <asp:BoundField DataField="SouLocalDateTimeFormatted"
                                                                                    HeaderText="Departure (Date Time)"
                                                                                    SortExpression="SouLocalDateTimeFormatted">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <%--6--%>
                                                                                <asp:BoundField DataField="SouUniverseDateTimeFormatted"
                                                                                    HeaderText="Departure UTC (Date Time)"
                                                                                    SortExpression="SouUniverseDateTimeFormatted">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <%--7--%>
                                                                                <asp:BoundField DataField="SouPlaceName" HeaderText="From"
                                                                                    SortExpression="SouPlaceName">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <%--8--%>
                                                                                <asp:BoundField DataField="DesLocalDateTimeFormatted"
                                                                                    HeaderText="Arrival (Date Time)"
                                                                                    SortExpression="DesLocalDateTimeFormatted">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <%--9--%>
                                                                                <asp:BoundField DataField="DesUniverseDateTimeFormatted"
                                                                                    HeaderText="Arrival UTC (Date Time)"
                                                                                    SortExpression="DesUniverseDateTimeFormatted">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <%--10--%>
                                                                                <asp:BoundField DataField="DesPlaceName" HeaderText="To"
                                                                                    SortExpression="DesPlaceName">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <%--11--%>
                                                                                <asp:BoundField DataField="TimeInAir" HeaderText="Airborne Time"
                                                                                    SortExpression="TimeInAir">
                                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <%--12--%>
                                                                                <asp:BoundField DataField="AirframeTotalCyclesOrLandings"
                                                                                    HeaderText="Cycles / Landings"
                                                                                    SortExpression="AirframeTotalCyclesOrLandings">
                                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <%--13--%>
                                                                                <asp:BoundField DataField="AirframeFinalHours"
                                                                                    HeaderText="Final Hours / Cycles / Landings" HtmlEncode="False"
                                                                                    SortExpression="AirframeFinalHours">
                                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="true"  Width="100px"/>
                                                                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <%--14--%>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <div class="dropdown">
                                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                                Style="cursor: pointer;" />
                                                                                            <div class="dropdownbtn-content">
                                                                                                <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="EditView" runat="server"
                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                CommandName="EditRec" CssClass="actionICNS"
                                                                                                                ImageUrl="~/images/edit.png" ToolTip="Edit Current Log." />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server"
                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                CommandName="DeleteRec" CssClass="largerActionICNS"
                                                                                                                ImageUrl="~/images/delete.png" Enabled='<%# not Eval("IsSyncFromCRS")%>'
                                                                                                                ToolTip="Delete Current Log." />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# 
                                                                                                                CType(Container, GridViewRow).RowIndex %>'
                                                                                                                CommandName="ViewRec" CssClass="attachmentICNS"
                                                                                                                ImageUrl="icons/CLIP01.ICO"
                                                                                                                ToolTip="View Attachment added to Log."
                                                                                                                Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" ForeColor="black" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <%--15--%>
                                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"
                                                                                    HeaderStyle-CssClass="hideGridColumn"
                                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                <%--16--%> <%--As per the Discussion Hiding the column, Harsh Sugandhi--%>
                                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                    <ItemStyle CssClass="DisplayNone" Width="5px" Height="5px" />
                                                                                </asp:BoundField>
                                                                                <%--17--%>
                                                                                <asp:BoundField DataField="IsValZero" HeaderText="IsValZero" HeaderStyle-CssClass="hideGridColumn"
                                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                <%--18--%>
                                                                                <asp:BoundField DataField="IsLogEdited" HeaderText="IsLogEdited" HeaderStyle-CssClass="hideGridColumn"
                                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                <%--19--%>
                                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                    DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                                <table>
                                                                    <tr>
                                                                        <!--Dummy panel to open modelpopup-->
                                                                        <td>
                                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                                                                <ContentTemplate>
                                                                                    <asp:Button ID="hdnBtnVoidLog" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                                                        Style="display: none;"></asp:Button>
                                                                                    <asp:Button ID="hdnBtnImportLogs" ClientIDMode="Static" runat="server" Text="----"
                                                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <!--End -->
                                                                        <td colspan="2" align="right">
                                                                            <asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <%--Added by Harsh on 15th July 2024--%>
                                                                                            <td>
                                                                                                <asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static" runat="server" 
                                                                                                    Text="----" CausesValidation="False" Style="display: none;" />
                                                                                                <asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static" runat="server" Text="----"
                                                                                                    CausesValidation="False" Style="display: none;" />
                                                                                                <asp:Button ID="hdnBtnImportCRSLogs" ClientIDMode="Static" runat="server" 
                                                                                                    Text="----" CausesValidation="False" Style="display: none;" />
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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <!-- Ajax Loader -->
            <div id="divSpinner">

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

        </div>

        <div id="modalPopUps">

            <!--Import Logs Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyImportLogs" Text="Import Logs" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlImportLogs" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeImportLogs" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                    allowtransparency="true" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupImportLogs" runat="server" TargetControlID="btnDummyImportLogs"
                PopupControlID="pnlImportLogs" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameImportLogsStateComplete() {
                    $("#btnDummyImportLogs").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenImportLogsWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeImportLogs").attr("src", "wfLogListToImport_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyImportLogs").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForImportLogs() {
                    var ImportLogswindow = $find("<%=mdlPopupImportLogs.ClientID %>");
                    //close Inspection History popup window
                    ImportLogswindow.hide();
                    //           release resources
                    $("#IframeImportLogs").attr("src", "JavaScript:''");
                    //call image button
                    $("#hdnBtnImportLogs").click();
                }
            </script>
            <!-- End-->

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
                    var params = { 'Date': datevalue, 'SetDefault': 'false' };
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

            <!-- Mainteanace / VoidLog Pop Up window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyVoidLog" Text="Maintenance Activity" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlVoidLog" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeVoidLog" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                    allowtransparency="true" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupVoidLog" runat="server" TargetControlID="btnDummyVoidLog"
                PopupControlID="pnlVoidLog" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>

            <script type="text/javascript">

                function IFrameVoidLogStateComplete() {
                    $("#btnDummyVoidLog").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenLogDetailWindow() {

                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeVoidLog").attr("src", "wfLogVoidMaintenance.aspx?Type=pup");
                        $("#btnDummyVoidLog").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                        return false;

                    } catch (e) {
                        alert(e);
                    }

                }

                function ParentCallBackFunctionForVoidLog() {

                    var VoidLogwindow = $find("<%=mdlPopupVoidLog.ClientID %>");
                    VoidLogwindow.hide();
                    $("#IframeVoidLog").attr("src", "JavaScript:''");
                    $("#hdnBtnVoidLog").click();

                }

            </script>
            <!-- End-->

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
            <!-- End-->

        </div>

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
            function MarkFav() {
                var redstar = document.getElementById("<%=favICN.ClientID%>");
                redstar.classList.add("fa-star");
                redstar.classList.remove("fa-star-o");
                redstar.style.color = '#fff';
                redstar.style.border = 'black';

            }
            function RemoveFav() {
                var redstar = document.getElementById("<%=favICN.ClientID%>");
                redstar.classList.add("fa-star-o");
                redstar.classList.remove("fa-star");
                redstar.style.border = 'black';
            }
            function ImportFromCRS(x) {
                $("#hdnBtnImportCRSLogs").click();

            }
        </script>

        <script type="text/javascript" >
            function myFunction() {

                $("#<%=gdvLogList.ClientID%> tr:has(td)").hide(); // Hide all the rows.;
                var iCounter = 0;
                var sSearchTerm = $('#<%=fname.ClientID%>').val(); //Get the search box value

                if (sSearchTerm.length == 0) //if nothing is entered then show all the rows.
                {
                    $("#<%=gdvLogList.ClientID%> tr:has(td)").show();
                    return false;
                }
                //Iterate through all the td.
                $("#<%=gdvLogList.ClientID%> tr:has(td)").children().each(function () {
                    var cellText = $(this).text().toLowerCase();
                    if (cellText.indexOf(sSearchTerm.toLowerCase()) >= 0) //Check if data matches
                    {
                        $(this).parent().show();
                        iCounter++;
                        return true;
                    }
                });
                if (iCounter == 0) {
                }
                else {
                    $('#<%=lblResult.ClientID%>').text('As per criteria : ' + iCounter + ' Record(s) found.');
                }

            }

        </script>

        <%--Added by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185--%>
        <!-- Log Selection Pop-Up -->
        <div>

            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyLogSelection" Text="Log Selection" ClientIDMode="Static" />
            </div>

            <asp:Panel runat="server" ID="pnlLogSelection" HorizontalAlign="Center">
                <asp:UpdatePanel ID="upnlLogSelection" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div>
                            <table class="clstablelistout" id="Table-MaxWidth">
                                <tr>
                                    <td class="clsFormHeader1Newstyle" colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblLogSelection" runat="server"
                                                                    CssClass="clsFormHeader" Text="Log Selection" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnCloseLogSelection" runat="server"
                                                                    CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                    TabIndex="0" Text="Close"
                                                                    ToolTip="Close Log Selection screen." />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblLogSelectionDate" Text="Log Date :" CssClass="clsLabelAuto" />
                                        <asp:TextBox runat="server" ID="txtLogSelectionDate" CssClass="clsTextBoxTagDateSearch"
                                            Width="100px" AutoComplete="off" AutoPostBack="true"
                                            onchange="ValidateDateText(this,'txtLogSelectionDate_TextBoxWatermarkExtender');" />
                                        <cc2:CalendarExtender ID="txtLogSelectionDate_CalendarExtender" runat="server" 
                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" 
                                            TargetControlID="txtLogSelectionDate" />
                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtLogSelectionDate"
                                            ID="txtLogSelectionDate_TextBoxWatermarkExtender" ClientIDMode="Static"
                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" />
                                    </td>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblLogSelectionNote" 
                                            Text="NOTE : Select the Log after which you want to add the Maintenance / Void Log."
                                            CssClass="clsLabelAuto" Font-Bold="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label runat="server" ID="lblLogSelectionLogList"
                                            Text="List of Log as per the Date Selected."
                                            CssClass="clsLabelAuto" Font-Bold="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlLogSelectionGridView" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="GV_LogSelection" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" AllowPaging="true"
                                                    ShowHeaderWhenEmpty="True" CssClass="clsGridNewStyle" PageSize="5"
                                                    CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader"
                                                        Font-Bold="True" ForeColor="black" Height="50px" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" 
                                                        HorizontalAlign="Right" />
                                                    <Columns>
                                                        <%--0--%>
                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                        <%--1--%>
                                                        <asp:BoundField DataField="LogDateFormatted" HeaderText="Date">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="black" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--2--%>
                                                        <asp:BoundField DataField="LogNo" HeaderText="Log No." SortExpression="LogNo">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--3--%>
                                                        <asp:BoundField DataField="LogPageNoFormatted" HeaderText="Log Page No."
                                                            SortExpression="LogPageNo">
                                                            <HeaderStyle HorizontalAlign="Right" Wrap="false" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <%--4--%>
                                                        <asp:BoundField DataField="FlightNo" HeaderText="Flight No."
                                                            SortExpression="FlightNo">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--5--%>
                                                        <asp:BoundField DataField="DepartureTime" HeaderText="Departure (Date Time)"
                                                            SortExpression="DepartureTime">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--6--%>
                                                        <asp:BoundField DataField="DepartureUTCTime" HeaderText="Departure UTC (Date Time)"
                                                            SortExpression="DepartureUTCTime">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--7--%>
                                                        <asp:BoundField DataField="DepartureFrom" HeaderText="From" 
                                                            SortExpression="DepartureFrom">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--8--%>
                                                        <asp:BoundField DataField="ArrivalTime" HeaderText="Arrival (Date Time)"
                                                            SortExpression="ArrivalTime">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--9--%>
                                                        <asp:BoundField DataField="ArrivalUTCTime" HeaderText="Arrival UTC (Date Time)"
                                                            SortExpression="ArrivalUTCTime">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--10--%>
                                                        <asp:BoundField DataField="ArrivalTo" HeaderText="To" SortExpression="ArrivalTo">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--11--%>
                                                        <asp:BoundField DataField="TimeInAir" HeaderText="Airborne Time" 
                                                            SortExpression="TimeInAir">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <%--12--%>
                                                        <asp:BoundField HeaderText="Final Hours / Cycles / Landings"
                                                            HtmlEncode="false" DataField="FinalHrsCyclesLandings">
                                                            <HeaderStyle HorizontalAlign="Right" Wrap="true" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--13--%>
                                                        <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select"
                                                            ItemStyle-ForeColor="Blue" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" ControlStyle-Font-Bold="true" />
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

            <cc2:ModalPopupExtender ID="mdlPopupLogSelection" runat="server" TargetControlID="btnDummyLogSelection"
                PopupControlID="pnlLogSelection" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>

        </div>

        <%--Added by Harsh Sugandhi on 5th Feb 2025 => Positioning the Log Selection Pop-Up at Center --%>
        <script type="text/javascript" id="pageLayout">

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(setPopUpLayout);

            function setPopUpLayout() {

                console.log("Modal Pop Up Loaded.");
                try {

                    $("body,html").css({ 'background-color': 'transparent' });
                    var tempMargtop = $("body #Table-MaxWidth:eq(0)").outerHeight();
                    var windowheight = $(window).height();

                    if (tempMargtop >= windowheight) {
                        $("body #Table-MaxWidth:eq(0)").css({ 'margin': 'auto' });
                        console.log("Inside IF.");
                    }
                    else {
                        var margintop = (windowheight / 2) - (tempMargtop / 2);
                        $("body #Table-MaxWidth:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                        console.log("Inside ELSE.");
                    }

                } catch (e) {
                    console.log("Error Occurer" + e.errormessage);
                }

            }

        </script>

    </form>
</body>
</html>
