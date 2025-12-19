<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeTrainingStatusList_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForEmployeeTrainingStatusList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Training Due Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
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
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clsFormHeader">Employee Training Status Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStepII" class="clsLabelHeader">Step I. Selection of Employee</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblEmployee" class="clsLabelAuto">Employee</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataValueField="ID" DataTextField="EmpNoName">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span2" class="clsLabelAuto">Department</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbDepartmentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataTextField="Name" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Training Details</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblTraining" class="clsLabelAuto">Training</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbTrainningList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataValueField="ID" DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblTrainingOrg" class="clsLabelAuto">Training Org.</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbTrainningOrgList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataValueField="ID" DataTextField="NameWithCity">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStep4" class="clsLabelHeader">Step III. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEmployeeCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTrainningCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTrainningOrgCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias.">
                                                        </asp:Button>
                                                    </td>
                                                    <%--  <td>
                                                        <asp:Button ID="btnDisplay" runat="server" Text="Display" ToolTip="Click to Display Report"
                                                            CssClass="clsButton"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExpotToExcel" runat="server" ToolTip="Click to Display Report"
                                                            Text="Export To Excel" CssClass="clsButtonLong_Ajax" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                    </td>--%>
                                                    <td>
                                                        <asp:Button ID="btnShowStatusOnGrid" runat="server" ToolTip="Click to Show Status (with Color)"
                                                            Text="Show Status (with Color)" CssClass="clsbtnH clsinfoH1">
                                                        </asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Click to close Employee Training Status Report screen"
                                                            CssClass="clsbtnH clsinfoH1"></asp:Button>
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
    <!--Show Status -->
    <div style="display: none">
        <asp:Button runat="server" ID="btndummyShowStatus" Text="Dummy Show Status" />
    </div>
    <asp:Panel runat="server" ID="pnlShowStatus" Style="display: none; position: absolute" ScrollBars="Auto"
        CssClass="clspanel1" Width="99%" Height="500px">
        <div style="max-height: 1000px;">
            <table class="clstablelistout" id="Table5">
                <tr>
                    <td align="left" class="style1">
                        <asp:UpdatePanel ID="upnlShowStatus" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <table class="clstablelistin" id="Table6">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <span id="Label1" class="clsFormHeader">Show Training Status</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="right">
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnShowStatusCloseTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            Text="Close" ToolTip=" Click to close Show Document Type screen" CausesValidation="False"
                                                            Visible="false"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="hyConverttoPdf" runat="server" ClientIDMode="Static" Text="Convert To Excel"
                                                CssClass="clsHyperlink1"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 70%">
                                                <tr>
                                                    <td>
                                                        <span id="spnConducted" class="clsLabel">✅ - Conducted </span>
                                                    </td>
                                                    <td>
                                                        <span id="spnApplied" class="clsLabel">✔ - Only Applicable and Not Conducted
                                                        </span>
                                                    </td>
                                                    <td>
                                                        <span id="Span1" class="clsLabel">❌ - Once Made APPLICABLE and Then is NOT Applicable
                                                        </span>
                                                    </td>
                                                    <td>
                                                        <span id="Span3" class="clsLabel">╳ - Not At ALL Added 
                                                        </span>
                                                    </td>
                                                   
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="318px" Width="100%">
                                                <asp:UpdatePanel ID="upnlGridShowStatus" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                                            <legend>Employee Training List</legend>
                                                            <asp:GridView ID="grdMain" runat="server" EnableViewState="true" AutoGenerateColumns="False"
                                                                OnRowDataBound="grdMain_RowDataBound" ClientIDMode="Static" 
                                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            </asp:GridView>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="right">
                                            <table id="tblNew">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnShowStatusClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            Text="Close" ToolTip=" Click to close Show Training Status screen" CausesValidation="False">
                                                        </asp:Button>
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
            </table>
            <asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="200" DynamicLayout="false"
                runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                        background-color: #000000; top: 0; z-index: 99999;">
                    </div>
                    <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                        z-index: 100000;">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                    Height="48px" Width="48px" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpShowStatus" runat="server" TargetControlID="btndummyShowStatus"
        X="10" Y="10" PopupControlID="pnlShowStatus" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!--End Show Status -->
    </form>
</body>
</html>
