'*********************************
'Style Conversion By: Sachin & Saylee dated: 1-Oct-2024
'*********************************

Public Class wfSalesOrder_Ajax
	Inherits Page

#Region " Variable Declaration "

	Public mSalesOrder As SalesOrder
	Public mVendorList As VendorList
	Public mStatusList As StatusList
	Public mCurrencyList As CurrencyList
	Public Flag As Integer
	Dim mVendorTerms As VendorTerms          'Added By Prashant 26-Apr-2010
	Dim EventLogID As Guid                  'Added by Vikrant on 21-July-2011
	Public mGSTPercentage As GSTPercentage
	Public mVendor As Vendor

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mSalesOrder = Session("mSalesOrder")
		mVendorList = Session("mVendorList")
		mStatusList = Session("mStatusList")
		mCurrencyList = Session("mCurrencyList")
		mVendorTerms = Session("mVendorTerms")
	End Sub

	Private Sub SetSession()
		Session("mSalesOrder") = mSalesOrder
		Session("mVendorList") = mVendorList
		Session("mStatusList") = mStatusList
		Session("mCurrencyList") = mCurrencyList
		Session("mVendorTerms") = mVendorTerms
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mVendorList")
		Session.Remove("mStatusList")
		Session.Remove("mCurrencyList")
		Session.Remove("mVendorTerms")
	End Sub

	Private Sub SetObject()

		mSalesOrder.Date = txtSalesOrderDate.Text
		mSalesOrder.Text = txtText.Text
		mSalesOrder.No = Val(txtNo.Text)
		mSalesOrder.CustomerReferenceNo = txtCustRefNo.Text
		mSalesOrder.UserName = User.Identity.Name
		mSalesOrder.IsRoundOff = chkIsRoundOff.Checked

		Dim txtValue As TextBox
		Dim mSalesOrderItem As SalesOrderItem
		Dim i As Integer = 0

		For Each mSalesOrderItem In mSalesOrder.SalesOrderItems

			mSalesOrderItem.ConversionFactor = Val(txtConversionFactor.Text)

			With mSalesOrderItem

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtQty"), TextBox)
				.Qty = CDec(Val(txtValue.Text))

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtRate"), TextBox)
				.CRate = CDec(Val(txtValue.Text))

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtOtherCharge"), TextBox)
				.COtherCharges = CDec(Val(txtValue.Text))

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtNote"), TextBox)
				.Note = txtValue.Text

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtRemark"), TextBox)
				.Remark = txtValue.Text

				'------------------------------------------------------------------
				If AppSettings("IsGSTApplicable") = "True" Then
					mVendor = Vendor.GetVendor(mSalesOrder.VendorID)

					If mVendor.ClientCountryName.ToUpper = "INDIA" Then

						If mVendor.CountryName.ToUpper = "INDIA" And mSalesOrder.Date >= CDate("01-Jul-2017") Then

							Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)

							If Len(mVendor.StateCode) > 0 Then

								If mVendor.StateCode = mVendor.ClientStateCode Then

									txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
									.CGSTPercentage = CDec(Val(txtValue.Text))
									txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
									.SGSTPercentage = Val(txtValue.Text.Trim)
									.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
									.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
									.HSNACSCode = mtmpItem.HSNACSCode
									.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
									.IGSTPercentage = 0
									.IGSTCAmount = 0
									mSalesOrder.StateCode = mVendor.StateCode
									mSalesOrder.ClientStateCode = mVendor.ClientStateCode
									mSalesOrder.VendorCountry = mVendor.CountryName
									mSalesOrder.Visibility = 1

								Else

									txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWIGST"), TextBox)
									.IGSTPercentage = CDec(Val(txtValue.Text))
									.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
									.CGSTPercentage = 0
									.SGSTPercentage = 0
									.CGSTCAmount = 0
									.SGSTCAmount = 0
									.HSNACSCode = mtmpItem.HSNACSCode
									.TotalCAmount = .CAmount + .IGSTCAmount
									mSalesOrder.StateCode = mVendor.StateCode
									mSalesOrder.ClientStateCode = mVendor.ClientStateCode
									mSalesOrder.VendorCountry = mVendor.CountryName
									mSalesOrder.Visibility = 2

								End If

							Else

								.CGSTPercentage = 0
								.SGSTPercentage = 0
								.CGSTCAmount = 0
								.SGSTCAmount = 0
								.IGSTPercentage = 0
								.IGSTCAmount = 0
								.HSNACSCode = ""
								mSalesOrder.StateCode = mVendor.StateCode
								mSalesOrder.ClientStateCode = mVendor.ClientStateCode
								mSalesOrder.VendorCountry = mVendor.CountryName
								mSalesOrder.Visibility = 3

							End If

						Else

							.CGSTPercentage = 0
							.SGSTPercentage = 0
							.CGSTCAmount = 0
							.SGSTCAmount = 0
							.IGSTPercentage = 0
							.IGSTCAmount = 0
							.HSNACSCode = ""
							mSalesOrder.StateCode = mVendor.StateCode
							mSalesOrder.ClientStateCode = mVendor.ClientStateCode
							mSalesOrder.VendorCountry = mVendor.CountryName
							mSalesOrder.Visibility = 3

						End If

					Else

						.CGSTPercentage = 0
						.SGSTPercentage = 0
						.CGSTCAmount = 0
						.SGSTCAmount = 0
						.IGSTPercentage = 0
						.IGSTCAmount = 0
						.HSNACSCode = ""
						mSalesOrder.StateCode = mVendor.StateCode
						mSalesOrder.ClientStateCode = mVendor.ClientStateCode
						mSalesOrder.VendorCountry = mVendor.CountryName
						mSalesOrder.Visibility = 3
					End If

				Else
					mSalesOrder.Visibility = 3
				End If

			End With

			i = i + 1

		Next

		mSalesOrder.CalculateTotal()

	End Sub

	Private Sub SetVendorDetails()

		mSalesOrder.VendorID = New Guid(cmbVendorList.SelectedValue)
		mSalesOrder.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
		mSalesOrder.ConversionFactor = Val(txtConversionFactor.Text)

	End Sub

	Private Sub DeleteRecord(Index As Int32)
		'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.RemoveItem, SIMsgBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo)

		MSGBoxCtrl.Show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteItem")
		mSalesOrder.SalesOrderItems.CurrentIndex = Index
		Session("mSalesOrder") = mSalesOrder

	End Sub

	Private Sub DeleteChargeRecord(Index As Int32)
		MSGBoxCtrl.Show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteCharge")
		mSalesOrder.SalesOrderCharges.CurrentIndex = Index
		Session("mSalesOrder") = mSalesOrder
	End Sub

	Private Overloads Sub SetFocus(control As WebControl)
		If control.Enabled = False Or control.Visible = False Then Exit Sub
		Dim str As String
		str = "try{document.getElementById('" + control.ClientID + "').focus();}catch (Error) {}"
		ScriptManager.RegisterStartupScript(Me, [GetType], "focusscript", str, True)
	End Sub

	Private Sub MessageBoxResult()

		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then

			Select Case Result1
				Case MsgBoxResult.Yes

					If MSGBoxCtrl.Sender = "DeleteItem" Then

						Try

							Session("Sender") = ""
							Dim mSalesOrder As SalesOrder
							mSalesOrder = CType(Session("mSalesOrder"), SalesOrder)
							mSalesOrder.SalesOrderItems.Remove(mSalesOrder.SalesOrderItems.CurrentItem)
							mSalesOrder.CalculateTotal()

							If mSalesOrder.IsRoundOff = True Then   'ALL25102012
								mSalesOrder.RoundCGrandTotal()
							End If

							Session("mSalesOrder") = mSalesOrder
							dgSalesOrderItems.DataSource = mSalesOrder.SalesOrderItems
							dgSalesOrderItems.DataBind()
							upnlSalesOrderItems.Update()

							upnlGrandTotal.Update()
							upnlGrandTotal.DataBind()

						Catch ex As SqlException

							If ex.Number = 8114 Or ex.Number = 8115 Then

								MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow,
												MSGBox.Message_text.NumericOverFlow,
												" Rate or Qty or Conversion Factor. ",
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							ElseIf ex.Number = 8145 Then

								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.ProcedureError,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							ElseIf ex.Number = 2627 Then

								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.Duplicate,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							End If

						End Try

					ElseIf MSGBoxCtrl.Sender = "DeleteCharge" Then

						Try

							Session("Sender") = ""
							Dim mSalesOrder As SalesOrder
							mSalesOrder = CType(Session("mSalesOrder"), SalesOrder)
							mSalesOrder.SalesOrderCharges.Remove(mSalesOrder.SalesOrderCharges.CurrentItem)
							mSalesOrder.CalculateTotal()            'Added By Saylee on 10-Sep-2007
							If mSalesOrder.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012 ALL25102012
								mSalesOrder.RoundCGrandTotal()
							End If

							Session("mSalesOrder") = mSalesOrder
							dgChargeList.DataSource = mSalesOrder.SalesOrderCharges
							dgChargeList.DataBind()
							upnlOrderCharge.Update()
							upnlSalesOrderItems.Update()
							upnlGrandTotal.Update()
							upnlGrandTotal.DataBind()

						Catch ex As SqlException

							If ex.Number = 8114 Or ex.Number = 8115 Then

								MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow,
												MSGBox.Message_text.NumericOverFlow,
												" Rate or Qty or Conversion Factor. ",
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							ElseIf ex.Number = 8145 Then

								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.ProcedureError,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							ElseIf ex.Number = 2627 Then

								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.Duplicate,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							End If

						End Try

					ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation

						'Added Code
						Session("sender") = ""

						If Session("IsValid") Then

							Session.Remove("IsValid")
							DataFieldBind()
							If (Not User.IsInRole("SalesOrderNew") And Not User.IsInRole("SalesOrderEdit")) Then

								MSGBoxCtrl.Show("Alert..!!", "You are not authorized user ", "", MsgBoxStyle.OkOnly, "")
								Exit Sub

							End If

							Save()

						Else
							Session.Remove("IsValid")
						End If

					ElseIf MSGBoxCtrl.Sender = "Status" Then

						Session("sender") = ""

						If Not CustomValidateObject() Then

							upnlValidationSAummary.Update()
							Exit Sub

						End If

						If mSalesOrder.IsValid Then

							DataFieldBind()
							Save()

						Else
							Session.Remove("IsValid")
						End If

					End If

				Case MsgBoxResult.No

					If MSGBoxCtrl.Sender = "Close" Then

						Session.Remove("IsValid")
						Session("Sender") = ""
						Response.Redirect("Index.aspx")

					ElseIf MSGBoxCtrl.Sender = "Status" Then

						If Not CustomValidateObject() Then

							upnlValidationSAummary.Update()
							Exit Sub

						End If

						Session("Sender") = ""

						If mSalesOrder.StatusID = 2 Then
							mSalesOrder.StatusID = 1
						ElseIf mSalesOrder.StatusID = 4 Then
							mSalesOrder.StatusID = 2
						End If

						Session("mSalesOrder") = mSalesOrder

					Else
						Session("Sender") = ""
					End If

				Case MsgBoxResult.Ok

					If MSGBoxCtrl.Sender = "Status" Then

						Session("sender") = ""

						If mSalesOrder.StatusID = 2 Then
							mSalesOrder.StatusID = 1
						ElseIf mSalesOrder.StatusID = 4 Then
							mSalesOrder.StatusID = 2
						End If

						Session("mSalesOrder") = mSalesOrder
						DataFieldBind()

						'Added by Utkarsh On 20-Nov-2013 For TransTextSeries
					ElseIf MSGBoxCtrl.Sender = "SalesOrderTransTextSeriesAlert" Then

						Session("sender") = ""
						Session("AddTransTextSeries") = "True"
						Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
						'ENd
					Else

						Session("sender") = ""
						DataFieldBind()

					End If

			End Select

		ElseIf Result1 = -1 Then

			If mSalesOrder.StatusID = 2 And MSGBoxCtrl.Sender <> "Close" Then
				mSalesOrder.StatusID = 1
			ElseIf mSalesOrder.StatusID = 4 Then
				mSalesOrder.StatusID = 2
			End If

			Session("mSalesOrder") = mSalesOrder
			Session("sender") = ""

		ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then   'Code Added

			Session("sender") = ""
			DataFieldBind()

		End If

	End Sub

	Private Sub AddAttributes()
		txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub SetControlStatus(StatusId As Int16)

		btnAdd.Enabled = IIf(StatusId > 1, False, True)
		cmbAdd.Enabled = IIf(StatusId > 1, False, True)
		btnAddTerm.Enabled = IIf(StatusId > 1, False, True)
		btnAddCustomerSpecificTerms.Enabled = IIf(StatusId > 1, False, True)
		btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
		btnSave.Visible = IIf(StatusId > 1, False, True)
		dgSalesOrderTerms.Columns(2).Visible = IIf(StatusId > 1, False, True)

	End Sub

	Private Sub SetPage()
		If mSalesOrder.No > 0 Then
			lblTitle.Text = "Sales Order [" & mSalesOrder.Text + "-" + CType(mSalesOrder.No, String) + "]"
		End If
	End Sub

	Private Sub ControlVisibility()
		txtText.Enabled = IIf(mSalesOrder.StatusID >= 2, False, True)
		txtNo.Enabled = IIf(mSalesOrder.StatusID >= 2, False, True)
		cmbVendorList.Enabled = (CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean) And mSalesOrder.SalesOrderItems.Count = 0) Or (mSalesOrder.SalesOrderItems.Count = 0)
		txtSalesOrderDate.Enabled = (CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean) And mSalesOrder.SalesOrderItems.Count = 0) Or (mSalesOrder.SalesOrderItems.Count = 0)
		cmbCurrencyList.Enabled = (CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean))
		txtConversionFactor.Enabled = (CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean))
		txtCustRefNo.Enabled = (CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean))
		'Authorized Status
		btnAuthorized.Visible = (Not mSalesOrder.SalesOrderItems.Count = 0) And (Not mSalesOrder.IsNew) And (mSalesOrder.StatusID = 1)
		'Canceled Status
		btnCancel.Visible = (Not mSalesOrder.IsNew) And (mSalesOrder.StatusID = 2)
		chkIsRoundOff.Enabled = (mSalesOrder.StatusID = 1)
		Dim txtValue As TextBox
		For i As Integer = 0 To dgSalesOrderItems.Rows.Count - 1
			txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtQty"), TextBox)
			txtValue.Enabled = CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean)
			txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtRate"), TextBox)
			txtValue.Enabled = CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean)

			txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtOtherCharge"), TextBox)
			txtValue.Enabled = CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean)
			txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtNote"), TextBox)
			txtValue.Enabled = CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean)

			txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtRemark"), TextBox)
			txtValue.Enabled = CType(IIf(mSalesOrder.StatusID >= 2, False, True), Boolean)
		Next

		'Added By Prashant 17-Aug-2011
		If Not User.IsInRole("SalesOrderAuthorized") Then
			btnAuthorized.Enabled = False
			btnAuthorized.ToolTip = "You are not authorized user "
			btnCancel.Enabled = False
			btnCancel.ToolTip = "You are not authorized user "
		End If

		'Added By Vikrant on 29-Jan-2018 For Deccan29012018-1
		If mSalesOrder.Visibility = 1 Or mSalesOrder.Visibility = 2 Then
			Dim txtCGSTPercentage As TextBox
			Dim txtSGSTPercentage As TextBox
			Dim txtIGSTPercentage As TextBox

			For i As Integer = 0 To dgSalesOrderItems.Rows.Count - 1
				txtCGSTPercentage = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
				txtSGSTPercentage = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWSGST"), TextBox)
				txtIGSTPercentage = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWIGST"), TextBox)

				txtCGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesOrder.StatusID >= 2 Or mSalesOrder.SalesOrderItems(i).HSNACSCode = "", True, False) 'CGSTPercentage 
				'txtSGSTPercentage.Enabled = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesOrder.StatusID = 2 Or mSalesOrder.StatusID = 4, True Or dgSalesOrderItems.Items(i).Cells(10).Text = "", False ) 'SGSTPercentage 
				txtIGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesOrder.StatusID >= 2, True Or mSalesOrder.SalesOrderItems(i).HSNACSCode = "", False) 'IGSTPercentage 

				txtCGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesOrder.StatusID >= 2 Or mSalesOrder.SalesOrderItems(i).HSNACSCode = "", Color.Gainsboro, Color.White) 'CGSTPercentage 
				'txtSGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesOrder.StatusID = 2 Or mSalesOrder.StatusID = 4 Or dgSalesOrderItems.Items(i).Cells(10).Text = "", Color.Gainsboro, Color.White ) 'SGSTPercentage 
				txtIGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesOrder.StatusID >= 2 Or mSalesOrder.SalesOrderItems(i).HSNACSCode = "", Color.Gainsboro, Color.White) 'IGSTPercentage 
			Next
		End If
		'End
		'---------------------------------------------------------------------------
		If mSalesOrder.Visibility = 1 Then
			dgSalesOrderItems.Columns(11).Visible = True 'CGSTPercentage 
			dgSalesOrderItems.Columns(12).Visible = True 'CGSTCAmount 
			dgSalesOrderItems.Columns(13).Visible = True 'SGSTPercentage 
			dgSalesOrderItems.Columns(14).Visible = True 'SGSTCAmount 
			dgSalesOrderItems.Columns(15).Visible = False 'IGSTPercentage 
			dgSalesOrderItems.Columns(16).Visible = False 'IGSTCAmount 

			lblTotalCGST.Visible = True
			txtTotalCGST.Visible = True
			lblTotalSGST.Visible = True
			txtTotalSGST.Visible = True
			lblTotalIGST.Visible = False
			txtTotalIGST.Visible = False
		ElseIf mSalesOrder.Visibility = 2 Then
			dgSalesOrderItems.Columns(11).Visible = False 'CGSTPercentage 
			dgSalesOrderItems.Columns(12).Visible = False 'CGSTCAmount 
			dgSalesOrderItems.Columns(13).Visible = False 'SGSTPercentage 
			dgSalesOrderItems.Columns(14).Visible = False 'SGSTCAmount 
			dgSalesOrderItems.Columns(15).Visible = True  'IGSTPercentage 
			dgSalesOrderItems.Columns(16).Visible = True 'IGSTCAmount 

			lblTotalCGST.Visible = False
			txtTotalCGST.Visible = False
			lblTotalSGST.Visible = False
			txtTotalSGST.Visible = False
			lblTotalIGST.Visible = True
			txtTotalIGST.Visible = True
		ElseIf mSalesOrder.Visibility = 3 Then
			dgSalesOrderItems.Columns(10).Visible = False 'HSNACSCode 
			dgSalesOrderItems.Columns(11).Visible = False 'CGSTPercentage 
			dgSalesOrderItems.Columns(12).Visible = False 'CGSTCAmount 
			dgSalesOrderItems.Columns(13).Visible = False 'SGSTPercentage 
			dgSalesOrderItems.Columns(14).Visible = False 'SGSTCAmount 
			dgSalesOrderItems.Columns(15).Visible = False  'IGSTPercentage 
			dgSalesOrderItems.Columns(16).Visible = False 'IGSTCAmount 

			lblTotalCGST.Visible = False
			txtTotalCGST.Visible = False
			lblTotalSGST.Visible = False
			txtTotalSGST.Visible = False
			lblTotalIGST.Visible = False
			txtTotalIGST.Visible = False
		End If
		'---------------------------------------------------------------------------

	End Sub

	Private Sub Enable()
		txtSalesOrderDate.Enabled = True
		txtText.Enabled = True
		txtNo.Enabled = True
		txtCustRefNo.Enabled = True
		cmbVendorList.Enabled = True
		txtAddress.Enabled = True
		cmbCurrencyList.Enabled = True
		txtConversionFactor.Enabled = True
	End Sub

	Private Sub Disable()
		txtSalesOrderDate.Enabled = False
		txtText.Enabled = False
		txtNo.Enabled = False
		txtCustRefNo.Enabled = False
		cmbVendorList.Enabled = False
		txtAddress.Enabled = False
		cmbCurrencyList.Enabled = False
		txtConversionFactor.Enabled = False
	End Sub

	Private Sub Save()

		'Authentication
		If mSalesOrder.Date IsNot DBNull.Value Then

			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

			If mCheck.WebAuthentication = True Then

				Dim mDays As Integer = 0
				mDays = mCheck.Number("Days")

				Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
				'---------------------------------

				If DateDiff(DateInterval.Day, CDate(mSalesOrder.Date), maxAllowableDate) < 0 Then

					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert,
									MSGBox.Message_text.saveAlert,
									" Your subscription has been expired. 
                                      Cannot save Sales Order. 
                                      <BR> Sales Order Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat),
									MsgBoxStyle.OkOnly,
									"")

					DataFieldBind()

					Exit Sub

				End If

			End If

		End If

		'Authentication
		Dim SalesOrderClone As SalesOrder
		SalesOrderClone = mSalesOrder.Clone

		Try

			If Not mSalesOrder.SalesOrderItems.Count = 0 Then

				SetObject()
				SetVendorDetails()
				Session("mSalesOrder") = mSalesOrder
				Dim mSalesOrderCharge As SalesOrderCharge

				For Each mSalesOrderCharge In mSalesOrder.SalesOrderCharges

					If (mSalesOrderCharge.Sign <> 1 And mSalesOrderCharge.CChargeAmount <= 0) Or
					   (Not (mSalesOrderCharge.IsValid)) Then

						MSGBoxCtrl.Show(MSGBox.Message_title.ValidationAlert,
										MSGBox.Message_text.ValidationAlert,
										"Percentage Sales Order Charge(s) are not allowed if Sales Order Amount Is Zero. ",
										MsgBoxStyle.OkOnly,
										"")

						mSalesOrder.CancelEdit()

						Exit Sub

					End If

				Next

				If mSalesOrder.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012 ALL25102012
					mSalesOrder.RoundCGrandTotal()
				End If

				'Added by Utkarsh on 20-Nov-2013 FOr TransTextSeries 
				'Check if Sales Order is blank then call TransTextSeries UI

				If (mSalesOrder.IsNew) And (mSalesOrder.Text = "") Then

					Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(Trans.SalesOrder,
																												 mSalesOrder.DateFormatted)

					If (mPreviousTransTextSeries.IsAutoRenew = False) Or
					   ((mPreviousTransTextSeries.IsAutoRenew = True) And
						(mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.SalesOrder) = False) Or
						(mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.SalesOrder) = True AndAlso
						mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.SalesOrder).TransText = "")) Then

						Dim str = "<script language='javascript'>openledgersame('wfSalesOrder_Ajax.aspx?BackPage=index.aspx');</script>"

						Session("BackPagestr_ForTransSeries") = str

						Session("TransName_ForTransSeries") = "Sales Order"
						Session("TransTypeID_ForTransSeries") = Trans.SalesOrder
						Session("TransDate_ForTransSeries") = mSalesOrder.DateFormatted

						MSGBoxCtrl.Show("Sales Order Transaction Series!!",
										"System does not find transaction series for this transaction. 
                                         Click Ok to enter transaction series.",
										"",
										MsgBoxStyle.OkOnly,
										"SalesOrderTransTextSeriesAlert")

						Exit Sub

					Else

						Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

						If mAutoRenewTransTextSeries.IsRenewed Then

							With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.SalesOrder)

								mSalesOrder.Text = .TransText
								mSalesOrder.No = .StartingTransNo

							End With

						Else

							Dim str = "<script language='javascript'>openledgersame('wfSalesOrder_Ajax.aspx?BackPage=index.aspx');</script>"

							Session("BackPagestr_ForTransSeries") = str
							Session("TransName_ForTransSeries") = "Sales Order"
							Session("TransTypeID_ForTransSeries") = Trans.SalesOrder
							Session("TransDate_ForTransSeries") = mSalesOrder.DateFormatted

							MSGBoxCtrl.Show("Sales Order Transaction Series!!",
											"System did not found transaction series for this transaction. 
                                             Click Ok to enter transaction series.",
											"",
											MsgBoxStyle.OkOnly,
											"SalesOrderTransTextSeriesAlert")

							Exit Sub

						End If

					End If

				End If

				mSalesOrder.Save()

				'Added by Vikrant on 21-July-2011
				Dim mOrderDetail As String = mSalesOrder.SalesOrderNo +
											 " Dated : " + mSalesOrder.DateFormatted +
											 " to " + mVendorList(mSalesOrder.VendorID).Name

				If mSalesOrder.StatusID = 2 Then

					MarkLog(Action.Authorize,
							"Sales Order",
							mOrderDetail,
							ErrorType.NoError,
							mSalesOrder.ID,
							EventLogID)

				ElseIf mSalesOrder.StatusID = 3 Then

					MarkLog(Action.Amend, "Sales Order",
							mOrderDetail,
							ErrorType.NoError,
							mSalesOrder.ID,
							EventLogID)

				ElseIf mSalesOrder.StatusID = 4 Then

					MarkLog(Action.Cancel,
							"Sales Order",
							mOrderDetail,
							ErrorType.NoError,
							mSalesOrder.ID,
							EventLogID)

				Else

					MarkLog(Action.Save,
							"Sales Order",
							mOrderDetail,
							ErrorType.NoError,
							mSalesOrder.ID,
							EventLogID)

				End If

				mSalesOrder.MarkClean()
				SetPage()
				UpdatePanel()
				SetChargeGrid()
				SetOrderItemGrid()

				lblTitle.Text = "Sales Order ( Saved ...)"
				Session("mSalesOrder") = mSalesOrder

				MSGBoxCtrl.Show(MSGBox.Message_title.SavedSuccessFully,
								MSGBox.Message_text.SavedSuccessFully,
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			Else

				MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert,
								MSGBox.Message_text.saveAlert,
								"Sales Order can not be saved without Item.",
								MsgBoxStyle.OkOnly,
								"")

				mSalesOrder = SalesOrderClone
				SetObject()
				SetVendorDetails()
				Session("mSalesOrder") = mSalesOrder
				DataFieldBind()

			End If

		Catch ex As SqlException

			Session("SalesOrderClone") = SalesOrderClone

			If ex.Number = 8114 Or ex.Number = 8115 Then

				MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow,
								MSGBox.Message_text.NumericOverFlow,
								" Rate or Qty or Conersion Factor. ",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			ElseIf ex.Number = 8145 Then

				MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
								MSGBox.Message_text.ProcedureError,
								ex.Procedure,
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			ElseIf ex.Number = 2627 Then

				MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
								MSGBox.Message_text.Duplicate,
								ex.Procedure,
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			ElseIf ex.Number = 547 Then

				If InStr(ex.Message, "FK_tabSalesOrderTerm_tabTerm", CompareMethod.Text) Then 'Added By Rajnish On 04-01-2008

					MSGBoxCtrl.Show("Term Deleted! ",
									"Term not available
                                     <BR><BR> Selected Term no longer exist in the Database 
                                     <BR><BR> Remove the Term and try Again.",
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				ElseIf InStr(ex.Message, "FKtabSalesOrderChargetabCharge", CompareMethod.Text) Then

					MSGBoxCtrl.Show("Sales Order Charge Deleted!",
									"Sales Order Charge not available
                                     <BR><BR> Selected Charge no longer exist in the Database 
                                     <BR><BR> Remove the Charge and try Again.",
									"",
									MsgBoxStyle.OkOnly, "")

					Exit Sub

				Else

					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete,
									MSGBox.Message_text.ReferenceDelete,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")
					Exit Sub

				End If

			End If

		Finally
			SalesOrderClone = Nothing
		End Try

	End Sub

	Private Sub AddMultipleParts()
		Dim mItem As Item
		Dim mItems As Items = Session("mItems")
		For Each mItem In mItems
			If mItem.IsSelected Then
				If Not mSalesOrder.SalesOrderItems.Contains(mItem.ID) Then
					mSalesOrder.SalesOrderItems.Add(mSalesOrder.ID)
					With mSalesOrder.SalesOrderItems.CurrentItem
						.ItemID = mItem.ID
						'------------------------------------------------------------------
						If AppSettings("IsGSTApplicable") = "True" Then
							mVendor = Vendor.GetVendor(mSalesOrder.VendorID)
							If mVendor.ClientCountryName.ToUpper = "INDIA" Then
								If mVendor.CountryName.ToUpper = "INDIA" And mSalesOrder.Date >= CDate("01-Jul-2017") Then
									mGSTPercentage = GSTPercentage.GetPercentage(mSalesOrder.Date, 1, .ItemID.ToString)
									If mGSTPercentage IsNot Nothing Then
										Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
										If Len(mVendor.StateCode) > 0 Then
											If mVendor.StateCode = mVendor.ClientStateCode Then
												.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
												.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
												.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
												.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)

												.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

												.IGSTPercentage = 0
												.IGSTCAmount = 0
												.HSNACSCode = mtmpItem.HSNACSCode
												mSalesOrder.StateCode = mVendor.StateCode
												mSalesOrder.ClientStateCode = mVendor.ClientStateCode
												mSalesOrder.VendorCountry = mVendor.CountryName
												mSalesOrder.Visibility = 1
											Else
												.IGSTPercentage = (mGSTPercentage.GSTPercentage)
												.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

												.CGSTPercentage = 0
												.SGSTPercentage = 0
												.CGSTCAmount = 0
												.SGSTCAmount = 0

												.TotalCAmount = .CAmount + .IGSTCAmount
												.HSNACSCode = mtmpItem.HSNACSCode
												mSalesOrder.StateCode = mVendor.StateCode
												mSalesOrder.ClientStateCode = mVendor.ClientStateCode
												mSalesOrder.VendorCountry = mVendor.CountryName
												mSalesOrder.Visibility = 2
											End If
										Else
											.HSNACSCode = ""
											mSalesOrder.StateCode = mVendor.StateCode
											mSalesOrder.ClientStateCode = mVendor.ClientStateCode
											mSalesOrder.VendorCountry = mVendor.CountryName
											mSalesOrder.Visibility = 3
										End If
									End If
								Else
									.HSNACSCode = ""
									mSalesOrder.StateCode = mVendor.StateCode
									mSalesOrder.ClientStateCode = mVendor.ClientStateCode
									mSalesOrder.VendorCountry = mVendor.CountryName
									mSalesOrder.Visibility = 3
								End If
							Else
								.HSNACSCode = ""
								mSalesOrder.StateCode = mVendor.StateCode
								mSalesOrder.ClientStateCode = mVendor.ClientStateCode
								mSalesOrder.VendorCountry = mVendor.CountryName
								mSalesOrder.Visibility = 3
							End If
						Else
							mSalesOrder.Visibility = 3
						End If
						'------------------------------------------------------------------
					End With
				Else
					'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Sales Order,Part already taken for Sales Order", MsgBoxStyle.OkOnly)
					'msg1.ReplacePage = "wfSalesOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
					'Session("sender") = "Close"
					'msg1.Show()
					MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Sales Order,Part already taken for Sales Order", MsgBoxStyle.OkOnly, "")
					DataFieldBind()
					Exit Sub
				End If
			End If
		Next
	End Sub

	Private Sub AddQuotationParts()
		Dim mPendingSalesQuotationItems As PendingSalesQuotationItems = Session("mPendingSalesQuotationItems")
		Dim PendingQuotationInfo As PendingSalesQuotationItem
		If mPendingSalesQuotationItems IsNot Nothing Then

			For Each PendingQuotationInfo In mPendingSalesQuotationItems
				If PendingQuotationInfo.IsSelected Then
					If Not mSalesOrder.SalesOrderItems.Contains(PendingQuotationInfo.ItemID) Then

						mSalesOrder.SalesOrderItems.Add(mSalesOrder.ID)
						With mSalesOrder.SalesOrderItems.CurrentItem

							mSalesOrder.SalesOrderItems.CurrentItem.ItemID = PendingQuotationInfo.ItemID
							mSalesOrder.SalesOrderItems.CurrentItem.QuotationID = PendingQuotationInfo.QuotationID  ' frm.FromItemID
							mSalesOrder.SalesOrderItems.CurrentItem.Qty = PendingQuotationInfo.QuotationQty  'frm.Qty
							mSalesOrder.SalesOrderItems.CurrentItem.QuotationItemID = PendingQuotationInfo.QuotationItemID  ' frm.FromItemID
							mSalesOrder.SalesOrderItems.CurrentItem.QuotationNo = PendingQuotationInfo.QuotationTextNo    'frm.FromNo
							mSalesOrder.SalesOrderItems.CurrentItem.QuotationDate = PendingQuotationInfo.QuotationDate   'frm.FromDate
							mSalesOrder.SalesOrderItems.CurrentItem.CRate = PendingQuotationInfo.CRate  'Add this line in ASP
							mSalesOrder.SalesOrderItems.CurrentItem.ConversionFactor = mSalesOrder.ConversionFactor

							'------------------------------------------------------------------
							If AppSettings("IsGSTApplicable") = "True" Then
								mVendor = Vendor.GetVendor(mSalesOrder.VendorID)
								If mVendor.ClientCountryName.ToUpper = "INDIA" Then
									If mVendor.CountryName.ToUpper = "INDIA" And mSalesOrder.Date >= CDate("01-Jul-2017") Then
										mGSTPercentage = GSTPercentage.GetPercentage(mSalesOrder.Date, 1, .ItemID.ToString)
										If mGSTPercentage IsNot Nothing Then
											Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
											If Len(mVendor.StateCode) > 0 Then
												If mVendor.StateCode = mVendor.ClientStateCode Then
													.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
													.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
													.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
													.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)

													.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

													.IGSTPercentage = 0
													.IGSTCAmount = 0
													.HSNACSCode = mtmpItem.HSNACSCode
													mSalesOrder.StateCode = mVendor.StateCode
													mSalesOrder.ClientStateCode = mVendor.ClientStateCode
													mSalesOrder.VendorCountry = mVendor.CountryName
													mSalesOrder.Visibility = 1
												Else
													.IGSTPercentage = (mGSTPercentage.GSTPercentage)
													.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

													.CGSTPercentage = 0
													.SGSTPercentage = 0
													.CGSTCAmount = 0
													.SGSTCAmount = 0

													.TotalCAmount = .CAmount + .IGSTCAmount
													.HSNACSCode = mtmpItem.HSNACSCode
													mSalesOrder.StateCode = mVendor.StateCode
													mSalesOrder.ClientStateCode = mVendor.ClientStateCode
													mSalesOrder.VendorCountry = mVendor.CountryName
													mSalesOrder.Visibility = 2
												End If

											Else
												.HSNACSCode = ""
												mSalesOrder.StateCode = mVendor.StateCode
												mSalesOrder.ClientStateCode = mVendor.ClientStateCode
												mSalesOrder.VendorCountry = mVendor.CountryName
												mSalesOrder.Visibility = 3
											End If
										End If
									Else
										.HSNACSCode = ""
										mSalesOrder.StateCode = mVendor.StateCode
										mSalesOrder.ClientStateCode = mVendor.ClientStateCode
										mSalesOrder.VendorCountry = mVendor.CountryName
										mSalesOrder.Visibility = 3
									End If
								Else
									.HSNACSCode = ""
									mSalesOrder.StateCode = mVendor.StateCode
									mSalesOrder.ClientStateCode = mVendor.ClientStateCode
									mSalesOrder.VendorCountry = mVendor.CountryName
									mSalesOrder.Visibility = 3
								End If
							Else
								mSalesOrder.Visibility = 3
							End If
							'------------------------------------------------------------------
						End With
					Else
						'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Sales Order,Part already taken for Sales Order", MsgBoxStyle.OkOnly)
						'msg1.ReplacePage = "wfSalesOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
						'Session("sender") = "Close"
						'msg1.Show()
						MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Sales Order,Part already taken for Sales Order", MsgBoxStyle.OkOnly, "")
						DataFieldBind()
						Exit Sub
					End If
				End If
			Next
		End If
	End Sub

	Private Sub SetChargeGrid()
		Try

			For j As Integer = 0 To dgChargeList.Rows.Count - 1

				If (Me.dgChargeList.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or
					Me.dgChargeList.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then

					dgChargeList.Rows.Item(j).Cells(4).Enabled = False

				End If

			Next

			For Each column As DataControlField In dgChargeList.Columns

				Select Case column.HeaderText
					Case "Action"
						column.Visible = IIf(mSalesOrder.StatusID > 1, False, True)
				End Select

			Next

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetOrderItemGrid()

		Try

			For Each column As DataControlField In dgSalesOrderItems.Columns

				Select Case column.HeaderText
					Case "Action"
						column.Visible = IIf(mSalesOrder.StatusID > 1, False, True)
				End Select

			Next

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub TextChanged(sender As Object, e As EventArgs)

		Dim txtValue As TextBox
		Dim mSalesOrderItem As SalesOrderItem
		Dim i As Integer = 0

		For Each mSalesOrderItem In mSalesOrder.SalesOrderItems

			With mSalesOrderItem

				Try

					txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
					txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
					txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWIGST"), TextBox)
					txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
					txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWSGST"), TextBox)
					txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				Catch ex As Exception
				End Try

			End With

			i = i + 1

		Next

	End Sub

	Private Sub UpdatePanel()

		ControlsDataBind()
		upnlStatusName.Update()
		upnlSalesOrderDetails.Update()
		upnlSupplierDetails.Update()
		upnlGrandTotal.Update()
		upnlButtons.Update()
		upnlSalesOrderTerms.Update()
		SetControlStatus(mSalesOrder.StatusID)
		ControlVisibility()

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		mCurrencyList = CurrencyList.GetCurrencyList(, , True)
		mVendorList = VendorList.GetVendortList(0, , , , , , True, True, False)
		mStatusList = StatusList.GetStatusList(mSalesOrder.StatusID, 1, True)
		cmbVendorList.DataSource = mVendorList
		cmbCurrencyList.DataSource = mCurrencyList
		Session("mCurrencyList") = mCurrencyList
		Session("mVendorList") = mVendorList
		Session("mStatusList") = mStatusList
		dgSalesOrderItems.DataSource = mSalesOrder.SalesOrderItems
		dgChargeList.DataSource = mSalesOrder.SalesOrderCharges
		dgSalesOrderTerms.DataSource = mSalesOrder.SalesOrderTerms
		txtSalesOrderDate.Text = mSalesOrder.DateFormatted

		DataBind()

	End Sub

	Private Sub ControlsDataBind()

		upnlStatusName.DataBind()
		upnlSalesOrderDetails.DataBind()
		upnlSupplierDetails.DataBind()
		upnlGrandTotal.DataBind()
		upnlButtons.DataBind()

	End Sub

	Private Sub SalesOrderChargesGrid()

		dgChargeList.DataSource = mSalesOrder.SalesOrderCharges
		dgChargeList.DataBind()
		upnlOrderCharge.Update()
		upnlGrandTotal.Update()
		upnlGrandTotal.DataBind()

	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		Dim custValidator As CustomValidator

		custValidator = CType(s, CustomValidator)

		If custValidator.ControlToValidate = "txtSalesOrderDate" Then

			If txtSalesOrderDate.Text.ToString = "" Then
				custValidator.ErrorMessage = "Select Sales Order Date."
				e.IsValid = False
			End If

		ElseIf custValidator.ControlToValidate = "cmbVendorList" Then

			If cmbVendorList.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Select Customer from the list."
				e.IsValid = False
			End If

		ElseIf custValidator.ControlToValidate = "cmbCurrencyList" Then

			If cmbCurrencyList.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Select Currency from the List."
				e.IsValid = False
			End If

		ElseIf custValidator.ControlToValidate = "txtConversionFactor" Then

			If Val(txtConversionFactor.Text) <= 0 Then
				custValidator.ErrorMessage = "Currency factor must be greater than zero."
				e.IsValid = False
			End If

		End If

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		GetSession()
		AddAttributes()
		SetControlStatus(mSalesOrder.StatusID)
		If CType(Session("AddParts"), String) = "True" Then
			'Add selected part(s) to Enquiry Items
			AddMultipleParts()
			Session("AddParts") = "False"
		Else
			Session("AddParts") = "False"
		End If
		If CType(Session("PendingQuotationItems"), String) = "True" Then
			AddQuotationParts()
			Session("PendingQuotationItems") = "False"
		Else
			Session("PendingQuotationItems") = "False"
		End If
		EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 21-July-2011
		TextChanged(sender, e)
		If Not IsPostBack And Session("sender") = "" Then
			If AppSettings("AutoCompleteTransText") <> "True" Then 'Added by VIkrant For ALL23052012
				If txtText.Enabled = True Then
					SetFocus(txtText)
				End If
			End If
			'Added by Utkarsh on 19-Nov-2013 for Trans Text Series
			If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Session("TransText_ForTransSeries") IsNot Nothing) Then
				If mSalesOrder.IsNew Then
					mSalesOrder.Text = Session("TransText_ForTransSeries")
					txtText.Text = mSalesOrder.Text
					Session("mSalesOrder") = mSalesOrder
					Session("AddTransTextSeries") = "False"
					Session.Remove("TransName_ForTransSeries")
					Session.Remove("TransText_ForTransSeries")
					Session.Remove("TransNo_ForTransSeries")
				End If
			End If
			'End
			DataFieldBind()
		End If
		SetPage()
		'MessageBoxResult()
		ControlVisibility()
		If chkIsRoundOff.Checked = True Then  'Added By Prashant on 21-May-2012
			SetChargeGrid()
		End If

		SetOrderItemGrid()

	End Sub

	Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAdd.Click

		If cmbAdd.SelectedIndex = 0 Then
			If IsValid Then
				SetObject()
				SetVendorDetails()
				mSalesOrder.SalesOrderItems.Add(mSalesOrder.ID)
				mSalesOrder.SalesOrderItems.CurrentItem.Currency = cmbCurrencyList.SelectedItem.Text
				mSalesOrder.SalesOrderItems.CurrentItem.ConversionFactor = txtConversionFactor.Text
				Session("mSalesOrder") = mSalesOrder
				Response.Redirect("wfSalesOrderItem.aspx?BackPage=wfSalesOrder_Ajax.aspx")
			End If
		End If

		If cmbAdd.SelectedIndex = 1 Then
			If IsValid Then
				SetVendorDetails()
				SetObject()
				SetSession()
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenWindow", "OpenPartsWindow('" + mSalesOrder.SalesOrderItems.Count.ToString + "', '" + mSalesOrder.DateFormatted.ToString + "');", True)

			End If
		End If
		If cmbAdd.SelectedIndex = 2 Then
			If IsValid Then
				SetObject()
				SetVendorDetails()
				SetSession()
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenWindow", "OpenQuotesWindow();", True)
			End If
		End If
	End Sub

	Private Sub HdnImgBtnCommonPartList_Click(sender As Object, e As EventArgs) Handles hdnimgBtnCommonPartList.Click, hdnimgBtnQuotationList.Click
		If CType(Session("PendingQuotationItems"), String) = "True" Then
			AddQuotationParts()
			Session("PendingQuotationItems") = "False"
		Else
			Session("PendingQuotationItems") = "False"
		End If

		DataFieldBind()
		mSalesOrder.CalculateTotal()

		upnlOrderCharge.Update()
		upnlGrandTotal.Update()
		upnlGrandTotal.DataBind()

		ControlVisibility()
		upnlSalesOrderItems.Update()
		upnlSupplierDetails.Update()
		upnlSalesOrderDetails.Update()
	End Sub

	Private Sub AddCharge(sender As Object, e As EventArgs) Handles btnAddCharge.Click
		If IsValid Then
			SetObject()
			SetVendorDetails()
			mSalesOrder.SalesOrderCharges.Add(mSalesOrder.ID)
			Session("mSalesOrder") = mSalesOrder
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenSalesChargeWindow", "OpenSalesChargeWindow();", True)
		End If
	End Sub

	Private Sub HdnBtnSalesCharge_Click(sender As Object, e As EventArgs) Handles hdnBtnSalesCharge.Click
		dgChargeList.DataSource = mSalesOrder.SalesOrderCharges
		dgChargeList.DataBind()
		mSalesOrder.CalculateTotal()
		SetChargeGrid()
		upnlOrderCharge.Update()
		upnlGrandTotal.Update()
		upnlGrandTotal.DataBind()
	End Sub

	Private Sub AddTerm(sender As Object, e As EventArgs) Handles btnAddTerm.Click

		If IsValid Then
			SetObject()
			SetVendorDetails()
			Session("mSalesOrder") = mSalesOrder
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenTermWindow", "OpenTermWindow()", True)
		End If
	End Sub

	Private Sub AddCustomerSpecificTerms(sender As Object, e As EventArgs) Handles btnAddCustomerSpecificTerms.Click

		mVendorTerms = VendorTerms.GetVendorTerms(New Guid(cmbVendorList.SelectedValue), 3, mSalesOrder.ID.ToString, 5)
		Dim i As Integer = 0
		While i < mVendorTerms.Count
			If mSalesOrder.SalesOrderTerms.Contains(mVendorTerms.Item(i).TermID) = False Then
				mSalesOrder.SalesOrderTerms.Add(mSalesOrder.ID)
				mSalesOrder.SalesOrderTerms.CurrentItem.Terms = mVendorTerms.Item(i).Terms
				mSalesOrder.SalesOrderTerms.CurrentItem.TermID = mVendorTerms.Item(i).TermID
			End If
			i = i + 1
		End While
		dgSalesOrderTerms.DataSource = mVendorTerms
		dgSalesOrderTerms.DataBind()
	End Sub

	Private Sub VendorList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbVendorList.SelectedIndexChanged
		txtAddress.Text = mVendorList(cmbVendorList.SelectedIndex).Address
		If cmbVendorList.Enabled = True Then
			SetFocus(cmbVendorList)
		End If
	End Sub

	Private Sub CurrencyList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
		txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
		If cmbCurrencyList.Enabled = True Then
			SetFocus(cmbCurrencyList)
		End If
	End Sub

	Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

		If (Not User.IsInRole("SalesOrderNew") And Not User.IsInRole("SalesOrderEdit")) Then

			MSGBoxCtrl.Show("Alert..!!",
							"You are not authorized user ",
							"",
							MsgBoxStyle.OkOnly,
							"")
			Exit Sub

		End If

		If Not CustomValidateObject() Then

			upnlValidationSAummary.Update()
			Exit Sub

		End If

		If IsValid Then
			Save()
		End If

	End Sub

	Private Sub ReturnBack(sender As Object, e As EventArgs) Handles btnBack.Click

		'Modified by Vikrant on 21-July-2011
		Dim mOrderDetail As String = mSalesOrder.SalesOrderNo + " Dated : " + mSalesOrder.DateFormatted + IIf(cmbVendorList.SelectedIndex > 0, " to " + cmbVendorList.SelectedItem.Text, "")
		MarkLog(Action.Close, "Sales Order", mOrderDetail, ErrorType.NoError, Guid.Empty, EventLogID)
		Session("IsValid") = IsValid
		If mSalesOrder.IsDirty Then

			MSGBoxCtrl.Show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "Sales Order,Part already taken for Sales Order", MsgBoxStyle.YesNo, "Close")
			If IsValid Then
				SetObject()
				SetVendorDetails()
			End If
		Else
			Response.Redirect("Index.aspx")
		End If
	End Sub

	Private Sub IsRoundOff_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsRoundOff.CheckedChanged
		Dim Child As SalesOrderCharge
		For i As Integer = mSalesOrder.SalesOrderCharges.Count - 1 To 0 Step -1
			Child = mSalesOrder.SalesOrderCharges(i)
			If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
				mSalesOrder.SalesOrderCharges.Remove(Child)
			End If
		Next
		dgChargeList.DataSource = mSalesOrder.SalesOrderCharges
		dgChargeList.DataBind()
	End Sub

	'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
	Private Sub SalesOrderDate_TextChanged(sender As Object, e As EventArgs) Handles txtSalesOrderDate.TextChanged
		mSalesOrder = Session("mSalesOrder")

		mSalesOrder.Date = txtSalesOrderDate.Text
		txtText.Text = mSalesOrder.Text

		Session("mSalesOrder") = mSalesOrder
	End Sub
	'End

	Private Sub GV_SalesOrderItems_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSalesOrderItems.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				Session("Edit") = True
				SetObject()
				SetVendorDetails()
				mSalesOrder.SalesOrderItems.CurrentIndex = Index
				Session("mSalesOrder") = mSalesOrder
				Response.Redirect("wfSalesOrderItem.aspx?BackPage=wfSalesOrder_Ajax.aspx")
			Case "DeleteRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				DeleteRecord(Index)
		End Select

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	Private Sub GV_SalesOrderTerms_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSalesOrderTerms.RowCommand
		Dim Index As Integer = CInt(e.CommandArgument)
		'Dim Index As Int32 = e.Item.ItemIndex + dgSalesOrderItems.PageIndex * dgSalesOrderItems.PageSize
		Select Case e.CommandName
			Case "DeleteRec"
				Index = CInt(e.CommandArgument)
				mSalesOrder.SalesOrderTerms.CurrentIndex = Index
				mSalesOrder.SalesOrderTerms.Remove(mSalesOrder.SalesOrderTerms.CurrentItem)
				Session("mSalesOrder") = mSalesOrder
				'Response.Redirect("wfSalesOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
				dgSalesOrderTerms.DataSource = mSalesOrder.SalesOrderTerms
				dgSalesOrderTerms.DataBind()
				upnlSalesOrderTerms.Update()
		End Select


	End Sub

	Private Sub GV_ChargeList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgChargeList.RowCommand
		Dim Index As Integer = CInt(e.CommandArgument)
		Select Case e.CommandName

			Case "EditRec"
				Index = CInt(e.CommandArgument)
				Session("EditCharge") = True
				SetObject()
				SetVendorDetails()
				mSalesOrder.SalesOrderCharges.CurrentIndex = Index
				Session("mSalesOrder") = mSalesOrder
				' Response.Redirect("wfSalesOrderCharge.aspx?BackPage=wfSalesOrder_Ajax.aspx")
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenSalesChargeWindow", "OpenSalesChargeWindow();", True)
			Case "DeleteRec"
				DeleteChargeRecord(Index)

		End Select
	End Sub

	Protected Sub AddAttributesForGridControls()
		Dim txtValue As TextBox
		Dim txtCGSTPer As TextBox
		Dim i As Integer = 0
		For i = 0 To dgSalesOrderItems.Rows.Count - 1
			Try
				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtQty"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtRate"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtDiscount"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtBillBackRate"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtDelInDays"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtCGSTPer = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtCGSTPer"), TextBox)
				txtCGSTPer.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtCGSTPer.ClientID + "').value,event)")

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtSGSTPer"), TextBox)
				txtValue.Text = Val(txtCGSTPer.Text)

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtIGSTPer"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
			Catch ex As Exception
			End Try
		Next


		Dim mSalesOrderItem As SalesOrderItem

		i = 0
		For Each mSalesOrderItem In mSalesOrder.SalesOrderItems
			mSalesOrderItem.ConversionFactor = Val(txtConversionFactor.Text)

			With mSalesOrderItem
				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtQty"), TextBox)
				.Qty = CDec(Val(txtValue.Text))

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtRate"), TextBox)
				.CRate = CDec(Val(txtValue.Text))

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtOtherCharge"), TextBox)
				.COtherCharges = CDec(Val(txtValue.Text))

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtNote"), TextBox)
				.Note = txtValue.Text

				txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtRemark"), TextBox)
				.Remark = txtValue.Text

				'------------------------------------------------------------------
				If AppSettings("IsGSTApplicable") = "True" Then
					mVendor = Vendor.GetVendor(mSalesOrder.VendorID)
					If mVendor.ClientCountryName.ToUpper = "INDIA" Then
						If mVendor.CountryName.ToUpper = "INDIA" And mSalesOrder.Date >= CDate("01-Jul-2017") Then
							'mGSTPercentage = GSTPercentage.GetPercentage(mSalesOrder.Date, 1, .ItemID.ToString)
							'If Not mGSTPercentage Is Nothing Then
							Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
							If Len(mVendor.StateCode) > 0 Then
								If mVendor.StateCode = mVendor.ClientStateCode Then
									txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
									.CGSTPercentage = CDec(Val(txtValue.Text))
									txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
									.SGSTPercentage = Val(txtValue.Text.Trim)
									.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
									.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
									.HSNACSCode = mtmpItem.HSNACSCode
									.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
									.IGSTPercentage = 0
									.IGSTCAmount = 0
									mSalesOrder.StateCode = mVendor.StateCode
									mSalesOrder.ClientStateCode = mVendor.ClientStateCode
									mSalesOrder.VendorCountry = mVendor.CountryName
									mSalesOrder.Visibility = 1
								Else
									txtValue = CType(Me.dgSalesOrderItems.Rows(i).FindControl("txtWIGST"), TextBox)
									.IGSTPercentage = CDec(Val(txtValue.Text))
									.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
									.CGSTPercentage = 0
									.SGSTPercentage = 0
									.CGSTCAmount = 0
									.SGSTCAmount = 0
									.HSNACSCode = mtmpItem.HSNACSCode
									.TotalCAmount = .CAmount + .IGSTCAmount
									mSalesOrder.StateCode = mVendor.StateCode
									mSalesOrder.ClientStateCode = mVendor.ClientStateCode
									mSalesOrder.VendorCountry = mVendor.CountryName
									mSalesOrder.Visibility = 2
								End If
							Else
								.CGSTPercentage = 0
								.SGSTPercentage = 0
								.CGSTCAmount = 0
								.SGSTCAmount = 0
								.IGSTPercentage = 0
								.IGSTCAmount = 0
								.HSNACSCode = ""
								mSalesOrder.StateCode = mVendor.StateCode
								mSalesOrder.ClientStateCode = mVendor.ClientStateCode
								mSalesOrder.VendorCountry = mVendor.CountryName
								mSalesOrder.Visibility = 3
							End If
							'End If
						Else
							.CGSTPercentage = 0
							.SGSTPercentage = 0
							.CGSTCAmount = 0
							.SGSTCAmount = 0
							.IGSTPercentage = 0
							.IGSTCAmount = 0
							.HSNACSCode = ""
							mSalesOrder.StateCode = mVendor.StateCode
							mSalesOrder.ClientStateCode = mVendor.ClientStateCode
							mSalesOrder.VendorCountry = mVendor.CountryName
							mSalesOrder.Visibility = 3
						End If
					Else
						.CGSTPercentage = 0
						.SGSTPercentage = 0
						.CGSTCAmount = 0
						.SGSTCAmount = 0
						.IGSTPercentage = 0
						.IGSTCAmount = 0
						.HSNACSCode = ""
						mSalesOrder.StateCode = mVendor.StateCode
						mSalesOrder.ClientStateCode = mVendor.ClientStateCode
						mSalesOrder.VendorCountry = mVendor.CountryName
						mSalesOrder.Visibility = 3
					End If
				Else
					mSalesOrder.Visibility = 3
				End If
				'------------------------------------------------------------------
			End With
			i = i + 1
		Next
		mSalesOrder.CalculateTotal()

		If mSalesOrder.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012 ALL25102012
			mSalesOrder.RoundCGrandTotal()
		End If
		upnlGrandTotal.Update()
		upnlGrandTotal.DataBind()
		upnlSalesOrderItems.Update()
	End Sub

	Private Sub HiddenImageButtons_Click(sender As Object, e As EventArgs) Handles hdnImgBtnSalesOrderTerms.Click, hdnImgBtnSalesOrderCharges.Click

		dgSalesOrderTerms.DataSource = mSalesOrder.SalesOrderTerms
		dgSalesOrderTerms.DataBind()
		upnlSalesOrderTerms.Update()

		dgChargeList.DataSource = mSalesOrder.SalesOrderCharges
		dgChargeList.DataBind()
		upnlOrderCharge.Update()
		upnlGrandTotal.Update()
		upnlGrandTotal.DataBind()

		SalesOrderChargesGrid()
		SetChargeGrid()

	End Sub

