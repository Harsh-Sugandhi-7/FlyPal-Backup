Public Class wfUserMappingwithStore_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mStoreList As StoreList
    Public mByStoreUserList As ByStoreUserList
    Public mStoreIDUserMappingwithStore As Guid

    Dim EventLogID As Guid

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mByStoreUserList = Session("mByStoreUserList")
        mStoreIDUserMappingwithStore = Session("StoreIDUserMappingwithStore")

    End Sub

    Private Sub RemoveSession()

        Session.Remove("StoreIDUserMappingwithStore")

    End Sub

    Private Sub DataFieldBind()

        mByStoreUserList = ByStoreUserList.GetUserStores(mStoreIDUserMappingwithStore, 1, User.Identity.Name)
        dgUserList.DataSource = mByStoreUserList
        dgUserList.DataBind()
        Session("mByStoreUserList") = mByStoreUserList

    End Sub

    Private Overloads Sub SetFocus(cntrl As WebControl)

        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()

    End Sub

    Private Sub SetGridObject()

        For i As Integer = 0 To dgUserList.Rows.Count - 1
            Dim chkselect As CheckBox
            chkselect = CType(dgUserList.Rows(i).FindControl("chkSelect"), CheckBox)
            mByStoreUserList.Item(i).IsSelected = chkselect.Checked
        Next
        Session("mByStoreUserList") = mByStoreUserList

    End Sub

#End Region

#Region "Events"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then

            DataFieldBind()

        End If

    End Sub
    Private Sub Save_Click(sender As Object, e As EventArgs) Handles btnSaveTop.Click,
                                                                     btnSaveBottom.Click

        Dim mopenas As String = Request.QueryString("Type")

        For i As Integer = 0 To dgUserList.Rows.Count - 1

            Dim chkselect As CheckBox

            chkselect = CType(dgUserList.Rows(i).FindControl("chkSelect"), CheckBox)
            mByStoreUserList.Item(i).IsSelected = chkselect.Checked

            If mByStoreUserList.Item(i).IsSelected = False Then

                ByStoreUserList.AddUM_tabUserStore(mStoreIDUserMappingwithStore,
                                                   mByStoreUserList.Item(i).UserID)

            Else

                ByStoreUserList.DeleteUM_tabUserStore(mStoreIDUserMappingwithStore,
                                                      mByStoreUserList.Item(i).UserID)

            End If

        Next

        RemoveSession()


        If mopenas IsNot Nothing AndAlso mopenas = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "onclose",
                                                "CallParentCallback();",
                                                True)

            Exit Sub

        End If

    End Sub

    Private Sub Close_Click(sender As Object, e As EventArgs) Handles btnCloseBottom.Click,
                                                                      btnCloseTop.Click

        Dim mopenas As String = Request.QueryString("Type")

        RemoveSession()

        If mopenas IsNot Nothing AndAlso mopenas = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "onclose",
                                                "CallParentCallback();",
                                                True)

            Exit Sub

        End If

    End Sub

    Private Sub Gridview_Pagination(sender As Object, e As GridViewPageEventArgs) Handles dgUserList.PageIndexChanging

        dgUserList.PageIndex = e.NewPageIndex
        DataFieldBind()

    End Sub

#End Region


End Class