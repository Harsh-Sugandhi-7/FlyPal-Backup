Public Class wfFuelInvoiceCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mFuelInvoice As FuelInvoice
    Private mChargeList As ChargeList
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mFuelInvoice = Session("mFuelInvoice")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mFuelInvoice") = mFuelInvoice
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
        Dim Id As New Guid(cmbCharge.SelectedValue.ToString)
        If Not Id.Equals(Guid.Empty) Then
            mFuelInvoice.FuelInvoiceCharges.CurrentItem.SrNo = mFuelInvoice.FuelInvoiceCharges.CurrentIndex + 1
            mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeID = Id
            mFuelInvoice.FuelInvoiceCharges.CurrentItem.ConversionFactor = mFuelInvoice.ConversionFactor
            mFuelInvoice.FuelInvoiceCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mFuelInvoice.FuelInvoiceCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            mFuelInvoice.FuelInvoiceCharges.CurrentItem.ConversionFactor = mFuelInvoice.ConversionFactor
            If mFuelInvoice.FuelInvoiceCharges.Count > 0 Then
                mFuelInvoice.FuelInvoiceCharges.CurrentItem.BasicAmount = mFuelInvoice.FuelInvoiceLogs.CTotalAmount
            End If
            If mFuelInvoice.FuelInvoiceCharges.Contains(mFuelInvoice.FuelInvoiceCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "FuelInvoice Charge.", MsgBoxStyle.OkOnly, "")
                mFuelInvoice.CancelEdit()
                Return False
                Exit Function
            Else
                mFuelInvoice.ApplyEdit()
                mFuelInvoice.CalculateTotal()
                'If mFuelInvoice.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
                '    mFuelInvoice.RoundCGrandTotal()
                'End If
                Return True
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mFuelInvoice") = mFuelInvoice
        Else
            mFuelInvoice.CancelEdit()
        End If
    End Function
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
        If (txtChargeAmount.ReadOnly = True Or txtChargeAmount.Enabled = False) Then
            '
        Else
            txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
        End If
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region " Binding Methods "
    Private Sub GetList()
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
    End Sub
    Public Sub DataFieldBind()
        cmbCharge.DataSource = mChargeList
        txtPercentage.DataBind()
        txtChargeAmount.DataBind()
        DataBind()
        'Code Added by DEVEN On 29/12/2007 --------------------------------------
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeName, mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        If Session("Edit") Then
            'Condation Added by DEVEN On 28/12/2007 --------------------------------------
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeName, mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeID)
                txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
                txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
                txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
                txtPercentage.ToolTip = IIf(Not txtPercentage.ReadOnly, "Enter Percentage", "Percentage") 'Code Added by DEVEN On 28/12/2007 --------------------------------------
                txtChargeAmount.ToolTip = IIf(Not txtChargeAmount.ReadOnly, "Enter Charge Amount", "Charge Amount")
            End If
        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        Dim Index As Int32 = IIf(cmbCharge.SelectedIndex <= 0, 0, cmbCharge.SelectedIndex)
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "cmbCharge" Then
            If cmbCharge.SelectedIndex = 0 Then
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If txtPercentage.Enabled = True Then
            If CustValidator.ControlToValidate = "txtPercentage" Then
                If IsNumeric(txtPercentage.Text) Then
                    If CDbl(txtPercentage.Text) <= 0 And mChargeList(Index).PercentageTypeID = 3 Then
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                Else
                    e.IsValid = False
                End If
            End If
        End If
        If CustValidator.ControlToValidate = "txtChargeAmount" Then
            If IsNumeric(txtChargeAmount.Text) Then
                If CDbl(txtChargeAmount.Text) <= 0 And mChargeList(Index).PercentageTypeID = 1 Then
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbCharge.Enabled = True Then
                setFocus(cmbCharge)
            End If
            GetList()
            DataFieldBind()
        End If
        If Session("Edit") Then
            lblTitle.Text = "FuelInvoice Charge [ " & mFuelInvoice.FuelInvoiceCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "FuelInvoice Charge [ New ]"
        End If
        Session("mFuelInvoice") = mFuelInvoice
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If Setobject() = True Then
                If (mFuelInvoice.FuelInvoiceCharges.CurrentItem.Sign <> 1 And mFuelInvoice.FuelInvoiceCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mFuelInvoice.FuelInvoiceCharges.CurrentItem.IsValid)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage FuelInvoice Charge(s) are not allowed if FuelInvoice Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                    mFuelInvoice.CancelEdit()
                    Exit Sub
                Else
                    Session.Remove("EditCharge")
                    Response.Redirect("wfFuelInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                End If
            Else
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfFuelInvoiceCharge_Ajax.aspx")
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
        If mFuelInvoice.FuelInvoiceCharges.CurrentItem.IsNew And Not Session("EditCharge") = True Then mFuelInvoice.FuelInvoiceCharges.Remove(mFuelInvoice.FuelInvoiceCharges.CurrentItem)
        Session.Remove("EditCharge")
        Response.Redirect("wfFuelInvoice_Ajax.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class