<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftMonthlyFlying_AJAX.aspx.vb"
    Inherits="Flypal.wfrptAircraftMonthlyFlying_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" /> 
    <title>Aircraft Monthly Flying</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <style type="text/css">
        .style1
        {
            height: 17px;
        }
    </style>
    <script language="javascript" id="clientEventHandlersJS">

        function openFile() {
            str = "wfExportToExcel.aspx"
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
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Search criteria for Aircraft Monthly Flying</asp:Label>
                                            </td>
                                           <%-- <td align="right">
                                                <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                                        Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" runat="server" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                        Text="Export to Excel"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" TabIndex="0" runat="server"
                                                                        Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                        </tr>
                                    </table>                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1">
                                                <tr>
                                                    <td align="left" colspan="5">
                                                        <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Year</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 11px" align="right" colspan="1" rowspan="1">
                                                    </td>
                                                    <td style="height: 11px" align="left" colspan="1">
                                                        <asp:Label ID="lblYear" runat="server" CssClass="clsLabelAuto">Year</asp:Label>
                                                    </td>
                                                    <td style="height: 11px" align="left" colspan="3">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbYear" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="5" rowspan="1">
                                                        <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 20px">
                                                    </td>
                                                    <td style="height: 20px">
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                                    </td>
                                                    <td style="height: 20px" align="left" colspan="3">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True"
                                                            DataValueField="ID" DataTextField="RegNo">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="5">
                                                        <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Period</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 3px">
                                                    </td>
                                                    <td style="height: 3px">
                                                        <asp:Label ID="lblPeriod" runat="server" CssClass="clsLabelAuto">Period</asp:Label>
                                                    </td>
                                                    <td style="height: 3px" align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPeriod" runat="server" AutoPostBack="True"
                                                            DataValueField="ID" DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkBlockTime" runat="server" CssClass="clsCheckBox" Text="Show Block Time" AutoPostBack="true"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="5">
                                                        <asp:Label ID="lblgrid" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgAircraftMonthlyList" runat="server" AllowSorting="True" PageSize="3"
                                                GridLines="Horizontal" CellPadding="3" CssClass="clsGridNewStyle" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RegNo" HeaderText="Reg No.">
                                                        <HeaderStyle ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="JanFlyingHrsInString" HeaderText="Jan">
                                                        <HeaderStyle  Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FebFlyingHrsInString" HeaderText="Feb">
                                                        <HeaderStyle  Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MarFlyingHrsInString" HeaderText="Mar">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AprFlyingHrsInString" HeaderText="Apr">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MayFlyingHrsInString" HeaderText="May">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="JunFlyingHrsInString" HeaderText="Jun">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="JulFlyingHrsInString" HeaderText="Jul">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AugFlyingHrsInString" HeaderText="Aug">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SepFlyingHrsInString" HeaderText="Sep">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OctFlyingHrsInString" HeaderText="Oct">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NovFlyingHrsInString" HeaderText="Nov">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DecFlyingHrsInString" HeaderText="Dec">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalFlyingHrsInString" HeaderText="Total">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalAvgFlyingHrsInString" HeaderText="Avg/Mth">
                                                        <HeaderStyle Width="40px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                                <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblyearselection" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style1">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPeriod1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                            Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                             Text="Export to Excel"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                            Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" Text="Close" CausesValidation="False"
                                                            ToolTip="Click to Close">
                                                        </asp:Button>
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
    </div>
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
    </form>
</body>
</html>
