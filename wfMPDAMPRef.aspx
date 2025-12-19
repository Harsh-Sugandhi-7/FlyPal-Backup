<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMPDAMPRef.aspx.vb" EnableEventValidation="false" Inherits="Flypal.wfMPDAMPRef" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>MPD/AMP Ref </title>
    <link href="Styles.css" rel="stylesheet" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain" >
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">

                        <table  id="tblLedgerList" width="100%">

                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <span id="lblPartList" class="clstitle1">MPD/AMP Revision</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>

                                <%-- MPD CODE--%>
                                <td valign="top" width="50%">
                                    <asp:UpdatePanel runat="server" ID="upnlMPDDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3" width="100%">
                                                <tr>
                                                    <td>
                                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                                    ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                                                    ValidationGroup="1" ErrorMessage="Select Revision Date" ControlToValidate="txtMPDFromDate"
                                                                                    Display="None"></asp:RequiredFieldValidator>--%>

                                                                                <asp:CustomValidator ID="cvValidator" runat="server" ControlToValidate="txtMPDNo"
                                                                                    CssClass="clsLabel" ValidationGroup="1" Display="None" ErrorMessage=""></asp:CustomValidator>

                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                  
                                                                    <td align="right" colspan="3">
                                                                        <asp:Button ID="btnNewMPD" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New MPD Revision"
                                                                             CausesValidation="false" Text="New"></asp:Button>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="lblModel" class="clsLabelAuto">Model</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtModel" autocomplete="off" runat="server" CssClass="clsTextBox_Ajax"
                                                                            ClientIDMode="Static"></asp:TextBox>

                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="spnMPDReferenceNo" class="clsLabelAuto">MPD No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMPDNo" ClientIDMode="Static" runat="server" CssClass="clsTextBox_Ajax"
                                                                            ReadOnly="true" ToolTip="Enter MPD No."></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="spnRevisionNo" class="clsLabelAuto">Revision No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMPDRevisionNo" ClientIDMode="Static" runat="server" CssClass="clsTextBox_Ajax"
                                                                            ReadOnly="true" ToolTip="Enter MPD Revision No."></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="spnMPDDate" class="clsLabelAuto">Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMPDFromDate" runat="server" autocomplete="off" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'EffectiveDate_watermarkextender','true');"
                                                                              Width="100px"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="calEffectiveDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            OnClientShown="onClientShown" OnClientHidden="onClientHide" Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                                            TargetControlID="txtMPDFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender ID="EffectiveDate_watermarkextender" runat="server"
                                                                            ClientIDMode="Static" TargetControlID="txtMPDFromDate" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" colspan="3">
                                                                        <asp:UpdatePanel ID="upnlMPDAttachment1" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                                    <legend>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <b>Attach Documents</b>
                                                                                                </td>
                                                                                                <td valign="top">
                                                                                                    <asp:ImageButton ID="btnMPDSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                        Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="true"></asp:ImageButton>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </legend>

                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="upnlGridMPDAttachment" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:GridView ID="dgMPDAttachment1" ToolTip="List of File Attachment(s)" runat="server"
                                                                                                            CssClass="clsGrid" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                                                            AllowPaging="False" AutoGenerateColumns="false">
                                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <Columns>
                                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                                <asp:BoundField Visible="False" DataField="ReferenceID" HeaderText="ReferenceID"></asp:BoundField>
                                                                                                                <asp:BoundField Visible="False" DataField="FileName" HeaderText="File Name">
                                                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                </asp:BoundField>
                                                                                                                <asp:TemplateField HeaderText="File Name">
                                                                                                                    <HeaderStyle Width="220px" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtMPDFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
                                                                                                                            ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'
                                                                                                                            Width="220px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton ID="ViewRec" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="ViewRec"
                                                                                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton ID="RemoveRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                                            CommandName="RemoveRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                                            CausesValidation="false" />
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
                                                                                    </table>

                                                                                </fieldset>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 0px;">
                                                                    <td style="height: 0px;" colspan="3">
                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                            <ContentTemplate>
                                                                                <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                <asp:Button ID="hdnBtnFileUploadAMP" ClientIDMode="Static" runat="server" Text="----"
                                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="right">
                                                                        <asp:UpdatePanel ID="upnlMPDActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Button ID="btnMPDCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous Page"
                                                                                                Visible="false" CausesValidation="false" Text="Back"></asp:Button>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnMPDSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save MPD Revision"
                                                                                                ValidationGroup="1" CausesValidation="true" Text="Save"></asp:Button>
                                                                                        </td>

                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:UpdatePanel ID="upnldgMPDRefList" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="dgMPDRefList" runat="server" AllowSorting="false" AutoGenerateColumns="False" ClientIDMode="Static"
                                                                                    AllowPaging="false" PageSize="25" ShowHeaderWhenEmpty="true"
                                                                                    CssClass="clsGrid">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                                        NextPageText="" PreviousPageText="" />
                                                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                                        <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                             <ItemStyle Wrap="false"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="MPDNo" HeaderText="MPD No.">
                                                                                            <HeaderStyle Wrap="true" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="true"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="RevNo" HeaderText="Revision No.">
                                                                                            <HeaderStyle Wrap="true" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="true"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="FromDateFormatted" HeaderText="Date">
                                                                                            <HeaderStyle Wrap="False" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                    CommandName="ViewRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                    CausesValidation="false" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                                                    Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
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
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                                <td valign="top" width="50%">

                                    <%-- AMP CODE--%>
                                    <asp:UpdatePanel runat="server" ID="upnAMPDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table4" width="100%">
                                                <tr>
                                                    <td>
                                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:UpdatePanel ID="upnlValidationSummary1" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                                                    ValidationGroup="2" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                                <%--<asp:RequiredFieldValidator ID="rfvEnquiryDate" runat="server" CssClass="clsLabelAuto"
                                                                                    ValidationGroup="2" ErrorMessage="Select Revision Date" ControlToValidate="txtAMPFromDate"
                                                                                    Display="None"></asp:RequiredFieldValidator>--%>

                                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtAmpNo"
                                                                                    CssClass="clsLabel" ValidationGroup="2" Display="None" ErrorMessage=""></asp:CustomValidator>

                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                   
                                                                    <td align="right" colspan="3">
                                                                        <asp:Button ID="btnNewAMP" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New AMP Revision"
                                                                              CausesValidation="false" Text="New"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="lblModelAMP" class="clsLabelAuto">Reg No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAMP" autocomplete="off" runat="server" CssClass="clsTextBox_Ajax"
                                                                            ReadOnly="true"   ClientIDMode="Static"></asp:TextBox>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblAmpNo" class="clsLabelAuto">AMP No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAmpNo" ClientIDMode="Static" runat="server" CssClass="clsTextBox_Ajax"
                                                                            ReadOnly="true" ToolTip="Enter AMP No."></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="spnAMPRevisionNo" class="clsLabelAuto">Revision No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAMPRevisionNo" ClientIDMode="Static" runat="server" CssClass="clsTextBox_Ajax"
                                                                            ReadOnly="true" ToolTip="Enter AMP Revision No."></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="spnAMPDate" class="clsLabelAuto">Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAMPFromDate" runat="server" autocomplete="off" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'AMPFromDate_watermarkextender','true');"
                                                                             Width="100px"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="calAMPFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                             OnClientHidden="onClientHide" Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                                            TargetControlID="txtAMPFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender ID="AMPFromDate_watermarkextender" runat="server"
                                                                            ClientIDMode="Static" TargetControlID="txtAMPFromDate" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" colspan="3">
                                                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                            <legend>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <b>Attach Documents</b>
                                                                                        </td>
                                                                                        <td valign="top">

                                                                                            <asp:ImageButton ID="btnASPSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="true"></asp:ImageButton>

                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </legend>
                                                                            <asp:UpdatePanel ID="upnlAMPAttachment" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td style="height: 15px">
                                                                                                <asp:UpdatePanel ID="upnldgAMPAttachment" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:GridView ID="dgASPAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                                                            CssClass="clsGrid" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                                                            AllowPaging="False" AutoGenerateColumns="false">
                                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <Columns>
                                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                                <asp:BoundField Visible="False" DataField="ReferenceID" HeaderText="ReferenceID"></asp:BoundField>
                                                                                                                <asp:BoundField Visible="False" DataField="FileName" HeaderText="File Name">
                                                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                </asp:BoundField>
                                                                                                                <asp:TemplateField HeaderText="File Name">
                                                                                                                    <HeaderStyle Width="220px" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
                                                                                                                            ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'
                                                                                                                            Width="220px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                                            CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                                            CausesValidation="false" />
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
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 0px;">
                                                                    <td style="height: 0px;" colspan="3">
                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel5">
                                                                            <ContentTemplate>
                                                                                <asp:Button ID="Button1" ClientIDMode="Static" runat="server" Text="----"
                                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="right">
                                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Button ID="btnAMPCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous Page"
                                                                                                Visible="false" CausesValidation="false" Text="Back"></asp:Button>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnAMPSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save AMP Revision"
                                                                                                ValidationGroup="2" CausesValidation="true" Text="Save"></asp:Button>
                                                                                        </td>
                                                                                        
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:UpdatePanel ID="upnDgAMP" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="dgAMP" runat="server" AllowSorting="false" AutoGenerateColumns="False" ClientIDMode="Static"
                                                                                    AllowPaging="false" PageSize="25" ShowHeaderWhenEmpty="true" EnableViewState="True"
                                                                                    CssClass="clsGrid">
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                                        NextPageText="" PreviousPageText="" />
                                                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No.">
                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                              <ItemStyle Wrap="false"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AMPNo" HeaderText="AMP No.">
                                                                                            <HeaderStyle Wrap="true" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="true"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="RevNo" HeaderText="Revision No.">
                                                                                            <HeaderStyle Wrap="true" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="true"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="FromDateFormatted" HeaderText="Date">
                                                                                            <HeaderStyle Wrap="False" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                    CommandName="ViewRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                    CausesValidation="false" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                                                    Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
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
                                                            </table>
                                                        </fieldset>
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
            <tr>
                <td align="right">
                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1" cellspacing="0" border="0">
                                <tr>

                                    <td>
                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go Previous page"
                                            CausesValidation="False" Text="Back"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
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
                //   $get("AjaxLoader").style.visibility = 'hidden';
            }


            function OpenFileUploadWindow() {
                try {

                    ///  $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                    //                if (!$.browser.msie) {
                    //    $("#btnDummyFileUpload").click();
                    //    $get("AjaxLoader").style.visibility = "hidden";
                    //                }
                    return false;
                } catch (e) {
                    alert(e);
                }

            }
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
        <script type="text/javascript">
            function BetweenDatesValidation(source, args) {
                if (source.controltovalidate == "txtBillingDate") {
                    var fromdate = $("#txtDate").val();
                    var todate = $("#txtBillingDate").val();
                }
                else {
                    return;
                }


                args.IsValid = false;

                if (!todate) {
                    rfvToDate.isvalid = false;
                    return;
                }
                if (!fromdate) {
                    rfvFromDate.isvalid = false;
                    return;
                }

                var param = { 'FromDate': fromdate, 'ToDate': todate };
                $.ajax({
                    type: "POST",
                    url: "BetweenDateValidationHandler.ashx",
                    cache: false,
                    data: param,
                    async: false,
                    beforeSend: OnBeforeSnd,
                    success: onSuces,
                    error: onErr
                });

                function onSuces(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    if (result == "True") {
                        args.IsValid = true;
                        return;
                    }

                }

                function onErr(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    source.errormessage = result;
                    return;
                }
                function OnBeforeSnd() {
                    $get("AjaxLoader").style.visibility = 'visible';
                }

            }

            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
                    // $find(extenderid).set_Text(result);
                    __doPostBack($(elem).id, "TextChanged");
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
        <script language="JavaScript" type="text/javascript">
            function CallParentFunction() {

                window.parent.autoResizeMPDRef();
            }
            function CallCloseChildPage() {

                window.parent.CloseChildPage();
            }
        //function CheckValidation() {
        //    if (!Page_ClientValidate()) {
        //        // Call Your custom JS function and return value.
        //        CallParentFunction();
        //    }
        //}
        </script>
        <script type="text/javascript" language="javascript">
            function onClientShown(sender, e) {
                window.parent.autoResizeMPDRef();
            }
            function onClientHide(sender, e) {
                window.parent.autoResizeMPDRef();
            }
        </script>
    </form>

    <%--  Call parent AutoResize function to resize the form--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeTankList();
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
    </script>
    <%--Called parent function to open Tank master page--%>
</body>
</html>
