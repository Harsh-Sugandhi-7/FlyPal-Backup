

'Imports System.Collections.Generic
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf

Imports System.Linq.Enumerable
Imports System
Imports System.IO
Public Class wfEvidenceDetail
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    'Dim mEventLogSession As EventLogSetSession
    Dim EventLogID As Guid


    Dim mLogID As Guid

    Public mEvidenceDetailLogs As EvidenceDetailLogs
    Dim mLogUserName As String

    Dim mUpdateDateTime As String
#End Region

#Region "Helper Method"

    Private Sub GetSession()

        mUser = Session("User")
        EventLogID = Session("EventLogID")
        mEvidenceDetailLogs = Session("mEvidenceDetailLogs")
        mLogID = Session("mLogID")
        mLogUserName = Session("LogUserName")
        mUpdateDateTime = Session("UpdateDateTime")
    End Sub

    Protected Sub MarkReadNotification()


    End Sub
    Public Sub DatafieldBind()

        lblLogDet.InnerText = Session("LogTextNo")

        mEvidenceDetailLogs = EvidenceDetailLogs.GetEvidenceLogsList(mLogID, mLogUserName, mUpdateDateTime)
        Session("mEvidenceDetailLogs") = mEvidenceDetailLogs
        'Maint Activities
        Dim TempEvidenceDetails = (From c In mEvidenceDetailLogs
                                  Where c.ActivityType = 1 Or c.ActivityType = 2 Or c.ActivityType = 3 Or c.ActivityType = 4 Or c.ActivityType = 5 Or c.ActivityType = 6
                                 Select c).ToList


        dgEvidenceMaintActivitiesDetailsLogList.DataSource = TempEvidenceDetails
        dgEvidenceMaintActivitiesDetailsLogList.DataBind()
        lblResultActivities.InnerText = " [ Total " & TempEvidenceDetails.Count.ToString & " Record(s) ]"
        upnlActivities.Update()

        'Logs
        Dim TempLogDetails = (From c In mEvidenceDetailLogs
                                Where c.ActivityType = 7
                               Select c).ToList
        grdLogs.DataSource = TempLogDetails
        grdLogs.DataBind()
        lblResultLogs.InnerText = " [ Total " & TempLogDetails.Count.ToString & " Record(s) ]"
        upnlLog.Update()
        Session("TempLogDetails") = TempLogDetails

        'AssemblyRemoval(s)
        Dim TempAssemblyRemoval = (From c In mEvidenceDetailLogs
                                Where c.ActivityType = 8
                               Select c).ToList
        grdAssemblyRemoval.DataSource = TempAssemblyRemoval
        grdAssemblyRemoval.DataBind()
        lblAssemblyRemovals.InnerText = " [ Total " & TempAssemblyRemoval.Count.ToString & " Record(s) ]"
        upnlAssemblyRemoval.Update()



        'AssemblyInstallation(s)
        Dim TempAssemblyInstallation = (From c In mEvidenceDetailLogs
                                Where c.ActivityType = 9
                               Select c).ToList
        grdAssemblyInstallation.DataSource = TempAssemblyInstallation
        grdAssemblyInstallation.DataBind()
        lblAssemblyInstallations.InnerText = " [ Total " & TempAssemblyInstallation.Count.ToString & " Record(s) ]"
        upnlAssemblyInstallation.Update()



        'CompRemoval(s)
        Dim TempCompRemoval = (From c In mEvidenceDetailLogs
                                Where c.ActivityType = 10
                               Select c).ToList
        grdCompRemoval.DataSource = TempCompRemoval
        grdCompRemoval.DataBind()
        lblCompRemovals.InnerText = " [ Total " & TempCompRemoval.Count.ToString & " Record(s) ]"
        upnlCompRemoval.Update()


        'CompInstallation(s)
        Dim TempCompInstallation = (From c In mEvidenceDetailLogs
                                Where c.ActivityType = 11
                               Select c).ToList
        grdCompInstallation.DataSource = TempCompInstallation
        grdCompInstallation.DataBind()
        lblCompInstallations.InnerText = " [ Total " & TempCompInstallation.Count.ToString & " Record(s) ]"
        upnlCompInstallation.Update()

    End Sub

   
    Private Sub ShowAlertMsg(ByVal Msg As String, ByVal MsgTitle As String)


        Dim str As String
        str = "opennotificationpopup('" & Msg & "','" & MsgTitle & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)

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
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseBottom.Click
        Session("MiddleFrame") = ""
        'Response.Redirect("Dashboard.aspx")

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

    End Sub
    Private Sub grdLogs_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLogs.PageIndexChanging
        mEvidenceDetailLogs = Session("mEvidenceDetailLogs")
        Dim TempLogDetails = (From c In mEvidenceDetailLogs
                              Where c.ActivityType = 7
                             Select c).ToList

        grdLogs.PageIndex = e.NewPageIndex
        grdLogs.DataSource = TempLogDetails
        Session("TempLogDetails") = TempLogDetails
        grdLogs.DataBind()
        upnlLog.Update()
    End Sub

    
    Private Sub dgEvidenceMaintActivitiesDetailsLogList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEvidenceMaintActivitiesDetailsLogList.PageIndexChanging
        mEvidenceDetailLogs = Session("mEvidenceDetailLogs")
        
        Dim TempEvidenceDetails = (From c In mEvidenceDetailLogs
                                  Where c.ActivityType = 1 Or c.ActivityType = 2 Or c.ActivityType = 3 Or c.ActivityType = 4 Or c.ActivityType = 5 Or c.ActivityType = 6
                                 Select c).ToList
        dgEvidenceMaintActivitiesDetailsLogList.PageIndex = e.NewPageIndex
        dgEvidenceMaintActivitiesDetailsLogList.DataSource = TempEvidenceDetails
        Session("TempEvidenceDetails") = TempEvidenceDetails
        dgEvidenceMaintActivitiesDetailsLogList.DataBind()
        upnlActivities.Update()
    End Sub

    Private Sub grdAssemblyInstallation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdAssemblyInstallation.PageIndexChanging
        mEvidenceDetailLogs = Session("mEvidenceDetailLogs")

        Dim TempAssemblyInstallation = (From c In mEvidenceDetailLogs
                               Where c.ActivityType = 9
                              Select c).ToList
        grdAssemblyInstallation.PageIndex = e.NewPageIndex
        grdAssemblyInstallation.DataSource = TempAssemblyInstallation
        Session("TempAssemblyInstallation") = TempAssemblyInstallation
        grdAssemblyInstallation.DataBind()
        upnlAssemblyInstallation.Update()
    End Sub

    Private Sub grdAssemblyRemoval_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdAssemblyRemoval.PageIndexChanging
        mEvidenceDetailLogs = Session("mEvidenceDetailLogs")

        Dim TempAssemblyRemoval = (From c In mEvidenceDetailLogs
                               Where c.ActivityType = 8
                              Select c).ToList
        grdAssemblyRemoval.PageIndex = e.NewPageIndex
        grdAssemblyRemoval.DataSource = TempAssemblyRemoval
        Session("TempAssemblyRemoval") = TempAssemblyRemoval
        grdAssemblyRemoval.DataBind()
        upnlAssemblyRemoval.Update()
    End Sub

    Private Sub grdCompInstallation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdCompInstallation.PageIndexChanging
        mEvidenceDetailLogs = Session("mEvidenceDetailLogs")

        Dim TempCompInstallation = (From c In mEvidenceDetailLogs
                                Where c.ActivityType = 11
                               Select c).ToList
        grdCompInstallation.PageIndex = e.NewPageIndex
        grdCompInstallation.DataSource = TempCompInstallation
        Session("TempCompInstallation") = TempCompInstallation
        grdCompInstallation.DataBind()
        upnlCompInstallation.Update()
    End Sub

    Private Sub grdCompRemoval_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdCompRemoval.PageIndexChanging
        mEvidenceDetailLogs = Session("mEvidenceDetailLogs")

        Dim TempCompRemoval = (From c In mEvidenceDetailLogs
                                Where c.ActivityType = 10
                               Select c).ToList
        grdCompRemoval.PageIndex = e.NewPageIndex
        grdCompRemoval.DataSource = TempCompRemoval
        Session("TempCompRemoval") = TempCompRemoval
        grdCompRemoval.DataBind()
        upnlCompRemoval.Update()
    End Sub
End Class