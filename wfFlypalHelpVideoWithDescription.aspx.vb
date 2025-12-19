Imports System.Linq
Public Class wfFlypalHelpVideoWithDescription
    Inherits System.Web.UI.Page

    Dim Temptbl As DataTable
    Dim mTempFlypalVideoHelpListDataTable As Object
#Region "Methods"
    Private Sub GetSession()
        Temptbl = Session("tbl")
        mTempFlypalVideoHelpListDataTable = Session("TempFlypalVideoHelpListFoSecondGrid")
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

        Dim TempFlypalVideoHelpList = (tbl.AsEnumerable().Select(Function(c) New With {.VideoPath = c.Field(Of String)("VideoPath"), .SrNo = c.Field(Of Integer)("SrNo"), .id = c.Field(Of Guid)("id"), .ThumbnailPath = c.Field(Of String)("ThumbnailPath"), .VideoName = c.Field(Of String)("VideoName"), .Description = c.Field(Of String)("Description")})).ToList

        If TempFlypalVideoHelpList.Count >= 11 Then

            img1.Src = TempFlypalVideoHelpList(9).ThumbnailPath
            a1.Title = TempFlypalVideoHelpList(9).SrNo.ToString
            lblVideoName1.Text = TempFlypalVideoHelpList(9).VideoName
            lblDescription1.Text = TempFlypalVideoHelpList(9).Description

            img2.Src = TempFlypalVideoHelpList(10).ThumbnailPath
            a2.Title = TempFlypalVideoHelpList(10).SrNo.ToString
            lblVideoName2.Text = TempFlypalVideoHelpList(10).VideoName
            lblDescription2.Text = TempFlypalVideoHelpList(10).Description

            upnlBelowList.Update()
        Else
            img1.Visible = False
            a1.Visible = False
            lblVideoName1.Visible = False
            lblDescription1.Visible = False

            img2.Visible = False
            a2.Visible = False
            lblVideoName2.Visible = False
            lblDescription2.Visible = False

            upnlBelowList.Update()
        End If
        If TempFlypalVideoHelpList.Count >= 14 Then
            img3.Src = TempFlypalVideoHelpList(11).ThumbnailPath
            a3.Title = TempFlypalVideoHelpList(11).SrNo.ToString
            lblVideoName3.Text = TempFlypalVideoHelpList(11).VideoName
            lblDescription3.Text = TempFlypalVideoHelpList(11).Description

            img4.Src = TempFlypalVideoHelpList(12).ThumbnailPath
            a4.Title = TempFlypalVideoHelpList(12).SrNo.ToString
            lblVideoName4.Text = TempFlypalVideoHelpList(12).VideoName
            lblDescription4.Text = TempFlypalVideoHelpList(12).Description

            img5.Src = TempFlypalVideoHelpList(13).ThumbnailPath
            a5.Title = TempFlypalVideoHelpList(13).SrNo.ToString
            lblVideoName5.Text = TempFlypalVideoHelpList(13).VideoName
            lblDescription5.Text = TempFlypalVideoHelpList(13).Description
            upnlBelowList.Update()
        Else
            img3.Visible = False
            a3.Visible = False
            lblVideoName3.Visible = False
            lblDescription3.Visible = False

            img4.Visible = False
            a4.Visible = False
            lblVideoName4.Visible = False
            lblDescription4.Visible = False

            img5.Visible = False
            a5.Visible = False
            lblVideoName5.Visible = False
            lblDescription5.Visible = False

            upnlBelowList.Update()
        End If
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
                upnlVideo.Update()
                Dim TempFlypalVideoHelpList = (Temptbl.AsEnumerable().Select(Function(c) New With {.VideoPath = c.Field(Of String)("VideoPath"), .SrNo = c.Field(Of Integer)("SrNo"), .id = c.Field(Of Guid)("id"), .ThumbnailPath = c.Field(Of String)("ThumbnailPath")}).Where(Function(c) c.SrNo >= mSrNo)).ToList
                dgGridView1.DataSource = Nothing
                dgGridView1.DataBind()

                dgGridView1.DataSource = TempFlypalVideoHelpList
                dgGridView1.PageIndex = 0
                dgGridView1.DataBind()

                Session("TempFlypalVideoHelpListFoSecondGrid") = TempFlypalVideoHelpList
                upnlVideoGridView.Update()
                'If TempFlypalVideoHelpList.ToList().Count > 0 Then
                '    img1.Src = TempFlypalVideoHelpList(mSrNo + 1).ThumbnailPath
                '    img2.Src = TempFlypalVideoHelpList(mSrNo + 2).ThumbnailPath
                '    img3.Src = TempFlypalVideoHelpList(mSrNo + 3).ThumbnailPath
                '    img4.Src = TempFlypalVideoHelpList(mSrNo + 4).ThumbnailPath
                '    img5.Src = TempFlypalVideoHelpList(mSrNo + 5).ThumbnailPath
                '    img6.Src = TempFlypalVideoHelpList(mSrNo + 6).ThumbnailPath
                'End If
        End Select
    End Sub
    Private Sub dgGridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgGridView1.PageIndexChanging
        dgGridView1.PageIndex = e.NewPageIndex
        dgGridView1.DataSource = mTempFlypalVideoHelpListDataTable
        dgGridView1.DataBind()
        upnlVideoGridView.Update()
        Session("TempFlypalVideoHelpListFoSecondGrid") = mTempFlypalVideoHelpListDataTable
    End Sub
#End Region

    Private Sub a1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles a1.ServerClick, a2.ServerClick, a3.ServerClick, a4.ServerClick, a5.ServerClick ', a6.ServerClick
        Dim SrNo As Integer = 0
        If sender.ClientID = "a1" Then
            SrNo = CInt(a1.Title)
        ElseIf sender.ClientID = "a2" Then
            SrNo = CInt(a2.Title)
        ElseIf sender.ClientID = "a3" Then
            SrNo = CInt(a3.Title)
        ElseIf sender.ClientID = "a4" Then
            SrNo = CInt(a4.Title)
        ElseIf sender.ClientID = "a5" Then
            SrNo = CInt(a5.Title)
            'ElseIf sender.ClientID = "a6" Then
            '    SrNo = CInt(a6.Title)
        End If
        Vediosource.Visible = False
        upnlVideo.Update()
        Dim TempFlypalVideoHelpList = (Temptbl.AsEnumerable().Select(Function(c) New With {.VideoPath = c.Field(Of String)("VideoPath"), .SrNo = c.Field(Of Integer)("SrNo"), .id = c.Field(Of Guid)("id"), .ThumbnailPath = c.Field(Of String)("ThumbnailPath")}).Where(Function(c) c.SrNo >= SrNo)).ToList
        dgGridView1.DataSource = Nothing
        dgGridView1.DataBind()

        dgGridView1.DataSource = TempFlypalVideoHelpList
        dgGridView1.PageIndex = 0
        dgGridView1.DataBind()
        Session("TempFlypalVideoHelpListFoSecondGrid") = TempFlypalVideoHelpList
        upnlVideoGridView.Update()
    End Sub

End Class