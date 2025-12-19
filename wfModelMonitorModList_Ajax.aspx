<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfModelMonitorModList_Ajax.aspx.vb"
    Inherits="Flypal.wfModelMonitorModList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Model Directive List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
       
    </script>
    <style type="text/css">
        .maxGridWidth
        {
            max-width: 1250px;
            min-width:400px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Model Directives List</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="2">
                                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                            <asp:CustomValidator ID="cvReason" runat="server" OnServerValidate="customvalidate1"
                                                                Display="None" ControlToValidate="cmbLookIn" ErrorMessage="Reason Required"></asp:CustomValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblSearchCriteria" class="clsLabelHeader">Search Criteria</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbLookIn" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">All</asp:ListItem>
                                                                    <asp:ListItem Value="1">Directive Type</asp:ListItem>
                                                                    <asp:ListItem Value="2">ATA Code</asp:ListItem>
                                                                    <asp:ListItem Value="3">Description</asp:ListItem>
                                                                    <asp:ListItem Value="4">Reference</asp:ListItem>
                                                                    <asp:ListItem Value="5">Directive No.</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFor" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                                    ToolTip="Enter value." MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                                                <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                                    ToolTip="Enter value." MaxLength="4"></asp:TextBox>
                                                                <asp:DropDownList ID="cmbSearchFor" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                    DataTextField="CodeType" DataValueField="ID">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsRII" runat="server" CssClass="clsLabelAuto" ClientIDMode="Static"
                                                                    Text=' Show "Is RII" records' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Find the list of Directives as per searching criteria"
                                                        Text="Find Now" CausesValidation="False"></asp:Button>
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
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Model Directive"
                                                                            CausesValidation="False" Text="Add New"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnConfigureTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Configure N/A Model Directives"
                                                                            CausesValidation="True" Text="Configure N/A"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print List of Model Directive"
                                                                            CausesValidation="False" Text="Print"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                                            CausesValidation="False" Text="Back"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgModelMonitorModList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                                        ShowHeaderWhenEmpty="true" DataKeyNames="ID" AutoGenerateColumns="False" ToolTip="Model Directive List">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                    <%--<asp:CheckBox ID="chkSelectAll" runat="server" CssClass="cbSelectRow" onclick = "checkAll(this);"></asp:CheckBox> --%>
                                                                    <%--<input type="checkbox" id="chkSelectAll" />--%>
                                                                    <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" ToolTip="Select All to Configure N/A Directives."
                                                                        runat="server"></asp:CheckBox>
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectList" runat="server" ToolTip="Select Configure N/A Directives."
                                                                        CssClass="cbSelectRow"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="CodeNumber" SortExpression="CodeNumber" HeaderText="Code/Form No.">
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
                                                                <ItemStyle Wrap="true" CssClass="maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IssueDateFormatted" SortExpression="IssueDateFormatted"
                                                                HeaderText="Issue Date">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Number" SortExpression="Number" HeaderText="Directive No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                 <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Is RII">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkISRII" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsRII") %>'
                                                                        Enabled="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Show In C of A">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkCOfA" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "ShowInCofA") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" CssClass="maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Applicability" HeaderText="Applicability">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="ModelMonitorModType" HeaderText="ModelMonitorModType"></asp:BoundField>
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
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Model Directive"
                                                        CausesValidation="False" Text="Add New"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnConfigure" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Configure N/A Model Directives"
                                                        CausesValidation="True" Text="Configure N/A"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print List of Model Directive"
                                                        CausesValidation="False" Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                        CausesValidation="False" Text="Back"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnModelModMaster" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    <!--Model Mod Master Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyModelModMaster" Text="Model Mod Master" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlModelModMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeModelModMaster" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupModelModMaster" runat="server" TargetControlID="btnDummyModelModMaster"
        PopupControlID="pnlModelModMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameModelModMasterStateComplete() {
            $("#btnDummyModelModMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenModelModMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeModelModMaster").attr("src", "wfModelMonitorMod_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

                if (!$.browser.msie) {
                    $("#btnDummyModelModMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForModelModMaster() {
            var ModelModMasterwindow = $find("<%=mdlPopupModelModMaster.ClientID %>");
            //close Model Mod Master popup window
            ModelModMasterwindow.hide();
            //           release resources
            $("#IframeModelModMaster").attr("src", "JavaScript:''");
            //call Model Mod Master image button
            $("#hdnBtnModelModMaster").click();
        }
    </script>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForModelMonitorModList();
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
             parent.IFrameModelMonitorModListStateComplete();
         }
    });

    <% End if %>
       Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            SetPageLayout();
        }

       function SetPageLayout()
       {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
          ReSetPageLayout();
          onResize();//for Top bottom link
           <% End if %>
       }
       function ReSetPageLayout()
       {
       $("body,html").css({ 'background-color': 'transparent' });
          var tempMargtop=$("body #tblMain:eq(0),html #tblMain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblMain:eq(0),html #tblMain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblMain:eq(0),html #tblMain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <%--End--%>
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgModelMonitorModList tr:gt(0)").each(function () {
                    //   $("#dgModelMonitorModList tr:gt(0)").find(".cbSelectRow").each(function () {
                    // var chkSelectList = $(this).find("[id*=chkSelectList]");
                    if (status == "checked") {
                        //   $(this).attr("checked", status);
                        var chk = $(this).parents("tr").find('input[id*="chkSelectList"]:checkbox')

                        chk.attr('checked', status);
                        SetRow(chk);
                    }
                    else {
                        var chk = $(this).parents("tr").find('input[id*="chkSelectList"]:checkbox')

                        //  chk.removeAttr("checked");
                        //  SetRow(chk);
                        for (var i = 0; i <= chk.length - 1; i++) {
                            if (chk[i].disabled == false) {
                                chk[i].checked = false;
                                var trParent = $(chk[i]).closest('tr');
                                trParent.removeClass('clslightColor')
                            }
                        }
                    }

                });
            });

            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).parents("tr").find('input[id*="chkSelectList"]:checkbox').attr("checked");
                // gets the table row indiect parent
                var trParent = $(this).closest('tr');
                // add or remove the css class according to the check state
                if (checked == "checked")
                    trParent.addClass('clslightColor')
                else
                    trParent.removeClass('clslightColor');
            });

        });
        function SetRow(elem) {
            var status = $(elem).attr("checked");

            // var status = $(elem).prop('checked');
            if (status == "checked") {
                $(elem).closest("tr").addClass('clslightColor');
            }
            else {
                $(elem).closest("tr").removeClass('clslightColor');
            }
        }
    </script>
</body>
</html>
