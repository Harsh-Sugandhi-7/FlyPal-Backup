<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobNRCList.aspx.vb"
    Inherits="Flypal.wfnWOJobNRCList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.O. Job NRC List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function openTranDetail() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx";
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
        <script type="text/javascript">

            var g_CurrentTextBox;
            var g_isTabPressed;

            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            $(document).ready(function () {
                function endRequestHandler() {

                    try {
                        $get(g_CurrentTextBox).focus();
                        $get(g_CurrentTextBox).select();
                        g_isTabPressed = 0;
                    }
                    catch (Error) { }

                }

            });
        </script>
        <script type="text/javascript">

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
                                                    <asp:Label ID="lblTitle" runat="server"
                                                        CssClass="clsFormHeader" Text="W.O. JOB NRC List" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlWOJobNRC" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnRaiseNRC" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    Text="Raise NRC" ToolTip="Click to raise NRC" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    Text="Close" ToolTip="Click to go back to previous screen"
                                                                    CausesValidation="false" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="vsJobNRCList" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary1" CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"
                                            runat="server"></asp:ValidationSummary>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                    <legend>
                                        <asp:Label ID="LabelNRC" runat="server" CssClass="clsLabelHeader" Text="Job Description" />
                                    </legend>
                                    <asp:TextBox ID="txtJobDescription" runat="server"
                                        CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                        TextMode="MultiLine" Text="<%# mnWOJob.WOJobDescription %>"
                                        ToolTip="Job Description"
                                        ReadOnly="True" BackColor="#E0E0E0" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblJobNRC" runat="server"
                                    CssClass="clsLabelHeader" Text="Job NRC" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlJobNRC" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgWOJobNRC" runat="server" CssClass="clsGridNewStyle"
                                            ToolTip="List of W.O. NRC Jobs" AutoGenerateColumns="False" Width="100%"
                                            ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
                                                ForeColor="black" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField Visible="false" DataField="ID" HeaderText="ID" />
                                                <%--0--%>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10px" />
                                                </asp:BoundField>
                                                <%--1--%>
                                                <asp:BoundField DataField="WOJobDescription" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--2--%>
                                                <asp:BoundField DataField="WOJobAction" HeaderText="Action">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--3--%>
                                                <asp:BoundField DataField="DueAsOfGrid" HeaderText="Due As Of" HtmlEncode="false">
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--4--%>
                                                <asp:BoundField DataField="WOJobEstimatedTime" HeaderText="Est. Man Hr">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <%--5--%>
                                                <asp:BoundField DataField="WOJobStartDateFormatted" HeaderText="Start Date">
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <%--6--%>
                                                <asp:BoundField DataField="WOJobCloseDateFormatted" HeaderText="Close Date">
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <%--7--%>
                                                <asp:BoundField DataField="WOJobActualTime" HeaderText="Actual Man Hr.">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--8--%>
                                                <asp:BoundField DataField="WOJobType" HeaderText="Job Type">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--9--%>
                                                <asp:BoundField DataField="WOJobStatusName" HeaderText="Status">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--10--%>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <div id="dropDownImg" class="dropdown">
                                                            <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server"
                                                                CssClass="clsActionbtn" />
                                                            <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                ToolTip="Edit this record." CausesValidation="false"
                                                                                CommandName="EditRecord" ImageUrl="~/images/edit.png" Visible='<%# IIf(mnWO.WOStatusID = 3, False, True) %>'/>
                                                                        </td>

                                                                        <td>
                                                                            <asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
                                                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                ToolTip="Delete this record." CommandName="DeleteRecord"
                                                                                ImageUrl="~/images/delete.png" CausesValidation="false" Visible='<%# IIf(mnWO.WOStatusID = 3, False, True) %>'/>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="viewICN" class="attachmentICNS" runat="server"
                                                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                ToolTip="View the Attachment Added." CommandName="View"
                                                                                ImageUrl="icons/CLIP01.ICO" CausesValidation="false"
                                                                                Visible='<%# Eval("IsAttachmentAdded") %>' />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="printICN" class="actionICNS" runat="server"
                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                ToolTip="Click to Print record"
                                                                                CommandName="PrintRec" ImageUrl="~/images/print.png" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--11--%>
                                                <asp:BoundField DataField="IsAttachmentAdded"
                                                    HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn"
                                                    HeaderText="IsAttachmentAdded" />
                                            </Columns>
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
                                            <tr style="height: 0px;">
                                                <td style="height: 0px;">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                        <ContentTemplate>
                                                            <asp:Button ID="hdnBtnAddWOJobNRCDetail"
                                                                ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;" />
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

        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

        <div id="popups">

            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">

            <% Dim mopen As String = Request.QueryString("Type") %>
            <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

                $(document).ready(function () {
                    SetPageLayout();
                    //if ($.browser.msie) {
                    parent.IFrameSelectNRCStateComplete();
                    //  }
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


            <script type="text/javascript">
                function CallParentOpenToAddNRCJobDetail() {
                    window.parent.OpenToAddWOJobNRCDetail();
                }
                function CallCloseChildPage() {
                    window.parent.CloseChildPage();
                }
            </script>


            <!-- WOJobNRCDetail Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyWOJobNRCDetail" Text="Dummy WOJobNRCDetail"
                    ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlPopupWOJobNRCDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                <iframe id="iPopupWOJobNRCDetail" frameborder="0" allowtransparency="true" height="100%"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupWOJobNRCDetail" runat="server" TargetControlID="btnDummyWOJobNRCDetail"
                PopupControlID="pnlPopupWOJobNRCDetail" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameWOJobNRCDetailStateComplete() {
                    $("#btnDummyWOJobNRCDetail").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }
                function OpenToAddWOJobNRCDetail() {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupWOJobNRCDetail").attr("src", "wfnWOJobNRC.aspx?Type=pup");
                        if (!$.browser.msie) {
                            $("#btnDummyWOJobNRCDetail").click();
                            $get("AjaxLoader").style.visibility = "hidden";
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }
                }
            </script>
            <script type="text/javascript">
                function ParentCallBackFunctionForWOJobNRCDetail() {
                    var WOJobNRCDetailWindow = $find("<%=mdlPopupWOJobNRCDetail.ClientID %>");
                    //close WOJobNRCDetail popup window
                    WOJobNRCDetailWindow.hide();
                    $("#iPopupWOJobNRCDetail").attr("src", "JavaScript:''");
                    //call ata image button
                    $("#hdnBtnAddWOJobNRCDetail").click();
                }
            </script>
            <!-- End-->

            <%--call parent function after completing subroutine..(when page open as popup)--%>
            <script type="text/javascript">
                function CallParentCallback() {
                    parent.ParentCallBackFunctionForSelectNRC();
                    return false;
                }
            </script>

        </div>
    </form>

    <script type="text/javascript">

        function SetTabCount(CountForTab) {
            if (CountForTab == -1) {
                var totalRowCount = 0;
                var rowCount = 0;
                var gridView = document.getElementById("<%=dgWOJobNRC.ClientID %>");
                var rows = gridView.getElementsByTagName("tr");
                for (var i = 0; i < rows.length; i++) {
                    totalRowCount++;
                    if (rows[i].getElementsByTagName("td").length > 0) {
                        rowCount++;
                    }
                }
                parent.document.getElementById("Label6").innerHTML = rowCount;
            }
            else {
                parent.document.getElementById("Label6").innerHTML = CountForTab;
            }
        }

    </script>
</body>
</html>
