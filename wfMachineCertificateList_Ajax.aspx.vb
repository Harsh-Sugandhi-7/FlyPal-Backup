Public Class wfMachineCertificateList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAttachToID As Guid
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
      End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCertificateList")
        Session.Remove("mMachineCertificateEdit")
        Session.Remove("mAttachToID")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub AttachmentSessionRemove()
        Session.Remove("Extension")
        Session.Remove("Size")
        Session.Remove("ImageFile")
        Session.Remove("FileUpload.FileName")
    End Sub
    Private Sub SetObject()
        mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).CertificateName = txtName.Text
        mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).CertificateNo = txtNo.Text
        If calIssueDate.Text = "" Then
            mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).IssueDate = System.DBNull.Value
        Else
            mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).IssueDate = CDate(calIssueDate.Text)
        End If
        If calExpiryDate.Text = "" Then
            mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).ExpiryDate = System.DBNull.Value
        Else
            mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).ExpiryDate = CDate(calExpiryDate.Text)
        End If
        mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).IsApplicable = chkApplicable.Checked
        mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).Remark = txtRemark.Text
        mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).OneTimeCertificate = chkOneTimeCertificate.Checked 'Added By Prashant 25-Apr-2013 'All-25042013-1
        mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).WarningDays = Val(txtWarningDays.Text.Trim) 'Added by Saylee on 6-Dec-2018 for ALL06122018
        If calEffectiveDate.Text = "" Then  'Added By Prashant 19-Jun-2020 ALL18062020-1
            mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).EffectiveDate = System.DBNull.Value
        Else
            mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).EffectiveDate = CDate(calEffectiveDate.Text)
        End If
    End Sub
    Private Sub SetFileObject()
        If CInt(Session("Size")) > 0 Then
            mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).IsAttachmentAdded = True
            If mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).FileAttachments.Count = 0 Then
                mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).FileAttachments.Add(mMachine.MachineCertificates.CurrentItem.ID, Session("ImageFile"), CInt(Session("Size")), Session("Extension"))
            Else
                If (Not mMachine.MachineCertificates.CurrentItem.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAllAttachmentChilds(mMachine.MachineCertificates.CurrentItem.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
                mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).FileAttachments(0).ImageFile = Session("ImageFile")
                mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).FileAttachments(0).Size = CInt(Session("Size"))
                mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).FileAttachments(0).Extension = Session("Extension")
            End If
            AttachmentSessionRemove()
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mAircraftDSCList As DailyStatusList
                            mAircraftDSCList = DailyStatusList.GetDailyStatusList(mMachine.MachineCertificates(mMachine.MachineCertificates.CurrentIndex).MachineID, Guid.Empty.ToString, Guid.Empty.ToString, 7, True)
                            If mAircraftDSCList.Contains(mMachine.MachineCertificates(mMachine.MachineCertificates.CurrentIndex).ID, "") Then
                                MSGBoxCtrl.show("Reference Delete!", "This certificate is added in Aircraft Daily Status. Please do not delete this Entry.", "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            If mMachine.MachineCertificates(mMachine.MachineCertificates.CurrentIndex).IsDone <> True Then
                                mMachine.MachineCertificates.Remove(mMachine.MachineCertificates(mMachine.MachineCertificates.CurrentIndex))
                                For i As Integer = 0 To mMachine.MachineCertificates.Count - 1
                                    mMachine.MachineCertificates(i).SerialNo = i + 1
                                Next
                                Session("mMachine") = mMachine
                                Session("mMachineCertificateEdit") = False
                                SetControlsToBlank()
                                setFocus(txtName)
                                DataFieldBind()
                                SetPage()
                                SetGrid()
                                ControlVisibilityForAttachment()
                                lblAircraftCertificateDetails.InnerText = "Aircraft Certificate Details [NEW]"
                                UpdatePanel()
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                            Else
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "Record cannot be deleted.It is already being renewed.", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.DatabaseException, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        SetGridView()
                        upnlGridView.Update()
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        txtName.Text = mMachine.MachineCertificates.Item(ID).CertificateName
        txtNo.Text = mMachine.MachineCertificates.Item(ID).CertificateNo
        calIssueDate.Text = mMachine.MachineCertificates.Item(ID).IssueDateFormatted.ToString
        calExpiryDate.Text = mMachine.MachineCertificates.Item(ID).ExpiryDateFormatted.ToString
        chkApplicable.Checked = mMachine.MachineCertificates.Item(ID).IsApplicable
        txtRemark.Text = mMachine.MachineCertificates.Item(ID).Remark
        chkApplicable.Enabled = Not mMachine.MachineCertificates.Item(ID).IsDone
        chkOneTimeCertificate.Checked = mMachine.MachineCertificates.Item(ID).OneTimeCertificate 'Added By Prashant 25-Apr-2013 'All-25042013-1
        If mMachine.MachineCertificates.Item(ID).IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
        lblAircraftCertificateDetails.InnerText = "Aircraft Certificate Details" & " [" & mMachine.MachineCertificates.Item(ID).CertificateName & "]"
        txtWarningDays.Text = mMachine.MachineCertificates.Item(ID).WarningDays
        calEffectiveDate.Text = mMachine.MachineCertificates.Item(ID).EffectiveDateFormatted.ToString 'Added By Prashant 19-Jun-2020 ALL18062020-1
        upnlAircraftCertificateDetails.Update()
    End Sub
    Private Sub SetPage()
          lblResult.Text = "List of Certificates: " & mMachine.MachineCertificates.Count & " Record(s) found"
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        SetObject()
        If Not mMachine.IsValid Then
            For i As Integer = 0 To mMachine.MachineCertificates.CurrentItem.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mMachine.MachineCertificates.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMSG.Trim <> "" Then
            cvDate.ErrorMessage = strMSG
            cvDate.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub ControlVisibilityForAttachment()
        If CInt(Session("Size")) > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
        upnlFileupload.Update()
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgCertificateList.Rows.Count - 1
            P = CType(Me.dgCertificateList.Rows.Item(j).Cells(14).Text, Boolean)
            If P = False Then
                dgCertificateList.Rows.Item(j).Cells(13).Enabled = False
            End If
        Next
    End Sub
    Private Sub UpdatePanel()
        upnlAircraftCertificateDetails.Update()
        upnlAdd.Update()
        upnlGridView.Update()
        upnlBack.Update()
    End Sub
    Private Sub SetControlsToBlank()
        txtName.Text = ""
        txtNo.Text = ""
        calIssueDate.Text = ""
        calExpiryDate.Text = ""
        chkApplicable.Checked = False
        txtRemark.Text = ""
        chkOneTimeCertificate.Checked = False
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        txtWarningDays.Text = "0"
        calEffectiveDate.Text = ""
        upnlAircraftCertificateDetails.Update()
    End Sub
    Private Sub SetGridView()
        dgCertificateList.DataSource = mMachine.MachineCertificates
        dgCertificateList.DataBind()
        SetGrid()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Session("mMachineCertificateEdit") = True Then
            mAttachToID = Session("mAttachToID")
            EditRecord(mAttachToID)
        End If
        dgCertificateList.DataSource = mMachine.MachineCertificates
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If

            DataFieldBind()


            SetControlsToBlank()
            lblAircraftCertificateDetails.InnerText = "Aircraft Certificate Details [NEW]"

            SetPage()
            SetGrid()
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If Not IsValid Then upnlValidation.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True) : Exit Sub

        If Session("mMachineCertificateEdit") = False Then
            mMachine.MachineCertificates.Add(mMachine.ID, txtName.Text.Trim, txtNo.Text, calIssueDate.Text.ToString, _
                                             calExpiryDate.Text.ToString, chkApplicable.Checked, txtRemark.Text, chkOneTimeCertificate.Checked, _
                                             calEffectiveDate.Text.ToString)
            If Not CustomValidate1() Then
                mMachine.MachineCertificates.Remove(mMachine.MachineCertificates.CurrentItem)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                upnlValidation.Update()
                Exit Sub
            End If

            For i As Integer = 0 To mMachine.MachineCertificates.Count - 1
                mMachine.MachineCertificates(i).SerialNo = i + 1
            Next
             If CInt(Session("Size")) > 0 Then
                mMachine.MachineCertificates.CurrentItem.IsAttachmentAdded = True
                mMachine.MachineCertificates.CurrentItem.FileAttachments.Add(mMachine.MachineCertificates.CurrentItem.ID, Session("ImageFile"), CInt(Session("Size")), Session("Extension"))
                AttachmentSessionRemove()
            End If
        Else
            SetObject()
            If Not CustomValidate1() Then
                upnlValidation.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                Exit Sub
            End If
            SetFileObject()
            Session("mMachineCertificateEdit") = False
        End If
        Session("mMachine") = mMachine
        SetControlsToBlank()
        lblAircraftCertificateDetails.InnerText = "Aircraft Certificate Details [NEW]"
        setFocus(txtName)
        DataFieldBind()
        SetPage()
        SetGrid()
        ControlVisibilityForAttachment()
        UpdatePanel()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub

    Private Sub dgCertificateList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCertificateList.PageIndexChanging
        dgCertificateList.PageIndex = e.NewPageIndex
        dgCertificateList.DataSource = mMachine.MachineCertificates
        dgCertificateList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgCertificateList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCertificateList.RowCommand
         Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                SetGridView()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                mMachine.MachineCertificates.CurrentIndex = Index
                Session("mMachine") = mMachine
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index As Int32 = CInt(e.CommandArgument) + dgCertificateList.PageIndex * dgCertificateList.PageSize
                SetGridView()
                If mMachine.MachineCertificates(index).IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachmentChild(mMachine.MachineCertificates(index).ID)
                    Dim path As String = AppSettings("DOCPath") & StrName & mMachine.MachineCertificates(index).FileAttachments(0).Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mMachine.MachineCertificates(index).FileAttachments(0).Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mMachine.MachineCertificates(index).FileAttachments(0).ImageFile, 0, mMachine.MachineCertificates(index).FileAttachments(0).ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str1 As String
                        Str1 = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str1, True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Case "EditRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
                mMachine.MachineCertificates.CurrentIndex = Index
                Dim mID As Guid = mMachine.MachineCertificates(Index).ID
                mAttachToID = mID
                EditRecord(mID)
                setFocus(txtName)
                SetGridView()
                Session("mMachineCertificateEdit") = True
                Session("mAttachToID") = mAttachToID
                Session("mMachine") = mMachine
        End Select
        upnlValidation.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If CInt(Session("Size")) > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & Session("Extension")
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & Session("Extension"))
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(Session("ImageFile"), 0, Session("ImageFile").Length)
                fs.Close()
                Session("DOCPath") = path
            End If
        Else
            If mMachine.MachineCertificates.CurrentItem.IsAttachmentAdded = True Then
                mFileAttach = FileAttach.GetAttachment(mMachine.MachineCertificates.CurrentItem.ID)
            End If
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
            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        If CInt(Session("Size")) > 0 Then
            AttachmentSessionRemove()
        Else
            mMachine.MachineCertificates.CurrentItem.IsAttachmentAdded = False
            mMachine.MachineCertificates.CurrentItem.FileAttachments.RemoveAt(0)
        End If
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        upnlAircraftCertificateDetails.Update()
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        RemoveSession()
        AttachmentSessionRemove()
        ' Response.Redirect("wfMachine.aspx?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgCertificateList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCertificateList.Sorting
        mMachine.MachineCertificates.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        SetGridView()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class