<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptPartWiseComponentStatus_Ajax.aspx.vb"
    Inherits="Flypal.wfrptPartWiseComponentStatus_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part wise Lifed Component Status</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clstitle1">Part wise Lifed Component Status</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                        ValidationGroup="1" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        ValidationGroup="1" Display="None" InitialValue="<%$AppSettings:DateFormat%>"
                                        ControlToValidate="txtAsOnDate" ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                        ValidationGroup="1" Display="None" ControlToValidate="txtAsOnDate" ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                        ValidationGroup="1" Display="None" ErrorMessage="Enter whole Part No. and Description"
                                        OnServerValidate="customvalidate"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvSelectPart" runat="server" CssClass="clsLabelAuto"
                                        ValidationGroup="1" ControlToValidate="txtSearch" Display="None" ErrorMessage="Enter Part No."></asp:RequiredFieldValidator>
                                    <%--  <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" ClientValidationFunction="ValidateService"
                                        Display="None" ControlToValidate="chkListServiceType" ErrorMessage="Please select the Service"></asp:CustomValidator>
                                    --%>
                                    <%-- Client side validation for comboboxes--%>
                                    <asp:CustomValidator ID="cvSelection" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ControlToValidate="cmbSerialNo" ErrorMessage="Please select at least One Service or Inspection"
                                        ValidationGroup="1" ClientValidationFunction="ValidateService"></asp:CustomValidator>
                                    <script type="text/javascript">
                                        //Service List
                                        function ValidateService(source, args) {
                                            args.IsValid = false;
                                            //                                            var dd = $get("cmbServiceType");
                                            //                                            if (dd.selectedIndex != 0) {
                                            //                                                args.IsValid = true;
                                            //                                                return;

                                            //                                            }
                                            var status, InspStatus;
                                            $('#chkListServiceType').find(":checkbox").each(function () {
                                                status = $(this).attr('checked');
                                                if (status == "checked") {
                                                    return false;
                                                }
                                            });

                                            $('#chkListInspType').find(":checkbox").each(function () {
                                                InspStatus = $(this).attr('checked');
                                                if (InspStatus == "checked") {
                                                    return false;
                                                }
                                            });

                                            if (status == "checked" || InspStatus == "checked") {
                                                args.IsValid = true;
                                                return;
                                            }

                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td width="80px">
                                                                    <span id="lblFromDate" class="clsLabelAuto">As On Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtAsOnDate" runat="server" ClientIDMode="Static"
                                                                        CausesValidation="true" onchange="ValidateDateText(this,'Calender_watermarkextender')"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ClientIDMode="static" TargetControlID="txtAsOnDate"
                                                                        ID="Calender_watermarkextender" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Part No.</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td align="left">
                                                                    <span id="lblAircraft" class="clsLabelAuto">Part No.</span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" autocomplete="off" runat="server"
                                                                        AutoPostBack="true"></asp:TextBox>
                                                                    <asp:CheckBox ID="chkCheckForAlternatePart" runat="server" CssClass="clsLabelAuto"
                                                                        Text="With Alternate Part"></asp:CheckBox>
                                                                    <!-- AutoComplete Extender-->
                                                                    <cc2:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
                                                                        ServicePath="" ServiceMethod="GetPartNoDescriptionList" TargetControlID="txtSearch"
                                                                        UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                        CompletionListHighlightedItemCssClass="ac_over_Main">
                                                                    </cc2:AutoCompleteExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left" style="height: 22px">
                                                                    <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Serial No.</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                </td>
                                                                <td align="left">
                                                                    <span id="lblAssembly" class="clsLabelAuto">Serial No.</span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSerialNo" runat="server" DataValueField="CompID"
                                                                        DataTextField="SerialNo">
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" Text='Show ONLY "APPLICABLE" records'>
                                                                    </asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <span id="Label2" class="clsLabelHeader">Step IV. Selection of Service Type</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span2" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td colspan="2">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Span1" class="clsLabelHeader">Service Type</span>
                                                                            </td>
                                                                            <td>
                                                                                <%--<asp:DropDownList ID="cmbServiceType" runat="server" CssClass="clsComboBox3_Ajax"
                                                                        DataTextField="CodeType" DataValueField="ID">
                                                                    </asp:DropDownList>--%>
                                                                                <asp:CheckBoxList ID="chkListServiceType" ClientIDMode="Static" runat="server" CssClass="clsComboBox2_Ajax"
                                                                                    Style="padding-right: 12px;" DataTextField="CodeType" DataValueField="ID">
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span3" class="clsLabelStar">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span4" class="clsLabelHeader">Inspection Type</span>
                                                                            </td>
                                                                            <td>
                                                                                <%--<asp:DropDownList ID="cmbServiceType" runat="server" CssClass="clsComboBox3_Ajax"
                                                                        DataTextField="CodeType" DataValueField="ID">
                                                                    </asp:DropDownList>--%>
                                                                                <asp:CheckBoxList ID="chkListInspType" ClientIDMode="Static" runat="server" CssClass="clsComboBox2_Ajax"
                                                                                    Style="padding-right: 12px;" DataTextField="CodeType" DataValueField="ID">
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <span id="lblType" class="clsLabelHeader">Step V. Estimated Flying Hours</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <table>
                                                                        <tr>
                                                                            <td colspan="1">
                                                                            </td>
                                                                            <td>
                                                                                <asp:GridView ID="gdPerDayLimit" runat="server" AutoGenerateColumns="False" 
                                                                                    CellPadding="5" CssClass="clsGridNewStyle"  ForeColor="Black" GridLines="Horizontal"
                                                                                    ShowHeaderWhenEmpty="true">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="white" Font-Bold="True" ForeColor="black" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="PeriodID" HeaderText="PeriodID" Visible="False" />
                                                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Limit">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtLimitPerDay" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value.">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                                <td colspan="2" align="left">
                                    <span id="lblDisplayReport" class="clsLabelHeader">Step VI. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                </td>
                                <td align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="0px">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblComponent1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                            Text="Current Criteria" ToolTip="Click to display current searching criterias"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" Text="Display"
                                                            ValidationGroup="1" ToolTip="Click to display report"></asp:Button>
                                                    </td>
                                                     <td>
                                                          <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" Text="Export to Excel"
                                                        ValidationGroup="1" ToolTip="Click to Export report" >
                                                    </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" Text="Close" ToolTip="Click to Close Part wise Lifed Component Status screen"
                                                            CausesValidation="False"></asp:Button>
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
    </div>
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
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartID" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartName" />
    <%-- Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
            //get id from autocomplete list
            var node;
            var IDValue = e.get_value();
            var NameValue = e.get_text();

            if (IDValue) node = e.get_item();
            else {
                IDValue = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }
            //Set id to relevent hidden field 
            var textboxID, textboxName;
            if (source._id == "txtSearch_AutoCompleteExtender") {
                textboxID = document.getElementById('PartID');
                textboxName = document.getElementById('PartName');
            }
            textboxID.value = IDValue;
            textboxName.value = NameValue;

        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetPartIdonChange(source, extenderid) {
            var popup = $find(extenderid);
            var complist = popup.get_completionList();
            var text = $(source).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;

                    if (extenderid == "txtSearch_AutoCompleteExtender") {
                        textboxID = document.getElementById('PartID');
                    }
                    textboxID.value = val;
                    //textboxName.value = text;
                    return;
                }

            }

            if (extenderid == "txtSearch_AutoCompleteExtender") {
                document.getElementById('PartID').value = '';
                //document.getElementById('PartName').value = '';
            }
        }
        
    </script>
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
    <script type="text/javascript">

        //Service list checking
        function ControlvisibilityForCheckboxlist(elem, childid) {
            //if selected then enable and select checkboxlist else uncheck and disable list
            var status = $(elem).attr('checked');
            if (status == "checked") {
                $('#' + childid).removeAttr('disabled');
            }
            else {
                $('#' + childid).attr('disabled', 'disabled');
            }

            $('#' + childid).find(":checkbox").each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                    $(this).removeAttr('disabled');
                }
                else {
                    $(this).removeAttr("checked");
                    $(this).attr('disabled', 'disabled');
                }
            });
        }

        //wo no checkbox status change event
        function ControlVisibilityForFormat(elem) {
            var status = $(elem).attr('checked');
            if (status == "checked") {
                $('#cmbFormat').attr('disabled', 'disabled');
                $('#cmbFormat').val('0');
            }
            else {
                $('#cmbFormat').removeAttr('disabled');
            }
        }
    </script>
    </form>
</body>
</html>
