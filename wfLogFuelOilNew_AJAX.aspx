<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogFuelOilNew_AJAX.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfLogFuelOilNew_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title></title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
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
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
     <script src="script/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="script/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
   <%-- <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=dgLogFuelOilList.ClientID %>').Scrollable({
                ScrollHeight: 300,
                IsInUpdatePanel: true
            });
        });
    </script>--%>
     <script src="js/query-1.7.1.js" type="text/javascript"></script>
    <%--   <script type="text/javascript" language="javascript">
           Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

               var gridHeader = $('#<%=dgLogFuelOilList.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
               $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
               $('#<%=dgLogFuelOilList.ClientID%> tr th').each(function (i) {
                   // Here Set Width of each th from gridview to new table(clone table) th 
                   $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width() + 1 ).toString() + "px");
               });
               $('#GHead').css('position', 'absolute');
               $("#GHead").append(gridHeader);
             
               $('#GHead').style('width', '100%');
               $('#GHead').css('top', $('#<%=dgLogFuelOilList.ClientID%>').offset().top);

           });
        </script>--%>
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
                                <%--<td colspan="2">--%>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle" style="width: 99%;">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 99%" valign="middle">
                                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblLedgerList" runat="server" CssClass="clsFormHeader" Style="width: 100%">Fuel And Oil List</asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Print">
                                                                    </asp:Button>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Log Fuel Oil List screen"
                                                                Text="Close"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 1%" align="center">
                                                <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px;
                                                    color: black; border: black; cursor: pointer" class="fa fa-star fa-spin fa-5x circle-icon"
                                                    title="Mark As Favourites"></i>
                                                    <%--  Ajay 07-Nov-2022--%>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            &nbsp;<table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                                        &nbsp; &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                            AutoPostBack="True" DataTextField="RegNo" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp; &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel">From Date</asp:Label>
                                                        &nbsp; &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off"
                                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                            ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabel">To Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        &nbsp; &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off"
                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
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
                                                            Text="Show ALL Records" Height="18px" Width="138px"></asp:CheckBox>
                                                    </td>
                                                    <td align="right">
                                                        <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Log Fuel Oil List as per searching criteria"
                                                            Text="Find Now" CausesValidation="False"></asp:Button>--%>
                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                            CausesValidation="False" ToolTip="Click to find Log Fuel Oil List as per searching criteria" />
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
                                <td align="right">
                                    <%-- <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" Text="Print">
                                                            </asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Log Fuel Oil List screen"
                                                                Text="Close"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <%-- <asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"></asp:TextBox>--%>
                                            <input type="text" id="fname" runat="server" class="clsTextBoxTagSearch" placeholder="Search here"
                                                onkeyup="myFunction();" style ="display:none;" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="GHead" style="z-index: 5; position: absolute; width: 100%;">
                                            </div>
                                             <div style="height: 275px; overflow: auto; width: 100%">
                                            <asp:GridView ID="dgLogFuelOilList" runat="server" CssClass="clsGridNewStyle" DataKeyNames="ID"
                                                GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="true" EnableViewState="false"
                                                AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" PageSize="1000">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="Date" HeaderText="Date">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LogTextNo" HeaderText="Log No.">
                                                       <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                         <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LogPageNoFormatted" HeaderText="Log Page No.">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FromTo" HeaderText="Departure / Arrival ">
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FlyingHrsStr" HeaderText="Total Time">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BlockTimeStr" HeaderText="Block Time">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Right" Width="45px"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                        <HeaderStyle Wrap="False"  HorizontalAlign="Left" Width="25px">
                                                        </HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FuelatDept" HeaderText="Fuel on Board">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Right" Width="50px"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FuelUplifted" HeaderText="Fuel Uplift">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BurnOnGround" HeaderText="Burn On Ground">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total Fuel at Departure">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FuelatArrive" HeaderText="Fuel at Arrival">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Consumed" HeaderText="Fuel Used">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AvgFuelOilConsumption" HeaderText="Avg. Con. / Hr.">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OilUplifted" HeaderText="Oil Uplift">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TOWeight" HeaderText="T.O. Weight" Visible ="false"  >
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Altitude" HeaderText="Altitude" Visible ="false">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Change" HeaderText="Change" CommandName="EditRec"></asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                            </div> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="clstableButton" align="right">
                                                <tr>
                                                    <td align="right">
                                                        <%-- <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print">
                                                        </asp:Button>--%>
                                                    </td>
                                                    <td>
                                                        <%--<asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Log Fuel Oil List screen"
                                                            Text="Close"></asp:Button>--%>
                                                        <%--Ajay 07-Nov-2022--%>
                                                        <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                            Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnLogFuelOil" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="100" ClientIDMode="Static" DynamicLayout="false"
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
    <!-- Log Fuel Oil Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyLogFuelOil" Text="Log Fuel Oil" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlLogFuelOil" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeLogFuelOil" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupLogFuelOil" runat="server" TargetControlID="btnDummyLogFuelOil"
        PopupControlID="pnlLogFuelOil" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameLogFuelOilStateComplete() {
            $("#btnDummyLogFuelOil").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenLogFuelOilWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeLogFuelOil").attr("src", "wfLogFuelOil_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyLogFuelOil").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForLogFuelOil() {
            var LogFuelOilwindow = $find("<%=mdlPopupLogFuelOil.ClientID %>");
            //close Log Fuel Oil popup window
            LogFuelOilwindow.hide();
            //           release resources
            $("#IframeLogFuelOil").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnLogFuelOil").click();
        }
    </script>
    <!-- End-->
    <!--Ajay S 28-03-2023 -->
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
    <!--Ajay s -->
    <script type="text/javascript" language="javascript">
        function myFunction() {

            $("#<%=dgLogFuelOilList.ClientID%> tr:has(td)").hide(); // Hide all the rows.;
            var iCounter = 0;
            var sSearchTerm = $('#<%=fname.ClientID%>').val(); //Get the search box value

            if (sSearchTerm.length == 0) //if nothing is entered then show all the rows.
            {
                $("#<%=dgLogFuelOilList.ClientID%> tr:has(td)").show();
                return false;
            }
            //Iterate through all the td.
            $("#<%=dgLogFuelOilList.ClientID%> tr:has(td)").children().each(function () {
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
