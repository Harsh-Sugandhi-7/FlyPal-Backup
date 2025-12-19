<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobComp_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOJobComp_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Removal / Installation Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="popup.css">
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <style type="text/css">
        .style1 {
            width: 100%;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblLedgerList" class="clstablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Removal / Installation Details</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlAddTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add Removal/Installation Item"
                                                                        Text="Add" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"
                                                Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvOffPart" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="cmbOffPartList"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvOnPart" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="cmbOnPartList"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvOffPosition" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="txtOffPosition"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvOnPosition" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="txtOnPosition"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                           
                            <tr>
                                <%-- <td>
                                    <asp:UpdatePanel ID="upnlWO" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsLabelHeader" style="background-color: #E0E0E0">
                                                <legend><b>Job Description </b></legend>
                                                <table width="99%">
                                                    <tr style="display: none">
                                                        <td style="height: 21px" bgcolor="#E0E0E0">
                                                            <asp:Label ID="lblWO" runat="server" CssClass="clsLabelAuto">W. O. # </asp:Label>
                                                        </td>
                                                        <td style="height: 21px">
                                                            <asp:Label ID="lblWOLabel" runat="server" CssClass="clsLabelauto" BackColor="#E0E0E0"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td bgcolor="#E0E0E0">
                                                            <asp:Label ID="lblJob" runat="server" CssClass="clsLabelAuto">Job # </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblJobLabel" runat="server" CssClass="clsLabelauto" BackColor="#E0E0E0"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" bgcolor="#E0E0E0">
                                                            <asp:Label ID="lblJobDescription" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>--%>

                                <td>
                                    <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                        <legend>
                                            <asp:Label ID="lblJobDescription" runat="server" Text="Job Description" CssClass="clsLabelHeader"></asp:Label>
                                        </legend>
                                        <asp:TextBox ID="txtJobDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                            ToolTip="Job Description" BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine" ></asp:TextBox>
                                    </fieldset>
                                </td>
                                <%-- </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel> 
                                </td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlIsAssembly" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsAssembly" runat="server" CssClass="clsLabelHeader" ToolTip="Check if this is Assembly"
                                                            AutoPostBack="True"></asp:CheckBox>
                                                        <asp:Label ID="Label6" runat="server" CssClass="clsLabelHeader">Is Assembly</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 23px">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="upnlRemoval" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset id="lblRemovalDetail" class="clsFieldSetNewStyle">
                                                            <legend>
                                                                <asp:CheckBox ID="chkRemoval" runat="server" CssClass="clsLabelauto" ToolTip="Check for Removal Detail "
                                                                    AutoPostBack="True"></asp:CheckBox>
                                                                <b>Removal Details </b></legend>
                                                            <table id="tblRem" runat="server">
                                                                <tr>
                                                                    <td style="height: 16px">
                                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                                    </td>
                                                                    <td style="height: 16px">
                                                                        <asp:Label ID="lblOffPartList" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                    </td>
                                                                    <td style="height: 16px">
                                                                        <asp:DropDownList ID="cmbOffPartList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
                                                                            DataTextField="Name" DataValueField="ID">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOffPartNo" runat="server" CssClass="clsLabel">Part Name</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOffPartNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part Name for Removed Component"
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOffDescription" runat="server" CssClass="clsLabel">Part Description</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOffDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2"
                                                                            ToolTip="Enter Description for Removed Component" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOffSerialNo" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                                    </td>
                                                                    <td style="height: 15px">
                                                                        <asp:DropDownList ID="cmbOffSerialNo" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                            AutoPostBack="True" DataTextField="SerialNo" DataValueField="CompID">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblOffSerialNo1" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOffSerialNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Serial Number for Removed Component"
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOffPosition" runat="server" CssClass="clsLabel">Position</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOffPosition" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Position of Component to be Removed."
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblRemovalReason" runat="server" CssClass="clsLabelAuto">Removal Reason</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbRemovalReason" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="Name"
                                                                                        DataValueField="ID">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="imgReason" runat="server" CausesValidation="False" Height="22px"
                                                                                        ImageUrl="~/images/plus1.png" ToolTip="Click to Add Reason" Width="24px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOffTSN" runat="server" CssClass="clsLabel">TSN</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOffTSN" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                            ToolTip="Enter TSN for Removed Component" MaxLength="50"></asp:TextBox>&nbsp;
                                                                    <asp:Label ID="lblOffCSN" runat="server" CssClass="clsLabel">CSN</asp:Label>
                                                                        <asp:TextBox ID="txtOffCSN" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                            ToolTip="Enter CSN for Removed Component" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOffRemark" runat="server" CssClass="clsLabel">Remark</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOffRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" ToolTip="Enter Remark for Removed Component"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="upnlInst" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset id="lblInstallationDetail" class="clsFieldSetNewStyle">
                                                            <legend>
                                                                <asp:CheckBox ID="chkInstallation" runat="server" CssClass="clsLabelauto" ToolTip="Check for Installation Detail"
                                                                    AutoPostBack="True"></asp:CheckBox>
                                                                <b>Installation Details </b></legend>
                                                            <table id="tblInst" runat="server">
                                                                <tr>
                                                                    <td style="height: 16px">
                                                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                                    </td>
                                                                    <td style="height: 16px">
                                                                        <asp:Label ID="lblOnPartList" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                    </td>
                                                                    <td style="height: 16px">
                                                                        <asp:DropDownList ID="cmbOnPartList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
                                                                            DataTextField="Name" DataValueField="ID">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOnPartNo" runat="server" CssClass="clsLabel">Part Name</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOnPartNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part Name for Installed Component"
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOnDescription" runat="server" CssClass="clsLabel">Part Description</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOnDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                            ToolTip="Enter Description for Installed Component" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label5" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblOnSerialNo1" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOnSerialNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Serial Number for Installed Component"
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOnPosition" runat="server" CssClass="clsLabel">Position</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOnPosition" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Position of Component to be Installed."
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:Label ID="lblGRN" runat="server" CssClass="clsLabel">GRN</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtGRN" runat="server" CssClass="clsTextBoxTagSearch"
                                                                            MaxLength="50" ToolTip="Enter GRN of Component to be Installed."></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:Label ID="lblFormNo" runat="server" CssClass="clsLabel">Form No</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFormNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                            MaxLength="50" ToolTip="Enter Form No of Component to be Installed."></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOnTSN" runat="server" CssClass="clsLabel">TSN</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOnTSN" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                            ToolTip="Enter TSN for Installed Component" MaxLength="50"></asp:TextBox>&nbsp;
                                                                    <asp:Label ID="lblOnCSN" runat="server" CssClass="clsLabel">CSN</asp:Label>
                                                                        <asp:TextBox ID="txtOnCSN" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                            ToolTip="Enter CSN for Installed Component" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblOnRemark" runat="server" CssClass="clsLabel">Remark</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOnRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2"
                                                                            ToolTip="Enter Remark for Installed Component" TextMode="MultiLine"></asp:TextBox>
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
                                <%--<td style="height: 43px" align="right">
                                <asp:UpdatePanel ID="upnlAddTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnAddTop" runat="server" CssClass="clsButton" ToolTip="Click to add Removal/Installation Item"
                                                        Text="Add" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgRemovalInstallation" runat="server" CssClass="clsGridNewStyle" ToolTip="List of Removal/Installation"
                                                 ShowHeaderWhenEmpty="true" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" Height="50px" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OffPartNo" HeaderText="Rem. Part No./Model">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OffDescription" HeaderText="Description">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OffSerialNo" HeaderText="Serial No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemovalReasonName" HeaderText="Removal Reason">
                                                        <ItemStyle Wrap="True"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OnPartNo" HeaderText="Inst. Part No/Model">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OnDescription" HeaderText="Description">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OnSerialNo" HeaderText="Serial No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%-- <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRecord">
                                                        <HeaderStyle  Wrap="False"  HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRecord">
                                                        <HeaderStyle  Wrap="False"  HorizontalAlign="Left" />
                                                    </asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRecord" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <%-- <td align="right">
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsButton" ToolTip="Click to add Removal/Installation Item"
                                                            Text="Add" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr>
                                <td align="right" class="style1">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnRemovalReason" ClientIDMode="Static" runat="server" Text="Add"
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
        <div>
            <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="400" DynamicLayout="false" runat="server">
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
        <!-- Removal Reason Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyRemovalReason" Text="Removal Reason" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlRemovalReason" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeRemovalReason" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupRemovalReason" runat="server" TargetControlID="btnDummyRemovalReason"
            PopupControlID="pnlRemovalReason" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameRemovalReasonStateComplete() {
                $("#btnDummyRemovalReason").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenRemovalReasonWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeRemovalReason").attr("src", "wfRemovalReason_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyRemovalReason").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForRemovalReason() {
                var RemovalReasonwindow = $find("<%=mdlPopupRemovalReason.ClientID %>");
                //close Removal Reason popup window
                RemovalReasonwindow.hide();
                //           release resources
                $("#IframeRemovalReason").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnRemovalReason").click();
            }
        </script>
        <!-- End-->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForJobCompDetail();
                return false;
            }
            function CallCloseChildPage() {

                window.parent.CloseChildPage();
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameJobCompDetailStateComplete();
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
            function CallCloseChildPage() {

                window.parent.CloseChildPage();
            }
        </script>
        <%--End--%>
    </form>
    <script language="javascript">
        function SetTabCount(CountForTab) {
//              if (CountForTab == -1) {
//                 var totalRowCount = 0;
//                 var rowCount = 0;
//                 var gridView = document.getElementById("<%=dgRemovalInstallation.ClientID %>");
            //                 var rows = gridView.getElementsByTagName("tr")
            //                 for (var i = 0; i < rows.length; i++) {
            //                     totalRowCount++;
            //                     if (rows[i].getElementsByTagName("td").length > 0) {
            //                         rowCount++;
            //                     }
            //                 }
            //                  parent.document.getElementById("Label5").innerHTML = rowCount;
            //             }
            //             else {
            parent.document.getElementById("Label5").innerHTML = CountForTab;
            //             }
        }
    </script>
</body>
</html>
