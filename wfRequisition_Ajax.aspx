<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisition_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfRequisition_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Requisition Details</title>
    <%-- <link id="MainStyle" type="text/css" rel="stylesheet" />--%>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function OpenLocation(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clspanel1" runat="server">
                    <table id="tblinner" class="clsTablelistin" border="0">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Requisition Details [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvEnquiryDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Select Requisition Date." ControlToValidate="txtRequisitionDate"
                                            Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvLocationName" runat="server" ErrorMessage="Select Requesting Location from the list."
                                            ControlToValidate="cmbLocationList" Display="None" ClientValidationFunction="validateLocation"
                                            ValidationGroup="1" CssClass="clsLabelAuto"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateLocation(source, args) {
                                                var dd = $get("cmbLocationList");
                                                if (dd.selectedIndex == 0) {
                                                    args.IsValid = false;
                                                    return;
                                                }
                                            }
                                        </script>
                                        <asp:CustomValidator ID="cvWorkShop" runat="server" Display="None" ControlToValidate="cmbWorkShop"
                                            ValidationGroup="1" ErrorMessage="Aircraft Required" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtEmployee"
                                            ValidateEmptyText="true" ValidationGroup="1" Display="None" ErrorMessage="Select Employee from list"
                                            OnServerValidate="customvalidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader" Text="<%# mRequisitionNew.StatusName %>">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlReqDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblRequisitionDetails" class="clsLabelHeader">Details </span>
                                                </td>
                                                <td colspan="3">
                                                    <span id="lblRequestedBy" class="clsLabelHeader">Requested By</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblSerialNo1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDate" class="clsLabel">Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRequisitionDate" CssClass="clsTextBoxTagSearchDate"
                                                        Width="100px" AutoPostBack="true" onchange="ValidateDateText(this,'RequisitionDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtRequisitionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRequisitionDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtRequisitionDate" ID="RequisitionDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <span id="lblLocation1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblLocation" class="clsLabel">Location</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbLocationList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        SelectedValue="<%# mRequisitionNew.LocationID %>" Enabled="<%# mRequisitionNew.IsNew %>"
                                                        DataValueField="ID" DataTextField="Name">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblNo" class="clsLabel">No.</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                    Text="<%# mRequisitionNew.Text %>" ToolTip="Enter Requisition Text">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Width="55px"
                                                                    MaxLength="8" Text="<%# mRequisitionNew.No %>" ToolTip="Enter Requisition No.">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStarEmployee" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>
                                                    <span id="lblEmployee" runat="server" class="clsLabel">Employee</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                        AutoPostBack="true" OnTextChanged="txtEmployee_TextChanged" Enabled="<%# mRequisitionNew.IsNew %>"
                                                        CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtEmployee_Autocomplete" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                        CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtEmployee"
                                                        UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                    </cc2:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnEmpId" runat="server" ClientIDMode="Static" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabel" Visible="<%#(mRequisitionNew.TransTypeID = 77)%>">Type</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbIndentType" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        Enabled="<%# mRequisitionNew.IsNew %>" Visible="<%#(mRequisitionNew.TransTypeID = 77)%>">
                                                        <asp:ListItem Text="SCHEDULE" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="NON-SCHEDULE" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto" Visible="<%#(mRequisitionNew.TransTypeID = 77)%>">Remark</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" TextMode="MultiLine"
                                                        Enabled="<%# mRequisitionNew.StatusID <= 1 %>" Text="<%# mRequisitionNew.Remark %>"
                                                        ToolTip="Enter Remark" Visible="<%#(mRequisitionNew.TransTypeID = 77)%>" Width="350px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStarNo" runat="server" CssClass="clsLabelStar" Visible="<%#(mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72 or mRequisitionNew.TransTypeID = 77 )%>">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel" Visible="<%#(mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72  or mRequisitionNew.TransTypeID = 77 )%>">Type</asp:Label>
                                                    <asp:Label ID="lblSupervisor" runat="server" CssClass="clsLabelAuto" Visible="<%#(mRequisitionNew.TransTypeID = 71)%>">Supervisor</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdoPartRequest" runat="server" AutoPostBack="true" Checked='<%#iif(((mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72 or (mRequisitionNew.TransTypeID = 77 and AppSettings("ClientCode")<> "Heligo")) and mRequisitionNew.ReqTypeID=1),True,False)%>'
                                                        CssClass="clsRadioButton" GroupName="X" Text="Part Request" TextAlign="Left"
                                                        Visible='<%#iif(((mRequisitionNew.TransTypeID = 77 and AppSettings("ClientCode") <>"Heligo") or mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72) ,True,False)%>' />
                                                    <asp:RadioButton ID="rdoPartPurchase" runat="server" AutoPostBack="true" Checked='<%#iif(((mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72 or mRequisitionNew.TransTypeID = 77) and mRequisitionNew.ReqTypeID=2) or (mRequisitionNew.TransTypeID = 77 and AppSettings("ClientCode") ="Heligo") ,True,False)%>'
                                                        CssClass="clsRadioButton" GroupName="X" Text="Part Purchase" TextAlign="Left"
                                                        Visible='<%#iif(((mRequisitionNew.TransTypeID = 65 and AppSettings("ClientCode") <>"Heligo") or mRequisitionNew.TransTypeID = 72 or mRequisitionNew.TransTypeID = 77) ,True,False)%>' />
                                                    <asp:TextBox ID="txtSupervisor" runat="server" CssClass="clsTextBox_Ajax" MaxLength="200"
                                                        Text="<%# mRequisitionNew.Supervisor %>" ToolTip="Enter Supervisor Name" Visible="<%#(mRequisitionNew.TransTypeID = 71)%>"
                                                        Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBranch" runat="server" class="clsLabel" Visible="<%#(mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72)%>">Branch</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbRequisitionEngineeringBranches" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        AutoPostBack="true" DataTextField="Branch" DataValueField="ID" SelectedValue="<%# mRequisitionNew.RequisitionEngineeringBrancheID %>"
                                                        Visible="<%#(mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72)%>">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" Visible="<%# mRequisitionNew.TransTypeID = 72%>">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWorkShop" runat="server" CssClass="clsLabelAuto" Visible="<%# mRequisitionNew.TransTypeID = 72 %>">WorkShop</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbWorkShop" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                        Width="100%" AutoPostBack="true" DataTextField="LocationWorkShop" DataValueField="ID"
                                                        SelectedValue="<%# mRequisitionNew.WorkShopID %>" Visible="<%# mRequisitionNew.TransTypeID=72 %>">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRecommendedBy" runat="server" CssClass="clsLabelAuto" Visible="false">Recommended By</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRecommendedBy" runat="server" CssClass="clsTextBoxTagSearch"
                                                        MaxLength="200" Text="<%# mRequisitionNew.RecommendedBy %>" ToolTip="Enter Recommended By Name"
                                                        Visible="false" Width="250px"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlReqItemAdd" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="Label2" class="clsLabelHeader">Item(s)<span>
                                                </td>
                                                <td align="right">
                                                    <table id="Table3" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" Visible="<%# mRequisitionNew.TransTypeID = 71 %>">
                                                                    <asp:ListItem Selected="True" Value="0">Part</asp:ListItem>
                                                                    <asp:ListItem Value="1">Re-Order Level Items</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <div class="dropdown">
                                                                    <asp:Button ID="btnAddCommon" CssClass="clsbtnH clsinfoH1" ClientIDMode="Static"
                                                                        runat="server" Text="Add &#9650;"></asp:Button>
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnCombo" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                                        Width="140px" Text="Add Item" ValidationGroup="1" ToolTip="Click to Add the Requisition Item">
                                                                                    </asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnSelectWONo" runat="server" CssClass="clsbtnH clsinfoH1" Width="140px"
                                                                                        Text="Select Work Order" Visible="<%# (mRequisitionNew.TransTypeID=65 or mRequisitionNew.TransTypeID=77) %>"
                                                                                        ClientIDMode="Static" ValidationGroup="1"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
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
                            <td>
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgRequisitionItems" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="true" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IsNewPart">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PartNo" HeaderText="Part No." HtmlEncode="false">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Req. Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"RequestedQty") %>' ClientIDMode="Static"
                                                            ToolTip="Enter Qty" MaxLength="4">
                                                        </asp:TextBox>
                                                        <asp:CustomValidator ID="cvBrokenRules" runat="server" ErrorMessage="Requested Qty Required."
                                                            ControlToValidate="txtQty" Display="None" OnServerValidate="CustomValidate1"
                                                            ValidationGroup="1"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Unit" HeaderText="Unit">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RegNo" HeaderText="Cost Center">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="WONo" HeaderText="WO No.">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReasonForRequest" HeaderText="Request Reason">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReasonForPurchase" HeaderText="Purchase Reason">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RequisitionItemTypeName" HeaderText="Main.Type">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>--%>
                                                <%--<asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>--%>
                                                <%--<asp:ButtonField Text="Part Status" HeaderText="Part Status" CommandName="ShowPartStatus">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>--%>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%-- <span id="button">Login</span>--%>
                                                        <div class="dropdown">
                                                            <div class="dropdownbtn-content">
                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImgEditView" runat="server" CommandName="EditRec" Style="height: 15px;
                                                                                width: 15px" ImageUrl="~/images/edit.png" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px;
                                                                                width: 20px" ImageUrl="~/images/delete.png" />
                                                                        </td>
                                                                        <%--<tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImgPartStatus" runat="server" CommandName="ShowPartStatus" Style="height: 20px;
                                                                                width: 20px" ImageUrl="~/images/s1.png"/>
                                                                        </td>--%>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowRight.png" runat="server" CssClass="clsActionbtn"
                                                                Style="cursor: pointer" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:ButtonField Text="Part Status" HeaderText="Part Status" CommandName="ShowPartStatus">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Text="Cancel"
                                                        ValidationGroup="1" ToolTip="Click to Cancel the Requisition" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send Mail"
                                                        ClientIDMode="Static" ToolTip="Click to Send Requisition by Mail"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH1" Text='<%# IIf(AppSettings("ClientCode") = "IND", "Authorize", "Submit") %>'
                                                        ToolTip="Click to Complete the Requisition" ValidationGroup="1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save"
                                                        ToolTip="Click to Save Requisition" ValidationGroup="1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print"
                                                        ToolTip="Click to Print Requisition" Enabled="<%# not mRequisitionNew.IsNew %>"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                        ToolTip="Click to go back to the previous page"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlInfoLabel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblIndicate" runat="server" Visible="<%# ((((mRequisitionNew.TransTypeID = 65 or mRequisitionNew.TransTypeID = 72) and mRequisitionNew.ReqTypeID=2) or mRequisitionNew.TransTypeID = 77) and (Not mRequisitionNew.IsNew)) %>"
                                            CssClass="clsLabelHeader">* : Indicates Part no. does not exist and need to be added in the Part Master.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnCommonPartList" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnWOList" ClientIDMode="Static" runat="server" Text="----"
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
    <!-- Send Mail Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySendMail" Text="Dummy Send Mail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupSendMail" HorizontalAlign="Center" Style="height: 100%;
        width: 100%; vertical-align: Center;">
        <iframe id="iPopupSendMail" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSendMail" runat="server" TargetControlID="btnDummySendMail"
        PopupControlID="pnlPopupSendMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSendMailStateComplete() {
            $("#btnDummySendMail").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#btnSendMail").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupSendMail").attr("src", "wfSendMail_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummySendMail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForSendMail() {
            var SendMailWindow = $find("<%=mdlPopupSendMail.ClientID %>");
            //close Send Mail popup window
            SendMailWindow.hide();
            $("#iPopupSendMail").attr("src", "JavaScript:''");
            //call Send Mail image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!-- End-->
    <!-- Re-Order Level Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCommonPartList" Text="Dummy Common Part List"
            ClientIDMode="Static" CausesValidation="False" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupCommonPartList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="iPopupCommonPartList" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCommonPartList" runat="server" TargetControlID="btnDummyCommonPartList"
        PopupControlID="pnlPopupCommonPartList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCommonPartListStateComplete() {
            $("#btnDummyCommonPartList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenToolsWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#iPopupCommonPartList").attr("src", "wfReOrderLevelItemsForRequisition_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCommonPartList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCommonPartList() {
            var CommonPartListWindow = $find("<%=mdlPopupCommonPartList.ClientID %>");
            //close Common Part List popup window
            CommonPartListWindow.hide();
            $("#iPopupCommonPartList").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnCommonPartList").click();
        }
    </script>
    <!-- End-->
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
                //                beforeSend: function (xhr, settings) {
                //                    $("[id$=processing]").dialog();
                //                },
                success: onSuccess,
                error: onError
            });

            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
    </script>
    <%--
    Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }
            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
            source.get_element().value = text;
            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtEmployee_Autocomplete") {
                textbox = document.getElementById('hdnEmpId');
            }
            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetEmpIdonChange() {
            var popup = $find("txtEmployee_Autocomplete");
            var complist = popup.get_completionList();
            var text = $("#txtEmployee").val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    var textbox = document.getElementById('hdnEmpId');
                    textbox.value = val.toString();
                    return;
                }
            }
            var textbox = document.getElementById('hdnEmpId');
            textbox.value = '';
            return;
        }                               
    </script>
    <!-- WO List Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyWOList" Text="Dummy WO List" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupWOList" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupWOList" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupWOList" runat="server" TargetControlID="btnDummyWOList"
        PopupControlID="pnlPopupWOList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameWOListStateComplete() {
            $("#btnDummyWOList").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        function OpenWOList() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#iPopupWOList").attr("src", "wfSelectListForNewRequisition_Ajax.aspx?Type=pup&OpenFrom=RequisitionDetailPage");

                if (!$.browser.msie) {
                    $("#btnDummyWOList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForWOList() {
            var WOListWindow = $find("<%=mdlPopupWOList.ClientID %>");
            //close WO List popup window
            WOListWindow.hide();
            $("#iPopupWOList").attr("src", "JavaScript:''");
            //call WO List image button
            $("#hdnimgBtnWOList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
