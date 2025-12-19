<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateMinMaxOrdLevelReOrdLevel_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateMinMaxOrdLevelReOrdLevel_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <title>Update Min. / Max. Stock Level and Re-Order Qty.</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr class="clsFormHeader1Newstyle">
                                <td colspan="5">
                                    <table class="clsFormHeader" width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle" class="clsFormHeader">Update Min. / Max. Stock Level and Re-Order Qty.</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAddClose">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnUpdate" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                                            ToolTip="Click to Update Min. / Max. Stock Level & Re-Order Qty"></asp:Button>

                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                            CausesValidation="false" ToolTip="Click to Close Screen."></asp:Button>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" runat="server"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                ValidateEmptyText="true" OnServerValidate="CustomValidate" Display="None" ControlToValidate="txtSearch"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblPartNo" class="clsLabel">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblCategory" class="clsLabel">Category</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                            DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td valign="top" align="right">
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
                                                    CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
                                                    ValidationGroup="1" CausesValidation="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span class="clsLabelHeader">Please enter Remark to be shown in event log</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlUpdateHistory" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto">Update Remark</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUpdateRemark" runat="server" CssClass="clsTextBoxSearch_Ajax"
                                                           MaxLength="500" ToolTip="Please enter Remark to be shown in event log"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkUpdationHistory" runat="server" Text="Updation History"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 100%">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Part as per criteria : Record(s) found.</asp:Label>
                                            </div>
                                            <div style="width: 100%">
                                                <asp:GridView ID="gdvItem" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    Width="100%" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" AllowSorting="True">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CategoryName" HeaderText="Category">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OldMinStockLevel" HeaderText="Old Min.Level">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="New Min. Level">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMinStockLevel" CssClass="clsTextBoxTagSearchSmall" runat="server"
                                                                    ClientIDMode="Static" OnTextChanged="txtMinStockLevel_TextChanged" AutoPostBack="true"
                                                                    MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "NewMinStockLevel") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="OldMaxStockLevel" HeaderText="Old Max.Level">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="New Max. Level">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMaxStockLevel" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                    ClientIDMode="Static" OnTextChanged="txtMaxStockLevel_TextChanged" AutoPostBack="true"
                                                                    MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "NewMaxStockLevel") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="OldMinReOrderLevel" HeaderText="Old Re-Order Level">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="New Re-Order Level">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMinReOrderLevel" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                    Enabled="false" ClientIDMode="Static" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "NewMinReOrderLevel") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <%-- Added by Shital on 08-Mar-2021--%>
                                                        <asp:BoundField DataField="ReOrderQty" HeaderText="Re-Order Qty.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%-------%>
                                                        <asp:TemplateField HeaderText="One Time Purchase">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkIsOneTimePurchase" onclick="EnableDisable(this);" runat="server"
                                                                    ClientIDMode="Static" CssClass="clsCheckBox" Checked='<%# DataBinder.Eval(Container.DataItem, "IsOneTimePurchase") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="IsConsiderForReOrder" HeaderStyle-CssClass="hideGridColumn"
                                                            ItemStyle-CssClass="hideGridColumn" HeaderText="IsConsiderForReOrder">
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="hideGridColumn" />
                                                            <ItemStyle HorizontalAlign="Left" CssClass="hideGridColumn" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <!--End-->
                            </tr>
                        </table>
                    </asp:Panel>
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
        <!--Updation History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyUpdationHistory" Text="Updation History" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlUpdationHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeUpdationHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupUpdationHistory" runat="server" TargetControlID="btnDummyUpdationHistory"
            PopupControlID="pnlUpdationHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameUpdationHistoryStateComplete() {
                $("#btnDummyUpdationHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenUpdationHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeUpdationHistory").attr("src", "wfUpdateHistoryForMinMaxOrdLevelReOrdLevel_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyUpdationHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForUpdationHistory() {
                var UpdationHistorywindow = $find("<%=mdlPopupUpdationHistory.ClientID %>");
                //close Updation History popup window
                UpdationHistorywindow.hide();
                //           release resources
                $("#IframeUpdationHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnUpdationHistory").click();
            }
        </script>
        <!-- End-->
    </form>
    <!-- Slider control events  -->
    <script type="text/javascript">

        function sliderStart() {
            $('#valuetodisplay').css('display', 'inline-block');
        }
    </script>
    <script type="text/javascript">
        function sliderEnd() {
            $('#valuetodisplay').css('display', 'none');

        }
    </script>
    <!-- End  -->
    <script type="text/javascript">
        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


        function EnableDisable(control) {
            var IsBAClientCode = '<%=System.Configuration.ConfigurationSettings.AppSettings("ClientCode").ToString()%>';

            if (IsBAClientCode == "BA") {
                var grid = $(control).closest("table");

                //Find and reference the Header CheckBox.
                //var chkHeader = $("[id*=chkHeader]", grid);

                //If the CheckBox is Checked then enable the TextBoxes in thr Row.

                if (!$(control).is(":checked")) {
                    var td = $("td", $(control).closest("tr"));
                    $("#txtMaxStockLevel,#txtMinStockLevel", td).removeAttr("disabled");
                } else {
                    var td = $("td", $(control).closest("tr"));
                    $("#txtMaxStockLevel,#txtMinStockLevel,#txtMinReOrderLevel", td).val('0');
                    $("#txtMaxStockLevel,#txtMinStockLevel", td).attr("disabled", "disabled");
                }
            }

        }

    </script>
</body>
</html>
