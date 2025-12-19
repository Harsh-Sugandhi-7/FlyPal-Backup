<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOGanttChart.aspx.vb"
    Inherits="Flypal.wfnWOGanttChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gantt Chart</title>
    <meta content="False" name="vs_showGrid" />
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
     <%-- --%> 
      <link id="MainStyle" type="text/css" rel="stylesheet">
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <%-- FusionCharts --%>
    <script src="FusionCharts/fusioncharts.js" type="text/javascript"></script>
    <script src="FusionCharts/fusioncharts.charts.js" type="text/javascript"></script>
    <script src="FusionCharts/themes/fusioncharts.theme.fint.js" type="text/javascript"></script>
    <script src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>
   
  <asp:PlaceHolder runat="server">
        <%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
                                <span style="font-size: 22px; font-weight: bold" class="text-info">TASK(s) GANTT CHART
                                </span>
                            </h1>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-md-12 col-sm-6 col-xs-12">
                        <div class="main-box infographic-box" style="background: whitesmoke;">
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <span class="clsLabel">24 Hrs Schedule</span>
                                        <div>
                                            <asp:RadioButton ID="rdb24Hrs" runat="server" Checked="true" GroupName="a" CssClass="clsCheckBox"
                                                AutoPostBack="true" />
                                        </div>
                                    </td>
                                    <td align="center">
                                        <span class="clsLabel">48 Hrs Schedule</span>
                                        <div>
                                            <asp:RadioButton ID="rdb48Hrs" runat="server" GroupName="a" CssClass="clsCheckBox"
                                                AutoPostBack="true" />
                                        </div>
                                    </td>
                                    <td align="center">
                                        <span class="clsLabel">7 Days Schedule</span>
                                        <div>
                                            <asp:RadioButton ID="rdb7Days" runat="server" GroupName="a" CssClass="clsCheckBox"
                                                AutoPostBack="true" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-md-12 col-sm-6 col-xs-12">
                        <asp:UpdatePanel ID="upnlWOGanttGraph" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="WOGantt" runat="server">
                                    <table id="Table3" runat="server" width="50%">
                                        <tr>
                                            <td align="left">
                                                <asp:Button ID="btnPrev" runat="server" TabIndex="0" Text="&#x276E;&#x276E;" />
                                                <asp:Button ID="btnToday" runat="server"  TabIndex="0" Text="Today" /><%--CssClass="clsButton_Ajax"--%>
                                                <asp:Button ID="btnNext" runat="server" TabIndex="0" Text="&#x276F;&#x276F;" />
                                                
                                                  <asp:TextBox runat="server" ID="txtcalDateTime" CssClass="clsTextBox_Ajax" Width="100px"
                                                        AutoPostBack="true" onchange="ValidateDateText(this,'calDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calDateTime_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtcalDateTime">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtcalDateTime" ID="calDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                            </td>
                                            <td>
                                            
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset id="fdsWOGanttGraph" style="border-width: 1px;">
                                                    <div id="WOGanttGraph">
                                                    </div>
                                                    <div id="WO7DaysGanttGraph">
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="400" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                        background-color: #d9d7d7; top: 0; z-index: 99999;">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function genProcessChart(TaskProcess) {
            var milestones = TaskProcess;
            var processLabels = [];
            var availableTags = TaskProcess.split(';;');

            for (var i = 0; i <= availableTags.length - 1; i++) {
                processLabels.push({
                    "label": '' + availableTags[i] + '',
                    "id": '' + i + '',
                    "width": "50"
                });
            }
            return processLabels;
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
        function genTaskChart(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, ActualStartDateFormatted, ActualEndDateFormatted) {
            var StartDateFormatted = TaskPlanStartDateFormatted.split(';;');
            var EndDateFormatted = TaskPlanEndDateFormatted.split(';;');

            var ActualStartDate = ActualStartDateFormatted.split(';;');
            var ActualEndDate = ActualEndDateFormatted.split(';;');

            var TaskLabels = [];
            var availableTags = TaskProcess.split(';;');

            for (var i = 0; i <= availableTags.length - 1; i++) {
                TaskLabels.push({
                    "start": '' + StartDateFormatted[i] + '',
                    "end": '' + EndDateFormatted[i] + '',
                    "processid": '' + i + '',
                    "id": '' + i + '',
                    "color": "#008ee4",
                    "label": "Planned",
                    "toppadding": "12%",
                    "height": "32%"
                });
                TaskLabels.push({
                    "start": '' + ActualStartDate[i] + '',
                    "end": '' + ActualEndDate[i] + '',
                    "processid": '' + i + '',
                    "id": '' + i + '-1' + '',
                    "color": "#6baa01",
                    "label": "Actual",
                    "toppadding": "56%",
                    "height": "32%"

                });
            }

            return TaskLabels;
        }
        
    </script>
    <script type="text/javascript">

        function FusionGanttFunc(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, ActualStartDate, ActualEndDate, CategoryList, WONo, CurrentTime, CurrentOnlyTime) {

            //  var StartDate = ["19/12/2019 0:00", "19/12/2019 1:00", "19/12/2019 2:00"];
            //  var EndDate = ["19/12/2019 1:00", "19/12/2019 2:00", "19/12/2019 3:00"];
            //  var milestones = ["taskAA1", "taskBB2", "taskCC3"];
            var processLabels = genProcessChart(TaskProcess);
            var TaskLabels = genTaskChart(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, ActualStartDate, ActualEndDate);
            var WOLabels = genWO(WONo);

            var CategoryListTags = CategoryList.split(';;');
            var ganttChart = new FusionCharts({
                "type": "gantt",
                "renderAt": "WOGanttGraph",
                width: '1100',
                height: '400',
                dataFormat: 'json',
                dataSource: {
                    "chart": {
                        "dateformat": "dd/mm/yyyy",
                        "outputdateformat": "hh:mn",
                        "caption": "Work Schedule",
                        "canvasBorderAlpha": "30",
                        "theme": "fusion",
                        "plottooltext": "$processName{br} $label starting time $start{br}$label ending time $end",
                        "legendPosition": "right"
                    },
                    "categories": [{
                        "category": [{
                            // "start": "20/12/2019 00:00",
                            //  "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 00:00' + '',
                            "end": '' + CategoryListTags[0] + ' 23:59:59' + '',
                            "label": '' + CategoryListTags[0] + ''
                        },
                        {
                            //  "start": "21/12/2019 00:00",
                            // "end": "21/12/2019 23:59:59",
                            "start": '' + CategoryListTags[1] + ' 00:00' + '',
                            "end": '' + CategoryListTags[1] + ' 23:59:59' + '',
                            "label": '' + CategoryListTags[1] + ''
                        }]
                    }, {
                        "align": "right",
                        "category": [{
                            //    "start": "20/12/2019 00:00",
                            //    "end": "20/12/2019 05:59:59",
                            "start": '' + CategoryListTags[0] + ' 00:00' + '',
                            "end": '' + CategoryListTags[0] + ' 05:59:59' + '',
                            "label": "6 am"
                        }, {
                            //    "start": "20/12/2019 06:00",
                            //    "end": "20/12/2019 11:59:59",
                            "start": '' + CategoryListTags[0] + ' 06:00' + '',
                            "end": '' + CategoryListTags[0] + ' 11:59:59' + '',
                            "label": "12 pm"
                        }, {
                            //   "start": "20/12/2019 12:00",
                            //   "end": "20/12/2019 17:59:59",
                            "start": '' + CategoryListTags[0] + ' 12:00' + '',
                            "end": '' + CategoryListTags[0] + ' 17:59:59' + '',
                            "label": "6 pm"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 18:00' + '',
                            "end": '' + CategoryListTags[0] + ' 23:59:59' + '',
                            "label": "Midnight"
                        },
                        {
                            // "start": "21/12/2019 00:00",
                            // "end": "21/12/2019 05:59:59",
                            "start": '' + CategoryListTags[1] + ' 00:00' + '',
                            "end": '' + CategoryListTags[1] + ' 05:59:59' + '',
                            "label": "6 am"
                        }, {
                            // "start": "21/12/2019 06:00",
                            //  "end": "21/12/2019 11:59:59",
                            "start": '' + CategoryListTags[1] + ' 06:00' + '',
                            "end": '' + CategoryListTags[1] + ' 11:59:59' + '',
                            "label": "12 pm"
                        }, {
                            //  "start": "21/12/2019 12:00",
                            //  "end": "21/12/2019 17:59:59",
                            "start": '' + CategoryListTags[1] + ' 12:00' + '',
                            "end": '' + CategoryListTags[1] + ' 17:59:59' + '',
                            "label": "6 pm"
                        }, {
                            //  "start": "21/12/2019 18:00",
                            // "end": "21/12/2019 23:59:59",
                            "start": '' + CategoryListTags[1] + ' 18:00' + '',
                            "end": '' + CategoryListTags[1] + ' 23:59:59' + '',
                            "label": "Midnight"
                        }]
                    }],
                    "tasks": {
                        "task": TaskLabels
                    },
                    "processes": {
                        "headertext": "Task",
                        "align": "left",
                        "process": processLabels
                    },
                    "datatable": {
                        "showprocessname": "1",
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
                        }, {
                            "label": "Actual",
                            "color": "#6baa01"
                        }]
                    },
                    "trendlines": [{
                        "line": [{
                            // "start": "9/12/2019 10:39:00",
                            //"start": '' + CategoryListTags[0] + ' 03:42' + '',
                            "start": '' + CurrentTime + '',
                            "displayvalue": '' + CurrentOnlyTime + '',
                            "color": "333333",
                            "thickness": "2",
                            "dashed": "1"
                        }]
                    }]
                }
            });
            ganttChart.render();
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
        function Fusion7DaysGanttFunc(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, ActualStartDate, ActualEndDate, CategoryList, WONo, CurrentTime, CurrentOnlyTime) {

            //  var StartDate = ["19/12/2019 0:00", "19/12/2019 1:00", "19/12/2019 2:00"];
            //  var EndDate = ["19/12/2019 1:00", "19/12/2019 2:00", "19/12/2019 3:00"];
            //  var milestones = ["taskAA1", "taskBB2", "taskCC3"];
            var processLabels = genProcessChart(TaskProcess);
            var TaskLabels = genTaskChart(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, ActualStartDate, ActualEndDate);
            //  var CategoryListTags = genCategoryChart(CategoryList);
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
                        "dateformat": "dd/mm/yyyy",
                        "outputdateformat": "hh:mn",
                        "caption": "Work Schedule",
                        "canvasBorderAlpha": "30",
                        "theme": "fusion",
                        "plottooltext": "$processName{br} $label starting date $start{br}$label ending date $end",
                        "legendPosition": "right"
                    },
                    "categories": [{

                        "align": "center",
                        "category": [{
                            "start": '' + CategoryListTags[0] + ' 00:00' + '',
                            "end": '' + CategoryListTags[0] + ' 23:59:59' + '',
                            "label": '' + 'Monday<br>' + CategoryListTags[0] + ''
                        }, {
                            "start": '' + CategoryListTags[1] + ' 00:00' + '',
                            "end": '' + CategoryListTags[1] + ' 23:59:59' + '',
                            "label": '' + 'Tuesday<br>' + CategoryListTags[1] + ''
                        }, {
                            "start": '' + CategoryListTags[2] + ' 00:00' + '',
                            "end": '' + CategoryListTags[2] + ' 23:59:59' + '',
                            "label": '' + 'Wednesday<br>' + CategoryListTags[2] + ''
                        }, {
                            "start": '' + CategoryListTags[3] + ' 00:00' + '',
                            "end": '' + CategoryListTags[3] + ' 23:59:59' + '',
                            "label": '' + 'Thrusday<br>' + CategoryListTags[3] + ''
                        },
                        {
                            "start": '' + CategoryListTags[4] + ' 00:00' + '',
                            "end": '' + CategoryListTags[4] + ' 23:59:59' + '',
                            "label": '' + 'Friday<br>' + CategoryListTags[4] + ''
                        }, {
                            "start": '' + CategoryListTags[5] + ' 00:00' + '',
                            "end": '' + CategoryListTags[5] + ' 23:59:59' + '',
                            "label": '' + 'Saturday<br>' + CategoryListTags[5] + ''
                        }, {
                            "start": '' + CategoryListTags[6] + ' 00:00' + '',
                            "end": '' + CategoryListTags[6] + ' 23:59:59' + '',
                            "label": '' + 'Sunday<br>' + CategoryListTags[6] + ''
                        }]
                    }],
                    "tasks": {
                        "task": TaskLabels
                    },
                    "processes": {
                        "headertext": "Task",
                        "align": "left",
                        "process": processLabels
                    },
                    "datatable": {
                        "showprocessname": "1",
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
                        }, {
                            "label": "Actual",
                            "color": "#6baa01"
                        }]
                    },
                    "trendlines": [{
                        "line": [{
                            // "start": "9/12/2019 10:39:00",
                            //"start": '' + CategoryListTags[0] + ' 03:42' + '',
                            "start": '' + CurrentTime + '',
                            "displayvalue": '' + CurrentOnlyTime + '',
                            "color": "333333",
                            "thickness": "2",
                            "dashed": "1"
                        }]
                    }]
                }
            });
            ganttChart.render();
        }
    </script>
    <script type="text/javascript">

        function FusionGantt24HrsFunc(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, ActualStartDate, ActualEndDate, CategoryList, WONo, CurrentTime, CurrentOnlyTime) {

            //  var StartDate = ["19/12/2019 0:00", "19/12/2019 1:00", "19/12/2019 2:00"];
            //  var EndDate = ["19/12/2019 1:00", "19/12/2019 2:00", "19/12/2019 3:00"];
            //  var milestones = ["taskAA1", "taskBB2", "taskCC3"];
            var processLabels = genProcessChart(TaskProcess);
            var TaskLabels = genTaskChart(TaskProcess, TaskPlanStartDateFormatted, TaskPlanEndDateFormatted, ActualStartDate, ActualEndDate);
            var WOLabels = genWO(WONo);

            var CategoryListTags = CategoryList.split(';;');
            var ganttChart = new FusionCharts({
                "type": "gantt",
                "renderAt": "WOGanttGraph",
                width: '1100',
                height: '400',
                dataFormat: 'json',
                dataSource: {
                    "chart": {
                        "dateformat": "dd/mm/yyyy",
                        "outputdateformat": "hh:mn",
                        "caption": "Work Schedule",
                        "canvasBorderAlpha": "30",
                        "theme": "fusion",
                        "plottooltext": "$processName{br} $label starting time $start{br}$label ending time $end",
                        "legendPosition": "right"
                    },
                    "categories": [{
                        "category": [{
                            // "start": "20/12/2019 00:00",
                            //  "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 00:00' + '',
                            "end": '' + CategoryListTags[0] + ' 23:59:59' + '',
                            "label": '' + CategoryListTags[0] + ''
                        }]
                    }, {
                        "align": "right",
                        "category": [{
                            //    "start": "20/12/2019 00:00",
                            //    "end": "20/12/2019 05:59:59",
                            "start": '' + CategoryListTags[0] + ' 00:00' + '',
                            "end": '' + CategoryListTags[0] + ' 00:59:59' + '',
                            "label": "1",
                            "width":"20"
                        }, {
                            //    "start": "20/12/2019 06:00",
                            //    "end": "20/12/2019 11:59:59",
                            "start": '' + CategoryListTags[0] + ' 01:00' + '',
                            "end": '' + CategoryListTags[0] + ' 01:59:59' + '',
                            "label": "2",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 12:00",
                            //   "end": "20/12/2019 17:59:59",
                            "start": '' + CategoryListTags[0] + ' 2:00' + '',
                            "end": '' + CategoryListTags[0] + ' 2:59:59' + '',
                            "label": "3",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 3:00' + '',
                            "end": '' + CategoryListTags[0] + ' 3:59:59' + '',
                            "label": "4"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 4:00' + '',
                            "end": '' + CategoryListTags[0] + ' 4:59:59' + '',
                            "label": "5",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 5:00' + '',
                            "end": '' + CategoryListTags[0] + ' 5:59:59' + '',
                            "label": "6",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 6:00' + '',
                            "end": '' + CategoryListTags[0] + ' 6:59:59' + '',
                            "label": "7",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 7:00' + '',
                            "end": '' + CategoryListTags[0] + ' 7:59:59' + '',
                            "label": "8",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 8:00' + '',
                            "end": '' + CategoryListTags[0] + ' 8:59:59' + '',
                            "label": "9",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 9:00' + '',
                            "end": '' + CategoryListTags[0] + ' 9:59:59' + '',
                            "label": "10",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 10:00' + '',
                            "end": '' + CategoryListTags[0] + ' 10:59:59' + '',
                            "label": "11",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 11:00' + '',
                            "end": '' + CategoryListTags[0] + ' 11:59:59' + '',
                            "label": "12",
                            "width": "20"
                        },
                        {
                            //    "start": "20/12/2019 00:00",
                            //    "end": "20/12/2019 05:59:59",
                            "start": '' + CategoryListTags[0] + ' 12:00' + '',
                            "end": '' + CategoryListTags[0] + ' 12:59:59' + '',
                            "label": "13",
                            "width": "20"
                        }, {
                            //    "start": "20/12/2019 06:00",
                            //    "end": "20/12/2019 11:59:59",
                            "start": '' + CategoryListTags[0] + ' 13:00' + '',
                            "end": '' + CategoryListTags[0] + ' 13:59:59' + '',
                            "label": "14"
                        }, {
                            //   "start": "20/12/2019 12:00",
                            //   "end": "20/12/2019 17:59:59",
                            "start": '' + CategoryListTags[0] + ' 14:00' + '',
                            "end": '' + CategoryListTags[0] + ' 14:59:59' + '',
                            "label": "15",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 15:00' + '',
                            "end": '' + CategoryListTags[0] + ' 15:59:59' + '',
                            "label": "16",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 16:00' + '',
                            "end": '' + CategoryListTags[0] + ' 16:59:59' + '',
                            "label": "17",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 17:00' + '',
                            "end": '' + CategoryListTags[0] + ' 17:59:59' + '',
                            "label": "18",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 18:00' + '',
                            "end": '' + CategoryListTags[0] + ' 1:59:59' + '',
                            "label": "19",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 19:00' + '',
                            "end": '' + CategoryListTags[0] + ' 19:59:59' + '',
                            "label": "20",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 20:00' + '',
                            "end": '' + CategoryListTags[0] + ' 20:59:59' + '',
                            "label": "21",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 21:00' + '',
                            "end": '' + CategoryListTags[0] + ' 21:59:59' + '',
                            "label": "22",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 22:00' + '',
                            "end": '' + CategoryListTags[0] + ' 22:59:59' + '',
                            "label": "23",
                            "width": "20"
                        }, {
                            //   "start": "20/12/2019 18:00",
                            //   "end": "20/12/2019 23:59:59",
                            "start": '' + CategoryListTags[0] + ' 23:00' + '',
                            "end": '' + CategoryListTags[0] + ' 23:59:59' + '',
                            "label": "24",
                            "width": "20"
                        }]
                    }],
                    "tasks": {
                        "task": TaskLabels
                    },
                    "processes": {
                        "headertext": "Task",
                        "align": "left",
                        "process": processLabels
                    },
                    "datatable": {
                        "showprocessname": "1",
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
                        }, {
                            "label": "Actual",
                            "color": "#6baa01"
                        }]
                    },
                    "trendlines": [{
                        "line": [{
                            // "start": "9/12/2019 10:39:00",
                            //"start": '' + CategoryListTags[0] + ' 03:42' + '',
                            "start": '' + CurrentTime + '',
                            "displayvalue": '' + CurrentOnlyTime + '',
                            "color": "333333",
                            "thickness": "2",
                            "dashed": "1"
                        }]
                    }]
                }
            });
            ganttChart.render();
        }
    </script>
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
