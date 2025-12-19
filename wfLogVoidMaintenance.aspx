<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogVoidMaintenance.aspx.vb"
    Inherits="Flypal.wfLogVoidMaintenance" %>


<%@ Import Namespace="Flypal.LogList" %>
<%@ Import Namespace="Flypal.Log" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Maintenance / Void Log</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openLedgerInSameWindow(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="JavaScript" type="text/javascript">
        /*Fuel Oil*/
        function autoResizeFuelOil() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeFuelOil').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeFuelOil').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeFuelOil').height = (newheight + 2) + "px";
            document.getElementById('IframeFuelOil').width = (newwidth) + "px";
            document.getElementById('tbpnlFuelOil').height = (newheight) + "px";
            document.getElementById('tbpnlFuelOil').width = (newwidth) + "px";

            document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
            document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";


        }
        function CallFuelOil() {
            document.getElementById('IframeFuelOil').src = 'wfLogFuelOil_Ajax.aspx?Type=pup';
        }

        /*Snag Reporting*/
        function autoResizeSnagReporting() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeSnagReporting').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeSnagReporting').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeSnagReporting').height = (newheight + 2) + "px";
            document.getElementById('IframeSnagReporting').width = (newwidth) + "px";
            document.getElementById('tbpnlSnagReporting').height = (newheight) + "px";
            document.getElementById('tbpnlSnagReporting').width = (newwidth) + "px";

            document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
            document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";


        }
        function CallSnagReporting() {
            document.getElementById('IframeSnagReporting').src = 'wfLogDefectActionList_Ajax.aspx?Type=pup';
        }

        /*Maint Activity*/
        function autoResizeMaintActivity() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeMaintActivity').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMaintActivity').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeMaintActivity').height = (newheight + 2) + "px";
            document.getElementById('IframeMaintActivity').width = (newwidth) + "px";
            document.getElementById('tbpnlMaintActivity').height = (newheight) + "px";
            document.getElementById('tbpnlMaintActivity').width = (newwidth) + "px";

            document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
            document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";


        }
        function CallMaintActivity() {
            document.getElementById('IframeMaintActivity').src = 'wfLogMaintenanceActivity_Ajax.aspx?Type=pup';
        }

        function autoResizeDeferredDiscrepancy() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeDeferredDiscrepancy').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeDeferredDiscrepancy').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeDeferredDiscrepancy').height = (newheight + 10) + "px";
            document.getElementById('IframeDeferredDiscrepancy').width = (newwidth + 20) + "px";
            document.getElementById('tbpnlDeferredDiscrepancies').height = (newheight + 10) + "px";
            document.getElementById('tbpnlDeferredDiscrepancies').width = (newwidth + 10) + "px";
            document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
            document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";
        }

        function callDeferredDiscrepancies() {
            document.getElementById('IframeDeferredDiscrepancy').src = 'wfDiscrepancyCorrectiveActionListFromLog.aspx?Type=pup&Troubleshoot=1';
        }

        function autoResizeDiscrepancyReporting() {

            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeDiscrepancyReporting').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeDiscrepancyReporting').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeDiscrepancyReporting').height = (newheight) + "px";
            document.getElementById('IframeDiscrepancyReporting').width = (newwidth + 120) + "px";
            document.getElementById('tbpnlDiscrepancyReporting').height = (newheight) + "px";
            document.getElementById('tbpnlDiscrepancyReporting').width = (newwidth) + "px";

            document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
            document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";



            // below line added by Saylee on 8-Oct-2024 as height was not showing as per required, dont remove
            document.getElementById('IframeDiscrepancyReporting').height = (document.getElementById('IframeDiscrepancyReporting').contentWindow.document.body.scrollHeight) + "px";//  alert(document.getElementById('IframeDiscrepancyReporting').contentWindow.document.body.scrollHeight);

        }
        function callDiscrepancyReporting() {
            document.getElementById('IframeDiscrepancyReporting').src = 'wfDiscrepancyCorrectiveActionListFromLog.aspx?Type=pup&Troubleshoot=0';
        }


    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <script language="JavaScript" type="text/javascript">
            function CloseChildPage() {
                $find('<%=tabLogDetailsContainer.ClientID%>').set_activeTabIndex(0);
            }
        </script>
        <table id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server">

                        <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%" style="margin-top: -5px">
                                    <asp:PlaceHolder runat="server" Visible="false">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblLogDetails" runat="server"
                                                                CssClass="clsLabelButton" ToolTip="Log details">Log details</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnFuelOil" runat="server"
                                                                CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                                Text="Fuel Oil" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDefectActionList" runat="server"
                                                                CssClass="clsButtonLong_Ajax"
                                                                CausesValidation="False"
                                                                Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True",
                                                                "Defect Reporting",
                                                                "Snag Reporting") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnMaintenanceAcitvity"
                                                                runat="server" CssClass="clsButtonLong_Ajax"
                                                                CausesValidation="False"
                                                                Text="Maintenance Activity" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <tr>
                                        <td>
                                            <cc2:TabContainer ID="tabLogDetailsContainer" runat="server" class="clstablelistin"
                                                AutoPostBack="true">
                                                <cc2:TabPanel ID="tabLogDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="Maintenance / Void Log Details" ID="lblPanelHeader"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="clsFormHeader1Newstyle">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="width: 99%" valign="middle">
                                                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblTitle" runat="server" 
                                                                                            CssClass="clsFormHeader">Maintenance / Void Log</asp:Label>
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
                                                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary"
                                                                                runat="server" HeaderText="Fill Up The Following Fields" />
                                                                            <asp:CustomValidator ID="cvRemark" runat="server"
                                                                                ErrorMessage="Remark Can't be greater than 200 chars"
                                                                                ControlToValidate="txtRemark" Display="None" />
                                                                            <asp:CustomValidator ID="cvTLPNo" runat="server" ErrorMessage="Enter TLP No."
                                                                                ControlToValidate="txtLogPageNo" Display="None" OnServerValidate="customvalidate" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblLogNo" runat="server" CssClass="clsLabelAuto">Log No.</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <table cellpadding="1" cellspacing="2">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtLogText" runat="server" 
                                                                                                        BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                                                        Width="130px" ReadOnly="True" 
                                                                                                        Text="<%# mLog.LogText %>" 
                                                                                                        ToolTip="Log Number" />

                                                                                                    <asp:TextBox ID="txtLogNo" runat="server" 
                                                                                                        BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                                                                        ReadOnly="True" Text="<%# mLog.LogNo %>" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblLogPageNo" runat="server" 
                                                                                                        CssClass="clsLabelAuto">Log Page No. </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtLogPageNo" runat="server" 
                                                                                                        CssClass="clsTextBoxTagSearchSmall" MaxLength="9"
                                                                                                        Text="<%# mLog.LogPageNoFormatted %>" 
                                                                                                        ToolTip="Enter Log Page No." />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblLogDate" runat="server" 
                                                                                                        class="clsLabelAuto" Visible="<%# mLog.LogTypeID = 2 %>"
                                                                                                        Text='<%# IIf(mLog.IsUTC = True, "Log Date (UTC)", "Log Date") %>'>Log Date </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtLogDate" runat="server" 
                                                                                                        CssClass="clsTextBoxTagSearchDate" 
                                                                                                        AutoPostBack="true"
                                                                                                        onchange="ValidateDateText(this,'LogDate_watermarkextender');" 
                                                                                                        OnTextChanged="txtLogDate_TextChanged"
                                                                                                        Visible="<%# mLog.LogTypeID = 2 %>" />

                                                                                                    <cc2:CalendarExtender ID="txtLogDate_CalendarExtender" 
                                                                                                        runat="server" CssClass="cal_Theme1"
                                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" 
                                                                                                        TargetControlID="txtLogDate" />

                                                                                                    <cc2:TextBoxWatermarkExtender ID="LogDate_watermarkextender"
                                                                                                        runat="server" ClientIDMode="Static"
                                                                                                        TargetControlID="txtLogDate" WatermarkCssClass="clsDateTextBox"
                                                                                                        WatermarkText="<%$AppSettings:DateFormat%>" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <br />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:UpdatePanel ID="upnlAPUDetail" runat="server" UpdateMode="Conditional">
                                                                                            <%--Add UpdatePanel for APU Grid--%>
                                                                                            <ContentTemplate>
                                                                                                <div style="width: 100%">
                                                                                                    <asp:Label ID="lblAPUPeriod" runat="server" CssClass="clsLabelHeader">APU Period</asp:Label>
                                                                                                </div>
                                                                                                <div style="width: 100%">
                                                                                                    <asp:GridView ID="dgAPUPeriods" runat="server" AutoGenerateColumns="False"
                                                                                                        Width="100%" CellPadding="5" ForeColor="Black" GridLines="Horizontal"
                                                                                                        BorderStyle="Solid" CssClass="clsGridNewStyle"
                                                                                                        AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
                                                                                                        SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
                                                                                                        PagerSettings-Mode="NextPreviousFirstLast">
                                                                                                        <RowStyle CssClass="clsdgItem" />
                                                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
                                                                                                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                                                                                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUHours" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>' ToolTip="Enter the Hours."
                                                                                                                        Width="93%" AutoPostBack="true" OnTextChanged="txtAPUHours_TextChanged" 
                                                                                                                        onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" /> 
                                                                                                                        <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>                                                                                                                    
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPULandings" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>' ToolTip="Enter the Landing."
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPULandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>' ToolTip="Enter Cycles."
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUStarts" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>' ToolTip="Enter Start Time."
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUNGCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>' ToolTip="Enter NG Cycles"
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUNFCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>' ToolTip="Enter NF Cycles"
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPURins" runat="server" CssClass="clsTextBoxTagSearchSmall" Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>'
                                                                                                                        ToolTip="Enter RINS." AutoPostBack="true" OnTextChanged="txtAPURins_TextChanged"
                                                                                                                        onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                                                                        Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUBleeds" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>' ToolTip="Enter Bleeds"
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUImpellerCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>' ToolTip="Enter Impeller Cycles"
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUImpellerCycles_TextChanged" Width="93%"
                                                                                                                        onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                                                                                    </asp:TextBox>
                                                                                                                    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUCTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>' ToolTip="Enter CT Cycles"
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUPTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>' ToolTip="Enter PT Cycles"
                                                                                                                        AutoPostBack="true" OnTextChanged="txtAPUPTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                                                        onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                    </asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtAPUGeneratorMods" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                                        Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
                                                                                                                        ToolTip="Enter the Generator Mods." AutoPostBack="true" OnTextChanged="txtAPUGeneratorMods_TextChanged"
                                                                                                                        onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                                                                                    </asp:TextBox>
                                                                                                                    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                                                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField HeaderText=""></asp:BoundField>
                                                                                                        </Columns>
                                                                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <%--AJAX- Add UpdatePanel for Remark--%>
                                                                                        <asp:UpdatePanel ID="upnlRemark" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchLong"
                                                                                                    MaxLength="500" Text="<%# mLog.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark"
                                                                                                    Width="613px" Height="60px" />
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span id="lblAttachFile" class="clsLabelAuto">Attach File</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <input type="button" id="btnSelectFile"
                                                                                                                value="Select File" style="width: 120px;"
                                                                                                                runat="server" class="clsbtnH clsinfoH1"
                                                                                                                causesvalidation="False" tabindex="13" />
                                                                                                        </td>
                                                                                                        <td style="padding-left: 3px;">
                                                                                                            <asp:Button ID="btnDelAttch" runat="server"
                                                                                                                CssClass="clsbtnH clsinfoH1" ToolTip="Remove the attachment added."
                                                                                                                Text="Remove Attachment" Enabled="False"
                                                                                                                Width="160px" TabIndex="14" />
                                                                                                        </td>
                                                                                                        <td style="padding-left: 2px;">
                                                                                                            <asp:ImageButton ID="ImageButton1" runat="server"
                                                                                                                CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                                                Height="20px" Width="20px" TabIndex="15" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="height: 0px;">
                                                                                    <td style="height: 0px;">
                                                                                        <asp:UpdatePanel ID="upnlImgBtn" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="----" />
                                                                                                <asp:Button ID="hdnBtnLogFuelOil" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="----" />
                                                                                                <asp:Button ID="hdnBtnLogDefectAction" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="----" />
                                                                                                <asp:Button ID="hdnBtnLogMaintenanceActivity" runat="server" CausesValidation="False"
                                                                                                    ClientIDMode="Static" Style="display: none;" Text="----" />
                                                                                                <asp:Button ID="hdnBtnDiscrepancyTroubleShoot1"
                                                                                                    runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;"
                                                                                                    Text="Add" />
                                                                                                <asp:Button ID="hdnBtnDiscrepancyDetail"
                                                                                                    runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;"
                                                                                                    Text="Add" />
                                                                                                <asp:Button ID="Button1" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                                    Style="display: none;" Text="----" />
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
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
                                                                            <table id="Table1" cellpadding="1" cellspacing="1">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to Save" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                                                            Text="Back" ToolTip="Click to go Back to Previous Page" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlFuelOil" runat="server" Visible="<%# Not mLog.IsNew %>" CssClass="clsPanel1" ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="Fuel Oil" ID="Label2"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlFuelOil" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeFuelOil" width="100%" height="200px" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeFuelOil()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlSnagReporting" runat="server" Visible='<%# Not mLog.IsNew And AppSettings("ShowNewDiscrepancyFlow") = "False" %>' CssClass="clsPanel1" ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect Reporting", "Snag Reporting") %>' ID="Label3"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlSnagReporting" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeSnagReporting" width="100%" height="200px" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeSnagReporting()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlMaintActivity" runat="server" Visible="<%# Not mLog.IsNew %>" CssClass="clsPanel1" ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="Maintenance Activity" ID="Label4"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlMaintActivity" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeMaintActivity" width="100%" height="200px" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeMaintActivity()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>

                                                <cc2:TabPanel ID="tbpnlDiscrepancyReporting" runat="server"
                                                    Visible='<%# Not mLog.IsNew And AppSettings("ShowNewDiscrepancyFlow") = "True" %>'
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="Discrepancy Reporting" ID="Label10"></asp:Label>
                                                    </HeaderTemplate>

                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeDiscrepancyReporting" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeDiscrepancyReporting()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>

                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlDeferredDiscrepancies" runat="server" ClientIDMode="Static" Visible='<%# AppSettings("ShowNewDiscrepancyFlow") = "True"  %>' CssClass="clsPanel1">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="Deferred Discrepancies" ID="lblHeaderDeferredDiscrepancies"></asp:Label>
                                                    </HeaderTemplate>

                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeDeferredDiscrepancy" width="100%" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeDeferredDiscrepancy();"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>

                                                </cc2:TabPanel>
                                            </cc2:TabContainer>

                                        </td>

                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>

        <!-- Ajax Loader -->
        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

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

            $(document).ready(function () {
                $("#btnSelectFile").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                        if (!$.browser.msie) {
                            $("#btnDummyFileUpload").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }
                });
            });
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
        <!-- Log Fuel Oil Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyLogFuelOil" Text="Log Fuel Oil" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlLogFuelOil" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeLogFuelOil" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupLogFuelOil" runat="server" TargetControlID="btnDummyLogFuelOil"
            PopupControlID="pnlLogFuelOil" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameLogFuelOilStateComplete() {
                $("#btnDummyLogFuelOil").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLogFuelOilWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeLogFuelOil").attr("src", "wfLogFuelOil_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyLogFuelOil").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForLogFuelOil() {
                var LogFuelOilwindow = $find("<%=mdlPopupLogFuelOil.ClientID %>");
                //close Log Fuel Oil popup window
                LogFuelOilwindow.hide();
                //           release resources
                $("#IframeLogFuelOil").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnLogFuelOil").click();
                CloseChildPage();
            }
        </script>
        <!-- End-->
        <!-- Log Maintenance Activity Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyLogMaintenanceActivity" Text="Log Fuel Oil"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlLogMaintenanceActivity" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeLogMaintenanceActivity" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupLogMaintenanceActivity" runat="server" TargetControlID="btnDummyLogMaintenanceActivity"
            PopupControlID="pnlLogMaintenanceActivity" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameLogMaintenanceActivityStateComplete() {
                $("#btnDummyLogMaintenanceActivity").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLogMaintenanceActivityWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeLogMaintenanceActivity").attr("src", "wfLogMaintenanceActivity_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyLogMaintenanceActivity").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForLogMaintenanceActivity() {
                var LogMaintenanceActivitywindow = $find("<%=mdlPopupLogMaintenanceActivity.ClientID %>");
                //close Log Fuel Oil popup window
                LogMaintenanceActivitywindow.hide();
                //           release resources
                $("#IframeLogMaintenanceActivity").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnLogMaintenanceActivity").click();
                CloseChildPage();
            }
        </script>
        <!-- End-->
        <%--Date Validations--%>
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': true };
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForVoidLog();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameVoidLogStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': 0 + 'px' });
                }

            }
        </script>
        <!-- Log Defect Action Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyLogDefectAction" Text="Log Fuel Oil" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlLogDefectAction" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeLogDefectAction" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupLogDefectAction" runat="server" TargetControlID="btnDummyLogDefectAction"
            PopupControlID="pnlLogDefectAction" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameLogDefectActionStateComplete() {
                $("#btnDummyLogDefectAction").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLogDefectActionWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeLogDefectAction").attr("src", "wfLogDefectActionList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyLogDefectAction").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForLogDefectAction() {
                var LogDefectActionwindow = $find("<%=mdlPopupLogDefectAction.ClientID %>");
                //close Log Fuel Oil popup window
                LogDefectActionwindow.hide();
                //           release resources
                $("#IframeLogDefectAction").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnLogDefectAction").click();
                CloseChildPage();
            }
        </script>
        <!-- End-->



        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDiscrepancyTroubleShoot" Text="Discrepancy TroubleShoot" ClientIDMode="Static" />
        </div>

        <asp:Panel runat="server" ID="pnlDiscrepancyTroubleShoot" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeDiscrepancyTroubleShoot" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupDiscrepancyTroubleShoot" runat="server" TargetControlID="btnDummyDiscrepancyTroubleShoot"
            PopupControlID="pnlDiscrepancyTroubleShoot" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>

        <script type="text/javascript">
            function IframeDiscrepancyTroubleShootStateComplete() {
                $("#btnDummyDiscrepancyTroubleShoot").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenDiscrepancyTroubleShootWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeDiscrepancyTroubleShoot").attr("src", "wfDiscrepancyTroubleshoot.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyDiscrepancyTroubleShoot").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForDiscrepancyTroubleShoot() {
                var DiscrepancyTroubleShootwindow = $find("<%=mdlPopupDiscrepancyTroubleShoot.ClientID %>");
                //close kit popup window
                DiscrepancyTroubleShootwindow.hide();
                //           release resources
                $("#IframeDiscrepancyTroubleShoot").attr("src", "JavaScript:''");
                //call kit image button
                $("#hdnBtnDiscrepancyTroubleShoot1").click();
                CloseChildPage();
            }
        </script>

        <script type="text/javascript">
            function delete_cookie() {
                //   $.cookie('HideInfoMessagepanel', false);

            }
            function ShowLastDet() {

                $pos = $("#<%=txtLogNo.ClientID%>").position();
                var top = $pos.top;
                var left = $pos.left;
                var searchHeight = $("#<%=txtLogNo.ClientID%>").height();
                var margin = top + searchHeight;

                var height = $("#tblmain").outerHeight();
                var h = margin - height;


                $("#InfoMessagepanel").css("display", "block");
                $("#InfoMessagepanel").animate({ marginTop: h, marginLeft: left - 5 }, 1000, 'swing', function () {
                    $("#InfoMessagepanel").delay(10000).fadeOut();

                });
            }

        </script>
        <div id="InfoMessagepanel" class="clsInfoMessage1" style="display: none; z-index: 100"
            draggable="true">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlLogInfo">
                <ContentTemplate>
                    <asp:GridView ID="grdLogDet" runat="server" AutoGenerateColumns="False" Width="100%"
                        CellPadding="5" ForeColor="Black" GridLines="Horizontal" CssClass="clsGridNewStyle" AlternatingRowStyle-CssClass="alt"
                        RowStyle-Wrap="false" HeaderStyle-Wrap="false" SelectedRowStyle-BackColor="ButtonShadow"
                        ShowHeaderWhenEmpty="True" PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
                        <RowStyle CssClass="clsdgItem" />
                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="LogTextNo" HeaderText="Log No."></asp:BoundField>
                            <asp:BoundField DataField="LogPageNoFormatted" HeaderText="Log Page No."></asp:BoundField>
                            <asp:BoundField DataField="SouUniverseDateTimeFormatted" HeaderText="Departure Info"></asp:BoundField>
                            <asp:BoundField DataField="DesUniverseDateTimeFormatted" HeaderText="Arrival Info"></asp:BoundField>
                            <asp:BoundField DataField="TimeInAir" HeaderText="Airborne Time"></asp:BoundField>
                            <asp:ButtonField ButtonType="Link" HeaderText="Select" Text="Select" CommandName="Select" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- DiscrepancyDetail Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDiscrepancyDetail" Text="Discrepancy Detail" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlDiscrepancyDetail" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeDiscrepancyDetail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupDiscrepancyDetail" runat="server" TargetControlID="btnDummyDiscrepancyDetail"
            PopupControlID="pnlDiscrepancyDetail" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameDiscrepancyDetailComplete() {
                $("#btnDummyDiscrepancyDetail").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenDiscrepancyDetailWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeDiscrepancyDetail").attr("src", "wfDiscrepancyCorrectiveAction.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyDiscrepancyDetail").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForDiscrepancyDetail() {
                var DiscrepancyDetailwindow = $find("<%=mdlPopupDiscrepancyDetail.ClientID %>");
                //close kit popup window
                DiscrepancyDetailwindow.hide();
                //           release resources
                $("#IframeDiscrepancyDetail").attr("src", "JavaScript:''");
                //call kit image button
                $("#hdnBtnDiscrepancyDetail").click();
                CloseChildPage();
            }
        </script>
        <!-- End-->


    </form>
</body>
</html>
