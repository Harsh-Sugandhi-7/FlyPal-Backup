Public Class wfMSPAssembly_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mMSP As MSP
    Dim EventLogID As Guid
#End Region

#Region " Methods "
    Private Sub SetPage()
        If mMSP.MSPAssemblys.CurrentItem.IsNew Then
            lblTitle.Text = "Applicable Assembly [New]"
        Else
            lblTitle.Text = "Applicable Assembly [" & mMSP.MSPAssemblys.CurrentItem.AssemblyName & "]"
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub DataFieldBind(Optional ByVal GetList As Boolean = True)
        cmbAssemblyList.DataSource = AssemblyList.GetAssemblyListForComboBox(0, Guid.Empty.ToString, Today.Date.ToString, "(SELECT)", IsInstalled:=True, IsForSpareAssembly:=False)
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mMSP = Session("mMSP")
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            setFocus(cmbAssemblyList)
            DataFieldBind()
            SetPage()
        End If
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub SaveFormtoObject()
        mMSP.MSPAssemblys.CurrentItem.Remark = Trim(txtRemark.Text)
        mMSP.MSPAssemblys.CurrentItem.AssemblyID = New Guid(cmbAssemblyList.SelectedValue)
        mMSP.MSPAssemblys.CurrentItem.AssemblyName = cmbAssemblyList.SelectedItem.Text
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid = False Then upnlValidationSummary.Update() : Exit Sub
        mMSP = Session("mMSP")
        SaveFormtoObject()
        Session("mMSP") = mMSP
        Try
            If mMSP.MSPAssemblys.CurrentItem.IsDirty Then
                If mMSP.MSPAssemblys.CurrentItem.IsValid Then
                    If mMSP.MSPAssemblys.Contains(mMSP.MSPAssemblys.CurrentItem) Then
                        MSGBoxCtrl.Show("Duplicate Alert!", "You are trying to add the duplicate entry.", "You can not add duplicate entry.", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                    mMSP.ApplyEdit()
                    Session("mMSP") = mMSP
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If
                Else
                    cvControlValidator.ErrorMessage = mMSP.MSPAssemblys.CurrentItem.GetBrokenRulesString
                    cvControlValidator.IsValid = mMSP.MSPAssemblys.CurrentItem.IsValid
                    upnlValidationSummary.Update()
                End If
            Else
                mMSP.ApplyEdit()
                Session("mMSP") = mMSP
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add duplicate entry in MSP.", MsgBoxStyle.OkOnly, "")
        End Try
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Session("EditMSPAssembly") = False Then Session.Remove("EditMSPAssembly") : mMSP.MSPAssemblys.Remove(mMSP.MSPAssemblys.CurrentItem)
        Session("EditMSPAssembly") = ""
        Session("Cnt") = 0
        mMSP.CancelEdit()
        Session("mMSP") = mMSP
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub

#End Region

End Class