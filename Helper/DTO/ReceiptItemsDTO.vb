'************************************
'Created by:	Harsh Sugandhi
'Created on:	10th November 2025
'Created for:	DTO to map the request data for Receipt Items
'************************************


Public Class ReceiptItemsDTO

	Public Property ID As Guid
	Public Property FromItemTypeID As Integer
	Public Property IsSerialized As Boolean
	Public Property OrderItemID As Guid
	Public Property IssueItemID As Guid
	Public Property ItemID As Guid
	Public Property SerialNo As String
	Public Property ReleaseNoteNo As String
	Public Property ExpiryDate As Date
	Public Property BatchNo As String

End Class