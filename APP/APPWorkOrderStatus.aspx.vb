Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports InfoSoftGlobal
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Services
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Imports System
Imports System.IO

Public Class APPWorkOrderStatus
    Inherits System.Web.UI.Page


#Region " Variables "
    Public mnWOStatusCountDashboard As nWOStatusCountDashboard
    Public mnWOStatusCountForPieGraph As nWOStatusCountForPieGraph
    Dim mMissingMonthData As Object
    Dim mMonthList As MonthList
    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
#End Region


#Region " Methods "
    Private Sub GetSession()
        mUser = Session("User")
        mGBUser = Session("GBUser")
        'mEventLogSession = Session("EventLogSession")
    End Sub

    'Private Sub ApplyRights()
    '    Try

    '        'If (mUser.IsInRole("CrewRosterNew") Or mUser.IsInRole("CrewRosterEdit") Or mUser.IsInRole("CrewRosterDelete") Or mUser.IsInRole("CrewRosterView") Or mUser.IsInRole("CrewRosterPrint")) = False Then
    '        If User.IsInRole("StoreBalanceView") = False Then '
    '            PartAvailabilityNode.Attributes("style") = "display: none"
    '            iPartAvailability.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
    '        End If

    '        If User.IsInRole("CalibrationDueReportView") = False Then
    '            CalibrationStatusNode.Attributes("style") = "display: none"
    '            'CalibrationStatusNode.Attributes("style") = "pointer-events: none"
    '            iCalibrationStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
    '        End If

    '        If User.IsInRole("ExpiryDateView") = False Then
    '            ExpiryStatusNode.Attributes("style") = "display: none"
    '            iExpiryStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
    '        End If

    '        If User.IsInRole("EmployeeDocumentDueList") = False Then
    '            EmployeeDocumentStatusNode.Attributes("style") = "display: none"
    '            iEmployeeDocumentStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
    '        End If

    '        If User.IsInRole("EmployeeTrainnigDueList") = False Then
    '            EmployeeTrainingStatusNode.Attributes("style") = "display: none"
    '            iEmployeeTrainingStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
    '        End If

    '        'If User.IsInRole("AircraftCurrentStatusView") = False Then '
    '        '    AircraftCurrentStatusNode.Attributes("style") = "pointer-events: none;display:none"
    '        '    iAircraftCurrentStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
    '        'End If

    '        If User.IsInRole("ShowWODashBoardView") = False Then '
    '            APPWorkOrderStatusNode.Attributes("style") = "pointer-events: none;display:none"
    '            hrefTimeline.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
    '        End If

    '        ''BottomMenu Rights
    '        ''
    '        If User.IsInRole("AircraftCurrentStatusView") = False Then '
    '            hrefTimeline.Attributes("style") = "pointer-events: none"
    '            iTimeline.Attributes.Remove("style")
    '        End If

    '        ''Flights
    '        If mUser.IsInRole("LogView") Then
    '            hrefFlights.Attributes("style") = "pointer-events: none"
    '            iFlights.Attributes.Remove("style")
    '        End If

    '        ''Availability
    '        ''
    '        ''
    '        'hrefAvailability.Attributes("style") = "pointer-events: none"
    '        'iAvailability.Attributes.Remove("style")

    '        ''Profile
    '        'If (mUser.IsInRole("CrewNew") Or mUser.IsInRole("CrewEdit") Or mUser.IsInRole("CrewDelete") Or mUser.IsInRole("CrewView") Or mUser.IsInRole("CrewPrint")) = False Then
    '        '    hrefProfile.Attributes("style") = "pointer-events: none"
    '        '    iProfile.Attributes.Remove("style")
    '        'End If

    '        ''-----


    '    Catch ex As Exception

    '    End Try


    'End Sub
    Public Sub DatafieldBind()
        mnWOStatusCountDashboard = nWOStatusCountDashboard.GetnWOStatusCountDashboard()
        Session("mnWOStatusCountDashboard") = mnWOStatusCountDashboard

        mnWOStatusCountForPieGraph = nWOStatusCountForPieGraph.GetWOStatusCountForPieGraph(mnWOStatusCountDashboard)
        Session("mnWOStatusCountForPieGraph") = mnWOStatusCountForPieGraph

        mMonthList = MonthList.GetMonthList()
        Session("mMonthList") = mMonthList

        mMissingMonthData = (From c In mMonthList
                          Select New With {c.Id, Today.Year, .MonthYear = c.MonthYear + " " + Today.Year.ToString, .RecordCount = 0})
        Session("mMissingMonthData") = mMissingMonthData

        SetPieGraph()
        GetMonthlyWorkOrder()
    End Sub
    Public Sub SetPieGraph()

        Dim PieGraphWOStatusCountForPieGraphValues As String = New JavaScriptSerializer().Serialize(mnWOStatusCountForPieGraph)
        PieGraphWOStatusCountForPieGraphValues = PieGraphWOStatusCountForPieGraphValues.Replace("StatusName", "label").Replace("StatusCnt", "value")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartPieFunc", "FusionChartPieFunc('" + PieGraphWOStatusCountForPieGraphValues.ToString + "');", True)
    End Sub
    Public Sub GetMonthlyWorkOrder()

        ' Dim mnWOPlannedList As nWOList
        Dim mnWOPlannedList As nWOListForPlanCalendar

        mnWOPlannedList = nWOListForPlanCalendar.GetWOListForPlanCalendar(FromDate:="1/1/1900", ToDate:="1/1/2200")

        Dim mnWOPlannedListGraph As Object
        Dim mnWOPlannedListFinalGraph As Object 'As List(Of String) = New List(Of String) '


        mnWOPlannedListGraph = (From c In mnWOPlannedList
                                Where (c.year = Today.Year)
                                Order By c.month Ascending, c.year Ascending
                                Group By mMonth = c.Month, mYear = c.Year, mMonthYear = c.MonthYear Into Group
                                Select New With {.Month = mMonth, .Year = mYear, .MonthYear = mMonthYear, .Recordcount = Group.Count})

        Dim templist As New System.Collections.ArrayList
        Dim tempinfo As nWOListForPlanCalendar.nWOListForDueJobsInfo


        For Each variable As Object In mMissingMonthData
            tempinfo.SortOrder = variable.Recordcount
            tempinfo.Year = variable.Year
            tempinfo.Month = variable.ID
            For Each variable1 As Object In mnWOPlannedListGraph
                If variable.id = variable1.month Then
                    tempinfo.SortOrder = variable1.Recordcount
                    Exit For
                End If
            Next

            templist.Add(tempinfo)
        Next

        mnWOPlannedListFinalGraph = (From c In templist
                         Select New With {c.Month, c.Year, .MonthYear = c.MonthYear, .RecordCount = c.SortOrder})


        Dim GraphWOPlannedListValues As String = New JavaScriptSerializer().Serialize(mnWOPlannedListFinalGraph)
        GraphWOPlannedListValues = GraphWOPlannedListValues.Replace("MonthYear", "label").Replace("Recordcount", "value")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthlyWorkOrder", "MonthlyWorkOrder('" + GraphWOPlannedListValues.ToString + "');", True)

    End Sub
    Private Sub ShowAlertMsg(ByVal Msg As String, ByVal MsgTitle As String)
        Dim str As String
        str = "opennotificationpopup('" & Msg & "','" & MsgTitle & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
    End Sub

    Private Sub DisableButtons()
        Try

            hrefTimeline.Attributes("style") = "pointer-events: none"
            iTimeline.Attributes.Remove("style")

            hrefFlights.Attributes("style") = "pointer-events: none"
            iFlights.Attributes.Remove("style")

            hrefAvailability.Attributes("style") = "pointer-events: none"
            iAvailability.Attributes.Remove("style")

            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")

        Catch ex As Exception

        End Try

    End Sub
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If mGBUser Is Nothing Then
            ShowAlertMsg("Session expired ! Please click on Home button on left side menu.", "Session expired..")
            DisableButtons()
            Exit Sub
        End If

        If Not IsPostBack Then
            DatafieldBind()
        End If
        ''ApplyRights()
        'BottomMenu Rights
        '          
        If User.IsInRole("AircraftCurrentStatusView") = False Then '
            hrefTimeline.Attributes("style") = "pointer-events: none"
            iTimeline.Attributes.Remove("style")
        End If

        ''Flights
        If User.IsInRole("LogView") = False Then
            hrefFlights.Attributes("style") = "pointer-events: none"
            iFlights.Attributes.Remove("style")
        End If

        ''Availability
        ''
        ''
        'hrefAvailability.Attributes("style") = "pointer-events: none"
        'iAvailability.Attributes.Remove("style")

        ''Profile
        'If (mUser.IsInRole("CrewNew") Or mUser.IsInRole("CrewEdit") Or mUser.IsInRole("CrewDelete") Or mUser.IsInRole("CrewView") Or mUser.IsInRole("CrewPrint")) = False Then
        '    hrefProfile.Attributes("style") = "pointer-events: none"
        '    iProfile.Attributes.Remove("style")
        'End If

        ''-----
        'Profile
        hrefProfile.Attributes("style") = "pointer-events: none"
        iProfile.Attributes.Remove("style")

    End Sub

    Protected Sub lnkHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHome.Click
        Try
            Response.Redirect("APPMenu.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub
End Class