Public Class wfInvoiceChargeRCI_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mReceiptCumInvoiceCharge As InvoiceCharge
    Private mChargeList As ChargeList
#End Region

#Region " Buisness Method "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mReceiptCumInvoice = Session("mReceiptCumInvoice")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
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
        mReceiptCumInvoice.BeginEdit()
        Dim Id As New Guid(cmbCharge.SelectedValue.ToString)
        If Not Id.Equals(Guid.Empty) Then
            mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.SrNo = mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentIndex + 1
            mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.ChargeID = Id
            mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.ConversionFactor = mReceiptCumInvoice.ConversionFactor
            mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.ConversionFactor = mReceiptCumInvoice.ConversionFactor
            mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then
                mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.BasicAmount = mReceiptCumInvoice.ReceiptCumInvoiceItems.CGrandTotalAmountItem                                                       'dated: 21-11-2005    
            End If
            If mReceiptCumInvoice.ReceiptCumInvoiceCharges.Contains(mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Invoice Charge.", MsgBoxStyle.OkOnly, "")
                mReceiptCumInvoice.CancelEdit()
                Return False
                Exit Function
            Else
                mReceiptCumInvoice.ApplyEdit()
                mReceiptCumInvoice.Invoice.CalculateTotal()         'Added By Saylee on 17-Sep-2007
                If mReceiptCumInvoice.IsRoundOff = True Then        'Added By Prashant on 21-May-2012 ALL25102012
                    mReceiptCumInvoice.Invoice.RoundCGrandTotal()
                End If
                Return True
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Else
            mReceiptCumInvoice.CancelEdit()
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
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mReceiptCumInvoice.Invoice.InvoiceCharges.CurrentItem.ChargeName, mReceiptCumInvoice.Invoice.InvoiceCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mReceiptCumInvoice.Invoice.InvoiceCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        '------------------------------------------------------------------------
        If CType(Session("Edit"), String) = "Edit" Then
            'Changes made by Kalpesh as per - Aircraft Removed_63
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mReceiptCumInvoice.Invoice.InvoiceCharges.CurrentItem.ChargeName, mReceiptCumInvoice.Invoice.InvoiceCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.ChargeID)
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
        End If

        If CType(Session("Edit"), String) = "Edit" Then
            lblTitle.Text = "Other Charge [ " & mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "Other Charge [ New ]"
        End If
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfInvoiceChargeRCI_Ajax.aspx")
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
        Dim id As New Guid(cmbCharge.SelectedValue.ToString)
        If mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.IsNew And Not CType(Session("Edit"), String) = "Edit" Then
            mReceiptCumInvoice.ReceiptCumInvoiceCharges.RemoveAt(mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentIndex)
        End If
        Session.Remove("Edit")
        Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
        mChargeList = Nothing
        mReceiptCumInvoice = Nothing
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            Setobject()
            If (mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.Sign <> 1 And mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.IsValid)) Then
                MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Invoice Charge(s) are not allowed if Invoice Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Session.Remove("Edit")
            Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
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