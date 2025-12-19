<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOPendingOJSList.aspx.vb" Inherits="Flypal.wfnWOPendingOJSList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Part Service List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
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
        .maxGridWidth
        {
            max-width: 350px;
        }
    </style>
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

      <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                           <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">W.O. Job List Pending for OJS W.O.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                          <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <span id="lblDateRange" class="clsLabel">Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server">From Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                        AutoPostBack="True"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                        ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server">To Date </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                        AutoPostBack="True"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblWorkOrderNumber" class="clsLabel">W.O. No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbWO" runat="server" CssClass="clsComboBox1_Ajax" DataValueField="WOText"
                                                            DataTextField="WOText" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" Width="100px" ToolTip="Enter Number"
                                                                        MaxLength="4" Visible="false"></asp:TextBox>
                                                                </td>
                                                                
                                                              
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right" valign="top">
                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Work Order as per searching criteria"
                                                Text="Find Now"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        <tr>
                            <td>
                               <%-- <asp:Label ID="lblList" runat="server" Font-Bold="true">WO Pending OJS/NRC List</asp:Label>--%>
                            </td>
                             <td align="right" >
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                        CausesValidation="False" Text="Back"></asp:Button>
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
                                        <td width="100%">
                                            <asp:UpdatePanel ID="upnldgGrid" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="lbldgGridResult" CssClass="clsLabelHeader"></asp:Label>
                                                    <asp:GridView ID="dgWOPendingOJS" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        CssClass="clsGrid" DataKeyNames="ID" PageSize="5" ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                           
                                                            <asp:BoundField DataField="WONumber" SortExpression="WONumber" HeaderText="WO No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="WOJobDescription" SortExpression="WOJobDescription" HeaderText="Description">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="DueAsOfGrid" SortExpression="DueAsOfGrid" HeaderText="Due As Of"
                                                                HtmlEncode="False">
                                                                <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WOJobAction" SortExpression="WOJobAction" HeaderText="Job Action">
                                                                <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WOJobStatusName" SortExpression="WOJobStatusName" HeaderText="Job Status">
                                                                <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WOJobType" SortExpression="WOJobType" HeaderText="Job Type">
                                                                <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
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
                                                    <table cellspacing="0">
                                                        <tr>
                                                              <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Back to Previous Page"
                                                                    CausesValidation="False" Text="Back"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlBtnSeriviceMaster" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="hdnBtnSeriviceMaster" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                        Style="display: none;" Text="Add" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
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
  
    </form>
</body>
</html>
