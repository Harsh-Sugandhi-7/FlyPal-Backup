<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeService_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeService_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Employee Service</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
                 
        }
    </script>
    
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    <style type="text/css">
        #Table3
        {
            height: 22px;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
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
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" TabIndex="1" CssClass="clsFormHeader" runat="server">Employee Service Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save Service Information"
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
                                <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" Width="440px" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDate"
                                            Display="None" ErrorMessage="Please Select Date." ValidationGroup="valGroup1">Date Required</asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvService" runat="server" CssClass="clsLabelAuto" Width="64px"
                                            ControlToValidate="cmbServiceList" Display="None" ErrorMessage="Please Select the Service."
                                            ValidationGroup="valGroup1" ClientValidationFunction="validateService"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateService(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbServiceList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlServiceDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblServiceDetails" class="clsLabelHeader">Employee Service Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Employee Name"
                                                        MaxLength="25" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mEmployee.Name %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="Label17" class="clsLabelAuto">Date</span>
                                                </td>
                                                <td> 
                                                    <asp:TextBox ID="txtDate" CssClass="clsTextBoxTagSearchDate"  ClientIDMode="Static" runat="server"
                                                        AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Calender_watermarkextender"
                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Label1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblService" class="clsLabelAuto">Service</span>
                                                </td>
                                                <td>
                                                    <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbServiceList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    DataValueField="ID" DataTextField="Name" SelectedValue="<%# mEmployeeService.ServiceID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                               <%-- <asp:Button ID="imgService" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to Add New Service" CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="imgService" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                    Width="24px" ToolTip="Click to Add New Service" CausesValidation="True"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblAttach" class="clsLabel">Attach File</span>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSelectFile" value="Select File"
                                                                    class="clsbtnH clsinfoH1">
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                    Text="Remove Attachment" Enabled="False"></asp:Button>
                                                            </td>
                                                            <td style="padding-left: 2px;">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                    Height="20px" Width="20px"></asp:ImageButton>
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
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save Service Information"
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
                        <!--Dummy panel to open modelpopup for category/nomenclature-->
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
                    //                        $("#IFileUpload").ready(function () {
                    //                            $("#btnDummyFileUpload").click();
                    //                            $get("AjaxLoader").style.visibility = 'hidden';
                    //                        });
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpService();
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
                 parent.IFrameEmpServiceStateComplete();
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
    <!-- Service --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyService" Text="Dummy Service" />
    </div>
    <asp:Panel runat="server" ID="pnlService" Style="display: none">
        <div>
            <table class="clstablelistout" id="Table2">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlService" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="Table4">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <asp:Label ID="lblTitleService" TabIndex="1" CssClass="clsFormHeader" runat="server">Service Information [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="valGroup2"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                                Display="None" ErrorMessage="Service Required" ValidationGroup="valGroup2">Service Required</asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvDocument" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                                Display="None" ErrorMessage="Document Name too Long." OnServerValidate="customvalidate"
                                                ValidationGroup="valGroup2"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                      <%--  <td colspan="3">
                                            <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnNewService" CssClass="clsbtnH clsinfoH1" runat="server" CausesValidation="False"
                                                ToolTip="Click to Add the Service" Text="New"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="Label3" class="clsLabelHeader">Service Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Label4" class="clsLabelStar" style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <span id="lblName" class="clsLabelAuto">Name</span>
                                        </td>
                                        <td colspan="1">
                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Service Name"
                                                Text="<%# mService.Name %>" MaxLength="50">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="3">
                                            <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSaveService" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Service Information"
                                                Text="Save" ValidationGroup="valGroup2"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblSearch" class="clsLabelHeader">Service List</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <%--<div style="width: 300px;">
                                                <table cellpadding="0" cellspacing="0" class="clsGrid" style="width: 300px; border-collapse: collapse;">
                                                    <tr>
                                                        <td class="clsdgHeader" width="170px">
                                                            <span>Name</span>
                                                        </td>
                                                        <td class="clsdgHeader" width="80px">
                                                            <span>Edit/View</span>
                                                        </td>
                                                        <td class="clsdgHeader" width="50px">
                                                            <span>Delete</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <div>
                                                <asp:GridView ID="dgService" runat="server" AutoGenerateColumns="False"
                                                    Style="width: 300px;" ShowHeader="true" ShowHeaderWhenEmpty="true" DataKeyNames="ID"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="true" PageSize="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Name">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="170px" />
                                                        </asp:BoundField>
                                                       <%-- <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="80px" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" />
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
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="right">
                                            <table>
                                                <tr>

                                                    <td align="right">
                                                        <asp:Button ID="btnNewService" CssClass="clsbtnH clsinfoH1" runat="server" CausesValidation="False"
                                                            ToolTip="Click to Add the new Service" Text="New"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnSaveService" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Service Information"
                                                            Text="Save" ValidationGroup="valGroup2"></asp:Button>
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnCloseService" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            CausesValidation="False" ToolTip="Click to close Service Information screen"
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
    <cc2:ModalPopupExtender ID="mdlPopUpService" runat="server" TargetControlID="btnDummyService"
        PopupControlID="pnlService" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End -->
    </form>
</body>
</html>
