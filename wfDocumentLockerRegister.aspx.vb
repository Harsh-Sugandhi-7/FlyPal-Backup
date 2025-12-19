'************************************
' Created By : Shital
' Date:     : 28-Oct-2021
' Modified by Harsh Sugandhi on 4th August 2025 for FLYPAL-2600
'************************************


Imports System.IO.Compression


Public Class wfDocumentLockerRegister
    Inherits Page

#Region " Variable Declaration "

    Public mDocumentLockerList As DocumentLockerList
    Public mDocumentLocker As DocumentLocker
    Dim mFileAttach As FileAttach
    Dim EventLogID As Guid
    Dim mDocCategoryList As DocCategoryList
    Dim mDepartmentList As DepartmentList
    Dim index As Guid
    Dim Report As ReportData
    Dim mUserList As UserList
    Public mUser As User
    Dim MachineNameValueList As MachineNameValueList

#End Region

#Region " Helper Methods "

    Private Sub GetSession()
        mDocumentLocker = Session("mDocumentLocker")
        mDocumentLockerList = Session("mDocumentLockerList")
        index = Session("Index")
        mUserList = Session("mUserList")
    End Sub

    Private Sub FindNow()

        Try

            Dim UserID As Guid
            UserID = mUserList.Item(HttpContext.Current.User.Identity.Name).UserID

            mDocumentLockerList = Nothing
            dgAttachment.DataSource = Nothing
            dgAttachment.DataBind()
            dgAttachment.DataBind()
            mDocumentLockerList = DocumentLockerList.GetDocumentLockerList(Name:=txtFileNameSearch.Text.ToString, "",
                                                                           CategoryID:=cmbCategorySearch.SelectedValue.ToString,
                                                                           DepartmentID:=IIf(cmbDepartmentsearch.SelectedValue.ToString = "(SELECT)",
                                                                                             Guid.Empty.ToString,
                                                                                             cmbDepartmentsearch.SelectedValue.ToString),
                                                                           UserID:=UserID.ToString,
                                                                           MachineID:=ddlAircraftSearch.SelectedValue,
                                                                           AddOrView:=1)
            dgAttachment.DataSource = mDocumentLockerList
            dgAttachment.DataBind()
            upnlManAttachment.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Function MessageBody(mUserOTP As String, mTempUserForOTP As String) As String

        Try

            Dim mCompanyDetail As New CompanyDetail
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Message As String

            Message = "<html>"
            Message += "<head>"
            Message += "</head>"
            Message += "<body>"
            Message += "Dear user,<p>"
            Message += " (" + mCompanyDetail.CompanyName + ")" + ",<p>"
            Message += "<font size=""3"">"
            Message += "FlyPal® user login " + "<B><font size=""5"">" + mTempUserForOTP.ToString + "</font></B>" + " needs to be validated by entering One Time Password [OTP] for Document Locker." + "</font></p>"
            Message += "OTP: " + mUserOTP.ToString
            Message += "<font size=""2""><BR>(Please Note, This OTP will be valid for next 00:30 Mins to verify your identity.)<BR></font>"
            Message += "<p><BR><span style=""font-size: 11pt"">Best Regards,</span><font style=""font-size: 11pt""></p>"
            Message += "<p>FlyPal Team.</p>"
            Message += "</body>"
            Message += "</html>"

            Return Message

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Public Function DecompressFile(ByRef inputFileName As String,
                                   ByRef DestinationFileName As String,
                                   ByRef DestinationDirectory As String) As String

        Try

            Dim stream As New MemoryStream(File.ReadAllBytes(inputFileName))
            Dim gZip As New GZipStream(stream, CompressionMode.Decompress)
            Dim buffer(3) As Byte

            stream.Position = stream.Length - 5
            stream.Read(buffer, 0, 4)
            Dim size As Integer = BitConverter.ToInt32(buffer, 0)

            stream.Position = 0
            Dim decompressed(size - 1) As Byte
            gZip.Read(decompressed, 0, size)
            gZip.Dispose()
            stream.Dispose()
            File.WriteAllBytes(DestinationDirectory & "\" & DestinationFileName, decompressed)

            Return DestinationDirectory & "\" & DestinationFileName

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            Return False
        End Try

    End Function


#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        Try

            Dim mUserId As Guid
            Dim mUserList As UserList = UserList.GetUserList(HttpContext.Current.User.Identity.Name, IsForDocLocker:=1)
            Session("mUserList") = mUserList
            mUserId = mUserList.Item(HttpContext.Current.User.Identity.Name).UserID

            mDocumentLockerList = DocumentLockerList.GetDocumentLockerList(UserID:=mUserId.ToString, AddOrView:=1)
            dgAttachment.DataSource = mDocumentLockerList
            dgAttachment.DataBind()
            Session("mDocumentLockerList") = mDocumentLockerList
            mDocCategoryList = DocCategoryList.GetDocCategoryList(, "(ALL)")
            cmbCategorySearch.DataSource = mDocCategoryList
            cmbCategorySearch.DataBind()
            Session("mDocCategoryList") = mDocCategoryList

            mUser = SI.UTILITY.User.GetUser(mUserList.Item(HttpContext.Current.User.Identity.Name).UserID)

            Dim mUserEmployeeDepartments As UserEmployeeDepartments
            mUserEmployeeDepartments = UserEmployeeDepartments.GetUserEmployeeDepartmentList(mUserList.Item(HttpContext.Current.User.Identity.Name).UserID, "(ALL)")
            cmbDepartmentsearch.DataSource = mUserEmployeeDepartments

            Session("mDepartmentList") = mDepartmentList

            MachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString, , , , , , ,
                                                                       IsTagRequired:=True,
                                                                       TagText:="(SELECT)", ,
                                                                       SkipIsForInventoryAircarft:=True)

            ddlAircraftSearch.DataSource = MachineNameValueList
            Session("MachineNameValueList") = MachineNameValueList

            DataBind()
            upnlManAttachment.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Sub CustomValidation(s As Object, e As ServerValidateEventArgs)

        Try

            Dim CustValid As CustomValidator
            CustValid = CType(s, CustomValidator)

            If CustValid.ControlToValidate = "txtPassword" Then

                If txtPassword.Text = "" Then
                    CustValid.ErrorMessage = " Password required "
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Try

            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)

            If Not Page.IsPostBack Then
                DataFieldBind()

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SearchRecords(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click

        Try

            FindNow()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GV_Attachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgAttachment.RowCommand

        Try

            Select Case e.CommandName
                Case "View"

                    Dim Index As New Guid(e.CommandArgument.ToString)
                    Session("Index") = Index
                    Dim DestinationFileName As String = ""
                    DestinationFileName = mDocumentLockerList.Item(Index).Path.Substring(mDocumentLockerList.Item(Index).Path.LastIndexOf("\") + 1, mDocumentLockerList.Item(Index).Path.LastIndexOf(".") - mDocumentLockerList.Item(Index).Path.LastIndexOf("\") - 1)

                    DecompressFile(mDocumentLockerList.Item(Index).Path, DestinationFileName, mDocumentLockerList.Item(Index).Path.Substring(0, mDocumentLockerList.Item(Index).Path.LastIndexOf("\")))
                    Session("DOCPath") = mDocumentLockerList.Item(Index).Path.Substring(0, mDocumentLockerList.Item(Index).Path.LastIndexOf("."))

                    '-----For OTP
                    Dim numbers As String = "1234567890"
                    Dim characters As String = numbers
                    Dim length As Integer = 5 'Integer.Parse(ddlLength.SelectedItem.Value)

                    Dim otp As String = String.Empty

                    For i As Integer = 0 To length - 1
                        Dim character As String = String.Empty
                        Do
                            Dim indec As Integer = New Random().Next(0, characters.Length)
                            character = characters.ToCharArray()(indec).ToString()
                        Loop While otp.IndexOf(character) <> -1
                        otp += character
                    Next

                    Session("OTP") = otp
                    Dim mEmpMobNo As String
                    mEmpMobNo = mUserList.Item(HttpContext.Current.User.Identity.Name).EmpMobilNo

                    'SMS Added code 024-Nov-2021
                    Try
                        If mEmpMobNo.Length > 0 Then
                            SendMailFile.SendMailFile(, HttpContext.Current.User.Identity.Name, "FlyPal Login OTP [One Time Password].", "", "", , mUserList.Item(HttpContext.Current.User.Identity.Name).Email.Trim, "", "", , , True, MessageBody(Session("OTP"), HttpContext.Current.User.Identity.Name))

                            Dim mMessage As String = "Dear user, OTP for " + "FlyPal-ENT" + " user " + HttpContext.Current.User.Identity.Name + " is " + Session("OTP").ToString + ". Regards, BytzSoft Team"
                            SendSMS.SendSMS("FLYPAL", "1207163168224899351", mMessage, mEmpMobNo)


                        Else 'If Mobile No. not exist then atleast send mail to user
                            SendMailFile.SendMailFile(, HttpContext.Current.User.Identity.Name, "FlyPal Login OTP [One Time Password].", "", "", , mUserList.Item(HttpContext.Current.User.Identity.Name).Email.Trim, "", "", , , True, MessageBody(Session("OTP"), HttpContext.Current.User.Identity.Name))
                        End If
                        MarkLog(Action.SendMail, "DocumentLocker", " OTP sent to view Document Name - " + mDocumentLockerList.Item(Index).Name + " by SMS/Mail to user", ErrorType.NoError, mDocumentLockerList.Item(Index).ID, EventLogID)
                    Catch ex As Exception

                    End Try
                    '-----

                    mdlPopupOTPMaster.Show()
                    upnlOTPMaster.Update()
                    '---------End OTP


            End Select

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            mdlPopUpLoginMaster.Hide()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub OTPOkay(sender As Object, e As EventArgs) Handles btnOTPOk.Click

        Try

            If CInt(Session("OTP")) = CInt(txtOTP.Text) Then

                mdlPopupOTPMaster.Hide()
                upnlOTPMaster.Update()

                MarkLog(Action.View,
                        "DocumentLocker",
                        "Document Name - " + mDocumentLockerList.Item(index).Name +
                              " Document Viewed by user - " + HttpContext.Current.User.Identity.Name,
                        ErrorType.NoError,
                        mDocumentLockerList.Item(index).ID,
                        EventLogID)

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openFile",
                                                    "OpenFile()",
                                                    True)
                Session.Remove("OTP")
                txtOTP.Text = ""

            Else

                lblOTPInvalid.Text = "Invalid OTP"
                MarkLog(Action.View,
                        "DocumentLocker",
                        "Document Name - " + mDocumentLockerList.Item(index).Name +
                              " Invalid OTP try by user - " + HttpContext.Current.User.Identity.Name,
                        ErrorType.NoError,
                        mDocumentLockerList.Item(index).ID,
                        EventLogID)

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub OTPClose(sender As Object, e As EventArgs) Handles btnOTPClose.Click

        Try

            Session.Remove("OTP")
            txtOTP.Text = ""
            mdlPopupOTPMaster.Hide()
            upnlOTPMaster.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GoBack(sender As Object, e As EventArgs) Handles btnBack.Click

        Try

            MarkLog(Action.Close, "DocumentLocker", "", ErrorType.NoError, Guid.Empty, EventLogID)
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

End Class