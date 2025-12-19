<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSearchPartList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptSearchPartList_Ajax" EnableEventValidation="false" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
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
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Part List</span>
                                        </td>

                                       <%-- <td colspan="2" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server"
                                                                    Text="Current Criteria" ToolTip=" Click to display current searching criterias"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" runat="server" Text="Export to Excel"
                                                                    ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" Text="Display"
                                                                    ToolTip="Click to display report"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close" ToolTip="Click to Close Part List screen"
                                                                    CausesValidation="False"></asp:Button>
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
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Category</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblCategory" class="clsLabelAuto">Category</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategory" runat="server" DataValueField="ID"
                                    DataTextField="Name" EnableViewState="false" onChange="setComboBoxValue(this,'Category')">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span id="lblStep3" class="clsLabelHeader">Step II. Selection of ATA</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblATA" class="clsLabelAuto">ATA</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbATAChapter" runat="server" 
                                    DataValueField="ID" DataTextField="ATAChapter">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep4" class="clsLabelHeader">Step III. Selection of Part 
                                Number/Description</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblSearch" class="clsLabelAuto">Search</span>
                            </td>
                            <td align="left">
                                <asp:TextBox cssclass="clsTextBoxSearch_Ajax" ID="txtPartDescription" runat="server" 
                                    ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="Span1" class="clsLabelHeader">Step IV. Selection of Model 
                                Applicability</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblModel" class="clsLabelAuto">Model </span>
                            </td>
                            <td>
                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtModelList" runat="server"></asp:TextBox>
                                <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelList"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                    CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                </cc2:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep5" class="clsLabelHeader">Step V. Format</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblFormat" class="clsLabelAuto">Format</span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel runat="server" ID="upnlFormat" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true">
                                            <asp:ListItem Value="0">Format 1</asp:ListItem>
                                            <asp:ListItem Value="1">Format 2</asp:ListItem>
                                            <asp:ListItem Value="2">Format 3</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAlternetPartListOnly" runat="server" CssClass="clsLabelHeader"
                                            Text="Alternate Part List Only" Visible="false"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:UpdatePanel runat="server" ID="upnllblStep5" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lblStep6" Class="clsLabelHeader" Text="Step VI. Sort By"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel runat="server" ID="upnlSortBySpan" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lblSortBy1" Class="clsLabelAuto" Text="Sort By"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel runat="server" ID="upnlSortBy" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSortBy" runat="server">
                                            <asp:ListItem Value="0">Part No.</asp:ListItem>
                                            <asp:ListItem Value="1">Description</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="Label2" Class="clsLabelHeader" Visible="false" Text="Step VII. Essential Category"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label1" Class="clsLabelAuto" Visible="false" Text="Essential Catagory"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbEssentialCatagory" Visible="false" runat="server">
                                    <asp:ListItem Text="All" Value="-1"> All</asp:ListItem>
                                    <asp:ListItem Text="Go" Value="0"> Go</asp:ListItem>
                                    <asp:ListItem Text="No Go" Value="1">No Go</asp:ListItem>
                                    <asp:ListItem Text="Go If" Value="2">Go If</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                <asp:Label runat="server" ID="lblIsOneTimePurchase" Class="clsLabelHeader" Text="Step VIII. Selection For Is One Time Purchase"></asp:Label>
                             </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label4" Class="clsLabelAuto" Text="Is One Time Purchase"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsOTP" runat="server" CssClass="clsCheckBox"  />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:UpdatePanel runat="server" ID="upnllblStep6" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lblStep7" Class="clsLabelHeader" Text="Step IX. Display Report"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblNomenclatureName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblATAChapter" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblModelCurrentCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                        Text="Current Criteria" ToolTip=" Click to display current searching criterias">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" Text="Export to Excel"
                                                        ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" Text="Display"
                                                        ToolTip="Click to display report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" Text="Close" ToolTip="Click to Close Part List screen"
                                                        CausesValidation="False"></asp:Button>
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
    <asp:HiddenField ID="hdnNomenclatureName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCategoryName" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        function setComboBoxValue(elem, combo) {
            switch (combo) {

                case 'Nomenclature':
                    if (($(":selected", elem)[0].index) != 0) {
                        name = $(":selected", elem).text();
                        $("#hdnNomenclatureName").val(name);
                    }
                    else {
                        $("#hdnNomenclatureName").val('');
                    }
                    break;
                case 'Category':
                    if (($(":selected", elem)[0].index) != 0) {
                        var name = $(":selected", elem).text();
                        $("#hdnCategoryName").val(name);
                    }
                    else {
                        $("#hdnCategoryName").val('');
                    }
                    break;
            }

        }
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtPartDescription.ClientID%>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                max: 100,
                delay: 0
            });
        });
    </script>
    </form>
</body>
</html>
