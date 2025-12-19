'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Web.Http


Public Class ModelController
	Inherits ApiController

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetModelList(ItemID As Guid,
								 Optional IsSelectTagRequired As Boolean = False) As ModelList

		Try

			Return ModelList.GetModelList(ItemID:=ItemID,
										  IsSelectTagRequired:=IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetModelList(ItemID As Guid,
								 AddNone As Boolean,
								 Optional IsSelectTagRequired As Boolean = False) As ModelList

		Try

			Return ModelList.GetModelList(ItemID:=ItemID,
										  AddNone:=AddNone,
										  IsSelectTagRequired:=IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetAirframeModelList(Optional AddTopItem As String = "",
										 Optional MachineIDStr As String = "") As ModelList

		Try

			Return ModelList.GetAirframeModelList(AddTopItem:=AddTopItem,
												  MachineIDStr:=MachineIDStr)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetValue(id As Integer) As String

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
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

	Public Sub PutValue(id As Integer, <FromBody()> value As String)

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
