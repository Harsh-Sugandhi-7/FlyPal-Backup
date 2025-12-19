<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditCalendarPlanList_AJAX.aspx.vb"
    Inherits="Flypal.wfAuditCalendarPlanList_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WO Planning</title>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <!-- Main content -->
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
    </asp:ScriptManager>
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="1">
            <tr>
             <td align="center">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCompany" runat="server" CssClass="clstitle1"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
            </tr>
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">CAMO QUALITY SYSTEM - AUDIT PLAN</asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <span class="clsLabel" style="font-size: medium">Year</span>
                        <div>
                            <asp:UpdatePanel ID="upnlYear" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbYear" runat="server"   DataTextField="Name"
                                        AutoPostBack="true" ClientIDMode="Static" DataValueField="ID">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upnlSchedulerList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="margin-top: -35px;">
                                    <div style="position: relative; top: 34px; z-index: 1; left: 1px; width: 50px; height: 33px;
                                        background-color: #F0F0F0;">
                                    </div>
                                    <%--DoneOnDate--%>
                                    <DayPilot:DayPilotScheduler ID="DayPilotScheduler1" runat="server" DataStartField="FromDateForControl" 
                                        DataEndField="ToDateForControl" DataTextField="AuditNo" DataValueField="ID" DataResourceField="AuditID"
                                        Width="100%" RowHeaderWidth="60" Scale="Month" CellWidth="120">
                                        <TimeHeaders>
                                            <DayPilot:TimeHeader GroupBy="Year" />
                                            <DayPilot:TimeHeader GroupBy="Month" Format="MMM" />
                                        </TimeHeaders>
                                    </DayPilot:DayPilotScheduler>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Audit Execution List screen"
                                                Text="Print"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH" ToolTip="Click to close Audit Calender List"
                                                Text="Close"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnPrint" />
                            </Triggers>
                        </asp:UpdatePanel>
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
    </form>
</body>
</html>
