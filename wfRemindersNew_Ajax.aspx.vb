Public Class wfRemindersNew_Ajax
    Inherits System.Web.UI.Page

    
#Region "Variable Declaration "
    Public mReminder As Reminder

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReminder = CType(Session("mReminder"), Reminder)
    End Sub
    Private Sub SetSession()
        Session("mReminder") = mReminder
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReminder")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Function Save() As Boolean
        'GetSession()
        If Not IsValid Then Exit Function
        SetObject()
        If mReminder.IsValid = True Then
            Try
                mReminder.ApplyEdit()
                mReminder = CType(mReminder.Save(), Reminder)
                DataFieldBind()
                Session("mReminder") = mReminder
                Return True
            Catch ex As SqlException

                If ex.Number = 8114 Or ex.Number = 8115 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemindersNew.aspx?"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemindersNew.aspx?"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemindersNew.aspx?"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemindersNew.aspx?"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub GetReminderDetails()
        mReminder = Reminder.GetAutoReminders(User.Identity.Name)
        Session("mReminder") = mReminder
    End Sub
    Private Sub SetObject() 'Added By Prashant 12/12/2007
        mReminder.Yes = rbActiveYes.Checked
        mReminder.No = rbActiveNo.Checked
        mReminder.IsOnMonday = chkIsOnMonday.Checked
        mReminder.IsOnTuesday = chkIsOnTuesday.Checked
        mReminder.IsOnWednesday = chkIsOnWednesday.Checked
        mReminder.IsOnThursday = chkIsOnThursday.Checked
        mReminder.IsOnFriday = chkIsOnFriday.Checked
        mReminder.IsOnSaturday = chkIsOnSaturday.Checked
        mReminder.IsOnSunday = chkIsOnSunday.Checked
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind() 'Added By Prashant 12/12/2007
        rbActiveYes.Checked = mReminder.Yes
        rbActiveYes.DataBind()
        rbActiveNo.Checked = mReminder.No
        rbActiveNo.DataBind()
        chkIsOnMonday.Checked = mReminder.IsOnMonday
        chkIsOnMonday.DataBind()
        chkIsOnTuesday.Checked = mReminder.IsOnTuesday
        chkIsOnTuesday.DataBind()
        chkIsOnWednesday.Checked = mReminder.IsOnWednesday
        chkIsOnWednesday.DataBind()
        chkIsOnThursday.Checked = mReminder.IsOnThursday
        chkIsOnThursday.DataBind()
        chkIsOnFriday.Checked = mReminder.IsOnFriday
        chkIsOnFriday.DataBind()
        chkIsOnSaturday.Checked = mReminder.IsOnSaturday
        chkIsOnSaturday.DataBind()
        chkIsOnSunday.Checked = mReminder.IsOnSunday
        chkIsOnSunday.DataBind()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If rbActiveYes.Enabled = True Then
                setFocus(rbActiveYes)
            End If
            GetReminderDetails()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        If IsValid Then
            If Save() = True Then
                btnApply.Enabled = False
            End If
        End If
    End Sub
    'Added By Prashant 12/12/2007
    Private Sub rbActiveYes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbActiveYes.CheckedChanged
        mReminder.Yes = rbActiveYes.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub rbActiveNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbActiveNo.CheckedChanged
        mReminder.No = rbActiveNo.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub chkIsOnMonday_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsOnMonday.CheckedChanged
        mReminder.IsOnMonday = chkIsOnMonday.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub chkIsOnTuesday_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsOnTuesday.CheckedChanged
        mReminder.IsOnTuesday = chkIsOnTuesday.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub chkIsOnWednesday_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsOnWednesday.CheckedChanged
        mReminder.IsOnWednesday = chkIsOnWednesday.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub chkIsOnThursday_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsOnThursday.CheckedChanged
        mReminder.IsOnThursday = chkIsOnThursday.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub chkIsOnFriday_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsOnFriday.CheckedChanged
        mReminder.IsOnFriday = chkIsOnFriday.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub chkIsOnSaturday_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsOnSaturday.CheckedChanged
        mReminder.IsOnSaturday = chkIsOnSaturday.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    Private Sub chkIsOnSunday_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsOnSunday.CheckedChanged
        mReminder.IsOnSunday = chkIsOnSunday.Checked
        btnApply.Enabled = mReminder.IsDirty
    End Sub
    '---------------------------------------
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class