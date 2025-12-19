Imports System.Web.Http
Imports System.Web.Script.Services

Public Class CalibrationController
	Inherits ApiController

	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetCalibrationChildList(Optional FromDate As String = "1/1/1753",
											Optional ToDate As String = "1/1/2200",
											Optional ItemName As String = "",
											Optional Description As String = "",
											Optional SerialNo As String = "",
											Optional ReceiptItemIDToBeSkipped As String = "{00000000-0000-0000-0000-000000000000}") As CalibrationItemChildList

		Try

			Return CalibrationItemChildList.GetCalibrationChildList(FromDate:=FromDate,
																	ToDate:=ToDate,
																	ItemName:=ItemName,
																	Description:=Description,
																	SerialNo:=SerialNo)

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetCalibrationItemChild(CalibrationItemID As String) As CalibrationItemChild

		Try

			Return CalibrationItemChild.GetCalibrationItemChild(CalibrationItemID:=New Guid(CalibrationItemID))

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetCalibrationItem(ID As String) As CalibrationItem

		Try

			Return CalibrationItem.GetCalibrationItem(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function NewComplyCalibrationItemChild(CalibrationItemID As Guid,
												  CalDoneOnDate As String,
												  PreviousCalibrationItemChildID As Guid,
												  Optional IsComply As Boolean = True,
												  Optional Frequency As Integer = 0,
												  Optional PeriodID As Integer = 0) As CalibrationItemChild

		Try

			Return CalibrationItemChild.NewComplyCalibrationItemChild(CalibrationItemID:=CalibrationItemID,
																	  CalDoneOnDate:=CalDoneOnDate,
																	  PreviousCalibrationItemChildID:=PreviousCalibrationItemChildID,
																	  IsComply:=IsComply,
																	  Frequency:=Frequency,
																	  PeriodID:=PeriodID)

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	' GET api/<controller>/5
	Public Function GetValue(id As Integer) As String
		Return "value"
	End Function

	' POST api/<controller>
	Public Sub PostValue(<FromBody()> value As String)

	End Sub

	' PUT api/<controller>/5
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

	' DELETE api/<controller>/5
	Public Sub DeleteValue(id As Integer)

	End Sub
End Class
