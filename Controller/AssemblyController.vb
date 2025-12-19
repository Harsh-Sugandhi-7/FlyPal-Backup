'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Web.Http
Imports System.Web.Script.Services


Public Class AssemblyController
	Inherits ApiController

#Region " Get Method(s) "

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetAssemblyParameterListForAssemblyStatus(LogDate As String,
															  MachineID As String,
															  Optional AddTopItem As String = "") As AssemblyParameterListForAssemblyStatus

		Try

			Return AssemblyParameterListForAssemblyStatus.GetAssemblyParameterListForAssemblyStatus(LogDate:=LogDate,
																									MachineID:=New Guid(MachineID),
																									AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetAssemblyListForComboBox(AssemblyTypeID As Integer,
											   Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
											   Optional InstalledOn As String = "",
											   Optional AddTopItem As String = "",
											   Optional IsInstalled As Boolean = False,
											   Optional SkipIsForInventoryAircraft As Boolean = False,
											   Optional Username As String = "",
											   Optional SkipReadOnlyAircrafts As Boolean = False,
											   Optional IsForSpareAssembly As Boolean = False,
											   Optional ShowEngineAndAPUOnly As Boolean = False) As AssemblyList

		Try

			Return AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=AssemblyTypeID,
														   MachineID:=MachineID,
														   InstalledOn:=InstalledOn,
														   AddTopItem:=AddTopItem,
														   IsInstalled:=IsInstalled,
														   SkipIsForInventoryAircarft:=SkipIsForInventoryAircraft,
														   Username:=Username,
														   SkipReadOnlyAircrafts:=SkipIsForInventoryAircraft,
														   IsForSpareAssembly:=IsForSpareAssembly,
														   ShowEngineAndAPUOnly:=ShowEngineAndAPUOnly)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub PostValue(<FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub DeleteValue(id As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
