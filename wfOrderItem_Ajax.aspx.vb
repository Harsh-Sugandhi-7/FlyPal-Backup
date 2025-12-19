Public Class wfOrderItem_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Description "
	Public mOrder As Order
	Public mModelList As ModelList
	Public mOrderItemQuotationItems As OrderItemQuotationItems
	Public mPriorityList As PriorityList
	Public mPartTypeList As PartTypeList
	Public Flag As Integer
	Public mGSTPercentage As GSTPercentage
	Public mVendor As Vendor
	Public mUnitConverterList As UnitConverterList 'Added By Prashant 5-Feb-2019 ALL04022019
#End Region

#Region " Enumaration "
	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		FindNow = 7
	End Enum
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mOrder = Session("mOrder")
		mModelList = Session("mModelList")
		mPriorityList = Session("mPriorityList")
	End Sub
	Private Sub setSession()
		Session("mOrder") = mOrder
		Session("mModelList") = mModelList
		Session("mPriorityList") = mPriorityList
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub addAttributes()
		txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
		txtRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRate').value,event)")
		txtbillBackRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtbillBackRate').value,event)")
		txtDeliveryInDays.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtDeliveryInDays').value,event)")
		txtPerDiscount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPerDiscount').value,event)")
	End Sub
	Private Sub SetPage()
		If Session("Edit") Then
			lblTitle.Text = "Purchase Order Item [" & mOrder.OrderItems.CurrentItem.ItemName & "]"
			imgbtnPartNo.BackColor = Color.Silver
			txtPartNo.BackColor = Color.Silver
		End If
	End Sub
	Private Function setObject() As Boolean
		mOrder.OrderItems.CurrentItem.SrNo = mOrder.OrderItems.CurrentIndex + 1
		mOrder.OrderItems.CurrentItem.Qty = Val(txtQty.Text)
		mOrder.OrderItems.CurrentItem.CRate = Val(txtRate.Text)
		mOrder.OrderItems.CurrentItem.ConversionFactor = mOrder.ConversionFactor
		mOrder.OrderItems.CurrentItem.ModelID = New Guid(cmbApplicable.SelectedValue)
		mOrder.OrderItems.CurrentItem.ModelName = cmbApplicable.SelectedItem.Text
		mOrder.OrderItems.CurrentItem.Remark = Trim(txtRemark.Text)
		mOrder.OrderItems.CurrentItem.Note = Trim(txtNote.Text)
		mOrder.OrderItems.CurrentItem.SerialNo = Trim(txtSerialNo.Text)
		mOrder.OrderItems.CurrentItem.CBillBackRate = Val(txtbillBackRate.Text)
		mOrder.OrderItems.CurrentItem.DeliveryInDays = Val(txtDeliveryInDays.Text)
		mOrder.OrderItems.CurrentItem.PriorityID = CInt(cmbPriority.SelectedValue)
		mOrder.OrderItems.CurrentItem.PerDiscount = Val(txtPerDiscount.Text)
		mOrder.OrderItems.CurrentItem.RequestedBy = Trim(txtRequestedBy.Text)
		mOrder.OrderItems.CurrentItem.IsWarrantyApplicable = chkWarrantyApplicable.Checked

		mOrder.OrderItems.CurrentItem.UnitID = New Guid(cmbUnitConverterList.SelectedValue) 'Added By Prashant 5-Feb-2019 ALL04022019
		mOrder.OrderItems.CurrentItem.UnitName = cmbUnitConverterList.SelectedItem.Text     'Added By Prashant 5-Feb-2019 ALL04022019

		If chkScheduleExpensesYes.Checked = True Then
			mOrder.OrderItems.CurrentItem.IsScheduleExpenses = True
		ElseIf chkScheduleExpensesNo.Checked = True Then
			mOrder.OrderItems.CurrentItem.IsScheduleExpenses = False
		End If

		mOrder.OrderItems.CurrentItem.ItemTypeID = Val(cmbPartType.SelectedValue)
		If (mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) Then
			mOrder.OrderItems.CurrentItem.TempEROQtyForEnableDisable = Val(txtQty.Text)
		End If

		Dim mOrderItemQuotationItem As OrderItemQuotationItem
		Dim txtValue As TextBox
		Dim i As Integer = 0
		For Each mOrderItemQuotationItem In mOrder.OrderItems.CurrentItem.OrderItemQuotationItems
			With mOrderItemQuotationItem
				txtValue = CType(Me.dgQuotaiontionItemList.Rows(i).FindControl("txtReqQty"), TextBox)
				.Qty = CDec(Val(txtValue.Text))
			End With
			i = i + 1
		Next
		txtQty.DataBind()

		If mOrder.OrderItems.Contains(mOrder.OrderItems.CurrentItem) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Order Item", MsgBoxStyle.OkOnly, "")
			mOrder.CancelEdit()
			Exit Function
		End If

		Dim mtmpItem As Item = Item.GetItem(mOrder.OrderItems.CurrentItem.ItemID)
		If mtmpItem.NotInUse = True Then
			If CDate(mtmpItem.NotInUseDate) <= CDate(mOrder.OrderDate) Then
				MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Part is not applicable since " + mtmpItem.NotInUseDateFormatted + " <br><br> Select another Part from list & try again", MsgBoxStyle.OkOnly, "")
				Exit Function
			End If
		End If
		'------------------------------------------------------------------
		mOrder.OrderItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
		If AppSettings("IsGSTApplicable") = "True" Then
			mVendor = Vendor.GetVendor(mOrder.VendorID)
			If mVendor.ClientCountryName.ToUpper = "INDIA" Then
				If mVendor.CountryName.ToUpper = "INDIA" And mOrder.OrderDate >= CDate("01-Jul-2017") Then
					mGSTPercentage = GSTPercentage.GetPercentage(mOrder.OrderDate, 1, mOrder.OrderItems.CurrentItem.ItemID.ToString)
					If mGSTPercentage IsNot Nothing Then
						If Len(mVendor.StateCode) > 0 Then
							If mVendor.StateCode = mVendor.ClientStateCode Then
								If mOrder.OrderItems.CurrentItem.CGSTPercentage = 0 Then
									mOrder.OrderItems.CurrentItem.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
									mOrder.OrderItems.CurrentItem.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
								End If
								mOrder.OrderItems.CurrentItem.CGSTCAmount = ((mOrder.OrderItems.CurrentItem.CGSTPercentage * mOrder.OrderItems.CurrentItem.CAmount) / 100)
								mOrder.OrderItems.CurrentItem.SGSTCAmount = ((mOrder.OrderItems.CurrentItem.SGSTPercentage * mOrder.OrderItems.CurrentItem.CAmount) / 100)

								mOrder.OrderItems.CurrentItem.TotalCAmount = mOrder.OrderItems.CurrentItem.CAmount + mOrder.OrderItems.CurrentItem.CGSTCAmount + mOrder.OrderItems.CurrentItem.SGSTCAmount
								mOrder.OrderItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

								mOrder.StateCode = mVendor.StateCode
								mOrder.ClientStateCode = mVendor.ClientStateCode
								mOrder.VendorCountry = mVendor.CountryName
								mOrder.Visibility = 1
							Else
								If mOrder.OrderItems.CurrentItem.IGSTPercentage = 0 Then
									mOrder.OrderItems.CurrentItem.IGSTPercentage = (mGSTPercentage.GSTPercentage)
								End If
								mOrder.OrderItems.CurrentItem.IGSTCAmount = ((mOrder.OrderItems.CurrentItem.IGSTPercentage * mOrder.OrderItems.CurrentItem.CAmount) / 100)

								mOrder.OrderItems.CurrentItem.TotalCAmount = mOrder.OrderItems.CurrentItem.CAmount + mOrder.OrderItems.CurrentItem.IGSTCAmount
								mOrder.OrderItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

								mOrder.StateCode = mVendor.StateCode
								mOrder.ClientStateCode = mVendor.ClientStateCode
								mOrder.VendorCountry = mVendor.CountryName
								mOrder.Visibility = 2
							End If
						Else
							mOrder.StateCode = mVendor.StateCode
							mOrder.ClientStateCode = mVendor.ClientStateCode
							mOrder.VendorCountry = mVendor.CountryName
							mOrder.Visibility = 3
						End If
					End If
				Else
					mOrder.StateCode = mVendor.StateCode
					mOrder.ClientStateCode = mVendor.ClientStateCode
					mOrder.VendorCountry = mVendor.CountryName
					mOrder.Visibility = 3
				End If
			Else
				mOrder.StateCode = mVendor.StateCode
				mOrder.ClientStateCode = mVendor.ClientStateCode
				mOrder.VendorCountry = mVendor.CountryName
				mOrder.Visibility = 3
			End If
		Else
			mOrder.Visibility = 3
		End If
		'------------------------------------------------------------------
		mOrder.ApplyEdit()
		mOrder.CalculateTotal()
		If mOrder.IsRoundOff = True Then
			mOrder.RoundCGrandTotal()
		End If
		Return True
	End Function
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							Session("Sender") = ""
							Dim mOrder As Order
							mOrder = CType(Session("mOrder"), Order)
							mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.Remove(mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.CurrentItem.ID)
							Session("mOrder") = mOrder
							Session("Sender") = ""
							mOrderItemQuotationItems = mOrder.OrderItems.CurrentItem.OrderItemQuotationItems
							dgQuotaiontionItemList.DataSource = mOrderItemQuotationItems
							dgQuotaiontionItemList.DataBind()
							upnlTSNTSOValues.Update()
						Catch ex As SqlException
							MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
							Exit Sub
						End Try
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Delete" Then
						Session("Sender") = ""
					End If
			End Select
		End If
	End Sub
	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		'Deciding IsInRole String to check Rights
		Select Case mOrder.TransTypeID
			Case Util.Trans.PurchaseOrder
				IsInRoleString = "Order"
			Case Util.Trans.PurchaseOrderForExchangeRepair
				IsInRoleString = "OrderForExchange"
			Case Util.Trans.OverHaulRepairOrder
				IsInRoleString = "PurchaseOrderRepairOverHaul"
			Case Util.Trans.RentialLeaseOtder
				IsInRoleString = "PurchaseOrderRentalLease"
		End Select
		'Depending upon decided IsInRole String; checkign Rights of the User
		Select Case CheckFor
			Case Rights.[New]
				Return User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return User.IsInRole(IsInRoleString + "Delete")
			Case Rights.View
				Return User.IsInRole(IsInRoleString + "View")
			Case Rights.Print
				Return User.IsInRole(IsInRoleString + "Print")
			Case Rights.FindNow
				Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
		End Select
	End Function
	Private Sub DeleteRecord(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "Delete")
		mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.CurrentIndex = Index
		Session("mOrder") = mOrder
	End Sub
	Private Sub AddPart()
		Dim mQuotationItem As QuotationItem
		Dim mQuotationItems As QuotationItems = Session("mQuotationItems")
		If mQuotationItems Is Nothing Then Exit Sub

		For Each mQuotationItem In mQuotationItems
			If mQuotationItem.IsSelect Then

				With mOrder.OrderItems.CurrentItem
					'Check is Quotation Part is present ?
					If Not .OrderItemQuotationItems.Contains(mQuotationItem.ID) Then
						'if NOT then add
						.OrderItemQuotationItems.Add(.ID, mQuotationItem.ID, mQuotationItem.Qty, mQuotationItem.QuotationNo, mQuotationItem.QuotationDate.ToString, mQuotationItem.QuotationID)
					Else
						MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Quotation item already taken for Order", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
				End With
			End If
		Next
		Session.Remove("mQuotationItems")
	End Sub
	Public Sub ControlVisibilityForQty()
		'If (mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 6) Or (mOrder.AgainstTypeID = 3) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.OrderItems.CurrentItem.IsSerializedPart = True) Then
		If (mOrder.AgainstTypeID = 3) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.OrderItems.CurrentItem.IsSerializedPart = True) Then
			txtQty.Enabled = False
		Else
			txtQty.Enabled = True
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mModelList = ModelList.GetModelList(mOrder.OrderItems.CurrentItem.ItemID, True)
		Session("mModelList") = mModelList
		cmbApplicable.DataSource = mModelList

		mOrderItemQuotationItems = mOrder.OrderItems.CurrentItem.OrderItemQuotationItems
		dgQuotaiontionItemList.DataSource = mOrderItemQuotationItems

		mPriorityList = PriorityList.GetPriorityList(, , "")
		Session("mPriorityList") = mPriorityList
		cmbPriority.DataSource = mPriorityList

		mPartTypeList = PartTypeList.GetPartTypeList(True)
		cmbPartType.DataSource = mPartTypeList

		mUnitConverterList = UnitConverterList.GetUnitConverterList(mOrder.OrderItems.CurrentItem.ItemID)
		cmbUnitConverterList.DataSource = mUnitConverterList

		If mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.Requisition Then
			lblSalesOrderNo.Text = "No. "
			lblSalesOrderDate.Text = "Date "
			lgdInformation.InnerText = "Requisition Information"
		ElseIf mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.SalesOrder Then
			lblSalesOrderNo.Text = "No. "
			lblSalesOrderDate.Text = "Date "
			lgdInformation.InnerText = "Sales Order Information"
		ElseIf mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.FromStock Then
			lblSalesOrderNo.Text = "No. "
			lblSalesOrderDate.Text = "Date "
			lgdInformation.InnerText = "Receipt Information"
		Else
			lblSalesOrderNo.Text = "No. "
			lblSalesOrderDate.Text = "Date "
			lgdInformation.InnerText = "Receipt Information"
		End If
		DataBind()
		If mOrder.TransTypeID = 5 And (mOrder.AgainstTypeID = 1 Or mOrder.AgainstTypeID = 2) And mUnitConverterList.Count > 1 And mOrder.OrderItems.CurrentItem.IsSerializedPart = False Then 'mOrder.AgainstTypeID = 1 New purchase against item i.e none
			cmbUnitConverterList.Enabled = True
		Else
			cmbUnitConverterList.Enabled = False
		End If
	End Sub
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
		ElseIf custValidator.ControlToValidate = "txtRemark" Then
			If Len(txtRemark.Text) > 250 Then
				custValidator.ErrorMessage = "Remark must not be greater than 250 Char."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtNote" Then
			If Len(txtNote.Text) > 250 Then
				custValidator.ErrorMessage = "Note must not be greater than 250 Char."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtPerDiscount" Then
			If Val(txtPerDiscount.Text) > 100 Then
				custValidator.ErrorMessage = "Discount can not be greater than 100 %"
				e.IsValid = False
			End If
		End If
	End Sub
	Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		If Flag = 1 Then Exit Sub

		Dim CustValidator As CustomValidator
		CustValidator = CType(s, CustomValidator)
		Dim strMsg As String = ""
		setObject()

		If Not mOrder.IsValid Then
			For i As Integer = 0 To mOrder.GetBrokenRulesCollection.Count - 1
				strMsg = strMsg + mOrder.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		Dim mOrderItem As OrderItem
		If Not mOrder.OrderItems.IsValid Then
			For Each mOrderItem In mOrder.OrderItems
				For i As Integer = 0 To mOrderItem.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mOrderItem.ItemName + " : " + mOrderItem.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			Next
		End If

		Dim mOrderItemQuotationItem As OrderItemQuotationItem
		If Not mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.IsValid Then
			For Each mOrderItemQuotationItem In mOrder.OrderItems.CurrentItem.OrderItemQuotationItems
				For i As Integer = 0 To mOrderItemQuotationItem.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mOrderItemQuotationItem.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			Next
		End If

		If strMsg.Trim <> "" Then
			CustValidator.ErrorMessage = strMsg
			e.IsValid = False
		End If

		Flag = 1
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		getSession()
		addAttributes()

		dgQuotaiontionItemList.Columns(5).Visible = (mOrder.AgainstTypeID = 3)

		If CType(Session("AddPart"), String) = "True" Then
			'Add selected part(s) to Enquiry Items
			AddPart()
			Session("AddPart") = "False"
		Else
			Session("AddPart") = "False"
		End If

		If Not IsPostBack Then
			If txtPartNo.Enabled = True Then
				setFocus(txtPartNo)
			End If
			DataFieldBind()
		End If
		ControlVisibilityForQty()
		SetPage()
	End Sub
	Private Sub imgbtnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnPartNo.Click
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 1 Then  'New Purchase and Part(Direct) 
			setObject()
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			Session("mOrder") = mOrder
			Session("PartNo") = txtPartNo.Text
			Session("mPriorityList") = mPriorityList
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 2 Then ' New Purchase and Requisition
			setObject()
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			Session("mOrder") = mOrder
			Session("PartNo") = txtPartNo.Text
			Session("mPriorityList") = mPriorityList
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 3 Then ' New Purchase and Approval Quots.
			setObject()
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			Session("mOrder") = mOrder
			Session("PartNo") = txtPartNo.Text
			Session("mPriorityList") = mPriorityList
			Response.Redirect("wfMgtApprovedQuotationItems.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 4 Then                                 ' New Purchase and Sales Order.
			setObject()
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			Session("mOrder") = mOrder
			Session("PartNo") = txtPartNo.Text
			Session("mPriorityList") = mPriorityList
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If (mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.AgainstTypeID = 1 Then    ' (Exchange, Overhaul, Repair) and Part (Direct).
			setObject()
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			Session("mOrder") = mOrder
			Session("PartNo") = txtPartNo.Text
			Session("mPriorityList") = mPriorityList
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If (mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.AgainstTypeID = 5 Then    ' (Exchange, Overhaul, Repair) and From Stock.
			setObject()
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			Session("mOrder") = mOrder
			Session("PartNo") = txtPartNo.Text
			Session("mPriorityList") = mPriorityList
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If mOrder.TransTypeID = 39 And mOrder.AgainstTypeID = 1 Then                                 'Purchase for Rentail / Lease and Part(Direct) 
			setObject()
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			Session("mOrder") = mOrder
			Session("PartNo") = txtPartNo.Text
			Session("mPriorityList") = mPriorityList
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
	End Sub
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If (Not IsInRole(Rights.[New]) And mOrder.IsNew) Or (Not IsInRole(Rights.Edit) And Not mOrder.IsNew) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		If IsValid Then
			If setObject() Then
				Session("mOrder") = mOrder
				Session.Remove("mModelList")
				Session.Remove("mPriorityList")
				Session.Remove("Edit")
				Response.Redirect(Request.QueryString("BackPage"))
			End If
		Else
			upnlValidationSummary.Update()
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If mOrder.OrderItems.CurrentItem.IsNew And Not Session("Edit") = True Then mOrder.OrderItems.Remove(mOrder.OrderItems.CurrentItem)
		Session.Remove("Edit")
		Session.Remove("mModelList")
		Session.Remove("mPriorityList")
		Response.Redirect(Request.QueryString("BackPage"))
	End Sub
	Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		setObject()
		mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
		Session("mOrder") = mOrder
		Session("PartNo") = txtPartNo.Text
		Session("mPriorityList") = mPriorityList

		If Not mOrder.OrderItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
			Session("TransDate") = mOrder.OrderDate.ToString
			Session("OrderItem") = mOrder.OrderItems.CurrentItem.ItemID
			Response.Redirect("wfMgtApprovedQuotationItems.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If

	End Sub
	Private Sub dgQuotaiontionItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgQuotaiontionItemList.RowCommand
		Dim Index As Integer = CInt(e.CommandArgument) + dgQuotaiontionItemList.PageIndex * dgQuotaiontionItemList.PageSize
		Select Case e.CommandName
			Case "ForDelete"
				DeleteRecord(Index)
		End Select
	End Sub
	Private Sub btnSerialNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSerialNo.Click
		setObject()
		Session.Remove("mIssue")
		Session("mOrder") = mOrder
		Session("PartNo") = txtPartNo.Text
		Response.Redirect("wfPartStockStatus_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub
	'Added By Vikrant On 12-Sep-2018 For ALL12092018
	Private Sub btnAlternatePart_Click(sender As Object, e As System.EventArgs) Handles btnAlternatePart.Click
		setObject()
		Session("mItem") = Item.GetItem(mOrder.OrderItems.CurrentItem.ItemID)
		Response.Redirect("wfAlternatePartListForOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfOrderItem_Ajax.aspx")
	End Sub
	'End
#End Region


End Class