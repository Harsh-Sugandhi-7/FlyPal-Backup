<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDueJobPlanning_Ajax.aspx.vb" Inherits="Flypal.wfDueJobPlanning_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Planning</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
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
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label CssClass="clsFormHeader" ID="lblTitle" runat="server"> Maintenance Planning</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>

                                                                    <asp:Button class="clsbtnH clsinfoH" ID="btnSave" runat="server" Text="Save" ToolTip="Click to save"
                                                                        ValidationGroup="a" Enabled='<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>'></asp:Button>

                                                                    <asp:Button class="clsbtnH clsinfoH" ID="btnPrint" runat="server" Text="Print" ToolTip="Click to Print"
                                                                        ValidationGroup="a" Enabled='<%#IIf(mDueJobPlanning.IsNew, False, True) %>'></asp:Button>

                                                                    <asp:Button class="clsbtnH clsinfoH" ID="btnBack" runat="server" Text="Close" ToolTip="Click to close"></asp:Button>
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
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" ErrorMessage="Maintenance Planning Date Required." ControlToValidate="txtDueJobPlanningDate"
                                                Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" ErrorMessage="" ControlToValidate="txtFromDate"
                                                Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                              <asp:CustomValidator ID="CustomValidator2" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a"  ErrorMessage="" ControlToValidate="txtToDate"
                                                Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator
                                                ID="rfvDate" runat="server" Display="None" ErrorMessage="Date Required."
                                                ValidationGroup="a" ControlToValidate="txtDueJobPlanningDate" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDueJobPlanningDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsEmpDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                <legend id="ledEmpDetails" class="clsLabelHeader">Details</legend>
                                                <table>
                                                    <tr>
                                                        <td width="50%">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblStarDate" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblDate" class="clsLabel">Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDueJobPlanningDate" CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ClientIDMode="Static" 
                                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                            Text="" ></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtDueJobPlanningDate_CalendarExtender" runat="server"
                                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDueJobPlanningDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender ID="txtDueJobPlanningDateWatermarkExtender" runat="server"
                                                                            TargetControlID="txtDueJobPlanningDate" WatermarkCssClass="clsDateTextBox"
                                                                            WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblStarInvoiceNo" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblNo" class="clsLabel">No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDueJobPlanningText" runat="server" Text="<%# mDueJobPlanning.Text %>"
                                                                                        CssClass="clsTextBoxTagSearch" MaxLength="25" Enabled='<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>'> </asp:TextBox>
                                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtDueJobPlanningText_Autocomplete"
                                                                                        runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                                        MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfDueJobPlanning_Ajax.aspx"
                                                                                        ServiceMethod="GetDistinctTextListAutoComplete" TargetControlID="txtDueJobPlanningText"
                                                                                        UseContextKey="False">
                                                                                    </cc2:AutoCompleteExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDueJobPlanningNo" runat="server" Text="<%# mDueJobPlanning.No %>"
                                                                                        CssClass="clsTextBoxTagSearchRightAlign1" MaxLength="8" ToolTip="Enter No." Enabled='<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>'> </asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>
                                                                    <td>
                                                                        <span id="lblFromStar" class="clsLabelStar">*</span>

                                                                    </td>
                                                                    <td>
                                                                        <span id="lblFromDate" class="clsLabelAuto">From Date</span>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                            Text="<%# mDueJobPlanning.FromDateFormatted %>" autocomplete="off" Enabled='<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>'></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="txtFromDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblToDateStar" class="clsLabelStar">*</span>

                                                                    </td>
                                                                    <td>
                                                                        <span id="lblToDate" class="clsLabelAuto">To Date</span>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                            Text="<%# mDueJobPlanning.ToDateFormatted %>" autocomplete="off" Enabled='<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>'></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="txtToDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="lblRegNo" class="clsLabel">Reg No.</span>
                                                                    </td>
                                                                    <td colspan="10">
                                                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                            Text="<%# mDueJobPlanning.RegNo %>" ToolTip="Enter Reg No." Enabled="false">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <input class="clsbtnH" type="button" id="btnSelectFile" value="Select File"
                                                                                                runat="server" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button class="clsbtnH" ID="btnDelAttach" runat="server" Enabled="False" Text="Remove Attachment" ToolTip="Click to Remove Attachment" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px" ImageUrl="icons/CLIP01.ICO" Width="20px" ToolTip="Click to view attachment " />
                                                                                            <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="----" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>


                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                                    </td>
                                                                    <td colspan="7">
                                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                            MaxLength="1000" Text="<%# mDueJobPlanning.Remark %>" ToolTip="Enter Remark" TextMode="MultiLine" Enabled="<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="50%">
                                                            <%--AJAX- Add UpdatePanel  --%>
                                                            <asp:UpdatePanel ID="upnlDueJobPlanningPeriods" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                        <legend class="clsFieldSet1"><b>
                                                                            <asp:Label ID="lblCurrentValue" runat="server" CssClass="clsLabelHeader">Current & Planning Details</asp:Label><!-- 'ALL27072020-->
                                                                        </b></legend>
                                                                        <table id="Table7" border="0" cellspacing="1" cellpadding="1" width="100%">
                                                                            <tr>
                                                                                <td valign="top" align="left">
                                                                                    <asp:GridView ID="dgDueJobPlanningPeriod" runat="server" CssClass="clsGridNewStyle" ToolTip="Planning Periods" Enabled='<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>'
                                                                                        PageSize="3" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                        <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                                                        <Columns>
                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="PeriodName" HeaderText="Periods">
                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current">
                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:TemplateField HeaderText="Planned">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="80px"
                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "PlannedValue") %>' ToolTip="Enter corresponding Period Value"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="EstimatedValueFormatted" HeaderText="Estimated Value" Visible="false">
                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                                            </asp:BoundField>
                                                                                        </Columns>
                                                                                        <PagerStyle HorizontalAlign="Right" BorderStyle="Solid"></PagerStyle>
                                                                                        <PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                                <%--    <td valign="top">
                                                                                    <asp:ImageButton ID="btnSelectPeriod" runat="server" ImageUrl="~/images/plus1.png"
                                                                                        Height="22px" Width="24px" ToolTip="Click to Add New Periods" CausesValidation="true"></asp:ImageButton>
                                                                                </td>--%>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
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

                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDueJobPlanningItem" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsCApDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                <legend id="ledCADetails" class="clsLabelHeader">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblDueJob" class="clsLabelHeader">Maintenance(s):</span>
                                                            </td>
                                                            <td>
                                                                <asp:Button class="clsbtnH" ID="btnAssemblyAdd" runat="server" Text="Add" ToolTip="Click To Add Maintenance(s)."
                                                                    ValidationGroup="a" Enabled='<%#IIf(mDueJobPlanning.IsWOCreated, False, True) %>'></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </legend>

                                                <table width="100%">
                                                    <tr>
                                                        <td align="right" valign="top">
                                                            <asp:ImageButton ID="imgCreateWO" runat="server" Enabled='<%#IIf(mDueJobPlanning.IsNew Or mDueJobPlanning.IsWOCreated, False, True) %>' ToolTip="Click to Create Work Order"
                                                                Style="height: 20px; width: 17px" ImageUrl="~/images/TaskCard.png" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgDueJobPlanningItem" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                                AutoGenerateColumns="False" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                <Columns>
                                                                    <%--0--%>
                                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                    <%--1--%>
                                                                    <asp:BoundField DataField="SrNo" HeaderText="SN">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />

                                                                    </asp:BoundField>
                                                                    <%--2--%>
                                                                    <asp:BoundField DataField="TaskNo" HeaderText="Task No.">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />

                                                                    </asp:BoundField>
                                                                    <%--3--%>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="false">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />

                                                                    </asp:BoundField>
                                                                    <%--4--%>
                                                                    <asp:BoundField DataField="FrequencyValue" HeaderText="Interval" HtmlEncode="false">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />

                                                                    </asp:BoundField>
                                                                    <%--5--%>
                                                                    <asp:BoundField DataField="DueAsOf" HeaderText="Due As Of" HtmlEncode="false">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />

                                                                    </asp:BoundField>
                                                                    <%--6--%>
                                                                    <asp:BoundField DataField="EstimatedHoursforGrid" HeaderText="Estimated Man Hrs.">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />

                                                                    </asp:BoundField>
                                                                    <%--7--%>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div class="dropdown">
                                                                                <div class="dropdownbtn-content">
                                                                                    <table id="T1" class="clsGridNew_Ajax">

                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRecord" Style="height: 20px; width: 20px"
                                                                                                    ImageUrl="~/images/delete.png" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' />
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
                                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnDueJobPlanningItem" ClientIDMode="Static" runat="server" Text="----"
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
        <!--Maintenance Planning Item Popup Window -->
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForDueJobPlanningSelection();
                return false;
            }
        </script>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDueJobPlanningItem" Text="DueJobPlanningItem" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlDueJobPlanningItem" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeDueJobPlanningItem" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlDueJobPlanningItem" runat="server" TargetControlID="btnDummyDueJobPlanningItem"
            PopupControlID="pnlDueJobPlanningItem" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IframeDueJobPlanningItemStateComplete() {
                $("#btnDummyDueJobPlanningItem").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenDueJobPlanningItemWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeDueJobPlanningItem").attr("src", "wfSelectDueJobList_Ajax.aspx?Type=pup");

                    /*if (!$.browser.msie) {*/
                    $("#btnDummyDueJobPlanningItem").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                    //}
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForDueJobPlanningItem() {
                var DueJobPlanningItemWindow = $find("<%=mdlDueJobPlanningItem.ClientID %>");
                //close popup window
                DueJobPlanningItemWindow.hide();
                //release resources
                $("#IframeDueJobPlanningItem").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnDueJobPlanningItem").click();
            }
        </script>
        <!-- End-->

        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
<% Dim mopen As String = Request.QueryString("Type") %>
 <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameDueJobStateComplete();
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
        <%--End--%>
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
