<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfADSBApplicability.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfADSBApplicability" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AD/SBs Applicability</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="StickyNote/css/style.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="Styles.css" id="Link1" type="text/css" rel="stylesheet" />
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <style type="text/css">
        .clsFieldSet legend
        {
            font-family: Verdana;
            font-size: 13px;
            color: Black;
            font-weight: 500;
            border-style: solid;
            padding: 2 2 2 2;
            margin: 2 2 2 2;
            width: auto; /*   height: auto; vertical-align:middle;*/
            text-align: left;
            margin-left: 10px;
            background-color: WhiteSmoke;
            border-width: 1.8;
        }
    </style>
    <script type="text/javascript">
        function resizeTextBox(txt) {
            txt.style.height = "25px";
            //   txt.style.height = (1 + txt.scrollHeight) + "px";

        }
        function OnResize(txt) {
            $(txt).animate({ width: 185, height: txt.scrollHeight }, "fast");
        }
        function OnLostResize(txt) {
            $(txt).animate({ width: 185, height: 25 }, "fast");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">

        function WaterMark(txt, evt) {
            var defaultText = "Select your prefix";
            if (txt.value.length == 0 && evt.type == "blur") {
                txt.style.color = "gray";
                txt.value = defaultText;
            }
            if (txt.value == defaultText && evt.type == "focus") {
                txt.style.color = "black";
                txt.value = "";
            }
        }
        $(document).ready(function () {
            var txt = document.getElementById("<%=txtADSBTechRecordingText.ClientID%>");
            var defaultText = "Select your prefix";
            if (txt.value.length == 0) {
                txt.style.color = "gray";
                txt.value = defaultText;
            }
        });
   
     
    </script>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain" style="margin-top: 5px; margin-left: 5px;">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <span id="lblTitle" style="font-size: 18px; font-weight: 100" class="text-text-primary clstitle1"
                                            runat="server">List Of AD/SB(s)</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
                                            ValidationGroup="a" ErrorMessage="Select Date" ControlToValidate="txtADSBTechRecordingDate"
                                            Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvADSBNo" runat="server" Display="None" ControlToValidate="txtADSBNO"
                                            ValidationGroup="a" ErrorMessage="AD/SB No. Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" Display="None" ControlToValidate="txtSubject"
                                            ValidationGroup="a" ErrorMessage="Subject Required"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblStatus" runat="server" Text="<%# mADSBTechRecording.StatusName %>"
                                                        Style="margin-right: 5px" CssClass="control-label clsLabelAuto" Font-Bold="true"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upnlADSBTechRecordingDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px" valign="top">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend>Detail </legend>
                                                        <table width="99%" style="margin-top: -22px">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDateStar" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDate" class="control-label clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBTechRecordingDate" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax"
                                                                        Enabled="false" Style="margin-bottom: 5px;" Height="25px" Text="" Width="110px"
                                                                        onchange="ValidateDateText(this,'txtADSBTechRecordingDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtADSBTechRecordingDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtADSBTechRecordingDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtADSBTechRecordingDateWatermarkExtender" runat="server"
                                                                        TargetControlID="txtADSBTechRecordingDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNoStar" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="control-label clsLabelAuto">No.</span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtADSBTechRecordingText" runat="server" Text="<%# mADSBTechRecording.Text %>"
                                                                                    Enabled="false" CssClass="input-sm clsTextBox_Ajax" Height="25px" onfocus="WaterMark(this, event);"
                                                                                    onblur="WaterMark(this, event);" ToolTip="Enter No." MaxLength="25" Width="208px"> </asp:TextBox>
                                                                                <%--   <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                CompletionInterval="1" ServicePath="wfADSBTechRecording_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                TargetControlID="txtADSBTechRecordingText" UseContextKey="False">
                                                            </cc2:AutoCompleteExtender>--%>
                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtADSBTechRecordingText_Autocomplete"
                                                                                    runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0"
                                                                                    CompletionInterval="1" ServicePath="wfADSBTechRecording.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                                    CompletionSetCount="0" TargetControlID="txtADSBTechRecordingText" UseContextKey="False"
                                                                                    ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                                    OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                                    OnClientShowing="ClientShowing">
                                                                                </cc2:AutoCompleteExtender>
                                                                                <script type="text/jscript">
                                                                                    function SetContextKey() {
                                                                                        var autoComplete = $find('txtText_Autocomplete');
                                                                                        var TransTypeID = 'TransTypeID=<%=mADSBTechRecording.TransTypeID%>¿Date=<%=mADSBTechRecording.Date%>';
                                                                                        autoComplete.set_contextKey(TransTypeID);
                                                                                    }
                                                                                </script>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtADSBTechRecordingNo" runat="server" Text="<%# mADSBTechRecording.No %>"
                                                                                    Enabled="false" CssClass="input-sm clsTextBoxSmall_Ajax" Height="25px" MaxLength="8"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span1" class="control-label clsLabelStar"></span>&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <span id="lblADSBNO" class="control-label clsLabelAuto">AD/SB No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtADSBNO" runat="server" CssClass="input-sm clsTextBox_Ajax" ToolTip="AD/SB No."
                                                                        Enabled="false" MaxLength="25" Text="<%# mADSBTechRecording.ADSBNo %>" Style="margin-bottom: 10px;"
                                                                        Width="208px"  height="25px"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp; <span id="Span2" class="control-label clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSubject" class="control-label clsLabelAuto">Subject</span> &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="input-sm clsTextBox3_Ajax"
                                                                        ToolTip="Subject" Enabled="false" MaxLength="25" Text="<%# mADSBTechRecording.ADSBSubject %>"
                                                                        Style="margin-bottom: 10px;" height="25px"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upnlADSBApplicability" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="99%" style="border-width: 1px; margin-left: 5px; margin-right: 5px;"
                                            valign="top;">
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px; margin-top: 10px; margin-left: 5px;">
                                                        <legend>Applicability </legend>
                                                        <table width="99%" valign="top" style="margin-top: -16px">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table valign="top">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp; <span id="lblNature" class="control-label clsLabelAuto">Nature</span> &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbNatureList" runat="server" Height="30px" CssClass="input-sm clsTextBox_Ajax" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                                    Style="margin-bottom: 10px" DataValueField="ID" DataTextField="Name" SelectedValue="<%# mADSBTechRecording.ADSBNatureID %>">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    &nbsp; <span id="Span3" class="control-label clsLabelHeader" style="margin-bottom: 15px">
                                                                        Applicable To</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 60%" valign="top">
                                                                    &nbsp; &nbsp;
                                                                    <asp:RadioButton runat="server" ID="rdbModel" GroupName="app" Checked="<%# mADSBTechRecording.ApplicableToModel %>"
                                                                        AutoPostBack="true" Enabled="<%#  (mADSBTechRecording.ADSBTechRecordingApplicableOns.Count=0) and mADSBTechRecording.ADSBStepsID<=2 %>"
                                                                        Text="Model" Style="margin-top: 10px" />
                                                                </td>
                                                                <td style="width: 40%" valign="top">
                                                                    <table valign="top">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButton runat="server" ID="rdbPart" GroupName="app" Checked="<%# mADSBTechRecording.ApplicableToPart %>"
                                                                                    AutoPostBack="true" Enabled="<%#  (mADSBTechRecording.ADSBTechRecordingApplicableOns.Count=0) and mADSBTechRecording.ADSBStepsID<=2 %>"
                                                                                    Text="Part" />
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 60%" valign="top">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp; &nbsp; <span id="lblAssemblyType" class="control-label clsLabelAuto" runat="server" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                                    style="margin-bottom: 10px" visible="<%#  (mADSBTechRecording.ApplicableToModel) %>">
                                                                                    Assembly Type</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="cmbAssemblyType" Height="30px" CssClass="input-sm clsTextBox_Ajax" 
                                                                                    AutoPostBack="true" Visible="<%#  (mADSBTechRecording.ApplicableToModel) %>"
                                                                                    Style="margin-bottom: 10px" DataValueField="ID" DataTextField="Name" SelectedValue="<%# mADSBTechRecording.AssemblyTypeID %>">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ListBox ID="cmbModelList" runat="server" ClientIDMode="Static" CssClass="input-sm clsTextBox_Ajax" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                                    Width="100%" SelectionMode="Multiple" Height="25px" DataTextField="Name" DataValueField="ID"
                                                                                    Visible="<%#  (mADSBTechRecording.ApplicableToModel) %>"></asp:ListBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 20%" valign="top">
                                                                    <asp:UpdatePanel runat="server" ID="upnlPartList" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:ListBox ID="cmbPartList" runat="server" ClientIDMode="Static" CssClass="input-sm" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                                SelectionMode="Multiple" Height="25px" DataTextField="Name" DataValueField="ID"
                                                                                Visible="<%#  (mADSBTechRecording.ApplicableToPart) %>"></asp:ListBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td align="right" style="width: 20%" valign="top">
                                                                    <asp:ImageButton ID="ImgAddEffectivityDetails" runat="server" CausesValidation="true" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                        Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add Effectivity Detail(s)"
                                                                        Width="24px" />
                                                                </td>
                                                            </tr>
                                                          <%--  <tr>
                                                                <td>
                                                                    &nbsp; <span id="Span7" class="control-label clsLabelHeader" runat="server">Click to
                                                                        add Effectivity Detail(s)</span>
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:GridView ID="dgEffectivityDetails" runat="server" AutoGenerateColumns="False"
                                                                        DataKeyNames="ID" CssClass="table table-striped table-bordered" ShowHeaderWhenEmpty="True">
                                                                        <PagerSettings Mode="NextPreviousFirstLast" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Part" HeaderStyle-ForeColor="black"
                                                                                ItemStyle-HorizontalAlign="Left">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblSparesPartNoStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                    <span id="Span6">Part</span>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:UpdatePanel ID="upnlPartNoValidate" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:CustomValidator ID="cvPartNo" runat="server" ControlToValidate="txtPartNo" SetFocusOnError="true"
                                                                                                CssClass="control-label" Visible="false" ErrorMessage="Enter whole Part" Font-Italic="true"
                                                                                                ForeColor="Red" ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                                            <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                                                                                                CssClass="control-label" Display="dynamic" ErrorMessage="Part No. Required" Font-Italic="true"
                                                                                                ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Part No. Required"
                                                                                                ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:Label ID="lblDuplicatePartNo" runat="server" ForeColor="Red" class="control-label"
                                                                                        Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                
                                                                                           <asp:TextBox ID="txtPartNo" runat="server" CssClass="input-sm clsTextBox_Ajax" MaxLength="200"
                                                                                        Height="25px" AutoPostBack="true" OnTextChanged="txtPart_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"PartName") %>'
                                                                                        ToolTip="Enter Part No." Width="185px" onkeyup="resizeTextBox(this)" onfocus="resizeTextBox(this)"> </asp:TextBox>
                                                                                    <cc2:AutoCompleteExtender ID="txtPartNo_Autocomplete" runat="server" CompletionInterval="1"
                                                                                        CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                        CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                        EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetPartNoList"
                                                                                        UseContextKey="true" ServicePath="wfADSBApplicability.aspx" TargetControlID="txtPartNo">
                                                                                    </cc2:AutoCompleteExtender>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Model" HeaderStyle-ForeColor="black"
                                                                                ItemStyle-HorizontalAlign="Left">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblModelStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                    <span id="Span6">Model</span>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:UpdatePanel ID="upnlModelValidate" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:CustomValidator ID="cvModelNo" runat="server" ControlToValidate="txtModel" SetFocusOnError="true"
                                                                                                CssClass="control-label" Visible="false" ErrorMessage="Enter whole Model" Font-Italic="true"
                                                                                                ForeColor="Red" ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                                            <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="txtModel"
                                                                                                CssClass="control-label" Display="dynamic" ErrorMessage="Model Required" Font-Italic="true"
                                                                                                ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Model Required"
                                                                                                ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:Label ID="lblDuplicateModel" runat="server" ForeColor="Red" class="control-label"
                                                                                        Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                    <asp:TextBox ID="txtModel" runat="server" CssClass="input-sm clsTextBox_Ajax" MaxLength="200"
                                                                                        Height="25px" AutoPostBack="true" OnTextChanged="txtModel_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"ModelName") %>'
                                                                                        ToolTip="Enter Model" Width="185px" onkeyup="resizeTextBox(this)"> </asp:TextBox>
                                                                                    <cc2:AutoCompleteExtender ID="txtModel_Autocomplete" runat="server" CompletionInterval="1"
                                                                                        CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                        CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                        EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetModelList"
                                                                                        UseContextKey="True" ServicePath="wfADSBApplicability.aspx" TargetControlID="txtModel"
                                                                                        OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                        OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                    </cc2:AutoCompleteExtender>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Serial No." HeaderStyle-ForeColor="black"
                                                                                ItemStyle-HorizontalAlign="Left">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblCompSerialNo" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                    <span id="Span6">Serial No.</span>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                  <asp:UpdatePanel ID="upnlSerialNoValidate" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:CustomValidator ID="cvSerialNo" runat="server" ControlToValidate="cmbCompSerialNo"
                                                                                                SetFocusOnError="true" CssClass="control-label" Visible="false" ErrorMessage="Enter whole Serial"
                                                                                                Font-Italic="true" ForeColor="Red" ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:Label ID="lblDuplicateSerialNo" runat="server" ForeColor="Red" class="control-label"
                                                                                        Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                    <asp:DropDownList ID="cmbCompSerialNo" runat="server" DataTextField="SerialNo" DataValueField="SerialNo" 
                                                                                        Height="25px" CssClass="input-sm clsTextBox_Ajax" onchange="CheckDuplicatePart();">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Serial No." HeaderStyle-ForeColor="black"
                                                                                ItemStyle-HorizontalAlign="Left">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblAssemblySerialNo" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                    <span id="Span6">Serial No.</span>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:UpdatePanel ID="upnlAssemblySerialNoValidate" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:CustomValidator ID="cvAssemblySerialNo" runat="server" ControlToValidate="cmbAssemblySerialNo"
                                                                                                SetFocusOnError="true" CssClass="control-label" Visible="false" ErrorMessage="Enter whole Model"
                                                                                                Font-Italic="true" ForeColor="Red" ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:Label ID="lblDuplicateAssemblySerialNo" runat="server" ForeColor="Red" class="control-label"
                                                                                        Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                    <asp:DropDownList ID="cmbAssemblySerialNo" runat="server" DataTextField="SerialNo"
                                                                                        Height="25px" CssClass="input-sm clsTextBox_Ajax" onchange="CheckDuplicateModel();"
                                                                                        DataValueField="SerialNo">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                            <asp:BoundField DataField="PartName" HeaderText="Part" SortExpression="PartName">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="false" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Effective Date" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtEffectiveDate" Height="25px" CssClass="input-sm clsTextBox_Ajax" Width="110px"
                                                                                       Text='<%# DataBinder.Eval(Container.DataItem, "EffectiveDateFormatted") %>'
                                                                                        onchange="ValidateDateText(this,'txtEffectiveDate_CalendarExtender')" AutoPostBack="true"
                                                                                        runat="server"></asp:TextBox>
                                                                                    <asp:UpdatePanel ID="upnlEffectiveDateValidate" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:Label ID="lblEffectiveDate" runat="server" class="clsLabel" Font-Italic="true"
                                                                                                ForeColor="Red" Text=""></asp:Label>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <cc2:CalendarExtender ID="txtEffectiveDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEffectiveDate">
                                                                                    </cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtEffectiveDate" ID="txtEffectiveDate_watermarkextender"
                                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Compliance Period" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtCompliance" runat="server" CssClass="input-sm clsTextBoxMultiLine"
                                                                                        TextMode="MultiLine" Height="25px" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem,"CompliancePeriod") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Remark" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="input-sm clsTextBoxMultiLine"
                                                                                        TextMode="MultiLine" Height="25px" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem,"Remark") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Remove" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                        CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:UpdatePanel runat="server" ID="upnlPreviewReq" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table valign="top" width="90%">
                                                                                <tr>
                                                                                    <td>
                                                                                        &nbsp;
                                                                                        <asp:CheckBox runat="server" ID="chkPreviewReq" class="control-label" Text=" Is Review Board Meeting Required?"  Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                                     Checked="<%# mADSBTechRecording.IsReviewBoardMeetingRequired %>" AutoPostBack="true" />
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp; &nbsp;
                                                                                        <asp:RadioButton runat="server" ID="rdbNormal" class="control-label" Text="Normal" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                                            Visible="<%#  mADSBTechRecording.IsReviewBoardMeetingRequired %>" Checked="<%# not mADSBTechRecording.IsMeetingPriority %>"
                                                                                            GroupName="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                                                                        <asp:RadioButton runat="server" ID="rdbPriority" class="control-label" Text="Priority" Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>'
                                                                                            Visible="<%#  mADSBTechRecording.IsReviewBoardMeetingRequired %>" Checked="<%# mADSBTechRecording.IsMeetingPriority %>"
                                                                                            GroupName="a" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm" Style="height: 100%;
                                            border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px;" Text="Save" CausesValidation="true" ValidationGroup="a"
                                            ToolTip="Click to Save"  Enabled='<%#  mADSBTechRecording.ADSBStepsID<=2  %>' />
                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="btn btn-sm"
                                            Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                            border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-top: 5px;
                                            margin-bottom: 5px; margin-right: 5px;" Text="Close" ToolTip="Click to go back to the previous page" />
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
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
    <script type="text/javascript">
        $(document).ready(function () {

            $("#dgEffectivityDetails tr").each(function () {
                var txtModel = $(this).find("[id*=txtModel]");
                OnLostResize(txtModel);
            });


        });
    </script>
    <%--Date Validations--%>
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
    <!--Duplicates Highlight -->
    <script type="text/javascript">
        function CheckDuplicateModel(sender, args) {
            var grid = document.getElementById("<%=dgEffectivityDetails.ClientID %>");
            var inputs = $('#<%=dgEffectivityDetails.ClientID %>').find('input[id$="txtModel"]');
            var span = $('#<%=dgEffectivityDetails.ClientID %>').find('span[id$="lblDuplicateModel"]');
            var spanSer = $('#<%=dgEffectivityDetails.ClientID %>').find('span[id$="lblDuplicateAssemblySerialNo"]');


            for (var i = 0; i < inputs.length; i++) {
                inputs[i].style.backgroundColor = "";
                span[i].style.display = 'none';
                spanSer[i].style.display = 'none';

            }
            for (var i = 0; i < inputs.length; i++) {
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[i] != inputs[j] && (inputs[i].value != "" || inputs[j].value != "") && inputs[i].value == inputs[j].value) {

                        inputs[i].style.backgroundColor = "Orchid";
                        inputs[j].style.backgroundColor = "Orchid";

                        span[i].style.display = 'block';
                        span[j].style.display = 'block';
                        spanSer[i].style.display = 'block';
                        spanSer[j].style.display = 'block';

                    }

                }
            }
        }
        function CheckDuplicatePart(sender, args) {
            var grid = document.getElementById("<%=dgEffectivityDetails.ClientID %>");
            var inputs = $('#<%=dgEffectivityDetails.ClientID %>').find('input[id$="txtPartNo"]');
            var span = $('#<%=dgEffectivityDetails.ClientID %>').find('span[id$="lblDuplicatePartNo"]');
            var spanSer = $('#<%=dgEffectivityDetails.ClientID %>').find('span[id$="lblDuplicateSerialNo"]');


            for (var i = 0; i < inputs.length; i++) {
                inputs[i].style.backgroundColor = "";
                span[i].style.display = 'none';
                spanSer[i].style.display = 'none';

            }
            for (var i = 0; i < inputs.length; i++) {
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[i] != inputs[j] && (inputs[i].value != "" || inputs[j].value != "") && inputs[i].value == inputs[j].value) {

                        inputs[i].style.backgroundColor = "Orchid";
                        inputs[j].style.backgroundColor = "Orchid";

                        span[i].style.display = 'block';
                        span[j].style.display = 'block';
                        spanSer[i].style.display = 'block';
                        spanSer[j].style.display = 'block';

                    }

                }
            }
        }
      
    </script>
    </form>
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
    <%-- <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
          Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=cmbModelList]').multiselect({
            onDropdownShow: function (event) {
             var i = 1;
              <% For Each item1 In mCustomerList%>
                var menu = $(event.currentTarget).find(".dropdown-menu>li>a");
                <% If  item1.NotInUse ="True" Then%>
               menu[i].style.cssText="font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                i = i + 1;
             <% Next%>
               },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 250,
                nonSelectedText: '(SELECT)',
                selectAllJustVisible: false,
                buttonWidth: '185px'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            });

    </script>--%>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=cmbPartList]').multiselect({

                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 250,
                nonSelectedText: '(SELECT PART)',
                selectAllJustVisible: false,
                buttonWidth: '185px'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=cmbModelList]').multiselect({

                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 250,
                nonSelectedText: '(SELECT MODEL)',
                selectAllJustVisible: false,
                buttonWidth: '185px'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });

    </script>
</body>
</html>
