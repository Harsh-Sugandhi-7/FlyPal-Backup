<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfServiceabilityBulkEntry_Ajax.aspx.vb"
    Inherits="Flypal.wfServiceabilityBulkEntry_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Serviceability</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="3">
                                <span id="lbltitle" class="clstitle1">Aircraft Serviceability</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <span id="lblFrom" class="clsLabelAuto">From</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                        <td align="left">
                                            <span id="lblTo" class="clsLabelAuto">To</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkSelectAll" CssClass="clsRadioButton" runat="server" Text="Select All" />
                                <script type="text/javascript">
                                    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                                        $("#<%=chkSelectAll.ClientID %>").click(function () {
                                            var status = $("#<%=chkSelectAll.ClientID %>").attr("checked");
                                            $("#<%=ChklistAircraft.ClientID %>").find(":checkbox").each(function () {
                                                if (status == "checked") {
                                                    $(this).attr("checked", status);
                                                }
                                                else {
                                                    $(this).removeAttr("checked");
                                                }

                                            });
                                        });
                                        return false;
                                    });
                                               
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                            </td>
                            <td align="left">
                                <asp:CheckBoxList ID="ChklistAircraft" runat="server" CssClass="clsComboBox" DataTextField="RegNo"
                                    DataValueField="ID" RepeatColumns="4" RepeatDirection="Horizontal" Width="500px">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <fieldset class="clsFieldSet" style="border-width: 1px">
                                    <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Serviceability/Schedule/Un-Schedule
                                        Detail</b></legend>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="chkServiceability" onclick="CheckDayPercent(this);" runat="server"
                                                    Checked="true" Text="Serviceability" CssClass="clsCheckBox" GroupName="A" name="SerGrp">
                                                </asp:RadioButton>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="chkSchedule" runat="server" GroupName="A" Text="Schedule" name="SerGrp"
                                                    CssClass="clsCheckBox" onclick="CheckDayPercent(this);"></asp:RadioButton>
                                            </td>
                                            <td colspan="1">
                                                <asp:RadioButton ID="chkUnSchedule" onclick="CheckDayPercent(this);" runat="server"
                                                    Text="Un-Schedule" CssClass="clsCheckBox" GroupName="A" name="SerGrp"></asp:RadioButton>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td>
                                         <label id="lbl1" runat="server" style ="height:100px;display:none"></label>
                                        </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span1" class="clsLabel">Day %</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtDayPercent" runat="server" Width="100px" CssClass="clsTextBox_Ajax"
                                                    ClientIDMode="Static" MaxLength="10">
                                                </asp:TextBox>
                                                <span id="Span4" class="clsLabel">(in case Schedule/UnSchedule)</span>
                                            </td>
                                        </tr>
                                        <%--Added by shital on 17-Dec-2021 For TSL--%>
                                        <asp:PlaceHolder runat="server" ID="phUnScheduleCatagory"   Visible='<%# AppSettings("ClientCode")="TSL" %>'>
                                         <tr>
                                            <td>
                                                <span id="Span5" class="clsLabel">UnSchedule Catagory</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="cmbUnscheduleCatagory" runat="server" CssClass="clsComboBoxSmall" ClientIDMode="Static"
                                                   DataValueField="ID" DataTextField="Name" DataSource="<%# munscheduleCatagoryList %>" Enabled="false">
                                                    </asp:DropDownList>
                                               
                                                <span id="Span6" class="clsLabel">(in case UnSchedule)</span>
                                            </td>
                                        </tr>
                                        </asp:PlaceHolder>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Span2" class="clsLabel" style="display: none;">Priority</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                    Style="display: none;" DataValueField="ID" onchange="CheckDuplicatePriority();"
                                    DataTextField="Name" DataSource="<%# mServiceabilityPriorityList %>">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblDuplicatePriority" runat="server" ForeColor="Red" class="clsLabel"
                                    Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="Span3" class="clsLabel">Remark</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBox1_Ajax" MaxLength="500"
                                    TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="3">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" align="right">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsButton" Text="Update"
                                                        ToolTip="Click to save Serviceability Entry"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to close "
                                                        CausesValidation="False"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    <%--Date Validations--%>
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
    <script type="text/javascript">
//        function CheckDayPercent(myradio) {
//            var td = $("td", $(myradio).closest("tr"));
//            //var inputs = document.getElementById('dgServiciabilityDetailList$' + myradio.id.split("_")[1] + '$txtDayPercent');
//            if (myradio.value == "chkServiceability") {
//                $("#txtDayPercent").val('100.00');
//            }
//            else if (myradio.value == "chkSchedule" || myradio.value == "chkUnSchedule") {
//                $("#txtDayPercent").val('0');

//                //alert(txtDayPercent.value);
//            }
//            //  alert(myradio.value);
        //        }

        function CheckDayPercent(myradio) {
            var td = $("td", $(myradio).closest("tr"));
            var inputs = document.getElementById('dgServiciabilityDetailList$' + myradio.id.split("_")[1] + '$txtDayPercent');
            if (myradio.value == "chkServiceability") {
                $("#txtDayPercent").val('100.00');
                $("#cmbUnscheduleCatagory").attr('disabled', true);
                $("#cmbUnscheduleCatagory").val(0);
            }
            else if (myradio.value == "chkSchedule") {
                $("#txtDayPercent").val('0');
                $("#cmbUnscheduleCatagory").attr('disabled', true);
                $("#cmbUnscheduleCatagory").val(0);
            }
            else if (myradio.value == "chkUnSchedule") {
                $("#txtDayPercent").val('0');
                $("#cmbUnscheduleCatagory").attr('disabled', false);
            }
        }
    </script>
    </form>
</body>
</html>
