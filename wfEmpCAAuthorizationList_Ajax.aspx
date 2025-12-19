<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmpCAAuthorizationList_Ajax.aspx.vb" Inherits="Flypal.wfEmpCAAuthorizationList_Ajax" %>

<%@ Import Namespace="Flypal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company Authorization List</title>
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

                                                                    <asp:Label ID="LblTitle" runat="server" CssClass="clsFormHeader">List Of Company Authorization
                                                                    </asp:Label>

                                                                </td>
                                                                <td align="right">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddNewTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                    Text="Add New" ToolTip="Click to Add New Record" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print"
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
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional" >
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Span8" class="clsLabel">Range</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
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
                                                                                <span id="Span3" class="clsLabel" runat="server" Visible="false">No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbEmpCAAuthorizationText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    AutoPostBack="True" DataTextField="Text" DataValueField="Text" Visible="false">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Width="40px"
                                                                                    MaxLength="8" Visible="false"></asp:TextBox>
                                                                            </td>

                                                                        </tr>

                                                                    </table>
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
                                                                <asp:DropDownList ID="cmbShowE" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="55px"
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
                                                                            <asp:CheckBox ID="chkExpiredMSP" runat="server" CssClass="clsCheckBox" Text="Expired Company Authorization" AutoPostBack="true"></asp:CheckBox>
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
                                                        <asp:GridView ID="dgEmpCAAuthorizationList" runat="server" AllowSorting="False" AllowPaging="true"
                                                            AutoGenerateColumns="False" CellPadding="10" CssClass="clsGridNewStyle" DataKeyNames="ID"
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
                                                                <asp:BoundField DataField="EmpCAAuthorizationDateFormatted" HeaderText="Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="Number" HeaderText="No." SortExpression="CAAuthorizationNo" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="CANumber" HeaderText="Authorization No." SortExpression="CANumber">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="EmployeeName" HeaderText="Employee" SortExpression="Employee">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="EmployeeCode" HeaderText="Code" SortExpression="Employee">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="CAInitialIssueDateFormatted" HeaderText="Issue Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="CAValidUptoFormatted" HeaderText="Valid Upto">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:BoundField DataField="RevisionNo" HeaderText="Revision No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--9--%>
                                                                <asp:BoundField DataField="RevisionDateFormatted" HeaderText="Revision Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--10--%>
                                                                <asp:BoundField DataField="CAStatusName" HeaderText="Status">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>

                                                                <%--11--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>

                                                                                        <asp:PlaceHolder ID="plCAlist" runat="server">

                                                                                            <td>
                                                                                                <asp:ImageButton ID="EditView" runat="server" CommandName="EditRec" ToolTip="Click to edit"
                                                                                                    Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                            </td>
                                                                                        </asp:PlaceHolder>
                                                                                        <asp:PlaceHolder ID="phDeleteRecord" runat="server" Visible='<%# IIf(Eval("CAStatusID") > 1, False, True) %>'>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRec" ToolTip="Click to delete" Visible='<%# IIf(Eval("CAStatusID") > 1, False, True) %>'
                                                                                                    Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                            </td>
                                                                                        </asp:PlaceHolder>
                                                                                        <asp:PlaceHolder ID="phIDRenew" runat="server" Visible='<%#  Eval("VisibleTrueFalse")%>'>

                                                                                            <td>
                                                                                                <asp:ImageButton ID="IDRenew" runat="server" ToolTip="Click to renew"
                                                                                                    CommandName="Renew" Style="width: 20px" ImageUrl="images/Renew1.png" Visible='<%#  Eval("VisibleTrueFalse")%>' />
                                                                                            </td>
                                                                                        </asp:PlaceHolder>
                                                                                        <asp:PlaceHolder ID="phIDHistory" runat="server" Visible='<%#  Eval("HistoryCount")%>'>

                                                                                            <td>
                                                                                                <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                    CommandName="History" ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>' />
                                                                                            </td>
                                                                                        </asp:PlaceHolder>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer;" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--12--%>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>
                                                                <%--13--%>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="VisibleTrueFalse" HeaderText="VisibleTrueFalse"></asp:BoundField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="CAStatusID" HeaderText="CAStatusID"></asp:BoundField>
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
        <!--EmpCAAuthorization History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpCAAuthorizationHistory" Text="EmpCAAuthorizationHistory"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpCAAuthorizationHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpCAAuthorizationHistory" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpCAAuthorizationHistory" runat="server" TargetControlID="btnDummyEmpCAAuthorizationHistory"
            PopupControlID="pnlEmpCAAuthorizationHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpCAAuthorizationHistoryStateComplete() {
                $("#btnDummyEmpCAAuthorizationHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }
            function OpenEmpCAAuthorizationHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpCAAuthorizationHistory").attr("src", "wfEmpCAAuthorizationHistory_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpCAAuthorizationHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForEmpCAAuthorizationHistory() {
                var EmpCAAuthorizationHistorywindow = $find("<%=mdlPopupEmpCAAuthorizationHistory.ClientID %>");
                //close EmpCAAuthorization History popup window
                EmpCAAuthorizationHistorywindow.hide();
                //           release resources
                $("#IframeEmpCAAuthorizationHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpCAAuthorizationHistory").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
