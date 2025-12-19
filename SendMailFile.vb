Imports System.Linq

Public Class SendMailFile

    Public Shared Sub SendMailFile(Optional rpt As Engine.ReportClass = Nothing,
                                   Optional UserName As String = "",
                                   Optional Subject As String = "",
                                   Optional Text As String = "",
                                   Optional Info As String = "",
                                   Optional VendorEmailID As String = "",
                                   Optional ToMailID As String = "",
                                   Optional CCMailID As String = "",
                                   Optional ReportPath As String = "",
                                   Optional ReportByMail As Boolean = False,
                                   Optional FromAudit As Integer = 0,
                                   Optional IsMailForLockedUser As Boolean = False,
                                   Optional MailBodyForLockedUser As String = "",
                                   Optional BCCMailID As String = "",
                                   Optional MailBody As String = "",
                                   Optional Remark As String = "",
                                   Optional ReportGeneratedBy As String = "",
                                   Optional ClientCode As String = "",
                                   Optional SmtpHost As String = "",
                                   Optional SmtpPort As Integer = 0,
                                   Optional SmtpUser As String = "",
                                   Optional SmtpPassword As String = "",
                                   Optional ShowCompanyName As Boolean = True,
                                   Optional TransTypeID As Integer = 0,
                                   Optional RegNo As String = "",
                                   Optional AttachedFile As String = "",
                                   Optional MultipleAttachment As String = "",
                                   Optional OtherInfo As String = "") 'Added by Bhushan for OTP password generation change ,-- added parameter IsMailForLockedUser,MailBodyforLockedUser

        Try

            Net.ServicePointManager.SecurityProtocol = 3072
            Dim a As New Random
            Dim mUser As User
            Dim mAttachment As Attachment = Nothing
            Dim MyMessage As New MailMessage
            Dim Company As String
            Dim myFile As String = ""
            Dim str As String
            Dim myAttachedFileNames As String()

            Company = CompanyName()

            'Added by Shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            Dim smtp As New SmtpClient()

            If SmtpHost <> "" And SmtpPort <> 0 And SmtpUser <> "" And SmtpPassword <> "" Then

                smtp.Host = SmtpHost
                smtp.Port = SmtpPort
                Dim wrapper As New Simple3Des("FlyPal")
                Dim cipherText As String = wrapper.DecryptData(SmtpPassword)
                smtp.Credentials = New Net.NetworkCredential(SmtpUser, cipherText)

            Else

                smtp.Host = "smtp.office365.com"
                smtp.Port = 587
                smtp.Credentials = New Net.NetworkCredential("fas@bytzsoft.com", "Hok89207")

            End If

            smtp.EnableSsl = True
            '---------------

            mUser = User.GetUser(UserName)
            If mUser.UserEmail = "" And mUser.ManagerEmail = "" Then
                'Do nothing 
            Else

                Try

                    If Not IsMailForLockedUser Then 'IF case added by Bhushan for OTP change at 26-JUL-2016

                        If Not ReportPath = "" Then
                            myFile = ReportPath
                        Else

                            If rpt IsNot Nothing Then

                                If Text IsNot Nothing AndAlso Text <> "" Then
                                    myFile = AppSettings("DOCPath") & Text.Replace("/", "-") & ".PDF"
                                Else
                                    myFile = AppSettings("DOCPath") & Text.Replace("/", "-") & a.Next & ".PDF"
                                End If

                                rpt.ExportToDisk(ExportFormatType.PortableDocFormat, myFile)
                                rpt.Close()

                            End If

                        End If

                    End If

                    If SmtpUser <> "" Then

                        MyMessage.From = New MailAddress(SmtpUser,
                                                         IIf(mUser.EmployeeName = "",
                                                             UserName + " (Flypal)",
                                                             mUser.EmployeeName + " (Flypal)"))

                    Else

                        MyMessage.From = New MailAddress("fas@bytzsoft.com",
                                                         IIf(mUser.EmployeeName = "",
                                                             UserName + " (Flypal)",
                                                             mUser.EmployeeName + " (Flypal)"))

                    End If

                    If ToMailID.Trim IsNot Nothing And ToMailID.Trim <> String.Empty Then

                        Dim ToMailIDs As String() = ToMailID.Trim.Split(",")
                        For i As Integer = 0 To ToMailIDs.Count - 1

                            MyMessage.To.Add(New MailAddress(ToMailIDs(i).Trim))
                        Next

                        Dim CCMailIDs As String() = CCMailID.Trim.Split(",")

                        If CCMailID.Trim IsNot Nothing And CCMailID.Trim <> String.Empty Then

                            For i As Integer = 0 To CCMailIDs.Count - 1
                                MyMessage.CC.Add(New MailAddress(CCMailIDs(i).Trim))
                            Next

                        End If

                        Dim BCCMailIDs As String() = BCCMailID.Trim.Split(",")

                        If BCCMailID.Trim IsNot Nothing And BCCMailID.Trim <> String.Empty Then

                            For i As Integer = 0 To BCCMailIDs.Count - 1
                                MyMessage.Bcc.Add(New MailAddress(BCCMailIDs(i).Trim))
                            Next

                        Else

                        End If

                    Else

                        If mUser.ManagerEmail.Trim <> String.Empty Then

                            MyMessage.To.Add(New MailAddress(mUser.ManagerEmail.Trim)) '

                            If AppSettings("StoreMailID") = "" Then
                                MyMessage.CC.Add(New MailAddress(mUser.UserEmail.Trim))
                            Else

                                Dim StoreMailID As String = mUser.UserEmail.Trim + "," + AppSettings("StoreMailID")
                                Dim StoreMailIDs As String() = StoreMailID.Trim.Split(",")

                                For i As Integer = 0 To StoreMailIDs.Count - 1
                                    MyMessage.CC.Add(New MailAddress(StoreMailIDs(i).Trim))
                                Next

                            End If

                        Else

                            If AppSettings("StoreMailID") = "" Then
                                MyMessage.To.Add(New MailAddress(mUser.UserEmail.Trim))
                            Else

                                Dim StoreMailID As String = mUser.UserEmail.Trim + "," + AppSettings("StoreMailID")
                                Dim StoreMailIDs As String() = StoreMailID.Trim.Split(",")

                                For i As Integer = 0 To StoreMailIDs.Count - 1
                                    MyMessage.To.Add(New MailAddress(StoreMailIDs(i).Trim))
                                Next

                            End If

                        End If

                    End If

                    MyMessage.Subject = Subject
                    MyMessage.IsBodyHtml = True

                    If Not IsMailForLockedUser Then 'IF case Added by Bhushan for OTP password generation change

                        If MailBody = "" Then

                            If FromAudit = 0 Then

                                If ClientCode = "Heligo" And TransTypeID = Trans.WOCAMO Then    ' IF condition Added by Shital on 17-Nov-2021
                                    str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Hi BMM and LMM " & ",</font></P> ")
                                ElseIf ClientCode = "Heligo" And TransTypeID = Trans.WO145 Then    ' IF condition Added by Saylee on 22-Aug-2023
                                    str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Dear Sir/ Madam, " & ",</font></P> ")

                                Else
                                    str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Dear " & "All" & ",</font></P> " & IIf(ShowCompanyName, "<font face=""Calibri""><p> " & Company & "</font></p>", "").ToString)
                                End If

                            Else
                                str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Dear " & "Auditee" & ",</font></P> " & IIf(ShowCompanyName, "<font face=""Calibri""><p> " & Company & "</font></p>", "").ToString)
                            End If

                            If myFile <> "" Then

                                If MultipleAttachment = "Multiple Attachment" Then

                                    myAttachedFileNames = myFile.Split(",")

                                    For v As Integer = 0 To myAttachedFileNames.Count - 1

                                        If myAttachedFileNames(v) <> "" Then

                                            mAttachment = New Attachment(myAttachedFileNames(v))
                                            MyMessage.Attachments.Add(mAttachment)

                                        End If

                                    Next

                                Else

                                    mAttachment = New Attachment(myFile)
                                    MyMessage.Attachments.Add(mAttachment)

                                End If

                                mAttachment = Nothing

                                If ReportByMail = True Then

                                    If TransTypeID = Trans.WOCAMO And AppSettings("ClientCode") = "STR" Then
                                        str = str + ("<p><font face=""Calibri"">Kindly find the attached Work Package for <b>" & RegNo & "</b>.</font></p>")
                                    Else

                                        If ClientCode = "Heligo" And TransTypeID = Trans.WOCAMO Then ' IF condition Added by Shital on 17-Nov-2021
                                            str = str + ("<p><font face=""Calibri"">Please find the attached CAMO Call Out for your information and planning.</font></p>")
                                        ElseIf ClientCode = "Heligo" And TransTypeID = Trans.WO145 Then ' IF condition Added by Saylee on 22-Aug-2023
                                            str = str + ("<p><font face=""Calibri"">Please find the attached QC Work Order for your information and necessary action.</font></p>")
                                        Else
                                            str = str + ("<p><font face=""Calibri""><b>" & Text & IIf(OtherInfo = "", "", " " & OtherInfo) & " </b> is attached for your information and planning.</font></p>")
                                        End If

                                    End If

                                End If

                            End If

                            str = str + ("<p><font face=""Calibri"">" + Info + "</font></p>")

                            If Remark IsNot Nothing AndAlso Not Remark.Trim.Trim = "" Then
                                str = str + ("<p><font face=""Calibri""> <b> Remark: " & Remark.Trim & "</b></font></p>")
                            End If

                            If ReportGeneratedBy IsNot Nothing AndAlso Not ReportGeneratedBy.Trim.Trim = "" Then
                                str = str + ("<p><font face=""Calibri""><b> Report generated by: " & ReportGeneratedBy.Trim & "</b></font></p>")
                            End If

                            str = str + ("</body></html>")
                            str = str + ("<p><font face=""Calibri"">")

                            If ClientCode = "Heligo" Then ' IF condition Added by Shital on 17-Nov-2021
                                str = str + ("Regards,</p>")
                            Else
                                str = str + ("<b>Regards,</b></p>")
                            End If

                            str = str + ("</font>")
                            str = str + ("<p><font face=""Calibri"">")

                            If FromAudit = 0 Then

                                If ClientCode = "Heligo" And TransTypeID = Trans.WOCAMO Then ' IF condition Added by Shital on 17-Nov-2021
                                    str = str + ("<b>CAM </b></p>")
                                ElseIf ClientCode = "Heligo" And TransTypeID = Trans.WO145 Then ' IF condition Added by Saylee on 22-Aug-2023

                                    str = str + ("<b>Quality Manager</b></p>")
                                    str = str + ("<b>" & Company & "</b></p>")

                                Else
                                    str = str + ("<b>FlyPal® </b></p>")
                                End If

                            Else

                                If AppSettings("ClientCode") = "STR" Then
                                    str = str + ("<b>Quality Auditor -Star Air (Flypal).</b></p>")
                                Else
                                    str = str + ("<b>Quality Manager.</b></p>")
                                End If

                            End If

                            str = str + ("</font>")
                            str = str + ("<p><font face=""Calibri"">")
                            str = str + ("<font color=""#FF0000"">*</font>This is automated Email generated by FlyPal® Mail Service. Please do not reply.</p>")
                            str = str + ("</font>")

                            ' Set the body of the mail message
                            MyMessage.Body = str

                        Else
                            MyMessage.Body = MailBody
                        End If

                    Else
                        MyMessage.Body = MailBodyForLockedUser
                    End If

                    smtp.Send(MyMessage)

                    If IsMailForLockedUser Then  'IF case added by Bhushan for OTP change at 26-JUL-2016
                        Exit Sub
                    End If

                    'Added by Shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
                    If SmtpUser <> "fas@bytzsoft.com" Then

                        Dim str1 As String = ""
                        str1 = "<p><font face=""Calibri""> <b> To Mail IDs: " &
                               MyMessage.To.ToString & "</b></font></p>" + "<p><font face=""Calibri""> <b> CC Mail IDs:  " &
                               MyMessage.CC.ToString & "</b></font></p>"

                        MyMessage.From = New MailAddress("fas@bytzsoft.com",
                                                         IIf(mUser.EmployeeName = "", UserName + " (Flypal)", mUser.EmployeeName + " (Flypal)"))
                        MyMessage.To.Clear()
                        MyMessage.CC.Clear()
                        MyMessage.Bcc.Clear()
                        MyMessage.To.Add(New MailAddress("fas@bytzsoft.com"))
                        MyMessage.Body = MyMessage.Body + str1
                        smtp.Send(MyMessage)

                    End If
                    '--------------

                    If mAttachment IsNot Nothing Then mAttachment.Dispose()

                    mAttachment = Nothing
                    str = ""
                    str = Nothing
                    MyMessage.Dispose()
                    MyMessage = Nothing

                    File.Delete(AppSettings("FilePath") & "\ABC1" & ".bmp")
                    File.Delete(AppSettings("FilePath") & "\dsQuotation.xsd")

                    If MultipleAttachment = "Multiple Attachment" Then

                        If myFile IsNot Nothing And myFile <> String.Empty Then

                            myAttachedFileNames = myFile.Split(",")

                            For v As Integer = 0 To myAttachedFileNames.Count - 1

                                If myAttachedFileNames(v) <> "" Then
                                    File.Delete(myAttachedFileNames(v))
                                End If

                            Next

                        End If

                    Else
                        If (myFile IsNot Nothing And myFile <> "") Then File.Delete(myFile)
                    End If

                Catch ex As Exception

					Dim Day, Month, Year As String

					Day = Format(Today.Date.Day, "0#")
					Month = Format(Today.Date.Month, "0#")
					Year = Format(Today.Date.Year, "0#")

					Dim TodayDate As String = Day & Month & Year
                    Dim Path As String = AppSettings("DOCPath") & TodayDate



					FileOpen(1,
                             Path,
                             OpenMode.Append,
                             OpenAccess.ReadWrite)

                    WriteLine(1,
                              Date.Now.ToString + " Mail service (SendMailFile): " + ex.GetBaseException.Message + vbLf)

                    FileClose(1)

                    If mAttachment IsNot Nothing Then mAttachment.Dispose()

                    mAttachment = Nothing
                    str = ""
                    str = Nothing
                    MyMessage.Dispose()
                    MyMessage = Nothing

                    File.Delete(AppSettings("FilePath") & "\ABC1" & ".bmp")
                    File.Delete(AppSettings("FilePath") & "\dsQuotation.xsd")

                    If MultipleAttachment = "Multiple Attachment" Then

                        If myFile IsNot Nothing And myFile <> String.Empty Then

                            myAttachedFileNames = myFile.Split(",")

                            For v As Integer = 0 To myAttachedFileNames.Count - 1

                                If myAttachedFileNames(v) <> "" Then
                                    File.Delete(myAttachedFileNames(v))
                                End If

                            Next

                        End If

                    Else
                        If (myFile IsNot Nothing And myFile <> "") Then File.Delete(myFile)
                    End If

                End Try

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Shared Function CompanyName() As String

        Dim mCompanyDetail As New CompanyDetail
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Return mCompanyDetail.CompanyName

    End Function

End Class
