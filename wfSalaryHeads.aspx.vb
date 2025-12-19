'****************************************************************************************
'Class name : EmployeeDisciplinary
'Developed By : Saylee
'Date : 12-Jan-10
'****************************************************************************************    
Partial Class wfSalaryHeads

    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cvSalaryHeads As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents lblSalaryHeadsDetails As System.Web.UI.WebControls.Label
    '' Protected WithEvents dgSalaryHeads As System.Web.UI.WebControls.DataGrid

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
    Public mSalaryHeadsList As SalaryHeadList
    Public mSalaryHeads As SalaryHead
    Public BackPage As String
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mSalaryHeads = Session("mSalaryHeads")
        mSalaryHeadsList = Session("mSalaryHeadsList")
    End Sub
    Private Sub SetSession()
        Session("mSalaryHeads") = mSalaryHeads
        Session("mSalaryHeadsList") = mSalaryHeadsList
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub NewRecord()
        mSalaryHeads = SalaryHead.NewSalaryHead()
        Session("mSalaryHeads") = mSalaryHeads
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mSalaryHeads = SalaryHead.GetChildSalaryHead(mID)
        Session("mSalaryHeads") = mSalaryHeads
        setFocus(txtCode)
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
        Session("sender") = "Delete"
        msg1.Show()
        mSalaryHeads = SalaryHead.GetChildSalaryHead(mID)
        Session("mSalaryHeads") = mSalaryHeads
    End Sub
    Private Sub SetObject()
        mSalaryHeads.Name = txtName.Text
        mSalaryHeads.Code = txtCode.Text
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
                            mSalaryHeads = Session("mSalaryHeads")
                            SalaryHead.DeleteSalaryHead(mSalaryHeads.ID)
                            'Response.Redirect("wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type"))
                            Response.Redirect("wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                                'MarkLog(Flypal.Util.Action.Delete, "SalaryHeads", "Can't delete : This is Currently in use", Flypal.Util.ErrorType.NoError, mSalaryHeads.ID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Flypal.Util.Action.Delete, "SalaryHeads", mSalaryHeads.Name, Flypal.Util.ErrorType.NoError, mSalaryHeads.ID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            Response.Redirect("wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub Save()
        SetObject()
        If Not mSalaryHeads.IsValid Then Exit Sub

        Try
            mSalaryHeads.Save()
            If txtCode.Enabled = True Then
                setFocus(txtCode)
            End If
            'MarkLog(Flypal.Util.Action.Save, "SalaryHeads", mSalaryHeads.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty)
            NewRecord()
            txtName.DataBind()
            txtCode.DataBind()
            DataFieldBind()
            SetSession()
            lblTitle.Text = "Salary Head Information [New]"
        Catch ex As SqlException
            If ex.Number = 8145 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 2627 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 2601 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 547 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfSalaryHeads.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                Session("sender") = "Delete"
                msg1.Show()
            End If
        End Try
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind()
        mSalaryHeadsList = SalaryHeadList.GetSalaryHeadList()
        dgSalaryHeads.DataSource = mSalaryHeadsList
        Session("mSalaryHeadsList") = mSalaryHeadsList
        dgSalaryHeads.DataBind()
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtName" Then
            If Len(Trim(txtName.Text)) > 50 Then
                CustValid.ErrorMessage = " SalaryHeads Name too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "txtCode" Then
            If Len(Trim(txtCode.Text)) > 5 Then
                CustValid.ErrorMessage = " SalaryHeads Code too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                setFocus(txtCode)
            End If
            BackPage = Request.QueryString("Backpage")
            Session("BackPage") = BackPage
            NewRecord()
            Session("mSalaryHeads") = mSalaryHeads
            DataFieldBind()
        Else
            dgSalaryHeads.DataSource = mSalaryHeadsList
            dgSalaryHeads.DataBind()
        End If
        If mSalaryHeadsList.Count > 25 Then
            btnBackTop.Visible = True
        Else
            btnBackTop.Visible = False
        End If
        setFocus(txtCode)
        MessageBoxResult()
        SetSession()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If txtName.Enabled = True Then
            setFocus(txtCode)
        End If
        'MarkLog(Flypal.Util.Action.[New], "SalaryHeads", "", Flypal.Util.ErrorType.NoError, mSalaryHeads.ID)
        NewRecord()
        txtName.Text = ""
        txtCode.Text = ""
        DataFieldBind()
        lblTitle.Text = "Salary Head Information [New]"
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("SalaryHeadsNew") And mSalaryHeads.IsNew) Or (Not User.IsInRole("SalaryHeadsEdit") And Not mSalaryHeads.IsNew) Then
            SetObject()
            SetSession()
            'MarkLog(Flypal.Util.Action.Save, "SalaryHeads", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
            'Session("sender") = "Authorization"
            'msg.Show()
            'Exit Sub
        End If
        If IsValid Then
            Save()
        End If
    End Sub
    Private Sub dgSalaryHeads_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSalaryHeads.ItemCommand
        Dim mID As New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "Edit"
                ''If (Not User.IsInRole("SalaryHeadsView") And Not User.IsInRole("SalaryHeadsEdit")) Then
                ''    SetObject()
                ''    SetSession()
                ''    'MarkLog(Flypal.Util.Action.Edit, "SalaryHeads", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                ''    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    'msg.ReplacePage = "wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
                ''    'Session("sender") = "Authorization"
                ''    'msg.Show()
                ''    'Exit Sub
                ''End If
                EditRecord(mID)
                txtCode.DataBind()
                txtName.DataBind()
                'MarkLog(Flypal.Util.Action.Edit, "SalaryHeads", mSalaryHeads.Name, Flypal.Util.ErrorType.NoError, mSalaryHeads.ID)
                If Len(mSalaryHeads.Name) > 15 Then
                    lblTitle.Text = "Salary Head Information [" & mSalaryHeads.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitle.Text = "Salary Head Information [" & mSalaryHeads.Name & " ]"
                End If
                If txtName.Enabled = True Then
                    setFocus(txtCode)
                End If
            Case "Delete"
                ''If (Not User.IsInRole("SalaryHeadsDelete")) Then
                ''    SetObject()
                ''    SetSession()
                ''    ''MarkLog(Flypal.Util.Action.Delete, "City", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                ''    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    'msg.ReplacePage = "wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
                ''    'Session("sender") = "Authorization"
                ''    'msg.Show()
                ''    'Exit Sub
                ''End If
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        'MarkLog(Flypal.Util.Action.Close, "SalaryHeads", "", Flypal.Util.ErrorType.NoError, Guid.Empty)
        Session("mSalaryHeadsList") = mSalaryHeadsList
        Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
    End Sub
#End Region


End Class
