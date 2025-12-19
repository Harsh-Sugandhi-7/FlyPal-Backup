''Çreated By: Saylee

Public Class APPNotificationList
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    Dim mAPP_UserNotificationList As APP_UserNotificationList
    Dim EventLogID As Guid
    ''Dim mEventLogSession As EventLogSetSession
    ''Dim mEmailConfigurationList As EmailConfigurationList

#End Region

#Region "Helper Method"

    Private Sub GetSession()

        mUser = Session("User")
        mGBUser = Session("GBUser")
        EventLogID = Session("EventLogID")
        mAPP_UserNotificationList = Session("APPNotificationList.APP_UserNotificationList")

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

#Region "Events"


    Public Sub LogPath(ByVal Path As String, ByVal message As String)
        Try

            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, message)
            FileClose(1)

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim Day, Month, Year As String

            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = "C:\TEMP\" & todaydate

            Try
                EventLogID = CType(Session("EventLogID"), Guid)
                LogPath(Path, Date.Now.ToString + "-------------------------NOTIFICATION LIST-----------------------------------" + vbLf)

                GetSession()

                If mGBUser Is Nothing Then
                    ShowAlertMsg("Session expired ! Please click on Home button on left side menu.", "Session expired..")
                    DisableButtons()
                    Exit Sub
                End If


                LogPath(Path, Date.Now.ToString + " mGBUser Is Nothing: " + IIf(mGBUser Is Nothing, "True", "False") + vbLf)

                If Not IsPostBack Then

                    LogPath(Path, Date.Now.ToString + " Inside Not IsPostBack Block" + vbLf)

                    mAPP_UserNotificationList = APP_UserNotificationList.GetAPP_UserNotificationList(mGBUser.UserID)

                    LogPath(Path, Date.Now.ToString + " mAPP_UserNotificationList " + mAPP_UserNotificationList.Count.ToString + vbLf)

                    grdNotificationList.DataSource = mAPP_UserNotificationList
                    grdNotificationList.DataBind()

                    Session("APPNotificationList.APP_UserNotificationList") = mAPP_UserNotificationList

                End If

                ApplyRights()

            Catch ex As Exception

                ShowAlertMsg(ex.Message, "Error")
                LogPath(Path, Date.Now.ToString + "Error: ex.Message - " + ex.Message + vbLf)

            End Try

        Catch ex As Exception

            ShowAlertMsg(ex.Message, "Error")

        End Try


    End Sub

    Protected Sub grdNotificationList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdNotificationList.RowCommand
        Try

            Dim i As Integer = CType(e.CommandArgument, Integer)

            If e.CommandName = "ViewDetails" Then

                If mAPP_UserNotificationList(i).ModuleType = 1 Then 'Log
                    Response.Redirect("APPNotificationFlightDet.aspx?NotificationID=" + mAPP_UserNotificationList(i).ID.ToString)
                ElseIf mAPP_UserNotificationList(i).ModuleType = 2 Then 'Order
                    Response.Redirect("APPNotificationOrderDet.aspx?NotificationID=" + mAPP_UserNotificationList(i).ID.ToString + "&ModuleID=" + mAPP_UserNotificationList(i).ModuleID.ToString + "&Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
                ElseIf mAPP_UserNotificationList(i).ModuleType = 3 Then 'Requisition
                    Response.Redirect("APPNotificationRequisitionDet.aspx?NotificationID=" + mAPP_UserNotificationList(i).ID.ToString + "&ModuleID=" + mAPP_UserNotificationList(i).ModuleID.ToString + "&Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
                ElseIf mAPP_UserNotificationList(i).ModuleType = 6 Then 'Requisition-Order-Receipt
                    Response.Redirect("APPNotificationReceiptDet.aspx?NotificationID=" + mAPP_UserNotificationList(i).ID.ToString + "&ModuleID=" + mAPP_UserNotificationList(i).ModuleID.ToString + "&Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
                ElseIf mAPP_UserNotificationList(i).ModuleType = 7 Then 'Certificate Renew 'Added By Vikrant On 01-Nov-2021
                    Response.Redirect("APPNotificationCertificateRenewDet.aspx?NotificationID=" + mAPP_UserNotificationList(i).ID.ToString + "&ModuleID=" + mAPP_UserNotificationList(i).ModuleID.ToString + "&Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
                End If

                Session.Remove("mAPP_UserNotificationList")

            ElseIf e.CommandName = "Delete" Then

                Try
                    Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.GetAPP_UserNotification(mAPP_UserNotificationList(i).ID)
                    mAPP_UserNotification.IsRead = True
                    mAPP_UserNotification.ReadOn = Now

                    mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

                    mAPP_UserNotificationList = APP_UserNotificationList.GetAPP_UserNotificationList(mGBUser.UserID)
                    grdNotificationList.DataSource = mAPP_UserNotificationList
                    grdNotificationList.DataBind()

                    Session("APPNotificationList.APP_UserNotificationList") = mAPP_UserNotificationList

                Catch ex As Exception
                    'event log
                End Try

            End If
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub grdNotificationList_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdNotificationList.RowDeleting
        '
    End Sub

    Protected Sub lnkHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHome.Click
        Try
            Response.Redirect("APPMenu.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub

#End Region
End Class