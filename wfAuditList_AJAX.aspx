<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditList_AJAX.aspx.vb"
    Inherits="Flypal.wfAuditList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Conduction List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
             
    </script>
    <script language="javascript" id="clientEventHandlersJS" type="text/javascript">
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
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
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
                        <table class="clstablelistin" id="tblLedgerList">
                            <tr>
                                <td colspan="2" class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblAuditExecutionList" runat="server" CssClass="clsFormHeader">Audit List</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtonsTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH"  
                                                                        ToolTip="Click to Add New Audit" Text="Add New"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH"  
                                                                        ToolTip="Click to close Audit Conduction List screen" Text="Close"></asp:Button>
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
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <%--<fieldset id="fdswodetail" class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend id="ldwodetail" runat="server"><b>Search Information</b></legend>--%>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel">Search</asp:Label>
                                                        </td>
                                                        <td>
                                                            <table id="Table1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="170px"
                                                                            AutoPostBack="True">
                                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                                            <asp:ListItem Value="2">Text</asp:ListItem>
                                                                            <asp:ListItem Value="3">Audit Type</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAuditType" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" DataValueField="ID"
                                                                            DataTextField="Name" Visible="False" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtSearchText" runat="server" CssClass="clsTextBox2_Ajax" ToolTip="Enter Search Text"
                                                                            BackColor="White" AutoPostBack="true"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Audit List as per searching criteria"
                                                                        Text="Find Now" Visible="False"></asp:Button>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                          <%--  </fieldset>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlAuditText" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto">Select Audit from the list. Click On Edit/View link To Modify The Selected Audit. Click On Delete link To Delete The Selected Audit. Click On Add button To Add A New Audit.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Audit as per criteria :   Record(s) found.</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <%--<asp:UpdatePanel ID="upnlButtonsTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddTop" runat="server" CssClass="clsButton_Ajax" Visible="<%# mAuditList.Count > 25 %>"
                                                            ToolTip="Click to Add New Audit" Text="Add New"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Visible="<%# mAuditList.Count > 25 %>"
                                                            ToolTip="Click to close Audit Conduction List screen" Text="Close"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnldgAuditList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgAuditList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                ShowHeaderWhenEmpty="true" AllowSorting="True" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                               <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="AuditNo" SortExpression="AuditNo" HeaderText="Audit No.">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="AuditStandardName" SortExpression="AuditStandardName"
                                                        HeaderText="Audit Standard">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AuditTypeName" SortExpression="AuditTypeName" HeaderText="Audit Type">
                                                        <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>

                                                     

                                                    <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference No.">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="Description" SortExpression="Description"
                                                        HeaderText="Description">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Is Scheduled Next" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsNextSchedule") %>'
                                                                Enabled="False"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Frequency" SortExpression="Frequency" HeaderText="Freq. (In Months)" HeaderStyle-Width="40px">
                                                        <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ExePeriod" SortExpression="ExePeriod" HeaderText="Duration">
                                                        <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="OtherInformation" SortExpression="OtherInformation"
                                                        HeaderText="Other Info.">
                                                        <HeaderStyle  ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                    <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                    </asp:ButtonField>--%>
                                                      <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>--%>
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                </td> 
                                                                                
                                                                                <td>
                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                        CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                                        Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                        Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                        HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" />
                                                        <ItemStyle CssClass="hideGridColumn" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="clstableButton" align="right">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Audit"
                                                            Text="Add New" Visible ="false"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Audit List screen"
                                                            Text="Close" Visible ="false"></asp:Button>
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
    </form>
</body>
</html>
