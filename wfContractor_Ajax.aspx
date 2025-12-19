<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfContractor_Ajax.aspx.vb"
    Inherits="Flypal.wfContractor_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Contractor</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Contractor Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnNew" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click to Add the new Contractor"
                                                                    CausesValidation="False"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Contractor Information" ValidationGroup="1"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Contractor Information screen"
                                                                    CausesValidation="False"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary" ValidationGroup="1">
                                </asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" CssClass="clsLabelAuto" ErrorMessage="Code Required."
                                    Display="None" ControlToValidate="txtCode" ValidationGroup="1">Code Required</asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required."
                                    Display="None" ControlToValidate="txtName" ValidationGroup="1">Name Required</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCode" runat="server" CssClass="clsLabelAuto" ErrorMessage="Contractor Code too Long."
                                    Display="None" ControlToValidate="txtCode" ClientValidationFunction="validateNameLength" ValidationGroup="1"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Contractor Name too Long."
                                    Display="None" ControlToValidate="txtName" ClientValidationFunction="validateNameLength" ValidationGroup="1"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvCity" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please select the City."
                                    Display="None" ControlToValidate="cmbCityInvList" ClientValidationFunction="validateCity" ValidationGroup="1"></asp:CustomValidator>
                                <script type="text/javascript">
                                    function validateCity(source, args) {
                                        args.IsValid = false;

                                        var dd = $get("cmbCityInvList");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }

                                    function validateNameLength(source, args) {
                                        //args.IsValid = false;
                                        var ControlName = source.controltovalidate;
                                        switch (ControlName) {
                                            case 'txtName':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 50) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtCode':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 10) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <%--<<td>
                                <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                            </td>
                            td align="right">
                                <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnNew" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click to Add the new Contractor"
                                            CausesValidation="False"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlContractorDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="5">
                                                    <span id="lblDocumentDetails" class="clsLabelHeader">Contractor Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px">
                                                    <span id="Label5" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblCode" class="clsLabelAuto">Code</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mContractor.Code %>"
                                                        ToolTip="Enter Code" MaxLength="10">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px;">
                                                    <span id="Label6" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblName" class="clsLabelAuto">Name</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.Name %>"
                                                        ToolTip="Enter Contracter name" MaxLength="50">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <span id="Label2" class="clsLabelHeader">Contact Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px">
                                                </td>
                                                <td>
                                                    <span id="lblAddress1" class="clsLabelAuto">Building/Society</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.Address1 %>"
                                                        ToolTip="Enter Building/Society name" MaxLength="250">
                                                    </asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <span id="lblPhone1" class="clsLabelAuto">Phone1</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPhone1" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.PhoneNo1 %>"
                                                        ToolTip="Enter Phone 1" MaxLength="20">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px">
                                                </td>
                                                <td>
                                                    <span id="lblAddress2" class="clsLabelAuto">Street Name</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.Address2 %>"
                                                        ToolTip="Enter Street name" MaxLength="250">
                                                    </asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <span id="Label1" class="clsLabelAuto">Phone2</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPhone2" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.PhoneNo2 %>"
                                                        ToolTip="Enter Phone 2" MaxLength="20">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px">
                                                </td>
                                                <td>
                                                    <span id="lblAddress3" class="clsLabelAuto">Area/Landmark</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAddress3" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.Address3 %>"
                                                        ToolTip="Enter your area name" MaxLength="250">
                                                    </asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <span id="lblFax" class="clsLabelAuto">Fax</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.Fax %>"
                                                        ToolTip="Enter Fax" MaxLength="20">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px">
                                                    <span id="Label7" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblCity" class="clsLabelAuto">City</span>
                                                </td>
                                                <td>
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCityInvList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    SelectedValue="<%# mContractor.CityID %>" AutoPostBack="True" DataValueField="ID"
                                                                    DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td> 
                                                                <%--<asp:Button ID="btnCityInvList" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to Add New City" CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="btnCityInvList" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                             Width="24px" ToolTip="Click to Add New City" CausesValidation="True"></asp:ImageButton>
                                                            </td>   
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left">
                                                    <span id="lblEmail" class="clsLabelAuto">Email</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.Email %>"
                                                        ToolTip="Enter Email" MaxLength="50">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px">
                                                </td>
                                                <td>
                                                    <span id="lblState" class="clsLabelAuto">State</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtState" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mContractor.StateName %>"
                                                        ToolTip="State Name" MaxLength="25" BackColor="#E0E0E0" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <span id="lblWebsite" class="clsLabelAuto">Website</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mContractor.Website %>"
                                                        ToolTip="Enter website" MaxLength="50">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 14px">
                                                </td>
                                                <td width="90px">
                                                    <span id="Label8" class="clsLabelAuto">Country</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mContractor.CountryName %>"
                                                        ToolTip="Country Name" MaxLength="25" BackColor="#E0E0E0" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                           <%-- <td>
                                <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Contractor Information" ValidationGroup="1">
                                        </asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Label3" class="clsLabelHeader">Search</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td width="14px">
                                                </td>
                                                <td width="90px">
                                                    <span id="Label4" class="clsLabelAuto">Search</span>
                                                </td>
                                                <td>
                                                    <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbSearchType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Code</asp:ListItem>
                                                                    <asp:ListItem Value="2">Name</asp:ListItem>
                                                                    <asp:ListItem Value="3">City</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 23px">
                                                                <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFor" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50" Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" Text="Find Now"
                                            ToolTip="Click to find the list as per the criteria." CausesValidation="False">
                                        </asp:Button>--%>

                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                            ToolTip="Click to find list as per searching criteria" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgContractor" runat="server"  AutoGenerateColumns="False"
                                                        AllowSorting="True" AllowPaging="true" PageSize="5" ShowHeaderWhenEmpty="true"
                                                        DataKeyNames="ID" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Address" SortExpression="Address" HeaderText="Address">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CityName" SortExpression="CityName" HeaderText="City">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StateName" SortExpression="StateName" HeaderText="State">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CountryName" SortExpression="CountryName" HeaderText="Country">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PhoneNo1" SortExpression="PhoneNo1" HeaderText="Phone1">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PhoneNo2" SortExpression="PhoneNo2" HeaderText="Phone2">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Fax" SortExpression="Fax" HeaderText="Fax">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
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
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
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
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Contractor Information screen"
                                            CausesValidation="False"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
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
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForContractor();
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
                parent.IFrameContractorStateComplete();
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
            $("#btnCityInvList").live("click", function () {
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
   
    </form>
</body>
</html>
