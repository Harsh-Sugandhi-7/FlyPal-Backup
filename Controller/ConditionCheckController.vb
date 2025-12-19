Imports System.Web.Http
Imports System.Web.Script.Services

Public Class ConditionCheckController
	Inherits ApiController

	' GET api/<controller>
	<HttpGet>
	<CLSCompliant(False)>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetConditionCheckItemChildList(Optional FromDate As String = "1/1/1753",
												   Optional ToDate As String = "1/1/2200",
												   Optional ItemName As String = "",
												   Optional Description As String = "",
												   Optional SerialNo As String = "",
												   Optional IsConditionCheckServicedInspected As Integer = 0,
												   Optional ItemServiceInspectionsID As String = "{00000000-0000-0000-0000-000000000000}",
												   Optional ServiceInspectionsID As String = "{00000000-0000-0000-0000-000000000000}") As ConditionCheckItemChildList

		Try

			Return ConditionCheckItemChildList.GetConditionCheckItemChildList(FromDate:=FromDate,
																			  ToDate:=ToDate,
																			  ItemName:=ItemName,
																			  Description:=Description,
																			  SerialNo:=SerialNo,
																			  IsConditionCheckServicedInspected:=IsConditionCheckServicedInspected,
																			  ItemServiceInspectionsID:=ItemServiceInspectionsID,
																			  ServiceInspectionsID:=ServiceInspectionsID)

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<HttpGet>
	<CLSCompliant(False)>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetCalibrationItemChild(ConditionCheckItemID As String) As ConditionCheckItemChild

		Try

			Return ConditionCheckItemChild.GetConditionCheckItemChild(ConditionCheckItemID:=New Guid(ConditionCheckItemID))

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<HttpGet>
	<CLSCompliant(False)>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetCalibrationItem(ID As String) As ConditionCheckItem

		Try

			Return ConditionCheckItem.GetConditionCheckItem(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<HttpGet>
	<CLSCompliant(False)>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function NewComplyCalibrationItemChild(ConditionCheckItemID As Guid,
												  DoneOnDate As String,
												  PreviousConditionCheckItemChildID As Guid,
												  Optional IsComply As Boolean = True) As ConditionCheckItemChild

		Try

			Return ConditionCheckItemChild.NewComplyConditionCheckItemChild(ConditionCheckItemID:=ConditionCheckItemID,
																			DoneOnDate:=New SmartDate(DoneOnDate, False),
																			PreviousConditionCheckItemChildID:=PreviousConditionCheckItemChildID,
																			IsComply:=IsComply)


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
