<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForDueWithAircraftSelection.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForDueWithAircraftSelection" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Due Periodwise Report</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
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
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner" border="0">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Search criteria for Due</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCustomer" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="cmbAircraft" ErrorMessage="Select Aircraft from the list."
                                    OnServerValidate="CustomValidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                                    OnServerValidate="CustomValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of As On Date</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">As On Date</asp:Label>
                            </td>
                            <td>
                                <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <uc1:SICalendar ID="txtFromDate" runat="server"></uc1:SICalendar>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox" Visible="False"
                                    DataValueField="MachineID" DataTextField="RegNo" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid ID="dgMachineList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="3" AllowSorting="True">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                </asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="RegNo" HeaderText="Reg No.">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 2px" align="left">
                                <asp:Label ID="lblSortBy" runat="server" CssClass="clsLabelAuto" Visible="False">Sort By</asp:Label>
                            </td>
                            <td style="height: 2px" align="left">
                                <asp:DropDownList ID="cmbSordBy" runat="server" CssClass="clsComboBox" Visible="False">
                                    <asp:ListItem Value="0">Remaining Value</asp:ListItem>
                                    <asp:ListItem Value="1">Maintenance Type</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader" Visible="False">Step III. Selection of Assembly</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto" Visible="False">Assembly</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox3" Visible="False"
                                    DataValueField="ID" DataTextField="ModelSerialNo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px" align="left" colspan="2">
                                <asp:LinkButton ID="lbtnAdvancedSearch" runat="server" CssClass="clsLabelAuto" Visible="False"
                                    CausesValidation="False">Advanced Search</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Panel ID="pnlAdvancedSearch" runat="server">
                                    <table class="clstablelistin" id="Table1" width="300" border="0">
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader" Visible="False">Step III. Selection of Type</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTypeStar1" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblType" runat="server" CssClass="clsLabelAuto" Visible="False">Type</asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlcmbType" runat="server" CssClass="clsPanel1">
                                                    <asp:CheckBoxList ID="cmbType" runat="server" CssClass="clsComboBox" Visible="False"
                                                        DataValueField="ID" DataTextField="Name" AutoPostBack="True">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlServiceType" runat="server" CssClass="clsPanel1">
                                                    &nbsp;
                                                    <table class="clstablelistin" id="Table3" width="300" border="0">
                                                        <tr>
                                                            <td style="width: 105px">
                                                                <p>
                                                                    <asp:Label ID="lblServiceType" runat="server" CssClass="clsLabel" Visible="False">Service Type</asp:Label></p>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBoxList ID="cmbServiceType" runat="server" CssClass="clsComboBox" Visible="False"
                                                                    DataValueField="ID" DataTextField="CodeType">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModificationType" runat="server" CssClass="clsPanel1">
                                                    <table class="clstablelistin" id="Table4" width="300" border="0">
                                                        <tr>
                                                            <td style="width: 107px">
                                                                <p>
                                                                    <asp:Label ID="lblInspectionType" runat="server" CssClass="clsLabel" Visible="False">Inspection Type</asp:Label></p>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBoxList ID="cmbInspectionType" runat="server" CssClass="clsComboBox" Visible="False"
                                                                    DataValueField="ID" DataTextField="CodeType">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlInspectionType" runat="server" CssClass="clsPanel1">
                                                    <table class="clstablelistin" id="Table5" width="300" border="0">
                                                        <tr>
                                                            <td style="width: 107px">
                                                                <p>
                                                                    <asp:Label ID="lblModificationType" runat="server" CssClass="clsLabel" Visible="False"
                                                                        Width="104px">Directive Type</asp:Label></p>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBoxList ID="cmbModificationType" runat="server" CssClass="clsComboBox"
                                                                    Visible="False" DataValueField="ID" DataTextField="CodeType">
                                                                </asp:CheckBoxList>
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
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step III. Selection of Due Limits / Percentage Life Remaining</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:RadioButton ID="rbdDueLimits" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                    Checked="True" Text="Due Limits" Font-Bold="True" GroupName="StepIII"></asp:RadioButton>
                            </td>
                            <td align="left">
                                <asp:RadioButton ID="rbdPercent" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                    Text="Percent Life Remaining" Font-Bold="True" GroupName="StepIII"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxSmall" Enabled="False"
                                    ToolTip="Enter Percentage" MaxLength="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Panel ID="Panel1" CssClass="clspanel1" runat="server">
                                    <asp:DataGrid ID="dgDuePeriodLimits" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="PeriodName" HeaderText="Period"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Limit">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLimit" runat="server" CssClass="clsTextBoxRightAlign" Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>'
                                                        ToolTip="Enter corresponding Limit Value." BackColor="White">
                                                    </asp:TextBox>
                                                    <asp:CustomValidator ID="cvPeriodLimitsValue" runat="server" Display="None" ControlToValidate="txtLimit"
                                                        ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step IV. Estimated Flying Hours.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">(For Estimated Due-Dates Calculation)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:RadioButton ID="rbdAvrageMonths" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                    Checked="True" Text="Average in Months" Font-Bold="True" GroupName="StepIV">
                                </asp:RadioButton>
                            </td>
                            <td align="left">
                                <asp:RadioButton ID="rbdSpecifyValues" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                    Text="Specify Values" Font-Bold="True" GroupName="StepIV"></asp:RadioButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAvgMnths" runat="server" CssClass="clsLabelAuto">Average for last</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtAvgMnths" runat="server" CssClass="clsTextBoxSmall" ToolTip="Enter Average Months"
                                    MaxLength="4"></asp:TextBox>
                                <asp:Label ID="lblMonths" runat="server" CssClass="clsLabelAuto">Months</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Visible="False">Enter per day Values of Following Periods</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Panel ID="pnlAvragePeriod" CssClass="clspanel1" runat="server" Visible="False">
                                    <asp:DataGrid ID="gdPerDayLimit" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="PeriodID" HeaderText="PeriodID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="PeriodName" HeaderText="Period"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Limit">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLimitPerDay" runat="server" CssClass="clsTextBoxRightAlign" Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>'
                                                        ToolTip="Enter corresponding Limit Value." BackColor="White">
                                                    </asp:TextBox>
                                                    <asp:CustomValidator ID="cvPeriodLimitsValuePerDay" runat="server" Display="None"
                                                        ControlToValidate="txtLimitPerDay" ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">Step V. Enter The Limit For Forecasting</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblLimit" runat="server" CssClass="clsLabelAuto">Limit</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtForecastingLimit" runat="server" CssClass="clsTextBoxSmall" ToolTip="Enter Limit"
                                    MaxLength="4">30</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAvgMnths1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblPercent" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <p>
                                                    <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonlong"
                                                        Text="Current Criteria" ToolTip="Click to display Current Searching criterias.">
                                                    </asp:Button></p>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPreview" TabIndex="0" runat="server" CssClass="clsButton" Text="Preview"
                                                    ToolTip="Click to Preview Report"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton" Text="Display"
                                                    ToolTip="Click to Display Report"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" CausesValidation="False"
                                                    Text="Close" ToolTip="Back to Previous Page"></asp:Button>
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
    </TABLE></form>
</body>
</html>
