Public Class wfDDImages_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mDisplayImage As DisplayImage
    Public mDisplayImageList As DisplayImageList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDisplayImage = CType(Session("mDisplayImage"), DisplayImage)
        mDisplayImageList = CType(Session("mDisplayImageList"), DisplayImageList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDisplayImage")
        Session.Remove("mDisplayImageList")
    End Sub
    Private Sub NewRecord()
        mDisplayImage = DisplayImage.NewImage()
        Session("mDisplayImage") = mDisplayImage
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDisplayImage = DisplayImage.GetImage(mId)
        Session("mDisplayImage") = mDisplayImage
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        EditRecord(mId)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub setObject()
        mDisplayImage.IsSetAsHomeScreen = chkSetAsHomeScrren.Checked
        Session("mDisplayImage") = mDisplayImage
    End Sub
    Private Sub MakeControlsBlank()

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mDisplayImage = Session("mDisplayImage")
                            DisplayImage.DeleteAttachment(mDisplayImage.ID)
                            MarkLog(Util.Action.Delete, "DisplayImage", mDisplayImage.FileName, Util.ErrorType.NoError, mDisplayImage.ID, EventLogID)
                            NewRecord()
                            MakeControlsBlank()
                            DataFieldBind()
                            upnlGridView.Update()
                            upnlAttachFile.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                DataFieldBind()
                                MakeControlsBlank()
                                upnlGridView.Update()
                                upnlAttachFile.Update()
                                Exit Sub
                            End If
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then

                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then

                    End If
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDisplayImageList = DisplayImageList.GetImagesList()
        Session("mDisplayImageList") = mDisplayImageList
        dgImageList.DataSource = mDisplayImageList
        DataBind()
        lblResult.Text = "Image List: " & mDisplayImageList.Count & " Record(s) Found."
    End Sub
    Private Sub ControlVisibility()
        If mDisplayImageList.Count > 15 Then
            btnBackTop.Visible = True
            btnSaveTop.Visible = True
        Else
            btnBackTop.Visible = False
            btnSaveTop.Visible = False
        End If
        If mDisplayImage.Size > 0 Then
            ImageButton1.Visible = True
            btnSelectFile.Disabled = True
        Else
            ImageButton1.Visible = False
            btnSelectFile.Disabled = False
        End If
    End Sub
    Private Sub AttachFile()
        '  If MyFile1.Value <> "" Then
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"

        Try
            NewRecord()
            mDisplayImage.FileName = Session("FileUpload.FileName")
            mDisplayImage.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mDisplayImage.Size = Session("FileUpload.FileSize")
            mDisplayImage.Extension = Session("FileUpload.FileExtension")

            Session("mDisplayImage") = mDisplayImage

            
        Catch ex As Exception
        End Try
    End Sub
    Private Sub View()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mDisplayImage.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mDisplayImage.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mDisplayImage.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mDisplayImage.ImageFile, 0, mDisplayImage.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                NewRecord()
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        Else
            MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
            ControlVisibility()
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            NewRecord()
            Session("MiddleFrame") = "wfDDImages_Ajax.aspx"
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
        If (Not User.IsInRole("UploadImagesNew") And mDisplayImage.IsNew) Or (Not User.IsInRole("UploadImagesEdit") And Not mDisplayImage.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            If IsValid Then
                If mDisplayImage.Size <= 0 Then
                    MSGBoxCtrl.show("Alert!", "Please select file first and then click on Save", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                setObject()
                mDisplayImage.Save()
                MarkLog(Util.Action.Save, "DisplayImage", mDisplayImage.FileName.ToString, Util.ErrorType.HandledError, mDisplayImage.ID, EventLogID)
                NewRecord()
                MakeControlsBlank()
                DataFieldBind()
                ControlVisibility()
                upnlGridView.Update()
                upnlAttachFile.Update()
            End If

        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End Try
    End Sub
    'Private Sub dgEventDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgImageList.PageIndexChanging
    '    dgImageList.PageIndex = e.NewPageIndex
    '    dgImageList.DataSource = mDisplayImageList
    '    Session("mDisplayImageList") = mDisplayImageList
    '    dgImageList.DataBind()
    'End Sub
    Private Sub dgEventDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgImageList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                If (Not User.IsInRole("UploadImagesDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Dim index As Integer = CInt(e.CommandArgument) + dgImageList.PageIndex * dgImageList.PageSize
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                DeleteRecord(mID)
            Case "ViewRec"
                If (Not User.IsInRole("UploadImagesDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Dim index As Integer = CInt(e.CommandArgument) + dgImageList.PageIndex * dgImageList.PageSize
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mID)
                View()
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        ControlVisibility()
        DataBind()
        MarkLog(Util.Action.[New], "DisplayImage", "", Util.ErrorType.NoError, mDisplayImage.ID, EventLogID)
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        View()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
        Session("sender") = ""
        MarkLog(Util.Action.Close, "DisplayImage", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachFile()
        ControlVisibility()
        upnlAttachFile.Update()
    End Sub
#End Region

    
End Class