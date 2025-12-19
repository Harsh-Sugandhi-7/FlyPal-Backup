Public Class wfFileAttachment_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private mFileAttachment As FileAttachment
#End Region

#Region "Methods"
    Private Sub GetSession()
        mFileAttachment = Session("mFileAttachment")
    End Sub
    Private Sub ControlVisibility()
        If mFileAttachment.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
            btnSelectFile.Disabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
            btnSelectFile.Disabled = False
        End If
    End Sub
    Private Sub PanelVisibility()
        If mFileAttachment.ImageSize > 0 Then
            Panel2.Visible = True
        Else
            Panel2.Visible = False
        End If
    End Sub
    Private Sub AttachMyFile()
        Try
            mFileAttachment.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mFileAttachment.ImageSize = Session("FileUpload.FileSize")
            mFileAttachment.FileExtension = Session("FileUpload.FileExtension")
            Session("mFileAttachment") = mFileAttachment
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibility()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Private Sub addattributes()
    End Sub
#End Region


#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            mFileAttachment = FileAttachment.GetFileAttachment()
            Session("mFileAttachment") = mFileAttachment
            ControlVisibility()
            PanelVisibility()
        End If
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mFileAttachment.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttachment.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachment.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttachment.ImageFile, 0, mFileAttachment.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        Else
            MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
            ControlVisibility()
        End If
    End Sub
      Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mFileAttachment.ImageFile = file1
        mFileAttachment.ImageSize = 0
        mFileAttachment.Save()
        Session("mFileAttachment") = mFileAttachment
        Panel2.Visible = False
        ControlVisibility()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        mFileAttachment = Session("mFileAttachment")
        If mFileAttachment.IsValid Then
           mFileAttachment.ApplyEdit()
            Try
                mFileAttachment.Save()
            Catch ex As Exception
                Throw ex
            End Try
            Session("mFileAttachment") = mFileAttachment
            If mFileAttachment.ImageSize > 0 Then
                'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Attachment Saved Successfully!"))
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                ImageButton1.Visible = True
            End If
        End If
        Panel2.Visible = False
     End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Panel2.Visible = False
        Session.Remove("mFileAttachment")
        Session.Remove("Removed")
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

End Class