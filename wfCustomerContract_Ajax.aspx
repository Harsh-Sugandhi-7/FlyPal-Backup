<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCustomerContract_Ajax.aspx.vb"
    Inherits="Flypal._wfCustomerContract_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="FlyPal" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contract Detail(s)</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openFile() {
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
    <script type="text/javascript">
        window.onload = blinknow;
        function blinknow() {
            var e = document.getElementById("<%=lblStatus.ClientID%>");

            e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
            setTimeout("blinknow();", 750);
        }
        
    </script>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Contract Detail</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnCancel" runat="server" ClientIDMode="Static" CssClass="clsbtnH clsinfoH"
                                                                    Visible="<%# mCustomerContract.StatusID = 2  and not mCustomerContract.IsNew %>"
                                                                    Text="Cancel" ToolTip="Click to Cancel the Contract" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH" Text="Authorize"
                                                                    Visible="<%# mCustomerContract.StatusID = 1  and not mCustomerContract.IsNew %>"
                                                                    ToolTip="Click to authorize Contract" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Contract" Enabled="<%# mCustomerContract.StatusID=1 %>" ValidationGroup="a"
                                                                    Visible="<%# mCustomerContract.StatusID = 1 %>"
                                                                    OnClientClick="return ValidateSave()" CausesValidation="true" />
                                                                <script type="text/javascript">
                                                                    function ValidateSave() {
                                                                        var isValid = false;
                                                                        isValid = Page_ClientValidate('a');
                                                                        if (isValid) {
                                                                            isValid = Page_ClientValidate('cert');
                                                                        }

                                                                        return isValid;
                                                                    }
                                                                </script>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                    Text="Close" ToolTip="Click to go back to the previous page" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                <asp:Button ID="hdnBtnCustomerContractTasks" ClientIDMode="Static" runat="server"
                                                                    Text="----" CausesValidation="false" Style="display: none;"></asp:Button>
                                                                <asp:Button ID="hdnimgBtnCustomerTerm" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="false" Style="display: none;"></asp:Button>
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
                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="a" Width="100%" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvConDate" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"  ValidationGroup="a"
                                            Display="None" ControlToValidate="txtContractDate" ErrorMessage="Select W.O. Date"></asp:CustomValidator>
                                              <asp:CustomValidator ID="cvtxtFromDate" runat="server" CssClass="clsLabelAuto" ValidationGroup="a" OnServerValidate="CustomValidate"
                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage=""></asp:CustomValidator>
                                             <asp:CustomValidator ID="cvcmbCurrencyList" runat="server" CssClass="clsLabelAuto" ValidationGroup="a" OnServerValidate="CustomValidate"
                                            Display="None" ControlToValidate="cmbCurrencyList" ErrorMessage=""></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlContractDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="right" colspan="7">
                                                                    <asp:UpdatePanel ID="upnlStatusHeader" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader" Font-Italic="true"
                                                                                Font-Size="10pt" Text="<%# mCustomerContract.StatusName %>"></asp:Label>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Label4" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblContractDate" class="clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtContractDate" runat="server" AutoPostBack="false" ClientIDMode="Static"
                                                                        CssClass="clsTextBoxTagSearchDate"  Enabled="<%# mCustomerContract.IsNew %>" onchange="ValidateDateText(this,'txtContractDate_CalendarExtender');"
                                                                        Width="85px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtContractDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtContractDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtContractDate_Watermarkextender" runat="server"
                                                                        TargetControlID="txtContractDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Text</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtText" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        CssClass="clsTextBoxTagSearch" Enabled="<%# mCustomerContract.IsNew %>" onblur="WaterMark(this, event);"
                                                                        onfocus="WaterMark(this, event);" Text="<%# mCustomerContract.Text %>" ToolTip="Enter Text"
                                                                        Width="140px"></asp:TextBox>
                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Enabled="<%# mCustomerContract.IsNew %>"
                                                                        MaxLength="7" Text="<%# mCustomerContract.No %>" ToolTip="Enter No." Width="40px"></asp:TextBox>
                                                                    <%-- <cc2:AutoCompleteExtender ID="txtText_Autocomplete" runat="server" ClientIDMode="Static"
                                                                        CompletionInterval="0" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionSetCount="0" ContextKey=""
                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" OnClientHiding="ClientHiding"
                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientShowing="ClientShowing"
                                                                        OnClientShown="ClientHiding" ServiceMethod="GetTextList" ServicePath="wfCustomerContract_Ajax.aspx"
                                                                        TargetControlID="txtText" UseContextKey="False">
                                                                    </cc2:AutoCompleteExtender>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span4" class="clsLabelAuto">Customer</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCustomer" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"  Enabled="<%# mCustomerContract.StatusID=1 %>"
                                                                        DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCurrencyStar" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCurrency" class="clsLabel">Currency/Factor</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        AutoPostBack="true"   Enabled="<%# mCustomerContract.CustomerContractTasks.Count=0 %>"
                                                                        DataTextField="Name" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtConversionFactor" runat="server" Text="<%# mCustomerContract.ConversionFactor %>"  Enabled="false" 
                                                                        CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Conversion Factor" MaxLength="9"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span9" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span10" class="clsLabel">Model</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ModelName"
                                                                        Enabled="<%# mCustomerContract.StatusID=1 %>" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span8" class="clsLabelAuto">Aircraft</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAircraftList" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataTextField="RegNo" DataValueField="ID" Enabled="<%# mCustomerContract.StatusID=1 %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span6" class="clsLabelAuto">From Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                                        AutoPostBack="false" onchange="ValidateDateText(this,'txtFromDate_CalendarExtender','true');"
                                                                        Enabled="<%# mCustomerContract.StatusID=1 %>" Width="85px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtFromDate_Watermarkextender" runat="server" TargetControlID="txtFromDate"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span7" class="clsLabelAuto">To Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                                        AutoPostBack="false" onchange="ValidateDateText(this,'txtToDate_CalendarExtender','true');"
                                                                        Enabled="<%# mCustomerContract.StatusID=1 %>" Width="85px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtToDate_Watermarkextender" runat="server" TargetControlID="txtToDate"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td class="clsInnerTable">
                                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                </td>
                                                                <td colspan="5">
                                                                    <table id="Table12" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                                        runat="server" class="clsbtnH clsinfoH1" />
                                                                                                </td>
                                                                                                <td style="padding-left: 3px;">
                                                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment" 
                                                                                                        Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
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
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlContractTasks" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblContractItems" class="clsLabelHeader">Contract Task(s)</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" TabIndex="0" runat="server" Text="Add" CssClass="clsbtnH clsinfoH1" Enabled="<%# mCustomerContract.StatusID=1 %>"
                                                        ToolTip="Click to add Contract Task(s)"></asp:Button> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgContractItems" runat="server" ShowHeaderWhenEmpty="True"
                                                       CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False">
                                                        <PagerSettings Mode="NextPreviousFirstLast" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                            <asp:BoundField DataField="LocationName" HeaderText="Location" />
                                                            <asp:BoundField DataField="CapabilityTaskDescription" HeaderText="Task Description" />
                                                            <asp:TemplateField HeaderText="Fixed Rate">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsFixedRate" runat="server" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem,"IsFixedRate") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="80px"
                                                                        Enabled="false" MaxLength="12" Text='<%# DataBinder.Eval(Container.DataItem,"CFixedRate") %>'> </asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                        CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                        CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div id="divd" class="dropdownbtn-content" runat="server">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>' CausesValidation="false"
                                                                                            CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"  
                                                                                          Visible='<%#IIf(mCustomerContract.StatusID = 1, True, False) %>'  />
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
                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="upnlContractTerms" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblContractTerms" class="clsLabelHeader">Contract Term(s)</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnAddTerm" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add" ToolTip="Click To Add Term" Enabled="<%# mCustomerContract.StatusID=1 %>" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgContractTerms" runat="server" AutoGenerateColumns="False"
                                                       CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True" Width="100%">
                                                        <PagerSettings Mode="NextPreviousFirstLast" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                <HeaderStyle HorizontalAlign="left" />
                                                                <ItemStyle HorizontalAlign="left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
                                                                <HeaderStyle HorizontalAlign="left" />
                                                                <ItemStyle CssClass="TextBreak" Width="500px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="RemoveTerm" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' Enabled="<%# mCustomerContract.StatusID=1 %>"
                                                                        CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                        CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCancel" runat="server" ClientIDMode="Static" CssClass="clsButton_Ajax"
                                                        Visible="<%# mCustomerContract.StatusID = 2  and not mCustomerContract.IsNew %>"
                                                        Text="Cancel" ToolTip="Click to Cancel the Contract" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAuthorized" runat="server" CssClass="clsButton_Ajax" Text="Authorize"
                                                        Visible="<%# mCustomerContract.StatusID = 1  and not mCustomerContract.IsNew %>"
                                                        ToolTip="Click to authorize Contract" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Transaction" Enabled="<%# mCustomerContract.StatusID=1 %>"  ValidationGroup="a"
                                                        Visible="<%# mCustomerContract.StatusID = 1 %>"
                                                        OnClientClick="return ValidateSave()" CausesValidation="true" />
                                                    <script type="text/javascript">
                                                        function ValidateSave() {
                                                            var isValid = false;
                                                            isValid = Page_ClientValidate('a');
                                                            if (isValid) {
                                                                isValid = Page_ClientValidate('cert');
                                                            }

                                                            return isValid;
                                                        }
                                                    </script>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                        Text="Close" ToolTip="Click to go back to the previous page" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                    <asp:Button ID="hdnBtnCustomerContractTasks" ClientIDMode="Static" runat="server"
                                                        Text="----" CausesValidation="false" Style="display: none;"></asp:Button>
                                                    <asp:Button ID="hdnimgBtnCustomerTerm" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="false" Style="display: none;"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
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

        function OpenFileUploadWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                //                if (!$.browser.msie) {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = "hidden";
                //                }
                return false;
            } catch (e) {
                alert(e);
            }

        }

       
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
    <!-- CustomerContractTasks-->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCustomerContractTasks" Text="Dummy CustomerContractTasks"
            ClientIDMode="Static" CausesValidation="false" />
    </div>
    <asp:Panel runat="server" ID="pnlCustomerContractTasks" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="ICustomerContractTasks" allowtransparency="true" frameborder="0" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCustomerContractTasks" runat="server" TargetControlID="btnDummyCustomerContractTasks"
        PopupControlID="pnlCustomerContractTasks" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCustomerContractTasksStateComplete() {
            $("#btnDummyCustomerContractTasks").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenCustomerContractTasksWindow() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#ICustomerContractTasks").attr("src", "wfCustomerContractTasks_Ajax.aspx?Type=pup");
                // if (!$.browser.msie) {
                $("#btnDummyCustomerContractTasks").click();
                $get("AjaxLoader").style.visibility = 'hidden';
                //  }
                return false;
            } catch (e) {
                alert(e);
            }
        }
       
    </script>
    <script type="text/javascript">
        function ParentCallBackFunction() {
            var CustomerContractTaskswindow = $find("<%=mdlPopupCustomerContractTasks.ClientID %>");
            //close Ass Insp Maint Done By Emp popup window
            CustomerContractTaskswindow.hide();
            //Free resources
            $("#ICustomerContractTasks").attr("src", "JavaScript:''");
            $("#hdnBtnCustomerContractTasks").click();

        }
    </script>
    <!-- End -->
    <!-- Customer Term List Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCustomerTerm" Text="Dummy Customer Term" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupCustomerTerm" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupCustomerTerm" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCustomerTerm" runat="server" TargetControlID="btnDummyCustomerTerm"
        PopupControlID="pnlPopupCustomerTerm" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCustomerTermStateComplete() {
            $("#btnDummyCustomerTerm").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#btnAddTerm").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupCustomerTerm").attr("src", "wfCustomerTermList_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyCustomerTerm").click();
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
        function ParentCallBackFunctionForCustomerTerm() {
            var CustomerTermWindow = $find("<%=mdlPopupCustomerTerm.ClientID %>");
            //close Customer Term popup window
            CustomerTermWindow.hide();
            $("#iPopupCustomerTerm").attr("src", "JavaScript:''");
            //call Customer Term button
            $("#hdnimgBtnCustomerTerm").click();
        }
    </script>
    <!-- End-->
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
