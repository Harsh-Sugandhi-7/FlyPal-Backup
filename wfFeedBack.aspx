<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFeedBack.aspx.vb"
    Inherits="Flypal.wfFeedBack" %>

<!DOCTYPE html>
<script src="js/notificationFx.js"></script>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>FlyPal - Feedback</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@3.4.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript">

        function opennotificationpopup(Message, Type) {
            // Set message
            document.getElementById("popupMessage").innerText = Message;

            // Show popup
            document.getElementById("customPopup").style.display = "flex";

            // Handle OK button
            document.getElementById("popupOkBtn").onclick = function () {
                document.getElementById("customPopup").style.display = "none";
            };
        }


        //function open_mdlFeedBack() {
        //    $("#mdlFeedBack").modal('show');
        //}
        //function hide_mdlFeedBack() {
        //    $("#mdlFeedBack").modal('hide');
        //}
        // Ok Msg Pop up  
        function open_mdlMsgPopup_popup(message) {
            //document.getElementById("submitMessage").innerText = message;
            //document.getElementById("submitPopup").style.display = "flex";
            $("#mdlMsgPopup").modal('show');
        }

        //function hide_mdlMsgPopup_popup() {
        //    $("#mdlMsgPopup").modal('hide');
        //}
        //function closeSubmitPopup() {
        //    // Hide popup
        //    document.getElementById("submitPopup").style.display = "none";

        //}

    </script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@3.3.7/dist/js/bootstrap.min.js"></script>

    <style>
        .image-container {
            margin-left: 25%;
            width: 50%;
            height: 11em;
            margin-top: 10px;
            /* margin-left: -15em; */
            border: 1px solid #ccc;
            background-color: #ffffff;
            border-radius: 10px;
            overflow: hidden; /* Ensure image doesn't overflow */
            position: relative; /* Set position to contain the image */
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

            .image-container img {
                width: 100%; /* Make the image fill the div width */
                height: 100%; /* Make the image fill the div height */
                /* object-fit: cover;*/ /* Ensures the image maintains aspect ratio */
                position: absolute; /* Keeps the image positioned correctly */
            }


        body {
            background-color: #b3ceeb;
            font-family: Arial, sans-serif;
        }

        .divForm {
            margin-left: 25%;
            width: 50%;
            height: 11em;
            margin-top: 10px;
            /* margin-left: -15em; */
            border: 1px solid #ccc;
            background-color: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        .divForm_Header {
            margin-left: 25%;
            width: 50%;
            height: 16em;
            margin-top: 10px;
            /* margin-left: -15em; */
            border: 1px solid #ccc;
            background-color: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        .Header_lbl {
            padding-left: 20px;
        }

        .lbl {
            padding: 20px;
            /*  font-family: 'docs-Roboto';*/
            /*font-weight: 400;*/
            font-size: 12pt;
            line-height: 1.5;
            letter-spacing: 0;
        }

        .rating-container {
            background: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            text-align: center;
            width: 400px;
        }

        .rating-label {
            font-size: 16px;
            margin-bottom: 15px;
            display: block;
            font-family: Arial, sans-serif;
            font-weight: 100;
        }

        .radio-group {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 40px;
        }

            .radio-group label {
                display: flex;
                flex-direction: column;
                align-items: center;
                font-size: 14px;
            }

            .radio-group input[type="radio"] {
                display: none;
            }

                .radio-group input[type="radio"] + span {
                    width: 20px;
                    height: 20px;
                    border: 2px solid #888;
                    border-radius: 50%;
                    margin-bottom: 5px;
                    display: inline-block;
                    cursor: pointer;
                }

                .radio-group input[type="radio"]:checked + span {
                    background-color: #007bff;
                    border-color: #007bff;
                }

            .radio-group label span:hover {
                border-color: #007bff;
            }

            .radio-group .labels {
                display: flex;
                justify-content: space-between;
                width: 100%;
            }

                .radio-group .labels span {
                    font-size: 12px;
                    color: #666;
                }

        .Sp_class {
            font-family: Arial, sans-serif;
            font-weight: 100;
        }

        .lblChkbox {
            margin-top: 10px;
        }

        .txtMargin {
            margin-top: 12px;
        }

        .txtbox {
            border: 1px solid Gray;
            box-sizing: border-box;
        }



        .large-radio {
            width: 25px;
            height: 25px;
            cursor: pointer; /* Optional: Adds a pointer cursor for better UX */
        }
        /* Overlay background */
        .popup-overlay {
            display: none; /* hidden by default */
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }

        /* Popup box */
        .popup-content {
            background: #fff;
            padding: 20px 30px;
            border-radius: 10px;
            text-align: center;
            min-width: 250px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.3);
            animation: scaleIn 0.3s ease-out;
        }

            /* Message */
            .popup-content p {
                margin-bottom: 15px;
                font-size: 16px;
                color: #333;
            }

        /* OK Button */
        #popupOkBtn {
            background: #007BFF;
            color: #fff;
            padding: 8px 16px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }

            #popupOkBtn:hover {
                background: #0056b3;
            }

        /* Animation */
        @keyframes scaleIn {
            from {
                transform: scale(0.8);
                opacity: 0;
            }

            to {
                transform: scale(1);
                opacity: 1;
            }
        }

        .popup-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 9999;
        }

        .popup-content {
            background: #fff;
            padding: 20px 30px;
            border-radius: 10px;
            text-align: center;
            box-shadow: 0 4px 10px rgba(0,0,0,0.3);
            min-width: 300px;
        }

            .popup-content p {
                margin-bottom: 20px;
                font-size: 16px;
                color: #333;
            }

            .popup-content button {
                background: #007bff;
                color: white;
                border: none;
                padding: 8px 20px;
                border-radius: 5px;
                cursor: pointer;
                font-size: 14px;
            }

                .popup-content button:hover {
                    background: #0056b3;
                }
    </style>
