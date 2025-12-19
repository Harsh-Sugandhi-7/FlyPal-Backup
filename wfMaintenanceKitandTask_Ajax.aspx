<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMaintenanceKitandTask_Ajax.aspx.vb"
    Inherits="Flypal.wfMaintenanceKitandTask_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Kit and Task Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="jquery.tablednd_0_5.js" type="text/javascript"></script>
    <script src="json2.js" type="text/javascript"></script>
    <style type="text/css">
        .GbiHighlight {
            background-color: Teal;
        }
    </style>
    <!-- End-->
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table border="0" id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblinner" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
																Maintenance Kit And Task Detail
                                                            </asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="2" align="right">
                                                    <asp:UpdatePanel ID="upnlKitButton" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                             <asp:Panel ID="pnlSpareToolsButton" runat="server" Visible="False">
                                                            <asp:Button ID="btnAddKit" runat="server"
                                                                CssClass="clsbtnH clsinfoH" Text="Add"
                                                                ToolTip="Click to Add Kit" />

                                                            <asp:Button ID="btnCloseKit" runat="server"
                                                                CssClass="clsbtnH clsinfoH" Text="Close"
                                                                ToolTip="Click to go to the Previous Page" />
                                                                 </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlInspDet" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsInspectionDetails" class="clsFieldSetNewStyle">
                                                    <legend id="lgdInspectionDetails" runat="server" style="font-weight: bold">Inspection Details
                                                    </legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <span id="lblCode" runat="server" class="clsLabel">Code</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                    BackColor="#E0E0E0" MaxLength="25" Text="<%# mMaintenanceTaskAndKit.Code %>">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="lblATAChapter" class="clsLabel">ATA Chapter</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                    BackColor="#E0E0E0" MaxLength="50" Text="<%# mMaintenanceTaskAndKit.ATAChapter %>">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblReference" class="clsLabel">Reference Doc.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                    BackColor="#E0E0E0" MaxLength="250" Text="<%# mMaintenanceTaskAndKit.Reference %>">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="lblDescription" class="clsLabel">Description</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                    Width="250px" BackColor="#E0E0E0" Text="<%# mMaintenanceTaskAndKit.Description %>"
                                                                    ReadOnly="True" ToolTip="Description" MaxLength="200" TextMode="MultiLine">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblType" class="clsLabel">Type</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtType" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                    BackColor="#E0E0E0" MaxLength="25" Text="<%# mMaintenanceTaskAndKit.MonitorTypeName %>">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="lblEstdManHours" class="clsLabel">Estd. Man Hours</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEstdManHours" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                    ReadOnly="True" BackColor="#E0E0E0" MaxLength="6" Text="<%# mMaintenanceTaskAndKit.RequiredManHours %>">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="3">
                                                                <asp:CheckBox ID="chkShowInCofA" runat="server" CssClass="clsLabelAuto" Text="Show in C of A"
                                                                    Enabled="False" Checked="<%# mMaintenanceTaskAndKit.ShowInCofA %>" TextAlign="Left"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlSpareTools" runat="server" Visible="False">
                                            <asp:UpdatePanel ID="upnlKitList" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdsKitDetails" class="clsFieldSetNewStyle">
                                                        <legend id="lgdKitDetails" runat="server" style="font-weight: bold">Kit Details</legend>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="right"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="dgKitList" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                                        ShowHeaderWhenEmpty="True" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
                                                                            ForeColor="black" HorizontalAlign="Left" />
                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                        <Columns>
                                                                            <%--0--%>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                                ItemStyle-CssClass="hideGridColumn">
                                                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--1--%>
                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <%--2--%>
                                                                            <asp:BoundField DataField="Name" HeaderText="Part No.">
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <%--3--%>
                                                                            <asp:BoundField DataField="Description" HeaderText="Description">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--4--%>
                                                                            <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>
                                                                            <%--5--%>
                                                                            <asp:ButtonField CommandName="EditRecord" HeaderText="Edit" Text="Edit">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:ButtonField>
                                                                            <%--6--%>
                                                                            <asp:ButtonField CommandName="RemoveRecord" HeaderText="Remove" Text="Remove">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:ButtonField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlTask" runat="server" Visible="False">
                                            <asp:UpdatePanel ID="upnlTaskList" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdsTaskDetails" class="clsFieldSetNewStyle">
                                                        <legend id="lgdTaskDetails" runat="server" style="font-weight: bold">Task Details</legend>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <table id="Table1">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddTask" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add" ToolTip="Click to Add Task" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnDelete" runat="server" AutoPostBack="True" CssClass="clsbtnH clsinfoH1"
                                                                                    Text="Delete" ToolTip="Click to Delete Task" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnCloseTask" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                                                    ToolTip="Click to go to the Previous Page" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table id="Table2">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:GridView ID="dgTaskList" runat="server" AutoGenerateColumns="False" CssClass="clsGrid"
                                                                                    PageSize="3" ShowHeaderWhenEmpty="True">
                                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                    <PagerStyle HorizontalAlign="Right" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle CssClass="clsdgHeader nodrag nodrop" />
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No.">
                                                                                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                Font-Underline="False" Wrap="False" />
                                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                                Font-Underline="False" Wrap="False" HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Task" HeaderText="Task">
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                                                                            <ItemStyle HorizontalAlign="Left" Width="500px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Note" HeaderText="Note">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:ButtonField CommandName="EditRecord" HeaderText="Edit" Text="Edit">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:ButtonField CommandName="RemoveRecord" HeaderText="Remove" Text="Remove">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:TemplateField HeaderText="Select">
                                                                                            <HeaderTemplate>
                                                                                                <input type="checkbox" id="chkSelectAll" />
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                                                    <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnSaveTasks" runat="server" ClientIDMode="Static" CssClass="clsbtnH clsinfoH1"
                                                                                    Text="Refresh" ToolTip="Click to save Tasks" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnBtnAddSelectTasks" ClientIDMode="Static" runat="server" Text="..."
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                <asp:Button ID="hdnBtnAddKit" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
                                                    Style="display: none;"></asp:Button>
                                                <asp:Button ID="hdnBtnAddTasks" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
                                                    Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
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

            <!--Tools Kit pop pup -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyAddKit" Text="Add Kit" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlAddKit" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IFrameMaintenanceKitStateComplete" frameborder="0" height="100%" width="100%"
                    src="JavaScript:''" scrolling="auto" allowtransparency="true"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupAddKit" runat="server" TargetControlID="btnDummyAddKit"
                PopupControlID="pnlAddKit" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameMaintenanceKitStateComplete() {
                    $("#btnDummyAddKit").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                function OpenToAddKit(URL) {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        //Edit Record
                        if (URL == "0") {
                            $("#IFrameMaintenanceKitStateComplete").attr("src", "wfMaintenanceKitDetail_Ajax.aspx?Type=pup");
                        }
                        //New Record
                        else if (URL == "1") {
                            $("#IFrameMaintenanceKitStateComplete").attr("src", "wfMaintenanceKitDetailMultipleItems_Ajax.aspx?Type=pup");
                        }

                        //                    $("#IFrameMaintenanceKitStateComplete").attr("src", "wfMaintenanceKitDetailMultipleItems_Ajax.aspx?Type=pup");
                        //                    $("#IFrameMaintenanceKitStateComplete").attr("src", decodeURIComponent(URL));
                        if (!$.browser.msie) {
                            $("#btnDummyAddKit").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForMaintenanceKit() {
                    var Kitwindow = $find("<%=mdlPopupAddKit.ClientID %>");
                    //close Service History popup window
                    Kitwindow.hide();
                    //           release resources
                    $("#IFrameMaintenanceKitStateComplete").attr("src", "JavaScript:''");
                    //call image button
                    $("#hdnBtnAddKit").click();
                }
            </script>
            <!--End Tools pop pup -->
            <%--call parent function after completing subroutine..(when page open as popup)--%>
            <script type="text/javascript">
                function CallParentCallback() {
                    parent.ParentCallBackFunctionForTools();
                    return false;
                }
            </script>
            <%--End--%>
            <div>
                <%--Set page layout when open as popup aspx page--%>
                <script type="text/javascript">

					<% Dim mopen As String = Request.QueryString("Type") %>
					<% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                    $(document).ready(function () {
                        SetPageLayout();
                        if ($.browser.msie) {
                            parent.IFrameToolsStateComplete();
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
                        var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                        var windowheight = $(window).height();
                        if (tempMargtop >= windowheight) {
                            $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                        }
                        else {
                            var margintop = (windowheight / 2) - (tempMargtop / 2);
                            $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                        }

                    }
                </script>
                <%--End--%>
            </div>
            <!--Tasks pop pup -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyAddTasks" Text="Add Tasks" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlAddTasks" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IFrameMaintenanceTasksStateComplete" frameborder="0" height="100%" width="100%"
                    src="JavaScript:''" scrolling="auto" allowtransparency="true"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupAddTasks" runat="server" TargetControlID="btnDummyAddTasks"
                PopupControlID="pnlAddTasks" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameMaintenanceTasksStateComplete() {
                    $("#btnDummyAddTasks").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                function OpenToAddTasks() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFrameMaintenanceTasksStateComplete").attr("src", "wfMaintenanceTaskDetail_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyAddTasks").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForMaintenanceTasks() {
                    var Taskswindow = $find("<%=mdlPopupAddTasks.ClientID %>");
                    //close Service History popup window
                    Taskswindow.hide();
                    //           release resources
                    $("#IFrameMaintenanceTasksStateComplete").attr("src", "JavaScript:''");
                    //call image button
                    $("#hdnBtnAddTasks").click();
                }
            </script>
            <!--End Tasks pop pup -->
            <!--SelectTasks pop pup -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyAddSelectTasks" Text="Add SelectTasks" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlAddSelectTasks" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IFrameSelectTasksStateComplete" frameborder="0" height="100%" width="100%"
                    src="JavaScript:''" scrolling="auto" allowtransparency="true"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupAddSelectTasks" runat="server" TargetControlID="btnDummyAddSelectTasks"
                PopupControlID="pnlAddSelectTasks" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameSelectTasksStateComplete() {
                    $("#btnDummyAddSelectTasks").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                function OpenToAddSelectTasks() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFrameSelectTasksStateComplete").attr("src", "wfSelectTaskCardList_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyAddSelectTasks").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForSelectTasks() {
                    var SelectTaskswindow = $find("<%=mdlPopupAddSelectTasks.ClientID %>");
                    //close Service History popup window
                    SelectTaskswindow.hide();
                    //           release resources
                    $("#IFrameSelectTasksStateComplete").attr("src", "JavaScript:''");
                    //call image button
                    $("#hdnBtnAddSelectTasks").click();
                }
            </script>
            <!--End SelectTasks pop pup -->
        </div>
    </form>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#btnSaveTasks").live("click", function () {
                var index = new Array();
                var srno = new Array();
                $("#<%= dgTaskList.ClientID %> tr:not(:first)").each(function (i) {
                    index[i] = i;
                    srno[i] = $(this).find("td:first").html();
                });
                var myobj = new Object();
                myobj.SrNo = srno;
                myobj.index = index;
                var myData = "{Ids:" + JSON.stringify(myobj) + "}";
                $.ajax({
                    url: "wfMaintenanceKitandTask_Ajax.aspx/GetTableIDs",
                    data: myData,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //$("#" + ID).html(data.d).slideDown("medium");
                        //alert(data);
                    },
                    error: function (data, status, jqXHR) {// $("#" + ID).html(status);
                    }
                });
                return true;
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%= dgTaskList.ClientID %>").tableDnD({
                scrollAmount: 5,
                onDragClass: "GbiHighlight",
                onDrop: function (table, row) {
                    var rows = table.tBodies[0].rows;
                    var myobj = new Object();
                    myobj.SrNo = "1";
                    myobj.index = "0";

                    var myData = "{Ids:" + JSON.stringify(myobj) + "}";
                    var data = $.tableDnD.serialize();
                },
                onDragStart: function (table, row) {
                    $("#debugArea").html("Started dragging row " + row.id);
                }
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).parents('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor');
                else
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
            })
                // the each is used when postback is triggered with checked rows
                .each(function (index, element) {
                    var checked = $(element).prop('checked');
                    if (checked == true)
                        $("td", $(this).closest("tr")).addClass('clslightColor');
                    else
                        $("td", $(this).closest("tr")).removeClass('clslightColor');
                });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });
        });
    </script>
</body>
</html>
