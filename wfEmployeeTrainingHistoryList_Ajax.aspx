<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeTrainingHistoryList_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeTrainingHistoryList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Training History List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }     

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTrainingHistoryDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="3" class="clsFormHeader1Newstyle">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblListOrder" class="clsFormHeader">Employee Training History List</span>
                                                                </td>
                                                                <td align="right">
                                                                    <table class="clstableButton" align="right">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to Close Employee Training History List Screen"></asp:Button> 
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--<div style="width: 790px;">
                                                        <table class="clsGrid" style="width: 790px; border-collapse: collapse;" cellpadding="0"
                                                            cellspacing="0">
                                                            <tr>
                                                                <td width="110px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeTrainingHistoryList','$TrainingName')">
                                                                        Training Name</a>
                                                                </td>
                                                                <td width="120px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeTrainingHistoryList','$CertificateNo')">
                                                                        Certificate No.</a>
                                                                </td>
                                                                <td width="90px" class="clsdgHeader">
                                                                    <span>Date</span>
                                                                </td>
                                                                <td width="60px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeTrainingHistoryList','$Duration')">
                                                                        Duration</a>
                                                                </td>
                                                                <td width="150px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeTrainingHistoryList','$TrainingOrgName')">
                                                                        Training Org. Name</a>
                                                                </td>
                                                                <td width="110px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeTrainingHistoryList','$YearOfTraining')">
                                                                        Year Of Training</a>
                                                                </td>
                                                                <td width="100px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeTrainingHistoryList','$Remark')">
                                                                        Remark</a>
                                                                </td>
                                                                <td width="50px" class="clsdgHeader">
                                                                    <span>Attach</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>--%>
                                                    <div>
                                                        <asp:GridView ID="dgEmployeeTrainingHistoryList" runat="server" AllowPaging="true"
                                                            PageSize="5" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            AutoGenerateColumns="False" ClientIDMode="Static" AllowSorting="True" Style="width: 790px;"
                                                            ShowHeader="True" DataKeyNames="ID" ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
													        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="TrainingName" SortExpression="TrainingName" HeaderText="Training Name">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CertificateNo" SortExpression="CertificateNo" HeaderText="Certificate No.">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EmployeeTrainingDate" HeaderText="Date">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Duration" SortExpression="Duration" HeaderText="Duration">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TrainingOrgNameWithCity" SortExpression="TrainingOrgNameWithCity"
                                                                    HeaderText="Training Org. Name">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="MonthOfTrainingName" SortExpression="MonthOfTrainingName"
                                                                    HeaderText="Month Of Training">
                                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="YearOfTraining" SortExpression="YearOfTraining" HeaderText="Year Of Training">
                                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="View">
                                                                    <ItemStyle CssClass="TextBreak" Width="50px" Wrap="true" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <%--<asp:ImageButton ID="lnkDocumentView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="Attach" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                            Visible='<%#  Eval("IsAttachmentAdded") %>' />--%>

                                                                         <asp:ImageButton ID="lnkDocumentView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                             CommandName="Attach" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("IsAttachmentAdded") %>'/>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>

                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td align="right">
                                                    <table class="clstableButton" align="right">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to Close to Go back to the Previous Screen">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>--%>
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
            parent.ParentCallBackFunctionForEmpTrainingHistory();
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
                parent.IFrameEmpTrainingHistoryStateComplete();
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
