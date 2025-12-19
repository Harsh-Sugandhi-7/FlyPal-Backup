<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAssetBalance_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAssetBalance_Ajax" %>
    <%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Store Balance Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
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
                    <span id="lbltitle" class="clstitle1">Asset Balance</span>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <span id="StepI" class="clsLabelHeader">Step I. Selection of Date</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblDate" class="clsLabelAuto">As On Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                onchange="ValidateDateText(this,'txtDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="span2" class="clsLabelHeader">Step II. Selection of Store</span>
                                        </td>
                                    </tr>
                                      <tr>
                                                    <td width="96px">
                                                        
                                                    </td>
                                                    <td>
                                                       <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                    <tr>
                                        <td>
                                            <span id="lblStore" class="clsLabel">Store</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                                DataTextField="LocationStore">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="span3" class="clsLabelHeader">Step III. Selection of Supplier</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblSupplier" class="clsLabelAuto">Supplier</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSupplierList" runat="server" CssClass="clsTextBox_Ajax" Width="275px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="span4" class="clsLabelHeader">Step IV. Selection of Category</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblCategory" class="clsLabelAuto">Category</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                                DataTextField="Name">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="span5" class="clsLabelHeader">Step V. Selection of ATA</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblATAChapter" class="clsLabel">ATA Chapter </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox3_Ajax"
                                                DataValueField="ID" DataTextField="ATAChapter">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="span6" class="clsLabelHeader">Step VI. Selection of Model</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="96px">
                                            <span id="spanModel" class="clsLabel">Model</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlModelSelection" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="ModelName"
                                                        DataValueField="ID">
                                                    </asp:DropDownList>
                                                    <asp:CheckBox ID="chkCommonOrApplicability" runat="server" AutoPostBack="true" CssClass="clsCheckBox"
                                                        Text="Common/No Applicability" ToolTip="Common/No Applicability" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="span7" class="clsLabelHeader">Step VII. Selection of With Amount</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkWithBalAmount" runat="server" CssClass="clsCheckBox" Text="With Bal. Amount">
                                            </asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="span8" class="clsLabelHeader">Step VIII. Selection of Part Number/Description</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblSearch" class="clsLabel">Search</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBox_Ajax" Width="275px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step IX. 
                                        Display Report</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
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
                                                                <asp:Label ID="lblSuppName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblATA" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
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
                                                            <td align="right">
                                                                <table border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong"
                                                                                Text="Current Criteria" ToolTip="Click to display current searching criterias">
                                                                            </asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsButton" Text="Export to Excel"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                                ToolTip="Click to Export report" Width="100px"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton" Text="Display"
                                                                                ToolTip="Click to display report"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnByMail" runat="server" CssClass="clsButton_Ajax" Text="Report By Mail"
                                                                                ToolTip="Click to report by mail" Width="96px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" Text="Close"
                                                                                ToolTip="Click to Close" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <!--Dummy panel to open modelpopup-->
                                                        <tr style="height: 0px;">
                                                            <td style="height: 0px;" align="right">
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <!--End -->
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
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
    <asp:HiddenField ID="hdnpartId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
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
            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtSearch_Autocomplete") {
                textbox = document.getElementById('hdnpartId');
            }
            else if (source._id == "txtCustomerList_AutoCompleteExtender") {
                textbox = document.getElementById('hdnCustomerID');
            }

            textbox.value = value;
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetPartIdonChange() {
            var popup = $find("txtSearch_Autocomplete");
            var complist = popup.get_completionList();
            var text = $("#txtSearch").val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    var textbox = document.getElementById('hdnpartId');
                    textbox.value = val;
                    return;
                }
            }
            document.getElementById('hdnpartId').value = '';
        }
    </script>
    <!-- Popup For By Mail -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
        PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyForByMail").click();

                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSupplierList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
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
</body>
</html>
