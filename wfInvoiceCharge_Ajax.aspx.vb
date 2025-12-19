Public Class wfInvoiceCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mChargeList As ChargeList
    Public mInvoice As Invoice
#End Region

#Region " Buisness Method "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mInvoice = Session("mInvoice")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mInvoice") = mInvoice
        Session("mChargeList") = mChargeList
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Function Setobject() As Boolean
        mInvoice.BeginEdit()
        Dim Id As New Guid(cmbCharge.SelectedValue.ToString)
        If Not Id.Equals(Guid.Empty) Then
            mInvoice.InvoiceCharges.CurrentItem.SrNo = mInvoice.InvoiceCharges.CurrentIndex + 1
            mInvoice.InvoiceCharges.CurrentItem.ChargeID = Id
            mInvoice.InvoiceCharges.CurrentItem.ConversionFactor = mInvoice.ConversionFactor
            mInvoice.InvoiceCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mInvoice.InvoiceCharges.CurrentItem.ConversionFactor = mInvoice.ConversionFactor
            mInvoice.InvoiceCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
          
            If mInvoice.InvoiceCharges.Contains(mInvoice.InvoiceCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Invoice Charge.", MsgBoxStyle.OkOnly, "")
                mInvoice.CancelEdit()
                Return False
                Exit Function
            Else
                mInvoice.ApplyEdit()
                mInvoice.CalculateTotal()            'Added By Saylee on 8-Sep-2007
                If mInvoice.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
                    mInvoice.RoundCGrandTotal()
                End If
                Return True
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mInvoice") = mInvoice
        Else
            mInvoice.CancelEdit()
        End If
    End Function
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
        txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
    End Sub
    Private Sub setControl(ByVal Index As Int32)
        txtPercentage.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 3)
        txtChargeAmount.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 1)
        txtPercentage.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, mChargeList(Index).Percentage)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, txtChargeAmount.Text, 0)
        txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, txtChargeAmount.Text)
    End Sub
#End Region

#Region " Data Binding  "
    Private Sub GetList()
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
    End Sub
    Public Sub DataFieldBind()
        cmbCharge.DataSource = mChargeList
        Session("mChargeList") = mChargeList
        txtPercentage.DataBind()
        txtChargeAmount.DataBind()

        DataBind()
        'Changes made by Kalpesh as per - Aircraft Removed_63
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mInvoice.InvoiceCharges.CurrentItem.ChargeName, mInvoice.InvoiceCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mInvoice.InvoiceCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        '------------------------------------------------------------------------
        If CType(Session("Edit"), String) = "Edit" Then
            'Changes made by Kalpesh as per - Aircraft Removed_63
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mInvoice.InvoiceCharges.CurrentItem.ChargeName, mInvoice.InvoiceCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mInvoice.InvoiceCharges.CurrentItem.ChargeID)
                txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
                txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
                txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
                txtPercentage.ToolTip = IIf(Not txtPercentage.ReadOnly, "Enter Percentage", "Percentage") 'Code Added by DEVEN On 28/12/2007 --------------------------------------
                txtChargeAmount.ToolTip = IIf(Not txtChargeAmount.ReadOnly, "Enter Charge Amount", "Charge Amount") '------------------------------------------------------------------------
            End If
        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        Dim Index As Int32 = IIf(cmbCharge.SelectedIndex <= 0, 0, cmbCharge.SelectedIndex)
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "cmbCharge" Then
            If cmbCharge.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Please select the Charge"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidator.ControlToValidate = "txtPercentage" Then
            If txtPercentage.Enabled = True And IsNumeric(txtPercentage.Text) Then
                If CDbl(Val(txtPercentage.Text)) <= 0 And mChargeList(Index).PercentageTypeID = 3 Then
                    CustValidator.ErrorMessage = "Percentage should be Positive Numeric value."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "txtChargeAmount" Then
            If IsNumeric(txtChargeAmount.Text) Then
                If CDbl(Val(txtChargeAmount.Text)) <= 0 And mChargeList(Index).PercentageTypeID = 1 Then
                    CustValidator.ErrorMessage = "Charge Amount should be Positive Numeric value."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("Sender") = "" Then  ' Or CType(Session("Edit"), String) = "Edit") Then
            If cmbCharge.Enabled = True Then
                setFocus(cmbCharge)
            End If
            GetList()
            DataFieldBind()
            If Session("Edit") Then
                lblTitle.Text = "Invoice Charge [ " & mInvoice.InvoiceCharges.CurrentItem.ChargeName & " ]"
            Else
                lblTitle.Text = "Invoice Charge [ New ]"
            End If
            Session("mInvoice") = mInvoice
        End If
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfInvoiceCharge_Ajax.aspx")
    End Sub
    Private Sub cmbCharge_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCharge.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
        setControl(Index)
        upnlOtherChargeDetails.Update()
        If cmbCharge.Enabled = True Then
            setFocus(cmbCharge)
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mInvoice.InvoiceCharges.CurrentItem.IsNew And Not Session("Edit") = True Then mInvoice.InvoiceCharges.Remove(mInvoice.InvoiceCharges.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
     End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            Setobject()
            If (mInvoice.InvoiceCharges.CurrentItem.Sign <> 1 And mInvoice.InvoiceCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mInvoice.InvoiceCharges.CurrentItem.IsValid)) Then
                MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Invoice Charge(s) are not allowed if Invoice Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Session.Remove("Edit")
            Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class