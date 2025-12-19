'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq


Public Class EmployeeController
	Inherits ApiController

#Region " Variable(s) "

	Private _MessageBox As New MSGBox
	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Get Method(s) "


	<HttpGet>
	<Route("api/Employee/GetPilotList")>
	<Route("api/Employee/GetEmployeeList")>
	Public Function GetEmployeeList(Optional Name As String = "",
									Optional Designation As String = "",
									Optional AddTopItem As String = "",
									Optional EmpNo As String = "",
									Optional Contractor As String = "",
									Optional IsUseInLogRequired As Boolean = False,
									Optional IsLicenceNoNamePropertyRequired As Boolean = False,
									Optional Department As String = "",
									Optional IsEmployeeWorking As Integer = 2,
									Optional SkipNames As String = "",
									Optional IsContractedEmployee As Integer = 2,
									Optional IsTechnicalCrew As Boolean = False,
									Optional SkipTechnicalCrewAndFlyingCrew As Boolean = False,
									Optional ShowAllTechnicalCrewAndUnassigned As Boolean = False,
									Optional ShowAllFlyingCrewAndUnassigned As Boolean = False) As EmployeeList

		Try

			Return EmployeeList.GetEmployeeList(Name:=Name,
												Designation:=Designation,
												AddTopItem:=AddTopItem,
												EmpNo:=EmpNo,
												Contractor:=Contractor,
												IsUseInLogRequired:=IsUseInLogRequired,
												IsLicenceNoNamePropertyRequired:=IsLicenceNoNamePropertyRequired,
												Department:=Department,
												IsEmployeeWorking:=IsEmployeeWorking,
												SkipNames:=SkipNames,
												IsContractedEmployee:=IsContractedEmployee,
												IsTechnicalCrew:=IsTechnicalCrew,
												SkipTechnicalCrewAndFlyingCrew:=SkipTechnicalCrewAndFlyingCrew,
												ShowAllTechnicalCrewAndUnassigned:=ShowAllTechnicalCrewAndUnassigned,
												ShowAllFlyingCrewAndUnassigned:=ShowAllFlyingCrewAndUnassigned)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/Employee/GetPilot")>
	<Route("api/Employee/GetEmployee")>
	Public Function GetEmployee(ID As String) As Employee

		Try

			Return Employee.GetEmployee(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetEmployeeWorkingStatus(Optional PilotID As String = "{00000000-0000-0000-0000-000000000000}",
											 Optional CoPilotID As String = "{00000000-0000-0000-0000-000000000000}",
											 Optional LogDate As String = "1/1/1900") As ReturnMessage

		Dim _EmployeeStatus As EmployeeStatus
		Dim Message As String = ""

		Try

			If PilotID.ToString <> "{00000000-0000-0000-0000-000000000000}" Then

				_EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=PilotID.ToString,
																		  EDate:=LogDate.ToString)
			Else

				_EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=CoPilotID.ToString,
																		  EDate:=LogDate.ToString)

			End If

			If _EmployeeStatus(0).Information <> "" Then

				Message = "<b>Pilot in Command : </b><br/>" & _EmployeeStatus(0).Information.ToString.Replace("Resource", "")

				If CoPilotID.ToString <> "{00000000-0000-0000-0000-000000000000}" Then

					Message = IIf(Message.Length > 0, Message & "<br/>", "") & "<b>Co-Pilot : </b><br/>" & _EmployeeStatus(0).Information.ToString.Replace("Resource", "")

				End If

			End If

			Return New ReturnMessage("Success",
									 Message:=Message.Replace("<br/>", "").Replace("<b>", "").Replace("</b>", ""))

		Catch ex As Exception

			Return New ReturnMessage(Status:="Error",
									 Message:=$"{ex.GetBaseException}")

		End Try

	End Function

	<HttpGet>
	Public Function NewPilot() As Employee

		Try

			Return Employee.NewPilot

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function SavePilot(<FromBody()> value As Object) As IHttpActionResult

		Dim JObject As JObject = JObject.Parse(value.ToString)
		Dim _IsNew As Boolean = CBool(JObject("mIsNew"))
		Dim _Pilot As Employee
		Dim Status As String
		Dim returnMessage As String

		Try

			If _IsNew Then

				_Pilot = Employee.NewPilot()
				returnMessage = "New Pilot Added Successfully!"

			Else

				_Pilot = Employee.GetEmployee(ID:=New Guid(JObject("mID").ToString))
				returnMessage = "Pilot Saved Successfully!"

			End If

			Status = SetPilotDetails(_Pilot,
									 JObject)

			If Status = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:=returnMessage))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=returnMessage))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

	Private Function SetPilotDetails(_Pilot As Employee,
									 JObject As JObject) As String

		Try

			With _Pilot

				.Name = JObject("mName").ToString
				.EmpNo = JObject("mEmpNo").ToString
				.DesignationID = New Guid(JObject("mDesignationID").ToString)

			End With

			_Pilot.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Pilot",
																						   ex:=ex)

			Return returnMessage

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
	Public Function DeletePilot(ID As String) As IHttpActionResult

		Try

			Employee.DeleteEmployee(ID:=New Guid(ID))

			Return Ok(New ReturnMessage("Success", "Pilot Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Pilot",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class