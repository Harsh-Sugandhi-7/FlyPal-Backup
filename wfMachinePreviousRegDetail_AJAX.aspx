<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachinePreviousRegDetail_AJAX.aspx.vb"
    Inherits="Flypal.wfMachinePreviousRegDetail_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Previous Reg Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body class="formBGColor">
    <form id="form1" runat="server" >
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <%-- <tr>
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <td>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Previous Registration Info.</asp:Label>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>--%>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="None" ControlToValidate="txtRegNo"
                                            ValidationGroup="a" ErrorMessage="Reg No.  Required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator><asp:CustomValidator
                                                ID="cvStartDate" runat="server" Display="None" ControlToValidate="txtStartTSN"
                                                ValidationGroup="a" ErrorMessage="Expiry Date must be greater than Issue Date"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvEndDate" runat="server" CssClass="clsLabelAuto" ValidationGroup="a"
                                            ErrorMessage="Expiry Date must be greater than Issue Date" ControlToValidate="txtStartCycle"
                                            Display="None"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlPrevReg" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsAircraftRegInfo" class="clsFieldSet" style="border-width: 1px">
                                            <legend id="lblAircraftPreviousRegistrationDetails" runat="server" style="font-weight: bold">
                                                <b>Aircraft Previous Registration Details</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCurrencyStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRegNo" runat="server" CssClass="clsLabelAuto">Reg No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBox2_Ajax" ToolTip="Enter Registeration Number"
                                                            MaxLength="25"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStartStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStartDate" runat="server" CssClass="clsLabelAuto">Start Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="calStartDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                                        OnFocus="checkDate" onchange="ValidateDateText(this,'calStartDate_watermarkextender');"
                                                                        Width="90px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="calStartDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="calStartDate_watermarkextender" runat="server"
                                                                        ClientIDMode="Static" Enabled="True" TargetControlID="calStartDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$ AppSettings:DateFormat %>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEndStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEndDate" Width="60px" runat="server" CssClass="clsLabelAuto">End Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <%-- <uc1:sicalendar id="calEndDate" runat="server"></uc1:sicalendar>--%>
                                                                    <asp:TextBox ID="calEndDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                                        OnFocus="checkDate" onchange="ValidateDateText(this,'calEndDate_watermarkextender');"
                                                                        Width="90px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="calEndDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="calEndDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                        Enabled="True" TargetControlID="calEndDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$ AppSettings:DateFormat %>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOperator" runat="server" CssClass="clsLabelAuto">Operator</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOperator" runat="server" CssClass="clsTextBox2_Ajax" MaxLength="100"
                                                            ToolTip="Enter Operator"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStartTSN" runat="server" CssClass="clsLabel" Height="16px" Width="60px">Start TSN</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtStartTSN" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                            MaxLength="50" ToolTip="Enter Start TSN Value"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEndTSN" runat="server" CssClass="clsLabel">End TSN</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEndTSN" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                            ToolTip="Enter End TSN Value" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCountry" runat="server" CssClass="clsLabel">Country</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBox2_Ajax" MaxLength="100"
                                                                ToolTip="Enter Country"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblStartCycle" runat="server" CssClass="clsLabelAuto" Width="80px">Start Cycle</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStartCycle" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                MaxLength="5" ToolTip="Enter Start Cycle Value"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblEndCycle" runat="server" CssClass="clsLabelAuto">End Cycle</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEndCycle" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                MaxLength="5" ToolTip="Enter End Cycle Value"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td valign="bottom" align="right">
                                <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnAdd" OnClientClick="return CheckValidation();" runat="server"
                                            CssClass="clsButton_Ajax" ToolTip="Click to add Tank in the List" Text="Add"
                                            ValidationGroup="a"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Previous Registration Details List</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGridPrevRegList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgPrevRegList" runat="server" AutoGenerateColumns="False" Visible="true"
                                            CssClass="clsGrid" PageSize="3" ShowHeaderWhenEmpty="true">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="SerialNo" HeaderText="Sr. No."></asp:BoundField>
                                                <asp:BoundField DataField="RegNo" SortExpression="CertificateName" HeaderText="Reg. No.">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StartDateFormatted" HeaderText="Start Date">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EndDateFormatted" HeaderText="End Date">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StartTSN" SortExpression="StartTSN" HeaderText="Start TSN">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EndTSN" HeaderText="End TSN">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StartCycle" HeaderText="Start Cycle">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EndCycle" HeaderText="End Cycle">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Operator" SortExpression="Remark" HeaderText="Operator">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Country" HeaderText="Country">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec" ValidationGroup="a">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:ButtonField>
                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" ValidationGroup="a">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the list of Certificates"
                                                        CausesValidation="False" Text="Print" Visible="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go Previous page"
                                                        CausesValidation="False" Text="Back"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" ClientIDMode="Static" DynamicLayout="false"
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
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizePreviousRegList('PreviousReg');
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
        function CheckValidation() {
            if (!Page_ClientValidate()) {
                // Call Your custom JS function and return value.
                CallParentFunction();
            }
        }
      
    </script>
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var resetTodaysDate = 'false';
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
