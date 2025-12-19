<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForService_Ajax.aspx.vb" Inherits="Flypal.wfSearchCriteriaForService_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Service Status Report</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="3" class="clsFormHeader1">
                                    <span id="lbltitle" class="clsFormHeader"><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Search criteria for MPD Report", "Search criteria for Service Report") %></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlValidations" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>

                                            <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ClientValidationFunction="ValidateAircraft"
                                                Display="None" ControlToValidate="cmbAircraft" ErrorMessage="Please select the Aircraft"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" ClientValidationFunction="ValidateService"
                                                Display="None" ControlToValidate="cmbServiceType" ErrorMessage='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Please select the MPD", "Please select the Service") %>'></asp:CustomValidator>


                                            <%-- Client side validation for comboboxes--%>
                                            <script type="text/javascript">
                                                //Aircraft List
                                                function ValidateAircraft(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbAircraft");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;

                                                    }

                                                }

                                                //Service List
                                                function ValidateService(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbServiceType");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;

                                                    }

                                                }
                                            </script>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnDisplay" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlMachine" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td style="width: 20px"></td>
                                                    <td style="width: 90px">
                                                        <span id="lblFromDate" class="clsLabelAuto">As On Date</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
                                                            runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="20px">
                                                        <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td align="left" width="90px">
                                                        <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="RegNo"
                                                            DataValueField="ID" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="20px"></td>
                                                    <td align="left" width="90px">
                                                        <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="ModelSerialNoPostion"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkAirframeDueAsOf" runat="server" CssClass="clsCheckBox" Text="Show Due As Of Airframe Values" />
                                                    </td>
                                                </tr>

                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep4" class="clsLabelHeader" runat="server"><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Step IV. Selection of Maintenance Event", " Step IV. Selection ofServices") %></span>
                                </td>
                            </tr>
                            <tr>

                                <td align="left" width="20px">
                                    <span id="lblTypeStar1" class="clsLabelStar">*</span>
                                </td>
                                <td align="left">
                                    <span id="lblType" class="clsLabelAuto" runat="server"><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %></span>
                                </td>
                                 
                                <td align="left">
                                    <asp:DropDownList ID="cmbServiceType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="Name"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="Label2" class="clsLabelHeader">Step V. Bottom Line of Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="Label3" class="clsLabelAuto">Enter Line which you want to print at the bottom of the report.</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:TextBox ID="txtBottomLine" runat="server" CssClass="clsTextBoxMultilineTask_Ajax"
                                        Width="552px" MaxLength="500" TextMode="MultiLine" ToolTip="Enter Note">I hereby certify that the data specified above has been certified throughout : Engineering Department Manager : ____________________   Date : __________ </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep6" class="clsLabelHeader">Step VI. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="20px"></td>
                                <td colspan="2" align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" width="20px"></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="20px"></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="20px"></td>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="20px"></td>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>

                                <td colspan="3" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                            ToolTip="Click to Display Current Searching criterias." Text="Current Criteria"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" ToolTip="Click to Display Report"
                                                            Text="Display"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH" ToolTip="Click to Close"
                                                            Text="Close" CausesValidation="False"></asp:Button>
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
    </form>
</body>
</html>
