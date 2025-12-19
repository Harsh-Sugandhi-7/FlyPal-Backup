<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskCardList_AJAX.aspx.vb"
    Inherits="Flypal.wfTaskCardList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script type="text/javascript" language="javascript" id="clientEventHandlersJS">
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
        <div>
        </div>
        <table id="tblmain" class="clstablelistout" border="1" cellspacing="1" cellpadding="1"
            width="100%">
            <tr>
                <td>
                    <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                        <tr>

                            <td class="clsFormHeader1Newstyle" colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTaskCardList" runat="server" CssClass="clsFormHeader">Task Card List</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="height: 25px" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table10" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                                    ToolTip="Click to add new Task Card" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Task Card List screen"
                                                                    CausesValidation="False"></asp:Button>
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
                            <td colspan="1">
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTaskCardNo" runat="server" CssClass="clsLabelAuto">Task Card No.</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTaskNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblModelName" runat="server" CssClass="clsLabelAuto">Model Name</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
                                                        DataTextField="ModelName">
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTaskCardDesc" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblInspTypeInterval" runat="server" Width="137px" CssClass="clsLabelAuto"
                                                        Height="16px">INSP. Type Interval </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInspTypeIntervalSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                        CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtInspTypeIntervalSearch_Autocomplete"
                                                        runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                        MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfTaskCardList_AJAX.aspx"
                                                        ServiceMethod="GetInspTypeIntervalSearchList" TargetControlID="txtInspTypeIntervalSearch"
                                                        OnClientItemSelected="" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                    </cc2:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsRII" runat="server" CssClass="clsLabelAuto" ClientIDMode="Static"
                                                        Text=' Show "IsRII" records' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="1" align="right">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel ID="UpnlFindNow" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now" ToolTip="Click to find List of Task Cards as per searching criteria"></asp:Button>--%>
                                                    <asp:ImageButton ID="imgFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                        ToolTip="Click to find List of Task Cards as per searching criteria"></asp:ImageButton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td colspan="2">
                                <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Width="872px" Height="24px">Select Task Card from the List. Click on Edit/View Link to Modify the selected Task Card. Click on Delete Link to Delete the selected Task Card. Click on Add New button to add a new Task Card. Click on View link to view the attached file of selected Task Card.</asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td style="height: 25px">
                                <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader" Width="376px">List of Task Cards as per criteria &nbsp : &nbsp  Record(s) found.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="height: 25px" align="right">
                                <%-- <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table10" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton" Text="Add New"
                                                        ToolTip="Click to add new Task Card" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to close Task Card List screen"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgTaskCardList" runat="server" CssClass="clsGridNewStyle"
                                            DataKeyNames="ID" ShowHeaderWhenEmpty="true" EnableViewState="true" AllowSorting="True"
                                            PageSize="25" AllowPaging="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                            <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField> <%--0--%>
                                                <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No."> 
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" Wrap="False" />
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--1--%>
                                                <asp:BoundField DataField="TaskDesc" HeaderText="Description/Subject">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--2--%>
                                                <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--3--%>
                                                <asp:BoundField DataField="AMPIssueRev" HeaderText="AMP Issue/Rev">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--4--%>
                                                <asp:BoundField DataField="INSPTypeInterval" HeaderText="INSP. Type Interval">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--5--%>
                                                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" Wrap="False" />
                                                </asp:BoundField> <%--6--%>
                                                <asp:BoundField DataField="RevNo" HeaderText="Revision No.">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--7--%>
                                                <asp:BoundField DataField="RevDate" HeaderText="Revision Date">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" Wrap="False" />
                                                </asp:BoundField> <%--8--%>
                                                <asp:BoundField DataField="Reference" HeaderText="Reference">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--9--%>
                                                <asp:BoundField DataField="Equipment" HeaderText="Equipment">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--10--%>
                                                <asp:BoundField DataField="Material" HeaderText="Material">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField> <%--11--%>
                                                <asp:BoundField DataField="EstimatedHours" HeaderText="Estimated Hr.">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField> <%--12--%>
                                                <asp:TemplateField HeaderText="Is RII">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkISRII" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsRII") %>'
                                                            Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField> <%--13--%>
                                                <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>--%>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%-- <span id="button">Login</span>--%>
                                                        <div class="dropdown">
                                                            <div class="dropdownbtn-content">
                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                        </td>

                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                Style="cursor: pointer" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table4" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton" Text="Add New" ToolTip="Click to add new Task Card"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to close Task Card List screen"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                    <asp:Button ID="hdnBtnTaskMaster" ClientIDMode="Static" runat="server" Text="..."
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
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
        <div>
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
        </div>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForTaskMaster();
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
                    $("#IframeTaskMaster").attr("src", "wfTaskCard_Ajax.aspx?Type=pup");

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
        <%--autocomplete css functions--%>
        <script type="text/javascript">
            //bold input value in list...
            function ClientPopulated(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
            //Alternate item style
            function ClientShowing(source, eventArgs) {
                $.elements = $(source.get_completionList());
                $.elements.find(".ac_results_li").each(function (i) {
                    if (i % 2 == 0) {
                        //$(this).addClass("ac_even");
                    }
                    else {
                        $(this).addClass("ac_odd");
                    }
                });
            }
            //add loader to textbox
            function ClientPopulating(source, e) {
                $("#" + source._element.id).addClass("ac_loading");
            }
            //remove loader from textbox
            function ClientHiding(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
        </script>
        <%--End--%>
    </form>
    <%-- <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtInspTypeIntervalSearch.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=InspTypeInterval', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>--%>
</body>
</html>
