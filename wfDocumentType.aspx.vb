Partial Class wfDocumentType
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblDocumentTypeDetails As System.Web.UI.WebControls.Label
    Protected WithEvents lblSearchByDocumentType As System.Web.UI.WebControls.Label
    Protected WithEvents lblDocumentTypeName As System.Web.UI.WebControls.Label

    '' Protected WithEvents dgDocumentTypeListList As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mDocumentType As DocumentType
    Public mDocumentTypeList As DocumentTypeList
    Public mDocumentTypeForList As DocumentTypeForList

    Public Type As Integer = 0
    Public mDocumentTypeForID As Integer
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        Type = Val(Session("Type"))
        mDocumentType = CType(Session("mDocumentType"), DocumentType)
        mDocumentTypeList = CType(Session("mDocumentTypeList"), DocumentTypeList)
        mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
    End Sub
    Private Sub SetSession()
        Session("mDocumentType") = mDocumentType
        Session("mDocumentTypeList") = mDocumentTypeList
        Session("Type") = Type
        Session("mDocumentTypeForID") = mDocumentTypeForID
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDocumentType.aspx?" And Session("Type") <> "1" Then
            Session.Remove("mDocumentType")
            Session.Remove("mDocumentTypeList")
        End If
    End Sub
    Private Sub OpenList(Optional ByVal Name As String = "")
        mDocumentTypeList = DocumentTypeList.GetDocumentTypeList(Name, mDocumentTypeForID)
        dgDocumentTypeList.DataSource = mDocumentTypeList
        dgDocumentTypeList.DataBind()
        Session("mDocumentTypeList") = mDocumentTypeList
    End Sub
    Private Sub NewRecord()
        mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
        mDocumentType = DocumentType.NewDocumentType(Guid.NewGuid)
        mDocumentTypeList = DocumentTypeList.GetDocumentTypeList(, mDocumentTypeForID)
        mDocumentType.DocumentTypeForID = mDocumentTypeForID
        SetSession()
        Session("mDocumentType") = mDocumentType
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDocumentType = DocumentType.GetDocumentType(mId)
        Session("mDocumentType") = mDocumentType
        txtName.DataBind()
        txtCode.DataBind()
        cmbDocumentTypeFor.DataBind()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type
        Session("sender") = "Delete"
        msg1.Show()
        mDocumentType = DocumentType.GetDocumentType(mId)
        Session("mDocumentType") = mDocumentType
    End Sub
    Private Sub setObject()
        mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
        mDocumentType.Name = Trim(txtName.Text)
        mDocumentType.Code = Trim(txtCode.Text)
        mDocumentType.DocumentTypeForID = mDocumentTypeForID
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtCode" Then
            If Len(txtCode.Text.Trim) > 4 Then
                custValidator.ErrorMessage = "Code should not be greater than 4 Characters."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtName" Then
            If Len(txtName.Text.Trim) > 25 Then
                custValidator.ErrorMessage = "Name too long."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Session("sender") = ""
                            mDocumentType = CType(Session("mDocumentType"), DocumentType)
                            DocumentType.DeleteDocumentType(mDocumentType.ID)
                            Response.Redirect("wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                'msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&Type=" & Type
                                msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type
                                MarkLog(Util.Action.Delete, "Document Type", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mDocumentType.ID, EventLogID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Document Type", mDocumentType.Name, Util.ErrorType.NoError, mDocumentType.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'Response.Redirect("wfDocumentType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Type)
                    Response.Redirect("wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type)
                Case MsgBoxResult.OK ''And Session("sender") = ""        
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type)
                Case MsgBoxResult.OK And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type)
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            Response.Redirect("wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type)
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If Not mDocumentType.IsNew Then
            If Len(mDocumentType.Name) > 15 Then
                lbltitle.Text = "Document Type [" & mDocumentType.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Document Type [" & mDocumentType.Name & "]"
            End If
        Else
            lbltitle.Text = "Document Type [New]"
        End If
        lblResult.Text = "Document Type List: " & mDocumentTypeList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        mDocumentTypeForList = DocumentTypeForList.GetDocumentTypeForList()
        cmbDocumentTypeFor.DataSource = mDocumentTypeForList

        mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
        mDocumentTypeList = DocumentTypeList.GetDocumentTypeList(, mDocumentTypeForID)
        dgDocumentTypeList.DataSource = mDocumentTypeList

        'Session("mDocumentTypeList") = mDocumentTypeList
        DataBind()
        lblResult.Text = "Document Type List: " & mDocumentTypeList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            NewRecord()
            Session("mDocumentTypeForID") = mDocumentTypeForID
            SetTitle()
            DataFieldBind()
        End If

        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        ClearAll()
        MarkLog(Util.Action.Close, "DocumentType", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Response.Redirect(Request.QueryString("BackPage1") & "?MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ''If (Not User.IsInRole("DocumentTypeNew") And mDocumentType.IsNew) Or (Not User.IsInRole("DocumentTypeEdit") And Not mDocumentType.IsNew) Then
        ''    setObject()
        ''    SetSession()
        ''    'MarkLog(Util.Action.Save, "DocumentType", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        'MarkLog(Util.Action.Save, "Document Type", User.Identity.Name & " is not Authorized User to save " & mDocumentType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage ="wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&Type=" & Type
        ''    Session("sender") = "Authorization"
        ''    msg.Show()
        ''    Exit Sub
        ''End If
        If IsValid Then
            Try
                GetSession()
                setObject()
                mDocumentType = CType(mDocumentType.Save(), DocumentType)
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                MarkLog(Util.Action.Save, "Document Type", mDocumentType.Name, Util.ErrorType.NoError, mDocumentType.ID, EventLogID)
                NewRecord()
                OpenList()
                Session("mDocumentTypeForID") = mDocumentTypeForID
                txtName.DataBind()
                txtCode.DataBind()
                SetSession()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                SetTitle()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type
                    Session("sender") = "Delete"
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type
                    Session("sender") = "Delete"
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Type
                    msg1.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&Type=" & Type
                    Session("sender") = "Delete"
                    msg1.Show()
                End If
            End Try
        End If
    End Sub
    Private Sub dgDocumentTypeList_ItemCommand1(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDocumentTypeList.ItemCommand
        Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "View"
                ''If (Not User.IsInRole("DocumentTypeView") And Not User.IsInRole("DocumentTypeEdit")) Then
                ''    setObject()
                ''    SetSession()
                ''    'MarkLog(Util.Action.Edit, "DocumentType", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                'MarkLog(Util.Action.Edit, "Document Type", User.Identity.Name & " is not Authorized User to edit " & mDocumentType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&Type=" & Type
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                EditRecord(mId)
                MarkLog(Util.Action.Edit, "Document Type", mDocumentType.Name, Util.ErrorType.NoError, mDocumentType.ID, EventLogID)
                SetTitle()
            Case "Delete"
                ''If (Not User.IsInRole("DocumentTypeDelete")) Then
                ''    setObject()
                ''    SetSession()
                ''    'MarkLog(Util.Action.Delete, "DocumentType", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                'MarkLog(Util.Action.Delete, "Document Type", User.Identity.Name & " is not Authorized User to delete " & mDocumentType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage ="wfDocumentType.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&Type=" & Type
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        OpenList(Trim(txtSearch.Text))
        lblResult.Text = "DocumentType List: " & mDocumentTypeList.Count & " Record(s) Found."
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        GetSession()
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        NewRecord()
        MarkLog(Util.Action.[New], "Document Type", "", Util.ErrorType.NoError, mDocumentType.ID, EventLogID)
        OpenList()
        txtName.DataBind()
        txtCode.DataBind()
        SetTitle()
        SetSession()
    End Sub

#End Region

End Class
