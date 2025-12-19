<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOPlannedList.aspx.vb"
    Inherits="Flypal.wfnWOPlannedList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>WO Planning List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
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
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblInner">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">List Of W.O.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                            <%--   <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table2" class="clstable">
                                                                    <tr>
                                                                        <td>
                                                                            <span class="clsLabel">Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="true"
                                                                                CausesValidation="true">
                                                                                <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                                <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                                <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                                <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                                <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                                <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel">From Date</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                AutoPostBack="True" CausesValidation="true" ValidationGroup="a" ClientIDMode="Static"
                                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td align="right">
                                                                            &nbsp;&nbsp;
                                                                            <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Width="78px" DESIGNTIMEDRAGDROP="19">To Date </asp:Label>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                AutoPostBack="True" CausesValidation="true" ValidationGroup="a" ClientIDMode="Static"
                                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblTextNo" runat="server" CssClass="clsLabel">Work Order No.</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlWO" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbWO" runat="server" CssClass="clsComboBox1_Ajax" DataTextField="WOText"
                                                                                        DataValueField="WOText" AutoPostBack="true">
                                                                                        <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnllblNo" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:Label ID="lblNo" runat="server" CssClass="clsLabel" Height="8px" Width="32px">No.</asp:Label>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td colspan="4">
                                                                            <asp:UpdatePanel ID="upnlNo" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" Width="184px" MaxLength="4"
                                                                                        ToolTip="Enter Number"></asp:TextBox>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="Span1" class="clsLabelAuto">Reg No.</span>
                                                                        </td>
                                                                        <td colspan="1">
                                                                            <asp:UpdatePanel ID="upnlAircraft" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox1_Ajax" DataTextField="Text"
                                                                                        DataValueField="Text" AutoPostBack="True">
                                                                                        <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblModel" runat="server" CssClass="clsLabel">Model</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlModel" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox1_Ajax" DataTextField="Text"
                                                                                        DataValueField="Text">
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkShowPlannedWO" runat="server" CssClass="clsLabelAuto" ClientIDMode="Static"  AutoPostBack ="true"
                                                                                Text='Show "Planned Work Order"' />
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:CheckBox ID="chkShowAllWOs" runat="server" CssClass="clsLabelAuto" ClientIDMode="Static" AutoPostBack ="true"
                                                                                Text='Show "ALL Work Order(s)"' />
                                                                        </td>
                                                                    </tr>
                                                                    <placeholder id="phStatus" runat="server" visible='<%# iif(chkShowAllWOs.Checked = True,True,False) %>'>
                                                                    <tr>
                                                                    
                                                                        <td>
                                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabel">WO Status</asp:Label>
                                                                        </td>
                                                                        <td colspan="6">
                                                                            <asp:UpdatePanel ID="upnlWOStatus" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsComboBox1_Ajax" DataTextField="Name"
                                                                                        DataValueField="ID" AutoPostBack="True">
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>

                                                                    </tr>
                                                                    </placeholder>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 4px">
                                                        <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                            Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="height: 43px" align="right" valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Work Order as  per searching criteria"
                                                            CausesValidation="True" ValidationGroup="a" Text="Find Now" Visible="true"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 43px" colspan="1">
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Work Order as per criteria :  Record(s) found.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPrintTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Text="Print" ToolTip="Click to Print the list of Work Order " Visible="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Text="Close" ToolTip="Click to close List of Work Order screen" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgWOList" runat="server" CssClass="clsGrid" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
                                                EnableViewState="false" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False"
                                                PageSize="25">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="WODateFormatted" HeaderText="Date">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WONumber" SortExpression="WONumber" HeaderText="W. O. No.">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CustomerWONo" SortExpression="CustomerWONo" HeaderText="Cust. WONo">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WOStartDateFormatted" HeaderText="Start Date">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WOBy" SortExpression="WOBy" HeaderText="Created  By ">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="DOC. Status"
                                                        Visible="false">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WOStatus" SortExpression="WOStatus" HeaderText="WO Status">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DoneByName" SortExpression="DoneByName" HeaderText="Done By">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WOCloseDateFormatted" HeaderText="Closing Date" Visible="false">
                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ClosedBy" SortExpression="ClosedBy" HeaderText="Closed By"
                                                        Visible="false">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Plan" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                    <asp:BoundField DataField="TransTypeID" HeaderText="TransTypeID" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Visible="False"
                                                            ToolTip="Click to Print the list of Work Order " Text="Print" CausesValidation="False">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Work Order screen"
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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

            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                var fromdate = $("#txtFromDate").val();
                var todate = $("#txtToDate").val();
                if (!todate) {
                    return;
                }
                if (!fromdate) {
                    return;
                }
                var param = { 'FromDate': fromdate, 'ToDate': todate };
                $.ajax({
                    type: "POST",
                    url: "BetweenDateValidationHandler.ashx",
                    cache: false,
                    data: param,
                    async: false,
                    beforeSend: OnBeforeSnd,
                    success: onSuces,
                    error: onErr
                });

                function onSuces(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    if (result == "True") {
                        args.IsValid = true;
                        return;
                    }

                }

                function onErr(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    source.errormessage = result;
                    return;
                }
                function OnBeforeSnd() {
                    $get("AjaxLoader").style.visibility = 'visible';
                }

            }

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
        <%-- <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                showTextField();
            });    
        </script>--%>
    </div>
    <!--WorkOrderAttach Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
        PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameAttachStateComplete() {
            $("#btnDummyAttach").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenAttachWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyAttach").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForAttach() {
            var Attachwindow = $find("<%=mdlAttach.ClientID %>");
            //close popup window
            Attachwindow.hide();
            //release resources
            $("#IframeAttach").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnAttach").click();
        }
    </script>
    <!-- End-->
    </form>
    <%--<script type="text/javascript">
        function DisableValidators() {
            var SearchIndex = $get("cmbSearch").selectedIndex;
            if (SearchIndex == 1) {
                var DateIndex = $get("cmbDateRange").selectedIndex;
                if (DateIndex == 6) {
                    return true;
                }
            }
            ToDo:
            {
                for (i = 0; i < Page_Validators.length; i++) {
                    if (Page_Validators[i].validationGroup == "a") {
                        ValidatorEnable(Page_Validators[i], false);
                    }
                }
                document.getElementById("<%= Validationsummary2.ClientID %>").style.display = 'none';


            }
        }   
    </script>--%>
</body>
</html>
