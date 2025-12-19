<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineTankList_Ajax.aspx.vb"
    Inherits="Flypal.wfMachineTankList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aircraft Tank List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body class="formBGColor">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvTankList" runat="server" ErrorMessage="Select Tanks form List."
                                            ControlToValidate="cmbTankList" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="fdsAircraftRegInfo" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblTankListInfo" style="font-weight: bold"><b>Aircraft Tank Details</b></legend>
                                    <asp:UpdatePanel ID="upnlTank" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Tank</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbTankList" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                            DataValueField="Id">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="imgbtnTank" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                            Width="24px" ToolTip="Click to Add New Tank" CausesValidation="False"></asp:ImageButton>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add Tank in the List"
                                                                    Text="Add"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGridTankList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Tank Details</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:GridView ID="dgTankList" runat="server" AutoGenerateColumns="False" Visible="true"
                                                    CssClass="clsGrid" PageSize="3" ShowHeaderWhenEmpty="true">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="TankName" HeaderText="Tank Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>
                                                    </Columns>
                                                </asp:GridView>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnTankMaster" runat="server" CausesValidation="False" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the list of Tanks"
                                                        CausesValidation="False" Text="Print" Visible="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go Previous page"
                                                        CausesValidation="False" Text="Back"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" ClientIDMode="Static" DynamicLayout="false"
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
    </form>
    <%--  Call parent AutoResize function to resize the form--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeTankList();
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
    </script>
    <%--Called parent function to open Tank master page--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentTankMasterFunction() {
            window.parent.OpenTankMasterWindow();
        }
    </script>
</body>
</html>
