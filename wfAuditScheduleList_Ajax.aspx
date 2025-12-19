<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditScheduleList_Ajax.aspx.vb"
    Inherits="Flypal.wfAuditScheduleList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Audit Schedule List</title>
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

    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
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
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>

                                <td colspan="2" class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblAuditScheduleList" class="clsFormHeader">Audit Schedule List</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Audit Schedule "
                                                                        Text="Add New"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Audit Schedule List screen"
                                                                        Text="Close"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                        ValidationGroup="a"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                    <script type="text/javascript">
                                        function showTextField() {
                                            var SearchIndex = $get("cmbSearch").selectedIndex;

                                            var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                            var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                            var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                            var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
                                            if (SearchIndex != 1) {
                                                txtFromDateobj.style.display = 'none';
                                                txtToDateobj.style.display = 'none';
                                                lblFromDateobj.style.display = 'none';
                                                lblToDateobj.style.display = 'none';
                                            }
                                            else {
                                                var DateIndex = $get("cmbDateRange").selectedIndex;
                                                if (DateIndex == 0) {
                                                    txtFromDateobj.style.display = 'none';
                                                    txtToDateobj.style.display = 'none';
                                                    lblFromDateobj.style.display = 'none';
                                                    lblToDateobj.style.display = 'none';
                                                }
                                            }

                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table id="Table1" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblSearch" class="clsLabel" style="width: 48px;">Search</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="170px"
                                                                        CausesValidation="true" ValidationGroup="b" AutoPostBack="True">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="1">Date Range</asp:ListItem>
                                                                        <asp:ListItem Value="2">Text</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                        Visible="False">
                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                        <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                        <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                        <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                        <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtSearchText" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Search Text"
                                                                        CausesValidation="true" ValidationGroup="b" BackColor="White" AutoPostBack="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Width="66px">From Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" 
                                                                        AutoPostBack="True" CausesValidation="true" ValidationGroup="a" ClientIDMode="Static"
                                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                        ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Width="52px">To Date</asp:Label>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch"  
                                                                        AutoPostBack="True" CausesValidation="true" ValidationGroup="a" ClientIDMode="Static"
                                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Audit Schedule List as per searching criteria"
                                                            Text="Find Now" ValidationGroup="a" OnClientClick="DisableValidators();" Visible="false"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Audit Schedule as per criteria :   Record(s) found.</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <%--<asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Audit Schedule "
                                                                                Text="Add New"></asp:Button>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Audit Schedule List screen"
                                                                                Text="Close"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgAuditSchedule" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                            ShowHeaderWhenEmpty="true" PageSize="25" AllowPaging="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="ScheduleDateFormatted" HeaderText="Date">
                                                                    <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AuditText" SortExpression="AuditText" HeaderText="Audit No.">
                                                                    <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="True"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AuditTypeName" SortExpression="AuditTypeName" HeaderText="Audit Type">
                                                                    <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="Description" HeaderText="Description">
                                                                    <HeaderStyle Wrap="True"   HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="True"></ItemStyle>
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="DepartmentName" SortExpression="DepartmentName" HeaderText="Responsible Department">
                                                                    <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AuditOnCostCenter" SortExpression="AuditOnCostCenter"
                                                                    HeaderText="Audit On">
                                                                    <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="True"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference No.">
                                                                    <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
                                                                    <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Is Scheduled Next" HeaderStyle-HorizontalAlign="Left"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "NextSchedule") %>'
                                                                            Enabled="False"></asp:CheckBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Frequency" SortExpression="Frequency" HeaderText="Freq. (In Months)">
                                                                    <HeaderStyle Wrap="True" HorizontalAlign="Left"  ></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="Note" SortExpression="Note" HeaderText="Note">
                                                                    <HeaderStyle  ></HeaderStyle>
                                                                </asp:BoundField>
                                                               <%-- <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="View" HeaderText="View" Text="View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>
                                                                  <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>--%>
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                </td> 
                                                                                
                                                                                <td>
                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                        CommandName="View" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                                        Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                        Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
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
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Audit Schedule"
                                                            Text="Add New" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Audit Schedule List screen"
                                                            Text="Close" Visible ="false" ></asp:Button>
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
        <script type="text/javascript">

            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                var fromdate = $("#txtFromDate").val();
                var todate = $("#txtToDate").val();
                if (!todate) {
                    rfvToDate.isvalid = false;
                    return;
                }
                if (!fromdate) {
                    rfvFromDate.isvalid = false;
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
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                showTextField();
            });



        </script>
    </form>
    <script type="text/javascript">
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
    </script>
</body>
</html>
