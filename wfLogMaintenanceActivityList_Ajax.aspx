<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogMaintenanceActivityList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfLogMaintenanceActivityList_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title></title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,tit0lebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <script src="js/query-1.7.1.js" type="text/javascript"></script>
        <script type="text/javascript" language="javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                var gridHeader = $('#<%=dgLogMaintenanceActivityList.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
                $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
                $('#<%=dgLogMaintenanceActivityList.ClientID%> tr th').each(function (i) {
                    // Here Set Width of each th from gridview to new table(clone table) th 
                    $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width() + 1).toString() + "px");
                });
                $('#GHead').css('position', 'absolute');
                $("#GHead").append(gridHeader);

                //               $('#GHead').style('width', '100%');
                $('#GHead').css('top', $('#<%=dgLogMaintenanceActivityList.ClientID%>').offset().top);

            });
        </script>
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
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="tblLedgerList" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle" colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblLedgerList" runat="server" CssClass="clsFormHeader">Log Maintenance Activity List</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnExportTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Export to Excel"
                                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" Width="120px" ToolTip="Click to Export report"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"></asp:Button>

                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Log Maintenance Activity List screen"
                                                                            Text="Close"></asp:Button>

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
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAircraft" runat="server" Width="80px" CssClass="clsLabel">Aircraft </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                AutoPostBack="True" Width="190px" DataTextField="RegNo" DataValueField="ID">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Assembly" runat="server" CssClass="clsLabel" Width="60px">Assembly</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                DataTextField="ModelSerialNoPostion" Width="180px" DataValueField="AssemblyStatusID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" >From Date</asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');" autocomplete="off"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                        <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                            ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                                    </td>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" >To Date</asp:Label>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');" autocomplete="off"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="bottom" align="right">
                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:CheckBox ID="chkShowAll" runat="server" CssClass="clsLabel" ToolTip='Check to see "ALL" records'
                                                                Text="ALL Records" Height="18px" Width="138px"></asp:CheckBox>
                                                        </td>
                                                        <td align="right">
                                                            <%--asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Log Maintenance Activity List as per searching criteria"
                                                            Text="Find Now" CausesValidation="False"></asp:Button>--%>
                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                CausesValidation="False" ToolTip="Click to find Log Maintenance Activity List as per searching criteria" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">As per criteria:  Record(s) found.</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%--<td align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnExportTop" runat="server" CssClass="clsButton_Ajax" Text="Export to Excel"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                Width="100px" ToolTip="Click to Export report"></asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" Text="Print">
                                                            </asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Log Maintenance Activity List screen"
                                                                Text="Close"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td --%>
                                    <td align="right">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%-- <asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"></asp:TextBox>--%>
                                                <input type="text" id="fname" runat="server" class="clsTextBoxTagSearch" placeholder="Search here"
                                                    onkeyup="myFunction();" style="display: none;" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="GHead" style="overflow: auto; z-index: 5; position: relative;">
                                                </div>
                                                <div style="height: 300px; overflow: auto; width: 100%">
                                                    <asp:GridView ID="dgLogMaintenanceActivityList" runat="server" CssClass="clsGridNewStyle"
                                                        DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                        AllowPaging="false" AutoGenerateColumns="False" PageSize="25" GridLines="Horizontal"
                                                        CellPadding="5">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="LogID" HeaderText="LogID"></asp:BoundField>
                                                            <asp:BoundField DataField="LogDate" HeaderText="Date">
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LogTextNo" SortExpression="LogTextNo" HeaderText="Log No.">
                                                                <HeaderStyle></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LogPageNoFormatted" SortExpression="LogPageNoFormatted"
                                                                HeaderText="Log Page No.">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LogTypeName" SortExpression="LogTypeName" HeaderText="Log Type">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MaintenanceActivity" HeaderText="Activity">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NRCWONO" SortExpression="NRCWONO" HeaderText="NRC/WO No.">
                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EmployeeName" SortExpression="EmployeeName" HeaderText="Done By">
                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Place" SortExpression="Place" HeaderText="Place">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ClosedDate" HeaderText="Closed Date">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Change" HeaderText="Change" CommandName="EditRec"></asp:ButtonField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="ViewRec"
                                                                        Style="height: 20px; width: 20px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("ImageSize") > 0 And Eval("LogTypeID") <> "3" %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ImageSize" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <asp:BoundField DataField="LogTypeID" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <%--<tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="clstableButton" align="right">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnExport" runat="server" CssClass="clsButton_Ajax" Text="Export to Excel"
                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" Width="100px" ToolTip="Click to Export report">
                                                        </asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Log Maintenance Activity List screen"
                                                            Text="Close"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                                <!--Dummy panel to open modelpopup-->
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnBtnLogMaintenanceActivity" ClientIDMode="Static" runat="server"
                                                    Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--End -->
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
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
        <!-- Log Maintenance Activity Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyLogMaintenanceActivity" Text="Log Fuel Oil"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlLogMaintenanceActivity" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeLogMaintenanceActivity" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupLogMaintenanceActivity" runat="server" TargetControlID="btnDummyLogMaintenanceActivity"
            PopupControlID="pnlLogMaintenanceActivity" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameLogMaintenanceActivityStateComplete() {
                $("#btnDummyLogMaintenanceActivity").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLogMaintenanceActivityWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeLogMaintenanceActivity").attr("src", "wfLogMaintenanceActivity_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyLogMaintenanceActivity").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForLogMaintenanceActivity() {
                var LogMaintenanceActivitywindow = $find("<%=mdlPopupLogMaintenanceActivity.ClientID %>");
                //close Log Fuel Oil popup window
                LogMaintenanceActivitywindow.hide();
                //           release resources
                $("#IframeLogMaintenanceActivity").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnLogMaintenanceActivity").click();
            }
        </script>
        <!-- End-->
        <!-- End-->
        <!--Ajay s -->
        <script type="text/javascript" language="javascript">
            function myFunction() {

                $("#<%=dgLogMaintenanceActivityList.ClientID%> tr:has(td)").hide(); // Hide all the rows.;
                var iCounter = 0;
                var sSearchTerm = $('#<%=fname.ClientID%>').val(); //Get the search box value

                if (sSearchTerm.length == 0) //if nothing is entered then show all the rows.
                {
                    $("#<%=dgLogMaintenanceActivityList.ClientID%> tr:has(td)").show();
                    return false;
                }
                //Iterate through all the td.
                $("#<%=dgLogMaintenanceActivityList.ClientID%> tr:has(td)").children().each(function () {
                    var cellText = $(this).text().toLowerCase();
                    if (cellText.indexOf(sSearchTerm.toLowerCase()) >= 0) //Check if data matches
                    {
                        $(this).parent().show();
                        iCounter++;
                        return true;
                    }
                });

                e.preventDefault();

            }

        </script>
        <!--Ajay E -->
    </form>
</body>
</html>
