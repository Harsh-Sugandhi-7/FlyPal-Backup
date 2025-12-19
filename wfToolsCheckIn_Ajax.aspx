<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfToolsCheckIn_Ajax.aspx.vb"
    Inherits="Flypal.wfToolsCheckIn_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Tools CheckIn Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function OpenLocation(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="AlertMessage.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
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
    <script type="text/javascript">
        function FireOnClickButton(e) {
            if (e.keyCode == 13 || e.keyCode == 9) {
                document.getElementById("btnAddBarcodeItem").click();
            }
        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clspanel1" runat="server">
                    <table id="tblinner" class="clsTablelistin" border="0">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Tools Details [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                            Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" CssClass="clsValidationSummary"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" Display="None"
                                            ErrorMessage="Remark field length must not be greater than 100 Character" ClientValidationFunction="validateNameLen"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateNameLen(source, args) {
                                                args.IsValid = false;

                                                var nameLength = $get("txtRemark").value.length;
                                                if (nameLength <= 100) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlReceiveDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="tabDetails" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="ldwodetail" runat="server"><b>Tools Receiving Details</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblIssueDate" class="clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtReceiptCumInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'ReceiptCumInvoiceDate_watermarkextender','true');"
                                                                        Text="" ></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="ReceiptCumInvoiceDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReceiptCumInvoiceDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="ReceiptCumInvoiceDate_watermarkextender" runat="server"
                                                                        TargetControlID="txtReceiptCumInvoiceDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblStarIssueNo" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="clsLabelAuto">No.</span>
                                                                </td>
                                                                <td>
                                                                    <table id="Table3" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtInvoiceText" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                                    Text="<%# mReceiptCumInvoice.InvText %>" ToolTip="Enter Text">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="clsTextBoxTagSearch"  Width="60px" MaxLength="4"
                                                                                    Text="<%# mReceiptCumInvoice.InvNo %>" ToolTip="Enter No.">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <span id="Span3" class="clsLabelAuto">Returned By</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtReceivedFromEmp" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtReceivedFromEmp_TextChanged" AutoPostBack="true" Enabled="<%# mReceiptCumInvoice.IsNew %>"
                                                                        CssClass="clsTextBoxTagSearch" onChange="SetEmpIdonChange('txtReceivedFromEmp','txtReceivedFromEmp_Autocomplete')"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtReceivedFromEmp_Autocomplete"
                                                                        runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                        MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList"
                                                                        TargetControlID="txtReceivedFromEmp" OnClientItemSelected="SetID" UseContextKey="False"
                                                                        ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                        CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                        OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                        OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnReceivedFromEmpId" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span5" class="clsLabelAuto">Returned To</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSubmittedByEmp" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtSubmittedByEmp_TextChanged" AutoPostBack="true" Enabled="<%# mReceiptCumInvoice.IsNew %>"
                                                                        CssClass="clsTextBoxTagSearch" onChange="SetEmpIdonChange('txtSubmittedByEmp','txtSubmittedByEmp_Autocomplete')"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtSubmittedByEmp_Autocomplete"
                                                                        runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                        MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList"
                                                                        TargetControlID="txtSubmittedByEmp" OnClientItemSelected="SetID" UseContextKey="False"
                                                                        ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                        CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                        OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                        OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnSubmittedByEmpId" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Remark</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemark" runat="server" Text="<%# mReceiptCumInvoice.Remark %>"
                                                                        CssClass="clsTextBoxTagSearchMultilineNewStyleLong" MaxLength="100" ToolTip="Enter Remark" TextMode="MultiLine"
                                                                        Rows="5">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlRecItem" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" border="0" width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblParts" class="clsLabelHeaderItem">Add Item(s)</span>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAddItem" runat="server" class="clsbtnH clsinfoH1" Text="Add" ToolTip="Click to Add New Tool"
                                                                    Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" ValidationGroup="1"  />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBarcodeNos" runat="server" CssClass="clsLabelAuto" Visible="false">Barcode No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarcodeItem" runat="server" CssClass="clsTextBox_Ajax" Visible="false"
                                                                    onkeydown="javascript:FireOnClickButton(event);" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
                                                                    ToolTip="Enter Barcode No. of item to be added"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAddBarcodeItem" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                    Visible="false" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" ClientIDMode="Static"
                                                                    ToolTip="Click to Add item with Barcode No" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgToolsReceipt" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                                        ShowHeaderWhenEmpty="true" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                       <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="CodeNo" HeaderText="Code No.">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="PartDescription" HeaderText="Description">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="Part" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:TemplateField HeaderText="Receiving Store">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsTextBoxTagSearchComboSmall" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "StoreID") %>'
                                                                        Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" DataSource="<%# mStoreList %>"
                                                                        DataTextField="LocationStore" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="DisplayQty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="Unit" HeaderText="Unit">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:TemplateField HeaderText="Phy. Condition">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtNote" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Note") %>' TextMode="MultiLine"
                                                                        Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" CssClass="clsTextBoxTagSearchMultilineNewStyle"
                                                                         MaxLength="499">
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--9--%>
                                                            <asp:TemplateField HeaderText="Remark">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRCIItemRemark" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Remark") %>' TextMode="MultiLine"
                                                                        Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" CssClass="clsTextBoxTagSearchMultilineNewStyle"
                                                                       MaxLength="499">
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           <%--10--%>
                                                            <asp:BoundField DataField="Location" HeaderText="Location" >
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                        CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                </ItemTemplate>
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
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAuthorized" runat="server" class="clsbtnH clsinfoH1"  Text="Check In"
                                                        Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" ToolTip="Click to Receive Tool"
                                                        ValidationGroup="1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" Text="Print" class="clsbtnH clsinfoH1" Enabled="<%# Not mReceiptCumInvoice.IsNew %>"
                                                        ToolTip="Click to print Tools Check-In" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" class="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to go back to the previous page"  />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlMessBox" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <!-- Alert Message -->
                                        <a class="poplight" href="#?w=450" rel="popup_name"></a>
                                        <div id="popup_name" class="popup_block" align="center">
                                            <div style="width: 400px; height: auto" align="left">
                                                <table border="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="middle" width="12%" align="left">
                                                            <img src="images\alert_icon.png" width="40" height="40">
                                                        </td>
                                                        <td>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblAlertTitle" runat="server" CssClass="clsTitleAlertLabel" ClientIDMode="Static"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <hr />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblAlertMessage" runat="server" CssClass="clsAlertLabel" Width="100%"
                                                                            ClientIDMode="Static"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <!-- End-->
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    </div>
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
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
                //                beforeSend: function (xhr, settings) {
                //                    $("[id$=processing]").dialog();
                //                },
                success: onSuccess,
                error: onError
            });

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
            if (source._id == "txtReceivedFromEmp_Autocomplete") {
                textbox = document.getElementById('hdnReceivedFromEmpId');
            }
            if (source._id == "txtSubmittedByEmp_Autocomplete") {
                textbox = document.getElementById('hdnSubmittedByEmpId');
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
                    if (cntrl == "txtReceivedFromEmp") {
                        var textbox = document.getElementById('hdnReceivedFromEmpId');
                    }
                    if (cntrl == "txtSubmittedByEmp") {
                        textbox = document.getElementById('hdnSubmittedByEmpId');
                    }
                    textbox.value = val.toString();
                    return;
                }

            }
            if (cntrl == "txtReceivedFromEmp") {
                var textbox = document.getElementById('hdnReceivedFromEmpId');
            }
            if (cntrl == "txtSubmittedByEmp") {
                textbox = document.getElementById('hdnSubmittedByEmpId');
            }
            textbox.value = '';
            return;
        }
                                  
    </script>
    </form>
</body>
</html>
