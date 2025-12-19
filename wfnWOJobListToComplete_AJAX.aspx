<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobListToComplete_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOJobListToComplete_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>WO Job List To Complete</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
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
    <style type="text/css">
        #theBox_3, #theBox_2 {
            display: none;
            width: 145px;
            height: auto;
        }

        a:active, a:focus {
            outline: none;
            ie-dummy: expression(this.hideFocus=true);
        }

       
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:msgbox id="MSGBoxCtrl" runat="server" />
                </ContentTemplate>                                         
            </asp:UpdatePanel>                                             
        </div>                                                             
        <div>
            <table id="tblMain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="2" class="clsFormHeader1">
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>

                                                <table width="100%">
                                                    <tr>
                                                        <td class="clsFormHeader1">
                                                           <table width="100%  ">
                                                               <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Allocated WO Job List</asp:Label>
                                                                    </td>
                                                                    <%--<td align="right" valign="top">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            

                                                                                                     <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" 
                                                                                             CssClass="clsSearch2btn" ToolTip="Click to find list as per searching criteria"/>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>--%>

                                                                    <td align="right">
                                                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                         <%--   <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print"
                                                                                                Visible="false" Text="Print"
                                                                                                CausesValidation="False"></asp:Button>--%>
                                                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close"
                                                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                </table>

                                               <%-- <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Allocated WO Job List</asp:Label>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table1">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server" Width="78px">From Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
                                                                CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');" autocomplete="off"></asp:TextBox>
                                                            <cc2:calendarextender id="txtFromDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtFromDate">
                                                            </cc2:calendarextender>
                                                            <cc2:textboxwatermarkextender targetcontrolid="txtFromDate" id="FromDate_watermarkextender"
                                                                clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                watermarkcssclass="clsDateTextBox">
                                                            </cc2:textboxwatermarkextender>
                                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Width="68px">To Date </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
                                                                CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');" autocomplete="off"></asp:TextBox>
                                                            <cc2:calendarextender id="txtToDate_CalendarExtender1" runat="server" cssclass="cal_Theme1"
                                                                enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtToDate">
                                                            </cc2:calendarextender>
                                                            <cc2:textboxwatermarkextender targetcontrolid="txtToDate" id="ToDate_watermarkextender"
                                                                clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                watermarkcssclass="clsDateTextBox">
                                                            </cc2:textboxwatermarkextender>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chkIsClosedJobs" runat="server" CssClass="clsLabelAuto" TextAlign="Right" Text='"COMPLETED" JOBS'></asp:CheckBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span>W.O.</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbWO" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="WOText"
                                                                DataTextField="WOText" onChange="setComboBoxValue(this,'WO')">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblNo" runat="server" CssClass="clsLabel" Height="8px" Width="32px">No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Number"
                                                                MaxLength="6" ></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span>WO. Job Type</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbWOJobType" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
                                                                DataTextField="Name">
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 4px" colspan="6">
                                                            <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>

                         

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    
                                    <td align="right" valign="top">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Work Order Job List as per searching criteria"
                                                                Text="Find Now"></asp:Button>--%>

                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" 
                                                                CssClass="clsSearch2btn" ToolTip="Click to find"/>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">As per criteria :  Record(s) found.</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%--<td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH"  ToolTip="Click to Print"
                                                                Visible="false" Text="Print"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close"
                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgWOJobList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="true" AllowPaging="True" PageSize="25" AllowSorting="True" 
                                                    CellPadding="5" ForeColor="Black" GridLines="Horizontal">

                                                    <SelectedRowStyle></SelectedRowStyle>
                                                    <EditRowStyle></EditRowStyle>

                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />

                                                    <FooterStyle Wrap="False"></FooterStyle>
                                                    <Columns>
                                                        <%--0--%>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <%--1--%>
                                                        <asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
                                                        <%--2--%>
                                                        <asp:BoundField DataField="TaskNo" HeaderText="Task No / Directive No" SortExpression="TaskNo">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <FooterStyle Wrap="False"></FooterStyle>
                                                        </asp:BoundField>
                                                        <%--3--%>
                                                        <asp:BoundField DataField="WODateFormatted" HeaderText="Date">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <FooterStyle Wrap="False"></FooterStyle>
                                                        </asp:BoundField>
                                                        <%--4--%>
                                                        <asp:BoundField DataField="WONumber" SortExpression="WONo" HeaderText="W.O. No.">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--5--%>
                                                        <asp:BoundField DataField="CustomerInformation" SortExpression="CustomerInformation" HeaderText="Customer Info." HtmlEncode="false">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--6--%>
                                                        <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--7--%>
                                                        <asp:BoundField DataField="WOJobDescription" SortExpression="WOJobDescription" HeaderText="Job Description">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="True"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--8--%>
                                                        <asp:BoundField DataField="WOJobAction" SortExpression="WOJobAction" HeaderText="Job Action">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>


                                                        <%--9--%>
                                                        <asp:BoundField DataField="WOJobType" SortExpression="WOJobType" HeaderText="Job Type">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--10--%>
                                                        <asp:BoundField DataField="WOJobStatusName" SortExpression="WOJobStatusName" HeaderText="Job Status">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <%--11--%>
                                                        <asp:BoundField DataField="Skill" SortExpression="Skill" HeaderText="Skill">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--12--%>
                                                        <asp:BoundField DataField="Zone" SortExpression="Zone" HeaderText="Zone">
                                                            <HeaderStyle Wrap="True"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="True"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--13--%>
                                                        <asp:BoundField DataField="Area" SortExpression="Area" HeaderText="Area"
                                                            HtmlEncode="False">
                                                            <HeaderStyle Wrap="True"  HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="True"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--14--%>
                                                        <asp:BoundField DataField="WOJobEstimatedTime" SortExpression="WOJobEstimatedTime" HeaderText="Estimated Time">
                                                            <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <%--15--%>
                                                        <asp:ButtonField Text="Complete" HeaderText="Complete" CommandName="CompleteRec" ControlStyle-ForeColor="Blue"></asp:ButtonField>
                                                        <%--16--%>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                    CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
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
                                                            <asp:Button ID="btnBottomPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print"
                                                                Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <%--<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close"
                                                                Text="Close" CausesValidation="False" Visible="false"></asp:Button>--%>
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
            <%--<script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                showTextField();
            });    
        </script>--%>
        </div>
        <!-- JobTaskDetail Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyJobTaskDetail" Text="Dummy JobTaskDetail"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupJobTaskDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupJobTaskDetail" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:modalpopupextender id="mdlPopupJobTaskDetail" runat="server" targetcontrolid="btnDummyJobTaskDetail"
            popupcontrolid="pnlPopupJobTaskDetail" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
        <script type="text/javascript">
            function IFrameJobTaskDetailStateComplete() {
                $("#btnDummyJobTaskDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddJobTaskDetail(Index) {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupJobTaskDetail").attr("src", "wfnWOJobTask_AJAX.aspx?Type=pup&Index=" + Index);
                    if (!$.browser.msie) {
                        $("#btnDummyJobTaskDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }


        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForJobTaskDetail() {
                var JobTaskDetailWindow = $find("<%=mdlPopupJobTaskDetail.ClientID %>");
                //close JobTaskDetail popup window
                JobTaskDetailWindow.hide();
                $("#iPopupJobTaskDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddJobTaskDetail").click();
            }
        </script>
        <!-- End-->
        <script type="text/javascript">
            function setComboBoxValue(elem, combo) {
                switch (combo) {
                    //Work Order No Set to 0                                                                                                                                                                                                                                              
                    case 'WO':
                        var id = $(":selected", elem).val();
                        var text = $(":selected", elem).text();
                        //set text of combo to the description text box
                        $("#txtNo").val(0);
                        break;
                }
            }
        </script>
    </form>
</body>
</html>
