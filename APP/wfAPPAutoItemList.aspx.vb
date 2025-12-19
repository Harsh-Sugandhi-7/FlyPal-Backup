Imports System.Text
Public Class wfAPPAutoItemList
    Inherits System.Web.UI.Page

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