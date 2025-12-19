Imports System.Linq
Imports System.Collections
Public Class wfScheduleDetail_Ajax
    Inherits System.Web.UI.Page

#Region "Enumeration"
    Enum WeekDay
        Monday = 1
        Tuesday = 2
        Wednesday = 3
        Thursday = 4
        Friday = 5
        Saturday = 6
        Sunday = 7
    End Enum
#End Region

#Region " Variable Declaration "

    Public mRoute As Route
    Public mWeekSchedule As WeekSchedule
    Dim WeekDaysIDs(50) As Integer
    Public mSearchListPilot As SearchList
    Public mSearchListPlace As SearchList
    Dim ApTime As String
    Dim DpTime As String

    ' Dim ApTime1 As String
    'im DpTime1 As String
#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mRoute = CType(Session("mRoute"), Route)
        mWeekSchedule = CType(Session("mWeekSchedule"), WeekSchedule)
        mSearchListPlace = Session("mSearchListPlace")

    End Sub

    Private Sub SetSession()
        Session("mRoute") = mRoute
        Session("mSearchListPlace") = mSearchListPlace
        Session("mWeekSchedule") = mWeekSchedule
    End Sub

    Private Sub setControl()
        TxtDepartureTime.Text = Format(CDate(mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC), AppSettings("TimeFormat"))
        TxtArrivalTime.Text = Format(CDate(mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC), AppSettings("TimeFormat"))
        'TxtDepartureTime.Text = mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC.ToString(AppSettings("TimeFormat"))
        ' TxtArrivalTime.Text = mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC

    End Sub

    Private Function SetObject(ByVal ValidaDate As Date) As Boolean
        'mRoute.BeginEdit()
        ' mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC = Trim(TxtDepartureTime.Text) 'New DateTime(0,0,0,Trim(TxtDepartureTime.Text)
        ' mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC = Trim(TxtArrivalTime.Text)
        'mRoute.RouteSchedules.CurrentItem.FromPlace = Trim(TxtFromplace.Text)
        'mRoute.RouteSchedules.CurrentItem.ToPlace = Trim(TxtToplace.Text)
        'mRoute.RouteSchedules.CurrentItem.FlightTime = 0.0

        mRoute.RouteSchedules.CurrentItem.SrNo = mRoute.RouteSchedules.CurrentIndex + 1
        mRoute.RouteSchedules.CurrentItem.FlightNo = Trim(txtFlightNo.Text)
        '''  DpTime = CType(ValidaDate, String) + " " + TxtDepartureTime.Text.ToString.Trim
        '''ApTime = CType(ValidaDate, String) + " " + TxtArrivalTime.Text.ToString.Trim

        DpTime = CType(CType(ValidaDate, String) + " " + TxtDepartureTime.Text.ToString.Trim, DateTime)
        ApTime = CType(CType(ValidaDate, String) + " " + TxtArrivalTime.Text.ToString.Trim, DateTime)

        ' DpTime1 = Format(DpTime, AppSettings("TimeFormat"))
        '  ApTime1 = Format(ApTime, AppSettings("TimeFormat"))

        Dim tempString As String
        Dim tempString1 As String
        tempString = TxtFromplace.Text.Trim
        If Not tempString = String.Empty Then
            If tempString.IndexOf("[") >= 0 Then
                tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
            End If
        End If
        tempString1 = TxtToplace.Text.Trim
        If Not tempString1 = String.Empty Then
            If tempString1.IndexOf("[") >= 0 Then
                tempString1 = tempString1.Substring(0, tempString1.IndexOf("[")).Trim
            End If
        End If

        mRoute.RouteSchedules.CurrentItem.FromPlaceID = mSearchListPlace.Item(tempString).GId
        mRoute.RouteSchedules.CurrentItem.FromPlace = mSearchListPlace.Item(tempString).Name
        mRoute.RouteSchedules.CurrentItem.ToPlaceID = mSearchListPlace.Item(tempString1).GId
        mRoute.RouteSchedules.CurrentItem.ToPlace = mSearchListPlace.Item(tempString1).Name
        mRoute.RouteSchedules.CurrentItem.WeekDaysID = (CInt(IIf(ValidaDate.DayOfWeek.ToString = "Sunday", 7, ValidaDate.DayOfWeek)))
        mRoute.RouteSchedules.CurrentItem.FlightTime = New Period(1, CType(CDate(ApTime) - CDate(DpTime), TimeSpan).Hours).DbValueDec
        mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC = CType(CType(ValidaDate, String) + " " + TxtDepartureTime.Text.ToString.Trim, DateTime) 'DpTime
        'mRoute.RouteSchedules.CurrentItem.DepartureTimeLocal = CDate(mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC).Subtract(mRoute.RouteSchedules.CurrentItem.SourceGMT)
        mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC = CType(CType(ValidaDate, String) + " " + TxtArrivalTime.Text.ToString.Trim, DateTime) 'ApTime
        '  mRoute.RouteSchedules.CurrentItem.ArrivalTimeLocal = CDate(mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC).Subtract(mRoute.RouteSchedules.CurrentItem.DestinationGMT)
        Session("mRoute") = mRoute
        Return True
    End Function

    Private Sub SetTitle()
        If mRoute.IsNew Then
            lblTitle.Text = "Schedule Details [New]"
        End If
        lblTitle.Text = "Schedule Details [" & mRoute.RouteName & "]"
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DatafieldBind()
        mWeekSchedule = WeekSchedule.GetWeekSchedule()
        ChckWeekDays.DataSource = mWeekSchedule
        'ChckWeekDays.DataBind()
        Session("mWeekSchedule") = mWeekSchedule
        mSearchListPlace = SearchList.GetSearchList("Place", "", "")
        Session("mSearchListPlace") = mSearchListPlace
        If mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC.ToString() = "" Or mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC.ToString() = "" Then
            TxtDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
            TxtArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            setControl()
        End If
        DataBind()
        For j As Integer = 0 To mWeekSchedule.Count - 1
            'For j As Integer = 0 To j <= i
            If mRoute.RouteSchedules.CurrentItem.WeekDaysID = mWeekSchedule(j).ID Then
                ChckWeekDays.Items(j).Selected = True
            End If
        Next

    End Sub

#End Region

         
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "TxtArrivalTime" Then
            Dim i As Integer
            Dim flag As Boolean = False
            For i = 0 To ChckWeekDays.Items.Count - 1
                If ChckWeekDays.Items(i).Selected Then
                    flag = True
                End If
            Next
            If flag = False Then
                custValidator.ErrorMessage = "Please select the Days"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        'If Not custValidator.ControlToValidate = "TxtArrivalTime" Then
        '    Dim i As Integer
        '    Dim flag As Boolean = True

        '    For i = 0 To ChckWeekDays.Items.Count - 1
        '        If ChckWeekDays.Items(i).Selected Then
        '            flag = True
        '        End If
        '    Next
        '    If flag = True Then
        '        custValidator.ErrorMessage = "Please select the Days"
        '        e.IsValid = True
        '    Else
        '        e.IsValid = False
        '    End If
        'End If
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)

        Dim custValidator1 As CustomValidator
        custValidator1 = CType(s, CustomValidator)
        If custValidator1.ControlToValidate = "TxtArrivalTime" Then
           Dim flag As Boolean = False
            If mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC.ToString <> "" And mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC.ToString <> "" Then
                If CDate(mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC).ToString(AppSettings("TimeFormat")) < CDate(mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC).ToString(AppSettings("TimeFormat")) Then
                    flag = True
                Else
                    If flag = False Then
                        custValidator1.ErrorMessage = "Departure Time Should be less than Arrival Time"
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                End If
            End If
           

        End If
    End Sub
    Private Sub TxtDepartureTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtDepartureTime.TextChanged
        For i As Integer = 0 To CType(CDate(mRoute.ValidTo) - CDate(mRoute.ValidFrom), TimeSpan).Days

            If IsValidTime(TxtDepartureTime.Text.ToString.Trim) = False Then
                TxtDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
            Else
                SetObject(CDate(mRoute.ValidFrom).AddDays(i))
                Dim DateTime As String = DpTime
                If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mRoute.ValidFrom.ToString), New SmartDate(DateTime).Date) <> 0 Then
                    mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC = DateTime
                    Session("mRoute") = mRoute
                End If
            End If
        Next
    End Sub

    Private Sub TxtArrivalTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtArrivalTime.TextChanged
        For i As Integer = 0 To CType(CDate(mRoute.ValidTo) - CDate(mRoute.ValidFrom), TimeSpan).Days

            If IsValidTime(TxtArrivalTime.Text.ToString.Trim) = False Then
                TxtArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
            Else
                SetObject(CDate(mRoute.ValidFrom).AddDays(i))
                Dim DateTime1 As String = ApTime
                If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mRoute.ValidFrom.ToString), New SmartDate(DateTime1).Date) <> 0 Then
                    mRoute.RouteSchedules.CurrentItem.ArrivalTimeUTC = DateTime1
                    Session("mRoute") = mRoute
                End If
            End If
        Next
    End Sub

    Private Function IsValidTime(ByVal TimeValue As String) As Boolean
        Dim TimeRegulerExpression As String = ""
        If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
            'TimeRegulerExpression = "^(([01][\d]+)|(2[0-3]))\:[0-5][0-9]( )*(AM|am|PM|pm)$"   '12 Hour Format
            TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
        Else
            TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
        End If
        If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            DatafieldBind()
        End If
        'SetTitle()
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click

        If IsValid Then
            WeekDaysIDs = (From c As System.Web.UI.WebControls.ListItem In ChckWeekDays.Items
                             Where c.Selected = True
                             Select CInt(c.Value)).ToArray
            For i As Integer = 0 To CType(CDate(mRoute.ValidTo) - CDate(mRoute.ValidFrom), TimeSpan).Days
                'For j As Integer = 0 To mWeekSchedule.Count - 1
                '    'For j As Integer = 0 To j <= i
                '    If Array.IndexOf(WeekDaysIDs, mWeekSchedule(j).ID) <> -1 And CDate(mRoute.ValidFrom).AddDays(i).DayOfWeek.ToString = mWeekSchedule(j).Day Then
                '        SetObject(CDate(mRoute.ValidFrom).AddDays(i))
                '        mRoute.RouteSchedules.Add(mRoute.ID)
                '        mRoute.RouteSchedules.CurrentIndex = mRoute.RouteSchedules.Count - 1
                '    End If
                'Next
                For j As Integer = 0 To mWeekSchedule.Count - 1
                    'For j As Integer = 0 To j <= i
                    If Array.IndexOf(WeekDaysIDs, mWeekSchedule(j).ID) <> -1 And CDate(mRoute.ValidFrom).AddDays(i).DayOfWeek.ToString = mWeekSchedule(j).Day Then
                        SetObject(CDate(mRoute.ValidFrom).AddDays(i))
                        mRoute.RouteSchedules.Add(mRoute.ID)
                        mRoute.RouteSchedules.CurrentIndex = mRoute.RouteSchedules.Count - 1
                    End If
                Next

            Next
            If Not mRoute.RouteSchedules(mRoute.RouteSchedules.Count - 1).IsValid Then
                mRoute.RouteSchedules.Remove(mRoute.RouteSchedules(mRoute.RouteSchedules.Count - 1))
            End If
            mRoute.Save()
            SetSession()

            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        Else
            upnlValidationSAummary.Update()
        End If
        'Dim mopenas As String = Request.QueryString("Type")
        'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
        '    Exit Sub
        'End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        If Session("EditmRoute") = False Then Session.Remove("EditmRoute") : mRoute.RouteSchedules.Remove(mRoute.RouteSchedules.CurrentItem)
        Session("EditmRoute") = ""
        mRoute.CancelEdit()
        Session("mRoute") = mRoute
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub

End Class