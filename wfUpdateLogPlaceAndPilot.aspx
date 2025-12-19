<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateLogPlaceAndPilot.aspx.vb"
    Inherits="Flypal.wfUpdateLogPlaceAndPilot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flight Log List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {

            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script id="clientEventHandlersJS" type="text/javascript" language="javascript">
        function openReport() {
            str = "frmshowreport.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body>
    <form id="frmgroup" method="post" runat="server">
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
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lbltitle" class="clsFormHeader">Flight Log List</span>
                                            </td>

                                            <td align="right">
                                                <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1" DESIGNTIMEDRAGDROP="229">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                    TabIndex="0" Text="Close" ToolTip="Click to close Flight Log List screen" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
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
                                                HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSearchCriteria" class="clsLabelHeader">Search Criteria</span>
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
                                                                <td width="105px">
                                                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                        DataTextField="RegNo" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                    <asp:CustomValidator ID="cvAircraftList" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                        ControlToValidate="cmbAircraft" ErrorMessage="Select Aircraft From The List."
                                                                        OnServerValidate="customvalidate"></asp:CustomValidator>
                                                                </td>
                                                                <td>
                                                                    <span id="lblStartDate" class="clsLabel" style="width: 64px;">Start Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtStartDate" CssClass="clsTextBoxTagSearchDate"
                                                                        onchange="ValidateDateText(this,'txtStartDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtStartDate" ID="txtStartDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblEndDate" class="clsLabelAuto">End Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtEndDate" CssClass="clsTextBoxTagSearchDate"
                                                                        onchange="ValidateDateText(this,'txtEndDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEndDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtEndDate" ID="txtEndDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowAll" runat="server" CssClass="clsLabel" Width="138px" ToolTip='Check to see "ALL" records'
                                                                        Text="Show ALL Records" Height="18px"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <%--<asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                        ToolTip="Click to find the list of Flight Log as per searching criteria" Text="Find Now">
                                                                    </asp:Button>--%>

                                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                        ToolTip="Click to find list of Flight Log as per searching criteria" />

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td width="105px">
                                                                    <asp:Label ID="lblLogPageNo" runat="server" CssClass="clsLabelAuto">TLP/Log Page No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLogPageNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Log Page No."
                                                                        MaxLength="9"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <%--<td align="right">
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1" DESIGNTIMEDRAGDROP="229">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close Flight Log List screen" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="gdvLogList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="True" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="25">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LogTextNo" HeaderText="Log No." SortExpression="LogTextNo">
                                                                    <HeaderStyle  Wrap="False" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LogPageNoFormatted" HeaderText="Log Page No." SortExpression="LogPageNo">
                                                                    <HeaderStyle  HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FlightNo" HeaderText="Flight No." SortExpression="FlightNo">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SouLocalDateTimeFormatted" HeaderText="Departure (Date Time)"
                                                                    SortExpression="SouLocalDateTimeFormatted">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SouUniverseDateTimeFormatted" HeaderText="Departure UTC (Date Time)"
                                                                    SortExpression="SouUniverseDateTimeFormatted">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SouPlaceName" HeaderText="From" SortExpression="SouPlaceName">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DesLocalDateTimeFormatted" HeaderText="Arrival (Date Time)"
                                                                    SortExpression="DesLocalDateTimeFormatted">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DesUniverseDateTimeFormatted" HeaderText="Arrival UTC (Date Time)"
                                                                    SortExpression="DesUniverseDateTimeFormatted">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DesPlaceName" HeaderText="To" SortExpression="DesPlaceName">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Pilot1Name" HeaderText="Pilot" SortExpression="Pilot1Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Pilot2Name" HeaderText="Co-Pilot" SortExpression="Pilot2Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TimeInAir" HeaderText="Airborne Time" SortExpression="TimeInAir">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AirframeTotalCyclesOrLandings" HeaderText="Cycles/Landings"
                                                                    SortExpression="AirframeTotalCyclesOrLandings">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="ChangePlace" HeaderText="Change Place" Text="Change Place"
                                                                    HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                <asp:ButtonField CommandName="ChangePilot" HeaderText="Change Pilot/CoPilot" Text="Change Pilot/CoPilot"
                                                                    HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
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
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <!--End -->
                                                                    <td>
                                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                                            TabIndex="0" Text="Close" ToolTip=" Click to close Flight Log List screen" Visible="false"/>
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
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
    <!-- Change Place -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPlace" Text="Dummy Place" />
    </div>
    <asp:Panel runat="server" ID="pnlChangePlace" Style="display: none">
        <div>
            <table class="clstablelistout" id="Table1">
                <tr>
                    <td align="right">
                        <asp:UpdatePanel runat="server" ID="upnlPlace" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="Table3">
                                    <tr>
                                        <td align="left" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="Span1" class="clsFormHeader">Change Source/Destination Place </span>
                                                    </td>
                                                    <td valign="top" align="right">
                                                <table id="Table4" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnPlaceOk" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Ok" ToolTip="Click to add new Place"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPlaceClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Close" ToolTip="Click to close Change Place screen" CausesValidation="False">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="1" runat="server"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvPlace1" runat="server" ErrorMessage="Enter correct Source name."
                                                ControlToValidate="cmbPlace1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvPlace2" runat="server" ErrorMessage="Enter correct Destination name."
                                                ControlToValidate="cmbPlace2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <table>
                                        <tr>
                                            <td>
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                    <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Departure</b></legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblCurrentPlace" class="clsLabel">Current Departure Place </span>
                                                            </td>
                                                            <td colspan="1">
                                                                <asp:TextBox ID="txtCurrentSouPlace" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Width="310px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblChangePlace1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblChangePlace" class="clsLabel">Change Departure Place</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbPlace1" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="Name" DataValueField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                    <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Arrival</b></legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="Span2" class="clsLabel">Current Arrival Place </span>
                                                            </td>
                                                            <td colspan="1">
                                                                <asp:TextBox ID="txtCurrentDesPlace" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Width="310px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblChangePlace2" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lb2lChangePlace" class="clsLabel">Change Arrival Place</span>
                                                            </td>
                                                            <td>
                                                                 <asp:DropDownList ID="cmbPlace2" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="Name" DataValueField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                            </td>
                                            <%--<td valign="top" align="right">
                                                <table id="Table4" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnPlaceOk" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                Text="Ok" ToolTip="Click to add new Place"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPlaceClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                Text="Close" ToolTip="Click to close Change Part Place screen" CausesValidation="False">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpChangePlace" runat="server" TargetControlID="btnDummyPlace"
        PopupControlID="pnlChangePlace" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End Change Place -->
    <!-- Change Pilot-->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPilot" Text="Dummy Pilot" />
    </div>
    <asp:Panel runat="server" ID="pnlChangePilot" Style="display: none">
        <div>
            <table class="clstablelistout" id="Table5">
                <tr>
                    <td align="right">
                        <asp:UpdatePanel runat="server" ID="upnlPilot" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="Table6">
                                    <tr>
                                        <td align="left" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="Span3" class="clsFormHeader">Change Pilot/Co-Pilot</span>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        <table id="Table7" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnPilotOk" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        Text="Ok" ToolTip="Click to add new Pilot"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPilotClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        Text="Close" ToolTip="Click to close Change Pilotscreen" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>

                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="1" runat="server"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvPilot1" runat="server" ErrorMessage="Enter correct Pilot name."
                                                ControlToValidate="cmbPilot1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvPilot2" runat="server" ErrorMessage="Enter correct Co-Pilot name."
                                                ControlToValidate="cmbPilot2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <table>
                                        <tr>
                                            <td>
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                    <legend id="Legend2" class="clsFieldSet1" runat="server"><b>Pilot</b></legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblCurrentPilot" class="clsLabel">Current Pilot</span>
                                                            </td>
                                                            <td colspan="1">
                                                                <asp:TextBox ID="txtCurrentPilot" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblChangePilot1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblChangePilot" class="clsLabel">Change Pilot</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbPilot1" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                    <legend id="Legend3" class="clsFieldSet1" runat="server"><b>Co-Pilot</b></legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="Span4" class="clsLabel">Current Co-Pilot</span>
                                                            </td>
                                                            <td colspan="1">
                                                                <asp:TextBox ID="txtCurrentCoPilot" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" ReadOnly="True" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblChangePilot2" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lb2lChangePilot" class="clsLabel">Change Co-Pilot</span>
                                                            </td>
                                                            <td>
                                                                 <asp:DropDownList ID="cmbPilot2" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                            </td>
                                            <%--<td valign="top" align="right">
                                                <table id="Table7" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnPilotOk" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Ok" ToolTip="Click to add new Pilot"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPilotClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Close" ToolTip="Click to close Change Part Pilotscreen" CausesValidation="False">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpChangePilot" runat="server" TargetControlID="btnDummyPilot"
        PopupControlID="pnlChangePilot" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End Change Pilot-->
    <script type="text/javascript">
        function delete_cookie() {
            $.cookie('HideInfoMessagepanel', null);
        }
    </script>
    </form>
  
    
</body>
</html>
