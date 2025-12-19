Imports System.Linq
Imports System.Text

'Imports System.Collections.Generic
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf

Imports System.Linq.Enumerable
Imports System
Imports System.IO
Public Class wfEvidence
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    'Dim mEventLogSession As EventLogSetSession
    Dim EventLogID As Guid

    Dim NotificationID As Guid
    Dim LogID As Guid

    Public mEvidenceDetails As EvidenceDetails
    Public mEvidenceDetailsTransaction As EvidenceDetails
    'Public mAPP_UserNotification As APP_UserNotification
#End Region

#Region "Helper Method"

    Private Sub GetSession()

        mUser = Session("User")
        mGBUser = Session("GBUser")
        EventLogID = Session("EventLogID")
        LogID = Session("APPNotificationFlightDet.LogID")
        mEvidenceDetails = Session("mEvidenceDetails")

    End Sub

    Protected Sub MarkReadNotification()

        'mAPP_UserNotification = APP_UserNotification.GetAPP_UserNotification(NotificationID)
        'mAPP_UserNotification.IsRead = True
        'mAPP_UserNotification.ReadOn = Now

        'mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

        'Session.Remove("APPNotificationFlightDet.NotificationID")


        'Response.Redirect("APPNotificationList.aspx")

    End Sub
    Public Sub DatafieldBind()
        'mAPP_UserNotification = APP_UserNotification.GetAPP_UserNotification(NotificationID)
        mEvidenceDetails = EvidenceDetails.GetEvidenceDetailsList()
        Dim TempEvidenceDetails = (From c In mEvidenceDetails
                        Group By KeyfieldID = c.KeyfieldID, LogTextNo = c.LogTextNo, UserName = c.UserName, c.DateTimeStampFormatted Into Group
                         Select New With {.KeyfieldID = KeyfieldID, .LogTextNo = LogTextNo, .UserName = UserName, .DateTimeStampFormatted = DateTimeStampFormatted, .ReceiptItemCollection = Group}).ToList
        dgEvidenceDetailsList.DataSource = TempEvidenceDetails
        dgEvidenceDetailsList.DataBind()
        Session("mEvidenceDetails") = mEvidenceDetails
    End Sub
    Private Sub ApplyRights()
        Try

            'BottomMenu Rights
            '
            'TimeLine
            'If (mUser.IsInRole("CrewRosterNew") Or mUser.IsInRole("CrewRosterEdit") Or mUser.IsInRole("CrewRosterDelete") Or mUser.IsInRole("CrewRosterView") Or mUser.IsInRole("CrewRosterPrint")) = False Then
            '    hrefTimeline.Attributes("style") = "pointer-events: none"
            '    iTimeline.Attributes.Remove("style")
            'End If

            ''Flights
            'If (mUser.IsInRole("FlightScheduleNew") Or mUser.IsInRole("FlightScheduleEdit") Or mUser.IsInRole("FlightScheduleDelete") Or mUser.IsInRole("FlightScheduleView") Or mUser.IsInRole("FlightSchedulePrint")) = False Then
            '    hrefFlights.Attributes("style") = "pointer-events: none"
            '    iFlights.Attributes.Remove("style")
            'End If

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

            '-----


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

            'hrefTimeline.Attributes("style") = "pointer-events: none"
            'iTimeline.Attributes.Remove("style")

            'hrefFlights.Attributes("style") = "pointer-events: none"
            'iFlights.Attributes.Remove("style")

            'hrefAvailability.Attributes("style") = "pointer-events: none"
            'iAvailability.Attributes.Remove("style")

            'hrefProfile.Attributes("style") = "pointer-events: none"
            'iProfile.Attributes.Remove("style")


        Catch ex As Exception

        End Try



    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)
            If Not IsPostBack Then
                DatafieldBind()

            End If

        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try


    End Sub

    Private Sub dgEvidenceDetailsList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEvidenceDetailsList.RowCommand

        Dim mID As New Guid
        mID = New Guid(e.CommandArgument.ToString)


        Select Case e.CommandName
            Case "EditRec"
                Session("mLogID") = mID
                  mEvidenceDetails = Session("mEvidenceDetails")

                Dim row As GridViewRow = CType((CType(e.CommandSource, Control)).Parent.Parent, GridViewRow)
             

                Session("LogTextNo") = dgEvidenceDetailsList.DataKeys(row.RowIndex)(1).ToString()
                Session("LogUserName") = dgEvidenceDetailsList.DataKeys(row.RowIndex)(2).ToString()
                Session("UpdateDateTime") = dgEvidenceDetailsList.DataKeys(row.RowIndex)(3).ToString()
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEvidenceWindow()", "OpenEvidenceWindow();", True)
        End Select
    End Sub

    '''Private Sub dgEvidenceDetailsList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgEvidenceDetailsList.RowDataBound
    '''    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '''        Dim KeyfieldID As Guid = (DataBinder.Eval(e.Row.DataItem, "KeyfieldID"))
    '''        Dim LogTextNo As String = (DataBinder.Eval(e.Row.DataItem, "LogTextNo"))
    '''        Dim dgTransactionDetails As GridView = DirectCast(e.Row.FindControl("dgTransactionDetails"), GridView)
    '''        'AddHandler dgTransactionDetails.RowCommand, AddressOf dgTransactionDetails_RowCommand
    '''        ''mEvidenceDetails = EvidenceDetails.GetEvidenceDetailsList(KeyfieldID.ToString)
    '''        mEvidenceDetailsTransaction = EvidenceDetails.GetEvidenceDetailsList(KeyfieldID.ToString)
    '''        ''Dim mRequisitionItemTransactionDetails As RequisitionItemTransactionDetails = RequisitionItemTransactionDetails.GetRequisitionItemTransactionDetails(ReqItemID.ToString, chkShowPPReqOnly.Checked, AppSettings("ClientCode").ToString)
    '''        dgTransactionDetails.DataSource = mEvidenceDetailsTransaction
    '''        dgTransactionDetails.DataBind()
    '''        Session("mEvidenceDetailsTransaction") = mEvidenceDetailsTransaction
    '''        If mEvidenceDetailsTransaction.Count > 0 Then
    '''            e.Row.Cells(0).BackColor = Color.Yellow ''Color.FromArgb(0, 157, 217)
    '''        End If
    '''    End If
    '''End Sub
    'Protected Sub lnkNotificationList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkNotificationList.Click

    '    Try
    '        Response.Redirect("APPNotificationList.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
    '    Catch ex As Exception

    '    End Try

    'End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
End Class