<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfQuotationListForComparison_Ajax.aspx.vb"
    Inherits="Flypal.wfQuotationListForComparison_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Quotation Item List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
       <script type="text/javascript" id="clientEventHandlersJS">
           function openFile() {
               str = "wfFileView.aspx"
               window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
           }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <script type="text/javascript">
        //        window.onload = DisableCheckBox;
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var dgQuotationList = document.getElementById("<%=dgQuotationList.ClientID %>");
            for (i = 1; i < dgQuotationList.rows.length; i++) {
                var Index = dgQuotationList.rows[i].innerText.toString().indexOf("No Quote");
                if (Index !== -1) {
                    dgQuotationList.rows[i].cells[0].getElementsByTagName("INPUT")[0].disabled = true;
                }
            }
        });   
    </script>
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
        });
    </script>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblPartList" class="clsFormHeader">Quotation Item List</span>
                                        </td>
                                        <td align="right" colspan="2">
                                            <asp:UpdatePanel ID="upnlTopActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnPrintTop" runat="server" Text="Print" CssClass="clsbtnH clsinfoH"
                                                                    ToolTip="Click to Print"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnByMailTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Report By Mail"
                                                                    />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCreateOrderTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to create Order"
                                                                    Text="Create Order"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous Page"
                                                                    Text="Back"></asp:Button>
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
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--  <asp:UpdatePanel ID="upnlPartDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTransactionDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'TransactionDate_watermarkextender','true');"
                                                                    Text="" Width="100px"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtTransactionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtTransactionDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="TransactionDate_watermarkextender" runat="server"
                                                                    TargetControlID="txtTransactionDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblSearch" class="clsLabelAuto">Part Search</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Part as per searching criteria"
                                                        Text="Find Now"></asp:Button>--%>

                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                        ToolTip="Click to find list of Part as per searching criteria" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td align="right" colspan="2">
                                            <asp:UpdatePanel ID="upnlTopActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnPrintTop" runat="server" Text="Print" CssClass="clsbtnH clsinfoH"
                                                                    ToolTip="Click to Print"></asp:Button>
                                                            </td>
                                                            <td>
                                                    <asp:Button ID="btnByMailTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Report By Mail"
                                                        Width="96px" />
                                                </td>
                                                            <td>
                                                                <asp:Button ID="btnCreateOrderTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to create Order"
                                                                    Text="Create Order"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous Page"
                                                                    Text="Back"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgQuotationList" DataKeyNames="QuotationItemID" runat="server"
                                                        AutoGenerateColumns="False"  PageSize="3" Width="100%" ShowHeaderWhenEmpty="True"
                                                     CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" OnDataBound="OnDataBound">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ItemID"></asp:BoundField>
                                                            <%--1--%>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <input type="checkbox" name="chkSelectList" class="cbSelectRow" value="<%# Eval("QuotationItemID") %>"
                                                                        onclick="EnableDisable(this);"></input>
                                                                    <input type="checkbox" id="chkItemID" name="chkItemIDList" class="cbSelectRow" value="<%# Eval("ItemName") %>"
                                                                        style="display: none;"></input>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Center"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                                                <HeaderStyle Wrap="true"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="QuotationTextNo" HeaderText="Quotation No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="QuotationDateFormatted" HeaderText="Quotation Date">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="QuotationItemQtyForGrid" HeaderText="Qty.">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="QuotationItemCRate" HeaderText="Rate">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="QuotationItemCAmount" HeaderText="Amount">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                           <%--10--%>
                                                             <asp:BoundField DataField="QuotationItemCRateAsPerBaseUnit" HeaderText="Rate As Per Base Unit">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                             <asp:BoundField DataField="ItemUnitName" HeaderText="Base Unit">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--12--%>
                                                            <asp:BoundField DataField="CurrencySymbol" HeaderText="Curr. Symbol">
                                                                <HeaderStyle Wrap="true"  HorizontalAlign="left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" HorizontalAlign="left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--13--%>
                                                            <asp:BoundField DataField="QuotationConversionFactor" HeaderText="Factor">
                                                                <HeaderStyle HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--14--%>
                                                            <asp:BoundField DataField="QuotationItemEffRate" HeaderText="Base Rate">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--15--%>
                                                            <asp:BoundField DataField="QuotationItemAmount" HeaderText="Base Amount">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--16--%>
                                                             <asp:BoundField DataField="QuotationItemEffRateAsPerBaseUnit" HeaderText="Base Rate As Per Base Unit">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--17--%>
                                                            <asp:BoundField DataField="QuotationItemEOQ" HeaderText="MOQ">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--18--%>
                                                            <asp:BoundField DataField="QuotationItemEOQCRate" HeaderText="EOQ Rate">
                                                                <HeaderStyle  HorizontalAlign="right"></HeaderStyle>
                                                                <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--19--%>
                                                            <asp:BoundField DataField="ItemType" SortExpression="ItemType" HeaderText="Cond.">
                                                                <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--20--%>
                                                           
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Remark">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="250"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' TextMode="MultiLine"
                                                                        ToolTip="Enter remark">
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <%--21--%>
                                                            <asp:BoundField DataField="DeliveryInDays" HeaderText="Lead Time (Days)">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--22--%>
                                                            <asp:BoundField Visible="False" DataField="QuotationItemID" HeaderText="QuotationItemID">
                                                            </asp:BoundField>
                                                            <%--23--%>
                                                            <asp:BoundField DataField="QuotationID" HeaderStyle-CssClass="hideGridColumn" HeaderText="QuotationID"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <%--24 --%>
                                                            <asp:BoundField DataField="RequisitionTextNo" HeaderText="Enq. No./Req. No." HtmlEncode="False">
                                                                <HeaderStyle HorizontalAlign="left" Wrap="false"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="left" Wrap="false"></ItemStyle>
                                                            </asp:BoundField>
                                                      
                                                         <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="View">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandName="ViewRec"
                                                                            CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' ImageUrl="icons/CLIP01.ICO"
                                                                        Visible='<%#  Eval("ImageSize")>0 %>'    Text="" Height="20px" Width="20px" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="20px" Height="20px" />
                                                                </asp:TemplateField>


                                                            <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle CssClass="hideGridColumn" />
                                                                <ItemStyle CssClass="hideGridColumn" />
                                                            </asp:BoundField>



                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <span id="lblNote" class="clsLabelAuto"><b>Note:</b> Please select record and click
                                                on print to save remark.</span>
                                        </td>
                                    </tr>
                                </table>
                                <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrintBottom" runat="server" Text="Print" CssClass="clsButton_Ajax"
                                                        ToolTip="Click to Print" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnByMailBottom" runat="server" CssClass="clsButton_Ajax" Text="Report By Mail"
                                                        Width="96px" Visible="false"/>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCreateOrder" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to create Order"
                                                        Text="Create Order" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous Page"
                                                        Text="Back" Visible="false"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;" align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForCommonPartList();
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
                parent.IFrameCommonPartListStateComplete();
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
            var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
            var windowheight=$(window).height();
            if (tempMargtop>=windowheight)
            {
            $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
            }
            else
            {
            var margintop=(windowheight/2)-(tempMargtop/2);
            $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
            }
       
        }
    </script>
    <%--End--%>
      <!-- Popup For Report By Mail-->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
        PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyReceipt1").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
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
    <script type="text/javascript">
        function EnableDisable(control) {
            var grid = $(control).closest("table");
            if (!$(control).is(":checked")) {
                var td = $("td", $(control).closest("tr"));
                $(control).closest("td").find("input[type=checkbox][id*=chkItemID]").attr("checked", false);
            } else {
                var td = $("td", $(control).closest("tr"));
                var s = $("#chkSelectList", td).val();
                $(control).closest("td").find("input[type=checkbox][id*=chkItemID]").attr("checked", true);
            }
        }
    </script>
</body>
</html>
