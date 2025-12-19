<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNRCList_Ajax.aspx.vb"
    Inherits="Flypal.wfNRCList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>NRC List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
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
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clsFormHeader">NRC List</asp:Label>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table2" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <span id="lblMaintenanceOn" class="clsLabel" style="display: none">Maintenance On</span>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                                    ValidationGroup="1" Text="Add New" ToolTip="Click to Add NRC" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                    TabIndex="0" Text="Close" ToolTip="Click to close List of NRC screen" />
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
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Width="78px">Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                    <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                    <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                    <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                    <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                    <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Width="78px">From Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
                                                                    CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                    AutoPostBack="True"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="TextBoxWatermarkExtender1"
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
                                                                <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
                                                                    CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                    AutoPostBack="True"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="TextBoxWatermarkExtender2"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
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
                        <tr >
                            <td>
                                <asp:Panel ID="ClpnlAdvancedSearch" runat="server" Style="border: none; width: 100%" class="clsCollapsePnl">
                                    <div>
                                        <div style="float: left; vertical-align: middle; width: 100%">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span style="vertical-align: middle; margin-left: 2px; width: 100%" id="lblMastersSelection"
                                                            class="clsLabelHeader">Advance Search</span>
                                                    </td>
                                                    <td align="right">
                                                        <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                            <image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Panel ID="pnlAdvancedSearch" runat="server" Style="max-height: 200px; overflow-y: auto;
                                    overflow: auto; overflow-x: hidden;">
                                    <asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblRegNo" class="clsLabelAuto">Reg No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Reg No."
                                                            MaxLength="50" AutoPostBack="True"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Serial Number"
                                                            MaxLength="50" AutoPostBack="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblATA" class="clsLabel">ATA</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                            AutoPostBack="true" DataValueField="ID" DataTextField="ATAChapter">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <%-- <span id="lblModel" class="clsLabelAuto">Model</span>--%>
                                                    </td>
                                                    <td>
                                                        <%--<asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                            AutoPostBack="true" DataTextField="ModelName">
                                                        </asp:DropDownList>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <%--<td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table2" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblMaintenanceOn" class="clsLabel" style="display: none">Maintenance On</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                                            ValidationGroup="1" Text="Add New" ToolTip="Click to Add NRC" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close List of NRC screen" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgNRCList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                    PageSize="25" ShowHeaderWhenEmpty="true" EnableViewState="True" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                                    AllowPaging="True">
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                        <asp:BoundField DataField="NRCDateFormatted" HeaderText="Date">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="NRCNumber" HeaderText="Number" SortExpression="NRCNumber">
                                                                            <HeaderStyle  HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg. No." SortExpression="RegNo">
                                                                            <HeaderStyle  HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ATAChapter" HeaderText="ATA" SortExpression="ATAChapter">
                                                                            <HeaderStyle  HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RaisedByEmpName" HeaderText="Raised By" SortExpression="RaisedByEmpName">
                                                                            <HeaderStyle  HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneOnDateFormatted" HeaderText="Done On Date">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>

                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <%-- <span id="button">Login</span>--%>
                                                                                <div class="dropdown">
                                                                                    <div class="dropdownbtn-content">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                        CausesValidation="false" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                                                        Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                                                </td>

                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>





                                                                    </Columns>
                                                                </asp:GridView>
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
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlBottomActionButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBottomAdd" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        ValidationGroup="1" Text="Add New" ToolTip="Click to Add NRC" Visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBottomClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close List of NRC List screen" Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnNRC" runat="server" CausesValidation="false" ClientIDMode="Static"
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
    <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
        ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
        CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
        CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
        ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
        SuppressPostBack="false" />
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
    </form>
</body>
</html>
