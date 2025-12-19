<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNewMPDService_Ajax.aspx.vb" Inherits="Flypal.wfNewMPDService_Ajax" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Model Service Details</title>
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
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
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
                                    <td colspan="2" class="clsFormHeader1Newstyle">
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Model Service [New]</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:Label ID="lblMPDNo" runat="server" Text="" CssClass="clsLabel" Font-Bold="true" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" Display="None" ControlToValidate="txtDescription"
                                                    ErrorMessage="Description Required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvDescription" runat="server" Display="None" ControlToValidate="txtDescription"
                                                    ErrorMessage="Description Can not be more than 1000 chars." CssClass="clsLabelAuto"
                                                    OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvNote" runat="server" Display="None" ControlToValidate="txtNote"
                                                    ErrorMessage="Note Can not be More than 250 chars" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvReference" runat="server" Display="None" ControlToValidate="txtReference"
                                                    ErrorMessage="Reference Too Long" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvATAChapter" runat="server" Display="None" ControlToValidate="cmbATAChapter"
                                                    ErrorMessage="Select ATA Chapter From List" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvMonitorSerType" runat="server" Display="None" ControlToValidate="cmbMonitorServiceType"
                                                    ErrorMessage="Select Service Type from List" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvFrequencyValue" runat="server" ErrorMessage="Enter valid Frequency value."
                                                    Display="None" OnServerValidate="CustomValidate1" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td valign="top">
                                                    <asp:UpdatePanel ID="upnlMonitorServiceDetails" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <fieldset id="fdsMonitorServiceingDetails" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                                <legend id="lgdMonitorServiceDetails" runat="server"> <b><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "MPD Details", "Service Details") %></b></legend>
                                                                <table>
                                                                    <asp:PlaceHolder ID="phTask" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'>
                                                                        <tr>
                                                                            <td><span id="lblStarTaskCardNo" class="clsLabelStar">*</span></td>
                                                                            <td>
                                                                                <span id="lblTaskCardNo" class="clsLabelAuto">Task No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTaskCardNo" runat="server"
                                                                                    CssClass="clsTextBoxTagSearch" Text="<%# mModelMonitorService.TaskCardNo %>"
                                                                                    MaxLength="500" ToolTip="Enter Task No."
                                                                                    Enabled='<%#IIf(mModelMonitorService.ReviseRemark = "", True, False) %>'></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <span id="lblTaskCardHeader" class="clsLabelAuto">Task Header</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTaskCardHeader" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mModelMonitorService.TaskHeading %>"
                                                                                    MaxLength="50" ToolTip="Enter Task Header" TextMode="MultiLine" Width="350px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phCode" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <span id="lblCode" class="clsLabelAuto">Code/Form No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mModelMonitorService.Code %>"
                                                                                    MaxLength="50" ToolTip="Enter Code" Width="250px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </asp:PlaceHolder>
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
                                                                                                            <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                                SelectedValue="<%# mModelMonitorService.ATAID %>" DataTextField="ATAChapter" DataValueField="ID">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="imgbtnATAChapter" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                                Height="22px" Width="24px" ToolTip="Click to add new ATA chapter." CausesValidation="False"></asp:ImageButton>
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
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblReference" CssClass="clsLabel">Reference</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                Text="<%# mModelMonitorService.Reference %>" ToolTip="Enter Reference" MaxLength="100"
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
                                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                ClientIDMode="Static" Text="<%# mModelMonitorService.Description %>" ToolTip="Enter Description"
                                                                                MaxLength="1000" TextMode="MultiLine" Width="250px">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <span id="lblStarMonitor" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblMonitorServiceType" class="clsLabelAuto"><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Task Type", "Service Type") %></span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlMonitorServiceType" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbMonitorServiceType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                        SelectedValue="<%# mModelMonitorService.ModelMonitorServiceTypeID %>" DataTextField="CodeType"
                                                                                        DataValueField="Id" AutoPostBack="True" Width="250px">
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" valign="top">
                                                                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                                        <div>
                                                                                            <div style="float: left; vertical-align: middle; width: 100%">
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <span style="vertical-align: middle; margin-left: 2px; width: 100%" id="lblMastersSelection"
                                                                                                                class="clsLabelHeader">More Details (eg. Zone,Area,Note etc)</span>
                                                                                                        </td>
                                                                                                        <td align="right">
                                                                                                            <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                                                                <image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show More Details...)" />
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>

                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" valign="top">
                                                                            <asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
                                                                                <ContentTemplate>

                                                                                    <asp:Panel ID="pnlAdvancedSearch" runat="server" Style="max-height: 400px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                                                        <table>
                                                                                            <asp:PlaceHolder ID="phApplicability" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <span id="lblSkillCode" runat="server" class="clsLabel">Skill </span>
                                                                                                    </td>

                                                                                                    <td>
                                                                                                        <table style="margin-left: -3px">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="cmbSkillcode" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                                        SelectedValue="<%# mModelMonitorService.MPDSkillID %>" DataTextField="CodeType"
                                                                                                                        DataValueField="Id" AutoPostBack="True" Width="150px" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <span id="lblMPDType" class="clsLabelAuto">Section</span>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="cmbMPDType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                                        SelectedValue="<%# mModelMonitorService.MPDTypeID %>" DataTextField="CodeType"
                                                                                                                        DataValueField="Id" AutoPostBack="True" Width="150px" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>

                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <span id="lblSource" class="clsLabel">Source Doc.</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtSource" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                                            ClientIDMode="Static" Text="<%# mModelMonitorService.Source %>" ToolTip="Enter Source"
                                                                                                            TextMode="MultiLine" Width="350px">
                                                                                                        </asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <span id="lblApplicability" class="clsLabel">Applicability</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtApplicability" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                                            ClientIDMode="Static" Text="<%# mModelMonitorService.Applicability %>" ToolTip="Enter Applicability"
                                                                                                            TextMode="MultiLine" Width="350px">
                                                                                                        </asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>


                                                                                            </asp:PlaceHolder>

                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <span id="lblZone" runat="server" class="clsLabel">Zone </span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                                        Text="<%# mModelMonitorService.Zone %>" ToolTip="Enter Zone" Width="350px"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <span id="lblArea" class="clsLabelAuto">Area</span>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <asp:TextBox ID="txtArea" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                                        Text="<%# mModelMonitorService.Area %>" ToolTip="Enter Area" Width="350px"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <span id="lblRequiredmanHours" class="clsLabelAuto">Estd. Man Hours</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtRequiredManHours" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                        MaxLength="8" Text="<%# mModelMonitorService.RequiredManHours %>" ToolTip="Enter Required Man Hours">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <asp:PlaceHolder ID="phAccess" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <span id="lblAccess" runat="server" class="clsLabel">Access </span>
                                                                                                    </td>

                                                                                                    <td>
                                                                                                        <table style="margin-left: -3px">
                                                                                                            <tr>
                                                                                                                <td>

                                                                                                                    <asp:TextBox ID="txtAccess" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="50"
                                                                                                                        Text="<%# mModelMonitorService.Access %>" ToolTip="Enter Access" Width="150px" TextMode="MultiLine"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <span id="lblAccessManHours" class="clsLabelAuto">Access Man Hours</span>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtAccessManHours" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        MaxLength="8" Text="<%# mModelMonitorService.AccessOpenCloseManHours %>" ToolTip="Enter Access Man Hours">
                                                                                                                    </asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>

                                                                                                    </td>

                                                                                                </tr>

                                                                                            </asp:PlaceHolder>

                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <span id="lblRII" class="clsLabelAuto">RII</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="chkIsRII" runat="server" Checked="<%# mModelMonitorService.IsRII %>"
                                                                                                        Text="(Check if Repeat/Independent Inspection Required)" CssClass="clsCheckBox" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <span id="lblNote" class="clsLabel">Note</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="1000"
                                                                                                        ClientIDMode="Static" Width="350px" Text="<%# mModelMonitorService.Note %>" TextMode="MultiLine"
                                                                                                        ToolTip="Enter Note">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <asp:PlaceHolder ID="phShowCofA" runat="server" Visible="false">
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <span id="lblShowInCofA" class="clsLabelAuto">Show In C of A</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="chkShowInCofA" runat="server" Checked="<%# mModelMonitorService.ShowInCofA %>"
                                                                                                            ToolTip="Check if want to display in C Of A." />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </asp:PlaceHolder>

                                                                                            <tr>
                                                                                                <td></td>
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
                                                                                                                            runat="server" class="clsbtnH clsinfoH1" causesvalidation="False" />
                                                                                                                    </td>
                                                                                                                    <td style="padding-left: 3px;">
                                                                                                                        <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                                                            CausesValidation="false" Text="Remove Attachment" Enabled="False"></asp:Button>
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
                                                                                    </asp:Panel>
                                                                                    <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
                                                                                        ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
                                                                                        CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
                                                                                        CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                                                                        ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                                                        SuppressPostBack="false" />

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
                                                                        <fieldset id="fdsFrequencyofMonitorService" class="clsFieldSetNewStyle" style="border-width: 1px; width: auto">
                                                                            <legend id="lgdFrequencyofMonitorService"><b>Threshold/Interval</b></legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td valign="top">
                                                                                        <asp:GridView ID="dgPeriods" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="3" AutoGenerateColumns="False"
                                                                                            ShowHeaderWhenEmpty="true">
                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                            <RowStyle CssClass="clsdgItem" />
                                                                                            <HeaderStyle CssClass="clsdgHeader nodrag nodrop" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="PeriodUnitName" HeaderText="Period">
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:TemplateField HeaderText="Threshold">
                                                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtFrequencyValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                            MaxLength="8" ReadOnly="<%# iif(mModelMonitorService.ReadOnlyFrequencyColumn=true,true,false) %>"
                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "FrequencyValueFormatted") %>'>
                                                                                                        </asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <%--<asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left">
                                                                                            </asp:ButtonField>--%>
                                                                                                <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/remove.jpg" Style="height: 20px; width: 20px" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </td>
                                                                                    <td valign="top" align="right">
                                                                                        <asp:ImageButton ID="btnAddPeriodUnit" runat="server" ImageUrl="~/images/plus1.png"
                                                                                            Height="22px" Width="24px" ToolTip="Click to Add New period" CausesValidation="False"></asp:ImageButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 100px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlOtherDetails" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <fieldset id="fdsOtherDetails" class="clsFieldSetNewStyle" style="border-width: 1px; width: auto">
                                                                            <legend id="Legend1"><b>Other Details</b></legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td valign="middle">
                                                                                        <table cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input id="imgTools" type="image" src="images/Tool.png" disabled="disabled" style="height: 22px; width: 24px" />
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
                                                                                                    <input id="imgSpares" type="image" src="images/Spare.png" disabled="disabled" style="height: 22px; width: 24px" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:LinkButton ID="lnkSpares" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                                        ToolTip="Click to add Spares" Text="Spares (0 records)"></asp:LinkButton>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <asp:PlaceHolder ID="tsk" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True" And AppSettings("ShowMaintenanceForNewClientsWithTaskCard") = "False", False, True) %>'>

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
                                                                                </asp:PlaceHolder>
                                                                                <tr>
                                                                                    <td valign="middle">
                                                                                        <table cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input id="Image2" type="image" src="images/LM2.png" disabled="disabled" style="height: 22px; width: 24px" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:LinkButton ID="lnkLinkMaintenance" runat="server" CausesValidation="true" CssClass="clsLinkButton"
                                                                                                        ToolTip="Click to add Link Maintenance Activity" Visible='<%# iif(AppSettings("LinkMaintenance")="True",true,false) %>'
                                                                                                        Text="Link Maintenance Activity (0 records)"></asp:LinkButton>
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
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to save Model Service"
                                                                Text="Save"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to print Model Service"
                                                                Visible="false" Text="Print" CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to previous page"
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
                                                            <asp:Label ID="lblResultServiceList" CssClass="clsLabelHeader" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgMonitorServiceStatusList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                PageSize="5" ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundField DataField="AssemblyID" HeaderText="AssemblyID" SortExpression="AssemblyID"
                                                                        HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyStatusID" HeaderText="AssemblyStatusID" SortExpression="AssemblyStatusID"
                                                                        HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyStatusAsOndateFormatted" HeaderText="AssemblyStatusAsOndateFormatted"
                                                                        SortExpression="AssemblyStatusAsOndateFormatted" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="HourType" HeaderText="HourType" SortExpression="HourType"
                                                                        HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ModelSerialNo" HeaderText="Model/Serial No." SortExpression="ModelSerialNo"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="true" Width="70px" />
                                                                        <ItemStyle Wrap="true" Width="70px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RegNo" HeaderText="Aircraft" SortExpression="RegNo">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnFormatted" HeaderText="Last Done On" SortExpression="DoneOnFormatted">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnWONo" HeaderText="Work Order No." SortExpression="DoneOnWONo">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneRemark" HeaderText="Remark" SortExpression="DoneRemark">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Effective From/DoneOn Value"
                                                                        HtmlEncode="false" SortExpression="DoneOnValueFormatted">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="true" Width="130px" />
                                                                        <ItemStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" SortExpression="CurrentValueFormatted"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ElapsedValueFormattedForGrid" HeaderText="Elapsed" SortExpression="ElapsedValueFormatted"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" SortExpression="ExtensionValueFormatted"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DueOnValueFormattedForGrid" HeaderText="Due At." SortExpression="DueOnValueFormatted"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyDueOnValueTextFormattedByAirFrameForGrid" HeaderText="Due At Airframe"
                                                                        HtmlEncode="false" SortExpression="AssemblyDueOnValueTextFormattedByAirFrameForGrid">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RemainingValueFormattedForGrid" HeaderText="Remaining"
                                                                        HtmlEncode="false" SortExpression="RemainingValueFormatted">
                                                                        <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:ButtonField CommandName="Configure" HeaderText="Configure" Text="Config">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <asp:BoundField DataField="IsConfigurable" HeaderText="IsConfigurable" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                    <%-- <asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <asp:ButtonField CommandName="History" HeaderText="History" Text="History">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>--%>
                                                                    <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                    <%--   <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>--%>
                                                                    <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                    <asp:BoundField DataField="IsMachineReadOnly" HeaderText="IsMachineReadOnly" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <%-- <span id="button">Login</span>--%>
                                                                            <div class="dropdown">
                                                                                <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="EditView" runat="server"
                                                                                                    CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" Visible='<%#  not Eval("IsMachineReadOnly") and not Eval("IsConfigurable")  %>'
                                                                                                    ImageUrl="~/images/edit.png" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server"
                                                                                                    CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                    CausesValidation="false" CommandName="DeleteRec"
                                                                                                    Style="height: 20px; width: 20px" Visible='<%#  not Eval("IsMachineReadOnly") and not Eval("IsConfigurable")  %>'
                                                                                                    ImageUrl="~/images/delete.png" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="History" runat="server"
                                                                                                    CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                    CommandName="History" Style="height: 20px; width: 20px"
                                                                                                    ImageUrl="~/images/History.png"
                                                                                                    Visible='<%#  Eval("IsMaster")%>' />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="View" runat="server"
                                                                                                    CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                    CommandName="ViewRec" Style="height: 20px; width: 13px"
                                                                                                    ImageUrl="icons/CLIP01.ICO"
                                                                                                    Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                    Style="cursor: pointer" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
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
                                                <asp:Button ID="hdnBtnLinkMaintActivity" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                <asp:Button ID="hdnBtnServiceHistory" ClientIDMode="Static" runat="server" Text="----"
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
            $(document).ready(function () {
                $("#imgbtnATAChapter").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupATA").attr("src", "wfATA_Ajax.aspx?Type=pup");
                        if (!$.browser.msie) {
                            $("#btnDummyATA").click();
                            $get("AjaxLoader").style.visibility = "hidden";
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }


                });
            });
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
        <!-- Link Maint Activity Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyLinkMaintActivity" Text="Tools" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlLinkMaintActivity" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeLinkMaintActivity" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupLinkMaintActivity" runat="server" TargetControlID="btnDummyLinkMaintActivity"
            PopupControlID="pnlLinkMaintActivity" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameLinkMaintActivityStateComplete() {
                $("#btnDummyLinkMaintActivity").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLinkMaintActivityWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeLinkMaintActivity").attr("src", "wfLinkMaintenanceActivity_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyLinkMaintActivity").click();
                        $get("AjaxLoader").style.visibility = 'hidden';

                    }


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForLinkMaintActivity() {
                var LinkMaintActivitywindow = $find("<%=mdlPopupLinkMaintActivity.ClientID %>");
                //close LinkMaintActivity popup window
                LinkMaintActivitywindow.hide();
                //           release resources
                $("#IframeLinkMaintActivity").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnLinkMaintActivity").click();
            }
        </script>
        <!-- End-->
        <!--Service History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyServiceHistory" Text="Service History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlServiceHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeServiceHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupServiceHistory" runat="server" TargetControlID="btnDummyServiceHistory"
            PopupControlID="pnlServiceHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameServiceHistoryStateComplete() {
                $("#btnDummyServiceHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenServiceHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeServiceHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorServiceStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyServiceHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForServiceHistory() {
                var ServiceHistorywindow = $find("<%=mdlPopupServiceHistory.ClientID %>");
                //close Service History popup window
                ServiceHistorywindow.hide();
                //           release resources
                $("#IframeServiceHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnServiceHistory").click();
            }
        </script>
        <!-- End-->
        <!--call parent function after completing subroutine..(when page open as popup)-->
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForModelServiceMaster();
                return false;
            }
        </script>
        <!--Set page layout when open as popup aspx page-->
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameModelServiceMasterStateComplete();
                }
            });

    <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
           <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
