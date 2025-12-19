<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAlternatePartChild_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfAlternatePartChild_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title></title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <asp:Panel runat="server" ID="pnlAlternatePart">
                <table class="clstablelistout" id="tblmain">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                                <table id="tblLedgerList" class="clstablelistin">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Alternate Part [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="tblAlternatePart" class="clsTablelistin" border="0" cellspacing="0" cellpadding="1"
                                                width="100%" align="left">
                                                <%-- <tr>
                                                    <td align="left">
                                                        <asp:UpdatePanel runat="server" ID="upnlTabs" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table2" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnPartInformation" runat="server" CssClass="clsButtonLong_Ajax"
                                                                                EnableViewState="False" Text="Part Information" ToolTip="Click to open the Part Information"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAlternatePart" runat="server" CssClass="clsLabelButton1" ToolTip="Current page of Aircraft Status Detail">Alternate Part</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnApplicability" runat="server" CssClass="clsButtonLong_Ajax" EnableViewState="False"
                                                                                Text="Applicability" ToolTip="Click to open the Applicability List"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnOpeningStock" runat="server" CssClass="clsButtonLong_Ajax" EnableViewState="False"
                                                                                Text="Opening Stock" ToolTip="Click to open the Opening Stock List"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel runat="server" ID="upnlSearchAlt" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td width="8px">
                                                                            <span id="lblOptions1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td width="100px">
                                                                            <span id="lblOptions" class="clsLabel">Options</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbOptions" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                                <asp:ListItem Value="1">New</asp:ListItem>
                                                                                <asp:ListItem Value="2">Existing</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <asp:Label ID="lblAltInfo" runat="server" CssClass="clsLabelHeader">Select Part Type, enter Part No. to add new Part and press Add button.</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <table style="width: 100%;" id="tblSelectAlternatePart" class="clstablelistout" cellspacing="1"
                                                                                cellpadding="0">
                                                                                <tr>
                                                                                    <td width="8px"></td>
                                                                                    <td align="left" width="100px">
                                                                                        <asp:Label ID="lblAltTypeList" runat="server" CssClass="clsLabel">Part Type</asp:Label>
                                                                                    </td>
                                                                                    <td align="left" colspan="2">
                                                                                        <asp:DropDownList ID="cmbAltTypeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                            onChange="setComboBoxValue(this)" EnableViewState="true" DataTextField="Name"
                                                                                            DataValueField="ID">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width="8px"></td>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblAltFindPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="txtAlternatePart" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part No." 
                                                                                            MaxLength="50"></asp:TextBox>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <table align="right" cellspacing="0" cellpadding="1">
                                                                                            <tr>
                                                                                                <%-- <td align="right" >
                                                                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH1" Text="Find Now" 
                                                                                                        ToolTip="Click to Find Now" Visible="False"></asp:Button>
                                                                                                </td>--%>
                                                                                                <td align="right" valign="top">
                                                                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:ImageButton ID="btnSearch" runat="server" CssClass="clsSearch2btn" ImageUrl="~/images/Search2.png" Visible="False" ToolTip="Click to find the list of Part as per searching criteria" />
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>

                                                                                                </td>  
                                                                                                <td align="right">
                                                                                                <asp:Button ID="btnAlternatePart" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                                                    ToolTip="Click to save the Part" />
                                                                                            </td>
                                                                                            </tr>
                                                                                          
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" colspan="4">
                                                                            <asp:GridView ID="gdvPartList" EnableViewState="false" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                                                DataKeyNames="ID" ForeColor="Black" GridLines="Horizontal" PageSize="10" ShowHeaderWhenEmpty="true">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader"></HeaderStyle>
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                <Columns>
                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part No">
                                                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="UnitName" SortExpression="UnitName" HeaderText="Unit">
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category">
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="AlternatePartPresent" SortExpression="AlternatePartPresent"
                                                                                        HeaderText="Alternate Part Present">
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:ButtonField Text="Select" HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="black" ItemStyle-ForeColor="blue"
                                                                                        CommandName="Select"></asp:ButtonField>
                                                                                </Columns>
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                            </asp:GridView>
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
                                                <tr>
                                                    <td style="width: 517px;">
                                                        <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div style="width: 100%; margin-bottom: 3px;">
                                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Alternate Part List</asp:Label>
                                                                </div>
                                                                <%-- <div style="width: 500px;">
                                                                    <table class="clsGrid clsdgHeader" style="width: 500px; border-collapse: collapse;"
                                                                        cellspacing="0">
                                                                        <tr>
                                                                            <td width="150px" class="clsdgHeader TextBreak" style="background-color:white;">
                                                                                <a style="color: black" href="javascript:__doPostBack('gdvAlternatePartList','Sort$PartName')">Part No</a>
                                                                            </td>
                                                                            <td width="200px" class="clsdgHeader TextBreak" style="background-color:white;">
                                                                                <a style="color: black" href="javascript:__doPostBack('gdvAlternatePartList','Sort$PartDescription')">Description</a>
                                                                            </td>
                                                                            <td width="90px" class="clsdgHeader TextBreak" style="background-color:white;">
                                                                                <a style="color: black" href="javascript:__doPostBack('gdvAlternatePartList','Sort$AltTypeName')">Part Type</a>
                                                                            </td>
                                                                            <td width="60px" style="background-color:white;" class="clsdgHeader TextBreak">
                                                                                <span class="clsdgHeader">Remove</span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>--%>
                                                                <div style="width: 100%; max-height: 300px; overflow-y: auto; overflow-x: hidden;">
                                                                    <asp:GridView ID="gdvAlternatePartList" ClientIDMode="Static"
                                                                        runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                                        DataKeyNames="ID" EnableViewState="false" ForeColor="Black" GridLines="Horizontal" PageSize="25" ShowHeaderWhenEmpty="true">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem TextBreak"></AlternatingRowStyle>
                                                                        <RowStyle CssClass="clsdgAltItem TextBreak"></RowStyle>
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgAltItem TextBreak"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No">
                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                                <ItemStyle Width="150px"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="PartDescription" SortExpression="PartDescription" HeaderText="Description">
                                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                                <ItemStyle Width="200px"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AltTypeName" SortExpression="AltTypeName" HeaderText="Part Type">
                                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                                <ItemStyle Width="90px"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:ButtonField Text="Remove" HeaderText="Remove" HeaderStyle-HorizontalAlign="Left"
                                                                                CommandName="Remove">
                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                                <ItemStyle Width="60px" ForeColor="blue"></ItemStyle>
                                                                            </asp:ButtonField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <div style="width: 100%;">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back" ToolTip="Click to go back to the previous page"
                                                                            CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
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
            </asp:Panel>
            <asp:HiddenField ID="PartTypeValue" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="PartTypeName" ClientIDMode="Static" runat="server" />
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
            function setComboBoxValue(elem) {
                var id = $(":selected", elem).val();
                var Name = $(":selected", elem).text();
                $("#PartTypeValue").val(id);
                $("#PartTypeName").val(Name);
            }
        </script>
    </form>
</body>
</html>
