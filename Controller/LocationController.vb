Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq

Public Class LocationController
	Inherits ApiController

#Region " Variable Declaration "

	Private _MessageBox As New MSGBox
	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " GET Method(s) "

	<HttpGet>
	Public Function GetValues(LookInType As Integer,
							  Optional Name As String = "",
							  Optional City As String = "",
							  Optional State As String = "",
							  Optional Country As String = "",
							  Optional ContactPerson As String = "",
							  Optional IsSelectTagRequired As Boolean = False) As LocationList

		Return LocationList.GetLocationList(LookInType,
											Name,
											City,
											State,
											Country,
											ContactPerson,
											IsSelectTagRequired)

	End Function

#End Region

#Region " POST Method(s) "

	<HttpPost>
	Public Function SaveLocation(<FromBody()> value As Object) As IHttpActionResult

		Dim jsonObject As JObject = JObject.Parse(value.ToString())
		Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
		Dim ReturnString As String = ""
		Try

			If mIsNew Then
				ReturnString = SetNewLocation(jsonObject)
			Else
				ReturnString = SetExistingLocation(jsonObject)
			End If

			If ReturnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Location Saved Successfully!"))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=ReturnString))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

#End Region

#Region " PUT Method(s) "

#End Region

#Region " DELETE Method(s) "

	<HttpDelete>
	Public Function DeleteLocation(ID As Guid) As IHttpActionResult

		Try

			StoreLocation.DeleteLocation(ID:=ID)

			Return Ok(New ReturnMessage("Success", "Location deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Location",
																						   ex:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

#Region " Helper Method(s) "

	Private Function SetNewLocation(jsonObject As JObject) As String

		Try

			Dim mStoreLocation As StoreLocation = StoreLocation.NewLocation(ID:=New Guid(jsonObject("mID").ToString()))
			SetLocation(jsonObject, mStoreLocation)
			mStoreLocation.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingLocation(jsonObject As JObject) As String

		Try

			Dim mStoreLocation As StoreLocation = StoreLocation.GetLocation(ID:=New Guid(jsonObject("mID").ToString()))
			SetLocation(jsonObject, mStoreLocation)
			mStoreLocation.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Public Sub SetLocation(jsonObject As JObject, Optional mStoreLocation As StoreLocation = Nothing)

		Try

			With mStoreLocation

				.Name = jsonObject("mName").ToString()
				.Address = jsonObject("mAddress").ToString()
				.CityID = New Guid(jsonObject("mCityID").ToString())
				.Phone1 = jsonObject("mPhone1").ToString()
				.Phone2 = jsonObject("mPhone2").ToString()
				.Phone3 = jsonObject("mPhone3").ToString()
				.Fax = jsonObject("mFax").ToString()
				.Email = jsonObject("mEmail").ToString()
				.ContactPerson = jsonObject("mContactPerson").ToString()

			End With

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
