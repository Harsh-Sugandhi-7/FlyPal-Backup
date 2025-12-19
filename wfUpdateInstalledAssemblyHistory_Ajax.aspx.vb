'AJAX Conversion By Vikrant On 18-Mar-2015

Public Class wfUpdateInstalledAssemblyHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mUpdateHistoryAssemblyStausList As UpdateHistoryAssemblyStatusList
    Private RemoveDate As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateHistoryAssemblyStausList = CType(Session("mUpdateHistoryAssemblyStausList"), UpdateHistoryAssemblyStatusList)
        RemoveDate = CType(Session("RemoveDate"), String)
    End Sub
    Private Sub SetSession()
        Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
        Session("RemoveDate") = RemoveDate
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateHistoryAssemblyStausList")

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateRemovedAssemblyHistory.aspx?" Then
            Session.Remove("mUpdateHistoryAssemblyStausList")
            Session.Remove("RemoveDate")
        End If
    End Sub
    Private Sub SetCaption()
        lblInstalledAssemblyList.Text = "History of Installed Assembly as of " & txtDate.Text & "  : " & mUpdateHistoryAssemblyStausList.Count & " Record(s) found."
    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgInstalledList.Rows.Count - 1
            B = CType(Me.dgInstalledList.Rows(j).Cells(8).Text, Boolean)
            If B = False Then
                dgInstalledList.Rows(j).Cells(7).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        If IsNothing(Session("RemoveDate")) Then
            txtDate.Text = CType(Session("RemoveDate"), String)
            RemoveDate = CType(Session("RemoveDate"), String)
        Else
            txtDate.Text = RemoveDate
        End If
        Session("RemoveDate") = txtDate.Text


        dgInstalledList.DataSource = mUpdateHistoryAssemblyStausList
        Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
        dgInstalledList.DataBind()

        txtModel.Text = Session("ModelName")
        txtSerialNo.Text = mUpdateHistoryAssemblyStausList(0).SerialNo

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM:put here the code to initialize the page
        GetSession()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            txtDate.Focus()
            DataFieldBind()
            SetCaption()
            SetGrid()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If IsValid Then
            Session("RemoveDate") = txtDate.Text
            SetCaption()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mUpdateHistoryAssemblyStausList")
        'Response.Redirect(Request.QueryString("BackPage")) '' & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgInstalledList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstalledList.RowCommand
        Dim Index As Int16
        Select Case e.CommandName
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgInstalledList.PageSize * dgInstalledList.PageIndex
                Dim No As New Random
                'Added By Saylee On 1-Dec-2014
                Dim mFileAttach As FileAttach
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mUpdateHistoryAssemblyStausList(Index).ID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgInstalledList_Sorting(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInstalledList.Sorting
        mUpdateHistoryAssemblyStausList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
        dgInstalledList.DataSource = mUpdateHistoryAssemblyStausList
        dgInstalledList.DataBind()
        SetGrid()
    End Sub
#End Region

End Class