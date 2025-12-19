'************************************
'Created by:	Sankalp Chaudhari
'Created on:	22th September 2025
'Created for:	To Map the Request parameters and type check
'************************************

Public Class IssueListReportRequest
#Region " Propertie(s) "

	Public Property ColumnHeaders As String()
	Public Property StoreName As String = String.Empty
	Public Property Text As String = String.Empty
	Public Property No As Integer = 0
	Public Property FromDate As String = "1/1/1900"
	Public Property ToDate As String = "1/1/2200"
	Public Property StatusID As Integer = 0
	Public Property VendorName As String = String.Empty
	Public Property VendorNo As String = String.Empty
	Public Property TransTypeID As Trans = Util.Trans.None
	Public Property RegNo As String = String.Empty
	Public Property IssueToType As Integer = 0
	Public Property ReceiptText As String = String.Empty
	Public Property ReceiptNo As Integer = 0
	Public Property ReleaseNoteNo As String = String.Empty
	Public Property SerialNo As String = String.Empty
	Public Property ItemName As String = String.Empty
	Public Property mIsVendor As Integer = 0
	Public Property WorkShop As String = String.Empty
	Public Property WOText As String = String.Empty
	Public Property CustomerName As String = String.Empty
	Public Property WorkSReqTexthop As String = String.Empty
	Public Property OrderText As String = String.Empty
	Public Property Amend As String = String.Empty
	Public Property ToStoreName As String = String.Empty
	Public Property BatchNo As String = String.Empty
	Public Property IssueToEmpName As String = String.Empty
	Public Property CategoryID As String = "{00000000-0000-0000-0000-000000000000}"
	Public Property Description As String = String.Empty
	Public Property SearchText As String = String.Empty
	Public Property ReqText As String = String.Empty
	Public Property WONo As Integer = 0
	Public Property ReqNo As Integer = 0
	Public Property OrderNo As Integer = 0
	Public Property CurrentPage As Integer = 0
	Public Property PageSize As Integer = 0
	Public Property IsForWO As Boolean = False
	Public Property IsUnusedReturnItem As Boolean = False
	Public Property IsCustomerName As Boolean = False
	Public Property IsCustomPaging As Boolean = False

#End Region
End Class
