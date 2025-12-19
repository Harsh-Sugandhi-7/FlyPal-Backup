
'Created By     :   Saylee
'Dated          :   5-Feb-2010
'Modified By    :   6-Apr-2010

Partial Class wfAuditScheduleTask
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblOtherAuditCategoryDetails As System.Web.UI.WebControls.Label
    Protected WithEvents lblAuditCategoryNameStar1 As System.Web.UI.WebControls.Label

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
    Public mAuditSchedule As AuditSchedule
    Public mAuditScheduleTask As AuditScheduleTask
    Private mAuditCategoryList As AuditCategoryList
#End Region

#Region " Buisness Method And Properties "

    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mAuditSchedule = Session("mAuditSchedule")
        mAuditCategoryList = Session("mAuditCategoryList")
    End Sub
    Private Sub SetSession()
        Session("mAuditSchedule") = mAuditSchedule
        Session("mAuditCategoryList") = mAuditCategoryList
    End Sub
    Private Function Setobject() As Boolean
        mAuditSchedule.BeginEdit()
        mAuditSchedule.AuditScheduleTasks.CurrentItem.SrNo = mAuditSchedule.AuditScheduleTasks.CurrentIndex + 1
        'mAuditSchedule.AuditScheduleTasks.CurrentItem.AuditCategoryID = New Guid(cmbAuditCategory.SelectedValue)
        'mAuditSchedule.AuditScheduleTasks.CurrentItem.Code = Trim(txtCode.Text)
        'mAuditSchedule.AuditScheduleTasks.CurrentItem.Description = Trim(txtDescription.Text)
        'mAuditSchedule.AuditScheduleTasks.CurrentItem.Note = Trim(txtNote.Text)

    End Function


    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            'Session("Sender") = ""
                            'Dim mAuditSchedule As AuditSchedule
                            'mAuditSchedule = CType(Session("mAuditSchedule"), AuditSchedule)
                            'mAuditSchedule.AuditScheduleItems.RemoveAt(mAuditSchedule.AuditScheduleItems.CurrentIndex)
                            'Session("mAuditSchedule") = mAuditSchedule
                            'Response.Redirect("wfAuditScheduleItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditScheduleTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2601 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditScheduleTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditScheduleTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    Response.Redirect("wfAuditScheduleTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfAuditScheduleTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
                Case Else
                    Session("Sender") = ""
                    Response.Redirect("wfAuditScheduleTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfAuditScheduleTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
        End If
    End Sub
#End Region

#Region " Binding Methods "

    Public Sub DataFieldBind()
        mAuditCategoryList = AuditCategoryList.GetAuditCategoryList("(SELECT)")
        cmbAuditCategory.DataSource = mAuditCategoryList
        Session("mAuditCategoryList") = mAuditCategoryList
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        Dim Index As Int32 = IIf(cmbAuditCategory.SelectedIndex <= 0, 0, cmbAuditCategory.SelectedIndex)
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "cmbAuditCategory" Then
            If cmbAuditCategory.SelectedIndex = 0 Then
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                CustValidator.ErrorMessage = "Note should not be greater than 1000 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 5000 Then
                CustValidator.ErrorMessage = "Description should not be greater than 5000 characters."
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
            If cmbAuditCategory.Enabled = True Then
                setFocus(cmbAuditCategory)
            End If
            DataFieldBind()
        End If
        If Session("Edit") Then
            lblTitle.Text = "Audit Schedule Task [ " & mAuditSchedule.AuditScheduleTasks.CurrentItem.Code & " ]"
        Else
            lblTitle.Text = "Audit Schedule Task [ New ]"
        End If
        Session("mAuditSchedule") = mAuditSchedule
        'MessageBoxResult()
    End Sub
    Private Sub imgbtnAuditCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnAuditCategory.Click
        Setobject()
        Response.Redirect("wfAuditCategory.aspx?BackPage2=wfAuditScheduleTask.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mAuditSchedule.AuditScheduleTasks.CurrentItem.IsNew And Not Session("Edit") = True Then mAuditSchedule.AuditScheduleTasks.Remove(mAuditSchedule.AuditScheduleTasks.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If (Not User.IsInRole("AuditScheduleNew") And mAuditSchedule.IsNew) Or (Not User.IsInRole("AuditScheduleEdit") And Not mAuditSchedule.IsNew) Then
            Setobject()
            SetSession()
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfAuditScheduleTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
            Session("sender") = "Authorization"
            msg.Show()
            Exit Sub
        End If

        If IsValid Then
            Setobject()
            If mAuditSchedule.AuditScheduleTasks.Contains(mAuditSchedule.AuditScheduleTasks.CurrentItem) Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, " AuditSchedule AuditCategory.", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfAuditScheduleTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
                mAuditSchedule.CancelEdit()
                Exit Sub
            Else
                mAuditSchedule.ApplyEdit()
                Session("mAuditSchedule") = mAuditSchedule
                Session.Remove("Edit")
                Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
            End If
        End If
    End Sub
#End Region

End Class
