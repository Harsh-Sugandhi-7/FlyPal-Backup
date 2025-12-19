Public Class wfHSNACSHistoryList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mHSNACSHistoryList As HSNACSHistoryList
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mHSNACSHistoryList = Session("mHSNACSHistoryList")
    End Sub

#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not Page.IsPostBack = True Then
            dgHSNACSHistoryList.DataSource = mHSNACSHistoryList
            dgHSNACSHistoryList.DataBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
#End Region

End Class