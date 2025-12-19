Public Class wfQuotationCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mQuotation As Quotation
    Private mChargeList As ChargeList
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mQuotation = Session("mQuotation")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mQuotation") = mQuotation
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
            mQuotation.QuotationCharges.CurrentItem.SrNo = mQuotation.QuotationCharges.CurrentIndex + 1
            mQuotation.QuotationCharges.CurrentItem.ChargeID = Id
            mQuotation.QuotationCharges.CurrentItem.ConversionFactor = mQuotation.ConversionFactor
            mQuotation.QuotationCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mQuotation.QuotationCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            mQuotation.QuotationCharges.CurrentItem.ConversionFactor = mQuotation.ConversionFactor
            If mQuotation.QuotationItems.Count > 0 Then
                mQuotation.QuotationCharges.CurrentItem.BasicAmount = mQuotation.QuotationItems.CTotalAmount
            End If
            If mQuotation.QuotationCharges.Contains(mQuotation.QuotationCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Quotation Charge.", MsgBoxStyle.OkOnly, "")
                mQuotation.CancelEdit()
                Return False
                Exit Function
            Else
                mQuotation.ApplyEdit()
                mQuotation.CalculateTotal()
                If mQuotation.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
                    mQuotation.RoundCGrandTotal()
                End If
                Return True
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mQuotation") = mQuotation
        Else
            mQuotation.CancelEdit()
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
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mQuotation.QuotationCharges.CurrentItem.ChargeName, mQuotation.QuotationCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mQuotation.QuotationCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        If Session("Edit") Then
            'Condation Added by DEVEN On 28/12/2007 --------------------------------------
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mQuotation.QuotationCharges.CurrentItem.ChargeName, mQuotation.QuotationCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mQuotation.QuotationCharges.CurrentItem.ChargeID)
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
            lblTitle.Text = "Quotation Charge [ " & mQuotation.QuotationCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "Quotation Charge [ New ]"
        End If
        Session("mQuotation") = mQuotation
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If Setobject() = True Then
                If (mQuotation.QuotationCharges.CurrentItem.Sign <> 1 And mQuotation.QuotationCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mQuotation.QuotationCharges.CurrentItem.IsValid)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Quotation Charge(s) are not allowed if Quotation Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                    mQuotation.CancelEdit()
                    Exit Sub
                Else
                    Session.Remove("EditCharge")
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If
                    'Response.Redirect("wfQuotation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                End If
            Else
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        'Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfQuotationCharge_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenChargeWindow", "OpenChargeWindow();", True)
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
        If mQuotation.QuotationCharges.CurrentItem.IsNew And Not Session("EditCharge") = True Then mQuotation.QuotationCharges.Remove(mQuotation.QuotationCharges.CurrentItem)
        Session.Remove("EditCharge")
        Response.Redirect("wfQuotation_Ajax.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnChargeList_Click(sender As Object, e As EventArgs) Handles hdnimgBtnChargeList.Click
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
        cmbCharge.DataSource = mChargeList
        cmbCharge.DataBind()
        upnlOtherChargeDetails.Update()
    End Sub
#End Region

End Class