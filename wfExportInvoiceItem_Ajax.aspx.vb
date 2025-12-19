Public Class wfExportInvoiceItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mExportInvoice As ExportInvoice
    Public Flag As Integer
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mExportInvoice = Session("mExportInvoice")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRate').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Export Invoice Item [" & mExportInvoice.ExportInvoiceItems.CurrentItem.PartNo & "]"
        End If
    End Sub
    Private Function setObject() As Boolean
        mExportInvoice.ExportInvoiceItems.CurrentItem.CRate = Val(txtRate.Text.Trim)
        mExportInvoice.ExportInvoiceItems.CurrentItem.Note = Trim(txtNote.Text)
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
        ElseIf custValidator.ControlToValidate = "txtRate" Then
            If Val(txtRate.Text) < 0 Then
                custValidator.ErrorMessage = "Rate must be greater than zero."
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        If Not IsPostBack Then
            If txtRate.Enabled = True Then
                setFocus(txtRate)
            End If
            DataBind()
            SetPage()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
       If IsValid Then
            If setObject() Then
                Session("mExportInvoice") = mExportInvoice
                Session.Remove("Edit")
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mExportInvoice.ExportInvoiceItems.CurrentItem.IsNew And Not Session("Edit") = True Then mExportInvoice.ExportInvoiceItems.Remove(mExportInvoice.ExportInvoiceItems.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region

End Class