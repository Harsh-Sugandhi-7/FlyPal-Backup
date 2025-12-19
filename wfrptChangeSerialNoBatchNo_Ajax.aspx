<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptChangeSerialNoBatchNo_Ajax.aspx.vb"
    Inherits="Flypal.wfrptChangeSerialNoBatchNo_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Change Serial No. / Batch No.</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
    </script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtReceiptTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=21', {
                width: 185,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
    <style type="text/css">
        .activerow {
            /* yellow*/
            background-color: rgb(255, 203, 96);
        }
    </style>
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
        <div style="width: 100%">
            <table id="tblmain" class="clstablelistout" border="0">
                <tr>
                    <td colspan="6">
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <div style="width: 100%">
                                <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr class="clsFormHeader1Newstyle">
                                                <td colspan="6">
                                                    <table width="100%" class="clsFormHeader">
                                                        <tr>
                                                            <td>
                                                                <span id="spntitle" class="clsFormHeader" runat="server">Change Serial No. / Batch No.</span>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    ToolTip="Click to Close Change Serial No./Batch No. Screen" Text="Close"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="spnPartNo" class="clsLabelMedium">Part No.</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"
                                                        ToolTip="Enter Part No."></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblSerailNo" class="clsLabel">Serial No.</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
                                                        ToolTip="Enter Serial No."></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblReceiptNo" class="clsLabel">Receipt No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReceiptTextList" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Receipt Text."
                                                        Wrap="False"></asp:TextBox>
                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="5"
                                                        ToolTip="Enter Receipt No."></asp:TextBox>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <span id="lblBatchNo" class="clsLabel">Batch No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBatchNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
                                                        ToolTip="Enter Batch No.">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
                                                        CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
                                                        ValidationGroup="1" CausesValidation="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div style="width: 100%">
                                <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts : </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgPartSearch" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        EnableViewState="false" PagerSettings-Mode="NumericFirstLast"
                                                        PageSize="10" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                            <asp:BoundField DataField="ItemID" HeaderText="ItemID" Visible="False" />
                                                            <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                                <HeaderStyle Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDesc" HeaderText="Description" SortExpression="ItemDesc">
                                                                <HeaderStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Receipt Date">
                                                                <HeaderStyle />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptNo">
                                                                <HeaderStyle Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BatchNo" HeaderText="Batch No." SortExpression="BatchNo"
                                                                ItemStyle-Wrap="true">
                                                                <HeaderStyle />
                                                                <ItemStyle Wrap="True" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PartSerialisedStatus" HeaderText="Serialized Status" SortExpression="PartSerialisedStatus">
                                                                <HeaderStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerializedStatus" HeaderText="PartSerializedStatus" Visible="False" />
                                                            <asp:ButtonField CommandName="Change" HeaderText="Change" Text="Change">
                                                                <HeaderStyle Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <%--Serial No. Batch No.--%>`
            <asp:Panel runat="server" ID="pnlSerialNoBatchNo" CssClass="clspanel1">
                <div style="display: none">
                    <asp:Button runat="server" ID="btndummyPartStore" Text="Dummy Part Type" />
                </div>
                <div style="width: 100%">
                    <asp:UpdatePanel runat="server" ID="upnlSerialNoBatchNo" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="clstablelistout" width="100%">
                                <tr class="clsFormHeader1Newstyle">
                                    <td colspan="4">
                                        <table width="100%" class="clsFormHeader">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Change Serial No. / Batch No.</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSaveSerialNo" runat="server" CssClass="clsbtnH clsinfoH"
                                                        ToolTip="Click To Save New Serial No./Batch No." Text="Save"></asp:Button>
                                                    <asp:Button ID="btnCloseModal" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                        ToolTip="Click to Close" Text="Close"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="spnPartNumber" class="clsLabelAuto">Part No. </span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Part No."
                                            BackColor="#E0E0E0" MaxLength="50" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="spnSerilisedStatus" class="clsLabelAuto">Serialized Status</span>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkSerialized" runat="server" Enabled="False" CssClass="clsLabelAuto"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="spnCurrentSerailNo" class="clsLabelAuto">Old Serial No. </span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOldSerialNo" runat="server" CssClass="clsTextBoxTagSearch"
                                            ReadOnly="True" MaxLength="50" BackColor="#E0E0E0" ToolTip="Old Serial No."></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="spnOldBatchNo" class="clsLabelAuto">Old Batch No.</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOldBatchNo" runat="server" CssClass="clsTextBoxTagSearch"
                                            ReadOnly="True" MaxLength="50" BackColor="#E0E0E0" ToolTip="Old Batch No."></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="spnChangeSerialNo" class="clsLabelAuto">New Serial No.</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewSerialNo" runat="server" CssClass="clsTextBoxTagSearch"
                                            MaxLength="50" ToolTip="Enter New Serial No."></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="spnChangeBatchNo" class="clsLabelAuto">New Batch No.</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewBatchNo" runat="server" CssClass="clsTextBoxTagSearch"
                                            MaxLength="50" ToolTip="Enter New Batch No."></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader" Font-Bold="True">List Of Receipts Having Same Serial No And Part No.</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView runat="server" ID="dgPartNoSrNo" EnableViewState="false" PageSize="5"
                                            ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                            AllowPaging="true" PagerSettings-Mode="NumericFirstLast">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ItemID"></asp:BoundField>
                                                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DateFormatted" HeaderText="Receipt Date">
                                                    <HeaderStyle></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RCIType" SortExpression="RCIType" HeaderText="From">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BatchNo" SortExpression="BatchNo" HeaderText="Batch No.">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="spanNote" class="clsLabelHeader">* Note : Above list of Receipts will also
                                        be affected when a Serial No. is changed. </span>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <cc2:ModalPopupExtender runat="server" ID="mdlSerialNoBatchNo" TargetControlID="btndummyPartStore"
                PopupControlID="pnlSerialNoBatchNo" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <%--End--%>
        </div>
        <%--My Message Box--%>
        <cc2:ModalPopupExtender ID="mdlPopupExit" runat="server" TargetControlID="Button2"
            PopupControlID="pnlExitMsg" CancelControlID="btnNo" BackgroundCssClass="ModalPopupBG">
        </cc2:ModalPopupExtender>
        <div style="display: none">
            <asp:Button ID="Button2" runat="server" Height="0px" Width="0px" />
        </div>
        <div>
            <asp:Panel runat="server" ID="pnlExitMsg" Height="150px" Width="400px">
                <div>
                    <div class="msgBoxShadow">
                        <div class="clsMsgBoxOuter">
                            <asp:Panel ID="TitleBar" runat="server">
                                <asp:UpdatePanel ID="upnlMessageBox" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div>
                                            <asp:Label runat="server" ID="lblMessageTitle" CssClass="clsMsgBoxTitle">Delete Confirmation!</asp:Label>
                                        </div>
                                        <div class="clsMsgBoxBody">
                                            <div class="clsMsgBoxInnerBody">
                                                <div style="padding: 10px; min-height: 20px;">
                                                    <div class="clsMsgInfoIcon">
                                                    </div>
                                                    <div class="clsMsgContent">
                                                        <asp:Label runat="server" ID="lblMessage" CssClass="clsMsgText">You have clicked on the Delete link to Delete this entry.</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clsMsgBoxFooterWrap">
                                            <div class="clsMsgBoxFooter" id="ButtonDiv" runat="server">
                                                <asp:Button ID="btnYes" runat="server" Text="Yes" Width="100px" CssClass="clsbtnH clsinfoH" />
                                                <asp:Button ID="btnNo" runat="server" Text="No" Width="100px" CssClass="clsbtnH clsinfoH" />
                                                <asp:Button ID="btnOk" runat="server" Text="Ok" Width="100px" Visible="false" CssClass="clsbtnH clsinfoH" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <%----------%>
        <%--To highlight grid row--%>
        <input id="gridrowindex" type="hidden" value="" />
        <input id="Hidden1" type="hidden" value="" />
        <input id="gridrowaction" type="hidden" value="" />
        <input id="TempValue" type="hidden" value="" />
        <script type="text/javascript">
            $(document).ready(function () {
                $("#<%=dgPartSearch.ClientID %> tr td a").live("click", function () {
                var temp = $(this).parent().parent()[0].rowIndex;
                $("#gridrowindex").val(temp);
                $("#gridrowaction").val('gridrow');
            });
        });
        </script>
        <script type="text/javascript">
            //event handler for end request i.e last event in client page cycle.
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            //event handler for begin request i.e before sending request to the server
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

            var element;
            var timerId;
            var timeoutforblink;
            var hideRowHighlight = false;

            function endRequestHandler(sender, args) {
                var tempval = parseInt($("#gridrowindex").val());
                if (tempval) {
                    $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
                var elem;
                var tempaction = $("#gridrowaction").val();
                if (hideRowHighlight) {

                    if (tempaction == "SerialNo") {
                        $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
                        elem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(2),#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(5)");
                        $("#gridrowaction").val('');
                    }
                    else if (tempaction == "SerialNoOnYesButton") {
                        $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
                        elem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(2),#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(5)");
                        $("#gridrowaction").val('');
                    }
                    else if (tempaction == "close") {
                        $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
                        $("#TempValue").val('');
                        $("#gridrowaction").val('');

                        return;
                    }
                    else {
                        return;
                    }
                    timeoutforblink = setInterval(function () {
                        if (elem.hasClass('activerow')) {
                            elem.removeClass('activerow');
                        }
                        else {
                            elem.addClass('activerow');
                        }

                    }, 500);
                    //stop blink column
                    timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);

                }
                else {
                    if (tempaction == "close") {
                        $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
                            $("#TempValue").val('');
                            $("#gridrowaction").val('');
                        }
                    }
                }
            }


            function BeginRequestHandler(sender, args) {
                clearTimeout(timerId);
                element = args.get_postBackElement();
                //change location popup ok button event occur
                if (element.id == "btnSaveSerialNo") {
                    hideRowHighlight = true;
                    $("#gridrowaction").val('SerialNo');
                    $("#TempValue").val("Changed") //After saving value gets change
                }
                else if (element.id == "btnYes") {
                    hideRowHighlight = true;
                    $("#gridrowaction").val('SerialNoOnYesButton');
                    $("#TempValue").val("Changed") //After saving value gets change
                }

                //any of change popup close button event occur 
                else if (element.id == "btnClose" || element.id == "btnNo" || element.id == "btnOk") {
                    if ($("#TempValue").val() == "Changed") {
                        hideRowHighlight = true;
                        $("#gridrowaction").val('close');
                    }
                    else if ($("#TempValue").val() == "") {
                        hideRowHighlight = false;
                        $("#gridrowaction").val('close');
                    }

                }
                //change parttype ||change location link event occur
                //reset rowindex value if other grid event occurs
                else if (element.id == "dgPartSearch") {
                    if ($("#gridrowaction").val() != "gridrow") {
                        $("#gridrowindex").val('');
                    }
                }
                //any other events
                else {
                    $("#gridrowindex").val('');
                }
            }

            //stop blinking
            function TimeOut(val, action) {
                var tempelem;
            //          if (action == "SerialNo") {
            //              tempelem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(2)");
            //              tempelem.removeClass('activerow');

            //          }
            //          else 
            if (action == "SerialNo") {
                tempelem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(2),#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(5)");
                    tempelem.removeClass('activerow');
                }
                else {
                    return;
                }
                $("#gridrowindex").val('');
                hideRowHighlight = false;
                clearInterval(timeoutforblink);
            }
        </script>
        <%-------------------------%>
    </form>
</body>
</html>