#End Region

#Region " Status "

	'Authorized
	Private Sub AuthorizedRecord(sender As Object, e As EventArgs) Handles btnAuthorized.Click

		If (Not User.IsInRole("SalesOrderAuthorized")) Then

			MSGBoxCtrl.Show("Alert..!!", "You are not authorized user ", "", MsgBoxStyle.OkOnly, "")
			Exit Sub

		End If

		If IsValid Then

			MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized,
							MSGBox.Message_text.StatusAuthorized,
							"<strong>Sales Order</strong>",
							MsgBoxStyle.YesNo,
							"Status")

			mSalesOrder.StatusID = 2
			Session("mSalesOrder") = mSalesOrder

			SetOrderItemGrid()

		End If

	End Sub

	'Cancel
	Private Sub Cancel(sender As Object, e As EventArgs) Handles btnCancel.Click
		If (Not User.IsInRole("SalesOrderAuthorized")) Then
			MSGBoxCtrl.Show("Alert..!!", "You are not authorized user ", "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		If IsValid Then
			Dim IsInUse As IsInUse = IsInUse.GetIsInUseSalesOrderINOrder(mSalesOrder.ID)
			If IsInUse.IsInUse Then
				Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Cancel, SIMsgBox.Message_text.Cancel, "<Strong>Sales Order,It is used in Purchase Order/Issue.</Strong>", MsgBoxStyle.OkOnly)
				msg.ReplacePage = "wfSalesOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
				Session("sender") = "Status"
				msg.Show()
				mSalesOrder.StatusID = 4
				Session("mSalesOrder") = mSalesOrder
				Exit Sub
			End If
			MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<strong>Sales Order</strong>", MsgBoxStyle.YesNo, "Status")
			mSalesOrder.StatusID = 4
			Session("mSalesOrder") = mSalesOrder
		End If
	End Sub

