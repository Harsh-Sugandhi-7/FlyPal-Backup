

'Created By: Saylee
'Dated:      23-Dec-2019



Imports System.Text

Public Class wfnWOGanttChart
    Inherits System.Web.UI.Page

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      
        If Not IsPostBack Then
            If rdb48Hrs.Checked Or rdb24Hrs.Checked Then
                SetGanttChartValues("", "")
                btnPrev.Visible = True
                btnNext.Visible = True
                btnToday.Visible = True
            Else
                btnPrev.Visible = False
                btnNext.Visible = False
                btnToday.Visible = False
            End If

        End If

        If rdb48Hrs.Checked Or rdb24Hrs.Checked Then
            btnPrev.Visible = True
            btnNext.Visible = True
            btnToday.Visible = True
        Else
            btnPrev.Visible = False
            btnNext.Visible = False
            btnToday.Visible = False
        End If
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As System.EventArgs) Handles btnPrev.Click
        Dim FromDate As String = "" '= hdnFromDate.Value
        Dim ToDate As String = "" '= hdnToDate.Value
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        If rdb48Hrs.Checked Then
            FromDate = DateAdd(DateInterval.Day, -1, CDate(FromDate)).ToString
            ToDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString
            SetGanttChartValues(FromDate, ToDate)
            'Else
            '    Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek

            '    FromDate = DateTime.Now.AddDays(delta)
            '    ToDate = DateAdd(DateInterval.Day, 6, CDate(FromDate)).ToString
            '    SetGantt7DaysChart(FromDate, ToDate)
        ElseIf rdb24Hrs.Checked Then
            FromDate = DateAdd(DateInterval.Day, -1, CDate(FromDate)).ToString
            ToDate = FromDate
            SetGanttChartValues(FromDate, ToDate)
        End If
    End Sub
    Private Sub btnToday_Click(sender As Object, e As System.EventArgs) Handles btnToday.Click
        If rdb48Hrs.Checked Or rdb24Hrs.Checked Then
            SetGanttChartValues("", "")
      
        End If
    End Sub
    Private Sub btnNext_Click(sender As Object, e As System.EventArgs) Handles btnNext.Click
        Dim FromDate As String = "" '= hdnFromDate.Value
        Dim ToDate As String = "" '= hdnToDate.Value
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        If rdb48Hrs.Checked Then
            FromDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString 'DateAdd(DateInterval.Day, 1, CDate(hdnFromDate.Value)).ToString
            ToDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString

            SetGanttChartValues(FromDate, ToDate)
        ElseIf rdb24Hrs.Checked Then
            FromDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString
            ToDate = FromDate
            SetGanttChartValues(FromDate, ToDate)
        End If

    End Sub

    Private Sub rdb7Days_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdb48Hrs.CheckedChanged, rdb7Days.CheckedChanged, rdb24Hrs.CheckedChanged
        Dim FromDate As String = "" '= hdnFromDate.Value
        Dim ToDate As String = "" '= hdnToDate.Value

        If rdb24Hrs.Checked Or rdb48Hrs.Checked Then
            txtcalDateTime.Visible = True
            txtcalDateTime.Text = Date.Today.Date
        Else
            txtcalDateTime.Visible = False
        End If

        upnlWOGanttGraph.Update()

        If rdb48Hrs.Checked Or rdb24Hrs.Checked Then
            SetGanttChartValues("", "")
        Else
            Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek

            FromDate = DateTime.Now.AddDays(delta).ToString("dd/MM/yyyy")
            ToDate = DateAdd(DateInterval.Day, 6, CDate(FromDate)).ToString("dd/MM/yyyy")
            SetGantt7DaysChart(FromDate, ToDate)
        End If
    End Sub

    Private Sub txtcalDateTime_TextChanged(sender As Object, e As System.EventArgs) Handles txtcalDateTime.TextChanged

        If rdb24Hrs.Checked Then
            SetGanttChartValues(txtcalDateTime.Text.ToString, txtcalDateTime.Text.ToString)
        ElseIf rdb48Hrs.Checked Then
            SetGanttChartValues(txtcalDateTime.Text.ToString, DateAdd(DateInterval.Day, 1, CDate(txtcalDateTime.Text)).ToString)
        End If

    End Sub
