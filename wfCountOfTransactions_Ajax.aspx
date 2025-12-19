<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCountOfTransactions_Ajax.aspx.vb"
    Inherits="Flypal.wfCountOfTransactions_Ajax" %>

<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="Flypal.CountOfTransactions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="FusionCharts/fusioncharts.js" type="text/javascript"></script>
    <script src="FusionCharts/fusioncharts.charts.js" type="text/javascript"></script>
    <script src="FusionCharts/themes/fusioncharts.theme.fint.js" type="text/javascript"></script>
    <div style="display: none">
        <table id="T4">
            <tr>
                <td>
                    Trans Type Name
                </td>
                <td>
                    Count Of Transactions
                </td>
            </tr>
            <% Dim Child6 As CountOfTransactionsInfo%>
            <% For Each Child6 In mCountOfTransactions%>
            <tr>
                <td>
                    <%= Child6.TransTypeName%>
                </td>
                <td>
                    <%= Child6.CountOfTransactionsCount%>
                </td>
            </tr>
            <% Next%>
        </table>
    </div>
    <script type="text/javascript">
        function CountOfTransactionsFunc() {
            var getTabularData = function () {
                var table = document.getElementById('T4'), // ‘T4’ here is the table ID
            rows = table.children[0].children,
            row,
            i,
            length,
            data = [];
                // get the table element and iterate over its children to extract the data
                for (i = 1, length = rows.length; i < length; i++) {
                    row = rows[i];
                    data.push({
                        label: row.children[0].innerHTML,
                        value: row.children[1].innerHTML
                    });
                }
                return data;
            };
            //            document.getElementById('convert').onclick = function () {
            // on click, create the chart using the data obtained by calling the getTabularData() function
            var revenueChart = new FusionCharts({
                type: 'column2d',
                renderAt: 'CountOFTransactions',
                width: '100%',
                height: '300',
                dataFormat: 'json',
                id: 'chart1',
                dataSource: {
                    "chart": {
                        "caption": "Count of Transactions",
                        //                            "subCaption": "Harry's SuperMart",
                        "xAxisName": "Traqnsactions" + "<br>" + "Compliance Count of Assembly/Component",
                        "yAxisName": "Count",
                        //                            "numberPrefix": "$",
                        "theme": "fint",
                        "rotateValues": "1",
                        "exportEnabled": "1",
                        "placeValuesInside": "0",
                        "valuefontcolor": "074868",
                        "rotateValues": "0"
                    },
                    "data": getTabularData()
                }
            });
            revenueChart.render();
            //            }
        }
    </script>
    <script type="text/javascript">
        function MonthwiseCountOfTransactions(MonthwiseCountOfTransactionsValues) {
            var revenueChart = new FusionCharts({
                "type": "Column2D",
                "renderAt": "DivMonthwiseCountOfTransactions",
                "width": "100%",
                "height": "300",
                "dataFormat": "json",
                "dataSource": {
                    "chart": {
                        "caption": "Monthwise Count Of Transactions",
                        "subCaption": $("#cmbYear :selected").text(),
                        "xAxisName": "NameOfMonth",
                        "yAxisName": "Count",
                        "exportEnabled": "1",
                        "theme": "carbon"
                    },
                    "data": JSON.parse(MonthwiseCountOfTransactionsValues)
                }
            });
            revenueChart.render();
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
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
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
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
                                        <td align="left" style="margin-left: 40px">
                                            <asp:Label ID="lblTransactions" runat="server" CssClass="clsLabelAuto">Transactions</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbTransactions" runat="server" CssClass="clsComboBox_Ajax"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="1">145 WO</asp:ListItem>
                                                <asp:ListItem Value="2">CAMO WO</asp:ListItem>
                                                <asp:ListItem Value="3">Order</asp:ListItem>
                                                <asp:ListItem Value="4">Receipt</asp:ListItem>
                                                <asp:ListItem Value="5">Issue</asp:ListItem>
                                                <asp:ListItem Value="6">Log</asp:ListItem>
                                                <asp:ListItem Value="7">Assembly Service</asp:ListItem>
                                                <asp:ListItem Value="8">Assembly Insp</asp:ListItem>
                                                <asp:ListItem Value="9">Assembly Diretive</asp:ListItem>
                                                <asp:ListItem Value="10">Comp. Service</asp:ListItem>
                                                <asp:ListItem Value="11">Comp. Insp</asp:ListItem>
                                                <asp:ListItem Value="12">Comp. Diretive</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel2" runat="server">
                                    <fieldset id="Fieldset8" style="border-width: 1px;">
                                        <asp:UpdatePanel ID="upnlCountOFTransactions" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div id="CountOFTransactions">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Panel ID="pnlMonthwiseCountOfTransactions" runat="server">
                                    <fieldset id="fdsMonthwiseCountOfTransactions" style="border-width: 1px;">
                                        <asp:UpdatePanel ID="upnlMonthwiseCountOfTransactions" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div id="DivMonthwiseCountOfTransactions">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Close" ToolTip="Click to close the Fleet Reliability Summary screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;" align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="false" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    <!-- Popup For Reliability -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReliability1" Text="Reliability1" ClientIDMode="Static"
            CausesValidation="false" />
    </div>
    <asp:Panel runat="server" ID="pnlReliability1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReliability1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupReliability1" runat="server" TargetControlID="btnDummyReliability1"
        PopupControlID="pnlReliability1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeReliability1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyReliability1").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var Reliabilitywindow1 = $find("<%=mdlPopupReliability1.ClientID %>");
            //close popup window
            Reliabilitywindow1.hide();
            //           release resources
            $("#IframeReliability1").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var Reliabilitywindow1 = $find("<%=mdlPopupReliability1.ClientID %>");
            //close popup window
            Reliabilitywindow1.hide();
            //           release resources
            $("#IframeReliability1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
</body>
</html>
