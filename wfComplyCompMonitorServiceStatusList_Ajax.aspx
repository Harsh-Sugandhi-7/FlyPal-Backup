<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyCompMonitorServiceStatusList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfComplyCompMonitorServiceStatusList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Component Service Status List</title>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">List of Component Service Status</asp:Label>
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
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbSpareComponent" GroupName="a" runat="server" Checked="true"
                                                                            AutoPostBack="true" Text="Show Services of Stock Component"></asp:RadioButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbRemovedComp" GroupName="a" runat="server" AutoPostBack="true"
                                                                            Text="Show Services of Removed Component"></asp:RadioButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbSpareAssemblyComponent" GroupName="a" AutoPostBack="true"
                                                                            runat="server" Text="Show Services of Components on Stock Assembly"></asp:RadioButton>
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
                                                                            Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                        </cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox">
                                                                        </cc2:TextBoxWatermarkExtender>
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
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px">
                                                                    <span id="lblMonitorType" runat="server"  class="clsLabelAuto">Task Type</span>
                                                                </td>
                                                                <td style="width: 260px">
                                                                    <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBox3_Ajax"
                                                                        Width="185px" DataTextField="PartMonitorServiceTypeName" DataValueField="ID"
                                                                        AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 80px">
                                                                    <span id="lblCodeFormNo" runat="server"  class="clsLabelAuto">Code/Form No./Description</span>
                                                                </td>
                                                                <td style="width: 260px">
                                                                    <asp:TextBox runat="server" ID="txtCodeFormNo" CssClass="clsTextBox_Ajax" Width="250px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  records'
                                                                        Text='Show ONLY "NOT  APPLICABLE" records' AutoPostBack="True"></asp:CheckBox>
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
                                                                            Text="Add New" ToolTip="Click to Add" ValidationGroup="1" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            Visible="false" TabIndex="0" Text="Print" ToolTip="Click to print List" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close List" />
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
                                                                    PageSize="5" ShowHeaderWhenEmpty="true" EnableViewState="true" CssClass="clsGrid"
                                                                    OnRowDataBound="dgDueMonitoringList_RowDataBound">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                       <asp:BoundField DataField="TaskNo" SortExpression="TaskNo" HeaderText="Task No.">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                        <asp:BoundField DataField="Reference" HeaderText="Reference Doc." SortExpression="Reference">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Aircraft Info." SortExpression="RegNo"
                                                                            Visible="False">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Assembly Type" SortExpression="RegNo"
                                                                            Visible="False">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Assembly Info." SortExpression="RegNo"
                                                                            Visible="False">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CompInfo" HeaderText="Comp. Info." SortExpression="CompInfo"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="MonitorTypeCode" HeaderText="Task Type" SortExpression="MonitorTypeCode">
                                                                            <HeaderStyle ForeColor="White" Width="5px" HorizontalAlign="Left" />
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
                                                                        <asp:BoundField DataField="DoneOnDate" HeaderText="Compliance Date">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
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
                                                                        <asp:BoundField DataField="Freq3ForGrid" HeaderText="Threshold" SortExpression="Freq3ForGrid"
                                                                            HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="White" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DoneAt2ForGrid" HeaderText="Effective From/Compliance Value"
                                                                            HtmlEncode="false" SortExpression="DoneAt2ForGrid">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="SinceNewTSNCSN" HeaderText="Current" HtmlEncode="false"
                                                                            SortExpression="SinceNewTSNCSN">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ElapsedValue" HeaderText="Elapsed" HtmlEncode="false"
                                                                            SortExpression="ElapsedValue">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" HtmlEncode="false"
                                                                            SortExpression="ExtensionValueFormatted">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DueAtTimeForCompliancePage" HeaderText="Due At." HtmlEncode="false"
                                                                            SortExpression="DueAtTimeForCompliancePage">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DueAsOf2ForGrid" HeaderText="Due At Airframe" HtmlEncode="false"
                                                                            SortExpression="DueAsOf2ForGrid" Visible="false">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RemainingTimeForCompliancePage" HtmlEncode="false" HeaderText="Remaining"
                                                                            SortExpression="RemainingTimeForCompliancePage">
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
                                                        ValidationGroup="1" Text="Add New" ToolTip="Click to Add" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Visible="false" TabIndex="0" Text="Print" ToolTip="Click to print" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close List screen" />
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
                                        <asp:Button ID="hdnBtnCompServiceHistory" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnCompServiceListNew" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
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
    <!--Comp Service History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCompServiceHistory" Text="Comp Service History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCompServiceHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCompServiceHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCompServiceHistory" runat="server" TargetControlID="btnDummyCompServiceHistory"
        PopupControlID="pnlCompServiceHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCompServiceHistoryStateComplete() {
            $("#btnDummyCompServiceHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCompServiceHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCompServiceHistory").attr("src", "wfUpdateComplyHistoryCompMonitorServiceStatusList_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCompServiceHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCompServiceHistory() {
            var CompServiceHistorywindow = $find("<%=mdlPopupCompServiceHistory.ClientID %>");
            //close Comp Service History popup window
            CompServiceHistorywindow.hide();
            //           release resources
            $("#IframeCompServiceHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCompServiceHistory").click();
        }
    </script>
    <!-- End-->
    <!--Comp Service List New Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCompServiceListNew" Text="Comp Service List New"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCompServiceListNew" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCompServiceListNew" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCompServiceListNew" runat="server" TargetControlID="btnDummyCompServiceListNew"
        PopupControlID="pnlCompServiceListNew" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCompServiceListNewStateComplete() {
            $("#btnDummyCompServiceListNew").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCompServiceListNewWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCompServiceListNew").attr("src", "wfCompMonitorServiceStatusListNew_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCompServiceListNew").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCompServiceListNew() {
            var CompServiceListNewwindow = $find("<%=mdlPopupCompServiceListNew.ClientID %>");
            //close Comp Service List New popup window
            CompServiceListNewwindow.hide();
            //           release resources
            $("#IframeCompServiceListNew").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCompServiceListNew").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
