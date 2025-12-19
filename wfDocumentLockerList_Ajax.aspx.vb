'************************************
' Created By : Shital
' Date:     : 28-Oct-2021
' Modified by Harsh Sugandhi on 4th August 2025 for FLYPAL-2600
'************************************


Imports System.IO.Compression

Imports System.Linq


Public Class wfDocumentLockerList_Ajax
    Inherits Page

#Region " Variable Declaration "

    Dim UserEmployeeDepartments As UserEmployeeDepartments
    Public DocumentLockerList As DocumentLockerList
    Public DocumentLocker As DocumentLocker
    Dim FileAttach As FileAttach
    Dim EventLogID As Guid
    Dim DocCategoryList As DocCategoryList
    Dim DepartmentList As DepartmentList
    Dim index As Guid
    Dim Report As ReportData
    Dim mUserList As UserList
    Public mUser As User
    Dim checkedUserList As String()
    Dim checkedDepartmentList As String()
    Dim checkedAircraftList As String()
    Dim MachineNameValueList As MachineNameValueList

#End Region

#Region " Helper Methods "

    Private Sub GetSession()
        DocumentLocker = Session("DocumentLocker")
        FileAttach = Session("FileAttach")
        DocumentLockerList = Session("DocumentLockerList")
        index = Session("Index")
        mUserList = Session("mUserList")
    End Sub

    Private Sub RemoveSession()

        If Session("MiddleFrame") <> "wfDocumentLockerList_Ajax.aspx.aspx" Then

            Session.Remove("DocumentLockerList")
            Session.Remove("DocumentLocker")
            Session.Remove("FileAttach")

        End If

    End Sub

    Public Function ConvertToByteArray(source() As Byte) As Byte()

        Try

            Dim memoryStream As New MemoryStream()
            Dim gZipStream As New GZipStream(memoryStream, CompressionMode.Compress, True)
            gZipStream.Write(source, 0, source.Length)
            gZipStream.Dispose()
            memoryStream.Position = 0

            Dim buffer(memoryStream.Length) As Byte
            memoryStream.Read(buffer, 0, buffer.Length)
            memoryStream.Dispose()

            Return buffer

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Public Function DecompressFile(ByRef InputFileName As String,
                                   ByRef DestinationFileName As String,
                                   ByRef DestinationDirectory As String) As String

        Try

            Dim stream As New MemoryStream(File.ReadAllBytes(InputFileName))
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

    Private Sub DeleteRecord(mID As Guid)

        Try

            DocumentLocker.DeleteDocumentLocker(DocumentLockerList.Item(mID).ID)
            File.Delete(DocumentLockerList.Item(mID).Path)
            DataFieldBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MessageBoxResult()

        Try

            Dim MsgBoxResult As MsgBoxResult
            MsgBoxResult = MSGBoxCtrl.Result

            If MsgBoxResult > 0 Then

                Select Case MsgBoxResult
                    Case MsgBoxResult.Yes

                        Dim DocName As String = ""

                        If MSGBoxCtrl.Sender = "Delete" Then

                            Try

                                DocumentLockerList = Session("DocumentLockerList")
                                DocName = DocumentLockerList.Item(index).Name
                                DeleteRecord(index)
                                txtFileName.Text = ""
                                DataFieldBind()
                                upnlDetails.Update()

                            Catch ex As SqlException

                                If ex.Number = 547 Then

                                    MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete,
                                                    MSGBox.Message_text.ReferenceDelete,
                                                    "",
                                                    MsgBoxStyle.OkOnly,
                                                    "")

                                    txtFileName.Text = ""

                                    Exit Sub

                                End If

                            Finally

                                MarkLog(Action.Delete,
                                        "DocumentLocker",
                                        " Document Name - " + DocName.ToString + " Document deleted successfully..",
                                        ErrorType.NoError,
                                        Guid.Empty,
                                        EventLogID)

                            End Try

                        End If

                    Case MsgBoxResult.No

                        If MSGBoxCtrl.Sender = "Delete" Then
                            DataFieldBind()
                            upnlDetails.Update()
                        End If

                End Select

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Function SaveDocFile() As String

        Dim DocPath As String = ""

        Try

            If rdbPublic.Checked Then

                DocPath = AppSettings("DOCPath")
                If Not Directory.Exists(AppSettings("DOCPath") + "Public") Then Directory.CreateDirectory(AppSettings("DOCPath") + "Public")
                DocPath = AppSettings("DOCPath") + "Public\" & DateTime.Now.ToString("yyyyMMddTHHmmss") & "-" & Path.GetFileName(FileUpload.FileName)
                FileUpload.SaveAs(DocPath)

                '----------- added this code for Compressed file--------------
                Dim name As String = Path.GetFileName(DocPath)
                Dim source() As Byte = File.ReadAllBytes(DocPath)
                Dim compressed() As Byte = ConvertToByteArray(source)
                File.WriteAllBytes(DocPath & "" & ".zip", compressed)
                File.Delete(DocPath)
                '-------

            ElseIf rdbPrivate.Checked Then

                DocPath = AppSettings("DOCPath")

                If Not Directory.Exists(AppSettings("DOCPath") + "Private") Then Directory.CreateDirectory(AppSettings("DOCPath") + "Private")

                DocPath = AppSettings("DOCPath") + "Private\"

                If Directory.Exists(DocPath + User.Identity.Name) Then
                    DocPath = DocPath + User.Identity.Name + "\" & DateTime.Now.ToString("yyyyMMddTHHmmss") & "-" & Path.GetFileName(FileUpload.FileName)
                    FileUpload.SaveAs(DocPath)
                Else

                    Directory.CreateDirectory(DocPath + User.Identity.Name + "\")
                    DocPath = DocPath + User.Identity.Name + "\" & DateTime.Now.ToString("yyyyMMddTHHmmss") & "-" & Path.GetFileName(FileUpload.FileName)
                    FileUpload.SaveAs(DocPath)

                End If

                '----------- added this code for Compressed file--------------
                Dim name As String = Path.GetFileName(DocPath & Path.GetFileName(FileUpload.FileName))
                Dim source() As Byte = File.ReadAllBytes(DocPath)
                Dim compressed() As Byte = ConvertToByteArray(source)
                File.WriteAllBytes(DocPath & "" & ".zip", compressed)
                File.Delete(DocPath)
                '-------

            End If

            Return DocPath

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Private Function MessageBody(mUserOTP As String, mTempUserForOTP As String) As String

        Try

            Dim CompanyDetail As New CompanyDetail
            CompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Message As String = String.Empty

            Message = "<html>"
            Message = Message + "<head>"
            Message = Message + "</head>"
            Message = Message + "<body>"
            Message = Message + "Dear user,<p>"
            Message = Message + " (" + CompanyDetail.CompanyName + ")" + ",<p>"
            Message = Message + "<font size=""3"">"
            Message = Message + "FlyPal® user login " + "<B><font size=""5"">" + mTempUserForOTP.ToString + "</font></B>" + " needs to be validated by entering One Time Password [OTP] for Document Locker." + "</font></p>"
            Message = Message + "OTP: " + mUserOTP.ToString
            Message = Message + "<font size=""2""><BR>(Please Note, This OTP will be valid for next 00:30 Mins to verify your identity.)<BR></font>"
            Message = Message + "<p><BR><span style=""font-size: 11pt"">Best Regards,</span><font style=""font-size: 11pt""></p>"
            Message = Message + "<p>FlyPal Team.</p>"
            Message = Message + "</body>"
            Message = Message + "</html>"

            Return Message

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Private Sub FindNow()

        Try

            Dim mUserId As Guid
            mUserId = mUserList.Item(HttpContext.Current.User.Identity.Name).UserID

            DocumentLockerList = Nothing
            dgAttachment.DataSource = Nothing
            DocumentLockerList = DocumentLockerList.GetDocumentLockerList(Name:=txtFileNameSearch.Text.ToString,
                                                                          AddTopItem:="",
                                                                          CategoryID:=cmbCategorySearch.SelectedValue.ToString,
                                                                          DepartmentID:=IIf(cmbDepartmentsearch.SelectedValue.ToString = "(SELECT)",
                                                                                            Guid.Empty.ToString,
                                                                                            cmbDepartmentsearch.SelectedValue.ToString),
                                                                          UserID:=mUserId.ToString,
                                                                          MachineID:=ddlAircraft.SelectedValue,
                                                                          AddOrView:=0)
            dgAttachment.DataSource = DocumentLockerList
            dgAttachment.DataBind()

            upnlManAttachment.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        Try

            Dim mUserId As Guid
            Dim mUserList As UserList = UserList.GetUserList(HttpContext.Current.User.Identity.Name, IsForDocLocker:=1)
            Dim ListOfUsers = (From c In mUserList Where c.Name.ToUpper <> HttpContext.Current.User.Identity.Name.ToUpper Select c).ToList

            Session("mUserList") = mUserList
            mUserId = mUserList.Item(HttpContext.Current.User.Identity.Name).UserID

            ListDocumentLockerUser.DataSource = ListOfUsers
            DocumentLockerList = DocumentLockerList.GetDocumentLockerList(UserID:=mUserId.ToString,
                                                                          AddOrView:=0)
            dgAttachment.DataSource = DocumentLockerList
            Session("DocumentLockerList") = DocumentLockerList

            dgAttachment.DataBind()

            DocCategoryList = DocCategoryList.GetDocCategoryList(, "(SELECT)")
            cmbCategory.DataSource = DocCategoryList
            cmbCategory.DataBind()
            cmbCategorySearch.DataSource = DocCategoryList
            cmbCategorySearch.DataBind()
            Session("DocCategoryList") = DocCategoryList

            mUser = SI.UTILITY.User.GetUser(mUserList.Item(HttpContext.Current.User.Identity.Name).UserID)
            DepartmentList = DepartmentList.GetDepartmentList(, "(SELECT)")

            UserEmployeeDepartments = UserEmployeeDepartments.GetUserEmployeeDepartmentList(mUserList.Item(HttpContext.Current.User.Identity.Name).UserID,
                                                                                            AddTopItem:="(SELECT)")

            Dim List = (From c In UserEmployeeDepartments Where c.EmployeeDepartmentName <> "(SELECT)" Select c).ToList
            ListDepartment.DataSource = List
            cmbDepartmentsearch.DataSource = UserEmployeeDepartments
            Session("DepartmentList") = DepartmentList

            MachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString, , , , , , ,
                                                                       IsTagRequired:=True,
                                                                       TagText:="(SELECT)", ,
                                                                       SkipIsForInventoryAircarft:=True)

            AircraftList.DataSource = MachineNameValueList
            ddlAircraft.DataSource = MachineNameValueList
            Session("MachineNameValueList") = MachineNameValueList

            txtexpiryDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

            DataBind()

            txtWarningDays.Text = "0"
            txtFileName.Text = ""
            upnlDetails.Update()
            upnlManAttachment.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub


