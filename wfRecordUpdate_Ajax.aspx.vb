Public Class wfRecordUpdate_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mOrderItemID As Guid
    Public mReceiptItemID As Guid
    Dim EventLogID As Guid
    Private mFileAttach As FileAttach
    Dim mItemName, mReceiptItemSerialNo As String
    Private IsAttachmentAdded As Boolean = False
    Private IsLoanTransaction As Boolean = False
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mOrderItemID = Session("mOrderItemID")
        mReceiptItemID = Session("mReceiptItemID")
        mItemName = Session("mItemName")
        mReceiptItemSerialNo = Session("mReceiptItemSerialNo")
        mFileAttach = Session("mFileAttach")
        IsAttachmentAdded = Session("IsAttachmentAdded")
        IsLoanTransaction = Session("IsLoanTransaction")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mOrderItemID")
        Session.Remove("mReceiptItemID")
        Session.Remove("mItemName")
        Session.Remove("mReceiptItemSerialNo")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentAdded")
        Session.Remove("IsLoanTransaction")
    End Sub
    Private Sub GetAttachment()
        If IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mReceiptItemID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
#End Region

#Region " DataBind Methods "

    Public Sub DataFieldBind()

        DataBind()
    End Sub
    Private Sub ControlVisibilityForFileAttachment()
        If IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibilityForFileAttachment()
        End If
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        'mFileAttach = Nothing
        GetAttachment()
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        GetAttachment()
        IsAttachmentAdded = False
        Session("IsAttachmentAdded") = IsAttachmentAdded
        'mEmployee.ImageFile = file1
        'mEmployee.ImageSize = 0
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mReceiptItemID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mReceiptItemID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
        IsAttachmentAdded = True
        Session("IsAttachmentAdded") = IsAttachmentAdded
        ControlVisibilityForFileAttachment()
        upnlUdateRecord.Update()
    End Sub
    Private Sub btnLocationClose_Click(sender As Object, e As System.EventArgs) Handles btnLocationClose.Click
        MarkLog(Util.Action.Close, "ExchangeRepairOverhaulOrderRecordUpdate", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnLocationOk_Click(sender As Object, e As System.EventArgs) Handles btnLocationOk.Click
        If IsValid Then
            Try
                If IsAttachmentAdded Then
                    If mFileAttach Is Nothing Then
                        mFileAttach = FileAttach.GetAttachment(mReceiptItemID)
                        Session("mFileAttach") = mFileAttach
                    End If
                    ExchangeRepairOverhaulOrderRecordsList.UpdateExchangeRepairOverhaulOrderRecord(mReceiptItemID, mOrderItemID, txtRemark.Text.Trim, IsConvertToOutright:=True, IsLoanTransaction:=IsLoanTransaction, IsAttachmentAdded:=True, Extension:=mFileAttach.Extension, ImageFile:=mFileAttach.ImageFile, Size:=mFileAttach.Size)
                Else
                    ExchangeRepairOverhaulOrderRecordsList.UpdateExchangeRepairOverhaulOrderRecord(mReceiptItemID, mOrderItemID, txtRemark.Text.Trim, IsConvertToOutright:=True, IsLoanTransaction:=IsLoanTransaction, IsAttachmentAdded:=False)
                End If
                Session("IsRecordSaved") = "True"
                MarkLog(Util.Action.Save, "ExchangeRepairOverhaulOrderRecordUpdate", "Part No : " + mItemName + "Serial No : " + mReceiptItemSerialNo + " Remark : " + txtRemark.Text.Trim + " Also attachment added in Receipt Item", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                RemoveSession()
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region

   
   
End Class