<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDocumentLockerRegister.aspx.vb" Inherits="Flypal.wfDocumentLockerRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document Locker Register View</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>

</head>
<body>
    <form id="frmDocumentLockerRegisterView" runat="server">
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
                            <td class="clsFormHeader1Newstyle">
                                <asp:Label ID="lblTitle" runat="server" 
                                    CssClass="clsFormHeader" Text="Document Locker Register" />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlExpandDocumentlock" runat="server" CssClass="clsExpandiblePnl">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFilename" runat="server"
                                                                        CssClass="clsLabelAuto" Text="Document Name" />
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtFileNameSearch" runat="server"
                                                                        CssClass="clsTextBoxTagSearch"
                                                                        ClientIDMode="Static" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCategory" runat="server" 
                                                                        CssClass="clsLabelAuto" Text="Category" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbCategorySearch" runat="server" 
                                                                        CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                                        DataTextField="Name" AutoPostBack="true" 
                                                                        DataValueField="ID" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblDepartment" runat="server" Text="Department" 
                                                                        CssClass="clsLabelAuto" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDepartmentsearch" runat="server" 
                                                                        CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                                        DataTextField="EmployeeDepartmentName"
                                                                        DataValueField="EmployeeDepartmentID" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblAircraftList" runat="server" Text="Aircraft" 
                                                                        CssClass="clsLabelAuto" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlAircraftSearch" runat="server" 
                                                                        CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataValueField="ID" DataTextField="RegNo" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="2">
                                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:ImageButton ID="btnSearch" runat="server"
                                                                                ImageUrl="~/images/Search2.png"
                                                                                ToolTip="Search as per Criteria."
                                                                                CausesValidation="false" class="clsSearch2btn" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSetNewStyle">
                                                        <legend class="clsFieldSet1">
                                                            File Attachments
                                                        </legend>
                                                        <asp:UpdatePanel ID="upnlManAttachment" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                               
                                                                <asp:GridView ID="dgAttachment" DataKeyNames="ID" ShowHeaderWhenEmpty="true" 
                                                                    AllowSorting="True" AllowPaging="True" AutoGenerateColumns="false" 
                                                                    PageSize="15" CssClass="clsGridNewStyle" GridLines="Horizontal" 
                                                                    CellPadding="5" runat="server">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" 
                                                                        ForeColor="black" HorizontalAlign="Left" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" 
                                                                        LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black"
                                                                        HorizontalAlign="Right" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="5%" 
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <%# Container.DataItemIndex + 1 %>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="table_04" HorizontalAlign="Left" />
                                                                            <ItemStyle CssClass="table_02" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
                                                                        <asp:BoundField DataField="Name" HeaderText="Document Name">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"  />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CategoryName" HeaderText="Category">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DepartmentName" HeaderText="Department">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="WarningDays" HeaderText="Warning Days"
                                                                            Visible="false">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Valid upto" 
                                                                            Visible="false">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="IsPublic" HeaderText="Document Type">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="UserName" HeaderText="User">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" 
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="View" runat="server"
                                                                                    CommandArgument='<%# Eval("ID") %>' CommandName="View"
                                                                                    Style="height: 20px; width: 13px" 
                                                                                    ImageUrl="icons/CLIP01.ICO" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="UserID" HeaderText="UserID"
                                                                            HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" 
                                            CssClass="clsbtnH clsinfoH" Text="Close"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

        <!-- Credential Master --ModalPopUp -->
        <div>

            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyLoginMaster" Text="Dummy Issuing Authority Master" />
            </div>

            <asp:Panel runat="server" ID="pnlLoginMaster" Style="display: none">
                <div>
                    <table class="clstablelistout" id="TABLE7">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlLoginMaster" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="clstablelistin" id="TABLE8">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" 
                                                        CssClass="clsValidationSummary" ValidationGroup="valGroup3" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                        runat="server" CssClass="clsLabelAuto"
                                                        ControlToValidate="txtLoginName" Display="None"
                                                        ErrorMessage="Login Name Required"
                                                        ValidationGroup="valGroup3" />
                                                    <asp:CustomValidator ID="cvLogin" runat="server"
                                                        CssClass="clsLabelAuto" ControlToValidate="txtPassword"
                                                        Display="None" ErrorMessage="Issuing Authority Name too Long."
                                                        OnServerValidate="CustomValidation"
                                                        ValidationGroup="valGroup3" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblInvalid" runat="server" ForeColor="DarkRed" CssClass="clsWidth"
                                                        Style="font-size: 7pt"></asp:Label>
                                                    <span id="Span2" class="clsLabelHeader">Login Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="center">
                                                    <span id="Span3" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="Span4" class="clsLabelAuto">Name</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLoginName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Login Name"
                                                        MaxLength="200">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="center">
                                                    <span id="Span1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="Span5" class="clsLabelAuto">Password</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="clsTextBoxTagSearch"
                                                        ToolTip="Enter Login Password" MaxLength="200">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="3">
                                                    <table id="Table9" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnLoginMaster" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    CausesValidation="False" Text="Ok"></asp:Button>
                                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                    Text="Close"></asp:Button>
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

            <cc2:ModalPopupExtender ID="mdlPopUpLoginMaster" runat="server" TargetControlID="btnDummyLoginMaster"
                PopupControlID="pnlLoginMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>

        </div>
        <!-- End -->

        <!-- Credential OTP --ModalPopUp -->
        <div>

            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyOTP" Text="Dummy Issuing Authority Master" />
            </div>

            <asp:Panel runat="server" ID="pnlOTPMaster" Style="display: none">
                <div>
                    <table class="clstablelistout" id="TABLE1">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlOTPMaster" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="clstablelistin" id="TABLE2">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="valGroup3" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                        runat="server" CssClass="clsLabelAuto"
                                                        ControlToValidate="txtOTP" Display="None"
                                                        ErrorMessage="Login Name Required"
                                                        ValidationGroup="valGroup3" />
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server"
                                                        CssClass="clsLabelAuto"
                                                        ControlToValidate="txtPassword" Display="None"
                                                        ErrorMessage="Issuing Authority Name too Long."
                                                        OnServerValidate="CustomValidation" ValidationGroup="valGroup3" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblOTPInvalid" runat="server" ForeColor="DarkRed" CssClass="clsWidth"
                                                        Style="font-size: 7pt"></asp:Label>
                                                    <span id="Span6" class="clsLabelHeader">OTP Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="center">
                                                    <span id="Span7" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="Span8" class="clsLabelAuto">OTP</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOTP" runat="server" CssClass="clsTextBoxDate_Ajax" Width="185px" ToolTip="Enter OTP"
                                                        MaxLength="200">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="3">
                                                    <table id="Table3" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOTPOk" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                    Text="Ok"></asp:Button>
                                                                <asp:Button ID="btnOTPClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    CausesValidation="False" Text="Close"></asp:Button>
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

            <cc2:ModalPopupExtender ID="mdlPopupOTPMaster" runat="server" TargetControlID="btnDummyOTP"
                PopupControlID="pnlOTPMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>

        </div>
        <!-- End -->

        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">

            <% Dim mopen As String = Request.QueryString("Type") %>

            <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

                    $(document).ready(function () {

                        SetPageLayout();
                        if ($.browser.msie) {
                            parent.IFrameAttachStateComplete();
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
                    onResize();
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

            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    cache: false,
                    async: false,
                    data: params,
                    beforeSend: OnBeforeSend,
                    success: onSuccess,
                    error: onError
                });
                return false;
                function onSuccess(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val(result);
                    $find(extenderid).set_Text(result);
                }

                function onError(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val('');
                    $find(extenderid).set_Text('');
                }
                function OnBeforeSend() {
                    $(elem).addClass('ac_loading');
                }
            }

        </script>
        <%--End--%>

    </form>
</body>
</html>
