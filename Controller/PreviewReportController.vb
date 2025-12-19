Imports System.Web.Http


Public Class PreviewReportController
	Inherits ApiController


	Private _companyDetail As CompanyDetailForAPI

	<HttpGet>
	Public Function GetCompanyDetail() As CompanyDetailForAPI

		Try

			_companyDetail = CompanyDetailForAPI.GetCompanyDetail(CompanyName:="",
																  Address:="",
																  Tel1:="",
																  Tel2:="",
																  Fax:="",
																  Email:="",
																  WebSite:="",
																  CurrencyName:="",
																  CurrencySymbol:="")

			Return _companyDetail

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	Public Function GetValue(Id As Integer) As String
		Return "value"
	End Function

	Public Sub PostValue(<FromBody()> value As String)

	End Sub

	Public Sub PutValue(Id As Integer, <FromBody()> value As String)

	End Sub

	Public Sub DeleteValue(Id As Integer)

	End Sub

End Class
