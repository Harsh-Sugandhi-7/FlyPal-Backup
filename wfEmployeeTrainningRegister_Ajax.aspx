<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeTrainningRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeTrainningRegister_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Trainning Register Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clsFormHeader">Employee Training Register</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStepI" class="clsLabelHeader">Step I. Selection of Employee </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblEmployee" class="clsLabel">Employee </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmployee" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                        onChange="setEmployeeID(this,'txtEmployee_AutocompleteExtender')"></asp:TextBox>
                                    <cc2:AutoCompleteExtender ID="txtEmployee_AutocompleteExtender" runat="server" ClientIDMode="Static" 
                                        DelimiterCharacters="" EnableCaching="false" CompletionInterval="1" CompletionListCssClass="ac_results_Main"
                                        CompletionListHighlightedItemCssClass="ac_over_Main" CompletionListItemCssClass="ac_results_li"
                                        CompletionSetCount="20" UseContextKey="false" ContextKey="" Enabled="true" MinimumPrefixLength="0"
                                        ServicePath="" ServiceMethod="GetCrewListAutoComplete" TargetControlID="txtEmployee"
                                        OnClientItemSelected="setID">
                                    </cc2:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Span1" class="clsLabelHeader">Step II. Selection of Training Details </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblTrainning" class="clsLabelAuto">Training</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbTrainningList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataValueField="ID" DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblTrainningOrg" class="clsLabelAuto">Training Org.</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbTrainningOrgList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataValueField="ID" DataTextField="NameWithCity">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStepIII" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEmployee1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTrainning1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTrainningOrg1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            ToolTip="Click to display Current Searching criterias." Text="Current Criteria">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            ToolTip="Click to Display Report" Text="Display"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="SelectedCrewID" />
     <asp:HiddenField runat="server" ClientIDMode="Static" ID="InccorectEmployee" />
    <%-- Autocomplete functions to set id--%>
    <script type="text/javascript">
        function setID(source, e) {
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
            if (source._id == "txtEmployee_AutocompleteExtender") {
                textbox = document.getElementById('SelectedCrewID');
            }


            textbox.value = value;
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function setEmployeeID(source, extenderid) {
            var popup = $find(extenderid);
            var complist = popup.get_completionList();
            var text = $(source).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                document.getElementById('InccorectEmployee').value = '';
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;

                    if (extenderid == "txtEmployee_AutocompleteExtender") {
                        textbox = document.getElementById('SelectedCrewID');
                    }

                    textbox.value = val;
                    return;
                }

            }

            if (extenderid == "txtEmployee_AutocompleteExtender") {
                document.getElementById('SelectedCrewID').value = '';
                document.getElementById('InccorectEmployee').value = text;
            }
        }
    </script>
    </form>
</body>
</html>
