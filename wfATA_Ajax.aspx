<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfATA_Ajax.aspx.vb" Inherits="Flypal.wfATA_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>ATA</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<style type="text/css">
    #divGvSubATAList {
        max-height: 150px;
        overflow-y: auto;
        overflow-x: hidden;
        width: 818px;
    }
</style>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="5" class="clsFormHeader1Newstyle" style="width: 450px">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">
															ATA [ New ]
                                                        </asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH"
                                                    ToolTip="Click to add New ATA"
                                                    Text="New" CausesValidation="False"></asp:Button>
                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server"
                                                    ToolTip="Click to save the ATA Information"
                                                    Text="Save" ValidationGroup="valGrpParent"></asp:Button>
                                                <asp:Button ID="btnClose" runat="server" CausesValidation="False"
                                                    CssClass="clsbtnH clsinfoH" Text="Close"
                                                    ToolTip="Click to close ATA Chapter screen" />
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="valGrpParent" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvCode" runat="server" CssClass="clsLabelAuto" ErrorMessage="Code should be Numeric and Greater than Zero"
                                                Display="None" ControlToValidate="txtATACode" OnServerValidate="customvalidate"
                                                ValidationGroup="valGrpParent" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvChapter" runat="server" CssClass="clsLabelAuto" ErrorMessage="Chapter should not be greater than 50 characters."
                                                Display="None" ControlToValidate="txtATANomenclature" OnServerValidate="customvalidate"
                                                ValidationGroup="valGrpParent" ValidateEmptyText="true"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                                <td align="right"></td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                                <td align="right"></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblATADetails" class="clsLabelHeader">ATA Details</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlATADetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td width="12px">
                                                        <span id="lblCode1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td width="50px">
                                                        <span id="lblATACode" class="clsLabelAuto">Code </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtATACode" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                            ToolTip="Enter Code" Text="<%# mATA.DispATACode %>" MaxLength="4" Width="50px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" width="12px">
                                                        <span id="lblChapter1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td valign="top" width="50px">
                                                        <span id="lblATANomenclature" class="clsLabelAuto">Chapter</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtATANomenclature" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Chapter"
                                                            Text="<%# mATA.ATANomenclature %>" MaxLength="50">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSubATALink" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblSubATA" runat="server" CssClass="clsLabelAuto" Visible="False">Sub ATA</asp:Label>
                                            <asp:LinkButton ID="lnkSubATACount" runat="server" CssClass="clsLinkButton" Visible="False"></asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td valign="bottom" align="right"></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblSearchHeader" class="clsLabelHeader">Search by Chapter</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td width="12px"></td>
                                            <td width="50px">
                                                <span id="lblSearch" class="clsLabelAuto">Chapter </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Chapter"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
                                                ToolTip="Click to search the list of ATA as per searching Criteria"
                                                ValidationGroup="1" CausesValidation="false" class="clsSearch2btn" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgATAList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            CssClass="clsGridNewStyle" AllowPaging="True" PageSize="10" ShowHeaderWhenEmpty="true"
                                                            GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                <asp:BoundField DataField="DispATACode" HeaderText="Code" SortExpression="DispATACode"></asp:BoundField>
                                                                <asp:BoundField DataField="ATANomenclature" HeaderText="Chapter" SortExpression="ATANomenclature">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="SubATA" DataTextField="SubATACountDisp" HeaderText="Add / Edit Sub ATA"></asp:ButtonField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <div id="dropDownImg" class="dropdown">
                                                                            <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png"
                                                                                runat="server" CssClass="clsActionbtn" />
                                                                            <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
                                                                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                ToolTip="Click to Edit record"
                                                                                                CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="deleteICN" class="actionICNS largerActionICNS" runat="server"
                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                ToolTip="Click to Delete record"
                                                                                                CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
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
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        <!-- SubATA Modal PopUp -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySubATA" Text="Dummy Sub ATA" />
        </div>
        <asp:Panel runat="server" ID="pnlPopUp" Style="display: none">
            <div>
                <table class="clstablelistout" id="Table1">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upnlSubATA" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="TABLE2" class="clstablelistin">
                                        <tr>
                                            <td colspan="4" class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTitleSubATA" CssClass="clsFormHeader" runat="server">
																Sub ATA [ New ]
                                                            </asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSaveSubATA" CssClass="clsbtnH clsinfoH" runat="server"
                                                                ToolTip="Click to save the Sub ATA Information"
                                                                Text="Save" ValidationGroup="valGrpChild"></asp:Button>
                                                            <asp:Button ID="btnCloseSubATA" CssClass="clsbtnH clsinfoH" runat="server"
                                                                ToolTip="Click to close Sub ATA Chapter screen"
                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"
                                                    ValidationGroup="valGrpChild" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                    ControlToValidate="txtSubATACode" Display="None" ErrorMessage="Sub ATA Code should be Numeric and Greater than Zero"
                                                    OnServerValidate="CustomValidate1" ValidateEmptyText="true" ValidationGroup="valGrpChild"></asp:CustomValidator>
                                                <asp:CustomValidator ID="Customvalidator2" runat="server" CssClass="clsLabelAuto"
                                                    ControlToValidate="txtSubATAChapter" Display="None" ErrorMessage="Sub ATA Chapter Should not be greater than 50 characters."
                                                    OnServerValidate="CustomValidate1" ValidateEmptyText="true" ValidationGroup="valGrpChild"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDescription"
                                                    Display="None" ErrorMessage="Description Should not be greater than 50 characters."
                                                    OnServerValidate="CustomValidate1" ValidationGroup="valGrpChild"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <span id="Label2" class="clsLabelHeader">Sub ATA Details</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <span id="Label3" class="clsLabelAuto">ATA Chapter</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtATAChapter" runat="server"
                                                    CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                    BackColor="Gainsboro" Width="278px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Label4" class="clsLabelStar">*</>
                                            </td>
                                            <td>
                                                <span id="Label5" class="clsLabelAuto">Sub ATA Code </span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtSubATACode" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
                                                    ToolTip="Enter Sub ATA Code."></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <span id="Span1" class="clsLabelAuto">Sub Code </span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtSubCode" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
                                                    ToolTip="Enter Sub Code."></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <span id="Label6" class="clsLabelStar">*</span>
                                            </td>
                                            <td valign="top">
                                                <span id="Label7" class="clsLabelAuto">Sub ATA Chapter</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtSubATAChapter" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                    ToolTip="Enter Sub ATA Chapter."></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top"></td>
                                            <td valign="top">
                                                <span id="Label8" class="clsLabelAuto">Description</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                    ToolTip="Enter Sub ATA Description." TextMode="MultiLine" Width="278px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblResultSubATA" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div id="divGvSubATAList">
                                                    <asp:GridView ID="dgSubATAList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                        ShowHeader="true" AllowSorting="false" AllowPaging="True"
                                                        PageSize="5" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="ATAID" SortExpression="ATAID" HeaderText="ATAID">
                                                                <HeaderStyle></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA">
                                                                <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SubATACodeSubCode" SortExpression="SubATACodeSubCode"
                                                                HeaderText="Sub-ATA Code">
                                                                <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SubATANomenclature" SortExpression="SubATANomenclature"
                                                                HeaderText="Sub-ATA Chapter">
                                                                <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SubATADescription" SortExpression="SubATADescription"
                                                                HeaderText="Description">
                                                                <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="290px"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="290px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div id="dropDownImg" class="dropdown">
                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            ToolTip="Click to Edit record"
                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="deleteICN" class="actionICNS largerActionICNS" runat="server"
                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                            ToolTip="Click to Delete record"
                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
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
        <cc2:modalpopupextender id="lnkSubATACount_ModalPopupExtender" runat="server" targetcontrolid="btnDummySubATA"
            popupcontrolid="pnlPopUp" backgroundcssclass="clsModalPopupBG" behaviorid="ModalBehaviourID">
        </cc2:modalpopupextender>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunction();
				return false;
			}
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

			$(document).ready(function () {
				SetPageLayout();
				if ($.browser.msie) {
					parent.IFrameATAStateComplete();
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
    </form>
</body>
</html>
