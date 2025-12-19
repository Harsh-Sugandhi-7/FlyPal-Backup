'Created BY: Saylee


Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class FlightLogClassificationController
	Inherits ApiController

#Region " Variable(s) "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetFlightLogClassificationList(Optional Name As String = "",
												   Optional AddTopItem As String = "") As FlightLogClassificationList

		Try

			Return FlightLogClassificationList.GetFlightLogClassificationList(Name:=Name,
																			  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetFlightLogClassification(ID As String) As FlightLogClassification

		Try

			Return FlightLogClassification.GetFlightLogClassification(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewFlightLogClassification() As FlightLogClassification

		Try

			Return FlightLogClassification.NewFlightLogClassification(ID:=Guid.NewGuid)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(S) "

	<HttpPost>
	Public Function SaveFlightLogClassification(<FromBody()> value As Object) As IHttpActionResult

		Try

			Dim jsonObject As JObject = JObject.Parse(value.ToString)
			Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
			Dim returnstring As String

			returnstring = SetFlightLogClassification(jsonObject, mIsNew)

			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "Flight Log Classification saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring.Replace("<p>", "").Replace("</p>", "").Replace("<strong>", "").Replace("</strong>", ""))
			'End If

			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Flight Log Classification Saved Successfully!"))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=returnstring))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteFlightLogClassification(ID As String) As IHttpActionResult

		Try

			FlightLogClassification.DeleteFlightLogClassification(ID:=New Guid(ID))

			Return Ok(New ReturnMessage("Success", "Flight Log Classification Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="FlightLogClassification",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

#Region " Set Method(s) "

	Public Function SetFlightLogClassification(jsonObject As JObject, IsNew As Boolean) As String

		Try

			Dim mFlightLogClassification As FlightLogClassification

			If IsNew Then
				mFlightLogClassification = FlightLogClassification.NewFlightLogClassification(ID:=Guid.NewGuid)
			Else
				mFlightLogClassification = FlightLogClassification.GetFlightLogClassification(ID:=New Guid(jsonObject("mID").ToString))
			End If

			mFlightLogClassification.Name = jsonObject(propertyName:="mName")

			mFlightLogClassification.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="FlightLogClassification",
																						   ex:=ex)

			Return returnMessage

		End Try

	End Function

#End Region

End Class
