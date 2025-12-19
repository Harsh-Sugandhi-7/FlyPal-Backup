Imports System.Web.Http

Public Class ItemStockStatusController
	Inherits ApiController

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetValues(Optional ItemName As String = "",
							  Optional ToDate As String = "1/1/3300",
							  Optional IsCalibrationOrder As Boolean = 0) As ItemStockStatusList

		Try

			Return ItemStockStatusList.GetItemStockStatusList(ItemName:=ItemName,
															  ToDate:=ToDate,
															  IsCalibrationOrder:=IsCalibrationOrder)

		Catch ex As Exception

			Throw ex.GetBaseException

		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Sub PostValue(<FromBody()> value As String)

		Try


		Catch ex As Exception

			Throw ex.GetBaseException

		End Try

	End Sub

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
	Public Sub DeleteValue(id As Integer)

		Try


		Catch ex As Exception

			Throw ex.GetBaseException

		End Try

	End Sub

#End Region

End Class