#End Region

#Region " Reports "

	Private Sub PrintRecord(sender As Object, e As EventArgs) Handles btnPrint.Click
		If Not User.IsInRole("SalesOrderPrint") Then
			MSGBoxCtrl.Show("Alert..!!", "You are not authorized user ", "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		Try
			Dim da As New ObjectAdapter
			Dim myReport As Engine.ReportClass
			Dim obj As rptSalesOrder
			Dim objchild As rptSalesOrderChilds

			Dim letter As rptLetterHead
			Dim ds As New dsSalesOrder
			If CDate(txtSalesOrderDate.Text) <= CDate("30-Jun-2017") Or mSalesOrder.Visibility = 3 Then
				myReport = New crptSalesOrderDetailPortrait
			Else
				myReport = New crptSalesOrderGSTDetail
			End If
			obj = rptSalesOrder.GetSalesOrder(mSalesOrder.ID)
			objchild = rptSalesOrderChilds.GetSalesOrderChilds(mSalesOrder.ID)
			letter = rptLetterHead.GetLetterHeadInfo(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "", "", AppSettings("Logo"))
			da.Fill(ds, obj)
			Dim mrptImage As rptImage = rptImage.GetImage(ds)
			da.Fill(ds, objchild)
			da.Fill(ds, mrptImage)
			da.Fill(ds, letter)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport
			Dim Str As String
			Str = "<script language=Javascript>openTranDetail();</script>"
			ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)

		Catch ex As Exception
		End Try
	End Sub

