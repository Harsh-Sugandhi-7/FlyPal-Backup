'************************************
'Created by:	Harsh Sugandhi
'Created on:	29th November 2024
'Created for:	To place all the Email related methods in one place for managing redundant code.
'************************************


Public Class EmailHelper

#Region " Varriable(s) "

	Private _ReportHelper As New ReportHelper

#End Region

#Region " Helper Method(s) "

	Public Function SaveReportToTempFile(ReportBytes As Byte(),
										 AttachmentName As String) As String

		Try

			' Save the byte array to a temporary file (e.g., a PDF)
			Dim tempFilePath As String = Path.Combine(path1:=Path.GetTempPath(),
													  path2:=AttachmentName.ToString() & ".pdf")

			File.WriteAllBytes(path:=tempFilePath,
							   bytes:=ReportBytes)

			Return tempFilePath

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	Public Function SendEmail(TransTypeID As Integer,
							  ModuleName As String,
							  Optional Text As String = "",
							  Optional Info As String = "",
							  Optional RegNo As String = "",
							  Optional Remark As String = "",
							  Optional Subject As String = "",
							  Optional CCMailID As String = "",
							  Optional MailBody As String = "",
							  Optional ToMailID As String = "",
							  Optional FromAudit As Integer = 0,
							  Optional BCCMailID As String = "",
							  Optional ClientCode As String = "",
							  Optional AttachedFile As String = "",
							  Optional VendorEmailID As String = "",
							  Optional AttachmentName As String = "",
							  Optional SuppliersCount As Integer = 0,
							  Optional ReportByMail As Boolean = False,
							  Optional ReportGeneratedBy As String = "",
							  Optional MultipleAttachment As String = "",
							  Optional ShowCompanyName As Boolean = True,
							  Optional MailBodyForLockedUser As String = "",
							  Optional IsMailForLockedUser As Boolean = False,
							  Optional IsVendorDetailsRequired() As Boolean = Nothing,
							  Optional OrderID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional EnquiryID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional ReceiptID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional InvoiceID As String = "{00000000-0000-0000-0000-000000000000}") As ReturnMessage

		Dim Status As String
		Dim Array() As String
		Dim ReportBytes As Byte()
		Dim TempReportPath As String
		Dim User As User = UserManagerController.FetchUser()

		Dim UserName As String = User.Name
		Dim CompanyName As String = String.Empty

		Try

			Dim UserEmailDetails As TransactionList = TransactionList.GetTransactionList()
			Dim SmtpHost = UserEmailDetails.Item(ID:=TransTypeID).SmtpHost
			Dim SmtpPort = UserEmailDetails.Item(ID:=TransTypeID).SmtpPort
			Dim SmtpUser = UserEmailDetails.Item(ID:=TransTypeID).SmtpUser
			Dim SmtpPassword = UserEmailDetails.Item(ID:=TransTypeID).SmtpPassword

			Select Case ModuleName
				Case "Order"

					Dim response As ReturnMessage = _ReportHelper.GetPODetailedReport(ByMail:=True,
																					  OrderID:=New Guid(OrderID))

					Status = response.Status
					ReportBytes = response.ReportData
					Info = $"{response.Message}"
					CompanyName = $"{response.Result}"

				Case "RCI"

					Dim Result = _ReportHelper.GetReceiptCumInvoiceDetailedReport(ByMail:=True,
																				  RequestFromAPI:=True,
																				  ReceiptID:=New Guid(ReceiptID),
																				  InvoiceID:=New Guid(InvoiceID))
					Status = $"{Result.Item1}"
					Info = $"{Result.Item2}"
					Text = $"{Result.Item3}"
					ReportBytes = Result.Item5
					CompanyName = $"{Result.Item4}"

				Case "Enquiry"

					Dim result As ReturnMessage = _ReportHelper.GetRequestForQuotationDetailedReport(ByMail:=True,
																									 ID:=EnquiryID,
																									 RequestFromAPI:=True,
																									 SuppliersCount:=SuppliersCount,
																									 IsVendorDetailsRequired:=IsVendorDetailsRequired)
					Info = $"{result.Message}"
					Status = $"{result.Status}"
					ReportBytes = result.ReportData
					CompanyName = $"{result.Result}"

			End Select

			Subject = $"{CompanyName} {ModuleName} No:- {AttachmentName}"

			If Status = "Success" Then
				TempReportPath = SaveReportToTempFile(ReportBytes:=ReportBytes,
													  AttachmentName:=AttachmentName)
			Else
				Return New ReturnMessage("Error", $"Error occurred while displaying report.")
			End If

			SendMailFile.SendMailFile(rpt:=Nothing,
									  UserName:=UserName,
									  Subject:=Subject,
									  Text:=Text,
									  Info:=Info,
									  VendorEmailID:=VendorEmailID,
									  ToMailID:=ToMailID,
									  CCMailID:=CCMailID,
									  ReportPath:=TempReportPath,
									  ReportByMail:=ReportByMail,
									  FromAudit:=FromAudit,
									  IsMailForLockedUser:=IsMailForLockedUser,
									  MailBodyForLockedUser:=MailBodyForLockedUser,
									  BCCMailID:=BCCMailID,
									  MailBody:=MailBody,
									  Remark:=Remark,
									  ReportGeneratedBy:=ReportGeneratedBy,
									  ClientCode:=ClientCode,
									  SmtpHost:=SmtpHost,
									  SmtpPort:=SmtpPort,
									  SmtpUser:=SmtpUser,
									  SmtpPassword:=SmtpPassword,
									  ShowCompanyName:=ShowCompanyName,
									  TransTypeID:=TransTypeID,
									  RegNo:=RegNo,
									  AttachedFile:=AttachedFile,
									  MultipleAttachment:=MultipleAttachment)

			Return New ReturnMessage("Success", "Email sent successfully!")

		Catch ex As Exception
			Return New ReturnMessage("Exception", $"{ex.Message}")
		End Try

	End Function

	Public Function SendEmail(TransTypeID As Integer,
							  UserName As String,
							  Optional Text As String = "",
							  Optional Info As String = "",
							  Optional RegNo As String = "",
							  Optional Remark As String = "",
							  Optional Subject As String = "",
							  Optional CCMailID As String = "",
							  Optional MailBody As String = "",
							  Optional ToMailID As String = "",
							  Optional FromAudit As Integer = 0,
							  Optional BCCMailID As String = "",
							  Optional ClientCode As String = "",
							  Optional AttachedFile As String = "",
							  Optional VendorEmailID As String = "",
							  Optional TempReportPath As String = "",
							  Optional ReportByMail As Boolean = False,
							  Optional ReportGeneratedBy As String = "",
							  Optional MultipleAttachment As String = "",
							  Optional ShowCompanyName As Boolean = True,
							  Optional MailBodyForLockedUser As String = "",
							  Optional IsMailForLockedUser As Boolean = False,
							  Optional CrystalReport As Engine.ReportClass = Nothing)

		Try

			Dim UserEmailDetails As TransactionList = TransactionList.GetTransactionList()
			Dim SmtpHost = UserEmailDetails.Item(TransTypeID).SmtpHost
			Dim SmtpPort = UserEmailDetails.Item(TransTypeID).SmtpPort
			Dim SmtpUser = UserEmailDetails.Item(TransTypeID).SmtpUser
			Dim SmtpPassword = UserEmailDetails.Item(TransTypeID).SmtpPassword

			SendMailFile.SendMailFile(rpt:=Nothing,
									  UserName:=UserName,
									  Subject:=Subject,
									  Text:=Text,
									  Info:=Info,
									  VendorEmailID:=VendorEmailID,
									  ToMailID:=ToMailID,
									  CCMailID:=CCMailID,
									  ReportPath:=TempReportPath,
									  ReportByMail:=ReportByMail,
									  FromAudit:=FromAudit,
									  IsMailForLockedUser:=IsMailForLockedUser,
									  MailBodyForLockedUser:=MailBodyForLockedUser,
									  BCCMailID:=BCCMailID,
									  MailBody:=MailBody,
									  Remark:=Remark,
									  ReportGeneratedBy:=ReportGeneratedBy,
									  ClientCode:=ClientCode,
									  SmtpHost:=SmtpHost,
									  SmtpPort:=SmtpPort,
									  SmtpUser:=SmtpUser,
									  SmtpPassword:=SmtpPassword,
									  ShowCompanyName:=ShowCompanyName,
									  TransTypeID:=TransTypeID,
									  RegNo:=RegNo,
									  AttachedFile:=AttachedFile,
									  MultipleAttachment:=MultipleAttachment)

		Catch ex As Exception
			Return ("Exception", ex.Message)
		End Try

	End Function

#End Region

End Class
