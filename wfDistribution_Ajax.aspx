<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDistribution_Ajax.aspx.vb"
    Inherits="Flypal.wfDistribution_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Distribution List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <asp:UpdatePanel ID="upnlDistributionList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clsTableListIn">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Distribution List</asp:Label>
                                                    </td>

                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                        ToolTip="Click to add the new Distribution" Text="New"></asp:Button>
                                                                </td>

                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save the Distribution"
                                                                        Text="Save"></asp:Button>
                                                                </td>

                                                                <td>
                                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                     ToolTip="Click to close Distribution List screen" Text="Close"></asp:Button>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </td>

                                                </tr>
                                            </table>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvModel" runat="server" CssClass="clsLabelAuto" ErrorMessage="Model Required"
                                                ClientValidationFunction="ValidateModel" Display="None"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabel" ControlToValidate="txtName"
                                                Display="None" ErrorMessage="Distribution Name Required."></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" ErrorMessage="Max Lenght of Distribution  should be 100 chars."
                                                ClientValidationFunction="ValidateName" Display="None"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="3">
                                            <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:Label>
                                        </td>
                                        <td align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                            ToolTip="Click to add the new Distribution" Text="New"></asp:Button>
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save the Distribution"
                                                            Text="Save"></asp:Button>
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="Button1" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                            ToolTip="Click to close Distribution List screen" Text="Close"></asp:Button>
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblDistributionDetails" runat="server" CssClass="clsLabelHeader">Distribution Details</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblPartNoStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblmodel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                DataTextField="ModelName" SelectedValue="<%# mDistribution.ModelID %>">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDescriptionStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtName" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" ToolTip="Enter Distribution Name."
                                                Text="<%# mDistribution.Name %>" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCategory" runat="server" CssClass="clsLabelAuto">Category</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                <asp:ListItem Text="(SELECT)"   Value="(SELECT)"></asp:ListItem>
                                                <asp:ListItem Text="Internal" Value ="Internal"></asp:ListItem>
                                                <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="right">
                                           
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" ToolTip="Enter Remark Name."
                                                Text="<%# mDistribution.Remark %>" TextMode="MultiLine" MaxLength="499">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="3">
                                            <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save the Distribution"
                                                Text="Save"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Click to Copy Distribution List from Existing Model to another Model</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left">
                                            <asp:LinkButton ID="lnkCopyDistribution" runat="server" CssClass="clsLinkButton"
                                                CausesValidation="False">Copy Distribution List</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Search by Name and Description</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSearchModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbSearchModelList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                DataValueField="ID" DataTextField="ModelName">
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="top" align="right">
                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                ToolTip="Click to find the list of Distribution as per the searching criteria"
                                                Text="Find Now"></asp:Button>--%>

                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                            CausesValidation="False"    ToolTip="Click to find list as per searching criteria" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSearchName" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSearchDesc" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle"
                                                ToolTip="Enter Distribution Name." TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="dgDistribution" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" PageSize="25" ShowHeaderWhenEmpty="true"
                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                    NextPageText="" PreviousPageText="" />
                                                <PagerStyle HorizontalAlign="Right" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="ModelID" HeaderText="ModelID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                        <HeaderStyle/>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="CategoryName" HeaderText="Category" SortExpression="CategoryName">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                      <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark">
                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField HeaderText="Edit/View" CommandName="EditRec" Text="Edit/View"></asp:ButtonField>
                                                    <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete"></asp:ButtonField>--%>

                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>

                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" CausesValidation="false"/>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" CausesValidation="false"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </td>
                                        <%--<td valign="bottom" align="right">
                                            <table id="Table2" border="0" cellspacing="0" cellpadding="0" align="right" height="100%">
                                                <tr>
                                                    <td valign="top" align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                            ToolTip="Click to close Distribution List screen" Text="Close" 
                                                            Visible="<%# mDistributionList.Count >25 %>">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="bottom" align="right">
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                            ToolTip="Click to close Distribution List screen" Text="Close"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="4" align="right">
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                ToolTip="Click to close Distribution List screen" Text="Close"></asp:Button>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
            runat="server">
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
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        function ValidateModel(source, args) {
            args.IsValid = false;
            var dd = document.getElementById("cmbModelList");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }


        function ValidateName(source, args) {
            args.IsValid = false;
            var txt = $get("txtName").value;
            var len = txt.length;
            if (len < 100) {
                args.IsValid = true;
                return;
            }
        } 
        
    </script>
    </form>
</body>
</html>
