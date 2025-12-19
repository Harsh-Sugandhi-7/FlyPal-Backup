<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCalibrationItemList_Ajax.aspx.vb"
    Inherits="Flypal.wfCalibrationItemList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calibration Item List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <%-- <script type="text/javascript">
         //        window.onload = DisableCheckBox;
         Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
             var dgQuotationList = document.getElementById("<%=dgCalibrationItemList.ClientID %>");
             for (i = 1; i < dgCalibrationItemList.rows.length-1; i++) {
                 var Index = dgCalibrationItemList.rows[i].cells[21].innerText

                 if (Index <= 0) {
                     dgQuotationList.rows[i].cells[0].getElementsByTagName("INPUT")[0].disabled = true;
                 }
             }
         });   
    </script>--%>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
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
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblList" class="clsFormHeader">List of Calibration Items</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Calibration Item"
                                                                    Text="Add New" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCreateOrderTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to create Order"
                                                                    Text="Create Order" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the Calibration Items List."
                                                                    Text="Print" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Calibration Items screen"
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
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSearchCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td style="width: 47px">
                                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelMedium">Search</asp:Label>
                                                            </td>
                                                            <td style="width: 198px">
                                                                <table id="Table6" border="0" cellspacing="1" cellpadding="1">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbSearchCriteria" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                                <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                                                <asp:ListItem Value="1">Part No.</asp:ListItem>
                                                                                <asp:ListItem Value="2">Description</asp:ListItem>
                                                                                <asp:ListItem Value="3">Serial No.</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtItemName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                                MaxLength="100"></asp:TextBox>
                                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                                MaxLength="100"></asp:TextBox>
                                                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                                MaxLength="100"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table id="Table5" border="0" cellspacing="1" cellpadding="1" align="right">
                                                        <tr>
                                                            <td>
                                                                <%--<asp:Button AccessKey="F" ID="btnFindNow" runat="server" CssClass="clsButton_Ajax"
                                                                    Text="Find Now" ToolTip="Click to find list of Calibration Item as per searching criteria.">
                                                                </asp:Button>--%>


                                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                    ToolTip="Click to find list of Calibration Item as  per searching criteria" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Calibration Item as per criteria: Record(s) found.</asp:Label>
                                                </td>
                                                <%--<td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Calibration Item"
                                                                            Text="Add New" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                      <td>
                                                                       <asp:Button ID="btnCreateOrderTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to create Order"
                                                                       Text="Create Order"  CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the Calibration Items List."
                                                                            Text="Print" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Calibration Items screen"
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
                                                    <asp:GridView ID="dgCalibrationItemList" runat="server" AllowPaging="true"
                                                        DataKeyNames="ID,CalibrationItemID" AutoGenerateColumns="False" PageSize="25"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                        AllowSorting="true">
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <Columns>
                                                         <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <input type="checkbox" name="chkSelectList" class="cbSelectRow" value="<%# Eval("CalibrationItemID") %>"
                                                                       ></input>
                                                                    <input type="checkbox" id="chkItemID" name="chkItemIDList" class="cbSelectRow" value="<%# Eval("ItemName") %>"
                                                                        style="display: none;"></input>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                                <HeaderStyle></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle ></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptItemLocation" SortExpression="ReceiptItemLocation"
                                                                HeaderText="Location">
                                                                <HeaderStyle  Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                                    Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="ManufacturingDateFormatted" SortExpression="ManufacturingDateFormatted"
                                                                HeaderText="Manufacturing Date">
                                                                <HeaderStyle  Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                                    Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyWithPeriod" HeaderText="Frequency">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CalibrationNo" HeaderText="Calibration No." SortExpression="CalibrationNo">
                                                                <HeaderStyle ></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DoneOnDate" HeaderText="Done On Date">
                                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NextDueDate" HeaderText="Next Due Date">
                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IsApplicableTag" SortExpression="IsApplicableTag" HeaderText="Applicable">
                                                                <HeaderStyle ></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DonebyAgency" SortExpression="DonebyAgency" HeaderText="Done by Agency">
                                                                <HeaderStyle ></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CertificateReference" SortExpression="CertificateReference"
                                                                HeaderText="Certificate Reference">
                                                                <HeaderStyle ></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark/Order Info." SortExpression="Remark">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                                    Font-Strikeout="False" Font-Underline="False" Wrap="True" />
                                                            </asp:BoundField>
                                                           <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Comply" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>



                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    
                                                                    
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    
                                                                    
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>




                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Action" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="1000px" HeaderStyle-Width="1000px">   
                                                                <ItemTemplate> 
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">    
                                                                            <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ComplyRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                            CommandName="ComplyRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/Comply.jpg"
                                                                                            Enabled='<%#  Eval("IsApplicable")%>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                            CommandName="EditRecord" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                            CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("CalibrationItemID") %>'
                                                                                            CommandName="History" ImageUrl="~/images/History.png" Enabled='<%#  Eval("HistoryStatus")%>' ToolTip="History"/>
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                            CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                                            Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                    </td>


                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>


                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <asp:BoundField DataField="HistoryStatus" HeaderStyle-CssClass="hideGridColumn" HeaderText="HistoryStatus"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <asp:BoundField DataField="IsApplicable" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsApplicable"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                 <asp:BoundField DataField="StockBalanceQty"  ItemStyle-CssClass="hideGridColumn" 
                                                               HeaderText="StockBalanceQty"  HeaderStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        </Columns>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
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
                                <asp:UpdatePanel runat="server" ID="upnlActionBtn" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Calibration Item"
                                                        Text="Add New" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCreateOrder" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to create Order"
                                                        Text="Create Order" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the Calibration Items List."
                                                        Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                                <td> 
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Calibration Items screen"
                                                        Text="Close" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnCalibrationHistory" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnCalibrationItem" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnNewCalibrationItem" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
    <!--Calibration History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCalibrationHistory" Text="Calibration History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCalibrationHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCalibrationHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCalibrationHistory" runat="server" TargetControlID="btnDummyCalibrationHistory"
        PopupControlID="pnlCalibrationHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCalibrationHistoryStateComplete() {
            $("#btnDummyCalibrationHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCalibrationHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCalibrationHistory").attr("src", "wfCalibrationItemHistoryList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCalibrationHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCalibrationHistory() {
            var CalibrationHistorywindow = $find("<%=mdlPopupCalibrationHistory.ClientID %>");
            //close Calibration History popup window
            CalibrationHistorywindow.hide();
            //           release resources
            $("#IframeCalibrationHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCalibrationHistory").click();
        }
    </script>
    <!-- End-->
    <!--Calibration Item Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCalibrationItem" Text="Calibration Item" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCalibrationItem" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCalibrationItem" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCalibrationItem" runat="server" TargetControlID="btnDummyCalibrationItem"
        PopupControlID="pnlCalibrationItem" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCalibrationItemStateComplete() {
            $("#btnDummyCalibrationItem").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCalibrationItemWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCalibrationItem").attr("src", "wfComplyCalibrationItem_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCalibrationItem").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCalibrationItem() {
            var CalibrationItemwindow = $find("<%=mdlPopupCalibrationItem.ClientID %>");
            //close Calibration Item popup window
            CalibrationItemwindow.hide();
            //           release resources
            $("#IframeCalibrationItem").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCalibrationItem").click();
        }
    </script>
    <!-- End-->
    <!--New Calibration Item Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyNewCalibrationItem" Text="New Calibration Item"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlNewCalibrationItem" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeNewCalibrationItem" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupNewCalibrationItem" runat="server" TargetControlID="btnDummyNewCalibrationItem"
        PopupControlID="pnlNewCalibrationItem" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameNewCalibrationItemStateComplete() {
            $("#btnDummyNewCalibrationItem").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenNewCalibrationItemWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeNewCalibrationItem").attr("src", "wfCalibrationItem_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyNewCalibrationItem").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForNewCalibrationItem() {
            var NewCalibrationItemwindow = $find("<%=mdlPopupNewCalibrationItem.ClientID %>");
            //close New Calibration Item popup window
            NewCalibrationItemwindow.hide();
            //           release resources
            $("#IframeNewCalibrationItem").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnNewCalibrationItem").click();
        }
    </script>
    <!-- End-->
    </form>
    <script type="text/javascript">
        //        window.onload = EnableDisable();

        function EnableDisable() {
            var dgCalibrationItemList = document.getElementById("<%=dgCalibrationItemList.ClientID %>");
            for (i = 1; (i <= dgCalibrationItemList.rows.length - 1) && (i<26); i++) {
              try
              {
                var Index = dgCalibrationItemList.rows[i].cells[21].innerText

                if (Index <= 0) {
                    dgCalibrationItemList.rows[i].cells[0].getElementsByTagName("INPUT")[0].disabled = true;
                }
            }
            
             catch (e) {
                alert(e);
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            EnableDisable()
        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            EnableDisable()
        });   


    </script>
</body>
</html>
