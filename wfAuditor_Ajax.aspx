<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditor_Ajax.aspx.vb"
    Inherits="Flypal.wfAuditor_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lead Auditor</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlAuditor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblInner" class="clstablelistin">
                                <tr>

                                    <td colspan="2" class="clsFormHeader1">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Lead Auditor [New]</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                    ToolTip="Click to add the new Lead Auditor" Text="New"></asp:Button>
                                                            </td>
                                                            <td>

                                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to save the Lead Auditor Information"
                                                                    Text="Save"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                            Text="Close" ToolTip="Click to close Lead Auditor screen"></asp:Button>
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
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                            ErrorMessage="Lead Auditor Name Required" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                            ErrorMessage="Lead Auditor Name should not be greater than 100 characters" Display="None"
                                            ClientValidationFunction="validateNameLen"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateNameLen(source, args) {
                                                args.IsValid = false;
                                                var nameLength = $get("txtName").value.length;
                                                if (nameLength <= 100) {
                                                    args.IsValid = true;
                                                    return;
                                                }

                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset id="fdswodetail" class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="ldwodetail" runat="server"><b>Lead Auditor Details</b></legend>
                                            <table>
                                                <tr>
                                                    <%--<td align="right" colspan="3">
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                            ToolTip="Click to add the new Lead Auditor" Text="New"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblName1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td style="width: 100px">
                                                        <span id="lblName" class="clsLabelAuto">Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Lead Auditor"
                                                            Text="<%# mAuditor.Name %>" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesignation" runat="server" CssClass="clsLabelAuto">Designation</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table id="Table15" cellspacing="1" cellpadding="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDesignationList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataTextField="Name" DataValueField="ID" SelectedValue="<%# mAuditor.DesignationID %>"
                                                                        Width="277px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="imgbtnDesignation" runat="server" ImageUrl="~/images/plus1.png"
                                                                        Height="22px" Width="24px" ToolTip="Click to Add New Designation" CausesValidation="False"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="Label39" class="clsLabelAuto">Work Status</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkWorkingStatus" runat="server" CssClass="clsCheckBox" Checked="<%# mAuditor.IsNotWorking %>"
                                                             Text="(Check in case not Working)"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- <td align="right" colspan="3">
                                                        <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to save the Lead Auditor Information"
                                                            Text="Save"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Auditor List</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="dgAuditorList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False" DataKeyNames="ID"
                                            AllowSorting="true" ShowHeaderWhenEmpty="true" CellPadding="5" GridLines="Horizontal">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="AuditorID"></asp:BoundField>
                                                <asp:BoundField DataField="Name" HeaderText="Lead Auditor" SortExpression="Name">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DesignationName" HeaderText="Designation" SortExpression="DesignationName">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="WorkingStatus" HeaderText="Working Status" SortExpression="WorkingStatus">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>

                                                <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>--%>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%-- <span id="button">Login</span>--%>
                                                        <div class="dropdown">
                                                            <div class="dropdownbtn-content">
                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" CausesValidation="false" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
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
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td colspan="2" align="right">
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                    Text="Close" ToolTip="Click to close Lead Auditor screen"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnimgBtnDesignation" runat="server" CausesValidation="false" ClientIDMode="Static"
                                                    Style="display: none;" Text="Add" />
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForAuditor();
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
                    parent.IFrameAuditorStateComplete();
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
        <!-- Designation Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDesignation" Text="Dummy Designation" ClientIDMode="Static"
                CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupDesignation" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupDesignation" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupDesignation" runat="server" TargetControlID="btnDummyDesignation"
            PopupControlID="pnlPopupDesignation" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameDesignationStateComplete() {
                $("#btnDummyDesignation").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenAuditDesignationWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupDesignation").attr("src", "wfAuditDesignation_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyDesignation").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForDesignation() {
                var Designationwindow = $find("<%=mdlPopupDesignation.ClientID %>");
                //close Designation popup window
                Designationwindow.hide();
                $("#iPopupDesignation").attr("src", "JavaScript:''");
                //call Designation image button
                $("#hdnimgBtnDesignation").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
