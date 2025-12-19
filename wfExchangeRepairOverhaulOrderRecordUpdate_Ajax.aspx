<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExchangeRepairOverhaulOrderRecordUpdate_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfExchangeRepairOverhaulOrderRecordUpdate_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GRO/Loan Receipt To Outright</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <style type="text/css">
        .activerow {
            /* yellow*/
            background-color: rgb(255, 203, 96); /* red 
           background-color: #ffd9eb  ;*/
        }

        .pagingclass {
            margin-top: 2px;
            padding: 1px;
            border: 1px solid #ddd;
        }
        
    </style>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmChangeLocation" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span class="clsFormHeader">GRO / Loan, Rental / Lease Receipt To Outright</span>
                                            </td>

                                            <td align="right">
                                                <div>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlClosebottom">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnClose" runat="server"
                                                                CssClass="clsbtnH clsinfoH" Text="Close" CausesValidation="False"
                                                                ToolTip="Click to Close"></asp:Button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Part No.</asp:ListItem>
                                                            <asp:ListItem Value="2">Receipt</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                            EnableViewState="false" MaxLength="100"></asp:TextBox>
                                                        <asp:DropDownList ID="cmbReceiptText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            AutoPostBack="True" Visible="False" DataValueField="Text" DataTextField="Text">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                            MaxLength="4"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkShowLoanTransactions" runat="server" CssClass="clsCheckBox"
                                                            Text="Show Loan,Rental/Lease Transactions" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <div>
                                        <%--<asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                        Text="Find Now" ToolTip="Click to find as per criteria"></asp:Button>--%>

                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                            ToolTip="Click to find  as  per searching criteria" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <div>
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlClose">
                                            <ContentTemplate>
                                                <asp:Button ID="btnClose1" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                    CausesValidation="false" Text="Close" Visible="False"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 100%">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
                                            </div>
                                            <div style="width: 100%">
                                                <asp:GridView ID="gdPartSearch" runat="server" PageSize="25" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="True" AllowPaging="True" AllowSorting="True"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    OnPageIndexChanging="gdPartSearch_PageIndexChanging">
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle HorizontalAlign="Right" CssClass="pagingclass" />
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ReceiptItemID" HeaderText="ReceiptItemID">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Date">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptNumber" HeaderText="Receipt No.">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptItemSerialNo" SortExpression="ReceiptItemSerialNo"
                                                            HeaderText="Serial No.">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RCIType" SortExpression="RCIType" HeaderText="From">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField Text="Update" HeaderText="Update" CommandName="UpdateRec">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="OrderItemID" HeaderText="OrderItemID" Visible="False">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left"></td>
                                <%--<td align="right">
                                <div>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlClosebottom">
                                        <ContentTemplate>
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" CausesValidation="False">
                                            </asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>--%>
                            </tr>
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnGROOutrightConversion" ClientIDMode="Static" runat="server"
                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
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
                        <div clawftaskss="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <!-- Record Update Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyGROOutrightConversion" Text="Dummy GRO Outright Conversion"
                ClientIDMode="Static" CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupGROOutrightConversion" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="iPopupGROOutrightConversion" frameborder="0" allowtransparency="true"
                height="100%" width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:modalpopupextender id="mdlPopupGROOutrightConversion" runat="server" targetcontrolid="btnDummyGROOutrightConversion"
            popupcontrolid="pnlPopupGROOutrightConversion" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
        <script type="text/javascript">
            function IFrameGROOutrightConversionStateComplete() {
                $("#btnDummyGROOutrightConversion").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenGROOutrightConversionWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupGROOutrightConversion").attr("src", "wfRecordUpdate_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyGROOutrightConversion").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForGROOutrightConversion() {
                var GROOutrightConversionwindow = $find("<%=mdlPopupGROOutrightConversion.ClientID %>");
                //close GROOutrightConversion popup window
                GROOutrightConversionwindow.hide();
                $("#iPopupGROOutrightConversion").attr("src", "JavaScript:''");
                //call GROOutrightConversion image button
                $("#hdnimgBtnGROOutrightConversion").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
