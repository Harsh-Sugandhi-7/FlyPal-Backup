Imports System.Linq
Public Class wfFlypalHelpVideoToUpload
    Inherits System.Web.UI.Page

#Region "Variables and Declarations"
    Dim strMsg As String = ""
    Dim EditString As String = ""
    Dim mModuleList As ModuleList
    Dim mTempFlypalHelpVideoToUploadID As Guid
#End Region

#Region "Methods"
    Private Sub GetSession()
        EditString = Session("FlypalHelpVideoToUploadEdit")
        mTempFlypalHelpVideoToUploadID = Session("FlypalHelpVideoToUploadID")
    End Sub
#End Region

#Region "Data Access"
    Private Sub DataAccess(ByVal ID As Guid, Optional ByVal VideoName As String = "", Optional ByVal VideoPath As String = "", _
                           Optional ByVal Description As String = "", Optional ByVal ThumbnailPath As String = "", _
                           Optional ByVal ModuleID As Integer = 0, Optional ByVal ForWhat As String = "")
        Dim conString As String = System.Configuration.ConfigurationManager.AppSettings("DB:MasterDataBase")
        Dim con = New SqlConnection(conString)
        Try
            con.Open()
            Dim cmd As New SqlCommand()
            cmd.Connection = con
            cmd.CommandType = CommandType.StoredProcedure
            If ForWhat = "New" Then
                cmd.CommandText = "FlypalVideoHelpAdd"
                cmd.Parameters.AddWithValue("@VideoName", VideoName)
                cmd.Parameters.AddWithValue("@VideoPath", VideoPath)
                cmd.Parameters.AddWithValue("@Description", Description)
                cmd.Parameters.AddWithValue("@ThumbnailPath", ThumbnailPath)
                cmd.Parameters.AddWithValue("@ModuleID", ModuleID)
                Dim Dr As New SafeDataReader(cmd.ExecuteReader)
            End If
            If ForWhat = "Update" Then
                cmd.CommandText = "FlypalVideoHelpUpdate"
                cmd.Parameters.AddWithValue("@ID", ID)
                cmd.Parameters.AddWithValue("@VideoName", txtVideoName.Text.Trim)
                cmd.Parameters.AddWithValue("@VideoPath", txtVideoPath.Text.Trim)
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim)
                cmd.Parameters.AddWithValue("@ThumbnailPath", txtThumbnailPath.Text.Trim)
                cmd.Parameters.AddWithValue("@ModuleID", ModuleID)
                Dim Dr As New SafeDataReader(cmd.ExecuteReader)
            End If
            If ForWhat = "Edit" Then
                cmd.CommandText = "FlypalVideoHelpFetch"
                cmd.Parameters.AddWithValue("@ID", ID)
                Dim Dr As New SafeDataReader(cmd.ExecuteReader)
            End If
            If ForWhat = "Delete" Then
                cmd.CommandText = "FlypalVideoHelpDelete"
                cmd.Parameters.AddWithValue("@ID", ID)
                Dim Dr As New SafeDataReader(cmd.ExecuteReader)
            End If
        Catch ex As Exception
            Throw ex.GetBaseException
        Finally
            con.Close()
        End Try
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = System.Configuration.ConfigurationManager.AppSettings("DB:MasterDataBase")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "FlypalVideoHelpListFetch"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Search", "")

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        Return dataTable
    End Function
    Private Sub GenerateList(ByVal tbl As DataTable)
        dgGridView.DataSource = tbl
        dgGridView.DataBind()
        upnlGrid.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModuleList = ModuleList.GetModuleList(AddTopItem:="~Select")
        Dim mTempModuleList = (From c In mModuleList
                                Order By c.Description
                                Select c).ToList
        cmbModuleName.DataSource = mTempModuleList
        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load 'FlypalHelpVideoToUploadEdit
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            GenerateList(CreateDataTable())
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If hdnValue.Value = "false" Then
            strMsg = strMsg + "Please Enter Valid path" + "<Br>"
        End If
        If strMsg.Trim <> "" Then
            cvCc.ErrorMessage = strMsg
            cvCc.IsValid = False
            Exit Sub
        End If
        If Not IsValid Then
            upnlDetails.Update()
            Exit Sub
        End If
        Try
            If EditString = "FlypalHelpVideoToUploadEdit" Then
                Session.Remove("FlypalHelpVideoToUploadEdit")
                Session.Remove("FlypalHelpVideoToUploadID")

                DataAccess(ID:=mTempFlypalHelpVideoToUploadID, VideoName:=txtVideoName.Text.Trim, VideoPath:=txtVideoPath.Text.Trim, Description:=txtDescription.Text.Trim, _
                           ThumbnailPath:=txtThumbnailPath.Text.Trim, ModuleID:=cmbModuleName.SelectedValue, ForWhat:="Update")

                EditString = ""
                mTempFlypalHelpVideoToUploadID = Guid.Empty
            Else
                DataAccess(ID:=Guid.Empty, VideoName:=txtVideoName.Text.Trim, VideoPath:=txtVideoPath.Text.Trim, Description:=txtDescription.Text.Trim, _
                           ThumbnailPath:=txtThumbnailPath.Text.Trim, ModuleID:=cmbModuleName.SelectedValue, ForWhat:="New")
            End If
            GenerateList(CreateDataTable())
            txtVideoName.Text = ""
            txtVideoPath.Text = ""
            txtDescription.Text = ""
            txtThumbnailPath.Text = ""
            upnlDetails.Update()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgGridView.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim mID As Guid = New Guid(dgGridView.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                SetFocus(txtVideoName)
                txtVideoName.Text = dgGridView.DataKeys(CInt(e.CommandArgument)).Values("VideoName").ToString
                txtVideoPath.Text = dgGridView.DataKeys(CInt(e.CommandArgument)).Values("VideoPath").ToString
                txtDescription.Text = dgGridView.DataKeys(CInt(e.CommandArgument)).Values("Description").ToString
                txtThumbnailPath.Text = dgGridView.DataKeys(CInt(e.CommandArgument)).Values("ThumbnailPath").ToString
                If dgGridView.DataKeys(CInt(e.CommandArgument)).Values("ModuleID").ToString = "" Then
                    cmbModuleName.SelectedValue = 0
                Else
                    cmbModuleName.SelectedValue = CInt(dgGridView.DataKeys(CInt(e.CommandArgument)).Values("ModuleID"))
                End If
                upnlDetails.Update()
                Session("FlypalHelpVideoToUploadID") = mID
                Session("FlypalHelpVideoToUploadEdit") = "FlypalHelpVideoToUploadEdit"
            Case "DeleteRecord"
                Dim mID As Guid = New Guid(dgGridView.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                DataAccess(ID:=mID, ForWhat:="Delete")
                GenerateList(CreateDataTable())
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Session.Remove("FlypalHelpVideoToUploadEdit")
        Session.Remove("FlypalHelpVideoToUploadID")
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region



End Class