<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSnagMonthWiseGraph_Ajax.aspx.vb"
    Inherits="Flypal.wfrptSnagMonthWiseGraph_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Graph Report</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td colspan="2">
                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Search criteria for ADD/Defect Month Wise Graph","Search criteria for MEL/Snag Month Wise Graph") %>'></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlTable" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="tblInner" width="100%">
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
                                            <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                DataTextField="RegNo" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Step III. Selection of ADD/Defect Type","Step III. Selection of MEL/Snag Type") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbAll" runat="server" CssClass="clsRadioButton" GroupName="a"
                                                Text="All" AutoPostBack="True"></asp:RadioButton>
                                            <asp:RadioButton ID="rbMajor" runat="server" CssClass="clsRadioButton" GroupName="a"
                                                Text="Major" AutoPostBack="True"></asp:RadioButton>
                                            <asp:RadioButton ID="rbMinor" runat="server" CssClass="clsRadioButton" GroupName="a"
                                                Text="Minor" AutoPostBack="True"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblSnagMEl" runat="server" CssClass="clsLabelHeader" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Step IV. Selection of ADD/Defect Part","Step IV. Selection of MEL/Snag Part") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbAllSnagMEL" runat="server" CssClass="clsRadioButton" GroupName="b"
                                                Text="All" AutoPostBack="True"></asp:RadioButton>
                                            <asp:RadioButton ID="rbSnag" runat="server" CssClass="clsRadioButton" GroupName="b"
                                                Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Defect","Snag") %>'
                                                AutoPostBack="True"></asp:RadioButton>
                                            <asp:RadioButton ID="rbMEL" runat="server" CssClass="clsRadioButton" GroupName="b"
                                                Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","ADD","MEL") %>' AutoPostBack="True">
                                            </asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step  V. Display Report</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblYear1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                                    Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias">
                                                                </asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnDisplay1" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                    Text="Display" ToolTip="Click to Display Report" Visible="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Display"
                                                                    ToolTip="Click to Display Report"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                                    CausesValidation="False" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Click to close the Search criteria for ADD/Defect Month Wise Graph screen","Click to close the Search criteria for MEL/Snag Month Wise Graph screen") %>'>
                                                                </asp:Button>
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
                <td>
                    <asp:UpdatePanel ID="upnlChart" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Chart ID="Chart1" runat="server" BackColor="WhiteSmoke" borderdashstyle="Solid"
                                BorderWidth="2" ImageType="Png" Palette="BrightPastel" Visible="false" Width="800px"
                                RenderType="ImageTag" ClientIDMode="Static">
                                <Titles>
                                    <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                        Text="No. of Defect per Month" ForeColor="26, 59, 105" Alignment="TopCenter">
                                    </asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend BackColor="Transparent" Enabled="False" Font="Verdana, 5.25pt, style=Bold"
                                        IsTextAutoFit="True" Name="Default">
                                    </asp:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                <Series>
                                    <asp:Series Name="Series1" CustomProperties="PixelPointWidth=10">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                        BackColor="#D3DFF0" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                        <AxisY LineColor="64, 64, 64, 64" IntervalAutoMode="FixedCount" IntervalType="Auto"
                                            Title="Snag Count" IsStartedFromZero="True" IsLabelAutoFit="true">
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
    </div>
    </form>
</body>
</html>
