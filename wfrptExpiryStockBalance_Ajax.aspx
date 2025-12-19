<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptExpiryStockBalance_Ajax.aspx.vb"
    Inherits="Flypal.wfrptExpiryStockBalance_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Expiry Stock Balance Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script type="text/javascript" language="javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>

                                       <td colspan="2" class="clsFormHeader1">
                                <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lbltitle" class="clsFormHeader">Expiry Stock Balance Report</span>
                                                </td>
                                                <td align="right">
                                                    <%--<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table cellspacing="0">
                                                                <tr>
                                                                     <td>
                                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Current Criteria" ToolTip=" Click to display current searching criterias"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                            Text="Export to Excel" ToolTip="Click to Export report" Width="100px"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                            ToolTip="Click to Close Expiry Stock Balance screen" CausesValidation="False"></asp:Button>
                                                                    </td> 
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepI" class="clsLabelHeader">Step I. Selection of Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDate" class="clsLabelAuto">Date</span>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox CssClass="clsTextBoxTagDateSearch" runat="server" ID="txtDate"   Width="100px"
                                                    onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepII" class="clsLabelHeader">Step II. Selection of Order By</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblOrderBy" class="clsLabelAuto">Order By</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbOrderBy" runat="server"  >
                                            <asp:ListItem Value="1">Part No.</asp:ListItem>
                                            <asp:ListItem Value="2">Expiry Date</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStep4" class="clsLabelHeader">Step III. Selection of Category</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblCategory" class="clsLabelAuto">Category</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle"  ID="cmbCategory" runat="server"   DataValueField="ID"
                                            DataTextField="Name">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIV" class="clsLabelHeader">Step IV. Selection of Part Number Or Description</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server"   Width="275px"
                                            AutoPostBack="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="Span1" class="clsLabelHeader">Step V. Enter text to be display at bottom line
                                        of report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span2" class="clsLabelAuto">Text</span>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtBottomLine" runat="server" AutoPostBack="False"  
                                            Text='<%# " Submitted By : " + User.Identity.Name %>' Width="275px" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIII" class="clsLabelHeader">Step VI. Selection of Store</span>
                                    </td>
                                </tr>
                                <tr>

                                    <td colspan="2">
                                        <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
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
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="padding-left: 4px;">
                                                                    <asp:CheckBox ID="chkIsValuedStore" runat="server" Text="Is Valued Store" ClientIDMode="Static"
                                                                        CssClass="clsCheckBox" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="ChkStoreList" runat="server" ClientIDMode="Static" CssClass="clsComboBox_Ajax"
                                                                        DataTextField="LocationStore" DataValueField="IDWithIsValuedAttribute" EnableViewState="false"
                                                                        RepeatColumns="4" RepeatDirection="Horizontal" Width="100%">
                                                                    </asp:CheckBoxList>
                                                                    <cc2:CollapsiblePanelExtender ID="clpStoreList" runat="Server" BehaviorID="clpStoreListBehaviour"
                                                                        ClientIDMode="Static" CollapseControlID="CpnlStoreList" Collapsed="True" CollapsedImage="~/images/expand_blue.jpg"
                                                                        CollapsedText="(Show Details...)" ExpandControlID="CpnlStoreList" ExpandedImage="~/images/collapse_blue.jpg"
                                                                        ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl" SkinID="CollapsiblePanelDemo"
                                                                        SuppressPostBack="false" TargetControlID="pnlStoreList"></cc2:CollapsiblePanelExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepV" class="clsLabelHeader">Step VII. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                      <td align="right" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Current Criteria" ToolTip=" Click to display current searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                            Text="Export to Excel" ToolTip="Click to Export report" Width="140px"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
                                                            ToolTip="Click to Close Expiry Stock Balance screen" CausesValidation="False">
                                                        </asp:Button>
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

            $("#chkIsValuedStore").click(function () {
                try {
                    EnableDisableAllStores();
                } catch (e) {
                    alert(e.Message);
                }
                return true;
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

        function EnableDisableAllStores() {
            var IsChecked = $("#chkIsValuedStore").attr("checked");
            if (IsChecked) {
                $("#chkSelectAllStore").prop('disabled', true);
                $("#chkSelectAllStore").prop('checked', false);
                $("#ChkStoreList").find(":checkbox").each(function () {
                    var IsValued = $(this).val().split('_')[1];
                    if (IsValued == "False") {
                        $(this).attr('checked', false);
                    }
                    else if (IsValued == "True") {
                        $(this).attr('checked', true);
                    }


                    $(this).attr('disabled', true);
                });

            }
            else {
                $("#chkSelectAllStore").removeAttr('disabled');

                $("#ChkStoreList").find(":checkbox").each(function () {
                    $(this).removeAttr('disabled');
                    $(this).removeAttr('checked');
                });
            }
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });
    </script>
</body>
</html>
