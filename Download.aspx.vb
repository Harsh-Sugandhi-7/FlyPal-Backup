'Devendra Naik 30/Aug/2025

Partial Class Download
    Inherits System.Web.UI.Page

    'THIS IS SECURE ROOT PATH OF THE LIBEARY FOLDER WHICH WILL COME FROM WEB.CONFIG
    Private rootPath As String = AppSettings("LegacyDataPath")   '"C:\Users\DeVeN\source\repos\flypal4.0\History_Data"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim path As String = Request.QueryString("path")
        If String.IsNullOrEmpty(path) Then
            Response.StatusCode = 400
            Response.End()
        End If

        Dim fullPath As String = System.IO.Path.Combine(rootPath, path)
        ' Dim fullPath As String = rootPath
        'USER CAN ACCESS THE FILES AND FOLDERS FROM THIS ROOT PATH ONLY NO OTHER FILES ARE VISIBLE TO USER
        ' Security check
        If Not File.Exists(fullPath) OrElse Not fullPath.StartsWith(rootPath) Then
            Response.StatusCode = 404
            Response.End()
        End If

        Dim fileName As String = path

        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName) 'to download file
        'Response.AppendHeader("Content-Disposition", "inline; filename=" & FileName) 'to directly open file in browser if supported
        'Response.WriteFile(fullPath)
        Response.TransmitFile(fullPath)
        Response.End()
    End Sub
End Class

''Imports System.IO

''Partial Class Download
''    Inherits System.Web.UI.Page

''    Private rootPath As String = "C:\Users\DeVeN\source\repos\flypal4.0"

''    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
''        Dim path As String = Request.QueryString("path")
''        If String.IsNullOrEmpty(path) Then
''            Response.StatusCode = 400
''            Response.End()
''        End If

''        Dim fullPath As String = System.IO.Path.Combine(rootPath, path)

''        ' Security check
''        If Not File.Exists(fullPath) OrElse Not fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) Then
''            Response.StatusCode = 404
''            Response.End()
''        End If

''        Dim fileName As String = System.IO.Path.GetFileName(fullPath)

''        Response.Clear()
''        Response.ContentType = "application/octet-stream"
''        Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
''        Response.TransmitFile(fullPath)
''        Response.End()
''    End Sub
''End Class
