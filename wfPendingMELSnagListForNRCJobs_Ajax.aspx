<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingMELSnagListForNRCJobs_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingMELSnagListForNRCJobs_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>>MEL/Snag List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
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
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","ADD/Defect List","MEL/Snag List") %>'></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table cellspacing="2" cellpadding="1">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Assembly" runat="server" CssClass="clsLabel">Assembly</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="ModelSerialNoPostion"
                                                                    DataValueField="AssemblyStatusID">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                &nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabel">ATA Chapter </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    DataValueField="ID" DataTextField="ATAChapter">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                &nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabel" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","ADD/Defect","MEL/Snag") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbMELSnag" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="1">MEL</asp:ListItem>
                                                                    <asp:ListItem Value="2">Snag</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Click to find list of ADD/Defect as per searching criteria","Click to find list of MEL/Snag as per searching criteria") %>'
                                                                CausesValidation="false" Text="Find Now"></asp:Button>--%>

                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" CausesValidation="false"
                                                            ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Click to find list of ADD/Defect as per searching criteria","Click to find list of MEL/Snag as per searching criteria") %>' />
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
                            <td colspan="2" align="left">
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of MEL Snag/Defect Corrective Action as per criteria:  Record(s) found.</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Click to close List of ADD/Defect screen","Click to close List of MEL/Snag screen") %>'
                                                                Text="Close" CausesValidation="False" Visible="false"></asp:Button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgSnagCorrectiveActionList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                        DataKeyNames="ID"
                                                        ShowHeaderWhenEmpty="True" EnableViewState="False" AllowSorting="True" AllowPaging="True"
                                                        AutoGenerateColumns="False" PageSize="25">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="SerialNo" SortExpression="SerialNo" HeaderText="Sr. No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DefectNo" SortExpression="DefectReportNo" HeaderText="Defect No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateOfOccurenceFormatted" HeaderText="Date Of Occurrence">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LogNoPageNo" SortExpression="LogNo" HeaderText="Log No."
                                                                HtmlEncode="False">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PartNoSerialNo" SortExpression="PartNoSerialNo" HeaderText="Component"
                                                                HtmlEncode="False">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Defect" SortExpression="Defect" HeaderText="Defect" HtmlEncode="False">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Sector" SortExpression="Sector" HeaderText="Sector">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MajorMinorTag" SortExpression="MajorMinorTag" HeaderText="Major/Minor">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="RegNo" SortExpression="RegNo" HeaderText="Reg No.">
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="LogDate" HeaderText="Log Date">
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateTimeOfDue" HeaderText="Due Date">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MELTag" SortExpression="MELTag" HeaderText="Is MEL">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NRCNosWithMELSnagAddedAsJob" HeaderText="NRC Nos.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
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
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Click to close List of ADD/Defect screen","Click to close List of MEL/Snag screen") %>'
                                            Text="Close" CausesValidation="False"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height: 0px;">
                            <td colspan="2" style="height: 0px;">
                                <asp:UpdatePanel runat="server" ID="upnlhdnButton" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnNRCJob" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
    <!-- NRCJob Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyNRCJob" Text="NRCJob" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlNRCJob" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeNRCJob" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupNRCJob" runat="server" TargetControlID="btnDummyNRCJob"
        PopupControlID="pnlNRCJob" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IframeNRCJobStateComplete() {
            $("#btnDummyNRCJob").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenNRCWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeNRCJob").attr("src", "wfNRCJob_Ajax.aspx?Type=MELpup");

                if (!$.browser.msie) {
                    $("#btnDummyNRCJob").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }
        }

        function ParentCallBackFunctionForNRCJob() {
            var NRCJobwindow = $find("<%=mdlPopupNRCJob.ClientID %>");
            //close NRC Job popup window
            NRCJobwindow.hide();
            //release resources
            $("#IframeNRCJob").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnNRCJob").click();
        }
        function ParentCallBackFunctionForNRCJobDirect() {
            var NRCJobwindow = $find("<%=mdlPopupNRCJob.ClientID %>");
            //close NRC Job popup window
            NRCJobwindow.hide();
            //release resources
            $("#IframeNRCJob").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnNRCJob").click();
            parent.ParentCallBackFunctionForPendingMELSnagList();
        }
    </script>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForPendingMELSnagList();
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
                     parent.IframePendingMELSnagListStateComplete();
                 }
            });
            <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
             }

               function SetPageLayout()
               {
               <% Dim mopenas As String = Request.QueryString("Type") %>
                  <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                  ReSetPageLayout();
                  onResize();//for Top bottom link
                   <% End if %>
               }
               function ReSetPageLayout()
               {
               $("body,html").css({ 'background-color': 'transparent' });
                  var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
                  var windowheight=$(window).height();
                  if (tempMargtop>=windowheight)
                  {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
                  }
                  else
                  {
                  var margintop=(windowheight/2)-(tempMargtop/2);
                   $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
                  }
       
               }
    </script>
    <%--End--%>
    </form>
</body>
</html>