#End Region

#Region " Custom Validations "

    Public Sub CustomValidation(s As Object, e As ServerValidateEventArgs)

        Try

            Dim CustomValidator As CustomValidator
            CustomValidator = CType(s, CustomValidator)

            If CustomValidator.ControlToValidate = "txtPassword" Then

                If txtPassword.Text = "" Then
                    CustomValidator.ErrorMessage = " Password required "
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

        Try

            Dim CustomValidator As CustomValidator
            CustomValidator = CType(s, CustomValidator)

            If CustomValidator.ControlToValidate = "txtFileName" Then
                If FileUpload.HasFile = False Then
                    CustomValidator.ErrorMessage = "Please Select File.."
                    e.IsValid = False
                End If
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)

            If Not Page.IsPostBack Then
                DataFieldBind()
            End If

            txtWarningDays.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtWarningDays').value,event)")
            Session("MiddleFrame") = "wfDocumentLockerList_Ajax.aspx"

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnBack.Click

        Try

            RemoveSession()
            MarkLog(Action.Close,
                    "DocumentLocker",
                    "",
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GV_Attachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgAttachment.RowCommand

        Try

            Select Case e.CommandName
                Case "View"

                    Dim Index As Guid = New Guid(e.CommandArgument.ToString)
                    Session("Index") = Index
                    Dim destFileName As String = ""
                    destFileName = DocumentLockerList.Item(Index).Path.Substring(DocumentLockerList.Item(Index).Path.LastIndexOf("\") + 1, DocumentLockerList.Item(Index).Path.LastIndexOf(".") - DocumentLockerList.Item(Index).Path.LastIndexOf("\") - 1)

                    DecompressFile(DocumentLockerList.Item(Index).Path, destFileName, DocumentLockerList.Item(Index).Path.Substring(0, DocumentLockerList.Item(Index).Path.LastIndexOf("\")))
                    Session("DOCPath") = DocumentLockerList.Item(Index).Path.Substring(0, DocumentLockerList.Item(Index).Path.LastIndexOf("."))

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

                            SendMailFile.SendMailFile(,
                                                      HttpContext.Current.User.Identity.Name,
                                                      "FlyPal Login OTP [One Time Password].",
                                                      "",
                                                      "", ,
                                                      mUserList.Item(HttpContext.Current.User.Identity.Name).Email.Trim,
                                                      "",
                                                      "", , ,
                                                      True,
                                                      MessageBody(Session("OTP"), HttpContext.Current.User.Identity.Name))

                            Dim mMessage As String = "Dear user, OTP for " + "FlyPal-ENT" + " user " + HttpContext.Current.User.Identity.Name +
                                                     " is " + Session("OTP").ToString + ". Regards, BytzSoft Team"
                            SendSMS.SendSMS("FLYPAL",
                                            "1207163168224899351",
                                            mMessage,
                                            mEmpMobNo)

                        Else 'If Mobile No. not exist then atleast send mail to user
                            SendMailFile.SendMailFile(,
                                                      HttpContext.Current.User.Identity.Name,
                                                      "FlyPal Login OTP [One Time Password].",
                                                      "",
                                                      "", ,
                                                      mUserList.Item(HttpContext.Current.User.Identity.Name).Email.Trim,
                                                      "",
                                                      "", , ,
                                                      True,
                                                      MessageBody(Session("OTP"), HttpContext.Current.User.Identity.Name))
                        End If

                        MarkLog(Action.SendMail,
                                "DocumentLocker",
                                " OTP sent to view Document Name - " + DocumentLockerList.Item(Index).Name + " by SMS / Mail to user",
                                ErrorType.NoError,
                                DocumentLockerList.Item(Index).ID,
                                EventLogID)

                    Catch ex As Exception
                        Throw ex.GetBaseException
                    End Try
                    '-----

                    mdlPopupOTPMaster.Show()
                    upnlOTPMaster.Update()

                Case "DeleteRec"

                    Dim Index As Guid = New Guid(e.CommandArgument.ToString)
                    If DocumentLockerList.Item(Index).UserID = mUserList.Item(HttpContext.Current.User.Identity.Name).UserID Then

                        MSGBoxCtrl.Show(MSGBox.Message_title.Delete,
                                        MSGBox.Message_text.Delete,
                                        "",
                                        MsgBoxStyle.YesNo,
                                        "Delete")

                        Session("Index") = Index

                    Else

                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "OpenScript",
                                                            MessageBox.Show("You are not authorized user", False), True)

                        Exit Sub

                    End If

            End Select

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GV_Attachment_PageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles dgAttachment.PageIndexChanging

        Try

            dgAttachment.PageIndex = e.NewPageIndex
            dgAttachment.DataSource = DocumentLockerList
            Session("DocumentLockerList") = DocumentLockerList
            dgAttachment.DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub UploadDocument(sender As Object, e As EventArgs) Handles btnupload.Click

        Try

            If Not IsValid Then
                upnlValidationSummary.Update()
                Exit Sub
            End If

            If FileUpload.HasFile And FileUpload.FileBytes.Length >= 104857600 Then        ' '100 Megabytes (MB)	=	104,857,600 Bytes (B)
                MSGBoxCtrl.Show("Alert", "File Size is too large..can not upload", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Dim DocPath As String = ""
            DocPath = SaveDocFile()

            Dim mUserId As Guid = mUserList.Item(HttpContext.Current.User.Identity.Name).UserID

            If FileUpload.HasFile Then

                Try

                    FileAttach = FileAttach.NewAttachmentChild(Guid.Empty, "")
                    FileAttach.FileName = FileUpload.FileName
                    Session("FileAttach") = FileAttach

                    DocumentLocker = DocumentLocker.NewDocumentLocker()
                    DocumentLocker.Name = txtFileName.Text.ToString
                    DocumentLocker.Path = DocPath + ".zip"
                    DocumentLocker.UserID = mUserId
                    DocumentLocker.DocCategoryID = New Guid(cmbCategory.SelectedValue)
                    DocumentLocker.ExpiryDate = txtexpiryDate.Text.ToString

                    If rdbPrivate.Checked = True Then
                        DocumentLocker.IsPrivate = True
                    ElseIf rdbPublic.Checked = True Then
                        DocumentLocker.IsPrivate = False
                    End If

                    If rdbUserwise.Checked = True Then
                        DocumentLocker.UserOrDepartmentWise = 1  ''1 for user wise
                    ElseIf rdbDepartmentwise.Checked = True Then
                        DocumentLocker.UserOrDepartmentWise = 2  ''2 for Department wise
                    End If

                    If txtWarningDays.Text = "" Then
                        DocumentLocker.WarningDays = 0
                    Else
                        DocumentLocker.WarningDays = Val(Trim(txtWarningDays.Text))
                    End If

                    DocumentLocker.DocumentLockerUsers.Add(ID:=New Guid, UserID:=mUserId.ToString)

                    If rdbUserwise.Checked = True Then

                        checkedUserList = (From c As ListItem In ListDocumentLockerUser.Items
                                           Where c.Selected = True
                                           Select (c.Value)).ToArray

                        If checkedUserList.Count > 0 Then

                            For i As Integer = 0 To checkedUserList.Count - 1
                                DocumentLocker.DocumentLockerUsers.Add(ID:=New Guid, UserID:=checkedUserList(i).ToString)
                            Next

                        End If

                    End If

                    If rdbDepartmentwise.Checked = True Then

                        checkedDepartmentList = (From d As ListItem In ListDepartment.Items
                                                 Where d.Selected = True
                                                 Select (d.Value)).ToArray

                        If checkedDepartmentList.Count > 0 Then

                            For i As Integer = 0 To checkedDepartmentList.Count - 1
                                DocumentLocker.DocumentLockerDepartments.Add(ID:=New Guid, DepartmentID:=checkedDepartmentList(i).ToString)
                            Next

                        End If

                    End If

                    If rdbAircraftWise.Checked Then

                        DocumentLocker.DocumentLockerMachines.Add(ID:=New Guid, MachineID:=AircraftList.SelectedValue.ToString)

                    End If

                    Session("DocumentLocker") = DocumentLocker

                    If DocumentLocker.IsValid Then

                        Try

                            DocumentLocker.Save()
                            MarkLog(Action.Save,
                                    "DocumentLocker",
                                    "Document Name - " + DocumentLocker.Name + " Document save by user - " + HttpContext.Current.User.Identity.Name,
                                    ErrorType.NoError,
                                    DocumentLocker.ID,
                                    EventLogID)

                        Catch ex As SqlException

                            MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
                                            MSGBox.Message_text.Duplicate,
                                            ex.Procedure,
                                            MsgBoxStyle.OkOnly,
                                            "")

                        End Try

                    End If

                    rdbPublic.Checked = False
                    rdbPrivate.Checked = True
                    upnlValidationSummary.Update()
                    DataFieldBind()

                    MSGBoxCtrl.Show(MSGBox.Message_title.SavedSuccessFully,
                                    MSGBox.Message_text.SavedSuccessFully,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                Catch ex As Exception

                    ClientScript.RegisterStartupScript([GetType],
                                                       "Alert Script",
                                                       "alert(" + ex.Message + ");",
                                                       True)
                End Try

            Else
                upnlValidationSummary.Update()
                DataFieldBind()
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub LoginMaster(sender As Object, e As EventArgs) Handles btnLoginMaster.Click

        Try

            Dim UserName As String = txtLoginName.Text
            Dim Password As String = txtPassword.Text
            Dim Login As New SI.UTILITY.Login(UserName, Password)
            Dim BusinessPrincipal As BusinessPrincipal = BusinessPrincipal.login(Login.UserName, Login.Password, Session("RequestInfo"))

            Session("AuthenticatedMessage") = CType(BusinessPrincipal.IdentityInfo, BusinessIdentity).AuthenticatedMessage
            Session("XStatus") = CType(BusinessPrincipal.IdentityInfo, BusinessIdentity).XStatus
            Session("XStatusMessage") = CType(BusinessPrincipal.IdentityInfo, BusinessIdentity).XStatusMessage

            If Session("AuthenticatedMessage") = "" Then

                Dim Str1 As String
                Str1 = "openFile();"
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openFile",
                                                    Str1,
                                                    True)

            Else
                lblInvalid.Text = Session("AuthenticatedMessage")
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            mdlPopUpLoginMaster.Hide()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub OTPOk(sender As Object, e As EventArgs) Handles btnOTPOk.Click

        Try

            If CInt(Session("OTP")) = CInt(txtOTP.Text) Then

                Dim Str1 As String
                Str1 = "openFile();"
                mdlPopupOTPMaster.Hide()
                upnlOTPMaster.Update()
                MarkLog(Action.View,
                        "DocumentLocker",
                        "Document Name - " + DocumentLockerList.Item(index).Name + " Document Viewed by user - " + HttpContext.Current.User.Identity.Name,
                        ErrorType.NoError,
                        DocumentLockerList.Item(index).ID,
                        EventLogID)

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openFile",
                                                    Str1,
                                                    True)
                Session.Remove("OTP")
                txtOTP.Text = ""

            Else
                lblOTPInvalid.Text = "Invalid OTP"
                MarkLog(Action.View,
                        "DocumentLocker",
                        "Document Name - " + DocumentLockerList.Item(index).Name + " Invalid OTP try by user - " + HttpContext.Current.User.Identity.Name,
                        ErrorType.NoError,
                        DocumentLockerList.Item(index).ID,
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

    Private Sub FindRecords(sender As Object, e As ImageClickEventArgs) Handles btnSearchRecords.Click

        Try

            FindNow()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        Try

            MSGBoxCtrl.HideControl()
            MessageBoxResult()

        Catch ex As Exception
        Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click


        Try
            Session("DocumentLockerList") = DocumentLockerList

            Dim da As New ObjectAdapter
            Dim ds As New dsDocumentList

            Dim RptDocumentLockerList As Engine.ReportClass

            Dim CompanyDetail As New CompanyDetail

            RptDocumentLockerList = New crDocumentLockerList

            Report = New ReportData(CompanyDetail.CompanyName,
                                    CompanyDetail.Address,
                                    CompanyDetail.Tel1,
                                    CompanyDetail.Tel2,
                                    CompanyDetail.Fax,
                                    CompanyDetail.Email,
                                    CompanyDetail.WebSite,
                                    "",
                                    "",
                                    "",
                                    "",
                                    "",
                                    "",
                                    "",
                                    "",
                                    "")

            ds.Clear()

            Dim companyLogo As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, companyLogo)
            da.Fill(ds, Report)
            da.Fill(ds, DocumentLockerList)

            RptDocumentLockerList.SetDataSource(ds)
            Session("CrystalReport") = RptDocumentLockerList

            Dim Str1 As String
            Str1 = "displayReportInPDF();"
            ScriptManager.RegisterStartupScript(Me, [GetType], "Display Report In PDF", Str1, True)

            MarkLog(Action.View,
                    "DocumentLocker",
                    "Document Locker List Print" + HttpContext.Current.User.Identity.Name,
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

End Class