Partial Class wfFileView
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub


	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

	Dim MyFile As String

#End Region

#Region " Page Event(s) "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Dim AttachmentName As String
		Dim FileName As String
		Dim Extension As String
		Try

			MyFile = CStr(Session("DOCPath"))
			Extension = MyFile.Substring(MyFile.LastIndexOf("."))
			AttachmentName = Session("AttachmentName")

			Response.ClearContent()
			Response.ClearHeaders()

			If Extension = ".pdf" Then
				Response.ContentType = "application/pdf"
			ElseIf (Extension = ".xls" Or Extension = ".xlsx") Then
				Response.ContentType = "application/vnd.ms-excel"
			ElseIf (Extension = ".doc" Or Extension = ".docx") Then
				Response.ContentType = "application/msword"
			ElseIf Extension = ".jpg" Or Extension = ".jpeg" Then
				Response.ContentType = "image/jpeg"
			ElseIf Extension = ".png" Then
				Response.ContentType = "image/png"
			ElseIf (Extension = ".ppt" Or Extension = ".pptx") Then
				Response.ContentType = "application/vnd.ms-powerpoint"
			ElseIf Extension = ".gif" Then
				Response.ContentType = "image/gif"
			ElseIf Extension = ".ico" Then
				Response.ContentType = "image/vnd.microsoft.icon"
			ElseIf Extension = ".zip" Then
				Response.ContentType = "application/zip"
			Else
				Response.ContentType = "application/octet-stream"
			End If

			If AttachmentName = "" Or AttachmentName Is Nothing Or Session("AttachmentName") Is Nothing Then
				FileName = MyFile.Substring(MyFile.LastIndexOf("\") + 1)
			Else
				FileName = AttachmentName
			End If

			Response.AppendHeader("Content-Disposition", $"attachment; filename=""{HttpUtility.UrlEncode(FileName)}""")
			Response.AddHeader("Pragma", "public")
			Response.AddHeader("Cache-Control", "public")
			Response.AddHeader("Expires", "0")

			Dim fileInfo As New FileInfo(MyFile)
			Response.AppendHeader("Content-Length", fileInfo.Length.ToString())

			Response.WriteFile(MyFile)
			Response.Flush()

			If Context IsNot Nothing Then
				Context.ApplicationInstance.CompleteRequest()
			End If

		Catch ex As Exception

			lblMsg.Visible = True
			lblMsg.Text = "Specified File not found in the default location ! <BR> <BR> The File must have been Relocated or Deleted "

		End Try

	End Sub

	Private Sub Page_Unload(sender As Object, e As EventArgs) Handles MyBase.Unload

		Try

			If MyFile IsNot Nothing Then
				File.Delete(MyFile)
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
