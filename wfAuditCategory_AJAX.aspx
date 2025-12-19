<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditCategory_AJAX.aspx.vb"
    Inherits="Flypal.wfAuditCategory_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Category</title>
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>

                                 <td colspan="2" class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Task Category [New]</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                             <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ClientIDMode="Static"
                                                                ToolTip="Click to add the new Task Category" CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ValidationGroup="a"
                                                                        Text="Save" ToolTip="Click to save the Task Category Information"></asp:Button>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>

                                                            <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                        ToolTip="Click to close Task Category screen" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ValidationGroup="a" ErrorMessage="Name Required" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ValidationGroup="a" ErrorMessage="Task Category Name should not be greater than 100 characters."
                                                ControlToValidate="txtName" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvIdentificationNo" runat="server" CssClass="clsLabelAuto"
                                                ValidationGroup="a" Display="None" ErrorMessage="Identification No. Required"
                                                ControlToValidate="txtIdentificationtxtNo"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvStandard" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ValidationGroup="a" ErrorMessage="Standard Required" ControlToValidate="cmbStandard"
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlTaskDet" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsTaskdetail" class="clsFieldSet" style="border-width: 1px">
                                                <legend id="ldTaskdetail" runat="server"><b>Task Category Details</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="3">&nbsp;
                                                        </td>
                                                        <td align="right">
                                                            <%--<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="New" ClientIDMode="Static"
                                                                ToolTip="Click to add the new Task Category" CausesValidation="False"></asp:Button>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblStandard" runat="server" CssClass="clsLabelAuto">Audit Standard</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <table id="Table11">
                                                                <tr>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlStandard" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="cmbStandard" CssClass="clsTextBoxTagSearchComboNewstyle" runat="server"   Enabled="False"
                                                                                    DataValueField="ID" DataTextField="Name" SelectedValue="<%# mAuditCategory.AuditStandardID %>">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="imgbtnStandard" runat="server" ImageUrl="~/images/plus1.png" Visible="false"
                                                                            Height="22px" Width="24px" ToolTip="Click to Add New Audit Standard" CausesValidation="False"></asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Name</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mAuditCategory.Name %>"
                                                                ToolTip="Enter Task Category" MaxLength="100"></asp:TextBox>
                                                        </td>
                                                        <td align="right"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblIdentificationNo" runat="server" CssClass="clsLabelAuto" Width="99px">Identification No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIdentificationtxtNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mAuditCategory.IdentificationNo %>" ToolTip="Enter Identification No."
                                                                MaxLength="100"></asp:TextBox>
                                                        </td>
                                                        <td align="right"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;
                                                        </td>
                                                        <td align="right">
                                                            <%--<asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" ValidationGroup="a"
                                                                        Text="Save" ToolTip="Click to save the Task Category Information"></asp:Button>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>--%>
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
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Task Category List</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlCloseTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                Text="Close" ToolTip="Click to close Task Category screen" Visible="false" /> <%--Visible="<%# mAuditCategoryList.count>10 %>"--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgAuditCategoryList" runat="server" AutoGenerateColumns="False"
                                                Visible="true" CssClass="clsGridNewStyle" PageSize="3" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Task Category"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderStyle  ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="AuditStandardName" SortExpression="AuditStandardName"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Audit Standard">
                                                        <HeaderStyle  ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IdentificationNo" SortExpression="IdentificationNo" HeaderText="Identification No."
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderStyle  ></HeaderStyle>
                                                    </asp:BoundField>
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
                                <td align="right">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnAuditStandard" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                Style="display: none;" Text="----" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td valign="bottom" align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                ToolTip="Click to close Task Category screen" CausesValidation="False"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
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
                    parent.IFrameAuditCategoryStateComplete();
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
        <!-- AuditStandard Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAuditStandard" Text="Dummy AuditStandard"
                ClientIDMode="Static" CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupAuditStandard" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupAuditStandard" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAuditStandard" runat="server" TargetControlID="btnDummyAuditStandard"
            PopupControlID="pnlPopupAuditStandard" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAuditStandardStateComplete() {
                $("#btnDummyAuditStandard").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenStandardWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupAuditStandard").attr("src", "wfAuditStandard_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAuditStandard").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForAuditStandard() {
                var AuditStandardwindow = $find("<%=mdlPopupAuditStandard.ClientID %>");
                //close AuditStandard popup window
                AuditStandardwindow.hide();
                $("#iPopupAuditStandard").attr("src", "JavaScript:''");
                //call AuditStandard image button
                $("#hdnimgBtnAuditStandard").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
