<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectWOForMulticompliance.aspx.vb"
    Inherits="Flypal.wfSelectWOForMulticompliance" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>List of Work Order</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }
    </script>
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <table id="Table1" class="clstablelistout" border="0" cellspacing="1" cellpadding="1">
        <tr>
            <td>
                <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1"> Work Order Compliance</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:ValidationSummary  ID="Validationsummary2" HeaderText="Fill Up The Following Fields"
                                CssClass="clsValidationSummary" runat="server"></asp:ValidationSummary>
                            <asp:CustomValidator  ID="cvWOList" runat="server" CssClass="clsLabelAuto"
                                ErrorMessage="Select Work Order from the list." Display="None" ControlToValidate="cmbWOList"
                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table id="Table5" border="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnMaintenanceActivity" CssClass="clsButtonLong" runat="server" ToolTip="Click to open the Maintenance Activity"
                                            Text="Maintenance Activity" EnableViewState="False" CausesValidation="False">
                                        </asp:Button>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblWorkOrder" runat="server" CssClass="clsLabelButton" ToolTip="Current page of Work Order Compliance">Work Order</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table  id="Table3" border="0" cellspacing="1" cellpadding="1">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label  ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Compliance Date</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label  ID="lblFromDate" runat="server" CssClass="clsLabelAuto">Compliance Date</asp:Label>
                                    </td>
                                    <td>
                                        <uc1:SICalendar  ID="txtAsOnDate" runat="server"></uc1:SICalendar>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 21px" colspan="4">
                                        <asp:Label  ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Work Order</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label  ID="lblStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label  ID="lblWO" runat="server" CssClass="clsLabelAuto">Work Order</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList  ID="cmbWOList" runat="server" CssClass="clsComboBox"
                                            AutoPostBack="True" DataValueField="ID" DataTextField="WONumber">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox  ID="chkShowAll" runat="server" CssClass="clsCheckBox"
                                            Text="Show All"></asp:CheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" colspan="2" align="right">
                            <table  id="Table45" border="0" cellspacing="1" cellpadding="1">
                                <tr>
                                    <td>
                                        <asp:Label  ID="lblCurrentValues" runat="server" CssClass="clsLabelHeader">Compliance On Values</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid  ID="dgDoneOnValue" runat="server" CssClass="clsGrid"
                                            AutoGenerateColumns="False" PageSize="3">
                                            <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                            <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="PeriodName" HeaderText="Period"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="AssemblyCurrentValueFormatted" HeaderText="Values"></asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="right">
                                        <asp:Button  ID="btnSelectLog" TabIndex="0" runat="server" CssClass="clsButton"
                                            ToolTip="Click to select the log" Text="Select Log"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2" align="right">
                            <table  id="Table7" border="0">
                                <tr>
                                    <td align="right">
                                        <asp:Button  ID="btnFindNow" runat="server" CssClass="clsButton"
                                            ToolTip="Click To Find records as Searching criteria" Text="Find Now"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label  ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Due Jobs as per selected criteria : 0 Record(s) found.</asp:Label>
                        </td>
                        <td colspan="2" align="right">
                            <table  id="Table6" border="0">
                                <tr>
                                    <td>
                                        <asp:Button  ID="btnSaveTop" runat="server" CssClass="clsButton"
                                            ToolTip="Click To Save" Text="Save" Visible="False"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" ToolTip="Click to close Work Order Compliance screen"
                                            Text="Close" CausesValidation="False" Visible="False"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:DataGrid  ID="dgDueJob" runat="server" CssClass="clsGrid"
                                ToolTip="Due Job." AutoGenerateColumns="False">
                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="LogBook" HeaderText="Assembly Info."></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ATAChapter" HeaderText="ATA Chapter"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="OnAssemblyOrComponent" HeaderText="On Assembly / Component">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DataType" HeaderText="Data Type"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="JobDescriptionDetailWeb" HeaderText="Info"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Freq3" HeaderText="Frequency">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SinceNew" HeaderText="Since New">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DoneAt2" HeaderText="Done At">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DueAsOf2" HeaderText="Due As Of">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="RemainingTime2" HeaderText="Remaining Time">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="EstimatedDate" HeaderText="Estimated Date">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="StartJobDate" HeaderText="Start Date">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="EndJobDate" HeaderText="Completion Date">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Comply Remark">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="txtAssemblyRemark" runat="server" CssClass="clsTextBoxMultiLine"
                                                MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <table id="Table4">
                                <tr>
                                    <td>
                                        <asp:Button  ID="btnSave" runat="server" CssClass="clsButton" ToolTip="Click To Save"
                                            Text="Save" Enabled="False"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to close Work Order Compliance screen"
                                            Text="Close" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
