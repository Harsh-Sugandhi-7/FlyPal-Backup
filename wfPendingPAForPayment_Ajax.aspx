<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingPAForPayment_Ajax.aspx.vb" Inherits="Flypal.wfPendingPAForPayment_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
   <title>Pending PA For Payment</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <%-- <link href="Styles.css" id="Link1" type="text/css" rel="stylesheet" />--%>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnltitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">List of Pending PA For Payment</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                   </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDate" runat="server" CssClass="clsLabel" Width="78px" Text="Date" />
                                                        <%--onchange="Disablecontrols();"--%>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="true">
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
                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="85px"
                                                        CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
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
                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="85px"
                                                        CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
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
                            <td align="right" valign="top">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" 
                                                        ClientIDMode="Static" Text="Find Now" ToolTip="Click to Find records" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr class="clsCollapsePanel">
                            <td style="width: 100%" colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="ClpnlAdvancedSearch" runat="server" Style="border: none; width: 100%">
                                            <div>
                                                <div style="float: left; vertical-align: middle; width: 100%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <span style="vertical-align: middle; margin-left: 2px; width: 100%" id="lblMastersSelection"
                                                                    class="clsLabelHeader">Advance Search</span></td>
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlAdvancedSearch" runat="server"  Style="max-height: 200px;overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPaymentAdviceNo" runat="server" CssClass="clsLabel">Payment Advice No</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlPaymentAdvice" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbPaymentAdviceText" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="true"
                                                                    DataTextField="Text" DataValueField="Text">
                                                                 </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlPaymentAdviceNo" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="7" 
                                                                    onchange="setattr(this);" ToolTip="Enter Payment Advice Number">0</asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <span id="Span2" class="clsLabelAuto">Supplier</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSupplier" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                            DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblOrderNo" class="clsLabelAuto">Order No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Text" AutoPostBack="true"
                                                            DataValueField="Text">
                                                         </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOrderNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="7"
                                                            onchange="setattr(this);" ToolTip="Enter Order Number">0</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span3" class="clsLabelAuto">Supplier Invoice #</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSupplierInvoiceNo" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                            MaxLength="7" onchange="setattr(this);" ToolTip="Enter Supplier Invoice #">0</asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                   <%-- <td>
                                                        <span id="lblStatus" class="clsLabel">Status</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsComboBox1_Ajax">
                                                                    <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Opened</asp:ListItem>
                                                                    <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
                                            ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
                                            CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
                                            CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                            ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                            SuppressPostBack="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlgrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Pending PA For Payments per criteria : Record(s) found</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table2" border="0" cellspacing="0">
                                                                <tr>
                                                                <td>
                                                                  <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                            Text="Print" ToolTip="Click to Print Reminder Report" />
                                                                  </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close List of Payment Advice screen" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnldgPaymentAdvice" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="dgPaymentAdviceList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                            PageSize="25" ShowHeaderWhenEmpty="True" CssClass="clsGrid" AllowPaging="True">
                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                            <RowStyle CssClass="clsdgItem" />
                                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                                               <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                                <asp:BoundField DataField="PaymentAdviceDateFormatted" HeaderText="Date">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="false" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="PaymentTextNo" HeaderText="Number" SortExpression="PaymentNo">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="false" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="PaymentTo" HeaderText="To" SortExpression="PaymentTo">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="True" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="PaymentFrom" HeaderText="From" SortExpression="PaymentFrom">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="VendorName" HeaderText="Supplier" SortExpression="VendorName">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="ModeOfPaymentName" HeaderText="Mode Of Payment" SortExpression="ModeOfPaymentName">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                 <asp:BoundField DataField="CTotalAmount" HeaderText="Total Amount" SortExpression="CTotalAmount"  HeaderStyle-HorizontalAlign="Right"
                                                                                ItemStyle-HorizontalAlign="Right">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"/>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                
                                                                            </Columns>
                                                                             <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
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
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlBottomActionButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7" border="0" cellspacing="0">
                                            <tr>
                                                 <td>
                                                    <asp:Button ID="btnBottomClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax" Visible="false"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close List of Payment Advice List screen" 
                                                         />
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
                                        <asp:Button ID="hdnBtnPaymentAdviceSerialNo" runat="server" CausesValidation="false"
                                            ClientIDMode="Static" Style="display: none;" Text="Add" />
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

        function setattr(elem) {
            var No = $(elem).val();
            if ($(elem).val() == "") {
                $(elem).val('0');
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
        $(document).keydown(function (e) {
            if (e.which == 13) {
                $("input[id=btnFindNow]").click();
            }
        });
    </script>
    <%--End--%>
    </form>
</body>
</html>
