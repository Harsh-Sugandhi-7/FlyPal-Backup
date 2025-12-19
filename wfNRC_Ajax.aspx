<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNRC_Ajax.aspx.vb" Inherits="Flypal.wfNRC_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NRC Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="5400">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="Table1" class="clstablelistout" border="0" cellspacing="1" cellpadding="1"
            width="100%">
            <tr>
                <td>
                    <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">NRC Detail</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <table id="Table8">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ValidationGroup="a"
                                                                    ToolTip="Click to save NRC" Text="Save"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to print"
                                                                    Enabled="<%# not mNRC.IsNew %>" Text="Print" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
                                                                    Text="Close" CausesValidation="False"></asp:Button>
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
                                <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" Width="100%" ValidationGroup="a"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" CssClass="clsValidationSummary"
                                            ValidationGroup="a">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="cvTechLicenseNoList" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="a" Display="None" ClientValidationFunction="validateName" ControlToValidate="txtRaisedBy"
                                            ErrorMessage="Select Raised By Person."></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAir" runat="server" ClientValidationFunction="validateName"
                                            CssClass="clsValidationSummary" ValidationGroup="a" Display="None" ControlToValidate="cmbAircraftList"
                                            ErrorMessage="Select Aircraft."></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvDoneByAME" runat="server" CssClass="clsValidationSummary"
                                            Display="None" ClientValidationFunction="validateName" ControlToValidate="txtDoneByAME"
                                            ErrorMessage="AME name should not be same as Technician." ValidationGroup="a"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCloseDate" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" Display="None" ControlToValidate="txtDoneOnDate"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateName(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'txtObservation':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 500) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtRectification':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 500) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtRaisedBy':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value == '') {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'cmbAircraftList':
                                                        var Value = $get("cmbAircraftList");
                                                        if (Value.selectedIndex == 0) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtDoneByAME':
                                                        var DoneByAME = $get(ControlName).value;
                                                        var DoneByTech = document.getElementById('txtDoneByTech');
                                                        if (DoneByAME == DoneByTech.value) {
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
                            <td>
                                <asp:UpdatePanel ID="upnlNRCDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="ldNRCDetail" class="clsFieldSet1" runat="server"><b>NRC Detail</b></legend>
                                            <table id="tblNRCDetail">
                                                <tr>
                                                    <td>
                                                        <span id="lblDateStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDate" class="clsLabelAuto">Date</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtNRCDate" runat="server" CssClass="clsTextBoxTagSearchDate" AutoPostBack="true"
                                                                        onchange="ValidateDateText(this,'txtNRCDate_CalendarExtender');" 
                                                                        Enabled="<%# mNRC.IsNew %>"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtNRCDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtNRCDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtNRCDate"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                    <asp:TextBox ID="txtTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                        Enabled="<%# mNRC.IsNew %>" Visible='<%# iif(AppSettings("ClientCode") = "STR",True,False) %>'
                                                                        MaxLength="10" ToolTip="Enter Time" Width="65px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNRCTextNoStar" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblText" class="clsLabelAuto">Text</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtText" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        CssClass="clsTextBoxTagSearch" Text="<%# mNRC.Text %>" ToolTip="Enter Text" Enabled="<%# mNRC.IsNew %>"></asp:TextBox>
                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mNRC.No %>"
                                                                        Enabled="<%# mNRC.IsNew %>" ToolTip="Enter No." MaxLength="4"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" CompletionInterval="1"
                                                                        ServicePath="wfnWODetail_AJAX.aspx" ServiceMethod="GetTextList" TargetControlID="txtText"
                                                                        UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblWoNo" class="clsLabelAuto">WO. No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtWONo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mNRC.WorkOrderNo %>"
                                                                        ToolTip="Enter Created By" MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblAircraftStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        AutoPostBack="True" DataValueField="ID" DataTextField="RegNo" SelectedValue="<%# mNRC.MachineID %>"
                                                                        Enabled="<%# mNRC.IsNew %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtModelNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="false"
                                                                        Text="<%# mNRC.ModelName %>" ToolTip="Aircraft Model">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="false"
                                                                        Text="<%# mNRC.SerialNo %>" ToolTip="Serial No.">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEng1" runat="server" CssClass="clsTextBoxTagSearch" Enabled="false"
                                                                        ToolTip="Eng. Model" Text="<%# mNRC.EngineModelName %>">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblRaisedByStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblRaisedBy" class="clsLabelAuto">Raised By</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtRaisedBy" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtRaisedBy_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
                                                                        Enabled="<%# mNRC.IsNew %>"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtRaisedBy_Autocomplete" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                        CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtRaisedBy"
                                                                        OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnRaisedByEmpID" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                                <td>
                                                                    <span id="lblAcceptedThrough" class="clsLabelAuto">Accepted By</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAcceptedThrough" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mNRC.AcceptedThrough %>">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                        SelectedValue="<%# mNRC.ATAID %>" DataValueField="ID" DataTextField="ATAChapter">
                                                                    </asp:DropDownList>
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
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlNRCJob" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabelHeader">NRC Job(s)</span>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:DropDownList ID="cmbAddJobType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                                            <asp:ListItem Selected="True" Text="New Job" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Add Job from MEL/Snag" Value="3"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <%--Value=3 from csnWOJobType table--%>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:ImageButton ID="btnNRCJob" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                            Width="24px" ToolTip="Click to Add NRC Job" CausesValidation="true" ValidationGroup="a">
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </legend>
                                            <table id="Table3">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgNRCJobs" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeaderWhenEmpty="True">
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Observation" HeaderText="Observation" HtmlEncode="false">
                                                                    <HeaderStyle Wrap="true" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak" Width="300px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ObserveByAMEName" HeaderText="AME">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Rectification" HeaderText="Rectification.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak" Width="300px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneByAMEName" HeaderText="AME">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneByTechEName" HeaderText="Tech.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>                                                                
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                CausesValidation="false" />

                                                                                        </td>
                                                                                        <td>
                                                                                             <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                 CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />

                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
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
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upblManHours" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Man Hours</b></legend>
                                            <table id="Table4">
                                                <tr>
                                                    <td>
                                                        <%--<span id="Span1" class="clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <span id="lblManHourAME" class="clsLabelAuto">AME</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtManHourAME" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="5"
                                                                        Text="<%# mNRC.ManHourAME %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblManHourTech" class="clsLabelAuto">Technician</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtManHourTech" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="5"
                                                                        Text="<%# mNRC.ManHourTech %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblManHourOther" class="clsLabelAuto">Other</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtManHourOther" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                        MaxLength="5" Text="<%# mNRC.ManHourOther %>"></asp:TextBox>
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
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlNRCPartOnOff" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabelHeader">Part On/Off Details</span>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnAddNRCPartOnOff" runat="server" ImageUrl="~/images/plus1.png"
                                                                Height="22px" Width="24px" ToolTip="Click to On/Off Part Information" CausesValidation="true"
                                                                ValidationGroup="a"></asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </legend>
                                            <table id="Table5">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgNRCPartOnOff" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeaderWhenEmpty="true">
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OffPartDescription" HeaderText="Description">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OffPartName" HeaderText="Off Part No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OffPartSerialNo" HeaderText="Off Serial No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OnPartName" HeaderText="On Part No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OnPartSerialNo" HeaderText="On Serial No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReleaseNoteNo" HeaderText="Rel. Note No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>  
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                CausesValidation="false" />
                                                                                            
                                                                                        </td>
                                                                                        <td>
                                                                                             <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                 CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                            
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
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
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlNRCSpare" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabelHeader">Used Spare Details</span>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnAddNRCSpare" runat="server" ImageUrl="~/images/plus1.png"
                                                                Height="22px" Width="24px" ToolTip="Click to add Spare info." CausesValidation="true"
                                                                ValidationGroup="a"></asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </legend>
                                            <table id="Table9">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgNRCSpare" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeaderWhenEmpty="true">
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" HeaderText="Description">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RequiredQty" HeaderText="Requested Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EffRate" HeaderText="Landing Rate">
                                                                    <HeaderStyle  Wrap="False"  HorizontalAlign="Right">
                                                                    </HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EstimatedCost" HeaderText="Actual Cost">
                                                                    <HeaderStyle  Wrap="False" HorizontalAlign="Right">
                                                                    </HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak" Width="200px" />
                                                                </asp:BoundField>
                                                               
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                           <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                               CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                               CausesValidation="false" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
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
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlPrevNRCNo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <%-- <legend id="Legend2" class="clsFieldSet1" runat="server"><b>NRC Detail</b></legend>--%>
                                            <table id="Table6">
                                                <tr>
                                                    <td>
                                                        <%--<span id="Span1" class="clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <span id="lblPrevNRCNo" class="clsLabelAuto">Prev. OJS/NRC No.</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtPrevNRCNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mNRC.PrevNRCNo %>"
                                                                        ToolTip="Enter Prev. NRC No." MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDoneOnDate" class="clsLabelAuto">Done On Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoneOnDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchDate"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtDoneOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDoneOnDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtDoneOnDate_TextBoxWatermarkExtender" runat="server"
                                                                        TargetControlID="txtDoneOnDate" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblPlace" class="clsLabelAuto">Place</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mNRC.Place %>"
                                                                        ToolTip="Enter Place" MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<span id="Span6" class="clsLabelStar">*</span>--%>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="Span1" class="clsLabelAuto">(If used in continuation)</span>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<span id="Span6" class="clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <span id="lblAME" class="clsLabelAuto">AME</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoneByAME" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtDoneByAME_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
                                                                        onChange="SetEmpIdonChange('txtDoneByAME','txtDoneByAME_Autocomplete')"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtDoneByAME_Autocomplete" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                        CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtDoneByAME"
                                                                        OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnDoneByAMEID" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoneByAMELicenseNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                        Text="<%# mNRC.DoneByAMELicenseNo %>" Enabled="false">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblTechnician" class="clsLabelAuto">Technician</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoneByTech" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtDoneByTech_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
                                                                        onChange="SetEmpIdonChange('txtDoneByTech','txtDoneByTech_Autocomplete')"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtDoneByTech_Autocomplete" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                        CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtDoneByTech"
                                                                        OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnDoneByTechID" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoneByTechLicenseNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                        Text="<%# mNRC.DoneByTechLicenseNo %>" Enabled="false">
                                                                    </asp:TextBox>
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
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDuplicateInsp" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="Legend2" class="clsFieldSet1" runat="server"><b>Duplicate Inspection(If
                                                Required)</b></legend>
                                            <table id="Table7">
                                                <tr>
                                                    <td>
                                                        <%-- <span id="Span1" class="clsLabelStar">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <span id="lblNRCDate" class="clsLabelAuto">Date</span>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtInspectionDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchDate"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtInspectionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInspectionDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtInspectionDate_TextBoxWatermarkExtender" runat="server"
                                                                        TargetControlID="txtInspectionDate" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblInspectedBy" class="clsLabelAuto">AME</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtInspectedByAME" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtInspectedByAME_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
                                                                        onChange="SetEmpIdonChange('txtInspectedByAME','txtInspectedByAME_Autocomplete')"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtInspectedByAME_Autocomplete"
                                                                        runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                        MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList"
                                                                        TargetControlID="txtInspectedByAME" OnClientItemSelected="SetID" UseContextKey="False"
                                                                        ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                        CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                        OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                        OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnInspectedByAMEID" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                                <td>
                                                                    <%--<span id="Span5" class="clsLabelAuto">WO. No.</span>--%>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtInspectedBy" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mNRC.InspectedByAMELicenseNo %>"
                                                                        MaxLength="100" Enabled="false">
                                                                    </asp:TextBox>
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
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table id="Table8">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ValidationGroup="a"
                                                        ToolTip="Click to save NRC" Text="Save"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to print"
                                                        Enabled="<%# not mNRC.IsNew %>" Text="Print" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" ID="upnlhdnButton" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnNRCJob" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnNRCPartOnOff" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;" />
                                        <asp:Button ID="hdnimgBtnNRCSpare" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;" />
                                        <asp:Button ID="hdnBtnPendingMELSnagList" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
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
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <!-- NRCJob Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyNRCJob" Text="NRCJob" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlNRCJob" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeNRCJob" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupNRCJob" runat="server" TargetControlID="btnDummyNRCJob"
        PopupControlID="pnlNRCJob" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IframeNRCJobStateComplete() {
            $("#btnDummyNRCJob").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenNRCWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeNRCJob").attr("src", "wfNRCJob_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyNRCJob").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForNRCJob() {
            var NRCJobwindow = $find("<%=mdlPopupNRCJob.ClientID %>");
            //close NRC Job popup window
            NRCJobwindow.hide();
            //release resources
            $("#IframeNRCJob").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnNRCJob").click();
        }
    </script>
    <!-- End-->
    <!-- Pending MELSnag List Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPendingMELSnagList" Text="PendingMELSnagList"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPendingMELSnagList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframePendingMELSnagList" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupPendingMELSnagList" runat="server" TargetControlID="btnDummyPendingMELSnagList"
        PopupControlID="pnlPendingMELSnagList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IframePendingMELSnagListStateComplete() {
            $("#btnDummyPendingMELSnagList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenPendingMELSnagListWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframePendingMELSnagList").attr("src", "wfPendingMELSnagListForNRCJobs_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyPendingMELSnagList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForPendingMELSnagList() {
            var PendingMELSnagListwindow = $find("<%=mdlPopupPendingMELSnagList.ClientID %>");
            //close Pending MELSnag List popup window
            PendingMELSnagListwindow.hide();
            //release resources
            $("#IframePendingMELSnagList").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnPendingMELSnagList").click();
        }
    </script>
    <!-- End-->
    <!-- NRC Part On Off Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyNRCPartOnOff" Text="NRC Part On Off" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlNRCPartOnOff" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeNRCPartOnOff" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupNRCPartOnOff" runat="server" TargetControlID="btnDummyNRCPartOnOff"
        PopupControlID="pnlNRCPartOnOff" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameNRCPartOnOffStateComplete() {
            $("#btnDummyNRCPartOnOff").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenNRCPartOnOffWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeNRCPartOnOff").attr("src", "wfNRCPartOnOff_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyNRCPartOnOff").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForNRCPartOnOff() {
            var NRCPartOnOffwindow = $find("<%=mdlPopupNRCPartOnOff.ClientID %>");
            //close NRC Part On Off popup window
            NRCPartOnOffwindow.hide();
            //release resources
            $("#IframeNRCPartOnOff").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnNRCPartOnOff").click();
        }
    </script>
    <!-- End-->
    <!-- NRC Spare Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyNRCSpare" Text="NRC Spare" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlNRCSpare" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeNRCSpare" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupNRCSpare" runat="server" TargetControlID="btnDummyNRCSpare"
        PopupControlID="pnlNRCSpare" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameNRCSpareStateComplete() {
            $("#btnDummyNRCSpare").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenNRCSpareWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeNRCSpare").attr("src", "wfNRCSpare_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyNRCSpare").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForNRCSpare() {
            var NRCSparewindow = $find("<%=mdlPopupNRCSpare.ClientID %>");
            //close NRC Spare popup window
            NRCSparewindow.hide();
            //release resources
            $("#IframeNRCSpare").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnNRCSpare").click();
        }
    </script>
    <!-- End-->
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
    <%--
    Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
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
            if (source._id == "txtRaisedBy_Autocomplete") {
                textbox = document.getElementById('hdnRaisedByEmpID');
            }
            if (source._id == "txtDoneByAME_Autocomplete") {
                textbox = document.getElementById('hdnDoneByAMEID');
            }
            if (source._id == "txtDoneByTech_Autocomplete") {
                textbox = document.getElementById('hdnDoneByTechID');
            }
            if (source._id == "txtInspectedByAME_Autocomplete") {
                textbox = document.getElementById('hdnInspectedByAMEID');
            }
            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetEmpIdonChange(cntrl, extender) {
            var cntrlName = '#' + cntrl;
            var popup = $find(extender);
            var complist = popup.get_completionList();
            var text = $(cntrlName).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    if (cntrl == "txtRaisedBy") {
                        var textbox = document.getElementById('hdnRaisedByEmpID');
                    }
                    if (cntrl == "txtDoneByAME") {
                        textbox = document.getElementById('hdnDoneByAMEID');
                    }
                    if (cntrl == "txtDoneByTech") {
                        textbox = document.getElementById('hdnDoneByTechID');
                    }
                    if (cntrl == "txtInspectedByAME") {
                        textbox = document.getElementById('hdnInspectedByAMEID');
                    }
                    textbox.value = val.toString();
                    return;
                }
            }
            if (cntrl == "txtRaisedBy") {
                var textbox = document.getElementById('hdnRaisedByEmpID');
            }
            if (cntrl == "txtDoneByAME") {
                textbox = document.getElementById('hdnDoneByAMEID');
            }
            if (cntrl == "txtDoneByTech") {
                textbox = document.getElementById('hdnDoneByTechID');
            }
            if (cntrl == "txtInspectedByAME") {
                textbox = document.getElementById('hdnInspectedByAMEID');
            }
            textbox.value = '';
            return;
        }
    </script>
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
    </form>
</body>
</html>
