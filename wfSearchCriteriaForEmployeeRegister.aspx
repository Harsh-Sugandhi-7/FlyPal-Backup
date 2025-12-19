<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeRegister.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForEmployeeRegister" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Register Report</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
   
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script id="clientEventHandlersJS" type="text/javascript">

        function openReport() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>

</head>
<body>
    <form id="EmployeeRegisterForm" runat="server">
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
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" 
                                                        runat="server">Employee Register</asp:Label>
                                                </td>

                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Employee</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCrew" runat="server" CssClass="clsLabelAuto">Employee</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSearch" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                            onChange="setEmployeeID(this,'txtSearch_AutocompleteExtender')" />
                                        <cc2:AutoCompleteExtender ID="txtSearch_AutocompleteExtender" runat="server" 
                                            ClientIDMode="Static" DelimiterCharacters="" EnableCaching="false" CompletionInterval="1" 
                                            CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main" 
                                            CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" UseContextKey="false" 
                                            ContextKey="" Enabled="true" MinimumPrefixLength="0" ServicePath="" 
                                            ServiceMethod="GetCrewListAutoComplete" TargetControlID="txtSearch"
                                            OnClientItemSelected="setID" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">
                                            Step II. Selection of Designation</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDesignation" runat="server" CssClass="clsLabelAuto">Designation</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbDesignation" runat="server" 
                                            CssClass="clsTextBoxTagSearchComboNewstyle"
                                            DataTextField="Name" DataValueField="ID" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">
                                            Step III. Selection of Department</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblValue" runat="server" CssClass="clsLabelAuto">Department</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbDepartmentList" runat="server" 
                                            CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                            DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label5" runat="server" CssClass="clsLabelHeader">
                                            Step IV. Select Yes / No for Employee is Working or not</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Label6" class="clsLabelAuto">Employee is Working</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbEmployeeIsWorking" runat="server" 
                                            CssClass="clsTextBoxTagSearchComboNewstyle">
                                            <asp:ListItem Value="2">(ALL)</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label7" runat="server" CssClass="clsLabelHeader">
                                            Step V. Select Yes / No for Employee is on Contract</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span1" class="clsLabelAuto">Contracted Employee</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbContractedEmployee" runat="server" 
                                            CssClass="clsTextBoxTagSearchComboNewstyle">
                                            <asp:ListItem Value="2">(ALL)</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label8" runat="server" 
                                            CssClass="clsLabelHeader">Step VI. Selection of Crew Type</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="SpanCrew" class="clsLabelAuto">Crew Type</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbCrewSelection" runat="server" 
                                            CssClass="clsTextBoxTagSearchComboNewstyle">
                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                            <asp:ListItem Value="1">Flight Crew</asp:ListItem>
                                            <asp:ListItem Value="2">Technical Staff</asp:ListItem>
                                            <asp:ListItem Value="3">Others</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <asp:PlaceHolder runat="server" ID="phSkillSelection"
                                    Visible='<%# IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>' >

                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep7" runat="server"
                                                CssClass="clsLabelHeader"
                                                Text="Step VII. Selection Of Skills" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblEmployeeSkills" CssClass="clsLabelAuto" Text="Skills" />
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="cmbEmployeeSkills"
                                                CssClass="clsTextBoxTagSearchComboNewstyle"
                                                DataTextField="Name" DataValueField="ID" />
                                        </td>
                                    </tr>

                                </asp:PlaceHolder>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader" 
                                            Text='<%# IIf(AppSettings("ShowMaintenanceForNewClients") = "True", 
                                            "Step VIII. Selection of Type", 
                                            "Step VII. Selection of Type") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdbSummary" runat="server" Checked="True" CssClass="clsRadioButton"
                                            GroupName="a" Text="Summary" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdbDetail" runat="server" CssClass="clsRadioButton" GroupName="a"
                                            Text="Detail" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblStepIV" runat="server" 
                                                                CssClass="clsLabelHeader" 
                                                                Text='<%# IIf(AppSettings("ShowMaintenanceForNewClients") = "True",
                                                                "Step IX. Display Report",
                                                                "Step VIII. Display Report") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSummary" runat="server" 
                                                                CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCrewSelection" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDesignationSelection" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblIsEmployeeWorking" runat="server"
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDepartmantSelection" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblIsContractedEmployee" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFlyingOrTechnicalCrew" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmployeeSkillsSelection"
                                                                CssClass="clsLabelAuto" Visible="False" />
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
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" 
                                                                runat="server" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Click To display Current Searching criterias." 
                                                                Text="Current Criteria" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH clsinfoH1" 
                                                                Text="Export To Excel"
                                                                ToolTip="Click To Export report" 
                                                                Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" TabIndex="0" runat="server"
                                                                CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Click to Display Report" 
                                                                Text="Display" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" 
                                                                CssClass="clsbtnH clsinfoH1" 
                                                                ToolTip="Click to close Employee Register screen"
                                                                Text="Close" CausesValidation="False" />
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="SelectedCrewID" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="InccorectEmployee" />

        </div>

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
                if (source._id == "txtSearch_AutocompleteExtender") {
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

                        if (extenderid == "txtSearch_AutocompleteExtender") {
                            textbox = document.getElementById('SelectedCrewID');
                        }

                        textbox.value = val;
                        return;

                    }

                }

                if (extenderid == "txtSearch_AutocompleteExtender") {
                    document.getElementById('SelectedCrewID').value = '';
                    document.getElementById('InccorectEmployee').value = text;
                }
            }

        </script>
    </form>
</body>
</html>
