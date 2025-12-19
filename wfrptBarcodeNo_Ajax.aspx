<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptBarcodeNo_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptBarcodeNo_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Acceptance Tag</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <script id="clientEventHandlersJS" type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr class="clsFormHeader1Newstyle">
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lbltitle" class="clsFormHeader">Acceptance Tag</span>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                    ToolTip="Click to Close" Text="Close"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="3"></td>
                                                    <td>
                                                        <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" Visible="false" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Part No.</asp:ListItem>
                                                            <asp:ListItem Value="2">Location</asp:ListItem>
                                                            <asp:ListItem Value="3">Part Type</asp:ListItem>
                                                            <asp:ListItem Value="4">Store</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                            MaxLength="100"></asp:TextBox>
                                                        <asp:DropDownList ID="cmbPartType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False"
                                                            DataTextField="Name" DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbStoreList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False"
                                                            EnableViewState="false" DataTextField="LocationStore" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <table id="Table4" border="0">
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
                                                    CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
                                                    ValidationGroup="1" CausesValidation="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 100%; margin-bottom: 3px;">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts : </asp:Label>
                                            </div>
                                            <div style="width: 100%;">
                                                <asp:GridView ID="gdPartSearch" ShowHeaderWhenEmpty="true" ClientIDMode="Static"
                                                    runat="server" AllowSorting="True" DataKeyNames="ID" AllowPaging="True" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    AutoGenerateColumns="False" PageSize="25">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="125px"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Store" SortExpression="Store" HeaderText="Store">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part Type">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DateFormatted" HeaderText="Receipt Date">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="StockBalQty" SortExpression="StockBalQty"
                                                            HeaderText="Stock Qty.">
                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="Release Note No.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:ButtonField Visible="False" Text="Change Part Type" HeaderText="Change Part Type"
                                                            CommandName="ChangePartType">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:ButtonField>
                                                        <asp:BoundField Visible="False" DataField="ItemTypeID" HeaderText="ItemTypeID"></asp:BoundField>
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
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPrintAcceptanceTag" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            Width="150px" ToolTip="Click to Print Acceptance Tag" Text="Print Acceptance Tag"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger EventName="click" ControlID="btnSearch" />
                                            <asp:AsyncPostBackTrigger EventName="click" ControlID="btnClose" />
                                        </Triggers>
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
        <script type="text/javascript">
            $(document).ready(function () {
                $("#chkSelectAll").live("click", function () {
                    var status = $("#chkSelectAll").attr("checked");
                    $("#gdPartSearch tr:gt(0)").find(":checkbox").each(function () {
                        if (status == "checked") {
                            $(this).attr("checked", status);
                            SetRow($(this));
                        }
                        else {
                            $(this).removeAttr("checked");
                            SetRow($(this));
                        }

                    });
                });
            });

            function SetRow(elem) {
                var status = $(elem).attr("checked");
                if (status == "checked") {
                    $(elem).closest("tr").addClass('HighLightRow');
                }
                else {
                    $(elem).closest("tr").removeClass('HighLightRow');
                }
            }

            function pageLoad() {
                var status;
                $("#gdPartSearch tr:gt(0)").find(":checkbox").each(function () {
                    status = $(this).attr("checked");
                    if (status == "checked") {
                        SetRow($(this));
                    }
                    else {
                        //$(this).removeAttr("checked");
                        SetRow($(this));
                    }

                });

            }
        </script>
    </form>
</body>
</html>
