<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfGSTValueList.aspx.vb" Inherits="Flypal.GSTValueListReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GST Value List Report</title>

    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" id="clientEventHandlersJS">

        function openCrystalReport() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>

</head>
<body>

    <form id="frmGSTValueListReport" runat="server">

        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <table id="Table-MaxWidth" class="clstablelistout">
            <tr>
                <td colspan="2">
                    <table width="100%">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <asp:Label runat="server" ID="lbltitle" class="clsFormHeader"
                                    Text="GST Value" />
                            </td>
                            <td id="tdFavICN" align="center">
                                <span id="spFavICN">
                                    <i id="favICN" runat="server" onclick="fnMarkOrRemoveFavorite(this)"
                                        class="fa fa-star fa-spin fa-5x circle-icon" />
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="upnlValidationErrors" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="vsGSTChargeList" runat="server"
                                ValidationGroup="GSTChargeList" CssClass="clsValidationSummary"
                                HeaderText="Kindly look into following message(s)." />
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                ErrorMessage="From Date is Required." ControlToValidate="txtFromDate" Display="None"
                                ValidationGroup="GSTChargeList" />
                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="GSTChargeList"
                                ErrorMessage="From Date should not be greater than To Date." />
                            <asp:CustomValidator ID="cvGSTChargeList" runat="server" CssClass="clsLabelAuto" Display="None"
                                ClientValidationFunction="ValidateGSTChargeList" ValidationGroup="GSTChargeList"
                                ErrorMessage="At least one GST charge must be selected." />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <script type="text/javascript" id="validationScript">
                        function ValidateGSTChargeList(source, args) {

                            console.log("Entered ValidateGSTChargeList Function");
                            var checkboxes = document.querySelectorAll('#chkGSTChargeList input[type="checkbox"]');
                            var isChecked = Array.prototype.slice.call(checkboxes).some(x => x.checked);
                            args.IsValid = isChecked;

                            console.log("Existing ValidateGSTChargeList Function");
                        }
                    </script>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblStep1Header"
                        class="clsLabelHeader" Text="Selection of Dates" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblFromDate"
                                            class="clsLabelAuto" Text="From" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFromDate"
                                            CssClass="clsTextBoxTagSearchDate" Width="100px"
                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');" />
                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server"
                                            CssClass="cal_Theme1" Enabled="true"
                                            Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate" />
                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                            WatermarkCssClass="clsDateTextBox" />
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblToDate"
                                                class="clsLabelAuto" Text="To" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtToDate"
                                                CssClass="clsTextBoxTagSearchDate" Width="100px"
                                                onchange="ValidateDateText(this,'ToDate_WatermarkExtender');" />
                                            <cc2:CalendarExtender ID="ToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate" />
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_WatermarkExtender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox" />
                                        </td>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblStep2Header" runat="server" class="clsLabelHeader"
                        Text="Selection of GST Charge" />
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="2">
                    <table width="100%">
                        <tr>
                            <td style="width: 25px">
                                <input type="checkbox" style="vertical-align: bottom;" id="chkSelectAll" />
                            </td>
                            <td style="width: 100%">
                                <asp:Panel ID="cpnlGSTChargeList" runat="server" CssClass="clsCollapsePnl">
                                    <asp:Label runat="server" ID="lblCollapsiblePanelTitle" class="clsLabelHeader"
                                        Text="GST Charge List" />
                                    <div id="divCollapsiblePnlImg">
                                        <image id="imgMasters" src="images/collapse_blue.jpg"
                                            alternatetext="(Show Details...)" />
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlGSTChargeList" runat="server" ClientIDMode="Static" Visible="true">
                                    <asp:CheckBoxList ID="chkGSTChargeList" runat="server" ClientIDMode="Static"
                                        CssClass="clsComboBox_Ajax" DataTextField="Charge" DataValueField="ID"
                                        EnableViewState="false" RepeatColumns="4" RepeatDirection="Horizontal"
                                        Width="100%" CausesValidation="true" />
                                </asp:Panel>
                                <cc2:CollapsiblePanelExtender ID="cpnlExtGSTChargeList" runat="Server"
                                    BehaviorID="cpnlGSTChargeListBhehavior" ClientIDMode="Static"
                                    CollapseControlID="cpnlGSTChargeList" Collapsed="false"
                                    CollapsedImage="~/images/expand_blue.jpg" CollapsedText="(Show Details...)"
                                    ExpandControlID="cpnlGSTChargeList" ExpandedImage="~/images/collapse_blue.jpg"
                                    ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl"
                                    SkinID="CollapsiblePanelDemo" SuppressPostBack="false"
                                    TargetControlID="pnlGSTChargeList" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDisplayReport" runat="server"
                                            CssClass="clsbtnH clsinfoH1" ValidationGroup="GSTChargeList"
                                            CausesValidation="true" Text="Display"
                                            ToolTip="Display GST Value List Report." />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Export to Excel"
                                            ToolTip="Click to Export report" Width="140px" Visible="True" ValidationGroup="GSTChargeList" CausesValidation="true"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClose" runat="server"
                                            CausesValidation="False"
                                            CssClass="clsbtnH clsinfoH1" Text="Close"
                                            ToolTip="Close GST Value List screen." />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static" runat="server"
                                            Text="----" CausesValidation="False" Style="display: none;" />
                                        <asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>

        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="10" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server"
                                    ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                    CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

        <div id="dateValidationsScripts">

            <script type="text/javascript">

                //From Date - To Date validation
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

            <script type="text/javascript" id="favIconScripts">

                function fnMarkOrRemoveFavorite(x) {
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

                function MarkAsFavorite() {

                    console.log("Entered function MarkAsFavorite.");
                    var star = document.getElementById("<%=favICN.ClientID%>");
                    star.classList.add("fa-star");
                    star.classList.remove("fa-star-o");
                    star.style.color = '#fff';
                    star.style.border = 'black';
                    console.log("function MarkAsFavorite Completed.");

                }

                function RemoveFromFavorite() {

                    console.log("Entered function RemoveFromFavorite.");
                    var star = document.getElementById("<%=favICN.ClientID%>");
                    star.classList.add("fa-star-o");
                    star.classList.remove("fa-star");
                    star.style.border = 'black';
                    console.log("function RemoveFromFavorite Completed.");

                }

            </script>

            <script type="text/javascript">

                /* Check / Uncheck All Checkboxes */
                $(document).ready(function () {

                    $("#chkSelectAll").click(function () {
                        var status = $("#chkSelectAll").attr("checked");
                        $("#chkGSTChargeList").find(":checkbox").each(function () {
                            if (status == "checked") {
                                $(this).attr("checked", status);
                            }
                            else {
                                $(this).removeAttr("checked");
                            }

                        });
                    });

                    $("#btnSearchCriteria,#btnDisplayReport,#btnExport").live('click', function () {
                        try {
                            SetSelectedGSTCharge();
                        } catch (e) {
                            alert(e.Message);
                        }
                        return true;
                    });
                });

                /* Set selected GST Charge Text (Charge Name) to Hidden Field to access it from Code Behind */
                function SetSelectedGSTCharge() {
                    var GSTChargelist = new Array();
                    $("#chkGSTChargeList :checked").each(function (i) {
                        GSTChargelist.push($(this).next().text());
                    });

                    $("#hdnBtnGSTChargeList").val('');
                    $("#hdnBtnGSTChargeList").val(GSTChargelist);
                }

            </script>

        </div>

        <div id=" fiddenField">

            <asp:HiddenField ID="hdnBtnGSTChargeList" runat="server" ClientIDMode="Static" />

        </div>

    </form>

</body>
</html>
