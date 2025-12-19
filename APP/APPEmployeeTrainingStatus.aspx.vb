'Created By : Prashant 
'Dated: 14-Jun-2021
Imports System.Collections.Generic
Imports System.Linq
Public Class APPEmployeeTrainingStatus
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    Dim mAPPEmployeeTrainningDueList As EmployeeTrainningDueList
    Dim mAPPEmployeeList As EmployeeList
    Dim EventLogID As Guid
#End Region

#Region "Helper Method"

    Private Sub GetSession()
        mUser = Session("User")
        mGBUser = Session("GBUser")
        'mEventLogSession = Session("EventLogSession")
    End Sub

    Private Sub ApplyRights()
        Try

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
        Catch ex As Exception

        End Try
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

#Region "Data Bind"
    Private Sub BindGrid()
        Try

            mAPPEmployeeTrainningDueList = EmployeeTrainningDueList.GetEmployeeTrainningDueList(New Guid(cmbEmployeeList.SelectedValue.ToString), _
                                                                                           Guid.Empty, Guid.Empty, Today.Date.ToString, 0)

            Dim DocumentDueList = (From res In mAPPEmployeeTrainningDueList).ToList.Take(10)

            grdEmployeeTrainingDueList.DataSource = DocumentDueList
            grdEmployeeTrainingDueList.DataBind()

            'Session("APPRosterList.App_CrewRosterList") = mApp_CrewRosterList
            If grdEmployeeTrainingDueList.Rows.Count = 10 Then
                lblTotalrecordCount.Text = "Top 10 Records displayed, "
            Else
                lblTotalrecordCount.Text = "Total records found: " + grdEmployeeTrainingDueList.Rows.Count.ToString + ", "
            End If
            lblRange.Text = "As On Date " + Today.Date.ToString(AppSettings("DateFormat").ToString) + " Date Range : Between 0 Days - 1 Month"
            ''Span2.InnerText = "Maximum 10 records Displayed"
            upnlRosterList.Update()
        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try

    End Sub
    Public Sub SetComboBox()

    End Sub
    Private Sub Loadcombos()
        mAPPEmployeeList = EmployeeList.GetEmployeeList(, , "(ALL)")
        cmbEmployeeList.DataSource = mAPPEmployeeList
        cmbEmployeeList.DataBind()
    End Sub

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)
            If mGBUser Is Nothing Then
                ShowAlertMsg("Session expired ! Please click on Home button on left side menu.", "Session expired..")
                DisableButtons()
                Exit Sub
            End If

            If Not IsPostBack Then
                Loadcombos()
            End If
            lblRange.Text = "As On Date " + Today.Date.ToString(AppSettings("DateFormat").ToString) + " Date Range : Between 0 Days - 1 Month"
            ApplyRights()
        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try
    End Sub
    Protected Sub lnkSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSearch.Click
        Try
            BindGrid()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lnkHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHome.Click
        Try
            Response.Redirect("APPMenu.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try
    End Sub
#End Region

End Class