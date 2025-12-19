<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfProgramType_Ajax.aspx.vb"
    Inherits="Flypal.wfProgramType_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Program Type</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Program Type [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="1"></asp:ValidationSummary>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required"
                                                        ValidationGroup="1" Display="None" ControlToValidate="txtProgramTypeName"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvName" runat="server" Display="None" ControlToValidate="txtProgramTypeName"
                                                        ValidationGroup="1" ErrorMessage="Program Type Name should not be greater than 250 Characters."
                                                        ClientValidationFunction="validateName"></asp:CustomValidator>
                                                    <script type="text/javascript">
                                                        function validateName(source, args) {
                                                            var Value = $get("txtProgramTypeName").value.length;
                                                            if (Value > 250) {
                                                                args.IsValid = false;
                                                                return
                                                            }

                                                        }
                                                    </script>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add the new Program Type"
                                                        Text="New" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblProgramTypeDetails" class="clsLabelHeader">Program Type Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblName1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblName" class="clsLabel">Name</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtProgramTypeName" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                    ToolTip="Enter Program Type's Name" Text="<%# mProgramType.Name %>" TextMode="MultiLine"
                                                                    MaxLength="250">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save the Program Type Information"
                                                        Text="Save" ValidationGroup="1"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Program Type List</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="dgProgramType" runat="server" CssClass="clsGrid" ToolTip="Program Type list"
                                                        DataKeyNames="ID" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="Name" HeaderText="Name">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="true" CssClass="TextBreak" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="View">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
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
                            <td colspan="4" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Program Type screen"
                                            Text="Close" CausesValidation="False"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
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
    <div>
        <!-- TankMaster Popup Window -->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForProgramTypeMaster();
                return false;
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameProgramTypeMasterStateComplete();
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
    </div>
    </form>
</body>
</html>
