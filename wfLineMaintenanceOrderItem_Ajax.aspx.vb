Public Class wfLineMaintenanceOrderItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mLineMaintenanceOrder As LineMaintenanceOrder
#End Region

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mLineMaintenanceOrder = Session("mLineMaintenanceOrder")
    End Sub
    Private Sub setSession()
        Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
        txtRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRate').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Service Order Item... "
        End If
    End Sub
    Private Function setObject() As Boolean
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.SrNo = mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentIndex + 1
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.JobDetails = Trim(txtJobDetails.Text)
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Qty = Val(txtQty.Text)
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Unit = Trim(txtUnit.Text)
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.CRate = Val(txtRate.Text)
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.ConversionFactor = mLineMaintenanceOrder.ConversionFactor
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Remark = Trim(txtRemark.Text)
        mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Note = Trim(txtNote.Text)

        mLineMaintenanceOrder.ApplyEdit()
        mLineMaintenanceOrder.CalculateTotal()
        If mLineMaintenanceOrder.IsRoundOff = True Then
            mLineMaintenanceOrder.RoundCGrandTotal()
        End If
        Return True
    End Function
#End Region

#Region " Data Binding "
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtJobDetails" Then
            If Len(txtJobDetails.Text) > 500 Then
                custValidator.ErrorMessage = "Job Details must not be greater than 500 Char."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtUnit" Then
            If Len(txtRate.Text) > 10 Then
                custValidator.ErrorMessage = "Unit must not be greater than 500 Char."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRate" Then
            If Val(txtRate.Text) <= 0 Then
                custValidator.ErrorMessage = "Rate must be greater than zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 250 Then
                custValidator.ErrorMessage = "Remark must not be greater than 250 Char."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 250 Then
                custValidator.ErrorMessage = "Note must not be greater than 250 Char."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        addAttributes()
        If Not IsPostBack Then
            If txtJobDetails.Enabled = True Then
                setFocus(txtJobDetails)
            End If
            SetPage()
            DataBind()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If setObject() Then
                Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
                Session.Remove("Edit")
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.IsNew And Not Session("Edit") = True Then mLineMaintenanceOrder.LineMaintenanceOrderItems.Remove(mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region

End Class