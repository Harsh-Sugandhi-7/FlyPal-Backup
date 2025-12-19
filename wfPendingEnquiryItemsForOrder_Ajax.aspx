<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingEnquiryItemsForOrder_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingEnquiryItemsForOrder_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>List of Pending Enquiry Items</title>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">List of Pending Enquiries</span>
                                        </td>
                                        <td align="right" colspan="2">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click To Go Back To Previous Page">
                                                    </asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransactionDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                        OnTextChanged="txtTransactionDate_TextChanged" AutoPostBack="true" onchange="ValidateDateText(this,'TransactionDate_watermarkextender');"
                                                        Text="" Width="100px"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtTransactionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtTransactionDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="TransactionDate_watermarkextender" runat="server"
                                                        TargetControlID="txtTransactionDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <span id="Span1" class="clsLabelAuto">Enquiry No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                        ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txtText_TextChanged"
                                                        ToolTip="Enter Enquiry text">
                                                    </asp:TextBox>
                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" CompletionInterval="1"
                                                        ServicePath="wfPendingEnquiryItemsForOrder_Ajax.aspx" ServiceMethod="GetTextList"
                                                        TargetControlID="txtText" UseContextKey="false" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                    </cc2:AutoCompleteExtender>
                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                        Width="40px" ClientIDMode="Static" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblSearch" class="clsLabelAuto">Part Name</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartName" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                        ClientIDMode="Static" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="Span2" class="clsLabelAuto">Vendor</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVendorName" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                        ClientIDMode="Static" MaxLength="100"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlEnquiryItemList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblEnqItemResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        <asp:GridView ID="dgEnquiryItemList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                            ClientIDMode="Static" PageSize="10" DataKeyNames="EnquiryID,EnquiryItemID,ItemID"
                                            AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" GridLines="Horizontal"
                                            CellPadding="5">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField Visible="false" DataField="EnquiryID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField Visible="false" DataField="EnquiryItemID" HeaderText="EnquiryItemID">
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ItemID"></asp:BoundField>
                                                <asp:BoundField DataField="PartNoDescription" HeaderText="Part Name </br> Description"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EnquiryDateFormatted" HeaderText="Date">
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EnquiryTextNo" HeaderText="Number">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor" HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRecord">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"   />
                                                    <ItemStyle ForeColor="Blue" Wrap="False" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upnlQuoteItems" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResultQuoteItem" runat="server" CssClass="clsLabelHeader" Visible="false">All Quotations</asp:Label>
                                        <asp:GridView ID="dgQuoteItems" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="QuotationDateFormatted" HeaderText="Date">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="QuotationNo" HeaderText="Number">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VendorName" HeaderText="Supplier"></asp:BoundField>
                                                <asp:BoundField DataField="Currency" HeaderText="Currency"></asp:BoundField>
                                                <asp:BoundField DataField="ConversionFactor" HeaderText="Conv. Factor">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CRate" HeaderText="Rate" DataFormatString="{0:#00.00}">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PriorityName" HeaderText="Priority"></asp:BoundField>
                                                <asp:BoundField DataField="DeliveryInDays" HeaderText="Del. In Days">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PaymentTerm" HeaderText="Payment Term"></asp:BoundField>
                                                <asp:BoundField DataField="Note" HeaderText="Note"></asp:BoundField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("QuotationID") %>'
                                                            CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                            Visible='<%#  Eval("size") > 0 %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="size" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRecord">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"   />
                                                    <ItemStyle ForeColor="Blue" Wrap="False" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upnlInvoiceItemList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResultInvItem" runat="server" CssClass="clsLabelHeader" Visible="false">Last 10 Purchases</asp:Label>
                                        <asp:GridView ID="dgInvoiceItemList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="true" PageSize="3" GridLines="Horizontal" CellPadding="5">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Invoice Date">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice No.">
                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OrderDateFormatted" HeaderText="Order Date">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OrderNumber" HeaderText="Order No.">
                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VendorName" HeaderText="Supplier"></asp:BoundField>
                                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency"></asp:BoundField>
                                                <asp:BoundField DataField="ConversionFactor" HeaderText="Conv. Factor">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReleaseNoteNo" HeaderText="Release Note No.">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CCommercialRate" HeaderText="Commercial Rate">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
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
    <%-- Row Highlight--%>
    <script type="text/javascript">
        //event handler for end request i.e last event in client page cycle.
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        //event handler for begin request i.e before sending request to the server
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

        var element;
        var timerId;
        var timeoutforblink;
        var hideRowHighlight = false;

        function endRequestHandler(sender, args) {
            var tempval = parseInt($("#gridrowindex").val()); //row number ..0 is header row..
            if (tempval) {
                $("#dgEnquiryItemList tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
                if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
                    var elem;
                    var tempaction = $("#gridrowaction").val(); //action to be performed

                    //button close of popup windows
                    //remove highlight row class... and return from function
                    if (tempaction == "close") {
                        $("#dgEnquiryItemList tr:eq(" + tempval + ")").removeClass('activerow');
                        $("#gridrowaction").val('');
                        return;
                    }
                    //change Rate button ok event
                    //blink Rate column of the row for perticular interval
                    else if (tempaction == "rate") {
                        $("#dgEnquiryItemList tr:eq(" + tempval + ")").removeClass('activerow');
                        elem = $("#dgEnquiryItemList tr:eq(" + tempval + ") td:eq(5)");
                        $("#gridrowaction").val('');
                    }

                    else {
                        return;
                    }
                    //blink column function
                    timeoutforblink = setInterval(function () {

                        if (elem.hasClass('activerow')) {
                            elem.removeClass('activerow');
                        }
                        else {
                            elem.addClass('activerow');
                        }

                    }, 500);
                    //stop blink column
                    timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);
                }


            }
        }

        function BeginRequestHandler(sender, args) {
            clearTimeout(timerId);
            element = args.get_postBackElement();
            //change location popup ok button event occur
            if (element.id == "txtTransactionDate" || element.id == "txtVendorName" || element.id == "txtPartName" || element.id == "txtNo" || element.id == "txtText") {
                hideRowHighlight = true;
                $("#gridrowaction").val('close');
            }
            //change parttype ||change location link event occur
            //reset rowindex value if other grid event occurs
            else if (element.id == "dgEnquiryItemList") {
                if ($("#gridrowaction").val() != "gridrow") {
                    $("#gridrowindex").val('');
                }
            }
            //any other events
            else {
                $("#gridrowindex").val('');
            }
        }

        //stop blinking
        function TimeOut(val, action) {
            var tempelem;
            if (action == "rate") {
                tempelem = $("#dgEnquiryItemList tr:eq(" + val + ") td:eq(5)");
                tempelem.removeClass('activerow');

            }
            else {
                return;
            }
            $("#gridrowindex").val('');
            hideRowHighlight = false;
            clearInterval(timeoutforblink);
        }
    </script>
    <input id="gridrowindex" type="hidden" value="" />
    <input id="gridrowaction" type="hidden" value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dgEnquiryItemList tr td a").live("click", function () {
                var temp = $(this).parent().parent()[0].rowIndex;
                $("#gridrowindex").val(temp);
                $("#gridrowaction").val('gridrow');
            });
        });
    </script>
    </form>
    <script type="text/javascript">
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
</body>
</html>
