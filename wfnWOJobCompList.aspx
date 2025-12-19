<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobCompList.aspx.vb" Inherits="Flypal.wfnWOJobCompList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
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
    <style type="text/css">
        .style1
        {
            width: 25px;
        }
        .style2
        {
            width: 10px;
        }
    </style>
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
                        <td colspan="2">
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1"> Removal/Installation List</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                                    <fieldset class="clsFieldSet" id="fdswodetail1" style="padding: 0px 4px 0px 0px;
                                        width: auto; z-index: 10000; border-width: 1px" runat="server">
                                        <legend id="ldwodetail1" class="clsFieldSet1" runat="server"><b>Job Details</b></legend>
                                        <table valign="top" cellspacing="1" cellpadding="1" width ="100%">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWO" runat="server" CssClass="clsLabel">W.O. No.</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWOLabel" runat="server" CssClass="clsLabelAuto" Text="<%# mnWO.WONumber %>"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWODate" runat="server" CssClass="clsLabel">W.O. Date</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWODate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                        Width="100px"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtWODate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWODate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtWODate"
                                                        WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <span id="lblJob" class="clsLabel">Job # </span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblJobLebel" runat="server" CssClass="clsLabel" Text="<%# mnWO.WOJobs.CurrentItem.SrNo %>"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <span id="lblWOJobTypeName" class="clsLabel">Job Type</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWOJobType" runat="server" CssClass="clsLabel" Text="<%# mnWO.WOJobs.CurrentItem.WOJobTypeName %>">
                                                    </asp:Label>
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
                            <asp:Label ID="lblPlannedTask" runat="server" CssClass="clsLabelHeader">List of Removal/Installation -</asp:Label>
                        </td>
                        <td align="right">
                            <asp:UpdatePanel ID="upnlRemInst" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnAddWORemInst" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                        Enabled="<%# mnWO.WOStatusID <> 3 %>" ToolTip="Click to Add Task"></asp:Button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <asp:UpdatePanel ID="upnldgWOJobComps" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="dgWOJobComps" runat="server" CssClass="clsGrid" ToolTip="List of Assembly/Component as Installation/Removal"
                                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OffPartNo" HeaderText="Off Part No.">
                                                <ItemStyle Wrap="False"></ItemStyle>
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OffDescription" HeaderText="Off Description">
                                                <ItemStyle Wrap="False"></ItemStyle>
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OffSerialNo" HeaderText="Off Serial No.">
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OnPartNo" HeaderText="On Part No.">
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OnDescription" HeaderText="On Part Description">
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OnSerialNo" HeaderText="On Serial No.">
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRecord">
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:ButtonField>
                                            <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRecord">
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:ButtonField>
                                        </Columns>
                                        <PagerSettings PreviousPageText="Prev" NextPageText="Next" />
                                        <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to previous screen"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr style="height: 0px;">
                                            <td style="height: 0px;">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                    <ContentTemplate>
                                                        <asp:Button ID="hdnBtnAddSelectTasks" ClientIDMode="Static" runat="server" Text="----"
                                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
    </table>
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
            parent.ParentCallBackFunctionForJobCompList();
            return false;
        }
    </script>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameJobCompListStateComplete();
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <%--End--%>
    <!-- JobCompDetail Popup Window -->
    <%-- 'Added by Saylee on 29-May-2019--%>
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyJobCompDetail" Text="Dummy JobCompDetail"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupJobCompDetail" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupJobCompDetail" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupJobCompDetail" runat="server" TargetControlID="btnDummyJobCompDetail"
        PopupControlID="pnlPopupJobCompDetail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameJobCompDetailStateComplete() {
            $("#btnDummyJobCompDetail").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }

        function OpenToAddJobCompDetail() {
            try {
                $get("AjaxLoader").style.visibility = "visible";
                $("#iPopupJobCompDetail").attr("src", "wfnWOJobComp_AJAX.aspx?Type=pup");
                if (!$.browser.msie) {
                    $("#btnDummyJobCompDetail").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }

                return false;
            } catch (e) {
                alert(e);
            }


        }
       
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForJobCompDetail() {
            var JobCompDetailWindow = $find("<%=mdlPopupJobCompDetail.ClientID %>");
            //close JobCompDetail popup window
            JobCompDetailWindow.hide();
            $("#iPopupJobCompDetail").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnBtnAddJobCompDetail").click();
        }
    </script>
    </form>
</body>
</html>
