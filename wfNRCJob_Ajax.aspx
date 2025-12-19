<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNRCJob_Ajax.aspx.vb"
    Inherits="Flypal.wfNRCJob_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NRC Job</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
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
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <asp:UpdatePanel ID="upnlNRCJobDetail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="clstablelistin" id="tblLedgerList">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblListEnquiry" class="clsFormHeader">NRC Job</span>
                                                </td>

                                                <td align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to Add the NRC Job"
                                                                    ValidationGroup="a" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="a" />
                                        <asp:RequiredFieldValidator ID="rfvObservation" runat="server" ControlToValidate="txtObservation"
                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Observation required"
                                            ValidationGroup="a"></asp:RequiredFieldValidator>
                                       <%-- <asp:RequiredFieldValidator ID="rfvRectification" runat="server" ControlToValidate="txtRectification"
                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Rectification required"
                                            ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                        <asp:CustomValidator ID="cvObservation" runat="server" ClientValidationFunction="validateName"
                                            ControlToValidate="txtObservation" CssClass="clsValidationSummary" Display="None"
                                            ErrorMessage="Observation should not be greater than 1000 characters" ValidationGroup="a"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRectification" runat="server" ClientValidationFunction="validateName"
                                            ControlToValidate="txtRectification" CssClass="clsValidationSummary" Display="None"
                                            ErrorMessage="Rectification should not be greater than 1000 characters" ValidationGroup="a"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvDoneByAME" runat="server" CssClass="clsValidationSummary"
                                            Display="None" ClientValidationFunction="validateName" ControlToValidate="txtDoneByAME"
                                            ErrorMessage="Rectification AME name should not be same as Technician." ValidationGroup="a"></asp:CustomValidator>
                                        <%--<asp:CustomValidator ID="cvDoneByTech" runat="server" CssClass="clsValidationSummary"
                                            Display="None" ClientValidationFunction="validateName" ControlToValidate="txtDoneByTech"
                                            ErrorMessage="Rectification AME name should not be same as Tech." ValidationGroup="a"></asp:CustomValidator>--%>
                                        <script type="text/javascript">
                                            function validateName(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'txtObservation':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 1000) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtRectification':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 1000) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtDoneByAME':
                                                        var DoneByAME = $get(ControlName).value;
                                                        var DoneByTech = document.getElementById('txtDoneByTech');
                                                        if (DoneByAME == DoneByTech.value) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                }
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend><b>NRC Job</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSrNo" class="clsLabelAuto">Sr. No.</span>
                                                                </td>
                                                                <td colspan="5">
                                                                    <asp:TextBox ID="txtSrNo" runat="server" BorderColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                                        Enabled="False" MaxLength="10" Text="<%# mNRC.NRCJobs.CurrentItem.SrNo %>" ToolTip="Enter Sr.  No."></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" class="clsLabelAuto">Observation</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtObservation" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                        ClientIDMode="Static" ToolTip="Enter Observation" Text="<%# mNRC.NRCJobs.CurrentItem.Observation %>"
                                                                        TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblObserveByAME" class="clsLabelAuto">AME</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtObserveByAME" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtObserveByAME_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
                                                                        onChange="SetEmpIdonChange('txtObserveByAME','txtObserveByAME_Autocomplete')">
                                                                    </asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtObserveByAME_Autocomplete"
                                                                        runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                        MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList"
                                                                        TargetControlID="txtObserveByAME" OnClientItemSelected="SetID" UseContextKey="False"
                                                                        ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                        CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                        OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                        OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnObserveByAMEID" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span4" class="clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Rectification</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRectification" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                        MaxLength="1000" Text="<%# mNRC.NRCJobs.CurrentItem.Rectification %>" TextMode="MultiLine"
                                                                        ToolTip="Enter Rectification"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDoneByAME" class="clsLabelAuto">AME</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoneByAME" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtDoneByAME_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
                                                                        onChange="SetEmpIdonChange('txtDoneByAME','txtDoneByAME_Autocomplete')"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtDoneByAME_Autocomplete" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                        CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtDoneByAME"
                                                                        OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnDoneByAMEID" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                                <td>
                                                                    <span id="lblDoneByTech" class="clsLabelAuto">Tech</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoneByTech" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                        OnTextChanged="txtDoneByTech_TextChanged" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
                                                                        onChange="SetEmpIdonChange('txtDoneByTech','txtDoneByTech_Autocomplete')"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtDoneByTech_Autocomplete" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                        CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtDoneByTech"
                                                                        OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdnDoneByTechID" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to Add the NRC Job"
                                                                        ValidationGroup="a" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
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
            parent.ParentCallBackFunctionForNRCJob();
            return false;
        }
        function CallParentCallbackDirect() {
            parent.ParentCallBackFunctionForNRCJobDirect();
            return false;
        }
    </script>
    <%--End--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
            <% Dim mopen As String = Request.QueryString("Type") %>
            <% If Not mopen Is Nothing AndAlso mopen = "pup" or mopen = "MELpup" Then %>  
                $(document).ready(function () {
               SetPageLayout();
                 if ($.browser.msie) {
                     parent.IframeNRCJobStateComplete();
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
                  <% If Not mopenas Is Nothing AndAlso mopenas = "pup" or mopenas = "MELpup" Then %>  
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
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--
    Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }

            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtObserveByAME_Autocomplete") {
                textbox = document.getElementById('hdnObserveByAMEID');
            }
            if (source._id == "txtDoneByAME_Autocomplete") {
                textbox = document.getElementById('hdnDoneByAMEID');
            }
            if (source._id == "txtDoneByTech_Autocomplete") {
                textbox = document.getElementById('hdnDoneByTechID');
            }
            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetEmpIdonChange(cntrl, extender) {
            var cntrlName = '#' + cntrl;
            var popup = $find(extender);
            var complist = popup.get_completionList();
            var text = $(cntrlName).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    if (cntrl == "txtObserveByAME") {
                        var textbox = document.getElementById('hdnObserveByAMEID');
                    }
                    if (cntrl == "txtDoneByAME") {
                        textbox = document.getElementById('hdnDoneByAMEID');
                    }
                    if (cntrl == "txtDoneByTech") {
                        textbox = document.getElementById('hdnDoneByTechID');
                    }
                    textbox.value = val.toString();
                    return;
                }
            }
            if (cntrl == "txtObserveByAME") {
                var textbox = document.getElementById('hdnObserveByAMEID');
            }
            if (cntrl == "txtDoneByAME") {
                textbox = document.getElementById('hdnDoneByAMEID');
            }
            if (cntrl == "txtDoneByTech") {
                textbox = document.getElementById('hdnDoneByTechID');
            }
            textbox.value = '';
            return;
        }
    </script>
    </form>
</body>
</html>
