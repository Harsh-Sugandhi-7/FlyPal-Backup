Public Class wfCompanyDocumentHistoryList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declarations"
    Protected mCompanyDocumentHistoryList As CompanyDocumentHistoryList
    Public mCompanyDocument As CompanyDocument
    Dim mVendorID As String
    Dim mDocumentID As String
    Dim mReferenceID As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCompanyDocumentHistoryList = Session("mCompanyDocumentHistoryList")
        mCompanyDocument = Session("mCompanyDocument")
        mVendorID = Session("mVendorID")
    End Sub
    Private Sub SetSession()
        Session("mCompanyDocumentHistoryList") = mCompanyDocumentHistoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCompanyDocumentHistoryList")
        Session.Remove("mVendorID")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Get List From the Database as per Criteria             
        mCompanyDocumentHistoryList = CompanyDocumentHistoryList.GetCompanyDocumentHistoryList(New Guid(mVendorID.ToString), New Guid(mDocumentID), _
                                                                                               New Guid(mReferenceID), 1)
        dgCompanyDocumentHistoryList.DataSource = mCompanyDocumentHistoryList
        Session("mCompanyDocumentHistoryList") = mCompanyDocumentHistoryList
        dgCompanyDocumentHistoryList.DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            mDocumentID = mCompanyDocument.DocumentID.ToString
            mReferenceID = mCompanyDocument.ReferenceID.ToString
            DataFieldBind()
          End If
    End Sub
    Private Sub dgCompanyDocumentHistoryList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCompanyDocumentHistoryList.RowCommand
        Dim Idx As Integer
        Dim mID As Guid
        Select Case e.CommandName
            Case "View"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                Idx = rowIndex + dgCompanyDocumentHistoryList.PageIndex * dgCompanyDocumentHistoryList.PageSize
                mID = CType(dgCompanyDocumentHistoryList.DataKeys(rowIndex).Values("ID"), Guid)

                mCompanyDocument = CompanyDocument.GetCompanyDocument(mID)
                If mCompanyDocument.ImageSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mCompanyDocument.FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mCompanyDocument.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mCompanyDocument.ImageFile, 0, mCompanyDocument.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", Str, True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgCompanyDocumentHistoryList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCompanyDocumentHistoryList.Sorting
        mCompanyDocumentHistoryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgCompanyDocumentHistoryList.DataSource = mCompanyDocumentHistoryList
        Session("mCompanyDocumentHistoryList") = mCompanyDocumentHistoryList
        dgCompanyDocumentHistoryList.DataBind()
      End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
#End Region

End Class