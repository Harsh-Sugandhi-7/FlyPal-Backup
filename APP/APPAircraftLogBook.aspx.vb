'Created By : Saylee 
'Dated: 29-Oct-2021

Public Class APPAircraftLogBook_aspx
    Inherits System.Web.UI.Page



#Region "Variable Declaration"
    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    Dim mAPPAircraftLogList As LogList

    Dim mMachineList As MachineList
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

            mAPPAircraftLogList = LogList.GetLogList(MachineID:=New Guid(cmbAircraft.SelectedValue.ToString), SouLocalDateTime:=txtFromDate.Text, DesLocalDateTime:=txtToDate.Text) ' APPPartAvailability.GetAPPPartAvailability(ItemName.Trim, ItemDescription.Trim, Date.Today.ToString("dd-MMM-yyyy"))

            grdAircraftCurrentStatusList.DataSource = mAPPAircraftLogList
            grdAircraftCurrentStatusList.DataBind()

            'Session("APPRosterList.App_CrewRosterList") = mApp_CrewRosterList

            lblTotalrecordCount.Text = "Total records found: " + mAPPAircraftLogList.Count.ToString

            upnlRosterList.Update()
        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try

    End Sub
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        mMachineList = MachineList.GetMachineListMonitoringStatus(AOnDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(SELECT)", SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        cmbAircraft.DataBind()

    End Sub
    Private Sub Loadcombos()
        SetComboOfMachine(Today.Date.ToString)
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
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
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