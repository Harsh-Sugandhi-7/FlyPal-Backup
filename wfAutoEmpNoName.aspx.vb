Imports System.Text

Public Class wfAutoEmpNoName
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim mEmpNoNameList As EmpNoNameAutoComplete
        Dim prefixText As String = Request.QueryString("q")
        Dim ExludeUseInLogRequried As Boolean = False
        'Added by Utkarsh on 20-Jan-2014 FOR ALL20012014
        If Not String.IsNullOrEmpty(Request.QueryString("ExludeUseInLogRequried")) Then
            ExludeUseInLogRequried = CBool(Request.QueryString("ExludeUseInLogRequried"))
        End If
        'End
        Dim sb As StringBuilder = New StringBuilder

        mEmpNoNameList = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText, User.Identity.Name, ExludeUseInLogRequried)
        For i As Integer = 0 To mEmpNoNameList.Count - 1
            sb.Append(mEmpNoNameList.Item(i).EmpNoName).Append(Environment.NewLine)
        Next
        Response.Write(sb.ToString)
    End Sub

End Class