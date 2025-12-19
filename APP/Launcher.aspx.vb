Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Imports Authenticate
Public Class Launcher
    Inherits System.Web.UI.Page

    Dim mGBUser As SI.UTILITY.User
    'Dim mRegInformation As RegInformation
    'Dim mEventLogSession As EventLogSetSession
    'Dim mEmailConfigurationList As EmailConfigurationList

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim mUsername As String = Request.QueryString("Username")
        mGBUser = SI.UTILITY.User.GetUser(mUsername)

        ' Dim mEventLogSessionID As Guid = New Guid(Request.QueryString("EventLogSessionID"))
        Dim mEventLogSessionID As Guid = MarkLog(Util.Action.Login, mUsername, mGBUser.Password, Me.Request.UserHostAddress, "", Thread.CurrentPrincipal.Identity.IsAuthenticated)

        Dim mModuleTypeID As Integer = Request.QueryString("ModuleTypeID")


        'mRegInformation = RegInformation.GetRegInformation()
        'mEventLogSession = EventLogSetSession.NewEventLogSession(mEventLogSessionID, mUsername, "", Me.Request.UserHostAddress, "")

        ' If mRegInformation.OperatorManagement = True And (mGBUser.UserTypeID = 2 Or mGBUser.UserTypeID = 3) Then  ' 2 - Crew , 3 - Operator

        '    mEmailConfigurationList = EmailConfigurationList.GetEmailConfigurationList(IIf(mGBUser.UserTypeID = 3, mGBUser.UserOperators(0).OperatorID, Guid.Empty))

        'Else
        '    mEmailConfigurationList = EmailConfigurationList.GetEmailConfigurationList(IIf(mGBUser.UserTypeID = 3, mGBUser.UserOperators.GetDefaultOperator.OperatorID, Guid.Empty))
        'End If


        Session("GBUser") = mGBUser
        Session("EventLogID") = mEventLogSessionID
        'Session("RegInformation") = mRegInformation
        'Session("EventLogSession") = mEventLogSession
        'Session("EmailConfigurationList") = mEmailConfigurationList

        Dim bp As BusinessPrincipal = BusinessPrincipal.login(mGBUser.Name, mGBUser.DBPassword, "") ' Session("RequestInfo"))

        Session("CSLA-Principal") = Threading.Thread.CurrentPrincipal
        HttpContext.Current.User = CType(Session("CSLA-Principal"), System.Security.Principal.IPrincipal)

        Session("User") = User

        'If mModuleTypeID = 1 Then 'Roster Notification Detail
        '    Response.Redirect("APPNotificationRoster.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString())
        'ElseIf mModuleTypeID = 2 Then 'Allocation Notification Detail
        '    Response.Redirect("APPNotificationAllocation.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString() + "&ModuleID=" + New Guid(Request.QueryString("ModuleID")).ToString())
        'ElseIf mModuleTypeID = 3 Then 'Renewal Notification Detail
        '    Response.Redirect("APPNotificationRenewal.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString())
        'ElseIf mModuleTypeID = 4 Then 'Home
        '    Response.Redirect("APPMenu.aspx")
        'ElseIf mModuleTypeID = 5 Then 'Notification List
        '    Response.Redirect("APPNotificationList.aspx")
        'End If
        If mModuleTypeID = 1 Then 'Home
            Response.Redirect("APPNotificationFlightDet.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString())
        ElseIf mModuleTypeID = 2 Then 'Order
            Response.Redirect("APPNotificationOrderDet.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString() + "&ModuleID=" + New Guid(Request.QueryString("ModuleID")).ToString())
        ElseIf mModuleTypeID = 3 Then 'Requisition
            Response.Redirect("APPNotificationRequisitionDet.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString() + "&ModuleID=" + New Guid(Request.QueryString("ModuleID")).ToString())
        ElseIf mModuleTypeID = 4 Then 'Home
            Response.Redirect("APPMenu.aspx")
        ElseIf mModuleTypeID = 5 Then 'Notification List
            Response.Redirect("APPNotificationList.aspx")
        ElseIf mModuleTypeID = 6 Then 'Requisition-Order-Receipt
            Response.Redirect("APPNotificationReceiptDet.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString() + "&ModuleID=" + New Guid(Request.QueryString("ModuleID")).ToString())
        ElseIf mModuleTypeID = 7 Then 'Certificate Renew 'Added By Vikrant On 01-Nov-2021
            Response.Redirect("APPNotificationCertificateRenewDet.aspx?NotificationID=" + New Guid(Request.QueryString("NotificationID")).ToString() + "&ModuleID=" + New Guid(Request.QueryString("ModuleID")).ToString())
       End If
    End Sub

End Class
