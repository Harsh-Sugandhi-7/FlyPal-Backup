<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditExecutionCalendarList_AJAX.aspx.vb"
    Inherits="Flypal.wfAuditExecutionCalendarList_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Audit Compliance List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <span id="lblAuditExecutionList" class="clsFormHeader">Audit Calendar List</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationsummary2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
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
                                                if (SearchIndex != 4) {
                                                    txtFromDateobj.style.display = 'none';
                                                    txtToDateobj.style.display = 'none';
                                                    lblFromDateobj.style.display = 'none';
                                                    lblToDateobj.style.display = 'none';
                                                }
                                                else {
                                                    var DateIndex = $get("cmbDateRange").selectedIndex;
                                                    if (DateIndex == 0) {
                                                        txtFromDateobj.style.display = 'none';
                                                        txtToDateobj.style.display = 'none';
                                                        lblFromDateobj.style.display = 'none';
                                                        lblToDateobj.style.display = 'none';
                                                    }
                                                }

                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Search Criteria</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table id="Table1" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="170px"
                                                                        AutoPostBack="True">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="1">Text</asp:ListItem>
                                                                        <asp:ListItem Value="2">Audit Status</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                        Visible="False">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                        <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                        <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                        <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="cmbAuditStatusName" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        Visible="False" AutoPostBack="True">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="1">Open</asp:ListItem>
                                                                        <asp:ListItem Value="2">Approaching Next 30 days</asp:ListItem>
                                                                        <asp:ListItem Value="3">Forecasting</asp:ListItem>
                                                                        <asp:ListItem Value="4">Pending</asp:ListItem>
                                                                        <asp:ListItem Value="5">Close</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtSearchText" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Search Text"
                                                                        BackColor="White" AutoPostBack="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Width="66px">From Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                        ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
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
                                                                <td>
                                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Width="52px">To Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                        ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
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
                                                    <td align="right">
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Audit Execution List as per searching criteria"
                                                            Text="Find Now" ValidationGroup="a" OnClientClick="DisableValidators();" Visible="False">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
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
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH" ToolTip="Click to close Audit Calender List"
                                                                            Text="Close" Visible ="false"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div id="toolbar" style="width: 100%">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <asp:UpdatePanel ID="upnlCurrentDate" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <a href="javascript:dpm.commandCallBack('previous');">◄</a> <a href="javascript:dpm.commandCallBack('next');">
                                                                                            ►</a> <a href="javascript:dpm.commandCallBack('today');">This Month </a>
                                                                                    </td>
                                                                                    <td>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td style="width: 90%;" align="center">
                                                                    <asp:UpdatePanel ID="upnlShowDate" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lblCurrentDate" runat="server" CssClass="clsLabelHeader" Height="16px"
                                                                                Text=""></asp:Label>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlcontrol" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel runat="server" ID="Panel1">
                                                                <div style="margin-top: -21px;">
                                                                    <div style="border-top: 1px solid Black; border-left: 1px solid Black; position: relative;
                                                                        top: 20px; z-index: 1; left: 0px; width: 35px; height: 19px; background-color: #efefef;">
                                                                    </div>
                                                                    <DayPilot:DayPilotMonth ID="DayPilotMonth1" runat="server" DataEndField="ToDateTime"
                                                                        DataStartField="FromDateTime" DataTextField="name" DataValueField="id" DataTagFields="name, id"
                                                                        ContextMenuID="DayPilotMenu1" ClientObjectName="dpm" EventMoveHandling="CallBack"
                                                                        OnEventMove="DayPilotMonth1_EventMove" Width="756px" EventResizeHandling="CallBack"
                                                                        OnEventResize="DayPilotMonth1_EventResize" OnTimeRangeSelected="DayPilotMonth1_TimeRangeSelected"
                                                                        TimeRangeSelectedHandling="PostBack" OnBeforeEventRender="DayPilotMonth1_BeforeEventRender"
                                                                        BubbleID="DayPilotBubble1" ShowToolTip="true" OnCommand="DayPilotMonth1_Command"
                                                                        EventClickHandling="PostBack" OnEventClick="DayPilotMonth1_EventClick" EventStartTime="false"
                                                                        EventEndTime="false" OnBeforeCellRender="DayPilotMonth1_BeforeCellRender" HeaderBackColor="#efefef"
                                                                        NonBusinessBackColor="White" BackColor="#ECF3FB" InnerBorderColor="#99BBE6" AutoRefreshEnabled="True"
                                                                        EventTextAlignment="Center" Height="356px" AfterRenderJavaScript="afterRender(data)">
                                                                    </DayPilot:DayPilotMonth>
                                                                </div>
                                                            </asp:Panel>
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
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH" ToolTip="Click to close Audit Calender List"
                                                        Text="Close"></asp:Button>
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            showTextField();
        });
    </script>
    <!-- AuditExecution Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAuditExecution" Text="Dummy AuditExecution"
            ClientIDMode="Static" CausesValidation="false" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupAuditExecution" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupAuditExecution" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupAuditExecution" runat="server" TargetControlID="btnDummyAuditExecution"
        PopupControlID="pnlPopupAuditExecution" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameAuditExecutionStateComplete() {
            $("#btnDummyAuditExecution").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        function OpenAuditExecutionWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#iPopupAuditExecution").attr("src", "wfAuditExecution_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyAuditExecution").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForAuditExecution() {
            var AuditExecutionwindow = $find("<%=mdlPopupAuditExecution.ClientID %>");
            //close AuditExecution popup window
            AuditExecutionwindow.hide();
            $("#iPopupAuditExecution").attr("src", "JavaScript:''");
            //call AuditExecution image button
            $("#hdnimgBtnAuditExecution").click();
        }
        function afterRender(data) {
            // check if the label should be updated
            if (data && data.label) {
                var label = document.getElementById("lblCurrentDate");
                label.innerHTML = data.label;
            }
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
</body>
</html>
