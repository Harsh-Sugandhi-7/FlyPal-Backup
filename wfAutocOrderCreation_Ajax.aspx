<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAutocOrderCreation_Ajax.aspx.vb"
    Inherits="Flypal.wfAutocOrderCreation_Ajax" %>

<%@ Import Namespace="FlyPal" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Create New Order</title>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
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
                            <td class="clsFormHeader1Newstyle">
                                <span id="lblPendingReceiptItemListTitle" class="clsFormHeader">Create New Order or
                                    Add Item to the Existing Order</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvFactor" runat="server" ControlToValidate="txtConversionFactor"
                                            CssClass="clsLabel" Display="None" ErrorMessage="Currency factor must be greater than zero"
                                            OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOrderDate" runat="server" ErrorMessage="Select Order Date"
                                            ControlToValidate="txtOrderDate" Display="None" OnServerValidate="CustomValidate"
                                            CssClass="clsLabel1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvQty" runat="server" ErrorMessage="Enter Quantity for Order_Item"
                                            ControlToValidate="txtQty" Display="None" OnServerValidate="CustomValidate" CssClass="clsLabel1"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvQty" runat="server" ErrorMessage="Quantity Required"
                                            ControlToValidate="txtQty" Display="None" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvVendor" runat="server" ControlToValidate="cmbVendor" CssClass="clsLabel"
                                            Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCurrency" runat="server" ControlToValidate="cmbCurrencyList"
                                            CssClass="clsLabel" Display="None" ErrorMessage="Select Currency from the list."
                                            OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOrder" runat="server" ControlToValidate="cmbOrderList"
                                            CssClass="clsLabel" Display="None" ErrorMessage="Select Order From List" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblInfo" class="clsLabelAuto">Select Item from the list to create order or
                                    add in the existing order.</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblSearch" class="clsLabelAuto">Part No</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part Number"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                        CausesValidation="False"></asp:Button>--%>
                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                        Text="Find Now" CausesValidation="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlPendingReceiptItemList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblListOfPendingReceipt" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Items:  Record(s) Found</asp:Label>
                                        <asp:GridView ID="dgPendingReceiptItemList" runat="server" CssClass="clsGridNewStyle"
                                            AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                                            ForeColor="Black" GridLines="Horizontal" CellPadding="5">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part No.">
                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Part Description">
                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Label ID="lblCreate" runat="server" CssClass="clsLabelHeader"> Create / Edit order for the above Part </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlOrderDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="4">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:RadioButton ID="optNewOrder" runat="server" CssClass="clsRadioButton" Text="Create New Order"
                                                        GroupName="grOrder" AutoPostBack="True"></asp:RadioButton>
                                                </td>
                                                <td colspan="2">
                                                    <asp:RadioButton ID="optExistingOrder" runat="server" CssClass="clsRadioButton" Text="Add Into Existing Order"
                                                        GroupName="grOrder" AutoPostBack="True"></asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblOrderdate" runat="server" CssClass="clsLabel">Order Date</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtOrderDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                        onchange="ValidateDateText(this,'Date_watermarkextender','true');" Text="" ></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOrderDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="txtOrderDateWatermarkExtender" runat="server" TargetControlID="txtOrderDate"
                                                        WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblQty" class="clsLabel">Qty.</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtQty" runat="server" ToolTip="Enter Quantity" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                        MaxLength="4" Width="60px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVendor" runat="server" CssClass="clsLabel">Supplier</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="cmbVendor" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
                                                        DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCurrency" runat="server" CssClass="clsLabel">Currency</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        AutoPostBack="True" DataTextField="Name" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblConvFactor" runat="server" CssClass="clsLabel">C Factor</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConversionFactor" runat="server" ToolTip="Enter Conversion Factor"
                                                        CssClass="clsTextBoxTagSearch" MaxLength="9"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblIntrd" runat="server" class="clsLabel">Int. Order No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInternelOrdNo" runat="server" ToolTip="Enter Internal Order No."
                                                        CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <%--<asp:Button ID="btnFindNow1" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Search the Record"
                                                        CausesValidation="False" Text="Find Now"></asp:Button>--%>
                                                    <%-- <asp:ImageButton ID="btnFindNow1" runat="server" ImageUrl="~/images/Search2.png"
                                                        CssClass="clsSearch2btn" />--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblOrder" runat="server" CssClass="clsLabel">Order No.</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="cmbOrderList" runat="server" Visible="False" CssClass="clsTextBoxTagSearchComboSmall"
                                                        DataTextField="OrderNo" DataValueField="ID">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtOrderList" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                        CssClass="clsTextBoxTagSearch" onChange="SetPartIdonChange()"></asp:TextBox>
                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                        CompletionInterval="1" ServicePath="wfAutocOrderCreation_Ajax.aspx" ServiceMethod="GetOrderList"
                                                        TargetControlID="txtOrderList" OnClientItemSelected="SetID" UseContextKey="False"
                                                        ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                        CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                        OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                        OnClientShowing="ClientShowing">
                                                    </cc2:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back"
                                                        ToolTip="Click To Go Back To Previous Page" CausesValidation="False"></asp:Button>
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
    <asp:HiddenField ID="hdnOrderId" runat="server" ClientIDMode="Static" />
    <%--   Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }

            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml; //Boolean Expression ? First Statement : Second Statement Ternary operator ?:
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtSearch_Autocomplete") {
                textbox = document.getElementById('hdnOrderId');
            }

            textbox.value = value.toString();
        }

        function SetPartIdonChange() {
            var popup = $find("txtSearch_Autocomplete");
            var complist = popup.get_completionList();
            var text = $("#txtOrderList").val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    var textbox = document.getElementById('hdnOrderId');
                    textbox.value = val.toString();
                    return;
                }

            }
            //alert(document.getElementById('hdnOrderId').value);
            //document.getElementById('hdnOrderId').value = '';
        }
    </script>
    <%--ReleaseNote No autocomplete--%>
    <script type="text/javascript">
        function GetPartID() {
            var partid = document.getElementById('hdnOrderId').value.toString();
            if (partid) {
                return partid;
            }
            else {
                return '{00000000-0000-0000-0000-000000000000}';
            }

        }
        function SetContextKeyForRelNoteNo() {
            var autoComplete = $find('txtRelNoteNo_AutoComplete');
            var str = 'PartID=' + GetPartID();
            autoComplete.set_contextKey(str);
        }
        function SetContextKeyForSerialNo() {
            var autoComplete = $find('txtSerialNo_AutoCompleteExtender');
            var str = 'PartID=' + GetPartID();
            autoComplete.set_contextKey(str);
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
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
           var ddCustomer = document.getElementById("cmbVendor");
            if  (ddCustomer != null) {
             if  (ddCustomer.disabled ==false)
             {
              var j = 0;
              <% For Each item2 In mVendorList%>
                <% If  item2.NotInUse ="True" Then%>
                ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                j = j + 1;
             <% Next%>
             }
             }
            });    
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>
