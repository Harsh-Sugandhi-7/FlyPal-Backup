<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTrainingOrg_Ajax.aspx.vb"
    Inherits="Flypal.wfTrainingOrg_Ajax" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html> 
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Training Organization</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder> 
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox runat="server" ID="MSGBoxCntrl" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Training Organization [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnNew" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click to add new Training Organization."
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ValidationGroup="1"
                                                            ToolTip="Click to save the current record."></asp:Button>
                                                    </td>
                                                    <td valign="bottom" align="right">
                                                        <asp:Button ID="btnBack" TabIndex="0" CssClass="clsbtnH clsinfoH" runat="server" Text="Close"
                                                            ToolTip="Click to close Training Organization screen" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ControlToValidate="txtName" ErrorMessage="Training Org Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCity" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ControlToValidate="cmbCityList" ErrorMessage="Please select the City." ValidationGroup="1"
                                            ClientValidationFunction="validateCity"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ControlToValidate="txtName" ErrorMessage="Training Org. Name too long." ValidationGroup="1"
                                            ClientValidationFunction="validateName"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateCity(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbCityList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }

                                            function validateName(source, args) {
                                                args.IsValid = false;
                                                var length = $("#txtName").val().length;
                                                if (length <= 100) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTrainingOrgDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <%--<tr>
                                                <td>
                                                    <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnNew" runat="server" CssClass="clsButton_Ajax" Text="New" ToolTip="Click to add new Training Organization."
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td>
                                                    <table id="Table1" border="0" cellspacing="1" cellpadding="1" width="100%">
                                                        <tr>
                                                            <td colspan="5">
                                                                <span id="lblTrainingOrgDetails" class="clsLabelHeader">Training Org Details</span>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Label2" class="clsLabelStar" style="color: Red;">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblName" class="clsLabelAuto">Name</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.Name %>"
                                                                    ToolTip="Enter Training Org Name" MaxLength="100">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                                <span id="lblContactDetails" class="clsLabelHeader">Contact Details</span>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td width="106">
                                                                <span id="lblAddress1" class="clsLabelAuto">Building / Society</span>
                                                            </td>
                                                            <td style="width: 358px">
                                                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mTrainingOrg.Address1 %>"
                                                                    ToolTip="Enter Building / Society" MaxLength="150">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblPhone1" class="clsLabelAuto">Phone1</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPhone1" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.PhoneNo1 %>"
                                                                    ToolTip="Enter Phone 1" MaxLength="20">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td width="106">
                                                                <span id="lblAddress2" class="clsLabelAuto">Street Name </span>
                                                            </td>
                                                            <td style="width: 358px">
                                                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mTrainingOrg.Address2 %>"
                                                                    ToolTip="Enter Street name" MaxLength="150">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblPhone2" class="clsLabelAuto">Phone2</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPhone2" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.PhoneNo2 %>"
                                                                    ToolTip="Enter Phone 2" MaxLength="20">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td width="106">
                                                                <span id="lblAreaLandmark" class="clsLabelAuto">Area / Landmark</span>
                                                            </td>
                                                            <td style="width: 358px">
                                                                <asp:TextBox ID="txtAddress3" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mTrainingOrg.Address3 %>"
                                                                    ToolTip="Enter Area / Landmark name" MaxLength="150">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblFax" class="clsLabelAuto">Fax</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFax" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.Fax %>"
                                                                    ToolTip="Enter Fax" MaxLength="20">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Label1" class="clsLabelStar" style="color: Red;">*</span>
                                                            </td>
                                                            <td width="106">
                                                                <span id="lblCity" class="clsLabelAuto">City</span>
                                                            </td>
                                                            <td style="width: 358px">
                                                                <table id="Table6" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbCityList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                                DataValueField="ID" DataTextField="Name" SelectedValue="<%# mTrainingOrg.CityID %>">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <%--<asp:Button ID="imgCity" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                                ToolTip="Click to Add New City" CausesValidation="False"></asp:Button>--%>

                                                                            <asp:ImageButton ID="imgCity" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px" ToolTip="Click to Add New City"
                                                                                CausesValidation="False"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblEmail" class="clsLabelAuto">Email</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.Email %>"
                                                                    ToolTip="Enter Email" MaxLength="50">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td width="106">
                                                                <span id="lblState" class="clsLabelAuto">State</span>
                                                            </td>
                                                            <td style="width: 358px">
                                                                <asp:TextBox ID="txtState" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.StateName %>"
                                                                    ToolTip="State Name" MaxLength="25" BackColor="#E0E0E0" ReadOnly="True">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblWebSite" class="clsLabelAuto">Website</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.Website %>"
                                                                    ToolTip="Enter Website">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblCountry" class="clsLabelAuto">Country</span>
                                                            </td>
                                                            <td style="width: 358px">
                                                                <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTrainingOrg.CountryName %>"
                                                                    ToolTip="Country Name" MaxLength="25" BackColor="#E0E0E0" ReadOnly="True">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:LinkButton ID="lnkTrainingDetail" runat="server" CssClass="clsLinkButton" ToolTip="Click to add Training Detail"
                                                        CausesValidation="False">Training Detail</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td>
                                                    <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                                </td>
                                                <td align="right">
                                                    <table id="Table4">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" Text="Save" ValidationGroup="1"
                                                                    ToolTip="Click to save the current record."></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblSearch" class="clsLabelHeader">Search</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td style="width: 18px;">
                                                                <span id="K1" class="clsLabelAuto"></span>
                                                            </td>
                                                            <td style="width: 106px;">
                                                                <span id="lblSearchBy" style="width: 107px; height: 12px;" class="clsLabel">Search By</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbSearchType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Name</asp:ListItem>
                                                                    <asp:ListItem Value="2">City</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFor" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Place Name"
                                                                    MaxLength="50" Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table id="Table5">
                                                        <tr>
                                                            <td align="right">
                                                               <%-- <asp:Button ID="btnFindNow" CssClass="clsButton_Ajax" runat="server" Text="Find Now"
                                                                    ToolTip="Click to find the list as per the criteria." CausesValidation="False"> </asp:Button>--%>

                                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                        ToolTip="Click to find the list as per the criteria." CausesValidation="False"/>
                                                                
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
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgTrainingOrgList" runat="server" AllowSorting="True"
                                                        AutoGenerateColumns="False" AllowPaging="true" PageSize="25" EnableViewState="false"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="true">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Address" SortExpression="Address" HeaderText="Address">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CityName" SortExpression="CityName" HeaderText="City">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StateName" SortExpression="StateName" HeaderText="State">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CountryName" SortExpression="CountryName" HeaderText="Country">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PhoneNo1" SortExpression="PhoneNo1" HeaderText="Phone No1">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PhoneNo2" SortExpression="PhoneNo2" HeaderText="Phone No2">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Fax" SortExpression="Fax" HeaderText="Fax">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Website" SortExpression="Website" HeaderText="Website">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div id="dropDownImg" class="dropdown">
                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="editICN" Style="height: 15px; width: 15px" runat="server" 
                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            ToolTip="Click to Edit record" 
                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                            ToolTip="Click to Delete record"
                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                    </td>

                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
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
                        <!--Dummy panel to open modelpopup for city-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnCity" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
                                            Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                        <tr>
                           <%-- <td align="right">
                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2">
                                            <tr>
                                                <td valign="bottom" align="right">
                                                    <asp:Button ID="btnBack" TabIndex="0" CssClass="clsButton_Ajax" runat="server" Text="Close"
                                                        ToolTip="Click to close Training Organization screen" CausesValidation="False">
                                                    </asp:Button>
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
    <div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        <!-- Training Detail --ModalPopUp -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTrainingDetail" Text="Dummy Training Detail" />
        </div>
        <asp:Panel runat="server" ID="Panl1" Style="display: none">
            <div>
                <table class="clstablelistout" id="Table8">
                    <tr>
                        <td colspan="1">
                            <asp:UpdatePanel ID="upnlTrainingDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="clstablelistin" id="Table9">
                                        <tr>
                                            <td colspan="4" class="clsFormHeader1Newstyle">
                                                <span id="lbltitleTrainingDet" class="clsFormHeader">Training Detail</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary">
                                                </asp:ValidationSummary>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <span id="lblTrainingDetails" class="clsLabelHeader">Training Detail </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblTrainingName" class="clsLabelAuto">Training Org.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTrainingOrgName" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"
                                                    MaxLength="5" ToolTip="Training Organization"></asp:TextBox>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td colspan="3">
                                                <span id="Label4" class="clsLabelAuto">Click To Save Current Record</span>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnSaveTrainingDet" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to Save Training Detail"
                                                    Text="Save"></asp:Button>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td colspan="4">
                                                <span id="lblResultTrainingDet" class="clsLabelHeader"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <%--<div style="width: 400px;">
                                                    <table class="clsGrid" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
                                                        width: 400px;">
                                                        <tr>
                                                            <td style="width: 40px" class="clsdgHeader">
                                                                <span>Select</span>
                                                            </td>
                                                            <td style="width: 360px;" class="clsdgHeader">
                                                                <span class="clsdgHeader">Training</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>--%>
                                                <div style="max-height: 150px; overflow-y: auto; overflow-x: hidden; width: 418px;">
                                                    <asp:GridView ID="dgTrainingDetailList" runat="server"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
                                                        ShowHeader="true" ShowHeaderWhenEmpty="true" Style="width: 400px;" AllowPaging="true"
                                                        PageSize="10">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkTrainingOrg" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" HorizontalAlign="left" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Name" HeaderText="Training">
                                                                <ItemStyle HorizontalAlign="Left" Width="360px" Wrap="true" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="right" valign="bottom">
                                                <table id="Table14" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="btnSaveTrainingDet" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Training Detail"
                                                                Text="Save"></asp:Button>
                                                        </td>
                                                        <td>&nbsp
                                                        </td>
                                                        <td valign="bottom" align="right">
                                                            <asp:Button ID="btnBackTrainingDet" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close Training Detail screen"
                                                                Text="Close" CausesValidation="False"></asp:Button>
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
                </table>
            </div>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="lnkTrainingDetail_ModalPopupExtender" runat="server"
            TargetControlID="btnDummyTrainingDetail" PopupControlID="Panl1" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
    </div>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForTrainingOrgMaster();
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
                parent.IFrameTrainingOrgMasterStateComplete();
            }
       
      
    });
        <% End if %>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            SetPageLayout();
        }

        function SetPageLayout()
        {
        <% Dim mopenas As String = Request.QueryString("Type") %>
            <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
            ReSetPageLayout();
            onResize();//for Top bottom link
            <% End if %>
        }
        function ReSetPageLayout()
        {
        $("body,html").css({ 'background-color': 'transparent' });
            var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
            var windowheight=$(window).height();
            if (tempMargtop>=windowheight)
            {
            $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
            }
            else
            {
            var margintop=(windowheight/2)-(tempMargtop/2);
            $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
            }
       
        }
    </script>
    <%--End--%>
    <!-- City Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCity" Text="Dummy City" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupCity" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupCity" frameborder="0" allowtransparency="true" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCity" runat="server" TargetControlID="btnDummyCity"
        PopupControlID="pnlPopupCity" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCityStateComplete() {
            $("#btnDummyCity").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#imgCity").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupCity").attr("src", "wfCityInv_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyCity").click();
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
        function ParentCallBackFunction() {
            var CityWindow = $find("<%=mdlPopupCity.ClientID %>");
            //close City popup window
            CityWindow.hide();
            $("#iPopupCity").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnCity").click();
        }
    </script>
    <!-- End-->
    <%-- hide validation summary when server event occurs--%>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
            //Page_ClientValidate();
            // ValidationSummaryOnSubmit();
            //Page_IsValid=true;
            //            Page_ClientValidate();
            //            if (Page_IsValid) {
            //                $("#ValidationSummary1").css('display', 'none');
            //            }

            if ((typeof (Page_ClientValidate) == 'function')) {
                if (Page_ValidationActive) {
                    if (!ValidatorCommonOnSubmit()) {
                        return false;
                    }
                    else {
                        $(".clsValidationSummary").css('display', 'none');
                        //ValidationSummaryOnSubmit();

                    }
                }
            }
        });
    </script>
    <%-- End--%>
    </form>
</body>
</html>
