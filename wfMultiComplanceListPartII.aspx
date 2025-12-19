<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMultiComplanceListPartII.aspx.vb"
    Inherits="Flypal.wfMultiComplanceListPartII" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Multi Compliance List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
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
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <style type="text/css">
        .style1
        {
            height: 27px;
        }
    </style>
  
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
     <!--Added by Saylee on 11-Mar-2014 for ALL11032014-->
    <script type="text/javascript">
        $(document).ready(function () {
            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).closest('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    trParent.addClass('clslightColor')
                else
                    trParent.removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).attr('checked');
                if (checked == true)
                    $(element).closest('tr').addClass('clslightColor');
                else
                    $(element).closest('tr').removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });

            // select all click
            $("#chkSelectAllComp").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });
        });

    </script>
    <!-- End-->
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Multi Compliance List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table id="Table6" class="clsTable1" cellpadding="0" designtimedragdrop="427">
                                    <tr>
                                        <td valign="top" colspan="2">
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table id="Table2" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblComplianceDate" runat="server" CssClass="clsLabelAuto">Compliance Date</asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <uc1:SICalendar ID="txtAsOnDate" runat="server"></uc1:SICalendar>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="txtAircraft" runat="server" CssClass="clsTextBox" ReadOnly="True"
                                                            BackColor="#E0E0E0"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="txtAssembly" runat="server" CssClass="clsTextBox" ReadOnly="True"
                                                            BackColor="#E0E0E0"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No.</asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox" ToolTip="Enter Work Order No."></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:CustomValidator ID="cvWorkOrderNo" runat="server" ControlToValidate="txtWorkOrderNo"
                                                Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                        </td>
                                        <td valign="top" align="right">
                                            <table id="Table1" cellspacing="0">
                                                <tr>
                                                    <td valign="top">
                                                        <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelHeader" Height="17px">Compliance On Values</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:DataGrid ID="dgDoneOnValue" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                            PageSize="3">
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
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblCriteria" runat="server" CssClass="clsLabelHeader">Search Criteria</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <table>
                                                <tr>
                                                    <td class="style1">
                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto" Width="95px">Note / Interval</asp:Label>
                                                    </td>
                                                    <td valign="top" class="style1">
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBox" ToolTip="Enter Note/Interval"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton" Text="Find Now">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <table id="Table3" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddToCartTop" TabIndex="0" runat="server" CssClass="clsButton"
                                                            ToolTip="Click to add into the Cart" Visible="False" Text="Add To Cart"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnNextTop" runat="server" CssClass="clsButton" ToolTip="Click to go onto next Page"
                                                            Visible="False" Text="Next"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
                                                            Visible="False" Text="Back" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DataGrid ID="dgInstalledList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                PageSize="3" Visible="False" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Select">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectInstalledList" runat="server" CssClass="clsCheckBox">
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn Visible="False" DataField="CompStatusID" HeaderText="ID"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HeaderText="Period">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ValueFormatted" SortExpression="ValueFormatted" HeaderText="Value">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Removal Reason">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="cmbReason" runat="server" CssClass="clsComboBox" DataValueField="ID"
                                                                DataTextField="Name">
                                                            </asp:DropDownList>
                                                            <asp:CustomValidator ID="cvReason" runat="server" OnServerValidate="customvalidate1"
                                                                Display="None" ControlToValidate="cmbReason" ErrorMessage="Reason Required"></asp:CustomValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Note">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtInstalledNote" runat="server" CssClass="clsTextBoxMultiline"
                                                                ToolTip="Enter Note" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:CustomValidator ID="cvNote" runat="server" ControlToValidate="txtInstalledNote"
                                                                ErrorMessage="Max Lenght of Note should be 200 Chars." Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn Visible="False" HeaderText="Is Expired">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsExpired" runat="server" CssClass="clsCheckBox"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Done By Agency">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtInstalledDoneByAgency" runat="server" CssClass="clsTextBox2"
                                                                ToolTip="Enter Done By Agency Name" MaxLength="100"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DataGrid ID="dgRemovedList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                PageSize="3" Visible="False" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn Visible="False" DataField="CompStatusID" HeaderText="CompStatusID ">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Select">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectRemovedList" runat="server" CssClass="clsCheckBox"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CompInfo" SortExpression="CompInfo" HeaderText="Component Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HeaderText="Period">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ValueFormatted" SortExpression="ValueFormatted" HeaderText="Value">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Done By">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemovedDoneByAgency" runat="server" CssClass="clsTextBox2" ToolTip="Enter Done By Agency Name"
                                                                MaxLength="100"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DataGrid ID="dgDueMonitoringList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                PageSize="1" Visible="False" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Select">
                                                        <HeaderTemplate>
                                                            <input type="checkbox" id="chkSelectAll" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <div>
                                                                <input type="checkbox" name="chkSelectAssemblyList" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                    <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="MachineInfo" SortExpression="MachineInfo"
                                                        HeaderText="Machine Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="AssemblyType" SortExpression="AssemblyType"
                                                        HeaderText="Assembly Type">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo"
                                                        HeaderText="Assembly Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MonitorType" SortExpression="MonitorType" HeaderText="Monitor Type">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ATA" SortExpression="ATA" HeaderText="ATA Chapter">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DoneOnFormatted" HeaderText="Done On">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="DoneOnWONo" SortExpression="DoneOnWONo"
                                                        HeaderText="Work Order No.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="DoneRemark" SortExpression="DoneRemark"
                                                        HeaderText="Remark">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="PeriodUnitName" SortExpression="PeriodUnitName" HeaderText="Period Unit">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="FrequencyValueFormatted" SortExpression="FrequencyValueFormatted"
                                                        HeaderText="Frequency">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DoneOnValueFormatted" SortExpression="DoneOnValueFormatted"
                                                        HeaderText="Done On Value ">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CurrentValueFormatted" SortExpression="CurrentValueFormatted"
                                                        HeaderText="Current">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ElapsedValueFormatted" SortExpression="ElapsedValueFormatted"
                                                        HeaderText="Elapsed">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ExtensionValueFormatted" SortExpression="ExtensionValueFormatted"
                                                        HeaderText="Extension">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DueOnValueFormatted" SortExpression="DueOnValueFormatted"
                                                        HeaderText="Due At">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RemainingValueFormatted" SortExpression="RemainingValueFormatted"
                                                        HeaderText="Remaining">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Note" SortExpression="Note" HeaderText="Note">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Comply Remark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAssemblyRemark" runat="server" CssClass="clsTextBoxMultiLine"
                                                                MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:CustomValidator ID="cvAssemblyRemark" runat="server" ControlToValidate="txtAssemblyRemark"
                                                                Display="None" OnServerValidate="customvalidate1" ErrorMessage="Assembly Remark too long"></asp:CustomValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DataGrid ID="dgDueMonitoringCompList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                Visible="False" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Select">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <HeaderTemplate>
                                                            <input type="checkbox" id="chkSelectAllComp" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <div>
                                                                <input type="checkbox" name="chkSelectCompList" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                    <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="MachineInfo" SortExpression="MachineInfo"
                                                        HeaderText="Aircraft Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="AssemblyType" SortExpression="AssemblyType"
                                                        HeaderText="Assembly Type">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo"
                                                        HeaderText="Assembly Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp. Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MonitorType" SortExpression="MonitorType" HeaderText="Monitor Type">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ATA" SortExpression="ATA" HeaderText="ATA Chapter">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DoneOnFormatted" HeaderText="Done On">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="PeriodUnitName" SortExpression="PeriodUnitName" HeaderText="Period">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="FrequencyValueFormatted" SortExpression="FrequencyValueFormatted"
                                                        HeaderText="Frequency">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DoneOnValueFormatted" SortExpression="DoneOnValueFormatted"
                                                        HeaderText="Done On Value ">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CurrentValueFormatted" SortExpression="CurrentValueFormatted"
                                                        HeaderText="Current">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ElapsedValueFormatted" SortExpression="ElapsedValueFormatted"
                                                        HeaderText="Elapsed">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ExtensionvalueFormatted" SortExpression="ExtensionvalueFormatted"
                                                        HeaderText="Extension">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DueOnvalueFormatted" SortExpression="DueOnvalueFormatted"
                                                        HeaderText="Due At.">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RemainingValueFormatted" SortExpression="RemainingValueFormatted"
                                                        HeaderText="Remaining">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Comply Remark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtCompRemark" runat="server" CssClass="clsTextBoxMultiLine" MaxLength="200"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                            <asp:CustomValidator ID="cvCompRemark" runat="server" ControlToValidate="txtCompRemark"
                                                                Display="None" OnServerValidate="customvalidate1" ErrorMessage="Component Remark too long"></asp:CustomValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddToCart" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to add into the Cart"
                                                            Text="Add To Cart"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnNext" runat="server" CssClass="clsButton" ToolTip="Click to go onto next Page"
                                                            Text="Next"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
                                                            Text="Back" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
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
    </TABLE></form>
</body>
</html>
