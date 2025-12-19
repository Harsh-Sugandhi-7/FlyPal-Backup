<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCustomerContractTasks_Ajax.aspx.vb"
    Inherits="Flypal.wfCustomerContractTasks_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="FlyPal" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link id="MainStyle" rel="stylesheet" type="text/css" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server"></uc2:MSGBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlTasks" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Customer Contract Tasks [New]</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to save Item"
                                                                                Enabled="<%# mCustomerContract.StatusID=1 %>" Text="Add"></asp:Button>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous screen"
                                                                                Text="Back" CausesValidation="false"></asp:Button>
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
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary" ValidationGroup="a"  >
                                                    </asp:ValidationSummary>
                                                    <asp:CustomValidator ID="cvValid" runat="server"  ErrorMessage="" ValidationGroup="a"  ControlToValidate="txtTaskDescription" Display="None" ></asp:CustomValidator>
                                                    <%--<asp:RequiredFieldValidator runat="server" CssClass="clsLabelAuto" ValidationGroup="a"  ErrorMessage="" ControlToValidate="txtTaskDescription" ></asp:RequiredFieldValidator>--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend class="clsLabelHeader"><b>Contract Task Details</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblLocationtar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLocation" runat="server" CssClass="clsLabel">Location</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbLocation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                Enabled="<%# mCustomerContract.StatusID=1 %>" DataTextField="Name">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCapabilityTask1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCapabilityTask" runat="server" CssClass="clsLabel">Capability Task</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCapabilityTask" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                AutoPostBack="true" Enabled="<%# mCustomerContract.StatusID=1 %>" DataValueField="ID"
                                                                DataTextField="TaskDescriptionLen15">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTaskDesc1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTaskDesc" runat="server" CssClass="clsLabel">Task Description</asp:Label>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtTaskDescription" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Enabled="<%# mCustomerContract.StatusID=1 %>" Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.CapabilityTaskDescription %>"
                                                              Width="470px"  Height="50px" TextMode="MultiLine" ToolTip="Enter Task Description"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabel">Turn Around Time</asp:Label>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtTATHours" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                Visible="<%# not mCustomerContract.CustomerContractTasks.CurrentItem.TATDays %>"
                                                                Enabled="<%# mCustomerContract.StatusID=1 %>" MaxLength="4" ClientIDMode="Static"
                                                                Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.TATHours %>" ToolTip="Enter Turn Around Time"></asp:TextBox>
                                                            <asp:TextBox ID="txtTATDays" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                Visible="<%#  mCustomerContract.CustomerContractTasks.CurrentItem.IsDays %>"
                                                                Enabled="<%# mCustomerContract.StatusID=1 %>" MaxLength="4" ClientIDMode="Static"
                                                                Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.TATDays %>" ToolTip="Enter Turn Around Time" ></asp:TextBox>
                                                            <asp:CheckBox ID="chkIsInDays" runat="server" class="clsLabelHeader" AutoPostBack="true"
                                                                Enabled="<%# mCustomerContract.StatusID=1 %>" Checked="<%# mCustomerContract.CustomerContractTasks.CurrentItem.IsDays %>"
                                                                Text="In Days (If not checked then TAT will be in Hours)" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend><b>
                                                    <asp:CheckBox ID="chkIsFixedRate" runat="server" class="clsLabelHeader" AutoPostBack="true"
                                                        Checked="<%# mCustomerContract.CustomerContractTasks.CurrentItem.IsFixedRate %>"
                                                        Enabled="<%# mCustomerContract.StatusID=1 %>" Text="Fixed Rate (if checked then enter Rate directly)" />
                                                </legend>
                                                <asp:Panel ID="pnlfixedRate" runat="server" Enabled="<%# mCustomerContract.StatusID=1 %>">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRate" runat="server" CssClass="clsLabel">Rate</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCRate" runat="server"  CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="12"
                                                                    Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.CFixedRate %>"
                                                                    ToolTip="Enter Fixed Rate" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRateCurrency" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                    ReadOnly="True" BackColor="#E0E0E0" Text="<%# mCustomerContract.CurrencyName %>">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                </b>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlSkill" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                        <legend class="clsLabelHeader"><b>If not Fixed Rate then enter following detail(s)
                                                        </legend>
                                                        <asp:Panel ID="pnlNotFixedRate" runat="server" Enabled="<%# mCustomerContract.StatusID=1 %>">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                            <legend class="clsLabelHeader"><b>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span class="clsLabelHeader">Skill Man Hours Rate(s)</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="btnAddSkill" runat="server" CausesValidation="true" Height="22px" Enabled="<%# mCustomerContract.StatusID=1 %>"
                                                                                                ImageUrl="~/images/plus1.png" ToolTip="Click to Add New Skill" ValidationGroup="1"
                                                                                                Width="24px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table></legend>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:GridView ID="dgSkillList" runat="server" AutoGenerateColumns="False" 
                                                                                           CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" Enabled="<%# mCustomerContract.StatusID=1 %>"
                                                                                            DataKeyNames="ID" PageSize="25" ShowHeaderWhenEmpty="True">
                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                            <RowStyle CssClass="clsdgItem" />
                                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HtmlEncode="false">
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                    <ItemStyle Wrap="False" />
                                                                                                </asp:BoundField>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:Label ID="lblHeaderStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                                        <asp:Label ID="Label2" runat="server" >Skill</asp:Label>
                                                                                                        <%--<span id="Span6" class="clsdgHeader">Skill</span>--%>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:UpdatePanel ID="upnlSkillValidate" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:RequiredFieldValidator ID="rfvHeader" runat="server" ControlToValidate="txtContractSkill"
                                                                                                                    CssClass="clsLabel" Display="dynamic" ErrorMessage="Header Required" Font-Italic="true"
                                                                                                                    ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Skill Required"
                                                                                                                    ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                        <asp:Label ID="lblDuplicateSkill" runat="server" ForeColor="Red" class="clsLabel"
                                                                                                            Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                                        <asp:TextBox ID="txtContractSkill" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" onblur="CheckDuplicateSkill();" onchange="CheckDuplicateSkill();"
                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "SkillName") %>' AutoPostBack="true" Width="150px"
                                                                                                            OnTextChanged="txtContractSkill_TextChanged"></asp:TextBox>
                                                                                                        <cc2:AutoCompleteExtender ID="txtContractSkill_AutoCompleteExtender" runat="server"
                                                                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                                            CompletionInterval="1" ServicePath="" ServiceMethod="GetSkillList" TargetControlID="txtContractSkill"
                                                                                                            UseContextKey="false" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                                            CompletionListHighlightedItemCssClass="ac_over_Main" CompletionListItemCssClass="ac_results_li"
                                                                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                                        </cc2:AutoCompleteExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Rate">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtSkillRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="150px"
                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "CRate") %>' onkeypress="return validateDecimalNo(this,event)"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            </b>
                                                                        </fieldset>
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" align="top">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblSpareMarkUpPercent" runat="server" CssClass="clsLabel">Spares MarkUp %</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtSpareMarkUpPercent" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        Enabled="<%# mCustomerContract.StatusID=1 %>" MaxLength="12" Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.SparesMarkupPercent %>"
                                                                                        ToolTip="Enter Spares MarkUp %" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                            <legend class="clsLabelHeader"><b>Hangars Rate </legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rdbHangarHour" Text="Hour(s) Rate" runat="server" GroupName="a"
                                                                                            AutoPostBack="true" Enabled="<%# mCustomerContract.StatusID=1 %>" Checked="<%# not mCustomerContract.CustomerContractTasks.CurrentItem.IsHangarInDays  %>" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtHangarHour" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                            MaxLength="12" Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.CHangarUsageperHourRate %>"
                                                                                            Enabled="<%# mCustomerContract.StatusID=1 %>" ToolTip="Enter Hangar Rate" Width="150px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rdbHangarDay" Text="Day(s) Rate" runat="server" GroupName="a"
                                                                                            AutoPostBack="true" Enabled="<%# mCustomerContract.StatusID=1 %>" Checked="<%# mCustomerContract.CustomerContractTasks.CurrentItem.IsHangarInDays  %>" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtHangarDay" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                            Enabled="<%# mCustomerContract.StatusID=1 %>" MaxLength="12" Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.CHangarUsageperDaytRate %>"
                                                                                            ToolTip="Enter Hangar Rate" Width="150px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            </b>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                            <legend class="clsLabelHeader"><b>Parking Space Rate </legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rdbParkingSpaceHour" Text="Hour(s) Rate" runat="server" GroupName="b"
                                                                                            AutoPostBack="true" Enabled="<%# mCustomerContract.StatusID=1 %>" Checked="<%# not mCustomerContract.CustomerContractTasks.CurrentItem.IsParkingInDays  %>" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtParkingSpaceHour" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                            Enabled="<%# mCustomerContract.StatusID=1 %>" MaxLength="12" Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.CParkingSpaceperHourRate %>"
                                                                                            ToolTip="Enter Parking Rate" Width="150px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rdbParkingSpaceDay" Text="Day(s) Rate" runat="server" GroupName="b"
                                                                                            AutoPostBack="true" Enabled="<%# mCustomerContract.StatusID=1 %>" Checked="<%# mCustomerContract.CustomerContractTasks.CurrentItem.IsParkingInDays  %>" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtParkingSpaceDay" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                            Enabled="<%# mCustomerContract.StatusID=1 %>" MaxLength="12" Text="<%# mCustomerContract.CustomerContractTasks.CurrentItem.CParkingSpaceperDayRate %>"
                                                                                            ToolTip="Enter Parking Rate" Width="150px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            </b>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        </b>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                       <td align="right">
                                            <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsButton" ToolTip="Click to save Item"
                                                                    Enabled="<%# mCustomerContract.StatusID=1 %>" Text="Add"></asp:Button>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous screen"
                                                                    Text="Back" CausesValidation="false"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlhidden" UpdateMode ="Conditional"  runat ="server"  >
                                                <ContentTemplate >
                                                    <asp:HiddenField runat ="server" ID="hiddenSkillError" />
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
    </div>
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
    <script type="text/javascript">
        function CheckDuplicateSkill(sender, args) {
            var grid = document.getElementById("<%=dgSkillList.ClientID %>");
            //            var inputs = $('#<%=dgSkillList.ClientID %>').find('textarea[id$="txtContractSkill"]');
            var inputs = $('#<%=dgSkillList.ClientID %>').find('[id$="txtContractSkill"]');
            var span = $('#<%=dgSkillList.ClientID %>').find('[id$="lblDuplicateSkill"]');
            hiddenSkillError.Value = '';
            for (var i = 0; i < inputs.length; i++) {
                inputs[i].style.backgroundColor = "";
                span[i].style.display = 'none';
                hiddenSkillError.value = '';
            }
            for (var i = 0; i < inputs.length; i++) {
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[i] != inputs[j] && (inputs[i].value != "" || inputs[j].value != "") && inputs[i].value == inputs[j].value) {
                        inputs[i].style.backgroundColor = "Orchid";
                        inputs[j].style.backgroundColor = "Orchid";
                        span[i].style.display = 'block';
                        span[j].style.display = 'block';
                        hiddenSkillError.value = 'error';
                    }
                }
            }
        }
    </script>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameCustomerContractTasksStateComplete();
         }
    });

    <% End if %>
       Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            SetPageLayout();
        }

       function SetPageLayout()
       {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
          ReSetPageLayout();
          onResize();//for Top bottom link
           <% End if %>
       }
       function ReSetPageLayout()
       {
       $("body,html").css({ 'background-color': 'transparent' });
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
         function CallParentCallback() {
            parent.ParentCallBackFunction();
            return false;
        }
    </script>
    <%--End--%>
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
    </form>
</body>
</html>
