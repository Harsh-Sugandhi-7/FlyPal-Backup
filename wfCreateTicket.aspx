<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCreateTicket.aspx.vb" Inherits="Flypal.wfCreateTicket" Async="true" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="msgbox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<%--<head runat="server">
    <title>Create Ticket</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />

</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>


        <table id="Ticket" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clsPanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td valign="middle">
                                                        <span id="lbltitle" class="clsFormHeader">We Are here To help You.!</span>
                                                    </td>

                                                </tr>

                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearchDate"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxTagSearchDate"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSubject" runat="server" Text="Subject:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="clsTextBoxTagSearchMultilineNewstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>



    </form>
</body>--%>



<head>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<title>Create Zendesk Ticket</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
	<style>
		/* Add your custom styles here */
		body {
			font-family: Arial, sans-serif;
			background-color: #f4f4f4;
			margin: 0;
			padding: 0;
			display: flex;
			justify-content: center;
			align-items: center;
			height: 100vh;
		}

		.container {
			position: relative;
			width: 100%;
			background-color: #fff;
			max-width: 600px;
			margin: auto;
			padding: 20px;
			border-radius: 5px;
			box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
			width: 300px;
			/*overflow: hidden;*/
		}

		h1 {
			text-align: center;
			color: #333;
		}

		label {
			color: #555;
		}

		.close-btn {
			position: absolute;
			top: 10px;
			left: 300px;
			background-color: #f44336;
			color: white;
			border: none;
			border-radius: 50%;
			width: 30px;
			height: 30px;
			cursor: pointer;
			font-size: 20px;
			line-height: 30px;
			text-align: center;
			box-shadow: 0 0 5px rgba(0, 0, 0, 0.3); /* Add shadow for better visibility */
		}


		input[type="text"], input[type="email"], textarea {
			width: 100%;
			padding: 10px;
			margin-bottom: 10px;
			border: 1px solid #ccc;
			border-radius: 4px;
		}

		button {
			width: 100%;
			padding: 10px;
			background-color: #007BFF;
			border: none;
			border-radius: 4px;
			color: #fff;
			font-size: 16px;
			cursor: pointer;
		}
		/*    input[type="text"], input[type="email"], textarea {
            width: 100%;
            padding: 10px;
            margin-bottom: 10px;
            border: 1px solid #ccc;
                    border-radius: 4px;
                   
        }

        button {
                       width: 100%;
                       padding: 10px;
                       background-color: #007BFF;
                       border: none;
                       border-radius: 4px;
                       color: #fff;
                       font-size: 16px;
                       cursor: pointer;
                   
        }

               button:hover {
                       background-color: #0056b3;
                   
        }*/
		.submit-btn {
			background-color: #007BFF;
			color: white;
			padding: 12px 24px;
			font-size: 16px;
			font-weight: 500;
			border: none;
			border-radius: 6px;
			cursor: pointer;
			box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
			transition: background-color 0.3s ease, transform 0.2s ease;
		}

			.submit-btn:hover {
				background-color: #0056b3;
				transform: scale(1.05);
			}

			.submit-btn:active {
				background-color: #004085;
				transform: scale(0.98);
			}

		.error-labelRFQ {
			font-size: 12px;
			color: red;
			display: inline-block;
			margin-top: -4px; /* Pulls it closer vertically */
			margin-left: 2px; /* Slight horizontal nudge */
			position: relative;
			vertical-align: top;
		}

		/*error-labelRFQ::before {
            content: "⚠ ";
            font-weight: bold;
            margin-right: 2px;
        }*/

		textarea {
			resize: vertical; /* Allows only vertical resizing */
			max-height: 300px; /* Prevents it from growing too large */
			overflow: auto;
		}

		input[type="text"],
		input[type="email"],
		textarea {
			width: 100%; /* or a fixed width like 300px */
			box-sizing: border-box; /* ensures padding doesn't affect width */
		}
	</style>
</head>
<body>
	<div class="container" id="ticketContainer">
		<button class="close-btn" onclick="closeContainer()">×</button>
		<h1>Create a Ticket</h1>
		<form id="ticketForm" runat="server">
			<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
				runat="server">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<uc2:msgbox id="MSGBoxCtrl" runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>
			<table>
				<tr>
					<td></td>
					<td>
						<asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Enabled="false"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
					</td>

				</tr>
				<tr>
					<td><span id="Label1" class="clsLabelStar">*</span></td>
					<td>
						<asp:Label ID="lblSubject" runat="server" Text="Subject:"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtSubject" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator1"
							runat="server"
							ControlToValidate="txtSubject"
							ErrorMessage="Subject is required."
							ForeColor="Red"
							Display="Dynamic"
							CssClass="error-labelRFQ"
							SetFocusOnError="True" />

					</td>
				</tr>
				<tr>
					<td><span id="Label1" class="clsLabelStar">*</span></td>
					<td>
						<asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="clsTextBoxTagSearchMultilineNewstyle"></asp:TextBox>

						<asp:RequiredFieldValidator
							ID="rfvDescription"
							runat="server"
							ControlToValidate="txtDescription"
							ErrorMessage="Description is required."
							ForeColor="Red"
							Display="Dynamic"
							CssClass="error-labelRFQ"
							SetFocusOnError="True" />

					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<asp:Label ID="lblccEmails" runat="server" Text="CC:"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtCC" runat="server" TextMode="MultiLine" CssClass="clsTextBoxTagSearchMultilineNewstyle"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td colspan="3" align="right">
						<asp:FileUpload ID="fuAttachments" runat="server" AllowMultiple="true" />
					</td>
				</tr>
				<tr>
					<td colspan="3" align="right">
						<asp:Button ID="btnSubmit" runat="server" CssClass="submit-btn" CausesValidation="true" Text="Submit" OnClick="btnSubmit_Click" />
					</td>
					<asp:Button ID="hdnBtnClose" ClientIDMode="Static" runat="server" Text="----"
						CausesValidation="False" Style="display: none;"></asp:Button>
				</tr>
			</table>
		</form>
	</div>
	<script src="script.js"></script>


	<script>
		function closeContainer() {
			document.getElementById('ticketContainer').style.display = 'none';
			window.location.href = "dashboard.aspx";
			//  $("#hdnBtnClose").click();
		}
	</script>

</body>


</html>
