<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPart_AJAX.aspx.vb" Inherits="Flypal.wfPart_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Part</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css"    />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
        function openReport() {
            str = "frmshowreport.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body ms_positioning="GridLayout" topmargin="5" bottommargin="5" leftmargin="5" rightmargin="5">
    <form id="wfgroup" method="post" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
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
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clsTableListIn" id="tblInner">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnltitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Part [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                        </asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvPart" runat="server" CssClass="clsLabel" ErrorMessage="Part Number Required"
                                            Display="None" ControlToValidate="txtPartNo"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clsLabel"
                                            ErrorMessage="Description Required" Display="None" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" ErrorMessage="Max Lenght of Description should be 200 chars."
                                            Display="None" ControlToValidate="txtDescription" OnServerValidate="customvalidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="fdsPartInfo" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblPartDetails" runat="server" style="font-weight: bold"><b>Part Details</b></legend>
                                    <asp:UpdatePanel ID="upnlPartOnfo" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" colspan="3">
                                                        <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="New"
                                                                    ToolTip="Click to add the new part" CausesValidation="False"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblPartNoStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                        Text="<%# mPart.Name %>" ToolTip="Enter Part Number"></asp:TextBox>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lblDescriptionStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox1_Ajax" MaxLength="200" Width="300px"
                                                                        Text="<%# mPart.Description %>" ToolTip="Enter Description of the part."></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save the Part Information" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="fdsSearch" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblSearch" runat="server" style="font-weight: bold"><b>Search by Name and
                                        Description</b></legend>
                                    <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPlaceName" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPartNoSearch" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                        ToolTip="Enter Part Number"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSearchbyDes" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSearchDesc" runat="server" CssClass="clsTextBox1_Ajax" MaxLength="200" Width="300px"
                                                                        ToolTip="Enter Description"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <table id="Table1" align="right" border="0" cellpadding="0" cellspacing="0" height="100%">
                                                            <tr>
                                                                <td align="right" valign="bottom">
                                                                    <asp:Button ID="btnFindNow" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                        Text="Find Now" ToolTip="Click to find the list of parts as per the searching criteria" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:UpdatePanel ID="upnlCloseTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                    Text="Close" ToolTip="Click to close Part screen" Visible="<%# mPartList.Count >25 %>" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="dgPart" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                    ShowHeaderWhenEmpty="true" CssClass="clsGrid" ToolTip="List of parts." Width="100%">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle CssClass="hideGridColumn" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Name" HeaderText="Part No" SortExpression="Name">
                                                                            <HeaderStyle ForeColor="#FFFFFF" Wrap="False" HorizontalAlign="Left" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                            <HeaderStyle ForeColor="#FFFFFF" Wrap="False" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:ButtonField CommandName="ViewRec" HeaderText="Edit/View" Text="Edit/View">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
                                                                        <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="bottom" colspan="3">
                                                        <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                    Text="Close" ToolTip="Click to close Part screen" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                        </tr>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForPart();
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
             parent.IFramePartStateComplete();
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <%--End--%>
    </form>
</body>
</html>
