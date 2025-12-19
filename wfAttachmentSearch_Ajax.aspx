<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAttachmentSearch_Ajax.aspx.vb"
    Inherits="Flypal.wfAttachmentSearch_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attachment List</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <style>
        body {
            font-family: 'Poppins', sans-serif;
            background: #f8f9fc;
            margin: 0;
            padding: 20px;
        }

        .card {
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 6px 20px rgba(0,0,0,0.1);
            max-width:100%; /*600px;*/
            margin: auto;
        }

        .header {
            background: linear-gradient(135deg, #4e73df, #1cc88a);
            padding: 16px;
            border-radius: 12px 12px 0 0;
            color: #fff;
        }

            .header h2 {
                margin: 0;
                font-size: 20px;
                font-weight: 700;
            }

        /* Flex container for controls */
        .header-controls {
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
            margin-top: 12px;
        }

        .clsLabelAuto {
            font-weight: 500;
            margin-right: 4px;
            color: #fff;
        }

        .myDropdown {
            font-family: 'Poppins', sans-serif;
            font-size: 14px;
            padding: 6px 10px;
            border: 1px solid #ccc;
            border-radius: 6px;
            background: #fff;
            cursor: pointer;
        }

        /* Search box with icon */
        .search-wrapper {
            position: relative;
            flex: 1;
        }

            .search-wrapper .search-icon {
                position: absolute;
                top: 50%;
                left: 14px;
                transform: translateY(-50%);
                color: #888;
                font-size: 14px;
                pointer-events: none;
            }

            .search-wrapper .search-input {
                width: 100%;
                padding: 8px 12px 8px 36px; /* left padding for icon */
                border: 1px solid #ccc;
                border-radius: 20px;
                font-size: 14px;
                outline: none;
                transition: box-shadow 0.3s ease;
            }

                .search-wrapper .search-input:focus {
                    border-color: #4e73df;
                    box-shadow: 0 0 5px rgba(78,115,223,0.4);
                }

        .myButton {
            font-family: 'Poppins', sans-serif;
            font-weight: 600;
            font-size: 14px;
            color: #fff;
            background: linear-gradient(135deg, #4e73df, #224abe);
            border: none;
            border-radius: 6px;
            padding: 8px 14px;
            cursor: pointer;
            transition: all 0.3s ease;
        }

            .myButton:hover {
                background: linear-gradient(135deg, #224abe, #1a2f6e);
                box-shadow: 0 3px 8px rgba(0,0,0,0.2);
            }

        .body {
            padding: 20px;
        }

        /* TreeView styling */
        .myTreeView {
            font-family: 'Poppins', sans-serif;
            font-size: 14px;
            color: #333;
            line-height: 1.6;
        }

            .myTreeView a {
                color: #333;
                text-decoration: none;
                padding: 2px 6px;
                border-radius: 4px;
                transition: background 0.2s ease, color 0.2s ease;
            }

                .myTreeView a:hover {
                    background: #f0f4ff;
                    color: #224abe;
                }

            .myTreeView .selected a {
                background: linear-gradient(135deg, #4e73df, #224abe);
                color: #fff !important;
                font-weight: 600;
            }

            /* Replace +/- with Font Awesome */
            .myTreeView .aspNetTreeView img {
                display: none !important;
            }

            .myTreeView .aspNetTreeView td img + a::before {
                font-family: "Font Awesome 6 Free";
                font-weight: 900;
                margin-right: 6px;
                content: "\f0da"; /* caret-right */
                color: #4e73df;
                transition: transform 0.2s ease;
            }

            .myTreeView .aspNetTreeView .Expanded a::before {
                content: "\f0d7"; /* caret-down */
                color: #224abe;
            }

        .clip-icon {
            width: 20px; /* adjust as needed */
            height: 20px; /* keep square for uniformity */
            vertical-align: middle; /* aligns with text */
            margin-left: 5px; /* spacing from filename */
        }
    </style>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script language="javascript" id="clientEventHandlersJS">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
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
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="card">
            <div class="header">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2>Attachment List</h2>
                        <div class="header-controls">
                            <label for="cmbYear" class="clsLabelAuto">Year</label>
                            <asp:DropDownList ID="cmbYear" runat="server" CssClass="myDropdown" AutoPostBack="true"></asp:DropDownList>

                            <%--<label for="cmbMonth" class="clsLabelAuto">Month</label>
                    <asp:DropDownList ID="cmbMonth" runat="server" CssClass="myDropdown" AutoPostBack="true"></asp:DropDownList>--%>

                            <!-- Search box -->
                            <div class="search-wrapper">
                                <i class="fa fa-search search-icon"></i>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input"
                                    ToolTip="Enter Search Criteria" AutoPostBack="true"
                                    placeholder="Search here" autocomplete="off" Width="300px"></asp:TextBox>
                            </div>

                            <asp:Button ID="btnCloseTop" runat="server" CssClass="myButton"
                                ToolTip="Click to close"
                                Text="Close" CausesValidation="False"></asp:Button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="body">
                <asp:UpdatePanel ID="upnlTreeView" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TreeView ID="TreeView1" runat="server"
                            CssClass="myTreeView aspNetTreeView"
                            ExpandDepth="0"
                            ShowExpandCollapse="True"
                            ShowLines="False"
                            OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                        </asp:TreeView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: transparent; top: 0; z-index: 99999;"></div>
                    <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle" Height="48px" Width="48px" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <!-- WorkOrderAttach Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
                ClientIDMode="Static" />
        </div>

        <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>

        <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
            PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>

        <script type="text/javascript">
            function IFrameAttachStateComplete() {
                $("#btnDummyAttach").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenAttachWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAttach").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForAttach() {
                var Attachwindow = $find("<%=mdlAttach.ClientID %>");
                //close popup window
                Attachwindow.hide();
                //release resources
                $("#IframeAttach").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnAttach").click();
            }
        </script>
        <!-- End -->
    </form>
</body>
</html>