#End Region

#Region " BrokenRulesCollection "

	Public Sub CustomValidate1(s As Object, e As ServerValidateEventArgs)

		Dim strMsg As String = ""
		Dim CustValidator As CustomValidator
		CustValidator = CType(s, CustomValidator)

		If Flag = 1 Then Exit Sub

		SetObject()

		If Not mSalesOrder.IsValid Then

			For i As Integer = 0 To mSalesOrder.GetBrokenRulesCollection.Count - 1
				strMsg = strMsg + mSalesOrder.GetBrokenRulesCollection(i).Description + "<Br>"
			Next

		End If

		Dim mSalesOrderItem As SalesOrderItem

		If Not mSalesOrder.SalesOrderItems.IsValid Then

			For Each mSalesOrderItem In mSalesOrder.SalesOrderItems

				For i As Integer = 0 To mSalesOrderItem.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mSalesOrderItem.ItemName + " : " + mSalesOrderItem.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

			Next

		End If

		If strMsg.Trim <> "" Then

			CustValidator.ErrorMessage = strMsg
			e.IsValid = False

		End If

		Flag = 1

	End Sub

	Public Function CustomValidateObject() As Boolean

		Dim strMsg As String = ""

		SetObject()

		If Not mSalesOrder.IsValid Then

			For i As Integer = 0 To mSalesOrder.GetBrokenRulesCollection.Count - 1
				strMsg = strMsg + mSalesOrder.GetBrokenRulesCollection(i).Description + "<Br>"
			Next

		End If

		Dim mSalesOrderItem As SalesOrderItem

		If Not mSalesOrder.SalesOrderItems.IsValid Then

			For Each mSalesOrderItem In mSalesOrder.SalesOrderItems

				For i As Integer = 0 To mSalesOrderItem.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mSalesOrderItem.ItemName + " : " + mSalesOrderItem.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

			Next

		End If

		If strMsg.Trim <> "" Then

			cvSalesOrder.ErrorMessage = strMsg
			cvSalesOrder.IsValid = False

			Return False

		End If

		Return True

	End Function

#End Region

End Class