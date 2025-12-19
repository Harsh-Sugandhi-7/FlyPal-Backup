<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHangarAircraftMaster.aspx.vb"
    Inherits="Flypal.wfHangarAircraftMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="wfgroup" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
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
                        <asp:UpdatePanel ID="upnlAircraftDetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Aircraft [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="Aircraft" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Aircraft Required" Display="None" ControlToValidate="txtAircraft"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ValidationGroup="a" ID="cvModel" runat="server" ErrorMessage="Select Model from the list."
                                                ControlToValidate="CmbModel" Display="None" ClientValidationFunction="ValidateModelList"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Serial No Required" Display="None" ControlToValidate="TxtSerialNo"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ValidationGroup="a" ID="cvAircraftTypeID" runat="server" ErrorMessage="Select Customer from the list."
                                                ControlToValidate="cmbCustomer" Display="None" ClientValidationFunction="ValidateAircraftList"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to add new Aircraft in the list"
                                                Text="New" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblManufacturerDetails" runat="server" CssClass="clsLabelHeader1">Aircraft Details . . .</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="spGMT1" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="spName" class="clsLabelAuto">Aircraft Reg.No </span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAircraft" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Aircraft Name"
                                                Text="<%# mAirCraftMaster.Haircraft %>" MaxLength="20">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Span3" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="Span4" class="clsLabelAuto">Model</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CmbModel" runat="server" CssClass="clsComboBox_Ajax" DataTextField="ModelName"
                                                SelectedValue="<%# mAirCraftMaster.ModelID %>" DataValueField="ID">
                                            </asp:DropDownList>
                                            
                                            <asp:ImageButton ID="imgbtnModel" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                Width="24px" ToolTip="Click to Add New Model" CausesValidation="False"></asp:ImageButton>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Span1" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="Span2" class="clsLabelAuto">Serial No</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Serial No. Name"
                                                Text="<%# mAirCraftMaster.SerialNo %>" MaxLength="20">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="spName1" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="spGMT" class="clsLabelAuto">Customer</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbCustomer" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                DataValueField="ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnSave" ValidationGroup="a" runat="server" CssClass="clsButton_Ajax"
                                                ToolTip="Click to save the Aircraft Information" Text="Save"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="dgAirCraft" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                AllowSorting="true" CssClass="clsGrid" PageSize="10" ShowHeaderWhenEmpty="True">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" Visible="false" HeaderText="ID" />
                                                    <asp:BoundField DataField="Haircraft" HeaderText="Aircraft" SortExpression="Haircraft">
                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HVendorName" HeaderText="Customer" SortExpression="HVendorName">
                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SerialNo" HeaderText="SerialNo" SortExpression="SerialNo">
                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="DeleteRec"
                                                                Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton"
                                                Text="Close" ToolTip="Click to close Aircraft screen" />
                                        </td>
                                    </tr>
                                    <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnModel" runat="server" CausesValidation="False" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>
    <script type="text/javascript">


        function ValidateAircraftList(source, args) {
            args.IsValid = false;
            var dd = $get("cmbCustomer");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
        function ValidateModelList(source, args) {
            args.IsValid = false;
            var dd = $get("CmbModel");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
    </script>
    
<!-- Select Model popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyModel" Text="TaskCard Tool" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlModel" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeModel" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupModel" runat="server" TargetControlID="btnDummyModel"
        PopupControlID="pnlModel" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameModelStateComplete() {
            $("#btnDummyModel").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenModelWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeModel").attr("src", "wfModel_Ajax.aspx?OpenAs=pup");

                if (!$.browser.msie) {
                    $("#btnDummyModel").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForModel() {
            var Modelwindow = $find("<%=mdlPopupModel.ClientID %>");
            //close Task Card Tool popup window
            Modelwindow.hide();
            //           release resources
            $("#IframeModel").attr("src", "JavaScript:''");
            //call image button

            $("#hdnBtnModel").click();
        }
    </script>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForAircraftMaster();
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
             parent.IFrameAircraftMasterStateComplete();
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       }
    </script>
    <%--End--%>
    </form>
</body>
</html>
