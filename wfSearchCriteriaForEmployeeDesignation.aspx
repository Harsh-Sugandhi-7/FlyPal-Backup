<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeDesignation.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForEmployeeDesignation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<%--<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>--%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Log Parameter </title>

    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" --> 
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }
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
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">

         <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox runat="server" ID="MSGBoxCtrl" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <p>
    </p>
    <table class="clstablelistout" id="tblmain">

       

        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="6" class="clsFormHeader1Newstyle">
                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Employee Designation  Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto">Date Range</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                    <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                    <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                    <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                    <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                    <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                    <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="4">
                                <table id="Table1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromDate" runat="server" Width="66px" CssClass="clsLabelAuto" Visible="False">From Date</asp:Label>
                                        </td>
                                        <td>
                                            <%--<uc1:SICalendar ID="txtFromDate" runat="server"></uc1:SICalendar>--%>

                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="clsTextBoxTagSearchDate" Height="25px"
                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                       
                                            <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                            </cc2:CalendarExtender>

                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>

                                        </td>
                                        <td>
                                            <asp:Label ID="lblToDate" runat="server" Width="52px" CssClass="clsLabelAuto">To Date</asp:Label>
                                        </td>
                                        <td>
                                            <%--<uc1:SICalendar ID="txtToDate" runat="server"></uc1:SICalendar>--%>

                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="clsTextBoxTagSearchDate" Height="25px"
                                                onchange="ValidateDateText(this,'ToDate_WatermarkExtender');"></asp:TextBox>
                                       
                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                            </cc2:CalendarExtender>

                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_WatermarkExtender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step II. Selection of Employee</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblCrew" runat="server" CssClass="clsLabelAuto">Employee</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbCrewList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="EmpNoName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabel"></asp:Label>
                            </td>
                            <td align="left">
                            </td>
                            <td align="left">
                                <asp:Label ID="Label4" runat="server" CssClass="clsLabel"></asp:Label>
                            </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Designation</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblDesignation" runat="server" CssClass="clsLabelAuto">Designation</asp:Label>
                            </td>
                            <td align="left" colspan="5">
                                <asp:DropDownList ID="cmbDesignation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="lblStepIV" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="lblCrewName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="lblDesignation1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="6">
                                <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                    Text="Current Criteria" ToolTip="Click to display Current Searching criterias.">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Display"
                                                    ToolTip="Click to Display Report"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                    ToolTip="Click to close Employee Designation screen" CausesValidation="False">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
