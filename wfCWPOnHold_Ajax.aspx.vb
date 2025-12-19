Public Class wfCWPOnHold_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mCWP As CWP
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCWP = Session("mCWP")
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If CDate(txtOnHoldDate.Text) < mCWP.CWPStatusChilds(mCWP.CWPStatusChilds.CurrentIndex - 1).StatusDate Then
                custValidator.ErrorMessage = "On Hold date[" + txtOnHoldDate.Text + "] should be greater than or equal to " + mCWP.CWPStatusChilds(mCWP.CWPStatusChilds.CurrentIndex - 1).StatusName + " date[" + mCWP.CWPStatusChilds(mCWP.CWPStatusChilds.CurrentIndex - 1).StatusDateFormatted.ToString + "]"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not Page.IsPostBack Then

        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If txtOnHoldDate.Text <> "" Then
                mCWP.CWPStatusChilds.CurrentItem.StatusDate = txtOnHoldDate.Text
            Else
                mCWP.CWPStatusChilds.CurrentItem.StatusDate = System.DBNull.Value
            End If
            mCWP.CWPStatusChilds.CurrentItem.Remark = txtRemark.Text
            mCWP.Save()
            Session("mCWP") = mCWP
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        If mCWP.CWPStatusChilds.CurrentItem.IsNew And Not Session("Edit") = True Then mCWP.CWPStatusChilds.Remove(mCWP.CWPStatusChilds.CurrentItem)
        Session("mCWP") = mCWP
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
#End Region
End Class