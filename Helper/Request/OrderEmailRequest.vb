'************************************
'Created by:	Harsh Sugandhi
'Created on:	29th November 2024
'Created for:	To Map the Request parameters and type check
'************************************


Public Class OrderEmailRequest

#Region " Properties "

	Public Property OrderID As String
	Public Property Subject As String = String.Empty
	Public Property Text As String = String.Empty
	Public Property Info As String = String.Empty
	Public Property VendorEmailID As String = String.Empty
	Public Property ToMailID As String = String.Empty
	Public Property CCMailID As String = String.Empty
	Public Property ReportPath As String = String.Empty
	Public Property ReportByMail As Boolean = True
	Public Property FromAudit As Integer = 0
	Public Property IsMailForLockedUser As Boolean = False
	Public Property MailBodyForLockedUser As String = False
	Public Property BCCMailID As String = String.Empty
	Public Property MailBody As String = String.Empty
	Public Property Remark As String = String.Empty
	Public Property ReportGeneratedBy As String = String.Empty
	Public Property ClientCode As String = String.Empty
	Public Property SmtpHost As String = String.Empty
	Public Property SmtpPort As Integer = 0
	Public Property SmtpUser As String = String.Empty
	Public Property SmtpPassword As String = String.Empty
	Public Property ShowCompanyName As Boolean = True
	Public Property TransTypeID As Integer = 0
	Public Property RegNo As String = String.Empty
	Public Property AttachedFile As String = String.Empty
	Public Property MultipleAttachment As String = String.Empty

#End Region

End Class
