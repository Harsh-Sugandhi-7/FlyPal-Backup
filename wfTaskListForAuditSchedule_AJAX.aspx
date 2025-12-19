<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskListForAuditSchedule_AJAX.aspx.vb"
    Inherits="Flypal.wfTaskListForAuditSchedule_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table class="clstablelistin" id="tblInner">
                            <tr>

                                <td colspan="2" class="clsFormHeader1">
                                        <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Task List For Schedule</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add the selected records in Task List"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"
                                                                        CausesValidation="False" ToolTip="Click to go back to the previous page"></asp:Button>
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
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlTask" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblNewTask" runat="server" CssClass="clsLabelAuto">Add New Task : </asp:Label>
                                                    </td>
                                                    <td align="right" style="padding: 0px;">
                                                        <asp:ImageButton ID="imgTask" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                            Width="24px" CausesValidation="False" ToolTip="Click to Add New Task" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend id="Legend1" runat="server"><b>Search Information</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table id="Table4" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="L1" runat="server" Width="12px" CssClass="clsLabelauto"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblSearchBy" runat="server" Width="112px" CssClass="clsLabelAuto"
                                                                            Height="16px">Search By</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                            <asp:ListItem Value="1">Task Category</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="L2" runat="server" Width="10px" CssClass="clsLabelAuto"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbTaskCategorySearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                            DataTextField="Name" DataValueField="ID" Visible="False">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <table id="Table6">
                                                                <tr>
                                                                    <td>
                                                                       <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                            ToolTip="Click to find Task List as per searching criteria" Text="Find Now"></asp:Button>--%>
                                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"            
                                                                         ToolTip="Click to find Task List as per searching criteria"  
                                                                        CausesValidation="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButtonsTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnOKTop" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to add the selected records in Task List"
                                                            Visible="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBackTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            Text="Back" CausesValidation="False" ToolTip="Click to go back to the previous page"
                                                            Visible="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskList" runat="server" AutoGenerateColumns="False" Visible="true"
                                                CssClass="clsGridNewStyle" PageSize="3" ShowHeaderWhenEmpty="true" CellPadding="10" GridLines="Horizontal">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                               <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="AuditCategoryName" HeaderText="Task Category" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="Code" HeaderText="Code" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="150px"
                                                        HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-Width="150px" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <input type="checkbox" id="chkSelectAll" />
                                                        </HeaderTemplate>
                                                        <%-- <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" class="cbSelectRow" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>--%>
                                                        <ItemTemplate>
                                                            <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%-- <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to add the selected records in Task List"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Back"
                                                            CausesValidation="False" ToolTip="Click to go back to the previous page"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnTaskMasterChapter" runat="server" CausesValidation="false"
                                                ClientIDMode="Static" Style="display: none;" Text="Add" />
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
                parent.ParentCallBackFunction();
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
                    parent.IFrameTaskMasterStateComplete();
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
        <!-- TaskMaster Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskMaster" Text="Dummy TaskMaster" ClientIDMode="Static"
                CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupTaskMaster" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupTaskMaster" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskMaster" runat="server" TargetControlID="btnDummyTaskMaster"
            PopupControlID="pnlPopupTaskMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskMasterDetailStateComplete() {
                $("#btnDummyTaskMaster").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenTaskMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupTaskMaster").attr("src", "wfTask_AJAX.aspx?Type=pup&AType=3");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var TaskMasterwindow = $find("<%=mdlPopupTaskMaster.ClientID %>");
                //close TaskMaster popup window
                TaskMasterwindow.hide();
                $("#iPopupTaskMaster").attr("src", "JavaScript:''");
                //call TaskMaster image button
                $("#hdnimgBtnTaskMasterChapter").click();
            }

        </script>
        <!-- End-->
    </form>
    <%--  <script type="text/javascript">
        function ColorChange() {
            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).parents('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor')
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

        }
    </script>--%>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).parents('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor')
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
