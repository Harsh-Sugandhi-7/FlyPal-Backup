'***********************************
'Created by:  Harsh Sugandhi
'Created for: To define the response of API
'***********************************


Public Class ReturnMessage

#Region " Varriable(s) "

	Public Result As Object
	Public Status As String
	Public Message As String
	Public EventLogID As Guid
	Public ReportData As Byte()
	Public TransactionID As Guid
	Public TransactionID1 As Guid

#End Region

#Region " Constructor "

	Public Sub New(Status As String,
				   Message As String,
				   Optional Result As Object = Nothing,
				   Optional ReportData As Byte() = Nothing,
				   Optional EventLogID As String = "{00000000-0000-0000-0000-000000000000}",
				   Optional TransactionID As String = "{00000000-0000-0000-0000-000000000000}",
				   Optional TransactionID1 As String = "{00000000-0000-0000-0000-000000000000}")

		Me.Status = Status
		Me.Result = Result
		Me.Message = Message
		Me.ReportData = ReportData
		Me.EventLogID = New Guid(EventLogID)
		Me.TransactionID = New Guid(TransactionID)
		Me.TransactionID1 = New Guid(TransactionID1)

	End Sub

#End Region

End Class