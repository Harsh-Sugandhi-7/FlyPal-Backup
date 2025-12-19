<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskCardTools_Ajax.aspx.vb"
    Inherits="Flypal.wfTaskCardTools_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Tools Detail</title>
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
        <div>
            <table id="tblmain" class="clstablelistout" border="0" cellspacing="1" cellpadding="1"
                width="300">
                <tr>
                    <td colspan="5">
                        <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                            <tr>

                                <td class="clsFormHeader1" colspan="2">
                                <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> Task Card Tools Detail</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="True"
                                                                        ValidationGroup="a" Text="OK" ToolTip="Click to Add Tool"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to go back to the previous screen"
                                                                        Text="Back"></asp:Button>
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
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvPart" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Part"
                                                ControlToValidate="txtItemList" Display="None" OnServerValidate="customvalidate"
                                                ValidationGroup="a"></asp:CustomValidator><asp:CustomValidator ID="cvDescription"
                                                    runat="server" CssClass="clsLabelAuto" ErrorMessage="Description must not be greater than 150 characters."
                                                    ControlToValidate="txtDescription" Display="None" OnServerValidate="customvalidate"
                                                    ValidationGroup="a"></asp:CustomValidator><asp:CustomValidator ID="cvReqty" runat="server"
                                                        CssClass="clsLabelAuto" ErrorMessage="Qty.Required" ControlToValidate="txtReqQty"
                                                        Display="None" OnServerValidate="customvalidate" ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                Display="None" ControlToValidate="txtRemark" ErrorMessage="Remark required" ValidationGroup="a"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlToolDetail" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdswodetail" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                                <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Tool Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblWO" runat="server" CssClass="clsLabelAuto">Task Card No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTaskCardNo" runat="server" CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0"
                                                                MaxLength="150" ReadOnly="True" Width="320px" ToolTip="Task Card No."></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblItemNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <%--  <asp:DropDownList ID="cmbItemList" runat="server" CssClass="clsComboBox_Ajax" SelectedValue="<%# mTaskCard.TaskCardTools.CurrentItem.ItemID %>"
                                                            DataTextField="Name" DataValueField="ID" AutoPostBack="True">
                                                        </asp:DropDownList>--%>
                                                            <asp:TextBox ID="txtItemList" autocomplete="off" runat="server" CssClass="clsTextBoxTagSearch" Width="320px"
                                                                AutoPostBack="True" onchange="SetPartIdonChange(this,'txtItemList_AutoCompleteExtender')"></asp:TextBox>
                                                            <!-- AutoComplete Extender-->
                                                            <cc2:AutoCompleteExtender ID="txtItemList_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
                                                                ServicePath="" ServiceMethod="GetItemList" TargetControlID="txtItemList" UseContextKey="True"
                                                                ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetID">
                                                            </cc2:AutoCompleteExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblPartNo1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mTaskCard.TaskCardTools.CurrentItem.PartNo %>"
                                                                Enabled="False"></asp:TextBox><asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">(Type Here If Part No. does not exist in above list)</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                Text="<%# mTaskCard.TaskCardTools.CurrentItem.Description %>" BackColor="White"
                                                                TextMode="MultiLine" MaxLength="200" ToolTip="Description" Width="400px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="margin-left: 40px">
                                                            <asp:Label ID="lblReqQty1" runat="server" CssClass="clsLabelStar"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblReqQty" runat="server" CssClass="clsLabelAuto">Required Qty.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReqQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                Text="<%# mTaskCard.TaskCardTools.CurrentItem.RequiredQty %>" MaxLength="5" ToolTip="Enter Required Quantity"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                Text="<%# mTaskCard.TaskCardTools.CurrentItem.Remark %>" TextMode="MultiLine"
                                                                MaxLength="500" ToolTip="Enter Remark" Width="400px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td colspan="3" align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" CausesValidation="True"
                                                        ValidationGroup="a" Text="OK" ToolTip="Click to Add Tool"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to go back to the previous screen"
                                                        Text="Back"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>
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
        </div>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForTaskCardTool();
                return false;
            }
        </script>
        <%--End--%>
        <div>
            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameTaskCardToolStateComplete();
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
        <%-- Autocomplete functions to set id--%>
        <asp:HiddenField ID="hdnpartId" runat="server" ClientIDMode="Static" />
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
                //Set id to relevent hidden field 
                var textbox;
                if (source._id == "txtItemList_AutoCompleteExtender") {
                    textbox = document.getElementById('hdnpartId');
                }
                textbox.value = value;
            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetPartIdonChange(source, extenderid) {
                var popup = $find(extenderid);
                var complist = popup.get_completionList();
                var text = $(source).val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;

                        if (extenderid == "txtItemList_AutoCompleteExtender") {
                            textbox = document.getElementById('hdnpartId');
                        }
                        textbox.value = val;
                        return;
                    }

                }

                if (extenderid == "txtItemList_AutoCompleteExtender") {
                    document.getElementById('hdnpartId').value = '';
                }
            }

        </script>
    </form>
</body>
</html>
