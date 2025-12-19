Public Class wfSparesInspectionKitList_Ajax
    Inherits System.Web.UI.Page
#Region " Variable Declaration "
    Dim mKit As Kit
    Dim mKitList As KitList
    Dim Index, Text As String
    Dim EventLogID As Guid
#End Region
#Region " Business Methods "
    Private Sub GetSession()
        mKitList = Session("mKitList")
        mKit = Session("mKit")
    End Sub

#End Region


#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            Try
                mKitList = KitList.GetKitList(0, "", "")
                Session("mKitList") = mKitList
                dgKitList.DataSource = mKitList
                dgKitList.DataBind()
            Catch ex As Exception
                Throw ex
            End Try

        End If

    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "spares Inspection Kit List From Wo Job Spares", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Session("mKit") = Nothing
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

    End Sub
    Private Sub dgKitList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgKitList.PageIndexChanging
        dgKitList.PageIndex = e.NewPageIndex
        dgKitList.DataSource = mKitList
        Session("mKitList") = mKitList
        dgKitList.DataBind()
    End Sub
    Private Sub dgKitList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgKitList.RowCommand
        Dim Idx As Int32
        Dim mId As Guid

        Dim mKitName As String
        Select Case e.CommandName
            Case "SelectRec"
                Idx = CInt(e.CommandArgument) + dgKitList.PageIndex * dgKitList.PageSize
                mId = mKitList(Idx).ID
                mKitName = mKitList(Index).KitName
                mKit = Kit.Getkit(mId)
                Session("mKit") = mKit
                Session("mKitItems") = mKit.KitItems
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
        End Select
    End Sub
#End Region




End Class