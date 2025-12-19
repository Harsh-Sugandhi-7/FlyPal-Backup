<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMPDMaster.aspx.vb" Inherits="Flypal.wfMPDMaster" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MPD Master Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
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
        <table id="Table1" class="clstablelistout" border="0" cellspacing="1" cellpadding="1"
            width="100%">

            <tr>
                <td valign="top" colspan="3">
                    <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1" width="100%">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table13" valign="top" border="0" cellspacing="1" cellpadding="1" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">MPD Master Details</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="clsbtnH clsinfoH" ToolTip="Click to save"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" Text="Close" class="clsbtnH clsinfoH" ToolTip="Click to close" CausesValidation="false"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvMPDTaskNo" runat="server" Display="None" ControlToValidate="txtMPDTaskNo"
                                            ErrorMessage="MPD Task No. Required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDesc" runat="server" Display="None" ControlToValidate="txtDescription"
                                            ErrorMessage="Description Required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                        <%--  <asp:RequiredFieldValidator ID="rfvTaskDesc" runat="server" Display="None" ControlToValidate="txtTaskTimings"
                                            ErrorMessage="Task Description Required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>--%>
                                        <asp:CustomValidator ID="cvATAChapter" runat="server" Display="None" ControlToValidate="cmbATA"
                                            ErrorMessage="Select ATA From List" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvType" runat="server" Display="None" ControlToValidate="cmbType"
                                            ErrorMessage="Select Type from List" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPrimaryModel" runat="server" Display="None" ControlToValidate="cmbPrimaryModel"
                                            ErrorMessage="Select Primary Model from List" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsMonitorServiceingDetails" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="lgdMonitorServiceDetails"><b>Details</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>

                                                        <asp:Label ID="lblText" runat="server" CssClass="clsLabelAuto">MPD Task No.</asp:Label>

                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMPDTaskNo" runat="server" AutoComplete="off" ClientIDMode="Static" Text="<%# mMPDMaster.MPDTaskNumber %>" Width="225px"
                                                            CssClass="clsTextBoxTagSearch" ToolTip="Enter Text" Enabled="<%# (mMPDMaster.IsNew)   %>"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblATA" runat="server" CssClass="clsLabelAuto">ATA</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbATA" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mMPDMaster.ATAID %>"
                                                            DataValueField="ID" DataTextField="ATAChapter" AutoPostBack="True" Width="225px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" Text="<%# mMPDMaster.Description %>"
                                                            ToolTip="Enter Description" MaxLength="500" TextMode="MultiLine" Enabled="<%# not (mMPDMaster.IsConfigured)   %>">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <%--<asp:Label ID="lblTaskTimingsStar" runat="server" CssClass="clsLabelStar">*</asp:Label>--%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTaskTimings" runat="server" CssClass="clsLabelAuto">Task Description</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTaskTimings" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" Text="<%# mMPDMaster.TaskIntervalDescription %>"
                                                            ToolTip="Enter Task Timings" MaxLength="1000" TextMode="MultiLine" Enabled="<%# not (mMPDMaster.IsConfigured)   %>">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTaskType" runat="server" CssClass="clsLabelAuto">Type</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mMPDMaster.ServiceTypeID %>" Enabled="<%# not (mMPDMaster.IsConfigured)   %>"
                                                            DataValueField="ID" DataTextField="Name" AutoPostBack="True" Width="225px">
                                                        </asp:DropDownList>
                                                    </td>


                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPrimaryModelreq" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPrimaryModel" runat="server" CssClass="clsLabelAuto">Primary Model</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbPrimaryModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mMPDMaster.PrimaryModelID %>"
                                                            DataValueField="ID" AutoPostBack="true" DataTextField="Name" Width="225px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <%--<asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>--%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblApplicability" runat="server" CssClass="clsLabelAuto">Applicability</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtApplicability" runat="server" AutoComplete="off" ClientIDMode="Static" Text="<%# mMPDMaster.Applicability %>"
                                                            CssClass="clsTextBoxTagSearch" ToolTip="Enter Text" Width="225px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblMRBCategories" runat="server" CssClass="clsLabelAuto">MRB Categories</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbMRBCategories" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mMPDMaster.MPDTypeID %>"
                                                            DataTextField="CodeType" DataValueField="Id" Width="225px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblSkill" runat="server" CssClass="clsLabelAuto">Skill</asp:Label>
                                                    </td>

                                                    <td>
                                                        <asp:DropDownList ID="cmbSkill" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mMPDMaster.MPDSkillID %>"
                                                            DataTextField="CodeWithName" DataValueField="Id" Width="225px">
                                                        </asp:DropDownList>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblZone" runat="server" CssClass="clsLabelAuto">Zone</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtZone" runat="server" AutoComplete="off" ClientIDMode="Static" Text="<%# mMPDMaster.Zone %>"
                                                            CssClass="clsTextBoxTagSearch" ToolTip="Enter Text" Width="225px"></asp:TextBox>
                                                    </td>
                                                    <td></td>

                                                    <td>
                                                        <asp:Label ID="lblAccess" runat="server" CssClass="clsLabelAuto">Access</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAccess" runat="server" AutoComplete="off" ClientIDMode="Static" Text="<%# mMPDMaster.Access %>"
                                                            CssClass="clsTextBoxTagSearch" ToolTip="Enter Access" Width="225px"></asp:TextBox>
                                                    </td>


                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%-- <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>--%>
                                                    </td>
                                                    <td>

                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto">Note</asp:Label>

                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNote" runat="server" AutoComplete="off" ClientIDMode="Static" Text="<%# mMPDMaster.Note %>" TextMode="MultiLine"
                                                            CssClass="clsTextBoxTagSearchMultilineNewstyle2" ToolTip="Enter Note"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" causesvalidation="false"
                                                                                runat="server" class="clsbtnH" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH" Enabled="False" Text="Remove Attachment" ToolTip="Click to Remove Attachment" CausesValidation="false" />
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
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlAssemblyDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResultInspList" CssClass="clsLabelHeader" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgMonitorList" runat="server" CssClass="clsGridNewStyle" AllowSorting="false"
                                                        ShowHeaderWhenEmpty="true" PageSize="5" AllowPaging="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField DataField="AssemblyID" HeaderText="AssemblyID" SortExpression="AssemblyID"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="AssemblyStatusID" HeaderText="AssemblyStatusID" SortExpression="AssemblyStatusID"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="AssemblyStatusAsOndateFormatted" HeaderText="AssemblyStatusAsOndateFormatted"
                                                                SortExpression="AssemblyStatusAsOndateFormatted" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="HourType" HeaderText="HourType" SortExpression="HourType"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="AMPTaskNo" HeaderText="AMP Task No." SortExpression="AMPTaskNo">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>


                                                            <%--5--%>
                                                            <asp:BoundField DataField="ModelSerialNo" HeaderText="Model/Serial No." SortExpression="ModelSerialNo"
                                                                HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="true" Width="70px" />
                                                                <ItemStyle Wrap="true" Width="70px" />
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="RegNo" HeaderText="Aircraft" SortExpression="RegNo">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="ModelMonitorServiceCode" HeaderText="Task Type" SortExpression="ModelMonitorServiceCode">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="DoneOnFormatted" HeaderText="Last Done On" SortExpression="DoneOnFormatted">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                               <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="DoneOnWONo" HeaderText="Work Order No." SortExpression="DoneOnWONo">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="DoneRemark" HeaderText="Remark" SortExpression="DoneRemark">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Applicable Status" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsApplicable" runat="server" Text="" enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--12--%>
                                                            <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Threshold/ Interval"
                                                                HtmlEncode="false" SortExpression="FrequencyValueFormatted">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="true" Width="130px" />
                                                                <ItemStyle Width="130px" />
                                                            </asp:BoundField>
                                                            <%--13--%>
                                                            <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Effective From/ DoneOn Value"
                                                                HtmlEncode="false" SortExpression="DoneOnValueFormatted">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="true" Width="130px" />
                                                                <ItemStyle Width="130px" />
                                                            </asp:BoundField>
                                                            <%--14--%>
                                                            <asp:BoundField DataField="CurrentValueFormatted" HeaderText="Current" SortExpression="CurrentValueFormatted"
                                                                HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--15--%>
                                                            <asp:BoundField DataField="ElapsedValueFormattedForGrid" HeaderText="Elapsed" SortExpression="ElapsedValueFormatted"
                                                                HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--16--%>
                                                            <asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" SortExpression="ExtensionValueFormatted"
                                                                HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--17--%>
                                                            <asp:BoundField DataField="DueOnValueFormattedForGrid" HeaderText="Due At." SortExpression="DueOnValueFormatted"
                                                                HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--18--%>
                                                            <asp:BoundField DataField="AssemblyDueOnValueTextFormattedByAirFrameForGrid" HeaderText="Due At Airframe"
                                                                HtmlEncode="false" SortExpression="AssemblyDueOnValueTextFormattedByAirFrameForGrid">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--19--%>
                                                            <asp:BoundField DataField="RemainingValueFormattedForGrid" HeaderText="Remaining"
                                                                HtmlEncode="false" SortExpression="RemainingValueFormatted">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--20--%>
                                                            <asp:ButtonField CommandName="Configure" HeaderText="Configure" Text="Config">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <%--21--%>
                                                            <asp:BoundField DataField="IsConfigurable" HeaderText="IsConfigurable" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>

                                                            <%--22--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" Enabled='<%# (not Eval("IsMachineReadOnly")) And (Not Eval("IsConfigurable")) %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" Enabled='<%# (not Eval("IsMachineReadOnly")) And (Not Eval("IsConfigurable")) %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ViewRec" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("IsAttachmentAdded") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--   20
                                                            <asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                                <HeaderStyle HorizontalAlign="Left" /> 
                                                            </asp:ButtonField>
                                                            --21
                                                            <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                <HeaderStyle HorizontalAlign="Left" /> 
                                                            </asp:ButtonField>
                                                           22
                                                            <asp:ButtonField CommandName="History" HeaderText="History" Text="History" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Left" /> 
                                                            </asp:ButtonField>
                                                                 <%--22
                                                            <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            --%>


                                                            <%--23--%>
                                                            <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>

                                                            <%--24--%>
                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <%--25--%>
                                                            <asp:BoundField DataField="IsMachineReadOnly" HeaderText="IsMachineReadOnly" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <%--26--%>
                                                            <asp:BoundField DataField="ModelID" HeaderText="ModelID" SortExpression="ModelID"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
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
                </td>
            </tr>

        </table>
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
    </form>
</body>
</html>
