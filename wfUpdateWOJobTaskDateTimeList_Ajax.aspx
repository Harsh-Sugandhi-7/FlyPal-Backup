<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateWOJobTaskDateTimeList_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateWOJobTaskDateTimeList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>WO List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
                        <table id="tblInner">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">List Of W.O.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                        ValidationGroup="a"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                    <script type="text/javascript">
                                        function showTextField() {
                                            var SearchIndex = $get("cmbSearch").selectedIndex;

                                            var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                            var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                            var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                            var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
                                            if (SearchIndex != 1) {
                                                txtFromDateobj.style.display = 'none';
                                                txtToDateobj.style.display = 'none';
                                                lblFromDateobj.style.display = 'none';
                                                lblToDateobj.style.display = 'none';
                                            }
                                            else {
                                                var DateIndex = $get("cmbDate").selectedIndex;
                                                if (DateIndex == 0) {
                                                    txtFromDateobj.style.display = 'none';
                                                    txtToDateobj.style.display = 'none';
                                                    lblFromDateobj.style.display = 'none';
                                                    lblToDateobj.style.display = 'none';
                                                }
                                            }

                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1">
                                                <tr>
                                                    <td style="width: 69px">
                                                        <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel" Height="8px" Width="55px">Search</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                            <asp:ListItem Value="0" Selected="True">(ALL)</asp:ListItem>
                                                            <asp:ListItem Value="1">Date</asp:ListItem>
                                                            <asp:ListItem Value="2">W.O.</asp:ListItem>
                                                            <asp:ListItem Value="3">Aircraft</asp:ListItem>
                                                            <asp:ListItem Value="4">Model</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="L1" runat="server" CssClass="clsLabel" Width="20px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="True"
                                                            Visible="False">
                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                            <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbWO" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="True"
                                                            Visible="False" DataTextField="WOText" DataValueField="WOText">
                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox1_Ajax" Visible="False"
                                                            DataTextField="RegNo" DataValueField="RegNo" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox1_Ajax" Visible="False"
                                                            DataTextField="Name" DataValueField="Name" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblNo" runat="server" CssClass="clsLabel" Height="8px" Width="32px"
                                                            Visible="False">No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" Width="184px" Visible="False"
                                                            MaxLength="4" ToolTip="Enter Number" AutoPostBack="True"></asp:TextBox>
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
                                                <tr>
                                                    <td style="padding-left: 4px" colspan="10">
                                                        <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                            Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto">Select Work Order from the
                                        list. Click On Update Link to Update Job and Task Start/End Date Time</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 43px">
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Work Order as per criteria :  Record(s) found.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="height: 43px" align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Work Order as  per searching criteria"
                                                            CausesValidation="true" ValidationGroup="a" Text="Find Now" Visible="False" OnClientClick="DisableValidators();">
                                                        </asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
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
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgWOList" runat="server" CssClass="clsGrid" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
                                                EnableViewState="false" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False"
                                                PageSize="25">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="WODateFormatted" HeaderText="Date">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WONumber" SortExpression="WONumber" HeaderText="W. O. No.">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WORemark" SortExpression="WORemark" HeaderText="W. O. Description">
                                                        <HeaderStyle Wrap="false" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CustomerWONo" SortExpression="CustomerWONo" HeaderText="Cust. WONo">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WOBy" SortExpression="WOBy" HeaderText="Created  By ">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Submitted By">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="UpdateRec" HeaderText="Update" Text="Update">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
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
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Work Order  screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnUpdate" runat="server" CausesValidation="false" ClientIDMode="Static"
                                                Style="display: none;" Text="Add" />
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
    </div>
    <!--Update Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyUpdate" Text="Update" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlUpdate" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeUpdate" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlUpdate" runat="server" TargetControlID="btnDummyUpdate"
        PopupControlID="pnlUpdate" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameUpdateStateComplete() {
            $("#btnDummyUpdate").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenUpdateWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeUpdate").attr("src", "wfUpdateWOJobTaskDateTime_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyUpdate").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForUpdate() {
            var Updatewindow = $find("<%=mdlUpdate.ClientID %>");
            //close popup window
            Updatewindow.hide();
            //release resources
            $("#IframeUpdate").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnUpdate").click();
        }
    </script>
    <!-- End-->
    </form>
    <script type="text/javascript">
        function DisableValidators() {
            var SearchIndex = $get("cmbSearch").selectedIndex;
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
    <%-- Row Highlight--%>
    <script type="text/javascript">
        //event handler for end request i.e last event in client page cycle.
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        //event handler for begin request i.e before sending request to the server
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

        var element;
        var timerId;
        var timeoutforblink;
        var hideRowHighlight = false;

        function endRequestHandler(sender, args) {
            var tempval = parseInt($("#gridrowindex").val()); //row number ..0 is header row..
            if (tempval) {
                $("#dgWOList tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
                if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
                    var elem;
                    var tempaction = $("#gridrowaction").val(); //action to be performed

                    //button close of popup windows
                    //remove highlight row class... and return from function
                    if (tempaction == "close") {
                        $("#dgWOList tr:eq(" + tempval + ")").removeClass('activerow');
                        $("#gridrowaction").val('');
                        return;
                    }
                    //blink Rate column of the row for perticular interval
                    else if (tempaction == "SaveRec") {
                        $("#dgWOList tr:eq(" + tempval + ")").removeClass('activerow');
                        elem = $("#dgWOList tr:eq(" + tempval + ") td:eq(5)");
                        $("#gridrowaction").val('');
                    }

                    else {
                        return;
                    }
                    //blink column function
                    timeoutforblink = setInterval(function () {

                        if (elem.hasClass('activerow')) {
                            elem.removeClass('activerow');
                        }
                        else {
                            elem.addClass('activerow');
                        }

                    }, 500);
                    //stop blink column
                    timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);
                }


            }
        }

        function BeginRequestHandler(sender, args) {
            clearTimeout(timerId);
            element = args.get_postBackElement();
            //change location popup ok button event occur
            if (element.id == "hdnBtnUpdate") {
                hideRowHighlight = true;
                $("#gridrowaction").val('SaveRec');
            }
            //any of change popup close button event occur 
//            else if (element.id == "btnBack") {
//                hideRowHighlight = true;
//                $("#gridrowaction").val('close');
//            }
            //change parttype ||change location link event occur
            //reset rowindex value if other grid event occurs
            else if (element.id == "dgWOList") {
                if ($("#gridrowaction").val() != "gridrow") {
                    $("#gridrowindex").val('');
                }
            }
            //any other events
            else {
                $("#gridrowindex").val('');
            }
        }

        //stop blinking
        function TimeOut(val, action) {
            var tempelem;
            if (action == "SaveRec") {
                tempelem = $("#dgWOList tr:eq(" + val + ")");
                tempelem.removeClass('activerow');

            }
            else {
                return;
            }
            $("#gridrowindex").val('');
            hideRowHighlight = false;
            clearInterval(timeoutforblink);
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dgWOList tr td a").live("click", function () {
                var temp = $(this).parent().parent()[0].rowIndex;
                $("#gridrowindex").val(temp);
                $("#gridrowaction").val('gridrow');
            });
        });
    </script>
</body>
</html>
