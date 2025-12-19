<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMSPAssembly_Ajax.aspx.vb" Inherits="Flypal.wfMSPAssembly_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSP Assembly</title>
     <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
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
                <table id="tblInner" class="clstablelistin">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="clsFormHeader1">
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> Applicable To</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    ValidationGroup="a"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvAssembly" runat="server" ClientValidationFunction="validateName"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbAssemblyList" ErrorMessage="Assembly Required."></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvNameLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Remark should be less than or equal to 500 charecters."
                                                    Display="None" ControlToValidate="txtRemark" ClientValidationFunction="validateName"
                                                    ValidationGroup="a"></asp:CustomValidator>
                                                
                                                <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"></asp:CustomValidator>
                                                <script type="text/javascript">
                                                    function validateName(source, args) {
                                                        var ControlName = source.controltovalidate;
                                                        switch (ControlName) {
                                                            case 'txtRemark':
                                                                var Value = $get(ControlName).value.length;
                                                                if (Value > 500) {
                                                                    args.IsValid = false;
                                                                    return
                                                                }
                                                                break;
                                                            case 'cmbAssemblyList':
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
                                        <span id="lblMSPAssemblyValue" class="clsLabelHeader">Applicable To</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlMSPAssemblyValue" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblAssemblyStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblAssembly" class="clsLabelAuto">Assembly</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAssemblyList" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                SelectedValue="<%# mMSP.MSPAssemblys.CurrentItem.AssemblyID %>"
                                                                DataValueField="ID" DataTextField="ModelSerialNo" >
                                                            </asp:DropDownList>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            
                                                        </td>
                                                        <td>
                                                            <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" TextMode="MultiLine"  Text="<%# mMSP.MSPAssemblys.CurrentItem.Remark %>"
                                                                ToolTip="Enter Remark" MaxLength="100" Width="208px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH1" Text="OK" ValidationGroup="a"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--Dummy panel to open modelpopup-->
                                <tr style="height: 0px;">
                                    <td style="height: 0px;">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnBtnMSPAssemblyValue" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--End -->
                            </table>
                        </td>
                    </tr>
                </table>
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
        <!--call parent function after completing subroutine..(when page open as popup)-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForMSPAssembly();
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
                parent.IFrameMSPAssemblyStateComplete();
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
        function ReSetPageLayout() {
            $("body,html").css({ 'background-color': 'transparent' });
            var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
            var windowheight = $(window).height();
            if (tempMargtop >= windowheight) {
                $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
            }
            else {
                var margintop = (windowheight / 2) - (tempMargtop / 2);
                $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
            }

        }
    </script>
    <%--End--%>
    </form>
</body>
</html>
