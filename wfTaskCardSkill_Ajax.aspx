<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskCardSkill_Ajax.aspx.vb" Inherits="Flypal.wfTaskCardSkill_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <title>Skill Details</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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

                                  <td class="clsFormHeader1">
                                <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Task Card Skill Information</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Select Skill Information"
                                                                        Text="Ok" ValidationGroup="valGroup1"></asp:Button>                      
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                        Text="Back" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" Width="440px" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvSkill" runat="server" CssClass="clsLabelAuto" ClientValidationFunction="validateSkill"
                                                Display="None" ValidationGroup="valGroup1" ErrorMessage="Please Select the Skill."></asp:CustomValidator>
                                            <!-- Client side validation for comboboxes ControlToValidate="cmbSkillList" -->

                                            <%-- Commented on 18-Aug-2016 by shital
                                       <script type="text/javascript">
                                            //Nomenclature
                                            function validateSkill(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbSkillList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;

                                                }
                                            }
                                        </script>--%>

                                            <%--  'Added by Shital on 18-Aug-2016--%>
                                            <script type="text/javascript">
                                                function validateSkill(source, args) {
                                                    var chkListModules = document.getElementById('<%= chkSkillList.ClientID %>');
                                                    var chkListinputs = chkListModules.getElementsByTagName("input");
                                                    for (var i = 0; i < chkListinputs.length; i++) {
                                                        if (chkListinputs[i].checked) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }
                                                    args.IsValid = false;
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSkillDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdswodetail" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                                <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Task Card Skill Details</b></legend>

                                                <table>
                                                    <%--<tr>
                                                <td colspan="3">
                                                    <span id="lblSkillDetails" class="clsLabelHeader">Task Card Skill Details</span>
                                                </td>
                                            </tr>--%>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblWO" runat="server" CssClass="clsLabelAuto">Task Card No.</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtTaskCardNo" runat="server" CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0"
                                                                MaxLength="150" ReadOnly="True" Width="320px" ToolTip="Task Card No."></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblSkill" class="clsLabelAuto">Skill</span>
                                                        </td>
                                                        <td>
                                                            <table id="Table6" cellspacing="0" cellpadding="0" border="0">
                                                                <tr>

                                                                    <td>
                                                                        <asp:CheckBoxList ID="chkSkillList" CssClass="clsComboBox2_Ajax" runat="server" RepeatColumns="2" DataValueField="ID" DataTextField="CodeWithName">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <%-- <asp:Button ID="imgSkill" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Skill"
                                                                    Text="Add Skill" Width="70" CausesValidation="False"></asp:Button>--%>
                                                                        <asp:ImageButton ID="imgSkill" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                            Width="24px" ToolTip="Click to Add New Skill" CausesValidation="False"></asp:ImageButton>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right">
                                    <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save Skill Information"
                                                            Text="Ok" ValidationGroup="valGroup1"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                            Text="Back" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnSkill" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForTaskCardSkill();
                return false;
            }
        </script>
        <div>
            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameTaskCardSkillStateComplete();
                    }


                });
        <% End if %>
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
                function endRequestHandler() {
                    SetPageLayout();

                }

                function SetPageLayout() {
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
        </div>

        <!-- Skill Master --ModalPopUp -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySkillMaster" Text="Dummy Skill Master" />
        </div>
        <asp:Panel runat="server" ID="pnlSkillMaster" ClientIDMode="Static" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IframeSkill" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopUpSkillMaster" runat="server" TargetControlID="btnDummySkillMaster"
            PopupControlID="pnlSkillMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameSkillStateComplete() {
                $("#btnDummySkillMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenSkillWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeSkill").attr("src", "wfSkill_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummySkillMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSkill() {
                var Skillwindow = $find("<%=mdlPopUpSkillMaster.ClientID %>");
                //close Skill popup window
                Skillwindow.hide();
                //   release resources
                $("#IframeSkill").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnSkill").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
