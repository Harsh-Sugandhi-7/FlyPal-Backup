<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftMaitenanceProgram_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAircraftMaitenanceProgram_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Aircraft Maintenance Program</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">

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
                <td class="clsFormHeader1" colspan="3">
                    <span class="clsFormHeader" id="lbltitle" >Aircraft Maintenance Program(AMP)</span>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                        CssClass="clsValidationSummary"></asp:ValidationSummary>
                    <asp:RequiredFieldValidator ID="rfvModel" runat="server" CssClass="clsLabelAuto"
                        Display="None" ControlToValidate="txtModelNo" ErrorMessage="Please Select Model."></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <span id="Label2" class="clsLabelHeader">Step I. Selection of Model</span>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="clsLabelStar">*</span>
                </td>
                <td>
                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtModelNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Select Model No. from AutoComplete or Type In Exact Model No.">
                    </asp:TextBox>
                    <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelNo"
                        ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                        CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                    </cc2:AutoCompleteExtender>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <span id="Span1" class="clsLabelHeader">Step II. Selection of Maint. Activity to include
                        in report</span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkIsService" runat="server" CssClass="clsCheckBox" Checked="true"
                        Text="Service" />
                    <asp:CheckBox ID="chkIsInspection" runat="server" CssClass="clsCheckBox" Checked="true"
                        Text="Inspection" />
                    <asp:CheckBox ID="ChkDirective" runat="server" CssClass="clsCheckBox" Checked="true"
                        Text="Directive" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnExport" CssClass="clsbtnH" TabIndex="0" runat="server" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                            Text="Export to Excel" ToolTip="Click to Export AMP Report for selected Model"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClose" CssClass="clsbtnH" TabIndex="0" runat="server" Text="Close"
                                            ToolTip="Click to Close screen" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
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
    </form>
</body>
</html>
