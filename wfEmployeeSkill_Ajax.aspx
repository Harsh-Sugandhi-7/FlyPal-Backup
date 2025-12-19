<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeSkill_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeSkill_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Skill</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Employee Skill Information</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Skill Information"
                                                                    Text="Save" ValidationGroup="valGroup1"></asp:Button>
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
                                            Display="None"  ValidationGroup="valGroup1" ErrorMessage="Please Select the Skill."></asp:CustomValidator>
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
                                       <script type = "text/javascript">
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
                                            <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Employee Skill Details</b></legend>
                                            <table>
                                                <%--<tr>
                                                <td colspan="3">
                                                    <span id="lblSkillDetails" class="clsLabelHeader">Employee Skill Details</span>
                                                </td>
                                            </tr>--%>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                            ToolTip="Employee Name" ReadOnly="True" BackColor="#E0E0E0"><%--Text="<%# mEmployee.Name %>"--%>
                                                        </asp:TextBox>
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
                                                                    <%-- <asp:DropDownList ID="cmbSkillList" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                  DataTextField="Name" SelectedValue="<%# mEmployeeSkill.SkillID %>">
                                                                </asp:DropDownList>--%>
                                                                    <asp:CheckBoxList ID="chkSkillList" CssClass="clsComboBox3_Ajax" runat="server" RepeatColumns="2"
                                                                        DataValueField="ID" DataTextField="CodeWithName">
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
                                                <%--
                                         
                                                'Commented by Shital on 18-Aug-2016
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblValue" class="clsLabelAuto">Value</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBox_Ajax" MaxLength="10"
                                                        ToolTip="Enter Value" Text="<%# mEmployeeSkill.Value %>" Height="16px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxLong_Ajax" MaxLength="255"
                                                        ToolTip="Enter Remark" Text="<%# mEmployeeSkill.Remark %>" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblIsSkill" class="clsLabelAuto">Skill</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsSkill" runat="server" CssClass="clsCheckBox" Checked="<%# mEmployeeSkill.IsSkill %>">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="clsInnerTable">
                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                    class="clsButton_Ajax">
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                            </td>
                                                            <td style="padding-left: 2px;">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                    Height="20px" Width="20px"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>--%>
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
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Skill Information"
                                                        Text="Save" ValidationGroup="valGroup1"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
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
            parent.ParentCallBackFunctionForEmpSkill();
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
                    parent.IFrameEmpSkillStateComplete();
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
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
        PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameFileUploadStateComplete() {
            $("#btnDummyFileUpload").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForFileUpload(fileattached) {
            var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
            //close File Upload popup window
            FileUpwindow.hide();
            //Free resources
            $("#IFileUpload").attr("src", "JavaScript:''");
            if (fileattached) {
                //call hidden button to set file upload content to object
                $("#hdnBtnFileUpload").click();
            }
        }
    </script>
    <!-- End -->

    <!-- Skill Popup Window -->
      <div style="display: none">
        <asp:Button runat="server" ID="btnDummySkill" Text="Employee Skill" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSkill" ClientIDMode="Static" HorizontalAlign="Center"   Style="height: 100%; width: 100%;">
        <iframe id="IframeSkill" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSkill" runat="server" TargetControlID="btnDummySkill"
        PopupControlID="pnlSkill" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSkillStateComplete() {
            $("#btnDummySkill").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSkillWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSkill").attr("src", "wfSkill_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySkill").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSkill() {
            var Skillwindow = $find("<%=mdlPopupSkill.ClientID %>");
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
