Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq


Public Class CurrencyController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Get Method(s) "
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValues(Optional ByVal Name As String = "", Optional ByVal Symbol As String = "",
							  Optional ByVal IsSelectTagRequired As Boolean = False) As CurrencyList
		Return CurrencyList.GetCurrencyList(Name, Symbol, IsSelectTagRequired)
	End Function

	' GET api/<controller>/5
	Public Function GetValue(ByVal ID As Guid) As Currency
		Return Currency.GetCurrency(ID:=ID)
	End Function
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetBaseCurrency() As Currency
		Return Currency.GetBaseCurrency()
	End Function
#End Region

#Region " Post Method(s) "
	' POST api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function SaveCurrency(<FromBody()> values As Object) As IHttpActionResult
		Try
			Dim jsonObject As JObject = JObject.Parse(values.ToString)
			Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
			Dim returnstring As String
			If mIsNew Then
				returnstring = SetNewCurrencyValues(jsonObject)
			Else
				returnstring = setExistingCurrencyValues(jsonObject)
			End If
			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "Currency saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring)
			'End If
			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Currency Saved Successfully!"))

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
	Private Function SetNewCurrencyValues(ByVal jsonObject As JObject) As String
		' Logic to set new currency values
		Try
			Dim mCurrency As Currency = Currency.NewCurrency(ID:=New Guid(jsonObject(propertyName:="mID").ToString))
			With mCurrency
				.Name = jsonObject(propertyName:="mName").ToString()
				.Symbol = jsonObject(propertyName:="mSymbol").ToString()
				.ConversionFactor = CDec(jsonObject(propertyName:="mConversionFactor").ToString())
				.NameAfterDecimal = jsonObject(propertyName:="mNameAfterDecimal").ToString
			End With
			mCurrency.Save()
			Return "Success"
		Catch ex As Exception
			Return ex.Message
		End Try
	End Function
	Private Function setExistingCurrencyValues(ByVal jsonObject As JObject) As String
		' Logic to set existing currency values
		Try
			Dim mCurrency As Currency = Currency.GetCurrency(ID:=New Guid(jsonObject(propertyName:="mID").ToString))
			With mCurrency
				.Name = jsonObject(propertyName:="mName").ToString()
				.Symbol = jsonObject(propertyName:="mSymbol").ToString()
				.ConversionFactor = CDec(jsonObject(propertyName:="mConversionFactor").ToString())
				.NameAfterDecimal = jsonObject(propertyName:="mNameAfterDecimal").ToString
			End With
			mCurrency.Save()
			Return "Success"
		Catch ex As Exception
			Return ex.Message
		End Try
	End Function
#End Region

#Region " Put Method(s) "
	' PUT api/<controller>/5
	Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

	End Sub
#End Region

#Region " Delete Method(s) "

	Public Function DeleteCurrency(ID As Guid) As IHttpActionResult

		Try

			Dim _Currency As Currency = Currency.GetCurrency(ID:=ID)
			_Currency.DeleteCurrency(ID:=ID)

			Return Ok(New ReturnMessage("Success", "Currency Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Currency",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
