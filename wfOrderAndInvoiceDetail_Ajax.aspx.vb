Public Class wfOrderAndInvoiceDetail_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mPaymentAdvice As PaymentAdvice
	Dim mId As New Guid
	Dim mPendingOrdersforPaymentAdvice As PendingOrdersForPaymentAdvice
#End Region

#Region "Methods"
	'Added by vikrant on 08-Aug-2018 For ALL08082018
	Private Sub AddAttributes()
		txtTotalValue.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
	End Sub
	'End
	Private Sub Getsession()
		mId = Session("mID")
		mPendingOrdersforPaymentAdvice = Session("mPendingOrdersforPaymentAdvice")
		mPaymentAdvice = Session("mPaymentAdvice")
	End Sub
	Private Sub DataFieldBind()
		DataBind()
		upnlOrderandInvoiceDetail.Update()
	End Sub
	Private Sub SetObject()
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.SupplierInvoiceDate = txtSupplierDate.Text
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.SupplierInvoiceNo = txtNo.Text
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.Remark = txtRemark.Text
		Session("mPaymentAdvice") = mPaymentAdvice
	End Sub
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtSupplierDate" Then
			If Not IsDBNull(CDate(txtSupplierDate.Text.ToString)) Then
				If CDate(txtSupplierDate.Text.ToString) > CDate(mPaymentAdvice.PaymentAdviceDateFormatted.ToString) Then
					custValidator.ErrorMessage = "Supplier invoice date Should not be greater than Payment Advice Date"
					e.IsValid = False
				End If
			End If


		End If
	End Sub
	Public Function CustomValidate2() As Boolean
		Dim strMsg As String = ""
		SetObject()

		If Not mPaymentAdvice.PaymentAdviceItems.CurrentItem.IsValid Then
			For i As Integer = 0 To mPaymentAdvice.PaymentAdviceItems.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMsg = strMsg + mPaymentAdvice.PaymentAdviceItems.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
			Next

		End If
		If strMsg <> "" Then
			CustValidator.ErrorMessage = strMsg
			CustValidator.IsValid = False
			Return False
		End If
		Return True
	End Function
#End Region

#Region "Events"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		AddAttributes() 'Added by vikrant on 08-Aug-2018 For ALL08082018
		Getsession()
		If Not IsPostBack Then
			DataFieldBind()
		End If
	End Sub

	Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
		If mPaymentAdvice.PaymentAdviceItems.CurrentItem.IsValid Then
			If mPaymentAdvice.PaymentAdviceItems.CurrentItem.IsNew And Not Session("PaymentAdviceEdit") = True Then mPaymentAdvice.PaymentAdviceItems.Remove(mPaymentAdvice.PaymentAdviceItems.CurrentItem)
			'  If mPaymentAdvice.IsNew Or mPaymentAdvice.PaymentAdviceItems.CurrentItem.IsNew Then mPaymentAdvice.PaymentAdviceItems.Remove(mPaymentAdvice.PaymentAdviceItems.CurrentItem)
			Session("mPaymentAdvice") = mPaymentAdvice
			Dim mopenas As String = Request.QueryString("Type")
			If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
				Exit Sub
			End If
		Else
			CustomValidate2()
		End If
	End Sub
	Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
		' SetObject()
		If Not Page.IsValid Then Exit Sub

		SetObject()

		If mPaymentAdvice.PaymentAdviceItems.CurrentItem.IsValid Then
			Session.Remove("PaymentAdviceEdit")
			Dim mopenas As String = Request.QueryString("Type")
			If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
				Exit Sub
			End If
		Else
			CustomValidate2()
		End If
	End Sub
	'Added by vikrant on 08-Aug-2018 For ALL08082018
	Private Sub txtTotalValue_TextChanged(sender As Object, e As System.EventArgs) Handles txtTotalValue.TextChanged
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.COrderAmount = CDec(Val(txtTotalValue.Text))
		'Commenrted & Added by Vikrant On 28-Feb-2019 For BA28022019
		'mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderAmount = mPaymentAdvice.PaymentAdviceItems.CurrentItem.ConversionFactor * CDec(Val(txtTotalValue.Text))
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderAmount = mPaymentAdvice.ConversionFactor * CDec(Val(txtTotalValue.Text))
		'End
		txtPAValueInOrderCurr.DataBind() 'Added by Vikrant On 28-Feb-2019 For BA28022019
		txtCTotalValue.DataBind()
	End Sub
	'End
#End Region

End Class