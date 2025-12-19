<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmpCAAuthorization_Ajax.aspx.vb" Inherits="Flypal.wfEmpCAAuthorization_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company Authorization</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>

    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
    <script type="text/javascript">
        function resizeTextBox(txt) {
            txt.style.height = "1px";
            txt.style.height = (1 + txt.scrollHeight) + "px";

        }
        //function OnResize(txt) {
        //    $(txt).animate({ width: 275, height: txt.scrollHeight }, "fast");
        //}
        //function OnLostResize(txt) {
        //    $(txt).animate({ width: 275, height: 16 }, "fast");
        //}
        function OnResize(txt) {
            $(txt).animate({ height: txt.scrollHeight }, "fast");
        }
        function OnLostResize(txt) {
            $(txt).animate({ height: 14 }, "fast");
        }
    </script>
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
                                <td class="clsFormHeader1" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Company Authorization </asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="clsbtnH clsinfoH" ToolTip="Click to save"
                                                                        ValidationGroup="a"></asp:Button>
                                                                    <asp:Button ID="btnAuthorized" runat="server" Text="Authorize" class="clsbtnH clsinfoH" ToolTip="Click to Authorize"
                                                                        ValidationGroup="a"></asp:Button>
                                                                    <asp:Button ID="btnPrint" runat="server" Text="Print" class="clsbtnH clsinfoH" ToolTip="Click to Print"></asp:Button>
                                                                    <asp:Button ID="btnBack" runat="server" Text="Close" class="clsbtnH clsinfoH" ToolTip="Click to close"></asp:Button>
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
                                <td colspan="1">
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" ErrorMessage="Company Authorization Date Required." ControlToValidate="txtEmpCAAuthorizationDate"
                                                Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator
                                                ID="rfvDate" runat="server" Display="None" ErrorMessage="Date Required."
                                                ValidationGroup="a" ControlToValidate="txtEmpCAAuthorizationDate" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator1" runat="server" Display="None" ErrorMessage="Authorization No is required."
                                                ValidationGroup="a" ControlToValidate="txtCANo" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator
                                                ID="rfvContractNo" runat="server" Display="None" ErrorMessage="Revision No is required."
                                                ValidationGroup="a" ControlToValidate="txtRevisionNo" CssClass="clsValidationSummary">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator
                                                ID="rfvPlanName" runat="server" Display="None" ErrorMessage="Plan Name Required."
                                                ValidationGroup="a" ControlToValidate="txtEmpCAAuthorizationText" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvEmployee" runat="server" ClientValidationFunction="ValidateEmployee"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbEmployee" ErrorMessage="Please Select Employee."
                                                CssClass="clsValidationSummary"></asp:CustomValidator>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>

                                <td colspan="1" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text="<%# mEmpCAAuthorization.StatusName %>" CssClass="clsLabelHeader">
                                            </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlEmpCAAuthorizationDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlCAAuthorizationDetails" runat="server" CssClass="clsPanel1">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="50%" valign="top">
                                                            <fieldset id="fdsEmpDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                                <legend id="ledEmpDetails" class="clsLabelHeader">Employee Details</legend>

                                                                <table>
                                                                    <asp:PlaceHolder ID="plno" Visible="false" runat="server">
                                                                        <tr>

                                                                            <td>
                                                                                <span id="lblStarInvoiceNo" class="clsLabelStar">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblNo" class="clsLabel">No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtEmpCAAuthorizationText" runat="server" Text="<%# mEmpCAAuthorization.Text %>"
                                                                                    CssClass="clsTextBoxTagSearch" ToolTip="Enter No." MaxLength="25"
                                                                                    Width="130px"> </asp:TextBox>
                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtEmpCAAuthorizationText_Autocomplete"
                                                                                    runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                                    MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfEmpCAAuthorization_Ajax.aspx"
                                                                                    ServiceMethod="GetDistinctTextListAutoComplete" TargetControlID="txtEmpCAAuthorizationText"
                                                                                    UseContextKey="False"></cc2:AutoCompleteExtender>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtEmpCAAuthorizationNo" runat="server" Text="<%# mEmpCAAuthorization.No %>"
                                                                                    CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="8" ToolTip="Enter No."> </asp:TextBox>
                                                                            </td>

                                                                        </tr>
                                                                    </asp:PlaceHolder>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblEmployeeStar" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblEmployee" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbEmployee" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mEmpCAAuthorization.EmployeeID %>"
                                                                                DataTextField="EmpNoName" DataValueField="ID"
                                                                                Width="225px" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:Label ID="lblEmployeeCode" runat="server" CssClass="clsLabelAuto">Code</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEmployeeCode" runat="server" Text="<%# mEmpCAAuthorization.EmployeeCode %>"
                                                                                CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                Width="60px" autocomplete="off">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:Label ID="lblAMELNo" runat="server" CssClass="clsLabelAuto">AMEL No</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAMELNo" runat="server" Text="<%# mEmpCAAuthorization.AMELNo %>"
                                                                                CssClass="clsTextBoxTagSearch" MaxLength="500"
                                                                                Width="208px" autocomplete="off">
                                                                            </asp:TextBox>
                                                                        </td>

                                                                        <td></td>
                                                                        <td>
                                                                            <asp:Label ID="lblAMELCat" runat="server" CssClass="clsLabelAuto">AMEL Cat</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAMELCat" runat="server" Text="<%# mEmpCAAuthorization.AMELCat %>"
                                                                                CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                Width="60px" autocomplete="off">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <span id="lblDateOfExpiry" class="clsLabel">AMEL Date Of Expiry</span>
                                                                        </td>
                                                                        <td>
                                                                             <asp:TextBox runat="server" ID="txtDateOfExpiry" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off" Text="<%# mEmpCAAuthorization.DateOfExpiryFormatted %>"
                                                                            onchange="ValidateDateText(this,'txtDateOfExpiry_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtDateOfExpiry_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDateOfExpiry"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDateOfExpiry" ID="txtDateOfExpiry_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>

                                                                         <td></td>
                                                                        <td>
                                                                            <span id="lblDateOfContinuationTrainingValidity" class="clsLabel">Continuation Training Validity</span>
                                                                        </td>
                                                                        <td>
                                                                             <asp:TextBox runat="server" ID="txtContinuationTrainingValidity" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off" Text="<%# mEmpCAAuthorization.ContinuationTrainingValidityFormatted %>"
                                                                            ></asp:TextBox>

                                                                            <%--onchange="ValidateDateText(this,'txtDateOfExpiry_watermarkextender');"--%>

                                                                        <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtContinuationTrainingValidity"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtContinuationTrainingValidity" ID="TextBoxWatermarkExtender1"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>



                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                        <td width="50%" valign="top">
                                                            <fieldset id="fdsCApDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                                <legend id="ledCADetails" class="clsLabelHeader">CA Details</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblCANoStar" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblCANo" class="clsLabel">Authorization No</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCANo" runat="server" Text="<%# mEmpCAAuthorization.CANumber %>"
                                                                                CssClass="clsTextBoxTagSearch" ToolTip="Enter Authorization No."
                                                                                Width="100px"> </asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblStarDate" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblDate" class="clsLabel">Date</span>
                                                                        </td>
                                                                        <td>
                                                                             <asp:TextBox runat="server" ID="txtEmpCAAuthorizationDate" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off" Text=""
                                                                            onchange="ValidateDateText(this,'txtEmpCAAuthorizationDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtEmpCAAuthorizationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEmpCAAuthorizationDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtEmpCAAuthorizationDate" ID="txtEmpCAAuthorizationDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblRevisionNoStar" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblRevisionNo" class="clsLabel">Revision No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRevisionNo" runat="server" Text="<%# mEmpCAAuthorization.RevisionNo %>"
                                                                                CssClass="clsTextBoxTagSearch" ToolTip="Enter Revision No."
                                                                                Width="100px"> </asp:TextBox>
                                                                        </td>
                                                                        <td></td>

                                                                        <td>
                                                                            <span id="lblRevisionDate" class="clsLabel">Revision Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtRevisionDate" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off" Text="<%# mEmpCAAuthorization.RevisionDateFormatted %>"
                                                                            onchange="ValidateDateText(this,'txtRevisionDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtRevisionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRevisionDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtRevisionDate" ID="txtRevisionDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>



                                                                        </td>

                                                                    </tr>
                                                                    <tr>

                                                                        <td><span id="lblFromStar" class="clsLabelStar">*</span></td>
                                                                        <td><span id="lblFromDate" class="clsLabelAuto">Issue Date</span> </td>
                                                                        <td>
                                                                             <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off" Text="<%# mEmpCAAuthorization.CAInitialIssueDateFormatted %>"
                                                                            onchange="ValidateDateText(this,'txtFromDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="txtFromDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td><span id="lblToDateStar" class="clsLabelStar">*</span></td>
                                                                        <td><span id="lblToDate" class="clsLabelAuto">Valid Upto</span> </td>
                                                                        <td>
                                                                             <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch" Width="100px" AutoComplete="off" Text="<%# mEmpCAAuthorization.CAValidUptoFormatted %>"
                                                                            onchange="ValidateDateText(this,'txtToDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="txtToDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" colspan="2">
                                                            <fieldset id="fdsEmpOtherDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                                <legend id="ledEmpOtherDetails" class="clsLabelHeader">Other Details</legend>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td width="55%">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <input type="button" id="btnSelectFile" value="Select File" 
                                                                                                                runat="server" class="clsbtnH" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH" Enabled="False"   Text="Remove Attachment" ToolTip="Click to Remove Attachment"  />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px" ImageUrl="icons/CLIP01.ICO" Width="20px" />
                                                                                                            <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="----" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td width="45%">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" MaxLength="1000" Text="<%# mEmpCAAuthorization.Remark %>" ToolTip="Enter Remark"  TextMode="MultiLine" >
                                                                                        </asp:TextBox>
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
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>

                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlEmpCAAuthorizationDetail" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlCompanyDetails" runat="server" CssClass="clsPanel1">
                                                <fieldset id="fdsEmpAuthorizationDetails" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledEmpAuthorizationDetails">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblCompanyAuthorizationDetailsAdd" class="clsLabelHeader">Company Authorization Details:</span>
                                                                </td>
                                                                <td>
                                                                    <%--<asp:Button ID="btnCompanyAuthorizationDetailsAdd" runat="server" class="clsbtnH clsinfoH1" Height="30px" Text="Add"
                                                                        ValidationGroup="a"></asp:Button>--%>
                                                                    <asp:ImageButton ID="ImgDetailsAdd" runat="server" CausesValidation="true"
                                                                        Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add Authorization Details"
                                                                        Width="24px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table width="100%">

                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgEmpCAAuthorizationDetail" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" DataKeyNames="ID"
                                                                    AutoGenerateColumns="False" CellPadding="10" ForeColor="Black" GridLines="Horizontal">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <Columns>
                                                                        <%--0--%>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="SN" HtmlEncode="false">
                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="true" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <%--1--%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblAuthorizationDetailsStar" runat="server"  class="clsLabelStar">*</asp:Label><span
                                                                                    id="Span6" class="clsdgHeader" style="background-color: White; color: Black; font-weight: bold;">AIRCRAFT/ ENGINE/
