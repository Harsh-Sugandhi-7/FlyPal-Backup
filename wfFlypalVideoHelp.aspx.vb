Imports System.Linq
Public Class wfFlypalVideoHelp
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim Temptbl As DataTable
    Dim mTempFlypalVideoHelpListDataTable As Object
#End Region
   
#Region "Methods"
    Private Sub GetSession()
        Temptbl = Session("tbl")
        mTempFlypalVideoHelpListDataTable = Session("TempFlypalVideoHelpList")
    End Sub
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

        dgGridView.DataSource = tbl
        dgGridView.DataBind()
        Session("tbl") = tbl

        upnlGrid.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            GenerateList(CreateDataTable())
            'Dim mVideoPath As String = "Vedio/PO-Final.mp4" 'CType(e.CommandSource, System.Web.UI.WebControls.GridView).DataKeys(CInt(e.CommandArgument)).Values("VideoPath").ToString
            'Vediosource.Attributes.Add("src", mVideoPath)
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
                Dim mSrNo As Integer = CInt(dgGridView.DataKeys(CInt(e.CommandArgument)).Values("SrNo"))
                'Vediosource.Attributes.Add("src", mVideoPath)
                Vediosource.Visible = False
                Dim TempFlypalVideoHelpList = (Temptbl.AsEnumerable().Select(Function(c) New With {.VideoPath = c.Field(Of String)("VideoPath"), .SrNo = c.Field(Of Integer)("SrNo"), .id = c.Field(Of Guid)("id")}).Where(Function(c) c.SrNo >= mSrNo)).ToList


                dgGridView1.DataSource = TempFlypalVideoHelpList
                dgGridView1.PageIndex = 0
                dgGridView1.DataBind()
                Session("TempFlypalVideoHelpList") = TempFlypalVideoHelpList
                upnlVideo.Update()
        End Select
    End Sub
    Private Sub dgGridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgGridView1.PageIndexChanging
        dgGridView1.PageIndex = e.NewPageIndex
        dgGridView1.DataSource = mTempFlypalVideoHelpListDataTable
        dgGridView1.DataBind()
        Session("TempFlypalVideoHelpList") = mTempFlypalVideoHelpListDataTable
    End Sub
#End Region
End Class