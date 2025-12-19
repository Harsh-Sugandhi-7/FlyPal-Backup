<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateATAChapterOfItems_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateATAChapterOfItems_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Change Part ATA/Location/Applicability</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
        runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">

                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblPartsList" class="clsFormHeader">Change Part ATA/Location/Applicability</span>
                                        </td>
                                        <td colspan="4" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional" Visible="false">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnPrevious" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    Text="Previous" ToolTip="Click to update ATA Chapter/Location/Applicability of Items and move to Previous  screen."
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSavenNext" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    ClientIDMode="Static" Text="Next" ToolTip="Click to update ATA Chapter/Location/Applicability of Items and move to Next  screen."
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                    ToolTip="Click to close Change Part ATA/Part Location/Applicability screen" CausesValidation="False"></asp:Button>
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
                            <td>
                                <span class="clsLabelAuto">Part No.</span>
                                <asp:TextBox ID="txtPartName" runat="server" CssClass="clsTextBoxSearch_Ajax"></asp:TextBox>
                            </td>
                            <td>
                                <span id="lblCategory" class="clsLabelAuto">Category</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataValueField="ID"
                                    DataTextField="Name">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkBlankLocation" runat="server" CssClass="clsCheckBox" Text="Show Items Without Location"
                                    Checked="true" />
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                        ToolTip="Click to Find" CausesValidation="False"></asp:Button>--%>

                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" 
                                                        CssClass="clsSearch2btn" ToolTip="Click to find"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Parts as per criteria : Record(s) found</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--<td colspan="4" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrevious" runat="server" CssClass="clsButtonLong_Ajax" Width="105px"
                                                        Text="Previous" ToolTip="Click to update ATA Chapter/Location/Applicability of Items and move to Previous  screen."
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSavenNext" runat="server" CssClass="clsButtonLong_Ajax" Width="105px"
                                                        ClientIDMode="Static" Text="Next" ToolTip="Click to update ATA Chapter/Location/Applicability of Items and move to Next  screen."
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                        ToolTip="Click to close Change Part ATA/Part Location/Applicability screen" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <asp:UpdatePanel ID="upnlgrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgItemsList" runat="server"  AutoGenerateColumns="False"
                                            ClientIDMode="Static" ShowHeaderWhenEmpty="true"
                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ID"></asp:BoundField>
                                                <asp:TemplateField Visible="False" HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemName" SortExpression="AuthorizedBy" HeaderText="Part No.">
                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CategoryName" HeaderText="Category">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="ATA Chapter">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="cmbATAList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ATAChapter"
                                                            EnableViewState="false" DataValueField="ID" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"ATAID") %>'
                                                            ClientIDMode="Static" DataSource="<%# mATAList %>">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLocation" CssClass="clsTextBoxTagSearchSmall" runat="server" MaxLength="15"
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"Location") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemApplicable" HeaderText="Item Applicable">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Applicability">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" EnableViewState="false"
                                                            DataTextField="ModelName" DataValueField="Id" DataSource="<%# mModelList %>"
                                                            ClientIDMode="Static">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPreviousBottom" runat="server" CssClass="clsbtnH clsinfoH1"
                                                        Text="Previous" ToolTip="Click to update ATA Chapter/Location/Applicability of Items and move to Previous  screen."
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSavenUpdateBottom" runat="server" CssClass="clsbtnH clsinfoH1"
                                                         Text="Next" ToolTip="Click to update ATA Chapter/Location/Applicability of Items and move to Next  screen."
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to close Change Part ATA/Part Location/Applicability screen"
                                                        CausesValidation="False"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <!-- hidden fields to set combobox selected values at client side -->
    <asp:HiddenField ID="hdnATAIDValueList" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnModelIDValueList" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnATANameValueList" runat="server" ClientIDMode="Static" />
    <!-- End-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnSavenNext,#btnSavenUpdateBottom,#btnPrevious,#btnPreviousBottom").live('click', function () {
                try {
                    setIDs();

                } catch (e) {
                    alert(e.Message);
                }
                return true;

            });

            function setIDs() {
                var ATAIDList = new Array();
                var ATANameList = new Array();
                var ModelIDList = new Array();
                $('#<%=dgItemsList.ClientID %>').find("[id*=cmbATAList]").each(function () {
                    var ID = $(":selected", this).val();
                    var Text = $(":selected", this).text();
                    ATAIDList.push(ID);
                    ATANameList.push(Text);
                });

                $('#<%=dgItemsList.ClientID %>').find("[id*=cmbModelList]").each(function () {
                    var ID = $(":selected", this).val();
                    ModelIDList.push(ID);
                });

                $("#hdnATAIDValueList").val('');
                $("#hdnATAIDValueList").val(ATAIDList);

                $("#hdnATANameValueList").val('');
                $("#hdnATANameValueList").val(ATANameList);

                $("#hdnModelIDValueList").val('');
                $("#hdnModelIDValueList").val(ModelIDList);
            }
        });
    </script>
    <!-- javascript function to set combobox selected value to appropriate hidden field for Part Information-->
    <script type="text/javascript">
        function setComboBoxValue(elem, combo) {
            switch (combo) {
                //ATA                       
                case 'ATA':
                    var id = $(":selected", elem).val();
                    var text = $(":selected", elem).text();
                    //set id to hidden field
                    $("#ATAIDValue").val(id);
                    $("#ATAName").val(text);
                    break;
                //Model                         
                case 'Model':
                    var id = $(":selected", elem).val();
                    var text = $(":selected", elem).text();
                    //set id to hidden field
                    $("#ModelIDValue").val(id);
                    $("#ModelName").val(text);
                    break;
            }
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
