<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualRevision_Ajax.aspx.vb"
    Inherits="Flypal.wfManualRevision_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Revision Detail</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
                            <table width="100%">
                                <tr>
                                <td>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Revision Detail</asp:Label>
                                </td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="OK" ValidationGroup="a" ToolTip="Click to Add Revision"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"></asp:Button>
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
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                        DisplayMode="BulletList" ValidationGroup="a" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="refEff" runat="server" ControlToValidate="calRevDate"
                                        ErrorMessage="Effective Date Required." Display="None" ValidationGroup="a" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvNote" runat="server" CssClass="clsLabelAuto" ErrorMessage="Note should be less than or equal to 255 characters."
                                        Display="None" ControlToValidate="txtNote" ClientValidationFunction="validateName"
                                        ValidationGroup="a"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" ErrorMessage="Remark should be less than or equal to 255 characters."
                                        Display="None" ControlToValidate="txtRemark" ClientValidationFunction="validateName"
                                        ValidationGroup="a"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"></asp:CustomValidator>
                                    <script type="text/javascript">
                                        function validateName(source, args) {
                                            var ControlName = source.controltovalidate;
                                            switch (ControlName) {
                                                case 'txtRemark':
                                                    var Value = $get(ControlName).value.length;
                                                    if (Value > 255) {
                                                        args.IsValid = false;
                                                        return
                                                    }
                                                    break;
                                                case 'txtNote':
                                                    var Value = $get(ControlName).value.length;
                                                    if (Value > 255) {
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
                            <span id="lblRevisionDetails" class="clsLabelHeader">Revision Details</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlRevisionDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblNo" class="clsLabelAuto">No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                    Text="<%# mManual.Revisions.CurrentItem.No %>" ToolTip="Enter No." Width="250px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblRevisionNo" class="clsLabelAuto">Revision No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRevNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                    Text="<%# mManual.Revisions.CurrentItem.RevNo %>" ToolTip="Enter Revision No."
                                                    Width="250px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblFrequency" class="clsLabelAuto">Frequency</span>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ID="txtFrequency" runat="server" AutoPostBack="True"
                                                    MaxLength="4" Text="<%# mManual.Revisions.CurrentItem.Frequency %>" ToolTip="Enter Frequency"></asp:TextBox>
                                                <span id="lblMonths" class="clsLabelAuto">In Months (0 Frequency means Unlimited)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span1" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblEffectiveDate" class="clsLabelAuto">Effective Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="calRevDate" CssClass="clsTextBoxTagSearch" Width="100px" AutoComplete="off"
                                                    AutoPostBack="true" onchange="ValidateDateText(this,'calRevDate_watermarkextender','false');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calRevDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calRevDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="calRevDate" ID="calRevDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="Span3" class="clsLabelAuto">Next Revision Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="calNextRevisionDate" CssClass="clsTextBoxTagSearch" Width="100px" AutoComplete="off"
                                                    AutoPostBack="true" onchange="ValidateDateText(this,'calNextRevisionDate_watermarkextender','true');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calNextRevisionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calNextRevisionDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="calNextRevisionDate" ID="calNextRevisionDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblManualIn" class="clsLabelAuto">Manual In</span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkHardCopy" runat="server" CssClass="clsLabelAuto" Text="Hard Copy"
                                                    Checked="<%# mManual.Revisions.CurrentItem.HardCopy %>" TextAlign="Left"></asp:CheckBox>
                                                <asp:CheckBox ID="chkSoftCopy" runat="server" CssClass="clsLabelAuto" Text="Soft Copy"
                                                    Checked="<%# mManual.Revisions.CurrentItem.SoftCopy %>" TextAlign="Left"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblNote" class="clsLabelAuto">Note</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="255"
                                                    ClientIDMode="Static" Width="250px" Text="<%# mManual.Revisions.CurrentItem.Note %>"
                                                    TextMode="MultiLine" ToolTip="Enter Note">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="255"
                                                    ClientIDMode="Static" Width="250px" Text="<%# mManual.Revisions.CurrentItem.Remark %>"
                                                    ToolTip="Enter remark" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                    <legend class="clsFieldSet1"><b>File Attachments</b></legend>
                                                    <asp:UpdatePanel ID="upnlManRevisionAttachment" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="height: 15px">
                                                                        <asp:GridView ID="dgManRevisionAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                            CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                            AllowPaging="False" AutoGenerateColumns="false"
                                                                            GridLines="Horizontal" CellPadding="3">
                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                            
                                                                            <Columns>
                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                <asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="File Name">
                                                                                    <HeaderStyle Width="350px" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
                                                                                            ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>'
                                                                                            Width="350px"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                            CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                            CausesValidation="false" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>

                                                                                <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <%-- <span id="button">Login</span>
                                                                                        <div class="dropdown">
                                                                                            <div class="dropdownbtn-content">
                                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                                                Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                                CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                                CausesValidation="false" />
                                                                                                        </td>

                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                                Style="cursor: pointer" />
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>--%>

                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
                                                                            CausesValidation="false" Height="22px" Width="24px" ToolTip="Click to Add New Attachment">
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                        runat="server" class="clsButton_Ajax" causesvalidation="False" />
                                                                </td>
                                                                <td style="padding-left: 3px;">
                                                                    <asp:Button ID="btnDelAttach" runat="server" CausesValidation="false" CssClass="clsButton_Ajax"
                                                                        Enabled="False" Text="Remove Attachment" ToolTip="Click to Remove Attachment"
                                                                        Width="120px" />
                                                                </td>
                                                                <td style="padding-left: 2px;">
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                                        ImageUrl="icons/CLIP01.ICO" Width="20px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>--%>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <%--<td align="right">
                            <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="OK" ValidationGroup="a">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>--%>
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

        function OpenFileUploadWindow() {
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
        }
       
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
    <!--call parent function after completing subroutine..(when page open as popup)-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForManualRevision();
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
             parent.IFrameManualRevisionStateComplete();
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
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var resetTodaysDate = 'false';
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
</body>
</html>
