<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfStoreLocation_Ajax.aspx.vb"
    Inherits="Flypal.wfStoreLocation_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Store Location </title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <style type="text/css">
        .hideGridColumn
        {     
            display: none;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <asp:Panel runat="server" ID="pnlAddStoreLocation">
            <div>
                <table id="Table1" class="clstablelistout">
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" CssClass="clspanel1" runat="server">
                                <asp:UpdatePanel runat="server" ID="upnlStoreLocationValidation" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="tabStoreLocationValidation" width="100%">
                                            <tr>
                                                <td colspan="2" class="clsFormHeader1Newstyle">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblStoreLocationTitle" CssClass="clsFormHeader" runat="server">Location [New]</asp:Label>
                                                            </td>

                                                            <td align="right">
                                                                <table id="Table4">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnStoreLocationNew" CssClass="clsbtnH clsinfoH" runat="server" CausesValidation="False"
                                                                                ToolTip="Click to Add New Location" Text="New"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnStoreLocationSaveBottom" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to save Location Information"
                                                                                Text="Save"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnStoreLocationCloseBottom" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                                CausesValidation="False" ToolTip="Click to close Location screen" Text="Close"></asp:Button>
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
                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="a"></asp:ValidationSummary>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required ."
                                                        ControlToValidate="txtStationName" Display="None" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" CssClass="clsLabelAuto"
                                                        ErrorMessage="Address Required ." ControlToValidate="txtAddress" Display="None"
                                                        ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvCity" runat="server" ErrorMessage="Select City from the List"
                                                        ControlToValidate="cmbCity" Display="None" OnServerValidate="customvalidate"
                                                        CssClass="clsLabelAuto" ValidationGroup="a"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cvAddress" runat="server" ControlToValidate="txtAddress"
                                                        Display="None" OnServerValidate="customvalidate" CssClass="clsLabelAuto" ValidationGroup="a"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtStationName"
                                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Select City from the List"
                                                        OnServerValidate="customvalidate" ValidationGroup="a"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td align="left">
                                                    <span id="spnCityInvAdd" class="clsLabelAuto">Click To Add New Record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnStoreLocationNew" CssClass="clsbtnH clsinfoH" runat="server" CausesValidation="False"
                                                        ToolTip="Click to Add New Location" Text="New"></asp:Button>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel runat="server" ID="upnlSotreLocationInformation" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                            <table id="tabSotreLocationInformation" width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <span id="lblNameStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="spanName" class="clsLabelAuto">Name </span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtStationName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Name"
                                                            Text="<%# mStoreLocation.Name %>" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <span id="spnAddressStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="spnAddress" class="clsLabelAuto">Address</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle"
                                                            ToolTip="Enter Address" Text="<%# mStoreLocation.Address %>" MaxLength="500"
                                                            TextMode="MultiLine">

                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <span id="spnStarCity" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="spnCity" class="clsLabelAuto">City</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlCity" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table3" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbCity" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                                DataTextField="Name" AutoPostBack="True" SelectedValue="<%# mStoreLocation.CityID %>">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imgbtnCity" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                Width="24px" ToolTip="Click to Add New City" CausesValidation="True" />
                                                                        </td>
                                                                        <td>
                                                                            &nbsp; &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtState" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                AutoCompleteType="HomeState" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="spnPhone1" class="clsLabelAuto">Phone1</span>
                                                    </td>
                                                    <td>
                                                        <table id="Table2" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtPhone1" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Phone1"
                                                                        Text="<%# mStoreLocation.Phone1 %>" MaxLength="25">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td width="96px">
                                                                    <span id="spnPhone2" class="clsLabel">Phone2</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPhone2" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Phone2"
                                                                        Text="<%# mStoreLocation.Phone2 %>" MaxLength="25">

                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="spnPhone3" class="clsLabelAuto">Phone3</span>
                                                    </td>
                                                    <td>
                                                        <table id="Table5" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtPhone3" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Phone3"
                                                                        Text="<%# mStoreLocation.Phone3 %>" MaxLength="25">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td width="96px">
                                                                    <span id="spnFax" class="clsLabel">Fax</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Fax"
                                                                        Text="<%# mStoreLocation.Fax %>" MaxLength="25">
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
                                                        <span id="spnEmail" class="clsLabelAuto">Email</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Email"
                                                            Text="<%# mStoreLocation.Email %>" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="spnContactPerson" class="clsLabelAuto">Contact Person</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Name of Contact Person"
                                                            Text="<%# mStoreLocation.ContactPerson %>" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel runat="server" ID="upnlSotoreLocationGridView" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="tabSotoreLocationGridView">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStoreLocationResult" runat="server" CssClass="clsLabelHeader">Station Details List</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgLocation" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                         EnableViewState="true" PagerSettings-Mode="NumericFirstLast"
                                                        PageSize="5" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
                                                                <HeaderStyle />
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Station">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Address" SortExpression="Address" HeaderText="Address">
                                                                <HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CityName" SortExpression="CityName" HeaderText="City">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StateName" SortExpression="StateName" HeaderText="State">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CountryName" SortExpression="CountryName" HeaderText="Country">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Phone1" SortExpression="Phone1" HeaderText="Phone1">
                                                                <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Phone2" SortExpression="Phone2" HeaderText="Phone2">
                                                                <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Phone3" SortExpression="Phone3" HeaderText="Phone3">
                                                                <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Fax" SortExpression="Fax" HeaderText="Fax">
                                                                <HeaderStyle></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ContactPerson" SortExpression="ContactPerson" HeaderText="Contact Person">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                        CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                        Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>

                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="EditView" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
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
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td align="right">
                                                    <table id="Table4">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnStoreLocationSaveBottom" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to save Location Information"
                                                                    Text="Save"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnStoreLocationCloseBottom" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    CausesValidation="False" ToolTip="Click to close Location screen" Text="Close">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>--%>
                                            </tr>
                                            <!--Dummy panel to open modelpopup-->
                                            <tr style="height: 0px;">
                                                <td style="height: 0px;">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                        <ContentTemplate>
                                                            <asp:Button ID="hdnimgBtnCity" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                Style="display: none;"></asp:Button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
            </div>
        </asp:Panel>
    </div>
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
            $("#imgbtnCity").live("click", function () {
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
