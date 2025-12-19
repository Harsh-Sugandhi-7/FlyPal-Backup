'************************************
'Added By Utkarsh On 22-Aug-2011: AutoComplete Textbox
'Modified by Harsh Sugandhi on 26th May 2025 for FlyPaL-2439.
'************************************

Imports System.Text


Partial Class wfAutoPilotPlace
    Inherits Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(sender As System.Object, e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Dim PilotList As PilotListAutoComplete
		Dim PlaceList As PlaceListAutoComplete

		Dim prefixText As String = Request.QueryString("q")
        Dim Type As String = Request.QueryString("Type")
        Dim sb As New StringBuilder

        Try

			If Type = "Pilot" Then  'Pilot List

				PilotList = PilotListAutoComplete.GetPilotList(prefixText)

				For i As Integer = 0 To PilotList.Count - 1
					sb.Append(PilotList.Item(i).Name).Append(Environment.NewLine)
				Next

			ElseIf Type = "Place" Then  'Place List

				PlaceList = PlaceListAutoComplete.GetPlaceList(prefixText)

				For j As Integer = 0 To PlaceList.Count - 1
					sb.Append(PlaceList.Item(j).Place).Append(Environment.NewLine) 'Changed By Utkarsh On 24-Nov-2011 For ALL23112011
				Next

			End If

			Response.Write(sb.ToString)

		Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

End Class
