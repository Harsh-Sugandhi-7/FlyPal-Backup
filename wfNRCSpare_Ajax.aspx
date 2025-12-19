<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNRCSpare_Ajax.aspx.vb"
    Inherits="Flypal.wfNRCSpare_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>NRC Spare Detail</title>
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
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="Label4" class="clsFormHeader">Spare Detail</span>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlAddButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table9" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to Add Spare"
                                                                    CausesValidation="true" ValidationGroup="b"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close  screen"
                                                                    CausesValidation="false"></asp:Button>
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
                                <asp:UpdatePanel ID="upnlSpareValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields" Width="100%" ValidationGroup="b"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvSelectPart" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="b" ControlToValidate="txtSearch" Display="None" ErrorMessage="Enter part no."></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                                    ValidationGroup="b" ValidateEmptyText="true" Display="None" ErrorMessage="Enter whole part no. and description"
                                                    OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvQty" runat="server" CssClass="clsValidationSummary"
                                                    ControlToValidate="txtReqQty" ErrorMessage="Qty  Required" Display="None" ValidationGroup="b"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                                    ControlToValidate="txtReqQty" ValidationGroup="b" ValidateEmptyText="true" Display="None"
                                                    ErrorMessage="" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="b" ControlToValidate="txtRemark" Display="None" ErrorMessage="Remark should not be greater than 500 characters."
                                                    ClientValidationFunction="ValidateRemark"></asp:CustomValidator>
                                                <script type="text/javascript">
                                                    function ValidateRemark(source, args) {
                                                        args.IsValid = false;
                                                        var nameLength = $get("txtRemark").value.length;
                                                        if (nameLength <= 500) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }
                                                </script>
                                            </td>
                                        </tr>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSpareDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="Label8" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblPlaceName" class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                           CssClass="clsTextBoxTagSearch" onChange="SetPartIdonChange()"></asp:TextBox>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                            CompletionInterval="1" ServicePath="wfNRCSpare_Ajax.aspx" ServiceMethod="GetPartNoDescriptionList"
                                                            TargetControlID="txtSearch" OnClientItemSelected="SetID" UseContextKey="False"
                                                            ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                            OnClientShowing="ClientShowing">
                                                        </cc2:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span2" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblReqQty" class="clsLabelAuto">Required Qty.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReqQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ClientIDMode="Static" AutoPostBack="true" MaxLength="4" Text="<%# mNRC.NRCSpares.CurrentItem.RequiredQty %>"
                                                            ToolTip="Enter Required Quantity"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRate" class="clsLabelAuto">Landing Rate</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEffRate" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearch" 
                                                         style="text-align:right" ClientIDMode="Static" MaxLength="7" Text="<%# mNRC.NRCSpares.CurrentItem.EffRate %>"
                                                            ></asp:TextBox>
                                                        <span id="Span1" class="clsLabelAuto">In Base Currency</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblEstimatedCost" class="clsLabelAuto">Actual Cost</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEstimatedCost" runat="server" CssClass="clsTextBoxTagSearch" style="text-align:right"
                                                            MaxLength="12" BackColor="#E0E0E0" ReadOnly="true" Text="<%# mNRC.NRCSpares.CurrentItem.EstimatedCost %>"
                                                            ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="Label11" class="clsLabelAuto">Is For Billing</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsForBilling" runat="server" Checked="<%# mNRC.NRCSpares.CurrentItem.IsForBilling %>"
                                                            ToolTip="Check if this is for Billing" CssClass="clsCheckBox"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" TextMode="MultiLine"
                                                            ClientIDMode="Static" Text="<%# mNRC.NRCSpares.CurrentItem.Remark %>" ToolTip="Remark"
                                                            MaxLength="500"></asp:TextBox>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlAddButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table9" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to Add Spare"
                                                        CausesValidation="true" ValidationGroup="b"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close  screen"
                                                        CausesValidation="false"></asp:Button>
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
    </table>
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" DynamicLayout="false" runat="server">
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
    <asp:HiddenField ID="hdnpartId" runat="server" ClientIDMode="Static" />
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
            if (source._id == "txtSearch_Autocomplete") {
                textbox = document.getElementById('hdnpartId');
            }


            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetPartIdonChange() {
            var popup = $find("txtSearch_Autocomplete");
            var complist = popup.get_completionList();
            var text = $("#txtSearch").val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    var textbox = document.getElementById('hdnpartId');
                    textbox.value = val.toString();
                    return;
                }

            }
            //alert(document.getElementById('hdnpartId').value);
            //document.getElementById('hdnpartId').value = '';
        }
                        
    </script>
    <%--ReleaseNote No autocomplete--%>
    <script type="text/javascript">
        function GetPartID() {
            var partid = document.getElementById('hdnpartId').value.toString();
            if (partid) {
                return partid;
            }
            else {
                return '{00000000-0000-0000-0000-000000000000}';
            }

        }
       
    </script>
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
    <%--End--%>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForNRCSpare();
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
                     parent.IFrameNRCSpareStateComplete();
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
</body>
</html>
