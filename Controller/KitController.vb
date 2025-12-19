Imports System.Web.Http

Public Class KitController
	Inherits ApiController

#Region " Variable Declaration "

#End Region

#Region " GET Method(s) "

	<HttpGet>
	Public Function GetKitItems(ItemID As Guid) As KitItems

		Try

			Return KitItems.GetKitItems(ItemID:=ItemID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetValue(ID As Integer) As String

		Try

			Return "value"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function PostValue(<FromBody()> value As Object)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(ID As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function Delete()

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class
