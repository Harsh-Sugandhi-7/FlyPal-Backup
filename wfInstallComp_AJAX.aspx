<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfInstallComp_AJAX.aspx.vb"
    Inherits="Flypal.wfInstallComp_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Install Component Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link href="js/semantic.css" rel="stylesheet" type="text/css" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <%--  <script type="text/javascript" src="jquery-1.6.1.min.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .clsCursorStyle
        {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <table class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Component</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <cc2:TabContainer ID="TbContInst" runat="server" AutoPostBack="true">
                                            <cc2:TabPanel ID="tbpnlInstComp" runat="server" CssClass="clsPanel1">
                                                <HeaderTemplate>
                                                    Install Component
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <table class="clsTablelistin" id="tblinner" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                        <asp:CustomValidator ID="cvLicenseNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="Enter correct License No"
                                                                            ControlToValidate="txtLicenceNo" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator><asp:CustomValidator
                                                                                ID="cvInstallationReason" runat="server" Display="None" ControlToValidate="txtInstallationReason"
                                                                                ErrorMessage="Max length of Installation Reason should not be greater than 1000 character."></asp:CustomValidator><asp:CustomValidator
                                                                                    ID="cvAssembly" runat="server" OnServerValidate="customvalidate" Display="None"
                                                                                    ControlToValidate="cmbAssemblyList" ErrorMessage="Please select the Assembly from the list."></asp:CustomValidator><asp:CustomValidator
                                                                                        ID="cvNote" runat="server" Display="None" ControlToValidate="txtNote" ErrorMessage="Max length of Note should not be greater than  200 character."></asp:CustomValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvATAChapter" runat="server" ControlToValidate="cmbATAChapter"
                                                                            CssClass="clslabelAuto" Display="None" ErrorMessage="ATA Chapter Required"></asp:RequiredFieldValidator>
                                                                        <asp:CustomValidator ID="cvATAChapter" runat="server" ControlToValidate="cmbATAChapter"
                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Select ATA Chapter From List."
                                                                            OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvPartNo" runat="server" OnServerValidate="customvalidate"
                                                                            Display="None" ControlToValidate="cmbPartNo" ErrorMessage="Part No Required."></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvPartDesc" runat="server" OnServerValidate="customvalidate"
                                                                            Display="None" ControlToValidate="txtPartDescription" ErrorMessage="Part No Required."></asp:CustomValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <fieldset id="fdsPartInfo" class="clsFieldSet" style="border-width: 1px">
                                                                    <legend id="lblPartInfo" runat="server" style="font-weight: bold"><b>Part Serial No.
                                                                        of the []</b></legend>
                                                                    <asp:UpdatePanel ID="upnlPartOnfo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                </td>
                                                                                                <td width="100px">
                                                                                                    <asp:Button ID="btnPartNo" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                                        Text="Part No." ToolTip="Click to Add the new Part"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlPartNo" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:DropDownList ID="cmbPartNo" runat="server" CssClass="clsComboBox1_Ajax" DataTextField="Name"
                                                                                                                            DataValueField="ID" SelectedValue="<%#  mCompStatus.Comp.PartID %>" AutoPostBack="True">
                                                                                                                        </asp:DropDownList>
                                                                                                                        <asp:TextBox ID="txtPartDescription" runat="server" AutoPostBack="True" CssClass="clsTextBox1_Ajax"
                                                                                                                            MaxLength="25" onfocus="SetContextKey();" Text="<%# mcompstatus.Comp.PartName %>"
                                                                                                                            ToolTip="Enter No" Width="208px"></asp:TextBox>
                                                                                                                        <cc2:AutoCompleteExtender ID="txtPartDescription_Autocomplete" runat="server" ClientIDMode="Static"
                                                                                                                            CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                                            CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetPartNoDescriptionList"
                                                                                                                            ServicePath="wfInstallComp_AJAX.aspx" TargetControlID="txtPartDescription" UseContextKey="False">
                                                                                                                        </cc2:AutoCompleteExtender>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:CheckBox ID="chkByModel" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                                                                                                            ClientIDMode="Static" Text="By Model" ToolTip="Select to search Model wise Part">
                                                                                                                        </asp:CheckBox>
                                                                                                                        <script type="text/javascript">

                                                                                                                            function SetContextKey() {
                                                                                                                                var autoComplete = $find('txtPartDescription_Autocomplete');
                                                                                                                                var IsModelChecked = $get("chkByModel").checked;
                                                                                                                                var ModelID = '<%=mCompStatus.ModelID %>';
                                                                                                                                var str = 'IsByModel=' + IsModelChecked + '¿ModelID=<%=mCompStatus.ModelID%>';
                                                                                                                                autoComplete.set_contextKey(str);
                                                                                                                            }
                                                                                                                        </script>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblATAChapterStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                </td>
                                                                                                <td width="110px">
                                                                                                    <asp:Label ID="lblATAChapter" runat="server" CssClass="clsLabel">ATA Chapter</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:UpdatePanel ID="upnlATAMaster" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox1_Ajax"
                                                                                                                            DataTextField="ATAChapter" DataValueField="ID" SelectedValue="<%# mCompStatus.ATAID %>"
                                                                                                                            Width="225px">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="ImgBtnATAChapter" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                                    Height="22px" Width="24px" ToolTip="Click to Add New ATAChapter" CausesValidation="False">
                                                                                                                </asp:ImageButton>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                </td>
                                                                                                <td width="100px">
                                                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Serial No. </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox2_Ajax" MaxLength="25"
                                                                                                        Text="<%# mCompStatus.Comp.SerialNo %>" ToolTip="Enter Serial Number"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                </td>
                                                                                                <td width="100px">
                                                                                                    <asp:Label ID="lblPosition" runat="server" CssClass="clsLabel">Position </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="clsTextBox2_Ajax" MaxLength="25"
                                                                                                        Text="<%# mCompStatus.Position %>" ToolTip="Enter Position"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <table id="Table1">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblDescription" runat="server" CssClass="clsLabel">Description</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBox2_Ajax"
                                                                                                        MaxLength="200" ReadOnly="True" Text="<%# mCompStatus.Description %>" TextMode="MultiLine"
                                                                                                        ToolTip="Description of the Part" Height="35px"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblCode" runat="server" CssClass="clsLabel" Visible="False">Code</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtCode" runat="server" BackColor="#E0E0E0" CssClass="clsTextBox2_Ajax"
                                                                                                        MaxLength="50" ReadOnly="True" Text="<%# mCompStatus.Comp.Code %>" ToolTip="Code of the Part."
                                                                                                        Visible="False"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblManufacturer" runat="server" CssClass="clsLabel">Manufacturer</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table id="Table6" cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:UpdatePanel ID="upnlManufacturer" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:DropDownList ID="cmbManufacturerList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                                            DataTextField="Name" DataValueField="ID" SelectedValue="<%# mCompStatus.ManufacturerID %>"
                                                                                                                            Width="225px">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:ImageButton ID="imgbtManufacturer1" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                                    Height="22px" Width="24px" ToolTip="Add New Manufacturer to the list" CausesValidation="False">
                                                                                                                </asp:ImageButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right" colspan="2" style="height: 40px">
                                                                                                    <asp:UpdatePanel ID="upnlHistoryCard" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:LinkButton ID="lnkHistoryCard" runat="server" CssClass="clsLinkButton" Font-Italic="true"
                                                                                                                            Font-Size="8pt">View History Card</asp:LinkButton>
                                                                                                                        &nbsp;
                                                                                                                    </td>
                                                                                                                    <td align="right">
                                                                                                                        <img width="25px" height="25px" style="border: 0" alt="" src="images/HistoryCard.png" />
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        &nbsp; &nbsp;
                                                                                                                        <asp:LinkButton ID="lnkPrintLogBookEntry" runat="server" CssClass="clsLinkButton"
                                                                                                                            Font-Italic="true" Font-Size="8pt">View Log Book Entry</asp:LinkButton>
                                                                                                                        &nbsp;
                                                                                                                    </td>
                                                                                                                    <td align="right">
                                                                                                                        <img width="25px" height="25px" style="border: 0" alt="" src="images/HistoryCard.png" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                               <placeholder id="phAC" runat="server" visible="false" >
                                                                                            <tr>
                                                                                                <td colspan="2" style="height: 40px">
                                                                                                 
                                                                                                         <table width="100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblACF" runat="server" CssClass="clsLabel" Visible="False">ACF </asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtACF" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    MaxLength="25" Text="<%# mCompStatus.Comp.ACF %>" ToolTip="Enter ACF" Visible="False"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblECF" runat="server" CssClass="clsLabelAuto" Visible="False">ECF</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtECF" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    MaxLength="25" Text="<%# mCompStatus.Comp.ECF %>" ToolTip="Enter ECF" Visible="False"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFCF" runat="server" CssClass="clsLabel" Visible="False">FCF</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtFCF" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    MaxLength="25" Text="<%# mCompStatus.Comp.FCF %>" ToolTip="Enter FCF" Visible="False"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblRTCF" runat="server" CssClass="clsLabel" Visible="False">RTCF</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtRTCF" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    MaxLength="25" Text="<%# mCompStatus.Comp.RTCF %>" ToolTip="Enter RTCF" Visible="False"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                     
                                                                                                </td>
                                                                                            </tr>
                                                                                             </placeholder>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlIsThrustCompOuter" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:UpdatePanel ID="upnlIsThrustComp" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:CheckBox ID="chkIsThrustComp" runat="server" ClientIDMode="Static" Style="display: none"
                                                                                                                        Checked="<%# mCompStatus.IsThrustMonitoringComp %>" />
                                                                                                                    <div class="ui toggle checkbox">
                                                                                                                        <input id="toggle-1" name="public" type="checkbox"></input><label id="Label1" class="dn"
                                                                                                                            for="toggle-1" data-content="OFF" runat="server"></label>Thrust Monitoring
                                                                                                                    </div>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                                <td align="right">
                                                                                                    <asp:UpdatePanel ID="upnlThrustyComponentDet" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Panel ID="pnlThrustyComponentDet" runat="server">
                                                                                                                <div class="dropdown">
                                                                                                                    <span id="lblThrustyComponentDet" class="dropbtn" runat="server">Thrust Monitoring Detail(s)
                                                                                                                        &#9660;</span>
                                                                                                                    <div class="dropdown-content" style="z-index: 1000000">
                                                                                                                        <div id="myDropdown">
                                                                                                                            <div>
                                                                                                                                <table class="clsGrid">
                                                                                                                                    <tr>
                                                                                                                                        <td class="clsdgHeader" style="font-style: italic; font-size: 9pt" align="center">
                                                                                                                                            <span id="Span1" class="clsLabelHeaderForCollapse" runat="server">Name </span>
                                                                                                                                        </td>
                                                                                                                                        <td class="clsdgHeader" style="font-style: italic; font-size: 9pt" align="center">
                                                                                                                                            <span id="Span2" class="clsLabelHeaderForCollapse" runat="server">Current Values
                                                                                                                                            </span>
                                                                                                                                        </td>
                                                                                                                                        <td class="clsdgHeader" style="font-style: italic; font-size: 9pt" align="center">
                                                                                                                                            <span id="Span3" class="clsLabelHeaderForCollapse" runat="server">Life Limit </span>
                                                                                                                                        </td>
                                                                                                                                        <td class="clsdgHeader" style="font-style: italic; font-size: 9pt" align="center">
                                                                                                                                            <span id="Span4" class="clsLabelHeaderForCollapse" style="height: 40px" runat="server">
                                                                                                                                                Monitor with </span>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td align="center">
                                                                                                                                            <span id="lblB22" class="clsLabel" runat="server">B22</span>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:TextBox ID="txtB22Current" runat="server" Height="20px" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                                Text="<%# mCompStatus.B22CurrentValue %>"></asp:TextBox>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:TextBox ID="txtB22LifeLimit" runat="server" Height="20px" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                                onchange="setattr(this);" Text="<%# mCompStatus.B22LifeLimit %>"></asp:TextBox>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:RadioButton ID="chkB22IsCurrent" runat="server" CssClass="clsCheckBox" GroupName="a"
                                                                                                                                                Checked="<%# mCompStatus.B22IsCurrentThrust %>" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td align="center">
                                                                                                                                            <span id="lblB24" class="clsLabel" runat="server">B24</span>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:TextBox ID="txtB24Current" runat="server" Height="20px" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                                Text="<%# mCompStatus.B24CurrentValue %>"></asp:TextBox>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:TextBox ID="txtB24LifeLimit" runat="server" Height="20px" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                                onchange="setattr(this);" Text="<%# mCompStatus.B24LifeLimit %>"></asp:TextBox>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:RadioButton ID="chkB24IsCurrent" runat="server" CssClass="clsCheckBox" GroupName="a"
                                                                                                                                                Checked="<%# mCompStatus.B24IsCurrentThrust %>" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td align="center">
                                                                                                                                            <span id="lblB26" class="clsLabel" runat="server">B26</span>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:TextBox ID="txtB26Current" runat="server" Height="20px" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                                Text="<%# mCompStatus.B26CurrentValue %>"></asp:TextBox>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:TextBox ID="txtB26LifeLimit" runat="server" Height="20px" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                                onchange="setattr(this);" Text="<%# mCompStatus.B26LifeLimit %>"></asp:TextBox>
                                                                                                                                        </td>
                                                                                                                                        <td align="center">
                                                                                                                                            <asp:RadioButton ID="chkB26IsCurrent" runat="server" CssClass="clsCheckBox" GroupName="a"
                                                                                                                                                Checked="<%# mCompStatus.B26IsCurrentThrust %>" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </asp:Panel>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlIsFanBladeDistribution" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <fieldset id="FieldsetFanBladeDistribution" class="clsFieldSet" style="border-width: 1px;"
                                                                            runat="server">
                                                                            <legend id="Legend1" runat="server" style="font-weight: bold;">Fan Blade Monitoring
                                                                                <asp:CheckBox ID="chkFanBladeMonitoring" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                                                                    Checked="<%# mCompStatus.IsFanBladeDistribution %>" />
                                                                            </legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span id="Span5" class="clsLabel" runat="server">Position </span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFanBladePosition" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                            ClientIDMode="Static" Text="<%# mCompStatus.FanBladePosition %>" Style="margin-left: 5px;"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <span id="Span6" class="clsLabel" runat="server" style="margin-left: 5px;">Moment Weight
                                                                                        </span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtMomentWeight" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                            onchange="setattr(this);" ClientIDMode="Static" Text="<%# mCompStatus.MomentWeight %>"
                                                                                            Style="margin-left: 5px;"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <span id="Span7" class="clsLabel" runat="server" style="margin-left: 5px;">Balance Screw
                                                                                        </span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtBalanceScrew" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                            onchange="setattr(this);" ClientIDMode="Static" Text="<%# mCompStatus.BalanceScrew %>"
                                                                                            Style="margin-left: 5px;"></asp:TextBox>
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
                                                                <fieldset id="fdsInstallationInfo" class="clsFieldSet" style="border-width: 1px">
                                                                    <legend id="lbInstallationInfo" runat="server" style="font-weight: bold"><b>Installation
                                                                        Information of the []</b></legend>
                                                                    <asp:UpdatePanel ID="upnlInstInfo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <asp:Button ID="btnSelectLog" runat="server" CssClass="clsButton_Ajax" Text="Select Log"
                                                                                            ToolTip="Click to open Select Log screen"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <table id="Table2" border="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblAsseblyStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbAssemblyList" runat="server" AutoPostBack="True" CssClass="clsComboBox1_Ajax"
                                                                                                                    DataTextField="RegNoModelSerialNo" DataValueField="ID" SelectedValue="<%# mCompStatus.AssemblyID %>"
                                                                                                                    Width="225px">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblInstalledOn" runat="server" CssClass="clsLabel">Installed On</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="calInstalledOn" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                                                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');" TabIndex="1"
                                                                                                                    Width="90px"></asp:TextBox>
                                                                                                                <cc2:CalendarExtender ID="calInstalledOn_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                                    Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="calInstalledOn">
                                                                                                                </cc2:CalendarExtender>
                                                                                                                <cc2:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                                                    Enabled="True" TargetControlID="calInstalledOn" WatermarkCssClass="clsDateTextBox"
                                                                                                                    WatermarkText="<%$ AppSettings:DateFormat %>"></cc2:TextBoxWatermarkExtender>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblInstallationReason" runat="server" CssClass="clsLabel">Installation Reason </asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtInstallationReason" runat="server" CssClass="clsTextBoxMultiLine2"
                                                                                                                    MaxLength="1000" Text="<%# mCompStatus.InstallationReason %>" TextMode="MultiLine"
                                                                                                                    ToolTip="Enter Installation Reason"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No. </asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox2_Ajax" MaxLength="150"
                                                                                                                    Text="<%# mCompStatus.InstallationWONo %>" ToolTip="Enter Work Order Number"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblNote" runat="server" CssClass="clsLabel">Note </asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiLine2" MaxLength="500"
                                                                                                                    Text="<%# mCompStatus.InstallationRemark %>" TextMode="MultiLine" ToolTip="Enter Note/Remark regarding Installation"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="tbtDoneby" runat="server" CssClass="clsLabelAuto">Done By Agency</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtDoneBy" runat="server" CssClass="clsTextBox2_Ajax" MaxLength="100"
                                                                                                                    Text="<%# mCompStatus.InstDoneBy %>" ToolTip="Enter Work Done by Name"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblLicenceNo" runat="server" CssClass="clsLabelAuto" Width="72px">License No.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
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
                                                                                                                                        CompletionInterval="1" ServicePath="wfComplyAssemblyMonitorInspStatus_Ajax.aspx"
                                                                                                                                        ServiceMethod="GetLicenseNoList" TargetControlID="txtLicenceNo" OnClientItemSelected="SetLicenceNo"
                                                                                                                                        UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                                                                    </cc2:AutoCompleteExtender>
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                                                        Height="22px" Width="24px" ToolTip="Click to Add New Licence No." CausesValidation="true" />
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td colspan="2">
                                                                                                                                    <asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mCompStatus.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                                                                        ToolTip="<%# mCompStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblPlace" runat="server" CssClass="clsLabelAuto" Width="72px">Place</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox2_Ajax" MaxLength="25"
                                                                                                                    Text="<%# mCompStatus.InstPlace %>" ToolTip="Enter Place"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <table id="Table12" border="0">
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                                                                                <ContentTemplate>
                                                                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                                                                                    clientidmode="Static" runat="server" class="clsButton_Ajax" />
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                                                                                    Text="Remove Attachment" Enabled="False" Width="120px"></asp:Button>
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                                    Height="20px" Width="20px"></asp:ImageButton>
                                                                                                                                                <asp:Button ID="Button1" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                                                                                                    Style="display: none;"></asp:Button>
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
                                                                                                    </table>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <label id="L1">
                                                                                                    </label>
                                                                                                </td>
                                                                                                <td valign="top" align="right">
                                                                                                    <table border="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblAssemblyValues" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Values at</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="cmbInstallationStatus" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mCompStatus.InstallationStatusID %>"
                                                                                                                    Width="130px">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="right" valign="top" colspan="2">
                                                                                                                <asp:UpdatePanel ID="upnlInstallationValue" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:GridView ID="dgInstallationValue" runat="server" AutoGenerateColumns="False"
                                                                                                                            Visible="true" CssClass="clsGrid" PageSize="3" ShowHeaderWhenEmpty="true">
                                                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                                            <Columns>
                                                                                                                                <asp:BoundField DataField="PeriodName" HeaderText="Period ">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:TemplateField HeaderText="Component">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtCompInstallationValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"CompInstallationValueFormatted") %>'
                                                                                                                                            AutoPostBack="true" OnTextChanged="txtCompInstallationValue_TextChanged" ClientIDMode="Static"></asp:TextBox>
                                                                                                                                        <asp:CustomValidator ID="cvCompInstallationValue" runat="server" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator></ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:BoundField DataField="AssemblyInstallationValueFormatted" HeaderText="Assembly"
                                                                                                                                    HtmlEncode="false">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:ButtonField CommandName="DeleteRec" HeaderText="Remove" Text="Remove">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:ButtonField>
                                                                                                                            </Columns>
                                                                                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                                                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                                        </asp:GridView>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                            <td align="left" rowspan="4" valign="top">
                                                                                                                <asp:ImageButton ID="btnAddPeriod" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                    Width="24px" ToolTip="Click to Add New Periods" CausesValidation="False"></asp:ImageButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:Button ID="hdnBtnSelectLog" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnBtnManufacturer" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnBtnPart" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnAddPeriod" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnBtnSeriviceMasterList" runat="server" CausesValidation="False"
                                                                                                    ClientIDMode="Static" Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnBtnInspMaster" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnimgBtnATAChapter" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnBtnModMaster" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="Add" />
                                                                                                <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                                <asp:Button ID="hdnThrustValue" ClientIDMode="Static" runat="server" Text="----"
                                                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <!--End -->
                                                                                <tr>
                                                                                    <td align="right" valign="top">
                                                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <table id="tblButton">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save information of Installation Component" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                                                Text="Print" ToolTip="Click to print Installed Component" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                                                Text="Back" ToolTip="Click to go back to previous page" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                                Style="display: none;" Text="----" />
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
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                            <cc2:TabPanel ID="tbpnlServiceList" runat="server" CssClass="clsPanel1" Visible="<%# Not mCompStatus.IsNew %>">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblServiceListTitle" runat="server" ></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <table id="tblmain1" class="clstablelistin">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="Panel1" runat="server" CssClass="clspnl1">
                                                                    <table id="Table3" class="clsTablelistin">
                                                                        <%-- <tr>
                                                                            <td colspan="5">
                                                                                <asp:Label ID="Label1" runat="server" CssClass="clstitle1">Install Component Monitor Service Status List</asp:Label>
                                                                            </td>
                                                                        </tr>--%>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:UpdatePanel ID="upnlServiceInfo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblServiceInfo" runat="server" CssClass="clsLabelAuto">List of all the Services on the Component as of Date: [As of Date].</asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" align="left">
                                                                                <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table5" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel">Search</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbLookIn" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                        <asp:ListItem Value="1">ATA Code</asp:ListItem>
                                                                                                        <asp:ListItem Value="2">Service Type</asp:ListItem>
                                                                                                        <asp:ListItem Value="3">Work Order No.</asp:ListItem>
                                                                                                        <asp:ListItem Value="4">Show In C of A</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right">
                                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    <asp:Label ID="lblFor" runat="server" CssClass="clsLabel" Visible="False">For</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtFor" runat="server" ToolTip="Enter value." CssClass="clsTextBox2_Ajax"
                                                                                                        Visible="False" MaxLength="25"></asp:TextBox>
                                                                                                    <asp:TextBox ID="txtCode1" runat="server" ToolTip="Enter value." CssClass="clsTextBox2_Ajax"
                                                                                                        Visible="False" MaxLength="5"></asp:TextBox>
                                                                                                    <asp:DropDownList ID="cmbSearchFor" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                                                        AutoPostBack="True" DataTextField="CodeType" DataValueField="ID">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="chkApplicableService" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  records'
                                                                                                        TextAlign="Left" Text='Show ONLY "NOT  APPLICABLE" records'></asp:CheckBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table7">
                                                                                            <tr>
                                                                                                <td align="right">
                                                                                                    <asp:Button ID="btnFindNow" runat="server" ToolTip="Click to Find the list as per searching criteria."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Find Now"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlCaption" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblCaption" runat="server" CssClass="clsLabelHeader">List of Component Service Status : Record(s) found.</asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td colspan="4" align="right">
                                                                                <asp:UpdatePanel ID="upnlServiceTopButtons" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table8" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddTopService" TabIndex="0" runat="server" ToolTip="Click to Add new Install Component Service Status "
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Visible="false" Text="Add New">
                                                                                                    </asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintTop" runat="server" ToolTip="Click to Print the Install Component Service Status List."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Visible="false" Text="Print">
                                                                                                    </asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnCloseTopService" runat="server" Visible="false" ToolTip="Back to Previous Page"
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Back"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:UpdatePanel ID="upnlServiceGrid" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="dgMonitorServiceStatusList" runat="server" AllowSorting="True"
                                                                                            AutoGenerateColumns="False" CssClass="clsGrid" DataKeyNames="CompStatusID" PageSize="5"
                                                                                            ShowHeaderWhenEmpty="True">
                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                            <Columns>
                                                                                                <asp:BoundField Visible="False" DataField="CompMonitorServiceStatusID" HeaderText="ID">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info."
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type"
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info."
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PartMonitorServiceCode" SortExpression="PartMonitorServiceCode"
                                                                                                    HeaderText="Monitor Info.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                                    HtmlEncode="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="200px" Wrap="true" />
                                                                                                    <ItemStyle Width="200px" Wrap="true" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
                                                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PeriodUnitName" HeaderText="Period Unit" Visible="false">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At." HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PartMonitorServiceID" HeaderText="PartMonitorServiceID"
                                                                                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                                                </asp:BoundField>
                                                                                                <asp:ButtonField Text="Comply" HeaderText="Comply" CommandName="Comply"></asp:ButtonField>
                                                                                                <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec"></asp:ButtonField>
                                                                                                <asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster">
                                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                </asp:ButtonField>
                                                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
                                                                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:ButtonField>
                                                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                                                    HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                                                    <ItemStyle CssClass="hideGridColumn" />
                                                                                                </asp:BoundField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5" align="right">
                                                                                <asp:UpdatePanel ID="upnlServiceButtons" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table9" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddService" TabIndex="0" runat="server" ToolTip="Click to Add new Install Component Service Status "
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintService" runat="server" ToolTip="Click to Print the Install Component Service Status List."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Print"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnCloseService" runat="server" ToolTip="Back to Previous Page" CssClass="clsButton_Ajax"
                                                                                                        CausesValidation="False" Text="Back"></asp:Button>
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
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                            <cc2:TabPanel ID="tbpnlInspList" runat="server" CssClass="clsPanel1" Visible="<%# Not mCompStatus.IsNew %>">
                                                <HeaderTemplate>
                                                    Inspection(s)
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <table id="Table4" class="clstablelistin">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="Panel2" runat="server" CssClass="clspnl1">
                                                                    <table id="Table10" class="clsTablelistin">
                                                                        <%-- <tr>
                                                                            <td colspan="5">
                                                                                <asp:Label ID="Label1" runat="server" CssClass="clstitle1">Install Component Monitor Insp Status List</asp:Label>
                                                                            </td>
                                                                        </tr>--%>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:UpdatePanel ID="upnlInspInfo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblInspInfo" runat="server" CssClass="clsLabelAuto">List of all the Inspections on the Component as of Date: [As of Date].</asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" align="left">
                                                                                <asp:UpdatePanel ID="upnlSearchInsp" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table11" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblSearchInsp" runat="server" CssClass="clsLabel">Search</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbLookInInsp" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                        <asp:ListItem Value="1">ATA Code</asp:ListItem>
                                                                                                        <asp:ListItem Value="2">Insp Type</asp:ListItem>
                                                                                                        <asp:ListItem Value="3">Work Order No.</asp:ListItem>
                                                                                                        <asp:ListItem Value="4">Show In C of A</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right">
                                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    <asp:Label ID="lblForInsp" runat="server" CssClass="clsLabel" Visible="False">For</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtForInsp" runat="server" ToolTip="Enter value." CssClass="clsTextBox2_Ajax"
                                                                                                        Visible="False" MaxLength="25"></asp:TextBox>
                                                                                                    <asp:TextBox ID="txtCode1Insp" runat="server" ToolTip="Enter value." CssClass="clsTextBox2_Ajax"
                                                                                                        Visible="False" MaxLength="5"></asp:TextBox>
                                                                                                    <asp:DropDownList ID="cmbSearchForInsp" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                                                        Visible="false" AutoPostBack="True" DataTextField="CodeType" DataValueField="ID">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="chkApplicableInspection" runat="server" CssClass="clsLabelAuto"
                                                                                                        ToolTip='Check to see only "NOT APPLICABLE"  records' TextAlign="Left" Text='Show ONLY "NOT  APPLICABLE" records'>
                                                                                                    </asp:CheckBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table13">
                                                                                            <tr>
                                                                                                <td align="right">
                                                                                                    <asp:Button ID="btnFindNowInsp" runat="server" ToolTip="Click to Find the list as per searching criteria."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Find Now"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlCaptionInsp" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblCaptionInsp" runat="server" CssClass="clsLabelHeader">List of Component Insp Status : Record(s) found.</asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td colspan="4" align="right">
                                                                                <asp:UpdatePanel ID="upnlInspTopButtons" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table14" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddTopInsp" TabIndex="0" runat="server" Visible="false" ToolTip="Click to Add new Install Component Insp Status "
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintTopInsp" runat="server" Visible="false" ToolTip="Click to Print the Install Component Insp Status List."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Print"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnCloseTopInsp" runat="server" Visible="false" ToolTip="Back to Previous Page"
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Back"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:UpdatePanel ID="upnlInspGrid" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="dgMonitorInspStatusList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                            CssClass="clsGrid" DataKeyNames="CompStatusID" PageSize="5" ShowHeaderWhenEmpty="True">
                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                            <Columns>
                                                                                                <asp:BoundField Visible="False" DataField="CompMonitorInspStatusID" HeaderText="ID">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info."
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type"
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info."
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PartMonitorInspCode" SortExpression="PartMonitorInspCode"
                                                                                                    HeaderText="Monitor Info.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                                    HtmlEncode="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
                                                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PeriodUnitName" HeaderText="Period Unit" Visible="false">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At." HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PartMonitorInspID" HeaderText="PartMonitorInspID" HeaderStyle-CssClass="hideGridColumn"
                                                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                                                </asp:BoundField>
                                                                                                <asp:ButtonField Text="Comply" HeaderText="Comply" CommandName="Comply"></asp:ButtonField>
                                                                                                <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec"></asp:ButtonField>
                                                                                                <asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster">
                                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                </asp:ButtonField>
                                                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
                                                                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:ButtonField>
                                                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                                                    HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                                                    <ItemStyle CssClass="hideGridColumn" />
                                                                                                </asp:BoundField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5" align="right">
                                                                                <asp:UpdatePanel ID="upnlInspButtons" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table15" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddInsp" TabIndex="0" runat="server" ToolTip="Click to Add new Install Component Insp Status "
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintInsp" runat="server" ToolTip="Click to Print the Install Component Insp Status List."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Print"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnCloseInsp" runat="server" ToolTip="Back to Previous Page" CssClass="clsButton_Ajax"
                                                                                                        CausesValidation="False" Text="Back"></asp:Button>
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
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                            <cc2:TabPanel ID="tbpnlModList" runat="server" CssClass="clsPanel1" Visible="<%# Not mCompStatus.IsNew %>">
                                                <HeaderTemplate>
                                                    Modification(s)
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <table id="Table16" class="clstablelistin">
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="Panel3" runat="server" CssClass="clspnl1">
                                                                    <table id="Table17" class="clsTablelistin">
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:UpdatePanel ID="upnlModInfo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblModInfo" runat="server" CssClass="clsLabelAuto">List of all the Modifications on the Component as of Date: [As of Date].</asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" align="left">
                                                                                <asp:UpdatePanel ID="upnlSearchMod" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table18" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblSearchMod" runat="server" CssClass="clsLabel">Search</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbLookInMod" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                        <asp:ListItem Value="1">ATA Code</asp:ListItem>
                                                                                                        <asp:ListItem Value="2">Mod Type</asp:ListItem>
                                                                                                        <asp:ListItem Value="3">Work Order No.</asp:ListItem>
                                                                                                        <asp:ListItem Value="4">Show In C of A</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right">
                                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    <asp:Label ID="lblForMod" runat="server" CssClass="clsLabel" Visible="False">For</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtForMod" runat="server" ToolTip="Enter value." CssClass="clsTextBox2_Ajax"
                                                                                                        Visible="False" MaxLength="25"></asp:TextBox>
                                                                                                    <asp:TextBox ID="txtCode1Mod" runat="server" ToolTip="Enter value." CssClass="clsTextBox2_Ajax"
                                                                                                        Visible="False" MaxLength="5"></asp:TextBox>
                                                                                                    <asp:DropDownList ID="cmbSearchForMod" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                                                        Visible="false" AutoPostBack="True" DataTextField="CodeType" DataValueField="ID">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="chkApplicableDirective" runat="server" CssClass="clsLabelAuto"
                                                                                                        ToolTip='Check to see only "NOT APPLICABLE"  records' TextAlign="Left" Text='Show ONLY "NOT  APPLICABLE" records'>
                                                                                                    </asp:CheckBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table19">
                                                                                            <tr>
                                                                                                <td align="right">
                                                                                                    <asp:Button ID="btnFindNowMod" runat="server" ToolTip="Click to Find the list as per searching criteria."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Find Now"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlCaptionMod" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblCaptionMod" runat="server" CssClass="clsLabelHeader">List of Component Mod Status : Record(s) found.</asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td colspan="4" align="right">
                                                                                <asp:UpdatePanel ID="upnlModTopButtons" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table20" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddTopMod" TabIndex="0" runat="server" ToolTip="Click to Add new Install Component Mod Status "
                                                                                                        CssClass="clsButton_Ajax" Visible="false" CausesValidation="False" Text="Add New">
                                                                                                    </asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintTopMod" runat="server" ToolTip="Click to Print the Install Component Mod Status List."
                                                                                                        CssClass="clsButton_Ajax" Visible="false" CausesValidation="False" Text="Print">
                                                                                                    </asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnCloseTopMod" runat="server" ToolTip="Back to Previous Page" CssClass="clsButton_Ajax"
                                                                                                        CausesValidation="False" Visible="false" Text="Back"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <asp:UpdatePanel ID="upnlModGrid" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="dgMonitorModStatusList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                            CssClass="clsGrid" DataKeyNames="CompStatusID" PageSize="5" ShowHeaderWhenEmpty="True">
                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                            <Columns>
                                                                                                <asp:BoundField Visible="False" DataField="CompMonitorModStatusID" HeaderText="ID">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info."
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type"
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info."
                                                                                                    Visible="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PartMonitorModCode" SortExpression="PartMonitorModCode"
                                                                                                    HeaderText="Monitor Info.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                                    HtmlEncode="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ModNumber" HeaderText="Mod No." SortExpression="ModNumber"
                                                                                                    HtmlEncode="false">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
                                                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PeriodUnitName" HeaderText="Period Unit" Visible="false">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At." HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining" HtmlEncode="false">
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PartMonitorModID" HeaderText="PartMonitorModID" HeaderStyle-CssClass="hideGridColumn"
                                                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                                                </asp:BoundField>
                                                                                                <asp:ButtonField Text="Comply" HeaderText="Comply" CommandName="Comply"></asp:ButtonField>
                                                                                                <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec"></asp:ButtonField>
                                                                                                <asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster">
                                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                                </asp:ButtonField>
                                                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
                                                                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                </asp:ButtonField>
                                                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                                                    HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                                                    <ItemStyle CssClass="hideGridColumn" />
                                                                                                </asp:BoundField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5" align="right">
                                                                                <asp:UpdatePanel ID="upnlModButtons" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table21" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAddMod" TabIndex="0" runat="server" ToolTip="Click to Add new Install Component Mod Status "
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnPrintMod" runat="server" ToolTip="Click to Print the Install Component Mod Status List."
                                                                                                        CssClass="clsButton_Ajax" CausesValidation="False" Text="Print"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnCloseMod" runat="server" ToolTip="Back to Previous Page" CssClass="clsButton_Ajax"
                                                                                                        CausesValidation="False" Text="Back"></asp:Button>
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
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                        </cc2:TabContainer>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" ClientIDMode="Static" DynamicLayout="false"
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
    <!-- Select SelectSelectLog popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySelectLog" Text="Maintenance Activity" ClientIDMode="Static" />
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
            //close Task Card Tool popup window
            SelectLogwindow.hide();
            //           release resources
            $("#IframeSelectLog").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSelectLog").click();
        }
    </script>
    <!-- End-->
    <!-- Select SelectManufacturer popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyManufacturer" Text="TaskCard Tool" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlManufacturer" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeManufacturer" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupManufacturer" runat="server" TargetControlID="btnDummyManufacturer"
        PopupControlID="pnlManufacturer" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameManufacturerStateComplete() {
            $("#btnDummyManufacturer").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenManufacturerWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeManufacturer").attr("src", "wfManufacturer_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyManufacturer").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForManufacturer() {
            var Manufacturerwindow = $find("<%=mdlPopupManufacturer.ClientID %>");
            //close Task Card Tool popup window
            Manufacturerwindow.hide();
            //           release resources
            $("#IframeManufacturer").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnManufacturer").click();
        }
    </script>
    <!-- End-->
    <div>
        <!-- Part Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyPart" Text="Dummy Part" ClientIDMode="Static">
            </asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlPart" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframePart" frameborder="0" height="100%" allowtransparency="true" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupPart" runat="server" TargetControlID="btnDummyPart"
            PopupControlID="pnlPart" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFramePartStateComplete() {
                $("#btnDummyPart").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenPartWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframePart").attr("src", "wfPart_AJAX.aspx?Type=pup");
                    // $("#IframePart").load(function () {
                    //                    var doc = IframePart.window;
                    //                    IframePart.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummyPart").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForPart() {
                var Partwindow = $find("<%=mdlPopupPart.ClientID %>");
                //close Part popup window
                Partwindow.hide();
                //           release resources
                $("#IframePart").attr("src", "JavaScript:''");
                //call Part image button
                $("#hdnBtnPart").click();
            }
        </script>
        <!-- End-->
    </div>
    <div>
        <!-- Period Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAddPeriod" Text="TaskCard Step" ClientIDMode="Static" />
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
    <div>
        <!-- SeriviceMasterList Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySeriviceMasterList" Text="Dummy SeriviceMasterList"
                ClientIDMode="Static"></asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlSeriviceMasterList" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeSeriviceMasterList" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSeriviceMasterList" runat="server" TargetControlID="btnDummySeriviceMasterList"
            PopupControlID="pnlSeriviceMasterList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameSeriviceMasterListStateComplete() {
                $("#btnDummySeriviceMasterList").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenSeriviceMasterListWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeSeriviceMasterList").attr("src", "wfPartMonitorServiceList_AJAX.aspx?Type=pup");
                    // $("#IframeSeriviceMasterList").load(function () {
                    //                    var doc = IframeSeriviceMasterList.window;
                    //                    IframeSeriviceMasterList.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummySeriviceMasterList").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSeriviceMasterList() {
                var SeriviceMasterListwindow = $find("<%=mdlPopupSeriviceMasterList.ClientID %>");
                //close SeriviceMasterList popup window
                SeriviceMasterListwindow.hide();
                //           release resources
                $("#IframeSeriviceMasterList").attr("src", "JavaScript:''");
                //call SeriviceMasterList image button
                $("#hdnBtnSeriviceMasterList").click();
            }
        </script>
        <!-- End-->
    </div>
    <!-- SeriviceMaster Popup Window -->
    <div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySeriviceMaster" Text="Dummy SeriviceMaster"
                ClientIDMode="Static"></asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlSeriviceMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeSeriviceMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSeriviceMaster" runat="server" TargetControlID="btnDummySeriviceMaster"
            PopupControlID="pnlSeriviceMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameServiceMasterStateComplete() {
                $("#btnDummySeriviceMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenSeriviceMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeSeriviceMaster").attr("src", "wfPartMonitorService_AJAX.aspx?Type=pup&GChildPage2=&GChildPage4=wfInstallComp_AJAX.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx");
                    // $("#IframeSeriviceMaster").load(function () {
                    //                    var doc = IframeSeriviceMaster.window;
                    //                    IframeSeriviceMaster.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummySeriviceMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForServiceMaster() {
                var SeriviceMasterwindow = $find("<%=mdlPopupSeriviceMaster.ClientID %>");
                //close SeriviceMaster popup window
                SeriviceMasterwindow.hide();
                //           release resources
                $("#IframeSeriviceMaster").attr("src", "JavaScript:''");
                //call SeriviceMaster image button
                $("#hdnBtnSeriviceMaster").click();
            }
        </script>
        <!-- End-->
    </div>
    <!-- InspMaster Popup Window -->
    <div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyInspMaster" Text="Dummy InspMaster" ClientIDMode="Static">
            </asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlInspMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeInspMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupInspMaster" runat="server" TargetControlID="btnDummyInspMaster"
            PopupControlID="pnlInspMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameInspMasterStateComplete() {
                $("#btnDummyInspMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenInspMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage2=&GChildPage4=wfInstallComp_AJAX.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx");
                    // $("#IframeInspMaster").load(function () {
                    //                    var doc = IframeInspMaster.window;
                    //                    IframeInspMaster.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummyInspMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForInspMaster() {
                var InspMasterwindow = $find("<%=mdlPopupInspMaster.ClientID %>");
                //close InspMaster popup window
                InspMasterwindow.hide();
                //           release resources
                $("#IframeInspMaster").attr("src", "JavaScript:''");
                //call InspMaster image button
                $("#hdnBtnInspMaster").click();
            }
        </script>
        <!-- End-->
    </div>
    <!-- ModMaster Popup Window -->
    <div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModMaster" Text="Dummy ModMaster" ClientIDMode="Static">
            </asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlModMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModMaster" runat="server" TargetControlID="btnDummyModMaster"
            PopupControlID="pnlModMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModMasterStateComplete() {
                $("#btnDummyModMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModMaster").attr("src", "wfPartMonitorMod_AJAX.aspx?Type=pup&GChildPage4=wfInstallComp_AJAX.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx");
                    // $("#IframeModMaster").load(function () {
                    //                    var doc = IframeModMaster.window;
                    //                    IframeModMaster.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummyModMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModMaster() {
                var ModMasterwindow = $find("<%=mdlPopupModMaster.ClientID %>");
                //close ModMaster popup window
                ModMasterwindow.hide();
                //           release resources
                $("#IframeModMaster").attr("src", "JavaScript:''");
                //call ModMaster image button
                $("#hdnBtnModMaster").click();
            }
        </script>
        <!-- End-->
    </div>
    <!-- ATA Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyATA" Text="Dummy ATA" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupATA" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
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
    <!-- Assembly Insp Maintenance Done By Employee Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
    </div>
    <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
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
                $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=3");

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
    <script type="text/javascript">
        function SetLicenceNo(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }

            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtLicenceNo_Autocomplete") {
                textbox = document.getElementById('hdnLicenceNo');
            }


            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
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
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <script type="text/javascript" src="Notification/jQuery/ui.core.js"></script>
    <script type="text/javascript" src="Notification/jQuery/ui.notificationmsg.js"></script>
    <script src="bootstrap/bootstrap-toggle.min.js" type="text/javascript"></script>
    <script src="js/semantic.js" type="text/javascript"></script>
    <script type="text/javascript">
        function setattr(elem) {
            var No = $(elem).val();
            if ($(elem).val() == "") {
                $(elem).val('0');
            }
        }
        function pnlThrustyComponentDetVisible() {
            $("#hdnThrustValue").click();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            if ($("#chkIsThrustComp").is(":disabled") == false) {
                if ($("#chkIsThrustComp").prop('checked') == true) {
                    $("label[for='" + "toggle-1" + "']").removeClass('dn').addClass('up').attr('data-content', 'ON');
                    //  $("label[for='" + "toggle-1" + "']").addClass('.ui.toggle.checkbox');
                }
                else {
                    $("label[for='" + "toggle-1" + "']").removeClass('up').addClass('dn').attr('data-content', 'OFF');
                }
            }
            else {
                $("#toggle-1").attr('disabled', 'disabled');
                $("label[for='" + "toggle-1" + "']").addClass('up').attr('data-content', 'ON');
                $("label[for='" + "toggle-1" + "']").unbind();
            }

            if ($("#chkIsThrustComp").is(":disabled") == false) {
                $('.ui.toggle.checkbox').checkbox().first().checkbox({ onChecked: function () {
                    if ($("#chkIsThrustComp").prop('checked') == true) {
                        $("label[for='" + $(this).attr("id") + "']").removeClass('up').addClass('dn').attr('data-content', 'OFF');
                        $("#chkIsThrustComp").prop('checked', false);

                    }
                    else {
                        $("label[for='" + $(this).attr("id") + "']").removeClass('dn').addClass('up').attr('data-content', 'ON');
                        $("#chkIsThrustComp").prop('checked', true);
                    }

                    pnlThrustyComponentDetVisible();
                },
                    onUnchecked: function () {
                        $("label[for='" + $(this).attr("id") + "']").removeClass('up').addClass('dn').attr('data-content', 'OFF');
                        $("#chkIsThrustComp").prop('checked', false);
                        pnlThrustyComponentDetVisible();
                    }
                });
            }
            else {
                $("#toggle-1").attr('disabled', 'disabled');
                $("label[for='" + "toggle-1" + "']").addClass('up').attr('data-content', 'ON');
                $("label[for='" + "toggle-1" + "']").unbind();
            }
        });
    </script>
    <script type="text/javascript">
        /*
        // Toggle with Labels and Green Background
        */
        if ($("#chkIsThrustComp").is(":disabled") == false) {
            $('.ui.toggle.checkbox').checkbox().first().checkbox({ onChecked: function () {
                if ($("#chkIsThrustComp").prop('checked') == true) {
                    $("label[for='" + $(this).attr("id") + "']").removeClass('up').addClass('dn').attr('data-content', 'OFF');
                    $("#chkIsThrustComp").prop('checked', false);

                }
                else {
                    $("label[for='" + $(this).attr("id") + "']").removeClass('dn').addClass('up').attr('data-content', 'ON');
                    $("#chkIsThrustComp").prop('checked', true);
                }

                pnlThrustyComponentDetVisible();
            },
                onUnchecked: function () {
                    $("label[for='" + $(this).attr("id") + "']").removeClass('up').addClass('dn').attr('data-content', 'OFF');
                    $("#chkIsThrustComp").prop('checked', false);
                    pnlThrustyComponentDetVisible();
                }
            });
        }
        else {
            $("#toggle-1").attr('disabled', 'disabled');
            $("label[for='" + "toggle-1" + "']").addClass('up').attr('data-content', 'ON');
            $("label[for='" + "toggle-1" + "']").unbind();
        }
    </script>
    </form>
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var resetTodaysDate = 'false';
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
</body>
</html>
