Imports System.Web.Http

Public Class PendingToIssueListController
	Inherits ApiController

	Public Function GetValues(Optional StoreID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional ItemName As String = "",
							  Optional ItemDesc As String = "",
							  Optional ItemCategory As String = "",
							  Optional ItemNomenclature As String = "",
							  Optional Store As String = "",
							  Optional IssueDate As String = "",
							  Optional TransTypeID As Trans = Util.Trans.None,
							  Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional AircraftID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional IsBERPart As Boolean = True,
							  Optional BarCodeNo As String = "",
							  Optional IsAllPartsRequired As Boolean = False,
							  Optional IssueToDiscardAsExpired As Integer = 0,
							  Optional ReceiptID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional ItemPrimaryCategory As Integer = 0,
							  Optional CodeNo As String = "",
							  Optional ToTypeIDOfIssue As Integer = 0,
							  Optional CategoryID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional IsForIssuetoSupplierasReturn As Boolean = False,
							  Optional SearchStr As String = "",
							  Optional ClientCode As String = "") As PendingToIssueList

		Return PendingToIssueList.GetPendingToIssueList(StoreID:=New Guid(StoreID),
														ItemName:=ItemName,
														ItemDesc:=ItemDesc,
														ItemNomenclature:=ItemNomenclature,
														Store:=Store,
														IssueDate:=IssueDate,
														TransTypeID:=TransTypeID,
														ItemID:=ItemID,
														AircraftID:=AircraftID,
														IsBERPart:=IsBERPart,
														BarCodeNo:=BarCodeNo,
														IsAllPartsRequired:=IsAllPartsRequired,
														IssueToDiscardAsExpired:=IssueToDiscardAsExpired,
														ReceiptID:=ReceiptID,
														ItemPrimaryCategory:=ItemPrimaryCategory,
														CodeNo:=CodeNo,
														ToTypeIDOfIssue:=ToTypeIDOfIssue,
														CategoryID:=CategoryID,
														IsForIssuetoSupplierasReturn:=IsForIssuetoSupplierasReturn,
														SearchStr:=SearchStr,
														ClientCode:=ClientCode)
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
