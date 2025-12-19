<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogFuelOil_Ajax.aspx.vb"
    Inherits="Flypal.wfLogFuelOil_Ajax" %>

<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Fuel Oil</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <meta name="vs_showGrid" content="True" />
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>

    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <script language="javascript" type="text/javascript">

            var g_CurrentTextBox;
            var g_isTabPressed;

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            function endRequestHandler() {

                try {

                    //if (g_isTabPressed == 1) {
                    $get(g_CurrentTextBox).focus();
                    $get(g_CurrentTextBox).select();

                    g_isTabPressed = 0;
                    //}


                }
                catch (Error) { }

            }


            function onTextFocus() {
                g_CurrentTextBox = event.srcElement.id;

            }

            function onkeyPressed(keycode, obj) {

                if (keycode == 9) {

                    g_isTabPressed = 1;
                }

            }

        </script>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                            <asp:UpdatePanel ID="upnlDetOil" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblinner" class="clsTablelistin" border="0" cellpadding="0">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">

                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Log Details</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="Table7" border="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlbtnSave" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save current record"
                                                                                            ValidationGroup="a"></asp:Button>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnBack" runat="server" Text="Back" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                    ToolTip="Click to go back to previous page"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="hdnBtnFuelOil" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                                    Style="display: none;" Text="----" />
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
                                            <asp:UpdatePanel ID="upnlbuttons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <td>
                                                        <table id="Table1" border="0" style="display: none;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnLogDetails" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                                        Text="Log Details"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFuelOil" runat="server" CssClass="clsLabelButton" ToolTip="Log Fuel Oil">Fuel Oil</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnDefectActionList" runat="server" CssClass="clsButtonLong_Ajax"
                                                                        CausesValidation="False" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Defect Reporting","Snag Reporting") %>'></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnParameterList" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                                        Text="Parameter List"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnLogPax" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                                        Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' Text="Passenger Log"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnHobbsOffset" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                                        Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' Text="Hobbs Offset"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnFlightCrew" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                                        Text="Flight Crew"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnMaintenanceAcitvity" runat="server" CssClass="clsButtonLong_Ajax"
                                                                        CausesValidation="False" Text="Maintenance Activity"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:ValidationSummary ID="vsValidationSummary" runat="server" CssClass="clsValidationSummary"
                                                            HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                                        <asp:CustomValidator ID="cvTankList" runat="server" Display="None" OnServerValidate="customvalidate"
                                                            ValidationGroup="a" ErrorMessage="Select tank form the list."></asp:CustomValidator><asp:CustomValidator
                                                                ID="cvFuelUpLiftList" runat="server" Display="None" OnServerValidate="customvalidate"
                                                                ValidationGroup="a" ErrorMessage="Select fuel uplift unit form the List." ControlToValidate="cmbFuelUpliftUnit"></asp:CustomValidator>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlDet" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table border="0" class="clsTable1">
                                                                        <tr>
                                                                            <td style="width: 79px">
                                                                                <asp:Label ID="lblFuelType" runat="server" CssClass="clsLabelAuto" Style="z-index: 0">Fuel Type</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:UpdatePanel ID="upnlFuelType" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table2" border="0" cellpadding="1" cellspacing="1" style="z-index: 0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbFuelType" runat="server" CssClass="clsTextBoxTagSearchCombo" Width="200px"
                                                                                                        DataTextField="Name" DataValueField="Id" SelectedValue="<%# mLog.FuelUpLifts.CurrentItem.FuelTypeID %>">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="btnFuelType" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                        Width="24px" ToolTip="Click To Add New Fuel Type" CausesValidation="False" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 79px">
                                                                                <asp:Label ID="lblTotalFuelUpliftedInTank" runat="server" CssClass="clsLabelAuto"
                                                                                    Height="11px" Style="z-index: 0" Width="160px">Total Fuel Uplifted In Tank</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:UpdatePanel ID="upnlTotalFuelUpLift" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table id="Table4" border="0" cellpadding="1" cellspacing="1" style="z-index: 0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtTotalFuelUplift" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
                                                                                                        MaxLength="10" Style="z-index: 0" Text="<%# mLog.FuelUpLifts.CurrentItem.UpLift %>"
                                                                                                        ToolTip="Fuel Uplift"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbFuelUpliftUnit" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchCombo" Width="100px"
                                                                                                        DataTextField="Name" DataValueField="Id" SelectedValue="<%# mLog.FuelUpLifts.CurrentItem.UnitID %>"
                                                                                                        Style="z-index: 0">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblEqual" runat="server" CssClass="clsLabelAuto" Style="z-index: 0"
                                                                                                        Visible="False">=</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblFuelOilUnit2" runat="server" CssClass="clsLabelAuto" Style="z-index: 0">Litre</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblTankList" runat="server" CssClass="clsLabelAuto" Style="z-index: 0"
                                                                                                        Visible="False">Tank</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbTankList" runat="server" CssClass="clsTextBoxTagSearchCombo" DataTextField="Name"
                                                                                                        DataValueField="Id" Style="z-index: 0" Visible="False">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 79px">
                                                                                <asp:Label ID="lblTOWeight" runat="server" CssClass="clsLabelAuto" Style="z-index: 0">T.O. Weight</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <table id="Table5" border="0" cellpadding="1" cellspacing="1" style="z-index: 0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtTOWeight" runat="server" CssClass="clsTextBoxTagSearch" Style="z-index: 0"
                                                                                                Text="<%# mLog.FuelUpLifts.CurrentItem.TOWeight %>" ToolTip="Enter Take Off Weight"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblAltitude" runat="server" CssClass="clsLabelAuto" Style="z-index: 0"
                                                                                                Width="90px">Altitude</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtAltitude" runat="server" CssClass="clsTextBoxTagSearch" Style="z-index: 0"
                                                                                                Text="<%# mLog.FuelUpLifts.CurrentItem.Altitude %>" ToolTip="Enter Altitude"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="width: 79px">
                                                                                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto" Style="z-index: 0">Remark</asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <table id="Table6" border="0" cellpadding="1" cellspacing="1" style="z-index: 0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                                                            MaxLength="501" Style="z-index: 0" Text="<%# mLog.FuelUpLifts.CurrentItem.Remark %>"
                                                                                                            TextMode="MultiLine" ToolTip="Enter Remark" Width="467px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                    </table>
                                                                </td>
                                                                <td align="right" valign="top">
                                                                    <table id="Table3" border="0" cellpadding="1" cellspacing="1" style="z-index: 0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                    Style="z-index: 0" TabIndex="0" Text="Add" ToolTip="Click to Add Tank" Visible="False" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <%--UPDATEPANEL 2--%>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnldgLogFuel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table class="clsTable1" border="0" width="100%" style="margin-top: -15px">
                                                            <tr>
                                                                <td>
                                                                    <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px;">
                                                                        <legend id="Legend4"><b>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblListInfo" runat="server" CssClass="clsLabelHeader">Log Fuel</asp:Label>

                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:UpdatePanel ID="upnlLabel3" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">(All The Values  Are In </asp:Label>
                                                                                                <asp:Label Style="z-index: 0" ID="lblFuelOilUnit1" runat="server" CssClass="clsLabelHeader">Litre</asp:Label>
                                                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">)</asp:Label>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </b></legend>
                                                                        <asp:GridView ID="dgLogFuel" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                            CellPadding="5" CssClass="clsGridNewStyle" RowStyle-Wrap="false" HeaderStyle-Wrap="True" GridLines="Horizontal"
                                                                            SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True">
                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                            <Columns>
                                                                                <asp:BoundField Visible="false" DataField="ID" />
                                                                                <asp:BoundField DataField="TankName" HeaderText="Tank">
                                                                                    <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="FuelOnDeparture" HeaderText="Fuel On Dept." SortExpression="FuelOnDeparture">
                                                                                    <HeaderStyle Font-Bold="true" HorizontalAlign="right" Wrap="false" Width="100px" />
                                                                                    <ItemStyle HorizontalAlign="right" Wrap="false" Width="100px" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Fuel Uplifted">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFuelUpLifted" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "FuelUpLifted") %>' MaxLength="10"
                                                                                            onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                                            Width="50%" OnTextChanged="txtFuelUpLifted_TextChanged" AutoPostBack="true" ToolTip="Fuel Uplifted">
                                                                                        </asp:TextBox>
                                                                                        <asp:CustomValidator ID="cvFuelUplifted" runat="server" Display="None" OnServerValidate="customvalidate1"
                                                                                            ErrorMessage="Fuel Uplifted value." ControlToValidate="txtFuelUpLifted"></asp:CustomValidator>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="right" Width="100px" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="right" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Burn On Ground">
                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtBurnOnGround" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BurnOnGround") %>'
                                                                                            onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                                            OnTextChanged="txtBurnOnGround_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                            Width="50%" ToolTip="Burn On Ground" MaxLength="10">
                                                                                        </asp:TextBox>
                                                                                        <asp:CustomValidator ID="cvBurnOnGround" runat="server" ErrorMessage="Fuel Uplifted value."
                                                                                            OnServerValidate="customvalidate1" Display="None" ControlToValidate="txtBurnOnGround"></asp:CustomValidator>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="right" Width="100px" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="right" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="TotalFuelOnDeparture" SortExpression="TotalFuelOnDeparture"
                                                                                    HeaderText="Total Fuel On Dept.">
                                                                                    <HeaderStyle Width="100px" HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                                                    <ItemStyle Wrap="False" Width="100px" HorizontalAlign="Right"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="WO Fuel Uplifted">
                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtWOFuelUpLifted" runat="server" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                            onfocus="onTextFocus();" OnTextChanged="txtWOFuelUpLifted_TextChanged" AutoPostBack="true"
                                                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text='<%# DataBinder.Eval(Container.DataItem, "WOFuelUplifted") %>'
                                                                                            ToolTip="WO. Fuel Uplifted" Width="50%" MaxLength="10" ClientIDMode="Static">
                                                                                        </asp:TextBox>
                                                                                        <asp:CustomValidator ID="cvWOFuelUplifted" runat="server" Display="None" OnServerValidate="customvalidate1"
                                                                                            ErrorMessage="WO. Fuel Uplifted value."></asp:CustomValidator>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="right" Width="100px" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="right" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="WO Fuel Drained Out">
                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtWOFuelDrainedOut" runat="server" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                            onfocus="onTextFocus();" OnTextChanged="txtWOFuelDrainedOut_TextChanged" AutoPostBack="true"
                                                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text='<%# DataBinder.Eval(Container.DataItem, "WOFuelDrainedOut") %>'
                                                                                            ToolTip="WO. Fuel Drained Out" Width="50%" MaxLength="10" ClientIDMode="Static">
                                                                                        </asp:TextBox>
                                                                                        <%--<asp:Button ID="btnWOFuelDrainedOut" runat="server" CssClass="clsButtonGrid" Text="..."
                                                        ToolTip="Click to Refresh the the Values in the Grid and to check the Validations."
                                                        CommandName="WOFuelDrainedOut"></asp:Button>--%>
                                                                                        <asp:CustomValidator ID="cvWOFuelDrainedOut" runat="server" Display="None" OnServerValidate="customvalidate1"
                                                                                            ErrorMessage="WO. Fuel Drained Out Value."></asp:CustomValidator>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="right" Width="100px" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="right" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Fuel At Arrival">
                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFuelAtArrival" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                            onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "FuelOnArrival") %>' ToolTip="Fuel at Arrival"
                                                                                            Width="50%" OnTextChanged="txtFuelAtArrival_TextChanged" AutoPostBack="true"
                                                                                            MaxLength="10">
                                                                                        </asp:TextBox>
                                                                                        <%--<asp:Button ID="btnFuelOnArrival" runat="server" CssClass="clsButtonGrid" Text="..."
                                                        ToolTip="Click to Refresh the the Values in the Grid and to check the Validations."
                                                        CommandName="FuelOnArrival"></asp:Button>--%>
                                                                                        <asp:CustomValidator ID="cvFuelAtArrival" runat="server" Display="None" OnServerValidate="customvalidate1"
                                                                                            ControlToValidate="txtFuelAtArrival"></asp:CustomValidator>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="right" Width="100px" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="right" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="Consumtion" SortExpression="Consumtion" HeaderText="Fuel Used">
                                                                                    <HeaderStyle Width="100px" HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                                                    <ItemStyle Wrap="False" Width="100px" HorizontalAlign="Right"></ItemStyle>
                                                                                </asp:BoundField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblTotalFuelOnDeparture" runat="server" CssClass="clsLabelAuto">Total Fuel On Dept.</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalFuelOnDeparture" runat="server" Text="<%# mLog.TotalFuelOnDeparture %>" Width="100px"
                                                                                    CssClass="clsTextBoxTagSearch" ToolTip="Total Fuel" MaxLength="10" ReadOnly="True"
                                                                                    BorderColor="White" BackColor="#E0E0E0"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblTotalFieldOnArrival" runat="server" CssClass="clsLabelAuto">Total Fuel On Arrival</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalFuelOnArrival" runat="server" Text="<%# mLog.TotalFuelOnArrival %>" Width="100px"
                                                                                    CssClass="clsTextBoxTagSearch" ToolTip="Remaining Fuel" MaxLength="10" ReadOnly="True"
                                                                                    BorderColor="White" BackColor="#E0E0E0"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblTotalFuelConsumtion" runat="server" CssClass="clsLabelAuto">Total Fuel Consumption</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalFuelConsumption" runat="server" Text="<%# mLog.TotalFuelConsumption %>" Width="100px"
                                                                                    CssClass="clsTextBoxTagSearch" ToolTip="Consumption Fuel" MaxLength="10" ReadOnly="True"
                                                                                    BorderColor="White" BackColor="#E0E0E0"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <%--UPDATEPANEL --%>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlLogOil" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <%--<asp:Label ID="lblLogOil" runat="server" CssClass="clsLabelHeader">Log Oil</asp:Label>--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <%--UPDATEPANEL 3--%>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpnldgLogOil" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>

                                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                                            <legend id="Legend3"><b>
                                                                <asp:Label ID="lblLogOil" runat="server" CssClass="clsLabelHeader">Log Oil</asp:Label></b></legend>
                                                            <asp:GridView ID="dgLogOil" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                CellPadding="5" CssClass="clsGridNewStyle" RowStyle-Wrap="false" HeaderStyle-Wrap="True" GridLines="Horizontal"
                                                                SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True">
                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <Columns>
                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyTypeName" SortExpression="AssemblyTypeName" HeaderText="Assembly Type">
                                                                        <HeaderStyle HorizontalAlign="left" Width="150px" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyName" SortExpression="AssemblyName" HeaderText="Assembly">
                                                                        <HeaderStyle HorizontalAlign="left" Width="150px" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Value">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Value") %>' ToolTip="Enter Value"
                                                                                MaxLength="10" OnTextChanged="txtValue_TextChanged" AutoPostBack="true" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                                onfocus="onTextFocus();" Width="50%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            </asp:TextBox>
                                                                            <%--<asp:Button ID="btnValue" runat="server" CssClass="clsButtonGrid" Text="..." ToolTip="Click to Refresh the the Values in the Grid and to check the Validations."
                                                CommandName="Value"></asp:Button>--%>
                                                                            <asp:CustomValidator ID="cvValue" runat="server" Display="None" OnServerValidate="customvalidate1"
                                                                                ErrorMessage="Value."></asp:CustomValidator>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="right" Width="100px" />
                                                                        <ItemStyle HorizontalAlign="right" Width="100px" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="UnitName" SortExpression="UnitName" HeaderText="Unit">
                                                                        <HeaderStyle HorizontalAlign="left" Width="150px" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Updated Date/Time" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtUpdatedDate" CssClass="clsTextBoxTagSearchDate" Width="90px" onchange="ValidateDateText(this,'txtUpdatedDate_CalendarExtender')"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "OilUpdatedDateFormatted") %>' ReadOnly="True"
                                                                                BackColor="#E0E0E0" runat="server"></asp:TextBox>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtUpdatedDate" ID="txtApprove_watermarkextender"
                                                                                runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                            <asp:TextBox ID="txtTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "OilUpdatedTimeFormatted") %>' OnTextChanged="txtTime_TextChanged"
                                                                                MaxLength="10" ToolTip="Enter Time" Width="60px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlMaxAvgFuel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblAvgFuelConsumption1" runat="server" CssClass="clsLabelHeader"> </asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlMaxAvgFuel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblAvgFuelConsumption2" runat="server" CssClass="clsLabelHeader"> </asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlMaxAvgOil1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblAvgOilConsumption1" runat="server" CssClass="clsLabelHeader"> </asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlMaxAvgOil2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblAvgOilConsumption2" runat="server" CssClass="clsLabelHeader"> </asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>

                                        <%--UPDATEPANEL--%>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="AjaxLoader" DynamicLayout="false" DisplayAfter="200" runat="server">
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
        </div>
        <%--<asp:DataGrid ID="dgLogOil" runat="server" CssClass="clsGrid" PageSize="3" AutoGenerateColumns="False">
                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="AssemblyTypeName" SortExpression="AssemblyTypeName" HeaderText="Assembly Type">
                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AssemblyName" SortExpression="AssemblyName" HeaderText="Assembly">
                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"Value") %>'
                                                ToolTip="Enter Value" MaxLength="10">
                                            </asp:TextBox>
                                            <asp:Button ID="btnValue" runat="server" CssClass="clsButtonGrid" Text="..." ToolTip="Click to Refresh the the Values in the Grid and to check the Validations."
                                                CommandName="Value"></asp:Button>
                                            <asp:CustomValidator ID="cvValue" runat="server" Display="None" OnServerValidate="customvalidate1"
                                                ErrorMessage="Value."></asp:CustomValidator>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="UnitName" SortExpression="UnitName" HeaderText="Unit">
                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                            </asp:DataGrid>--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForLogFuelOil();
                return false;
            }
        </script>
        <%--<asp:Button ID="btnValue" runat="server" CssClass="clsButtonGrid" Text="..." ToolTip="Click to Refresh the the Values in the Grid and to check the Validations."
                                                CommandName="Value"></asp:Button>--%>
        <div>
            <%--UPDATEPANEL --%>
            <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameLogFuelOilStateComplete();
                    }


                });
        <% End if %>
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
                function endRequestHandler() {
                    SetPageLayout();

                }

                function SetPageLayout() {
            <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                    ReSetPageLayout();

                    onResize();//for Top bottom link
                <% End if %>
                }
                function ReSetPageLayout() {
                    $("body,html").css({ 'background-color': 'transparent' });
                    var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                    var windowheight = $(window).height();
                    if (tempMargtop >= windowheight) {
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                    }
                    else {
                        var margintop = (windowheight / 2) - (tempMargtop / 2);
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                    }

                }
            </script>
            <!-- FuelOil popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyFuelOil" Text="Maintenance Activity" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlFuelOil" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeFuelOil" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                    allowtransparency="true" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupFuelOil" runat="server" TargetControlID="btnDummyFuelOil"
                PopupControlID="pnlFuelOil" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameFuelOilStateComplete() {
                    $("#btnDummyFuelOil").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenFuelOilWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeFuelOil").attr("src", "wfFuelType_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyFuelOil").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForFuelOil() {
                    var FuelOilwindow = $find("<%=mdlPopupFuelOil.ClientID %>");
                    //close Task Card Tool popup window
                    FuelOilwindow.hide();
                    //           release resources
                    $("#IframeFuelOil").attr("src", "JavaScript:''");
                    //call image button
                    $("#hdnBtnFuelOil").click();
                }
            </script>
            <!-- End-->

        </div>
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
            function CallParentFunction() {

                window.parent.autoResizeFuelOil();
            }
        </script>
    </form>
</body>
</html>
