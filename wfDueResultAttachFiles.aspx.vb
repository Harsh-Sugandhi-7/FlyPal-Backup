Partial Class wfDueResultAttachFiles
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public mAttachFiles As AttachFiles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mAttachFiles = Session("mAttachFiles")
        dgAttachFileList.DataSource = mAttachFiles
        DataBind()
    End Sub

    Private Sub dgAttachFileList_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAttachFileList.ItemCommand
        '-----------------------------------------------------------
        Dim MyPath, MyName As String
        Dim mAttachFileDetail As AttachFileDetail

        MyPath = "C:\Temp"                  ' Set the path.
        MyName = Dir(MyPath, vbDirectory)   ' Retrieve the first entry.
        If MyName = "" Then                 ' The folder is not there & to be created
            MkDir("C:\Temp\")               ' Folder created
        End If
        '-----------------------------------------------------------

        Dim mID As Guid = mAttachFiles(e.Item.ItemIndex).ID
        mAttachFileDetail = AttachFileDetail.GetAttachFileDetail(mID)

        Session("DOCPath") = AppSettings("DOCPath") & mAttachFileDetail.ID.ToString & ".PDF"
        If mAttachFileDetail.Path <> "" Then
            Dim Str As String
            Str = "<script language=Javascript>openFile();</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("Index.aspx")
    End Sub
End Class
