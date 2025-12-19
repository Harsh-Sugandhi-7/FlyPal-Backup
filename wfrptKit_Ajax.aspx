<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptKit_Ajax.aspx.vb"
    Inherits="Flypal.wfrptKit_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Kit report</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script id="clientEventHandlersJS" language="javascript">

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="1000" runat="server" ID="ScriptManager1"
            EnablePageMethods="true">
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
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr>

                                    <td class="clsFormHeader1" colspan="2">
                                <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lbltitle" class="clsFormHeader">Kit Report</span>
                                                </td>
                                                <td colspan="2" align="right">
                                                    <%--<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH clsinfoH"
                                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                            <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Visible='<%#IIf(AppSettings("ClientCode") = "BA", False, True) %>' Text="Export to Excel"
                                                                ToolTip="Click to Export report" Width="140px"></asp:Button>
                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                                Text="Display" ToolTip="Click to display report" />
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                TabIndex="0" Text="Close" ToolTip="Click to Close Inspection Kit Report screen" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                                    Display="None" ControlToValidate="txtAsOnDate" ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepI" class="clsLabelHeader">Selection of As On Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblFromDate" class="clsLabelAuto">As On Date</span>
                                    </td>
                                    <td>
                                        <%-- <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                        onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="true" TargetControlID="txtAsOnDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>--%>
                                        <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                    onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStep1" class="clsLabelHeader">Enter Kit Name</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblKit" class="clsLabelAuto">Kit Name</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbKitList" runat="server"  DataValueField="Id"
                                            DataTextField="KitName">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIII" class="clsLabelHeader">Selection of Store</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="96px"></td>
                                    <td>
                                        <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                            Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblStore" class="clsLabelAuto">Store</span>
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                        DataTextField="LocationStore">
                                    </asp:DropDownList>--%>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 25px">
                                                    <input type="checkbox" style="vertical-align: bottom;" id="chkSelectAllStore" />
                                                </td>
                                                <td style="width: 100%">
                                                    <asp:Panel ID="CpnlStoreList" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                        <div style="float: left; vertical-align: middle;">
                                                            <span id="lblStoreList" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">Store List</span>
                                                        </div>
                                                        <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                            <image id="imgbtnClpnl" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlStoreList" runat="server" ClientIDMode="Static" Visible="true">
                                                        <asp:CheckBoxList ID="ChkStoreList" runat="server" ClientIDMode="Static" CssClass="clsComboBox_Ajax"
                                                            DataTextField="LocationStore" DataValueField="IDWithIsValuedAttribute" EnableViewState="false"
                                                            RepeatColumns="4" RepeatDirection="Horizontal" Width="100%">
                                                        </asp:CheckBoxList>
                                                        <cc2:CollapsiblePanelExtender ID="clpStoreList" runat="Server" BehaviorID="clpStoreListBehaviour"
                                                            ClientIDMode="Static" CollapseControlID="CpnlStoreList" Collapsed="True" CollapsedImage="~/images/expand_blue.jpg"
                                                            CollapsedText="(Show Details...)" ExpandControlID="CpnlStoreList" ExpandedImage="~/images/collapse_blue.jpg"
                                                            ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl" SkinID="CollapsiblePanelDemo"
                                                            SuppressPostBack="false" TargetControlID="pnlStoreList"></cc2:CollapsiblePanelExtender>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIV" class="clsLabelHeader">Selection of Part Number/Description, If
                                        For Location Wise</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server"   Width="520px"
                                            AutoPostBack="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkForLocation" CssClass="clsCheckBox" Text="For Location Wise"
                                            Visible='<%# iif(AppSettings("ClientCode") = "BA",false,true) %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="Span1" class="clsLabelHeader" runat="server" visible='<%# iif(AppSettings("ClientCode") = "BA",false,true) %>'>Selection of Alternate Part(s)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkConsiderAlternatePart" CssClass="clsCheckBox"
                                            Text="Consider Alternate Part(s)" Visible='<%# iif(AppSettings("ClientCode") = "BA",false,true) %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="Span2" class="clsLabelHeader" runat="server" visible='<%# iif(AppSettings("ClientCode") = "BA",false,true) %>'>Select Format of Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span4" class="clsLabelHeader" runat="server" visible='<%# iif(AppSettings("ClientCode") = "BA",false,true) %>'>Format</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server"  Visible='<%# iif(AppSettings("ClientCode") = "BA",false,true) %>'>
                                            <asp:ListItem Text="Format1" Value="0">Format1</asp:ListItem>
                                            <asp:ListItem Text="Format2" Value="1">Format2</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepV" class="clsLabelHeader">Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlselection" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblKitName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblToStore" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                  <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                            <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                Visible='<%# iif(AppSettings("ClientCode") = "BA"  ,false,true) %>' Text="Export to Excel"
                                                ToolTip="Click to Export report" Width="140px"></asp:Button>
                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                Text="Display" ToolTip="Click to display report" />
                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                TabIndex="0" Text="Close" ToolTip="Click to Close Inspection Kit Report screen" />
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
        <asp:HiddenField ID="hdnStoreIDList" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnStoreNameList" runat="server" ClientIDMode="Static" />
    </form>
    <script type="text/javascript">
        //check all/ uncheck all checkbox of aircraft list
        $(document).ready(function () {
            $("#chkSelectAllStore").click(function () {
                var status = $("#chkSelectAllStore").attr("checked");
                $("#ChkStoreList").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");
                    }

                });
            });
            $("#btnExport,#btnDisplay,#btnCurrentSearchCriteria").live('click', function () {
                try {
                    SetSelectedStore();
                } catch (e) {
                    alert(e.Message);
                }
                return true;
            });
        });
        //set aircraft list text(i.e. aircraft name) to hidden field to access from code behind
        function SetSelectedStore() {
            var StoreIDlist = new Array();
            var StoreNamelist = new Array();
            $("#ChkStoreList :checked").each(function (i) {
                StoreIDlist.push($(this).val().split('_')[0]);
                StoreNamelist.push($(this).next().text());
            });

            $("#hdnStoreIDList").val('');
            $("#hdnStoreIDList").val(StoreIDlist);

            $("#hdnStoreNameList").val('');
            $("#hdnStoreNameList").val(StoreNamelist);
        }
    </script>
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
        $(document).ready(function () {
            $("#<%=txtSearch.ClientID%>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });

        });
    </script>
</body>
</html>
