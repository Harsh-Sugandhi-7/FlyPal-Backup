Public Class wfWorkInvoiceTool_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mWorkInvoice As WorkInvoice
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mWorkInvoice = Session("mWorkInvoice")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtQty').value,event)")
        txtRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRate').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("EditTools") Then
            lblTitle.Text = "Work Invoice Tool"
        End If
    End Sub
    Private Function setObject() As Boolean
        mWorkInvoice.BeginEdit()
        mWorkInvoice.WorkInvoiceTools.CurrentItem.ToolDescription = txtDescription.Text
        mWorkInvoice.WorkInvoiceTools.CurrentItem.SrNo = mWorkInvoice.WorkInvoiceTools.CurrentIndex + 1
        mWorkInvoice.WorkInvoiceTools.CurrentItem.Qty = Val(txtQty.Text)
        mWorkInvoice.WorkInvoiceTools.CurrentItem.CRate = Val(txtRate.Text)
        mWorkInvoice.WorkInvoiceTools.CurrentItem.Remark = Trim(txtRemark.Text)
        mWorkInvoice.WorkInvoiceTools.CurrentItem.Note = Trim(txtNote.Text)
        txtQty.DataBind()
        mWorkInvoice.ApplyEdit()
        Return True
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            If Val(txtQty.Text.Trim) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than Zero."
                e.IsValid = False
            ElseIf (Val(txtQty.Text) > 0) Then
                If (Val(txtRate.Text) <= 0.0) Then
                    custValidator.ErrorMessage = "Enter Rate."
                    e.IsValid = False
                End If
            End If
        ElseIf custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text.Trim) > 1999 Then
                custValidator.ErrorMessage = "Tool Description should not more than 2000 Charcters."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text.Trim) > 1999 Then
                custValidator.ErrorMessage = "Tool Remark should not more than 2000 Charcters."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text.Trim) > 1999 Then
                custValidator.ErrorMessage = "Tool Note should not more than 2000 Charcters."
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
            If txtDescription.Enabled = True Then
                setFocus(txtDescription)
            End If
            DataFieldBind()
        End If
        SetPage()
    End Sub
   Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
       If IsValid Then
            If setObject() Then
                Session("mWorkInvoice") = mWorkInvoice
                Session.Remove("EditTools")
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mWorkInvoice.WorkInvoiceTools.CurrentItem.IsNew And Not Session("EditTools") = True Then mWorkInvoice.WorkInvoiceTools.Remove(mWorkInvoice.WorkInvoiceTools.CurrentItem)
        Session.Remove("EditTools")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region

End Class
