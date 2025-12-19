<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHSNACSHistoryList_Ajax.aspx.vb"
    Inherits="Flypal.wfHSNACSHistoryList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>HSN/ACS History List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

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
                    <table id="tblinner" class="clstablelistin">
                        <tr>
                            <td>
                                <span id="lblTitle" class="clstitle1">HSN/ACS History List</span>
                            </td>
                        </tr>
                        <tr>
                            <asp:UpdatePanel runat="server" ID="upnlHSNACSHistoryList" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <td>
                                        <asp:GridView ID="dgHSNACSHistoryList" runat="server" AllowPaging="True" AllowSorting="True"
                                            DataKeyNames="ID" AutoGenerateColumns="False" CssClass="clsGrid" PageSize="25"
                                            ShowHeaderWhenEmpty="True">
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GSTPercent" HeaderText="Percent">
                                                    <HeaderStyle HorizontalAlign="right" ForeColor="White" Wrap="False" />
                                                    <ItemStyle HorizontalAlign="right" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FromDate" HeaderText="From Date">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ToDate" HeaderText="To Date">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                        </asp:GridView>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                               
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Back" ToolTip="Click to go back to previous page" />
                                                </td>
                                            </tr>
                                        </table>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForHSNACSHistory();
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
             parent.IFrameHSNACSHistoryStateComplete();
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
    </form>
    <script type="text/javascript">
        //Date validations
        function BetweenDatesValidation(source, args) {
            var IsScrapDateBlank = Sys.Extended.UI.TextBoxWrapper.get_Wrapper($get("txtScrapDate"))._isWatermarked;
            if (!IsScrapDateBlank) {
                args.IsValid = false;
                var fromdate = $("#txtManufacturingDate").val();
                var todate = $("#txtScrapDate").val();
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
        }

        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });

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
</body>
</html>
