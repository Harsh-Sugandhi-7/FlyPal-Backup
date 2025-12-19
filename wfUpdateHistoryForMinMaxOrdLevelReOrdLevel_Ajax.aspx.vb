Public Class wfUpdateHistoryForMinMaxOrdLevelReOrdLevel_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mUpdateHistoryForMinMaxReOrdLevelList As UpdateHistoryForMinMaxReOrdLevelList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateHistoryForMinMaxReOrdLevelList = CType(Session("mUpdateHistoryForMinMaxReOrdLevelList"), UpdateHistoryForMinMaxReOrdLevelList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateHistoryForMinMaxReOrdLevelList")
    End Sub
    
    Private Sub ControlVisibility()
        btnBackTop.Visible = (mUpdateHistoryForMinMaxReOrdLevelList.Count > 25)
    End Sub
#End Region

#Region " DataBind "
    Private Sub DataFieldBind()
        mUpdateHistoryForMinMaxReOrdLevelList = UpdateHistoryForMinMaxReOrdLevelList.GetList()
        Session("mUpdateHistoryForMinMaxReOrdLevelList") = mUpdateHistoryForMinMaxReOrdLevelList
        dgList.DataSource = mUpdateHistoryForMinMaxReOrdLevelList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    
    Private Sub btnBackTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub dgList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgList.PageIndexChanging
        dgList.PageIndex = e.NewPageIndex
        dgList.DataSource = mUpdateHistoryForMinMaxReOrdLevelList
        dgList.DataBind()
        Session("mUpdateHistoryForMinMaxReOrdLevelList") = mUpdateHistoryForMinMaxReOrdLevelList
    End Sub
#End Region

End Class