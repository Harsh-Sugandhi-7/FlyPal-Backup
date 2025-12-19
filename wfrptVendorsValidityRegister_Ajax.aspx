<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptVendorsValidityRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptVendorsValidityRegister_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Vendors Validity Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 09-Nov-2022--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">

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

                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td class="clsFormHeader1" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <span id="lbltitle" class="clsFormHeader" style="width: 100%">Vendors Validity Register</span>
                                                            </td>
                                                            <td colspan="2" align="right">
                                                                <%--<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH clsinfoH"
                                                                                        TabIndex="0" Text="Current Criteria" ToolTip=" Click to display current searching criterias" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Export report"
                                                                                        Width="140px" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                                                        Text="Display" ToolTip="Click to display report" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Report By Mail"
                                                                                        ToolTip="Click to report by mail" Width="140px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                        TabIndex="0" Text="Close" ToolTip="Click to Close" />
                                                                                </td>
                                                                                <td>
                                                                                    <%--Ajay 09-Nov-2022 
                                                                                    <asp:Button ID="Button1" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                                        Style="display: none;"></asp:Button>
                                                                                    <asp:Button ID="Button2" ClientIDMode="Static" runat="server" Text="----"
                                                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 1%" align="center">
                                                    <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                        class="fa fa-star fa-spin fa-5x circle-icon"
                                                        title="Mark As Favourites"></i>
                                                        <%--  Ajay 09-Nov-2022--%>
                                                    </span>
                                                </td>
                                    </td>
                                </tr>
                            </table>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="lblStepI" class="clsLabelHeader">Step I. Selection of As On Date</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span id="lblDate" class="clsLabelAuto">As On Date</span>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox CssClass="clsTextBoxTagDateSearch" runat="server" ID="txtDate"   Width="100px"
                                                onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbRange" runat="server"  
                                                Height="25px">
                                                <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                <asp:ListItem Value="1">0 Days - 1 Month</asp:ListItem>
                                                <asp:ListItem Value="2">0 Days - 2 Month</asp:ListItem>
                                                <asp:ListItem Value="3">0 Days - 3 Month</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="lblStepIII" class="clsLabelHeader">Step II. Selection of Vendor</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span id="lblStore" class="clsLabelAuto">Vendor</span>
                    </td>
                    <td>
                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbVendor" runat="server"   DataTextField="Name"
                            DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="lblStep4" class="clsLabelHeader">Step III. Enter of Nature of Vendor</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span id="lblNatureOfVendor" class="clsLabel">Nature Of Vendor</span>
                    </td>
                    <td>
                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtNatureOfVendor" runat="server"  ToolTip="Enter Nature Of Vendor"
                            MaxLength="100">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="lblStepV" class="clsLabelHeader">Step IV. Display Report</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upnlSelection" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVendorName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVendorType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                     <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Current Criteria" ToolTip=" Click to display current searching criterias" />
                                                    </td>
                                                    <td>
                                                    <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH" ToolTip="Click to Export report"
                                                        Width="140px" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                            Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" Text="Report By Mail"
                                                            ToolTip="Click to report by mail" Width="140px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Close" ToolTip="Click to Close" />
                                                    </td>
                                                   
                                    <%--Ajay 09-Nov-2022--%>
                       <td>
                    <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                        Style="display: none;"></asp:Button>
                    <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                        CausesValidation="False" Style="display: none;"></asp:Button>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td> 
                            </tr>
                            <!--Dummy panel to open modelpopup-->
            <tr style="height: 0px;">
                <td style="height: 0px;" colspan="2" align="right">
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
        <!--Ajay S 09-Nov-2022 -->
        <script type="text/javascript">
            function FunctionFav(x) {
                if (x.classList.contains("fa-star")) {
                    x.classList.remove("fa-star");
                    x.classList.add("fa-star-o");
                    x.style.color = 'black';
                    x.style.border = 'black';
                    $("#hdnBtnRemoveFav").click();
                }
                else {
                    x.classList.remove("fa-star-o");
                    x.classList.add("fa-star");
                    x.style.color = '#fff';
                    x.style.border = 'black';
                    $("#hdnBtnMarkFav").click();
                }
            }
            function MarkFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star");
                redstar.classList.remove("fa-star-o");
                redstar.style.color = '#fff';
                redstar.style.border = 'black';

            }
            function RemoveFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star-o");
                redstar.classList.remove("fa-star");
                redstar.style.border = 'black';
            }
        </script>
        <!--Ajay E -->
    </form>
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
