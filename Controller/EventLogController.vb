'*************************************
'Created by:  Harsh Sugandhi
'Created on:  14th October 2024
'Created for: FLYPAL-1965 Controller for Adding MarkLog from New Application
'*************************************


Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq


Public Class EventLogController
	Inherits ApiController


#Region " Get Method(s) "

	<HttpGet>
	Public Function GetEventLogDetailList(Optional Machine As String = "",
										  Optional UserName As String = "",
										  Optional Password As String = "",
										  Optional IPAddress As String = "",
										  Optional ModuleName As String = "",
										  Optional ToTime As String = "00:00",
										  Optional LoginStatusID As Short = 0,
										  Optional FromTime As String = "00:00",
										  Optional ToDate As String = "1-1-2200",
										  Optional FromDate As String = "1-1-1900",
										  Optional ActionID As Action = Action.Save,
										  Optional ByBTPLAdminUser As Boolean = False,
										  Optional ErrorTypeID As ErrorType = ErrorType.NoError,
										  Optional EventLogID As String = "{00000000-0000-0000-0000-000000000000}",
										  Optional EventLogDetailID As String = "{00000000-0000-0000-0000-000000000000}")

		Try

			Return EventLogDetailList.GetEventLogDetailList(ToDate:=ToDate,
															ToTime:=ToTime,
															Machine:=Machine,
															FromTime:=FromTime,
															ActionID:=ActionID,
															UserName:=UserName,
															Password:=Password,
															FromDate:=FromDate,
															IPAddress:=IPAddress,
															ModuleName:=ModuleName,
															EventLogID:=EventLogID,
															ErrorTypeID:=ErrorTypeID,
															LoginStatusID:=LoginStatusID,
															ByBTPLAdminUser:=ByBTPLAdminUser,
															EventLogDetailID:=EventLogDetailID)

		Catch ex As Exception
			ex.GetBaseException()
			Return ""
		End Try

	End Function

	<HttpGet>
	Public Function GetEventLogList(Optional ActionID As Action = 0,
									Optional Machine As String = "",
									Optional UserName As String = "",
									Optional Password As String = "",
									Optional IPAddress As String = "",
									Optional ModuleName As String = "",
									Optional ToTime As String = "00:00",
									Optional FromTime As String = "00:00",
									Optional ToDate As String = "1-1-2200",
									Optional FromDate As String = "1-1-1900",
									Optional ByBTPLAdminUser As Boolean = False,
									Optional LoginStatusID As LoginStatus = LoginStatus.Success,
									Optional ID As String = "{00000000-0000-0000-0000-000000000000}")

		Try

			Return EventLogList.GetEventLogList(ID:=ID,
												ToDate:=ToDate,
												ToTime:=ToTime,
												Machine:=Machine,
												FromDate:=FromDate,
												FromTime:=FromTime,
												UserName:=UserName,
												ActionID:=ActionID,
												Password:=Password,
												IPAddress:=IPAddress,
												ModuleName:=ModuleName,
												LoginStatusID:=LoginStatusID,
												ByBTPLAdminUser:=ByBTPLAdminUser)

		Catch ex As Exception
			ex.GetBaseException()
			Return ""
		End Try

	End Function

	<HttpGet>
	Public Function GetEventLog(EventLogID As Guid)

		Try

			Return EventLog.GetEventLog(ID:=EventLogID)

		Catch ex As Exception
			ex.GetBaseException()
			Return ""
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function PostLoginStatus(Optional Machine As String = "",
									Optional IPAddress As String = "",
									Optional IsAuthenticated As Boolean = True) As IHttpActionResult
		Try

			Dim User As User = UserManagerController.FetchUser()
			Dim Username = User.Name
			Dim DBPassword = User.DBPassword

			Dim EventLogID As Guid = MarkLog(Machine:=Machine,
											 UserName:=Username,
											 Action:=Action.Login,
											 Password:=DBPassword,
											 IPAddress:=IPAddress,
											 IsAuthenticated:=IsAuthenticated)


			Return Ok(New ReturnMessage(Status:="Success",
											   Message:="Event-Log added Successfully!",
											   EventLogID:=EventLogID.ToString))

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

	<HttpPost>
	Public Function PostMarkLog(TransID As Guid,
								Action As Action,
								Detail As String,
								EventLogID As Guid,
								ModuleName As String,
								ErrorType As ErrorType,
								<FromBody> JSONPayload As Object,
								Optional ModuleNameforGettingID As String = "") As IHttpActionResult

		Dim JSONPayloadString As String

		Try

			If Detail Is Nothing Then
				Detail = ""
			End If

			If String.IsNullOrEmpty(ModuleName) OrElse
			   TransID = Guid.Empty OrElse
			   EventLogID = Guid.Empty Then

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:="Mandatory fields are missing or Invalid."))

			End If

			If TypeOf JSONPayload Is String OrElse
			   TypeOf JSONPayload Is JObject OrElse
			   TypeOf JSONPayload Is JArray Then

				JSONPayloadString = $"{JSONPayload}"

				Try
					JToken.Parse(JSONPayloadString)
				Catch ex As Exception
					Return Content(HttpStatusCode.BadRequest,
							  New ReturnMessage(Status:="Error",
													  Message:="Invalid JSON format."))
				End Try

			Else
				Return Content(HttpStatusCode.BadRequest,
							 New ReturnMessage(Status:="Error",
													 Message:="Invalid JSON Payload format."))
			End If

			MarkLog(Action:=Action,
					ModuleName:=ModuleName,
					Detail:=Detail,
					ErrorType:=ErrorType,
					TransID:=TransID,
					EventLogID:=EventLogID,
					JSONPayload:=JSONPayloadString,
					ModuleNameforGettingID:=ModuleNameforGettingID)

			Return Ok(New ReturnMessage(Status:="Success",
											   Message:="Event-Log added Successfully!"))

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						  New ReturnMessage(Status:="Error",
												  Message:=$"{ex.GetBaseException}"))
		End Try

	End Function

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(ID As Integer, <FromBody()> Value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Sub DeleteValue(ID As Integer)

	End Sub

#End Region

End Class