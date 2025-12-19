<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptInspectionWiseStatusReport_Ajax.aspx.vb"
    Inherits="Flypal.wfrptInspectionWiseStatusReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Inspection wise Status Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">

        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).closest('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    trParent.addClass('clslightColor')
                else
                    trParent.removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).attr('checked');
                if (checked == true)
                    $(element).closest('tr').addClass('clslightColor');
                else
                    $(element).closest('tr').removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });


        });

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="600">
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
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <span id="lblEmployeeList" class="clsFormHeader">Inspection wise Status List</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="top: 8px; left: 3px" class="clsFieldSetNewStyle">
                                    <legend><b>Search Criteria</b> </legend>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upblSearch" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblSearchIn" class="clsLabelAuto">Code/Form No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCodeFormNo" runat="server" ToolTip="Enter Code/Form No. to search"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Reference</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtReference" runat="server" ToolTip="Enter Reference to search"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" class="clsLabelAuto">Description</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtDescription" runat="server" ToolTip="Enter Description to search"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span4" class="clsLabelAuto">Inspection Type</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbInspType" runat="server" DataTextField="CodeType"
                                                                        DataValueField="ID" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find the Model Inspection(s) as per searching criteria"
                                                    Text="Find Now"></asp:Button>--%>
                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find the Model Inspection(s) as per searching criteria"/>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="clsLabelHeader">Enter Search Criteria and click on Find Now Button to get
                                    Inspection List.Then select Inspection(s)[Max Allowed 25] to print their Status
                                    across all fleets.</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <fieldset style="top: 8px; left: 3px" class="clsFieldSetNewStyle">
                                                <legend><b>Show Status For</b> </legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="Span5" class="clsLabelAuto">Aircraft</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" ClientIDMode="Static"
                                                                DataTextField="RegNo" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 5px;">
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkTaskCard" Text="With Task Cards" runat="server" CssClass="clsCheckBox" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlAddTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnPrintTop" runat="server"  ToolTip="Click to Print Status of Selected Inspection(s)"
                                                                    Text="Print"></asp:Button>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCloseTop" runat="server" ToolTip="Click to close screen"
                                                                    Text="Close"></asp:Button>
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
                                <asp:UpdatePanel ID="upnlGridTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <td>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            </td>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgMonitorActivityList" runat="server" ToolTip="Inspection List"
                                            DataKeyNames="ID" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"
                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowSorting="True">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"></input>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                    <HeaderStyle ></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                    <HeaderStyle></HeaderStyle>
                                                    <ItemStyle Wrap="true" Width="200px" CssClass="TextBreak" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Show In C of A">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkCOfA" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "ShowInCofA") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours"></asp:BoundField>
                                                <asp:BoundField DataField="Note" HeaderText="Note"></asp:BoundField>
                                                <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="Size" HeaderText="Size"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlAddBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="clstableButton">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Status of Selected Inspection(s)"
                                                        Text="Print"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close screen"
                                                        Text="Close"></asp:Button>
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgMonitorActivityList tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");

                    }
                });
            });
        });
    </script>
    </form>
    <script type="text/javascript">
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
</body>
</html>
