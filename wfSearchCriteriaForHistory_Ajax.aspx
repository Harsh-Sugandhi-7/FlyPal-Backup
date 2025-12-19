<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForHistory_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfSearchCriteriaForHistory_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Common History Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
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
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
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
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <span id="lbltitle" class="clsFormHeader">Common History Register</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                    ErrorMessage="From Date Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                    ErrorMessage="To Date Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="1"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvCheckBoxValidation" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="Please Select at least one of the Installation/Removal/Compliance"
                                    ClientValidationFunction="validateCheckBox" Display="None" ValidationGroup="1"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Dates</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                TabIndex="2" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;" 
                                                TabIndex="3" onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                runat="server"></asp:TextBox>
                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Work Order No.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblWorkOrderNo" class="clsLabelAuto">Work Order No.</span>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkOrderNo" runat="server" MaxLength="25"
                                    ToolTip="Enter Work Order No."></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="Span1" class="clsLabelHeader">Step III. Selection of Maintenance Type</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlchkInstRem" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkInstallation" runat="server" CssClass="clsCheckBox" Text="Installation"
                                                        onclick="ControlTSICSIVisibility();" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRemoval" runat="server" ClientIDMode="Static" CssClass="clsCheckBox"
                                                        Text="Removal" onclick="ControlVisibility(this);" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCompliance" runat="server" CssClass="clsCheckBox" Text="Compliance" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsRemUnschedule" runat="server" CssClass="clsCheckBox" Text="Unschedule Removal"
                                                        onclick="ControlTSICSIVisibility();" Style="visibility: hidden;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span id="lblStep3" class="clsLabelHeader">Step IV. Selection of Installation To/Removal
                                    From/Compliance On</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblType" class="clsLabelAuto" style="display: none;">To/From/of : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsComboBox_Ajax"
                                    Visible="false">
                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                    <asp:ListItem Value="1">Airframe</asp:ListItem>
                                    <asp:ListItem Value="2">Engine</asp:ListItem>
                                    <asp:ListItem Value="3">Propeller</asp:ListItem>
                                    <asp:ListItem Value="4">Auxiliary Power Unit</asp:ListItem>
                                    <asp:ListItem Value="5">Combined Gear Box</asp:ListItem>
                                    <asp:ListItem Value="6">Main Gear Box</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlFrmAssembly" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblModelNo" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtModelNo" runat="server" MaxLength="50"
                                                        ToolTip="Enter Model No."></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSerialNo" runat="server" MaxLength="50"
                                                        ToolTip="Enter Serial No."></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now" />--%>

                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="5">
                                                    <asp:Panel ID="pnlModel" runat="server" CssClass="clspanel1">
                                                        <table id="Table1" border="0" class="clstablelistin" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="dgModel" runat="server" AllowPaging="True" AllowSorting="True"
                                                                        AutoGenerateColumns="False" ClientIDMode="Static" DataKeyNames="ID"
                                                                        CellPadding="5" CssClass="clsGridNewStyle"  ForeColor="Black" GridLines="Horizontal"
                                                                         PageSize="5" ShowHeaderWhenEmpty="true">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                            <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:ButtonField CommandName="Select" HeaderStyle-HorizontalAlign="Left" HeaderText="Select"
                                                                                Text="Select" ControlStyle-ForeColor="Blue"/>

                                                                            

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
                            <td colspan="2" align="left">
                                <span id="lblStep4" class="clsLabelHeader">Step V. Selection of Installation/Removal/Compliance
                                    On/of</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlCheckBoxList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkAssembly" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                                        Text="Assembly" />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkComponent" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                                        Text="Component" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblAModelNo" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtAModelNo" runat="server" MaxLength="50"
                                                        ToolTip="Enter Assembly Model No."></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblASerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtASerialNo" runat="server" MaxLength="50"
                                                        ToolTip="Enter Assembly Serial No."></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <%--<asp:Button ID="btnFindModel" runat="server" CssClass="clsButton_Ajax" Text="Find Now" />--%>

                                                    <asp:ImageButton ID="btnFindModel" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find" />

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
                                                                        AutoGenerateColumns="False" ClientIDMode="Static"  DataKeyNames="ID"
                                                                        CellPadding="5" CssClass="clsGridNewStyle"  ForeColor="Black" GridLines="Horizontal"
                                                                         PageSize="5" ShowHeaderWhenEmpty="true">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                            <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:ButtonField CommandName="Select" HeaderStyle-HorizontalAlign="Left" HeaderText="Select"
                                                                                Text="Select" ControlStyle-ForeColor="Blue"/>

                                                                           <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditViewEmodel" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Select" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>--%>

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
                            <td colspan="2" align="left">
                                <asp:UpdatePanel ID="upnlComponent" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCPartNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCPartNo" runat="server" MaxLength="50"
                                                        ToolTip="Enter Component Part No."></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblCSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCSerialNo" runat="server" MaxLength="50"
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
                                                                        CellPadding="5" CssClass="clsGridNewStyle"  ForeColor="Black" GridLines="Horizontal"
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
                                                                                Text="Select" ControlStyle-ForeColor="Blue"/>


                                                                           <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditViewPart" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Select" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>--%>

                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="Span2" class="clsLabelHeader">Step VI. Selection of ATA</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblATAChapter" class="clsLabelAuto" style="width: 100px">ATA Chapter</span>
                            </td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbATAChapter" runat="server"
                                                DataValueField="ID" DataTextField="ATAChapter">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTSICSI" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkTSICSI" runat="server" CssClass="clsCheckBox" Text="Show TSI/CSI"
                                                        Style="visibility: hidden;" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep5" class="clsLabelHeader">Step VII. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblWorkOrderNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAssemblyType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblRemovalFrom" runat="server" CssClass="clsLabelAuto" Visible="False">To/From/On : </asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblModelNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblRemovalof" runat="server" CssClass="clsLabelAuto" Visible="False">On/of : </asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblAModelNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblASerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblCPartNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblCSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                         Text="Current Criteria" ToolTip="Click to Display Current Searching criterias." />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" Text="Display"
                                                        ToolTip="Click to Display Report" ValidationGroup="1" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExportToExcel" runat="server" Text="Export To Excel"
                                                        ToolTip="Click to Export Report" ValidationGroup="1" Visible="<%$AppSettings:ShowExportToExcelButton%>"/>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                        Text="Close" ToolTip="Click to Close" />
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
    <%--Date Validations--%>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {

            args.IsValid = false;
            var fromdate = $("#txtFromDate").val();
            var todate = $("#txtToDate").val();
            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate };
            $.ajax({
                type: "POST",
                url: "BetweenDateValidationHandler.ashx",
                cache: false,
                data: param,
                async: false,
                beforeSend: OnBeforeSnd,
                success: onSuces,
                error: onErr
            });

            function onSuces(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                if (result == "True") {
                    args.IsValid = true;
                    return;
                }

            }

            function onErr(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                source.errormessage = result;
                return;
            }
            function OnBeforeSnd() {
                $get("AjaxLoader").style.visibility = 'visible';
            }
        }


        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
       
    </script>
    <script type="text/javascript">
        function validateCheckBox(source, args) {
            var IsInstallation = $get("chkInstallation").checked;
            var IsRemoval = $get("chkRemoval").checked;
            var IsCompliance = $get("chkCompliance").checked;

            args.IsValid = false;
            if (IsInstallation || IsRemoval || IsCompliance) {
                args.IsValid = true;

            }
        }
    </script>
    <script type="text/javascript">
        //wo no checkbox status change event
        function ControlTSICSIVisibility() {
            
            var IsInstallation = $get("chkInstallation").checked;
            var IsRemoval = $get("chkRemoval").checked;
            var IsRemUnschedule = $get("chkIsRemUnschedule").checked;
            var str="<%=System.Configuration.ConfigurationManager.AppSettings("ClientCode").ToString()%>";
          
            if (IsInstallation || IsRemoval || IsRemUnschedule) {
                if (str == "BA" || str=="YA" || str=="TA") {
                    $("#chkTSICSI").css('visibility', 'visible');
                    $("#chkTSICSI").next().css('visibility', 'visible');
                }
                else {
                    $("#chkTSICSI").css('visibility', 'hidden');
                    $("#chkTSICSI").next().css('visibility', 'hidden');
                    $("#chkTSICSI").removeAttr('checked');
                }
            }
            else {
                $("#chkTSICSI").css('visibility', 'hidden');
                $("#chkTSICSI").next().css('visibility', 'hidden');
                $("#chkTSICSI").removeAttr('checked');
            }
        }
        function ControlVisibility(elem) {
            var status = $(elem).attr('checked');
            if (status == "checked") {
                $("#chkIsRemUnschedule").css('visibility', 'visible');
                $("#chkIsRemUnschedule").next().css('visibility', 'visible');
             
            }
            else {
                $("#chkIsRemUnschedule").css('visibility', 'hidden');
                $("#chkIsRemUnschedule").next().css('visibility', 'hidden');
                $("#chkIsRemUnschedule").removeAttr('checked');
            }
            ControlTSICSIVisibility();
        }
    </script>
    </form>
</body>
</html>
