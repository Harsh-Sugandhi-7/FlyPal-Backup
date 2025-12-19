Imports System.Linq
Imports System.Linq.Enumerable

Public Class wfAutocOrderCreation_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mReceipt As Receipt
	Public mReceiptCumInvoice As ReceiptCumInvoice
	Public mType As String
	Public mItemID As Guid = Guid.Empty
	Public mItemNo As String
	Public mItemList As ItemList
	Public mVendorList As VendorList
	Public mVendorID As Guid
	Public BackPage As String
	Public mOrderList As OrderList
	Public mCurrencyList As CurrencyList
	Dim OrderDate, Qty, Supplier, Currency, Factor, IntOrderNo, OrderNo As String
	Dim Indx1 As Integer = -1
	Dim SelectedItemName As String = String.Empty
#End Region

#Region "Service Methods"
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetOrderList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim tmpOrderlist As OrderList
		Dim OrderText As String()
		OrderText = prefixText.Split("-")

		tmpOrderlist = OrderList.GetOrderList(, "", , , "", "1-1-1850", "1-1-2200", , , "")

		If count = 0 Then
			Return (From c As OrderList.OrderInfo In tmpOrderlist Where c.OrderNo.Contains(prefixText.ToString.ToUpper)
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.OrderNo, c.ID.ToString())).ToArray
		Else
			Return (From c As OrderList.OrderInfo In tmpOrderlist Where c.OrderNo.Contains(prefixText.ToString.ToUpper)
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.OrderNo, c.ID.ToString())).Take(count).ToArray

		End If
	End Function
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mReceipt = CType(Session("mReceipt"), Receipt)
		mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)

		If mReceipt IsNot Nothing Then
			mVendorID = IIf(mReceipt IsNot Nothing, mReceipt.VendorID, Guid.Empty)
		End If

		If mReceiptCumInvoice IsNot Nothing Then
			mVendorID = IIf(mReceiptCumInvoice IsNot Nothing, mReceiptCumInvoice.VendorID, Guid.Empty)
		End If

		mVendorID = IIf(mReceipt IsNot Nothing, mReceipt.VendorID, Guid.Empty)
		mItemList = Session("mItemList")
		mVendorList = Session("mVendorList")
		mOrderList = CType(Session("mOrderList"), OrderList)
		mCurrencyList = CType(Session("mCurrencyList"), CurrencyList)
		OrderDate = Session("OrderDate")
		Qty = Session("Qty")
		Supplier = Session("Supplier")
		Currency = Session("Currency")
		Factor = Session("Factor")
		IntOrderNo = Session("IntOrderNo")
		OrderNo = Session("OrderNo")
		Indx1 = Session("Indx1")
		SelectedItemName = Session("SelectedItemName")
	End Sub
	Private Sub SetSession()
		Session("mReceipt") = mReceipt
		Session("mReceiptCuminvoice") = mReceiptCumInvoice
		Session("mVendorID") = mVendorID
		Session("mItemList") = mItemList
		Session("mVendorList") = mVendorList
		Session("mOrderList") = mOrderList
		Session("mCurrencyList") = mCurrencyList
		Session("OrderDate") = OrderDate
		Session("Qty") = Qty
		Session("Supplier") = Supplier
		Session("Currency") = Currency
		Session("Factor") = Factor
		Session("IntOrderNo") = IntOrderNo
		Session("OrderNo") = OrderNo
		Session("SelectedItemName") = SelectedItemName
	End Sub
	Private Sub RemoveSessions()
		Session.Remove("mVendorList")
		'Session.Remove("mItemList")
		Session.Remove("mOrderList")
		Session.Remove("mCurrencyList")
		Session.Remove("OrderDate")
		Session.Remove("Qty")
		Session.Remove("Supplier")
		Session.Remove("Currency")
		Session.Remove("Factor")
		Session.Remove("IntOrderNo")
		Session.Remove("OrderNo")
		Session.Remove("SelectedItemName")
	End Sub
	Private Sub ControlVisibilityNewOrder()
		cmbOrderList.Visible = False
		txtOrderList.Visible = False
		txtOrderList.Text = ""
		lblOrder.Visible = False
		'btnFindNow1.Visible = False Ajay 09-02-2023
		txtOrderDate.Visible = True
		lblOrderdate.Visible = True
		txtOrderDate.Enabled = True
		txtConversionFactor.Visible = True
		lblConvFactor.Visible = True
		cmbCurrencyList.Visible = True
		lblCurrency.Visible = True
		lblVendor.Visible = True
		cmbVendor.Visible = True
		btnAdd.Enabled = True
		btnAdd.Text = "Create"
		btnAdd.ToolTip = "Click to create order"
	End Sub
	Private Sub ControlVisibilityExistingOrder()
		txtOrderDate.Visible = False
		lblOrderdate.Visible = False
		txtOrderDate.Enabled = False
		txtConversionFactor.Visible = False
		lblConvFactor.Visible = False
		cmbCurrencyList.Visible = False
		lblCurrency.Visible = False
		lblVendor.Visible = False
		cmbVendor.Visible = False
		' cmbOrderList.Visible = True         Commented on 14-May-2019 by Shital
		txtOrderList.Visible = True             'Added on 14-May-2019 by Shital
		lblOrder.Visible = True
		'btnFindNow1.Visible = True Ajay 09-02-20203
		btnAdd.Enabled = True
		btnAdd.Text = "Add"
		btnAdd.ToolTip = "Click to add an item into selected order"
		lblIntrd.Visible = False
		txtInternelOrdNo.Visible = False

	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
		cntrl.Focus()
	End Sub
	Private Sub FindNowList(Optional ByVal ItemName As String = "")
		mItemList = ItemList.GetItemList(1, ItemName)
		dgPendingReceiptItemList.DataSource = mItemList
		Session("mItemList") = mItemList
		dgPendingReceiptItemList.DataBind()
		lblListOfPendingReceipt.Text = "List of Items as per criteria:" & mItemList.Count & " Record(s) found."
		upnlPendingReceiptItemList.Update()
	End Sub
	Private Sub SetVariables()
		OrderDate = IIf(txtOrderDate.Text <> "", txtOrderDate.Text, Today.Date)
		Qty = IIf(txtQty.Text <> "", txtQty.Text, "0")
		Supplier = IIf(cmbVendor.SelectedIndex < 0, 0, cmbVendor.SelectedIndex)
		Currency = IIf(cmbCurrencyList.SelectedIndex < 0, 0, cmbCurrencyList.SelectedIndex)
		Factor = txtConversionFactor.Text.Trim
		IntOrderNo = Trim(txtInternelOrdNo.Text)
		OrderNo = IIf(cmbOrderList.SelectedIndex < 0, 0, cmbOrderList.SelectedIndex)

		Session("OrderDate") = OrderDate
		Session("Qty") = Qty
		Session("Supplier") = Supplier
		Session("Currency") = Currency
		Session("Factor") = Factor
		Session("IntOrderNo") = IntOrderNo
		Session("OrderNo") = OrderNo
	End Sub
	Private Sub SetControl()
		If Not IsNothing(OrderDate) Then 'CNDC
			txtOrderDate.Text = OrderDate
		End If
		txtQty.Text = Qty
		cmbVendor.SelectedIndex = Supplier 'IIf(Supplier = "", "(All)", Supplier)
		cmbCurrencyList.SelectedIndex = Currency 'IIf(Currency = "", "(All)", Currency)
		txtConversionFactor.Text = Factor
		txtInternelOrdNo.Text = IntOrderNo
		cmbOrderList.SelectedIndex = OrderNo ' IIf(OrderNo = "", "(All)", OrderNo)
	End Sub
	Private Sub addAttributes()
		txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
		txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
	End Sub
	'Added by Utkarsh on 17-Dec-2013 for Trans Text Series
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "OrderTransTextSeriesAlert" Then
						Session("Sender") = ""
						Session("AddTransTextSeries") = "True"
						Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
					End If
			End Select
		End If
	End Sub
	'End