</head>
<body>
    <!-- Popup Modal -->
    <div id="customPopup" class="popup-overlay" style="display: none;">
        <div class="popup-content">
            <p id="popupMessage"></p>
            <button id="popupOkBtn">OK</button>
        </div>
    </div>
    <%--    <!-- Popup Modal -->
    <div id="submitPopup" class="popup-overlay" style="display: none;">
        <div class="popup-content">
            <p id="submitMessage"></p>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button runat="server" ID="Button1" Text="Continue" CssClass="btn btn-info btn-xs" Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>--%>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="feedback-container">

            <!-- Header -->

            <div class="image-container">
                <img src="images/Bytzsoft_Form.jpg" alt="Example Image">
            </div>
            <div class="divForm_Header">
                <div style="padding: 20px;">
                    <h1>We’d Love Your Feedback!</h1>
                </div>
                <div class="Header_lbl">
                    <p>Your opinion helps us make the product better. Please take a minute to answer 5 quick questions. Your feedback will guide us in improving your experience!</p>
                </div>
                <div class="Header_lbl" style="margin-top: 20px;">
                    <p style="color: red; font-size: 12px;">* Indicates required question</p>
                </div>
            </div>

            <!-- Body -->
            <div class="feedback-body">

                <!-- Question 1 -->
                <div class="divForm">
                    <div class="lbl">
                        <label id="lblQuestion01" runat="server" class="rating-label">How satisfied are you with the product overall? <span style="color: red;">*</span></label>
                        <div class="radio-group">

                            <label>
                                <input runat="server" type="radio" name="rating" value="0">

                                <span style="display: none"></span>
                                <span class="Sp_class">Not satisfied at all</span>
                            </label>

                            <label>
                                <%--   <input type="radio" name="rating" value="1">--%>

                                <asp:RadioButton ID="rdbQ1_1" runat="server" AutoPostBack="false" GroupName="Q1" />
                                <span></span>
                                <span class="Sp_class">1</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ1_2" runat="server" AutoPostBack="false" GroupName="Q1" />
                                <span></span>
                                <span class="Sp_class">2</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ1_3" runat="server" AutoPostBack="false" GroupName="Q1" />
                                <span></span>
                                <span class="Sp_class">3</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ1_4" runat="server" AutoPostBack="false" GroupName="Q1" />
                                <span></span>
                                <span class="Sp_class">4</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ1_5" runat="server" AutoPostBack="false" GroupName="Q1" />
                                <span></span>
                                <span class="Sp_class">5</span>
                            </label>

                            <label>
                                <input type="radio" name="rating" value="6">
                                <span style="display: none"></span>
                                <span class="Sp_class">Very satisfied</span>
                            </label>

                        </div>
                        <%--<table class="radio-group">
                    <tr>
                        <td style="text-align: right; padding-right: 10px;">Not satisfied at all</td>

                        <td style="text-align: center;">
                            <asp:RadioButton ID="rbQ1_1" runat="server" GroupName="Q1" />
                        </td>
                        <td style="text-align: center;">
                            <asp:RadioButton ID="rbQ1_2" runat="server" GroupName="Q1" />
                        </td>
                        <td style="text-align: center;">
                            <asp:RadioButton ID="rbQ1_3" runat="server" GroupName="Q1" />
                        </td>
                        <td style="text-align: center;">
                            <asp:RadioButton ID="rbQ1_4" runat="server" GroupName="Q1" />
                        </td>
                        <td style="text-align: center;">
                            <asp:RadioButton ID="rbQ1_5" runat="server" GroupName="Q1" />
                        </td>

                        <td style="text-align: left; padding-left: 10px;">Very satisfied</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="Sp_class" style="text-align: center;">1</td>
                        <td class="Sp_class" style="text-align: center;">2</td>
                        <td class="Sp_class" style="text-align: center;">3</td>
                        <td class="Sp_class" style="text-align: center;">4</td>
                        <td class="Sp_class" style="text-align: center;">5</td>
                        <td></td>
                    </tr>
                </table>--%>
                    </div>
                </div>


                <%-- Question 2 --%>
                <div class="divForm">
                    <div class="lbl">
                        <label id="lblQuestion02" runat="server" class="rating-label">How easy is it to use our product? <span style="color: red;">*</span></label>
                        <div class="radio-group">

                            <label>
                                <input type="radio" name="rating1" value="0">
                                <span style="display: none"></span>
                                <span class="Sp_class">Very difficult</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ2_1" runat="server" AutoPostBack="false" GroupName="Q2" />
                                <span></span>
                                <span class="Sp_class">1</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ2_2" runat="server" AutoPostBack="false" GroupName="Q2" />
                                <span></span>
                                <span class="Sp_class">2</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ2_3" runat="server" AutoPostBack="false" GroupName="Q2" />
                                <span></span>
                                <span class="Sp_class">3</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ2_4" runat="server" AutoPostBack="false" GroupName="Q2" />
                                <span></span>
                                <span class="Sp_class">4</span>
                            </label>

                            <label>
                                <asp:RadioButton ID="rdbQ2_5" runat="server" AutoPostBack="false" GroupName="Q2" />
                                <span></span>
                                <span class="Sp_class">5</span>
                            </label>


                            <label>
                                <input type="radio" name="rating1" value="6">
                                <span style="display: none"></span>
                                <span class="Sp_class">Very easy</span>
                            </label>

                        </div>
                    </div>
                </div>

                <!-- Question 3 -->
                <div class="divForm">
                    <div class="lbl">
                        <label id="lblQuestion03" runat="server" class="rating-label">How well does the product perform? Is it fast and reliable?  <span style="color: red;">*</span></label>
                        <div class="radio-group">

                            <label>
                                <input type="radio" name="rating2" value="0">
                                <span style="display: none"></span>
                                <span class="Sp_class">Slow and unreliable</span>
                            </label>


                            <label>

                                <asp:RadioButton ID="rdbQ3_1" runat="server" AutoPostBack="false" GroupName="Q3" />
                                <span></span>
                                <span class="Sp_class">1</span>


                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ3_2" runat="server" AutoPostBack="false" GroupName="Q3" />
                                <span></span>
                                <span class="Sp_class">2</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ3_3" runat="server" AutoPostBack="false" GroupName="Q3" />
                                <span></span>
                                <span class="Sp_class">3</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ3_4" runat="server" AutoPostBack="false" GroupName="Q3" />
                                <span></span>
                                <span class="Sp_class">4</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ3_5" runat="server" AutoPostBack="false" GroupName="Q3" />
                                <span></span>
                                <span class="Sp_class">5</span>
                            </label>



                            <label>
                                <input type="radio" name="rating2" value="6">
                                <span style="display: none"></span>
                                <span class="Sp_class">Fast and reliable</span>
                            </label>
                        </div>
                    </div>
                </div>

                <!-- Question 4 -->
                <div class="divForm">
                    <div class="lbl">
                        <label id="lblQuestion04" runat="server" class="rating-label">Are the features of the product meeting your needs?  <span style="color: red;">*</span></label>
                        <div class="radio-group">

                            <label>
                                <input type="radio" name="rating3" value="0">
                                <span style="display: none"></span>
                                <span class="Sp_class">Not useful</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ4_1" runat="server" AutoPostBack="false" GroupName="Q4" />
                                <span></span>
                                <span class="Sp_class">1</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ4_2" runat="server" AutoPostBack="false" GroupName="Q4" />
                                <span></span>
                                <span class="Sp_class">2</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ4_3" runat="server" AutoPostBack="false" GroupName="Q4" />
                                <span></span>
                                <span class="Sp_class">3</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ4_4" runat="server" AutoPostBack="false" GroupName="Q4" />
                                <span></span>
                                <span class="Sp_class">4</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ4_5" runat="server" AutoPostBack="false" GroupName="Q4" />
                                <span></span>
                                <span class="Sp_class">5</span>
                            </label>
                            <label>
                                <input type="radio" name="rating3" value="6">
                                <span style="display: none"></span>
                                <span class="Sp_class">Exactly what I need</span>
                            </label>
                        </div>
                    </div>
                </div>

                <!-- Question 5-->
                <div class="divForm">
                    <div class="lbl">
                        <label id="lblQuestion05" runat="server" class="rating-label">How likely are you to recommend our product to others?  <span style="color: red;">*</span></label>
                        <div class="radio-group">

                            <label>
                                <input type="radio" name="rating4" value="0">
                                <span style="display: none"></span>
                                <span class="Sp_class">Not likely at all</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ5_1" runat="server" AutoPostBack="false" GroupName="Q5" />
                                <span></span>
                                <span class="Sp_class">1</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ5_2" runat="server" AutoPostBack="false" GroupName="Q5" />
                                <span></span>
                                <span class="Sp_class">2</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ5_3" runat="server" AutoPostBack="false" GroupName="Q5" />
                                <span></span>
                                <span class="Sp_class">3</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ5_4" runat="server" AutoPostBack="false" GroupName="Q5" />
                                <span></span>
                                <span class="Sp_class">4</span>
                            </label>
                            <label>
                                <asp:RadioButton ID="rdbQ5_5" runat="server" AutoPostBack="false" GroupName="Q5" />
                                <span></span>
                                <span class="Sp_class">5</span>
                            </label>
                            <label>
                                <input type="radio" name="rating4" value="6">
                                <span style="display: none"></span>
                                <span class="Sp_class">Very likely</span>
                            </label>
                        </div>
                    </div>
                </div>

                <!-- Suggestions -->
                <div class="divForm">
                    <div class="lbl">
                        <label class="rating-label">What could we improve to make your experience even better? </label>

                        <asp:TextBox TextMode="MultiLine" ID="txtSuggestionAnswer" runat="server" class="input-sm txtbox" Width="100%" Height="70px"
                            placeholder="Your answer"></asp:TextBox>

                    </div>
                </div>
                <!-- Checkbox -->
                <div style="margin-left: 25%; width: 50%; padding: 10px 0px 10px 0px;">
                    <asp:CheckBox ID="ChkContactBack" runat="server" AutoPostBack="true"
                        CssClass="checkbox-inline" Style="margin-top: -10px;" />
                    <asp:Label ID="lblContactBack" CssClass="lblChkbox" runat="server" Text="Do you require immediate assistance?"></asp:Label>
                </div>
                <div class="divForm_Header" id="divContactBack" runat="server" visible="false">

                    <div class="lbl">
                        <label id="lblContactDetails" runat="server" class="rating-label">Contact Details<span style="color: red;">*</span></label>
                        <hr />
                        <div class="txtMargin" style="display: inline-flex;">
                            <div style="width: 150px;">
                                <label id="lblContactNumber" runat="server" class="rating-label">Contact Number</label>
                            </div>
                            <div style="width: 400px; margin-left: 20px;">
                                <asp:TextBox TextMode="Phone" ID="txtContactNumber" runat="server" class=" input-sm txtbox" Width="300px"
                                    placeholder="Contact Number"></asp:TextBox>
                            </div>
                        </div>

                        <div class="txtMargin" style="display: inline-flex;">
                            <div style="width: 150px;">
                                <label id="lblContactEmail" runat="server" class="rating-label">Contact Email</label>
                            </div>
                            <div style="width: 400px; margin-left: 20px;">
                                <asp:TextBox TextMode="Email" ID="txtContactEmail" runat="server" class="input-sm txtbox" Width="300px"
                                    placeholder="Contact Email"></asp:TextBox>
                            </div>

                        </div>

                    </div>

                </div>

                <!-- Actions -->
                <div style="margin-left: 25%; width: 50%; padding: 10px 0px 10px 0px;">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" />
                    <a href="wfFeedBack.aspx" style="float: right;">Clear form</a>
                </div>
            </div>
        </div>
        <%-- POPUP on OK --%>
        <div class="modal fade" id="mdlMsgPopup" role="dialog" tabindex="-1"
            aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <asp:UpdatePanel ID="upnlMsgPopup" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="modal-header" style="background: #3498db;">
                                <h4 class="modal-title text-white">
                                    <asp:Label runat="server" ID="Label6" Text="Feedback"></asp:Label>
                                </h4>
                            </div>

                            <div class="modal-body">
                                <asp:Label runat="server" ID="lblMsglabel" CssClass="text-dark" Text=""></asp:Label>
                            </div>

                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnMsgOk" Text="Continue" CssClass="btn btn-info btn-xs" Visible="false" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>



</body>
</html>
