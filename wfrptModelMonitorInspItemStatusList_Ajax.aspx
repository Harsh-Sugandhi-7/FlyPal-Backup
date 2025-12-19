<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptModelMonitorInspItemStatusList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptModelMonitorInspItemStatusList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" rel="stylesheet" type="text/css"    />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
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
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        
    </script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout"
    style="font-size: small">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">

        var g_CurrentTextBox;
        var g_isTabPressed;

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        $(document).ready(function () {
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

        }); 
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            function onTextFocus() {
                g_CurrentTextBox = event.srcElement.id;

            }

            function onkeyPressed(keycode, obj) {

                if (keycode == 9) {

                    g_isTabPressed = 1;
                }

            }
        }); 
    </script>
    <%--AJAX- ScriptManager Added--%>
    <div>
        <table id="tblModelMonitorStatus" class="clstablelistout">
            <tr>
                <td colspan="4" class="clsFormHeader1Newstyle">
                    <asp:Label ID="Label4" runat="server" CssClass="clstitle1">Inspection Item wise Aircraft Inspection Status</asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                            </asp:ValidationSummary>
                            <asp:CustomValidator ID="cvModelList" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbModelList"
                                ErrorMessage="CustomValidator" Display="None" OnServerValidate="CustomValidate"
                                Class="clsValidationSummary"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvInspectionType" runat="server" ControlToValidate="cmbInspType"
                                ErrorMessage="CustomValidator" Display="None" OnServerValidate="CustomValidate"
                                Class="clsValidationSummary" CssClass="clsLabelAuto"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvAssemblyTypeList" runat="server" 
                                Class="clsValidationSummary" ControlToValidate="cmbAssemblyTypeList" 
                                CssClass="clsLabelAuto" Display="None" ErrorMessage="CustomValidator" 
                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvCustomValidate" runat="server" 
                                ControlToValidate="cmbInspType" CssClass="clsValidationSummary" Display="None" 
                                ErrorMessage="Select Work Shop from the list." 
                                OnServerValidate="customvalidate"></asp:CustomValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upnlModel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAssemblyType" runat="server" CssClass="clsLabelAuto">Assembly Type</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle"  ID="cmbAssemblyTypeList" runat="server" AutoPostBack="True" 
                                             DataTextField="Name" DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbModelList" runat="server" AutoPostBack="True" 
                                             DataTextField="ModelName" DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblModelInspType" runat="server" CssClass="clsLabelAuto">Inspection Type</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbInspType" runat="server"  DataTextField="CodeType"
                                            DataValueField="ID" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblSearchHeader" runat="server" CssClass="clsLabelHeader">Search in following List</asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table width="99%">
                        <tr>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel">Search</asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlSearchFor" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSearchFor" runat="server" 
                                                        AutoPostBack="True">
                                                        <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                        <asp:ListItem Value="1">Reference</asp:ListItem>
                                                        <asp:ListItem Value="2">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTextSearchFor" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearchFor" runat="server" MaxLength="100"
                                                        ToolTip="Enter Reference/Desc to search" Visible="False"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="1" align="right">
                    <table>
                        <tr>
                            <td align="right">
                                <%--<asp:Button CssClass="clsbtnH clsinfoH1" ID="btnSearch" TabIndex="0" runat="server" 
                                    ToolTip="Click to Search the criteria" Text="Search"></asp:Button>--%>

                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to search criteria" />

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Panel ID="pnlInner" runat="server">
                        <table width="99%">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgModelMonitorInsp" runat="server" AllowPaging="True" AllowSorting="True" PageSize="20"
                                                AutoGenerateColumns="False"
                                                CellPadding="5" CssClass="clsGridNewStyle"  ForeColor="Black" GridLines="Horizontal"
                                                ShowHeaderWhenEmpty="True" ToolTip="List of Model Inspections as per criteria">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings  Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last"/>
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="white" Font-Bold="True" ForeColor="black" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="ATAChapter" HeaderText="ATA Chapter" SortExpression="ATAChapter"
                                                        Visible="False">
                                                        <HeaderStyle  />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                        <HeaderStyle  />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                        <HeaderStyle  />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ModelMonitorInspTypeCodeName" HeaderText="Inspection Type"
                                                        SortExpression="ModelMonitorInspTypeCodeName">
                                                        <HeaderStyle  />
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" />--%>

                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Select" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" ToolTip="Click to Close"
                                    Text="Close" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <%-- <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                                <PagerStyle HorizontalAlign="Right" />--%>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                 background-color: #000000;   top: 0; z-index: 99999;">
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
</body>
</html>