COMPONENT TYPES</span>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:UpdatePanel ID="upnlAuthorizationDetailsValidate" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:RequiredFieldValidator ID="rfvAuthorizationDetails" runat="server" ControlToValidate="txtAuthorizationDetails"
                                                                                            CssClass="clsLabel" Display="dynamic" ErrorMessage="Authorization Detail(s) Required"
                                                                                            Font-Italic="true" ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Authorization Detail(s) Required"
                                                                                            ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                                <asp:TextBox ID="txtAuthorizationDetails" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                    onFocus="OnResize(this)" onkeyup="resizeTextBox(this)" Text='<%# DataBinder.Eval(Container.DataItem, "AuthorizationDetails") %>'
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <%--2--%>
                                                                        <asp:TemplateField HeaderText="SCOPE OF AUTH./CODE">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkbtnAddSCOPE" runat="server" CommandName="SCOPERec" CausesValidation="True" CommandArgument='<%# Eval("SrNo") %>'></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                        <%--3--%>
                                                                        <asp:BoundField DataField="AuthorizationScope" HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn" HeaderText="AuthorizationScope">
                                                                            <HeaderStyle CssClass="hideGridColumn" />
                                                                            <ItemStyle CssClass="hideGridColumn" />
                                                                        </asp:BoundField>

                                                                        <%--4--%>
                                                                        <asp:TemplateField HeaderText="LICENSE LIMITATION">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkbtnAddLICENSE" runat="server" CommandName="LICENSRec" CausesValidation="True" CommandArgument='<%# Eval("SrNo") %>'></asp:LinkButton>
                                                                            </ItemTemplate>

                                                                        </asp:TemplateField>

                                                                        <%--5--%>
                                                                        <asp:BoundField DataField="LicenseLimitation" HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn" HeaderText="LicenseLimitation">
                                                                            <HeaderStyle CssClass="hideGridColumn" />
                                                                            <ItemStyle CssClass="hideGridColumn" />
                                                                        </asp:BoundField>
                                                                        <%--6--%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="LIMITATION">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblLimitationsStar" runat="server"   class="clsLabelStar">*</asp:Label><span
                                                                                    id="Span6" class="clsdgHeader" style="background-color: White; color: Black; font-weight: bold;">LIMITATION</span>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:UpdatePanel ID="upnlLimitationsValidate" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:RequiredFieldValidator ID="rfvLimitations" runat="server" ControlToValidate="txtLimitations"
                                                                                            CssClass="clsLabel" Display="dynamic" ErrorMessage="Authorization Detail(s) Required"
                                                                                            Font-Italic="true" ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Limitations Required"
                                                                                            ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                                <asp:TextBox ID="txtLimitations" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                    onFocus="OnResize(this)" onkeyup="resizeTextBox(this)" Text='<%# DataBinder.Eval(Container.DataItem, "LimitationsDetails") %>'
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <%--7--%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="black" HeaderText="REV NO.">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblRevNoStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label><span
                                                                                    id="Span6" class="clsdgHeader" style="background-color: White; color: Black; font-weight: bold;">REV NO.</span>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>

                                                                                <asp:TextBox ID="txtRevNo" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                   Text='<%# DataBinder.Eval(Container.DataItem, "RevNo") %>' MaxLength="50"></asp:TextBox>
                                                                            </ItemTemplate>

                                                                        </asp:TemplateField>
                                                                        <%--8--%>
                                                                        <asp:TemplateField HeaderText="REV DATE" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:UpdatePanel ID="upnlRevValidate" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:CustomValidator ID="cvRev" runat="server" ControlToValidate="txtRev"
                                                                                            CssClass="clsLabel" Display="dynamic" Font-Italic="true" ForeColor="Red" InitialValue="-1"
                                                                                            SetFocusOnError="true" Text="* Rev Date" ValidationGroup='<%#String.Format("Group_{0}", Eval("ID")) %>'></asp:CustomValidator>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                                <asp:TextBox ID="txtRev" CssClass="clsTextBoxTagSearch" Width="85px" onchange="ValidateDateText(this,'txtRev_CalendarExtender')"
                                                                                    runat="server" autocomplete="off"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtRev_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRev"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtRev" ID="txtRev_watermarkextender"
                                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <%--9--%>
                                                                        <asp:TemplateField HeaderText="Attach File" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <%--  <input type="button" id="btnSelectDetailFile" value="Select File" style="width: 77px;"
                                                                                                    runat="server" class="clsbtnH clsinfoH1" commandname="SelectFile" />--%>
                                                                                                                <asp:Button ID="btnSelectDetailFile1" runat="server" CommandArgument='<%# Eval("SrNo") %>' class="clsbtnH clsinfoH1" Text="Select File" CommandName="SelectFile" />

                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="imgDelDetailAttach1" runat="server" CommandArgument='<%# Eval("SrNo") %>' CausesValidation="False" ImageUrl="images/remove.jpg" CommandName="RemoveAttachRec"
                                                                                                                    Visible='<%#  Eval("IsAttachmentAdded")%>' ToolTip="Click to Remove Attachment" Height="20px" Width="20px"></asp:ImageButton>
                                                                                                            </td>

                                                                                                        </tr>
                                                                                                    </table>

                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <%--10--%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <div class="dropdown">
                                                                                    <div class="dropdownbtn-content">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRecord" Style="height: 20px; width: 20px"
                                                                                                        ImageUrl="~/images/delete.png" CommandArgument='<%# Eval("SrNo") %>' />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                        CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                                                        Visible='<%#  Eval("IsAttachmentAdded")%>' />
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
                                                                        <%--11--%>
                                                                        <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                            ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                    </Columns>
                                                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDetailsAddBottom" runat="server" CausesValidation="true" Visible="false"
                                                                    Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add Authorization Details"
                                                                    Width="24px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlEmpCAAuthorizationTerms" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlTermsDetails" runat="server" CssClass="clsPanel1">

                                                <fieldset id="fdsEmpTermsDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                    <legend id="ledEmpTermsDetails">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblEmpCAAuthorizationTermsAdd" class="clsLabelHeader">Company Authorization Term(s):</span>
                                                                </td>
                                                                <td>
                                                                    <%--<asp:Button ID="btnEmpCAAuthorizationTermsAdd" runat="server" class="clsbtnH clsinfoH1" Height="30px" Text="Add"
                                                                        ValidationGroup="a"></asp:Button>--%>
                                                                    <asp:ImageButton ID="imgTermsAdd" runat="server" CausesValidation="true"
                                                                        Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add Authorization Terms"
                                                                        Width="24px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table width="100%">

                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgEmpCAAuthorizationTerms" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                                    AutoGenerateColumns="False" CellPadding="10" ForeColor="Black" GridLines="Horizontal">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />

                                                                    <Columns>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="SN" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Terms" HeaderText="Terms and Conditions" HeaderStyle-Width="80%" ItemStyle-Width="80%">
                                                                            <ItemStyle CssClass="TextBreak" Width="500px" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;" colspan="2">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnEmpCAAuthorizationDetail" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnAuthorizationDetailsScope" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnAuthorizationDetailsLimitation" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnimgBtnCAAuthorizationTerm" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenFileUploadWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                    //                if (!$.browser.msie) {
                    $("#btnDummyFileUpload").click();
                    $get("AjaxLoader").style.visibility = "hidden";
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
        <!-- End -->
        <!--EmpCAAuthorizationAssembly Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpCAAuthorizationAssembly" Text="EmpCAAuthorizationAssembly" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpCAAuthorizationAssembly" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpCAAuthorizationAssembly" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpCAAuthorizationAssembly" runat="server" TargetControlID="btnDummyEmpCAAuthorizationAssembly"
            PopupControlID="pnlEmpCAAuthorizationAssembly" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpCAAuthorizationAssemblyStateComplete() {
                $("#btnDummyEmpCAAuthorizationAssembly").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpCAAuthorizationAssemblyWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpCAAuthorizationAssembly").attr("src", "wfEmpCAAuthorizationAssembly_Ajax.aspx?Type=pup");

                    /*if (!$.browser.msie) {*/
                    $("#btnDummyEmpCAAuthorizationAssembly").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                    //}
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForEmpCAAuthorizationAssembly() {
                var EmpCAAuthorizationAssemblyWindow = $find("<%=mdlPopupEmpCAAuthorizationAssembly.ClientID %>");
                //close popup window
                EmpCAAuthorizationAssemblyWindow.hide();
                //release resources
                $("#IframeEmpCAAuthorizationAssembly").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnEmpCAAuthorizationAssembly").click();
            }
            function CallLostCAAuthorizationResize() {
                $("#dgEmpCAAuthorizationDetail tr").each(function () {
                    var txtAuthorizationDetails = $(this).find("[id*=txtAuthorizationDetails]");
                    OnLostResize(txtAuthorizationDetails);
                });
            }
        </script>
        <!-- End-->

        <!-- Authorization Scope Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAuthorizationDetailsScope" Text="Employee Skill" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAuthorizationDetailsScope" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAuthorizationDetailsScope" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAuthorizationDetailsScope" runat="server" TargetControlID="btnDummyAuthorizationDetailsScope"
            PopupControlID="pnlAuthorizationDetailsScope" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAuthorizationDetailsScopeStateComplete() {
                $("#btnDummyAuthorizationDetailsScope").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenScopeWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAuthorizationDetailsScope").attr("src", "wfEmpCAAuthorizationDetailsScopeList.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAuthorizationDetailsScope").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForAuthorizationDetail() {
                var AuthorizationDetailsScopewindow = $find("<%=mdlPopupAuthorizationDetailsScope.ClientID %>");
                //close Skill popup window
                AuthorizationDetailsScopewindow.hide();
                //           release resources
                $("#IframeAuthorizationDetailsScope").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnAuthorizationDetailsScope").click();
            }
        </script>
        <!-- End-->
        <!-- Authorization Limitation Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAuthorizationDetailsLimitation" Text="Employee Skill" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAuthorizationDetailsLimitation" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAuthorizationDetailsLimitation" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupAuthorizationDetailsLimitation" runat="server" TargetControlID="btnDummyAuthorizationDetailsLimitation"
            PopupControlID="pnlAuthorizationDetailsLimitation" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAuthorizationDetailsLimitationStateComplete() {
                $("#btnDummyAuthorizationDetailsLimitation").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLimitationWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAuthorizationDetailsLimitation").attr("src", "wfEmpCAAuthorizationDetailsLicenseLimitationList.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAuthorizationDetailsLimitation").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForAuthorizationDetailLimitation() {
                var AuthorizationDetailsLimitationwindow = $find("<%=mdlPopupAuthorizationDetailsLimitation.ClientID %>");
                //close Skill popup window
                AuthorizationDetailsLimitationwindow.hide();
                //           release resources
                $("#IframeAuthorizationDetailsLimitation").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnAuthorizationDetailsLimitation").click();
            }
        </script>
        <!-- End-->


        <!-- Term Popup Window  btnEmpCAAuthorizationTermsAdd btnDummyTerm hdnimgBtnTerm pnlPopupTerm iPopupTerm mdlPopupTerm-->

        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCAAuthorizationTerm" Text="Dummy Term" ClientIDMode="Static" CausesValidation="false" />

        </div>
        <asp:Panel runat="server" ID="pnlPopupCAAuthorizationTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupCAAuthorizationTerm" frameborder="0" allowtransparency="true" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupCAAuthorizationTerm" runat="server" TargetControlID="btnDummyCAAuthorizationTerm"
            PopupControlID="pnlPopupCAAuthorizationTerm" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>

        <script type="text/javascript">

            function IFrameTermStateComplete() {

                $("#btnDummyCAAuthorizationTerm").click();
                $get("AjaxLoader").style.visibility = 'hidden';

            }
            function OpenTermWindow() {
                try {
                    $("#iPopupCAAuthorizationTerm").attr("src", "wfCATerm.aspx?Type=pup&OpenFrom=11");
                    if (!$.browser.msie) {
                        $("#btnDummyCAAuthorizationTerm").click();
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
        </script>

        <script type="text/javascript">
            function ParentCallBackFunctionForTerm() {
                var TermWindow = $find("<%=mdlPopupCAAuthorizationTerm.ClientID %>");
                //close Term popup window
                TermWindow.hide();
                $("#iPopupCAAuthorizationTerm").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnCAAuthorizationTerm").click();
            }
        </script>
        <!-- End-->
        <script type="text/javascript">
            function ValidateEmployee(source, args) {
                args.IsValid = false;
                var dd = $get("cmbEmployee");
                if (dd.selectedIndex != 0) {
                    args.IsValid = true;
                    return;
                }
            }
        </script>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForEmpCAAuthorizationDetails();
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
                    parent.IFrameEmpCAAuthorizationDetailsStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>

        <script type="text/javascript">
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
