<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDuePlanningGanttChart.aspx.vb" Inherits="Flypal.wfDuePlanningGanttChart" %>

<!DOCTYPE html>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="False" name="vs_showGrid" />
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />

    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="FusionCharts/fusioncharts.js" type="text/javascript"></script>
    <script src="FusionCharts/fusioncharts.charts.js" type="text/javascript"></script>
    <script src="FusionCharts/themes/fusioncharts.theme.fint.js" type="text/javascript"></script>
    <link href="bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>

        <asp:UpdatePanel runat="server" ID="upnlWO" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-xs-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <h1 class="pull-center">
                                    <span style="font-size: 22px; font-weight: bold" class="text-info">PLANNED Vs ACTUAL ACTIVITIES
                                    </span>
                                </h1>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="col-md-6 col-sm-6 col-xs-6">
                            <asp:UpdatePanel ID="upnlWOGanttGraph" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Panel ID="WOGantt" runat="server">
                                        <table id="Table3" runat="server" width="50%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnPrev" runat="server" TabIndex="0" Text="&#x276E;&#x276E;" />
                                                    <asp:Button ID="btnToday" runat="server" TabIndex="0" Text="Today" /><%--CssClass="clsButton_Ajax"--%>
                                                    <asp:Button ID="btnNext" runat="server" TabIndex="0" Text="&#x276F;&#x276F;" />

                                                    <asp:TextBox runat="server" ID="txtcalDateTime" CssClass="clsTextBoxTagDateSearch" Height="25px" autocomplete="off"
                                                        AutoPostBack="true" onchange="ValidateDateText(this,'calDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calDateTime_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtcalDateTime"></cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtcalDateTime" ID="calDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" width="100%">
                                                    <fieldset id="fdsWOGanttGraph" style="border-width: 1px;">

                                                        <div id="WO7DaysGanttGraph">
                                                        </div>

                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div>
                                                        <asp:Button ID="hdnBtnDueJobPlanning" ClientIDMode="Static" runat="server"
                                                            Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
                                                        <asp:HiddenField ID="hdnID" runat="server" />
                                                        <asp:HiddenField ID="hdnWOID" runat="server" />

                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
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
                <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="400" DynamicLayout="false" runat="server">
                    <ProgressTemplate>
                        <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #d9d7d7; top: 0; z-index: 99999;">
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <script type="text/javascript">
            function genProcessChart(TaskProcess, PlanningID) {
                var milestones = TaskProcess;
                var processLabels = [];
                var availableTags = TaskProcess.split(';;');
                var processid = PlanningID.split(';;');

                for (var i = 0; i <= availableTags.length - 1; i++) {
                    processLabels.push({
                        "label": '' + availableTags[i] + '',
                        "id": '' + processid[i] + '',
                        "width": "50"
                    });
                }
                return processLabels;
            }

            function genPlannedID(PlanningID) {
                var milestones = PlanningID;
                var events = [];
                var availableTags = PlanningID.split(';;');

                for (var i = 0; i <= availableTags.length - 1; i++) {
                    events.push({
                        "label": '' + availableTags[i] + '',
                        "id": '' + i + ''
                    });
                }
                return events;
            }
            function genWO(WONo) {
                var milestones = WONo;
                var processLabels = [];
                var availableTags = WONo.split(';;');

                for (var i = 0; i <= availableTags.length - 1; i++) {
                    processLabels.push({
                        "label": '' + availableTags[i] + '',
                        "id": '' + i + ''
                    });
                }
                return processLabels;
            }
            function genTaskChart(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, PlannedIDs, ActualStartDateFormatted, ActualEndDateFormatted, WONo, WOID, WOPlannedDateFormatted) {
                var StartDateFormatted = TaskPlanStartDateFormatted.split(';;');
                var EndDateFormatted = TaskPlanEndDateFormatted.split(';;');

                var ActualStartDate = ActualStartDateFormatted.split(';;');
                var ActualEndDate = ActualEndDateFormatted.split(';;');
                var WOPlannedDate = WOPlannedDateFormatted.split(';;');

                var TaskLabels = [];
                var availableTags = TaskProcess.split(';;');
                var availableIds = PlannedIDs.split(';;');
                var availableWONo = WONo.split(';;');
                var availableWOID = WOID.split(';;');
                if (availableTags[0] == '') {
                      // do nothing 
                }
                else {
                    for (var i = 0; i <= availableTags.length - 1; i++) {
                        TaskLabels.push({
                            "start": '' + StartDateFormatted[i] + ' 00:00:00' + '',
                            "end": '' + EndDateFormatted[i] + ' 23:59:59' + '',
                            "processid": availableIds[i],
                            "id": '' + '00000000-0000-0000-0000-000000000000' + '',
                            "color": "#008ee4",
                            "label": availableTags[i],
                            "toppadding": "18%",
                            "height": "32%"
                        });
                        //TaskLabels.push({
                        //        "start": '' + WOPlannedDate[i] + ' 00:00:00' + '',
                        //        "end": '' + WOPlannedDate[i] + ' 23:59:59' + '',
                        //        "processid": availableIds[i],
                        //        "id": availableWOID[i], //'' + i + '-1' + '',
                        //        "color": "#6baa01",
                        //        "label": "Planned WO",
                        //        "toppadding": "56%",
                        //        "height": "32%"
                        //    });
                        if (availableWOID[i] == '00000000-0000-0000-0000-000000000000') {
                            // do nothing     
                            //task bar not required if no WO 
                        }
                        else {
                            TaskLabels.push({
                                "start": '' + WOPlannedDate[i] + ' 00:00:00' + '',
                                "end": '' + WOPlannedDate[i] + ' 23:59:59' + '',
                                "processid": availableIds[i],
                                "id": availableWOID[i], //'' + i + '-1' + '',
                                "color": "#6baa01",
                                "label": availableWONo[i],
                                "toppadding": "75%",
                                "height": "22%"
                            });
                        }
                    }
                }
                return TaskLabels;

            }

        </script>

        <asp:HiddenField ID="hdnFromDate" runat="server" />
        <asp:HiddenField ID="hdnToDate" runat="server" />
        <script type="text/javascript">
            function genCategoryChart(CategoryList) {
                var CategoryListTags = CategoryList.split(';;');
                var CategoryLabels = [];

                CategoryLabels.push({
                    "start": '' + CategoryListTags[0] + ' 00:00' + '',
                    "end": '' + CategoryListTags[0] + ' 23:59:59' + '',
                    "label": "Monday"
                });
                CategoryLabels.push({
                    "start": '' + CategoryListTags[1] + ' 00:00' + '',
                    "end": '' + CategoryListTags[1] + ' 23:59:59' + '',
                    "label": "Tuesday"
                });
                CategoryLabels.push({
                    "start": '' + CategoryListTags[2] + ' 00:00' + '',
                    "end": '' + CategoryListTags[2] + ' 23:59:59' + '',
                    "label": "Wednesday"
                });
                CategoryLabels.push({
                    "start": '' + CategoryListTags[3] + ' 00:00' + '',
                    "end": '' + CategoryListTags[3] + ' 23:59:59' + '',
                    "label": "Thrusday"
                });
                CategoryLabels.push({
                    "start": '' + CategoryListTags[4] + ' 00:00' + '',
                    "end": '' + CategoryListTags[4] + ' 23:59:59' + '',
                    "label": "Friday"
                });
                CategoryLabels.push({
                    "start": '' + CategoryListTags[5] + ' 00:00' + '',
                    "end": '' + CategoryListTags[5] + ' 23:59:59' + '',
                    "label": "Saturday"
                });
                CategoryLabels.push({
                    "start": '' + CategoryListTags[6] + ' 00:00' + '',
                    "end": '' + CategoryListTags[6] + ' 23:59:59' + '',
                    "label": "Sunday"
                });

                return CategoryLabels;
            }
            function Fusion7DaysGanttFunc(TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, CategoryList, PlanningNo, CurrentTime, CurrentOnlyTime, Appsettingsdateformat, AppsettingsdateasperFusionformat, PlannedID, WONo, ActualStartDate, ActualEndDate, WOID, WOPlannedDateFormatted) {


                var processLabels = genProcessChart(PlanningNo, PlannedID);
                //var PlannedIDs = genPlannedID(PlannedID);
                debugger;
                var TaskLabels = genTaskChart(PlanningNo, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, PlannedID, ActualStartDate, ActualEndDate, WONo, WOID, WOPlannedDateFormatted);
                //var PlannedIDs = genPlannedID(PlannedID);
                var WOLabels = genWO(WONo);
                var CategoryListTags = CategoryList.split(';;');
                var ganttChart = new FusionCharts({
                    "type": "gantt",
                    "renderAt": "WO7DaysGanttGraph",
                    width: '1100',
                    height: '400',
                    dataFormat: 'json',
                    dataSource: {
                        "chart": {
                            "dateformat": Appsettingsdateformat,
                            "outputdateformat": AppsettingsdateasperFusionformat,
                            "caption": "Planned Activities",
                            "canvasBorderAlpha": "30",
                            "theme": "fusion",
                            "plottooltext": "$processName{br} Start date $start{br} Ending date $end",
                            "legendPosition": "right",
                            "taskBarRoundRadius": "6",
                            "showTaskLabels": "0",
                            //set it as per your category data array
                            "ganttpaneduration": "10",
                            "ganttpanedurationunit": "m"

                        },

                        "tasks": {
                            "showlabels": "1",
                            "task": TaskLabels
                        },
                        "processes": {
                            "headertext": "Planning No",
                            "align": "left",
                            "fontsize": "12",
                            "isbold": "1",
                            "align": "left",
                            "headerfontsize": "14",
                            "headervalign": "middle",
                            "process": processLabels
                        },
                        "datatable": {
                            "namealign": "left",
                            "fontcolor": "#000000",
                            "fontsize": "10",
                            "valign": "right",
                            "align": "center",
                            "headervalign": "middle",
                            "headeralign": "center",
                            "headerbgcolor": "#eeeeee",
                            "headerfontcolor": "#000000",
                            "headerfontsize": "12",
                            "datacolumn": [{
                                "bgcolor": "#eeeeee",
                                "headertext": "WO No.",
                                "width": "90",
                                "text": WOLabels
                            }]
                        },
                        "legend": {
                            "item": [{
                                "label": "Planned",
                                "color": "#008ee4"
                            },
                            {
                                "label": "Actual WO",
                                "color": "#6baa01"
                            }]
                        },
                        "categories": [{

                            "align": "center",
                            "width": "50px",
                            "category": [{
                                "start": '' + CategoryListTags[0] + ' 00:00' + '',
                                "end": '' + CategoryListTags[0] + ' 23:59:59' + '',
                                "label": '' + 'Mon<br>' + CategoryListTags[0] + ''
                            }, {
                                "start": '' + CategoryListTags[1] + ' 00:00' + '',
                                "end": '' + CategoryListTags[1] + ' 23:59:59' + '',
                                "label": '' + 'Tues<br>' + CategoryListTags[1] + ''
                            }, {
                                "start": '' + CategoryListTags[2] + ' 00:00' + '',
                                "end": '' + CategoryListTags[2] + ' 23:59:59' + '',
                                "label": '' + 'Wed<br>' + CategoryListTags[2] + ''
                            }, {
                                "start": '' + CategoryListTags[3] + ' 00:00' + '',
                                "end": '' + CategoryListTags[3] + ' 23:59:59' + '',
                                "label": '' + 'Thrus<br>' + CategoryListTags[3] + ''
                            },
                            {
                                "start": '' + CategoryListTags[4] + ' 00:00' + '',
                                "end": '' + CategoryListTags[4] + ' 23:59:59' + '',
                                "label": '' + 'Fri<br>' + CategoryListTags[4] + ''
                            }, {
                                "start": '' + CategoryListTags[5] + ' 00:00' + '',
                                "end": '' + CategoryListTags[5] + ' 23:59:59' + '',
                                "label": '' + 'Sat<br>' + CategoryListTags[5] + ''
                            }, {
                                "start": '' + CategoryListTags[6] + ' 00:00' + '',
                                "end": '' + CategoryListTags[6] + ' 23:59:59' + '',
                                "label": '' + 'Sun<br>' + CategoryListTags[6] + ''
                            }]
                        }],
                        "trendlines": [{
                            "line": [{
                                // "start": "9/12/2019 10:39:00",
                                //"start": '' + CategoryListTags[0] + ' 03:42' + '',
                                "start": '' + CurrentTime + '',
                                "displayvalue": '' + TaskPlanStartDateFormatted + '',
                                "color": "333333",
                                "thickness": "2",
                                "dashed": "1"
                            }]
                        }]

                    },
                    "events": { // place events outside datasource
                        //Using dataplot click event
                        "dataplotClick": function (evtObj, argObj) {
                            //$("#successModal").modal("show");
                            // alert(argObj.processid);

                            console.log(argObj);

                            document.getElementById("hdnID").value = argObj.processId;
                            document.getElementById("hdnWOID").value = argObj.taskId;
                            $("#hdnBtnDueJobPlanning").click();

                            //  $("#successModal .modal-body p").html(argObj.id);
                            //$("#successModal .modal-title p").html(argObj.planningno);
                            // OpenDueJobPlanningSelectionWindow();
                            // alert("abc");
                        }
                    }
                });
                ganttChart.render();
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
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
</body>
</html>
