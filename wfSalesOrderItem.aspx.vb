Partial Class wfSalesOrderItem
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents btnContactInfo As System.Web.UI.WebControls.Button
	Protected WithEvents btnBankInfo As System.Web.UI.WebControls.Button
	Protected WithEvents btnTaxInfo As System.Web.UI.WebControls.Button


	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable Description "
	Public mSalesOrder As SalesOrder
	Public mModelList As ModelList
	Dim mGSTPercentage As GSTPercentage
	Dim mVendor As Vendor
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mSalesOrder = Session("mSalesOrder")
		mModelList = Session("mModelList")
	End Sub
	Private Sub setSession()
		Session("mSalesOrder") = mSalesOrder
		Session("mModelList") = mModelList
	End Sub
	Private overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub addAttributes()
		txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
		txtRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRate').value,event)")
		txtOtherCharges.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOtherCharges').value,event)")
	End Sub
	Private Sub SetPage()
		If Session("Edit") Then
			lblTitle.Text = "Sales Order Item [" & mSalesOrder.SalesOrderItems.CurrentItem.ItemName & "]"
			imgbtnPartNo.BackColor = Color.Silver
			txtPartNo.BackColor = Color.Silver
		End If
	End Sub
	Private Function setObject() As Boolean
		mSalesOrder.BeginEdit()
		mSalesOrder.SalesOrderItems.CurrentItem.SrNo = mSalesOrder.SalesOrderItems.CurrentIndex + 1
		mSalesOrder.SalesOrderItems.CurrentItem.Qty = Val(txtQty.Text)
		mSalesOrder.SalesOrderItems.CurrentItem.CRate = Val(txtRate.Text)
		mSalesOrder.SalesOrderItems.CurrentItem.COtherCharges = Val(txtOtherCharges.Text)
		mSalesOrder.SalesOrderItems.CurrentItem.ModelID = New Guid(cmbApplicable.SelectedValue)
		mSalesOrder.SalesOrderItems.CurrentItem.ModelName = cmbApplicable.SelectedItem.Text
		mSalesOrder.SalesOrderItems.CurrentItem.Remark = Trim(txtRemark.Text)
		mSalesOrder.SalesOrderItems.CurrentItem.Note = Trim(txtNote.Text)

		'------------------------------------------------------------------
		If AppSettings("IsGSTApplicable") = "True" Then
			mVendor = Vendor.GetVendor(mSalesOrder.VendorID)
			If mVendor.ClientCountryName.ToUpper = "INDIA" Then
				If mVendor.CountryName.ToUpper = "INDIA" And mSalesOrder.Date >= CDate("01-Jul-2017") Then
					mGSTPercentage = GSTPercentage.GetPercentage(mSalesOrder.Date, 1, mSalesOrder.SalesOrderItems.CurrentItem.ItemID.ToString)
					If mGSTPercentage IsNot Nothing Then
						Dim mtmpItem As Item = Item.GetItem(mSalesOrder.SalesOrderItems.CurrentItem.ItemID)
						If Len(mVendor.StateCode) > 0 Then
							If mVendor.StateCode = mVendor.ClientStateCode Then
								If mSalesOrder.SalesOrderItems.CurrentItem.CGSTPercentage = 0 Then
									mSalesOrder.SalesOrderItems.CurrentItem.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
									mSalesOrder.SalesOrderItems.CurrentItem.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
								End If
								mSalesOrder.SalesOrderItems.CurrentItem.CGSTCAmount = ((mSalesOrder.SalesOrderItems.CurrentItem.CGSTPercentage * mSalesOrder.SalesOrderItems.CurrentItem.CAmount) / 100)
								mSalesOrder.SalesOrderItems.CurrentItem.SGSTCAmount = ((mSalesOrder.SalesOrderItems.CurrentItem.SGSTPercentage * mSalesOrder.SalesOrderItems.CurrentItem.CAmount) / 100)
								mSalesOrder.StateCode = mVendor.StateCode
								mSalesOrder.ClientStateCode = mVendor.ClientStateCode
								mSalesOrder.VendorCountry = mVendor.CountryName
								mSalesOrder.Visibility = 1
							Else
								If mSalesOrder.SalesOrderItems.CurrentItem.IGSTPercentage = 0 Then
									mSalesOrder.SalesOrderItems.CurrentItem.IGSTPercentage = (mGSTPercentage.GSTPercentage)
								End If
								mSalesOrder.SalesOrderItems.CurrentItem.IGSTCAmount = ((mSalesOrder.SalesOrderItems.CurrentItem.IGSTPercentage * mSalesOrder.SalesOrderItems.CurrentItem.CAmount) / 100)
								mSalesOrder.StateCode = mVendor.StateCode
								mSalesOrder.ClientStateCode = mVendor.ClientStateCode
								mSalesOrder.VendorCountry = mVendor.CountryName
								mSalesOrder.Visibility = 2
							End If
							mSalesOrder.SalesOrderItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode
						Else
							mSalesOrder.SalesOrderItems.CurrentItem.CGSTPercentage = 0
							mSalesOrder.SalesOrderItems.CurrentItem.SGSTPercentage = 0
							mSalesOrder.SalesOrderItems.CurrentItem.CGSTCAmount = 0
							mSalesOrder.SalesOrderItems.CurrentItem.SGSTCAmount = 0
							mSalesOrder.SalesOrderItems.CurrentItem.IGSTPercentage = 0
							mSalesOrder.SalesOrderItems.CurrentItem.IGSTCAmount = 0
							mSalesOrder.SalesOrderItems.CurrentItem.HSNACSCode = ""
							mSalesOrder.StateCode = mVendor.StateCode
							mSalesOrder.ClientStateCode = mVendor.ClientStateCode
							mSalesOrder.VendorCountry = mVendor.CountryName
							mSalesOrder.Visibility = 3
						End If
					End If
				Else
					mSalesOrder.SalesOrderItems.CurrentItem.CGSTPercentage = 0
					mSalesOrder.SalesOrderItems.CurrentItem.SGSTPercentage = 0
					mSalesOrder.SalesOrderItems.CurrentItem.CGSTCAmount = 0
					mSalesOrder.SalesOrderItems.CurrentItem.SGSTCAmount = 0
					mSalesOrder.SalesOrderItems.CurrentItem.IGSTPercentage = 0
					mSalesOrder.SalesOrderItems.CurrentItem.IGSTCAmount = 0
					mSalesOrder.SalesOrderItems.CurrentItem.HSNACSCode = ""
					mSalesOrder.StateCode = mVendor.StateCode
					mSalesOrder.ClientStateCode = mVendor.ClientStateCode
					mSalesOrder.VendorCountry = mVendor.CountryName
					mSalesOrder.Visibility = 3
				End If
			Else
				mSalesOrder.SalesOrderItems.CurrentItem.CGSTPercentage = 0
				mSalesOrder.SalesOrderItems.CurrentItem.SGSTPercentage = 0
				mSalesOrder.SalesOrderItems.CurrentItem.CGSTCAmount = 0
				mSalesOrder.SalesOrderItems.CurrentItem.SGSTCAmount = 0
				mSalesOrder.SalesOrderItems.CurrentItem.IGSTPercentage = 0
				mSalesOrder.SalesOrderItems.CurrentItem.IGSTCAmount = 0
				mSalesOrder.SalesOrderItems.CurrentItem.HSNACSCode = ""
				mSalesOrder.StateCode = mVendor.StateCode
				mSalesOrder.ClientStateCode = mVendor.ClientStateCode
				mSalesOrder.VendorCountry = mVendor.CountryName
				mSalesOrder.Visibility = 3
			End If
		Else
			mSalesOrder.Visibility = 3
		End If
		'------------------------------------------------------------------

		If mSalesOrder.SalesOrderItems.Contains(mSalesOrder.SalesOrderItems.CurrentItem) Then
			Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "SalesOrder Item", MsgBoxStyle.OKOnly)
			msg1.ReplacePage = "wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
			msg1.Show()
			mSalesOrder.CancelEdit()
			Exit Function
		Else
			mSalesOrder.ApplyEdit()
		End If
		mSalesOrder.CalculateTotal()
		If mSalesOrder.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
			mSalesOrder.RoundCGrandTotal()
		End If
		Return True
	End Function
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
			Result1 = -1
		Else
			Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		End If
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If CType(Session("sender"), String) = "Delete" Then
						Try
							'Session("Sender") = ""
							'Dim mSalesOrder As SalesOrder
							'mSalesOrder = CType(Session("mSalesOrder"), SalesOrder)
							'mSalesOrder.SalesOrderItems.RemoveAt(mSalesOrder.SalesOrderItems.CurrentIndex)
							'Session("mSalesOrder") = mSalesOrder
							'Response.Redirect("wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
						Catch ex As SqlException
							If ex.Number = 8145 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
								msg1.ReplacePage = "wfSalesOrderItem.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							ElseIf ex.Number = 2627 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
								msg1.ReplacePage = "wfSalesOrderItem.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							ElseIf ex.Number = 547 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
								msg1.ReplacePage = "wfSalesOrderItem.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							End If
						End Try
					End If
				Case MsgBoxResult.No
					Session("Sender") = ""
					Response.Redirect("wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.OK 'And Session("sender") = ""        'Code Added
					Session("sender") = ""
					DataFieldBind()
					Response.Redirect("wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
					DataFieldBind()
					Response.Redirect("wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			Response.Redirect("wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			DataFieldBind()
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mModelList = ModelList.GetModelList(mSalesOrder.SalesOrderItems.CurrentItem.ItemID, True)
		Session("mModelList") = mModelList
		cmbApplicable.DataSource = mModelList
		DataBind()
	End Sub
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		' If custValidator.ControlToValidate = "cmbApplicable" Then
		'If cmbApplicable.SelectedIndex <= 0 Then
		'    custValidator.ErrorMessage = "Select applicable model for part from the list."
		'    e.IsValid = False
		'End If
		If custValidator.ControlToValidate = "txtQty" Then
			If Val(txtQty.Text) <= 0 Then
				custValidator.ErrorMessage = "Quantity can't be Zero or negative."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtRate" Then
			If Val(txtRate.Text) <= 0 Then
				custValidator.ErrorMessage = "Rate must be greater than zero."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtOtherCharges" Then
			If Val(txtOtherCharges.Text) < 0 Then
				custValidator.ErrorMessage = "Other Charge Can't be negative."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtRemark" Then
			If txtRemark.Text.Length > 250 Then
				custValidator.ErrorMessage = "Remark too long."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtNote" Then
			If txtNote.Text.Length > 250 Then
				custValidator.ErrorMessage = "Note too long."
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
			If txtPartNo.Enabled = True Then
				setFocus(txtPartNo)
			End If
			DataFieldBind()
		End If
		SetPage()
		If mSalesOrder.SalesOrderItems.CurrentItem.QuotationItemID.Equals(Guid.Empty) Then
			txtQuotaionDate.Visible = False
			txtQuotationNo.Visible = False
			lblQuodate.Visible = False
			lblQuoNo.Visible = False
			lblQuoItemInformation.Visible = False
		End If
	End Sub
	Private Sub imgbtnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnPartNo.Click
		''If Not (User.IsInRole("SalesOrderNew") And User.IsInRole("SalesOrderEdit") And User.IsInRole("SalesOrderDelete")) Then
		''    setObject()
		''    setSession()
		''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
		''    msg.ReplacePage = "wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
		''    Session("sender") = "Authorization"
		''    msg.Show()
		''    Exit Sub
		''End If
		setObject()
		mSalesOrder.SalesOrderItems.CurrentItem.ModelID = Guid.Empty
		Session("mSalesOrder") = mSalesOrder
		Session("PartNo") = txtPartNo.Text
		Response.Redirect("wfPartListForSalesOrder.aspx?BackPage=wfSalesOrder_Ajax.aspx&ChildPage=wfSalesOrderItem.aspx&Name=" & txtPartNo.Text)
	End Sub
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		''If (Not User.IsInRole("SalesOrderNew") And mSalesOrder.IsNew) Or (Not User.IsInRole("SalesOrderEdit") And Not mSalesOrder.IsNew) Then
		''    setObject()
		''    setSession()
		''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
		''    msg.ReplacePage = "wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
		''    Session("sender") = "Authorization"
		''    msg.Show()
		''    Exit Sub
		''End If

		If IsValid Then
			If setObject() Then
				Session("mSalesOrder") = mSalesOrder
				Session.Remove("mModelList")
				Session.Remove("Edit")
				Response.Redirect(Request.QueryString("BackPage"))
			End If
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If mSalesOrder.SalesOrderItems.CurrentItem.IsNew And Not Session("Edit") = True Then mSalesOrder.SalesOrderItems.Remove(mSalesOrder.SalesOrderItems.CurrentItem)
		Session.Remove("Edit")
		Session.Remove("mModelList")
		Response.Redirect(Request.QueryString("BackPage"))
	End Sub
#End Region

End Class
