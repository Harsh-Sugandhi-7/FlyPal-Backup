Imports System.Security.Cryptography
Imports System.Text


Public Class TokenUtility

	Private Shared ReadOnly secret As String = "I am not in danger, Skyler. I am the danger."

	Public Shared Function CreateToken(userKey As String) As String

		Dim payload = userKey & "|" & DateTime.UtcNow.Ticks
		Dim signature = Sign(payload)

		Return Convert.ToBase64String(Encoding.UTF8.GetBytes(payload & "|" & signature))

	End Function

	Public Shared Function ValidateToken(token As String) As String

		Try

			Dim decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token))
			Dim parts = decoded.Split("|"c)
			If parts.Length <> 3 Then Return Nothing

			Dim payload = parts(0) & "|" & parts(1)
			Dim signature = parts(2)

			If Sign(payload) <> signature Then Return Nothing

			Return parts(0) ' this is userKey

		Catch
			Return Nothing
		End Try

	End Function

	Private Shared Function Sign(data As String) As String

		Using h = New HMACSHA256(Encoding.UTF8.GetBytes(secret))
			Return Convert.ToBase64String(h.ComputeHash(Encoding.UTF8.GetBytes(data)))
		End Using

	End Function

End Class
