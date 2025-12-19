<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeDocumentDueList_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForEmployeeDocumentDueList_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Document Due Reporty</title>
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
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

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
    <style type="text/css">
        .clstxtbox
        {
            border-top-left-radius: 20px;
            border-top-right-radius: 20px;
            border-bottom-left-radius: 20px;
            border-bottom-right-radius: 20px;
        }
    </style>
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
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clsFormHeader">Employee Document Due Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
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
                                            <asp:TextBox ID="txtAsOnDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                            </cc2:CalendarExtender>

                                             <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        >
                                        <asp:ListItem Value="0">Between 0 Days - 1 Month</asp:ListItem>
                                        <asp:ListItem Value="1">Between 0 Days - 2 Month</asp:ListItem>
                                        <asp:ListItem Value="2">Between 0 Days - 3 Month</asp:ListItem>
                                        <asp:ListItem Value="3">Between 0 Days - 6 Month</asp:ListItem>
                                        <asp:ListItem Value="4">Between 0 Days - 12 Month</asp:ListItem>
                                        <asp:ListItem Value="5">Between 0 Days - 24 Month</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStepII" class="clsLabelHeader">Step II. Selection of Employee</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblEmployee" class="clsLabelAuto">Employee</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                        DataValueField="ID" DataTextField="EmpNoName">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span2" class="clsLabelAuto">Department</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="cmbDepartmentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataTextField="Name" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Document Details</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblDocument" class="clsLabelAuto">Document</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="cmbDocumentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                        DataValueField="ID" DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblTrainingOrg" class="clsLabelAuto">Document No.</span>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="20"
                                        ToolTip="Enter Doc No."></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep4" class="clsLabelHeader">Step IV. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 15px">
                                                        <asp:Label ID="lblAsOnDate1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td style="height: 15px">
                                                        <asp:Label ID="lblRangeDisp" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblEmployeeCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDocumentCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDocumentNoCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias.">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" Text="Display" ToolTip="Click to Display Report"
                                                            CssClass="clsbtnH clsinfoH1"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExpotToExcel" runat="server" ToolTip="Click to Export To Excel"
                                                            Text="Export To Excel" CssClass="clsbtnH clsinfoH1" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnShowStatusOnGrid" runat="server" ToolTip="Click to Show Status (with Color)"
                                                             Text="Show Status (with Color)" 
                                                            CssClass="clsbtnH clsinfoH1">
                                                        </asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Click to close Employee Document Due Report screen"
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
    <asp:Panel runat="server" ID="pnlShowStatus" Style="display: none;position:absolute" CssClass="clspanel1" 
        Width="97%" Height="500px">
        <div style="max-height: 1000px; overflow: auto;">
            <table class="clstablelistin" id="Table5">
                <tr>
                    <td align="left" class="style1">
                        <asp:UpdatePanel ID="upnlShowStatus" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <table class="clstablelistin" id="Table6">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <span id="Label1" class="clsFormHeader">Show Document Status</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="right">
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnShowStatusCloseTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            Text="Close" ToolTip=" Click to close Show Document Type screen" CausesValidation="False" Visible ="false">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="lblExpiredItems" runat="server" BackColor="Red" Height="12px" 
                                                            Style="border-top-left-radius: 20px;
                                                        border-top-right-radius: 20px; border-bottom-left-radius: 20px; border-bottom-right-radius: 20px;" 
                                                            Width="12px"></asp:TextBox>
                                                        <span class="clsLabelAuto">Expired</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TextBox1" runat="server" BackColor="Orange" Height="12px" 
                                                            Style="border-top-left-radius: 20px;
                                                        border-top-right-radius: 20px; border-bottom-left-radius: 20px; border-bottom-right-radius: 20px;" 
                                                            Width="12px"></asp:TextBox>
                                                        <span class="clsLabelAuto">Expiration Within 3 Months</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TextBox2" runat="server" BackColor="Green" Height="12px" 
                                                            Style="border-top-left-radius: 20px;
                                                        border-top-right-radius: 20px; border-bottom-left-radius: 20px; border-bottom-right-radius: 20px;" 
                                                            Width="12px"></asp:TextBox>
                                                        <span class="clsLabelAuto">Expiration Over 3 Months</span>
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
                                         <asp:Panel ID="Panel1" runat="server" ScrollBars="both" Height="350px" Width="100%">
                                            <asp:UpdatePanel ID="upnlGridShowStatus" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend>Employee Document List</legend>
                                                        <asp:GridView ID="grdMain" runat="server" EnableViewState="true" AutoGenerateColumns="False"
                                                            OnRowDataBound="grdMain_RowDataBound" ClientIDMode="Static" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
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
                                                            Text="Close" ToolTip=" Click to close Show Document Type screen" CausesValidation="False">
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
    <cc2:ModalPopupExtender ID="mdlPopUpShowStatus" runat="server" TargetControlID="btndummyShowStatus" X="10" Y="10"  
        PopupControlID="pnlShowStatus" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!--End Show Status -->
    </form>
</body>
</html>
