Imports System.Web.Script.Serialization

Public Class wfDuePlanningCalendar
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Shared PlannedList As String = ""
    Shared tmpMonth As Integer = 0
    Shared tmpYear As Integer = 0
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfDuePlanningCalendar.aspx?"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)

        End If
    End Sub
    Private Sub hdnBtnDueJobPlanning_Click(sender As Object, e As EventArgs) Handles hdnBtnDueJobPlanning.Click
        Dim mDueJobPlanning As DueJobPlanning
        mDueJobPlanning = DueJobPlanning.GetDueJobPlanning(New Guid(hdnID.Value.ToString))
        mDueJobPlanning.MarkClean()
        Session("mDueJobPlanning") = mDueJobPlanning
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDueJobPlanningSelectionWindow", "OpenDueJobPlanningSelectionWindow();", True)
    End Sub
#End Region

#Region "Web Methods"

    '    <WebMethod(EnableSession:=True)> _
    <System.Web.Services.WebMethod()>
    Public Shared Function TestOnWebService(month As String, year As String) As String


        'Dim mnWOPlannedList As nWOListForPlanCalendar
        Dim mDueJobPlanningList As DueJobPlanningList
        Dim StartDateM As New SmartDate
        Dim EndDateM As New SmartDate

        StartDateM = New SmartDate(DateAdd(DateInterval.Month, 0, DateSerial(Val(year), Val(month) + 1, 1)), False)
        EndDateM = New SmartDate(CStr(DateSerial(StartDateM.Date.Year, StartDateM.Date.Month, DateTime.DaysInMonth(StartDateM.Date.Year, StartDateM.Date.Month))), False)
        If (Not (tmpMonth = Val(month)) Or Not (tmpYear = Val(year))) Then
            'mnWOPlannedList = nWOList.GetWOList(WOStatusID:=4)

            tmpMonth = Val(month)
            tmpYear = Val(year)
            mDueJobPlanningList = DueJobPlanningList.GetDueJobPlanningList(FromDate:=StartDateM.ToString, ToDate:=EndDateM.ToString, IsAsperPlanningDate:=True)
            PlannedList = New JavaScriptSerializer().Serialize(mDueJobPlanningList)
        End If




        Dim jss = New JavaScriptSerializer()

        Dim data = jss.Deserialize(Of Object)(PlannedList) 'JsonConvert.DeserializeObject(Of MaintenanceActiivtyStatusList.MaintenanceActiivtyStatusListInfo)(DueValues)


        PlannedList = PlannedList.Replace("DueJobPlanningNo", "title").Replace("FromDateFormattedCalender", "start").Replace("ID", "id")
        '  PlannedList = PlannedList.Replace("DescriptionCalender", "title").Replace("WOPlanedAndWODateCalender", "start").Replace("ID", "id")
        Return PlannedList
    End Function
#End Region

End Class