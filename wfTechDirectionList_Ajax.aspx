<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTechDirectionList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfTechDirectionList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Technical Direction List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame12() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblTitle" class="clsFormHeader">Technical Direction List</span>
                                        </td>
                                        <td colspan="2" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Technical Direction"
                                                                    Text="Print" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Technical Direction List Screen"
                                                                    Text="Close" CausesValidation="False"></asp:Button>
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
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                    ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                    ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                <script type="text/javascript">
                                    function showTextField(SearchIndex) {

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
                                        <table>
                                            <tr>
                                                <td style="width: 69px">
                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel" Height="8px" Width="55px">Search</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                        <asp:ListItem Value="1">Date</asp:ListItem>
                                                        <asp:ListItem Value="2">TD No.</asp:ListItem>
                                                        <asp:ListItem Value="3">Part No.</asp:ListItem>
                                                        <asp:ListItem Value="4">Serial No.</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="L1" runat="server" CssClass="clsLabel" Width="20px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                        Visible="False" onchange="showTextField(this.selectedIndex);">
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                        <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                        <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                        <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="cmbTDText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                        Visible="False" DataValueField="Text" DataTextField="Text">
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="middle">
                                                    <asp:Label ID="lblNo" runat="server" CssClass="clsLabel" Height="8px" Width="32px"
                                                        Visible="False">No.</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax " Width="184px"
                                                        Visible="False" ToolTip="Enter Number" MaxLength="4" AutoPostBack="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                        ToolTip="Enter Text" AutoPostBack="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server" Width="78px">From Date</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true" 
                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');" 
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Width="68px">To Date </asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxTagSearchDate"
                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true" AutoPostBack="True"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 69px">
                                                    <asp:Label ID="lblIntExt" runat="server" CssClass="clsLabel" Height="8px" 
                                                        Width="55px">Int/Ext TD</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbIntExt" runat="server" AutoPostBack="True" 
                                                        CssClass="clsTextBoxTagSearchComboNewstyle" ClientIDMode="Static"   >
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Internal</asp:ListItem>
                                                        <asp:ListItem Value="2">External</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td valign="middle">
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td align="left">
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td align="left">
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblInfo" class="clsLabelAuto">Select Technical direction from the list. Click
                                    On Edit Link To Modify The Selected Technical direction.</span>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Technical Direction(s) as  per searching criteria"
                                            Text="Find Now" Visible="False"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Technical Direction"
                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Technical Direction List Screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Technical Direction(s) as per criteria :  Record(s) found.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgTechnicalDirectionList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="25" AllowSorting="True" DataKeyNames="StatusID,TypeID,MachineID"
                                                        ShowHeaderWhenEmpty="true" OnRowDataBound="dgTechnicalDirectionList_RowDataBound">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="StatusID" HeaderText="StatusID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="TypeID" HeaderText="TypeID"></asp:BoundField>
                                                            <asp:BoundField DataField="TDDateFormatted" HeaderText="Date">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TDNo" SortExpression="TDNo" HeaderText="TD No.">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Is External">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="True"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center" Wrap="True"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsNoteRequired") %>'
                                                                        Enabled="False"></asp:CheckBox></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Type" SortExpression="Type" HeaderText="Type">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PartDescription" SortExpression="PartDescription" HeaderText="Description">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CompSerialNo" SortExpression="CompSerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ToAddress" SortExpression="ToAddress" HeaderText="Send To">
                                                                <HeaderStyle Wrap="true"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="200px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WorkRequired" SortExpression="WorkRequired" HeaderText="Work Detail">
                                                                <HeaderStyle Wrap="true"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="200px"></ItemStyle>
                                                            </asp:BoundField>
                                                               <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                <HeaderStyle Wrap="true"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="200px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" Visible="false">
                                                            </asp:ButtonField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditViewRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" Visible="false"/>

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
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Technical Direction"
                                                        Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Technical Direction List Screen"
                                                        Text="Close" CausesValidation="False" Visible="false"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
            var selectedSearchIndex = $get("cmbSearch").selectedIndex;


            if (selectedSearchIndex == 1) {
                var selectedDateIndex = $get("cmbDate").selectedIndex;
                if (selectedDateIndex == 6) {
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            var dd = $get("cmbSearch").selectedIndex;
            showTextField(dd);
        }    
    </script>
    </form>
</body>
</html>
