'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Web.Http


Public Class RegisterController
	Inherits ApiController


#Region " Get Method(s) "

	<HttpGet>
	Public Function GetRectifiedLog(StartDate As String,
									EndDate As String,
									Optional AssemblyID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional CalculateTotal As Boolean = True,
									Optional FlightLogClassificationName As String = " ",
									Optional StatusSelectLog As Integer = 0,
									Optional IsLogNo As Boolean = False,
									Optional IsLogPageNo As Boolean = False,
									Optional IsFlightNo As Boolean = False,
									Optional AddTopItem As String = "",
									Optional IsFromMEL As Boolean = False,
									Optional LogID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional SkipVoidLog As Boolean = False,
									Optional SkipMaintenanceLog As Boolean = False,
									Optional IsFlightLogClassification As Boolean = False) As ReportLogRegister

		Try

			Return ReportLogRegister.GetRectifiedLog(StartDate:=StartDate,
													 EndDate:=EndDate,
													 AssemblyID:=AssemblyID,
													 MachineID:=MachineID,
													 CalculateTotal:=CalculateTotal,
													 FlightLogClassificationName:=FlightLogClassificationName,
													 StatusSelectLog:=StatusSelectLog,
													 IsLogNo:=IsLogNo,
													 IsLogPageNo:=IsLogPageNo,
													 IsFlightNo:=IsFlightNo,
													 AddTopItem:=AddTopItem,
													 IsFromMEL:=IsFromMEL,
													 LogID:=LogID,
													 SkipVoidLog:=SkipVoidLog,
													 SkipMaintLog:=SkipMaintenanceLog,
													 IsFlightLogClassification:=IsFlightLogClassification)

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