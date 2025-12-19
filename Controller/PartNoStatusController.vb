Imports System.Web.Http


Public Class PartNoStatusController
	Inherits ApiController

	Public Function GetStockPartStatusList(LinkID As Guid,
										   Optional StoreID As String = "{00000000-0000-0000-0000-000000000000}",
										   Optional IsValuedStore As Boolean = False,
										   Optional IsOpenTransactionsRequired As Boolean = False) As rptStockPartStatus

		Try

			Return rptStockPartStatus.GetStockPartStatusList(ItemID:=LinkID,
															 StoreID:=StoreID,
															 IsValuedStore:=IsValuedStore,
															 IsOpenTransactionsRequired:=IsOpenTransactionsRequired)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function


	Public Function GetOnOrderPartStatusList(LinkID As Guid,
											 Optional StoreID As String = "{00000000-0000-0000-0000-000000000000}",
											 Optional IsOpenTransactionsRequired As Boolean = False) As rptOnOrderPartStatus

		Try

			Return rptOnOrderPartStatus.GetrptOnOrderPartStatusList(ItemID:=LinkID,
																	StoreID:=StoreID,
																	IsOpenTransactionsRequired:=IsOpenTransactionsRequired)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function


	Public Function GetReturnnablePartStatusList(LinkID As Guid,
												 Optional StoreID As String = "{00000000-0000-0000-0000-000000000000}") As rptReturnablePartStatus

		Try

			Return rptReturnablePartStatus.GetrptReturnnablePartStatusList(ItemID:=LinkID,
																		   StoreID:=StoreID)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function


	Public Function GetTransitPartList(LinkID As Guid,
									   Optional IssueDate As String = "") As rptTransitPartList

		If IssueDate = "" Then
			IssueDate = Today.Date.ToString
		End If

		Try

			Return rptTransitPartList.GetTransitPartList(ItemID:=LinkID,
														 IssueDate:=IssueDate)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function


	Public Function GetRequisitionItemsForPartNoStatus(LinkID As Guid,
													   Optional ClientCode As String = "") As RequisitionItemsNew
		Try

			Return RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(ItemID:=LinkID,
																		  ClientCode:=ClientCode)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function


	Public Function GetValue(id As Integer) As String
		Return "value"
	End Function


	Public Sub PostValue(<FromBody()> value As String)

	End Sub


	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub


	Public Sub DeleteValue(id As Integer)

	End Sub

End Class
