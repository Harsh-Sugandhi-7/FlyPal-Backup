<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineMaintenanceList_Ajax.aspx.vb"
    Inherits="Flypal.wfMachineMaintenanceList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aircraft Maintenance</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <script src="jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
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
                            <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td colspan="4" class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Aircraft Maintenance</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                TabIndex="0" Text="Close" ToolTip="Click to Close" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="vsAircraftmaintainence" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a">
                                                </asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None" ValidationGroup="a">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                    ErrorMessage="To Date Required" ValidationGroup="a">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                    ErrorMessage="From Date Required" ValidationGroup="a">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required" ValidationGroup="a">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="115px">
                                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <table cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                                runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" 
                                                                ErrorMessage="From Date should not be greater than To Date ">
                                                            </asp:CustomValidator>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToDate" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                ClientIDMode="Static" runat="server"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="115px">
                                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                    DataTextField="RegNo" DataValueField="ID">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="115px">
                                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="ModelSerialNoPostion"
                                                    DataValueField="ID">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="115px">
                                                <asp:Label ID="lblMaintenaceActivity" runat="server" CssClass="clsLabelAuto">Maintenance Activity</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="cmbMaintenanceActivity" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                    DataTextField="Name" DataValueField="ID">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
                                                    CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
                                                    ValidationGroup="a" CausesValidation="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="dgMachineMaintenanceList" runat="server" AllowPaging="True" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    PageSize="50" ShowHeaderWhenEmpty="true">
                                                    <SelectedRowStyle Wrap="False" />
                                                    <EditRowStyle Wrap="False" />
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="MachineID" HeaderText="MachineID">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="MaintenanceActivityTypeID" HeaderText="MaintenanceActivityTypeID">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg No.">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MaintenanceActivityTypeName" SortExpression="MaintenanceActivityTypeName"
                                                            HeaderText="Maintenance Activity">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <FooterStyle Wrap="False"></FooterStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CodeNo" SortExpression="CodeNo" HeaderText="CodeNo">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Width="200px" CssClass="TextBreak" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DoneWONo" SortExpression="DoneWONo" HeaderText="WO No.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReferenceModNumber" SortExpression="ReferenceModNumber"
                                                            HeaderText="Reference/Directive No.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Width="150px" CssClass="TextBreak" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <FooterStyle Wrap="False"></FooterStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp. Info.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="MaintenanceID" HeaderText="MaintenanceID">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="LogID" HeaderText="LogID">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LogTextNo" SortExpression="LogNo" HeaderText="Log No.">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <FooterStyle Wrap="False"></FooterStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LogPageNo" SortExpression="LogPageNo" HeaderText="Log Page No.">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="AssemblyStatusID" HeaderText="AssemblyStatusID">
                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
        </div>
        <script type="text/javascript">

            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                var fromdate = $("#txtFromDate").val();
                var todate = $("#txtToDate").val();
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
        <script language="javascript" type="text/javascript">

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

                onResize();
            });

        </script>
    </form>
</body>
</html>
