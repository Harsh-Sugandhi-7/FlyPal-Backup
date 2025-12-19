<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptEngineOilConsumptionForMonth_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptEngineOilConsumptionForMonth_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <%--   <script src="json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script src="FusionCharts/fusioncharts.js" type="text/javascript"></script>--%>
    <script language="javascript">
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
    </script>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblInner" class="clstablelistin" border="0">
                                <tr>
                                    <td colspan="3">
                                        <span id="lbltitle" class="clstitle1">Engine Oil Consumption For Month </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select the Aircraft"
                                            ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvAssembly" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select the Assembly"
                                            ControlToValidate="cmbAssemblyList" Display="None" ClientValidationFunction="ValidateAssembly"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="lblStep2" class="clsLabelHeader">Step I. Selection of Month and Year</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                    </td>
                                    <td align="left">
                                        <span id="lblYear" class="clsLabelAuto">Month and Year</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbMonth" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox1_Ajax" Width="112px"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                    </td>
                                    <td>
                                        <span id="lblModel" class="clsLabelAuto">Aircraft</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbAircraft" ClientIDMode="Static" runat="server" CssClass="clsComboBox3_Ajax"
                                            AutoPostBack="true" DataTextField="RegNo" DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="Label3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>  <span id="Span1" class="clsLabelStar">*</span>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbAssemblyList" runat="server" CssClass="clsComboBox3_Ajax"
                                            DataTextField="ModelSerialNoPostion" DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <asp:Label ID="lblSummaryDetail" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Summary/Detail
                                            Report</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:RadioButton ID="rdoDetail" runat="server" Checked="true" Text="Detail" GroupName="A"
                                            CssClass="clsCheckBox" />
                                        <asp:RadioButton ID="rdoSummary" runat="server" Text="Summary (6 Days)" GroupName="A" CssClass="clsCheckBox" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="left">
                                                        </td>
                                                        <td colspan="2" align="left">
                                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="False">Your selection is as follows </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblyear1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                        </td>
                                                        <td colspan="2" align="left">
                                                            <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                        </td>
                                                        <td colspan="2" align="left">
                                                            <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" valign="top">
                                        <asp:UpdatePanel ID="upnlLine" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Chart ID="ChartLine" runat="server" BackColor="WhiteSmoke" borderdashstyle="Solid"
                                                    Height="400px" BorderWidth="2px" Visible="False" Width="800px" ClientIDMode="Static">
                                                    <Titles>
                                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                                            Text="Oil Consumption Report" ForeColor="26, 59, 105" Alignment="TopCenter">
                                                        </asp:Title>
                                                    </Titles>
                                                    <Legends>
                                                        <asp:Legend BackColor="Transparent" Enabled="True" Font="Verdana, 5.25pt, style=Bold"
                                                            IsTextAutoFit="True" Name="Default">
                                                        </asp:Legend>
                                                    </Legends>
                                                    <BorderSkin SkinStyle="Raised"></BorderSkin>
                                                    <Series>
                                                        <asp:Series Name="TrendLine" CustomProperties="PixelPointWidth=5" ChartType="Line"
                                                            Color="Red" IsVisibleInLegend="true">
                                                            <EmptyPointStyle IsValueShownAsLabel="false" IsVisibleInLegend="false" />
                                                        </asp:Series>
                                                    </Series>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                                            BackColor="#D3DFF0" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                                            <AxisY LineColor="64, 64, 64, 64" IntervalAutoMode="FixedCount" IntervalType="Number"
                                                                Title="Quart/Hour Rate" IsStartedFromZero="True" IsLabelAutoFit="true">
                                                                <LabelStyle Font="Verdana, 7pt, style=Bold" />
                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                            </AxisY>
                                                            <AxisX LineColor="64, 64, 64, 64" Title="Log Date" IntervalAutoMode="FixedCount"
                                                                Interval="1" IsStartedFromZero="True">
                                                                <LabelStyle Font="Verdana, 7pt, style=Bold" Angle="0" Interval="1" />
                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                            </AxisX>
                                                        </asp:ChartArea>
                                                    </ChartAreas>
                                                </asp:Chart>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlActions">
                                            <ContentTemplate>
                                                <table border="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                                ToolTip="Click to Display Current Searching criterias" Text="Current Criteria"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" TabIndex="1" runat="server" CssClass="clsButton_Ajax"
                                                                ToolTip="Click to Display Report" Text="Display Here"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPrint" TabIndex="2" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Report"
                                                                Text="Print"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close the Engine Oil Consumption For Month screen"
                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <script type="text/javascript">
        //Aircraft validation
        function ValidateAircraft(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAircraft");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;

            }

        }
        function ValidateAssembly(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAssemblyList");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;

            }

        }
    </script>
    <%-- <script src="FusionCharts/fusioncharts.charts.js" type="text/javascript"></script>
    <script src="FusionCharts/themes/fusioncharts.theme.fint.js" type="text/javascript"></script>
    <script type="text/javascript">
        function FusionChartLineFunc(Eng1HourRate, Eng2HourRate, LogDate) {
            var revenueChart = new FusionCharts({
                "type": "msline",
                "renderAt": "MELPirepsChart",
                "dataFormat": "json",
                "width": '550',
                "height": '350',
                "dataSource": {
                    "chart": {
                        "caption": "Total Consumption per Year",
                        "subCaption": $("#cmbYear :selected").text() + ' for ' + $("#cmbAircraft :selected").text(),
                        "xAxisName": "Log Date",
                        "yAxisName": "Quart/Hour Rate",
                        "yaxismaxvalue": "1.28",
                        "yAxisValuesPadding": "0.06",
                        "legendshadow": "0",
                        "canvasborderalpha": "0",
                        "canvasbordercolor": "CCCCCC",
                        "canvasborderthickness": "1",
                        "showborder": "0",
                        "showexportdatamenuitem": "1",
                        "showDivLineSecondaryValue": "1",
                        "showSecondaryLimits": "1"
                    },
                    //"data": JSON.parse(PierpsMELGraphCount)
                    "categories": [{
                        "category": [{
                            "label": JSON.parse(LogDate)
                        }
                       ]
                    }],
                    "dataset": [

                                 {
                                     "seriesname": "Eng1",
                                     "color": "DBDC25",
                                     "anchorbordercolor": "DBDC25",
                                     "anchorbgcolor": "DBDC25",
                                     "data": JSON.parse(Eng1HourRate)
                                 },
                                {
                                    "seriesname": "Eng2",
                                    "color": "2AD62A",
                                    "anchorbordercolor": "2AD62A",
                                    "anchorbgcolor": "2AD62A",
                                    "data": JSON.parse(Eng2HourRate)
                                }

                            ],
                    "trendlines": [
                                         {
                                             "line": [
                                                        {
                                                            "startvalue": "1.28",
                                                            "endvalue": "",
                                                            "istrendzone": "",
                                                            "valueonright": "1",
                                                            "color": "#ff0000",
                                                            "displayvalue": "RedLine",
                                                            "showontop": "1",
                                                            "thickness": "2",
                                                            "origText": "High"
                                                        }
                                                    ]
                                         }
                                 ]
                }

            });

            revenueChart.render();
        }
        
    </script>--%>
    </form>
</body>
</html>
