<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemInstComp_AJAX.aspx.vb"
    Inherits="Flypal.wfRemInstComp_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table class="clsTablelistin" id="tblinner">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Remove Component</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                                            CssClass="clsLabelAuto" ErrorMessage="Model Required" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvReason" runat="server" ControlToValidate="cmbReason" ErrorMessage="Reason Required"
                                            Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCompValue" runat="server" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAssembly" runat="server" ControlToValidate="cmbInstAssemblyList"
                                            ErrorMessage="Please select the Assembly from the list." Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table id="Table1">
                                    <tr>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upnlRemovalDetail" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdsRemovalDetail" class="clsFieldSet" style="border-width: 1px;">
                                                        <legend id="lblRemovalDetail"><b>Removal Detail
                                                            <asp:CheckBox ID="chkRemoval" runat="server" CssClass="clsLabelAuto" ToolTip="Check for Removal Detail "
                                                                AutoPostBack="True" Enabled="False"></asp:CheckBox></b></legend>
                                                        <table>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblCompRemInfo" runat="server" CssClass="clsLabelHeader">Assembly and Component Info</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 97px">
                                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                </td>
                                                                <td style="height: 28px">
                                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Part No."
                                                                        Text="<%# mRemCompStatus.Comp.PartName %>" ReadOnly="True" BackColor="#E0E0E0">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 90px">
                                                                    <asp:Label ID="lblDescription" runat="server" CssClass="clsLabel">Description</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine_Ajax"
                                                                        ToolTip="Description of the Part" Text="<%# mRemCompStatus.Comp.Description %>"
                                                                        ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 90px">
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Serial No. </asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Serial Number"
                                                                        Text="<%# mRemCompStatus.Comp.SerialNo %>" ReadOnly="True" BackColor="#E0E0E0">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 90px">
                                                                    <asp:Label ID="lblCode" runat="server" CssClass="clsLabel">Code</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Code"
                                                                        Text="<%# mRemCompStatus.Comp.Code %>" ReadOnly="True" BackColor="#E0E0E0">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 90px">
                                                                    <asp:Label ID="lblPosition" runat="server" CssClass="clsLabel">Position </asp:Label>
                                                                </td>
                                                                <td style="height: 27px" align="left">
                                                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Position"
                                                                        Text="<%# mRemCompStatus.Position %>" ReadOnly="True" BackColor="#E0E0E0">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="3">
                                                                    <asp:Button ID="btnSelectLog" runat="server" CssClass="clsButton" ToolTip="Click to open Select Log screen"
                                                                        Text="Select Log" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblRemovalInfo" runat="server" CssClass="clsLabelHeader">Removal Information of the []</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRole1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabel">Removed On</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table id="Table5" border="0">
                                                                    </table>
                                                                    <%--  <uc1:sicalendar id="calRemove" runat="server"></uc1:sicalendar>--%>
                                                                    <asp:TextBox runat="server" ID="calRemove" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        ReadOnly="True" BackColor="#E0E0E0" AutoPostBack="true" onchange="ValidateDateText(this,'calRemove_watermarkextender','true');">
                                                                    </asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calRemove_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calRemove">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calRemove" ID="calRemove_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblReason" runat="server" CssClass="clsLabelAuto">Reason</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table id="Table3" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbReason" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                                    DataTextField="Name">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnReason" runat="server" CausesValidation="False" Height="22px"
                                                                                    ImageUrl="~/images/plus1.png" ToolTip="Add New Reason" Width="24px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
															<%-- Sankalp  --%>
															<tr>
																<td></td>
																<td></td>
																<td align="left" colspan="2">
																	<table id="Table3" border="0">
																		<tr>
																			<td>
																				<asp:CheckBox ID="chkIsRemUnscheduled" runat="server" Checked="<%# mRemCompStatus.IsRemUnschedule %>"
																					CssClass="clsCheckBox" Text="Un-Schedule(for reliability monitoring)" />
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblWorkOrderNo" runat="server" CssClass="clsLabel">Work Order No. </asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Work Order Number"
                                                                        Text="<%# mRemCompStatus.RemovalWONO %>" MaxLength="150">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto">Note </asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiline" ToolTip="Enter Note"
                                                                        Text="<%# mRemCompStatus.RemovalRemark %>" TextMode="MultiLine">
                                                                    </asp:TextBox>
                                                                    <asp:CustomValidator ID="cvNote" runat="server" ControlToValidate="txtNote" ErrorMessage="Max Lenght of Note should be 200 Chars."
                                                                        Display="None" OnServerValidate="CustomValidate">
                                                                    </asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Expired</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkExpired" runat="server" Enabled="<%# mRemCompStatus.IsExpiredEnabled %>"
                                                                        Checked="<%# mRemCompStatus.IsExpired %>"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDoneByAgency" runat="server" CssClass="clsLabel">Done By Agency</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtRemDoneBy" runat="server" CssClass="clsTextBox2_Ajax" ToolTip="Enter Done By Agency Name"
                                                                        Text="<%# mRemCompStatus.RemDoneBy %>" MaxLength="100">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" colspan="3">
                                                                    <asp:Label ID="lblRemovalValues" runat="server" CssClass="clsLabelHeader" DESIGNTIMEDRAGDROP="557">Values at Removal</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" colspan="3">
                                                                    <table id="Table11">
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:GridView ID="dgRemovalValue" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                                                                    EnableViewState="true" CssClass="clsGridLog">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period ">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CompRemovalValueFormatted" HeaderText="Component">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyRemovalValueFormatted" HeaderText="Assembly">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                        </asp:BoundField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394--%>
                                                            <tr>
                                                                <td colspan="3" >
                                                                    <asp:Label ID="lblRemovalFanBladeMonitoring" runat="server" CssClass="clsLabelHeader"
                                                                        DESIGNTIMEDRAGDROP="557">Fan Blade Monitoring</asp:Label>
                                                                    <asp:CheckBox ID="chkRemCompStatusFanBladeMonitoring" runat="server" ClientIDMode="Static" Checked="<%# mRemCompStatus.IsFanBladeDistribution %>"
                                                                        Enabled="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <table id="Table4" >
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblRemCompStatusFanBladePosition" class="clsLabel" runat="server">Position </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtRemCompStatusFanBladePosition" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                    ClientIDMode="Static" Text="<%# mRemCompStatus.FanBladePosition %>" Enabled="false"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblRemCompStatusMomentWeight" class="clsLabel" runat="server">Moment Weight </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtRemCompStatusMomentWeight" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                    onchange="setattr(this);" ClientIDMode="Static" Text="<%# mRemCompStatus.MomentWeight %>"
                                                                                    Enabled="false"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblRemCompStatusBalanceScrew" class="clsLabel" runat="server">Balance Screw </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtRemCompStatusBalanceScrew" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                    onchange="setattr(this);" ClientIDMode="Static" Text="<%# mRemCompStatus.BalanceScrew %>"
                                                                                    Enabled="false"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394--%>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upnlInstallationDetail" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdsInstallationDetail" class="clsFieldSet" style="border-width: 1px;">
                                                        <legend id="lblInstallationDetail"><b>Installation Detail
                                                            <asp:CheckBox ID="chkInstallation" runat="server" CssClass="clsLabelAuto" ToolTip="Check for Installation Detail"
                                                                AutoPostBack="True"></asp:CheckBox></b></legend>
                                                        <table id="Table6">
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Label ID="lblCompInstInfo" runat="server" CssClass="clsLabelHeader">Part Serial No. of the []</asp:Label>&nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 14px">
                                                                </td>
                                                                <td style="width: 97px; height: 14px">
                                                                    <asp:Label ID="lblInstATAChapter" runat="server" CssClass="clsLabel">ATA Chapter</asp:Label>
                                                                </td>
                                                                <td style="height: 14px" colspan="2">
                                                                    <asp:DropDownList ID="cmbInstATAChapter" runat="server" CssClass="clsComboBox_Ajax"
                                                                        DataValueField="ID" DataTextField="ATAChapter" Height="17px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 60px; height: 20px">
                                                                    <asp:Label ID="lblInstPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:DropDownList ID="cmbInstPartNo" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                                        DataValueField="ID" DataTextField="Name" Height="17px">
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox ID="chkByModel" runat="server" CssClass="clsCheckBox" ToolTip="Select to search Model wise Part"
                                                                        AutoPostBack="True" Text="By Model" Visible="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkInstallSelected" runat="server" Style="text-decoration: underline;"
                                                                                    CssClass="clsHyperlink1" ToolTip="Click to Install selected existing Removed Component">Install Removed Component</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkInstallSpareComponent" runat="server" CssClass="clsHyperlink1"
                                                                                    Style="text-decoration: underline;" ToolTip="Click to Install Spare Component">Install Spare Component</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 60px">
                                                                    <asp:Label ID="lblInstDescription" runat="server" CssClass="clsLabel">Description</asp:Label>
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    <asp:TextBox ID="txtInstDescription" runat="server" CssClass="clsTextBoxMultiLine_Ajax"
                                                                        ToolTip="Description of the Part" Text="<%# mRemCompStatus.Comp.Description %>"
                                                                        ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 22px">
                                                                </td>
                                                                <td style="width: 60px">
                                                                    <asp:Label ID="lblInstSerialNo" runat="server" CssClass="clsLabel">Serial No. </asp:Label>
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    <asp:TextBox ID="txtInstSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Serial Number"
                                                                        Text="<%# mInstCompStatus.Comp.SerialNo %>" MaxLength="25">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 60px">
                                                                    <asp:Label ID="lblInstPosition" runat="server" CssClass="clsLabel">Position </asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtInstPosition" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Position"
                                                                        Text="<%# mInstCompStatus.Position %>" MaxLength="25">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="4">
                                                                    <asp:CustomValidator ID="cvInstNote" runat="server" ControlToValidate="txtInstNote"
                                                                        Display="None" ErrorMessage="Max length of Note should not be greater than  200 character.">
                                                                    </asp:CustomValidator>
                                                                    <asp:Button ID="btnInstSelectLog" runat="server" CssClass="clsButton" ToolTip="Click to open Select Log screen"
                                                                        Text="Select Log" Visible="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="4">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="4">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" colspan="4">
                                                                    <asp:Label ID="lbInstallationInfo" runat="server" CssClass="clsLabelHeader">Installation Information of the []</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblAsseblyStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblInstAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:DropDownList ID="cmbInstAssemblyList" runat="server" CssClass="clsTextBoxMultiLine3_Ajax"
                                                                        Enabled="False" DataValueField="ID" DataTextField="RegNoModelSerialNo" SelectedValue="<%# mInstCompStatus.AssemblyID %>"
                                                                        Height="17px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblInstInstalledOn" runat="server" CssClass="clsLabel">Installed On </asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <%-- <uc1:sicalendar id="calInstalledOn" runat="server"></uc1:sicalendar>--%>
                                                                    <asp:TextBox runat="server" ID="calInstalledOn" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDate_watermarkextender','true');">
                                                                    </asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calInstalledOn_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calInstalledOn">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calInstalledOn" ID="calInstalledOn_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                    <asp:CustomValidator ID="cvInstalledOn" runat="server" ControlToValidate="calInstalledOn"
                                                                        Display="None" OnServerValidate="CustomValidate">
                                                                    </asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblInstWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No. </asp:Label>
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    <asp:TextBox ID="txtInstWorkOrderNo" runat="server" CssClass="clsTextBox3_Ajax" ToolTip="Enter Work Order Number"
                                                                        Text="<%# mInstCompStatus.InstallationWONo %>" MaxLength="150" Height="17px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblInstNote" runat="server" CssClass="clsLabelAuto" Visible="False">Note </asp:Label>
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    <asp:TextBox ID="txtInstNote" runat="server" CssClass="clsTextBoxMultiLine3_Ajax"
                                                                        ToolTip="Enter Note/Remark regarding Installation" Text="<%# mInstCompStatus.InstallationRemark %>"
                                                                        TextMode="MultiLine" Visible="False" MaxLength="200">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblInstDoneby" runat="server" CssClass="clsLabelAuto">Done By</asp:Label>
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    <asp:TextBox ID="txtInstDoneBy" runat="server" CssClass="clsTextBox3_Ajax" ToolTip="Enter Work Done by Name"
                                                                        Text="<%# mInstCompStatus.InstDoneBy %>" MaxLength="100" Height="17px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" colspan="4">
                                                                    <asp:Label ID="Label12" runat="server" CssClass="clsLabelHeader">Values at Installation</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" colspan="4">
                                                                    <table id="Table10">
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:GridView ID="dgInstallationValue" runat="server" AutoGenerateColumns="False"
                                                                                    ShowHeaderWhenEmpty="true" EnableViewState="true" CssClass="clsGridLog">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period ">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Component" HeaderStyle-HorizontalAlign="Right">
                                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCompInstallationValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                    OnTextChanged="txtCompInstallationValue_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"CompInstallationValueFormatted") %>'></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="AssemblyInstallationValueFormatted" HeaderText="Assembly">
                                                                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                                        </asp:BoundField>
                                                                                        <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:ImageButton ID="btnAddPeriod" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                    Width="24px" ToolTip="Click to Add New Period." CausesValidation="False"></asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394--%>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Label ID="lblInstCompStatusFanBladeMonitoring" runat="server" CssClass="clsLabelHeader"
                                                                        DESIGNTIMEDRAGDROP="557">Fan Blade Monitoring</asp:Label>
                                                                    <asp:CheckBox ID="chkInstCompStatusFanBladeMonitoring" runat="server" ClientIDMode="Static" AutoPostBack="true" Checked="<%# mInstCompStatus.IsFanBladeDistribution %>" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table id="Table7">
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblInstCompStatusPosition" class="clsLabel" runat="server">Position </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtInstCompStatusFanBladePosition" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                    ClientIDMode="Static" Text="<%# mInstCompStatus.FanBladePosition %>"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblInstCompStatusMomentWeight" class="clsLabel" runat="server">Moment Weight </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtInstCompStatusMomentWeight" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                    onchange="setattr(this);" ClientIDMode="Static" Text="<%# mInstCompStatus.MomentWeight %>"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblInstCompStatusBalanceScrew" class="clsLabel" runat="server">Balance Screw </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtInstCompStatusBalanceScrew" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                    onchange="setattr(this);" ClientIDMode="Static" Text="<%# mInstCompStatus.BalanceScrew %>"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394--%>
                                                            <tr style="height: 0px;">
                                                                <td style="height: 0px;">
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            <asp:Button ID="hdnBtnRemovalReason" ClientIDMode="Static" runat="server" Text="Add"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            <asp:Button ID="hdnBtnInstallSelected" ClientIDMode="Static" runat="server" Text="Add"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            <asp:Button ID="hdnBtnSpareCompInstallList" ClientIDMode="Static" runat="server"
                                                                                Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton" ToolTip="Click to save information of Removal/Installation Component"
                                                        Text="Save"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton" ToolTip="Click to Print the Removed Component"
                                                        Text="Print" CausesValidation="False" Visible="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton" ToolTip="Click to go Back to Previous Page"
                                                        Text="Back" CausesValidation="False"></asp:Button>
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
    <!-- Select Log popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySelectLog" Text="Select Log" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSelectLog" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSelectLog" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSelectLog" runat="server" TargetControlID="btnDummySelectLog"
        PopupControlID="pnlSelectLog" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSelectLogStateComplete() {
            $("#btnDummySelectLog").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSelectLogWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSelectLog").attr("src", "wfSelectLog_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySelectLog").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSelectLog() {
            var SelectLogwindow = $find("<%=mdlPopupSelectLog.ClientID %>");
            //close Select Log  popup window
            SelectLogwindow.hide();
            //           release resources
            $("#IframeSelectLog").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSelectLog").click();
        }
    </script>
    <!-- End-->
    <div>
        <!-- Period Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAddPeriod" Text="TaskCard Step" ClientIDMode="Static" />
            <asp:Button ID="hdnAddPeriod" runat="server" CausesValidation="False" ClientIDMode="Static"
                Style="display: none;" Text="Add" />
        </div>
        <asp:Panel runat="server" ID="pnlAddPeriod" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAddPeriod" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardStep" runat="server" TargetControlID="btnDummyAddPeriod"
            PopupControlID="pnlAddPeriod" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameStateComplete() {
                $("#btnDummyAddPeriod").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenAddPeriodWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAddPeriod").attr("src", "wfSelectPeriod_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAddPeriod").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForAddPeriod() {
                var TaskCardStepwindow = $find("<%=mdlPopupTaskCardStep.ClientID %>");
                //close Task Card Step popup window
                TaskCardStepwindow.hide();
                //           release resources
                $("#IframeAddPeriod").attr("src", "JavaScript:''");
                //call image button
                $("#hdnAddPeriod").click();
            }
        </script>
        <!-- End-->
    </div>
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
    <!-- Removal Reason Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemovalReason" Text="Removal Reason" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemovalReason" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemovalReason" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemovalReason" runat="server" TargetControlID="btnDummyRemovalReason"
        PopupControlID="pnlRemovalReason" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRemovalReasonStateComplete() {
            $("#btnDummyRemovalReason").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenRemovalReasonWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemovalReason").attr("src", "wfRemovalReason_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemovalReason").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRemovalReason() {
            var RemovalReasonwindow = $find("<%=mdlPopupRemovalReason.ClientID %>");
            //close Removal Reason popup window
            RemovalReasonwindow.hide();
            //           release resources
            $("#IframeRemovalReason").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnRemovalReason").click();
        }
    </script>
    <!-- End-->
    <!-- Install Selected Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyInstallSelected" Text="Install Selected" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlInstallSelected" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeInstallSelected" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupInstallSelected" runat="server" TargetControlID="btnDummyInstallSelected"
        PopupControlID="pnlInstallSelected" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameInstallSelectedStateComplete() {
            $("#btnDummyInstallSelected").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenInstallSelectedWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeInstallSelected").attr("src", "wfRemovedCompListForWO_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyInstallSelected").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForInstallSelected() {
            var InstallSelectedwindow = $find("<%=mdlPopupInstallSelected.ClientID %>");
            //close Install Selected popup window
            InstallSelectedwindow.hide();
            //           release resources
            $("#IframeInstallSelected").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnInstallSelected").click();
        }
    </script>
    <!-- End-->
    <!-- Install Spare Comp Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySpareCompInstallList" Text="Install Spare Comp"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSpareCompInstallList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSpareCompInstallList" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSpareCompInstallList" runat="server" TargetControlID="btnDummySpareCompInstallList"
        PopupControlID="pnlSpareCompInstallList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSpareCompInstallListStateComplete() {
            $("#btnDummySpareCompInstallList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSpareCompInstallListWindow() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSpareCompInstallList").attr("src", "wfSpareCompListForInstallation_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySpareCompInstallList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSpareCompInstallList() {
            var SpareCompInstallListwindow = $find("<%=mdlPopupSpareCompInstallList.ClientID %>");
            //close Install SpareComp popup window
            SpareCompInstallListwindow.hide();
            //           release resources
            $("#IframeSpareCompInstallList").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSpareCompInstallList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
