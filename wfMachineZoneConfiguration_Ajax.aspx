<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineZoneConfiguration_Ajax.aspx.vb" EnableEventValidation="false" Inherits="Flypal.wfMachineZoneConfiguration_Ajax" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<HTML>
<head runat="server">
    <title>Aircraft Zone Configuration List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
     <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout" class="formBGColor">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
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
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidation" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" />                                             
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name  Required."
                                                Display="None" ControlToValidate="txtName" ValidationGroup="a"></asp:RequiredFieldValidator>
                                              <%--  <asp:RequiredFieldValidator ID="rfvMaxWeight" runat="server" CssClass="clsLabelAuto" ErrorMessage="Weight  Required."
                                                Display="None" ControlToValidate="txtMaxWeight" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvReferenceArm" runat="server" CssClass="clsLabelAuto" ErrorMessage="Reference Arm Required."
                                                Display="None" ControlToValidate="txtReferenceArm" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvMoments" runat="server" CssClass="clsLabelAuto" ErrorMessage="Moments Required."
                                                Display="None" ControlToValidate="txtMoments" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlAircraftZoneConfigurationDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsAircraftZoneConfigurationDetails" class="clsFieldSet" style="border-width: 1px">
                                                <legend id="lblAircraftZoneConfigurationDetails" runat="server" style="font-weight: bold">
                                                    <b>Zone Configuration Details [NEW]</b></legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblName" class="clsLabelAuto">Name</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Certificate Name"
                                                                Width="250px" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        
                                                        <td>
                                                           <%-- <span id="Span1" class="clsLabelStar">*</span>--%>
                                                        </td>
                                                        <td>
                                                            <span id="lblMaxWeight" class="clsLabelAuto">Max. Weight</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMaxWeight" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Max. Weight"
                                                                Width="150px" MaxLength="50"></asp:TextBox>
                                                                &nbsp;
                                                                <span id="lblMaxWeightUnit" class="clsLabelAuto" style="font-size: x-small; font-weight: bold; font-style: italic">lbs</span>

                                                        </td>
                                                         
                                                    </tr>
                                                    <tr>
                                                        
                                                        <td>
                                                           <%-- <span id="Span2" class="clsLabelStar">*</span>--%>
                                                        </td>
                                                        <td>
                                                            <span id="lblReferenceArm" class="clsLabelAuto">Ref. ARM</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReferenceArm" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Ref. ARM"
                                                                Width="150px" MaxLength="50"></asp:TextBox>
                                                                &nbsp;
                                                                <span id="lblReferenceArmUnit" class="clsLabelAuto" 
                                                                style="font-size: x-small; font-weight: bold; font-style: italic">lbs</span>

                                                        </td>

                                                    </tr>
                                                     <tr>
                                                        
                                                        <td>
                                                            <%--<span id="Span3" class="clsLabelStar">*</span>--%>
                                                        </td>
                                                        <td>
                                                            <span id="lblMoments" class="clsLabelAuto">Moments</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMoments" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Moments"
                                                                Width="150px" MaxLength="50"></asp:TextBox>
                                                                &nbsp;
                                                                <span id="lblMomentsUnit" class="clsLabelAuto" style="font-size: x-small; font-weight: bold; font-style: italic">lbs</span>
                                                        </td>

                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" OnClientClick="return CheckValidation();"
                                                CssClass="clsButton_Ajax" ToolTip="Click to Add the Zone Configuration" ValidationGroup="a"
                                                Text="Add"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Zone Configuration Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="margin-left: 40px">
                                                        <asp:GridView ID="dgZoneConfigurationList" runat="server" ClientIDMode="Static" PageSize="25"
                                                            ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
                                                            CssClass="clsGrid" AllowPaging="True" AllowSorting="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Sr. No."/>
                                                                <asp:BoundField DataField="ZoneConfigurationName" HeaderText="Zone Name">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MaxWeight" HeaderText="Max Weight">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReferenceArm" HeaderText="Reference Arm">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Moments" HeaderText="Moments">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                 
                                                                <asp:ButtonField CommandName="EditRec" HeaderText="Edit/View" Text="Edit/View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                
                                                                
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
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlBack" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                TabIndex="0" Text="Back" ToolTip="Click to go Previous page" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
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

    </form>

     <script language="JavaScript" type="text/javascript">
         function CallParentFunction() {

             window.parent.autoResizeZoneConfigurationList();
         }
         function CallCloseChildPage() {

             window.parent.CloseChildPage();
         }
         function CheckValidation() {
             if (!Page_ClientValidate()) {
                 // Call Your custom JS function and return value.
                 CallParentFunction();
             }
         }
    </script>
     <script type="text/javascript" language="javascript">
         function onClientShown(sender, e) {
             window.parent.autoResizeCerti();
         }
         function onClientHide(sender, e) {
             window.parent.autoResizeCertificateList();
         }
         </script>
</body>
</html>
