Imports System.Net
Imports System.Runtime.Remoting.Messaging
Imports System.Web.Http
Imports System.Web.Http.Results


Public Class WOController
	Inherits ApiController


#Region " Get Method(s) "

	<HttpGet>
	Public Function GetPendingWOListForRemoveComp(Optional [Date] As String = "",
												  Optional WOID As String = "{00000000-0000-0000-0000-000000000000}") As nPendingWOListForRemoveComp

		Try

			Return nPendingWOListForRemoveComp.GetnPendingWOListForRemoveComp(Date:=[Date],
																			  WOID:=WOID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetPendingWOItemListForRemovedComp(Optional WOID As String = "{00000000-0000-0000-0000-000000000000}") As nPendingWOItemListForRemovedComp

		Try

			Return nPendingWOItemListForRemovedComp.GetnPendingWOItemListForRemovedComp(WOID:=New Guid(WOID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetWOListForCombo(Optional AddTopItem As String = "",
									  Optional Text As String = "",
									  Optional FromDate As String = "",
									  Optional ToDate As String = "",
									  Optional RegNo As String = "<SELECT>",
									  Optional ModelName As String = "<SELECT>",
									  Optional StatusID As Integer = 0,
									  Optional WOStatusID As Integer = 0,
									  Optional WONumber As String = "<SELECT>",
									  Optional TransTypeID As Integer = 0) As nWOListForCombo

		Try

			Return nWOListForCombo.GetnWOListForCombo(AddTopItem:=AddTopItem,
													  Text:=Text,
													  FromDate:=FromDate,
													  ToDate:=ToDate,
													  RegNo:=RegNo,
													  ModelName:=ModelName,
													  StatusID:=StatusID,
													  WOStatusID:=WOStatusID,
													  WONumber:=WONumber,
													  TranstypeID:=TransTypeID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetWO(ID As String,
						  Optional AllWOJobType As Boolean = True,
						  Optional GetAircraftValuesAsOnCompletionDate As Boolean = False) As IHttpActionResult

		Dim WorkOrder As nWO
		Try

			WorkOrder = nWO.GetWO(ID:=New Guid(ID),
								  AllWOJobType:=AllWOJobType,
								  getAircraftValuesAsOnCompletionDate:=GetAircraftValuesAsOnCompletionDate)

			Return Ok(WorkOrder)

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Exception",
												   Message:=ex.GetBaseException.ToString))

		End Try

	End Function

#End Region

#Region " Post Method(s) "

	Public Sub PostValue(<FromBody()> Value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	Public Sub PutValue(ID As Integer, <FromBody()> Value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	Public Sub DeleteValue(ID As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
