<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNewCompMPD_Ajax.aspx.vb"
    Inherits="Flypal.wfNewCompMPD_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Part Inspection Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        button:disabled, button[disabled]
        {
            border: 1px solid #999999;
            background-color: #cccccc;
            color: #666666;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
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
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                        <table id="tblinner" class="clsTablelistin">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Part Insp [New]</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" Display="None" ControlToValidate="txtDescription"
                                                ErrorMessage="Description Required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvDescription" runat="server" Display="None" ControlToValidate="txtDescription"
                                                ErrorMessage="Description can not be more than 1000 chars" CssClass="clsLabelAuto"
                                                ClientValidationFunction="validateName"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvNote" runat="server" Display="None" ControlToValidate="txtNote"
                                                ErrorMessage="Note can not be more than 1000 chars" CssClass="clsLabelAuto" ClientValidationFunction="validateName"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvFrequencyValue" runat="server" Display="None" ErrorMessage="Enter valid Frequency value."
                                                CssClass="clsLabelAuto" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvATAChapter" runat="server" Display="None" ControlToValidate="cmbATAChapter"
                                                ErrorMessage="Select ATA Chapter From List" CssClass="clsLabelAuto" ClientValidationFunction="validateSelection"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvMonitorSerType" runat="server" Display="None" ControlToValidate="cmbMonitorInspType"
                                                ErrorMessage="Select Insp Type from List" CssClass="clsLabelAuto" ClientValidationFunction="validateSelection"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateSelection(source, args) {
                                                    var ControlName = source.controltovalidate;
                                                    switch (ControlName) {
                                                        case 'cmbATAChapter':
                                                            var Value = $get(ControlName).selectedIndex;
                                                            if (Value == 0) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                        case 'cmbMonitorInspType':
                                                            var Value = $get(ControlName).selectedIndex;
                                                            if (Value == 0) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                    }

                                                }

                                                function validateName(source, args) {
                                                    //args.IsValid = false;
                                                    var ControlName = source.controltovalidate;
                                                    switch (ControlName) {
                                                        case 'txtDescription':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 1000) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                        case 'txtNote':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 1000) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                    }
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="upnlMonitorInspectionDetails" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset id="fdsMonitorInspectioningDetails" class="clsFieldSet" style="border-width: 1px;">
                                                            <legend id="lgdMonitorInspectionDetails"><b>Inspection Details</b></legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblCode" class="clsLabelAuto">Code/Form No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mPartMonitorInsp.Code %>"
                                                                            ToolTip="Enter Code" MaxLength="4" Width="252px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <span id="lblStarATA" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblATAChapter" class="clsLabelAuto">ATA Chapter</span>
                                                                    </td>
                                                                    <td>
                                                                        <table cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel ID="upnlATAMaster" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox2_Ajax"
                                                                                                            SelectedValue="<%# mPartMonitorInsp.ATAID %>" DataTextField="ATAChapter" DataValueField="ID"
                                                                                                            Width="255px">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="imgbtnATAChapter" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                            Height="22px" Width="24px" ToolTip="Click to add new ATA chapter." CausesValidation="False">
                                                                                                        </asp:ImageButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblReference" CssClass="clsLabel">Reference</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                            Text="<%# mPartMonitorInsp.Reference %>" ToolTip="Enter Reference" MaxLength="100"
                                                                            TextMode="MultiLine" Width="250px">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <span id="lblStarDesc" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblDescription" class="clsLabel">Description</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                            ClientIDMode="Static" Text="<%# mPartMonitorInsp.Description %>" ToolTip="Enter Description"
                                                                            MaxLength="1000" TextMode="MultiLine" Width="250px">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <span id="lblStarMonitor" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblMonitorInspectionType" class="clsLabelAuto">Inspection Type</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlMonitorInspectionType" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="cmbMonitorInspType" runat="server" CssClass="clsComboBox2_Ajax"
                                                                                    SelectedValue="<%# mPartMonitorInsp.PartMonitorInspTypeID %>" DataTextField="CodeType"
                                                                                    DataValueField="Id" AutoPostBack="True" Width="257px">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblZone" class="clsLabel">Zone </span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                            Text="<%# mPartMonitorInsp.Zone %>" ToolTip="Enter Zone" Width="250px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblArea" class="clsLabelAuto">Area</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                            Text="<%# mPartMonitorInsp.Area %>" ToolTip="Enter Area" Width="250px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblRII" class="clsLabelAuto">RII</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkIsRII" runat="server" Checked="<%# mPartMonitorInsp.IsRII %>"
                                                                            Text="(Check if RII)" CssClass="clsCheckBox" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblNote" class="clsLabel">Note</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="1000"
                                                                            ClientIDMode="Static" Width="250px" Text="<%# mPartMonitorInsp.Note %>" TextMode="MultiLine"
                                                                            ToolTip="Enter Note">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblShowInCofA" class="clsLabelAuto">Show In C of A</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkShowInCofA" runat="server" Checked="<%# mPartMonitorInsp.ShowInCofA %>"
                                                                            ToolTip="Check if want to display in C Of A." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblRequiredmanHours" class="clsLabelAuto">Estd. Man Hours</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRequiredManHours" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                                            MaxLength="8" Text="<%# mPartMonitorInsp.RequiredManHours %>" ToolTip="Enter Required Man Hours">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                                runat="server" class="clsButton_Ajax" causesvalidation="False" />
                                                                                        </td>
                                                                                        <td style="padding-left: 3px;">
                                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                                CausesValidation="false" Text="Remove Attachment" Enabled="False" Width="120px">
                                                                                            </asp:Button>
                                                                                        </td>
                                                                                        <td style="padding-left: 2px;">
                                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                                Height="20px" Width="20px"></asp:ImageButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td valign="top">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlPeriods" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <fieldset id="fdsTotWtAndCapacity" class="clsFieldSet" style="border-width: 1px;">
                                                                        <legend id="lblTotWtAndCapacity"><b>Threshold/Interval</b></legend>
                                                                        <table>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <asp:GridView ID="dgPeriods" runat="server" CssClass="clsGrid" PageSize="3" AutoGenerateColumns="False"
                                                                                        ShowHeaderWhenEmpty="true">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                        <RowStyle CssClass="clsdgItem" />
                                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                            </asp:BoundField>
                                                                                            <asp:TemplateField HeaderText="Threshold">
                                                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtFrequencyValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                        ReadOnly="<%# mPartMonitorInsp.ReadOnlyFrequencyColumn %>" Text='<%# DataBinder.Eval(Container.DataItem, "FrequencyValueFormatted") %>'>
                                                                                                    </asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left">
                                                                                            </asp:ButtonField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                                <td valign="top" align="right">
                                                                                    <asp:ImageButton ID="btnAddPeriodUnit" runat="server" ImageUrl="~/images/plus1.png"
                                                                                        Height="22px" Width="24px" ToolTip="Click to Add New period" CausesValidation="False">
                                                                                    </asp:ImageButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlOtherDetails" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <fieldset id="fdsOtherDetails" class="clsFieldSet" style="border-width: 1px;">
                                                                        <legend id="Legend1"><b>Other Details</b></legend>
                                                                        <table>
                                                                            <tr>
                                                                                <td valign="middle">
                                                                                    <table cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <input id="imgTools" type="image" src="images/Tool.png" disabled="disabled" style="height: 22px;
                                                                                                    width: 24px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton ID="lnkTools" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                                    ToolTip="Click to add Tools" Text="Tools (0 records)"></asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="middle">
                                                                                    <table cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <input id="imgSpares" type="image" src="images/Spare.png" disabled="disabled" style="height: 22px;
                                                                                                    width: 24px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton ID="lnkSpares" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                                    ToolTip="Click to add Spares" Text="Spares (0 records)"></asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="middle">
                                                                                    <table cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <input id="imgTaskCard" type="image" src="images/TaskCard.png" disabled="disabled"
                                                                                                    style="height: 22px; width: 24px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton ID="lnkTaskCards" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                                    ToolTip="Click to add Task Cards" Text="Task Cards (0 records)"></asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
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
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to save Part Inspection"
                                                            Text="Save"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print Part Inspection"
                                                            Visible="false" Text="Print" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                            Text="Back" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlAssemblyDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResultInspList" CssClass="clsLabelHeader" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgMonitorInspStatusList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            PageSize="5" ShowHeaderWhenEmpty="true" CssClass="clsGrid">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <asp:BoundField DataField="CompID" HeaderText="CompID" SortExpression="CompID" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CompStatusID" HeaderText="CompStatusID" SortExpression="CompStatusID"
                                                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CompStatusAsOndateFormatted" HeaderText="CompStatusAsOndateFormatted"
                                                                    SortExpression="AssemblyStatusAsOndateFormatted" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="HourType" HeaderText="HourType" SortExpression="HourType"
                                                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartSerialNo" HeaderText="Part/Serial No." SortExpression="PartSerialNo"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" Width="70px" />
                                                                    <ItemStyle Wrap="true" Width="70px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Aircraft" SortExpression="RegNo">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Last Done On" SortExpression="DoneOnFormatted">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneOnWONo" HeaderText="Work Order No." SortExpression="DoneOnWONo">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneRemark" HeaderText="Remark" SortExpression="DoneRemark">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Effective From/DoneOn Value"
                                                                    HtmlEncode="false" SortExpression="DoneOnValueFormatted">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" Width="130px" />
                                                                    <ItemStyle Width="130px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" SortExpression="CurrentValueFormatted"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ElapsedValueFormattedForGrid" HeaderText="Elapsed" SortExpression="ElapsedValueFormatted"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" SortExpression="ExtensionValueFormatted"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DueOnValueFormattedForGrid" HeaderText="Due At." SortExpression="DueOnValueFormatted"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AssemblyDueOnValueTextFormattedByAirFrameForGrid" HeaderText="Due At Airframe"
                                                                    HtmlEncode="false" SortExpression="AssemblyDueOnValueTextFormattedByAirFrameForGrid">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RemainingValueFormattedForGrid" HeaderText="Remaining"
                                                                    HtmlEncode="false" SortExpression="RemainingValueFormatted">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="Configure" HeaderText="Configure" Text="Config">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="IsConfigurable" HeaderText="IsConfigurable" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="History" HeaderText="History" Text="History">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <asp:BoundField DataField="IsMachineReadOnly" HeaderText="IsMachineReadOnly" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnATAChapter" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnPeriodUnit" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnTools" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnInspHistory" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <!-- Tools Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyTools" Text="Tools" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlTools" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeTools" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupTools" runat="server" TargetControlID="btnDummyTools"
        PopupControlID="pnlTools" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameToolsStateComplete() {
            $("#btnDummyTools").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenToolsWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeTools").attr("src", "wfMaintenanceKitandTask_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyTools").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForTools() {
            var Toolswindow = $find("<%=mdlPopupTools.ClientID %>");
            //close TTools popup window
            Toolswindow.hide();
            //           release resources
            $("#IframeTools").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnTools").click();
        }
    </script>
    <!-- End-->
    <!--Inspection History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemHistory" Text="TaskCard Tool" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemHistory" runat="server" TargetControlID="btnDummyRemHistory"
        PopupControlID="pnlRemHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRemHistoryStateComplete() {
            $("#btnDummyRemHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenInspectionHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemHistory").attr("src", "wfUpdateComplyHistoryCompMonitorInspStatusList_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRemHistory() {
            var RemHistorywindow = $find("<%=mdlPopupRemHistory.ClientID %>");
            //close Removal History popup window
            RemHistorywindow.hide();
            //           release resources
            $("#IframeRemHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnInspHistory").click();
        }
    </script>
    <!-- End-->
    <!-- Period Unit popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPeriodUnit" Text="Period Unit" ClientIDMode="Static"
            CausesValidation="false" />
    </div>
    <asp:Panel runat="server" ID="pnlPeriodUnit" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframePeriodUnit" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupPeriodUnit" runat="server" TargetControlID="btnDummyPeriodUnit"
        PopupControlID="pnlPeriodUnit" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFramePeriodUnitStateComplete() {
            $("#btnDummyPeriodUnit").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenPeriodUnitWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframePeriodUnit").attr("src", "wfSelectPeriodUnit_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyPeriodUnit").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForPeriodUnit() {
            var PeriodUnitwindow = $find("<%=mdlPopupPeriodUnit.ClientID %>");
            //close Period Unit popup window
            PeriodUnitwindow.hide();
            //           release resources
            $("#IframePeriodUnit").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnPeriodUnit").click();
        }
    </script>
    <!-- End-->
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
        PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameFileUploadStateComplete() {
            $("#btnDummyFileUpload").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForFileUpload(fileattached) {
            var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
            //close File Upload popup window
            FileUpwindow.hide();
            //Free resources
            $("#IFileUpload").attr("src", "JavaScript:''");
            if (fileattached) {
                //call hidden button to set file upload content to object
                $("#hdnBtnFileUpload").click();
            }
        }
    </script>
    <!-- End -->
    </form>
</body>
</html>
