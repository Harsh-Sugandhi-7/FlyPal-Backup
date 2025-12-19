'***********************************
'Modified by:  Harsh Sugandhi on 8th April 2025 for FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Web.Http
Imports System.Web.Script.Services

Imports System.Net


Public Class AutoCompleteController
	Inherits ApiController


#Region " Get Method(s) "

#Region " DistinctTextList "

	<HttpGet>
	Public Function GetDistinctTextListAutoComplete(Optional Text As String = "",
													Optional [Of] As String = "",
													Optional IsForText As Boolean = False,
													Optional TransTypeID As Integer = 0,
													Optional ToDate As String = "1/1/5500",
													Optional TagText As String = "") As DistinctTextListAutoComplete
		Try

			Return DistinctTextListAutoComplete.GetDistinctTextList(Text:=Text,
																	[Of]:=[Of],
																	IsForText:=IsForText,
																	TransTypeID:=TransTypeID,
																	ToDate:=ToDate,
																	TagText:=TagText)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " WorkShopList "

	<HttpGet>
	Public Function GetWorkShopList(LookInType As Integer,
									Optional WorkShopName As String = "",
									Optional LocationID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional IsSelectTagRequired As Boolean = False,
									Optional TagText As String = "<SELECT>") As WorkShopList
		Try

			Return WorkShopList.GetWorkShopList(LookInType:=LookInType,
												WorkShopName:=WorkShopName,
												LocationID:=LocationID,
												IsSelectTagRequired:=IsSelectTagRequired,
												TagText:=TagText)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " PilotList "

	<HttpGet>
	Public Function GetPilotListAutoComplete(Optional Name As String = "",
											 Optional AddTopItem As String = "") As PilotListAutoComplete

		Try

			Return PilotListAutoComplete.GetPilotList(Name:=Name,
													  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " PlaceList "

	<HttpGet>
	Public Function GetPlaceListAutoComplete(Optional Name As String = "",
											 Optional AddTopItem As String = "") As PlaceListAutoComplete

		Try

			Return PlaceListAutoComplete.GetPlaceList(Name:=Name,
													  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try
	End Function

#End Region

#Region " LicenseNoListWithEmployee "

	<HttpGet>
	Public Function GetLicenseNoList(Optional SearchText As String = "",
									 Optional User As String = "",
									 Optional IsTagRequired As Boolean = False,
									 Optional TagName As String = "",
									 Optional ExcludeUseInLogRequire As Boolean = False,
									 Optional WithoutLicenseNoAlso As Integer = 0,
									 Optional OnlyEmployeesHavingDocuments As Boolean = False,
									 Optional OnlyEmployeesHavingTrainings As Boolean = False,
									 Optional GroupByDesignationRequired As Boolean = False) As LicenseNoListWithEmployee

		Try

			Return LicenseNoListWithEmployee.GetLicenseNoList(SearchText:=SearchText,
															  User:=User,
															  IsTagRequired:=IsTagRequired,
															  TagName:=TagName,
															  ExludeUseInLogRequried:=ExcludeUseInLogRequire,
															  WithoutLicenseNoAlso:=WithoutLicenseNoAlso,
															  OnlyEmployeesHavingDocuments:=OnlyEmployeesHavingDocuments,
															  OnlyEmployeesHavingTrainings:=OnlyEmployeesHavingTrainings,
															  GroupByDesignationRequired:=GroupByDesignationRequired)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region "Distinct Order Aircraft-Reg List"

	<HttpGet>
	Public Function DistinctOrderAircraftRegList(RegNo As String) As IHttpActionResult

		Try
			Dim DistinctOrderAircraftList As DistinctOrderAircraftRegAutoComplete

			DistinctOrderAircraftList = DistinctOrderAircraftRegAutoComplete.GetDistinctOrderAircraftRegList(RegNo:=RegNo)

			Return Ok(DistinctOrderAircraftList)


		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString))

		End Try

	End Function

#End Region

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
	Public Sub PutValue(ID As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Sub DeleteValue(ID As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
