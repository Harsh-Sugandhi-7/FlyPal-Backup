'************************************
'Created by:	Harsh Sugandhi
'Created on:	16th September 2025
'Created for:	To Map the Request parameters and type check
'************************************


Public Class EnquiryListReportRequest


#Region " Propertie(s) "

	Public Property ColumnHeaders As String()
	Public Property ItemName As String = String.Empty
	Public Property Text As String = String.Empty
	Public Property No As Integer = 0
	Public Property FromDate As String = "1/1/1900"
	Public Property ToDate As String = "1/1/2200"
	Public Property StatusID As Integer = 0
	Public Property VendorName As String = String.Empty
	Public Property VendorNo As String = String.Empty
	Public Property TransTypeID As Integer = 0

#End Region


End Class
