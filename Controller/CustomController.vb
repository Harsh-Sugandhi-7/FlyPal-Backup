Imports System.Web.Http


<Route("api/[controller]")>
Public Class CustomController
	Inherits ApiController

	' GET: api/custommethod/getcustom
	<HttpGet>
	<Route("getcustom")>
	Public Function GetCustom() As IHttpActionResult
		Return Ok(New With {.message = "This is a custom-named GET method."})
	End Function

	' GET: api/custommethod/getcustombyid/5
	<HttpGet>
	<Route("getcustombyid/{id}")>
	Public Function GetCustomById(id As Integer) As IHttpActionResult
		Return Ok(New With {.message = $"This is a custom-named GET method with ID {id}."})
	End Function


	' GET: api/custommethod/getcustombyid1/5
	<HttpGet>
	<Route("getcustombyid1/{id}")>
	Public Function GetCustomById1(id As Integer, id1 As Integer) As IHttpActionResult
		Return Ok(New With {.message = $"This is a custom-named GET method with 1/ 2  ID {id} / {id1}."})
	End Function

	' POST: api/custommethod/postcustom
	<HttpPost>
	<Route("postcustom")>
	Public Function PostCustom(<FromBody> value As String) As IHttpActionResult
		Return Ok(New With {.message = "This is a custom-named POST method.", .value = value})
	End Function

	' PUT: api/custommethod/putcustom/5
	<HttpPut>
	<Route("putcustom/{id}")>
	Public Function PutCustom(id As Integer, <FromBody> value As String) As IHttpActionResult
		Return Ok(New With {.message = $"This is a custom-named PUT method with ID {id}.", .value = value})
	End Function

	' DELETE: api/custommethod/deletecustom/5
	<HttpDelete>
	<Route("deletecustom/{id}")>
	Public Function DeleteCustom(id As Integer) As IHttpActionResult
		Return Ok(New With {.message = $"This is a custom-named DELETE method with ID {id}."})
	End Function

End Class


