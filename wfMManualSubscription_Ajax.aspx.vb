Public Class wfMManualSubscription_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mManual As Manual
    Public mAttachToID As Guid
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mManual = CType(Session("mManual"), Manual)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub SetSession()
        Session("mManual") = mManual
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mManualSubscription")
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
        If txtFromDate.Text = "" Then
            mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).FromDate = ""
        Else
            mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).FromDate = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        End If
        If txtToDate.Text = "" Then
            mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).ToDate = ""
        Else
            mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).ToDate = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))
        End If
        mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).Remark = txtRemark.Text
    End Sub
    Private Sub SetFileObject()
        If CInt(Session("Size")) > 0 Then
            mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).IsAttachment = True
            If mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).FileAttachments.Count = 0 Then
                mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).FileAttachments.Add(mManual.MManualSubscriptions.CurrentItem.ID, Session("ImageFile"), CInt(Session("Size")), Session("Extension"))
            Else
                mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).FileAttachments(0).ImageFile = Session("ImageFile")
                mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).FileAttachments(0).Size = CInt(Session("Size"))
                mManual.MManualSubscriptions.Item(mManual.MManualSubscriptions.CurrentIndex).FileAttachments(0).Extension = Session("Extension")
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
                            'Dim mAircraftDSCList As DailyStatusList
                            'mAircraftDSCList = DailyStatusList.GetDailyStatusList(mManual.MManualSubscriptions(mManual.MManualSubscriptions.CurrentIndex).ManualID, Guid.Empty.ToString, Guid.Empty.ToString, 7, True)
                            'If mAircraftDSCList.Contains(mManual.MManualSubscriptions(mManual.MManualSubscriptions.CurrentIndex).ID, "") Then
                            '    MSGBoxCtrl.show("Reference Delete!", "This certificate is added in Aircraft Daily Status. Please do not delete this Entry.", "", MsgBoxStyle.OkOnly, "")
                            '    Exit Sub
                            'End If
                            mManual.MManualSubscriptions.Remove(mManual.MManualSubscriptions(mManual.MManualSubscriptions.CurrentIndex))
                            For i As Integer = 0 To mManual.MManualSubscriptions.Count - 1
                                mManual.MManualSubscriptions(i).SrNo = i + 1
                            Next
                            Session("mManual") = mManual
                            Session("mManualSubscription") = False
                            SetControlsToBlank()
                            DataFieldBind()
                            SetPage()
                            SetGrid()
                            ControlVisibilityForAttachment()
                            UpdatePanel()

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
        txtFromDate.Text = mManual.MManualSubscriptions.Item(ID).FromDate.ToString
        txtToDate.Text = mManual.MManualSubscriptions.Item(ID).ToDate.ToString
        txtRemark.Text = mManual.MManualSubscriptions.Item(ID).Remark
        If mManual.MManualSubscriptions.Item(ID).IsAttachment = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If

        upnlManualPropertyDetails.Update()
    End Sub
    Private Sub SetPage()
        lblResult.Text = "List of Manual Subscriptions: " & mManual.MManualSubscriptions.Count & " Record(s) found"
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        SetObject()
        If Not mManual.IsValid Then
            For i As Integer = 0 To mManual.MManualSubscriptions.CurrentItem.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mManual.MManualSubscriptions.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
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
        upnlAttach.Update()
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgManualSubscriptionList.Rows.Count - 1
            P = CType(Me.dgManualSubscriptionList.Rows.Item(j).Cells(7).Text, Boolean)
            If P = False Then
                dgManualSubscriptionList.Rows.Item(j).Cells(6).Enabled = False
            End If
        Next
    End Sub
    Private Sub UpdatePanel()
        upnlManualPropertyDetails.Update()
        upnlAdd.Update()
        upnlGridView.Update()
        upnlBack.Update()
    End Sub
    Private Sub SetControlsToBlank()
        txtFromDate.Text = ""
        txtToDate.Text = ""
        txtRemark.Text = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        upnlManualPropertyDetails.Update()
    End Sub
    Private Sub SetGridView()
        dgManualSubscriptionList.DataSource = mManual.MManualSubscriptions
        dgManualSubscriptionList.DataBind()
        SetGrid()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Session("mManualSubscription") = True Then
            mAttachToID = Session("mAttachToID")
            EditRecord(mAttachToID)
        End If
        dgManualSubscriptionList.DataSource = mManual.MManualSubscriptions
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            SetControlsToBlank()
            SetPage()
            SetGrid()
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("ManualNew") And mManual.IsNew) Or (Not User.IsInRole("ManualEdit") And Not mManual.IsNew) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If Session("mManualSubscription") = False Then
            mManual.MManualSubscriptions.Add(ManualID:=mManual.ID, FromDate:=txtFromDate.Text.ToString, ToDate:=txtToDate.Text.ToString, Remark:=txtRemark.Text)
            If Not CustomValidate1() Then
                mManual.MManualSubscriptions.Remove(mManual.MManualSubscriptions.CurrentItem)
                upnlValidationSummary.Update()
                Exit Sub
            End If

            For i As Integer = 0 To mManual.MManualSubscriptions.Count - 1
                mManual.MManualSubscriptions(i).SrNo = i + 1
            Next
            If CInt(Session("Size")) > 0 Then
                mManual.MManualSubscriptions.CurrentItem.IsAttachment = True
                mManual.MManualSubscriptions.CurrentItem.FileAttachments.Add(mManual.MManualSubscriptions.CurrentItem.ID, Session("ImageFile"), CInt(Session("Size")), Session("Extension"))
                AttachmentSessionRemove()
            End If
        Else
            SetObject()
            If Not CustomValidate1() Then
                upnlValidationSummary.Update()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                Exit Sub
            End If
            SetFileObject()
            Session("mManualSubscription") = False
        End If
        Session("mManual") = mManual
        SetControlsToBlank()
        DataFieldBind()
        SetPage()
        SetGrid()
        ControlVisibilityForAttachment()
        UpdatePanel()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub dgManualSubscriptionList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManualSubscriptionList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgManualSubscriptionList.PageSize * dgManualSubscriptionList.PageIndex
                If (Not User.IsInRole("ManualNew") And mManual.IsNew) Or (Not User.IsInRole("ManualEdit") And Not mManual.IsNew) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                SetGridView()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                mManual.MManualSubscriptions.CurrentIndex = Index
                Session("mManual") = mManual
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index As Int32 = CInt(e.CommandArgument) + dgManualSubscriptionList.PageIndex * dgManualSubscriptionList.PageSize
                SetGridView()
                If mManual.MManualSubscriptions(index).IsAttachment Then
                    mFileAttach = FileAttach.GetAttachmentChild(mManual.MManualSubscriptions(index).ID)
                    Dim path As String = AppSettings("DOCPath") & StrName & mManual.MManualSubscriptions(index).FileAttachments(0).Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mManual.MManualSubscriptions(index).FileAttachments(0).Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mManual.MManualSubscriptions(index).FileAttachments(0).ImageFile, 0, mManual.MManualSubscriptions(index).FileAttachments(0).ImageFile.Length)
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
                Dim Index As Int32 = CInt(e.CommandArgument) + dgManualSubscriptionList.PageSize * dgManualSubscriptionList.PageIndex
                mManual.MManualSubscriptions.CurrentIndex = Index
                Dim mID As Guid = mManual.MManualSubscriptions(Index).ID
                mAttachToID = mID
                EditRecord(mID)
                SetGridView()
                Session("mManualSubscription") = True
                Session("mAttachToID") = mAttachToID
                Session("mManual") = mManual
        End Select
        upnlValidationSummary.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
        upnlAttach.Update()
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
            If mManual.MManualSubscriptions.CurrentItem.IsAttachment = True Then
                'mFileAttach = FileAttach.GetAttachment(mManual.MManualSubscriptions.CurrentItem.ID)
                mFileAttach = FileAttach.GetAttachmentChild(mManual.MManualSubscriptions.CurrentItem.ID)
            End If
            'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim path As String = AppSettings("DOCPath") & StrName & mManual.MManualSubscriptions(mManual.MManualSubscriptions.CurrentItem.ID).FileAttachments(0).Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                'System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mManual.MManualSubscriptions(mManual.MManualSubscriptions.CurrentItem.ID).FileAttachments(0).Extension)
                ' Create the file.
                'fs = File.Create(path)
                ''' Add some information to the file.
                'fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mManual.MManualSubscriptions(mManual.MManualSubscriptions.CurrentItem.ID).FileAttachments(0).ImageFile, 0, mManual.MManualSubscriptions(mManual.MManualSubscriptions.CurrentItem.ID).FileAttachments(0).ImageFile.Length)
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
            mManual.MManualSubscriptions.CurrentItem.IsAttachment = False
            mManual.MManualSubscriptions.CurrentItem.FileAttachments.RemoveAt(0)
        End If
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        upnlManualPropertyDetails.Update()
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        SetSession()
        RemoveSession()
        AttachmentSessionRemove()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentCallback", "CallParentCallback();", True)
    End Sub
    'Private Sub dgManualSubscriptionList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgManualSubscriptionList.Sorting
    '    mManual.MManualSubscriptions.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    '    SetGridView()
    'End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region


End Class