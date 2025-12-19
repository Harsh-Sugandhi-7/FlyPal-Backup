<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEngineDerateRegister.aspx.vb"
    Inherits="Flypal.SearchCriteriaForEngineDerateRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjakToolKit" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Engine Derate Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="2000" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBOX id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>

            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader"
                                        Text="Engine Derate Register" />
                                </td>
                                <td id="tdFavICN" align="center">
                                    <span id="spFavICN">
                                        <i id="favICN" runat="server" onclick="fnMarkFavoriteUnFavorite(this)"
                                            class="fa fa-star fa-spin fa-5x circle-icon" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server">
                            <table id="tblInner">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server"
                                                    HeaderText="Fill Up The Following Fields"
                                                    CssClass="clsValidationSummary" ValidationGroup="vsEngineDerateRegister" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    CssClass="clsLabelAuto" ErrorMessage="To Date Required"
                                                    ControlToValidate="txtToDate" Display="None" ValidationGroup="vsEngineDerateRegister" />
                                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                    ErrorMessage="To Date Required" ValidationGroup="vsEngineDerateRegister" />
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                    ErrorMessage="From Date Required" ValidationGroup="vsEngineDerateRegister" />
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="From Date should not be greater than To Date."
                                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="vsEngineDerateRegister" />
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server"
                                                    CssClass="clsLabelAuto" Display="None" ControlToValidate="txtToDate"
                                                    ErrorMessage="To Date Required" ValidationGroup="vsEngineDerateRegister" />
                                                <asp:CustomValidator ID="cvAircraft" runat="server"
                                                    CssClass="clsLabelAuto" Display="None"
                                                    ControlToValidate="ddlAircraft" ErrorMessage="Select the Aircraft"
                                                    OnServerValidate="CustomValidations" ValidationGroup="vsEngineDerateRegister" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSelectDates" runat="server" CssClass="clsLabelHeader"
                                            Text="Step I. Selection of Dates" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlDateSelection" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFromDateStar" runat="server" CssClass="clsLabelStar" Text="*" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Text="From Date" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"
                                                                ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');" />
                                                            <AjakToolKit:CalendarExtender ID="calFromDate_CalendarExtender" runat="server"
                                                                CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="txtFromDate" />
                                                            <AjakToolKit:TextBoxWatermarkExtender TargetControlID="txtFromDate"
                                                                ID="FromDate_watermarkextender" ClientIDMode="Static" runat="server"
                                                                WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Text="To Date" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate"
                                                                Style="margin-left: 3px;"
                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                ClientIDMode="Static" runat="server" />
                                                            <AjakToolKit:CalendarExtender ID="calToDate_CalendarExtender" runat="server"
                                                                CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="txtToDate" />
                                                            <AjakToolKit:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="4" align="left">
                                                            <asp:Label ID="lblSelectAircraft" runat="server" CssClass="clsLabelHeader"
                                                                Text="Step II. Selection of Aircraft" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblAircraftStar" runat="server"
                                                                CssClass="clsLabelStar" Text="*" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblAircraft" runat="server"
                                                                CssClass="clsLabelAuto" Text="Aircraft " />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                ID="ddlAircraft" runat="server" DataValueField="ID"
                                                                DataTextField="RegNo" AutoPostBack="True" />
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlLocalUTC" UpdateMode="Conditional" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:RadioButton ID="rdbLocal" runat="server" GroupName="a"
                                                                        Text="Local" Visible="false"
                                                                        CssClass="clsRadioButton" />
                                                                    &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rdbUTC" runat="server" GroupName="a"
                                                                        Text="UTC" Visible="false" CssClass="clsRadioButton" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <asp:PlaceHolder ID="phEngineDerate" runat="server"
                                                        Visible='<%#IIf(CBool(AppSettings("ShowEngineDerateOptions")), True, False) %>'>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblSelectEngineDerate" runat="server"
                                                                    CssClass="clsLabelHeader" Text="Step III. Selection of Engine Derate" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblEngineDerate" runat="server"
                                                                    CssClass="clsLabelAuto" Text="Engine Derate" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlEngineDerate" runat="server"
                                                                    CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    DataTextField="Name" DataValueField="ID" />
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
                                        <asp:Label ID="lblDisplayReport" runat="server"
                                            CssClass="clsLabelHeader" Text="Step IV. Display Report" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSummary" runat="server" Visible="false"
                                            CssClass="clsLabelAuto" Text="Your selection are as follows " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlSearchCriteria" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td></td>
                                                        <td align="left">
                                                            <asp:Label ID="lblSearchCriteriaDateRangeFrom" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblSearchCriteriaDateRangeTo" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaAircraft" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaEngineDerate" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlButtons" runat="server" CssClass="clspanel1">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1"
                                                                ID="btnCurrentSearchCriteria"
                                                                TabIndex="0" runat="server"
                                                                Text="Current Criteria" CausesValidation="False"
                                                                ToolTip="Display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1"
                                                                ID="btnDisplay" TabIndex="0" runat="server"
                                                                ValidationGroup="vsEngineDerateRegister" Text="Display"
                                                                ToolTip="Display Report in PDF" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose"
                                                                runat="server" Text="Close"
                                                                CausesValidation="False" />
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
                <tr>
                    <td colspan="2" align="right">
                        <asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="hdnBtnMarkFavorite" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnRemoveFavorite" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnImportCRSLogs" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>

        </div>

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

        <div>

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

            <%--Mark as Favorite--%>
            <script type="text/javascript">

                function fnMarkFavoriteUnFavorite(x) {

                    if (x.classList.contains("fa-star")) {

                        x.classList.remove("fa-star");
                        x.classList.add("fa-star-o");
                        x.style.color = 'black';
                        x.style.border = 'black';
                        $("#hdnBtnRemoveFavorite").click();

                    }
                    else {

                        x.classList.remove("fa-star-o");
                        x.classList.add("fa-star");
                        x.style.color = '#fff';
                        x.style.border = 'black';
                        $("#hdnBtnMarkFavorite").click();

                    }

                }

                function MarkAsFavorite() {

                    var redstar = document.getElementById("<%=favICN.ClientID%>");

                    redstar.classList.add("fa-star");
                    redstar.classList.remove("fa-star-o");
                    redstar.style.color = '#fff';
                    redstar.style.border = 'black';

                }

                function RemoveFromFavorite() {

                    var redstar = document.getElementById("<%=favICN.ClientID%>");

                    redstar.classList.add("fa-star-o");
                    redstar.classList.remove("fa-star");
                    redstar.style.border = 'black';

                }

            </script>

        </div>

    </form>
</body>
</html>
