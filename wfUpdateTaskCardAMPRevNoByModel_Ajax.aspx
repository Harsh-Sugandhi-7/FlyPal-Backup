<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateTaskCardAMPRevNoByModel_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateTaskCardAMPRevNoByModel_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update AMP Issue/Rev No.</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Table1" class="clstablelistout">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="Table2" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTaskCardList" runat="server" CssClass="clsFormHeader">Update AMP Issue/Rev No.</asp:Label>
                                            </td>

                                            <td colspan="2" align="right">
                                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnUpdateAMPNo" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                                                        ValidationGroup="valGroup1" ToolTip="Click to Update AMP Issue/Rev No."></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close List screen"
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
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvNameLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="AMP Issue/Rev No. sholud not be greater than 150 characters."
                                        Display="None" ControlToValidate="txtNewAMPNo" ClientValidationFunction="validateName"
                                        ValidationGroup="valGroup1"></asp:CustomValidator>
                                    <script type="text/javascript">


                                        function validateName(source, args) {
                                            //args.IsValid = false;
                                            var ControlName = source.controltovalidate;
                                            var Value = $get(ControlName).value.length;
                                            if (Value > 150) {
                                                args.IsValid = false;
                                                return
                                            }
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <span id="lblModelName" class="clsLabelAuto">Model Name</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                    AutoPostBack="true" ClientIDMode="Static" DataTextField="ModelName">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblExistingAMPNo" class="clsLabelAuto">Existing AMP Issue/Rev No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExistingAMPNo" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                    MaxLength="150" TextMode="MultiLine" ToolTip="Existing AMP Issue/Rev No."
                                                    ReadOnly="true" BackColor="#E0E0E0"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblNewAMPNo" class="clsLabelAuto">New AMP Issue/Rev No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNewAMPNo" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                    MaxLength="150" TextMode="MultiLine" ToolTip="New AMP Issue/Rev No."></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="right">
                                    <asp:Button ID="btnUpdateAMPNo" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                        ValidationGroup="valGroup1" ToolTip="Click to Update AMP Issue/Rev No."></asp:Button>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Task Cards as per criteria : Record(s) found.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="dgTaskCardList" runat="server" ShowHeaderWhenEmpty="true"
                                        OnSorting="dgTaskCardList_Sorting" AllowSorting="True" 
                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False">
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                            <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No." SortExpression="TaskCardNo">
                                                <ItemStyle Wrap="False" />
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TaskDesc" HeaderText="Description/Subject" SortExpression="TaskDesc">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="INSPTypeInterval" HeaderText="INSP. Type Interval" SortExpression="INSPTypeInterval">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AMPIssueRev" HeaderText="AMP Issue/Rev" SortExpression="AMPIssueRev">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="true" CssClass="TextBreak" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <%--<td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnUpdateAMPNo" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                        ValidationGroup="valGroup1" ToolTip="Click to Update AMP Issue/Rev No."></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close List screen"
                                                CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    </form>
</body>
</html>
