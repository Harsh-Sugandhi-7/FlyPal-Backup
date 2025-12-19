<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDocumentLockerList_Ajax.aspx.vb"
    Inherits="Flypal.wfDocumentLockerList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Document Attachments</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />

    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link id="Link1" type="text/css" rel="stylesheet" />
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>

</head>
<body>
    <form id="frmDocumentManagement" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <script type="text/javascript">

            function showProgress() {

                if (Page_ClientValidate("a")) {  //a is ValidationGroupName

                    var updateProgress = $get("<%= UpdateProgress.ClientID %>");
                    updateProgress.style.display = "block";

                }

            }

        </script>

        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader" Text="Document Locker" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1"
                                            runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="a" />
                                        <asp:RequiredFieldValidator ID="rfvCode" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtFileName" Display="None"
                                            ErrorMessage="Document Name Required" ValidationGroup="a" />
                                        <asp:CustomValidator ID="cvFilkeAttach" runat="server" CssClass="clsLabelAuto"
                                            OnServerValidate="CustomValidate"
                                            Display="None" ControlToValidate="txtFileName" ValidationGroup="a"
                                            ErrorMessage="Select W.O. Date" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="spName1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblName" CssClass="clsLabelAuto" runat="server" Text="Document Name" />
                                                            </td>
                                                            <td>&nbsp;
                                                            <asp:TextBox ID="txtFileName" runat="server" 
                                                                CssClass="clsTextBoxTagSearch" Height="25px"
                                                                ToolTip="Enter Document Name" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Text="Category" />
                                                            </td>
                                                            <td>&nbsp;
                                                            <asp:DropDownList ID="cmbCategory" runat="server"
                                                                CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                                DataTextField="Name" DataValueField="ID">
                                                            </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td rowspan="2">
                                                    <img src="images/pngwing.com.png" height="150" width="150" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDocumentType" CssClass="clsLabelAuto"
                                                                            runat="server" Text="Document Type" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbPrivate"
                                                                            CssClass="clsRadioButton" 
                                                                            runat="server" Checked="true"
                                                                            Text="Private" GroupName="a" 
                                                                            AutoPostBack="true" />
                                                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                                                        <asp:RadioButton ID="rdbPublic" 
                                                                            CssClass="clsRadioButton" 
                                                                            runat="server" Text="Public"
                                                                            GroupName="a" AutoPostBack="true" />
                                                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                                                        <asp:CheckBox ID="IsPrivate" 
                                                                            runat="server" Text="OTP Required"
                                                                            Enabled="false" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblUser" runat="server" CssClass="clsLabelAuto" 
                                                                            Text="User wise" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbUserwise" CssClass="clsRadioButton" 
                                                                            runat="server" Checked="true"
                                                                            Text="" GroupName="b" AutoPostBack="true" />
                                                                        <asp:ListBox ID="ListDocumentLockerUser" runat="server" 
                                                                            ClientIDMode="Static" SelectionMode="Multiple" 
                                                                            CssClass="clsLabelAuto" DataTextField="Name" 
                                                                            DataValueField="UserID" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto" 
                                                                            Text="Department wise" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbDepartmentwise" CssClass="clsRadioButton"
                                                                            runat="server" Text="" GroupName="b" AutoPostBack="true" />
                                                                        <asp:ListBox ID="ListDepartment" runat="server" ClientIDMode="Static"
                                                                            SelectionMode="Multiple" CssClass="clsLabelAuto" 
                                                                            DataTextField="EmployeeDepartmentName" 
                                                                            DataValueField="EmployeeDepartmentID" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="Label6" runat="server" CssClass="clsLabelAuto"
                                                                            Text="Aircraft wise" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbAircraftWise" CssClass="clsRadioButton"
                                                                            runat="server" Text="" GroupName="b" AutoPostBack="true" />
                                                                        <asp:DropDownList ID="AircraftList" runat="server"
                                                                            CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                            DataValueField="ID" DataTextField="RegNo" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto" 
                                                        Text="Valid Upto" Visible="false" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtexpiryDate" runat="server" ClientIDMode="Static" 
                                                        CssClass="clsTextBoxTagSearch"
                                                        AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                        Text="" Width="100px" Visible="false" />
                                                    <cc2:CalendarExtender ID="txtReceiptDate_CalendarExtender" runat="server" 
                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" 
                                                        TargetControlID="txtexpiryDate" />
                                                    <cc2:TextBoxWatermarkExtender ID="txtReceiptDateWatermarkExtender" runat="server"
                                                        TargetControlID="txtexpiryDate" WatermarkCssClass="clsDateTextBox" 
                                                        WatermarkText="<%$AppSettings:DateFormat%>" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" CssClass="clsLabelAuto" runat="server" 
                                                        Text="Warning Days" Visible="false" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWarningDays" runat="server" ClientIDMode="Static" 
                                                        CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                        ToolTip="Enter Warning Days" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlFileSelection" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <span id="lblInstr" class="clsLabelAuto" 
                                                                            style="color: Brown; font-size: 9px; font-weight: bold; 
                                                                                   font-style: italic">
                                                                            File Size should not be greater than 100 MB. 
                                                                            Do not use special characters in file name. </span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="Span9" class="clsLabelStar">*</span> 
                                                                        <asp:Label runat="server" ID="lblBrowse" 
                                                                            CssClass="clsLabelAuto" Text="Select Document" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="fileUpload1 uploadbtn">
                                                                            <asp:Label runat="server" ID="lblBrowseBtn" 
                                                                                CssClass="clsLabelAuto" Text="Browse..." />
                                                                            <asp:FileUpload ID="FileUpload" 
                                                                                CssClass="clsbtnH clsinfoH1" 
                                                                                runat="server" />
                                                                        </div>
                                                                        <asp:ImageButton ID="Viewfile" Visible="false" runat="server" 
                                                                            Style="height: 20px; width: 13px"
                                                                            ToolTip="View the Documents attached / uploaded."
                                                                            ImageUrl="icons/CLIP01.ICO" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnPrint" runat="server" 
                                                                            CssClass="clsbtnH clsinfoH" 
                                                                            ToolTip="Dispaly Report in PDf format."
                                                                            Text="Print" Visible="false" />
                                                                        &nbsp;&nbsp;
                                                                        <asp:Button ID="btnupload" ValidationGroup="a" Text="Upload" 
                                                                            runat="server" CssClass="clsbtnH clsinfoH"
                                                                            OnClientClick="showProgress()" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnupload" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="upnlFileSelection">
                                                        <ProgressTemplate>
                                                            <div class="overlay">
                                                                <div style="z-index: 1000; margin-left: 350px; 
                                                                            margin-top: 200px; opacity: 1; -moz-opacity: 1;">
                                                                    <asp:Image ID="Image4" runat="server" 
                                                                        ImageUrl="~/images/lock-gif-1.gif" 
                                                                        ImageAlign="Middle"
                                                                        Height="150px" Width="150px" />
                                                                </div>
                                                            </div>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlTargetDepartment" runat="server">
                                                       
                                                        <div style="vertical-align: middle;" class="clsCollapsePnl">
                                                            <div style="float: left;">
                                                                <asp:Label runat="server" ID="lblDepartmentRecCount" CssClass="clsLabelHeader"
                                                                    Text="Search criteria for Document Locker" />
                                                            </div>
                                                            <div style="float: right;">
                                                                <span id="lblMessageDepartment" class="clsLabelHeader"></span>
                                                                <asp:Image ID="imgArrowsDepartment" 
                                                                    Style="vertical-align: middle;" runat="server" />
                                                            </div>
                                                            <div style="clear: both">
                                                            </div>
                                                        </div>

                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlExpandDocumentlock" runat="server" CssClass="clsExpandiblePnl">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFilename" runat="server"
                                                                        CssClass="clsLabelAuto" Text="Document Name" />
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtFileNameSearch" runat="server" Height="25px"
                                                                        CssClass="clsTextBoxTagSearch" ClientIDMode="Static" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCategory" runat="server" 
                                                                        CssClass="clsLabelAuto" Text="Category" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbCategorySearch" runat="server" 
                                                                        CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataTextField="Name" AutoPostBack="true" DataValueField="ID" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label5" runat="server" Text="Department" 
                                                                        CssClass="clsLabelAuto" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDepartmentsearch" runat="server" 
                                                                        CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataTextField="EmployeeDepartmentName" 
                                                                        DataValueField="EmployeeDepartmentID" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblAircraftList" runat="server" Text="Aircraft" 
                                                                        CssClass="clsLabelAuto" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlAircraft" runat="server" 
                                                                        CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataValueField="ID" DataTextField="RegNo" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <cc2:CollapsiblePanelExtender ID="cpeDocumentlocker" runat="Server" 
                                                                        TargetControlID="pnlExpandDocumentlock" Collapsed="true" 
                                                                        ExpandControlID="pnlTargetDepartment" 
                                                                        CollapseControlID="pnlTargetDepartment"
                                                                        AutoCollapse="False" AutoExpand="False" 
                                                                        ScrollContents="false" TextLabelID="lblMessageDepartment"
                                                                        CollapsedText="Show Details..." ExpandedText="Hide Details" 
                                                                        ImageControlID="imgArrowsDepartment"
                                                                        ExpandedImage="~/images/collapse_blue.jpg" 
                                                                        CollapsedImage="~/images/expand_blue.jpg"
                                                                        ExpandDirection="Vertical" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="2">
                                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:ImageButton ID="btnSearchRecords" runat="server"
                                                                                ImageUrl="~/images/Search2.png"
                                                                                ToolTip="Click to search as per Criteria."
                                                                                CausesValidation="false" class="clsSearch2btn" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <fieldset class="clsFieldSetNewStyle">
                                                        <legend class="clsFieldSet1">
                                                            File Attachments
                                                        </legend>
                                                        <asp:UpdatePanel ID="upnlManAttachment" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>

                                                                <asp:GridView ID="dgAttachment" ToolTip="List of File Attachment(s)" 
                                                                    DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                    AllowPaging="True" AutoGenerateColumns="false" PageSize="15"
                                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" runat="server">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" 
                                                                        ForeColor="black" HorizontalAlign="Left" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" 
                                                                        LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black"
                                                                        HorizontalAlign="Right" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="5%"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <%# Container.DataItemIndex + 1 %>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="table_04" HorizontalAlign="Left" />
                                                                            <ItemStyle CssClass="table_02" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
                                                                        <asp:BoundField DataField="Name" HeaderText="Document Name">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CategoryName" HeaderText="Category">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DepartmentName" HeaderText="Department">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="true" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="WarningDays" HeaderText="Warning Days" Visible="false">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Valid upto" 
                                                                            Visible="false">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="IsPublic" HeaderText="Document Type">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="UserName" HeaderText="User">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="false" CssClass="TextBreak" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="false" CssClass="TextBreak" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="View" runat="server" 
                                                                                    CommandArgument='<%# Eval("ID") %>' CommandName="View"
                                                                                    Style="height: 20px; width: 13px" 
                                                                                    ImageUrl="icons/CLIP01.ICO" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" 
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="Delete" runat="server"
                                                                                    CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                                    Style="height: 20px; width: 13px" Text="Delete" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="UserID" HeaderText="UserID"
                                                                            HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
                                <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
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


        <!-- Credential Master --ModalPopUp -->
        <div>

            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyLoginMaster" Text="Dummy Issuing Authority Master" />
            </div>

            <asp:Panel runat="server" ID="pnlLoginMaster" Style="display: none">
                <div>
                    <table class="clstablelistout" id="TABLE7">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlLoginMaster" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="clstablelistin" id="TABLE8">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="valGroup3" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                        ControlToValidate="txtLoginName" Display="None" ErrorMessage="Login Name Required"
                                                        ValidationGroup="valGroup3" />
                                                    <asp:CustomValidator ID="cvLogin" runat="server" CssClass="clsLabelAuto"
                                                        ControlToValidate="txtPassword"
                                                        Display="None" ErrorMessage="Issuing Authority Name too Long."
                                                        OnServerValidate="CustomValidation"
                                                        ValidationGroup="valGroup3" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblInvalid" runat="server" ForeColor="DarkRed" CssClass="clsWidth"
                                                        Style="font-size: 7pt" />
                                                    <span id="Span2" class="clsLabelHeader">Login Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="center">
                                                    <span id="Span3" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="Span4" class="clsLabelAuto">Name</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLoginName" runat="server" CssClass="clsTextBoxTagSearch"
                                                        ToolTip="Enter Login Name" MaxLength="200">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="center">
                                                    <span id="Span1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="Span5" class="clsLabelAuto">Password</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                                        CssClass="clsTextBoxTagSearch"
                                                        ToolTip="Enter Login Password" MaxLength="200">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="3">
                                                    <table id="Table9" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnLoginMaster" TabIndex="0" runat="server"
                                                                    CssClass="clsbtnH clsinfoH"
                                                                    CausesValidation="False" Text="Ok" />
                                                                <asp:Button ID="btnClose" TabIndex="0" runat="server"
                                                                    CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                    Text="Close" />
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

            <cc2:ModalPopupExtender ID="mdlPopUpLoginMaster" runat="server" TargetControlID="btnDummyLoginMaster"
                PopupControlID="pnlLoginMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>

        </div>
        <!-- End -->

        <!-- Credential OTP --ModalPopUp -->
        <div>

            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyOTP" Text="Dummy Issuing Authority Master" />
            </div>

            <asp:Panel runat="server" ID="pnlOTPMaster" Style="display: none">
                <div>
                    <table class="clstablelistout" id="TABLE1">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlOTPMaster" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="clstablelistin" id="TABLE2">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="valGroup3" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        CssClass="clsLabelAuto" ControlToValidate="txtOTP" Display="None"
                                                        ErrorMessage="Login Name Required"
                                                        ValidationGroup="valGroup3" />
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                        ControlToValidate="txtPassword" Display="None"
                                                        ErrorMessage="Issuing Authority Name too Long."
                                                        OnServerValidate="CustomValidation" ValidationGroup="valGroup3" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblOTPInvalid" runat="server" ForeColor="DarkRed" CssClass="clsWidth"
                                                        Style="font-size: 7pt" />
                                                    <span id="Span6" class="clsLabelHeader">OTP Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="center">
                                                    <span id="Span7" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="Span8" class="clsLabelAuto">OTP</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOTP" runat="server" CssClass="clsTextBoxTagSearch"
                                                        Width="185px" ToolTip="Enter OTP" MaxLength="200">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="3">
                                                    <table id="Table3" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOTPOk" TabIndex="0" runat="server"
                                                                    CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                    Text="Ok" />
                                                                <asp:Button ID="btnOTPClose" TabIndex="0" runat="server"
                                                                    CssClass="clsbtnH clsinfoH"
                                                                    CausesValidation="False" Text="Close" />
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

            <cc2:ModalPopupExtender ID="mdlPopupOTPMaster" runat="server" TargetControlID="btnDummyOTP"
                PopupControlID="pnlOTPMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <!-- End -->

        </div>

        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">

            <% Dim openAs As String = Request.QueryString("Type") %>
            <% If openAs IsNot Nothing AndAlso openAs = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameAttachStateComplete();
                }
            });

            <% End if %>

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {

            <% Dim Open As String = Request.QueryString("Type") %>    
            <% If Open IsNot Nothing AndAlso Open = "pup" Then %>  

                ReSetPageLayout();
                onResize();

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

            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
        <%--End--%>

    </form>

    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>

    <script type="text/javascript">

        function disableEnableOnPageLoad() {

            var rdbPublic = document.getElementById("rdbPublic").checked; 
            var rdbPrivatestatus = document.getElementById("rdbPrivate").checked; 

            var rdbUserwise = document.getElementById("rdbUserwise");
            var rdbDepartmentwise = document.getElementById("rdbDepartmentwise");
            var rdbAircraftWise = document.getElementById("rdbAircraftWise");

            var Userwisestatus = document.getElementById("rdbUserwise").checked;
            var Departmentwisestatus = document.getElementById("rdbDepartmentwise").checked;
            var AircraftWiseStatus = document.getElementById("rdbAircraftWise").checked

            if (rdbPublic) {

                $('[id*=ListDocumentLockerUser]').multiselect('clearSelection', true);
                $('[id*=ListDocumentLockerUser]').multiselect('disable', false);

                $('[id*=rdbUserwise').attr('checked', false);
                $('[id*=rdbUserwise').attr('disabled', true);

                $('[id*=rdbDepartmentwise').attr('checked', true);
                $('[id*=ListDepartment]').multiselect('enable', true);

            }

            if (Departmentwisestatus) {

                $('[id*=ListDocumentLockerUser]').multiselect('clearSelection', true);
                $('[id*=ListDocumentLockerUser]').multiselect('disable', false);

                if (rdbPrivatestatus) {
                    document.getElementById("rdbUserwise").disabled = false;
                }
                else {
                    document.getElementById("rdbUserwise").disabled = true;
                }

                document.getElementById("AircraftList").disabled = true;

            }

            if (Userwisestatus) {

                $('[id*=ListDepartment]').multiselect('clearSelection', true);
                $('[id*=ListDepartment]').multiselect('disable', false);

                document.getElementById("AircraftList").disabled = true;
            }

            if (AircraftWiseStatus) {

                $('[id*=ListDepartment]').multiselect('clearSelection', true);
                $('[id*=ListDepartment]').multiselect('disable', false);

                $('[id*=ListDocumentLockerUser]').multiselect('clearSelection', true);
                $('[id*=ListDocumentLockerUser]').multiselect('disable', false);

            }

        }

    </script>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            UserMultiSelect();
            DepartmentMultiSelect();
            disableEnableOnPageLoad();

        });

    </script>

    <script type="text/javascript">

        function UserMultiSelect() {

            $('[id*=ListDocumentLockerUser]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'User',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'User',
                nSelectedText: 'User'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');

        }

        function DepartmentMultiSelect() {

            $('[id*=ListDepartment]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Department',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Department',
                nSelectedText: 'Department'

            });

            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');

        }

    </script>

</body>
</html>

