<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAudit_AJAX.aspx.vb"
    Inherits="Flypal.wfAudit_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Conduction</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
       
        //wfrptChangeLocation_Ajax

        //wfRemindersNew_Ajax

        //wfReminderList_Ajax
        //wfDailyStatus_Ajax.aspx

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <table id="tblInner" class="clstablelistin">
                        <tr>

                            <td class="clsFormHeader1">
                                <table width="100%">
                                    <tr>
                                        <td class="clstitle1">
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Audit [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table18" align="right" border="0" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save the Audit Information" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server"   CssClass="clsbtnH clsinfoH" CausesValidation ="false" 
                                                                    TabIndex="0" Text="Close" ToolTip="Click to close Audit screen" />
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
                                <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                            ControlToValidate="txtDescription" ErrorMessage="Description should not be greater than 500 characters."
                                            Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvAuditNo" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtAuditNo" ErrorMessage="Audit No. Required" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtDescription" ErrorMessage="Description Required" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvAuditType" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="cmbAuditTypeList" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvAuditType" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                            ControlToValidate="cmbAuditTypeList" ErrorMessage="Select Audit Type." Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAuditStandard" runat="server" CssClass="clsLabelAuto"
                                            OnServerValidate="customvalidate" ControlToValidate="cmbStandard" ErrorMessage="Select Audit Standard."
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvFrequency" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                            ControlToValidate="txtFrequency" ErrorMessage="Frequency Required" Display="None"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="upnlAuditDet" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdswodetail" class=" clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="ldwodetail" runat="server"><b>Audit Details</b></legend>
                                            <table id="tblinner">
                                                <td valign="top">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAuditNo" runat="server" CssClass="clsLabelAuto">Audit No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAuditNo" runat="server" CssClass=" clsTextBoxTagSearch" MaxLength="100"
                                                                    ToolTip="Enter Audit No." Text="<%# mAudit.AuditNo %>" Width="275px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAuditType" runat="server" CssClass="clsLabelAuto">Audit Type</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbAuditTypeList" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                                    SelectedValue="<%# mAudit.AuditTypeID %>" DataTextField="Name" DataValueField="ID"
                                                                     >
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                    MaxLength="5000" ToolTip="Enter Description" Text="<%# mAudit.Description %>"
                                                                    TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblOtherInformation" runat="server" CssClass="clsLabelLong">Other Information</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOtherInformation" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                    MaxLength="1000" ToolTip="Enter Other Information" Text="<%# mAudit.OtherInformation %>"
                                                                    TextMode="MultiLine" ></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <input type="button" id="btnSelectFile" runat="server" value="Select File" style="width: 100px;"
                                                                                        clientidmode="Static" class="clsbtnH clsinfoH1" />
                                                                                </td>
                                                                                <td style="padding-left: 3px;">
                                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                        Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                                </td>
                                                                                <td style="padding-left: 2px;">
                                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                        Height="24px" Width="15px"></asp:ImageButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAuditStandard" runat="server" CssClass="clsLabel">Audit Standard</asp:Label>
                                                            </td>
                                                            <td>
                                                                <table id="Table11">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlStandard" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbStandard" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mAudit.AuditStandardID %>"
                                                                                        DataTextField="Name" DataValueField="ID" Width="275px">
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imgbtnStandard" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Audit Standard" CausesValidation="False"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblReferenceNo" runat="server" CssClass="clsLabelAuto">Reference No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <table id="Table2">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtReferenceNo" runat="server" CssClass=" clsTextBoxTagSearch" MaxLength="500"
                                                                                ToolTip="Enter Reference No." Text="<%# mAudit.Reference %>" Width="275px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblExePeriod" runat="server" CssClass="clsLabel">Duration</asp:Label>
                                                            </td>
                                                            <td>
                                                                <table id="Table1">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtExePeriod" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                                MaxLength="4" ToolTip="Enter Duration" Text="<%# mAudit.ExePeriod %>" AutoPostBack="True"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">Days</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:UpdatePanel ID="upnlIsNextSchedule" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <fieldset id="Fieldset1" class=" clsFieldSetNewStyle" style="border-width: 1px">
                                                                            <legend id="Legend1" runat="server"><b>
                                                                                <asp:CheckBox ID="chkIsScheduleNextAudit" runat="server" CssClass="clsCheckBox" Text="Schedule Next Audit"
                                                                                    AutoPostBack="True" Checked="<%# mAudit.IsNextSchedule %>"></asp:CheckBox>
                                                                            </b></legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblFrequency" runat="server" CssClass="clsLabel">Frequency</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <table id="Table5" cellspacing="1" cellpadding="1">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtFrequency" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                                                        MaxLength="4" ToolTip="Enter Frequency" Text="<%# mAudit.Frequency %>" onchange="setValues(this);"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblDays" runat="server" CssClass="clsLabelAuto">Months <b>(0 means on condition audit)</b></asp:Label>
                                                                                                </td>

                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlAuditMasterTask" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAuditMasterTask" runat="server" CssClass="clsLabelHeaderItem">Audit Task(s)</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAddTask" runat="server" CssClass="clsbtnH" Text="Add" ToolTip="Click to add Audit Task"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgAuditMasterTask" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                            PageSize="3" Width="100%" ShowHeaderWhenEmpty="true" CellPadding="7" GridLines="Horizontal"> 
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                             <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No." SortExpression="SrNo">
                                                    <HeaderStyle   Width="12px" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AuditCategoryName" HeaderText="Task Category" SortExpression="AuditCategoryName">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:ButtonField CommandName="Edit" HeaderText="Edit" Text="Edit" Visible="False"></asp:ButtonField>
                                                <%--<asp:ButtonField CommandName="RemoveRec" HeaderText="Remove" Text="Remove" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left"></asp:ButtonField>--%>
                                                 <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                           <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="RemoveRec" Style="height: 20px;
                                                                                                width: 20px" ImageUrl="~/images/delete.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnTaskMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                        <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
                                            Style="display: none;" Text="----" />
                                        <asp:Button ID="hdnimgBtnAuditStandard" runat="server" CausesValidation="False" ClientIDMode="Static"
                                            Style="display: none;" Text="----" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%-- <td>
                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table18" align="right" border="0" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" CssClass="clsButton" Text="Save" ToolTip="Click to save the Audit Information" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton"
                                                    TabIndex="0" Text="Close" ToolTip="Click to close Audit screen" />
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div clawftaskss="clsLoad_ajax">
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
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
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
                        //                        $("#IFileUpload").ready(function () {
                        //                            $("#btnDummyFileUpload").click();
                        //                            $get("AjaxLoader").style.visibility = 'hidden';
                        //                        });
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
        <!-- TaskMaster Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyTaskMaster" Text="Dummy TaskMaster" ClientIDMode="Static"
                CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupTaskMaster" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupTaskMaster" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskMaster" runat="server" TargetControlID="btnDummyTaskMaster"
            PopupControlID="pnlPopupTaskMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameTaskMasterStateComplete() {
                $("#btnDummyTaskMaster").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenTaskWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupTaskMaster").attr("src", "wfTaskListForAuditSchedule_AJAX.aspx?Type=pup&AType=3");

                    if (!$.browser.msie) {
                        $("#btnDummyTaskMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var TaskMasterwindow = $find("<%=mdlPopupTaskMaster.ClientID %>");
                //close TaskMaster popup window
                TaskMasterwindow.hide();
                $("#iPopupTaskMaster").attr("src", "JavaScript:''");
                //call TaskMaster image button
                $("#hdnimgBtnTaskMaster").click();
            }
        </script>
        <!-- End-->
        <!-- AuditStandard Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAuditStandard" Text="Dummy AuditStandard"
                ClientIDMode="Static" CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupAuditStandard" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupAuditStandard" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAuditStandard" runat="server" TargetControlID="btnDummyAuditStandard"
            PopupControlID="pnlPopupAuditStandard" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAuditStandardStateComplete() {
                $("#btnDummyAuditStandard").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenStandardWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupAuditStandard").attr("src", "wfAuditStandard_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAuditStandard").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForAuditStandard() {
                var AuditStandardwindow = $find("<%=mdlPopupAuditStandard.ClientID %>");
                //close AuditStandard popup window
                AuditStandardwindow.hide();
                $("#iPopupAuditStandard").attr("src", "JavaScript:''");
                //call AuditStandard image button
                $("#hdnimgBtnAuditStandard").click();
            }
        </script>
        <!-- End-->
    </form>
    <script type="text/javascript">
        function setValues(elem) {
            var text = $get("elem").val;

            if (text == '') {
                $get("elem").val('0');
            }
        }
    </script>
</body>
</html>
