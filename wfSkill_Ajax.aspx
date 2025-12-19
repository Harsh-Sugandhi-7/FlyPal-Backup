<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSkill_Ajax.aspx.vb"
    Inherits="Flypal.wfSkill_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="jquery-1.11.1.js" type="text/javascript"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Skill Master --ModalPopUp -->
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlSkillMaster">
                        <asp:UpdatePanel ID="upnlskillmast" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitleSkillMaster" CssClass="clsFormHeader" runat="server">Skill</asp:Label>
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAddSkillMaster" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                ToolTip="Click to add the new Skill" Text="New"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSaveSkillMaster" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save the Skill Information"
                                                                Text="Save" ValidationGroup="valGroup2"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCloseSkillMaster" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                ToolTip="Click to close Skill Screen" Text="Close"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSkillMaster" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdswodetail" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                                <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Skill Details</b></legend>
                                                <table class="clstablelistin" id="Table3">
                                                    <tr>
                                                        <td colspan="4"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                                Height="40px" ValidationGroup="valGroup2"></asp:ValidationSummary>
                                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Skill Name Required."
                                                                ControlToValidate="txtSkill" Display="None" ValidationGroup="valGroup2"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" CssClass="clsLabelAuto" ErrorMessage="Skill Code Required."
                                                                ControlToValidate="txtCode" Display="None" ValidationGroup="valGroup2"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td colspan="3">
                                                            <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnAddSkillMaster" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                ToolTip="Click to add the new Skill" Text="New"></asp:Button>
                                                        </td>
                                                    </tr>--%>
                                                    <%--'Added by Shital on 18-Aug-2016--%>
                                                    <tr>
                                                        <td>
                                                            <span id="Span2" class="clsLabelStar" style="color: Red;">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="Span1" class="clsLabel">Code</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Skill Code"
                                                                Text="<%# mSkill.Code %>" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Label2" class="clsLabelStar" style="color: Red;">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblName" class="clsLabel">Name</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSkill" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Skill"
                                                                Text="<%# mSkill.Name %>" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td align="right"></td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td colspan="3">
                                                            <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSaveSkillMaster" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to Save the Skill Information"
                                                                Text="Save" ValidationGroup="valGroup2"></asp:Button>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td colspan="4">
                                                            <span id="lblSearch" class="clsLabelHeader">Skill List</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <div>
                                                                <asp:GridView ID="dgSkill" runat="server" CssClass="clsGridNewStyle" ToolTip="Skill List" 
                                                                    AutoGenerateColumns="False" DataKeyNames="ID" Width="100%" OnSortCommand="dgMachineList_SortCommand"
                                                                    AllowPaging="True" PageSize="5" GridLines="Horizontal" CellPadding="5" >
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                                    <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                                  <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                        <%-- 'Added by Shital on 18-Aug-2016--%>
                                                                        <asp:BoundField DataField="Code" HeaderText="Skill Code">
                                                                            <HeaderStyle HorizontalAlign="left" />
                                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Name" HeaderText="Skill Name">
                                                                            <HeaderStyle HorizontalAlign="left" />
                                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="140px" Wrap="true" />
                                                                        </asp:BoundField>
                                                                        <%-- <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                    CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                                    Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>--%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <%-- <span id="button">Login</span>--%>
                                                                                <div class="dropdown">
                                                                                    <div class="dropdownbtn-content">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="EditView" runat="server"  CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                         CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
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
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td align="right" colspan="4">
                                                            <asp:Button ID="btnCloseSkillMaster" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                ToolTip="Click to close Skill Screen" Text="Close"></asp:Button>
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    </asp:Panel>
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
                parent.ParentCallBackFunctionForSkill();
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
                    parent.IFrameSkillStateComplete();
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
    </form>
</body>
</html>
