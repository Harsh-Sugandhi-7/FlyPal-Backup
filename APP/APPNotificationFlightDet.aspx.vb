Public Class APPNotificationFlightDet
    Inherits System.Web.UI.Page


#Region "Variable Declaration"

    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    'Dim mEventLogSession As EventLogSetSession
    Dim EventLogID As Guid

    Dim NotificationID As Guid
    Dim LogID As Guid

    Public mApp_FlightLog As Log
    Public mAPP_UserNotification As APP_UserNotification
#End Region

#Region "Helper Method"

    Private Sub GetSession()

        mUser = Session("User")
        mGBUser = Session("GBUser")
        EventLogID = Session("EventLogID")
        LogID = Session("APPNotificationFlightDet.LogID")
        mApp_FlightLog = Session("mApp_FlightLog")

    End Sub

    Protected Sub MarkReadNotification()

        mAPP_UserNotification = APP_UserNotification.GetAPP_UserNotification(NotificationID)
        mAPP_UserNotification.IsRead = True
        mAPP_UserNotification.ReadOn = Now

        mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

        Session.Remove("APPNotificationFlightDet.NotificationID")


        'Response.Redirect("APPNotificationList.aspx")

    End Sub
    Public Sub DatafieldBind()
        mAPP_UserNotification = APP_UserNotification.GetAPP_UserNotification(NotificationID)
        mApp_FlightLog = Log.GetLog(mAPP_UserNotification.ModuleID)
        With mApp_FlightLog
            spnRegNo.InnerText = .RegNo
            spnSourceName.InnerText = .SourceCode ''.SourceName
            spnSouDate.InnerText = .SouUniverseDateTimeFormatted
            spnLogTextNo.InnerText = .LogTextNo
            spnLogPageNo.InnerText = .LogPageNo
            spnDestinationName.InnerText = .DestinationCode ''.DestinationName
            spnDesDate.InnerText = .DesUniverseDateTimeFormatted
            spnTimeInAir.InnerText = .TimeInAir
            spnPilot1Name.InnerText = .Pilot1Name

        End With
        DataBind()
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

                NotificationID = New Guid(Request.QueryString("NotificationID"))

                Session("APPNotificationFlightDet.NotificationID") = NotificationID

                DatafieldBind()
                'grdAllocationList.DataSource = mApp_FlightScheduleCrewAllocationList
                'grdAllocationList.DataBind()


                'If mApp_FlightScheduleCrewAllocationList.Count = 0 Then
                '    lblTotalrecordCount.Text = "This Allocation has been cancelled / deleted."
                'Else
                '    lblTotalrecordCount.Text = "Total records found : " + mApp_FlightScheduleCrewAllocationList.Count.ToString
                'End If

                MarkReadNotification()

            End If

            ApplyRights()

        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try


    End Sub


    Protected Sub lnkNotificationList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkNotificationList.Click

        Try
            Response.Redirect("APPNotificationList.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub
End Class