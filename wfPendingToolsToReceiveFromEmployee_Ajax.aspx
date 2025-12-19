<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingToolsToReceiveFromEmployee_Ajax.aspx.vb"
	Inherits="Flypal.PendingToolsToReceiveFromEmployeeDetailPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part List For Goods Receipt</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script type="text/javascript">
        function FireOnClickButton(e) {
            if (e.keyCode == 13 || e.keyCode == 9) {
                document.getElementById("btnAddBarcodeItem").click();
            }
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="wfgroup" method="post" runat="server">
    <script type="text/javascript" language="javascript">
        function CheckAllEmp(Checkbox) {
            var dgPartList = document.getElementById("<%=dgPartList.ClientID %>");
            for (i = 1; i < dgPartList.rows.length; i++) {
                dgPartList.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).closest('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    trParent.addClass('clslightColor')
                else
                    trParent.removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).prop('checked');
                if (checked == true)
                    $(element).closest('tr').addClass('clslightColor');
                else
                    $(element).closest('tr').removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });
        });
    </script>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
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
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblPartList" class="clsFormHeader">Tools List</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAdd" runat="server" class="clsbtnH clsinfoH" ToolTip="Click to add Items"
                                                        CausesValidation="true" Text="Add" ValidationGroup="a"></asp:Button>
                                                    <asp:Button ID="btnClose" runat="server" class="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                        Text="Back"></asp:Button>
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
                                        <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="a"></asp:ValidationSummary>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span1" class="clsLabelAuto">Issue To Employee</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIssuedToEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
														AutoPostBack="true" CssClass="clsTextBoxTagSearch"
														OnTextChanged="IssuedToEmployeeChanged"></asp:TextBox>
                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtIssuedToEmployee_Autocomplete"
                                                        runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                        MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfPendingToolsToReceiveFromEmployee_Ajax.aspx"
                                                        ServiceMethod="GetEmployeeList" TargetControlID="txtIssuedToEmployee" OnClientItemSelected="SetID"
                                                        UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                    </cc2:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnIssuedToEmployeeId" runat="server" ClientIDMode="Static" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"
														AutoPostBack="true" OnTextChanged="DateChanged" onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender runat="server" ID="txtDateCalendarExtender" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                    </cc2:CalendarExtender>
                                                </td>
                                                <td>
                                                    <span id="lblSearch" class="clsLabelAuto">Part No</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part Number"
                                                        AutoPostBack="true" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Search the Record"
                                                        Visible="false" Text="Find Now"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCodeNo" runat="server" CssClass="clsLabelAuto">GSE No</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCodeNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter GSE No."
                                                        AutoPostBack="true" MaxLength="20"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblWorkOrderNo" class="clsLabelAuto" runat="server" Visible="false">Work Order No.</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbWorkOrder" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        DataTextField="WONumber" AutoPostBack="true" DataValueField="ID" Visible="false" >
                                                    </asp:DropDownList>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBarcodeNos" runat="server" CssClass="clsLabelAuto" Visible="false">Barcode No.</asp:Label>
                                                </td>
                                                <td colspan="5">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtBarcodeItem" Visible="false" runat="server" CssClass="clsTextBox_Ajax"
                                                                    onkeydown="javascript:FireOnClickButton(event);"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAddBarcodeItem" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                    Visible="false" ClientIDMode="Static" ToolTip="Click to Add Barcode No" />
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlPartList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        <asp:GridView ID="dgPartList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                            AllowSorting="True" ShowHeaderWhenEmpty="True" CellPadding="5" ForeColor="Black"
                                            GridLines="Horizontal">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <%--0--%>
                                                <asp:BoundField Visible="False" DataField="IssueID" HeaderText="IssueID"></asp:BoundField>
                                                <%--1--%>
                                                <asp:BoundField Visible="False" DataField="IssueItemID" HeaderText="IssueItemID">
                                                </asp:BoundField>
                                                <%--2--%>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" ClientIDMode="Static" CssClass="cbSelectRow" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--3--%>
                                                <asp:BoundField DataField="IssueDateFormatted" SortExpression="IssueDateFormatted"
                                                    HeaderText="Issue Date">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <%--4--%>
                                                <asp:BoundField DataField="IssueTextNo" SortExpression="IssueTextNo" HeaderText="Issue No.">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--5--%>
                                                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <%--6--%>
                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--7--%>
                                                <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Issue For Aircarft">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--8--%>
                                                <asp:BoundField DataField="WONo" SortExpression="WONo" HeaderText="Issue For WO.">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--9--%>
                                                <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--10--%>
                                                <asp:BoundField DataField="IssueToEmployeeName" SortExpression="IssueToEmployeeName"
                                                    HeaderText="Issue To Employee">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--11--%>
                                                <asp:BoundField DataField="CodeNo" SortExpression="CodeNo" HeaderText="Code No.">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--12--%>
                                                <asp:BoundField DataField="CalibrationDoneOnDateFormatted" HeaderText="Calibration Done On Date">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--13--%>
                                                <asp:BoundField DataField="FromStoreWithLocation" HeaderText="Store">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server"  class="clsbtnH clsinfoH1" ToolTip="Click to add Items"
                                                        CausesValidation="true" Text="Add" ValidationGroup="a"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" class="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                        Text="Back"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
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
    <%--
    Autocomplete functions to set id--%>
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

            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtIssuedToEmployee_Autocomplete") {
                textbox = document.getElementById('hdnIssuedToEmployeeId');
            }
            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetEmpIdonChange(cntrl, extender) {
            var cntrlName = '#' + cntrl;
            var popup = $find(extender);
            var complist = popup.get_completionList();
            var text = $(cntrlName).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    if (cntrl == "txtIssuedToEmployee") {
                        textbox = document.getElementById('hdnIssuedToEmployeeId');
                    }
                    textbox.value = val.toString();
                    return;
                }
            }
            if (cntrl == "txtIssuedToEmployee") {
                textbox = document.getElementById('hdnIssuedToEmployeeId');
            }
            textbox.value = '';
            return;
        }
    </script>
    </form>
</body>
</html>
