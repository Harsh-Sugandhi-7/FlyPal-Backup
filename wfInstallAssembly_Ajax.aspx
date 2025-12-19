<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfInstallAssembly_Ajax.aspx.vb"
    Inherits="Flypal.wfInstallAssembly_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Installed Assembly</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <style type="text/css">
        .clsCursorStyle {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table border="0" class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Install Assembly</asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlContainer" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <cc2:TabContainer ID="tabContainer" runat="server" AutoPostBack="true">
                                <cc2:TabPanel ID="tbPnlInstallAssembly" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        Install Assembly
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="tblinner" class="clsTablelistin" border="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                    <asp:CustomValidator ID="cvLicenseNo" runat="server" OnServerValidate="customvalidate"
                                                                        ControlToValidate="txtLicenceNo" ErrorMessage="Enter correct License No" Display="None"
                                                                        CssClass="clsLabelAuto"></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="cvATAChapter" runat="server" OnServerValidate="CustomValidate"
                                                                        ControlToValidate="cmbATAChapter" ErrorMessage="Select ATA Chapter From List."
                                                                        Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="cvModelList" runat="server" OnServerValidate="CustomValidate"
                                                                        ControlToValidate="cmbModelList" ErrorMessage="Select Model From List." Display="None"
                                                                        CssClass="clsLabelAuto"></asp:CustomValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                                                        ErrorMessage="Serial No Required." Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                                    <asp:CustomValidator ID="cvInstallationReasonLen" runat="server" OnServerValidate="CustomValidate"
                                                                        ControlToValidate="txtInstallationReason" ErrorMessage="Installation Reason Too Long."
                                                                        Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="cvInstallationRemark" runat="server" OnServerValidate="CustomValidate"
                                                                        ControlToValidate="txtNote" ErrorMessage="Installation Remark Too Long." Display="None"
                                                                        CssClass="clsLabelAuto"></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="cvAssemblyInstallationValue" runat="server" Display="None"
                                                                        OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="CustomValidate"
                                                                        ControlToValidate="txtInstalledOnDate" ValidateEmptyText="true" ErrorMessage="Installation Remark Too Long."
                                                                        Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlInstallationDetails" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                                                        <legend id="lblInstallationInfo" runat="server" style="font-weight: bold;">Installation
                                                                        Information of the []</legend>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="lblNameStar1" class="clsLabelStar">*</span>
                                                                                </td>
                                                                                <td style="width: 105px">
                                                                                    <span id="lblAircraft" class="clsLabel">Aircraft</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:DropDownList ID="cmbMachineList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                        DataValueField="ID" DataTextField="RegNo" SelectedValue="<%# mAssemblyStatus.MachineID %>"
                                                                                        Width="100px" Enabled="False">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Label1" class="clsLabelStar">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="lblInstalledOn" class="clsLabel">Installed On</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:TextBox runat="server" ID="txtInstalledOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'InstalledOnDate_watermarkextender','true');"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtInstalledOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInstalledOnDate"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtInstalledOnDate" ID="InstalledOnDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="lblATAChapterStar1" class="clsLabelStar">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="lblATAChapter" class="clsLabel">ATA Chapter</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:UpdatePanel ID="upnlATADetails" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <table id="Table8" border="0" cellspacing="0" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                                                            DataTextField="ATAChapter" SelectedValue="<%# mAssemblyStatus.ATAID %>" Width="185px">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="ImgBtnATAChapter" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                            Height="22px" Width="24px" ToolTip="Click to Add New ATA Chapter" CausesValidation="False"></asp:ImageButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <asp:UpdatePanel ID="upnlModelDetails" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 105px">
                                                                                                        <span id="lblManufacturer" class="clsLabel">Manufacturer</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtManufacturer" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Manufacturer's Name"
                                                                                                            Text="<%# mAssemblyStatus.ManufacturerName %>" ReadOnly="True" MaxLength="50"
                                                                                                            BackColor="#E0E0E0"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <span id="lblModelStar1" class="clsLabelStar">*</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <span id="lblModel" class="clsLabel">Model</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                                                            DataTextField="ModelName" SelectedValue="<%# mAssemblyStatus.Assembly.ModelID %>"
                                                                                                            Width="185px" AutoPostBack="True">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="lblSerialNoStar1" class="clsLabelStar">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Serial Number"
                                                                                        Text="<%# mAssemblyStatus.Assembly.SerialNo %>" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <span id="lblPosition" class="clsLabel">Position </span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Position"
                                                                                        Text="<%# mAssemblyStatus.Position %>" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <span id="lblInstallationReason" class="clsLabel">Installation Reason</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:TextBox ID="txtInstallationReason" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                                        ToolTip="Enter Installation Reason" Text="<%# mAssemblyStatus.InstallationReason %>"
                                                                                        Width="250px" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <span id="lblWorkOrNo" class="clsLabel">Work Order No.</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:TextBox ID="txtWorkOrNo" runat="server" CssClass="clsTextBoxTextSearch_Ajax"
                                                                                        ToolTip="Enter Installation Work Order Number" Text="<%# mAssemblyStatus.InstallationWONo %>"
                                                                                        MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <span id="lblLicenceNo" class="clsLabelAuto">License No.</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter License No."
                                                                                                            AutoComplete="off" ClientIDMode="Static" OnTextChanged="txtLicenceNo_TextChanged"
                                                                                                            AutoPostBack="true" MaxLength="200"></asp:TextBox>
                                                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_Autocomplete" runat="server"
                                                                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                                            CompletionInterval="1" ServicePath="wfInstallAssembly_Ajax.aspx" ServiceMethod="GetLicenceList"
                                                                                                            TargetControlID="txtLicenceNo" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                                        </cc2:AutoCompleteExtender>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                            Height="22px" Width="24px" ToolTip="Click to select multiple Licence No." CausesValidation="true" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mAssemblyStatus.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                                            ToolTip="<%# mAssemblyStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <span id="lblPlace" class="clsLabelAuto">Place</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBoxTextSearch_Ajax" ToolTip="Enter Place"
                                                                                        Text="<%# mAssemblyStatus.InstPlace %>" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <span id="lblNote" class="clsLabelAuto">Note</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" ToolTip="Enter Installation Note/Remark"
                                                                                        Text="<%# mAssemblyStatus.InstallationRemark %>" MaxLength="200" Width="250px"
                                                                                        TextMode="MultiLine"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                                </td>
                                                                                <td style="padding-left: 4px">
                                                                                    <asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                                            class="clsButton_Ajax" />
                                                                                                    </td>
                                                                                                    <td style="padding-left: 3px;">
                                                                                                        <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                                            Text="Remove Attachment" Enabled="False" Width="120px"></asp:Button>
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
                                                                        <asp:UpdatePanel ID="upnlInstallationValues" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px;">
                                                                                    <legend id="lblAssemblyValues" runat="server" style="font-weight: bold;">Values at Installation</legend>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">
                                                                                                <asp:GridView ID="dgInstallationValue" runat="server" CssClass="clsGrid" ShowHeaderWhenEmpty="true"
                                                                                                    AutoGenerateColumns="False">
                                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                    <Columns>
                                                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period ">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:TemplateField HeaderText="Assembly" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:TextBox ID="txtAssemblyInstallationValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    AutoPostBack="true" OnTextChanged="txtAssemblyInstallationValue_TextChanged"
                                                                                                                    CausesValidation="true" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem,"AssemblyInstallationValueFormatted") %>'>
                                                                                                                </asp:TextBox>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="MachineInstallationValueFormatted" HeaderText="Airframe">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </td>
                                                                                            <td valign="top" align="right">
                                                                                                <asp:ImageButton ID="btnAddPeriod" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                    Width="24px" ToolTip="Click to Add New Periods" CausesValidation="False"></asp:ImageButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </fieldset>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <fieldset id="Fieldset3" class="clsFieldSet" style="border-width: 1px;">
                                                                            <asp:LinkButton ID="lnkPrintLogBookEntry" runat="server" CssClass="clsLinkButton"
                                                                                Font-Italic="true" Font-Size="8pt">View Log Book Entry</asp:LinkButton>
                                                                            <img width="25px" height="25px" style="border: 0" alt="" src="images/HistoryCard.png" />
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">
                                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table2" border="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save Assembly Installation Information"
                                                                                    Text="Save"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Assembly Installation Information"
                                                                                    CausesValidation="False" Text="Print"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                                                    CausesValidation="False" Text="Back"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <!--Dummy panel to open modelpopup-->
                                                    <tr style="height: 0px;">
                                                        <td style="height: 0px;">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                                                <ContentTemplate>
                                                                    <td>
                                                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnimgBtnATAChapter" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnParameter" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnModelModMaster" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnModelServiceMaster" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnModelInspMaster" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnAddPeriod" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                            Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <!--End -->
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="tbPnlComponent" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        Component(s)
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upnlComponentList" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table class="clsTablelistin" id="TABLE1">
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Label ID="lblComponentsCaption" runat="server" CssClass="clsLabelHeader">List of all the Components on the Machine as of Date: [As of Date]. The Time Since New values of all the Components will be as of Date: [As On Date].</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:UpdatePanel ID="upnlFindNowComponentList" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblSearchComponentList" runat="server" CssClass="clsLabelAuto">Search</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbLookInComponentList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                AutoPostBack="True">
                                                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                <asp:ListItem Value="1">ATA Code</asp:ListItem>
                                                                                                <asp:ListItem Value="2">ATA Nomenclature</asp:ListItem>
                                                                                                <asp:ListItem Value="3">Part No.</asp:ListItem>
                                                                                                <asp:ListItem Value="4">Description</asp:ListItem>
                                                                                                <asp:ListItem Value="5">Serial No.</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblForComponentList" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtForComponentList" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
                                                                                                ClientIDMode="Static" Visible="False"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtCodeComponentList" runat="server" CssClass="clsTextBox_Ajax"
                                                                                                ClientIDMode="Static" ToolTip="Enter value." Visible="False"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnFindNowComponentList" runat="server" Text="Find Now" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to find the List as per searching criteria"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:UpdatePanel ID="upnlGridComponentList" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblResultComponentList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                            </td>
                                                                            <td colspan="4">
                                                                                <asp:UpdatePanel ID="upnlActionBtnTopComponentList" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table4" align="right">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddTopComponentList" runat="server" Text="Add New" CausesValidation="False"
                                                                                                        CssClass="clsButton_Ajax" ToolTip="Add New Component Status"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintTopComponentList" runat="server" Text="Print" CausesValidation="False"
                                                                                                        CssClass="clsButton_Ajax" ToolTip="Click to print List of Component"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:GridView ID="dgCompStatusList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                                                                    PageSize="3" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundField Visible="False" DataField="CompStatusID" HeaderText="CompStatusID "></asp:BoundField>
                                                                                        <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
                                                                                            <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info">
                                                                                            <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                                                            <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info">
                                                                                            <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info">
                                                                                            <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="PeriodNameForWeb" HeaderText="Period" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ValueFormatted" HeaderText="At Installation" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="5">
                                                            <asp:UpdatePanel ID="upnlActionBtnComponentList" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table5">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddComponentList" runat="server" Text="Add New" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Add New Component Status"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintComponentList" runat="server" Text="Print" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to print List of Component"></asp:Button>
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
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="tbPnlService" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblServiceListTitle" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upnlService" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table3" class="clsTablelistin">
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Label ID="lblServiceText" runat="server" CssClass="clsLabelHeader">List of all the Services on the Machine as of Date: [As of Date].</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:UpdatePanel ID="upnlFindNowService" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="lblSearchService" class="clsLabelAuto">Search</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbLookInService" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                AutoPostBack="True">
                                                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                <asp:ListItem Value="1">ATA Code</asp:ListItem>
                                                                                                <asp:ListItem Value="2">Description</asp:ListItem>
                                                                                                <asp:ListItem Value="3">Service Type</asp:ListItem>
                                                                                                <asp:ListItem Value="4">Work Order No.</asp:ListItem>
                                                                                                <asp:ListItem Value="5">Show In C of A</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblForService" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtForService" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
                                                                                                Visible="False" MaxLength="25"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtCodeService" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
                                                                                                ClientIDMode="Static" Visible="False" MaxLength="5"></asp:TextBox>
                                                                                            <asp:DropDownList ID="cmbSearchForService" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                                                AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkApplicableService" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  records'
                                                                                                TextAlign="Left" Text='Show ONLY "NOT  APPLICABLE" records'></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnFindNowService" runat="server" Text="Find Now" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to find the List as per searching criteria"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:UpdatePanel ID="upnlGridService" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblResultService" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                            </td>
                                                                            <td colspan="4">
                                                                                <asp:UpdatePanel ID="upnlActionBtnTopService" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table7" cellspacing="0" align="right">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddTopService" runat="server" Text="Add New" CausesValidation="False"
                                                                                                        CssClass="clsButton_Ajax" ToolTip="Click to add new Assembly Service"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintTopService" runat="server" Text="Print" CausesValidation="False"
                                                                                                        CssClass="clsButton_Ajax" ToolTip="Click to print List of Assembly Service"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:GridView ID="dgMonitorServiceStatusList" runat="server" CssClass="clsGrid" PageSize="3"
                                                                                    ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" AllowSorting="True">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                            DataField="AssemblyMonitorServiceStatusID" HeaderText="AssemblyMonitorServiceStatusID"></asp:BoundField>
                                                                                        <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info."
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type"
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info."
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="TaskNo" SortExpression="TaskNo" HeaderText="Task No.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                          <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                            HtmlEncode="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="200px" Wrap="true" />
                                                                                            <ItemStyle Width="200px" Wrap="true" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ModelMonitorServiceCode" SortExpression="ModelMonitorServiceCode"
                                                                                            HeaderText="Monitor Info.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                                        </asp:BoundField>
                                                                                      
                                                                                        <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="PeriodUnitName" HeaderText="Period Unit" HtmlEncode="false"
                                                                                            Visible="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                            DataField="ModelMonitorServiceID" HeaderText="ModelMonitorServiceID"></asp:BoundField>
                                                                                        <asp:ButtonField Text="Comply" HeaderText="Comply" CommandName="Comply" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                        <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                        <asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster"
                                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                                            <HeaderStyle Wrap="False" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                        <asp:ButtonField Text="View" HeaderText="View" CommandName="View" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                        <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
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
                                                    <tr>
                                                        <td colspan="5" align="right">
                                                            <asp:UpdatePanel ID="upnlActionBtnService" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table9" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddService" runat="server" Text="Add New" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to add new Assembly Service"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintService" runat="server" Text="Print" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to print List of Assembly Service"></asp:Button>
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
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="tbPnlInspection" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        Inspection(s)
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upnlInspection" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table10" class="clsTablelistin">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblInspectionsCaption" runat="server" CssClass="clsLabelHeader"> List of all the Inspections on the Machine as of Date: [As of Date].</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlFindNowInspection" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="lblSearchInspection" class="clsLabelAuto">Search</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbLookInInspection" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                AutoPostBack="True">
                                                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                <asp:ListItem Value="1">ATA Code</asp:ListItem>
                                                                                                <asp:ListItem Value="2">Description</asp:ListItem>
                                                                                                <asp:ListItem Value="3">Insp. Type</asp:ListItem>
                                                                                                <asp:ListItem Value="4">Work Order No.</asp:ListItem>
                                                                                                <asp:ListItem Value="5">Show In C of A</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblForInspection" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtForInspection" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
                                                                                                Visible="False" MaxLength="25"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtCodeInspection" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
                                                                                                ClientIDMode="Static" Visible="False" MaxLength="5"></asp:TextBox>
                                                                                            <asp:DropDownList ID="cmbSearchForInspection" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                                                AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkApplicableInspection" runat="server" CssClass="clsLabelAuto"
                                                                                                ToolTip='Check to see only "NOT APPLICABLE"  records' TextAlign="Left" Text='Show ONLY "NOT  APPLICABLE" records'></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnFindNowInspection" runat="server" Text="Find Now" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to find the List as per searching criteria"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlGridInspection" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblResultInspection" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlActionBtnTopInspection" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table14" align="right">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddTopInspection" runat="server" Text="Add New" CausesValidation="False"
                                                                                                        CssClass="clsButton_Ajax" ToolTip="Click to add new Assembly Inspection"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintTopInspection" runat="server" Text="Print" CausesValidation="False"
                                                                                                        CssClass="clsButton_Ajax" ToolTip="Click to print List of Assembly Inspection"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:GridView ID="dgMonitorInspStatusList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                                                                    ShowHeaderWhenEmpty="true" PageSize="3" AutoGenerateColumns="False">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                            DataField="AssemblyMonitorInspStatusID" HeaderText="AssemblyMonitorInspStatusID"></asp:BoundField>
                                                                                        <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info."
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type"
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info."
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ModelMonitorInspCode" SortExpression="ModelMonitorInspCode"
                                                                                            HeaderText="Monitor Info.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                            HtmlEncode="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="PeriodUnitName" HeaderText="Period Unit" HtmlEncode="false"
                                                                                            Visible="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                            DataField="ModelMonitorInspID" HeaderText="ModelMonitorInspID"></asp:BoundField>
                                                                                        <asp:ButtonField Text="Comply" HeaderText="Comply" CommandName="Comply">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster">
                                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
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
                                                    <tr>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlActionBtnInspection" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table13">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddInspection" runat="server" Text="Add New" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to add new Assembly Inspection"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintInspection" runat="server" Text="Print" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to print List of Assembly Inspection"></asp:Button>
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
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="tbPnlDirective" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        Directive(s)
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upnlDirective" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table6" class="clsTablelistin">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblModifications" runat="server" CssClass="clsLabelHeader">List of all the Directives on the Machine as of Date: [As of Date].</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlFindNowDirective" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="lblSearchDirective" class="clsLabelAuto">Search</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbLookInDirective" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                AutoPostBack="True">
                                                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                <asp:ListItem Value="1">ATA Code</asp:ListItem>
                                                                                                <asp:ListItem Value="2">Directive Number</asp:ListItem>
                                                                                                <asp:ListItem Value="3">Description</asp:ListItem>
                                                                                                <asp:ListItem Value="4">Directive Type</asp:ListItem>
                                                                                                <asp:ListItem Value="5">Work Order No.</asp:ListItem>
                                                                                                <asp:ListItem Value="6">Show In C of A</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblForDirective" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtForDirective" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
                                                                                                Visible="False" MaxLength="25"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtCodeDirective" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
                                                                                                ClientIDMode="Static" Visible="False" MaxLength="5"></asp:TextBox>
                                                                                            <asp:DropDownList ID="cmbSearchForDirective" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                                                AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkApplicableDirective" runat="server" CssClass="clsLabelAuto"
                                                                                                ToolTip='Check to see only "NOT APPLICABLE"  records' TextAlign="Left" Text='Show ONLY "NOT  APPLICABLE" records'></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnFindNowDirective" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                                    Text="Find Now" ToolTip="Click to find the List as per searching criteria"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlGridDirective" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblResultDirective" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:UpdatePanel ID="upnlActionBtnTopDirective" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table11" align="right">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddTopDirective" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                                                        Text="Add New" ToolTip="Click to add new Assembly Modification"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintTopDirective" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                                                        Text="Print" ToolTip="Click to print List of Assembly Modification"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:GridView ID="dgMonitorModStatusList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                                                                    ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" PageSize="3">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                            DataField="AssemblyMonitorModStatusID" HeaderText="AssemblyMonitorModStatusID "></asp:BoundField>
                                                                                        <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info."
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type"
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info."
                                                                                            Visible="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ModelMonitorModCode" SortExpression="ModelMonitorModCode"
                                                                                            HeaderText="Monitor Info.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ModNumber" SortExpression="ModNumber" HeaderText="Directive Number">
                                                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                            HtmlEncode="false">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="PeriodUnitNameForWeb" HeaderText="Period Unit" HtmlEncode="false"
                                                                                            Visible="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining" HtmlEncode="false">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                            DataField="ModelMonitorModID" HeaderText="ModelMonitorModID"></asp:BoundField>
                                                                                        <asp:ButtonField Text="Comply" HeaderText="Comply" CommandName="Comply">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster">
                                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
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
                                                    <tr>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlActionBtnDirective" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table12">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddDirective" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                                    Text="Add New" ToolTip="Click to add new Assembly Modification"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintDirective" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                                    Text="Print" ToolTip="Click to print List of Assembly Modification"></asp:Button>
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
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="tbPnlParameters" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        Parameter(s)
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upnlParameters" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table15" class="clsTablelistin" border="0" width="100%">
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                                HeaderText="Fill Up The Following Fields" ValidationGroup="valGrpParameter"></asp:ValidationSummary>
                                                            <asp:CustomValidator ID="cvParameterList" runat="server" Display="None" ControlToValidate="cmbParameterList"
                                                                ClientValidationFunction="validateParameterList" ErrorMessage="Select Parameters from List."
                                                                ValidationGroup="valGrpParameter"></asp:CustomValidator>
                                                            <asp:RequiredFieldValidator ID="rfvMin" ControlToValidate="txtMin" Display="None"
                                                                runat="server" CssClass="clsLabelAuto" ErrorMessage="Min Value Required" ValidationGroup="valGrpParameter"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvMax" ControlToValidate="txtMax" Display="None"
                                                                runat="server" CssClass="clsLabelAuto" ErrorMessage="Max Value Required" ValidationGroup="valGrpParameter"></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cvMin" runat="server" Display="None" ClientValidationFunction="validateMinMaxValue"
                                                                CssClass="clsLabelAuto" ErrorMessage="Max value should be greater than Min value."
                                                                ValidationGroup="valGrpParameter"></asp:CustomValidator>
                                                            <script type="text/javascript">
                                                                function validateParameterList(source, args) {
                                                                    args.IsValid = false;

                                                                    var dd = $get("cmbParameterList");
                                                                    if (dd.selectedIndex != 0) {
                                                                        args.IsValid = true;
                                                                        return;
                                                                    }
                                                                }

                                                                function validateMinMaxValue(source, args) {
                                                                    //args.IsValid = false;
                                                                    var MinValue = parseFloat($get("txtMin").value);
                                                                    var MaxValue = parseFloat($get("txtMax").value);

                                                                    if (MinValue > MaxValue) {
                                                                        args.IsValid = false;
                                                                        return
                                                                    }
                                                                }
                                                            </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="lblParameterListInfo" class="clsLabelHeader">Aircraft Parameter Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table border="0">
                                                                <tr>
                                                                    <td>
                                                                        <span id="Label3" class="clsLabelAuto">Parameter</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlParamterCombo" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="cmbParameterList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                    ClientIDMode="Static" DataValueField="Id" DataTextField="Name">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlParameterMaster" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:ImageButton ID="imgbtnParameter1" runat="server" ImageUrl="~/images/plus1.png"
                                                                                    Height="22px" Width="24px" ToolTip="Click to add new Parameter" CausesValidation="False"></asp:ImageButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="width: 5px"></td>
                                                                    <td>
                                                                        <span id="lblMin" class="clsLabelAuto">Min</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMin" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                            ClientIDMode="Static" MaxLength="4"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblMax" class="clsLabelAuto">Max</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMax" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                            ClientIDMode="Static" MaxLength="4"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="clsButton_Ajax" ToolTip="Click to add parameter in the List"
                                                                ValidationGroup="valGrpParameter"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblResultParameters" runat="server" CssClass="clsLabelHeader">Aircraft Parameter Details</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" colspan="2">
                                                            <asp:GridView ID="dgParameterList" runat="server" CssClass="clsGrid" ToolTip="Assembly Parameter List."
                                                                PageSize="3" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="true">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                    <asp:BoundField DataField="ParameterName" SortExpression="ParameterName" HeaderText="Parameter Name">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ParameterDescription" SortExpression="ParameterDescription"
                                                                        HeaderText="Parameter Description ">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MinValue" HeaderText="Min.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MaxValue" HeaderText="Max">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">
                                                            <asp:UpdatePanel ID="upnlActionBtnParameters" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table17" border="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintParameters" runat="server" Text="Print" CausesValidation="False"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to Print the list of Parameters" Visible="False"></asp:Button>
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
                                    </ContentTemplate>
                                </cc2:TabPanel>
                            </cc2:TabContainer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
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
                        //                        $("#IFileUpload").ready(function () {
                        //                            $("#btnDummyFileUpload").click();
                        //                            $get("AjaxLoader").style.visibility = 'hidden';
                        //                        });
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
        <!-- ATA Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyATA" Text="Dummy ATA" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupATA" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupATA" frameborder="0" allowtransparency="true" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupATA" runat="server" TargetControlID="btnDummyATA"
            PopupControlID="pnlPopupATA" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameATAStateComplete() {
                $("#btnDummyATA").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenATAWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupATA").attr("src", "wfATA_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyATA").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var atawindow = $find("<%=mdlPopupATA.ClientID %>");
                //close ata popup window
                atawindow.hide();
                $("#iPopupATA").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnATAChapter").click();
            }
        </script>
        <!-- End-->
        <!-- Period Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAddPeriod" Text="TaskCard Step" ClientIDMode="Static"
                CausesValidation="false" />
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
        <!-- Parameter Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyParameter" Text="Parameter" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlParameter" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeParameter" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupParameter" runat="server" TargetControlID="btnDummyParameter"
            PopupControlID="pnlParameter" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameParameterStateComplete() {
                $("#btnDummyParameter").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenParameterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeParameter").attr("src", "wfParameter_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyParameter").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForParameter() {
                var Parameterwindow = $find("<%=mdlPopupParameter.ClientID %>");
                //close Parameter popup window
                Parameterwindow.hide();
                //           release resources
                $("#IframeParameter").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnParameter").click();
            }
        </script>
        <!-- End-->
        <!--Model Service Master Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModelServiceMaster" Text="Model Service Master"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlModelServiceMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModelServiceMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModelServiceMaster" runat="server" TargetControlID="btnDummyModelServiceMaster"
            PopupControlID="pnlModelServiceMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModelServiceMasterStateComplete() {
                $("#btnDummyModelServiceMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModelServiceMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModelServiceMaster").attr("src", "wfModelMonitorService_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

                    if (!$.browser.msie) {
                        $("#btnDummyModelServiceMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModelServiceMaster() {
                var ModelServiceMasterwindow = $find("<%=mdlPopupModelServiceMaster.ClientID %>");
                //close Model Service Master popup window
                ModelServiceMasterwindow.hide();
                //           release resources
                $("#IframeModelServiceMaster").attr("src", "JavaScript:''");
                //call Model Service Master image button
                $("#hdnBtnModelServiceMaster").click();
            }
        </script>
        <!-- End-->
        <!--Model Mod Master Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModelModMaster" Text="Model Mod Master" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlModelModMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModelModMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModelModMaster" runat="server" TargetControlID="btnDummyModelModMaster"
            PopupControlID="pnlModelModMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModelModMasterStateComplete() {
                $("#btnDummyModelModMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModelModMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModelModMaster").attr("src", "wfModelMonitorMod_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

                    if (!$.browser.msie) {
                        $("#btnDummyModelModMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModelModMaster() {
                var ModelModMasterwindow = $find("<%=mdlPopupModelModMaster.ClientID %>");
                //close Model Mod Master popup window
                ModelModMasterwindow.hide();
                //           release resources
                $("#IframeModelModMaster").attr("src", "JavaScript:''");
                //call Model Mod Master image button
                $("#hdnBtnModelModMaster").click();
            }
        </script>
        <!-- End-->
        <!--Model Insp Master Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModelInspMaster" Text="Model Insp Master"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlModelInspMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModelInspMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModelInspMaster" runat="server" TargetControlID="btnDummyModelInspMaster"
            PopupControlID="pnlModelInspMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModelInspMasterStateComplete() {
                $("#btnDummyModelInspMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModelInspMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModelInspMaster").attr("src", "wfModelMonitorInspection_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

                    if (!$.browser.msie) {
                        $("#btnDummyModelInspMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModelInspMaster() {
                var ModelInspMasterwindow = $find("<%=mdlPopupModelInspMaster.ClientID %>");
                //close Model Insp Master popup window
                ModelInspMasterwindow.hide();
                //           release resources
                $("#IframeModelInspMaster").attr("src", "JavaScript:''");
                //call Model Insp Master image button
                $("#hdnBtnModelInspMaster").click();
            }
        </script>
        <!-- End-->
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
        <%--autocomplete css functions--%>
        <script type="text/javascript">
            //bold input value in list...
            function ClientPopulated(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
            //Alternate item style
            function ClientShowing(source, eventArgs) {
                $.elements = $(source.get_completionList());
                $.elements.find(".ac_results_li").each(function (i) {
                    if (i % 2 == 0) {
                        //$(this).addClass("ac_even");
                    }
                    else {
                        $(this).addClass("ac_odd");
                    }
                });
            }
            //add loader to textbox
            function ClientPopulating(source, e) {
                $("#" + source._element.id).addClass("ac_loading");
            }
            //remove loader from textbox
            function ClientHiding(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
        </script>
        <%--End--%>
        <!-- Assembly Insp Maintenance Done By Employee Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
        </div>
        <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
            PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameMaintDoneByStateComplete() {
                $("#btnDummyMaintDoneBy").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }


            function AddEmployeeLicNo() {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=1");

                    if (!$.browser.msie) {
                        $("#btnDummyMaintDoneBy").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForMaintDoneBy() {
                var MaintDoneBywindow = $find("<%=mdlPopupMaintDoneBy.ClientID %>");
                //close Ass Insp Maint Done By Emp popup window
                MaintDoneBywindow.hide();
                //Free resources
                $("#IMaintDoneBy").attr("src", "JavaScript:''");
                $("#hdnBtnMaintDoneBy").click();

            }
        </script>
        <!-- End -->
    </form>
</body>
</html>
