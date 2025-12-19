<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobResourceAllocation_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOJobResourceAllocation_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resource Allocation</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="popup.css">
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clsTableListIn" id="tblInner" border="0">
                            <tr>

                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Resource Allocation</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="UpnlAddTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table10">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddTop" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Add Resource"
                                                                        ValidationGroup="a" Text="Add" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close the Resource Allocation screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
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
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"
                                                Display="None" ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvResource" runat="server" ControlToValidate="cmbResource"
                                                CssClass="clsLabelAuto" Display="None" ClientValidationFunction="validateResource" ErrorMessage="Resource Required."
                                                ValidationGroup="a"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <script type="text/javascript">
                                        function validateResource(source, args) {
                                            args.IsValid = false;

                                            var dd = $get("cmbResource");
                                            if (dd.selectedIndex != 0) {
                                                args.IsValid = true;
                                                return;
                                            }
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlResource" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <table width="99%">
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td>
                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="clsLabelAuto">Designation</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="clsTextBoxTagSearch1" ToolTip="Designation"
                                                                Text="<%# mnWOJobDesignationAllocation.DesignationName %>" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td style="width: 87px">
                                                            <asp:Label ID="lblEstimatedManHours" runat="server" CssClass="clsLabel">Estimated Man Hours</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEstimatedManHours" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
                                                                ToolTip="Estimated Man Hours" Text="<%# mnWOJobDesignationAllocation.EstimatedTime %>"
                                                                ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" ForeColor="Red">*</asp:Label>
                                                        </td>
                                                        <td style="width: 87px">
                                                            <asp:Label ID="lblResource" runat="server" CssClass="clsLabelAuto">Resource</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbResource" runat="server" 
                                                                CssClass="clsTextBoxTagSearchCombo" DataTextField="EmpNoName"
                                                                DataValueField="ID" Enabled="<%# mnWO.WOStatusID <> 3 %>"
                                                                ValidationGroup="a" AutoPostBack="true" >
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td style="width: 87px">
                                                            <asp:Label ID="lblActualTime" runat="server" CssClass="clsLabelAuto">Actual Time</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtActualTime" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchRightAlign1"
                                                                ReadOnly="True" ToolTip="Actual Time"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:UpdatePanel ID="upnlResourceList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResourcelist" runat="server" CssClass="clsLabelHeader">Resource list</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <%--<asp:UpdatePanel ID="UpnlAddTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table10">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddTop" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to Add Resource"
                                                            ValidationGroup="a" Text="Add" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to close the Resource Allocation screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgResourceAllocation" runat="server" CssClass="clsGridNewStyle" ToolTip="List of Resource"
                                                ShowHeaderWhenEmpty="true" AllowSorting="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle ForeColor="black"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ResourceName" HeaderText="Resource">
                                                        <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WOTotalResourceActualTime" HeaderText="Actual Time"></asp:BoundField>
                                                    <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRecord"></asp:ButtonField>
                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRecord"></asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>

                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRecord" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>

                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:ButtonField Text="Add Resource Detail" HeaderText="Add Resource Detail" CommandName="AddResourceDetail">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="UpnlClose" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnClose" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to close the Resource Allocation screen"
                                                Text="Close" CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnAddResourceDetail" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div>
            <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        <!-- ResourceDetail Popup Window -->
        <%-- 'Added by Saylee on 29-May-2019--%>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyResourceDetail" Text="Dummy ResourceDetail"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupResourceDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupResourceDetail" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupResourceDetail" runat="server" TargetControlID="btnDummyResourceDetail"
            PopupControlID="pnlPopupResourceDetail" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameResourceAllocationStateComplete() {
                $("#btnDummyResourceDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddResourceDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupResourceDetail").attr("src", "wfnWOJobResourceDetail_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyResourceDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForResourceAllocation() {
                var ResourceDetailWindow = $find("<%=mdlPopupResourceDetail.ClientID %>");
                //close ResourceDetail popup window
                ResourceDetailWindow.hide();
                $("#iPopupResourceDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddResourceDetail").click();
            }
        </script>
        <!-- End-->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForResourceAllocation();
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
                    parent.IFrameResourceAllocationStateComplete();
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
    </form>
</body>
</html>
