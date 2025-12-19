<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogPax_Ajax.aspx.vb" Inherits="Flypal.wfLogPax_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Log Pax</title>
    <meta http-equiv="x-ua-compatible" content="IE=9">
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
    </script>
    
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Log Pax</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <%--                                        <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtCompanyName" Display="None" ErrorMessage="Company Name Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvPassengerName" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtPassengerName" Display="None" ErrorMessage="Passenger Name Required"></asp:RequiredFieldValidator>--%>
                                            <asp:CustomValidator Style="z-index: 0" ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"
                                                OnServerValidate="CustomValidate1" Display="None" ControlToValidate="txtCompanyName"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 127px">
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabel">Aircraft</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mMachine.RegNo %>"
                                                            BackColor="#E0E0E0" ReadOnly="True" ToolTip="Aircraft"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 127px">
                                                        <asp:Label ID="lblDeparture" runat="server" CssClass="clsLabel">Departure</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDeparture" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mLog.SourceName %>"
                                                            BackColor="#E0E0E0" ReadOnly="True" ToolTip="Departure"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDepDateTime" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDepDateTime" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                            ReadOnly="True" ToolTip="Date/Time"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 127px">
                                                        <asp:Label ID="lblArrival" runat="server" CssClass="clsLabel">Arrival</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtArrival" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mLog.DestinationName %>"
                                                            BackColor="#E0E0E0" ReadOnly="True" ToolTip="Arrival"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblArrDateTime" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtArrDateTime" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                            ReadOnly="True" ToolTip="Date/Time"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 27px">
                                                    </td>
                                                    <td style="width: 127px; height: 27px">
                                                        <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Sr. No.</asp:Label>
                                                    </td>
                                                    <td style="height: 27px">
                                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxMedium_Ajax" Text="<%# mLogPax.SerialNo %>"
                                                            BackColor="#E0E0E0" ReadOnly="True" ToolTip="Serial No."></asp:TextBox>
                                                    </td>
                                                    <td style="height: 27px">
                                                    </td>
                                                    <td style="height: 27px" align="right">
                                                    </td>
                                                    <td style="height: 27px" align="right">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 38px">
                                                        <asp:Label ID="lblCompanyNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table id="Table3">
                                                            <tr>
                                                                <td valign="middle" align="left">
                                                                    <asp:Label ID="lblCompanyName" runat="server" CssClass="clsLabelAuto">Company Name</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="imgbtnCompanyName" runat="server" 
                                                                        CssClass="clsButtonImg_Ajax" ToolTip="Select Company"
                                                                        ImageUrl="ICONS/ADD.ICO" CausesValidation="False"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="height: 38px">
                                                        <table id="Table2" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mLogPax.CompanyName %>" Height="30px" TextMode="MultiLine" 
                                                                        BackColor="#E0E0E0" ReadOnly="True" ToolTip="Company Name"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="imgbtnCompany" TabIndex="1" runat="server" CssClass="clsButtonGrid_Ajax"
                                                                        Text="..." ToolTip="Click to add new company" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="height: 38px">
                                                    </td>
                                                    <td style="height: 38px" align="right">
                                                    </td>
                                                    <td style="height: 38px" align="right">
                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="New"
                                                            ToolTip="Click to add new Log Pax" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPassengerNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td style="width: 127px">
                                                        <asp:Label ID="lblPassengerName" runat="server" CssClass="clsLabelAuto">Passenger Name</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPassengerName" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mLogPax.PassengerName %>"
                                                            ToolTip="Enter Passenger's Name" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 127px">
                                                        <asp:Label ID="lblPassengerWeight" runat="server" CssClass="clsLabelAuto">Passenger Weight</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPassengerWeight" runat="server" 
                                                            CssClass="clsTextBoxMedium_Ajax" Text="<%# mLogPax.PassengerWeight %>"
                                                            ToolTip="Enter Passenger's Weight" MaxLength="5"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvpw" runat="server" ControlToValidate="txtPassengerWeight"
                                                            Display="None" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 127px">
                                                        <asp:Label ID="lblLuggageWeight" runat="server" CssClass="clsLabelAuto">Luggage Weight</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLuggageWeight" runat="server" 
                                                            CssClass="clsTextBoxMedium_Ajax" Text="<%# mLogPax.LuggageWeight %>"
                                                            ToolTip="Enter Luggage Weight" MaxLength="5"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvlw" runat="server" ControlToValidate="txtLuggageWeight"
                                                            Display="None" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 127px">
                                                        <asp:Label ID="lblPercentUsage" runat="server" CssClass="clsLabelAuto">Percent Usage</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPercentUsage" runat="server" 
                                                            CssClass="clsTextBoxMedium_Ajax" Text="<%# mLogPax.PercentUsage %>"
                                                            ToolTip="Enter Percent Usage" MaxLength="5"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvpu" runat="server" ControlToValidate="txtPercentUsage"
                                                            Display="None" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" 
                                                            ToolTip="Click to save current Log Pax">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:DataGrid ID="dgLogPaxList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                                            <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SerialNo" HeaderText="Sr. No."></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="CompanyName" HeaderText="Company Name"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="PassengerName" HeaderText="Passenger"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="PassengerWeight" HeaderText="Passenger Weight">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="LuggageWeight" HeaderText="Luggage Weight">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="PercentUsage" HeaderText="Percent Usage">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundColumn>
                                                                <asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
                                                                <asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </td>
                                                    <td valign="top">
                                                        <table id="Table1" border="0" cellspacing="0" cellpadding="0" align="right" height="100%">
                                                            <tr>
                                                                <td valign="top" align="right">
                                                                    <asp:Button ID="btnCloseClick" runat="server" CssClass="clsButton_Ajax" 
                                                                        Text="Close" ToolTip="Click to close Flight Log Classification screen"
                                                                        CausesValidation="False" Visible="<%# mLogPaxList.Count >25 %>"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                    </td>
                                                    <td align="right" colspan="5">
                                                        <table id="Table7" border="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print" ToolTip="Click to print Log Pax List"
                                                                        CausesValidation="False" Visible="<%# mLogPaxList.Count >0 %>"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to close Log Pax screen"
                                                                        CausesValidation="False"></asp:Button>
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
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
 <asp:UpdateProgress ID="AjaxLoader" DynamicLayout="false" DisplayAfter="200" runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%;width: 100%; left: 0; position: fixed; background-color: #000000;
                    top: 0;  z-index: 99999;">
               </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left:-27px;margin-top:-27px; z-index: 100000; ">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                           <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px"  /> 
                        </div>
                    </div>
                </div> 
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
</html>
