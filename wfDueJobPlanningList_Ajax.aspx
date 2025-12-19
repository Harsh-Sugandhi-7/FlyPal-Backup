<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDueJobPlanningList_Ajax.aspx.vb" Inherits="Flypal.wfDueJobPlanningList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List Of Planned Due Job</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblMain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>

                                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">List Of Planned Due Job
                                                                    </asp:Label>

                                                                </td>
                                                                <td align="right">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddNewTop" runat="server" CausesValidation="true" CssClass="clsbtnH clsinfoH"
                                                                                    Text="Add New" ToolTip="Click to Add New Record" ValidationGroup="a" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print" visible="false"
                                                                                    Text="Print" CausesValidation="False"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                    Text="Close" ToolTip="Click to Close" />
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
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

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
                                                                <td>
                                                                    <span id="Span8" class="clsLabel">Range</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                        <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                        <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                        <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblFromDate" class="clsLabel" runat="server">From Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblToDate" class="clsLabel" runat="server">To Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <span id="Span3" class="clsLabel">Plan No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbPlanNo" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                        AutoPostBack="True" DataTextField="Text" DataValueField="Text">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Width="40px"
                                                                        MaxLength="8"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                        </table>

                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="imgFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                    ToolTip="Click to find list as per searching criteria" ValidationGroup="a"></asp:ImageButton>
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
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto " Font-Bold="True">As per criteria : Record(s) found</asp:Label>
                                                    </td>


                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                &nbsp;
                                                            <asp:Label ID="lblShowEntries" runat="server" Text="Show Entries"></asp:Label>
                                                                <asp:DropDownList ID="cmbShowE" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="55px"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                                                    <asp:ListItem Value="0">5</asp:ListItem>
                                                                    <asp:ListItem Value="1">10</asp:ListItem>
                                                                    <asp:ListItem Value="2">15</asp:ListItem>
                                                                    <asp:ListItem Value="3">20</asp:ListItem>
                                                                    <asp:ListItem Value="4">25</asp:ListItem>
                                                                    <asp:ListItem Value="5">30</asp:ListItem>
                                                                    <asp:ListItem Value="6">40</asp:ListItem>
                                                                    <asp:ListItem Value="7">45</asp:ListItem>
                                                                    <asp:ListItem Value="8">50</asp:ListItem>
                                                                    <asp:ListItem Value="9">55</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>

                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlSearchBox" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblAircraft" class="clsLabel">Aircraft</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
                                                                                DataTextField="RegNo" DataValueField="ID">
                                                                            </asp:DropDownList>
                                                                            <asp:CustomValidator ID="cvAircraftList" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                                ControlToValidate="cmbAircraft" ErrorMessage="Select Aircraft From The List."
                                                                                OnServerValidate="customvalidate" ValidationGroup="a"></asp:CustomValidator>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"
                                                                                AutoPostBack="true"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgDueJobPlanningList" runat="server" AllowSorting="True" AllowPaging="true"
                                                            AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle" DataKeyNames="ID"
                                                            EnableViewState="true" GridLines="Horizontal"
                                                            PageSize="10" ShowHeaderWhenEmpty="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                <%--1--%>
                                                                <asp:BoundField DataField="DueJobPlanningDateFormatted" HeaderText="Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="DueJobPlanningNo" HeaderText="No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Reg No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>

                                                                <%--4--%>
                                                                <asp:BoundField DataField="FromDateFormatted" HeaderText="From">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="ToDateFormatted" HeaderText="To">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>

                                                                <%--6--%>
                                                                <asp:BoundField DataField="WONumber" HeaderText="W. O. No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandName="EditRec" ToolTip="Click to edit"
                                                                                                Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>

                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRec" ToolTip="Click to delete"
                                                                                                Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </td>
                                                                                        <asp:PlaceHolder runat="server" ID="phCreateWO" Visible='<%# Not Eval("IsWOCreated")%>'>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="CreateWO" runat="server" CommandArgument="CreateWORec" ToolTip="Click to Create Work Order"
                                                                                                    CommandName="CreateWORec" Style="height: 20px; width: 17px" ImageUrl="~/images/TaskCard.png" />
                                                                                            </td>
                                                                                        </asp:PlaceHolder>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn" width="20px" height="20px"
                                                                                Style="cursor: pointer;" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        </asp:GridView>
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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


    </form>
</body>
</html>
