<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptFlyPalStatusReport_Ajax.aspx.vb" Inherits="Flypal.wfrptFlyPalStatusReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>FlyPal Status Report</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link    id="MainStyle" type="text/css" rel="stylesheet">
     <asp:PlaceHolder runat="server">
            <!-- #include file= "LocalFunctionAjax.htm" -->
        </asp:PlaceHolder>
</head>
<body>
    <form id="Form1" method="post" runat="server">
     <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
      <%--Added on 20-Sep-2016--%>
      <script type="text/javascript">
          window.onload = blinknow;
          function blinknow() {
              var e = document.getElementById("<%=MailImgID.ClientID%>");
              e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
              setTimeout("blinknow();", 750);
          }
      </script>
    <div>
        <table id="tblMain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <asp:UpdatePanel ID="upnlFlyPalStatusReport" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin" border="0" >
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <asp:Label ID="lblDayBook" runat="server" CssClass="clsFormHeader">FlyPal Status Report</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Step I.  Select Date</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table border="0">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblAsOnDate" runat="server" CssClass="clsLabelAuto" DESIGNTIMEDRAGDROP="19">As  On Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                                        ClientIDMode="Static" runat="server" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:RequiredFieldValidator ID="rfvtodates" runat="server" ErrorMessage="Select To Date for Search"
                                                            Display="None" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Label ID="lblinfo1" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Step II.   Display Reports</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto"></asp:Label>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 16px" align="right">
                                            <table id="Table1" border="0">
                                             <%--20-Sep-2016--%>
                                            <tr>
                                            <td>
                                            &nbsp;
                                            </td>
                                           
                                            <td align="center">
                                            <asp:Image ID="MailImgID" runat="server" ImageUrl="images/new.png" Height="31px"
                                              Width="52px" />
                                            </td>
                                            <td>
                                            &nbsp;
                                            </td>
                                            </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" ToolTip="Click to display report"
                                                            Text="Display"></asp:Button>
                                                    </td>
                                                     <%--20-Sep-2016--%>
                                                       <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" 
                                                            Text="Report By Mail" ToolTip="Click to receive Report through mail" ValidationGroup="1"
                                                           />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCloseTop" runat="server"  ToolTip="Click to Close Flypal Status screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                                 <!--Dummy panel to open modelpopup 20-Sep-2016-->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;" colspan="2" align="right">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgMELBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--End -->
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>

       <!-- Popup For Report By Mail 20-Sep-2016-->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
        PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyReceipt1").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgMELBtnSendMail").click();
        }


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
    <!---End-->
    </form>
</body>
</html>
