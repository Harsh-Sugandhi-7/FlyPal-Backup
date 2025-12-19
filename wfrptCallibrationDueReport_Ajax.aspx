<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptCallibrationDueReport_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptCallibrationDueReport_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Callibration Due Report</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
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
                                <td colspan="5" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clsFormHeader">Calibration Due Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="As On Date required" ControlToValidate="txtAsOnDate" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvAsOnDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtAsOnDate"
                                        ErrorMessage="As On Date required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <span id="Label3" class="clsLabelHeader">Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblDate" class="clsLabelAuto">As On Date</span>
                                </td>
                                <td align="left">
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAsOnDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                        runat="server" CausesValidation="true" onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="AsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                </td>
                                <td align="left">
                                    <span id="lblRange" class="clsLabelAuto">Range</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                        <asp:ListItem Value="0">As On Date</asp:ListItem>
                                        <asp:ListItem Value="1">0 Days - 1 Month</asp:ListItem>
                                        <asp:ListItem Value="2">0 Days - 1 Quarter</asp:ListItem>
                                        <asp:ListItem Value="3">0 Days - 1 Year</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <span id="lblStep" class="clsLabelHeader">Selection of Store</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="96px" colspan="2">
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                        Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblStore" class="clsLabelAuto">Store</span>
                                </td>
                                <td align="left">
                                </td>
                                <td colspan="3" align="left">
                                    <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                        DataTextField="LocationStore" onChange="setStoreID()" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <span id="lblStep4" class="clsLabelHeader">Selection of Category</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblCategory" class="clsLabelAuto">Category</span>
                                </td>
                                <td align="left">
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                        DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <span id="lblStep2" class="clsLabelHeader">Selection of Part Number/Description</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSearch" class="clsLabel">Search</span>
                                </td>
                                <td align="left">
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxSearch_Ajax"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <span id="Span1" class="clsLabelHeader">Selection of Tool Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="Span2" class="clsLabel">Tool Type</span>
                                </td>
                                <td align="left">
                                </td>
                                <td align="left" colspan="3">
                                    <asp:DropDownList ID="cmbToolType" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" DataValueField="ID"
                                        ClientIDMode="Static" DataTextField="Name" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <span id="Span3" class="clsLabelHeader">Selection of Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="Span4" class="clsLabel">Format</span>
                                </td>
                                <td align="left">
                                </td>
                                <td align="left" colspan="3">
                                    <asp:DropDownList ID="cmbFormat" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                        <asp:ListItem Value="0">Format 1</asp:ListItem>
                                        <asp:ListItem Value="1">Format 2</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <span id="lblWorkshopSelection" class="clsLabelHeader" runat="server" Visible='<%# iif(AppSettings("ClientCode") = "BA",True,False) %>'>Selection of Workshop</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblWorkshop" class="clsLabelAuto" runat="server" Visible='<%# iif(AppSettings("ClientCode") = "BA",True,False) %>'>Workshop</span>
                                </td>
                                <td align="left">
                                </td>
                                <td colspan="3" align="left">
                                    <asp:DropDownList ID="cmbWorkShopList" runat="server" CssClass="clsComboBox3_Ajax"
                                        DataValueField="ID" DataTextField="LocationWorkShop" Visible='<%# iif(AppSettings("ClientCode") = "BA",True,False) %>'>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <span id="lblIsphysicalQtyAvailable" class="clsLabelHeader" runat="server" Visible='<%# iif(AppSettings("ClientCode") = "BA",True,False) %>'>Selection For Quantity Availability</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblPhysicalQtyAvailable" class="clsLabelAuto" runat="server" Visible='<%# iif(AppSettings("ClientCode") = "BA",True,False) %>'>Availability</span>
                                </td>
                                <td align="left">
                                </td>
                                <td colspan="3" align="left">
                                    <asp:DropDownList ID="cmbPhysicalQtyAvailable" runat="server" CssClass="clsComboBox3_Ajax"
                                          Visible='<%# iif(AppSettings("ClientCode") = "BA",True,False) %>'>
                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                        <asp:ListItem Value="1">Physical Qty. Available</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel runat="server" ID="upnlCurrentCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblStores" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="right">
                                    <asp:UpdatePanel ID="upnlActionButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            Text="Current Criteria" ToolTip="Click to display current searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH clsinfoH1" Text="Export to Excel"
                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" ToolTip="Click to Export report">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1" Text="Display"
                                                            ToolTip="Click to display report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to Close Calibration Due Report screen"
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
    <asp:HiddenField ID="StoreID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="StoreName" runat="server" ClientIDMode="Static" />
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
    <script type="text/javascript">
        function setStoreID() {
            var id = $('#cmbStore').val();
            var text = $('#cmbStore :selected').text();
            //set id to hidden field
            $("#StoreID").val(id);
            //set text of combo to the description text box
            $("#StoreName").val(text);
        }
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
    </form>
</body>
</html>
