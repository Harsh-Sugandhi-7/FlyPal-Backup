Public Class wfMaintenanceTaskDetail_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMaintenanceTask As MaintenanceTask
    Public mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Dim mTaskCard As TaskCard
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMaintenanceTask = Session("mMaintenanceTask")
        mMaintenanceTaskAndKit = Session("mMaintenanceTaskAndKit")
        mTaskCard = Session("mTaskCard")
    End Sub
    Private Sub setSession()
        Session("mMaintenanceTask") = mMaintenanceTask
        Session("mTaskCard") = mTaskCard
    End Sub
    Private Sub setObject()
        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.SrNo = mMaintenanceTask.MaintenanceTaskDetails.CurrentIndex + 1
        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task = Trim(txtTask.Text)
        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardNo = Trim(txtTaskCardNo.Text)
        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Note = Trim(txtNote.Text)
        Session("mMaintenanceTask") = mMaintenanceTask
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTaskCard = TaskCard.GetTaskCard(mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardID)
        Session("mTaskCard") = mTaskCard
        dgTaskSteps.DataSource = mTaskCard.TaskSteps
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtNote" Then
            If txtNote.Text.Trim.Length > 500 Then
                'txtNote.Text = txtNote.Text.Trim.Substring(0, 497) + "..."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If txtTask.Enabled = True Then
            setFocus(txtNote)
        End If
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()

            If mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.IsNew Then
                lblTitle.Text = "Maintenance Task [New]"
            Else
                If Len(mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task) > 15 Then
                    lblTitle.Text = "Maintenance Task [" & mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task.Substring(0, 15) & "...]"
                Else
                    lblTitle.Text = "Maintenance Task [" & mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task & "]"
                End If
            End If
        End If
      
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mTaskCard")

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("BackPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            setObject()
            mMaintenanceTask.ApplyEdit()
            mMaintenanceTask.Save()
            mMaintenanceTaskAndKit.MaintenanceTaskID = mMaintenanceTask.ID
            setSession()

            Session.Remove("mTaskCard")

            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If


            Response.Redirect(Request.QueryString("BackPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4"))
        End If
    End Sub
#End Region
End Class