Public Class wfFileAttachmentAndOtherInfo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mOrder As Order
    Dim EventLogID As Guid
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mOrder = CType(Session("mOrder"), Order)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub SetSession()
        Session("mOrder") = mOrder
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
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
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub ControlVisibility()
         ControlVisibilityForAttachment()
    End Sub
    Private Sub SetObject()
        mOrder = Session("mOrder")
        With mOrder
             .Remark = txtRemark.Text
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If
        End With
        Session("mOrder") = mOrder
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
        If mOrder.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mOrder.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Function SaveAttachment() As Boolean
        If mFileAttach.Size > 0 Then
            Try
                mFileAttach.Save()
                Return True
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
            End Try
        Else
            If (Not mOrder.IsNew) And IsAttachmentDeleted Then
                FileAttach.DeleteAttachment(mFileAttach.ID, mOrder.ID)
            End If
            IsAttachmentDeleted = False
            Session("IsAttachmentDeleted") = IsAttachmentDeleted
            Return False
        End If
    End Function
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
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 100 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 100 char."
                e.IsValid = False
            Else
                e.IsValid = True
            End If     
        End If
    End Sub
     Private Sub DataFieldBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
         GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If txtRemark.Enabled = True Then
                setFocus(txtRemark)
            End If
            DataFieldBind()
            GetAttachment()
        End If
        ControlVisibility()
    End Sub
    'Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
    '    If IsValid Then
    '        Try
    '            If SaveAttachment() = True Then
    '                mOrder.UpdateInfromation(mOrder.ID, txtRemark.Text.Trim, IsAttachmentAdded:=True)
    '            Else
    '                mOrder.UpdateInfromation(mOrder.ID, txtRemark.Text.Trim, IsAttachmentAdded:=False)
    '            End If
    '            mOrder = Order.GetOrder(mOrder.ID)
    '            Session("mOrder") = mOrder
    '        Catch ex As SqlException
    '        Finally
    '        End Try
    '    Else
    '        upnlValidationsummary.Update()
    '    End If
    '    Dim mopenas As String = Request.QueryString("Type")
    '    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
    '        Exit Sub
    '    End If
    'End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            Try
                If Not mFileAttach Is Nothing Then
                    If mFileAttach.Size > 0 Then
                        Try
                            mFileAttach.Save()
                        Catch ex As Exception
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                        End Try
                    Else
                        If (Not mOrder.IsNew) And IsAttachmentDeleted Then
                            FileAttach.DeleteAttachment(mFileAttach.ID, mOrder.ID)
                        End If
                        IsAttachmentDeleted = False
                        Session("IsAttachmentDeleted") = IsAttachmentDeleted
                    End If
                End If

                With mOrder
                    .Remark = txtRemark.Text.Trim
                    If Not mFileAttach Is Nothing Then
                        If mFileAttach.Size > 0 Then
                            .IsAttachmentAdded = True
                        Else
                            .IsAttachmentAdded = False
                        End If
                    End If
                End With
                mOrder.Save()
                Session("mOrder") = mOrder
            Catch ex As SqlException
            Finally
            End Try
        Else
            upnlValidationsummary.Update()
        End If
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session("BackOrSaveFromwfFileAttachmentAndOtherInfo_Ajax") = "Back"
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
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mOrder.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mOrder.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mOrder.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
#End Region
End Class