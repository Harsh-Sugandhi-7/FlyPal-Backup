

'Created By: Saylee
'Dated:      6-Dec-2023




Imports System.Text

Public Class wfDuePlanningGanttChart
    Inherits System.Web.UI.Page

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            txtcalDateTime.Text = Today.Date.ToString(AppSettings("DateFormat"))
            SetGantt7DaysChart("", "")
            btnPrev.Visible = True
            btnNext.Visible = True
            btnToday.Visible = True
            Session("MiddleFrame") = "wfDuePlanningGanttChart.aspx?"
        Else
            SetGantt7DaysChart("", "")
        End If


    End Sub

    Private Sub btnPrev_Click(sender As Object, e As System.EventArgs) Handles btnPrev.Click
        Dim FromDate As String = "" '= hdnFromDate.Value
        Dim ToDate As String = "" '= hdnToDate.Value
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        ''If rdb48Hrs.Checked Then
        ''    FromDate = DateAdd(DateInterval.Day, -1, CDate(FromDate)).ToString
        ''    ToDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString
        ''    SetGanttChartValues(FromDate, ToDate)
        ''    'Else
        ''    '    Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek

        ''    '    FromDate = DateTime.Now.AddDays(delta)
        ''    '    ToDate = DateAdd(DateInterval.Day, 6, CDate(FromDate)).ToString
        ''    '    SetGantt7DaysChart(FromDate, ToDate)
        ''ElseIf rdb24Hrs.Checked Then
        ''    FromDate = DateAdd(DateInterval.Day, -1, CDate(FromDate)).ToString
        ''    ToDate = FromDate
        ''    SetGanttChartValues(FromDate, ToDate)
        ''End If
        Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek
        FromDate = DateAdd(DateInterval.Day, -7, CDate(FromDate)).ToString

        ToDate = DateAdd(DateInterval.Day, 6, CDate(FromDate)).ToString(AppSettings("DateFormat"))
        SetGantt7DaysChart(FromDate, ToDate)
    End Sub
    Private Sub btnToday_Click(sender As Object, e As System.EventArgs) Handles btnToday.Click
        Dim FromDate As String = ""
        Dim ToDate As String = ""
        Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek

        FromDate = CDate(Today.Date.ToString).AddDays(delta).ToString(AppSettings("DateFormat"))
        ToDate = DateAdd(DateInterval.Day, 6, CDate(Today.Date.ToString)).ToString(AppSettings("DateFormat"))
        SetGantt7DaysChart(FromDate, ToDate)

    End Sub
    Private Sub btnNext_Click(sender As Object, e As System.EventArgs) Handles btnNext.Click
        Dim FromDate As String = "" '= hdnFromDate.Value
        Dim ToDate As String = "" '= hdnToDate.Value
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        'If rdb48Hrs.Checked Then
        '    FromDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString 'DateAdd(DateInterval.Day, 1, CDate(hdnFromDate.Value)).ToString
        '    ToDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString

        '    SetGanttChartValues(FromDate, ToDate)
        'ElseIf rdb24Hrs.Checked Then
        '    FromDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString
        '    ToDate = FromDate
        '    SetGanttChartValues(FromDate, ToDate)
        'End If
        Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek

        FromDate = DateAdd(DateInterval.Day, 7, CDate(FromDate)).ToString
        ToDate = DateAdd(DateInterval.Day, 6, CDate(FromDate)).ToString(AppSettings("DateFormat"))
        SetGantt7DaysChart(FromDate, ToDate)
    End Sub


    Private Sub txtcalDateTime_TextChanged(sender As Object, e As System.EventArgs) Handles txtcalDateTime.TextChanged
        Dim FromDate As String = "" '= hdnFromDate.Value
        Dim ToDate As String = "" '= hdnToDate.Value
        'If rdb24Hrs.Checked Then
        '    SetGanttChartValues(txtcalDateTime.Text.ToString, txtcalDateTime.Text.ToString)
        'ElseIf rdb48Hrs.Checked Then
        '    SetGanttChartValues(txtcalDateTime.Text.ToString, DateAdd(DateInterval.Day, 1, CDate(txtcalDateTime.Text)).ToString)
        'End If
        Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek

        FromDate = CDate(txtcalDateTime.Text.ToString).AddDays(delta).ToString(AppSettings("DateFormat"))
        ToDate = DateAdd(DateInterval.Day, 6, CDate(txtcalDateTime.Text.ToString)).ToString(AppSettings("DateFormat"))
        SetGantt7DaysChart(FromDate, ToDate)

    End Sub
    Private Sub hdnBtnDueJobPlanning_Click(sender As Object, e As EventArgs) Handles hdnBtnDueJobPlanning.Click
        Dim mDueJobPlanning As DueJobPlanning
        mDueJobPlanning = DueJobPlanning.GetDueJobPlanning(New Guid(hdnID.Value.ToString))
        mDueJobPlanning.MarkClean()
        Session("mDueJobPlanning") = mDueJobPlanning
        If (New Guid(hdnWOID.Value.ToString)).Equals(Guid.Empty) Then

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDueJobPlanningSelectionWindow", "OpenDueJobPlanningSelectionWindow();", True)
        Else
            Dim DueWO As nWO
            DueWO = nWO.GetWO(mDueJobPlanning.WOID)
            Session("mnWO") = DueWO
            Dim str As String
            str = "openledgersame('wfnWODetail_Ajax.aspx?BackPage=index.aspx');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End If
    End Sub
#End Region

