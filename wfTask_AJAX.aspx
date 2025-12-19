<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTask_AJAX.aspx.vb" Inherits="Flypal.wfTask_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task</title>
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
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>

                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Task [New]</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                            <td align="right">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                            ToolTip="Click to add the new Task" Text="New"></asp:Button>

                                                        <asp:Button ID="btnSaveTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to save the Task Information"
                                                            Text="Save"></asp:Button>

                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                            ToolTip="Click to close Task screen" Text="Close"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvAuditCategory" runat="server" CssClass="clsLabelAuto"
                                                ControlToValidate="cmbAuditCategory" ErrorMessage="Please select Audit Category."
                                                Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvDepartment" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbDepartmentList"
                                                ErrorMessage="Please select Department." Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDescription"
                                                ErrorMessage="Description should not be greater than 1000 characters." Display="None"
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCode" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtCode"
                                                ErrorMessage="Code should not be greater than 100 characters." Display="None"
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvNote" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtNote"
                                                ErrorMessage="Note should not be greater than 1000 characters." Display="None"
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <%-- <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                                ErrorMessage="Description Required." Display="None"></asp:RequiredFieldValidator>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <asp:UpdatePanel ID="upnlTaskDet" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsTaskdetail" class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend id="ldTaskdetail" runat="server"><b>Task Details</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td align="right">
                                                            <%--<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                ToolTip="Click to add the new Task" Text="New"></asp:Button>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblAuditStandard" runat="server" Width="104px" CssClass="clsLabel">Audit Standard</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAuditStandard" runat="server" CssClass="clsTextBoxTagSearch"
                                                                            ToolTip="Audit Standard" MaxLength="100" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblAuditCategory" runat="server" CssClass="clsLabelAuto">Task Category</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table2">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel ID="upnlAuditCategory" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:DropDownList ID="cmbAuditCategory" runat="server" Width="275px" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                SelectedValue="<%# mAuditTask.AuditCategoryID %>" DataTextField="CategoryIdnNo"
                                                                                                DataValueField="ID">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td align="right" style="padding: 0px;">
                                                                                    <asp:ImageButton ID="imgbtnAuditCategory" runat="server" ImageUrl="~/images/plus1.png"
                                                                                        Height="22px" Width="24px" CausesValidation="False" ToolTip="Click to Add New Task Category" />
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
                                                                        <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                            MaxLength="5000" Text="<%# mAuditTask.Description %>" TextMode="MultiLine" ToolTip="Enter Description"></asp:TextBox>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblCode" runat="server" CssClass="clsLabelAuto">Code</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Code"
                                                                            Text="<%# mAuditTask.Code %>" MaxLength="100" Width="275px"></asp:TextBox>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto">Note</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" MaxLength="1000"
                                                                            Text="<%# mAuditTask.Note %>" TextMode="MultiLine" ToolTip="Enter Note"></asp:TextBox>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table border="0" cellpadding="1" cellspacing="1">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <input type="button" id="btnSelectFile" runat="server" value="Select File" style="width: 100px;"
                                                                                                clientidmode="Static" class="clsbtnH" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH" ToolTip="Click to Remove Attachment"
                                                                                                Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                                        </td>
                                                                                        <td style="padding-left: 2px;">
                                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                                Height="24px" Width="15px"></asp:ImageButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>

                                                                <asp:PlaceHolder ID="pl" runat="server" Visible="false">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblDepartment" runat="server" CssClass="clsLabelAuto">Department</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <table id="Table13">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:UpdatePanel ID="upnlDepartment" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:DropDownList ID="cmbDepartmentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                    SelectedValue="<%# mAuditTask.DepartmentID %>" DataTextField="Name" DataValueField="ID"
                                                                                                    Width="280px">
                                                                                                </asp:DropDownList>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                    <td align="right" style="padding: 0px;">
                                                                                        <asp:ImageButton ID="imgbtnDepartment" runat="server" ImageUrl="~/images/plus1.png"
                                                                                            Height="22px" Width="24px" CausesValidation="False" ToolTip="Click to Add New Department" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </asp:PlaceHolder>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <legend id="Legend1" runat="server"><b>Search Information</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table id="Table4" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="L1" runat="server" Width="12px" CssClass="clsLabelauto"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblSearchBy" runat="server" Width="112px" CssClass="clsLabelAuto"
                                                                            Height="16px">Search By</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                            <asp:ListItem Value="1">Task Category</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="L2" runat="server" Width="10px" CssClass="clsLabelAuto"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbDepartmentListSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                            DataTextField="Name" DataValueField="ID" Visible="False">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="cmbTaskCategorySearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                            DataTextField="Name" DataValueField="ID" Visible="False">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <table id="Table6">
                                                                <tr>
                                                                    <td>
                                                                        <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                            ToolTip="Click to find Task List as per searching criteria" Text="Find Now"></asp:Button>--%>
                                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                            ToolTip="Click to find Task List as per searching criteria"
                                                                            CausesValidation="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Task List</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskList" runat="server" AutoGenerateColumns="False" Visible="true"
                                                CssClass="clsGridNewStyle" PageSize="3" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="AuditCategoryName" SortExpression="Audit Category" HeaderText="Task Category">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IdentificationNo" SortExpression="IdentificationNo" HeaderText="Identification No.">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%-- <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
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
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
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
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to save the Task Information"
                                                            Text="Save" Visible="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False" Visible="False"
                                                            ToolTip="Click to close Task screen" Text="Close"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgBtnAuditCategory" runat="server" CausesValidation="false" ClientIDMode="Static"
                                                                    Style="display: none;" Text="Add" />
                                                                <asp:Button ID="hdnimgBtnAuditDepartment" runat="server" CausesValidation="false"
                                                                    ClientIDMode="Static" Style="display: none;" Text="Add" />
                                                                <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                    Style="display: none;" Text="----" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
            PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameFileUploadStateComplete() {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            $(document).ready(function () {
                $("#btnSelectFile").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                        //                        $("#IFileUpload").ready(function () {
                        //                            $("#btnDummyFileUpload").click();
                        //                            $get("AjaxLoader").style.visibility = 'hidden';
                        //                        });
                        if (!$.browser.msie) {
                            $("#btnDummyFileUpload").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }


                });
            });
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForFileUpload(fileattached) {
                var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
                //close File Upload popup window
                FileUpwindow.hide();
                //Free resources
                $("#IFileUpload").attr("src", "JavaScript:''");
                if (fileattached) {
                    //call hidden button to set file upload content to object
                    $("#hdnBtnFileUpload").click();
                }
            }
        </script>
        <!-- End -->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunction();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameTaskMasterDetailStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
        <!-- AuditCategory Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAuditCategory" Text="Dummy AuditCategory"
                ClientIDMode="Static" CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupAuditCategory" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupAuditCategory" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAuditCategory" runat="server" TargetControlID="btnDummyAuditCategory"
            PopupControlID="pnlPopupAuditCategory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAuditCategoryStateComplete() {
                $("#btnDummyAuditCategory").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenAuditCategoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupAuditCategory").attr("src", "wfAuditCategory_AJAX.aspx?Type=pup&AType=3");

                    if (!$.browser.msie) {
                        $("#btnDummyAuditCategory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var AuditCategorywindow = $find("<%=mdlPopupAuditCategory.ClientID %>");
                //close AuditCategory popup window
                AuditCategorywindow.hide();
                $("#iPopupAuditCategory").attr("src", "JavaScript:''");
                //call AuditCategory image button
                $("#hdnimgBtnAuditCategory").click();
            }
        </script>
        <!-- End-->
        <!-- AuditDepartment Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAuditDepartment" Text="Dummy AuditDepartment"
                ClientIDMode="Static" CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupAuditDepartment" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupAuditDepartment" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAuditDepartment" runat="server" TargetControlID="btnDummyAuditDepartment"
            PopupControlID="pnlPopupAuditDepartment" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAuditDeptStateComplete() {
                $("#btnDummyAuditDepartment").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenAuditDepartmentWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupAuditDepartment").attr("src", "wfAuditDepartment_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAuditDepartment").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForAuditDept() {
                var AuditDepartmentwindow = $find("<%=mdlPopupAuditDepartment.ClientID %>");
                //close AuditDepartment popup window
                AuditDepartmentwindow.hide();
                $("#iPopupAuditDepartment").attr("src", "JavaScript:''");
                //call AuditDepartment image button
                $("#hdnimgBtnAuditDepartment").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
