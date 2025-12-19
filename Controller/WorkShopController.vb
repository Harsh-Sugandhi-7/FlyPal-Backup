Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class WorkShopController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " GET Method(s) "

	Public Function GetValues(LookInType As Integer,
							  Optional WorkShopName As String = "",
							  Optional LocationID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional IsSelectTagRequired As Boolean = False,
							  Optional TagText As String = "(SELECT)") As WorkShopList

		Return WorkShopList.GetWorkShopList(LookInType,
											WorkShopName,
											LocationID,
											IsSelectTagRequired,
											TagText)

	End Function

	Public Function GetWorkShop(ID As Guid) As WorkShop
		Return WorkShop.GetWorkShop(ID)
	End Function

#End Region

#Region " Methods "

	Public Sub SetWorkShop(jsonObject As JObject, Optional mWorkShop As WorkShop = Nothing)

		With mWorkShop

			.Name = jsonObject("mName").ToString()
			.LocationID = New Guid(jsonObject("mLocationID").ToString())
			.LocationName = jsonObject("mLocationName").ToString()

		End With

	End Sub

#End Region

#Region " POST Method(s) "

	Public Function SaveWorkShop(<FromBody()> value As Object) As IHttpActionResult

		Try

			Dim jsonObject As JObject = JObject.Parse(value.ToString())
			Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
			Dim ReturnString As String = ""

			If mIsNew Then
				ReturnString = SetNewWorkShop(jsonObject)
			Else
				ReturnString = SetExistingWorkShop(jsonObject)
			End If

			If ReturnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="WorkShop Saved Successfully!"))

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

	Private Function SetNewWorkShop(jsonObject As JObject) As String

		Try

			Dim mWorkShop As WorkShop = WorkShop.NewWorkShop(ID:=New Guid(jsonObject("mID").ToString()))

			SetWorkShop(jsonObject, mWorkShop)

			mWorkShop.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="WorkShop",
																						   ex:=ex)

			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingWorkShop(jsonObject As JObject) As String

		Try

			Dim mWorkShop As WorkShop = WorkShop.GetWorkShop(ID:=New Guid(jsonObject("mID").ToString()))

			SetWorkShop(jsonObject, mWorkShop)

			mWorkShop.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="WorkShop",
																						   ex:=ex)

			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " PUT Method(s) "

	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

#End Region

#Region " DELETE Method(s) "

	Public Function DeleteWorkShop(ID As Guid) As IHttpActionResult

		Try

			WorkShop.DeleteWorkShop(ID:=ID)

			Return Ok(New ReturnMessage("Success", "WorkShop Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="WorkShop",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
