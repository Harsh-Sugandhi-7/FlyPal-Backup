Public Class wfVendorApproval_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mVendorApproval As VendorApproval
    Public mVendorApprovals As VendorApprovals
    Dim EventLogID As Guid
    Dim mVendorApprovalCertificateDetails As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mVendorApproval = CType(Session("mVendorApproval"), VendorApproval)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
     End Sub
    Private Sub SetSession()
        Session("mVendorApproval") = mVendorApproval
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorApproval")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("VendorName")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        Save()
                        GetSession()
                        DataFieldBind()
                        SetPage()
                        ControlVisibility()
                        upnlValidationsummary.Update()
                        upnlDetails.Update()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.No
                 
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        
                    Session("sender") = ""
                    DataFieldBind()
                 Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub SetPage()
        If mVendorApproval.IsNew = True Then
            lblTitle.Text = "Vendor Document Approval [" + "New" + "]"
        Else
            lblTitle.Text = "Vendor Document Approval [" & mVendorApproval.Name & "]"
        End If
    End Sub
    Private Sub ControlVisibility()
        'If mVendorApproval.IsNew Then
        '    txtApprovalNo.ReadOnly = False
        'Else
        '    txtApprovalNo.ReadOnly = True
        '    txtApprovalNo.BackColor = System.Drawing.Color.Gainsboro
        'End If
        ControlVisibilityForAttachment()
    End Sub
    Private Sub SetObject()
        mVendorApproval = Session("mVendorApproval")
         With mVendorApproval
            .Name = txtName.Text.Trim
            .ApprovalNo = txtApprovalNo.Text
            .IsOneTime = chkIsOneTime.Checked
            .IsApplicable = chkIsApplicable.Checked
            .FromDate = txtFromDate.Text
            .ToDate = txtToDate.Text
            .Remark = txtRemark.Text
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If
         End With
        Session("mVendorApproval") = mVendorApproval
    End Sub
     Private Sub ControlVisibilityForAttachment()
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then 'change from  to current condition
                ImageButton1.Visible = True
                btnDelAttach.Enabled = True
            Else
                ImageButton1.Visible = False
            End If
        Else
            ImageButton1.Visible = False
        End If
        upnlAttach.Update()
    End Sub
     Private Sub GetAttachment()
        If mVendorApproval.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mVendorApproval.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '
        If mFileAttach.Size > 0 Then
            Try
                mFileAttach.Save()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
            End Try
        Else
            If (Not mVendorApproval.IsNew) And IsAttachmentDeleted Then
                FileAttach.DeleteAttachment(mFileAttach.ID, mVendorApproval.ID)
            End If
            IsAttachmentDeleted = False
            Session("IsAttachmentDeleted") = IsAttachmentDeleted
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
    'End
    Private Function Save() As Boolean
        SetObject()
        If mVendorApproval.IsValid Then
            Try
                mVendorApproval.ApplyEdit()
                SaveAttachment()
                mVendorApproval = CType(mVendorApproval.Save(), VendorApproval)
                Session("mVendorApproval") = mVendorApproval
                mVendorApprovalCertificateDetails = "Approval No. : " & mVendorApproval.ApprovalNo & " Name : " & mVendorApproval.Name
                MarkLog(Util.Action.Save, "Vendor", mVendorApprovalCertificateDetails, Util.ErrorType.NoError, mVendorApproval.ID, EventLogID)
                Return True
            Catch ex As SqlException
                'Return False
                If ex.Number = 50000 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.ProcedureError, ex.Message, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Finally
                mVendorApproval = Nothing
            End Try
        Else
            Return False
            upnlValidationsummary.Update()
        End If
    End Function
   
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 500 char."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtToDate" Then
            If (chkIsOneTime.Checked = False And txtToDate.Text = "") Then
                custValidator.ErrorMessage = "To Date Required"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Function CustomValidate1() As Boolean
        SetObject()
        Dim strMSG As String = ""
        If Not mVendorApproval.IsValid Then
            For i As Integer = 0 To mVendorApproval.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mVendorApproval.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMSG.Trim <> "" Then
            cvDate.ErrorMessage = strMSG
            cvDate.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub DataFieldBind()
        txtFromDate.Text = mVendorApproval.FromDateFormatted.ToString
        txtToDate.Text = mVendorApproval.ToDateFormatted.ToString
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                setFocus(txtApprovalNo)
            End If
            lblVendorName.Text = Session("VendorName").ToString
            DataFieldBind()
            GetAttachment()
        End If
        SetPage()
        ControlVisibility()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("VendorNew")) Or (Not User.IsInRole("VendorEdit")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If CustomValidate1() = False Then upnlValidationsummary.Update() : Exit Sub

        mVendorApprovals = VendorApprovals.GetVendorApprovalList(mVendorApproval.VendorID)
        If (mVendorApprovals.Contains(txtName.Text.Trim, mVendorApproval.VendorID) = True And mVendorApproval.IsNew = True And mVendorApproval.SortNo = 1) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are trying to save the duplicate entry.", False), True)
            mVendorApprovals = Nothing
            Exit Sub
        End If

        If IsValid Then
            If Save() Then
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                upnlValidationsummary.Update()
                upnlDetails.Update()

                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
   End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
         ControlVisibilityForAttachment()
    End Sub
#End Region
End Class