<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyAssemblyMonitorServiceStatusList_Ajax.aspx.vb"
    Inherits="Flypal.wfComplyAssemblyMonitorServiceStatusList_Ajax" EnableEventValidation="false" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Assembly Service Status List</title>
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
        .style1 {
            height: 17px;
        }

        .aspNetDisabled {
            color: Black !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            OnAsyncPostBackError="ScriptManager1_AsyncPostBackError" EnablePageMethods="true">
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
                                    <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1" Text='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "List of Maintenance Event", "List of Assembly Service Status") %>'></asp:Label>
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
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
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
                                                                    <asp:PlaceHolder ID="pllblAircraft" runat="server">
                                                                        <td>
                                                                            <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="plAircraft" runat="server">
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                AutoPostBack="true" Width="100px" DataTextField="RegNo" DataValueField="ID">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="lblModel" class="clsLabelAuto">Assembly</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAircraftAssembly" runat="server" CssClass="clsComboBox_Ajax"
                                                                            AutoPostBack="true" DataValueField="ID" DataTextField="ModelSerialNoPostion">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkOneTimeMasterRecords" runat="server" CssClass="clsLabelAuto"
                                                                            ToolTip='Check to get one time done master records' AutoPostBack="true" Text='"ONE TIME DONE" Master Records'
                                                                            TextAlign="Left"></asp:CheckBox>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 80px">
                                                                        <span id="lblMonitorType" class="clsLabelAuto" runat="server" ><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Task Type", "Monitor Type") %> </span>
                                                                    </td>
                                                                    <td style="width: 260px">
                                                                        <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                            AutoPostBack="true" DataValueField="ID" DataTextField="ModelMonitorServiceTypeName">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width: 80px">
                                                                        <span id="lblCodeFormNo" runat="server"  class="clsLabelAuto"><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Task No.", "Code/Form No./Description") %></span>
                                                                    </td>
                                                                    <td style="width: 260px">
                                                                        <asp:TextBox runat="server" ID="txtCodeFormNo" CssClass="clsTextBox_Ajax" AutoPostBack="true"
                                                                            Width="250px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  records'
                                                                            AutoPostBack="true" Text='Show ONLY "NOT  APPLICABLE" records' TextAlign="right"></asp:CheckBox>
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
                                                                        ToolTip="Click to find list of Service as per searching criteria" Text="Find Now"
                                                                        ValidationGroup="1" Visible="False"></asp:Button>
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
                                                                <table id="Table3" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNewTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                                ValidationGroup="1" ToolTip="Click to Add" Text="Add New"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                                Visible="false" ToolTip="Click to print List" Text="Print"
                                                                                CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnBackTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                                ToolTip="Click to close List" Text="Close"
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
                                                                    <asp:GridView ID="dgDueMonitoringList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                        ShowHeaderWhenEmpty="true" EnableViewState="true" CssClass="clsGrid" PageSize="5"
                                                                        OnRowDataBound="dgDueMonitoringList_RowDataBound">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="TaskNo" SortExpression="TaskNo" HeaderText="Task No.">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap ="false" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Reference" HeaderText="Reference Doc." SortExpression="Reference">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RegNo" HeaderText="Machine Info." SortExpression="RegNo"
                                                                                Visible="False">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField Visible="False" DataField="RegNo" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RegNo" HeaderText="Assembly Info." SortExpression="AssemblyInfo"
                                                                                Visible="False">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="MonitorTypeCode" HeaderText="Task Type" SortExpression="MonitorTypeCode">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="MonitorType" HeaderText="Monitor Type" Visible="false"
                                                                                SortExpression="MonitorType">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ATAChapter" HeaderText="ATA" SortExpression="ATAChapter">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Code_Desc" HeaderText="Description" SortExpression="Code_Desc"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DoneOnDate" HeaderText="Compliance Date" HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DoneWONO" HeaderText="Work Order No." SortExpression="DoneWONO">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="DoneRemark">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RegNo" HeaderText="Period Unit" Visible="false" HtmlEncode="false"
                                                                                SortExpression="RegNo">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Freq3ForGrid" HeaderText="Threshold" SortExpression="Freq3ForGrid"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DoneAt2ForGrid" HeaderText="Effective From/Compliance Value"
                                                                                SortExpression="DoneAt2ForGrid" HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SinceNewTSNCSN" HeaderText="Current" SortExpression="SinceNewTSNCSN"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" SortExpression="ElapsedValueFormatted"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" SortExpression="ExtensionValueFormatted"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DueAtTimeForCompliancePage" HeaderText="Due At" SortExpression="DueAsOf2ForGrid"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DueAsOf2ForGrid" HeaderText="Due At Airframe" HeaderStyle-CssClass="hideGridColumn"
                                                                                ItemStyle-CssClass="hideGridColumn" HtmlEncode="false" SortExpression="AssemblyDueOnValueTextFormattedByAirFrameForGrid">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RemainingTimeForCompliancePage" SortExpression="RemainingValueFormatted"
                                                                                HtmlEncode="false" HeaderText="Remaining">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="False"></ItemStyle>
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
																			<%--Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity--%>
																			<asp:ButtonField CommandName="Revise" HeaderText="Revise" Text="Revise">
																				<HeaderStyle HorizontalAlign="Left" />
																			</asp:ButtonField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style1">
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
                                            <table id="Table2" border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                            ValidationGroup="1" Text="Add New" ToolTip="Click to Add" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Visible="false" TabIndex="0" Text="Print" ToolTip="Click to print List" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close screen" />
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
                                            <asp:Button ID="hdnBtnServiceHistory" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnAssemblyServiceListNew" ClientIDMode="Static" runat="server"
                                                Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
											<%--Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity--%>
											<asp:Button ID="hdnBtnModelServiceMaster" ClientIDMode="Static" runat="server"
												Text="..." CausesValidation="False" Style="display: none;">
											</asp:Button>
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
        <!--Service History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyServiceHistory" Text="Service History" ClientIDMode="Static" />
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
        <!--Assembly Service List New Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAssemblyServiceListNew" Text="Assembly Service List New"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAssemblyServiceListNew" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAssemblyServiceListNew" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAssemblyServiceListNew" runat="server" TargetControlID="btnDummyAssemblyServiceListNew"
            PopupControlID="pnlAssemblyServiceListNew" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAssemblyServiceListNewStateComplete() {
                $("#btnDummyAssemblyServiceListNew").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenAssemblyServiceListNewWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAssemblyServiceListNew").attr("src", "wfAssemblyMonitorServiceStatusListNew_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAssemblyServiceListNew").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForAssemblyServiceListNew() {
                var AssemblyServiceListNewwindow = $find("<%=mdlPopupAssemblyServiceListNew.ClientID %>");
                //close Assembly Service List New popup window
                AssemblyServiceListNewwindow.hide();
                //           release resources
                $("#IframeAssemblyServiceListNew").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnAssemblyServiceListNew").click();
            }
        </script>
        <!-- End-->

		<%--Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity--%>

		<!--Model Service Master Popup Window -->

		<div style="display: none">

			<asp:Button runat="server" ID="btnDummyModelServiceMaster" Text="Model Servuice Master"
				ClientIDMode="Static" />

		</div>

		<asp:Panel runat="server" ID="pnlModelServiceMaster" ClientIDMode="Static" HorizontalAlign="Center"
			Height="100%" Width="100%">

			<iframe id="IframeModelServiceMaster" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto">
			</iframe>

		</asp:Panel>

		<cc2:ModalPopupExtender ID="mdlPopupModelServiceMaster" runat="server" BackgroundCssClass="clsModalPopupBG"
			TargetControlID="btnDummyModelServiceMaster" PopupControlID="pnlModelServiceMaster">
		</cc2:ModalPopupExtender>

		<script type="text/javascript">

			function IframeModelServiceMasterStateComplete() {
				$("#btnDummyModelServiceMaster").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModelServiceMasterWindow() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelServiceMaster").attr("src", "wfModelMonitorService_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

					if (!$.browser.msie) {
						$("#btnDummyModelServiceMaster").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
                    return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForModelServiceMaster() {
				var ModelInspMasterwindow = $find("<%=mdlPopupModelServiceMaster.ClientID %>");
				ModelInspMasterwindow.hide();
				$("#IframeModelServiceMaster").attr("src", "JavaScript:''");
				$("#hdnBtnModelServiceMaster").click();
			}
		</script>

    </form>
</body>
</html>
