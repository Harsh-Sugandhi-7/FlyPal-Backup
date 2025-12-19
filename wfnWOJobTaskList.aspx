<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobTaskList.aspx.vb"
    Inherits="Flypal.wfnWOJobTaskList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    <form id="form1" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <script language="javascript" type="text/javascript">

            var g_CurrentTextBox;
            var g_isTabPressed;

            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            $(document).ready(function () {
                function endRequestHandler() {

                    try {

                        //if (g_isTabPressed == 1) {
                        $get(g_CurrentTextBox).focus();
                        $get(g_CurrentTextBox).select();

                        g_isTabPressed = 0;
                        //}


                    }
                    catch (Error) { }

                }

            });
        </script>
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                function onTextFocus() {
                    g_CurrentTextBox = event.srcElement.id;

                }

                function onkeyPressed(keycode, obj) {

                    if (keycode == 9) {

                        g_isTabPressed = 1;
                    }

                }
            });
        </script>
        <%--AJAX- ScriptManager Added--%>
        <table class="clstablelistout" id="tblmain" cellspacing="1" cellpadding="1" border="0">
            <tr>
                <td>
                    <table class="clstablelistin" id="InnerTable" border="0">
                        <tr>
                            <td class="clsFormHeader1Newstyle" colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> W.O. JOB Task List</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddWOJobTask" runat="server" CssClass="clsbtnH clsinfoH" Text="Add"
                                                                    Enabled="<%# mnWO.WOStatusID <> 3 %>" ToolTip="Click to Add Task"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to previous screen"
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 0px;">
                                                            <td style="height: 0px;">
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="hdnBtnAddSelectTasks" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnAddJobTaskDetail" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnAttach" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                            Style="display: none;"></asp:Button>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
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
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"
                                            runat="server"></asp:ValidationSummary>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top">
                                <asp:UpdatePanel ID="upnlWOJobDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" id="fdswodetail1" 
                                            runat="server">
                                            <legend id="ldwodetail1" runat="server"><b>Job Description</b></legend>
                                            <table valign="top" cellspacing="1" cellpadding="1" width="100%">
                                                <tr style="display: none">
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWO" runat="server" CssClass="clsLabel">W.O. No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWOLabel" runat="server" CssClass="clsLabelAuto" Text="<%# mnWO.WONumber %>"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWODate" runat="server" CssClass="clsLabel">W.O. Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWODate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
                                                            Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtWODate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWODate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtWODate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblJob" class="clsLabel">Job # </span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblJobLebel" runat="server" CssClass="clsLabel" Text="<%# mnWO.WOJobs.CurrentItem.SrNo %>"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblWOJobTypeName" class="clsLabel">Job Type</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWOJobType" runat="server" CssClass="clsLabel" Text="<%# mnWO.WOJobs.CurrentItem.WOJobTypeName %>">
                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" bgcolor="#E0E0E0">
                                                        <asp:Label ID="lblJobDescription" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtJobDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2"
                                                            TextMode="MultiLine" Text="<%# mnWOJob.WOJobDescription %>" ToolTip="Job Description" Width="97%"
                                                            ReadOnly="True" BackColor="#E0E0E0" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <asp:Label ID="lblPlannedTask" runat="server" CssClass="clsLabelHeader">List of Tasks to complete the Job -</asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlWOJobTask" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgWOJobTask" runat="server" CssClass="clsGridNewStyle" ToolTip="List of Task"
                                            DataKeyNames="ID" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="Id" HeaderText="ID"></asp:BoundField>
                                                <%--0--%>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <%--1--%>
                                                <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No.">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <%--2--%>
                                                <asp:BoundField DataField="TaskDescription" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <%--3--%>
                                                <asp:BoundField DataField="TaskAction" HeaderText="Action">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <%--4--%>
                                                <asp:BoundField DataField="ActualStartDateFormatted" HeaderText="Actual Start Date">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <%--5--%>
                                                <asp:BoundField DataField="ActualEndDateFormatted" HeaderText="Actual End Date">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <%--6--%>
                                                <asp:BoundField DataField="ActualTime" HeaderText="Actual Time">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <%--7--%>
                                                <%--  <asp:ButtonField CommandName="EditRecord" HeaderText="Edit" Text="Edit">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>--%>  <%-- 8--%>
                                                <%--<asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>--%> <%--9--%>
                                                <%--<asp:TemplateField HeaderText="Attach">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkWOJobTaskView" runat="server" Text="View" CommandName="Attach"
                                                            CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                </asp:TemplateField>--%>  <%--10--%>
                                                 <%-- 8--%>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>

                                                        <div class="dropdown">
                                                            <div class="dropdownbtn-content">
                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                CommandName="EditRecord" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="btnView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                CommandName="Attach" Style="height: 20px; width: 20px" ImageUrl="~/icons/CLIP01.ico" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                                Style="cursor: pointer" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%--9--%>
                                                <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                <%--10--%>
                                                <asp:BoundField DataField="AttachmentCount" HeaderText="AttachmentCount"  HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                     
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                            <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to previous screen"
                                                        CausesValidation="False" Visible ="false"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr style="height: 0px;">
                                                <td style="height: 0px;">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                        <ContentTemplate>
                                                            <asp:Button ID="hdnBtnAddSelectTasks" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnBtnAddJobTaskDetail" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnBtnAttach" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                Style="display: none;"></asp:Button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
        <!-- SelectTasks Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySelectTasks" Text="Dummy SelectTasks" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupSelectTasks" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupSelectTasks" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSelectTasks" runat="server" TargetControlID="btnDummySelectTasks"
            PopupControlID="pnlPopupSelectTasks" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameSelectTasksStateComplete() {
                $("#btnDummySelectTasks").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddSelectTasks() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupSelectTasks").attr("src", "wfSelectTaskCardList_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummySelectTasks").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }
            function CallParentOpenToAddSelectTasks() {
                window.parent.OpenToAddSelectTasks();
            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForSelectTasks() {
                var SelectTasksWindow = $find("<%=mdlPopupSelectTasks.ClientID %>");
                //close SelectTasks popup window
                SelectTasksWindow.hide();
                $("#iPopupSelectTasks").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddSelectTasks").click();
            }
        </script>
        <!-- End-->
        <script type="text/javascript">
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForJobList();
                return false;
            }

            function CallCloseChildPage() {

                window.parent.CloseChildPage();
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameJobListStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
        <!-- JobTaskDetail Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyJobTaskDetail" Text="Dummy JobTaskDetail"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupJobTaskDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupJobTaskDetail" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupJobTaskDetail" runat="server" TargetControlID="btnDummyJobTaskDetail"
            PopupControlID="pnlPopupJobTaskDetail" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
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
            function CallParentOpenToAddJobTaskDetail(Index) {
                window.parent.OpenToAddJobTaskDetail(Index);
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
    </form>
    <script language="javascript">
        function SetTabCount(CountForTab) {
            //            if (CountForTab == -1) {
            //                var totalRowCount = 0;
            //                var rowCount = 0;
            //                var gridView = document.getElementById("<%=dgWOJobTask.ClientID %>");
            //                var rows = gridView.getElementsByTagName("tr")
            //                for (var i = 0; i < rows.length; i++) {
            //                    totalRowCount++;
            //                    if (rows[i].getElementsByTagName("td").length > 0) {
            //                        rowCount++;
            //                    }
            //                }
            //                parent.document.getElementById("lblHeader").innerHTML = rowCount;
            //            }
            //            else {
            parent.document.getElementById("lblHeader").innerHTML = CountForTab;
            //    }
        }
    </script>
</body>
</html>
