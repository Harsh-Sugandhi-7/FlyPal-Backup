<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForHistory.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Order Register</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="javascript" id="clientEventHandlersJS">
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="2000" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <td class="clsFormHeader1Newstyle" colspan="6">
                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader"></asp:Label>
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <table id="Table2" class="clstablelistin" border="0" cellspacing="0">
                                                                    <tr>


                                                                        <td>
                                                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static" Height="25px"
                                                                                TabIndex="2" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                        </td>

                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">&nbsp;
                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabel">To Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <table id="Table3" class="clstablelistin" border="0" cellspacing="0">
                                                                    <tr>

                                                                        <td>
                                                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;" Height="25px"
                                                                                TabIndex="3" onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                                                runat="server"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                        </td>

                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Work Order No.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>

                                                <td align="left">
                                                    <span id="lblWorkOrderNo" class="clsLabelAuto">Work Order No.</span>
                                                </td>
                                                <td align="left" colspan="5">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkOrderNo" runat="server" MaxLength="25" Height="25px"
                                                        ToolTip="Enter Work Order No."></asp:TextBox>
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>


                                    </tr>
                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Removal From</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 4px" align="left">
                                            <asp:Label ID="lblType" runat="server" CssClass="clsLabelAuto">Removal From</asp:Label>
                                        </td>
                                        <td style="height: 4px" align="left">
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssemblyType" runat="server">
                                                <asp:ListItem Value="0">(All)</asp:ListItem>
                                                <asp:ListItem Value="1">Airframe</asp:ListItem>
                                                <asp:ListItem Value="2">Engine</asp:ListItem>
                                                <asp:ListItem Value="3">Propeller</asp:ListItem>
                                                <asp:ListItem Value="4">Auxiliary Power Unit</asp:ListItem>
                                                <asp:ListItem Value="5">Combined Gear Box</asp:ListItem>
                                                <asp:ListItem Value="6">Main Gear Box</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblModelNo" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtModelNo" runat="server" MaxLength="50" Height="25px"
                                                ToolTip="Enter Model No."></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSerialNo" runat="server" MaxLength="50" Height="25px"
                                                ToolTip="Enter Serial No."></asp:TextBox>
                                        </td>
                                        <td colspan="2" align="left">

                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:Panel ID="pnlModel" runat="server" CssClass="clspanel1">
                                                <table id="Table1" class="clstablelistin" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <asp:GridView ID="dgModel" runat="server" AllowPaging="True" AllowSorting="True"
                                                                AutoGenerateColumns="False" ClientIDMode="Static" DataKeyNames="ID"
                                                                CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal"
                                                                PageSize="5" ShowHeaderWhenEmpty="true">

                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                <PagerStyle CssClass="paging" Font-Size="12px" HorizontalAlign="Right" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                    <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:ButtonField CommandName="Select" HeaderStyle-HorizontalAlign="Left" HeaderText="Select"
                                                                        Text="Select" ControlStyle-ForeColor="Blue" />

                                                                </Columns>
                                                            </asp:GridView>

                                                        </td>


                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Removal of</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:CheckBox ID="chkAssembly" runat="server" CssClass="clscheckBox" Text="Assembly"
                                                AutoPostBack="True"></asp:CheckBox>
                                        </td>
                                        <td colspan="5" align="left">
                                            <asp:CheckBox ID="chkComponent" runat="server" CssClass="clscheckBox" Text="Component"
                                                AutoPostBack="True"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="6">
                                            <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblAModelNo" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtAModelNo" runat="server" MaxLength="50" Height="25px"
                                                                    ToolTip="Enter Assembly Model No."></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblASerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtASerialNo" runat="server" MaxLength="50" Height="25px"
                                                                    ToolTip="Enter Assembly Serial No."></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <%--<asp:Button ID="btnFindModel" runat="server" CssClass="clsButton_Ajax" Text="Find Now" />--%>

                                                                <asp:ImageButton ID="btnFindModel" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find" />

                                                            </td>
                                                        </tr>



                                                        <tr>
                                                            <td colspan="6" align="left">
                                                                <asp:Panel ID="pnlAModel" runat="server" CssClass="clspanel1">
                                                                    <table id="Table4" class="clstablelistin" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>

                                                                            <td>
                                                                                <asp:GridView ID="dgAModel" runat="server" AllowPaging="True" AllowSorting="True"
                                                                                    AutoGenerateColumns="False" ClientIDMode="Static" DataKeyNames="ID"
                                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal"
                                                                                    PageSize="5" ShowHeaderWhenEmpty="true">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                                        <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:ButtonField CommandName="Select" HeaderStyle-HorizontalAlign="Left" HeaderText="Select"
                                                                                            Text="Select" ControlStyle-ForeColor="Blue" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="left" colspan="5">
                                                                <asp:Panel ID="pnlEModel" runat="server" CssClass="clspanel1">
                                                                    <table id="Table6" border="0" class="clstablelistin" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblResult3" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:GridView ID="dgEModel" runat="server" AllowPaging="True" AllowSorting="True"
                                                                                    AutoGenerateColumns="False" ClientIDMode="Static" DataKeyNames="ID"
                                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal"
                                                                                    PageSize="5" ShowHeaderWhenEmpty="true">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                                        <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:ButtonField CommandName="Select" HeaderStyle-HorizontalAlign="Left" HeaderText="Select"
                                                                                            Text="Select" ControlStyle-ForeColor="Blue" />

                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:UpdatePanel ID="upnlComponent" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblCPartNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCPartNo" runat="server" MaxLength="50" Height="25px"
                                                                    ToolTip="Enter Component Part No."></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblCSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCSerialNo" runat="server" MaxLength="50" Height="25px"
                                                                    ToolTip="Enter Component Serial No."></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <%--<asp:Button ID="btnFindPart" runat="server" CssClass="clsButton_Ajax" Text="Find Now" />--%>

                                                                <asp:ImageButton ID="btnFindPart" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find" />

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                                <asp:Panel ID="pnlPart" runat="server" CssClass="clspanel1">
                                                                    <table id="Table5" border="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblResult2" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:GridView ID="dgPart" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                                                                    ClientIDMode="Static" DataKeyNames="ID"
                                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal"
                                                                                    PageSize="5" ShowHeaderWhenEmpty="true">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                                        <asp:BoundField DataField="PartName" HeaderText="Part No." SortExpression="PartName">
                                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            <ItemStyle Wrap="False" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:ButtonField CommandName="Select" HeaderStyle-HorizontalAlign="Left" HeaderText="Select"
                                                                                            Text="Select" ControlStyle-ForeColor="Blue" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <%-- <tr>
                                                <td colspan="3">
                                                    <span id="Span2" class="clsLabelHeader">Step VI. Selection of ATA</span>
                                                </td>
                                            </tr>--%>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 17px" colspan="2" align="left">
                                            <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td style="height: 17px" colspan="4" align="left">
                                            <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Label ID="lblWorkOrderNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="lblAssemblyType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px" align="left">
                                            <asp:Label ID="lblRemovalFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td style="height: 20px" colspan="5" align="left">
                                            <asp:Label ID="lblModelNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td colspan="5" align="left">
                                            <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblRemovalof" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td colspan="5" align="left">
                                            <asp:Label ID="lblAModelNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="5">
                                            <asp:Label ID="lblASerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td colspan="5" align="left">
                                            <asp:Label ID="lblCPartNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td colspan="5" align="left">
                                            <asp:Label ID="lblCSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="6" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                                    ToolTip="Click to Display Current Searching criterias." Text="Current Criteria"
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" ToolTip="Click to Display Report"
                                                                    Text="Display"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" ToolTip="Click to Close"
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
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    //        contentType: "application/json",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
                    //                beforeSend: function (xhr, settings) {
                    //                    $("[id$=processing]").dialog();
                    //                },
                    success: onSuccess,
                    error: onError
                });

                function onSuccess(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val(result);
                    $find(extenderid).set_text(result);
                }

                function onError(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val('');
                    $find(extenderid).set_text('');
                }
                function OnBeforeSend() {
                    $(elem).addClass('ac_loading');
                }
            }

        </script>
    </form>
</body>
</html>
