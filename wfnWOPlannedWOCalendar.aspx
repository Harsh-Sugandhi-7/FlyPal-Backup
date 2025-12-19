<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOPlannedWOCalendar.aspx.vb"
    Inherits="Flypal.wfnWOPlannedWOCalendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WO Planning</title>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="JQGridReq/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <link href="FullCalendar/fullcalendar.css" rel="stylesheet" type="text/css" />
    <link href="FullCalendar/fullcalendar.print.min.css" rel="stylesheet" type="text/css"
        media='print' />
    <style>
        #script-warning
        {
            display: none;
            background: #eee;
            border-bottom: 1px solid #ddd;
            padding: 0 10px;
            line-height: 40px;
            text-align: center;
            font-weight: bold;
            font-size: 12px;
            color: red;
        }
        
        #loading
        {
            display: none;
            position: absolute;
            top: 10px;
            right: 10px;
        }
        #calendar .dot-event
        {
            width: 0.1em;
            height: 0.1em;
            border-radius: 50%;
            display: inline-block;
            margin-left: 5px;
            vertical-align: text-bottom;
        }
        .modal-header
        {
            padding: 9px 15px;
            border-bottom: 1px solid #eee;
            background-color: #0480be;
            -webkit-border-top-left-radius: 5px;
            -webkit-border-top-right-radius: 5px;
            -moz-border-radius-topleft: 5px;
            -moz-border-radius-topright: 5px;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
        }
        .modal-header.primary
        {
            color: #fff;
            background-color: #337ab7;
            border-color: #337ab7;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script src="FullCalendar/moment.min.js" type="text/javascript"></script>
    <script src="FullCalendar/jquery.min.js" type="text/javascript"></script>
    <script src="FullCalendar/fullcalendar.min.js" type="text/javascript"></script>
    <script src="bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <!-- Main content -->
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="center">
                        <span class="clsLabel" style="font-size: medium">Status</span>
                        <div>
                            <asp:UpdatePanel ID="upnlWOStatus" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsComboBox1_Ajax" DataTextField="Name"
                                        ClientIDMode="Static" DataValueField="ID">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td align="center">
                        <span class="clsLabel" style="font-size: medium">Customer</span>
                        <div>
                            <asp:UpdatePanel ID="upnlCustomer" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="cmbCustomerList" runat="server" CssClass="clsComboBox_Ajax"
                                        DataTextField="Name" ClientIDMode="Static" DataValueField="ID">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
            <div style="height: 9px;">
            </div>
            <div style="width: 90%; display: inline-block;" id="calendar">
                <button id="prev-year" style="display: none">
                    Prev year</button>
                <select id="months-tab" style="display: none">
                    <option data-month="0">January</option>
                    <option data-month="1">February</option>
                    <option data-month="2">March</option>
                    <option data-month="3">April</option>
                    <option data-month="4">May</option>
                    <option data-month="5">June</option>
                    <option data-month="6">July</option>
                    <option data-month="7">August</option>
                    <option data-month="8">September</option>
                    <option data-month="9">October</option>
                    <option data-month="10">November</option>
                    <option data-month="11">December</option>
                </select>
            </div>
            <p>
                <div>
                    <asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="Label2" BackColor="#7d5c7c"
                        ForeColor="#7d5c7c">OPEN</asp:Label>
                    <span class="clsTextBox_Ajax">OPEN</span>
                    <asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="lbl1" BackColor="#d934d1"
                        ForeColor="#d934d1">OPEN</asp:Label>
                    <span class="clsTextBox_Ajax">Authorized</span>
                    <asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="Label3" BackColor="#ccc62b"
                        ForeColor="#ccc62b">OPEN</asp:Label>
                    <span class="clsTextBox_Ajax">Planned</span>
                    <asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="Label1" BackColor="#014501"
                        ForeColor="014501">OPEN</asp:Label>
                    <span class="clsTextBox_Ajax">Completed</span>



           
                </div>
                <p>
                </p>
                <%--Bootstrap Modal POPUP--%>
                <div id="successModal" aria-hidden="true" aria-labelledby="successModalLabel" class="modal fade"
                    role="dialog" tabindex="-1">
                    <div class="modal-dialog" role="dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button aria-label="Close" class="close" data-dismiss="modal" type="button">
                                    <span aria-hidden="true">×</span>
                                </button>
                                <h4 class="modal-title">
                                    <p>
                                    </p>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                    <h4>
                                    </h4>
                                </h4>
                            </div>
                            <div class="modal-body primary">
                                <span aria-hidden="true"></span>
                                <p>
                                </p>
                            </div>
                            <%-- <div class="modal-footer">
                            <button type="button" class="btn btn-info" data-dismiss="modal">
                                Plan</button>
                        </div>--%>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>
                <!-- /.modal -->
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#calendar').fullCalendar('rerenderEvents');

        });
    </script>
      <script src="jquery.tooltip.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function FullCalendarDueFunc() {
           $('#calendar').fullCalendar({
                schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source',

                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: ''
                    //   right: 'month,agendaWeek,agendaDay'
                },

                defaultView: 'month',
                defaultDate: new Date(),
                editable: true,
                navLinks: false, // can click day/week names to navigate views
                height: 350,
                selectable: true,
                //  selectHelper: true,
                //slotMinutes: 15,
                allDayDefault: false,
                buttonText: {
                    today: 'today',
                    month: 'month',
                    week: 'week',
                    day: 'day'
                },

                events: function (start, end, timezone, callback) {
                    WOStatusID=document.getElementById("hdnStatus").value;
                    CustomerID=document.getElementById("hdnCustomer").value;
                    var date = new Date($('#calendar').fullCalendar('getDate'));
                    var month_int = date.getMonth();
                    var year_int = date.getFullYear();
                    $.ajax({
                        type: "POST",
                      //  data: "{ 'WOStatusID': '" + WOStatusID + "'}",
                      //  data: "{ 'WOStatusID': '" + WOStatusID + "', 'CustomerID': '" + CustomerID + "' }",
                        data: "{ 'WOStatusID': '" + WOStatusID + "', 'CustomerID': '" + CustomerID + "', 'month': '" + month_int + "', 'year': '" + year_int + "' }",
                        url: "wfnWOPlannedWOCalendar.aspx/TestOnWebService",
                        dataType: 'json',
                        contentType: "application/json",

                        success: function (data) {
                            var events = [];
                            var obj = jQuery.parseJSON(data.d);
                            $(obj).each(function () {
                                //var nowdate = new Date($(this).attr('start')).toDateString("yyyy-MM-dd");
                                events.push({
                                    title: $(this).attr('title'),
                                    start: $(this).attr('start'), // will be parsed
                                    WOStatus: $(this).attr('WOStatus'),
                                    WOStatusid: $(this).attr('WOStatusid'),
                                    DescriptionCalender: $(this).attr('DescriptionCalender'),
                                    CustomerName:$(this).attr('CustomerName'),
                                    IsBillingRequiredStatus:$(this).attr('IsBillingRequiredStatus'),
                                    IsCAMOUpdatedStatus:$(this).attr('IsCAMOUpdatedStatus'),
                                    IsQCStatusApprovedStatus:$(this).attr('IsQCStatusApprovedStatus'),
                                    IsQCStatusApproved:$(this).attr('IsQCStatusApproved'),
                                    BillingRequired:$(this).attr('BillingRequired'),
                                    StatusId:$(this).attr('StatusId'),
                                    className: ["user_block", "bday_block"],
                                    tooltip: $(this).attr('WOStatus'),
                                    // color: '#BEEABE',
                                    allday: true,
                                    // eventLimit: 6
                                    });
                                });
                                callback(events);
                            },
                            error: function (xhr, status, error) {

                                alert(xhr.responseText + "i am in error");

                            }

                        });


                    }, //events ends
                    displayEventTime: false,
                    eventRender: function (event, element) {
                      
                                $(element).tooltip({
                                title: event.DescriptionCalender,
                                placement: "top",
                                trigger: "hover",
                                container: "body",
                                html:true,
                                animation:true
                        
                            });
                      
                      //  element.attr('title', event.tooltip);
                        element.css("font-size", "0.7em");
                        element.find('.fc-title').html(event.title);
                        element.css("padding", "5px");
                        //                     element.css('background-color', '#FF0000');
//                        if (event.WOStatusid == "1") {
//                            element.css('background-color', '#d934d1'); //pink : Open
//                        }
//                        else if (event.WOStatusid == "3") {
//                            element.css('background-color', '#014501'); //dark green : Complete
//                        }
//                        else if (event.WOStatusid == "4") {
//                            element.css('background-color', '#ccc62b'); //yellow : Planned
//                        }
//                        else if (event.WOStatusid == "5") {
//                            element.css('background-color', '#21c416'); //Light Green : QC Approved
//                        }
//                        else if (event.WOStatusid == "6") {
//                            element.css('background-color', '#FF0000'); //red : QC Rejected
//                        }
//                        else if (event.WOStatusid == "8") {
//                            element.css('background-color', '#662bcc'); //Purple :  CAMO Updated
//                            // element.css('background-color', '#008000');

//                        }
                         if (event.IsBillingRequiredStatus == "" && event.IsCAMOUpdatedStatus == "" && event.IsQCStatusApprovedStatus == "") {
                                if (event.WOStatusid == "1") {
                                 
                                   if (event.WOStatus == "Authorized") {
                                    element.css('background-color', '#d934d1'); //pink : Authorized
                                   }
                                   else {
                                    element.css('background-color', '#7d5c7c'); //light pink : Open
                                   }
                                }
                                else if (event.WOStatusid == "3") {
                                    element.css('background-color', '#014501'); //dark green : PPC Complete
                                }
                                else if (event.WOStatusid == "4") {
                                    element.css('background-color', '#ccc62b'); //yellow : Planned
                                }
                                else if (event.WOStatusid == "7") {
                                    element.css('background-color', '#21c416'); //dark green : AME Complete
                             }
                         }
                         else {
                                if (event.IsQCStatusApprovedStatus == "QC Rejected"){
                                       element.css('background-color', '#f20707'); //red : QC Rejected                                
                                }
                                else if (event.BillingRequired == "2") {
                                        element.css('background-color', '#05f7c3'); //Billing Not Required
                                }
                                else {
                                      element.css('background-color', '#662bcc'); //Purple :  QC Approved,CAMO Updated or Billing done
                                }

                         }

                    }, //eventRender ends
                    eventClick: function (event, jsEvent, view) {



                        $("#successModal").modal("show");
                        // $("#successModal .modal-body p").text(' \n<h3>Reg No. :</h3> ' + event.RegNo + ' \nStatus: ' + event.WOStatus);
                        $("#successModal .modal-body p").html(event.DescriptionCalender);
                        $("#successModal .modal-title p").html(event.title);


                        return false;
                    }

                });
                //calendar ends
                //            $('#cmbStatus').on('change', function () {
                //                //  console.log("Event");
                //                alert('abc');
                //               // $('#calendar').fullCalendar('rerenderEvents');
                //            });

            }

            //        $(function () {

            //    });
    </script>
    <script type="text/javascript">
        // change month
        $('#months-tab').on('change', function () {
            // get month from the tab. Get the year from the current fullcalendar date
            var month = $(this).find(":selected").attr('data-month'),
        year = $("#calendar").fullCalendar('getDate').format('YYYY');

            var m = moment([year, month, 1]).format('YYYY-MM-DD');

            $('#calendar').fullCalendar('gotoDate', m);
        });

        // go to prev year
        $("#prev-year").on('click', function () {
            //  alert('ss');
            $('#calendar').fullCalendar('prevYear');

        });

        // set the month as the current month
        // has to be -1 because this is 0 based
        var month = $(this).find(":selected").attr('data-month') - 1;  //$("#calendar").fullCalendar('getDate').format('MM') - 1;
        // set the correct month selected
        $("#months-tab").find('option[data-month=' + month + ']').prop('selected', true);

    
    </script>
    <%--    <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick">
    </asp:Timer>--%>
    <asp:HiddenField ID="hdnStatus" runat="server" />
    <asp:HiddenField ID="hdnCustomer" runat="server" />
    </form>
    <script type="text/javascript">
        $("#cmbStatus").change(function () {
            //  $('#calendar').fullCalendar('rerenderEvents');
            document.getElementById("hdnStatus").value = $("#cmbStatus").val();
            //  $('#calendar').fullCalendar('refetchEvents');
            FullCalendarDueFunc();

            $('#calendar').fullCalendar('refetchEvents');
            //  alert('abc');
        });
        $("#cmbCustomerList").change(function () {
            //  $('#calendar').fullCalendar('rerenderEvents');
            document.getElementById("hdnCustomer").value = $("#cmbCustomerList").val();
            //  $('#calendar').fullCalendar('refetchEvents');
            FullCalendarDueFunc();

            $('#calendar').fullCalendar('refetchEvents');
            //  alert('abc');
        });
    </script>
</body>
</html>
