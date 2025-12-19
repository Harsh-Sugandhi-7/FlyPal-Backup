<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUnderWarrantyItemList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptUnderWarrantyItemList_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Under Warranty Item List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
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
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmrptPartHitory" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel CssClass="clspanel1" ID="pnlmain" runat="server">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td class="clsFormHeader1">
                                    <span class="clsFormHeader" id="lbltitle">Under Warranty Item List</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <span class="clsLabelHeader" id="lblStep1">Step I. Selection of Part Number/Description</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span class="clsLabelAuto" id="lblSearch">Search</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchLong" ID="txtSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                            onChange="SetPartIdonChange()"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Serial No.</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchLong" ID="txtSerialNo" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                            Visible="true">

                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                CompletionInterval="1" ServicePath="wfrptUnderWarrantyItemList_Ajax.aspx" ServiceMethod="GetPartNoDescriptionList"
                                                TargetControlID="txtSearch" OnClientItemSelected="" UseContextKey="False" ContextKey=""
                                                CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                OnClientShowing="ClientShowing">
                                            </asp:AutoCompleteExtender>
                                            <asp:AutoCompleteExtender ID="txtSerialNo_AutoCompleteExtender" runat="server" ClientIDMode="Static"
                                                CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" ContextKey=""
                                                DelimiterCharacters="" EnableCaching="false" Enabled="True" MinimumPrefixLength="0"
                                                OnClientHiding="ClientHiding" OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating"
                                                OnClientShowing="ClientShowing" OnClientShown="ClientHiding" ServiceMethod="GetSerialNo"
                                                ServicePath="wfrptUnderWarrantyItemList_Ajax.aspx" TargetControlID="txtSerialNo"
                                                UseContextKey="True">
                                            </asp:AutoCompleteExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSerachCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto">

                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" runat="server"
                                                            CausesValidation="false" TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnExport" runat="server" Text="Export to Excel"
                                                            ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server" TabIndex="0"
                                                            Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" CausesValidation="False"
                                                            TabIndex="0" Text="Close" />
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </form>
</body>
</html>
