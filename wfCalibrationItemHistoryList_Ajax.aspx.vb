Public Class wfCalibrationItemHistoryList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mCalibrationItemHistoryList As CalibrationItemHistoryList
    Dim mCalibrationItemChild As CalibrationItemChild
    Dim mFileAttach As FileAttach
    Dim EventLogID As Guid
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mCalibrationItemHistoryList = Session("mCalibrationItemHistoryList")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub setgrid()
        Dim IsAttachmentAdded As Boolean
        For j As Integer = 0 To dgCalibrationItemHistoryList.Rows.Count - 1
            IsAttachmentAdded = CType(Me.dgCalibrationItemHistoryList.Rows.Item(j).Cells(9).Text, Boolean)

            If IsAttachmentAdded = False Then
                dgCalibrationItemHistoryList.Rows.Item(j).Cells(8).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            dgCalibrationItemHistoryList.DataSource = mCalibrationItemHistoryList
            dgCalibrationItemHistoryList.DataBind()
            setgrid()
        End If

    End Sub
    Private Sub dgCalibrationItemHistoryList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCalibrationItemHistoryList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                mFileAttach = FileAttach.GetAttachment(New Guid(e.CommandArgument.ToString))
                Session("mFileAttach") = mFileAttach
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
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mFileAttach")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgCalibrationItemHistoryList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCalibrationItemHistoryList.Sorting
        mCalibrationItemHistoryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgCalibrationItemHistoryList.DataSource = mCalibrationItemHistoryList
        Session("mCalibrationItemHistoryList") = mCalibrationItemHistoryList
        dgCalibrationItemHistoryList.DataBind()
        setgrid()
    End Sub
#End Region

    
End Class