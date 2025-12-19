<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptStoreBalanceForNilRotable_Ajax.aspx.vb"
    Inherits="Flypal.wfrptStoreBalanceForNilRotable_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Min Stock Rotable</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
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
                    <td class="clsFormHeader1" colspan="2">

                        <span class="clsFormHeader" id="lbltitle">Min Stock Rotable</span>



                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span class="clsLabelHeader" id="StepI">Step I. Selection of Date</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="clsLabelAuto" id="lblDate">As On Date</span>
                    </td>
                    <td>
                        <asp:TextBox CssClass="clsTextBoxTagDateSearch" runat="server" ID="txtDate" Width="100px"
                            onchange="ValidateDateText(this,'txtDate_watermarkextender');"></asp:TextBox>
                        <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span class="clsLabelHeader" id="lblStep2">Step II. Selection of Part Number/Description</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="clsLabel" id="lblSearch">Search</span>
                    </td>
                    <td>
                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server" Width="275px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table border="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnExport" TabIndex="0" runat="server" Text="Export to Excel"
                                                ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnDisplay" TabIndex="0" runat="server" Text="Display"
                                                ToolTip="Click to display report"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnByMail" runat="server" Text="Report By Mail"
                                                ToolTip="Click to report by mail" />
                                        </td>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                ToolTip="Click to Close" CausesValidation="False"></asp:Button>
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
