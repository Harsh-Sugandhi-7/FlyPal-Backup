<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyAssemblyMonitorModStatusListShowValues_Ajax.aspx.vb"
    Inherits="Flypal.wfComplyAssemblyMonitorModStatusListShowValues_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Assembly Directive Status List</title>
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
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">List of Assembly Directives Status</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Search Criteria</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="width: 80px;">
                                                                                <span id="lblDate" class="clsLabelAuto">Date</span>
                                                                            </td>
                                                                            <td style="width: 195px;">
                                                                                <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBox_Ajax" ReadOnly="True"
                                                                                    BackColor="#E0E0E0" Width="100px" onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                                </cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox">
                                                                                </cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td style="width: 80px;">
                                                                                <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                                            </td>
                                                                            <td style="width: 220px;">
                                                                                <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                    AutoPostBack="true" DataTextField="RegNo" DataValueField="ID">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td style="width: 80px">
                                                                                <span id="lblModel" class="clsLabelAuto">Assembly</span>
                                                                            </td>
                                                                            <td style="width: 250px">
                                                                                <asp:DropDownList ID="cmbAircraftAssembly" runat="server" CssClass="clsComboBox_Ajax"
                                                                                    AutoPostBack="true" DataValueField="ID" DataTextField="ModelSerialNoPostion">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkOneTimeMasterRecords" runat="server" CssClass="clsLabelAuto"
                                                                                    ToolTip='Check to get one time done master records' AutoPostBack="true" Text='"ONE TIME DONE" Master Records'
                                                                                    TextAlign="Left"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="width: 80px;">
                                                                                <span id="lblDirectiveNo" class="clsLabelAuto">Directive No.</span>
                                                                            </td>
                                                                            <td style="width: 195px;">
                                                                                <asp:TextBox ID="txtDirectiveNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                                    AutoPostBack="true" ToolTip="Enter Directive No."></asp:TextBox>
                                                                            </td>
                                                                            <td style="width: 80px;">
                                                                                <span id="lblMonitorType" class="clsLabel">Monitor Type</span>
                                                                            </td>
                                                                            <td style="width: 220px;">
                                                                                <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                                    AutoPostBack="true" DataTextField="ModelMonitorModTypeName" DataValueField="ID">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td style="width: 80px">
                                                                                <span id="Span1" class="clsLabelAuto">Code/Form No./Description</span>
                                                                            </td>
                                                                            <td style="width: 250px">
                                                                                <asp:TextBox runat="server" ID="txtCodeFormNo" CssClass="clsTextBox_Ajax" AutoPostBack="true"
                                                                                    Width="250px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  records'
                                                                                    AutoPostBack="true" TextAlign="Left" Text='Show ONLY "NOT  APPLICABLE" records'>
                                                                                </asp:CheckBox>
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
                                                                                ValidationGroup="1" Visible="False"></asp:Button>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
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
                                                            <table id="Table5" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                            ToolTip="Click to Add Directives" Text="Add New"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                            Visible="false" ToolTip="Click to print List of Assembly Directives" Text="Print"
                                                                            CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                            ToolTip="Click to close List of Assembly Directives Status screen" Text="Close"
                                                                            CausesValidation="False"></asp:Button>
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
                                                                <asp:GridView ID="dgDueMonitoringList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                    DataKeyNames="ID" PageSize="5" AllowSorting="True" ShowHeaderWhenEmpty="true">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="AssemblyID" HeaderText="AssemblyID" SortExpression="AssemblyID"
                                                                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="AssemblyStatusID" HeaderText="AssemblyStatusID" SortExpression="AssemblyStatusID"
                                                                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="HourType" HeaderText="HourType" SortExpression="HourType"
                                                                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Aircraft" SortExpression="RegNo" HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Number" SortExpression="Number" HeaderText="Directive Number">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Monitor Info.">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ModelMonitorModCode_Desc" SortExpression="ModelMonitorModCode_Desc"
                                                                            HeaderText="Code/Form No./Description" HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneWONo" SortExpression="DoneWONo" HeaderText="Work Order No.">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Frequency" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFreqValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkFreqValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Effective From/DoneOn Value" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDoneOnValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkDoneOnValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Current" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkCurrentValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Elapsed" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblElapsedValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkElapsedValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Extension" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblExtensionValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkExtensionValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Due At." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDueAtValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkDueAtValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Due At Airframe" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDueAtAirframeValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkDueAtAirframeValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Remaining" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRemainingValues" runat="server" CssClass="clsLabelAuto" ClientIDMode="Static"></asp:Label>
                                                                                <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemainingValue" CommandName="ShowVal"
                                                                                    runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:ButtonField Text="Comply" HeaderText="Comply" CommandName="Comply">
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
                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                            DataField="IsMaster" HeaderText="IsMaster"></asp:BoundField>
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
                        <!--Dummy panel to open modelpopup for city-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnDirectiveHistory" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnAssemblyDirectiveListNew" ClientIDMode="Static" runat="server"
                                            Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table21" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                        ToolTip="Click to Add Directives" Text="Add New"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print List of Assembly Directives"
                                                        Visible="false" Text="Print" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Assembly Directives Status screen"
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
    <!--Directive History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyDirectiveHistory" Text="Directive History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlDirectiveHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeDirectiveHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupDirectiveHistory" runat="server" TargetControlID="btnDummyDirectiveHistory"
        PopupControlID="pnlDirectiveHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameDirectiveHistoryStateComplete() {
            $("#btnDummyDirectiveHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenDirectiveHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeDirectiveHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorModStatusList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyDirectiveHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForDirectiveHistory() {
            var DirectiveHistorywindow = $find("<%=mdlPopupDirectiveHistory.ClientID %>");
            //close Directive History popup window
            DirectiveHistorywindow.hide();
            //           release resources
            $("#IframeDirectiveHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnDirectiveHistory").click();
        }
    </script>
    <!-- End-->
    <!--Assembly Directive List New Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAssemblyDirectiveListNew" Text="Assembly Directive List New"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlAssemblyDirectiveListNew" ClientIDMode="Static"
        HorizontalAlign="Center" Style="height: 100%; width: 100%;">
        <iframe id="IframeAssemblyDirectiveListNew" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupAssemblyDirectiveListNew" runat="server" TargetControlID="btnDummyAssemblyDirectiveListNew"
        PopupControlID="pnlAssemblyDirectiveListNew" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameAssemblyDirectiveListNewStateComplete() {
            $("#btnDummyAssemblyDirectiveListNew").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenAssemblyDirectiveListNewWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeAssemblyDirectiveListNew").attr("src", "wfAssemblyMonitorModStatusListNew_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyAssemblyDirectiveListNew").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForAssemblyDirectiveListNew() {
            var AssemblyDirectiveListNewwindow = $find("<%=mdlPopupAssemblyDirectiveListNew.ClientID %>");
            //close Assembly Directive List New popup window
            AssemblyDirectiveListNewwindow.hide();
            //           release resources
            $("#IframeAssemblyDirectiveListNew").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnAssemblyDirectiveListNew").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
