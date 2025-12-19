<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptComponentHistoryCard_AJAX.aspx.vb"
    Inherits="Flypal.wfrptComponentHistoryCard_AJAX" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Component History Card</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblList" runat="server" CssClass="clsFormHeader">Component History Card</asp:Label>
                                            </td>
                                            <%--<td align="right" colspan="3">
                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrint" runat="server" ToolTip="Click to display the list of Component History Card"
                                                                        Text="Display" ValidationGroup="1"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" ToolTip="Click to Close Component History Card Report screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                        </tr>
                                    </table>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Information" ValidationGroup="1"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="AS On Date  Required." Display="None" ControlToValidate="txtAsOnDate"
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvPart" runat="server" ErrorMessage="Please Select Part"
                                                Display="None" ControlToValidate="txtPartDescription" CssClass="clsLabelAuto"
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvComponent" runat="server" ErrorMessage="Please select the Component"
                                                Display="None" ControlToValidate="cmbComponent" OnServerValidate="CustomValidate"
                                                CssClass="clsLabelAuto" ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvPart" runat="server" ErrorMessage="Please select the Part"
                                                Display="None" ControlToValidate="txtPartDescription" OnServerValidate="CustomValidate"
                                                CssClass="clsLabelAuto" ValidationGroup="1"></asp:CustomValidator></TD>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td>
                                    <span id="lblAsOnDate" runat="server" class="clsLabelAuto">As On Date</span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtAsOnDate"  ClientIDMode="Static"
                                        runat="server" onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="calAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 7px">
                                    <asp:Label ID="Label5" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td style="height: 7px">
                                    <span id="lblPart" runat="server" class="clsLabelAuto">Part No.</span>
                                </td>
                                <td style="height: 7px">
                                    <asp:UpdatePanel ID="upnlPart" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <%--  <asp:DropDownList ID="cmbPart" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                DataTextField="Name" DataValueField="ID">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox cssclass="clsTextBoxSearch_Ajax" ID="txtPartDescription" autocomplete="off" runat="server"
                                                AutoPostBack="True" onchange="SetPartIdonChange(this,'txtPartDescription_AutoCompleteExtender')"></asp:TextBox>
                                            <!-- AutoComplete Extender-->
                                            <cc2:AutoCompleteExtender ID="txtPartDescription_AutoCompleteExtender" runat="server"
                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1"
                                                CompletionInterval="1" ServicePath="" ServiceMethod="GetPartNoDescriptionList"
                                                TargetControlID="txtPartDescription" UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                OnClientItemSelected="SetID">
                                            </cc2:AutoCompleteExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td style="height: 21px">
                                    <span id="Label1" runat="server" class="clsLabelAuto">Serial No.</span>
                                </td>
                                <td style="height: 21px">
                                    <asp:UpdatePanel ID="upnlComponent" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbComponent" runat="server" DataTextField="SerialNo"
                                                DataValueField="ID">
                                            </asp:DropDownList>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbtempComponent" runat="server" 
                                                DataTextField="SerialNo" Visible="false" DataValueField="ID">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdoAirframe" class="clsLabelAuto" runat="server" TextAlign="Right"
                                                    Text="Value as of Airframe" Checked="true" GroupName="x" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdoAssembly" class="clsLabelAuto" runat="server" TextAlign="Right"
                                                    Text="Value as of Assembly" GroupName="x" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdoComponent" class="clsLabelAuto" runat="server" TextAlign="Right"
                                                    Text="Value as of Component" GroupName="x" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:LinkButton ID="lnkViewID" runat="server" CssClass="clsLinkButton" Visible='<%# iif(AppSettings("ClientCode") = "STR",False,True) %>'>View Here </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:Panel runat="server" ID="pnlTBOSLL">
                                        <table class="clsGridLog">
                                            <tr>
                                                <td class="clsdgHeader" style="width: 35px">
                                                    <span class="clsdgHeader">TBO : </span>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp; <span id="lblTBOFreq" class="clsLabel" style="width: 60px" runat="server">
                                                    </span>
                                                </td>
                                                <td class="clsdgHeader" style="width: 35px">
                                                    <span class="clsdgHeader">SLL : </span>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp; <span id="lblSLLFreq" runat="server" class="clsLabel" style="width: 60px">
                                                        &nbsp;</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlCompHistory" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgCompHistory" runat="server" AutoGenerateColumns="False" Visible="true"
                                                GridLines="Horizontal" CellPadding="3" CssClass="clsGridNewStyle" PageSize="3" ShowHeaderWhenEmpty="true">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="InstalledInfo" HeaderText="Inst. Date/ Log Page No./ WO NO."
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InstalledOnInfo" HeaderText="Inst. On A/C No." HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Position" HeaderText="Pos." HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="InstDoneBy" HeaderText="By Activity" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="AirframeCurrentValueStr" HeaderText="Inst. At A/C Values"
                                                        HeaderStyle-HorizontalAlign="Center" HtmlEncode="false" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyInstallationValueStr" HeaderText="Inst. At Assembly Values"
                                                        HeaderStyle-HorizontalAlign="Center" HtmlEncode="false" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CompInstallationValueStr" HeaderText="InstStatus" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left" HtmlEncode="false">
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InstallationStatusID" HeaderText="InstallationStatusID"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HtmlEncode="false"
                                                        HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                    <asp:BoundField DataField="SinceOH" HeaderText="Since OH" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left" HtmlEncode="false">
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InstReason" HtmlEncode="false" HeaderText="Inst. Remark"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:TemplateField Visible="false" HeaderText="View" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="EditInstRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                CommandName="EditInstRec" Style="height: 25px; width: 25px" ImageUrl="~/images/View.jpg" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DueAtOH" HtmlEncode="false" HeaderText="Due At for OH"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DueAtSLL" HtmlEncode="false" HeaderText="Due At for SLL"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DueAtOHAirframe" HtmlEncode="false" HeaderText="Due At A/C for OH"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DueAtOHAssembly" HtmlEncode="false" HeaderText="Due At Assembly for OH"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SLLDueAtAirframe" HtmlEncode="false" HeaderText="Due At A/C for SLL"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SLLDueAtAssembly" HtmlEncode="false" HeaderText="Due At Assembly for SLL"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemovedInfo" HeaderText="Rem. Date/ Log Page No./ WO NO."
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemDoneBy" HeaderText="By Activity" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyRemovalValueByAirframeStr" HeaderText="Rem. At A/C Values"
                                                        HeaderStyle-HorizontalAlign="Center" HtmlEncode="false" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyRemovalValueStr" HeaderText="Rem. At Assembly Values"
                                                        HeaderStyle-HorizontalAlign="Center" HtmlEncode="false" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CompRemovalValueStr" HeaderText="InstStatus" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemovalSince" HeaderText="Since OH" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left" HtmlEncode="false">
                                                        <ItemStyle Wrap="true"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemovalRemark" HtmlEncode="false" HeaderText="Removal Remark"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:TemplateField Visible="false" HeaderText="View" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="EditRemRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                CommandName="EditRemRec" Style="height: 25px; width: 25px" ImageUrl="~/images/View.jpg" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlComplianceHistory" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgComplianceHistory" runat="server" AutoGenerateColumns="False"
                                               GridLines="Horizontal" CellPadding="3" CssClass="clsGridNewStyle" Visible="true" PageSize="3" ShowHeaderWhenEmpty="true">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="Event" HeaderText="Maintenance Activity" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Left" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FrequencyValue" HeaderText="Component Interval" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ComplainceDate" HeaderText="Component Done On Date" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DoneOnValue" HeaderText="Component Total Time/Rin" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DoneWONO" HeaderText="WO NO." HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DescriptionWithTaskNo" HeaderText="HISTORY OF INSPECTION,OVERHAUL,REPAIR AND APPLICATION OF TECHNICAL BULLETIN,SERVICE BULLETIN,AIRWORTHINESS DIRECTIVES, ETC"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DueOnValue" HeaderText="Component Due Values" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AirframeDueAsofValue" HeaderText="Due As Of Airframe"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyDueAsofValue" HeaderText="Due As Of Assembly"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                                        <HeaderStyle Wrap="true"></HeaderStyle>
                                                        <ItemStyle Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField Visible="false" HeaderText="View" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="EditRemRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                CommandName="EditRemRec" Style="height: 25px; width: 25px" ImageUrl="~/images/View.jpg" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnPrint" runat="server" ToolTip="Click to display the list of Component History Card"
                                                            Text="Display" ValidationGroup="1"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server"  ToolTip="Click to Close Component History Card Report screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
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
    </div>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartID" />
    <%-- Autocomplete functions to set id--%>
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
            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtPartDescription_AutoCompleteExtender") {
                textbox = document.getElementById('PartID');
            }
            textbox.value = value;
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetPartIdonChange(source, extenderid) {
            var popup = $find(extenderid);
            var complist = popup.get_completionList();
            var text = $(source).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;

                    if (extenderid == "txtPartDescription_AutoCompleteExtender") {
                        textbox = document.getElementById('PartID');
                    }
                    textbox.value = val;
                    return;
                }

            }

            //            if (extenderid == "txtPartDescription_AutoCompleteExtender") {
            //                document.getElementById('PartID').value = '';
            //            }
        }
        
    </script>
    <%--Date Validations--%>
    <script type="text/javascript">

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
    </form>
</body>
</html>
