<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTechDirection.aspx.vb"
    Inherits="Flypal.wfTechDirection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Technical Direction</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
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
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                        <table id="tblinner" class="clsTablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle" class="clsFormHeader">Technical Direction</span>
                                            </td>
                                            <td align="right" valign="top">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save information of Technical Direction" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                        Text="Print" ToolTip="Click to Print the Technical Direction" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                        Text="Back" ToolTip="Click to go Back to Previous Page" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                        </tr>


                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                                InitialValue="<%$AppSettings:DateFormat%>" ErrorMessage="Date Required" ControlToValidate="txtFromDate"
                                                Display="None"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clslabelauto"
                                                ErrorMessage="Date Required" validateEmptyText="true" ControlToValidate="txtFromDate"
                                                Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clslabelauto" Display="None"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="110px">
                                                                    <span class="clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                                        AutoPostBack="true" Text="<%# mrptTechDirection.DateFormatted %>" CausesValidation="true"
                                                                        onchange="ValidateDateText(this,'Calender_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ClientIDMode="Static" TargetControlID="txtFromDate"
                                                                        ID="Calender_watermarkextender" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td colspan="1">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIsNoteRequired" runat="server" Checked="<%# mrptTechDirection.IsNoteRequired %>"
                                                                        CssClass="clsCheckBox" Text="External TD" ClientIDMode="Static" AutoPostBack="true" />
                                                                </td>
                                                                <td valign="bottom">
                                                                    <span class="clsLabelAuto">(Check if TD is External)</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="110px">
                                                                    <span class="clsLabelAuto">Sr. No.</span>
                                                                </td>
                                                                <td>
                                                                    <table border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtText" runat="server" Text="<%# mrptTechDirection.Text %>" CssClass="clsTextBoxTagSearch"
                                                                                    ToolTip="Enter No." MaxLength="50" Width="208px" ReadOnly="true" BackColor="Gainsboro"> </asp:TextBox>
                                                                            </td>
                                                                            <td style="margin-left: 3px;">
                                                                                <asp:TextBox ID="txtNo" runat="server" Text="<%# mrptTechDirection.No %>" ReadOnly="true"
                                                                                    BackColor="Gainsboro" CssClass="clsTextBoxTagSearchSmall" MaxLength="8"> </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span class="clsLabelAuto" style="margin-left: 3px">From</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblRemovalValues" class="clsLabelAuto">To</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbLocation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                        DataValueField="ID" DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtFrom" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mrptTechDirection.From %>"
                                                            TextMode="MultiLine" Width="385px" />
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtTo" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mrptTechDirection.To %>"
                                                            TextMode="MultiLine" Width="385px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="110px">
                                                        <span class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartNo" CssClass="clsTextBoxTagSearch" ReadOnly="true"
                                                            BackColor="Gainsboro" Text="<%# mrptTechDirection.PartNo %>" />
                                                    </td>
                                                    <td>
                                                        <span class="clsLabelAuto">Model Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtModel" CssClass="clsTextBoxTagSearch" ReadOnly="true"
                                                            BackColor="Gainsboro" Text="<%# mrptTechDirection.ModelName %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtSerialNo" CssClass="clsTextBoxTagSearch" ReadOnly="true"
                                                            BackColor="Gainsboro" Text="<%# mrptTechDirection.SerialNo %>" />
                                                    </td>
                                                    <td>
                                                        <span class="clsLabelAuto">Removed From</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAircaft" CssClass="clsTextBoxTagSearch" ReadOnly="true"
                                                            BackColor="Gainsboro" Text="<%# mrptTechDirection.AircaftName %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearch"
                                                            ReadOnly="true" Text="<%# mrptTechDirection.Description %>" />
                                                    </td>
                                                    <td>
                                                        <span class="clsLabelAuto">Removal Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemovalDate" runat="server" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                            BackColor="Gainsboro" ReadOnly="true" Enabled="false" AutoPostBack="true" Text="<%# mrptTechDirection.RemovalDateFormatted %>"
                                                            CausesValidation="true" onchange="ValidateDateText(this,'Calender_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calRemovalDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRemovalDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ClientIDMode="Static" TargetControlID="txtRemovalDate"
                                                            ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Aircraft S/N</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAircarftSrNo" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearch"
                                                            ReadOnly="true" Text="<%# mrptTechDirection.AircaftSrNo %>" />
                                                    </td>
                                                    <td>
                                                        <span class="clsLabelAuto">ATA Chapter</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtATA" CssClass="clsTextBoxTagSearch" ReadOnly="true"
                                                            BackColor="Gainsboro" Text="<%# mrptTechDirection.ATA %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Approved Life</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtOHFreq" CssClass="clsTextBoxTagSearch" ReadOnly="true"
                                                            BackColor="Gainsboro" Text="<%# mrptTechDirection.OHFreq %>" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTimeSince" runat="server" CssClass="clsLabelAuto">Time Since New</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtTimeSinceNew" CssClass="clsTextBoxTagSearch" ReadOnly="true"
                                                            BackColor="Gainsboro" Text="<%# mrptTechDirection.TimeSinceNew %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Removal Type</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsRemUnscheduled" runat="server" Checked="<%# mrptTechDirection.IsRemUnschedule %>"
                                                            AutoPostBack="true" CssClass="clsCheckBox" Enabled="false" Text="Un-Schedule(for reliability monitoring)" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Visible="<%# mrptTechDirection.TypeID = 1%>">Time Since Overhaul</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtTimeSinceOverhaul" CssClass="clsTextBox3_Ajax"
                                                            Visible="<%# mrptTechDirection.TypeID = 1%>" ReadOnly="true" BackColor="Gainsboro"
                                                            Text="<%# mrptTechDirection.TimeSinceOverhaul %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Postion</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPostion" runat="server" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                            BackColor="Gainsboro" ReadOnly="true" Text="<%# mrptTechDirection.Position %>"   
                                                            CausesValidation="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" CssClass="clsLabelAuto" Visible="<%# mrptTechDirection.TypeID = 1%>">Due On</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDueOn" CssClass="clsTextBox3_Ajax" ReadOnly="true"
                                                            Visible="<%# mrptTechDirection.TypeID = 1%>" BackColor="Gainsboro" Text="<%# mrptTechDirection.DueOn %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label runat="server" CssClass="clsLabelHeader" Visible="<%# mrptTechDirection.TypeID = 2%>">Maintenance Activity List For Component</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:GridView ID="dgMaintenanceActivityList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            Visible="<%# mrptTechDirection.TypeID = 2%>" ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            <Columns>
                                                                <asp:BoundField DataField="MonitorType" HeaderText="Maintenance Activity" SortExpression="MonitorType">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneOnDate" HeaderText="Done On" SortExpression="DoneOnDate">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Freq1" HeaderText="Frequency" SortExpression="Freq1" HtmlEncode="false">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneOnValue" HeaderText="Done On Value" SortExpression="DoneOnValue"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ElapsedTime" HeaderText="Elapsed Value" SortExpression="ElapsedTime"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Extension" HeaderText="Extension Value" SortExpression="Extension"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DueAsof" HeaderText="Due At" SortExpression="DueAsof"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Removal Reason</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtRemovalReason" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            Text="<%# mrptTechDirection.RemovalReason %>" TextMode="MultiLine" Width="580px"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Work Required</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtWorkRequired" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            Text="<%# mrptTechDirection.WorkRequired %>" TextMode="MultiLine" Width="580px"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Reports Required</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtReportsRequired" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            Text="<%# mrptTechDirection.ReportsRequired %>" TextMode="MultiLine" Width="580px"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">External TD Note</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            Enabled="<%# iif(chkIsNoteRequired.checked,True,False) %>" ClientIDMode="Static"
                                                            Text="<%# mrptTechDirection.Note %>" TextMode="MultiLine" Width="580px"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            ClientIDMode="Static" Text="<%# mrptTechDirection.Remark %>" TextMode="MultiLine" Width="580px"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right" valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save information of Technical Direction" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Text="Print" ToolTip="Click to Print the Technical Direction" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Text="Back" ToolTip="Click to go Back to Previous Page" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkIsNoteRequired").change(function () {
                $("#txtNote").prop("disabled", !this.checked);
            });

        });
    </script>
    </form>
</body>
</html>
