Public Class BackPage

	Public Shared Sub Push(ByRef session As Object, ByVal url As String)
		Dim s As Stack
		If session Is Nothing Then
			s = New Stack
		Else
			s = session
		End If
		s.Push(url)
		session = s
	End Sub
	Public Shared Function Pop(ByRef session As Object) As String
		Dim s As Stack = session
		Dim url As String
		url = CType(s.Pop, String)
		session = s
		Return url
	End Function

End Class
