Public Class wfFlypalVideo
    Inherits System.Web.UI.Page

#Region "Methods"
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = System.Configuration.ConfigurationManager.AppSettings("DB:MasterDataBase")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "FlypalVideoDisplayListFetch"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim)

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        Return dataTable
    End Function
    Private Sub GenerateList(ByVal tbl As DataTable)
        'If (tbl.Rows.Count = 0) Then
        '    'MSGBoxCtrl.show(MsgBox.Message_title.NoRecordFound, MsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        dgGridView.DataSource = tbl
        dgGridView.DataBind()

        upnlGrid.Update()
    End Sub
#End Region


#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenerateList(CreateDataTable())
         End If
    End Sub
    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        GenerateList(CreateDataTable())
    End Sub
    Private Sub dgGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgGridView.RowCommand
        Select Case e.CommandName
            Case "VideoView"
                Dim Index As Integer = CInt(e.CommandArgument)
                Dim mVideoPath As String = dgGridView.DataKeys(CInt(e.CommandArgument)).Values("VideoPath").ToString
                Vediosource.Attributes.Add("src", mVideoPath)
                upnlVideo.Update()
        End Select
    End Sub
#End Region

End Class