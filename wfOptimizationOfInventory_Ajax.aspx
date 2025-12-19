<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOptimizationOfInventory_Ajax.aspx.vb"
    Inherits="Flypal.wfOptimizationOfInventory_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Update Min./Max. Stock Level and Re-Order Qty. Screen</title>
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
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle" class="clsFormHeader">Update Min./Max. Stock Level and Re-Order Qty.
                                        Screen</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAddClose">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="tabUpdateTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                                                        ToolTip="Click to Update"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                        CausesValidation="false" ToolTip="Click to Close"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="tabUpdate" EventName="click" />
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
                                                CssClass="clsValidationSummary" runat="server" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:ValidationSummary ID="Validationsummary1" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" runat="server"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvSarch" runat="server" CssClass="clsLabelAuto" ValidateEmptyText="true"
                                                OnServerValidate="CustomValidate" Display="None" ControlToValidate="txtSearch"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvMin" runat="server" CssClass="clsLabelAuto" ValidateEmptyText="true"
                                                OnServerValidate="CustomValidator1" Display="None" ControlToValidate="txtMaxMonth"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvAvgM" runat="server" CssClass="clsLabelAuto" ValidateEmptyText="true"
                                                OnServerValidate="CustomValidator1" Display="None" ControlToValidate="txtAvgMonth"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cv4year" runat="server" CssClass="clsLabelAuto" ValidateEmptyText="true"
                                                OnServerValidate="CustomValidator1" Display="None" ControlToValidate="txt4Year"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cv6Month" runat="server" CssClass="clsLabelAuto" ValidateEmptyText="true"
                                                OnServerValidate="CustomValidator1" Display="None" ControlToValidate="txt6Month"
                                                ValidationGroup="a"></asp:CustomValidator>
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
                                                                <td>
                                                                    <span id="spanModel" class="clsLabel">Model</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ModelName"
                                                                        DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkManuallyUpdated" ClientIDMode="Static" runat="server" CssClass="clsLabelAuto"
                                                                        TextAlign="right" Width="168px" Text="Manually Updated Record"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span1" class="clsLabel">Calculate Avg. Monthly Consumption of Last</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAvgMonth" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Width="38px" Text="12"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" class="clsLabel">Month</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="clsLabel">Update Max Level as</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMaxMonth" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Width="38px" Text="4"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span4" class="clsLabel">Month of Avg. monthly consumption</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span5" class="clsLabel">Update Min Level as</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMinMonth" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Width="38px" Text="1.5"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span6" class="clsLabel">Month of Avg. monthly consumption</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span7" class="clsLabel">E.g. For Part xxx-xx-xx Last <b>12</b> month consumption
                                                                        =<b>260</b> Qty. So average consumption per month =260/12=<b>22</b> Qty. Max Level=22*<b>4</b>=<b>88</b>
                                                                        And Min Level =22*<b>1.5</b>=<b>33</b></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblToMarkOneTimePurchase" runat="server" CssClass="clsLabelAuto" Font-Bold="True">To Mark One Time Purchase </asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span8" class="clsLabel">Calculate Consumption of Last</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt4Year" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Width="38px" Text="4"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span9" class="clsLabel">Year</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span10" class="clsLabel">Update One Time Purchase if</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt6Month" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Width="38px" Text="6"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span11" class="clsLabel">Month consumption < 1</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span12" class="clsLabel">E.g. For Part xxx-xx-xx Last <b>4</b> year consumption
                                                                        =<b>9</b> Qty. So average consumption =9/((4*12)/6)=9/8=<b>1.125</b>. Marked One Time
                                                                        Purchase, if average consumption < 1 </span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCalculate" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td valign="top" align="right">
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                    ToolTip="Click to find the list of Part as per searching criteria" ValidationGroup="a">
                                                </asp:Button>--%>

                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                    ToolTip="Click to find the part as per searching criteria" ValidationGroup="a" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCalculate" runat="server" CssClass="clsbtnH clsinfoH1" Text="Calculate"
                                                    ToolTip="Click to Calculate" ValidationGroup="a"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <%--<td align="right">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAddClose">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="tabUpdateTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Update"
                                                            ToolTip="Click to Add New Part"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                            CausesValidation="false" ToolTip="Click to close Part List screen"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="tabUpdate" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 100%">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Part as per criteria : Record(s) found.</asp:Label>
                                            </div>
                                            <div style="width: 100%">
                                                <asp:GridView ID="gdvItem" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="True" AllowPaging="False" AllowSorting="True"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" 
                                                        HorizontalAlign="Left"></HeaderStyle>
                                                    <Columns>
                                                        <%--0--%>
                                                        <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ID"></asp:BoundField>
                                                        <%--1--%>
                                                        <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--2--%>
                                                        <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--3--%>
                                                        <asp:BoundField DataField="CategoryName" HeaderText="Category">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--4--%>
                                                        <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--5--%>
                                                        <asp:BoundField DataField="SumOfQty" HeaderText="Consumed Qty.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--6--%>
                                                        <asp:BoundField DataField="OldMinStockLevel" HeaderText="Old Min.Level">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--7--%>
                                                        <asp:TemplateField HeaderText="New Min. Level">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMinStockLevel" CssClass="clsTextBoxTagSearchSmall" runat="server"
                                                                    ClientIDMode="Static" OnTextChanged="txtMinStockLevel_TextChanged" AutoPostBack="true"
                                                                    MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"NewMinStockLevel") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <%--8--%>
                                                        <asp:BoundField DataField="OldMaxStockLevel" HeaderText="Old Max.Level">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--9--%>
                                                        <asp:TemplateField HeaderText="New Max. Level">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMaxStockLevel" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                    ClientIDMode="Static" OnTextChanged="txtMaxStockLevel_TextChanged" AutoPostBack="true"
                                                                    MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"NewMaxStockLevel") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <%--10--%>
                                                        <asp:BoundField DataField="OldMinReOrderLevel" HeaderText="Old Re-Order Qty.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--11--%>
                                                        <asp:TemplateField HeaderText="New Re-Order Qty.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMinReOrderLevel" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                    Enabled="false" ClientIDMode="Static" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"NewMinReOrderLevel") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                         <%--12--%>
                                                        <asp:BoundField DataField="SumOfConsumedQtyInEnteredYear"   HeaderText="Consumed Qty.In Year">
                                                            <HeaderStyle HorizontalAlign="Right"   />
                                                            <ItemStyle HorizontalAlign="Right"  />
                                                        </asp:BoundField>
                                                         <%--13--%>
                                                        <asp:BoundField DataField="AverageOfSumOfConsumedQtyInEnteredYear"   HeaderText="Average">
                                                            <HeaderStyle HorizontalAlign="Right"   />
                                                            <ItemStyle HorizontalAlign="Right"  />
                                                        </asp:BoundField>
                                                        <%--14--%>
                                                        <asp:TemplateField HeaderText="One Time Purchase">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkIsOneTimePurchase" onclick="EnableDisable(this);" runat="server"
                                                                    ClientIDMode="Static" CssClass="clsCheckBox" Checked='<%# DataBinder.Eval(Container.DataItem,"IsOneTimePurchase") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <%--15--%>
                                                        <asp:BoundField DataField="IsConsiderForReOrder" HeaderStyle-CssClass="hideGridColumn"
                                                            ItemStyle-CssClass="hideGridColumn" HeaderText="IsConsiderForReOrder">
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="hideGridColumn" />
                                                            <ItemStyle HorizontalAlign="Left" CssClass="hideGridColumn" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <%--<asp:Panel ID="PnlPaging" runat="server">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <table border="0" cellpadding="2" cellspacing="1" align="right">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label Text="" EnableViewState="false" runat="server" ClientIDMode="Static" ID="valuetodisplay"
                                                                                class="letterbox" />
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnfirstpage" class="first" onclick="setValue(0);" title="Move First">
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnprevpage" onclick="setValue(1);" class="prev" title="Move Previous">
                                                                            </span>
                                                                        </td>
                                                                        <td align="center">
                                                                            <div align="center">
                                                                                <asp:TextBox runat="server" Text="" ID="Slidercontrol">
                                                                                </asp:TextBox>
                                                                                <cc2:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="Slidercontrol"
                                                                                    Minimum="-100" Maximum="100" BoundControlID="txtPageDisplay" EnableHandleAnimation="true"
                                                                                    Length="300" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnnextvpage" onclick="setValue(2);" class="next" title="Move Next"></span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnlastpage" onclick="setValue(3);" class="last" title="Move Last"></span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtPageDisplay" ToolTip="Enter page no." CssClass="clsTextBoxMegaSmall_Ajax" />
                                                                        </td>
                                                                        <td>
                                                                            <span>of </span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label Text="" ID="lblpagecount" CssClass="clsLabelHeader" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <div>
                                                                                <asp:Button ID="btnGridPaging" CssClass="clsButtonPlus_Ajax" runat="server" Text="Go" />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <!--End-->
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <table class="clstableButton" align="right">
                                        <tr>
                                            <td>
                                                <asp:Button ID="tabUpdate" runat="server" CssClass="clsbtnH clsinfoH" Text="Update" Visible="false"
                                                    ToolTip="Click to Add New Part"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnClose" runat="server" CausesValidation="false" CssClass="clsbtnH clsinfoH" Visible="false"
                                                    Text="Close" ToolTip="Click to close Part List screen"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
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
    </div>
    </form>
    <!-- Slider control events  -->
    <%--<script type="text/javascript">
        //initialize slider control and attach events
        function pageLoad(sender, e) {
            var slider = $find('<%=SliderExtender1.ClientID %>');
            if (slider) {
                slider.add_slideStart(sliderStart);
                slider.add_slideEnd(sliderEnd);
                slider.add_valueChanged(valChanged);
            }
        }

            
    </script>
    <script type="text/javascript">
        function valChanged() {
            var showval = $('#valuetodisplay');
            var curval = $('#<%=Slidercontrol.ClientID %>');
            showval.html(curval.val());
        }
       
        
    </script>
    <script type="text/javascript">

        function sliderStart() {
            $('#valuetodisplay').css('display', 'inline-block');
        }
    </script>
    <script type="text/javascript">
        function sliderEnd() {
            $('#valuetodisplay').css('display', 'none');

        }
    </script>--%>
    <%-- <script type="text/javascript">
        function setValue(val) {
            if (val === 0) {//first
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var minval = slider.get_Minimum();
                $('#<%=txtPageDisplay.ClientID %>').val(minval);
                $('#<%=Slidercontrol.ClientID %>').val(minval);
                slider.set_Value(minval);


            }
            else if (val === 1) {//prev
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval - 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);


            }
            else if (val === 2) {//next
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval + 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);
                //                            sliderStart();
                //                            valChanged();
                //                            sliderEnd();

            }
            else if (val === 3) {//last
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var maxval = slider.get_Maximum();
                $('#<%=txtPageDisplay.ClientID %>').val(maxval);
                $('#<%=Slidercontrol.ClientID %>').val(maxval);
                slider.set_Value(maxval);
            }
        }
    </script>--%>
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
