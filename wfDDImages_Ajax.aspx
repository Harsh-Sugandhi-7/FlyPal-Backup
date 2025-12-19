<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDDImages_Ajax.aspx.vb"
    Inherits="Flypal.wfDDImages_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Upload Images</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clspanel1" runat="server">
                    <table id="tblinner" class="clsTablelistin" border="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Upload Images</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table8" width="100%">
                                            <tr>
                                                <td>
                                                    <span id="spAdd" class="clsLabelAuto">Click To Add New Record </span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="New" ToolTip="Click to add new Image" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="Legend1" runat="server"><b>Image Details</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="clsLabelHeader">Click on select File then click on Save to save the record</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="clsLabelAuto">Set As Home Screen</span>
                                                                            </td>
                                                                            <td colspan="2">
                                                                                <asp:CheckBox ID="chkSetAsHomeScrren" runat="server" CssClass="clsCheckBox" Checked="<%# mDisplayImage.IsSetAsHomeScreen %>" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">Image</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <input type="button" runat="server" id="btnSelectFile" value="Select File" class="clsButton_Ajax" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                    Height="20px" Width="20px"></asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3">
                                                                                <span class="clsLabelHeader">Note : Upload image with 920 X 574 resolution for best view</span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                        <!--Dummy panel to open modelpopup for FileUpload-->
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
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlGridView">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlActionBtnTop">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnSaveTop" runat="server" CssClass="clsButton_Ajax" Text="Save"
                                                                            ToolTip="Click to Save the record" ValidationGroup="1" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                                            ToolTip="Click to close screen" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgImageList" runat="server" AutoGenerateColumns="False" CssClass="clsGrid"
                                                        ShowHeaderWhenEmpty="true">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FileName" HeaderText="File Name">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="true" CssClass="TextBreak" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Set As Home Screen" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSetAsHomeScreen") %>'
                                                                        Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("Size") > 0 %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                        Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlActionBtn">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to Save the record"
                                                        ValidationGroup="1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to close screen" />
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
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IFileUpload" frameborder="0" height="100%" width="100%" allowtransparency="true"
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
                    $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                    $("#IFileUpload").ready(function () {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    });

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
    <!-- End File Upload Modal Dialog-->
    </form>
</body>
</html>
