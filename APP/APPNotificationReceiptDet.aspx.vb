Public Class APPNotificationReceiptDet
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    'Dim mEventLogSession As EventLogSetSession
    Dim EventLogID As Guid
    Dim NotificationID As Guid
    Public mApp_ReceiptCumInvoice As ReceiptCumInvoice
    Dim ReceiptID As Guid
#End Region

#Region "Helper Method"

    Private Sub GetSession()

        mUser = Session("User")
        mGBUser = Session("GBUser")
        EventLogID = Session("EventLogID")
        mApp_ReceiptCumInvoice = Session("mApp_ReceiptCumInvoice")

    End Sub

    Protected Sub MarkReadNotification()

        Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.GetAPP_UserNotification(NotificationID)
        mAPP_UserNotification.IsRead = True
        mAPP_UserNotification.ReadOn = Now

        mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

        Session.Remove("APPNotificationFlightDet.NotificationID")

        'Response.Redirect("APPNotificationList.aspx")

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
                ReceiptID = New Guid(Request.QueryString("ModuleID"))

                'Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.GetAPP_UserNotification(NotificationID)
                Dim mApp_Receipt As Receipt = Receipt.GetReceipt(ReceiptID)
                Dim mApp_ReceiptCumInvoice As ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID, mApp_Receipt.InvoiceID)

                Session("mApp_ReceiptCumInvoice") = mApp_ReceiptCumInvoice

                Session("APPNotificationFlightDet.NotificationID") = NotificationID

                lblTotalrecordCount.Text = "Requested Part(s) received in " + mApp_ReceiptCumInvoice.ReceiptNo + " Dated " + mApp_ReceiptCumInvoice.RecCumInvDateFormatted + " By User " + Thread.CurrentPrincipal.Identity.Name '"Parts(s) has been requested by " + mApp_ReceiptCumInvoice.UserName + " in Requisition " + mApp_ReceiptCumInvoice.ReceiptNo + " ,Created on " + mApp_ReceiptCumInvoice.RecCumInvDateFormatted

                grdReceiptList.DataSource = mApp_ReceiptCumInvoice.ReceiptCumInvoiceItems
                grdReceiptList.DataBind()


                'If mApp_FlightScheduleCrewAllocationList.Count = 0 Then
                '    lblTotalrecordCount.Text = "This Allocation has been cancelled / deleted."
                'Else
                '    lblTotalrecordCount.Text = "Total records found : " + mApp_FlightScheduleCrewAllocationList.Count.ToString
                'End If

                MarkReadNotification()
                'DataBind()
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