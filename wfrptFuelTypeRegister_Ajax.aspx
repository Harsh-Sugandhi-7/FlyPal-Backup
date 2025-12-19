<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptFuelTypeRegister_Ajax.aspx.vb" Inherits="Flypal.wfrptFuelTypeRegister_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Fuel Type Register</title>
    
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
     <script id="clientEventHandlersJS" language="javascript">
     
         function openFile() {
             str = "wfExportToExcel.aspx"
             window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
         }
    </script>
</head>
<body bottomMargin="5" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
	    <tr>
		    <td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
				    <TABLE id="tblInner" class="clstablelistin">
					    <TR>
                            <td colspan="4" class="clsFormHeader1Newstyle">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Fuel Type Register</span>
                                        </td>

                                        <%--<td align="right" colspan="4">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server"
                                                                    TabIndex="0" Text="Current Criteria"
                                                                    ToolTip="Click to display current searching criterias" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" TabIndex="0" runat="server" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                    Text="Export to Excel" ToolTip="Click to Export report"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" TabIndex="0"
                                                                    Text="Display" ToolTip="Click to display report" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" CausesValidation="False"
                                                                    TabIndex="0" Text="Close"
                                                                    ToolTip="Click to Close Fuel Type Register Screen." />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>--%>

                                    </tr>
                                </table>

                            </td>
					    </TR>
					    <TR>
						    <TD colSpan="4">
							    <asp:ValidationSummary id="Validationsummary2" Runat="server" HeaderText="Fill Up The Following Fields"
								    Cssclass="clsValidationSummary"></asp:ValidationSummary>
							    
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                    ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                    ErrorMessage="To Date Required."></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation"
                                    Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please select the Aircraft."
                                        ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>

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
                                </script>
                            </TD>
					    </TR>
					    <TR>
						    <TD colSpan="4">
							    <span id="lblStep1" Class="clsLabelHeader">Step I. Selection of Dates</span>
                            </TD>
					    </TR>
					    <TR>
						    <TD>
							    <span id="lblFromDate" Class="clsLabelAuto">From Date</span></TD>
						    <TD>
							    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                            </TD>
						    <TD>
							    <span id="lblToDate" class="clsLabelAuto">To Date</span>
                            </TD>
						    <TD>
                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                    runat="server" CausesValidation="true"></asp:TextBox>
                                <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
							</TD>
					    </TR>
					    <tr>
						    <td colspan="4" align="left">
							    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                            </td>
					    </tr>
					    <tr>
						    <td align="left">
							    <span id="lblAircraft" class="clsLabelAuto">Aircraft </span></td>
						    <td colspan="3" align="left">
							    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbAircraft" runat="server" DataValueField="ID" DataTextField="RegNo"></asp:DropDownList>
                            </td>
					    </tr>
					    <tr>
						    <td colspan="4" align="left">
							    <span id="lblStepIII" class="clsLabelHeader">Step III. Display Report</span>
                            </td>
					    </tr>
					    <tr>
						    <td colspan="4" align="left">
							    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
					    </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
						                        <td align="left">
							                        <asp:label id="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></td>
						                        <td align="left">
							                        <asp:label id="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></td>
					                        </tr>
					                        <tr>
						                        <td align="left">
							                        <asp:label id="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></td>
						                        <td align="left"></td>
					                        </tr>
                                        </table>    
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
					    
					    <TR>
						    <td align="right" colspan="4">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" 
                                                        tabIndex="0" Text="Current Criteria" 
                                                        ToolTip="Click to display current searching criterias" />
                                                </td>
                                                  <td>
                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                        Text="Export to Excel" ToolTip="Click to Export report" ></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" tabIndex="0" 
                                                        Text="Display" ToolTip="Click to display report" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False" 
                                                         tabIndex="0" Text="Close" 
                                                        ToolTip="Click to Close Fuel Type Register Screen." />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel> 
                            </td>
                        </TR> 
				    </TABLE>
			    </asp:panel>
            </td>
	    </tr>
    </table>
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
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
