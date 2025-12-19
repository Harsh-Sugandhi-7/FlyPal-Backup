<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHobbsOffset_Ajax.aspx.vb"
    Inherits="Flypal.wfHobbsOffset_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Hobbs Offset</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder Runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Hobbs Offset</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                Width="300px"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvOffset" runat="server" CssClass="clsLabelAuto"
                                                ControlToValidate="txtOffset" Display="None" ErrorMessage="Offset Value Required"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvOffSet" runat="server" ControlToValidate="txtOffset" Display="None"
                                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Hobbs Offset"
                                                            CausesValidation="False" Text="New"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 21px" colspan="4">
                                                        <asp:Label ID="lblDetails" runat="server" CssClass="clsLabelHeader">Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 39px">
                                                        <asp:Label ID="lblCompanyNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td style="width: 83px; height: 39px">
                                                        <asp:Label ID="lblDate" runat="server" CssClass="clsLabel">Date</asp:Label>
                                                    </td>
                                                    <td style="height: 39px">
                                                        <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                
                                                                <td>
                                                                    <%--<uc1:SICalendar ID="calDate" runat="server"></uc1:SICalendar>--%>
                                                                    <asp:TextBox ID="calDate" runat="server" CssClass="clsTextBox_Ajax" Width="100px" 
                                                                        AutoPostBack="True"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calClosedDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDate">
                                                                    </cc2:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="height: 39px" align="right">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td style="width: 83px">
                                                        <asp:Label ID="lblOffset" runat="server" CssClass="clsLabel">Offset</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOffset" runat="server" CssClass="clsTextBoxRightAlignMedium_Ajax"
                                                            ToolTip="Enter Hobbs Offset" Text="<%# mHobbsOffset.Offset %>" ReadOnly="True"
                                                            MaxLength="10"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to save current Hobbs Offset"
                                                            Text="Save"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:DataGrid ID="dgHobbsOffsetList" runat="server" CssClass="clsGrid" Width="264px"
                                                            AutoGenerateColumns="False">
                                                            <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="DateFormatted" HeaderText="Date"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Offset" HeaderText="Offset">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundColumn>
                                                                <asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
                                                                <asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </td>
                                                    <td>
                                                        <table id="Table1" style="height: 100%;" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Button ID="btnCloseTop" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to close Hobbs Offset screen"
                                                                        CausesValidation="False" Text="Close" 
                                                                        Visible="<%# mHobbsOffsetList.Count>25 %>">
                                                                    </asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom">
                                                                    <asp:Button ID="btnClose" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to close Hobbs Offset screen"
                                                                        CausesValidation="False" Text="Close"></asp:Button>
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
