
'Created By Utkarsh On 23-Apr-2012 For ALL23042012 

Imports System.Text

Partial Class wfAutoPartList
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
        Dim Partlist As PartListAutoComplete
        Dim prefixText As String = Request.QueryString("q")
        Dim sb As StringBuilder = New StringBuilder
        Dim IsByModel As Boolean = CType(Request.QueryString("IsByModel"), Boolean)

        If Not IsByModel Then
            Partlist = PartListAutoComplete.GetPartList(prefixText)
        Else
            Dim ModelID As String = Request.QueryString("ModelID")
            Partlist = PartListAutoComplete.GetPartList(prefixText, ModelID)
        End If

        For i As Integer = 0 To Partlist.Count - 1
            sb.Append(Partlist(i).Name).Append(Environment.NewLine)
        Next
        Response.Write(sb.ToString)
    End Sub

End Class
