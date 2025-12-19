<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOModelMaintActivityJobList_Ajax.aspx.vb"
    Inherits="Flypal.wfnWOModelMaintActivityJobList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Model Maintenance Activity Jobs</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script src="json2.js" type="text/javascript"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <style type="text/css">
        .GbiHighlight
        {
            background-color: Aqua;
        }
    </style>
    <!--Added by Saylee on 11-Mar-2014 for ALL11032014-->
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
                var checked = $(element).prop('checked');
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
    <!-- End-->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td align="right">
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table class="clsTablelistin" id="tblinner">
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Model Maintenance Activity Job(s)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" CssClass="clslabelAuto"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset id="fdsAircraftRegInfo" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblAircraftRegDetails" style="font-weight: bold"><b>Search Criteria</b></legend>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabelAuto">ATA</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtATACode" runat="server" CssClass="clsTextBox_Ajax" MaxLength="10"
                                                                ToolTip="Enter ATA Code to search" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span class="clsLabelAuto">Description</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Description to search"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span class="clsLabelAuto">Reference</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Reference to search"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right" valign="top">
                                                <asp:UpdatePanel ID="UpnlFindNow" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                            ToolTip="Click to find as per searching criteria" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td align="right" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpnlResult" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Jobs as per criteria :  Record(s) found.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="UpnlDone" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnDoneTop" runat="server" CssClass="clsButton_Ajax" Enabled="<%# mModelMaintActivityList.Count > 0 %>"
                                                        Text="Done" ToolTip="Click to add checked records" Visible="False" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" Text="Back"
                                                        Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="2">
                                <asp:UpdatePanel ID="UpnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgDueJob" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                        ToolTip="Job." PageSize="5" ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader nodrag nodrop" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                    <input type="checkbox" id="chkSelectAll" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                        <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="MaintActivityTypeName" SortExpression="MaintActivityTypeName"
                                                                HeaderText="Activity">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Right" BorderStyle="Solid" />
                                                        <PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="UpnlDone1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table4" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnDone" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add checked records"
                                                        Enabled="<%# mModelMaintActivityList.Count > 0 %>" Text="Done"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </asp:UpdateProgress>--%>
    </form>
</body>
</html>
