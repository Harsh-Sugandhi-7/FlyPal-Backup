<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDuePlanningCalendar.aspx.vb"
    Inherits="Flypal.wfDuePlanningCalendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Due Planning Calender</title>

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
        #script-warning {
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

        #loading {
            display: none;
            position: absolute;
            top: 10px;
            right: 10px;
        }

        #calendar .dot-event {
            width: 0.1em;
            height: 0.1em;
            border-radius: 50%;
            display: inline-block;
            margin-left: 5px;
            vertical-align: text-bottom;
        }

        .modal-header {
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

            .modal-header.primary {
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
                <div class="row">
                    <div class="col-xs-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <h1 class="pull-center">
                                    <span style="font-size: 22px; font-weight: bold" class="text-info">PLAN(s) USING CALANDAR
                                    </span>
                                </h1>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6">
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
                    </div>
                </div>
                <div>
                    <asp:Button ID="hdnBtnDueJobPlanning" ClientIDMode="Static" runat="server"
                        Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
                    <asp:HiddenField ID="hdnID" runat="server" />
                </div>

                <p>
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
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
                                        <h4></h4>
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
                        var date = new Date($('#calendar').fullCalendar('getDate'));
                        var month_int = date.getMonth();
                        var year_int = date.getFullYear();
                        $.ajax({
                            type: "POST",
                            //  data: "{ 'WOStatusID': '" + WOStatusID + "'}",
                            //  data: "{ 'WOStatusID': '" + WOStatusID + "', 'CustomerID': '" + CustomerID + "' }",
                            data: "{'month': '" + month_int + "', 'year': '" + year_int + "' }",
                            url: "wfDuePlanningCalendar.aspx/TestOnWebService",
                            dataType: 'json',
                            contentType: "application/json",

                            success: function (data) {
                                debugger;
                                var events = [];
                                var obj = jQuery.parseJSON(data.d);
                                $(obj).each(function () {

                                    //var nowdate = new Date($(this).attr('start')).toDateString("yyyy-MM-dd");
                                    events.push({

                                        title: $(this).attr('title'),
                                        start: $(this).attr('start'), // will be parsed
                                        id: $(this).attr('id'),
                                        PlannedDetailsCalender: $(this).attr('PlannedDetailsCalender'),

                                        // color: '#BEEABE',
                                        allday: true,

                                        // eventLimit: 6
                                    });
                                    debugger;
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
                            title: event.PlannedDetailsCalender,
                            placement: "top",
                            trigger: "hover",
                            container: "body",
                            html: true,
                            animation: true


                        });

                        //  element.attr('title', event.tooltip);
                        element.css("font-size", "0.7em");
                        element.find('.fc-title').html(event.title);
                        element.css("padding", "5px");
                        //element.css('background-color', '#662bcc');
                        element.css('background-color', '#4A63A0');
                        //                        }


                    }, //eventRender ends

                    eventClick: function (event, jsEvent, view) {
                        document.getElementById("hdnID").value = event.id;
                        debugger;
                        $("#hdnBtnDueJobPlanning").click();

                        //$("#successModal").modal("show");
                        //// $("#successModal .modal-body p").text(' \n<h3>Reg No. :</h3> ' + event.RegNo + ' \nStatus: ' + event.WOStatus);
                        //$("#successModal .modal-body p").html(event.DueJobPlanningNo);
                        //$("#successModal .modal-title p").html(event.title);

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
        </script>
        <!-- Popup For DueJobPlanningSelection -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDueJobPlanningSelection" Text="DueJobPlanningSelection"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlDueJobPlanningSelection" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeDueJobPlanningSelection" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto" allowtransparency="true"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupDueJobPlanningSelection" runat="server" TargetControlID="btnDummyDueJobPlanningSelection"
            PopupControlID="pnlDueJobPlanningSelection" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function OpenDueJobPlanningSelectionWindow() {
                try {
                    $("#IframeDueJobPlanningSelection").attr("src", "wfDueJobPlanning_Ajax.aspx?");
                    $("#btnDummyDueJobPlanningSelection").click();

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForDueJobPlanningSelection() {
                var DueJobPlanningSelectionwindow = $find("<%=mdlPopupDueJobPlanningSelection.ClientID %>");
                //close popup window
                DueJobPlanningSelectionwindow.hide();
                //           release resources
                $("#IframeDueJobPlanningSelection").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnDueJobPlanningSelection").click();
            }
        </script>
        <!---End-->
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
    </form>
</body>
</html>
