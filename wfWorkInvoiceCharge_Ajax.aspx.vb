Public Class wfWorkInvoiceCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mWorkInvoice As WorkInvoice
    Private mChargeList As ChargeList
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mWorkInvoice = Session("mWorkInvoice")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mWorkInvoice") = mWorkInvoice
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
            mWorkInvoice.WorkInvoiceCharges.CurrentItem.SrNo = mWorkInvoice.WorkInvoiceCharges.CurrentIndex + 1
            mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeID = Id
            mWorkInvoice.WorkInvoiceCharges.CurrentItem.ConversionFactor = mWorkInvoice.ConversionFactor
            mWorkInvoice.WorkInvoiceCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mWorkInvoice.WorkInvoiceCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            mWorkInvoice.WorkInvoiceCharges.CurrentItem.ConversionFactor = mWorkInvoice.ConversionFactor
            If mWorkInvoice.WorkInvoiceItems.Count > 0 Then
                mWorkInvoice.WorkInvoiceCharges.CurrentItem.BasicAmount = mWorkInvoice.WorkInvoiceItems.CTotalAmount
            End If
            If mWorkInvoice.WorkInvoiceCharges.Contains(mWorkInvoice.WorkInvoiceCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "WorkInvoice Charge.", MsgBoxStyle.OkOnly, "")
                mWorkInvoice.CancelEdit()
                Return False
                Exit Function
            Else
                mWorkInvoice.ApplyEdit()
                mWorkInvoice.CalculateTotal()
                If mWorkInvoice.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
                    mWorkInvoice.RoundCGrandTotal()
                End If
                Return True
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mWorkInvoice") = mWorkInvoice
        Else
            mWorkInvoice.CancelEdit()
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
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeName, mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        If Session("Edit") Then
            'Condation Added by DEVEN On 28/12/2007 --------------------------------------
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeName, mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeID)
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
            lblTitle.Text = "WorkInvoice Charge [ " & mWorkInvoice.WorkInvoiceCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "WorkInvoice Charge [ New ]"
        End If
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If Setobject() = True Then
                If (mWorkInvoice.WorkInvoiceCharges.CurrentItem.Sign <> 1 And mWorkInvoice.WorkInvoiceCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mWorkInvoice.WorkInvoiceCharges.CurrentItem.IsValid)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage WorkInvoice Charge(s) are not allowed if WorkInvoice Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                    mWorkInvoice.CancelEdit()
                    Exit Sub
                Else
                    Session.Remove("EditCharge")
                    Response.Redirect("wfWorkInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                End If
            Else
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfWorkInvoiceCharge_Ajax.aspx")
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
        If mWorkInvoice.WorkInvoiceCharges.CurrentItem.IsNew And Not Session("EditCharge") = True Then mWorkInvoice.WorkInvoiceCharges.Remove(mWorkInvoice.WorkInvoiceCharges.CurrentItem)
        Session.Remove("EditCharge")
        Response.Redirect("wfWorkInvoice_Ajax.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class