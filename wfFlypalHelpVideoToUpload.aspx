<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFlypalHelpVideoToUpload.aspx.vb"
    Inherits="Flypal.wfFlypalHelpVideoToUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flypal Help Video To Upload</title>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Flypal Help Video To Upload</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="a" />
                                                    <asp:RequiredFieldValidator ID="rfVideoName" runat="server" ControlToValidate="txtVideoName"
                                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Video Name Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvVideoPath" runat="server" ControlToValidate="txtVideoPath"
                                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Video Path Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvCc" runat="server" Display="None" ControlToValidate="txtVideoPath"
                                                        ErrorMessage="Please Enter Valid path" CssClass="" ClientValidationFunction="validURL"
                                                        ValidationGroup="a"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="rfvThumbnailPath" runat="server" ControlToValidate="txtThumbnailPath"
                                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Thumbnail Path Required"
                                                        ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <%--<asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ControlToValidate="txtThumbnailPath"
                                                        ErrorMessage="Please Enter Thumbnail path" CssClass="" ClientValidationFunction="validURL"
                                                        ValidationGroup="a"></asp:CustomValidator>--%>
                                                    <asp:HiddenField ID="hdnValue" runat="server" ClientIDMode="Static" />
                                                    <script type="text/javascript">
                                                        function validURL(source, args) {
                                                            var text = $("#txtVideoPath").val();
                                                            var pattern = new RegExp('^((https?:)?\\/\\/)?' + // protocol
                                                                            '(?:\\S+(?::\\S*)?@)?' + // authentication
                                                                            '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
                                                                            '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
                                                                            '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
                                                                            '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
                                                                            '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locater
                                                            var seperator = ',';
                                                            if (!pattern.test(text)) {
                                                                args.IsValid = false;
                                                                $("#hdnValue").val(args.IsValid);
                                                                return;
                                                            }
                                                            else {
                                                                args.IsValid = true;
                                                                $("#hdnValue").val(args.IsValid);
                                                                return;
                                                            }
                                                        }
                                                    </script>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblVideoNameStar" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblVideoName" class="clsLabel">Video Name</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVideoName" runat="server" CssClass="clsTextBoxTextSearch_Ajax"
                                                        MaxLength="1000"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblVideoPathStar" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblVideoPath" class="clsLabel">Video Path</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVideoPath" runat="server" CssClass="clsTextBoxTextSearch_Ajax"
                                                        MaxLength="1000"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblThumbnailPathStar" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblThumbnailPath" class="clsLabel">Thumbnail Path</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtThumbnailPath" runat="server" CssClass="clsTextBoxTextSearch_Ajax"
                                                        MaxLength="1000"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblDescription" class="clsLabel">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTextSearch_Ajax"
                                                        MaxLength="1000"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblModuleDescription" class="clsLabel">Module Description</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbModuleName" runat="server" CssClass="clsComboBox2_Ajax"
                                                        DataTextField="DescriptionMainMenu" DataValueField="ModuleID" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save"
                                                        ValidationGroup="a" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                CssClass="clsGrid" PageSize="25" ShowHeaderWhenEmpty="True" DataKeyNames="ID,VideoName,VideoPath,Description,ThumbnailPath,ModuleID">
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle HorizontalAlign="Right" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="VideoName" HeaderText="Video Name">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="VideoPath" HeaderText="Video Path">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ThumbnailPath" HeaderText="Thumbnail Path">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Module Description" HeaderText="Module Description" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--5--%>
                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <%--6--%>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            Text="Close" ToolTip="Click to close" />
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
