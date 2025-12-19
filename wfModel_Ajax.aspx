<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfModel_Ajax.aspx.vb"
    Inherits="Flypal.wfModel_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Model Information</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <asp:UpdatePanel ID="upnlModel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="clstablelistin" id="tblInner">
                                        <tr>
                                            <td colspan="2" class="clsFormHeader1Newstyle clsFormHeaderTD">

                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Model Information [New]</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAdd" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="New" ToolTip="Click to add new Model" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" ValidationGroup="1" runat="server"
                                                                            Text="Save" ToolTip="Click to save the Model Information"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" ValidationGroup="1" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Close" ToolTip="Click to close Model Information screen" CausesValidation="False"></asp:Button>
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
                                                <asp:ValidationSummary ID="Validationsummary1" ValidationGroup="1" runat="server"
                                                    HeaderText="Fill Up The Following Fields" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:CustomValidator ValidationGroup="1" ID="cvPrimaryModel" runat="server" ErrorMessage="Select Primary Model from the list."
                                                    ControlToValidate="cmbPrimaryModelList" Display="None"
                                                    ClientValidationFunction="ValidatePrimaryModelList" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                <asp:CustomValidator ValidationGroup="1" ID="cvAssemblyTypeId" runat="server" ErrorMessage="Select Manufacturer from the list."
                                                    ControlToValidate="cmbManufacturerList" Display="None"
                                                    ClientValidationFunction="ValidateManufacturerList" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="1" CssClass="clsLabelAuto"
                                                    ErrorMessage="Name Required" ControlToValidate="txtName" Display="None"></asp:RequiredFieldValidator>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2">
                                                <fieldset id="fdsModelInfo" class="clsFieldSet" style="border-width: 1px">
                                                    <legend id="lblModel" style="font-weight: bold"><b>Model Details </b></legend>
                                                    <table>

                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="lblStarManufacturer" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblManufacturer" runat="server" CssClass="clsLabel">Manufacturer</asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <table cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbManufacturerList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                DataValueField="ID" DataTextField="Name" SelectedValue="<%# mModel.ManufacturerID %>">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imgbtnManufacturer" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Model" CausesValidation="False"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="lblModelNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Name</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mModel.Name %>"
                                                                    ToolTip="Enter Name" MaxLength="25" Width="179px"></asp:TextBox>
                                                            </td>
                                                            <td align="right"></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblForAssembly" runat="server" CssClass="clsLabel">For Assembly</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbForAssemblyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    DataValueField="ID" DataTextField="Name" SelectedValue="<%# mModel.AssemblyTypeID %>" AutoPostBack ="true"
                                                                    Enabled='<%# CType(Session("Type"), Boolean) %>'>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="right"></td>
                                                        </tr>
                                                        <asp:PlaceHolder runat="server" ID="PrimaryModelPlaceHolder">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblStarPrimaryModel" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPrimaryModel" runat="server" CssClass="clsLabel">Primary Model</asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <table cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbPrimaryModelList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    DataValueField="ID" DataTextField="Name" SelectedValue="<%# mModel.PrimaryModelID %>">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnPrimaryModel" runat="server" ImageUrl="~/images/plus1.png"
                                                                                    Height="22px" Width="24px" ToolTip="Click to Add New record" CausesValidation="False"></asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </asp:PlaceHolder>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbFixedWing" CssClass="clsRadioButton" runat="server" Text="Fixed Wing" Checked="true"
                                                                    GroupName="a" Visible="false" />
                                                                <asp:RadioButton ID="rdbRotaryWing" CssClass="clsRadioButton" runat="server" Text="Rotary Wing"
                                                                    GroupName="a" Visible="false" />
                                                            </td>
                                                            <td align="right"></td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto" Visible="False">Click To Save Current Record</asp:Label>
                                            </td>
                                            <td align="right"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Model List</asp:Label>
                                            </td>
                                            <td align="right"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="dgModel" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                    DataKeyNames="ID" EnableViewState="True" ForeColor="Black" GridLines="Horizontal" PageSize="10" ShowHeaderWhenEmpty="true">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                        NextPageText="" PreviousPageText="" />
                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="False"></asp:BoundField>
                                                        <asp:BoundField DataField="PrimaryModelName" HeaderText="Primary Model" SortExpression="PrimaryModelName">
                                                            <HeaderStyle ForeColor="black" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ManufacturerName" HeaderText="Manufacturer" SortExpression="ManufacturerName">
                                                            <HeaderStyle ForeColor="black" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                            <HeaderStyle ForeColor="black" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyTypeName" HeaderText="Assembly" SortExpression="AssemblyTypeName">
                                                            <HeaderStyle ForeColor="black" HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <%-- <asp:ButtonField HeaderText="Edit/View" CommandName="EditRec" Text="Edit/View">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        </asp:ButtonField>--%>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>--%>
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
                                        <tr>
                                            <td colspan="2" align="right"></td>
                                        </tr>
                                    </table>
                                    <asp:Button ID="htnBtnManufacturer" ValidationGroup="1" ClientIDMode="Static" runat="server"
                                        Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
                                    <asp:Button ID="htnBtnPrimaryModel" ValidationGroup="1" ClientIDMode="Static" runat="server"
                                        Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
        </div>
        <!-- Select Manufacturer popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyManufacturer" Text="TaskCard Tool" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlManufacturer" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeManufacturer" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupManufacturer" runat="server" TargetControlID="btnDummyManufacturer"
            PopupControlID="pnlManufacturer" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameManufacturerStateComplete() {
                $("#btnDummyManufacturer").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenManufacturerWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeManufacturer").attr("src", "wfManufacturer_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyManufacturer").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForManufacturer() {
                var Manufacturerwindow = $find("<%=mdlPopupManufacturer.ClientID %>");
                //close Task Card Tool popup window
                Manufacturerwindow.hide();
                //           release resources
                $("#IframeManufacturer").attr("src", "JavaScript:''");
                //call image button
                $("#htnBtnManufacturer").click();
            }
        </script>
        <!-- End-->
        <!-- Select PrimaryModel popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyPrimaryModel" Text="TaskCard Tool" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPrimaryModel" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframePrimaryModel" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupPrimaryModel" runat="server" TargetControlID="btnDummyPrimaryModel"
            PopupControlID="pnlPrimaryModel" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFramePrimaryModelStateComplete() {
                $("#btnDummyPrimaryModel").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenPrimaryModelWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframePrimaryModel").attr("src", "wfPrimaryModel_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyPrimaryModel").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForPrimaryModel() {
                var PrimaryModelwindow = $find("<%=mdlPopupPrimaryModel.ClientID %>");
                //close Task Card Tool popup window
                PrimaryModelwindow.hide();
                //           release resources
                $("#IframePrimaryModel").attr("src", "JavaScript:''");
                //call image button
                $("#htnBtnPrimaryModel").click();
            }
        </script>
        <!-- End-->
    </form>
    <script type="text/javascript">

        function ValidateManufacturerList(source, args) {
            args.IsValid = false;
            var dd = $get("cmbManufacturerList");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }

        function ValidatePrimaryModelList(source, args) {
            args.IsValid = false;
            if ((<%# mModel.AssemblyTypeID %>  == 1)) && (<%# mCompanyDetail.IsSyncApplication %>  == True) {
                var dd = $get("cmbPrimaryModelList");
                if (dd.selectedIndex != 0) {
                    args.IsValid = true;
                    return;
                }
            }

        }

    </script>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("OpenAs") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
            SetPageLayout();
            if ($.browser.msie) {
                parent.IFrameModelStateComplete();
            }
        });

    <% End if %>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            SetPageLayout();
        }

        function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("OpenAs") %>
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
        function CallParentCallback() {
            parent.ParentCallBackFunctionForModel();
            return false;
        }
    </script>
    <%--End--%>
</body>
</html>
