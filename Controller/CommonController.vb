Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Public Class CommonController
	Inherits ApiController

#Region " Varriable(s) "

	Private _ModuleHelper As New ModuleHelper

#End Region

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetValue(ID As Integer) As String
		Return "value"
	End Function

	<HttpGet>
	<Route("api/Common/GetDistinctTextListForEnquiry")>
	<Route("api/Common/GetDistinctTextList")>
	Public Function GetDistinctTextList([Of] As String,
										Optional WithType As Integer = 0,
										Optional IsSelectTagRequired As Boolean = False,
										Optional Tag As String = "(SELECT)",
										Optional TransTypeID As Integer = 0) As DistinctTextList

		Try

			Return DistinctTextList.GetDistinctTextList([Of]:=[Of],
													WithType:=WithType,
													IsSelectTagRequired:=IsSelectTagRequired,
													Tag:=Tag,
													TransTypeID:=TransTypeID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetDistinctWOText(Optional AddTopItem As String = "",
									  Optional TransTypeID As Integer = 0) As nDistinctWOText

		Return nDistinctWOText.GetDistinctWOText(AddTopItem:=AddTopItem,
												 TransTypeID:=TransTypeID)
	End Function

	<HttpGet>
	<Route("api/Common/GetModuleName")>
	<Route("api/Common/GetModuleNameForEventLog")>
	Public Function GetModuleNameForEventLog(TransTypeID As Integer) As IHttpActionResult

		Try

			Dim ModuleName As String = _ModuleHelper.GetModuleName(TransTypeID:=TransTypeID)

			Return Ok(ModuleName)

		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError, ex.GetBaseException)
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	Public Sub PostValue(<FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	Public Sub PutValue(ID As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	Public Sub DeleteValue(id As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