#End Region

#Region " Data Binding "
	Private Sub DataFieldBindList()
		mItemList = ItemList.GetItemList(1, "0")
		dgPendingReceiptItemList.DataSource = mItemList
		Session("mItemList") = mItemList
		lblListOfPendingReceipt.Text = "List of Pending Receipt Items as per criteria:" & mItemList.Count & " Record(s) found."
		'Added New
		mCurrencyList = CurrencyList.GetCurrencyList(, , True)
		cmbCurrencyList.DataSource = mCurrencyList
		mVendorList = VendorList.GetVendortList(0, , , , , , True, , True)
		Session("mVendorList") = mVendorList
		cmbVendor.DataSource = mVendorList
		Dim mVendor As Vendor
		'mVendor = Vendor.GetVendor(mReceipt.VendorID)
		'Coad Added 
		'DEVEN 19/03/2008
		If mReceipt IsNot Nothing Then
			mVendor = Vendor.GetVendor(mReceipt.VendorID)
		ElseIf mReceiptCumInvoice IsNot Nothing Then
			mVendor = Vendor.GetVendor(mReceiptCumInvoice.VendorID)
		End If

		mOrderList = OrderList.GetOrderList(, , , , txtInternelOrdNo.Text, "1-1-1850", "1-1-2200", , , mVendor.Name)
		cmbOrderList.DataSource = mOrderList
		Session("mOrderList") = mOrderList
		mVendor = Nothing
		DataBind()
	End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtQty" Then
			If Val(txtQty.Text) <= 0 And txtQty.Visible = True Then
				custValidator.ErrorMessage = "Enter Qty for Order Item."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtOrderDate" Then
			If txtOrderDate.Text = "" And txtOrderDate.Visible = True Then
				custValidator.ErrorMessage = "Select Order Date."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "cmbCurrencyList" Then
			If cmbCurrencyList.SelectedIndex <= 0 And cmbCurrencyList.Visible = True Then
				custValidator.ErrorMessage = "Select Currency from the List."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtConversionFactor" Then
			If Val(txtConversionFactor.Text) <= 0 And txtConversionFactor.Visible Then
				custValidator.ErrorMessage = "Currency factor must be greater than zero."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "cmbVendor" Then
			If Val(cmbVendor.SelectedIndex) <= 0 And cmbVendor.Visible Then
				custValidator.ErrorMessage = "Select Supplier from the List"
				e.IsValid = False
			End If
		ElseIf txtConversionFactor.Text = "" And txtConversionFactor.Visible Then
			custValidator.ErrorMessage = "Currency factor must be greater than zero."
			e.IsValid = False
		End If
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		addAttributes()
		If Not IsPostBack And Session("Sender") = "" Then
			If txtSearch.Enabled = True Then
				setFocus(txtSearch)
			End If
			btnAdd.Text = "Create"
			cmbOrderList.Visible = False
			txtOrderList.Visible = False         'Added on 14-May-2019 by Shital
			lblOrder.Visible = False
			'btnFindNow1.Visible = False Ajay 09-02-2023
			optNewOrder.Checked = True
			If OrderDate = "" Then
				txtOrderDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			End If
			mItemNo = Request.QueryString("ItemNo")
			txtSearch.Text = mItemNo
			DataFieldBindList()
			SetControl()
		End If
		'lblListOfPendingReceipt.Text = "List of Pending Receipt Items as per criteria:" & mItemList.Count & " Record(s) found."
		SetSession()
		'Coad Added 
		'DEVEN 19/03/2008
		mType = Request.QueryString("mType")
		BackPage = Request.QueryString("BackPage")
		MessageBoxResult() 'Added by Utkarsh on 17-Dec-2013 for Trans Text Series
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		dgPendingReceiptItemList.PageIndex = 0
		FindNowList(Trim(txtSearch.Text))
	End Sub
	Private Sub dgPendingReceiptItemList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingReceiptItemList.RowCommand
		Select Case e.CommandName
			Case "SelectRec"
				Indx1 = CInt(e.CommandArgument) + dgPendingReceiptItemList.PageIndex * dgPendingReceiptItemList.PageSize
				Session("Indx1") = Indx1
				SelectedItemName = mItemList.Item(Indx1).Name
				Session("SelectedItemName") = SelectedItemName
				lblCreate.Text = " Create / Edit order for the above Part : " + mItemList.Item(Indx1).Name
		End Select
	End Sub
	Private Sub dgPendingReceiptItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingReceiptItemList.PageIndexChanging
		dgPendingReceiptItemList.PageIndex = e.NewPageIndex
		dgPendingReceiptItemList.DataSource = mItemList
		dgPendingReceiptItemList.DataBind()
		upnlPendingReceiptItemList.Update()
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Session.Remove("mItemList")
		mItemList = Nothing
		SetSession()
		RemoveSessions()
		'Coad Added 
		'DEVEN 19/03/2008
		'Response.Redirect("wfReceiptPendingOrderList.aspx?BackPage=" & BackPage & "&mType=1" 
		Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=" & BackPage & "&mType=" & mType & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	''Ajay 09-02-2023
	'Private Sub btnFindNow1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow1.Click
	'    Dim tmpVendorID As Guid = mVendorList.Item(cmbVendor.SelectedIndex).ID
	'    Dim mVendor As Vendor
	'    mVendor = Vendor.GetVendor(tmpVendorID)
	'    mOrderList = OrderList.GetOrderList(, , , , txtInternelOrdNo.Text, "1-1-1850", "1-1-2200", 2, , mVendor.Name)
	'    cmbOrderList.DataSource = mOrderList
	'    cmbOrderList.DataBind()
	'    upnlOrderDetails.Update()
	'    mVendor = Nothing
	'End Sub
	Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
		txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
		If cmbCurrencyList.Enabled = True Then
			setFocus(cmbCurrencyList)
		End If
	End Sub
	Private Sub optExistingOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optExistingOrder.CheckedChanged
		ControlVisibilityExistingOrder()
	End Sub
	Private Sub optNewOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optNewOrder.CheckedChanged
		ControlVisibilityNewOrder()
	End Sub
	Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		SetVariables()
		If IsValid Then
			' If dgPendingReceiptItemList.Rows.Count = 0 Or dgPendingReceiptItemList.SelectedIndex = -1 Then
			SelectedItemName = Session("SelectedItemName")
			If dgPendingReceiptItemList.Rows.Count = 0 Or String.IsNullOrEmpty(SelectedItemName) = True Then 'Or Indx1 = -1 Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Please select Part from the above List.", False), True)
				Exit Sub
			End If
			Dim mOrder As Order
			mItemList = Session("mItemList")
			If optNewOrder.Checked = True Then
				Dim tmpCurrencyID As Guid = mCurrencyList.Item(cmbCurrencyList.SelectedIndex).ID
				Dim tmpVendorID As Guid = mVendorList.Item(cmbVendor.SelectedIndex).ID
				Dim tmpNotInUse As Boolean = mVendorList.Item(tmpVendorID).NotInUse

				mOrder = Order.NewOrder
				If (txtOrderDate.Text = "") Or Not IsDate(txtOrderDate.Text) Then
					mOrder.OrderDate = Today.Date
				Else
					mOrder.OrderDate = txtOrderDate.Text
				End If
				Dim mtmpItem As Item = Item.GetItem(mItemList(Indx1).ID)

				'Added by Saylee on 1-Aug-2012
				If tmpNotInUse = True Then
					If CDate(mVendorList.Item(tmpVendorID).NotInUseDate) <= CDate(mOrder.OrderDate) Then
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Supplier is not applicable since " + mVendorList(tmpVendorID).NotInUseDateFormatted + "\n" + "Select another Supplier from list or select date before " + mVendorList(tmpVendorID).NotInUseDateFormatted + " & try again", False), True)
						mOrder = Nothing
						Exit Sub
					End If
				ElseIf mtmpItem.NotInUse = True Then
					If CDate(mtmpItem.NotInUseDate) <= CDate(mOrder.OrderDate) Then
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Part is not applicable since " + mtmpItem.NotInUseDateFormatted + " <br><br> Select another Part from list & try again", False), True)
						Exit Sub
					End If
				End If
				'Added by Utkarsh on 17-Dec-2013 for Trans Text Series
				If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Session("TransText_ForTransSeries") IsNot Nothing) Then
					If mOrder.IsNew Then
						mOrder.Text = Session("TransText_ForTransSeries")
						Session("AddTransTextSeries") = "False"
						Session.Remove("TransName_ForTransSeries")
						Session.Remove("TransText_ForTransSeries")
						Session.Remove("TransNo_ForTransSeries")
					End If
				End If
				'End

				mOrder.IntOrderNo = txtInternelOrdNo.Text
				mOrder.VendorID = tmpVendorID
				mOrder.CurrencyID = tmpCurrencyID
				If txtConversionFactor.Text <> "" Then
					mOrder.ConversionFactor = CDec(txtConversionFactor.Text)
				End If
				mOrder.AuthorizedBy = User.Identity.Name
				mOrder.UserName = User.Identity.Name

				mOrder.OrderItems.Add(mOrder.ID)
				If dgPendingReceiptItemList.SelectedIndex = -1 Then dgPendingReceiptItemList.SelectedIndex = 0
				With mOrder.OrderItems.CurrentItem
					.ItemID = mItemList(Indx1).ID
					.Qty = CDec(txtQty.Text)
					.CRate = 0
					.ModelID = Guid.Empty
					.Remark = "Order generated by automatic process through Receipt on " + New SmartDate(Today.Date.ToString).FormattedText
					.Note = ""
					.UnitID = mItemList(Indx1).UnitID 'Added By Prashant 5-Feb-2019 ALL04022019
				End With
				Dim a As Decimal = mOrder.CGrandTotal
				mOrder.StatusID = 2 'Authorized
				'Added by Utkarsh ON 17-Dec-2013 FOr TransTextSeries
				'Check if text is blank then call TransTextSeries UI

				If (mOrder.IsNew) And (mOrder.Text = "") Then

					Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mOrder.TransTypeID, mOrder.OrderDateFormatted)

					If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mOrder.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mOrder.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mOrder.TransTypeID).TransText = "")) Then

						Dim str = "<script language='javascript'>openledgersame('wfAutocOrderCreation_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&mType=" & Request.QueryString("mType") & "');</script>"
						Session("BackPagestr_ForTransSeries") = str

						Session("TransName_ForTransSeries") = "Purchase Order"
						Session("TransTypeID_ForTransSeries") = mOrder.TransTypeID
						Session("TransDate_ForTransSeries") = mOrder.OrderDateFormatted

						MSGBoxCtrl.show("Purchase Order Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "OrderTransTextSeriesAlert")
						Exit Sub
					Else
						Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

						If mAutoRenewTransTextSeries.IsRenewed Then
							With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mOrder.TransTypeID)
								mOrder.Text = .TransText
								mOrder.No = .StartingTransNo
							End With
						Else
							Dim str = "<script language='javascript'>openledgersame('wfAutocOrderCreation_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&mType=" & Request.QueryString("mType") & "');</script>"
							Session("BackPagestr_ForTransSeries") = str

							Session("TransName_ForTransSeries") = "Purchase Order"
							Session("TransTypeID_ForTransSeries") = mOrder.TransTypeID
							Session("TransDate_ForTransSeries") = mOrder.OrderDateFormatted

							MSGBoxCtrl.show("Purchase Order Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "OrderTransTextSeriesAlert")
							Exit Sub
						End If
					End If

				End If

				'End
				mOrder = CType(mOrder.Save(), Order)
				txtQty.Text = ""
				cmbVendor.SelectedIndex = 0
				cmbCurrencyList.SelectedIndex = 0
				txtConversionFactor.Text = ""
				txtInternelOrdNo.Text = ""
				lblCreate.Text = " Create / Edit order for the above Part : "
				upnlPendingReceiptItemList.Update()
				OrderDate = ""
				Qty = 0
				Supplier = 0
				Currency = 0
				Factor = 0
				IntOrderNo = ""
				OrderNo = 0

				SetSession()
				MSGBoxCtrl.show(MSGBox.Message_title.OrderCreate, MSGBox.Message_text.OrderCreate, "", MsgBoxStyle.OkOnly, "")
				Session.Remove("SelectedItemName")
				upnlOrderDetails.Update()
			ElseIf optExistingOrder.Checked = True Then
				'If cmbOrderList.SelectedIndex < 0 Or IsNothing(cmbOrderList.SelectedItem) Then
				If Len(txtOrderList.Text.Trim) = 0 Or txtOrderList.Text.Trim = "" Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "<Strong>Please select Order from the List.</Strong>", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'  mOrder = Order.GetOrder(New Guid(cmbOrderList.SelectedValue))
				mOrder = Order.GetOrder(New Guid(hdnOrderId.Value.ToString))
				'Added by Saylee on 1-Aug-2012
				Dim mtmpItem As Item = Item.GetItem(mItemList(Indx1).ID)
				If mtmpItem.NotInUse = True Then
					If CDate(mtmpItem.NotInUseDate) <= CDate(mOrder.OrderDate) Then
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Part is not applicable since " + mtmpItem.NotInUseDateFormatted + " <br><br> Select another Part from list & try again", False), True)
						Exit Sub
					End If
				End If
				'*********************
				If mOrder IsNot Nothing Then
					'Identifing is Item Exists ?
					If dgPendingReceiptItemList.SelectedIndex = -1 Then dgPendingReceiptItemList.SelectedIndex = 0
					If mOrder.OrderItems.Contains(mItemList(Indx1).ID) Then
						mOrder.OrderItems.CurrentIndex = mOrder.OrderItems.ItemIndex(mItemList(Indx1).ID)
						With mOrder.OrderItems.CurrentItem
							.Remark = txtQty.Text + " Qty added to existing Order item by automatic process through Receipt on " + New SmartDate(Today.Date.ToString).FormattedText
							.Qty = .Qty + CDec(txtQty.Text) '(Qty + Qty) 
						End With
					Else
						mOrder.OrderItems.Add(mOrder.ID)
						With mOrder.OrderItems.CurrentItem
							.ItemID = mItemList(Indx1).ID
							.Qty = CDec(txtQty.Text)
							.CRate = 0
							.ModelID = Guid.Empty
							.Remark = "Order item added by automatic process through Receipt on " + New SmartDate(Today.Date.ToString).FormattedText
							.Note = ""
							.UnitID = mItemList(Indx1).UnitID 'Added By Prashant 5-Feb-2019 ALL04022019
						End With
					End If
					Dim a As Decimal = mOrder.CGrandTotal
					mOrder = CType(mOrder.Save(), Order)
					txtQty.Text = ""
					txtInternelOrdNo.Text = ""
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.OrderAdd, MSGBox.Message_text.OrderAdd, "", MsgBoxStyle.OkOnly, "")
					mOrderList = OrderList.GetOrderList(, , , , txtInternelOrdNo.Text, "1-1-1850", "1-1-2200", , , "")
					cmbOrderList.DataSource = mOrderList
					cmbOrderList.DataBind()
					lblCreate.Text = " Create / Edit order for the above Part : "
					upnlPendingReceiptItemList.Update()
					RemoveSessions()
					upnlOrderDetails.Update()
				End If
			End If
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub dgPendingReceiptItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingReceiptItemList.Sorting
		mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mItemList") = mItemList
		dgPendingReceiptItemList.DataSource = mItemList
		dgPendingReceiptItemList.DataBind()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub
#End Region

End Class