Imports System.Text

Partial Class wfAutoItemList
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
        Dim mItemList As ItemListAutoComplete
        Dim prefixText As String = Request.QueryString("q")
        Dim sb As StringBuilder = New StringBuilder
        Dim mIsSerialisedPartsList As String = Request.QueryString("IsSerialisedPartsList")
        Dim PartsWithAlternatePartsOnly As String = Request.QueryString("PartsWithAlternatePartsOnly") 'Added by Vikrant On 11-Jul-2019 For ALL11072019
        'ItemList
        If mIsSerialisedPartsList = "True" Then
            mItemList = ItemListAutoComplete.GetItemList(prefixText, True)
        ElseIf PartsWithAlternatePartsOnly = "True" Then 'Added by Vikrant On 11-Jul-2019 For ALL11072019
            mItemList = ItemListAutoComplete.GetItemList(prefixText, PartsWithAlternatePartsOnly:=PartsWithAlternatePartsOnly)
        Else
            mItemList = ItemListAutoComplete.GetItemList(prefixText, False)
        End If

        For i As Integer = 0 To mItemList.Count - 1
            sb.Append(mItemList.Item(i).Item).Append(Environment.NewLine)
        Next
        Response.Write(sb.ToString)
    End Sub

End Class
