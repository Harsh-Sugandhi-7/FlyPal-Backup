<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDailyStatus_Ajax.aspx.vb"
    Inherits="Flypal.wfDailyStatus_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Daily Status</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Daily Status</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" CssClass="clsbtnH clsinfoH" runat="server" Text="Add" ToolTip="Click to Add"
                                                                        ValidationGroup="valGroup1"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to save the current record."
                                                                        ValidationGroup="valGroup1"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" TabIndex="0" CssClass="clsbtnH clsinfoH" runat="server" Text="Close"
                                                                        ToolTip="Click to Close Daily Status Screen" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvModel" runat="server" CssClass="clsLabelAuto" ErrorMessage="Model Required"
                                                Display="None" ControlToValidate="cmbModel" ClientValidationFunction="validateModel"
                                                ValidationGroup="valGroup1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvMaintenanceActivityType" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Maintenance Activity Type Required" Display="None" ControlToValidate="cmbMaintenanceActivityType"
                                                ClientValidationFunction="validateMaintActivityType" ValidationGroup="valGroup1"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateModel(source, args) {
                                                    args.IsValid = false;
                                                    var modelCombo = $get("cmbModel");
                                                    var maintActivityTypeCombo = $get("cmbMaintenanceActivityType")
                                                    if (modelCombo.selectedIndex != 0 || maintActivityTypeCombo.value == 7) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }

                                                function validateMaintActivityType(source, args) {
                                                    args.IsValid = false;
                                                    var maintActivityTypeCombo = $get("cmbMaintenanceActivityType")
                                                    if (maintActivityTypeCombo.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }


                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>

                                <td>
                                    <asp:Label ID="Label1" runat="server"  CssClass="clsLabelAuto" Text="Daily Status Details"></asp:Label>


                                  <%--  <span id="lblAttachFileDetails" class="clstitle1">Daily Status Details</span>--%>



                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table id="Table4">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDocumentStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblMaintenanceActivityType" class="clsLabelAuto">Maintenance Activity Type</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbMaintenanceActivityType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataTextField="Name" DataValueField="ID" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLabelStar" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <span id="lblModel" class="clsLabelAuto">Model</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ModelName"
                                                                        DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <table id="Table5" border="0" cellspacing="0" cellpadding="0" align="right" height="100%">
                                                            <tr>
                                                                <td valign="top" align="right"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 44px" valign="bottom">
                                                                    <table id="Table3">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <%-- <asp:Button ID="btnFindNow" CssClass="clsButton" runat="server" Text="Find Now" ToolTip="Click to Find Now"
                                                                                            ValidationGroup="valGroup1"></asp:Button>--%>
                                                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                                            CausesValidation="false" />

                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom" align="right"></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td valign="top" align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgDailyStatusList" runat="server" CssClass="clsGridNewStyle" Visible="False"
                                                            ToolTip="Daily Status List" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="true" CellPadding="5" GridLines="Horizontal"
                                                            PageSize="25" ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No."></asp:BoundField>
                                                                <asp:BoundField DataField="MaintenanceActivityTypeName" SortExpression="MaintenanceActivityTypeName"
                                                                    HeaderText="Maintenance Activity Type">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%-- <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>--%>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Delete" runat="server" CommandName="DeleteRec"
                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgDailyStatusCertificateList" runat="server" CssClass="clsGridNewStyle"
                                                            Visible="False" ToolTip="Daily Status List" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal"
                                                            AllowPaging="true" PageSize="25">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No."></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="MaintenanceActivityTypeNameForCertificate"
                                                                    SortExpression="MaintenanceActivityTypeNameForCertificate" HeaderText="Maintenance Activity Type">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CertificateName" SortExpression="CertificateName" HeaderText="Certificate Name">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CertificateNo" SortExpression="CertificateNo" HeaderText="Certificate No.">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date"></asp:BoundField>
                                                                <asp:BoundField DataField="ElapsedDays" HeaderText="Elapsed Days">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--<asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>--%>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Delete" runat="server" CommandName="DeleteRec"
                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAdd" CssClass="clsButton_Ajax" runat="server" Text="Add" ToolTip="Click to Add"
                                                            ValidationGroup="valGroup1"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" Text="Save" ToolTip="Click to save the current record."
                                                            ValidationGroup="valGroup1"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" CssClass="clsButton_Ajax" runat="server" Text="Close"
                                                            ToolTip="Click to Close Daily Status Screen" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
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
    </form>
</body>
</html>
