<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfGroupTrainingRenewal.aspx.vb"
    Inherits="Flypal.wfGroupTrainingRenewal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Group Employee Training Renewal</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script src="jquery.tablednd_0_5.js" type="text/javascript"></script>
    <script src="json2.js" type="text/javascript"></script>
    <style type="text/css">
        .GbiHighlight
        {
            background-color: Teal;
        }
    </style>
    <!-- End-->
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox runat="server" ID="MSGBoxCtrl" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Group Employee Training Renewal</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td align="right">
                                            <%-- AJAX Update Panel --%>
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="clsbtnH clsinfoH"
                                                                    Text="Renew" ToolTip="Click to Renew" ValidationGroup="valGroup1" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go to the Previous Page" />
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
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvTrainingOrgName" runat="server" ErrorMessage="Please select the Training Organization."
                                            ControlToValidate="cmbTrainingOrgList" Display="None" ClientValidationFunction="validateTrainingOrgName"
                                            ValidationGroup="valGroup1" CssClass="clsLabelAuto"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                            Display="None" ErrorMessage="Date should not be blank" ValidationGroup="valGroup1"
                                            CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <!-- Client side validation for comboboxes CHK if script executes for posted back to server everytime-->
                                <script type="text/javascript">
                                    //Training Org Name
                                    function validateTrainingOrgName(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbTrainingOrgList");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;

                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlTrainingDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsTrainingInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="lblTrainingDetails" runat="server" style="font-weight: bold"><b>Training
                                                Details</b></legend>
                                            <table id="Table1" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <%--<span id="lblTrainingName" class="clsLabelAuto">Training Name </span>--%>
                                                        <asp:Label runat="server" id="lblTrainingName" Text="Training Namess"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Training Name"
                                                            Text="<%# mTraining.Name %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTrainingType" runat="server" CssClass="clsLabelAuto">Training Type  </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbTrainingType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataValueField="ID" DataTextField="Name" SelectedValue="<%# mTraining.TrainingTypeID %>"
                                                            BackColor="#E0E0E0" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblRecurringStatus" class="clsLabel">Recurring Status </span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkRecurringStatus" runat="server" CssClass="clsCheckBox" ToolTip="Check this in case the Training is Recurring"
                                                            Text="(in case the Training is Recurring)" Checked="<%# mTraining.RecurringStatus %>"
                                                            Enabled="false"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblFreqInMonths" class="clsLabelAuto">Freq In Months </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFreqInMonths" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ToolTip="Enter Freq In Month" Text="<%# mTraining.FreqInMonths %>" MaxLength="5"
                                                            BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblWarningDays" class="clsLabelAuto">Warning Days</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ToolTip="Enter Warning Days" Text="<%# mTraining.WarningDays %>" MaxLength="5"
                                                            BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">List of Employees for Selection</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="max-height: 300px; overflow-y: auto; overflow-x: hidden">
                                                        <%--<asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" >--%>
                                                        <asp:GridView ID="dgEmpTrainingList" runat="server" AutoGenerateColumns="False"
                                                            DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true" ClientIDMode="Static"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <input type="checkbox" id="chkSelectAll" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                            <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="15px" Wrap="true" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                                <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DesignationName" HeaderText="Designation" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left"  />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Date" HeaderText="Date">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Duration" HeaderText="Training Duration">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="true"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TrainingOrgNameWithCity" HeaderText="Training Org" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FreqInMonths" HeaderText="Freq In Months">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="true"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="right" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days"> 
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="true"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="right" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="left" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="History" HeaderText="History" CommandName="History" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>
                                                                <asp:ButtonField Text="View" HeaderText="Attach" CommandName="Attach" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div id="dropDownImg" class="dropdown">
                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                <tr>

                                                                                    <td>
                                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            CommandName="History"
                                                                                            ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>'
                                                                                            ToolTip="Click to View History" />

                                                                                    </td>
                                                                                   
                                                                                    <td>
                                                                                        <asp:ImageButton ID="View" Style="height: 20px; width: 13px" runat="server"
                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            ToolTip="Click to View Attachment"
                                                                                            CommandName="Attach" ImageUrl="icons/CLIP01.ICO"
                                                                                            Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                    </td>
                                                                                    
                                                                                    
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <%--  </asp:Panel>--%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlRenewalInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsRenewalInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="ldgRenewalInfo" runat="server" style="font-weight: bold"><b>Renewal Details</b></legend>
                                            <table>
                                                <tr>
                                                    <td align="center">
                                                        <span id="Label2" class="clsLabelStar" style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Label17" class="clsLabelAuto">Done On Date</span>
                                                    </td>
                                                    <td>
                                                        <!-- CHK if seperate update panel is required -->
                                                        <asp:TextBox ID="txtDate" CssClass="clsTextBoxTagSearchDate"  ClientIDMode="Static" runat="server"
                                                            onchange="ValidateDateText(this,'txtDate_CalendarExtender');" CausesValidation="true"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Calender_watermarkextender"
                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="lblDuration" class="clsLabelAuto">Duration</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDuration" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ToolTip="Enter Duration" MaxLength="2">
                                                        </asp:TextBox>
                                                        <span id="lblInmonth" class="clsLabelAuto">(In Days)</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <!-- CHK if seperate update panel is required -->
                                                        <span id="Label3" class="clsLabelStar" style="color: Red;">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblTrainingOrgName" class="clsLabelAuto">Training Org Name</span>
                                                    </td>
                                                    <td>
                                                        <table id="Table4" border="0" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:DropDownList ID="cmbTrainingOrgList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataTextField="NameWithCity" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td class="clsInnerTable">
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" runat="server" id="btnSelectFile" value="Select File" 
                                                                                class="clsbtnH clsinfoH1" causesvalidation="False" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                Text="Remove Attachment" ></asp:Button>
                                                                        </td>
                                                                        <td style="padding-left: 2px;">
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Height="20px" Width="20px"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
                                <%-- AJAX Update Panel 
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="clsbtnH clsinfoH"
                                                        Text="Renew" ToolTip="Click to save Training" ValidationGroup="valGroup1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go to the Previous Page" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <!--Dummy panel to open modelpopup-->
        <tr style="height: 0px;">
            <td style="height: 0px;">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                    <ContentTemplate>
                        <asp:Button ID="hdnBtnEmpTrainingHistory" ClientIDMode="Static" runat="server" Text="Add"
                            CausesValidation="False" Style="display: none;"></asp:Button>
                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                            CausesValidation="False" Style="display: none;"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpTraining();
            return false;
        }
    </script>
    <%--End--%>
    <!--Set page layout when open as popup aspx page-->
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameEmployeeTrainingStateComplete();
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
    <!-- End-->
    <!-- Employee Training History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpTrainingHistory" Text="Employee Training History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpTrainingHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpTrainingHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpTrainingHistory" runat="server" TargetControlID="btnDummyEmpTrainingHistory"
        PopupControlID="pnlEmpTrainingHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpTrainingHistoryStateComplete() {
            $("#btnDummyEmpTrainingHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpTrainingHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpTrainingHistory").attr("src", "wfEmployeeTrainingHistoryList_Ajax.aspx?Type=pup");


                $("#btnDummyEmpTrainingHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpTrainingHistory() {
            var EmpTrainingHistorywindow = $find("<%=mdlPopupEmpTrainingHistory.ClientID %>");
            //close Training popup window
            EmpTrainingHistorywindow.hide();
            //           release resources
            $("#IframeEmpTrainingHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpTrainingHistory").click();
        }
    </script>
    <!-- End-->
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
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
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
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
    </form>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#btnSave").live("click", function () {
                var index = new Array();
                var srno = new Array();
                $("#<%= dgEmpTrainingList.ClientID %> tr:not(:first)").each(function (i) {
                    index[i] = i;
                    srno[i] = $(this).find("td:first").html();
                });
                var myobj = new Object();
                myobj.SrNo = srno;
                myobj.index = index;
                var myData = "{Ids:" + JSON.stringify(myobj) + "}";
                $.ajax({
                    url: "wfGroupTrainingRenewal.aspx/GetTableIDs",
                    data: myData,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //$("#" + ID).html(data.d).slideDown("medium");
                        //alert(data);
                    },
                    error: function (data, status, jqXHR) {// $("#" + ID).html(status);
                    }
                });
                return true;
            });
        });       
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).parents('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor')
                else
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).prop('checked');
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor');
                else
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });
        });
    </script>
</body>
</html>
