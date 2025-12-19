Public Class wfManualLastRevAttachmentList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mRevision As Revision
    Dim EventLogID As Guid
    Dim mFileAttachments As New FileAttachments
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mRevision = Session("mRevision")
        mFileAttachments = Session("mFileAttachments")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRevision")
        Session.Remove("mFileAttachments")
    End Sub
   
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                   
                Case MsgBoxResult.Ok
                    
                Case MsgBoxResult.No

            End Select
        End If
    End Sub
    Private Sub DataFieldBind(Optional ByVal GetList As Boolean = True)
         If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblTitle.Text = "Subscription " & "[" & mRevision.No & "]"
        Else
            lblTitle.Text = "Revision " & "[" & mRevision.No & "]"
        End If
        ' dgManRevisionAttachment.DataSource = mRevision.FileAttachments
        dgManRevisionAttachment.DataSource = mFileAttachments
        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            DataFieldBind()
            'ControlVisibilityForAttachment()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub dgManRevisionAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManRevisionAttachment.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                'mFileAttachments = mRevision.FileAttachments
                mFileAttachments = Session("mFileAttachments")

                'mFileAttachments.CurrentIndex = Index - 1

                If mFileAttachments.Count = 1 Then
                    mFileAttachments.CurrentIndex = 0
                Else
                    mFileAttachments.CurrentIndex = Index - 1
                End If

                If mFileAttachments.CurrentItem.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                        Dim Detail As String = "Manual Revision Attachment( " + mFileAttachments.CurrentItem.FileName + ") viewed by  " + User.Identity.Name
                        MarkLog(Util.Action.View, "ManualRevision", Detail, Util.ErrorType.HandledError, mFileAttachments.CurrentItem.ReferenceID, EventLogID)
                    End If
                End If
        End Select

    End Sub
#End Region

End Class