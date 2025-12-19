'Created By Utkarsh 23-Nov-2010
Partial Class wfEmployeeTrainingHistoryList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declarations"
    Protected mEmployeeTrainingHistoryList As EmployeeTrainingHistoryList
    Public mEmployeeTraining As EmployeeTraining
    Dim mEmployeeID As String
    Dim mTrainingID As String
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
        mEmployeeTrainingHistoryList = Session("mEmployeeTrainingHistoryList")
        mEmployeeTraining = Session("mEmployeeTraining")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeTrainingHistoryList") = mEmployeeTrainingHistoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeTrainingHistoryList")
        Session.Remove("mEmployeeID")
        Session.Remove("mEmployeeTraining")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
  
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Get List From the Database as per Criteria             
        mEmployeeTrainingHistoryList = EmployeeTrainingHistoryList.GetEmployeeTrainingHistoryList(New Guid(mEmployeeID.ToString), New Guid(mTrainingID), New Guid(mReferenceID))
        dgEmployeeTrainingHistoryList.DataSource = mEmployeeTrainingHistoryList
        Session("mEmployeeTrainingHistoryList") = mEmployeeTrainingHistoryList
        dgEmployeeTrainingHistoryList.DataBind()
    End Sub

#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            'mEmployeeID = Request.QueryString("EmployeeID")
            'mTrainingID = Request.QueryString("TrainingID")
            'mReferenceID = Request.QueryString("ReferenceID")
            mEmployeeID = Session("mEmployeeID")
            mTrainingID = mEmployeeTraining.TrainingID.ToString
            mReferenceID = mEmployeeTraining.ReferenceID.ToString
            DataFieldBind()
        End If
    End Sub
    Private Sub dgEmployeeTrainingHistoryList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmployeeTrainingHistoryList.RowCommand
        Dim mID As Guid
        Select Case e.CommandName
            Case "Attach"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                'Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                'Dim rowIndex As Integer = gvr.RowIndex
                'Idx = rowIndex + dgEmployeeTrainingHistoryList.PageIndex * dgEmployeeTrainingHistoryList.PageSize
                mID = New Guid(e.CommandArgument.ToString) 'CType(dgEmployeeTrainingHistoryList.DataKeys(CInt(e.CommandArgument)).Values("ID"), Guid)

                ' mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
                Dim mFileAttach As FileAttach
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach

                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgEmployeeTrainingHistoryList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEmployeeTrainingHistoryList.Sorting
        mEmployeeTrainingHistoryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgEmployeeTrainingHistoryList.DataSource = mEmployeeTrainingHistoryList
        Session("mEmployeeTrainingHistoryList") = mEmployeeTrainingHistoryList
        dgEmployeeTrainingHistoryList.DataBind()
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

    Private Sub dgEmployeeTrainingHistoryList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgEmployeeTrainingHistoryList.PageIndexChanging
        dgEmployeeTrainingHistoryList.PageIndex = e.NewPageIndex
        dgEmployeeTrainingHistoryList.DataSource = mEmployeeTrainingHistoryList
        Session("mEmployeeTrainingHistoryList") = mEmployeeTrainingHistoryList
        dgEmployeeTrainingHistoryList.DataBind()
    End Sub
#End Region

End Class
