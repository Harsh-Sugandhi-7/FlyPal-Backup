'CReated By: Ajay
'Dated:     30-Sep-2022

Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Public Class wfADSBReviewCalendar
    Inherits System.Web.UI.Page
#Region " Variable Declaration "
    Shared Count As Integer = 0
    Shared PlannedList As String = ""
    Shared tmpMonth As Integer = 0
    Shared tmpYear As Integer = 0
    Shared mDistinctADSBNo As DistinctADSBNo


    Shared tmpADSB As Integer = 0
#End Region
#Region " Methods "
    Private Sub GetSession()
        Count = Session("Count")
        PlannedList = ""
        mDistinctADSBNo = Session("mDistinctADSBText")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfADSBReviewCalendar.aspx?") <= 0 Then
            Session.Remove("mDistinctADSBText")

        End If
    End Sub
    Private Sub DatafieldBind()
        mDistinctADSBNo = DistinctADSBNo.GetDistinctADSBNoList(True, "(ALL)")
        cmbADSBNo.DataSource = mDistinctADSBNo
        Session("mDistinctADSBNo") = mDistinctADSBNo
        '   cmbADSBNo.SelectedIndex = 1
        cmbADSBNo.DataBind()
        hdnADSBNo.Value = ""

       
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfADSBReviewCalendar.aspx?"
            DatafieldBind()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)

        End If
    End Sub
    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CInt(Session("Count")) > 0 Then
                'Count = CInt(Session("Count")) + 1
                'hdncount.Value = Count
                'Session("Count") = Count
                If CInt(Session("Count")) = 3 Then
                    Count = 0
                    Session("Count") = Count
                    Session("ChangeForm") = "ChangeForm"
                    'Timer1.Enabled = False
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
#Region "Web Methods"

    '    <WebMethod(EnableSession:=True)> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function TestOnWebService(ADSBNo As String, month As String, year As String) As String

        ' Dim mnWOPlannedList As nWOList
        Dim mADSBReviewList As ADSBReviewRegisterReport
        Dim StartDateM As New SmartDate
        Dim EndDateM As New SmartDate
        Dim tmpADSBNo As String
        StartDateM = New SmartDate(DateAdd(DateInterval.Month, 0, DateSerial(Val(year), Val(month) + 1, 1)), False)
        EndDateM = New SmartDate(CStr(DateSerial(StartDateM.Date.Year, StartDateM.Date.Month, DateTime.DaysInMonth(StartDateM.Date.Year, StartDateM.Date.Month))), False)
        If Not (Not tmpADSBNo = ADSBNo) Or Not (tmpMonth = Val(month)) Or Not (tmpYear = Val(year)) Then
            tmpADSBNo = ADSBNo
            tmpMonth = Val(month) + 1
            tmpYear = Val(year)

            If ADSBNo = "(ALL)" Then ADSBNo = ""

            mADSBReviewList = ADSBReviewRegisterReport.GetADSBReviewRegisterList(FromDate:=StartDateM.ToString, ToDate:=EndDateM.ToString, ADSBNo:=ADSBNo)
            PlannedList = New JavaScriptSerializer().Serialize(mADSBReviewList)
        End If





            Dim jss = New JavaScriptSerializer()

            Dim data = jss.Deserialize(Of Object)(PlannedList) 'JsonConvert.DeserializeObject(Of MaintenanceActiivtyStatusList.MaintenanceActiivtyStatusListInfo)(DueValues)


        PlannedList = PlannedList.Replace("ADSBNo", "title").Replace("IssueDateCalender", "start").Replace("ID", "id")

            Return PlannedList
    End Function
#End Region
End Class