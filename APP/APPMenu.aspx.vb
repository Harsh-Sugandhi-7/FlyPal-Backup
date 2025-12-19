'https://materialdesignicons.com/

Imports CSLA

Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Public Class APPMenu
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    'Dim mRegInformation As RegInformation
    'Dim mEventLogSession As EventLogSetSession
    'Dim mEmailConfigurationList As EmailConfigurationList
    Dim mAPP_UserNotificationCount As APP_UserNotificationCount
    Dim EventLogID As Guid
#End Region

#Region "Helper Method"

    Private Sub ApplyRights()
        Try

            'If (mUser.IsInRole("CrewRosterNew") Or mUser.IsInRole("CrewRosterEdit") Or mUser.IsInRole("CrewRosterDelete") Or mUser.IsInRole("CrewRosterView") Or mUser.IsInRole("CrewRosterPrint")) = False Then
            If User.IsInRole("StoreBalanceView") = False Then '
                PartAvailabilityNode.Attributes("style") = "display: none"
                iPartAvailability.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"


            End If

            If User.IsInRole("CalibrationDueReportView") = False Then
                CalibrationStatusNode.Attributes("style") = "display: none"
                'CalibrationStatusNode.Attributes("style") = "pointer-events: none"
                iCalibrationStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"


            End If

            If User.IsInRole("ExpiryDateView") = False Then
                ExpiryStatusNode.Attributes("style") = "display: none"
                iExpiryStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"

            End If

            If User.IsInRole("EmployeeDocumentDueList") = False Then
                EmployeeDocumentStatusNode.Attributes("style") = "display: none"
                iEmployeeDocumentStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"

            End If

            If User.IsInRole("EmployeeTrainnigDueList") = False Then
                EmployeeTrainingStatusNode.Attributes("style") = "display: none"
                iEmployeeTrainingStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"

            End If

            If User.IsInRole("NewDueReportView") = True Or User.IsInRole("Due-PeriodWiseView") = True Then
                'nothing
            Else
                AircraftDuetStatusNode.Attributes("style") = "display: none"
                iAircraftDuetStatusNode.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
            End If

            If User.IsInRole("ShowWODashBoardView") = False Then '
                APPWorkOrderStatusNode.Attributes("style") = "pointer-events: none;display:none"
                hrefTimeline.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"

            End If

            ''BottomMenu Rights
            ''
            If User.IsInRole("AircraftCurrentStatusView") = False Then '
                hrefTimeline.Attributes("style") = "pointer-events: none"
                iTimeline.Attributes.Remove("style")

                AircraftCurrentStatusNode.Attributes("style") = "display: none"
                iAircraftCurrentStatus.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"
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
            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")


        Catch ex As Exception

        End Try


    End Sub

    Private Sub DisableButtons()
        'Try


        '    CrewRosterNode.Attributes("style") = "pointer-events: none"
        '    iRosters.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"

        '    AllocationNode.Attributes("style") = "pointer-events: none"
        '    iAllocations.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"

        '    CTDNode.Attributes("style") = "pointer-events: none"
        '    iRenewals.Attributes("class") = "mdi mdi-file-document z-depth-1 grey"

        '    hrefTimeline.Attributes("style") = "pointer-events: none"
        '    iTimeline.Attributes.Remove("style")

        '    hrefFlights.Attributes("style") = "pointer-events: none"
        '    iFlights.Attributes.Remove("style")

        '    hrefAvailability.Attributes("style") = "pointer-events: none"
        '    iAvailability.Attributes.Remove("style")

        '    hrefProfile.Attributes("style") = "pointer-events: none"
        '    iProfile.Attributes.Remove("style")

        'Catch ex As Exception

        'End Try


    End Sub

    Private Sub ShowAlertMsg(ByVal Msg As String, ByVal MsgTitle As String)


        Dim str As String
        str = "opennotificationpopup('" & Msg & "','" & MsgTitle & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)

    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Try
            mUser = Session("User")
            mGBUser = Session("GBUser")
            '    mRegInformation = Session("RegInformation")
            '    mEventLogSession = Session("EventLogSession")
            '    mEmailConfigurationList = Session("EmailConfigurationList")

            If mGBUser Is Nothing Then
                ShowAlertMsg("Session expired ! Please click on Home button on left side menu.", "Session expired..")
                DisableButtons()
                Exit Sub
            End If

            If Not IsPostBack Then
                ' 
            End If

            mAPP_UserNotificationCount = APP_UserNotificationCount.GetAPP_UserNotificationCount(mGBUser.UserID)

            If mAPP_UserNotificationCount.NotificationCount = 0 Then
                divNotificationCount.InnerText = ""
                divNotificationCount.Style.Add("background-color", "whitesmoke")
            Else
                divNotificationCount.InnerText = mAPP_UserNotificationCount.NotificationCount.ToString
                divNotificationCount.Style.Add("background-color", "red")
            End If


            ApplyRights()


        Catch ex As Exception

            DisableButtons()

            ShowAlertMsg(ex.Message, "Error")

        End Try


    End Sub

    Protected Sub lnkNotification_Click(sender As Object, e As System.EventArgs) Handles lnkNotification.Click
        Try
            Response.Redirect("APPNotificationList.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub




#End Region

End Class
