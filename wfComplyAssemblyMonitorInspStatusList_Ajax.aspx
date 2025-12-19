<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyAssemblyMonitorInspStatusList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfComplyAssemblyMonitorInspStatusList_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Assembly Inspection Status List</title>
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
        .aspNetDisabled {
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
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">List of Assembly Inspection Status</asp:Label>
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
                                                                        <span id="lblMonitorType" class="clsLabelAuto">Monitor Type</span>
                                                                    </td>
                                                                    <td style="width: 260px">
                                                                        <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                            AutoPostBack="true" Width="250px" DataTextField="ModelMonitorInspTypeName" DataValueField="ID">
                                                                        </asp:DropDownList>
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
                                                                            AutoPostBack="true" Text='Show ONLY "NOT  APPLICABLE" records'></asp:CheckBox>
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
                                                                        ToolTip="Click to find list of Inspection as per searching criteria" Text="Find Now"
                                                                        Visible="false" ValidationGroup="1"></asp:Button>
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
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkLoadMoreTop" runat="server" CssClass="clsLinkButton" ForeColor="Red"
                                                            Visible="<%$AppSettings:IsShowAllRecordsVisible%>" Text="Show All Records"></asp:LinkButton>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table2" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                                Text="Add New" ToolTip="Click to Add Inspection" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                Visible="false" TabIndex="0" Text="Print" ToolTip="Click to print List of Assembly Inspection" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnBackTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                TabIndex="0" Text="Close" ToolTip="Click to close List of Assembly Inspection Status screen" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:GridView ID="dgDueMonitoringList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            PageSize="5" ShowHeaderWhenEmpty="true" EnableViewState="true" CssClass="clsGrid"
                                                            OnRowDataBound="dgDueMonitoringList_RowDataBound">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Machine Info." SortExpression="RegNo"
                                                                    Visible="False">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Assembly Type" SortExpression="AssemblyType"
                                                                    Visible="False">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Assembly Info." SortExpression="AssemblyInfo"
                                                                    Visible="False">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="MonitorTypeCode" HeaderText="Monitor Info." SortExpression="MonitorTypeCode">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%-- 5--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Monitor Type" Visible="false" SortExpression="MonitorType">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="ATAChapter" HeaderText="ATA" SortExpression="ATAChapter">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:BoundField DataField="DoneOnDate" HeaderText="Done On">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--9--%>
                                                                <asp:BoundField DataField="DoneWONO" HeaderText="Work Order No." SortExpression="DoneOnWONo">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--10--%>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--11--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Period Unit" Visible="false" HtmlEncode="false"
                                                                    SortExpression="PeriodUnitName">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--12--%>
                                                                <asp:BoundField DataField="Freq3ForGrid" HeaderText="Frequency" SortExpression="Freq3ForGrid"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--13--%>
                                                                <asp:BoundField DataField="DoneAt2ForGrid" HeaderText="Effective From/DoneOn Value"
                                                                    SortExpression="DoneAt2ForGrid" HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--14--%>
                                                                <asp:BoundField DataField="SinceNewTSNCSN" HeaderText="Current" SortExpression="SinceNewTSNCSN"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--15--%>
                                                                <asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" SortExpression="ElapsedValueFormatted"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--16--%>
                                                                <asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" SortExpression="ExtensionValueFormatted"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--17--%>
                                                                <asp:BoundField DataField="DueAtTimeForCompliancePage" HeaderText="Due At." SortExpression="DueAtTimeForCompliancePage"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--18--%>
                                                                <asp:BoundField DataField="DueAsOf2ForGrid" HeaderText="Due At Airframe" HtmlEncode="false"
                                                                    Visible="false" SortExpression="DueAsOf2ForGrid">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--19--%>
                                                                <asp:BoundField DataField="RemainingTimeForCompliancePage" HeaderText="Remaining"
                                                                    HtmlEncode="false" SortExpression="RemainingTimeForCompliancePage">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--20--%>
                                                                <asp:ButtonField CommandName="Comply" HeaderText="Comply" Text="Comply">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <%--21--%>
                                                                <asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <%--22--%>
                                                                <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <%--23--%>
                                                                <asp:ButtonField CommandName="History" HeaderText="History" Text="History">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <%--24--%>
                                                                <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <%--25--%>
                                                                <%--   <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="View" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                            CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                            Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <%--26--%>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <%--27--%>
                                                                <asp:ButtonField CommandName="Revise" HeaderText="Revise" Text="Revise">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <%--28--%>
                                                                <asp:BoundField DataField="IsApplicable" HeaderText="IsApplicable" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <%--29--%>
                                                                <asp:BoundField DataField="MonitorTypeID" HeaderText="MonitorTypeID" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:LinkButton ID="lnkLoadMore" runat="server" CssClass="clsLinkButton" Text="Show All Records"
                                                            Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red"></asp:LinkButton>
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
                                                            Text="Add New" ToolTip="Click to Add Inspections" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Visible="false" TabIndex="0" Text="Print" ToolTip="Click to print List of Assembly Inspection" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close List of Assembly Inspection Status screen" />
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
                                            <asp:Button ID="hdnBtnInspectionHistory" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnAssemblyInspectionListNew" ClientIDMode="Static" runat="server"
                                                Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnModelInspMaster" ClientIDMode="Static" runat="server"
                                                Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
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
        <!--Inspection History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyInspectionHistory" Text="Inspection History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlInspectionHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeInspectionHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupInspectionHistory" runat="server" TargetControlID="btnDummyInspectionHistory"
            PopupControlID="pnlInspectionHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameInspectionHistoryStateComplete() {
                $("#btnDummyInspectionHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenInspectionHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeInspectionHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorInspStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyInspectionHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForInspectionHistory() {
                var InspectionHistorywindow = $find("<%=mdlPopupInspectionHistory.ClientID %>");
                //close Inspection History popup window
                InspectionHistorywindow.hide();
                //           release resources
                $("#IframeInspectionHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnInspectionHistory").click();
            }
        </script>
        <!-- End-->
        <%--Date Validations--%>
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': false };
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
        <!--End-->
        <!--Assembly Inspection List New Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAssemblyInspectionListNew" Text="Assembly Inspection List New"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAssemblyInspectionListNew" ClientIDMode="Static"
            HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IframeAssemblyInspectionListNew" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAssemblyInspectionListNew" runat="server" TargetControlID="btnDummyAssemblyInspectionListNew"
            PopupControlID="pnlAssemblyInspectionListNew" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAssemblyInspectionListNewStateComplete() {
                $("#btnDummyAssemblyInspectionListNew").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenAssemblyInspectionListNewWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAssemblyInspectionListNew").attr("src", "wfAssemblyMonitorInspStatusListNew_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAssemblyInspectionListNew").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForAssemblyInspectionListNew() {
                var AssemblyInspectionListNewwindow = $find("<%=mdlPopupAssemblyInspectionListNew.ClientID %>");
                //close Assembly Inspection List New popup window
                AssemblyInspectionListNewwindow.hide();
                //           release resources
                $("#IframeAssemblyInspectionListNew").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnAssemblyInspectionListNew").click();
            }
        </script>
        <!-- End-->
        <%--'Added by Saylee on 27-Jul-2023, to give Revise on comply list page--%>
        <!--Model Insp Master Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModelInspMaster" Text="Model Insp Master"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlModelInspMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModelInspMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModelInspMaster" runat="server" TargetControlID="btnDummyModelInspMaster"
            PopupControlID="pnlModelInspMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModelInspMasterStateComplete() {
                $("#btnDummyModelInspMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModelInspMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModelInspMaster").attr("src", "wfModelMonitorInspection_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

                    if (!$.browser.msie) {
                        $("#btnDummyModelInspMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModelInspMaster() {
                var ModelInspMasterwindow = $find("<%=mdlPopupModelInspMaster.ClientID %>");
                //close Model Insp Master popup window
                ModelInspMasterwindow.hide();
                //           release resources
                $("#IframeModelInspMaster").attr("src", "JavaScript:''");
                //call Model Insp Master image button
                $("#hdnBtnModelInspMaster").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
