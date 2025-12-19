Partial Class wfRegistration
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

    Public mRegistration As Registration.Registration

    Private Sub getSession()
        mRegistration = Session("mRegistration")
    End Sub
    Private Sub setSession()
        Session("mRegistration") = mRegistration
    End Sub
    Private Sub setObject()
        mRegistration.CompanyName = txtCompName.Text
        mRegistration.DeptName = txtDeptName.Text
        mRegistration.ShortName = txtShortName.Text
        mRegistration.Address1 = txtAddress1.Text
        mRegistration.Address2 = txtAddress2.Text
        mRegistration.Tel1 = txtTel1.Text
        mRegistration.Tel2 = txtTel2.Text
        mRegistration.Tel3 = txtTel3.Text
        mRegistration.Fax = txtFax.Text
        mRegistration.Email = txtEmail.Text
        mRegistration.BaseCurrencyName = txtBaseCurrency.Text
        mRegistration.BaseCurrencySymboll = txtSymbol.Text
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            Dim RegClone As Registration.Registration
                            RegClone = mRegistration.Clone
                            Try
                                mRegistration.Save()
                                Session("mRegistration") = mRegistration
                                Response.Redirect("Index.aspx?")
                            Catch ex As SqlClient.SqlException
                                Session("RegClone") = RegClone
                                If ex.Number = 8114 Or ex.Number = 8115 Then
                                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                                    msg1.Show()
                                ElseIf ex.Number = 8145 Then
                                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                                    msg1.Show()
                                ElseIf ex.Number = 2627 Then
                                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                                    msg1.Show()
                                ElseIf ex.Number = 547 Then
                                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                                    msg1.Show()
                                End If
                            Finally
                                RegClone = Nothing
                            End Try
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    End If
                Case MsgBoxResult.No
                    If CType(Session("sender"), String) = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    Else
                        Session("Sender") = ""
                        Response.Redirect("wfRegistration.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case Else
                    Session("Sender") = ""
                    Response.Redirect("wfRegistration.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfRegistration.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub DataFieldBind()
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtCompName" Then
            If txtCompName.Text = "" Then
                custValidator.ErrorMessage = "Please enter Company Name."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtDeptName" Then
            If txtDeptName.Text = "" Then
                custValidator.ErrorMessage = "Please enter Department Name."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        If Not IsPostBack And Session("sender") = "" Then
            If txtCompName.Enabled = True Then
                setFocus(txtCompName)
            End If
            Session("MiddleFrame") = "wfRegistration.aspx"
            DataFieldBind()
        End If
        MessageBoxResult()
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        If IsValid Then
            Dim RegClone As Registration.Registration
            RegClone = mRegistration.Clone
            Try
                setObject()
                mRegistration.Save()
                mRegistration.MarkClean()

                Session("mRegistration") = mRegistration
                Response.Redirect("Index.aspx?BackPage=" & Request.QueryString("BackPage"))
            Catch ex As SqlClient.SqlException
                Session("RegClone") = RegClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfRegistration.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                End If
            Finally
                RegClone = Nothing
            End Try
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect(Request.QueryString("BackPage") & "?")
    End Sub
End Class
