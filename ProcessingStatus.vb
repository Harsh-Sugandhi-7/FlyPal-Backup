<Serializable()> _
Public Class ProcessingStatus
	Private Shared Status As New Hashtable

	Public Shared Function getValue(ByVal itemId As Guid) As Object
		Return Status(itemId)
	End Function

	Public Shared Sub add(ByVal ItemId As Guid, ByVal oStatus As Object)
		'make sure that oStatus contains only the values 0 through 100 or -1
		Status(ItemId) = oStatus
	End Sub

	Public Shared Sub update(ByVal ItemId As Guid, ByVal oStatus As Object)
		'make sure that oStatus contains only the values 0 through 100 or -1
		Status(ItemId) = oStatus
	End Sub

	Public Shared Sub remove(ByVal ItemId As Guid)
		Status.Remove(ItemId)
	End Sub

	Public Shared Function Contains(ByVal ItemId As Guid) As Boolean
		Return Status.Contains(ItemId)
	End Function

End Class
