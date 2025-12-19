'************************************
'Created by:	Prashant Potpite
'Created on:	18-Dec-2024
'Created for:	To restrict the creation of Transaction after Subscription Date has Expired.
'************************************


Public Class CheckForSubscriptionExpired

#Region " Helper Method(s) "

	Public Function CheckForSubscriptionExpired(TransactionDate As Date,
												Optional ModuleName As String = "") As String

		Try

			Dim ReturnMessage As String = ""
			Dim BaseDirectory As String = AppDomain.CurrentDomain.BaseDirectory
			Dim FilePath As String = Path.Combine(BaseDirectory, "bin\Authority.xml")
			Dim Authentication As New Authenticate.CheckAuthentication(True, FilePath)
			Dim Prefix As String = $"{IIf(ModuleName = "", "Transaction", ModuleName)}"

			If Authentication.WebAuthentication Then

				Dim Days As Integer = Authentication.Number("Days")
				Dim MaxAllowableDate As DateTime = DateAdd(DateInterval.Day, Days, Authentication.SubscriptionDate)

				If DateDiff(DateInterval.Day, TransactionDate, MaxAllowableDate) < 0 Then
					ReturnMessage = $"Your Subscription has been Expired. Cannot Save {Prefix}. 
									  {Prefix} Date cannot be Greater than {MaxAllowableDate.ToString(WebDateFormat)}"
				End If

			End If

			If String.IsNullOrEmpty(ReturnMessage) Then
				Return "Success"
			Else
				Return ReturnMessage
			End If

		Catch ex As Exception
			Return ex.GetBaseException.Message
		End Try

	End Function

#End Region

End Class
