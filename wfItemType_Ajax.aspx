<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfItemType_Ajax.aspx.vb"
    Inherits="Flypal.wfItemType_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part Type</title>
    <script language="javascript">
        function OpenLocation(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }   
    </script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="jPicker\jPicker.css">
    <link rel="stylesheet" type="text/css" href="jPicker\css\jpicker-1.1.6.min.css">
    <script type="text/javascript" src="jPicker\jpicker-1.1.6.js"></script>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        $(document).ready(function () {
            $('#<%=txtColor.ClientID%>').jPicker();
        });
      
    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td class="clsFormHeader1Newstyle">
                                                    <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Part Type [New]</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="1"></asp:ValidationSummary>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlItemTypeDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                                        ControlToValidate="txtName" ErrorMessage="Name Required." ValidationGroup="1">Name Required.</asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvNameLen" runat="server" CssClass="clsLabelAuto" Display="None"
                                                        ErrorMessage="Name is too long." ControlToValidate="txtName" ClientValidationFunction="validateName"
                                                        ValidationGroup="1"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cvPartStatus" runat="server" CssClass="clsLabelAuto" Display="None"
                                                        ErrorMessage="Select Part Status." ControlToValidate="cmbPartStatusList" ValidationGroup="1"
                                                        ClientValidationFunction="validatePartStatus"></asp:CustomValidator>
                                                    <script type="text/javascript">
                                                        function validatePartStatus(source, args) {
                                                            args.IsValid = false;

                                                            var dd = $get("cmbPartStatusList");
                                                            if (dd.selectedIndex != 0) {
                                                                args.IsValid = true;
                                                                return;
                                                            }
                                                        }

                                                        function validateName(source, args) {
                                                            var Value = $get("txtName").value.length;
                                                            if (Value > 25) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                        }
                                                    </script>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add New Part Type"
                                                        CausesValidation="False" Text="New"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblCategoryDetails" class="clsLabelHeader">Part Type Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <span id="lblName1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblName" class="clsLabelAuto">Name </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Name"
                                                        Text="<%# mPartType.Name %>" MaxLength="25">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td>
                                                    <span id="lblGLCode" class="clsLabelAuto">Code</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtGLCode" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter  Code"
                                                        Text="<%# trim(mPartType.Code) %>" MaxLength="4">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <span id="lblStarColor" class="clsLabelStar" style="display: none;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblColor" class="clsLabelAuto">Color</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtColor" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Select Color"
                                                        Text="<%# mPartType.Color %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <span id="Label2" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblPartStatus" class="clsLabelAuto">Part Status</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbPartStatusList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        DataValueField="PartStatusID" DataTextField="PartStatusName" SelectedValue="<%# mPartType.PartStatusID %>">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save"
                                                        Text="Save" ValidationGroup="1"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgCategory" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                        OnRowDataBound="OnRowDataBound" ShowHeaderWhenEmpty="True" AllowPaging="True" 
                                                        PageSize="25" AllowSorting="True" DataKeyNames="ID" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                      <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="PartType ID"></asp:BoundField>
                                                            <asp:BoundField DataField="Name" HeaderText="Part Type">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Code" HeaderText="Code">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Color">
                                                                <ItemStyle CssClass="clsColorLabel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PartStatusName" SortExpression="PartStatusName" HeaderText="Part Status">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                          <%--  <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ImgEditView" runat="server" CommandName="EditRec" Style="height: 15px;
                                                                                                width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px;
                                                                                                width: 20px" ImageUrl="~/images/delete.png" />
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
                                                            <asp:BoundField DataField="Color" HeaderStyle-CssClass="hideGridColumn" HeaderText="Color"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                         <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="right" width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                        Text="Close" ToolTip="Click to close the Part Type screen" />
                                                </td>
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
  <%--  <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </asp:UpdateProgress>--%>
    </form>
    <%--<script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('#<%=txtColor.ClientID%>').jPicker();
        });
      
    </script>--%>
    <script type="text/javascript">
        function ParentCallBackFunction() {
            $('#<%=txtColor.ClientID%>').jPicker();
        }
    </script>
</body>
</html>
