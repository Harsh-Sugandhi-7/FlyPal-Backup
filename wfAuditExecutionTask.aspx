<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditExecutionTask.aspx.vb"
    Inherits="Flypal.wfAuditExecutionTask" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html>
<head runat="server">
    <title>Audit Compliance Task</title>
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
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
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
                <table class="clstablelistin" id="tblInner">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblTitle" CssClass="clstitle1" runat="server">Audit Compliance Task</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                            <asp:CustomValidator ID="cvAuditCategory" runat="server" CssClass="clslabelauto"
                                ControlToValidate="cmbAuditCategory" ErrorMessage="Select Audit Category" Display="None"
                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="rfvDepartment" runat="server" CssClass="clslabelauto" ControlToValidate="cmbDepartment"
                                ErrorMessage="Select Department" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvTaskStatus" runat="server" CssClass="clslabelauto" ControlToValidate="cmbTaskStatus"
                                ErrorMessage="Select Task Status" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                Display="None" ErrorMessage="Description should not be greater than 1000 characters."
                                ControlToValidate="txtDescription"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clslabelauto"
                                Display="None" ErrorMessage="Enter Description" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblOtherChargeDetails" runat="server" CssClass="clsLabelHeader">Audit Compliance Task Details</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table id="Table2" cellspacing="1" cellpadding="1" border="0">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAuditCategory" runat="server" CssClass="clsLabel"> Task Category</asp:Label>
                                    </td>
                                    <td>
                                        <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="cmbAuditCategory" runat="server" CssClass="clsTextBox1" SelectedValue="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditCategoryID %>"
                                                        DataValueField="ID" DataTextField="Name" Enabled="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="imgbtnAuditCategory" runat="server" CssClass="clsButtonGrid" ToolTip="Click to Add New Task Category"
                                                        CausesValidation="False" Text="..." Visible="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCode" runat="server" CssClass="clsLabelAuto">Code </asp:Label>
                                    </td>
                                    <td>
                                        <table id="Table7" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBox1" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.Code %>"
                                                        BackColor="#E0E0E0" MaxLength="100" ToolTip="Code" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDepartment" runat="server" CssClass="clsLabelAuto">Department</asp:Label>
                                    </td>
                                    <td>
                                        <table id="Table4" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="cmbDepartment" runat="server" CssClass="clsComboBox2" SelectedValue="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.DepartmentID %>"
                                                        DataValueField="ID" DataTextField="Name" Enabled="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="imgbtnDepartment" runat="server" CssClass="clsButtonGrid" ToolTip="Click to Add New Department"
                                                        CausesValidation="False" Text="..." Visible="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblKindAttention" runat="server" CssClass="clsLabel">Kind Attention</asp:Label>
                                    </td>
                                    <td>
                                        <table id="Table8" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtKindAttention" runat="server" CssClass="clsTextBox1" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.KindAttention %>"
                                                        MaxLength="100" BackColor="White" ToolTip="Enter Kind Attention">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto"> Description </asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <table id="Table9" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultilineDefectAction"
                                                        Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.Description %>" MaxLength="5000"
                                                        BackColor="#E0E0E0" TextMode="MultiLine" ToolTip="Description" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTaskStatus" runat="server" CssClass="clsLabel">Task Status</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <table id="Table5" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="cmbTaskStatus" runat="server" CssClass="clsComboBox2" DataTextField="Name"
                                                        DataValueField="ID" SelectedValue="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.TaskStatusID %>">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto">Note</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <table id="Table10" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultilineDefectAction"
                                                        Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.Note %>" ToolTip="Note"
                                                        MaxLength="1000" BackColor="#E0E0E0" TextMode="MultiLine" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:CustomValidator ID="cvNote" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                            Display="None" ErrorMessage="Note should not be greater than 1000 characters."
                                            ControlToValidate="txtNote"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table id="Table6">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAuditScheduleTask" runat="server" CssClass="clsLabelHeaderItem"
                                            Width="220px">Audit Compliance Task Finding(s)</asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnAddExecutionTaskFinding" runat="server" CssClass="clsButton" Text="Add"
                                            ToolTip="Click to add Audit Compliance Task Finding" CausesValidation="False">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:DataGrid ID="dgAuditExecutionTaskFinding" runat="server" CssClass="clsGrid"
                                PageSize="3" AutoGenerateColumns="False">
                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SrNo" HeaderText="Sr.No."></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FindingNo" HeaderText="Finding No."></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Reference" HeaderText="Reference No."></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Finding" HeaderText="Finding"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="RootCause" HeaderText="Root Cause"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="PriorityName" HeaderText="Priority">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FindingStatusName" HeaderText="Finding Status"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DeadlineDateFormatted" HeaderText="Deadline Date">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Location" HeaderText="Location"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Category" HeaderText="Evidence"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="KindAttention" HeaderText="Responsible Person"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="CAPA" HeaderText="C/P Action"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="CorrectionDateFormatted" HeaderText="Correction Date">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ToMailID" HeaderText="ToMailID"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="CCMailID" HeaderText="CCMailID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Remark" HeaderText="Remark"></asp:BoundColumn>
                                    <asp:ButtonColumn Text="Edit" HeaderText="Edit" CommandName="Edit"></asp:ButtonColumn>
                                    <asp:ButtonColumn Text="Remove" HeaderText="Remove" CommandName="Remove"></asp:ButtonColumn>
                                    <asp:TemplateColumn HeaderText="Attach">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="View" CommandName="View" CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="ImageSize" HeaderText="Size"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Send Mail">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSendMail" runat="server" Text="Send Mail" CommandName="SendMail"
                                                CausesValidation="false" ToolTip="First click on Save button to Save the record then click Send Mail."></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                            <table id="Table1">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnOk" runat="server" CssClass="clsButton" ToolTip="Click to Save Audit Compliance Task"
                                            Text="Save"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous page"
                                            CausesValidation="False" Text="Close"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
