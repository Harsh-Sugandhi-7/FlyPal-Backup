<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAssemblyHistoryCard_AJAX.aspx.vb"
    Inherits="Flypal.wfrptAssemblyHistoryCard_AJAX" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assembly History Card</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblMain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblList" runat="server" CssClass="clsFormHeader">Assembly History Card</asp:Label>
                                            </td>

                                            <%--<td colspan="3" align="right">
                                                <asp:UpdatePanel ID="upnlPrint" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrint" runat="server" ToolTip="Click to display the list of Assembly History Card"
                                                                        Text="Display" CausesValidation="true"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" ToolTip="Click to Close Assembly History Card Report screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>

                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                                ControlToValidate="txtAsOnDate" Display="None" ErrorMessage="AS On Date  Required."></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="txtModel"
                                                Display="None" ErrorMessage="Please Select Model"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvAssembly" runat="server" ControlToValidate="cmbAssembly"
                                                Display="None" ErrorMessage="Please select the Assembly" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvModel" runat="server" ControlToValidate="txtModel" Display="None"
                                                ErrorMessage="Please select the Model" OnServerValidate="CustomValidate"></asp:CustomValidator></TD>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAsOnDate" CssClass="clsLabelAuto" runat="server">As On Date</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtAsOnDate" ClientIDMode="Static"
                                        runat="server" onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="calAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 7px">
                                    <asp:Label ID="Label5" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td style="height: 7px">
                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                </td>
                                <td style="height: 7px">
                                    <asp:UpdatePanel ID="upnlModel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <%-- <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                DataTextField="ModelName" AutoPostBack="True">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox cssclass="clsTextBoxSearch_Ajax" ID="txtModel" autocomplete="off" runat="server" 
                                                AutoPostBack="True" onchange="SetPartIdonChange(this,'txtModel_AutoCompleteExtender')"></asp:TextBox>
                                            <!-- AutoComplete Extender-->
                                            <cc2:AutoCompleteExtender ID="txtModel_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
                                                ServicePath="" ServiceMethod="GetModelList" TargetControlID="txtModel" UseContextKey="True"
                                                ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetID">
                                            </cc2:AutoCompleteExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td style="height: 21px">
                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                </td>
                                <td style="height: 21px">
                                    <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataValueField="ID"
                                                DataTextField="SerialNo">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="cmbtempAssembly" runat="server" CssClass="clsComboBox_Ajax"
                                                DataTextField="SerialNo" Visible="false" DataValueField="ID">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:UpdatePanel ID="upnlPrint" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnPrint" runat="server" ToolTip="Click to display the list of Assembly History Card"
                                                            Text="Display" CausesValidation="true"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" ToolTip="Click to Close Assembly History Card Report screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
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
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="ModelID" />
    <%-- Autocomplete functions to set id--%>
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
            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtModel_AutoCompleteExtender") {
                textbox = document.getElementById('ModelID');
            }
            textbox.value = value;
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

                    if (extenderid == "txtModel_AutoCompleteExtender") {
                        textbox = document.getElementById('ModelID');
                    }
                    textbox.value = val;
                    return;
                }

            }

            if (extenderid == "txtModel_AutoCompleteExtender") {
                document.getElementById('ModelID').value = '';
            }
        }
        
    </script>
    </form>
</body>
</html>
