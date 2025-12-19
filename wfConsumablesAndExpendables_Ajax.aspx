<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfConsumablesAndExpendables_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfConsumablesAndExpendables_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>C&E Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

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
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Consumables & Expendables(C&E) Details</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH" Text="Authorize"
                                                                    ToolTip="Click to Authorize Transaction" ValidationGroup="1"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Transaction"
                                                                    ValidationGroup="1"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go back to the previous page"></asp:Button>
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
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Transaction Date Required" ControlToValidate="txtDate" Display="None"
                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvWorkShop" runat="server" Display="None" ControlToValidate="txtReqTextNo" ValidateEmptyText="true" 
                                            ValidationGroup="1" ErrorMessage="Parts Requisition Sheet No.(PRS) Required"
                                            OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <%-- <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbMachine"
                                            ValidateEmptyText="true" ValidationGroup="1" Display="None" ErrorMessage="Aircarft Required"
                                            OnServerValidate="customvalidate"></asp:CustomValidator>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader" Text="<%# mConsumableAndExpendable.StatusName %>">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend><b>C&E Detail</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblSerialNo1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDate" class="clsLabel">Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearchDate"
                                                            AutoPostBack="true" OnTextChanged="txtDate_TextChanged" onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="Span1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblNo" class="clsLabel">No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                            Enabled="<%#  mConsumableAndExpendable.IsNew  %>" Text="<%# mConsumableAndExpendable.Text %>"
                                                            ToolTip="Enter Text">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="8"
                                                            Enabled="<%#  mConsumableAndExpendable.IsNew  %>" Text="<%# mConsumableAndExpendable.No %>"
                                                            ToolTip="Enter No.">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblLocation1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblLocation" class="clsLabel">Parts Requisition Sheet No.(PRS)</span>
                                                    </td>
                                                    <td>
                                                        <%--<asp:DropDownList ID="cmbRequisitionText" runat="server" CssClass="clsComboBox1_Ajax"
                                                            SelectedValue="<%# mConsumableAndExpendable.ReqID %>" DataTextField="RequisitionTextNo"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>--%>
                                                        <asp:TextBox ID="txtReqTextNo" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                            AutoPostBack="true" OnTextChanged="txtReqTextNo_TextChanged" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtReqTextNo_Autocomplete" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                            CompletionInterval="1" ServicePath="" ServiceMethod="GetReqTextNoList" TargetControlID="txtReqTextNo"
                                                            UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                        </cc2:AutoCompleteExtender>
                                                    </td>
                                                    <td colspan="3">
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                <td>
                                                    <span id="Span2" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="Span3" class="clsLabel">Aircraft</span>
                                                </td>
                                                <td colspan="4">
                                                    <asp:DropDownList ID="cmbMachine" runat="server" CssClass="clsComboBox_Ajax" DataTextField="RegNo"
                                                        SelectedValue="<%# mConsumableAndExpendable.MachineID %>" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                               
                                            </tr>--%>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                    <legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="Label2" class="clsLabelHeader">Item(s)</span>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlItemAdd" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="btnAddItem" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                Width="24px" ToolTip="Click to Add Item" CausesValidation="true" ValidationGroup="1">
                                                            </asp:ImageButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </legend>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgItems" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right"></PagerStyle>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SRNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemName" HeaderText="Part No." HtmlEncode="false">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemDescription" HeaderText="Description" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RequestedQty" HeaderText="Requested Qty">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IssuedQty" HeaderText="Total Issued Qty">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Used Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtUsedQty" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"UsedQty") %>' ClientIDMode="Static"
                                                                            ToolTip="Enter Used Qty." MaxLength="4">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvBrokenRules4" runat="server" ErrorMessage="Used Qty. Required."
                                                                            ControlToValidate="txtUsedQty" Display="None" OnServerValidate="CustomValidate1"
                                                                            ValidationGroup="1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Scrap Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtScrapQty" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"ScrapQty") %>' ClientIDMode="Static"
                                                                            ToolTip="Enter Scrap Qty." MaxLength="4">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvBrokenRules5" runat="server" ErrorMessage="Scrap Qty. Required."
                                                                            ControlToValidate="txtScrapQty" Display="None" OnServerValidate="CustomValidate1"
                                                                            ValidationGroup="1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DisplayUnitName" HeaderText="Unit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Serial No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtSerialNo" Width="100px" runat="server" CssClass="clsTextBox_Ajax"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"SerialNo") %>' ClientIDMode="Static"
                                                                            ToolTip="Enter Serial No." MaxLength="50">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvBrokenRules6" runat="server" ErrorMessage="Serial No. Required."
                                                                            ControlToValidate="txtSerialNo" Display="None" OnServerValidate="CustomValidate1"
                                                                            ValidationGroup="1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Position">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPosition" runat="server" Width="50px" CssClass="clsTextBox_Ajax"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"Position") %>' ClientIDMode="Static"
                                                                            ToolTip="Enter Position" MaxLength="25">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvBrokenRules1" runat="server" ErrorMessage="Position Required."
                                                                            ControlToValidate="txtPosition" Display="None" OnServerValidate="CustomValidate1"
                                                                            ValidationGroup="1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="*Reference">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"Reference") %>'
                                                                            Width="130px" ClientIDMode="Static" ToolTip="Enter Reference" MaxLength="250">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvBrokenRules2" runat="server" ErrorMessage="Reference Required."
                                                                            ControlToValidate="txtReference" Display="None" OnServerValidate="CustomValidate1"
                                                                            ValidationGroup="1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cost Center">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCostCenter" runat="server" Width="50px" CssClass="clsTextBox_Ajax"
                                                                            ReadOnly="true" BackColor="Gainsboro" Text='<%# DataBinder.Eval(Container.DataItem,"RegNo") %>'
                                                                            ClientIDMode="Static" ToolTip="Enter Cost Center" MaxLength="25">
                                                                        </asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Note">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxLong_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"Note") %>'
                                                                            TextMode="MultiLine" Height="30px" Width="100px" ClientIDMode="Static" ToolTip="Enter Note"
                                                                            MaxLength="500">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvBrokenRules3" runat="server" ErrorMessage="Note Required."
                                                                            ControlToValidate="txtNote" Display="None" OnServerValidate="CustomValidate1"
                                                                            ValidationGroup="1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <%--<tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH" Text="Authorize"
                                                        ToolTip="Click to Authorize Transaction" ValidationGroup="1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Transaction"
                                                        ValidationGroup="1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go back to the previous page">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnCEPartList" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
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
    <%--Date Validations--%>
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
    <!-- Re-Order Level Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCEList" Text="Dummy CE Part List" ClientIDMode="Static"
            CausesValidation="False" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupCEPartList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="iPopupCEPartList" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCEPartList" runat="server" TargetControlID="btnDummyCEList"
        PopupControlID="pnlPopupCEPartList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCEPartListStateComplete() {
            $("#btnDummyCEList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenToolsWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#iPopupCEPartList").attr("src", "wfConsumablesAndExpendablePendingList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCEList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCEPartList() {
            var CEPartListWindow = $find("<%=mdlPopupCEPartList.ClientID %>");
            //close CE Part List popup window
            CEPartListWindow.hide();
            $("#iPopupCEPartList").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnCEPartList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
