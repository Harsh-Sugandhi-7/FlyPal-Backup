<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskList.aspx.vb" Inherits="Flypal.wfTaskList" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Task List</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 
        }

    </script>
    <meta content="True" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                    <table class="clsTablelistin" id="tblinner">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Work order Job List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id="Table10" cellspacing="2" cellpadding="2" width="100%" border="0">
                                    <tr>
                                        <td width="10%">
                                            <asp:Label ID="lNumber" runat="server" CssClass="clsLabel">Number</asp:Label>
                                        </td>
                                        <td style="width: 211px" width="211">
                                            <asp:Label ID="lblWONo" runat="server" CssClass="clsLabelAuto" Text='<%# mWO.WOText &amp; "-" &amp; mWO.WONo %>'>
                                            </asp:Label>
                                        </td>
                                        <td style="width: 137px" width="137" colspan="1" rowspan="1">
                                            <asp:Label ID="Label8" runat="server" CssClass="clsLabelHeader">Estimated</asp:Label>
                                        </td>
                                        <td width="12%" colspan="1" rowspan="1">
                                        </td>
                                        <td width="12%" colspan="1" rowspan="1">
                                            <asp:Label ID="Label11" runat="server" CssClass="clsLabelHeader">Actual</asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lRaisedDate" runat="server" CssClass="clsLabel">Raised Date</asp:Label>
                                        </td>
                                        <td style="width: 211px">
                                            <asp:Label ID="lblRaisedDate" runat="server" CssClass="clsLabelAuto" Text="<%# mWO.Date %>">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 137px">
                                            <asp:Label ID="lDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEstimatedDate" runat="server" CssClass="clsLabelAuto" Text="<%# mWo.EstimatedDate %>">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lStarted" runat="server" CssClass="clsLabel">Started</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStarted" runat="server" CssClass="clsLabelAuto" Text="<%# mWO.StartDate %>">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lDeliveryDate" runat="server" CssClass="clsLabel">Delivery Date</asp:Label>
                                        </td>
                                        <td style="width: 211px">
                                            <asp:Label ID="lblDeliveryDate" runat="server" CssClass="clsLabelAuto" Text="<%# mWO.DeliveryDate %>">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 137px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lCompleted" runat="server" CssClass="clsLabel">Completed</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCompleted" runat="server" CssClass="clsLabelAuto" Text="<%# mWO.CompletionDate %>">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lCallOutNo" runat="server" CssClass="clsLabel">Callout No.</asp:Label>
                                        </td>
                                        <td style="width: 211px">
                                            <asp:Label ID="lblCallOutNo" runat="server" CssClass="clsLabelAuto" Text='<%# mWO.CallOutText &amp; "-" &amp; mWO.CallOutNo %>'>
                                            </asp:Label>
                                        </td>
                                        <td style="width: 137px">
                                            <asp:Label ID="lHours" runat="server" CssClass="clsLabelAuto">Hours</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHours" runat="server" CssClass="clsLabelAuto" Text="<%# mWo.EstimatedHours %>">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" CssClass="clsLabel">Hours</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHours1" runat="server" CssClass="clsLabelAuto" Text="<%# mWo.EstimatedHours %>">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lCustomer" runat="server" CssClass="clsLabel">Customer</asp:Label>
                                        </td>
                                        <td style="width: 211px">
                                            <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Text="<%# mWO.CustomerName %>">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 137px">
                                            <asp:Label ID="Label19" runat="server" CssClass="clsLabelHeader">Status Details</asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lLocation" runat="server" CssClass="clsLabel">Location</asp:Label>
                                        </td>
                                        <td style="width: 211px">
                                            <asp:Label ID="lblLocation" runat="server" CssClass="clsLabelAuto" Text="<%# mWO.LocationName %>">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 137px">
                                            <asp:Label ID="lStatus" runat="server" CssClass="clsLabelAuto">Status</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto" Text="<%# mWO.StatusName %>">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label21" runat="server" CssClass="clsLabel">% Complete</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCompletionStatus" runat="server" CssClass="clslabelAuto" Text="<%# mWO.PercentComplete %>">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblJobList" runat="server" CssClass="clsLabelHeader">Job List</asp:Label>
                            </td>
                            <td align="right">
                                <table id="Table3">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsButton" Text="Save"
                                                CausesValidation="False" ToolTip="Click to save Work Order"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:DataGrid ID="dgJobList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="3">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="SrNo" HeaderText="Sr. No"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="JobTypeName" HeaderText="Type"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="JobDescription" HeaderText="Job"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="JobAction" HeaderText="Action"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="StartDateForGrid" HeaderText="Started"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CompletionDateForGrid" HeaderText="Completed"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="EstimatedHours" HeaderText="Estd. Hours">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UsedHours" HeaderText="Used Hours">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="StatusName" HeaderText="Status"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CompletionStatus" HeaderText="% Completed">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="EstimatedDateForGridFormatted" HeaderText="Est. Date">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CRate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CAmount" HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Select Job" HeaderText="Select Job" CommandName="Edit"></asp:ButtonColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id="Table1" align="right">
                                    <tr>
                                        <td colspan="1">
                                            <asp:Button ID="btnAddNewTop" TabIndex="0" runat="server" CssClass="clsButton" Text="Add New"
                                                CausesValidation="False" ToolTip="Add New Task"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCloseTop" TabIndex="0" runat="server" CssClass="clsButton" Text="Close">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblWOJob" runat="server" CssClass="clsLabelHeader"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:DataGrid ID="dgTaskList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="3">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="SrNo" HeaderText="Sr. No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="TaskDescription" HeaderText="Task"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="WorkShopName" HeaderText="WorkShop"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="StartDateForGridFormatted" HeaderText="Started"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CompletionDateForGridFormatted" HeaderText="Completed">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="EstimatedHours" HeaderText="Estd. Hours">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UsedHours" HeaderText="Used Hours">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="StatusName" HeaderText="Status"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="EstimatedDateForGridFormatted" HeaderText="Estd. Date">
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Edit" HeaderText="Edit" CommandName="Edit"></asp:ButtonColumn>
                                        <asp:ButtonColumn Text="Remove" HeaderText="Remove" CommandName="Remove"></asp:ButtonColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="3">
                                <table id="Table2" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddNew" TabIndex="0" runat="server" CssClass="clsButton" Text="Add New"
                                                CausesValidation="False" ToolTip="Add New Task"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" Text="Close">
                                            </asp:Button>
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
    </form>
</body>
</html>