#End Region

#Region " Methods "

    Public Sub SetGanttChartValues(FromDate As String, ToDate As String)


        If FromDate = "" And ToDate = "" Then
            If rdb48Hrs.Checked Then
                FromDate = Today.Date.ToString
                ToDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString
            ElseIf rdb24Hrs.Checked Then
                FromDate = Today.Date.ToString
                ToDate = Today.Date.ToString
            End If

        End If

        hdnFromDate.Value = FromDate
        hdnToDate.Value = ToDate
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        Dim mTaskList As nWOTaskCardListForGanttChart = nWOTaskCardListForGanttChart.GetnWOTaskCardListForGanttChart(FromDate:=FromDate.ToString, ToDate:=ToDate) 'DateAdd(DateInterval.Day, 1, Today.Date).ToString)
        'Dim TaskProcess As String = New JavaScriptSerializer().Serialize(mTaskList)
        'Dim task As String = New JavaScriptSerializer().Serialize(mTaskList)

        'TaskProcess = TaskProcess.Replace("TaskCardID", "id").Replace("TaskCardDescription", "label")
        'task = task.Replace("TaskCardID", "processid").Replace("TaskPlanStartDateFormatted", "start").Replace("TaskPlanEndDateFormatted", "end")
        Dim mTP As New StringBuilder
        Dim TaskPlanStartDateFormatted As New StringBuilder
        Dim TaskPlanEndDateFormatted As New StringBuilder
        Dim ActualStartDateFormatted As New StringBuilder
        Dim ActualEndDateFormatted As New StringBuilder

        Dim WONo As New StringBuilder
        'Dim ActualEndDateFormatted As New StringBuilder




        Dim CurrentTime As String = DateTime.Now.ToString("dd/MM/yyyy H:mm").Replace("-", "/")
        Dim CurrentOnlyTime As String = DateTime.Now.ToString("H:mm").Replace("-", "/")

        If FromDate <> Today.Date.ToString Then
            CurrentTime = ""
            CurrentOnlyTime = ""
        End If


        Dim tempWO As String = ""
        For i As Integer = 0 To mTaskList.Count - 1
            If tempWO = mTaskList(i).WONumber Then
                WONo.Append(" " + ";;")
            Else
                WONo.Append(mTaskList(i).WONumber + ";;")
            End If
            tempWO = mTaskList(i).WONumber
            mTP.Append(mTaskList(i).TaskCardDescription + ";;")

            TaskPlanStartDateFormatted.Append(mTaskList(i).PlanStartDateFormatted + ";;")
            TaskPlanEndDateFormatted.Append(mTaskList(i).PlanEndDateFormatted + ";;")
            ActualStartDateFormatted.Append(mTaskList(i).ActualStartDateFormatted + ";;")
            ActualEndDateFormatted.Append(mTaskList(i).ActualEndDateFormatted + ";;")
        Next


        If rdb48Hrs.Checked = True Then
            Dim CategoryList As New StringBuilder
            CategoryList.Append(CDate(FromDate).ToString("dd/MM/yyyy").Replace("-", "/") + ";;")
            CategoryList.Append(CDate(ToDate).ToString("dd/MM/yyyy").Replace("-", "/") + ";;")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionGanttFunc", "FusionGanttFunc('" + mTP.ToString.TrimEnd(";;") + "', '" + TaskPlanStartDateFormatted.ToString.TrimEnd(";;") + "', '" + TaskPlanEndDateFormatted.ToString.TrimEnd(";;") + "', '" + ActualStartDateFormatted.ToString.TrimEnd(";;") + "', '" + ActualEndDateFormatted.ToString.TrimEnd(";;") + "', '" + CategoryList.ToString.TrimEnd(";;") + "', '" + WONo.ToString.TrimEnd(";;") + "', '" + CurrentTime.ToString + "', '" + CurrentOnlyTime.ToString + "');", True)
        Else
            Dim CategoryList As New StringBuilder
            CategoryList.Append(CDate(FromDate).ToString("dd/MM/yyyy").Replace("-", "/") + ";;")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionGantt24HrsFunc", "FusionGantt24HrsFunc('" + mTP.ToString.TrimEnd(";;") + "', '" + TaskPlanStartDateFormatted.ToString.TrimEnd(";;") + "', '" + TaskPlanEndDateFormatted.ToString.TrimEnd(";;") + "', '" + ActualStartDateFormatted.ToString.TrimEnd(";;") + "', '" + ActualEndDateFormatted.ToString.TrimEnd(";;") + "', '" + CategoryList.ToString.TrimEnd(";;") + "', '" + WONo.ToString.TrimEnd(";;") + "', '" + CurrentTime.ToString + "', '" + CurrentOnlyTime.ToString + "');", True)
        End If

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionGanttFunc", "FusionGanttFunc();", True)
    End Sub
    Public Sub SetGantt7DaysChart(FromDate As String, ToDate As String)
        If FromDate = "" And ToDate = "" Then
            FromDate = Today.Date.ToString
            ToDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString
        End If

        hdnFromDate.Value = FromDate
        hdnToDate.Value = ToDate

        Dim mTaskList As nWOTaskCardListForGanttChart = nWOTaskCardListForGanttChart.GetnWOTaskCardListForGanttChart(FromDate:=FromDate, ToDate:=ToDate) 'DateAdd(DateInterval.Day, 1, Today.Date).ToString)
        Dim mTP As New StringBuilder
        Dim TaskPlanStartDateFormatted As New StringBuilder
        Dim TaskPlanEndDateFormatted As New StringBuilder
        Dim ActualStartDateFormatted As New StringBuilder
        Dim ActualEndDateFormatted As New StringBuilder
        Dim WONo As New StringBuilder

        Dim CategoryList As New StringBuilder

        Dim CurrentTime As String = DateTime.Now.ToString("dd/MM/yyyy H:mm").Replace("-", "/")
        Dim CurrentOnlyTime As String = DateTime.Now.ToString("H:mm").Replace("-", "/")

        If FromDate <> Today.Date.ToString Then
            CurrentTime = ""
            CurrentOnlyTime = ""
        End If

        For i As Integer = 0 To 6
            Dim tempDate As String = DateAdd(DateInterval.Day, i, CDate(FromDate)).ToString("dd/MM/yyyy")
            CategoryList.Append(CDate(tempDate).ToString("dd/MM/yyyy").Replace("-", "/") + ";;")
        Next

        Dim tempWO As String = ""
        For i As Integer = 0 To mTaskList.Count - 1
            If tempWO = mTaskList(i).WONumber Then
                WONo.Append(" " + ";;")
            Else
                WONo.Append(mTaskList(i).WONumber + ";;")
            End If
            tempWO = mTaskList(i).WONumber
            mTP.Append(mTaskList(i).TaskCardDescription + ";;")

            TaskPlanStartDateFormatted.Append(mTaskList(i).PlanStartDateFormatted + ";;")
            TaskPlanEndDateFormatted.Append(mTaskList(i).PlanEndDateFormatted + ";;")
            ActualStartDateFormatted.Append(mTaskList(i).ActualStartDateFormatted + ";;")
            ActualEndDateFormatted.Append(mTaskList(i).ActualEndDateFormatted + ";;")
        Next
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Fusion7DaysGanttFunc", "Fusion7DaysGanttFunc('" + mTP.ToString.TrimEnd(";;") + "', '" + TaskPlanStartDateFormatted.ToString.TrimEnd(";;") + "', '" + TaskPlanEndDateFormatted.ToString.TrimEnd(";;") + "', '" + ActualStartDateFormatted.ToString.TrimEnd(";;") + "', '" + ActualEndDateFormatted.ToString.TrimEnd(";;") + "', '" + CategoryList.ToString.TrimEnd(";;") + "', '" + WONo.ToString.TrimEnd(";;") + "', '" + CurrentTime.ToString + "', '" + CurrentOnlyTime.ToString + "');", True)

    End Sub
#End Region



End Class