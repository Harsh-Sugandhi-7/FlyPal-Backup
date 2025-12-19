<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManual_Ajax.aspx.vb"
    Inherits="Flypal.wfManual_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manual Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Manual [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                    <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table5" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSendNotification" CssClass="clsbtnH clsinfoH" runat="server"
                                                            Text="Send Notification" ToolTip="Click to Send Notification" Enabled="<%# Not mManual.IsNew and mManual.ManualSubscribers.Count > 0 %>">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Manual"
                                                            ValidationGroup="a"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" CssClass="clsbtnH clsinfoH" runat="server" Text="Print" ToolTip="Click to Print Manual"
                                                            Enabled="<%# Not mManual.IsNew %>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to go back to the previous page"
                                                            CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvCategory" runat="server" ClientValidationFunction="validateName"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbCategoryList" ErrorMessage="Category Required."></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                ErrorMessage="Name Required." Display="None" ValidationGroup="a" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvNameLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name should be less than or equal to 50 charecters."
                                                Display="None" ControlToValidate="txtName" ClientValidationFunction="validateName"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvApplicableLen" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Applicable For should be less than or equal to 50 characters."
                                                Display="None" ControlToValidate="txtApplicableFor" ClientValidationFunction="validateName"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvDesLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Short Description should be less than or equal to 500 characters."
                                                Display="None" ControlToValidate="txtDescription" ClientValidationFunction="validateName"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvNoteLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Note should be less than or equal to 255 charecters."
                                                Display="None" ControlToValidate="txtNote" ClientValidationFunction="validateName"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"></asp:CustomValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <script type="text/javascript">
                                                function validateName(source, args) {
                                                    var ControlName = source.controltovalidate;
                                                    switch (ControlName) {
                                                        case 'txtName':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 50) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;

                                                        case 'txtApplicableFor':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 50) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                        case 'txtDescription':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 500) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                        case 'txtNote':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 255) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                        case 'cmbCategoryList':
                                                            var Value = $get(ControlName);
                                                            if (Value.selectedIndex == 0) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                    }
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblManualDetail" class="clsLabelHeader">Manual Details</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlManualDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblNameStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblName" class="clsLabel">Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Width="250px"
                                                            Text="<%# mManual.Name %>" ToolTip="Enter Manual name" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="Span1" class="clsLabelAuto">Manual No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtManualNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                            Text="<%# mManual.ManualNo %>" ToolTip="Enter Manual No." Width="250px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblCategoryListStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblCategoryList" class="clsLabel">Category</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbCategoryList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataTextField="Name" DataValueField="ID" SelectedValue="<%# mManual.MCategoryID %>">
                                                        </asp:DropDownList>
                                                        <%--<asp:Button ID="btnAddCategory" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                            ToolTip="Click To Add Manual Category" CausesValidation="False"></asp:Button>--%>

                                                        <asp:ImageButton ID="btnAddCategory" runat="server" ImageUrl="~/images/plus1.png"
                                                            Height="22px" Width="24px" ToolTip="Click to Add Manual Category" CausesValidation="False" Style="margin-top: 6px"></asp:ImageButton>

                                                    </td>
                                                    <td>
                                                        <span id="lblApplicableFor" class="clsLabel">Applicable For</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtApplicableFor" runat="server" CssClass="clsTextBoxTagSearch" Width="250px"
                                                            Text="<%# mManual.ApplicableFor %>" ToolTip="Enter For which the Manual is applicable"
                                                            MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDescription" class="clsLabel">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            Text="<%# mManual.ShortDesc %>" ToolTip="Enter Description" MaxLength="500" TextMode="MultiLine"
                                                            Width="250px">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblNote" class="clsLabel">Note</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mManual.Note %>"
                                                            ToolTip="Enter Note of Manual" MaxLength="255" TextMode="MultiLine" Width="250px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblIsInUse" class="clsLabel">In Use</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsInUse" runat="server" CssClass="clsCheckBox" Checked="<%# mManual.IsInUse %>"
                                                            BorderStyle="None"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset id="Fieldset7" style="padding: 0px 4px 0px 0px; width: auto; z-index: 9000;"
                                        class="clsLabelHeader">
                                        <legend><b>Manual Subscription</b></legend>
                                        <asp:UpdatePanel runat="server" ID="upnlMManualSubscription" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabel">Validity</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkValidity" runat="server" CssClass="clsCheckBox" BorderStyle="None"
                                                                AutoPostBack="true" Checked="<%# mManual.Validity %>" Enabled="<%#  mManual.MManualSubscriptions.Count =0 %>"
                                                                Text="(Check if One Time)"></asp:CheckBox>
                                                        </td>
                                                        <td>
                                                            &nbsp<span class="clsLabel">|</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblFromDate" class="clsLabel">From Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagSearch" ClientIDMode="Static"
                                                                Text="<%# mManual.FromDate %>" runat="server" Enabled="false"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                            </cc2:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <span id="lblToDate" class="clsLabel">To Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mManual.ToDate %>" runat="server" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                            </cc2:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnRenew" CssClass="clsbtnH clsinfoH" runat="server" Text="Renew/View"
                                                                ToolTip="Click To Add Manual Subscription" Enabled="<%# IIF(mManual.Validity=True,False,True) %>">
                                                            </asp:Button>
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
                                    <asp:UpdatePanel runat="server" ID="upnlRevisions" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRevisions" runat="server" CssClass="clsLabelHeader">Manual Revision(s) </asp:Label>
                                                        <%--<asp:Button ID="btnAddRevision" CssClass="clsButton_Ajax" runat="server" Text="Add"
                                                            ToolTip="Click to Manual Revision Add"></asp:Button>--%>


                                                        <asp:ImageButton ID="btnAddRevision" runat="server" ImageUrl="~/images/plus1.png"
                                                            Height="22px" Width="24px" ToolTip="Click to Add Manual Revision" CausesValidation="False" Style="margin-top: 6px"></asp:ImageButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgRevisions" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="3" DataKeyNames="ID">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True"/>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="No" HeaderText="No.">
                                                                    <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RevNo" HeaderText="Revision No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Frequency" HeaderText="Frequency">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RevDate" HeaderText="Effective Date">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EffectiveDate" HeaderText="Next Revision Date">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="HardCopyString" HeaderText="Hard Copy">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"/>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SoftCopyString" HeaderText="Soft Copy">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"/>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Note" HeaderText="Note">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="EditView" HeaderText="Edit" Text="Edit" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlPropertyValue" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblManualPropertyValue" class="clsLabelHeader">Manual Property Value(s)</span>
                                                        <%--<asp:Button ID="btnAddPropertyValue" CssClass="clsButton_Ajax" runat="server" Text="Add"
                                                            ToolTip="Click to Add Property &amp; Its Value"></asp:Button>--%>

                                                        <asp:ImageButton ID="btnAddPropertyValue" runat="server" ImageUrl="~/images/plus1.png"
                                                            Height="22px" Width="24px" ToolTip="Click to Add Property & its Value" CausesValidation="False" Style="margin-top: 6px"></asp:ImageButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgManualPropertyValues" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="3" DataKeyNames="ID">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            
                                                            <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />

                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ManualPropertyName" HeaderText="Property">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Value" HeaderText="Value">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="EditView" HeaderText="Edit" Text="Edit" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>



                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditViewManpropval" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecordManpropval" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSubscriber" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblSubscriber" class="clsLabelHeader">Manual Subscriber(s)</span>
                                                        <%--<asp:Button ID="btnAddSubscriber" CssClass="clsButton_Ajax" runat="server" Text="Add"
                                                            ToolTip="Click to Add Manual Subscriber"></asp:Button>--%>

                                                        <asp:ImageButton ID="btnAddSubscriber" runat="server" ImageUrl="~/images/plus1.png"
                                                            Height="22px" Width="24px" ToolTip="Click to Add Subscriber" CausesValidation="False" Style="margin-top: 6px"></asp:ImageButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgSubscriberList" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="3" DataKeyNames="ID">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <input type="checkbox" id="chkSelectAll" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                            <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EmployeeName" HeaderText="Employee">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Email" HeaderText="Email">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AttachmentSendTiming" HeaderText="Notification Send On">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AttachmentReadTiming" HeaderText="Notification Read On">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="EditView" HeaderText="Edit" Text="Edit" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>


                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditViewManSub" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecordManSub" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table5" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSendNotification" Width="105px" CssClass="clsbtnH clsinfoH" runat="server"
                                                            Text="Send Notification" ToolTip="Click to Send Notification" Enabled="<%# Not mManual.IsNew and mManual.ManualSubscribers.Count > 0 %>">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Manual"
                                                            ValidationGroup="a"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" CssClass="clsbtnH clsinfoH" runat="server" Text="Print" ToolTip="Click to Print Manual"
                                                            Enabled="<%# Not mManual.IsNew %>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to go back to the previous page"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnManualRevision" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnSubscriber" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnManualCategory" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnPropertyValue" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnMManualSubscription" ClientIDMode="Static" runat="server" Text="----"
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
    </div>
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
    <!--ManualCategory Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyManualCategory" Text="ManualCategory" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlManualCategory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeManualCategory" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlManualCategory" runat="server" TargetControlID="btnDummyManualCategory"
        PopupControlID="pnlManualCategory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameManualCategoryStateComplete() {
            $("#btnDummyManualCategory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenManualCategoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeManualCategory").attr("src", "wfManualCategory_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyManualCategory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForManualCategory() {
            var ManualCategorywindow = $find("<%=mdlManualCategory.ClientID %>");
            //close popup window
            ManualCategorywindow.hide();
            //release resources
            $("#IframeManualCategory").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnManualCategory").click();
        }
    </script>
    <!-- End-->
    <!--ManualRevision Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyManualRevision" Text="ManualRevision" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlManualRevision" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeManualRevision" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlManualRevision" runat="server" TargetControlID="btnDummyManualRevision"
        PopupControlID="pnlManualRevision" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameManualRevisionStateComplete() {
            $("#btnDummyManualRevision").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenManualRevisionWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeManualRevision").attr("src", "wfManualRevision_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyManualRevision").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForManualRevision() {
            var ManualRevisionwindow = $find("<%=mdlManualRevision.ClientID %>");
            //close popup window
            ManualRevisionwindow.hide();
            //release resources
            $("#IframeManualRevision").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnManualRevision").click();
        }
    </script>
    <!-- End-->
    <!--Subscriber Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySubscriber" Text="Subscriber" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSubscriber" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSubscriber" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSubscriber" runat="server" TargetControlID="btnDummySubscriber"
        PopupControlID="pnlSubscriber" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSubscriberStateComplete() {
            $("#btnDummySubscriber").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSubscriberWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSubscriber").attr("src", "wfManualSubscriber_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySubscriber").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForSubscriber() {
            var Subscriberwindow = $find("<%=mdlPopupSubscriber.ClientID %>");
            //close popup window
            Subscriberwindow.hide();
            //release resources
            $("#IframeSubscriber").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnSubscriber").click();
        }
    </script>
    <!-- End-->
    <!--PropertyValue Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPropertyValue" Text="PropertyValue" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPropertyValue" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframePropertyValue" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupPropertyValue" runat="server" TargetControlID="btnDummyPropertyValue"
        PopupControlID="pnlPropertyValue" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFramePropertyValueStateComplete() {
            $("#btnDummyPropertyValue").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenPropertyValueWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframePropertyValue").attr("src", "wfManualPropertyValue_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyPropertyValue").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForPropertyValue() {
            var PropertyValueWindow = $find("<%=mdlPopupPropertyValue.ClientID %>");
            //close popup window
            PropertyValueWindow.hide();
            //release resources
            $("#IframePropertyValue").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnPropertyValue").click();
        }
    </script>
    <!-- End-->
    <!--MManual Subscription Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyMManualSubscription" Text="MManualSubscription"
            CausesValidation="false" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlMManualSubscription" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeMManualSubscription" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlMManualSubscription" runat="server" TargetControlID="btnDummyMManualSubscription"
        PopupControlID="pnlMManualSubscription" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameMManualSubscriptionComplete() {
            $("#btnDummyMManualSubscription").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenMManualSubscriptionWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeMManualSubscription").attr("src", "wfMManualSubscription_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyMManualSubscription").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForMManualSubscription() {
            var MManualSubscriptionwindow = $find("<%=mdlMManualSubscription.ClientID %>");
            //close popup window
            MManualSubscriptionwindow.hide();
            //release resources
            $("#IframeMManualSubscription").attr("src", "JavaScript:''");
            //call button click
            $("#hdnMManualSubscription").click();
        }
    </script>
    <!-- End-->
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).parents('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor')
                else
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).prop('checked');
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor');
                else
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });
        });
    </script>
</body>
</html>
