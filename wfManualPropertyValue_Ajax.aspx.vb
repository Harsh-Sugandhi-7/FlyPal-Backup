Public Class wfManualPropertyValue_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mManual As Manual
    Dim EventLogID As Guid
#End Region

#Region " Methods "
    Private Sub SetPage()
        If mManual.ManualPropertyValues.CurrentItem.IsNew Then
            lblTitle.Text = "Manual Property Value Information [New]"
        Else
            lblTitle.Text = "Manual Property Value Information [" & mManual.ManualPropertyValues.CurrentItem.ManualPropertyName & "]"
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub DataFieldBind(Optional ByVal GetList As Boolean = True)
        cmbPropertyList.DataSource = ManualPropertyList.GetManualPropertyList(, "(SELECT)")
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mManual = Session("mManual")
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            setFocus(cmbPropertyList)
            DataFieldBind()
            SetPage()
        End If
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub SaveFormtoObject()
        mManual.ManualPropertyValues.CurrentItem.Value = Trim(txtName.Text)
        mManual.ManualPropertyValues.CurrentItem.ManualPropertyID = New Guid(cmbPropertyList.SelectedValue)
        mManual.ManualPropertyValues.CurrentItem.ManualPropertyName = cmbPropertyList.SelectedItem.Text
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid = False Then upnlValidationSummary.Update() : Exit Sub
        mManual = Session("mManual")
        SaveFormtoObject()
        Session("mManual") = mManual
        Try
            If mManual.ManualPropertyValues.CurrentItem.IsDirty Then
                If mManual.ManualPropertyValues.CurrentItem.IsSavable Then
                    If mManual.ManualPropertyValues.Contains(mManual.ManualPropertyValues.CurrentItem) Then
                        MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry in Property Information", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                    mManual.ApplyEdit()
                    Session("mManual") = mManual
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If
                Else
                    cvControlValidator.ErrorMessage = mManual.ManualPropertyValues.CurrentItem.GetBrokenRulesString
                    cvControlValidator.IsValid = mManual.ManualPropertyValues.CurrentItem.IsValid
                    upnlValidationSummary.Update()
                End If
            Else
                mManual.ApplyEdit()
                Session("mManual") = mManual
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add duplicate entry in Manual.", MsgBoxStyle.OkOnly, "")
        End Try
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Session("EditPropertyValues") = False Then Session.Remove("EditPropertyValues") : mManual.ManualPropertyValues.Remove(mManual.ManualPropertyValues.CurrentItem)
        Session("EditPropertyValues") = ""
        Session("Cnt") = 0
        mManual.CancelEdit()
        Session("mManual") = mManual
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnAddProperty_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddProperty.Click
        ' If IsValid = False Then upnlValidationSummary.Update() : Exit Sub
        SaveFormtoObject()
        Session("mManual") = mManual
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManualPropertyValueWindow", "OpenManualPropertyValueWindow()", True)
    End Sub
    Private Sub hdnBtnManualPropertyValue_Click(sender As Object, e As System.EventArgs) Handles hdnBtnManualPropertyValue.Click
        cmbPropertyList.DataSource = ManualPropertyList.GetManualPropertyList(, "(SELECT)")
        cmbPropertyList.DataBind()
        upnlManualPropertyValue.Update()
    End Sub
#End Region

End Class