<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMultiComplianceCartListPartII_Ajax.aspx.vb"
    Inherits="Flypal.wfMultiComplianceCartListPartII_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>MultiCompliance Cart</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script type="text/javascript" language="javascript" id="clientEventHandlersJS">
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
    </script>
    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/close.gif";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/detail.gif";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Multi Compliance Cart</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                        <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Compliance Details</b></legend>
                                        <table id="Table1" cellspacing="0" width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:UpdatePanel ID="upnlDet" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table id="Table2" cellspacing="0">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblComplianceDate" runat="server" CssClass="clsLabelAuto" Width="100px">Compliance Date</asp:Label>
                                                                                </td>
                                                                                <td valign="top" align="left">
                                                                                    <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'txtAsOnDate_watermarkextender');"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtAsOnDate">
                                                                                    </cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="txtAsOnDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$ AppSettings:DateFormat %>"
                                                                                        WatermarkCssClass="clsDateTextBox" Enabled="True">
                                                                                    </cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <asp:TextBox ID="txtAircraft" runat="server" CssClass="clsTextBox" BackColor="#E0E0E0"
                                                                                        ReadOnly="True"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAssembly" runat="server" CssClass="clsTextBox" BackColor="#E0E0E0"
                                                                                        ReadOnly="True"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td align="right">
                                                                <asp:UpdatePanel ID="upnlCurrentValues" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelHeader" Height="17px">Compliance On Values</asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                                        ShowHeaderWhenEmpty="true" EnableViewState="false" AllowSorting="True" AllowPaging="True"
                                                                                        AutoGenerateColumns="False" PageSize="5">
                                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="PeriodName" HeaderText="Period"></asp:BoundField>
                                                                                            <asp:BoundField DataField="AssemblyCurrentValueFormatted" HeaderText="Values"></asp:BoundField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
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
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButtonsTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSaveTop" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click To Comply"
                                                            Text="Comply" Visible="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddMoreTop" runat="server" CssClass="clsButton" ToolTip="Click to add more"
                                                            Text="Add More" Visible="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
                                                            Text="Back" Visible="False" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgMultiComplianceList" runat="server" AutoGenerateColumns="False"
                                                CssClass="clsGrid" AllowSorting="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <%-- <div class="clstooltip" style="display: none;">
                                                                <b>Monitor Info:</b>&nbsp;
                                                                <%# Eval("TypeDet")%>
                                                            </div>--%>
                                                            <div>
                                                                <itemtemplate>
                                                                        <a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
                                                                            <img id="imageID-<%# Eval("ID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif" />
                                                                        </a>
                                                                    </itemtemplate>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="MaintenanceActivityName" SortExpression="MaintenanceActivityName"
                                                        HeaderText="Maintenance Activity">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MaintenanceOn" SortExpression="MaintenanceOn" HeaderText="Maintenance On"
                                                        HtmlEncode="false">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ModelMonitorTypeCode" SortExpression="ModelMonitorTypeCode"
                                                        HeaderText="Monitor Type">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="MachineInfo" SortExpression="MachineInfo"
                                                        HeaderText="Aircraft Info">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="AssemblyType" SortExpression="AssemblyType"
                                                        HeaderText="Assembly Type">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo"
                                                        HeaderText="Assembly Info">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description"
                                                        HtmlEncode="false">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="True"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DirectiveNumber" SortExpression="DirectiveNumber" HeaderText="Directive Number">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb"
                                                        HeaderText="Period">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="ValueFormatted" SortExpression="ValueFormatted"
                                                        HeaderText="Value">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="RemovalReasonName" SortExpression="RemovalReasonName"
                                                        HeaderText="Removal Reason">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DoneOnWONO" SortExpression="DoneOnWONO" HeaderText="Work Order No.">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Place" SortExpression="Place" HeaderText="Place">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Comply Remark">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                        DisplayMode="List" ValidationGroup='<%# string.Format("Group_{0}", Eval("ID")) %>'
                                                                        HeaderText=""></asp:ValidationSummary>
                                                                    <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" Display="None"
                                                                        ValidationGroup='<%# string.Format("Group_{0}", Eval("ID")) %>' ErrorMessage="Remark should be less than 500 characters"></asp:CustomValidator>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine" MaxLength="500"
                                                                Height="30px" TextMode="MultiLine"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actual Man Hrs." HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="upnlValidationSummary1" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:ValidationSummary ID="Validationsummary21" runat="server" CssClass="clsValidationSummary"
                                                                        DisplayMode="List" ValidationGroup='<%# string.Format("Group_{0}", Eval("ID")) %>'
                                                                        HeaderText=""></asp:ValidationSummary>
                                                                    <asp:CustomValidator ID="cvActManHrs" runat="server" ControlToValidate="txtActualManHrs"
                                                                        Display="None" ValidationGroup='<%# string.Format("Group_{0}", Eval("ID")) %>'
                                                                        ErrorMessage="a"></asp:CustomValidator>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:TextBox ID="txtActualManHrs" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                                 AutoPostBack="true" MaxLength="8" ToolTip="Actual Man Hours"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="License No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle Wrap="True" HorizontalAlign="Left" BorderStyle="None"></ItemStyle>
                                                        <ItemTemplate>
                                                            <table style="border: 0;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="200"
                                                                            AutoPostBack="true" OnTextChanged="txtLicenceNo_TextChanged"></asp:TextBox>
                                                                        <cc2:AutoCompleteExtender ID="txtLicenceNo_Autocomplete" runat="server" CompletionInterval="1"
                                                                            CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                            CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                            TargetControlID="txtLicenceNo">
                                                                        </cc2:AutoCompleteExtender>
                                                                        <asp:HiddenField ID="hdnLicenceNo" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnLicenseEmpNo" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnActualManHrs" runat="server" ClientIDMode="Static" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                                            CommandName="EmployeeLicence" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            Height="22px" Width="24px" CausesValidation="true" />
                                                                    </td>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="lblLicenceCount1" Visible="false" runat="server" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </tr>
                                                            </table>
                                                            <asp:Label ID="lblLicenceCount" runat="server" Visible="false" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="Remove"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td colspan="100%" bgcolor="White" width="0px">
                                                                    <div id="ID-<%# Eval("ID") %>" style="display: none; position: relative; left: 25px;">
                                                                        <asp:GridView ID="grdLinkActivity" runat="server" AutoGenerateColumns="False" Width="95%"
                                                                            BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                                                            AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                            SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                                            <Columns>
                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                <asp:BoundField DataField="LinkedMaintenanceTypeName" HeaderText="Linked with">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField Visible="False" DataField="MonitorType" SortExpression="MonitorType"
                                                                                    HeaderText="Monitor Type">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA Chapter">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="DirectiveNo" SortExpression="DirectiveNo" HeaderText="Directive Number">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                    <HeaderStyle ForeColor="White" Wrap="true" Width="330px" HorizontalAlign="Left">
                                                                                    </HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Width="330px" CssClass="TextBreak" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="MaintenanceActionName" SortExpression="MaintenanceActionName"
                                                                                    HeaderText="Action Type">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle BackColor="ControlDark" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click To Comply"
                                                            Text="Comply"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddMore" runat="server" CssClass="clsButton" ToolTip="Click to add more"
                                                            Text="Add More"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
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
            <tr style="height: 0px;">
                <td style="height: 0px;">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                        <ContentTemplate>
                            <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                CausesValidation="False" Style="display: none;"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <div>
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
    </div>
    <!-- Done By Employee Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
    </div>
    <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
        X="90" PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameMaintDoneByStateComplete() {
            $("#btnDummyMaintDoneBy").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }


        function AddEmployeeLicNo(MaintTypeID) {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=" + MaintTypeID);

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
    </form>
    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            //            if ("<%= page.IsPostback%>" == "False") {
            $(".clstooltip").closest("tr").mousemove(function (event) {
                $(this).find(".clstooltip").css({
                    "left": event.pageX + 1,
                    "top": event.pageY + 1
                }).show();
            }).mouseout(function () { $(this).find(".clstooltip").hide(); }); ;
            //            }
        });
    </script>
</body>
</html>
