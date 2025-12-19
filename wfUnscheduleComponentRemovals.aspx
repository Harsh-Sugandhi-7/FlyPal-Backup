<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUnscheduleComponentRemovals.aspx.vb"
    Inherits="Flypal.wfUnscheduleComponentRemovals" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <title></title>
    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/close.gif";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/detail.gif";
            }
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
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td class="clsFormHeader1">

                        <asp:Label CssClass="clsFormHeader" ID="lbltitle" runat="server">Unschedule Component Removals</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server">
                            <table id="tblInner">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                    CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                    ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                    ErrorMessage="From Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="a"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"
                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                                    ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" OnServerValidate="CustomValidate"
                                                    ValidationGroup="a"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Label CssClass="clsLabelHeader" ID="lblStep1" runat="server">Step I. Selection of Dates</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <asp:Label CssClass="clsLabelStar" ID="Label3" runat="server">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="clsLabel" ID="lblFromDate" runat="server">From Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtFromDate" ClientIDMode="Static"
                                                                AutoPostBack="true" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="clsLabelAuto" ID="lblToDate" runat="server">To Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtToDate" Style="margin-left: 3px;"
                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                                runat="server"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Label CssClass="clsLabelHeader" ID="lblStep2" runat="server">Step II. Selection of Aircraft</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="width: 2px">
                                                            <asp:Label CssClass="clsLabelStar" ID="lblAircraftStar1" runat="server">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="clsLabelAuto" ID="lblAircraft" runat="server">Aircraft </asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server"
                                                                DataTextField="RegNo" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>


                                <tr>
                                    <td style="height: 24px">
                                        <asp:Label CssClass="clsLabelHeader" ID="lblStep4" runat="server">Step III. Display Report</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="clsLabelAuto" ID="lblSummary" runat="server">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 2px"></td>
                                                        <td align="left">
                                                            <asp:Label CssClass="clsLabelAuto" ID="lblDateRangeFrom" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label CssClass="clsLabelAuto" ID="lblDateRangeTo" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 2px; height: 20px"></td>
                                                        <td style="height: 20px">
                                                            <asp:Label CssClass="clsLabelAuto" ID="lblAircraft1" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                        <td style="height: 20px"></td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlGrid" runat="server" CssClass="clspanel1" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal" ID="dgUnscheduleComponentRemovalsList" runat="server"
                                                    AutoGenerateColumns="False" AllowSorting="true" EmptyDataText="No Records Found..."
                                                    DataKeyNames="PartID" ShowHeaderWhenEmpty="false" PageSize="10">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <HeaderTemplate>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <div>
                                                                    <a href="javascript:showNestedGridView('ID-<%# Eval("PartID") %>');">
                                                                        <img id="imageID-<%# Eval("PartID") %>" alt="Click to show/hide Type" border="0"
                                                                            src="images/detail.gif" />
                                                                    </a>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField Visible="False" DataField="PartID" HeaderText="PartID"></asp:BoundField>
                                                        <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="True" DataField="PartDescription" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NoOfOccurrence" HeaderText="NoOfOccurrence">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CurrentlyInstCount" HeaderText="Currently Inst.">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AvgFailedAt" HeaderText="Avg. Failed At" HtmlEncode="false" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="Right">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td colspan="95%" bgcolor="White" width="0px">
                                                                        <div id="ID-<%# Eval("PartID") %>" style="display: none; position: relative; left: 17px">
                                                                            <panel>
                                                                                <table width="95%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label CssClass="clsLabelHeaderItem" ID="lblDetails" runat="server">In Detail(s) </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="100%" bgcolor="White" width="0px">
                                                                                            <asp:GridView ID="grdUnscheduleComponentRemovalsDetails" runat="server" AutoGenerateColumns="False" Width="99.9%" DataKeyNames="CompStatusID"
                                                                                                GridLines="None" CellPadding="0" ForeColor="#333333" CssClass="clsGrid"
                                                                                                AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                                                SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="CompStatusID" HeaderText="CompStatusID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="ATACode" HeaderText="ATA" HeaderStyle-HorizontalAlign="Left">
                                                                                                        <HeaderStyle Width="10px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No." HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Removedon" HeaderText="Removedon" HeaderStyle-HorizontalAlign="Left">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="PeriodValues" HeaderText="Removal Values." HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="Right" HtmlEncode="false">
                                                                                                        <HeaderStyle Width="100px" />
                                                                                                        <ItemStyle Width="100px" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--  <asp:BoundField DataField="FreqValues" HeaderText="Freq." HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign ="Right"  HtmlEncode="false">
                                                                                          <HeaderStyle Width="100px"  />
                                                                                           <ItemStyle Width="100px"  />
                                                                                    </asp:BoundField> --%>
                                                                                                    <asp:BoundField DataField="TSOValues" HeaderText="TSO" HeaderStyle-HorizontalAlign="right" HtmlEncode="false"
                                                                                                        ItemStyle-HorizontalAlign="Right">
                                                                                                        <HeaderStyle Width="90px" />
                                                                                                        <ItemStyle Width="90px" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="TSIValues" HeaderText="TSI" HeaderStyle-HorizontalAlign="right" HtmlEncode="false"
                                                                                                        ItemStyle-HorizontalAlign="Right">
                                                                                                        <HeaderStyle Width="90px" />
                                                                                                        <ItemStyle Width="90px" />
                                                                                                    </asp:BoundField>

                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label CssClass="clsLabelHeaderItem" ID="lblCompDetails" runat="server">In Detail(s) </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="100%" bgcolor="White" width="0px">
                                                                                            <asp:GridView ID="grdInstComponentDetails" runat="server" AutoGenerateColumns="False" Width="99.9%" DataKeyNames="CompStatusID"
                                                                                                GridLines="None" CellPadding="0" ForeColor="#333333" CssClass="clsGrid"
                                                                                                AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                                                SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="CompStatusID" HeaderText="CompStatusID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="ATACode" HeaderText="ATA" HeaderStyle-HorizontalAlign="Left">
                                                                                                        <HeaderStyle Width="10px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="CompSerialNo" HeaderText="Serial No." HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On" HeaderStyle-HorizontalAlign="Left">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="ValueFormatted" HeaderText="Inst. Values." HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="Right" HtmlEncode="false">
                                                                                                        <HeaderStyle Width="90px" />
                                                                                                        <ItemStyle Width="90px" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="TSNFormatted" HeaderText="Current Values" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="Right" HtmlEncode="false">
                                                                                                        <HeaderStyle Width="90px" />
                                                                                                        <ItemStyle Width="90px" />
                                                                                                    </asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </panel>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>

                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:UpdatePanel CssClass="clspanel1" ID="upnlButtons" runat="server">
                            <ContentTemplate>
                                <table cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnDisplay" TabIndex="0" runat="server"
                                                ValidationGroup="a" Text="Show Removals" ToolTip="Click to Unschedule Component Removals"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" Text="Close" CausesValidation="False"
                                                ToolTip="Click to close the Unschedule Component Removals"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
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
        <div>
            <%--Date Validations--%>
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
        </div>
    </form>
</body>
</html>
