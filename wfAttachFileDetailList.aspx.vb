Partial Class wfAttachFileDetailList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable declaration "
    Public mAttachFileDetailList As AttachFileDetailList
    Public mAttachFileDetail1 As AttachFileDetail
    Public IsSelected As Boolean
    Public mDocumentTypeForID As Integer
#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mAttachFileDetail1 = CType(Session("mAttachFileDetail"), AttachFileDetail)
        mAttachFileDetailList = CType(Session("mAttachFileDetailList"), AttachFileDetailList)
        mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
    End Sub
    Private Sub SetSession()
        Session("mAttachFileDetail1") = mAttachFileDetail1
        Session("mAttachFileDetailList") = mAttachFileDetailList
        Session("mDocumentTypeForID") = mDocumentTypeForID
    End Sub
    Private Sub FindNow(Optional ByVal Name As String = "")
        GetSession()
        mAttachFileDetailList = AttachFileDetailList.GetAttachFileDetailList(Name, mDocumentTypeForID)
        'Set DataSource of the Grid
        Me.dgAttachFileList.DataSource = mAttachFileDetailList
        dgAttachFileList.DataBind()
        lblResult.Text = "Attach File List: " & mAttachFileDetailList.Count & " record(s) found."
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            FindNow()
            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
        End If

    End Sub
    Private Sub dgAttachFileList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAttachFileList.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Dim mID As New Guid(e.Item.Cells(0).Text)
                mAttachFileDetail1 = AttachFileDetail.GetAttachFileDetail(mID)
                IsSelected = True
                Session("mAttachFileDetail1") = mAttachFileDetail1
                Session("IsSelected") = IsSelected
                'Response.Redirect(Request.QueryString("BackPage2") & "?MainBackPage=" & Request.QueryString("MainBackPage"))
                Response.Redirect(Request.QueryString("BackPage2") & "?MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow(Trim(txtSearch.Text))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("sender") = "Existing"
        'MarkLog(Util.Action.Close, "AttachFileDetail", "", Util.ErrorType.NoError, Guid.Empty)
        'Response.Redirect(Request.QueryString("BackPage2") & "?MainBackPage=" & Request.QueryString("MainBackPage"))
        Response.Redirect(Request.QueryString("BackPage2") & "?MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
    End Sub
#End Region

End Class
