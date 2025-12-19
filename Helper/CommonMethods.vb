'************************************
'Created by:	Harsh Sugandhi
'Created on:	16th October 2025
'Created for:	To handle the Common methods required in RCI.
'************************************


Public Class CommonMethods

#Region " Method(s) "

	Public Function HtmlEncode(InputString As String) As String

		If String.IsNullOrEmpty(InputString) Then Return ""
		Try

			Return InputString.Replace("&", "&amp;").
							   Replace("<", "&lt;").
							   Replace(">", "&gt;").
							   Replace("""", "&quot;").
							   Replace("'", "&#39;")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function NullIfEmpty(Input As String) As String

		Try

			Return If(String.IsNullOrWhiteSpace(Input), Nothing, Input)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class


