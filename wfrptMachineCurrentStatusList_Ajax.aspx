<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptMachineCurrentStatusList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptMachineCurrentStatusList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Aircraft List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td class="clsFormHeader1">
                                            <asp:Label ID="lblList" runat="server" CssClass="clsFormHeader">Aircraft List</asp:Label>
                                        </td>
                                        <td style="width: 1%" align="center">
                                            <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 17px;
                                                color: black; border: black; cursor: pointer" class="fa fa-star fa-spin fa-5x circle-icon"
                                                title="Mark As Favourites"></i></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtDate"
                                    ErrorMessage="Date Required."></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtDate" ErrorMessage="Date Required."></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="77px">
                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table2" class="clsTable1" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbLookIn" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Reg. No.</asp:ListItem>
                                                                    <asp:ListItem Value="2">Model Name</asp:ListItem>
                                                                    <asp:ListItem Value="3">Manufacturer</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False"
                                                                    BackColor="White" DataValueField="ID" DataTextField="ModelName">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFor" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                    ToolTip="Enter value."></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top" align="right">
                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Find as per search criteria"
                                                        Text="Find Now"></asp:Button>--%>
                                                      <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"   ToolTip="Click to Find as per search criteria"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td width="75px">
                                            <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date  </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static" runat="server"
                                                CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="FromDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label3" class="clsLabelAuto">Enter line to be print at the bottom of the report.</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtBottomLine" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax"
                                    Width="900px" MaxLength="500" TextMode="MultiLine" ToolTip="Enter Note">I hereby certify that the data specified above has been verified throughout. Planning Manager: __________________ License No.: __________ Date: _____________</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div style="width: 100%; margin-bottom: 3px;">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Aircraft as per criteria:  Record(s) found.</asp:Label>
                                        </div>
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gdvMachineList" runat="server" CssClass="clsGridNewStyle" ToolTip="Aircraft List" CellPadding="5" GridLines="Horizontal"
                                                AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                                PageSize="25" OnDataBound="OnDataBound">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                              <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                <Columns>
                                                    <%--0--%>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <%--1--%>
                                                    <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
                                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <%--2--%>
                                                    <asp:BoundField DataField="Type" SortExpression="Type" HeaderText="Type">
                                                        <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <%--3--%>
                                                    <asp:BoundField DataField="ManufacturerName" SortExpression="ManufacturerName" HeaderText="Manufacturer">
                                                        <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <%--4--%>
                                                    <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                                        <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--5--%>
                                                    <asp:BoundField DataField="SerialNoPosition" SortExpression="SerialNoPosition" HeaderText="Serial No.">
                                                        <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--6--%>
                                                    <asp:BoundField DataField="ManufacturingDateFormatted" HeaderText="Mfg. Date / Inst. Date">
                                                        <HeaderStyle HorizontalAlign="right" Wrap="True" ></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <%--7--%>
                                                    <asp:BoundField DataField="Hours" SortExpression="Hours" HeaderText="Hours">
                                                        <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--8--%>
                                                    <asp:BoundField DataField="Landings" SortExpression="Landings" HeaderText="Landings">
                                                        <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--9--%>
                                                    <asp:BoundField DataField="Cycles" SortExpression="Cycles" HeaderText="Cycles">
                                                        <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--   <asp:BoundField DataField="RINS" SortExpression="RINS" HeaderText="RINS">
                                                        <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>--%>
                                                    <%--10--%>
                                                    <asp:BoundField DataField="AllPeriodsForUI" SortExpression="AllPeriodsForUI" HeaderText="All Periods"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Right"  Wrap="False"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--11--%>
                                                    <asp:BoundField DataField="SinceOH" SortExpression="SinceOH" HeaderText="Since OH"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Right"  Wrap="False"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--12--%>
                                                    <asp:BoundField DataField="LastFlownDateFormatted" HeaderText="Flight Log(s) Updated Till">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
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
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH" ToolTip="Click to Print the list of Aircrafts"
                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH" ToolTip="Click to Close Aircraft List screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                    <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                        Style="display: none;"></asp:Button>
                                                    <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
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
    <%--Date Validations--%>
    <script type="text/javascript">

        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
       
    </script>
     <!-- Prashant 12-Dec-2022 -->
    <script type="text/javascript">
        function FunctionFav(x) {
            if (x.classList.contains("fa-star")) {
                x.classList.remove("fa-star");
                x.classList.add("fa-star-o");
                x.style.color = 'black';
                x.style.border = 'black';
                $("#hdnBtnRemoveFav").click();
            }
            else {
                x.classList.remove("fa-star-o");
                x.classList.add("fa-star");
                x.style.color = '#fff';
                x.style.border = 'black';
                $("#hdnBtnMarkFav").click();
            }
        }
        function MarkFav() {
            var redstar = document.getElementById("<%=FavIClk.ClientID%>");
            redstar.classList.add("fa-star");
            redstar.classList.remove("fa-star-o");
            redstar.style.color = '#fff';
            redstar.style.border = 'black';

        }
        function RemoveFav() {
            var redstar = document.getElementById("<%=FavIClk.ClientID%>");
            redstar.classList.add("fa-star-o");
            redstar.classList.remove("fa-star");
            redstar.style.border = 'black';
        }
    </script>
    <!-- Prashant 12-Dec-2022 End -->
    </form>
</body>
</html>
