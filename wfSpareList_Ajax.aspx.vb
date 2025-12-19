Imports System.Configuration.ConfigurationManager
Public Class wfSpareList_Ajax
    Inherits System.Web.UI.Page

#Region "Variables and Declarations"
    Dim mSpareListByMaintenanceActivity As SpareListByMaintenanceActivity
    Public mMaintenanceKit As MaintenanceKit
    Public mMaintenanceTask As MaintenanceTask
    Dim mStatusMasterID As Guid
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mSpareListByMaintenanceActivity = Session("mSpareListByMaintenanceActivity")
        mStatusMasterID = Session("StatusMasterID")
    End Sub
    Private Sub SetSession()
        Session("mSpareListByMaintenanceActivity") = mSpareListByMaintenanceActivity
    End Sub
    Private Sub DataFieldBind()
        mSpareListByMaintenanceActivity = SpareListByMaintenanceActivity.GetList(Today.Date.ToString, mStatusMasterID.ToString)

        If mSpareListByMaintenanceActivity.Count = 0 Then
            PlaceHolderSpare.Visible = False
        Else
            dgPartList.DataSource = mSpareListByMaintenanceActivity
            Session("mSpareListByMaintenanceActivity") = mSpareListByMaintenanceActivity
            dgPartList.DataBind()
        End If

        mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mStatusMasterID, True)

        If mMaintenanceKit.MaintenanceKitDetails.Count = 0 Then
            PlaceHolderTools.Visible = False
        Else
            dgKitList.DataSource = mMaintenanceKit.MaintenanceKitDetails
            dgKitList.DataBind()
        End If

        mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mStatusMasterID)

        If mMaintenanceTask.MaintenanceTaskDetails.Count = 0 Then
            PlaceHolderTaskCard.Visible = False
        Else
            dgTaskList.DataSource = mMaintenanceTask.MaintenanceTaskDetails
            dgTaskList.DataBind()
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mSpareListByMaintenanceActivity")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
#End Region

End Class