<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfWOJobNRCWatchList.aspx.vb"
    Inherits="Flypal.JobNRCWatchList" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTlkt" %>
<%@ Register TagPrefix="msgBox" TagName="MSGBox" Src="MSGBox.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WO Job NRC WatchList</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="js/query-1.7.1.js" type="text/javascript"></script>

    <script type="text/javascript" id="openScript">

        function openPageInSameWindow(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function displayReport() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
</head>
<body>
    <form id="frmWOJobNRCWatchList" runat="server">

        <script type="text/javascript" id="pageLoadScript">

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                console.log("Entered the Page Load function.");

                $("#txtWONO").on("keypress", function (event) {
                    console.log("keypress event fired.");
                    validateText('D', document.getElementById('txtWONO').value, event);
                    console.log("validateText function completed.");
                });

                console.log("Page Load function completed.");
            });

        </script>
        <script type="text/javascript" id="favIconScripts">

            function fnMarkOrRemoveFavorite(x) {
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

        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <msgBox:msgbox id="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <table class="clstablelistout" id="Table-MaxWidth">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblInner" class="clstablelistin" width="100%">
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTitle" runat="server"
                                                                CssClass="clsFormHeader"
                                                                Text="W.O. JOB NRC WatchList" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnPrintReport" runat="server"
                                                                CssClass="clsbtnH clsinfoH"
                                                                ToolTip="Print Job NRC WatchList Report."
                                                                Text="Print" CausesValidation="False" />

                                                            <asp:Button ID="btnClose" runat="server"
                                                                CssClass="clsbtnH clsinfoH"
                                                                ToolTip="Close Job NRC Watch List screen."
                                                                Text="Close" CausesValidation="False" />

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="tdFavICN" align="center">
                                                <span id="spFavICN">
                                                    <i id="favICN" runat="server"
                                                        onclick="fnMarkOrRemoveFavorite(this)"
                                                        class="fa fa-star fa-spin fa-5x circle-icon" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="vsWOJobNRCWatchList" runat="server"
                                        CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields."
                                        ValidationGroup="NRCWatchList" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblWOText" runat="server"
                                                            CssClass="clsLabelAuto" Text="W.O." />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlWOText" runat="server"
                                                            CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataTextField="WOText" DataValueField="WOText">
                                                            <asp:ListItem Value="0" Text="(ALL)" />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWONo" runat="server"
                                                            CssClass="clsLabel" Text="No" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWONO" runat="server"
                                                            CssClass="clsTextBoxTagSearchSmall"
                                                            MaxLength="6" ToolTip="Enter Number" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="btnFilterRecords" runat="server"
                                        ImageUrl="~/images/Search2.png" class="clsSearch2btn"
                                        ToolTip="Click to find list of Discrepancy as per searching criteria"
                                        ValidationGroup="1" CausesValidation="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div>
                                                <asp:GridView ID="gvJobNRCWatchList" runat="server"
                                                    ShowHeaderWhenEmpty="True" AllowSorting="True"
                                                    AllowPaging="True" AutoGenerateColumns="False" PageSize="10"
                                                    CellPadding="5" CssClass="clsGridNewStyle" GridLines="Horizontal">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader"
                                                        Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First"
                                                        LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging"
                                                        ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <%--0--%>
                                                        <asp:BoundField Visible="False"
                                                            DataField="ID" HeaderText="ID">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--1--%>
                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--2--%>
                                                        <asp:BoundField DataField="WONumber" HeaderText="W.O. Number">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--3--%>
                                                        <asp:BoundField DataField="JobDescription"
                                                            HeaderText="Description" HtmlEncode="False">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--4--%>
                                                        <asp:BoundField DataField="JobAction"
                                                            HeaderText="Action" HtmlEncode="False">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--5--%>
                                                        <asp:BoundField DataField="WatchListInstructions"
                                                            HeaderText="WatchList Instructions">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--6--%>
                                                        <asp:BoundField DataField="JobStartDateFormatted"
                                                            HeaderText="Start Date">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--7--%>
                                                        <asp:BoundField DataField="JobCloseDateFormatted"
                                                            HeaderText="Close Date">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--8--%>
                                                        <asp:BoundField DataField="JobActualTime"
                                                            HeaderText="Actual Man Hr.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--9--%>
                                                        <asp:TemplateField HeaderText="Add To Inspection"
                                                            ItemStyle-VerticalAlign="Middle"
                                                            HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-Wrap="True"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkAddToInspection"
                                                                    runat="server" CausesValidation="false"
                                                                    Text='Add To Inspection'
                                                                    CommandArgument='<%# Eval("ID") %>'
                                                                    CommandName="AddToInspection">		
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
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
                                                        <asp:Button ID="hdnBtnMarkFavorite" ClientIDMode="Static"
                                                            runat="server" CausesValidation="False"
                                                            CssClass="DisplayNone" />
                                                        <asp:Button ID="hdnBtnRemoveFavorite" ClientIDMode="Static"
                                                            runat="server" CausesValidation="False"
                                                            CssClass="DisplayNone" />
                                                        <asp:Button ID="hdnBtnAddToInspection" ClientIDMode="Static"
                                                            runat="server" CausesValidation="False"
                                                            CssClass="DisplayNone" />
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

        </div>

    </form>

</body>
</html>
