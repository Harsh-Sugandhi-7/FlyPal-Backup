<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemovedAssemblyList_Ajax.aspx.vb"
    Inherits="Flypal.wfRemovedAssemblyList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Assembly Installation</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

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
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <span id="lbltitle" class="clstitle1">Assembly Installation</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdswodetail" class="clsFieldSet" style="border-width: 1px;">
                                                        <legend id="ldwodetail" class="clsFieldSet1"><b>Assembly Search Information</b></legend>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblInstallationDate" class="clsLabelAuto">Date</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtInstallationDate" CssClass="clsTextBox_Ajax" Width="80px"
                                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'InstallationDate_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtInstallationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInstallationDate">
                                                                                </cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtInstallationDate" ID="InstallationDate_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox">
                                                                                </cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblAircraft" class="clsLabel">Aircraft </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                                    Width="100px" DataTextField="RegNo" AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblModel" class="clsLabelAuto">Assembly</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbAircraftAssembly" runat="server" CssClass="clsComboBox_Ajax"
                                                                                    AutoPostBack="true" DataValueField="ID" DataTextField="ModelSerialNoPostion">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkSpareAssembly" runat="server" Text="Install Stock Assembly" Visible="false"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-left: 4px" colspan="6">
                                                                                <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                                    Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Assembly as per searching criteria"
                                                                                Text="Find Now" ValidationGroup="2" Visible="False"></asp:Button>
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
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlInstallAssembly" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <%--<fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; z-index: 10000;">
                                            <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Installation Information</b></legend>--%>
                                        <table id="Table3">
                                            <tr>
                                                <td>
                                                    <span id="lblInstallOn" class="clsLabel">Install On </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbInstallOnMachine" runat="server" CssClass="clsComboBox_Ajax"
                                                        AutoPostBack="true" Width="100px" DataValueField="ID" DataTextField="RegNo">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                        Width="150px" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" TabIndex="0" Text="Install"
                                                        ToolTip="Click to Install the Assembly" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="left">
                                                    <asp:Label ID="lblReadOnlyInstalledOn" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                        Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                        <%--</fieldset>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlRemovedAssemblyList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRemovedAssembly" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgRemovedList" runat="server" AllowSorting="True" CssClass="clsGrid"
                                                        ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" DataKeyNames="AssemblyStatusID"
                                                        OnRowDataBound="dgRemovedList_RowDataBound">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="AssemblyStatusID" HeaderText="AssemblyStatusID">
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Reg No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemovedOnFormatted" SortExpression="RemovedOnFormatted"
                                                                HeaderText="Removed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HtmlEncode="false"
                                                                HeaderText="Period">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ValueFormatted" SortExpression="ValueFormatted" HtmlEncode="false"
                                                                HeaderText="Value">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TSOFormatted" SortExpression="TSOFormatted" HtmlEncode="false"
                                                                HeaderText="TSO">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Install Selected" ValidationGroup="1" CausesValidation="true"
                                                                HeaderText="Install Selected" CommandName="InstallSelected">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrintRemovedAssembly" runat="server" CssClass="clsButton_Ajax"
                                                        Visible="false" ToolTip="Click to print List of Removed Assembly" Text="Print"
                                                        CausesValidation="False" Enabled="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlInstalledAssembly" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblInstalledAssembly" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgInstalledList" runat="server" AllowSorting="True" CssClass="clsGrid"
                                                        ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" OnRowDataBound="dgInstalledList_RowDataBound">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Reg No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InstalledOnFormatted" SortExpression="InstalledOnFormatted"
                                                                HeaderText="Installed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PeriodNameForWeb" HtmlEncode="false" SortExpression="PeriodNameForWeb"
                                                                HeaderText="Period ">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ValueFormatted" HtmlEncode="false" SortExpression="ValueFormatted"
                                                                HeaderText="Value">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TSNFormatted" HtmlEncode="false" SortExpression="TSNFormatted"
                                                                HeaderText="TSN">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TSOFormatted" HtmlEncode="false" SortExpression="TSOFormatted"
                                                                HeaderText="TSO">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Revert Installation" HeaderText="Revert Installation" CommandName="RevertInstallation">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:ButtonField>
                                                            <asp:ButtonField HeaderText="Edit" Text="Edit" CommandName="EditRec"></asp:ButtonField>
                                                            <asp:ButtonField HeaderText="History" Text="History" CommandName="History"></asp:ButtonField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="AssemblyTypeID" HeaderText="AssemblyTypeID"></asp:BoundField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="IsMaster" HeaderText="IsMaster"></asp:BoundField>
                                                            <asp:ButtonField HeaderText="View" Text="View" CommandName="View" />
                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrintInstalledAssembly" runat="server" CssClass="clsButton_Ajax"
                                                        Visible="false" ToolTip="Click to print List of Installed Assembly" Text="Print"
                                                        CausesValidation="False" Enabled="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Removed Assembly screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
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
                                        <asp:Button ID="hdnBtnInstallationHistory" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnSpareAssemblyInstallList" ClientIDMode="Static" runat="server"
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
    <!-- Installation History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyInstallationHistory" Text="Installation History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlInstallationHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeInstallationHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupInstallationHistory" runat="server" TargetControlID="btnDummyInstallationHistory"
        PopupControlID="pnlInstallationHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameInstallationHistoryStateComplete() {
            $("#btnDummyInstallationHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenInstallationHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeInstallationHistory").attr("src", "wfUpdateInstalledAssemblyHistory_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyInstallationHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForInstallationHistory() {
            var InstallationHistorywindow = $find("<%=mdlPopupInstallationHistory.ClientID %>");
            //close Installation History popup window
            InstallationHistorywindow.hide();
            //           release resources
            $("#IframeInstallationHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnInstallationHistory").click();
        }
    </script>
    <!-- End-->
    <!--Spare Assembly Install List Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySpareAssemblyInstallList" Text="Assembly Inspection List New"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSpareAssemblyInstallList" ClientIDMode="Static"
        HorizontalAlign="Center" Style="height: 100%; width: 100%;">
        <iframe id="IframeSpareAssemblyInstallList" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSpareAssemblyInstallList" runat="server" TargetControlID="btnDummySpareAssemblyInstallList"
        PopupControlID="pnlSpareAssemblyInstallList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSpareAssemblyInstallListStateComplete() {
            $("#btnDummySpareAssemblyInstallList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSpareAssemblyInstallListWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSpareAssemblyInstallList").attr("src", "wfSpareAssemblyListForInstallation_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySpareAssemblyInstallList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSpareAssemblyInstallList() {
            var SpareAssemblyInstallListwindow = $find("<%=mdlPopupSpareAssemblyInstallList.ClientID %>");
            //close Assembly Inspection List New popup window
            SpareAssemblyInstallListwindow.hide();
            //           release resources
            $("#IframeSpareAssemblyInstallList").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSpareAssemblyInstallList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
