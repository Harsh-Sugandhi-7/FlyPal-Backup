Public Class wfExportInvoiceCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mExportInvoice As ExportInvoice
    Private mChargeList As ChargeList
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mExportInvoice = Session("mExportInvoice")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mExportInvoice") = mExportInvoice
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
            mExportInvoice.ExportinvoiceCharges.CurrentItem.SrNo = mExportInvoice.ExportinvoiceCharges.CurrentIndex + 1
            mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeID = Id
            mExportInvoice.ExportinvoiceCharges.CurrentItem.ConversionFactor = mExportInvoice.ConversionFactor
            mExportInvoice.ExportinvoiceCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mExportInvoice.ExportinvoiceCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            mExportInvoice.ExportinvoiceCharges.CurrentItem.ConversionFactor = mExportInvoice.ConversionFactor
           If mExportInvoice.ExportinvoiceCharges.Contains(mExportInvoice.ExportinvoiceCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Export Invoice Charge.", MsgBoxStyle.OkOnly, "")
                mExportInvoice.CancelEdit()
                Return False
                Exit Function
            Else
                mExportInvoice.ApplyEdit()
                mExportInvoice.CalculateTotal()
                'If mExportInvoice.IsRoundOff = True Then
                '    mExportInvoice.RoundCGrandTotal()
                'End If
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mExportInvoice") = mExportInvoice
            Return True
        Else
            mExportInvoice.CancelEdit()
            Return False
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
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeName, mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        If Session("Edit") Then
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeName, mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeID)
                txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
                txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
                txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
                txtPercentage.ToolTip = IIf(Not txtPercentage.ReadOnly, "Enter Percentage", "Percentage")
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
            lblTitle.Text = "Export Invoice Charge [ " & mExportInvoice.ExportinvoiceCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "Export Invoice Charge [ New ]"
        End If
        Session("mExportInvoice") = mExportInvoice
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If Setobject() = True Then
                If (mExportInvoice.ExportinvoiceCharges.CurrentItem.Sign <> 1 And mExportInvoice.ExportinvoiceCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mExportInvoice.ExportinvoiceCharges.CurrentItem.IsValid)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Quotation Charge(s) are not allowed if Quotation Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                    mExportInvoice.CancelEdit()
                    Exit Sub
                Else
                    Session.Remove("EditCharge")
                    Response.Redirect("wfExportInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                End If
            Else
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfExportInvoiceCharge_Ajax.aspx")
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
        If mExportInvoice.ExportinvoiceCharges.CurrentItem.IsNew And Not Session("Edit") = True Then mExportInvoice.ExportinvoiceCharges.Remove(mExportInvoice.ExportinvoiceCharges.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect("wfExportInvoice_Ajax.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region


End Class