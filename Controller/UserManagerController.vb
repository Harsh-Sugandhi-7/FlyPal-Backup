'*************************************
'Created by: Harsh Sugandhi
'Created on: 11th October 2024
'Created for: FLYPAL-1965 API Creation for given list of Methods
'*************************************

Imports System.Collections.Generic
Imports System.Web.Http

Public Class UserManagerController
	Inherits ApiController

	Public Function GetValues() As IEnumerable(Of String)
		Return New String() {"value1", "value2"}
	End Function

	Public Function GetCurrencyRightsBeforeSave(CurrencyID As Guid) As Boolean

		Try

			Dim mUser = FetchUser()

			If (mUser.IsCurrencywisePOLimit = True And
				mUser.UserCurrencywisePOLimits.Count > 0) Then

				If mUser.UserCurrencywisePOLimits.Contains(mIsApplicable:=True, mCurrencyID:=CurrencyID) = False Then

					Return False

				End If

			End If

			Return True

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function

	Public Function GetCurrencyRightsAndLimitBeforeAutorized(CurrencyID As Guid,
															 CGrandTotal As Decimal) As Boolean

		Try

			Dim mUser = FetchUser()

			If (mUser.IsCurrencywisePOLimit = True And
				mUser.UserCurrencywisePOLimits.Count > 0) Then

				If mUser.UserCurrencywisePOLimits.Contains(mIsApplicable:=True, mCurrencyID:=CurrencyID) = False Then

					Return False

				End If


				If (mUser.UserCurrencywisePOLimits.Item(CurrencyID).Limit > 0 And
					CGrandTotal > mUser.UserCurrencywisePOLimits.Item(CurrencyID).Limit) Then

					Return False

				End If

			End If

			Return True

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function

	Public Function GetLimitAfterAutorized(StatusID As Integer,
										   CurrencyID As Guid,
										   CGrandTotal As Decimal) As Boolean

		Try

			Dim mUser = FetchUser()

			If (mUser.IsCurrencywisePOLimit = True And
				mUser.UserCurrencywisePOLimits.Count > 0) Then

				If StatusID = 2 Then

					If (mUser.UserCurrencywisePOLimits.Item(CurrencyID).Limit > 0 And
						CGrandTotal > mUser.UserCurrencywisePOLimits.Item(CurrencyID).Limit) Then

						Return False

					End If

				End If

			End If

			Return True

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function

	Public Function GetUserHasNoStoreRights(Optional ByVal UserName As String = "",
										   Optional ByVal StoreID As String = "{00000000-0000-0000-0000-000000000000}") As UserHasNoStoreRights
		Return UserHasNoStoreRights.GetUserHasNoStoreRights(UserName:=UserName, StoreID:=StoreID)
	End Function

	Public Shared Function FetchUser() As User

		Try

			HttpContext.Current.User = Thread.CurrentPrincipal
			Dim mUser As User = SI.UTILITY.User.GetUser(HttpContext.Current.User.Identity.Name.ToString)

			Return mUser

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function

	Public Sub PostValue(<FromBody()> value As String)

	End Sub

	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

	Public Sub DeleteValue(id As Integer)

	End Sub

End Class
