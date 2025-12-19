<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectTaskCardList_Ajax.aspx.vb"
    Inherits="Flypal.wfSelectTaskCardList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Task Card List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript" id="clientEventHandlersJS">
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
    <style type="text/css">
        .GbiHighlight {
            background-color: Aqua;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.cbSelectRow').change(function () {
                    // detect if the checkbox is checked
                    var checked = $(this).prop('checked');
                    // gets the table row indiect parent
                    var trParent = $(this).closest('tr');
                    // add or remove the css class according to the check state
                    if (checked == true)
                        trParent.addClass('clslightColor')
                    else
                        trParent.removeClass('clslightColor');
                })
                    // the each is used when postback is triggered with checked rows
                    .each(function (index, element) {
                        var checked = $(element).prop('checked');
                        if (checked == true)
                            $(element).closest('tr').addClass('clslightColor');
                        else
                            $(element).closest('tr').removeClass('clslightColor');
                    });
                // select all click
                $("#chkSelectAll").change(function () {
                    var checked = $(this).prop('checked');
                    $('.cbSelectRow').prop('checked', checked).trigger('change');
                });


            });

        </script>
        <!-- End-->
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <table class="clstablelistin" id="Table2" cellspacing="0" cellpadding="0" border="0">
                        <tr>

                            <td class="clsFormHeader1" colspan="2">
                                <table width ="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTaskCardList" class="clsFormHeader" runat="server"> Task Card List</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlJobTask" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAddWOJobTask" runat="server" CssClass="clsbtnH clsinfoH" Text="Add Task Manually" Width="134px"
                                                        ToolTip="Click to Add Task"></asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="14px"></td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset id="fdsTaskCardInfo" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblTaskCardInfo" runat="server" style="font-weight: bold"><b>Search Criteria</b></legend>
                                    <asp:UpdatePanel ID="upnlTaskCardOnfo" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="1">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto" Width="88px">Task Card No. </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTaskCardNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"
                                                                                    ToolTip="Enter Task Card No."></asp:TextBox>
                                                                            </td>
                                                                            <td align="right" style="padding: 0px;">
                                                                                <asp:ImageButton ID="imgTaskCard" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                    Width="24px" CausesValidation="False" ToolTip="Click to Add New Task Card" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblModelList" runat="server" Width="50px" CssClass="clsLabelAuto">Model </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
                                                                        DataTextField="ModelName">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lblInspTypeInterval" runat="server" Width="137px" CssClass="clsLabelAuto"
                                                                        Height="16px">INSP. Type Interval </asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:TextBox ID="txtInspTypeIntervalSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                                                        MaxLength="150" ToolTip="Enter INSP. Type Interval to Search"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td align="right">
                                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find List of Task Cards as per searching criteria"
                                                                    Text="Find Now"></asp:Button>--%>
                                                                    <asp:ImageButton ID="imgFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                        ToolTip="Click to find List of Task Cards as per searching criteria"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTaskCardInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsResult" class="clsFieldSet" style="border-width: 1px">
                                            <legend id="lblResult" runat="server" style="font-weight: bold"><b>List of Task Cards
                                            as per Model: Record(s) found.</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto">Select Task Card(s) from the List.</asp:Label>
                                                    </td>
                                                    <td align="right" class="style1">
                                                        <asp:Button ID="btnDone1" runat="server" CssClass="clsbtnH" ToolTip="Click to add the selected Task Cards to the previous forms."
                                                            Text="Done" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgTaskCardList" runat="server" AutoGenerateColumns="False" Visible="true"
                                                            CssClass="clsGridNewStyle" PageSize="3" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                           <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <input type="checkbox" id="chkSelectAll" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                            <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No.">
                                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                        Font-Underline="False" Wrap="False" />
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TaskDesc" HeaderText="Task Description /Subject">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="INSPTypeInterval" HeaderText="INSP. Type Interval">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RevNo" HeaderText="Revision No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="true"></HeaderStyle>
                                                                    <ItemStyle Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RevDate" HeaderText="Revision Date">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Reference" HeaderText="Reference">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Equipment" HeaderText="Equipment">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Material" HeaderText="Material"></asp:BoundField>
                                                                <asp:BoundField DataField="EstimatedHours" HeaderText="Estd. Hr.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <asp:Button ID="btnDone" runat="server" CssClass="clsbtnH" ToolTip="Click to add the selected Task Cards to the previous forms."
                                                            Text="Done" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" ID="upnlBtnTaskMaster" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnTaskMaster" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnAddJobTaskDetail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForSelectTasks();
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
                    parent.IFrameSelectTasksStateComplete();
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
        <!-- Task Master popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskMaster" Text="Task Master" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlTaskMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeTaskMaster" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskMaster" runat="server" TargetControlID="btnDummyTaskMaster"
            PopupControlID="pnlTaskMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskMasterStateComplete() {
                $("#btnDummyTaskMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenTaskMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeTaskMaster").attr("src", "wfTaskCardList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForTaskMaster() {
                var TaskMasterwindow = $find("<%=mdlPopupTaskMaster.ClientID %>");
                //close Task Master popup window
                TaskMasterwindow.hide();
                //release resources
                $("#IframeTaskMaster").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnTaskMaster").click();
            }
        </script>
        <!-- End-->
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

            function ParentCallBackFunctionForJobTaskDetail() {
                var JobTaskDetailWindow = $find("<%=mdlPopupJobTaskDetail.ClientID %>");
                //close JobTaskDetail popup window
                JobTaskDetailWindow.hide();
                $("#iPopupJobTaskDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddJobTaskDetail").click();
                parent.ParentCallBackFunctionForSelectTasks();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
