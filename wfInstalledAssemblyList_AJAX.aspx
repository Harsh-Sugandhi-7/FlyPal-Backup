<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfInstalledAssemblyList_AJAX.aspx.vb"
    Inherits="Flypal.wfInstalledAssemblyList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Installed Assembly List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css"    />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
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
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">Assembly Removal</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="1" valign="top">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="Legend1" runat="server"><b>Removal Information</b></legend>
                                                        <table id="Table3">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRemovalDate" runat="server" Width="94px" CssClass="clsLabelAuto">Removal Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="calDate" CssClass="clsTextBox_Ajax" Width="90px"
                                                                        AutoPostBack="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                        TabIndex="1"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdswodetail" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="ldwodetail" runat="server"><b>Search Criteria</b></legend>
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbMachine" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                        AutoPostBack="true" DataTextField="RegNo" Width="100px" TabIndex="2">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAircraftAssembly" runat="server" CssClass="clsComboBox_Ajax"
                                                                        AutoPostBack="true" DataValueField="ID" DataTextField="ModelSerialNoPostion">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="right" colspan="1">
                                                                    <table id="Table4" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="btnFindNow" TabIndex="4" runat="server" CssClass="clsButton_Ajax"
                                                                                            ToolTip="Click to find list of Assembly as per searching criteria" Text="Find Now"
                                                                                            Visible="False"></asp:Button>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                               <tr>
                                                                <td>
                                                                </td>
                                                                <td style="padding-left: 4px" colspan="6">
                                                                    <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                        Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlInstalledAssemblyHeader" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblInstalledAssemblyList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpnlInstalledAssemblyList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgInstalledAssemblyList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            CssClass="clsGrid" DataKeyNames="AssemblyStatusID" ShowHeaderWhenEmpty="True"
                                            TabIndex="5" OnRowDataBound="dgInstalledAssemblyList_RowDataBound">
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:BoundField DataField="AssemblyStatusID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PeriodNameForweb" HeaderText="Period" SortExpression="PeriodNameForweb"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ValueFormatted" HeaderText="Value" SortExpression="ValueFormatted"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TSNFormatted" HeaderText="TSN" SortExpression="TSNFormatted"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TSOFormatted" HeaderText="TSO" SortExpression="TSOFormatted"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:ButtonField CommandName="DeleteRec" HeaderText="Remove" Text="Remove">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlPrintInstalledAssemblyList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrintInstalledAssemblyList" runat="server" CssClass="clsButton_Ajax" Visible="false"
                                                        Enabled="False" TabIndex="6" Text="Print" ToolTip="Click to print List of Installed Assembly" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlRemovedAssemblyHeader" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRemovedAssemblyList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlRemovedAssemblyList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgRemovedAssemblyList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            CssClass="clsGrid" DataKeyNames="AssemblyStatusID" ShowHeaderWhenEmpty="True"
                                            TabIndex="7" OnRowDataBound="dgRemovedAssemblyList_RowDataBound">
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:BoundField DataField="AssemblyStatusID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info" SortExpression="AssemblyInfo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On" HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PeriodNameForweb" HeaderText="Period" SortExpression="PeriodNameForweb"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ValueFormatted" HeaderText="Value" SortExpression="ValueFormatted"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TSOFormatted" HeaderText="TSO" SortExpression="TSOFormatted"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:ButtonField CommandName="RevertRemoval" HeaderText="Revert Removal" Text="Revert Removal">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="History" HeaderText="History" Text="History">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                    HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                    <ItemStyle CssClass="hideGridColumn" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnPrintRemovedAssemblyList" runat="server" CssClass="clsButton_Ajax" Visible="false"
                                                        Enabled="False" TabIndex="8" Text="Print" ToolTip="Click to Print List of Removed Assembly" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="9" Text="Close" ToolTip="Click to close List of Installed Assembly screen" />
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
                                        <asp:Button ID="hdnBtnRemHistory" ClientIDMode="Static" runat="server" Text="Add"
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
    <!-- Removal History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemHistory" Text="TaskCard Tool" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemHistory" runat="server" TargetControlID="btnDummyRemHistory"
        PopupControlID="pnlRemHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRemHistoryStateComplete() {
            $("#btnDummyRemHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenRemHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemHistory").attr("src", "wfUpdateRemovedAssemblyHistory_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRemHistory() {
            var RemHistorywindow = $find("<%=mdlPopupRemHistory.ClientID %>");
            //close Removal History popup window
            RemHistorywindow.hide();
            //           release resources
            $("#IframeRemHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnRemHistory").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
