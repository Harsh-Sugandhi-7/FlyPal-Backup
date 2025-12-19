<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyCompMonitorModStatusList_Ajax.aspx.vb"
    Inherits="Flypal.wfComplyCompMonitorModStatusList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Component Modification Status List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
        .aspNetDisabled
        {
            color: Black !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
    i<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">List of Component Modification Status</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <%-- <asp:CustomValidator ID="cvMachine" runat="server" ClientValidationFunction="validateAircraft"
                                    ErrorMessage="Please Select the Aircraft from the list." ControlToValidate="cmbAircraftList"
                                    Display="None" ValidationGroup="1"></asp:CustomValidator>
                                <script type="text/javascript">

                                    function validateAircraft(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbAircraftList");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }
                                </script>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Search Criteria</b></legend>
                                            <table width="100%">
                                                <asp:PlaceHolder ID="phSpareComp" runat="server">
                                                    <tr>
                                                        <td colspan="1">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbSpareComponent" GroupName="a" runat="server" Checked="true"
                                                                            AutoPostBack="true" Text="Show Modifications of Stock Component"></asp:RadioButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbRemovedComp" GroupName="a" runat="server" AutoPostBack="true"
                                                                            Text="Show Modifications of Removed Component"></asp:RadioButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbSpareAssemblyComponent" GroupName="a" AutoPostBack="true"
                                                                            runat="server" Text="Show Modifications of Components on Stock Assembly"></asp:RadioButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </asp:PlaceHolder>
                                                <tr>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <asp:PlaceHolder ID="phDateAircraft" runat="server">
                                                                    <td style="width: 80px">
                                                                        <span id="lblDate" class="clsLabelAuto">Date</span>
                                                                    </td>
                                                                    <td style="width: 260px">
                                                                        <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBox_Ajax" ReadOnly="True"
                                                                            BackColor="#E0E0E0" Width="100px" onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                   
                                                                    <td>
                                                                        <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsComboBox_Ajax"
                                                                            AutoPostBack="true" Width="100px" DataTextField="RegNo" DataValueField="ID">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    
                                                                </asp:PlaceHolder>
                                                                <asp:PlaceHolder ID="phAssembly" runat="server">
                                                                    <td>
                                                                        <span id="lblAssembly" class="clsLabelauto">Assembly</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                                                            DataTextField="ModelSerialNoPostion" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </asp:PlaceHolder>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 80px">
                                                                    <span id="lblPart" class="clsLabelauto">Part No.</span>
                                                                </td>
                                                                <td style="width: 260px">
                                                                    <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Part"
                                                                        MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelauto">Serial No. </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Serial Number"
                                                                        MaxLength="50"></asp:TextBox>
                                                                </td>
                                                               
                                                                <td>
                                                                    <span id="lblMonitorType" class="clsLabelAuto">Monitor Type</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBox3_Ajax"
                                                                        DataTextField="PartMonitorModTypeName" DataValueField="ID" Width="250px" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px">
                                                                    <span id="lblDirectiveNo" class="clsLabelauto">Mod No.</span>
                                                                </td>
                                                                <td style="width: 260px">
                                                                    <asp:TextBox ID="txtDirectiveNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Directive No."
                                                                        MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 80px">
                                                                    <span id="Span1" class="clsLabelAuto">Code/Form No./Description</span>
                                                                </td>
                                                                <td style="width: 260px">
                                                                    <asp:TextBox runat="server" ID="txtCodeFormNo" CssClass="clsTextBox_Ajax" AutoPostBack="true"
                                                                        Width="250px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  records'
                                                                        Text='Show ONLY "NOT  APPLICABLE" records' AutoPostBack="True"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td style="padding-left: 4px">
                                                                    <asp:CheckBox ID="chkOneTimeMasterRecords" runat="server" AutoPostBack="true" CssClass="clsLabelAuto"
                                                                        Text="&quot;ONE TIME DONE&quot; Master Records" ToolTip="Check to get one time done master records" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 4px">
                                                        <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                            Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                    ToolTip="Click to find list of Modification as per searching criteria" Text="Find Now"
                                                                    ValidationGroup="1"></asp:Button>
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
                                <asp:UpdatePanel ID="upnlgrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkShowAllRecordsTop" runat="server" CssClass="clsLinkButton"
                                                                    Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table2" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                            ValidationGroup="1" Text="Add New" ToolTip="Click to Add Modification" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            Visible="false" TabIndex="0" Text="Print" ToolTip="Click to print List of Component Modification" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close List of Component Modification Status screen" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgDueMonitoringList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                    PageSize="5" ShowHeaderWhenEmpty="true" EnableViewState="false" CssClass="clsGrid"
                                                                    OnRowDataBound="dgDueMonitoringList_RowDataBound">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False">
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <%--<asp:BoundField DataField="Number" HeaderText="Mod No." SortExpression="Number">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>--%>
                                                                         <asp:TemplateField HeaderText="Mod No." ItemStyle-Width="30">
                                                                            <ItemTemplate>
                                                                                <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# Eval("RefAttachlink") %>'
                                                                                    Text='<%# Eval("Number") %>' ToolTip='<%# Eval("RefAttachlink") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="MachineInfo" HeaderText="Aircraft Info." SortExpression="MachineInfo"
                                                                           Visible="False" >
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Assembly Type" SortExpression="RegNo"
                                                                            Visible="False">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo"
                                                                           Visible="False">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CompInfo" HeaderText="Comp. Info." SortExpression="CompInfo"  HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="MonitorTypeCode" HeaderText="Monitor Info." SortExpression="MonitorTypeCode">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="MonitorType" HeaderText="Monitor Type" Visible="false"
                                                                            SortExpression="MonitorType">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ATAChapter" HeaderText="ATA" SortExpression="ATAChapter" >
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneOnDate" HeaderText="Done On">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneWONO" HeaderText="Work Order No." SortExpression="DoneWONO">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Period" Visible="false" SortExpression="RegNo"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Freq3ForGrid" HeaderText="Frequency" SortExpression="FrequencyValueFormatted"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneAt2ForGrid" HeaderText="Effective From/DoneOn Value"
                                                                            SortExpression="DoneAt2ForGrid" HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="True" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="SinceNewTSNCSN" HeaderText="Current" SortExpression="SinceNewTSNCSN"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ElapsedValue" HeaderText="Elapsed" SortExpression="ElapsedValue"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" SortExpression="ExtensionValueFormatted"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DueAtTimeForCompliancePage" HeaderText="Due At" SortExpression="DueAtTimeForCompliancePage"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DueAsOf2ForGrid" HeaderText="Due At Airframe"
                                                                            HtmlEncode="false" SortExpression="DueAsOf2ForGrid" Visible="false" > 
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RemainingTimeForCompliancePage" HeaderText="Remaining"
                                                                            HtmlEncode="false" SortExpression="RemainingTimeForCompliancePage">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:ButtonField CommandName="Comply" HeaderText="Comply" Text="Comply">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
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
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lnkShowAllRecords" runat="server" CssClass="clsLinkButton" ForeColor="Red"
                                                                    Visible="<%$AppSettings:IsShowAllRecordsVisible%>" Text="Show All Records"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        ValidationGroup="1" Text="Add New" ToolTip="Click to Add Modification" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Visible="false" TabIndex="0" Text="Print" ToolTip="Click to print List of Component Modification" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close List of Component Modification Status screen" />
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
                                        <asp:Button ID="hdnBtnCompDirectiveHistory" ClientIDMode="Static" runat="server"
                                            Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnCompModListNew" ClientIDMode="Static" runat="server" Text="Add"
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
    <!--Comp Directive History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCompDirectiveHistory" Text="Comp Directive History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCompDirectiveHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCompDirectiveHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCompDirectiveHistory" runat="server" TargetControlID="btnDummyCompDirectiveHistory"
        PopupControlID="pnlCompDirectiveHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCompDirectiveHistoryStateComplete() {
            $("#btnDummyCompDirectiveHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCompDirectiveHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCompDirectiveHistory").attr("src", "wfUpdateComplyHistoryCompMonitorModStatusList_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCompDirectiveHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCompDirectiveHistory() {
            var CompDirectiveHistorywindow = $find("<%=mdlPopupCompDirectiveHistory.ClientID %>");
            //close Comp Directive History popup window
            CompDirectiveHistorywindow.hide();
            //           release resources
            $("#IframeCompDirectiveHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCompDirectiveHistory").click();
        }
    </script>
    <!-- End-->
    <!--Comp Mod List New Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCompModListNew" Text="Comp Mod List New" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCompModListNew" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCompModListNew" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCompModListNew" runat="server" TargetControlID="btnDummyCompModListNew"
        PopupControlID="pnlCompModListNew" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCompModListNewStateComplete() {
            $("#btnDummyCompModListNew").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCompModListNewWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCompModListNew").attr("src", "wfCompMonitorModStatusListNew_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCompModListNew").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCompModListNew() {
            var CompModListNewwindow = $find("<%=mdlPopupCompModListNew.ClientID %>");
            //close Comp Mod List New popup window
            CompModListNewwindow.hide();
            //           release resources
            $("#IframeCompModListNew").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCompModListNew").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
