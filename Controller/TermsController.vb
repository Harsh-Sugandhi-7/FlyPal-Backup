Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq

Public Class TermsController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _MessageBox As New MSGBox

#End Region

#Region " Get Method(s) "

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function TermList(Optional Terms As String = "",
							 Optional Type As Integer = 1) As TermList

		Try

			Return TermList.GetTermList(Terms:=Terms,
										Type:=Type)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try


	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	<Route("api/Terms/RFQVendorTerms")>
	<Route("api/Terms/GetValues")>
	Public Function GetValues(ID As String,
							  Type As Integer) As Terms

		Try

			Return Terms.GetTerms(ID:=New Guid(ID),
								  Type:=Type)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function GetTerm(ID As Guid,
							Type As Integer,
							Optional UseAsChild As Boolean = False) As Term
		Try

			Return Term.GetTerm(ID:=ID,
								Type:=Type,
								UseAsChild:=UseAsChild)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function GetVendorTerms(VendorID As Guid,
								   TransTypeID As Integer,
								   Optional ID As String = "{00000000-0000-0000-0000-000000000000}",
								   Optional Type As Integer = 0) As VendorTerms
		Try

			Return VendorTerms.GetVendorTerms(VendorID:=VendorID,
											  TransTypeID:=TransTypeID,
											  ID:=ID,
											  Type:=Type)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function SaveTerms(<FromBody()> values As Object) As IHttpActionResult

		Dim jsonObject As JObject = JObject.Parse(values.ToString)
		Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
		Dim returnstring As String

		Try

			If mIsNew Then
				returnstring = SetNewTermsValues(jsonObject)
			Else
				returnstring = SetExistingTermsValues(jsonObject)
			End If

			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "Term saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring)
			'End If

			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Term Saved Successfully!"))

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

	Private Function SetNewTermsValues(jsonObject As JObject) As String

		' Logic to set new terms values
		Try

			Dim mTerm As Term = Term.NewTerm(ID:=New Guid(jsonObject(propertyName:="mID").ToString),
											 Type:=CInt(jsonObject(propertyName:="mType").ToString))
			With mTerm

				.Terms = jsonObject(propertyName:="mTerms").ToString()
				.Type = CInt(jsonObject(propertyName:="mType"))

			End With

			mTerm.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingTermsValues(jsonObject As JObject) As String

		' Logic to set existing terms values
		Try

			Dim mTerm As Term = Term.GetTerm(ID:=New Guid(jsonObject(propertyName:="mID").ToString),
											 Type:=CInt(jsonObject(propertyName:="mType").ToString))
			With mTerm

				.Terms = jsonObject(propertyName:="mTerms").ToString()
				.Type = CInt(jsonObject(propertyName:="mType"))

			End With

			mTerm.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " Put Method(s) "

	' PUT api/<controller>/5
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteTerm(ID As String,
							   Type As Integer) As IHttpActionResult

		Dim Term As Term
		Try

			Term = Term.GetTerm(ID:=New Guid(ID),
								Type:=Type)

			Term.Delete()

			Return Ok(New ReturnMessage("Success", "Term Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Term",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
