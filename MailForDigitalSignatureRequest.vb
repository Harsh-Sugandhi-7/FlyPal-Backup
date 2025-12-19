Imports System.Linq

Public Class MailForDigitalSignatureRequest
	Public Shared Sub MailForDigitalSignatureRequest(Optional ByVal rpt As CrystalDecisions.CrystalReports.Engine.ReportClass = Nothing,
													 Optional ByVal UserName As String = "",
													 Optional ByVal AuthorizedUserName As String = "",
													 Optional ByVal Subject As String = "",
													 Optional ByVal DocumentName As String = "",
													 Optional ByVal Info As String = "",
													 Optional ByVal ToMailID As String = "",
													 Optional ByVal CCMailID As String = "",
													 Optional ByVal BCCMailID As String = "",
													 Optional ByVal ReportPath As String = "",
													 Optional ByVal ReportByMail As Boolean = False,
													 Optional ByVal MailBody As String = "",
													 Optional ByVal Remark As String = "",
													 Optional ByVal ReportGenratedBy As String = "",
													 Optional ByVal ClientCode As String = "",
													 Optional ByVal SmtpHost As String = "",
													 Optional ByVal SmtpPort As Integer = 0,
													 Optional ByVal SmtpUser As String = "",
													 Optional ByVal SmtpPassword As String = "",
													 Optional ByVal ShowCompanyName As Boolean = True,
													 Optional ByVal TransTypeID As Integer = 0,
													 Optional ByVal RegNo As String = "",
													 Optional ByVal AttachedFile As String = "",
													 Optional ByVal MultipleAttachment As String = "")

		System.Net.ServicePointManager.SecurityProtocol = 3072
		Dim a As New Random
		Dim mUser As User
		Dim mAttachment As Net.Mail.Attachment = Nothing
		Dim MyMessage As New MailMessage
		Dim Company As String
		Dim myFile As String = ""
		Dim str As String
		Dim myAttachedFileNames As String()

		Company = CompanyName()

		Dim smtp As SmtpClient = New SmtpClient()
		If SmtpHost <> "" And SmtpPort <> 0 And SmtpUser <> "" And SmtpPassword <> "" Then
			smtp.Host = SmtpHost
			smtp.Port = SmtpPort
			Dim wrapper As New Simple3Des("FlyPal")
			Dim cipherText As String = wrapper.DecryptData(SmtpPassword)
			smtp.Credentials = New System.Net.NetworkCredential(SmtpUser, cipherText)
		Else
			smtp.Host = "smtp.office365.com"
			smtp.Port = 587
			smtp.Credentials = New System.Net.NetworkCredential("fas@bytzsoft.com", "Hok89207")
		End If
		smtp.EnableSsl = True
		'---------------

		mUser = User.GetUser(UserName)
		Try
			If SmtpUser <> "" Then
				MyMessage.From = New MailAddress(SmtpUser, IIf(mUser.EmployeeName = "", UserName + " (Flypal)", mUser.EmployeeName + " (Flypal)"))
			Else
				MyMessage.From = New MailAddress("fas@bytzsoft.com", IIf(mUser.EmployeeName = "", UserName + " (Flypal)", mUser.EmployeeName + " (Flypal)"))
			End If
			If ToMailID.Trim IsNot Nothing And ToMailID.Trim <> String.Empty Then
				Dim ToMailIDs As String() = ToMailID.Trim.Split(",")
				For i As Integer = 0 To ToMailIDs.Count - 1
					MyMessage.To.Add(New MailAddress(ToMailIDs(i).Trim))
				Next
			End If
			MyMessage.Subject = Subject
			MyMessage.IsBodyHtml = True


			str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Dear Sir/ Madam " + AuthorizedUserName + "," + "</font></P> ")
			str = str + ("<p><font face=""Calibri"">Hope this email finds you well. I am writing this email to request your signature on the " + DocumentName + ".</font></p>")
			str = str + ("<p><font face=""Calibri"">So please review, sign and share the document.</font></p>")
			str = str + ("<p><font face=""Calibri"">Thank you for your cooperation.</font></p>")
			str = str + ("</body></html>")
			str = str + ("<p><font face=""Calibri"">")
			str = str + ("<b>Regards,</b></p>")
			str = str + ("</font>")
			str = str + ("<p><font face=""Calibri"">")
			str = str + ("<b>" + UserName + "</b></p>")
			str = str + ("</font>")

			MyMessage.Body = str

			smtp.Send(MyMessage)

			If mAttachment IsNot Nothing Then mAttachment.Dispose()
			mAttachment = Nothing
			str = ""
			str = Nothing
			MyMessage.Dispose()
			MyMessage = Nothing
			System.IO.File.Delete(AppSettings("FilePath") & "\ABC1" & ".bmp")
			System.IO.File.Delete(AppSettings("FilePath") & "\dsQuotation.xsd")
			If MultipleAttachment = "Multiple Attachment" Then
				If myFile IsNot Nothing And myFile <> String.Empty Then
					myAttachedFileNames = myFile.Split(",")
					For v As Integer = 0 To myAttachedFileNames.Count - 1
						If myAttachedFileNames(v) <> "" Then
							System.IO.File.Delete(myAttachedFileNames(v))
						End If
					Next
				End If
			Else
				If (myFile IsNot Nothing And myFile <> "") Then System.IO.File.Delete(myFile)
			End If

		Catch ex As Exception
			Dim Day, Month, Year As String
			Day = Format(Today.Date.Day, "0#")
			Month = Format(Today.Date.Month, "0#")
			Year = Format(Today.Date.Year, "0#")
			Dim todaydate As String = Day & Month & Year
			Dim Path As String = AppSettings("DOCPath") & todaydate
			FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
			FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (MailForDigitalSignatureRequest): " + ex.GetBaseException.Message + vbLf)
			FileClose(1)
			If mAttachment IsNot Nothing Then mAttachment.Dispose()
			mAttachment = Nothing
			str = ""
			str = Nothing
			MyMessage.Dispose()
			MyMessage = Nothing
			System.IO.File.Delete(AppSettings("FilePath") & "\ABC1" & ".bmp")
			System.IO.File.Delete(AppSettings("FilePath") & "\dsQuotation.xsd")
			If MultipleAttachment = "Multiple Attachment" Then
				If myFile IsNot Nothing And myFile <> String.Empty Then
					myAttachedFileNames = myFile.Split(",")
					For v As Integer = 0 To myAttachedFileNames.Count - 1
						If myAttachedFileNames(v) <> "" Then
							System.IO.File.Delete(myAttachedFileNames(v))
						End If
					Next
				End If
			Else
				If (myFile IsNot Nothing And myFile <> "") Then System.IO.File.Delete(myFile)
			End If
		End Try
	End Sub
	Public Shared Function CompanyName() As String
		Dim mCompanyDetail As New CompanyDetail
		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Return mCompanyDetail.CompanyName
	End Function
End Class
