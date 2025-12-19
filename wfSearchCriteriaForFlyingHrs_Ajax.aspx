<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForFlyingHrs_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForFlyingHrs_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lead Time Analysis</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link    id="MainStyle" type="text/css" rel="stylesheet">
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
        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="clstablelistout" id="tblmain">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                                <table class="clstablelistin" id="tblInner">
                                    <tr>
                                        <td colspan="2" >
                                            <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Search criteria for Flying Hours</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <%--   <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                                ErrorMessage="Select the Aircraft" ControlToValidate="cmbAircraft" Display="None"></asp:CustomValidator>--%>
                                            <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" >
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Year</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblYear" runat="server" CssClass="clsLabelAuto">Year</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                            ClientIDMode="Static">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step II. Selection of Graph Type</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbPieGraph" runat="server" CssClass="clsRadioButton" ToolTip="Select Pie Graph"
                                                            Text="Pie Graph" GroupName="Graph" Checked="True" AutoPostBack="True"></asp:RadioButton>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbBarGraph" runat="server" CssClass="clsRadioButton" ToolTip="Select Bar Graph"
                                                            Text="Bar Graph" GroupName="Graph" AutoPostBack="True"></asp:RadioButton>
                                                        <asp:RadioButton ID="rdbLineGraph" runat="server" CssClass="clsRadioButton" ToolTip="Select Line Graph"
                                                            Text="Line Graph" GroupName="Graph" AutoPostBack="True"></asp:RadioButton>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step III. Selection of Aircraft</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" DataTextField="RegNo"
                                                            AutoPostBack="true" DataValueField="MachineID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblYear1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                                            ToolTip="Click to Display Current Searching criterias." Text="Current Criteria"
                                                                            CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to Display Report"
                                                                            Text="Display"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to Close"
                                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:LinkButton runat="server" CssClass="clsLabel" ID="lnkExportToPDF" 
                                                            Text="Export To PDF" Visible="false" ></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlLine" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Chart ID="ChartLine" runat="server" BackColor="WhiteSmoke" borderdashstyle="Solid"
                                                                    BorderWidth="2px" Visible="False" Width="800px" ClientIDMode="Static">
                                                                    <Titles>
                                                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                                                            Text="Total Time(Hrs) per Month" ForeColor="26, 59, 105" Alignment="TopCenter">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <Legends>
                                                                        <asp:Legend BackColor="Transparent" Enabled="False" Font="Verdana, 5.25pt, style=Bold"
                                                                            IsTextAutoFit="True" Name="Default">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                                                    <Series>
                                                                        <asp:Series Name="Series1" CustomProperties="PixelPointWidth=10" ChartType="Line">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                                                            BackColor="#D3DFF0" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                                                            <AxisY LineColor="64, 64, 64, 64" IntervalAutoMode="FixedCount" IntervalType="Auto"
                                                                                Title="Total Time(Hrs)" IsStartedFromZero="True" IsLabelAutoFit="true">
                                                                                <LabelStyle Font="Verdana, 7pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisY>
                                                                            <AxisX LineColor="64, 64, 64, 64" Title="Month">
                                                                                <LabelStyle Font="Verdana, 7pt, style=Bold" Angle="-90" Interval="1" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                </asp:Chart>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlBarPie" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Chart ID="ChartBarPie" runat="server" BackColor="WhiteSmoke" borderdashstyle="Solid"
                                                                    BorderWidth="2px" Visible="False" Width="800px" ClientIDMode="Static">
                                                                    <Titles>
                                                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                                                            Text="Total Time(Hrs)" ForeColor="26, 59, 105" Alignment="TopCenter">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <Legends>
                                                                        <asp:Legend BackColor="Transparent" Enabled="False" Font="Verdana, 5.25pt, style=Bold"
                                                                            IsTextAutoFit="True" Name="Default">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                                                    <Series>
                                                                        <asp:Series Name="Series1" 
                                                                            
                                                                            CustomProperties="PieLineColor=Black, PieLabelStyle=Disabled, PixelPointWidth=10" 
                                                                            ChartType="Pie" LegendText="#VALX - #VALY">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                                                            BackColor="#D3DFF0" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                                                            <AxisY LineColor="64, 64, 64, 64" IntervalAutoMode="FixedCount" IntervalType="Auto"
                                                                                Title="Flying Hrs" IsStartedFromZero="True" IsLabelAutoFit="true">
                                                                                <LabelStyle Font="Verdana, 7pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisY>
                                                                            <AxisX LineColor="64, 64, 64, 64" Title="Reg No.">
                                                                                <LabelStyle Font="Verdana, 7pt, style=Bold" Angle="-90" Interval="1" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                </asp:Chart>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
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
    </div>
    <script type="text/javascript">
        //Aircraft validation
        function ValidateAircraft(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAircraft");

            if (($get("rdbLineGraph").checked == true) && (dd.selectedIndex == 0)) {
                args.IsValid = false;
                return;

            }
            else { args.IsValid = true; return; }


        }
    </script>
    </form>
</body>
</html>
