'Created By Utkarsh 23-Nov-2010
Partial Class wfEmployeeDocumentHistoryList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declarations"
    Protected mEmployeeDocumentHistoryList As EmployeeDocumentHistoryList
    Public mEmployeeDocument As EmployeeDocument
    Dim mEmployeeID As String
    Dim mDocumentID As String
    Dim mReferenceID As String
#End Region

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

#Region " Business Methods "
    Private Sub GetSession()
        mEmployeeDocumentHistoryList = Session("mEmployeeDocumentHistoryList")
        mEmployeeDocument = Session("mEmployeeDocument")
        mEmployeeID = Session("mEmployeeID")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDocumentHistoryList") = mEmployeeDocumentHistoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeDocumentHistoryList")
        Session.Remove("mEmployeeID")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Get List From the Database as per Criteria             
        mEmployeeDocumentHistoryList = EmployeeDocumentHistoryList.GetEmployeeDocumentHistoryList(New Guid(mEmployeeID.ToString), New Guid(mDocumentID), New Guid(mReferenceID))
        dgEmployeeDocumentHistoryList.DataSource = mEmployeeDocumentHistoryList
        Session("mEmployeeDocumentHistoryList") = mEmployeeDocumentHistoryList
        dgEmployeeDocumentHistoryList.DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            'mEmployeeID = Request.QueryString("EmployeeID")
            'mDocumentID = Request.QueryString("DocumentID")
            'mReferenceID = Request.QueryString("ReferenceID")
            mDocumentID = mEmployeeDocument.DocumentID.ToString
            mReferenceID = mEmployeeDocument.ReferenceID.ToString
            DataFieldBind()
         End If
    End Sub
    Private Sub dgEmployeeDocumentHistoryList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmployeeDocumentHistoryList.RowCommand
        Dim Idx As Integer
        Dim mID As Guid
        Select Case e.CommandName
            Case "Attach"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                'Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                'Dim rowIndex As Integer = gvr.RowIndex
                'Idx = rowIndex + dgEmployeeDocumentHistoryList.PageIndex * dgEmployeeDocumentHistoryList.PageSize
                mID = New Guid(e.CommandArgument.ToString) 'CType(dgEmployeeDocumentHistoryList.DataKeys(rowIndex).Values("ID"), Guid)

                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                If mEmployeeDocument.ImageSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeDocument.FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDocument.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mEmployeeDocument.ImageFile, 0, mEmployeeDocument.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgEmployeeDocumentHistoryList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEmployeeDocumentHistoryList.Sorting
        mEmployeeDocumentHistoryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgEmployeeDocumentHistoryList.DataSource = mEmployeeDocumentHistoryList
        Session("mEmployeeDocumentHistoryList") = mEmployeeDocumentHistoryList
        dgEmployeeDocumentHistoryList.DataBind()
     End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub

    Private Sub dgEmployeeDocumentHistoryList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgEmployeeDocumentHistoryList.PageIndexChanging
        dgEmployeeDocumentHistoryList.PageIndex = e.NewPageIndex
        dgEmployeeDocumentHistoryList.DataSource = mEmployeeDocumentHistoryList
        Session("mEmployeeDocumentHistoryList") = mEmployeeDocumentHistoryList
        dgEmployeeDocumentHistoryList.DataBind()
    End Sub
#End Region

End Class
