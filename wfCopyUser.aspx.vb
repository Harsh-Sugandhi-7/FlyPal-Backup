Partial Class wfCopyUser
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents rfvName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblSkillDetails As System.Web.UI.WebControls.Label
    Protected WithEvents dgSkillList As System.Web.UI.WebControls.DataGrid

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
    Public mUser As User
    Public mUserID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUser = CType(Session("mUser"), User)
        mUserID = CType(Session("mUserID"), Guid)
    End Sub
    Private Sub SetSession()
        Session("mUser") = mUser
        Session("mUserID") = mUserID
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUser")
        Session.Remove("mUserID")
    End Sub
    Private Sub NewRecord()
        mUser = SI.UTILITY.User.NewUser()
        Session("mUser") = mUser
        If mUser.IsNew Then
            txtUserName.Enabled = True
            txtPassword.Enabled = True
            txtConPass.Enabled = True
            btnSav.Enabled = True
        Else
            txtUserName.Enabled = False
            txtPassword.Enabled = False
            txtConPass.Enabled = False
            btnSav.Enabled = True
        End If
        If mUser.IsNew Then
            txtPassword.TextMode = TextBoxMode.Password
            txtConPass.TextMode = TextBoxMode.Password
        End If
    End Sub
    Private Function setObject() As Boolean
        If mUser.IsNew Then
            mUser.Name = Trim(txtUserName.Text)
            mUser.Password = Trim(txtPassword.Text)
            mUser.ConfirmPassword = Trim(txtConPass.Text)

        End If
        mUser.ChangePassword = chkLogon.Checked
        Dim j As Integer = 0
        While j < mUser.UserRoles.Count
            Dim item As DataGridItem
            item = dgUser.Items(j)
            mUser.UserRoles.Item(j).IsSelected = CType(item.FindControl("CheckBox1"), CheckBox).Checked
            j = j + 1
        End While
        Dim i As Integer = 0
        While i < mUser.UserMachines.Count
            Dim item As DataGridItem
            item = dgMachine.Items(i)
            mUser.UserMachines.Item(i).IsSelected = CType(item.FindControl("chkSelect"), CheckBox).Checked
            i = i + 1
        End While

        Return True
    End Function
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
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
                            mUser = CType(Session("mUser"), User)
                            Response.Redirect("wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfUser.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfUser.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfUser.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            End If
                            DataFieldBind()
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Function AllowNewAircraft() As Boolean
        Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
        Dim tmpUserList As UserList = UserList.GetUserList()
        If tmpUserList.Count >= mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
            'MessageBox.Show("This version does not supports more than " & mCheck.Number("Aircraft").ToString & " Aircrafts", "Version 1.0", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        Else
            Return True
        End If
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgUser.DataSource = mUser.UserRoles
        dgMachine.DataSource = mUser.UserMachines
        DataBind()
        If mUser.UserMachines.Count > 0 Then
            lblAircraftList.Text = "List of Aircrafts as per criteria: " & mUser.UserMachines.Count & " Record(s) found."
        End If
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "txtPassword" Then
            If Len(Trim(txtPassword.Text)) < 4 Or Len(Trim(txtPassword.Text)) > 10 Then
                CustValid.ErrorMessage = "Minimum Password should be of 4 characters and Maximum 10 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "txtConPass" Then
            If txtPassword.Text <> txtConPass.Text Then
                CustValid.ErrorMessage = "Confirm Password should be same as the Password."
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
        If mUser.IsNew Then
            txtUserName.Enabled = True
            txtPassword.Enabled = True
            txtConPass.Enabled = True
            btnSav.Enabled = True
        Else
            txtUserName.Enabled = False
            txtPassword.Enabled = False
            txtConPass.Enabled = False
            btnSav.Enabled = True
        End If
        If mUser.IsNew Then
            txtPassword.TextMode = TextBoxMode.Password
            txtConPass.TextMode = TextBoxMode.Password
        End If
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtUserName.Enabled = True Then
                setFocus(txtUserName)
            End If
            DataFieldBind()
        End If
        If mUser.IsNew Then
            lbltitle.Text = " User[New] "
        Else
            lbltitle.Text = "New User Copy of[ " & mUser.Name & "]"
        End If
        MessageBoxResult()
        btnNewUser.Enabled = AllowNewAircraft()

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnSav_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSav.Click, btnSaveTop.Click
        Try
            If IsValid Then
                setObject()
                Session("mUser") = mUser
                mUser.Save()
                DataFieldBind()
                SetSession()
                txtUserName.Text = ""

                btnNewUser.Enabled = AllowNewAircraft()
                btnSav.Enabled = AllowNewAircraft()
                'Response.Redirect("wfuser.aspx?BackPage=Index.aspx")
                '   Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 2627 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 547 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                Session("sender") = "Delete"
                msg1.Show()
            End If
        End Try
    End Sub
    Private Sub dgUser_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgUser.ItemCommand
        Dim index As Int32 = e.Item.ItemIndex + dgUser.CurrentPageIndex * dgUser.PageSize
        dgUser.DataSource = mUser.UserRoles
        dgUser.DataBind()
    End Sub
    'Code Added By Girish
    Private Sub btnNewUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewUser.Click

        NewRecord()
        'DataFieldBind()
        If txtUserName.Enabled = True Then
            setFocus(txtUserName)
        End If
    End Sub
    'End of Code
#End Region

End Class
