<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineLeaseInfo_Ajax.aspx.vb"
    Inherits="Flypal.wfMachineLeaseInfo_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Leased Information List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout"
    class="formBGColor">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnableScriptGlobalization="true" EnableScriptLocalization="true">
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvLeasedType" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbLeasedType"
                                                Display="None" ClientValidationFunction="validateLeasedType" ErrorMessage="Select Leased Type."></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Lease Start Date Required." ControlToValidate="txtLeaseStartDate"
                                                Display="None"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtLeaseEndDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="Lease End Date Required."></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="Lease End Date must be greater than Lease Start Date."
                                                ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvDate" runat="server" ErrorMessage="Lease End Date must be greater than Lease Start Date."
                                                Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCurr" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbCurrency"
                                                Display="None" ClientValidationFunction="validateCurrency" ErrorMessage="Select Currency."></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateLeasedType(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbLeasedType");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }

                                                function validateCurrency(source, args) {
                                                    args.IsValid = false;
                                                    var Index = $get("cmbCurrency").selectedIndex;
                                                    var rate = parseInt($get("txtRateForMinHrs").value);

                                                    //If txtRateForMinHrs.Text <> "" And txtRateForMinHrs.Text <> "0" Then
                                                    if ((Index == 0 && (!isNaN(rate) || rate <= 0)) || (Index > 0)) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlLeaseInfoDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="lblLeasedInfoListInfo" class="clsFieldSet" style="border-width: 1px">
                                                <legend id="lblTitle" runat="server" style="font-weight: bold"><b>Aircraft Leased Information
                                                    Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblLeasedTypeStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblLeasedType" class="clsLabelAuto">Leased Type</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbLeasedType" runat="server" CssClass="clsComboBox1_Ajax"
                                                                ClientIDMode="Static">
                                                                <asp:ListItem Value="(SELECT)" Selected="True">(SELECT)</asp:ListItem>
                                                                <asp:ListItem Value="Wet Lease/ACMI">Wet Lease/ACMI</asp:ListItem>
                                                                <asp:ListItem Value="Damp Lease/AMI">Damp Lease/AMI</asp:ListItem>
                                                                <asp:ListItem Value="Dry Lease">Dry Lease</asp:ListItem>
                                                                <asp:ListItem Value="Operating Lease">Operating Lease</asp:ListItem>
                                                                <asp:ListItem Value="Financial Leasing">Financial Leasing</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Label1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblLeaseStartDate" class="clsLabelAuto">Lease Start Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLeaseStartDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                                Width="100px" runat="server" onchange="ValidateDateText(this,'LeaseStartDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calLeaseStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1" OnClientShown="onClientShown" OnClientHidden="onClientHide"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="txtLeaseStartDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtLeaseStartDate" ID="LeaseStartDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <span id="Label2" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="Label3" class="clsLabelAuto">Lease End Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLeaseEndDate" CssClass="clsTextBoxDate_Ajax" onchange="ValidateDateText(this,'LeaseEndDate_watermarkextender');"
                                                                ClientIDMode="Static" Width="100px" runat="server"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calLeaseEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                OnClientShown="onClientShown" OnClientHidden="onClientHide" Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="txtLeaseEndDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtLeaseEndDate" ID="LeaseEndDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblMinUtilHrs" class="clsLabelAuto">Min. Utilization Hrs.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMinHrs" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                ClientIDMode="Static" MaxLength="8">0</asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="Label5" class="clsLabelAuto">Currency</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCurrency" runat="server" CssClass="clsComboBoxMedium_Ajax"
                                                                DataTextField="Name" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="Label4" class="clsLabelAuto">Rate/Hrs. For Min. Utilization</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRateForMinHrs" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                ClientIDMode="Static" MaxLength="8">0</asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="Label6" class="clsLabelAuto">Rate/Hrs. Beyond Min. Utilization</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRateBeyondMinHrs" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                ClientIDMode="Static" MaxLength="8">0</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Leased Information Details</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlAddLeaseInfo" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnAdd" runat="server" Text="Add" ToolTip="Click to Add the Leased Information"
                                                                    OnClientClick="return CheckValidation();" CssClass="clsButton_Ajax"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgLeasedInformationList" runat="server" CssClass="clsGrid" ToolTip="Aircraft Leased Information List"
                                                            DataKeyNames="ID" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" PageSize="3"
                                                            AllowSorting="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID" SortExpression="LeasedType">
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LeasedType" HeaderText="Leased Type">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LeasedStartDateFormatted" HeaderText="Lease Start Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LeasedEndDateFormatted" HeaderText="Lease End Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MinUtilizationHrs" HeaderText="Min. Utilization Hrs.">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RateForMinUtilizationHrs" HeaderText="Rate ">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RateBeyondMinHrs" HeaderText="Rate Beyond Minimum Hrs">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CausesValidation="False" ToolTip="Click to Print the list of Certificates"
                                                            CssClass="clsButton_Ajax" Visible="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" Text="Back" CausesValidation="False" ToolTip="Click to go Previous page"
                                                            CssClass="clsButton_Ajax"></asp:Button>
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
    <script type="text/javascript">
        //Date validations

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $find('LeaseStartDate_watermarkextender').get_Text(); // $("#txtFromDate").val();
            var todate = $find('LeaseEndDate_watermarkextender').get_Text(); // $("#txtToDate").val();

            if (fromdate == "" || todate == "") {
                args.IsValid = true;
                return;
            }

            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate };
            $.ajax({
                type: "POST",
                url: "BetweenDateValidationHandler.ashx",
                cache: false,
                data: param,
                async: false,
                beforeSend: OnBeforeSnd,
                success: onSuces,
                error: onErr
            });

            function onSuces(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                if (result == "True") {
                    args.IsValid = true;
                    return;
                }

            }

            function onErr(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                source.errormessage = result;
                return;
            }
            function OnBeforeSnd() {
                $get("AjaxLoader").style.visibility = 'visible';
            }

        }

        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
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
    </form>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeLeaseInfoList();
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
        function onClientShown(sender, e) {
            window.parent.autoResizeLease();
        }
        function onClientHide(sender, e) {
            window.parent.autoResizeLeaseInfoList();
        }
    </script>
</body>
</html>
