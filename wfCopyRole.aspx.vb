Partial Class wfCopyRole
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mRoleList As RoleList
    Public mRole As Role
    Public mRoleID As Guid
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents rfvName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblSkillDetails As System.Web.UI.WebControls.Label
    Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
    Protected WithEvents dgSkillList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnFindNow As System.Web.UI.WebControls.Button
    Protected WithEvents dgRoleList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbllistroles As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRole = CType(Session("mRole"), Role)
        mRoleList = CType(Session("mRoleList"), RoleList)
    End Sub
    Private Sub SetSession()
        Session("mRole") = mRole
        Session("mRoleList") = mRoleList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRole")
        Session.Remove("mRoleList")
    End Sub
    Private Function SetObject() As Boolean
        mRole.Name = Trim(txtRoleName.Text)
        Dim j As Integer = 0
        While j < mRole.EntryModules.Count
            Dim item As DataGridItem
            item = dgEntry.Items(j)
            mRole.EntryModules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
            mRole.EntryModules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
            mRole.EntryModules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
            mRole.EntryModules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
            mRole.EntryModules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
            j = j + 1
        End While
        j = 0
        While j < mRole.ReportModules.Count
            Dim item As DataGridItem
            item = dgReport.Items(j)
            mRole.ReportModules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
            j = j + 1
        End While
    End Function
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
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
                            mRole = CType(Session("mRole"), Role)
                            Response.Redirect("wfRole.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            End If
                            DataFieldBind()
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfRole.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfRole.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfRole.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfRole.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '  DataFieldBind()
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgEntry.DataSource = mRole.EntryModules
        dgReport.DataSource = mRole.ReportModules
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtRoleName.Enabled = True Then
                setFocus(txtRoleName)
            End If
            DataFieldBind()
        End If
        If mRole.IsNew Then
            lbltitle.Text = " Role[New] "
        Else
            lbltitle.Text = "New Role Copy of[ " & mRole.Name & "]"
        End If
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose1.Click, btnClose.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnSav_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSav.Click
        Try
            If IsValid Then
                SetObject()
                mRole.Save()
                Session("mRole") = mRole
                DataFieldBind()
                SetSession()
                '  Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End If
        Catch ex As SqlClient.SqlException

            If ex.Number = 8114 Or ex.Number = 8115 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            ElseIf ex.Number = 8145 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            ElseIf ex.Number = 2627 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            ElseIf ex.Number = 547 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If
        Catch ex As Exception
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Exception, SIMsgBox.Message_text.Exception, ex.Message, MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfRole.aspx?BackPage=" & Request.QueryString("BackPage")
            msg1.Show()
        Finally
        End Try
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        txtRoleName.Text = ""
        If txtRoleName.Enabled = True Then
            setFocus(txtRoleName)
        End If
    End Sub
    Private Sub dgEntry_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEntry.ItemCommand
        Dim index As Int32 = e.Item.ItemIndex + dgReport.CurrentPageIndex * dgReport.PageSize
        dgEntry.DataSource = mRole.EntryModules
        dgEntry.DataBind()
    End Sub
    Private Sub dgReport_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEntry.ItemCommand
        Dim index As Int32 = e.Item.ItemIndex + dgReport.CurrentPageIndex * dgReport.PageSize
        dgReport.DataSource = mRole.ReportModules
        dgReport.DataBind()
    End Sub
#End Region
End Class
