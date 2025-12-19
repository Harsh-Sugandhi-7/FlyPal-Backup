<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDDUpComingEvents_Ajax.aspx.vb"
    Inherits="Flypal.wfDDUpComingEvents_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>UpComing Event Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
                                <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">UpComing Event Details [New]</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                        Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                    <asp:RequiredFieldValidator ID="rfvEnquiryDate" runat="server" CssClass="clsLabelAuto"
                                                        ErrorMessage="Event Date Required" ControlToValidate="txtEventDate" Display="None"
                                                        ValidationGroup="1"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                        ErrorMessage="Event Details Required" ControlToValidate="txtEventDetails" Display="None"
                                                        ValidationGroup="1"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtEventDetails"
                                                        Display="None" ErrorMessage="Event Details length must not be greater than 1000 Characters"
                                                        ClientValidationFunction="validateDetails" ValidationGroup="1"></asp:CustomValidator>
                                                    <script type="text/javascript">
                                                        function validateDetails(source, args) {
                                                            var Value = $get("txtEventDetails").value.trim().length;
                                                            if (Value > 1000) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                        }
                                                    </script>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="spAdd" class="clsLabelAuto">Click To Add New Record </span>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="New" ToolTip="Click to add new Event" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="Legend1" runat="server"><b>Event Details</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblIssueDate" class="clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEventDate" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                                                        onchange="ValidateDateText(this,'EventDateWatermarkExtender','false');" Text=""
                                                                        Width="100px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="EventDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEventDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="EventDateWatermarkExtender" runat="server" TargetControlID="txtEventDate"
                                                                        WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblReferenceNo" runat="server" CssClass="clsLabelAuto">Details</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEventDetails" runat="server" CssClass="clsTextBoxMultiLine3_Ajax"
                                                                        Text="<%# mUpcomingEvent.EventDesc %>" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">To Show</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkToShow" CssClass="clsCheckBox" runat="server" Checked="<%# mUpcomingEvent.InfoToShow %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <table border="0">
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgEventDetails" runat="server" AutoGenerateColumns="False" CssClass="clsGrid"
                                                        AllowPaging="true" PageSize="15" ShowHeaderWhenEmpty="true">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="EventDateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EventDesc" HeaderText="Events">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="true" Width="600px" CssClass="TextBreak" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="To Show" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "InfoToShow") %>'
                                                                        Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
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
                                            <tr>
                                                <td align="right" colspan="2">
                                                    <table border="0">
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
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
    </form>
</body>
</html>
