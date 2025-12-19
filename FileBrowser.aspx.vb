'Devendra Naik 30/Aug/2025

Imports System.Collections.Generic

Partial Class FileBrowser
    Inherits System.Web.UI.Page

    'THIS IS SECURE ROOT PATH OF THE LIBEARY FOLDER WHICH WILL COME FROM WEB.CONFIG
    'USER CAN ACCESS THE FILES AND FOLDERS FROM THIS ROOT PATH ONLY NO OTHER FILES ARE VISIBLE TO USER

    Private rootPath As String = AppSettings("LegacyDataPath") '"C:\Users\DeVeN\source\repos\flypal4.0\History_Data" ' secure root

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Get current path from query string
        ' It can be rootpath or inner folder path 
        Dim currentPath As String = Request.QueryString("path")

        If String.IsNullOrEmpty(currentPath) Then
            currentPath = "" ' default root
        End If

        ' Always call LoadFolder (no IsPostBack check, because query string drives navigation)
        LoadFolder(currentPath)
    End Sub

    Private Sub LoadFolder(path As String)
        Dim safePath As String = If(path, "")
        Dim fullPath As String = System.IO.Path.Combine(rootPath, safePath)

        '-----*****-----Very IMP --------------****-------------- 'DEVEN 02/Sep/2025
        'USER CAN ACCESS THE FILES AND FOLDERS FROM THIS ROOT PATH ONLY NO OTHER FILES ARE VISIBLE TO USER
        ' Prevent directory traversal

        If Not fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) Then
            Response.StatusCode = 403
            Response.End()
        End If

        Dim dirInfo As New DirectoryInfo(fullPath)
        Session("DOCPath") = fullPath
        Label1.Text = IIf(dirInfo.Name <> "", safePath, "")

        ' Get and show Folders from the current Path
        Dim folders As New List(Of String)
        Dim d As DirectoryInfo

        For Each d In dirInfo.GetDirectories()
			Dim newPath As String = If(String.IsNullOrEmpty(path), d.Name, path & "\" & d.Name)
			Dim creatdate As String = New SmartDate(d.LastWriteTime.ToString).FormattedText
			'folders.Add($"<li>📁 <a href='FileBrowser.aspx?path={Server.UrlEncode(newPath)}'>{d.Name}</a></li>")
			folders.Add($"<div class='folder-entry'>
					<span class='folder-icon'><i class='fa-solid fa-folder icon-folder'></i></span>
					<span class='folder-name'>
						<a href='FileBrowser.aspx?path={Server.UrlEncode(newPath)}'>{d.Name}</a>
						<small>Last modified: {creatdate}</small>
					</span>
					</div>")
		Next

        ' Get and show Files from the current Path
        Dim files As New List(Of String)
        Dim f As FileInfo
        For Each f In dirInfo.GetFiles()
            Dim filePath As String = If(String.IsNullOrEmpty(path), f.Name, path & "\" & f.Name)

            Dim ext As String = f.Extension.ToLower()

            Dim icon As String = ""
            Select Case ext
                Case ".pdf"
                    icon = "<i class='fa-solid fa-file-pdf icon-pdf'></i>"
                Case ".doc", ".docx"
                    icon = "<i class='fa-solid fa-file-word icon-word'></i>"
                Case ".xls", ".xlsx"
                    icon = "<i class='fa-solid fa-file-excel icon-excel'></i>"
                Case ".ppt", ".pptx"
                    icon = "<i class='fa-solid fa-file-powerpoint icon-ppt'></i>"
                Case ".jpg", ".jpeg", ".png", ".gif"
                    icon = "<i class='fa-solid fa-file-image icon-image'></i>"
                Case ".txt"
                    icon = "<i class='fa-solid fa-file-lines icon-txt'></i>"
                Case ".zip", ".rar"
                    icon = "<i class='fa-solid fa-file-zipper icon-zip'></i>"
                Case Else
                    icon = "<i class='fa-solid fa-file icon-default'></i>"
            End Select



			'files.Add($"<li>{icon} <a href='Download.aspx?path={Server.UrlEncode(filePath)}'>{f.Name}</a> 
			'                     <small>({f.Length} bytes, {f.LastWriteTime})</small></li>")
			Dim creatdate As String = New SmartDate(f.LastWriteTime.ToString).FormattedText
			files.Add($"<div class='file-entry'>
								<span class='file-icon'>{icon}</span>
								<span class='file-name'>
									<a href='Download.aspx?path={Server.UrlEncode(filePath)}'>{f.Name}</a>
									<small>({f.Length} bytes, {creatdate})</small>
								</span>
								</div>")

		Next

        ' Back link if not root   ----- *** Root folder will not have back link at all to secure other files 
        Dim backLink As String = ""
        If Not String.IsNullOrEmpty(path) AndAlso path.Contains("\") Then
            Dim parent As String = path.Substring(0, path.LastIndexOf("\"))
            backLink = $"<li><a href='FileBrowser.aspx?path={Server.UrlEncode(parent)}'>⬆️ Back</a></li>"
        ElseIf Not String.IsNullOrEmpty(path) Then
            ' if only one level deep
            backLink = $"<li><a href='FileBrowser.aspx'>⬆️ Back</a></li>"
        End If

        litContent.Text = $"<ul>{backLink}{String.Join("", folders)}{String.Join("", files)}</ul>"
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
End Class




''Imports System.Collections.Generic
''Imports System.IO
''Imports System.Web.Script.Serialization

''Partial Class FileBrowser
''    Inherits System.Web.UI.Page

''    Private rootPath As String = "C:\Users\DeVeN\source\repos\flypal4.0"

''    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
''        Dim mode As String = Request.QueryString("mode")

''        If mode = "json" Then
''            Dim path As String = Request.QueryString("path")
''            If String.IsNullOrEmpty(path) Then path = ""

''            Dim safePath As String = If(path, "")
''            Dim fullPath As String = System.IO.Path.Combine(rootPath, safePath)

''            ' Security check
''            If Not fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) Then
''                Response.StatusCode = 403
''                Response.End()
''            End If

''            Dim nodes As New List(Of Object)
''            Dim dirInfo As New DirectoryInfo(fullPath)
''            ' Folders

''            Dim d As DirectoryInfo

''            For Each d In dirInfo.GetDirectories()
''                Dim di As DirectoryInfo = d
''                nodes.Add(New With {
''                    Key .id = If(String.IsNullOrEmpty(path), di.Name, path & "\" & di.Name),
''                    Key .text = di.Name,
''                    Key .children = True,
''                    Key .type = "folder"
''                })
''            Next

''            ' Files
''            Dim f As FileInfo

''            For Each f In dirInfo.GetFiles()
''                Dim fi As FileInfo = f
''                Dim ext As String = fi.Extension.ToLower()
''                Dim fileType As String

''                Select Case ext
''                    Case ".pdf" : fileType = "pdf"
''                    Case ".doc", ".docx" : fileType = "word"
''                    Case ".xls", ".xlsx" : fileType = "excel"
''                    Case ".jpg", ".jpeg", ".png", ".gif" : fileType = "image"
''                    Case Else : fileType = "file"
''                End Select

''                nodes.Add(New With {
''                    Key .id = If(String.IsNullOrEmpty(path), fi.Name, path & "\" & fi.Name),
''                    Key .text = fi.Name,
''                    Key .children = False,
''                    Key .type = fileType
''                })
''            Next

''            Dim serializer As New JavaScriptSerializer()
''            Dim json As String = serializer.Serialize(nodes)

''            Response.ContentType = "application/json"
''            Response.Write(json)
''            Response.End()
''        End If
''    End Sub

''End Class





