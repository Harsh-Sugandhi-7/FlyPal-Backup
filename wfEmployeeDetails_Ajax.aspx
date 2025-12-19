<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeDetails_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeDetails_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Detail</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style>
        .clsCollapsePnl
        {
            background: url("css/img/BGLink.png") repeat-x #ccc;
            font-family: Verdana; /*font-size: 14pt; */
            font-size: 12pt;
            color: White;
            font-weight: 500;
            width: 100%;
            display: inline-block;
            border: 1px solid gray;
        }
        .clsExpandiblePnl
        {
            overflow: hidden;
            height: 0px;
            border: 1px solid #ccc;
        }
        .hideGridColumn
        {
            display: none;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" AsyncPostBackTimeout="6000">
    </asp:ScriptManager>
    <%--AJAX- Add MSGBox Control--%>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">Employee Detail</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="Table8">
                                    <tr>
                                        <td>
                                            <span id="lblCODE" class="clsLabelAuto">Emp No</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmpNo" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                ReadOnly="True" ToolTip="Enter Code" Text="<%# mEmployee.EmpNo %>">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <span id="lblName" class="clsLabelAuto">Name</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                ReadOnly="True" ToolTip="Enter Name" Text="<%# mEmployee.Name %>">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                             <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    &nbsp;<cc2:TabContainer ID="tabWoJobDetailsContainer" runat="server" class="clstablelistin"
                                        AutoPostBack="true">
                                        <cc2:TabPanel ID="tabWoJobDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                                 <asp:Label runat="server" Text="" ID="lblDepartmentRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>

                                <asp:UpdatePanel ID="upnlDepartment" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlEmployeeDepartmentInfoList" runat="server">
                                            <table id="Table13" border="0" cellspacing="0" cellpadding="0" width="100%">
                                               
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlExpandDepartment" runat="server" >
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table id="Table11" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <%--<asp:label id="lblEmployeeDepartmentInfoList" runat="server" CssClass="clsLabelHeaderWidth"></asp:label>--%>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:UpdatePanel ID="upnlAddDeptChild" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:Button ID="btnEmployeeDepartmentInfoList" runat="server" CssClass="clsButton_Ajax"
                                                                                                ToolTip="Click to Add New Department Info" Text="Add" CausesValidation="False">
                                                                                            </asp:Button>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <!--CHK-->
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 100%;">
                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                cellspacing="0">
                                                                                <tr>
                                                                                    <td width="90px" class="clsdgHeader">
                                                                                        <span>Date</span>
                                                                                    </td>
                                                                                    <td width="60px" class="clsdgHeader">
                                                                                        <span>Name</span>
                                                                                    </td>
                                                                                    <td width="120px" class="clsdgHeader">
                                                                                        <span>Remark</span>
                                                                                    </td>
                                                                                    <td width="70px" class="clsdgHeader">
                                                                                        <span>Edit/View</span>
                                                                                    </td>
                                                                                    <td width="50px" class="clsdgHeader">
                                                                                        <span>Delete</span>
                                                                                    </td>
                                                                                    <td width="50px" class="clsdgHeader">
                                                                                        <span>Attach</span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                            <asp:GridView ID="dgEmployeeDepartmentInfoList" runat="server" CssClass="clsGrid"
                                                                                DataKeyNames="ID" AutoGenerateColumns="False" ShowHeader="false" ShowHeaderWhenEmpty="true"
                                                                                Style="width: 100%;" AllowPaging="True" PageSize="10">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                <Columns>
                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                    <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="EmployeeDepartmentName" HeaderText="Name">
                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                    </asp:ButtonField>
                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                    </asp:ButtonField>
                                                                                    <asp:TemplateField HeaderText="Attach">
                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkDepartmentView" runat="server" Text="View" CommandName="View"
                                                                                                CausesValidation="false"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="ImageSize" HeaderText="Size"></asp:BoundField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                               
                                                                <%--ExpandedSize="137"--%>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                  </ContentTemplate>
                                 </cc2:TabPanel>
                                        <cc2:TabPanel ID="tabWOJobDesignationAllocation" runat="server" CssClass="clsPanel1"
                                            ClientIDMode="Static">
                                            <HeaderTemplate>
                                               <asp:Label runat="server" Text="" ID="lblContactRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlContactInfo1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlContactInfoResult" runat="server">
                                                            <table id="Table23" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td >
                                                                        <asp:Panel ID="pnlExpandContactInfo" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table id="Table6" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="right" >
                                                                                                    <asp:UpdatePanel ID="upnlAddContactInfoChild" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnContactInfoAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Next To Kin Info"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button></TD>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                    <!--CHK-->
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="40px" class="clsdgHeader">
                                                                                                        <span>Name</span>
                                                                                                    </td>
                                                                                                    <td width="60px" class="clsdgHeader">
                                                                                                        <span>Relation</span>
                                                                                                    </td>
                                                                                                    <td width="60px" class="clsdgHeader">
                                                                                                        <span>Address</span>
                                                                                                    </td>
                                                                                                    <td width="30px" class="clsdgHeader">
                                                                                                        <span>City</span>
                                                                                                    </td>
                                                                                                    <td width="40px" class="clsdgHeader">
                                                                                                        <span>State</span>
                                                                                                    </td>
                                                                                                    <td width="60px" class="clsdgHeader">
                                                                                                        <span>Country</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>PhoneNo1</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>PhoneNo2</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Mobile</span>
                                                                                                    </td>
                                                                                                    <td width="40px" class="clsdgHeader">
                                                                                                        <span>Email</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Attach</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width:100%;">
                                                                                            <asp:GridView ID="dgContactInfoList" runat="server" ShowHeaderWhenEmpty="true" CssClass="clsGrid"
                                                                                                AutoGenerateColumns="False" ShowHeader="false" Style="width: 100%;" AllowPaging="True"
                                                                                                DataKeyNames="ID" PageSize="10">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Name" HeaderText="Name">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="40px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Relation" HeaderText="Relation">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Address" HeaderText="Address">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="CityName" HeaderText="City">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="30px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="StateName" HeaderText="State">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="40px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="CountryName" HeaderText="Country">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="PhoneNo1" HeaderText="PhoneNo1">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="PhoneNo2" HeaderText="PhoneNo2">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Mobile" HeaderText="Mobile">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Email" HeaderText="Email">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="40px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:TemplateField HeaderText="Attach">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton runat="server" ID="lnkContactInfoView" Text="View" CommandName="View"
                                                                                                                CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="tabWOJobSpares" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                              <asp:Label runat="server" Text="" ID="lblDesignationRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlDesignation" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlDesignationResult" runat="server">
                                                            <table id="Table14" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandDesignation" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <table id="Table5" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                </td>
                                                                                                <td >
                                                                                                    <asp:UpdatePanel ID="upnlAddEmpDesgChild" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnDesignationAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Designation"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="90px" class="clsdgHeader">
                                                                                                        <span>Date</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Designation</span>
                                                                                                    </td>
                                                                                                    <td width="60px" class="clsdgHeader">
                                                                                                        <span>Promoted</span>
                                                                                                    </td>
                                                                                                    <td width="90px" class="clsdgHeader">
                                                                                                        <span>Remark</span>
                                                                                                    </td>
                                                                                                    <td width="80px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="60px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <td width="60px" class="clsdgHeader">
                                                                                                        <span>Attach</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%">
                                                                                            <asp:GridView ID="dgDesignationList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                                                Style="width: 100%;" ShowHeader="false" DataKeyNames="ID,DesignationName" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="DesignationName" HeaderText="Designation">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="Promoted">
                                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="Center" Width="60px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkIsPromoted" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsPromoted") %>'
                                                                                                                Enabled="False"></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="80px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:TemplateField HeaderText="Attach">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton runat="server" ID="lnkDesignationView" Text="View" CommandName="View"
                                                                                                                CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="TabPanel1" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                               <asp:Label runat="server" Text="" ID="lblServiceRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlService" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlServiceResult" runat="server">
                                                            <table id="Table15" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandService" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table id="Table1" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="right">
                                                                                                    <asp:UpdatePanel ID="upnlAddServiceChild" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnServiceAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Service"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                    <!--CHK-->
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="90px" class="clsdgHeader">
                                                                                                        <span>Date</span>
                                                                                                    </td>
                                                                                                    <td width="180px" class="clsdgHeader">
                                                                                                        <span>Service Name</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Attach</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%">
                                                                                            <asp:GridView ID="dgServiceList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                                                DataKeyNames="ID" Style="width: 100%;" ShowHeader="false" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="EmployeeServiceDateFormatted" HeaderText="Date">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="ServiceName" HeaderText="Service Name">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="180px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:TemplateField HeaderText="Attach">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton runat="server" Text="View" ID="lnkServiceView" CommandName="View"
                                                                                                                CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="TabPanel2" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                             <asp:Label runat="server" Text="" ID="lblDocumentRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upnlDocument" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlDocumentResult" runat="server">
                                                            <table id="Table16" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandDocument" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table id="Table4" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="right">
                                                                                                    <asp:UpdatePanel runat="server" ID="upnlAddDocumentChild" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnDocumentAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Document"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="110px" class="clsdgHeader">
                                                                                                        <span>Document Name</span>
                                                                                                    </td>
                                                                                                    <td width="90px" class="clsdgHeader">
                                                                                                        <span>Document No</span>
                                                                                                    </td>
                                                                                                    <td width="90px" class="clsdgHeader">
                                                                                                        <span>Date of Issue </span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Place of Issue</span>
                                                                                                    </td>
                                                                                                    <td width="55px" class="clsdgHeader">
                                                                                                        <span>Validity</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Date of Expiry</span>
                                                                                                    </td>
                                                                                                    <td width="75px" class="clsdgHeader">
                                                                                                        <span>Applicability</span>
                                                                                                    </td>
                                                                                                    <td width="115px" class="clsdgHeader">
                                                                                                        <span>Issuing Authority</span>
                                                                                                    </td>
                                                                                                    <td width="95px" class="clsdgHeader">
                                                                                                        <span>Warning Days</span>
                                                                                                    </td>
                                                                                                    <td width="55px" class="clsdgHeader">
                                                                                                        <span>Remark</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Renew</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Attach</span>
                                                                                                    </td>
                                                                                                    <td width="55px" class="clsdgHeader">
                                                                                                        <span>History</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                            <asp:GridView ID="dgDocumentList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                                                Style="width: 100%;" ShowHeader="false" DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <%--0--%>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <%--1--%>
                                                                                                    <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                                                                    <%--2--%>
                                                                                                    <asp:BoundField DataField="DocumentName" HeaderText="Document Name">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--3--%>
                                                                                                    <asp:BoundField DataField="DocNo" HeaderText="Document No">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--4--%>
                                                                                                    <asp:BoundField DataField="DateOfIssueFormatted" HeaderText="Date of Issue">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true">
                                                                                                        </ItemStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <%--5--%>
                                                                                                    <asp:BoundField DataField="PlaceOfIssue" HeaderText="Place of Issue">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--6--%>
                                                                                                    <asp:BoundField DataField="Validity" HeaderText="Validity">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--7--%>
                                                                                                    <asp:BoundField DataField="DateOfExpiryFormatted" HeaderText="Date of Expiry">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true">
                                                                                                        </ItemStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <%--8--%>
                                                                                                    <asp:TemplateField HeaderText="Applicable">
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Center" CssClass="TextBreak" Width="75px" Wrap="true">
                                                                                                        </ItemStyle>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                                                                                Enabled="False"></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <%--9--%>
                                                                                                    <asp:BoundField DataField="IssuingAuthority" HeaderText="Issuing Authority">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="115px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--10--%>
                                                                                                    <asp:BoundField DataField="WarningDays" HeaderText="Warning Days">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="95px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--11--%>
                                                                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--12--%>
                                                                                                    <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <%--13--%>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <%--14--%>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <%--15--%>
                                                                                                    <asp:TemplateField HeaderText="Attach">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton runat="server" ID="lnkDocumentView" Text="View" CommandName="View"
                                                                                                                CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <%--16--%>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                    <%--17--%>
                                                                                                    <asp:TemplateField HeaderText="History">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lnkDocumentHistory" runat="server" Text="History" CommandName="History"
                                                                                                                CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <%--18--%>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>
                                                                                                    <%--19--%>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="IsApplicable" HeaderText="IsApplicable"></asp:BoundField>
                                                                                                    <%--20--%>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="OneTimeDocument" HeaderText="OneTimeDocument"></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="TabPanel3" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                               <asp:Label runat="server" Text="" ID="lblTrainingRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlTraining" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlTrainingResult" runat="server">
                                                            <table id="Table17" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandTraining" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <table id="Table3" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td >
                                                                                                    <asp:UpdatePanel ID="upnlAddTrainingChild" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnTrainingAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Training"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="113px" class="clsdgHeader">
                                                                                                        <span>Training Name</span>
                                                                                                    </td>
                                                                                                    <td width="103px" class="clsdgHeader">
                                                                                                        <span>Certificate No</span>
                                                                                                    </td>
                                                                                                    <td width="90px" class="clsdgHeader">
                                                                                                        <span>Date</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Duration</span>
                                                                                                    </td>
                                                                                                    <td width="75px" class="clsdgHeader">
                                                                                                        <span>Freq. In Months</span>
                                                                                                    </td>
                                                                                                    <td width="135px" class="clsdgHeader">
                                                                                                        <span>Training Org Name</span>
                                                                                                    </td>
                                                                                                    <td width="128px" class="clsdgHeader">
                                                                                                        <span>Month Of Training</span>
                                                                                                    </td>
                                                                                                    <td width="115px" class="clsdgHeader">
                                                                                                        <span>Year of Training</span>
                                                                                                    </td>
                                                                                                    <td width="55px" class="clsdgHeader">
                                                                                                        <span>Remark</span>
                                                                                                    </td>
                                                                                                    <td width="80px" class="clsdgHeader">
                                                                                                        <span>Not Applicable</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Renew</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Attach</span>
                                                                                                    </td>
                                                                                                    <td width="55px" class="clsdgHeader">
                                                                                                        <span>History</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 270px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                            <asp:GridView ID="dgTrainingList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                                                Style="width: 100%;" ShowHeader="false" DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="TrainingName" HeaderText="Training Name">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No">
                                                                                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="EmployeeTrainingDate" HeaderText="Date">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Duration" HeaderText="Training Duration">
                                                                                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="FreqInMonths" HeaderText="Freq In Months">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="TrainingOrgNameWithCity" HeaderText="Training Org Name">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="130px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="MonthOfTrainingName" HeaderText="Month Of Training">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="125px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="YearOfTraining" HeaderText="Year of Training">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="115px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="NOT Applicable" ItemStyle-Width="80px">
                                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkIsNOTApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsNOTApplicable") %>'
                                                                                                                Enabled="False"></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <%--12--%>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <%--13--%>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <%--14--%>
                                                                                                    <asp:ButtonField Text="View" HeaderText="Attach" CommandName="View">
                                                                                                        <%--15--%>
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
                                                                                                    <%--16--%>
                                                                                                    <asp:ButtonField Text="History" HeaderText="History" CommandName="History">
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <%--17--%>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>
                                                                                                    <%--18--%>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="IsNOTApplicable" HeaderText="IsNOTApplicable"></asp:BoundField>
                                                                                                    <%--19--%>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="TabPanel4" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="" ID="lblSkillRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlSkill" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlSkillResult" runat="server">
                                                            <table id="Table18" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandSkill" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td >
                                                                                                    <asp:UpdatePanel ID="upnlAddSkillChild" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnSkillAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Skill"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width:100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="110px" class="clsdgHeader">
                                                                                                        <span>Skill Code</span>
                                                                                                    </td>
                                                                                                    <td width="110px" class="clsdgHeader">
                                                                                                        <span>Skill Name</span>
                                                                                                    </td>
                                                                                                    <%--
                                                                        'Added by Shital on 18-Aug-2016--%>
                                                                                                    <%-- <td width="80px" class="clsdgHeader">
                                                                                       <span>Value</span>
                                                                                    </td>--%>
                                                                                                    <%-- <td width="50px" class="clsdgHeader">
                                                                                        <span>Skill</span>
                                                                                    </td>--%>
                                                                                                    <%--  <td width="100px" class="clsdgHeader">
                                                                                      <span>Remark</span>
                                                                                    </td>--%>
                                                                                                    <%--<td width="70px" class="clsdgHeader">
                                                                                        <span>Edit/View</span>
                                                                                    </td>--%>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <%-- <td width="50px" class="clsdgHeader">
                                                                                       <span>Attach</span>
                                                                                    </td>--%>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                            <asp:GridView ID="dgSkillList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                                                DataKeyNames="ID" Style="width: 100%;" ShowHeader="false" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <%--'Added by Shital on 18-Aug-2016--%>
                                                                                                    <asp:BoundField DataField="SkillCode" HeaderText="Skill Code">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="SkillName" HeaderText="Skill Name">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Value" HeaderText="Value" Visible="false">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="80px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="Skill" Visible="false">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="Center" Width="50px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkIsSkill" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSkill") %>'
                                                                                                                Enabled="False"></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="Remark" HeaderText="Remark" Visible="false">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="Center" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec" Visible="false">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:TemplateField HeaderText="Attach" Visible="false">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton runat="server" ID="lnkSkillView" Text="View" CommandName="View" CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="ImageSize" HeaderText="ImageSize" Visible="false"></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="TabPanel5" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                               <asp:Label runat="server" Text="" ID="lblDisciplinaryRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlDisciplinary" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlDisciplinaryResult" runat="server">
                                                            <table id="Table19" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandDisciplinary" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <table id="Table9" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td >
                                                                                                    <asp:UpdatePanel ID="upnlAddDisciplinaryChild" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnDisciplinaryAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Disciplinary "
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Incident Date</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Description</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Reported By</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Disciplinary Action</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Comments</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>FeedBack</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Attach</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                            <asp:GridView ID="dgDisciplinaryList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                                                DataKeyNames="ID" Style="width: 100%;" ShowHeader="false" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="IncidentDateFormatted" HeaderText="Incident Date">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="ReportedBy" SortExpression="ReportedBy" HeaderText="Reported By">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="DisciplinaryName" HeaderText="Disciplinary Action">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Comments" SortExpression="Comments" HeaderText="Comments">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="FeedBack" SortExpression="FeedBack" HeaderText="FeedBack">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:TemplateField HeaderText="Attach">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton runat="server" ID="lnkDisciplinaryView" Text="View" CommandName="View"
                                                                                                                CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="TabPanel6" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" ID="lblLeaveRecCount">Leave Record</asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlLeaves" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlLeaveResult" runat="server">
                                                            <table id="Table20" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandLeaveRecord" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <table id="Table10" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td >
                                                                                                    <asp:UpdatePanel ID="upnlAddLeaveChild" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnLeaveAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Leave"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="110px" class="clsdgHeader">
                                                                                                        <span>Classification</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>From Date</span>
                                                                                                    </td>
                                                                                                    <td width="120px" class="clsdgHeader">
                                                                                                        <span>No Of Days</span>
                                                                                                    </td>
                                                                                                    <td width="140px" class="clsdgHeader">
                                                                                                        <span>Re-Joining Date</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Note</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Attach</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                            <asp:GridView ID="dgLeaveRecordList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                                                DataKeyNames="ID" Style="width: 100%;" ShowHeader="false" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="ClassificationName" HeaderText="Classification">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="NoOfDays" HeaderText="No Of Days">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField Visible="False" DataField="ToDateFormatted" HeaderText="To Date">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="140px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="ReJoiningDateFormatted" HeaderText="Re-Joining Date">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="140px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Note" HeaderText="Note">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:TemplateField HeaderText="Attach">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton runat="server" ID="lnkLeaveView" Text="View" CommandName="View" CausesValidation="false"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                        DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel ID="TabPanel7" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="" ID="lblEquipmentRecCount"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="upnlCompanyEquipment" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlCompanyEquipment" runat="server">
                                                            <table id="Table21" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlExpandCompanyEquipment" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <table id="Table12" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td >
                                                                                                    <asp:UpdatePanel runat="server" ID="upnlEquipmentAdd" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Button ID="btnCompanyEquipment" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Company Equipment"
                                                                                                                Text="Add" CausesValidation="False"></asp:Button>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="width: 100%;">
                                                                                            <table class="clsGrid" style="width: 100%; border-collapse: collapse;" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="120px" class="clsdgHeader">
                                                                                                        <span>Equipment</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Details</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Issued Date</span>
                                                                                                    </td>
                                                                                                    <td width="100px" class="clsdgHeader">
                                                                                                        <span>Returned Date</span>
                                                                                                    </td>
                                                                                                    <td width="70px" class="clsdgHeader">
                                                                                                        <span>Edit/View</span>
                                                                                                    </td>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Delete</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                            <asp:GridView ID="dgCompanyEquipmentList" runat="server" CssClass="clsGrid" Style="width: 100%;"
                                                                                                ShowHeader="false" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="EquipmentName" HeaderText="Equipment">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="EquipmentDetails" HeaderText="Details">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="EquipmentIssuedDateFormatted" HeaderText="Issued Date">
                                                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="EquipmentReturnedDateFormatted" HeaderText="Returned Date">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                        <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                    </asp:ButtonField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                            

                                  </cc2:TabContainer>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </td> 
                        </tr>
                   
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton" Text="Print" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton" Text="Back" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnEmpDept" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpContactInfo" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpDesg" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpService" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpDocument" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpDocumentHistory" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpTraining" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpTrainingHistory" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpSkill" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpDisciplinary" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpLeave" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpCompanyEquipment" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    <!-- Employee Department Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDept" Text="Employee Department" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDept" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDept" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDept" runat="server" TargetControlID="btnDummyEmpDept"
        PopupControlID="pnlEmpDept" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDeptStateComplete() {
            $("#btnDummyEmpDept").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDeptWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDept").attr("src", "wfEmployeeDepartmentInfo_Ajax.aspx?Type=pup");
                // $("#IframeKit").load(function () {
                //                    var doc = IframeKit.window;
                //                    IframeKit.SetPageLayout();

                if (!$.browser.msie) {
                    $("#btnDummyEmpDept").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDept() {
            var EmpDeptwindow = $find("<%=mdlPopupEmpDept.ClientID %>");
            //close kit popup window
            EmpDeptwindow.hide();
            //           release resources
            $("#IframeEmpDept").attr("src", "JavaScript:''");
            //call kit image button
            $("#hdnBtnEmpDept").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Contact Info Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpContactInfo" Text="Employee Contact Info"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpContactInfo" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpContactInfo" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpContactInfo" runat="server" TargetControlID="btnDummyEmpContactInfo"
        PopupControlID="pnlEmpContactInfo" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpContactInfoStateComplete() {
            $("#btnDummyEmpContactInfo").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpContactInfoWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpContactInfo").attr("src", "wfEmployeeContactInfo_Ajax.aspx?Type=pup");


                if (!$.browser.msie) {
                    $("#btnDummyEmpContactInfo").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpContactInfo() {
            var EmpContactInfowindow = $find("<%=mdlPopupEmpContactInfo.ClientID %>");
            //close kit popup window
            EmpContactInfowindow.hide();
            //           release resources
            $("#IframeEmpContactInfo").attr("src", "JavaScript:''");
            //call kit image button
            $("#hdnBtnEmpContactInfo").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Designation Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDesg" Text="Employee Designation" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDesg" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDesg" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDesg" runat="server" TargetControlID="btnDummyEmpDesg"
        PopupControlID="pnlEmpDesg" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDesgStateComplete() {
            $("#btnDummyEmpDesg").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDesgWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDesg").attr("src", "wfEmployeeDesignation_Ajax.aspx?Type=pup");


                if (!$.browser.msie) {
                    $("#btnDummyEmpDesg").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDesg() {
            var EmpDesgwindow = $find("<%=mdlPopupEmpDesg.ClientID %>");
            //close kit popup window
            EmpDesgwindow.hide();
            //           release resources
            $("#IframeEmpDesg").attr("src", "JavaScript:''");
            //call kit image button
            $("#hdnBtnEmpDesg").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Service Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpService" Text="Employee Service" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpService" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpService" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpService" runat="server" TargetControlID="btnDummyEmpService"
        PopupControlID="pnlEmpService" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpServiceStateComplete() {
            $("#btnDummyEmpService").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpServiceWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpService").attr("src", "wfEmployeeService_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpService").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpService() {
            var EmpServicewindow = $find("<%=mdlPopupEmpService.ClientID %>");
            //close kit popup window
            EmpServicewindow.hide();
            //           release resources
            $("#IframeEmpService").attr("src", "JavaScript:''");
            //call kit image button
            $("#hdnBtnEmpService").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Document Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDocument" Text="Employee Document" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDocument" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDocument" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDocument" runat="server" TargetControlID="btnDummyEmpDocument"
        PopupControlID="pnlEmpDocument" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDocumentStateComplete() {
            $("#btnDummyEmpDocument").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDocumentWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDocument").attr("src", "wfEmployeeDocument_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpDocument").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDocument() {
            var EmpDocumentwindow = $find("<%=mdlPopupEmpDocument.ClientID %>");
            //close kit popup window
            EmpDocumentwindow.hide();
            //           release resources
            $("#IframeEmpDocument").attr("src", "JavaScript:''");
            //call kit image button
            $("#hdnBtnEmpDocument").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Document History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDocumentHistory" Text="Employee Document History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDocumentHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDocumentHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDocumentHistory" runat="server" TargetControlID="btnDummyEmpDocumentHistory"
        PopupControlID="pnlEmpDocumentHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDocumentHistoryStateComplete() {
            $("#btnDummyEmpDocumentHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDocumentHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDocumentHistory").attr("src", "wfEmployeeDocumentHistoryList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpDocumentHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDocumentHistory() {
            var EmpDocumentHistorywindow = $find("<%=mdlPopupEmpDocumentHistory.ClientID %>");
            //close popup window
            EmpDocumentHistorywindow.hide();
            //           release resources
            $("#IframeEmpDocumentHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpDocumentHistory").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Training Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpTraining" Text="Employee Training" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpTraining" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpTraining" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpTraining" runat="server" TargetControlID="btnDummyEmpTraining"
        PopupControlID="pnlEmpTraining" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpTrainingStateComplete() {
            $("#btnDummyEmpTraining").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpTrainingWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpTraining").attr("src", "wfEmployeeTraining_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpTraining").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function OpenTrainingGroupWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';

                $("#IframeEmpTraining").attr("src", "wfTrainingGroupSelectionList_Ajax.aspx?Type=pup");
                if (!$.browser.msie) {
                    $("#btnDummyEmpTraining").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpTraining() {
            var EmpTrainingwindow = $find("<%=mdlPopupEmpTraining.ClientID %>");
            //close Training popup window
            EmpTrainingwindow.hide();
            //           release resources
            $("#IframeEmpTraining").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpTraining").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Training History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpTrainingHistory" Text="Employee Training History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpTrainingHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpTrainingHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpTrainingHistory" runat="server" TargetControlID="btnDummyEmpTrainingHistory"
        PopupControlID="pnlEmpTrainingHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpTrainingHistoryStateComplete() {
            $("#btnDummyEmpTrainingHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpTrainingHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpTrainingHistory").attr("src", "wfEmployeeTrainingHistoryList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpTrainingHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpTrainingHistory() {
            var EmpTrainingHistorywindow = $find("<%=mdlPopupEmpTrainingHistory.ClientID %>");
            //close Training popup window
            EmpTrainingHistorywindow.hide();
            //           release resources
            $("#IframeEmpTrainingHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpTrainingHistory").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Skill Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpSkill" Text="Employee Skill" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpSkill" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpSkill" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpSkill" runat="server" TargetControlID="btnDummyEmpSkill"
        PopupControlID="pnlEmpSkill" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpSkillStateComplete() {
            $("#btnDummyEmpSkill").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpSkillWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpSkill").attr("src", "wfEmployeeSkill_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpSkill").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpSkill() {
            var EmpSkillwindow = $find("<%=mdlPopupEmpSkill.ClientID %>");
            //close Skill popup window
            EmpSkillwindow.hide();
            //           release resources
            $("#IframeEmpSkill").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpSkill").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Disciplinary Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDisciplinary" Text="Employee Disciplinary"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDisciplinary" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDisciplinary" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDisciplinary" runat="server" TargetControlID="btnDummyEmpDisciplinary"
        PopupControlID="pnlEmpDisciplinary" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDisciplinaryStateComplete() {
            $("#btnDummyEmpDisciplinary").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDisciplinaryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDisciplinary").attr("src", "wfEmployeeDisciplinary_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpDisciplinary").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDisciplinary() {
            var EmpDisciplinarywindow = $find("<%=mdlPopupEmpDisciplinary.ClientID %>");
            //close Disciplinary popup window
            EmpDisciplinarywindow.hide();
            //           release resources
            $("#IframeEmpDisciplinary").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpDisciplinary").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Leave Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpLeave" Text="Employee Leave" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpLeave" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpLeave" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpLeave" runat="server" TargetControlID="btnDummyEmpLeave"
        PopupControlID="pnlEmpLeave" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpLeaveStateComplete() {
            $("#btnDummyEmpLeave").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpLeaveWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpLeave").attr("src", "wfEmployeeLeaves_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpLeave").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpLeave() {
            var EmpLeavewindow = $find("<%=mdlPopupEmpLeave.ClientID %>");
            //close Leave popup window
            EmpLeavewindow.hide();
            //           release resources
            $("#IframeEmpLeave").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpLeave").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Equipment Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpEquipment" Text="Employee Equipment" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpEquipment" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpEquipment" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpEquipment" runat="server" TargetControlID="btnDummyEmpEquipment"
        PopupControlID="pnlEmpEquipment" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpEquipmentStateComplete() {
            $("#btnDummyEmpEquipment").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpEquipmentWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpEquipment").attr("src", "wfCompanyEquipment_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpEquipment").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpEquipment() {
            var EmpEquipmentwindow = $find("<%=mdlPopupEmpEquipment.ClientID %>");
            //close Equipment popup window
            EmpEquipmentwindow.hide();
            //           release resources
            $("#IframeEmpEquipment").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpCompanyEquipment").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
