Public Class wfOtherChargeDocketDetails_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mOtherCharge As OtherCharge
    Public mOtherChargeDetail As OtherChargeDetail
    Private mChargeList As ChargeList
    Private mOtherChargeTypeList As OtherChargeTypeList
    Private mCurrencyList As CurrencyList
    Public mVendorList As VendorList
#End Region

#Region " Buisness Method And Properties "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mOtherCharge = Session("mOtherCharge")
        mChargeList = Session("mChargeList")
        mOtherChargeTypeList = Session("mOtherChargeTypeList")
        mOtherChargeDetail = Session("mOtherChargeDetail")
        mCurrencyList = Session("mCurrencyList")
        mVendorList = Session("mVendorList")
    End Sub
    Private Sub SetSession()
        Session("mOtherCharge") = mOtherCharge
        Session("mChargeList") = mChargeList
        Session("mOtherChargeTypeList") = mOtherChargeTypeList
        Session("mOtherChargeDetail") = mOtherChargeDetail
        Session("mCurrencyList") = mCurrencyList
        Session("mVendorList") = mVendorList
    End Sub
    Private Function Setobject() As Boolean
        mOtherCharge.BeginEdit()
        Dim ChargeID As New Guid(cmbCharge.SelectedValue.ToString)
        Dim VendorID As New Guid(cmbVendorList.SelectedValue.ToString)
        Dim CurrencyID As New Guid(cmbCurrencyList.SelectedValue.ToString)

        mOtherCharge.OtherChargeDetails.CurrentItem.SrNo = mOtherCharge.OtherChargeDetails.CurrentIndex + 1
        mOtherCharge.OtherChargeDetails.CurrentItem.ChargeID = ChargeID
        mOtherCharge.OtherChargeDetails.CurrentItem.VendorID = VendorID
        mOtherCharge.OtherChargeDetails.CurrentItem.CurrencyID = CurrencyID
        mOtherCharge.OtherChargeDetails.CurrentItem.OtherChargeTypeID = cmbChargeType.SelectedValue
        mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceNo = txtInvNo.Text
        If Not IsDate(txtInvDate.Text) Then
            mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDate = System.DBNull.Value
        Else
            mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDate = CType(Trim(txtInvDate.Text), Object)
        End If
        mOtherCharge.OtherChargeDetails.CurrentItem.ConversionFactor = Val(txtConversionFactor.Text)
        mOtherCharge.OtherChargeDetails.CurrentItem.CServiceCharges = Val(txtCSeriveCharge.Text)
        mOtherCharge.OtherChargeDetails.CurrentItem.CAmount = Val(txtChargeAmount.Text)

        If mOtherCharge.OtherChargeDetails.Contains(mOtherCharge.OtherChargeDetails.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Order Charge.", MsgBoxStyle.OkOnly, "")
            mOtherCharge.CancelEdit()
            Return False
            Exit Function
        Else
            mOtherCharge.ApplyEdit()
            Return True
        End If
        txtGrandTotal.DataBind()
        Session("mOtherCharge") = mOtherCharge
    End Function
    Private Sub addAttributes()
        txtCSeriveCharge.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCSeriveCharge').value,event)")
        txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
        txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
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
#End Region

#Region " Binding Methods "

    Public Sub DataFieldBind()
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
        cmbCharge.DataSource = mChargeList

        mOtherChargeTypeList = OtherChargeTypeList.GetOtherChargeTypeList()
        Session("mOtherChargeTypeList") = mOtherChargeTypeList
        cmbChargeType.DataSource = mOtherChargeTypeList

		mVendorList = VendorList.GetVendortList(0, , , , , , True, , IsSupplier:=True, IsServiceProvider:=True)
		cmbVendorList.DataSource = mVendorList

        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        Session("mCurrencyList") = mCurrencyList
        cmbCurrencyList.DataSource = mCurrencyList
        txtInvDate.Text = mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDateFormatted.ToString
        DataBind()

        'Code Added by DEVEN On 28/12/2007 --------------------------------------
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mOtherCharge.OtherChargeDetails.CurrentItem.ChargeName, mOtherCharge.OtherChargeDetails.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mOtherCharge.OtherChargeDetails.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        '------------------------------------------------------------------------
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
        If CustValidator.ControlToValidate = "txtChargeAmount" Then
            If IsNumeric(txtChargeAmount.Text) Then
                If CDbl(txtChargeAmount.Text) <= 0 And mChargeList(Index).Sign <> 1 Then  ''And mChargeList(Index).PercentageTypeID = 1 Then
                    CustValidator.ErrorMessage = "Charge Amount should be Greater than Zero."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "txtCSeriveCharge" Then
            If IsNumeric(txtCSeriveCharge.Text) Then
                If CDbl(txtCSeriveCharge.Text) < 0 Then
                    CustValidator.ErrorMessage = "Service Charge should be Positive."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Select Service Provider from the list."
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "cmbCurrencyList" Then
            If cmbCurrencyList.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Select Currency from the List."
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "txtConversionFactor" Then
            If Val(txtConversionFactor.Text) <= 0 Then
                CustValidator.ErrorMessage = "Currency factor must be greater than zero."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbVendorList.Enabled = True Then
                setFocus(cmbVendorList)
            End If
            DataFieldBind()
        End If
        If Session("EditCharge") Then
            lblTitle.Text = " Charge [ " & mOtherCharge.OtherChargeDetails.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = " Charge [ New ]"
        End If
        If txtConversionFactor.Enabled = True Then
            txtConversionFactor.ToolTip = "Conversion Factor"
        Else
            txtConversionFactor.ToolTip = "Enter Conversion Factor"
        End If
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfOtherChargeDocketDetails_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mOtherCharge.OtherChargeDetails.CurrentItem.IsNew And Not Session("EditCharge") = True Then mOtherCharge.OtherChargeDetails.Remove(mOtherCharge.OtherChargeDetails.CurrentItem)
        Session.Remove("EditCharge")
        ' Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Response.Redirect("wfOtherChargeDocket_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            If Setobject() Then
                Session.Remove("EditCharge")
                'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                Response.Redirect("wfOtherChargeDocket_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
            End If
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub cmbCharge_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCharge.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
        Setobject()
        If cmbCharge.Enabled = True Then
            setFocus(cmbCharge)
        End If
    End Sub
    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        If cmbCurrencyList.Enabled = True Then
            setFocus(cmbCurrencyList)
        End If
        upnlConversionFactor.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class