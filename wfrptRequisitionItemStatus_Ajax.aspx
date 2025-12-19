<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptRequisitionItemStatus_Ajax.aspx.vb"
    Inherits="Flypal.wfrptRequisitionItemStatus_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Requistion Item Status</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/close.gif";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/detail.gif";
            }
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
                                <td class="clsFormHeader1" colspan="2" nowrap>
                                    <span class="clsFormHeader" id="LblTitle">Requisition Item Status </span>
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
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblFromDate" class="clsLabel">From Date </span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtFromDate"
                                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                        ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be grater than To Date "></asp:CustomValidator>
                                                                </td>
                                                                <td align="right">
                                                                    <span id="lblToDate" class="clsLabel">To Date </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtToDate"
                                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabel">Search</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSearch" runat="server" AutoPostBack="True">
                                                                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                                        <asp:ListItem Value="1">Requisition</asp:ListItem>
                                                                        <asp:ListItem Value="2">Part No</asp:ListItem>
                                                                        <asp:ListItem Value="3">Type</asp:ListItem>
                                                                        <asp:ListItem Value="4">Order</asp:ListItem>
                                                                        <asp:ListItem Value="5">Receipt</asp:ListItem>
                                                                        <asp:ListItem Value="6">Issue</asp:ListItem>
                                                                        <asp:ListItem Value="7">Issued</asp:ListItem>
                                                                        <asp:ListItem Value="8">Not Issued</asp:ListItem>
                                                                        <asp:ListItem Value="9">Priority</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbReqType" runat="server"
                                                                        AutoPostBack="true" Visible="False">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="65">Engineering Requisition</asp:ListItem>
                                                                        <asp:ListItem Value="71">Stores Requsition</asp:ListItem>
                                                                        <asp:ListItem Value="72">WorkShop Requsition</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbRequisitionText" runat="server"
                                                                        AutoPostBack="True" Visible="False" DataTextField="Text" DataValueField="Text">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbOrderText" runat="server" AutoPostBack="True"
                                                                        Visible="False" DataTextField="Text" DataValueField="Text">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbReceipText" runat="server"
                                                                        AutoPostBack="True" Visible="False" DataTextField="Text" DataValueField="Text">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbIssueText" runat="server" AutoPostBack="True"
                                                                        Visible="False" DataTextField="Text" DataValueField="Text">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                        MaxLength="50"></asp:TextBox>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall1" ID="cmbPriority" runat="server" AutoPostBack="true"
                                                                        DataTextField="Name" DataValueField="ID" Visible="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label class="clsLabel" ID="lblBranch" runat="server" Visible="false">Branch</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbRequisitionEngineeringBranches" runat="server"
                                                                        DataTextField="Branch" DataValueField="ID" Visible="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label CssClass="clsLabelAuto" ID="lblNo" runat="server" Width="24px" Visible="False">No.</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtNo" runat="server" Visible="False"
                                                                        MaxLength="8"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span1" class="clsLabelAuto">Requisition Type</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall1" ID="cmbRequisitionType" runat="server"
                                                                        Width="160px" AutoPostBack="true" Visible="true">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="1">Part Request</asp:ListItem>
                                                                        <asp:ListItem Value="2">Part Purchase</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblFormat" class="clsLabelAuto">Format</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall1" ID="cmbFormat" runat="server" AutoPostBack="true">
                                                                        <asp:ListItem Value="0">Format 1</asp:ListItem>
                                                                        <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox ID="chkShowPPReqOnly" runat="server" CssClass="clsCheckBox" Text="Show Part Purchase Transaction Only"
                                                                        Visible='<%# AppSettings("ClientCode") = "BA" %>' />
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Requisition Item Status List as per searching criteria"
                                            Text="Find Now" ValidationGroup="a"></asp:Button>--%>
                                            <asp:ImageButton CssClass="clsSearch2btn" ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" ToolTip="Click to find records as per criteria." />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto " Font-Bold="True">List of Requisition Item as per criteria : Record(s) found</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button CssClass="clsbtnH" ID="btnPrintTop" runat="server" CausesValidation="False"
                                                                                Text="Print" ToolTip="Click to Print list of Requisition" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button CssClass="clsbtnH" ID="btnExportToExcelTop" runat="server" Text="Export to Excel"
                                                                                ToolTip="Click to Export to Excel" Width="100px" Visible="false" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button CssClass="clsbtnH" ID="btnCloseTop" runat="server" CausesValidation="False"
                                                                                Text="Close" ToolTip="Click to Close list of Requisition" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal" ID="dgRequisitionItemList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            PageSize="25" ShowHeaderWhenEmpty="true" SelectedRowStyle-BackColor="ButtonShadow"
                                                            DataKeyNames="ReqID,ReqItemID">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <a href="javascript:showNestedGridView('ID-<%# Eval("ReqItemID") %>');">
                                                                            <img id="imageID-<%# Eval("ReqItemID") %>" alt="Click to show/hide details" border="0"
                                                                                src="images/detail.gif" />
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ReqID" HeaderText="ReqID" Visible="False" />
                                                                <asp:BoundField DataField="ReqItemID" HeaderText="ReqItemID" Visible="False" />
                                                                <asp:BoundField DataField="DateFormatted" HeaderText="Req. Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="false" HorizontalAlign="Left"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RequisitionTextNo" HeaderText="Requisition No." SortExpression="RequisitionTextNo">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TransTypeName" HeaderText="Requisition" SortExpression="TransTypeName">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="false" HorizontalAlign="Left"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EmployeeName" HeaderText="Requested By" SortExpression="EmployeeName">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="false" HorizontalAlign="Left"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartNo" HeaderText="Part No." SortExpression="PartNo">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartDescription" HeaderText="Part Description" SortExpression="PartDescription">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReqQty" HeaderText="Req. Qty.">
                                                                    <HeaderStyle  HorizontalAlign="Right"  Wrap="false"/>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td colspan="100%" bgcolor="White" width="0px">
                                                                                <div id="ID-<%# Eval("ReqItemID") %>" style="display: none; position: relative; left: 25px;">
                                                                                    <asp:GridView ID="dgTransactionDetails" runat="server" AutoGenerateColumns="False"
                                                                                        OnRowCommand="dgTransactionDetails_RowCommand" Width="60%" BorderStyle="Solid"
                                                                                        CellPadding="0" ForeColor="#333333" CssClass="clsGridLog" AlternatingRowStyle-CssClass="alt"
                                                                                        RowStyle-Wrap="false" HeaderStyle-Wrap="false" SelectedRowStyle-BackColor="ButtonShadow"
                                                                                        DataKeyNames="ID,Type,InvoiceID" ShowHeaderWhenEmpty="True" PageSize="5">
                                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="false"></asp:BoundField>
                                                                                            <asp:BoundField DataField="Type" HeaderText="Type" Visible="false"></asp:BoundField>
                                                                                            <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" Visible="false"></asp:BoundField>
                                                                                            <asp:BoundField DataField="TypeName" HeaderText="Transaction">
                                                                                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" Height="22px" />
                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="left" Width="100px" />
                                                                                            </asp:BoundField>
                                                                                            <asp:ButtonField DataTextField="TranasactionNo" HeaderText="No." CommandName="TranasactionNo">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                            </asp:ButtonField>
                                                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="80px" />
                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="left" Width="80px" />
                                                                                            </asp:BoundField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="BtnPrint" runat="server" CausesValidation="False"
                                                            Text="Print" ToolTip="Click to Print Requisition Item Status List" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnExportToExcelBottom" runat="server"
                                                            Text="Export to Excel" ToolTip="Click to Export to Excel" Width="100px" Visible="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" CausesValidation="False"
                                                            Text="Close" ToolTip="Click to Close Requisition Item Status List" />
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
        <%--End --%>
    </form>
</body>
</html>
