<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfhangarPlanningCalendarList.aspx.vb"
    Inherits="Flypal.wfhangarPlanningCalendarList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Hangar Planning Schedule List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet"/>
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
    <style type="text/css">
        .clsComboBox_Ajax
        {
        }
        .style1
        {
            width: 20%;
            height: 29px;
        }
        .style2
        {
            width: 90%;
            height: 29px;
        }
    </style>
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
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td colspan="3">
                                <span id="lblAuditExecutionList" class="clstitle1">hangar Planning Schedule Graph</span>
                            </td>
                        </tr>
               
                        <tr>
                            <td colspan="3" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step I. Selection of Month and Year</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="Right">
                            </td>
                            <td align="left">
                                <span id="lblYear" class="clsLabelAuto">Month and Year</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbMonth" runat="server" CssClass="clsComboBox1_Ajax">
                                </asp:DropDownList>
                                <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox1_Ajax">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Hangar</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<span id="lblAircraftStar1" class="clsLabelStar">*</span>--%>
                            </td>
                            <td>
                                <span id="lblModel" class="clsLabelAuto">Hangar</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox1_Ajax" DataTextField="HHangerWithCity"
                                    DataValueField="HID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="lblStep4" class="clsLabelHeader">Step III. Display Report</span>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan ="3">
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Search Criteria</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Audit Execution List as per searching criteria"
                                                            Text="Find Now" ValidationGroup="a" OnClientClick="DisableValidators();" Visible="False">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0" 
                                                                            Text="FindNow" ToolTip="Click to Display Report" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close CalendarGraph"
                                                                            Text="Close"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="toolbar" style="width: 100%">
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="style1">
                                                                    <asp:UpdatePanel ID="upnlCurrentDate" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <a href="javascript:dpm.commandCallBack('previous');">◄</a> <a href="javascript:dpm.commandCallBack('next');">
                                                                                            ►</a> <a href="javascript:dpm.commandCallBack('today');">This Month </a>
                                                                                    </td>
                                                                                    <td>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td align="center" class="style2">
                                                                    <asp:UpdatePanel ID="upnlShowDate" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lblCurrentDate" runat="server" CssClass="clsLabelHeader" Height="16px"
                                                                                Text=""></asp:Label>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlcontrol" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel runat="server" ID="Panel1">
                                                                <div style="margin-top: -21px;">
                                                                    <div style="border-top: 1px solid Black; border-left: 1px solid Black; position: relative;
                                                                        top: 20px; z-index: 1; left: 0px; width: 35px; height: 19px; background-color: #efefef;">
                                                                    </div>
                                                                    <DayPilot:DayPilotMonth ID="DayPilotMonth1" runat="server" DataEndField="ToDateTime"
                                                                        DataStartField="FromDateTime" DataTextField="maircraft" DataValueField="id" DataTagFields="maircraft, id"
                                                                        ContextMenuID="DayPilotMenu1" ClientObjectName="dpm" EventMoveHandling="CallBack"
                                                                        OnEventMove="DayPilotMonth1_EventMove" Width="756px" EventResizeHandling="CallBack"
                                                                        OnEventResize="DayPilotMonth1_EventResize" OnTimeRangeSelected="DayPilotMonth1_TimeRangeSelected"
                                                                        TimeRangeSelectedHandling="PostBack" OnBeforeEventRender="DayPilotMonth1_BeforeEventRender"
                                                                        BubbleID="DayPilotBubble1" ShowToolTip="true" OnCommand="DayPilotMonth1_Command"
                                                                        EventClickHandling="PostBack" OnEventClick="DayPilotMonth1_EventClick" EventStartTime="False"
                                                                        EventEndTime="False" OnBeforeCellRender="DayPilotMonth1_BeforeCellRender" HeaderBackColor="#efefef"
                                                                        NonBusinessBackColor="White" BackColor="#ECF3FB" InnerBorderColor="#99BBE6" AutoRefreshEnabled="True"
                                                                        EventTextAlignment="Center" Height="356px" AfterRenderJavaScript="afterRender(data)"
                                                                        ForeColor="Black" EventBackColor="Black" EventFontColor="White" EventTimeFontColor="White">
                                                                    </DayPilot:DayPilotMonth>
                                                                </div>
                                                            </asp:Panel>
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
                            <td colspan="3" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close CalendarGraph"
                                                        Text="Close"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup for category/nomenclature-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnGraph" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <%--   <asp:DropDownList ID="cmbHanger" runat="server" CssClass="clsComboBox" DataTextField="HHangerWithCity"
                                                                                                DataValueField="HID" Style="margin-left: 0px" Width="250px">
                                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtCrewName" runat="server" Width="250px" CssClass="clsTextBoxReadOnly"
                                                                                ReadOnly="true" TabIndex="-1"></asp:TextBox>--%>
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
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyHanger" Text="Dummy Hanger" ClientIDMode="Static"
            CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlHanger" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeHanger" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupHanger" runat="server" TargetControlID="btnDummyHanger"
        PopupControlID="pnlHanger" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameHangerStateComplete() {
            $("#btnDummyHanger").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenHangerWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeHanger").attr("src", "wfHangarPlanning.aspx?Type=pup");
                $('#IframeHanger').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyHanger").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForHanger() {
            varHangerwindow = $find("<%=mdlPopupHanger.ClientID %>");
            //close Hanger popup window
            varHangerwindow.hide();
            //           release resources
            $("#IframeHanger").attr("src", "JavaScript:''");
            //call Hanger image button
            $("#hdnGraph").click();
        }
        function afterRender(data) {
            // check if the label should be updated
            if (data && data.label) {
                var label = document.getElementById("lblCurrentDate");
                label.innerHTML = data.label;
            }
        }
    </script>
    <!-- End-->
    <script type="text/javascript">
     <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
       SetPageLayout();
         if ($.browser.msie) {
             parent.IFramePropertyValueStateComplete();
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
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForPropertyValue();
            return false;
        }
    </script>
    </form>
</body>
</html>
