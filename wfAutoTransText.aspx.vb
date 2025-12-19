Imports System.Text
Partial Class wfAutoTransText
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
        Dim PreFixText As String = Request.QueryString("q")
        Dim sb As StringBuilder = New StringBuilder
        Dim mTransTypeID As Integer = CType(Request.QueryString("TransTypeID"), Integer)
        Dim mToDate As String = Request.QueryString("ToDate")
        Dim TextType As String = Request.QueryString("TextType")

        If ((mTransTypeID = 14 Or mTransTypeID = 44) And TextType = "18") Then
            TextType = "18"
        Else
            TextType = ""
        End If

        mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(PreFixText, TextType, True, mTransTypeID, mToDate)
        For i As Integer = 0 To mDistinctTextAutoComplete.Count - 1
            sb.Append(mDistinctTextAutoComplete.Item(i).Text).Append(Environment.NewLine)
        Next
        Response.Write(sb.ToString)

    End Sub

End Class
