Imports System.Linq
Imports System.Collections.Generic

Partial Class MainMenu
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

#Region " Variable "
    Public objModuleList As UserModuleList 'Changed by Utkarsh on 10-Jan-2013 For  ALL10012013
    'Public Child1 As UserModuleList.UserModuleInfo 'Added by Utkarsh on 10-Jan-2013 For ALL10012013
    Public objModuleListLinq
    Public mEventLog As EventLog
    Dim path As String = String.Empty
    Public mEmployee As Employee
    Public mUserFavouritesList As UserFavouritesList
    Public mUserFavouritesListLinq
#End Region

#Region " Events "
    'Public Sub SessionClear()
    '    Session("mStatus") = ""
    '    Session.Remove("mStatus")
    '    Session.Remove("objModel")
    '    Session.Remove("objItem")
    'End Sub
    Public Function IsModuleListExist(ByVal ModTypeName As String, ByVal flag() As String) As Boolean
        Dim j As Int16
        For j = 0 To UBound(flag)
            If ModTypeName.Equals(flag(j)) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim k As Int16 = 0
        If Session("MenuID") = "" Then
            'smMainMenu.Visible = True
        End If
        objModuleList = UserModuleList.GetUserModuleList(HttpContext.Current.User.Identity.Name)
        objModuleListLinq = From c As UserModuleList.UserModuleInfo In objModuleList
                      Where c.MainMenu <> ""
                      Group c By key = c.MainMenu Into MenuList = Group
                      Select MainMenu = key, SubMenuCollection = MenuList

        'Session("objModuleList") = objModuleList
        mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))

        mUserFavouritesList = UserFavouritesList.GetUserFavourites(HttpContext.Current.User.Identity.Name)

        mUserFavouritesListLinq = From c As UserFavourites In mUserFavouritesList
                      Where c.MainMenu <> ""
                      Group c By key = c.MainMenu Into MenuList = Group
                      Select MainMenu = key, SubMenuCollection = MenuList

        ShowPicture()
    End Sub
    Public Sub ShowPicture()
        Dim mUser As User = mUser.GetUser(mEventLog.UserID)
        mEmployee = Employee.GetEmployee(mUser.EmployeeID)
        path = Session("path")

        If (path <> "") Then
            System.IO.File.Delete(path)
            path = String.Empty
            Session("path") = path
        End If

        Dim di As System.IO.DirectoryInfo = New DirectoryInfo(AppSettings("DOCPath"))
        Dim fiArr As FileInfo() = di.GetFiles()
        Dim fi As IO.FileInfo
        For Each fi In fiArr
            If (fi.Extension.ToString().ToUpper = (".pdf").ToUpper Or fi.Extension.ToString().ToUpper = (".xsd").ToUpper Or _
                fi.Extension.ToString().ToUpper = ("").ToUpper Or fi.Extension.ToString().ToUpper = (".bmp").ToUpper Or _
                fi.Extension.ToString().ToUpper = (".jpg").ToUpper Or fi.Extension.ToString().ToUpper = (".xls").ToUpper Or _
                fi.Extension.ToString().ToUpper = (".xlsx").ToUpper) And fi.CreationTime < Today.Date Then
                fi.Delete()
            End If
        Next

        Dim Tempdi As System.IO.DirectoryInfo = New DirectoryInfo(AppSettings("TempDir"))
        Dim TempfiArr As FileInfo() = Tempdi.GetFiles()
        Dim Tempfi As IO.FileInfo
        For Each Tempfi In TempfiArr
            If (Tempfi.Extension.ToString().ToUpper = (".jpg").ToUpper Or Tempfi.Extension.ToString().ToUpper = ".png" Or _
                Tempfi.Extension.ToString().ToUpper = (".bmp").ToUpper Or Tempfi.Extension.ToString().ToUpper = (".pdf").ToUpper Or _
                Tempfi.Extension.ToString().ToUpper = (".xsd").ToUpper Or Tempfi.Extension.ToString().ToUpper = (".xls").ToUpper Or _
                Tempfi.Extension.ToString().ToUpper = (".xlsx").ToUpper) And Tempfi.CreationTime < Today.Date Then
                Tempfi.Delete()
            End If
        Next

        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & mEmployee.Name  '& No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployee.ImageSize > 0 Then
            path = AppSettings("DOCPath") & StrName & mEmployee.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployee.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployee.ImageFile, 0, mEmployee.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ''MyImage.ImageUrl = path ''& "\MyTest" & mEmployee.FileExtension
                ''MyImage.ImageUrl = "http://" & Me.Request.Url.Host & "/FlyPal/Documents/" & StrName & mEmployee.FileExtension
                ''For local
                'MyImage1.ImageUrl = "http://" & Me.Request.Url.Host & "/" & Me.Request.Url.Segments(1) & "Documents/" & StrName & mEmployee.FileExtension

                'For Server
                MyImage1.ImageUrl = AppSettings("HTTPSecurity") & Me.Request.Url.Host & "/" & Me.Request.Url.Segments(1) & "Documents/" & StrName & mEmployee.FileExtension


                ' MyImage.ImageUrl = "images/abc408708192.jpg"
                MyImage1.Visible = True
                Session("path") = path
            End If
        Else
            MyImage1.Visible = False
        End If
    End Sub
    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
        Session("Message") = Context.Server.GetLastError.Message
        Session("Source") = Context.Server.GetLastError.Source
        Session("Trace") = Context.Server.GetLastError.StackTrace
    End Sub
#End Region

#Region " Stuff "
    'While k < objModuleList.Count
    '    If objModuleList(k).MainMenu <> "" Then

    '    Else
    '        smMainMenu.AddParent("M" & k.ToString, objModuleList(k).MainMenu)
    '        MainMenu = objModuleList(k).MainMenu
    '        While MainMenu = objModuleList(k).MainMenu And k < objModuleList.Count
    '            smMainMenu.AddChild(objModuleList(k).ModuleID.ToString, objModuleList(k).SubMenu, objModuleList(k).URL & "&ModuleID=" & objModuleList(k).ModuleID.ToString)
    '            k += 1
    '            If k = objModuleList.Count Then Exit While
    '        End While
    '        If k = objModuleList.Count Then Exit While
    '    End If
    'End While
#End Region

End Class
