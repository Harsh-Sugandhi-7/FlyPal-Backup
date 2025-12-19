Imports System.Web.Http

Public Class PendingTransactionCountController
	Inherits ApiController

	Public Function GetValues(Optional TransDate As String = "01/01/3300",
							  Optional TransTypeID As Integer = 0,
							  Optional ClientCode As String = "") As PendingTransactionCount

		Return PendingTransactionCount.GetCount(TransDate, TransTypeID, ClientCode)

	End Function

End Class
