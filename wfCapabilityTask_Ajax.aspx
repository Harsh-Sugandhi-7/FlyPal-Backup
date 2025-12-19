<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCapabilityTask_Ajax.aspx.vb"
    Inherits="Flypal.wfCapabilityTask_Ajax" %>

<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Capability Task</title>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function resizeTextBox(txt) {
            txt.style.height = "1px";
            txt.style.height = (1 + txt.scrollHeight) + "px";

        }
        function OnResize(txt) {
            $(txt).animate({ width: 275, height: txt.scrollHeight }, "fast");
        }
        function OnLostResize(txt) {
            $(txt).animate({ width: 275, height: 16 }, "fast");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
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
                            <asp:UpdatePanel ID="upnlCapabilityTask" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td colspan="2" class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Capability Task[New]</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Capability Task in the list"
                                                                            Text="New" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to save the Capability Task Information"
                                                                            CausesValidation="true" Text="Save"></asp:Button>
                                                                    </td>
                                                                    <td colspan="4" align="right">
                                                                        <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Capability Task screen"
                                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                        
                                                        
                                                    </tr>
                                                </table>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                            ValidationGroup="a"></asp:ValidationSummary>
                                                        <asp:RequiredFieldValidator ID="rfvDesc" runat="server" CssClass="clsLabelauto" Display="None"
                                                            ValidationGroup="a" ControlToValidate="txtTaskDescription" ErrorMessage="Description Required"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvDesc" runat="server" Display="None" ControlToValidate="txtTaskDescription"
                                                            ValidationGroup="a" ErrorMessage="" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <%--<td align="right">
                                                <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to add new Capability Task in the list"
                                                    Text="New" CausesValidation="False"></asp:Button>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                    <legend><b>Capability Task Details</b></legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCapabilityStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCapability" runat="server" CssClass="clsLabel">Capability</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCapability" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="Name"
                                                                    DataValueField="ID" Width="330px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Task</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTaskDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                    Width="320px" ToolTip="Enter Description" MaxLength="1000" TextMode="MultiLine">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="lblHSNACS" class="clsLabel" runat="server" 
                                                                    visible='<%# IIf(AppSettings("HSNACSCodeVisibleInCapabilityTaskMaster") = "True", True, False) %>'
                                                                    >HSN/SAC </span>
                                                            <td>

                                                                <asp:DropDownList ID="cmbHSNACSList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                    DataTextField="Code" onChange="setComboBoxValue(this,'HSNACS')"
                                                                    Visible='<%# IIf(AppSettings("HSNACSCodeVisibleInCapabilityTaskMaster") = "True", True, False) %>'
                                                                    >
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <%--<td align="right">
                                                <asp:Button ID="btnSave" CssClass="clsButton" runat="server" ToolTip="Click to save the Capability Task Information"
                                                    CausesValidation="true" Text="Save"></asp:Button>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="dgCapabilityTask" runat="server" AllowPaging="true" AllowSorting="true"
                                                    AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="10" ShowHeaderWhenEmpty="true">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                        NextPageText="" PreviousPageText="" />
                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>

                                                        <asp:BoundField DataField="CapabilityName" HeaderText="Capability" SortExpression="CapabilityName">
                                                            <HeaderStyle  HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TaskDescription" HeaderText="Description" SortExpression="TaskDescription">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" CssClass="TextBreak" Wrap="true"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code" SortExpression="HSNACSCode">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" CssClass="TextBreak"/>
                                                        </asp:BoundField>
                                                        <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                    CommandName="ViewRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                    CausesValidation="false" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("ID") %>'
                                                                    CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>


                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>--%>
                                                                <div class="dropdown">
                                                                    <div id="divd" class="dropdownbtn-content" runat="server">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                        CommandName="ViewRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>' CausesValidation="false"
                                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                        Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td colspan="4" align="right">
                                                <asp:Button ID="btnClose" CssClass="clsButton" runat="server" ToolTip="Click to close Capability Task screen"
                                                    Text="Close" CausesValidation="False"></asp:Button>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
                runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                    </div>
                    <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                    Height="48px" Width="48px" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <%-- <script type="text/javascript">

        function ValidateName(source, args) {
            args.IsValid = false;
            var Nametxt = document.getElementById("txtTaskDescription").value;
            var len = Nametxt.length;
            if (len < 50) {
                args.IsValid = true;
                return;
            }
        }
    </script>--%>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForCapabilityTask();
                return false;
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameCapabilityTaskStateComplete();
                }
            });

    <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
           <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
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
        <asp:HiddenField ID="HSNACSValue" runat="server" ClientIDMode="Static" />
        <script type="text/javascript">
            function setComboBoxValue(elem, combo) {
                switch (combo) {
                    //HSNACS                                                                                                                                                                                                                         
                    case 'HSNACS':
                        var id = $(":selected", elem).val();
                        //set id to hidden field
                        $("#HSNACSValue").val(id);
                        break;

                }
            }
        </script>
    </form>
</body>
</html>
