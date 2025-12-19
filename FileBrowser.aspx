<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="FileBrowser.aspx.vb" Inherits="Flypal.FileBrowser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="MSGBox.ascx" TagPrefix="uc2" TagName="MSGBox" %>

<!DOCTYPE html>
<html>
<head runat="server">
	<title>Legacy Data</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script src="js/query-1.7.1.js" type="text/javascript"></script>
	<style>
		/* File type icon colors */
		.icon-pdf {
			color: #e63946; /* Red for PDF */
		}

		.icon-word {
			color: #2b579a; /* Blue for Word */
		}

		.icon-excel {
			color: #217346; /* Green for Excel */
		}

		.icon-ppt {
			color: #d24726; /* Orange/Red for PowerPoint */
		}

		.icon-image {
			color: #8e44ad; /* Purple for images */
		}

		.icon-txt {
			color: #6c757d; /* Gray for text files */
		}

		.icon-zip {
			color: goldenrod; /* Gold for zip archives */
		}

		.icon-default {
			color: #555555; /* Neutral gray for unknowns */
		}
	</style>
	<style>
		.file-entry {
			display: flex;
			align-items: center;
			gap: 12px;
			padding: 7px;
			font-size: 13px;
			border-bottom: 1px solid #ddd;
		}

			.file-entry i {
				font-size: 15px;
				color: #444;
			}

		.file-name {
			word-break: break-word;
			flex-grow: 1;
		}
	</style>
	<style>
		.folder-entry {
			display: flex;
			align-items: flex-start;
			gap: 10px;
			padding: 10px 14px;
			font-size: 15px;
			border-bottom: 1px solid #e0e0e0;
			background-color: #f0f8ff; /* Light blue tint for folders */
			transition: background-color 0.2s ease;
		}

			.folder-entry:hover {
				background-color: #e6f2ff;
			}

		.folder-icon i {
			font-size: 20px;
			color: #f4b400; /* Golden folder color */
			min-width: 24px;
		}

		.folder-name {
			flex-grow: 1;
			word-break: break-word;
		}

			.folder-name a {
				text-decoration: none;
				color: #333;
				font-weight: 600;
			}

				.folder-name a:hover {
					text-decoration: underline;
				}

			.folder-name small {
				display: block;
				font-size: 12px;
				color: #666;
				margin-top: 4px;
			}
	</style>
</head>
<body>
	<form id="form2" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table border="0" id="tblMain" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
						<table id="tblinner" class="clstablelistin" border="0">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Digital Document Locker </asp:Label>
													</td>
													<td align="right">
														<asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
																	Text="Close" CausesValidation="False"></asp:Button>
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
									<div>
										<p>
											<%--<h2 class="clsFormHeader" color="black">Welcome here to view legacy data...!!</h2>--%>
											<asp:Label ID="Label2" runat="server" CssClass="clsFormHeader" ForeColor="black">Welcome here to view legacy data...!!</asp:Label>
										</p>
										<h2>
											<asp:Label CssClass="clsLabelAuto" Font-Size="13pt" ID="Label1" runat="server" Text=""></asp:Label>
										</h2>
										<span class="clsLabelAuto" font-size="30pt" height="100px">
											<asp:Literal ID="litContent" runat="server"></asp:Literal>
										</span>
									</div>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>


	</form>
</body>
</html>


<%--<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="FileBrowser.aspx.vb" Inherits="Flypal.FileBrowser" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>File Explorer</title>

    <!-- jsTree CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/jstree@3.3.15/dist/themes/default/style.min.css" />

    <style>
        /* Folder & file type icons */
        .jstree-folder > .jstree-icon {
            background: url('https://cdn-icons-png.flaticon.com/512/716/716784.png') no-repeat center center !important;
            background-size: 16px !important;
        }
        .jstree-file > .jstree-icon {
            background: url('https://cdn-icons-png.flaticon.com/512/337/337946.png') no-repeat center center !important;
            background-size: 16px !important;
        }
        .jstree-pdf > .jstree-icon {
            background: url('https://cdn-icons-png.flaticon.com/512/337/337946.png') no-repeat center center !important;
            background-size: 16px !important;
            filter: hue-rotate(330deg); /* red tint */
        }
        .jstree-word > .jstree-icon {
            background: url('https://cdn-icons-png.flaticon.com/512/732/732223.png') no-repeat center center !important;
            background-size: 16px !important;
        }
        .jstree-excel > .jstree-icon {
            background: url('https://cdn-icons-png.flaticon.com/512/732/732220.png') no-repeat center center !important;
            background-size: 16px !important;
        }
        .jstree-image > .jstree-icon {
            background: url('https://cdn-icons-png.flaticon.com/512/136/136524.png') no-repeat center center !important;
            background-size: 16px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>File Explorer</h2>
        <div id="jstree"></div>
    </form>

    <!-- jQuery + jsTree -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/jstree@3.3.15/dist/jstree.min.js"></script>

    <script>
        $(function () {
            $('#jstree').jstree({
                'core': {
                    'data': {
                        'url': 'FileBrowser.aspx?mode=json&path=',
                        'data': function (node) {
                            return { 'path': node.id === "#" ? "" : node.id };
                        }
                    }
                },
                'types': {
                    'folder': { 'icon': 'jstree-folder' },
                    'file': { 'icon': 'jstree-file' },
                    'pdf': { 'icon': 'jstree-pdf' },
                    'word': { 'icon': 'jstree-word' },
                    'excel': { 'icon': 'jstree-excel' },
                    'image': { 'icon': 'jstree-image' }
                },
                'plugins': ["types"]
            });

            // Handle file click → download
            $('#jstree').on("select_node.jstree", function (e, data) {
                if (data.node.original.type !== "folder") {
                    window.location = 'Download.aspx?path=' + encodeURIComponent(data.node.id);
                }
            });
        });
    </script>
</body>
</html>--%>