#Region " Methods "
    Public Sub SetGantt7DaysChart(FromDate As String, ToDate As String)
        If FromDate = "" And ToDate = "" Then
            'FromDate = Today.Date.ToString
            'ToDate = DateAdd(DateInterval.Day, 1, CDate(FromDate)).ToString
            Dim delta As Integer = DayOfWeek.Monday - DateTime.Now.DayOfWeek

            FromDate = DateTime.Now.AddDays(delta).ToString(AppSettings("DateFormat"))
            ToDate = DateAdd(DateInterval.Day, 6, CDate(FromDate)).ToString(AppSettings("DateFormat"))
        End If

        Dim WONo As New StringBuilder
        Dim CategoryList As New StringBuilder
        For i As Integer = 0 To 6
            Dim tempDate As String = DateAdd(DateInterval.Day, i, CDate(FromDate)).ToString(AppSettings("DateFormat"))
            'CategoryList.Append(CDate(tempDate).ToString(AppSettings("DateFormat")).Replace("-", "/") + ";;")
            CategoryList.Append(CDate(tempDate).ToString(AppSettings("DateFormat")) + ";;")
        Next

        hdnFromDate.Value = FromDate
        hdnToDate.Value = ToDate
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        ' Dim mTaskList As nWOTaskCardListForGanttChart = nWOTaskCardListForGanttChart.GetnWOTaskCardListForGanttChart(FromDate:=FromDate, ToDate:=ToDate) 'DateAdd(DateInterval.Day, 1, Today.Date).ToString)
        Dim mPlannedList As DueJobPlanningList = DueJobPlanningList.GetDueJobPlanningList(FromDate:=FromDate, ToDate:=ToDate, IsAsperPlanningDate:=True)
        ' Dim mTP As New StringBuilder
        Dim TaskPlanStartDateFormatted As New StringBuilder
        Dim TaskPlanEndDateFormatted As New StringBuilder

        Dim ActualStartDateFormatted As New StringBuilder
        Dim ActualEndDateFormatted As New StringBuilder
        Dim WOPlannedDateFormatted As New StringBuilder

        Dim PlannedID As New StringBuilder

        Dim PlanningNo As New StringBuilder
        Dim WOID As New StringBuilder

        Dim CurrentTime As String = DateTime.Now.ToString(AppSettings("TimeFormat").ToString).Replace("-", "/")
        Dim CurrentOnlyTime As String = DateTime.Now.ToString(AppSettings("TimeFormat").ToString).Replace("-", "/")

        If FromDate <> Today.Date.ToString Then
            CurrentTime = ""
            CurrentOnlyTime = ""
        End If



        Dim tempPlanningNo As String = ""
        Dim tempWO As Guid = Guid.Empty
        For i As Integer = 0 To mPlannedList.Count - 1
            If tempPlanningNo = mPlannedList(i).DueJobPlanningNo Then
                PlanningNo.Append(" " + ";;")
                PlannedID.Append(" " + ";;")
            Else
                PlanningNo.Append(mPlannedList(i).DueJobPlanningNo + ";;")
                PlannedID.Append(mPlannedList(i).ID.ToString + ";;")
            End If

            If tempWO.Equals(mPlannedList(i).WOID) Then
                WONo.Append(" " + ";;")
                WOID.Append(Guid.Empty.ToString + ";;")
                ActualStartDateFormatted.Append(" " + ";;")
                ActualEndDateFormatted.Append(" " + ";;")
                WOPlannedDateFormatted.Append(" " + ";;")
            Else
                WONo.Append(mPlannedList(i).WONumber + ";;")
                WOID.Append(mPlannedList(i).WOID.ToString + ";;")
                If mPlannedList(i).WOStartDateFormatted.ToString <> "" Then
                    ActualStartDateFormatted.Append(mPlannedList(i).WOStartDateFormatted + ";;")
                Else
                    ActualStartDateFormatted.Append(" " + ";;")
                End If

                If mPlannedList(i).WOCloseDateFormatted.ToString <> "" Then
                    ActualEndDateFormatted.Append(mPlannedList(i).WOCloseDateFormatted + ";;")
                Else
                    ActualEndDateFormatted.Append(" " + ";;")
                End If

                If mPlannedList(i).WOPlanDateFormatted.ToString <> "" Then
                    WOPlannedDateFormatted.Append(mPlannedList(i).WOPlanDateFormatted + ";;")
                Else
                    WOPlannedDateFormatted.Append(" " + ";;")
                End If

            End If

            tempPlanningNo = mPlannedList(i).DueJobPlanningNo
            ' mTP.Append(mPlannedList(i).DueJobPlanningNo + ";;")
            tempWO = mPlannedList(i).WOID

            TaskPlanStartDateFormatted.Append(mPlannedList(i).FromDateFormatted + ";;")
            TaskPlanEndDateFormatted.Append(mPlannedList(i).ToDateFormatted + ";;")



        Next
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Fusion7DaysGanttFunc", "Fusion7DaysGanttFunc('" + TaskPlanStartDateFormatted.ToString.TrimEnd(";;") + "', '" + TaskPlanEndDateFormatted.ToString.TrimEnd(";;") + "', '" + CategoryList.ToString.TrimEnd(";;") + "', '" + PlanningNo.ToString.TrimEnd(";;") + "', '" + CurrentTime.ToString + "', '" + CurrentOnlyTime.ToString + "', '" + AppSettings("DateFormat").ToString + "', '" + AppSettings("DateFormat").ToString.Replace("MMM", "mns") + "', '" + PlannedID.ToString.TrimEnd(";;") + "', '" + WONo.ToString.TrimEnd(";;") + "', '" + ActualStartDateFormatted.ToString.TrimEnd(";;") + "', '" + ActualEndDateFormatted.ToString.TrimEnd(";;") + "', '" + WOID.ToString.TrimEnd(";;") + "', '" + WOPlannedDateFormatted.ToString.TrimEnd(";;") + "');", True)


    End Sub
#End Region


End Class