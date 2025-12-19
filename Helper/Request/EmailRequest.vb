'************************************
'Created by:	Harsh Sugandhi
'Created on:	15th September 2025
'Created for:	To Map the Request parameters and type check
'************************************


Public Class EmailRequest

#Region " Propertie(s) "

	Public Property FromAudit As Integer = 0
	Public Property TransTypeID As Integer = 0
	Public Property SuppliersCount As Integer = 0
	Public Property Info As String = String.Empty
	Public Property Text As String = String.Empty
	Public Property ReportByMail As Boolean = False
	Public Property RegNo As String = String.Empty
	Public Property Remark As String = String.Empty
	Public Property OrderID As String = String.Empty
	Public Property Subject As String = String.Empty
	Public Property MailBody As String = String.Empty
	Public Property ShowCompanyName As Boolean = True
	Public Property CCMailID As String = String.Empty
	Public Property ToMailID As String = String.Empty
	Public Property EnquiryID As String = String.Empty
	Public Property ReceiptID As String = String.Empty
	Public Property InvoiceID As String = String.Empty
	Public Property BCCMailID As String = String.Empty
	Public Property IsVendorDetailsRequired As Boolean()
	Public Property ReportPath As String = String.Empty
	Public Property ClientCode As String = String.Empty
	Public Property CompanyName As String = String.Empty
	Public Property AttachedFile As String = String.Empty
	Public Property VendorEmailID As String = String.Empty
	Public Property RequisitionID As String = String.Empty
	Public Property IsMailForLockedUser As Boolean = False
	Public Property MailBodyForLockedUser As String = False
	Public Property AttachmentName As String = String.Empty
	Public Property ReportGeneratedBy As String = String.Empty
	Public Property MultipleAttachment As String = String.Empty

#End Region

End Class