Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web.Script.Serialization


Public Class wfPurchaseOrder_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mOrder As Order
	Public mVendorList As VendorList
	Public mCustomerList As VendorList
	Public mCurrencyList As CurrencyList
	Public Flag As Integer                                          'Kalpesh - 03-05-2007 ------
	Public mPriorityList As PriorityList
	Public mPrevTransID As Guid = Guid.Empty                        'Added By Prashant 12-Feb-2010
	Dim mVendorTerms As VendorTerms                                 'Added By Prashant 26-Apr-2010
	Dim EventLogID As Guid                                          'Added by Saylee on 19-July-2011
	Dim mModuleName As String                                       'Added by Saylee on 19-July-2011
	Public mRequisitionItemOrderItems As RequisitionItemOrderItems  'Added by vikrant For New Requisition
	Public IssueDetail As String
	Dim NumberOfIssusDetails As StringBuilder = New StringBuilder
	Dim BaseCurrencysymbol As String = ""
	Dim mOrderItem As OrderItem
	Public mBillToShipToTypeList As BillToShipToTypeList
	Public mShipToTypeList As BillToShipToTypeList
	Public mLocationList As LocationList
	Dim mOpenFrom As String 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
	Dim mFileAttach As FileAttach 'Added By Vikrant On 23-Dec-2014 For All23122014-2
	Dim IsAttachmentDeleted As Boolean = False 'End
	Dim mUser As User
	Public mVendorApprovals As VendorApprovals
	Dim email As Thread
	Public mGSTPercentage As GSTPercentage
	Public mVendor As Vendor
	Dim ChangeInfoDetails As StringBuilder = New StringBuilder
	Dim EmployeeName As String = ""
	Public mTransactionList As TransactionList   'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
	Public mPOTowards As POTowards
	Dim ListofOrderItems As New StringBuilder
	Dim ChargeInfo As String = ""
	'Added By Utkarsh ON 15-May-2013 FOR All13052013-1
	Dim mListOfKitItemsForOrderItem As ListOfKitItemsForOrderItem
	Dim ListOfKitItemsForOrderItemCount As String = "0"
	Dim mFileAttachments As FileAttachments = New FileAttachments() 'Sankalp 25-08-25
	Dim mReceiptCumInvoice As ReceiptCumInvoice
	Public AttachmentHelper As New AttachmentHelper
	Public ReportHelper As New ReportHelper

#End Region

#Region " Enum "
	Private Enum RequstFor
		Supplier = 0
		Customer = 1
	End Enum
	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		FindNow = 7
		Authorized = 8
	End Enum
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mOrder = Session("mOrder")
		mVendorList = Session("mVendorList")
		mCurrencyList = Session("mCurrencyList")
		mCustomerList = Session("mCustomerList")
		mVendorTerms = Session("mVendorTerms")
		mModuleName = Session("mModuleName") 'Added by Saylee on 19-July-2011
		IssueDetail = Session("IssueDetail")
		mFileAttach = Session("mFileAttach") 'Added By Vikrant On 23-Dec-2014 For All23122014-2
		IsAttachmentDeleted = Session("IsAttachmentDeleted") 'End
		mPriorityList = Session("mPriorityList")
		mTransactionList = Session("mTransactionList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
	End Sub
	Private Sub setSession()
		Session("mOrder") = mOrder
		Session("mVendorList") = mVendorList
		Session("mCurrencyList") = mCurrencyList
		Session("mCustomerList") = mCustomerList
		Session("mVendorTerms") = mVendorTerms
		Session("mModuleName") = mModuleName 'Added by Saylee on 19-July-2011
		Session("mFileAttach") = mFileAttach 'Added By Vikrant On 23-Dec-2014 For All23122014-2
		Session("IsAttachmentDeleted") = IsAttachmentDeleted 'End
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mVendorList")
		Session.Remove("mCurrencyList")
		Session.Remove("mCustomerList")
		Session.Remove("Address")
		Session.Remove("Attention")
		Session.Remove("mVendorTerms")
		Session.Remove("mFileAttach") 'Added By Vikrant On 23-Dec-2014 For All23122014-2
		Session.Remove("IsAttachmentDeleted") 'End
	End Sub
	Private Sub setObject()
		If calOrderDate.Text = "" Then
			mOrder.OrderDate = Today.Date
		Else
			mOrder.OrderDate = CDate(calOrderDate.Text)
		End If
		mOrder.DeliveryWithinDays = Val(txtDeliveryWithinDays.Text)
		mOrder.IntOrderNo = txtIntOrderNo.Text
		mOrder.UserName = User.Identity.Name
		mOrder.Text = txtText.Text
		mOrder.No = Val(txtNo.Text)
		mOrder.Amend = txtAmend.Text
		mOrder.OpeningLine = txtOpeningLine.Text
		mOrder.AircraftReg = txtAircraftReg.Text
		mOrder.OrderConfirmationNo = txtOrderConfirmationNo.Text
		mOrder.IsRoundOff = chkIsRoundOff.Checked               'Added By Prashant on 29-Oct-2012
		mOrder.CAdvancePayment = Val(txtAdvancePayment.Text)    'Added By Prashant on 29-Jan-2014
		mOrder.ShipInVia = txtShipInVia.Text.Trim
		mOrder.ShipOutVia = txtShipOutVia.Text.Trim
		mOrder.IsCalibrationOrder = chkIsCalibrationOrder.Checked
		mOrder.POTowardsID = CInt(cmbPOTowards.SelectedValue)
		mOrder.Remark = txtOrderRemark.Text.Trim  'Added By Prashant On 3-Feb-2021 For BA03022021
		mOrder.IsPBHPurchase = chkIsPBHPurchase.Checked 'Added By Prashant On 10-Jan-2023 FLYPAL-552
		mOrder.CalculateTotal()
		'Commented by Sankalp 25-08-25
		'If Not mFileAttach Is Nothing Then 'Added By Vikrant On 23-Dec-2014 For All23122014-2
		'    If mFileAttach.Size > 0 Then
		'        mOrder.IsAttachmentAdded = True
		'    Else
		'        mOrder.IsAttachmentAdded = False
		'    End If
		'End If 'End
		If mOrder.FileAttachments IsNot Nothing Then 'Added By Sankalp on 25-08-25
			If mOrder.FileAttachments.Count > 0 Then
				mOrder.IsAttachmentAdded = True
			Else
				mOrder.IsAttachmentAdded = False
			End If
		End If
		Dim txtValue As TextBox
		Dim txtRemark As TextBox
		Dim cmbValue As DropDownList
		Dim mOrderItem As OrderItem
		Dim i As Integer = 0
		For Each mOrderItem In mOrder.OrderItems
			With mOrderItem
				Try
					txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtQty"), TextBox)
					.Qty = CDec(Val(txtValue.Text))

					If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 7 Then 'Added By Vikrant On 04-Jan-2017 For ALL04012017
						.OrderItemQuotationItems(0).Qty = CDec(Val(txtValue.Text))
					End If 'End

					If Session("RateChangeEventLog") = "RateChangeEventLog" Then
						ChangeInfoDetails.Append(" Old Rate : " + mOrderItem.CRate.ToString)
					End If

					txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtRate"), TextBox)
					.CRate = CDec(Val(txtValue.Text))

					If Session("RateChangeEventLog") = "RateChangeEventLog" Then
						ChangeInfoDetails.Append(" New Rate : " + mOrderItem.CRate.ToString)
						ChangeInfoDetails.Append(" Old Remark : " + mOrderItem.Remark.ToString)
					End If

					txtRemark = CType(Me.dgOrderItems.Rows(i).FindControl("txtRemark"), TextBox)
					.Remark = txtRemark.Text.Trim

					If Session("RateChangeEventLog") = "RateChangeEventLog" Then
						ChangeInfoDetails.Append(" New Remark : " + mOrderItem.Remark.ToString)
					End If

					cmbValue = CType(Me.dgOrderItems.Rows(i).FindControl("cmbPriority"), DropDownList)
					.PriorityID = CInt(cmbValue.SelectedValue)

					txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtDelInDays"), TextBox)
					.DeliveryInDays = CInt(Val(txtValue.Text))

					txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtBillBackRate"), TextBox)
					.CBillBackRate = CDec(Val(txtValue.Text))

					txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtDiscount"), TextBox)
					.PerDiscount = CDec(Val(txtValue.Text))

					'------------------------------------------------------------------
					If AppSettings("IsGSTApplicable") = "True" Then
						Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
						mVendor = Vendor.GetVendor(mOrder.VendorID)
						If mVendor.ClientCountryName.ToUpper = "INDIA" Then
							If mVendor.CountryName.ToUpper = "INDIA" And mOrder.OrderDate >= CDate("01-Jul-2017") Then
								mGSTPercentage = GSTPercentage.GetPercentage(mOrder.OrderDate, 1, .ItemID.ToString)
								If mGSTPercentage IsNot Nothing Then
									If Len(mVendor.StateCode) > 0 Then
										If mVendor.StateCode = mVendor.ClientStateCode Then
											txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtCGSTPer"), TextBox)
											.CGSTPercentage = CDec(Val(txtValue.Text))
											'.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
											txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtSGSTPer"), TextBox)
											.SGSTPercentage = CDec(Val(txtValue.Text))
											'.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
											.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
											.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)

											.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

											.IGSTPercentage = 0
											.IGSTCAmount = 0
											.HSNACSCode = mtmpItem.HSNACSCode
											mOrder.StateCode = mVendor.StateCode
											mOrder.ClientStateCode = mVendor.ClientStateCode
											mOrder.VendorCountry = mVendor.CountryName
											mOrder.Visibility = 1
										Else
											'.IGSTPercentage = (mGSTPercentage.GSTPercentage)
											txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtIGSTPer"), TextBox)
											.IGSTPercentage = CDec(Val(txtValue.Text))
											.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

											.CGSTPercentage = 0
											.SGSTPercentage = 0
											.CGSTCAmount = 0
											.SGSTCAmount = 0

											.TotalCAmount = .CAmount + .IGSTCAmount
											.HSNACSCode = mtmpItem.HSNACSCode
											mOrder.StateCode = mVendor.StateCode
											mOrder.ClientStateCode = mVendor.ClientStateCode
											mOrder.VendorCountry = mVendor.CountryName
											mOrder.Visibility = 2
										End If
									Else
										.CGSTPercentage = 0
										.SGSTPercentage = 0
										.CGSTCAmount = 0
										.SGSTCAmount = 0
										.IGSTPercentage = 0
										.IGSTCAmount = 0
										.TotalCAmount = 0
										.HSNACSCode = ""
										mOrder.StateCode = mVendor.StateCode
										mOrder.ClientStateCode = mVendor.ClientStateCode
										mOrder.VendorCountry = mVendor.CountryName
										mOrder.Visibility = 3
									End If
								End If
							Else
								.CGSTPercentage = 0
								.SGSTPercentage = 0
								.CGSTCAmount = 0
								.SGSTCAmount = 0
								.IGSTPercentage = 0
								.IGSTCAmount = 0
								.TotalCAmount = 0
								.HSNACSCode = ""
								mOrder.StateCode = mVendor.StateCode
								mOrder.ClientStateCode = mVendor.ClientStateCode
								mOrder.VendorCountry = mVendor.CountryName
								mOrder.Visibility = 3
							End If
						Else
							.CGSTPercentage = 0
							.SGSTPercentage = 0
							.CGSTCAmount = 0
							.SGSTCAmount = 0
							.IGSTPercentage = 0
							.IGSTCAmount = 0
							.TotalCAmount = 0
							.HSNACSCode = ""
							mOrder.StateCode = mVendor.StateCode
							mOrder.ClientStateCode = mVendor.ClientStateCode
							mOrder.VendorCountry = mVendor.CountryName
							mOrder.Visibility = 3
						End If
					Else
						mOrder.Visibility = 3
					End If
					'------------------------------------------------------------------
				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			i = i + 1
		Next
		Session("mOrder") = mOrder
	End Sub
	Private Sub setVendorDetails()
		mOrder.VendorID = New Guid(cmbVendorList.SelectedValue)
		mOrder.QuotationNo = txtQuotationNo.Text
		If txtQuotationDate.Text <> "" Then
			mOrder.QuotationDate = CDate(txtQuotationDate.Text)
		Else
			mOrder.QuotationDate = System.DBNull.Value
		End If
		mOrder.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
		mOrder.ConversionFactor = Val(txtConversionFactor.Text)
		Session("mOrder") = mOrder
	End Sub
	Private Sub DeleteRecord(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
		mOrder.OrderItems.CurrentIndex = Index
		Session("mOrder") = mOrder
	End Sub
	Private Sub DeleteCharge(ByVal index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
		mOrder.OrderCharges.CurrentIndex = index
		Session("mOrder") = mOrder
	End Sub
	Private Sub DeleteTerm(ByVal index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveTerm, MSGBox.Message_text.RemoveTerm, "", MsgBoxStyle.YesNo, "DeleteTerm")
		mOrder.OrderTerms.CurrentIndex = index
		Session("mOrder") = mOrder
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
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
							ListofOrderItems.Append(mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " Part No. " + mOrder.OrderItems.CurrentItem.ItemName + " Rate " + mOrder.OrderItems.CurrentItem.CRate.ToString)
							mOrder.OrderItems.Remove(mOrder.OrderItems.CurrentItem)
							mOrder.CalculateTotal()             'Added By Saylee on 10-Sep-2007
							If mOrder.IsRoundOff = True Then    'Added By Prashant on 29-Oct-2012
								mOrder.RoundCGrandTotal()
							End If
							'cmbVendorList.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Or ((mOrder.TransTypeID = 5 Or mOrder.TransTypeID = 39) And (mOrder.AgainstTypeID = 7 Or mOrder.AgainstTypeID = 2) And mOrder.OrderItems.Count > 0) Or (AppSettings("IsGSTApplicable") = "True" And mOrder.OrderItems.Count > 0 And mOrder.AgainstTypeID <> 4 And mOrder.AgainstTypeID <> 6), False, True), Boolean)

							If (mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) _
							Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" _
							Or ((mOrder.TransTypeID = 5 Or mOrder.TransTypeID = 38 Or mOrder.TransTypeID = 39) And (mOrder.AgainstTypeID = 7 Or mOrder.AgainstTypeID = 2) And mOrder.OrderItems.Count > 0) _
							Or (AppSettings("IsGSTApplicable") = "True" And mOrder.OrderItems.Count > 0 And (mOrder.AgainstTypeID = 1 Or mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7 Or (mOrder.AgainstTypeID = 5 And mOrder.ExchangeOrderTypeID = 1))) _
							Or (AppSettings("IsGSTApplicable") = "True" And mOrder.OrderItems.Count > 0 And mOrder.IsNew = False And mOrder.AmendCount = 0 And (mOrder.AgainstTypeID = 4 Or mOrder.AgainstTypeID = 6 Or (mOrder.AgainstTypeID = 5 And mOrder.ExchangeOrderTypeID = 2)))
							) Then
								cmbVendorList.Enabled = False
							Else
								cmbVendorList.Enabled = True
							End If

							If (mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or _
							  ((mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7) And mOrder.OrderItems.Count > 0) Or _
							  ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or _
								Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange") Then
								cmbCurrencyList.Enabled = False
								txtConversionFactor.Enabled = False
							Else
								cmbCurrencyList.Enabled = True
								txtConversionFactor.Enabled = True
							End If
							upnlSupplierDetails.Update()
							chkIsCalibrationOrder.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.OrderItems.Count > 0, False, True), Boolean)
							upnlOrderDetails.Update()
							Session("mOrder") = mOrder
							OrderItemDataGrid()
						Catch ex As SqlException
							'MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
							'Exit Sub
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
							Exit Sub
						Finally
							MarkLog(Util.Action.Delete, mModuleName, ListofOrderItems.ToString + " Removed", Util.ErrorType.NoError, mOrder.ID, EventLogID)
						End Try
					End If
					If MSGBoxCtrl.Sender = "Close" Then
						Session("sender") = ""
						If mOrder.IsValid = True Then
							Session.Remove("IsValid")
							DataFieldBind()
							If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
								ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
								Exit Sub
							End If
							If LimitAfterAutorized() = False Then
								Session("ToMakeAuthorizeButtonVisibleFalse") = ""
								Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange"
								SetControlStatus(mOrder.StatusID)
								ControlVisibility()
								SetControlStatusAfterAmendOrder(mOrder.StatusID)
								upnlOrderItems.Update()
								Exit Sub
							End If
							If Save() Then
								RemoveSession()
								Response.Redirect("Index.aspx")
							Else
								Exit Sub
							End If
						Else
							If CustomValidate2() = False Then
								upnlValidationsummary.Update()
								Exit Sub
							End If
							'Session.Remove("IsValid")
							'RemoveSession()
						End If
					End If
					If MSGBoxCtrl.Sender = "DeleteCharge" Then
						Try
							Session("Sender") = ""
							Dim mOrder As Order
							mOrder = CType(Session("mOrder"), Order)
							If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
								ChargeInfo = "After Change Info. Charge Name " + mOrder.OrderCharges.CurrentItem.ChargeName + " Of Amount " + mOrder.OrderCharges.CurrentItem.CChargeAmount.ToString + " Deleted"
							Else
								ChargeInfo = "Charge Name " + mOrder.OrderCharges.CurrentItem.ChargeName + " Of Amount " + mOrder.OrderCharges.CurrentItem.CChargeAmount.ToString + " Deleted"
							End If
							mOrder.OrderCharges.Remove(mOrder.OrderCharges.CurrentItem)
							mOrder.CalculateTotal()
							If mOrder.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012
								mOrder.RoundCGrandTotal()
							End If
							Session("mOrder") = mOrder
							OrderChargeDataGrid()
							MarkLog(Util.Action.Delete, mModuleName, ChargeInfo, Util.ErrorType.NoError, mOrder.ID, EventLogID)
						Catch ex As SqlException
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
							Exit Sub
						End Try
					End If
					If MSGBoxCtrl.Sender = "DeleteTerm" Then
						Try
							Session("Sender") = ""
							Dim mOrder As Order
							mOrder = CType(Session("mOrder"), Order)
							mOrder.OrderTerms.Remove(mOrder.OrderTerms.CurrentItem)
							Session("mOrder") = mOrder
							dgOrderTerms.DataSource = mOrder.OrderTerms
							dgOrderTerms.DataBind()
							upnlOrderTerms.Update()
						Catch ex As SqlException
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
							Exit Sub
						End Try
					End If
					If MSGBoxCtrl.Sender = "Status" Then
						Session("sender") = ""
						If Session("IsValid") Then
							Session.Remove("IsValid")
							mOrder.StatusID = 2
							DataFieldBind()
							If Save() = True Then 'Added By Saylee on 17-Oct-2012
								Dim ReceiptItemCount As Integer
								ReceiptItemCount = (From m In mOrder.OrderItems
													Where Not m.ReceiptItemID.Equals(Guid.Empty)
													Select m
													).Count
								'If (mOrder.StatusID = 2) And (mOrder.TransTypeID = Util.Trans.PurchaseOrderForExchangeRepair Or mOrder.TransTypeID = Util.Trans.OverHaulRepairOrder) And ReceiptItemCount > 0 Then
								If (mOrder.StatusID = 2) And (mOrder.TransTypeID = Util.Trans.OverHaulRepairOrder) And ReceiptItemCount > 0 And User.IsInRole("IssueToVendorForExchangeNew") = True Then
									MSGBoxCtrl.show("Alert!", "Issue Creation! <BR>Do you want to create Issue? ", "", MsgBoxStyle.YesNo, "IssueCreate")
									Session("IssueCreate") = "IssueCreate"
									Exit Sub
								Else
									'SendMail() 'Added By Prashant 16-Sep-2013 ALL16092013 Add Send Mail Button on 16-Jan-2017
									If mOrder.AmendCount > 0 Then
										'If (mOrder.StatusID = 2) And (mOrder.TransTypeID = Util.Trans.PurchaseOrderForExchangeRepair Or mOrder.TransTypeID = Util.Trans.OverHaulRepairOrder) And ReceiptItemCount > 0 Then
										If (mOrder.StatusID = 2) And (mOrder.TransTypeID = Util.Trans.OverHaulRepairOrder) And ReceiptItemCount > 0 And User.IsInRole("IssueToVendorForExchangeNew") = True Then
											MSGBoxCtrl.show("Alert!", "Issue Creation! <BR>Do you want to create Issue? ", "", MsgBoxStyle.YesNo, "IssueCreate")
											Session("IssueCreate") = "IssueCreate"
											Exit Sub
										Else
											'Response.Redirect("Index.aspx")
											Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")) 'Commeneted and Added On 16-Jan-2017
										End If
									Else
										'Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
										UpdatePanel()
										upnlOrderItems.Update()
										upnlOrderCharges.Update()
										upnlOrderTerms.Update()
									End If
								End If
							End If
						Else
							Session.Remove("IsValid")
							Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
						End If
					End If
					If MSGBoxCtrl.Sender = "StatusCancel" Then
						Session("sender") = ""
						If Session("IsValid") Then
							Session.Remove("IsValid")
							mOrder.StatusID = 4
							DataFieldBind()
							If Save() = True Then
								'SendMail() Add Send Mail Button on 16-Jan-2017
								UpdatePanel()
								If AppSettings("ClientCode") = "BA" Then
									ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileAttachmentAndOtherInfoWindow", "OpenFileAttachmentAndOtherInfoWindow()", True)
									Exit Sub
								End If
							End If
						Else
							Session.Remove("IsValid")
							Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
						End If
					End If
					If MSGBoxCtrl.Sender = "IssueCreate" Then
						Session("sender") = ""
						'Added By Vikrant On 28-July-2014 For BA24072014
						If AppSettings("LockBackDatedTransaction") = "True" Then
							If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
								'Do nothing
							Else
								If CheckDateForTransactionLock(mOrder.OrderDate) Then
									Session("IssueCreate") = ""
									MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mOrder.OrderDate).AddMonths(1)), Month(CDate(mOrder.OrderDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
									Exit Sub
								End If
							End If
						End If
						'End
						If AutoIssueCreation() = True Then
							DataFieldBind()
						End If
						DataFieldBind()
					End If
					If MSGBoxCtrl.Sender = "AmendStatus" Then
						Session("sender") = ""
						If Session("IsValid") Then
							Session.Remove("IsValid")
							DataFieldBind()
							If SaveAmendOrder() = True Then
								'SendMail() Add Send Mail Button on 16-Jan-2017
								'Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
								UpdatePanel()
								upnlOrderItems.Update()
								upnlOrderCharges.Update()
								upnlOrderTerms.Update()
							End If
						Else
							Session.Remove("IsValid")
							Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
						End If
					End If
					'Sankalp 25-05-25
					If MSGBoxCtrl.Sender = "RemoveAttachment" Then
						Try
							Session("Sender") = ""
							mOrder = CType(Session("mOrder"), Order)
							mOrder.FileAttachments.Remove(mOrder.FileAttachments.CurrentItem)
							dgItemAttachment.DataSource = mOrder.FileAttachments
							dgItemAttachment.DataBind()
							upnldgItemAttachment.Update()
							upnlItemAttachment.Update()
							Session("mOrder") = mOrder
						Catch ex As SqlException

						End Try
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						Session.Remove("IsValid")
						Session("Sender") = ""
						If mOrder.IsNew Then Session.Remove("mOrder")
						RemoveSession()
						Response.Redirect("Index.aspx")
					End If
					If MSGBoxCtrl.Sender = "Status" And Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
						Session("Sender") = ""
						Session("ToOpenOrderForRateChange") = "" ' Added By Prashant 28-Jan-2014
						Session.Remove("IsValid")
						UpdatePanel()
						upnlOrderItems.Update()
						upnlOrderCharges.Update()
						upnlOrderTerms.Update()
						'Response.Redirect("wfPurchaseOrder_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					End If
					If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then
						Session("Sender") = ""
						Session.Remove("IsValid")
						Session("mOrder") = mOrder
						DataFieldBind()
						UpdatePanel()
						upnlOrderItems.Update()
						upnlOrderCharges.Update()
						upnlOrderTerms.Update()
						'Response.Redirect("wfPurchaseOrder_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					End If
					If MSGBoxCtrl.Sender = "AmendStatus" Then
						Session("Sender") = ""
						Session.Remove("IsValid")
						If mOrder.StatusID = 2 Then
							mOrder.StatusID = 1
						ElseIf mOrder.StatusID = 3 Or mOrder.StatusID = 4 Then
							mOrder.StatusID = 2
						End If
						Session("mOrder") = mOrder
						DataFieldBind()
						UpdatePanel()
					End If
					If MSGBoxCtrl.Sender = "IssueCreate" Then
						Session("Sender") = ""
						Session("IssueCreate") = ""
						If mOrder.AmendCount > 0 Then
							Response.Redirect("Index.aspx")
						Else
							Response.Redirect("wfPurchaseOrder_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
						End If
					End If
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "Status" Then
						Session("sender") = ""
						If mOrder.StatusID = 2 Then
							mOrder.StatusID = 1
						ElseIf mOrder.StatusID = 3 Or mOrder.StatusID = 4 Then
							mOrder.StatusID = 2
						End If
						Session("mOrder") = mOrder
						Session("NotEqualsQty") = "NotEqualsQty"
						DataFieldBind()
						UpdatePanel()
						upnlOrderItems.Update()
						upnlOrderCharges.Update()
						upnlOrderTerms.Update()
					End If
					If MSGBoxCtrl.Sender = "IssueCreated" Then
						Session("sender") = ""
						DataFieldBind()
						UpdatePanel()
						upnlOrderItems.Update()
						upnlOrderCharges.Update()
						upnlOrderTerms.Update()
						If mOrder.AmendCount > 0 Then
							Response.Redirect("Index.aspx")
						End If
					End If
					If MSGBoxCtrl.Sender = "IssueTransTextSeriesAlert" Then
						Session("AddTransTextSeries") = "True"
						Session("sender") = "IssueCreate" 'Need to set again
						Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
					End If
					If MSGBoxCtrl.Sender = "OrderAuthorizedWithNewRate" Then
						setObject()
						OrderItemDataGrid()
						SetControlStatus(mOrder.StatusID)
						ControlVisibility()
						upnlButtons.Update()
						upnlButtons.DataBind()
					End If
					If MSGBoxCtrl.Sender = "vendornotvalid" Then
						cmbVendorList.ClearSelection()
						upnlSupplierDetails.Update()
					End If
			End Select
		End If
	End Sub
	Private Sub CreateAutoIssue() 'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
		Session("sender") = ""
		If AutoIssueCreation() = True Then
			DataFieldBind()
		End If
		DataFieldBind()
	End Sub 'End
	Private Function CheckDateForTransactionLock(ByVal TransDate As Date) As Boolean 'Added By Vikrant On 28-July-2014 For BA24072014
		Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
		Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
		If (TransDate >= FirstDayofLastMonth) Then
			If (TransDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then
				Return True
			Else
				Return False
			End If
		Else
			Return True
		End If
	End Function 'End
	Private Sub MethodAutoIssueCreation()
		mOrder = Order.GetOrder(mOrder.ID) 'We are not geting ERo Qty first time so get fetch order again
		Dim storewiseitem = (From c In mOrder.OrderItems
							 Where c.EROQty <> 0 _
							 Group By StoreID = c.StoreID Into Group
							 Select New With {.StoreID = StoreID, .ReceiptItemCollection = Group})
		Dim variable

		For Each variable In storewiseitem
			If Not variable.StoreID.Equals(Guid.Empty) Then
				Dim mIssue As Issue = Issue.NewIssue(Util.Trans.ExchangeRepairIssueToVendor)
				mIssue.IDate = mOrder.OrderDate
				mIssue.VendorID = mOrder.VendorID
				mIssue.StoreID = variable.StoreID
				mIssue.MachineID = Guid.Empty
				mIssue.ToStoreID = Guid.Empty
				mIssue.WorkShopID = Guid.Empty
				mIssue.nWOID = Guid.Empty
				mIssue.UserName = User.Identity.Name
				mIssue.StatusID = 2

				Dim receiptitemchildcol
				For Each receiptitemchildcol In variable.ReceiptItemCollection
					mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
					mIssue.IssueItems.CurrentItem.ReceiptItemID = receiptitemchildcol.ReceiptItemID
					mIssue.IssueItems.CurrentItem.DisplayQty = receiptitemchildcol.Qty
					mIssue.IssueItems.CurrentItem.OrderItemID = receiptitemchildcol.ID
				Next
				'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
				If Session("AddTransTextSeries") = "True" Then
					mIssue.Text = Session("TransText_ForTransSeries")
					mIssue.No = Session("TransNo_ForTransSeries")
					Session("AddTransTextSeries") = "False"
					Session.Remove("TransName_ForTransSeries")
					Session.Remove("TransText_ForTransSeries")
					Session.Remove("TransNo_ForTransSeries")
				End If
				'End

				Try
					If mIssue.IsValid Then
						DataFieldBind()
						'Changes for TransTextSeries
						If (mIssue.IsNew) And (mIssue.Text = "") Then

							Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mIssue.TransTypeID, mIssue.IDateFormatted)

							If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID).TransText = "")) Then
								Dim str = "<script language='javascript'>openledgersame('" + "wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") + "'); </script>"

								Session("BackPagestr_ForTransSeries") = str
								Session("TransName_ForTransSeries") = "Issue"
								Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
								Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
								'MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "You have requested to create Exchange Issue against this Order. But, system does not find transaction series for Issue. Click Ok to enter transaction series.", MsgBoxStyle.OkOnly, "IssueTransTextSeriesAlert")
								setObject()
								Session("RedirectFromTransSeries") = "RedirectFromTransSeries"
								Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
								' Exit Sub
							Else
								Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

								If mAutoRenewTransTextSeries.IsRenewed Then
									With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID)
										mIssue.Text = .TransText
										mIssue.No = .StartingTransNo
									End With
								Else
									Dim str = "<script language='javascript'>openledgersame('wfPurchaseOrder_Ajax.aspx');</script>"

									Session("BackPagestr_ForTransSeries") = str

									Session("TransName_ForTransSeries") = "Issue"
									Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
									Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
									Session("AddTransTextSeries") = "True"

									Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
								End If
							End If
						End If

						mIssue.Save()
						IssueDetail = IssueDetail + "Issue : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + "<BR>"
						NumberOfIssusDetails.Append(mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + "<BR>")
						Session("IssueDetail") = IssueDetail
						MarkLog(Action.Save, "Issue", IssueDetail.Replace("<BR>", "") & " Authorized By: " & mOrder.AuthorizedBy, Util.ErrorType.NoError, mIssue.ID, EventLogID)
					Else
					End If
				Catch ex1 As Exception
					If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then
						'MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OkOnly, "Status")
						'Session("mIssue") = mIssue
						'DataFieldBind()
						'Exit Sub
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", False), True)
						Exit Sub
					ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
						'MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OkOnly, "")
						'Exit Sub
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", False), True)
						Exit Sub
					End If
				End Try
			End If
		Next
		ShowMessage(mOrderItem, Nothing, NumberOfIssusDetails.ToString) 'Changed By Prashant 13-Dec-2013  ALL13122013-2
		'End If
	End Sub
	Private Function AutoIssueCreation() As Boolean 'Added By Saylee on 17-Oct-2012
		MethodAutoIssueCreation()
		Return True
	End Function
	Private Sub ShowMessage(ByVal mOrderItem As OrderItem, ByVal mIssue As Issue, Optional ByVal IssueDetail As String = "") ''Added By Prashant 13-Dec-2013  ALL13122013-2
		Dim str1 As String = ""
		Dim Count As Integer = 0
		Dim Count1 As Integer = 0
		For Each mOrderItem In mOrder.OrderItems
			If (mOrderItem.ReceiptItemID.Equals(Guid.Empty) Or (Not mOrderItem.ReceiptItemID.Equals(Guid.Empty) And mOrderItem.EROQty = (0 Or 0.0))) Then
				Count = Count + 1
			Else
				Count1 = Count1 + 1
			End If
		Next
		If (Count > 0 And Count1 > 0) Then
			str1 = str1 + ("<span class=""clsLabelAuto"">Issue(s) Created Successfully! <BR>" + IssueDetail + "</BR></span>")
			str1 = str1 + ("<p><span class=""clsLabelAuto"">Automated issue will not be created for following items. As source receipt not selected/Qty. is zero " + "</span></p>")
			str1 = str1 + ("<TABLE width =""100%"" BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
			str1 = str1 + ("<tr>" & "<td WIDTH=60px align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Part No.</b>" & "</font>" & "</td><td WIDTH=100px align=""right"">" & "<font face=""Calibri""><b>Qty.</b>" & "</font>" & "</td></tr>")
			For Each mOrderItem In mOrder.OrderItems
				If (mOrderItem.ReceiptItemID.Equals(Guid.Empty) Or (Not mOrderItem.ReceiptItemID.Equals(Guid.Empty) And mOrderItem.EROQty = (0 Or 0.0))) Then
					str1 = str1 + ("<TR>")
					str1 = str1 + ("<TD WIDTH=60px align=""left"">")
					str1 = str1 + ("<font face=""Calibri"">")
					str1 = str1 + CStr(mOrderItem.SrNo)
					str1 = str1 + ("</font>")
					str1 = str1 + ("</TD>")

					str1 = str1 + ("<TD align=""left"">")
					str1 = str1 + ("<font face=""Calibri"">")
					str1 = str1 + mOrderItem.ItemName
					str1 = str1 + ("</font>")
					str1 = str1 + ("</TD>")

					str1 = str1 + ("<TD WIDTH=100px align=""right"">")
					str1 = str1 + ("<font face=""Calibri"">")
					str1 = str1 + CStr(mOrderItem.EROQty)
					str1 = str1 + ("</font>")
					str1 = str1 + ("</TD>")

					str1 = str1 + ("</TR>")
				End If
			Next
			str1 = str1 + ("</TABLE>")
		ElseIf (Count > 0) Then
			str1 = str1 + ("<p><span class=""clsLabelAuto"">Automated issue will not be created for following items. As source receipt not selected/Qty. is zero " + "</span></p>")
			str1 = str1 + ("<TABLE width =""100%"" BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
			str1 = str1 + ("<tr>" & "<td WIDTH=60px align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Part No.</b>" & "</font>" & "</td><td WIDTH=100px align=""right"">" & "<font face=""Calibri""><b>Qty.</b>" & "</font>" & "</td></tr>")
			For Each mOrderItem In mOrder.OrderItems
				If (mOrderItem.ReceiptItemID.Equals(Guid.Empty) Or (Not mOrderItem.ReceiptItemID.Equals(Guid.Empty) And mOrderItem.EROQty = (0 Or 0.0))) Then
					str1 = str1 + ("<TR>")
					str1 = str1 + ("<TD WIDTH=60px align=""left"">")
					str1 = str1 + ("<font face=""Calibri"">")
					str1 = str1 + CStr(mOrderItem.SrNo)
					str1 = str1 + ("</font>")
					str1 = str1 + ("</TD>")

					str1 = str1 + ("<TD align=""left"">")
					str1 = str1 + ("<font face=""Calibri"">")
					str1 = str1 + mOrderItem.ItemName
					str1 = str1 + ("</font>")
					str1 = str1 + ("</TD>")

					str1 = str1 + ("<TD WIDTH=100px align=""right"">")
					str1 = str1 + ("<font face=""Calibri"">")
					str1 = str1 + CStr(mOrderItem.EROQty)
					str1 = str1 + ("</font>")
					str1 = str1 + ("</TD>")

					str1 = str1 + ("</TR>")
				End If
			Next
			str1 = str1 + ("</TABLE>")
		ElseIf (Count1 > 0) Then
			str1 = str1 + ("<span class=""clsLabelAuto"">Issue(s) Created Successfully! <BR>" + IssueDetail + "</BR></span>")
		End If
		Session.Remove("IssueDetail")
		Session("IssueCreate") = ""
		MSGBoxCtrl.show("Alert!", str1, "", MsgBoxStyle.OkOnly, "IssueCreated")
		Exit Sub
	End Sub
	Protected Sub AddAttributesForGridControls()
		Dim txtValue As TextBox
		Dim txtCGSTPer As TextBox
		For i As Integer = 0 To dgOrderItems.Rows.Count - 1
			Try
				txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtQty"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtRate"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtDiscount"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtBillBackRate"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtDelInDays"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtCGSTPer = CType(Me.dgOrderItems.Rows(i).FindControl("txtCGSTPer"), TextBox)
				txtCGSTPer.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtCGSTPer.ClientID + "').value,event)")

				txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtSGSTPer"), TextBox)
				txtValue.Text = Val(txtCGSTPer.Text)

				txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtIGSTPer"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
			Catch ex As Exception
			End Try
		Next
		upnlOrderItems.Update()
	End Sub
	Private Sub addAttributes()
		txtDeliveryWithinDays.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtDeliveryWithinDays').value,event)")
		txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value),event")
		txtAdvancePayment.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)") 'Added By Prashant 29-Jan-2014
	End Sub
	Private Sub SetControlStatusAfterAmendOrder(ByVal StatusId As Int16)
		Dim txtValue As TextBox
		Dim txtRate As TextBox
		Dim txtDiscount As TextBox
		Dim txtDelInDays As TextBox
		Dim txtCGSTPer As TextBox
		Dim txtIGSTPer As TextBox
		Dim txtRemark As TextBox
		Dim mOrderItem As OrderItem
		Dim i As Integer = 0
		For Each mOrderItem In mOrder.OrderItems
			With mOrderItem
				Try
					txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtQty"), TextBox)
					txtRate = CType(Me.dgOrderItems.Rows(i).FindControl("txtRate"), TextBox)
					txtDiscount = CType(Me.dgOrderItems.Rows(i).FindControl("txtDiscount"), TextBox)
					txtDelInDays = CType(Me.dgOrderItems.Rows(i).FindControl("txtDelInDays"), TextBox)
					txtCGSTPer = CType(Me.dgOrderItems.Rows(i).FindControl("txtCGSTPer"), TextBox)
					txtIGSTPer = CType(Me.dgOrderItems.Rows(i).FindControl("txtIGSTPer"), TextBox)
					txtRemark = CType(Me.dgOrderItems.Rows(i).FindControl("txtRemark"), TextBox)
					'If (Me.dgOrderItems.Rows.Item(i).Cells(20).Text = "0" Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.OrderItems(i).EROQty = (0 Or 0.0))) Then
					'If (Me.dgOrderItems.Rows.Item(i).Cells(29).Text = "0" Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.OrderItems(i).TempEROQtyForEnableDisable = (0 Or 0.0))) Then
					If (Me.dgOrderItems.Rows.Item(i).Cells(28).Text = "0" Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.OrderItems(i).TempEROQtyForEnableDisable = (0 Or 0.0))) Then
						'If ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.OrderItems(i).EROQty = (0 Or 0.0) And mOrder.AmendCount > 0) Then
						If ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.OrderItems(i).TempEROQtyForEnableDisable = (0 Or 0.0) And mOrder.AmendCount > 0) Then
							txtValue.Enabled = False
							If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then  'Added By Prashant 29-jan-2014
								If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
									txtRate.Enabled = False
									txtRemark.Enabled = False
								Else
									txtRate.Enabled = True
									txtRemark.Enabled = True
								End If
							Else
								txtRate.Enabled = False
								txtRemark.Enabled = False
							End If
							txtDiscount.Enabled = False
							txtDelInDays.Enabled = False
							'txtCGSTPer.Enabled = False
							'txtIGSTPer.Enabled = False
							dgOrderItems.Rows.Item(i).Cells(27).Enabled = False  'Edit
							dgOrderItems.Rows.Item(i).Cells(28).Enabled = False  'Remove
						Else
							If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
								If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
									txtRate.Enabled = False
									txtRemark.Enabled = False
								Else
									'Commented and Added by Prashant on 11-Jun-2020 ALL11062020
									'txtRate.Enabled = True
									'txtRemark.Enabled = True
									If (mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7) Then  'As Order is against 2 Quoation and 7 Enqiery item indirectly Quotation
										txtRate.Enabled = False
										txtRemark.Enabled = False
										txtDiscount.Enabled = False
									Else
										txtRate.Enabled = True
										txtRemark.Enabled = True
									End If
									'End of Commented and Added by Prashant on 11-Jun-2020 ALL11062020
								End If
							Else
								'txtRate.Enabled = False
							End If
							'txtDiscount.Enabled = False
							'txtDelInDays.Enabled = False
						End If
						'ElseIf (Not (Me.dgOrderItems.Rows.Item(i).Cells(20).Text).Equals(txtValue.Text) And StatusId <> 3) Then
						'ElseIf ((Not (Me.dgOrderItems.Rows.Item(i).Cells(20).Text).Equals(txtValue.Text) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And (Not mOrder.OrderItems(i).EROQty.Equals(txtValue.Text)))) And StatusId <> 3) Then
						'ElseIf ((Not (Me.dgOrderItems.Rows.Item(i).Cells(29).Text).Equals(txtValue.Text) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And (Not mOrder.OrderItems(i).TempEROQtyForEnableDisable.Equals(txtValue.Text)))) And StatusId <> 3) Then
					ElseIf ((Not (Me.dgOrderItems.Rows.Item(i).Cells(28).Text).Equals(txtValue.Text) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And (Not mOrder.OrderItems(i).TempEROQtyForEnableDisable.Equals(txtValue.Text)))) And StatusId <> 3) Then
						'If (((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And Not mOrder.OrderItems(i).EROQty.Equals(txtValue.Text)) And StatusId <> 3) Then
						If (((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And Not mOrder.OrderItems(i).TempEROQtyForEnableDisable.Equals(txtValue.Text)) And StatusId <> 3) Then
							'txtDelInDays.Enabled = False
							'txtCGSTPer.Enabled = False
							'txtIGSTPer.Enabled = False
							If (StatusId = 2 Or StatusId = 4) Then
								txtValue.Enabled = False
								txtRate.Enabled = False
								txtDiscount.Enabled = False
								txtDelInDays.Enabled = False
								txtRemark.Enabled = False
							Else
								'txtValue.Enabled = True
								If AppSettings("AddChargesInRCI") = "True" And mOrder.TransTypeID = 31 Then
									txtValue.Enabled = False
								Else
									txtValue.Enabled = True
								End If
								txtDelInDays.Enabled = True
								txtRemark.Enabled = True
								If (mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7) Then
									txtRate.Enabled = False
									txtDiscount.Enabled = False
								Else
									txtRate.Enabled = True
									txtDiscount.Enabled = True
								End If
							End If
							If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
								If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
									txtRate.Enabled = False
									txtRemark.Enabled = False
								Else
									txtRate.Enabled = True
									txtRemark.Enabled = True
								End If
							End If
						Else
							txtRate.Enabled = False
							txtDiscount.Enabled = False
							txtDelInDays.Enabled = False
							txtRemark.Enabled = False
							If StatusId = 2 Then
								txtValue.Enabled = False
							Else
								'If (mOrder.AgainstTypeID = 6 Or mOrder.AgainstTypeID = 3) Then
								If (mOrder.AgainstTypeID = 3) Then
									txtValue.Enabled = False
								Else
									txtValue.Enabled = True
								End If
							End If
						End If
						'ElseIf (((Me.dgOrderItems.Rows.Item(i).Cells(20).Text).Equals(txtValue.Text) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And (mOrder.OrderItems(i).EROQty.Equals(txtValue.Text)))) And StatusId = 1 And mOrder.AmendCount > 0) Then
						'ElseIf (((Me.dgOrderItems.Rows.Item(i).Cells(29).Text).Equals(txtValue.Text) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And (mOrder.OrderItems(i).TempEROQtyForEnableDisable.Equals(txtValue.Text)))) And StatusId = 1 And mOrder.AmendCount > 0) Then
					ElseIf (((Me.dgOrderItems.Rows.Item(i).Cells(28).Text).Equals(txtValue.Text) Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And (mOrder.OrderItems(i).TempEROQtyForEnableDisable.Equals(txtValue.Text)))) And StatusId = 1 And mOrder.AmendCount > 0) Then
						'If (mOrder.AgainstTypeID = 6 Or mOrder.AgainstTypeID = 3) Then
						If (mOrder.AgainstTypeID = 3) Then
							txtValue.Enabled = False
							txtRate.Enabled = False
							txtDiscount.Enabled = False
							txtDeliveryWithinDays.Enabled = False
							txtRemark.Enabled = False
						Else
							'txtValue.Enabled = True
							If AppSettings("AddChargesInRCI") = "True" and mOrder.TransTypeID = 31 Then
								txtValue.Enabled = False
							Else
								txtValue.Enabled = True
							End If
							'Commented and Added by Prashant on 11-Jun-2020 ALL11062020
							'txtRate.Enabled = True
							'txtDiscount.Enabled = True
							'txtRemark.Enabled = True
							If mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7 Then  'As Order is against 2 Quoation and 7 Enqiery item indirectly Quotation
								txtRate.Enabled = False
								txtDiscount.Enabled = False
								txtRemark.Enabled = False
							Else
								txtRate.Enabled = True
								txtDiscount.Enabled = True
								txtRemark.Enabled = True
							End If
							'End of Commented and Added by Prashant on 11-Jun-2020 ALL11062020
						End If
					ElseIf Session("NotEqualsQty") = "NotEqualsQty" Then
						Session("NotEqualsQty") = ""
						txtRate.Enabled = False
						txtDiscount.Enabled = False
						txtDelInDays.Enabled = False
						txtRemark.Enabled = False
						'txtCGSTPer.Enabled = False
						'txtIGSTPer.Enabled = False
						'If StatusId = 2 Then
						'	txtValue.Enabled = True
						'End If
						If StatusId = 2 Then
							If AppSettings("AddChargesInRCI") = "True" Then
								txtValue.Enabled = False
							Else
								txtValue.Enabled = True
							End If
						End If
					End If
				Catch ex As Exception

				End Try
			End With
			i = i + 1
		Next
		If (mOrder.AmendCount > 0 And mOrder.StatusID = 1 And (mOrder.ReceiptCount > 0 Or mOrder.IssueCount > 0)) Then
			btnBack.Enabled = False
		Else
			btnBack.Enabled = True
		End If
	End Sub
	Private Sub SetControlStatus(ByVal StatusId As Int16)
		'btnAdd.Enabled = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
		btnAdd.Enabled = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or (AppSettings("AddChargesInRCI") = "True" And mOrder.OrderItems.Count > 0 And mOrder.TransTypeID = 31), False, True)
		dgOrderItems.Columns(25).Visible = (mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7 Or mOrder.AgainstTypeID = 6) And AppSettings("ClientCode") = "BA" '7 against enquiry , 6 against requisition
		dgOrderItems.Columns(26).Visible = (AppSettings("ClientCode") = "STR") 'Added By Prashant on 23-Sep-2020 STR23092020
		dgOrderItems.Columns(27).Visible = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or mOrder.IssueCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True)
		'dgOrderItems.Columns(28).Visible = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or mOrder.IssueCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True)
		dgOrderItems.Columns(12).Visible = IIf(mOrder.TransTypeID <> 5, True, False)
		'dgOrderItems.Columns(30).Visible = IIf(mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38, True, False) 'View Tech Direction 
		dgOrderItems.Columns(29).Visible = IIf(mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38, True, False) 'View Tech Direction 
		If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then  'Added By Prashant 29-jan-2014
			If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
				btnAddTerm.Enabled = False
				btnAddCharges.Enabled = False
			Else
				btnAddTerm.Enabled = True
				btnAddCharges.Enabled = True
			End If
			upnlOrderTerms.Update()
			upnlOrderCharges.Update()
		Else
			btnAddTerm.Enabled = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
			btnAddCharges.Enabled = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
		End If
		'end
		If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then 'Added By Prashant 29-jan-2014
			If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
				dgOrderTerms.Columns(2).Visible = False
			Else
				dgOrderTerms.Columns(2).Visible = True
			End If
			upnlOrderTerms.Update()
		Else
			dgOrderTerms.Columns(2).Visible = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
		End If
		btnSave.Visible = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True)
		If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
			If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
				'dgOrderCharges.Columns(4).Visible = False
				'dgOrderCharges.Columns(5).Visible = False
				dgOrderCharges.Columns(4).Visible = False
			Else
				'dgOrderCharges.Columns(4).Visible = True
				'dgOrderCharges.Columns(5).Visible = True
				dgOrderCharges.Columns(4).Visible = True
			End If
			upnlOrderCharges.Update()
		Else
			'dgOrderCharges.Columns(4).Visible = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
			'dgOrderCharges.Columns(5).Visible = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
			dgOrderCharges.Columns(4).Visible = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
		End If
		btnAddSupplierSpecificTerms.Enabled = IIf(StatusId > 1 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0), False, True)
	End Sub
	Private Sub SetPage()
		If mOrder.No > 0 Then
			If mOrder.TransTypeID = 5 Then
				lblTitle.Text = "Purchase Order for New Purchase [" & mOrder.Text & " - " & mOrder.No & IIf(mOrder.Amend = "", "", " (" & mOrder.Amend & ") ").ToString & "]" '24-11-2006
			End If
			If mOrder.TransTypeID = 31 Then
				lblTitle.Text = "Purchase Order for Exchange Goods [" & mOrder.Text & " - " & mOrder.No & IIf(mOrder.Amend = "", "", " (" & mOrder.Amend & ") ").ToString & "]" '24-11-2006
			End If
			If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = True Then
				lblTitle.Text = "Purchase Order for OverHaul Goods [" & mOrder.Text & " - " & mOrder.No & IIf(mOrder.Amend = "", "", " (" & mOrder.Amend & ") ").ToString & "]" '24-11-2006
			End If
			If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = False Then
				lblTitle.Text = "Purchase Order for Repair Goods [" & mOrder.Text & " - " & mOrder.No & IIf(mOrder.Amend = "", "", " (" & mOrder.Amend & ") ").ToString & "]" '24-11-2006
			End If
			If mOrder.TransTypeID = 39 Then
				lblTitle.Text = "Purchase Order for Rental/Lease Goods [" & mOrder.Text & " - " & mOrder.No & IIf(mOrder.Amend = "", "", " (" & mOrder.Amend & ") ").ToString & "]" '24-11-2006
			End If
		Else
			If mOrder.TransTypeID = 5 Then
				lblTitle.Text = "Purchase Order for New Purchase [NEW]"
			End If
			If mOrder.TransTypeID = 31 Then
				lblTitle.Text = "Purchase Order for Exchange Goods [NEW]"
			End If
			If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = True Then
				lblTitle.Text = "Purchase Order for OverHaul Goods [NEW]"
			End If
			If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = False Then
				lblTitle.Text = "Purchase Order for Repair Goods [NEW]"
			End If
			If mOrder.TransTypeID = 39 Then
				lblTitle.Text = "Purchase Order for Rental/Lease Goods [NEW]"
			End If
		End If
		'If mOrder.TransTypeID = 5 Then
		'    lblTitle.Text = "Purchase Order for New Purchase"
		'End If
		'If mOrder.TransTypeID = 31 Then
		'    lblTitle.Text = "Purchase Order for Exchange Goods"
		'End If
		'If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = True Then
		'    lblTitle.Text = "Purchase Order for OverHaul Goods"
		'End If
		'If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = False Then
		'    lblTitle.Text = "Purchase Order for Repair Goods"
		'End If
		'If mOrder.TransTypeID = 39 Then
		'    lblTitle.Text = "Purchase Order for Rental/Lease Goods"
		'End If
		upnlTitle.Update()
	End Sub
	Private Sub ControlVisibility()
		txtText.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		txtNo.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		txtAmend.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		'cmbVendorList.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Or ((mOrder.TransTypeID = 5 Or mOrder.TransTypeID = 39) And (mOrder.AgainstTypeID = 7 Or mOrder.AgainstTypeID = 2) And mOrder.OrderItems.Count > 0) Or (AppSettings("IsGSTApplicable") = "True" And mOrder.OrderItems.Count > 0 And mOrder.AgainstTypeID <> 4 And mOrder.AgainstTypeID <> 6), False, True), Boolean)

		If (mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) _
			Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" _
			Or ((mOrder.TransTypeID = 5 Or mOrder.TransTypeID = 38 Or mOrder.TransTypeID = 39) And (mOrder.AgainstTypeID = 7 Or mOrder.AgainstTypeID = 2) And mOrder.OrderItems.Count > 0) _
			Or (AppSettings("IsGSTApplicable") = "True" And mOrder.OrderItems.Count > 0 And (mOrder.AgainstTypeID = 1 Or mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7 Or (mOrder.AgainstTypeID = 5 And mOrder.ExchangeOrderTypeID = 1))) _
			Or (AppSettings("IsGSTApplicable") = "True" And mOrder.OrderItems.Count > 0 And mOrder.IsNew = False And mOrder.AmendCount = 0 And (mOrder.AgainstTypeID = 4 Or mOrder.AgainstTypeID = 6 Or (mOrder.AgainstTypeID = 5 And mOrder.ExchangeOrderTypeID = 2)))
			) Then
			cmbVendorList.Enabled = False
		Else
			cmbVendorList.Enabled = True
		End If
		'txtQuotationNo.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		If (Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" And Session("ToMakeAuthorizeButtonVisibleFalse") = "") Then
			txtQuotationNo.Enabled = True
			txtQuotationDate.Enabled = True
			txtOpeningLine.Enabled = True
			upnlSupplierDetails.Update()
			upnlOpeningLine.Update()
			upnldgItemAttachment.Update()  'Sankalp 03-09-25
			upnlItemAttachment.Update() 'Sankalp 03-09-25
		Else
			txtQuotationNo.Enabled = CType(IIf(mOrder.StatusID >= 2, False, True), Boolean)
			txtQuotationDate.Enabled = CType(IIf(mOrder.StatusID >= 2, False, True), Boolean)
			txtOpeningLine.Enabled = CType(IIf(mOrder.StatusID >= 2, False, True), Boolean)
			upnlSupplierDetails.Update()
			upnlOpeningLine.Update()
			upnldgItemAttachment.Update()  'Sankalp 03-09-25
			upnlItemAttachment.Update() 'Sankalp 03-09-25
		End If

		'Commented and Added by Prashant on 11-Jun-2020 ALL11062020
		'cmbCurrencyList.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		'txtConversionFactor.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		If (mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or _
		   ((mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7) And mOrder.OrderItems.Count > 0) Or _
		   ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or _
			 Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange") Then
			cmbCurrencyList.Enabled = False
			txtConversionFactor.Enabled = False
		Else
			cmbCurrencyList.Enabled = True
			txtConversionFactor.Enabled = True
		End If
		'End of Commented and Added by Prashant on 11-Jun-2020 ALL11062020

		calOrderDate.Enabled = (mOrder.AgainstTypeID = 1 And mOrder.IsNew) Or (mOrder.AgainstTypeID <> 1 And mOrder.OrderItems.Count = 0)
		'txtQuotationDate.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		txtDeliveryWithinDays.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		txtIntOrderNo.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		chkIsRoundOff.Enabled = (mOrder.StatusID = 1)
		chkIsCalibrationOrder.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.OrderItems.Count > 0, False, True), Boolean)
		'Authorized Status
		If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then  'Added By Prashant 28-Jan-2014
			If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
				btnAuthorized.Visible = False
			Else
				btnAuthorized.Visible = (Not mOrder.OrderItems.Count = 0) And (Not mOrder.IsNew)
			End If
		Else
			btnAuthorized.Visible = (Not mOrder.OrderItems.Count = 0) And (Not mOrder.IsNew) And (mOrder.StatusID = 1)
		End If
		'Added by Saylee on 22-Nov-2012 for ALL22112012
		Dim mShowTopAmendedOrderNo As ShowTopAmendedOrderNo
		mShowTopAmendedOrderNo = ShowTopAmendedOrderNo.GetTopAmendedOrderNo(mOrder.Text, mOrder.No)
		'Amended Status
		Dim mSumOfReceiptBalanceQtyFromOrderItemTab As SumOfReceiptBalanceQtyFromOrderItemTab
		mSumOfReceiptBalanceQtyFromOrderItemTab = SumOfReceiptBalanceQtyFromOrderItemTab.GetSumOfReceiptBalanceQtyFromOrderItemTab(mOrder.ID)
		'Amended Status
		'btnAmend.Visible = Not mOrder.IsNew And mSumOfReceiptBalanceQtyFromOrderItemTab.ReceiptBalanceQtySum > (0 Or 0.0) And ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mSumOfReceiptBalanceQtyFromOrderItemTab.SumOfEROQty > (0 Or 0.0)) And (mOrder.StatusID = 2 Or (mOrder.StatusID = 3 And mShowTopAmendedOrderNo.ID.Equals(mOrder.ID))) And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))
		If (Not mOrder.TransTypeID = 31 And Not mOrder.TransTypeID = 38) Then 'Added by Saylee on 23-Oct-2012
			'Canceled Status
			btnAmend.Visible = Not mOrder.IsNew And mSumOfReceiptBalanceQtyFromOrderItemTab.ReceiptBalanceQtySum > (0 Or 0.0) And (mOrder.StatusID = 2 Or (mOrder.StatusID = 3 And mShowTopAmendedOrderNo.ID.Equals(mOrder.ID))) And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))
			btnCancel.Visible = (Not mOrder.IsNew) And (mOrder.StatusID = 2) And (Not mOrder.TransTypeID = 31 Or Not mOrder.TransTypeID = 38)
		Else
			'Commented by Shital on 09-Dec-2019 for BA Requirement open CANCEL for exchange/OverHaul/Repaire 
			'btnCancel.Visible = False
			btnCancel.Visible = (Not mOrder.IsNew) And (mOrder.StatusID = 2)

			btnAmend.Visible = Not mOrder.IsNew And mSumOfReceiptBalanceQtyFromOrderItemTab.ReceiptBalanceQtySum > (0 Or 0.0) And mSumOfReceiptBalanceQtyFromOrderItemTab.SumOfEROQty > (0 Or 0.0) And (mOrder.StatusID = 2 Or (mOrder.StatusID = 3 And mShowTopAmendedOrderNo.ID.Equals(mOrder.ID))) And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))
		End If
		btnChangeRate.Visible = (Not mOrder.IsNew) And (mOrder.StatusID = 2) And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print)) And (mOrder.TransTypeID = 31 Or (mOrder.TransTypeID = 38 And mOrder.AgainstTypeID <> 2)) 'Added By Prashant 28-Jan-2014 mOrder.AgainstTypeID = 2 'Repair/Overhul order against quotation
		txtAdvancePayment.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or ((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean) 'Added By Prashant 29-Jan-2014
		Dim txtQty As TextBox
		Dim txtRate As TextBox
		Dim txtRemark As TextBox
		Dim txtNote As TextBox
		Dim txtBillBackRate As TextBox
		Dim txtDelInDays As TextBox
		Dim txtDiscount As TextBox
		Dim cmbValue As DropDownList
		Dim txtCGSTPer As TextBox
		Dim txtIGSTPer As TextBox
		For i As Integer = 0 To dgOrderItems.Rows.Count - 1
			txtQty = CType(Me.dgOrderItems.Rows(i).FindControl("txtQty"), TextBox)
			'txtQty.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.AgainstTypeID = 3 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
			txtQty.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.AgainstTypeID = 3 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Or (AppSettings("AddChargesInRCI") = "True" And mOrder.TransTypeID = 31), False, True), Boolean)
			txtRate = CType(Me.dgOrderItems.Rows(i).FindControl("txtRate"), TextBox)
			txtRemark = CType(Me.dgOrderItems.Rows(i).FindControl("txtRemark"), TextBox)
			txtDiscount = CType(Me.dgOrderItems.Rows(i).FindControl("txtDiscount"), TextBox)
			If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then  'Added By Prashant 29-jan-2014
				If Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse" Then
					txtRate.Enabled = False
					txtRemark.Enabled = False
				Else
					txtRate.Enabled = True
					txtRemark.Enabled = True
				End If
				txtDiscount.Enabled = False
			Else
				txtRemark.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or mOrder.AgainstTypeID = 3, False, True), Boolean)
				'Commented and Added by Prashant on 11-Jun-2020 ALL11062020
				'txtRate.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or mOrder.AgainstTypeID = 3 Or mOrder.AgainstTypeID = 6, False, True), Boolean)
				'txtRate.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or mOrder.AgainstTypeID = 3, False, True), Boolean)
				txtRate.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or mOrder.AgainstTypeID = 3 Or mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7, False, True), Boolean)
				txtDiscount.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or mOrder.AgainstTypeID = 3 Or mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7, False, True), Boolean)
				'End of Commented and Added by Prashant on 11-Jun-2020 ALL11062020
			End If
			txtNote = CType(Me.dgOrderItems.Rows(i).FindControl("txtNote"), TextBox)
			txtNote.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
			cmbValue = CType(Me.dgOrderItems.Rows(i).FindControl("cmbPriority"), DropDownList)
			cmbValue.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
			txtBillBackRate = CType(Me.dgOrderItems.Rows(i).FindControl("txtBillBackRate"), TextBox)
			txtBillBackRate.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
			txtDelInDays = CType(Me.dgOrderItems.Rows(i).FindControl("txtDelInDays"), TextBox)
			'txtDelInDays.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or mOrder.AgainstTypeID = 6 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
			txtDelInDays.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)

			'Commented and Added above by Prashant on 11-Jun-2020 ALL11062020
			'txtDiscount = CType(Me.dgOrderItems.Rows(i).FindControl("txtDiscount"), TextBox)
			'''txtDiscount.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or mOrder.AgainstTypeID = 6 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
			'txtDiscount.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
			'End of Commented and Added by Prashant on 11-Jun-2020 ALL11062020
		Next
		If Not IsInRole(Rights.Authorized) Then
			btnAuthorized.Enabled = False
			btnAuthorized.ToolTip = "You are not authorized user "
			btnCancel.Enabled = False
			btnCancel.ToolTip = "You are not authorized user "
			btnChangeRate.Enabled = False                           'Added By Prashant on 30-Jul-2021 ALL30072021 As Authorized By Name was changing
			btnChangeRate.ToolTip = "You are not authorized user "  'Added By Prashant on 30-Jul-2021 ALL30072021 As Authorized By Name was changing
			btnAmend.Enabled = False                                'Added By Prashant on 30-Jul-2021 ALL30072021 As Authorized By Name was changing
			btnAmend.ToolTip = "You are not authorized user "       'Added By Prashant on 30-Jul-2021 ALL30072021 As Authorized By Name was changing
		End If
		dgOrderItems.Columns(3).Visible = IIf(mOrder.TransTypeID = 38 Or mOrder.TransTypeID = 31, True, False) 'Added by Shweta on 15-July-2013 For ALL12072013 
		'Added By Vikrant On 23-Dec-2014 For All23122014-2
		'btnSelectFile.Disabled = IIf(mOrder.StatusID >= 2, True, False)
		'btnSelectFile.Disabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", True, False), Boolean) 'Commented by sankalp 25-08-25

		ControlVisibilityForAttachment()
		'End
		btnSendMail.Visible = (mOrder.StatusID > 1 And mOrder.OrderItems.Count > 0)
		'---------------------------------------------------------------------------
		For i As Integer = 0 To dgOrderItems.Rows.Count - 1
			txtCGSTPer = CType(Me.dgOrderItems.Rows(i).FindControl("txtCGSTPer"), TextBox)
			txtCGSTPer.Enabled = IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Or AppSettings("ChangeGSTPercentage") = "False" Or mOrder.OrderItems(i).HSNACSCode = "", False, True)
			txtIGSTPer = CType(Me.dgOrderItems.Rows(i).FindControl("txtIGSTPer"), TextBox)
			txtIGSTPer.Enabled = IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Or AppSettings("ChangeGSTPercentage") = "False" Or mOrder.OrderItems(i).HSNACSCode = "", False, True)
		Next
		If mOrder.Visibility = 1 Then
			dgOrderItems.Columns(18).Visible = True  'HSNACSCode 
			dgOrderItems.Columns(19).Visible = True 'CGSTPercentage 
			dgOrderItems.Columns(20).Visible = True 'CGSTCAmount 
			dgOrderItems.Columns(21).Visible = True 'SGSTPercentage 
			dgOrderItems.Columns(22).Visible = True 'SGSTCAmount 
			dgOrderItems.Columns(23).Visible = False 'IGSTPercentage 
			dgOrderItems.Columns(24).Visible = False 'IGSTCAmount 


			lblTotalCGST.Visible = True
			txtTotalCGST.Visible = True
			lblTotalSGST.Visible = True
			txtTotalSGST.Visible = True

			lblTotalIGST.Visible = False
			txtTotalIGST.Visible = False
		ElseIf mOrder.Visibility = 2 Then
			dgOrderItems.Columns(18).Visible = True  'HSNACSCode 
			dgOrderItems.Columns(19).Visible = False 'CGSTPercentage 
			dgOrderItems.Columns(20).Visible = False 'CGSTCAmount 
			dgOrderItems.Columns(21).Visible = False 'SGSTPercentage 
			dgOrderItems.Columns(22).Visible = False 'SGSTCAmount 
			dgOrderItems.Columns(23).Visible = True  'IGSTPercentage 
			dgOrderItems.Columns(24).Visible = True 'IGSTCAmount 

			lblTotalCGST.Visible = False
			txtTotalCGST.Visible = False
			lblTotalSGST.Visible = False
			txtTotalSGST.Visible = False

			lblTotalIGST.Visible = True
			txtTotalIGST.Visible = True
		ElseIf mOrder.Visibility = 3 Then
			If AppSettings("HSNACSCodeVisibleInPartMaster") = "True" Then
				dgOrderItems.Columns(18).Visible = True 'HSNACSCode 
			Else
				dgOrderItems.Columns(18).Visible = False 'HSNACSCode  
			End If
			dgOrderItems.Columns(19).Visible = False 'CGSTPercentage 
			dgOrderItems.Columns(20).Visible = False 'CGSTCAmount 
			dgOrderItems.Columns(21).Visible = False 'SGSTPercentage 
			dgOrderItems.Columns(22).Visible = False 'SGSTCAmount 
			dgOrderItems.Columns(23).Visible = False  'IGSTPercentage 
			dgOrderItems.Columns(24).Visible = False 'IGSTCAmount 
			lblTotalCGST.Visible = False
			txtTotalCGST.Visible = False
			lblTotalSGST.Visible = False
			txtTotalSGST.Visible = False
			lblTotalIGST.Visible = False
			txtTotalIGST.Visible = False
		End If
		'---------------------------------------------------------------------------
		'If mOrder.Visibility = 1 Or mOrder.Visibility = 2 Then
		'    Dim txtCGSTPercentage As TextBox
		'    Dim txtSGSTPercentage As TextBox
		'    Dim txtIGSTPercentage As TextBox

		'    For i As Integer = 0 To dgOrderItems.Rows.Count - 1
		'        txtCGSTPercentage = CType(Me.dgOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
		'        txtSGSTPercentage = CType(Me.dgOrderItems.Rows(i).FindControl("txtWSGST"), TextBox)
		'        txtIGSTPercentage = CType(Me.dgOrderItems.Rows(i).FindControl("txtWIGST"), TextBox)

		'        txtCGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, False, True) 'CGSTPercentage 
		'        'txtSGSTPercentage.Enabled = IIf(AppSettings("ChangeGSTPercentage") = "True" And mQuotation.StatusID <> 2 And mQuotation.StatusID <> 4, False,True ) 'SGSTPercentage 
		'        txtIGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, False, True) 'IGSTPercentage 

		'        txtCGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, Color.White, Color.Gainsboro) 'CGSTPercentage 
		'        'txtSGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") = "True" And mQuotation.StatusID <> 2 And mQuotation.StatusID <> 4, Color.White,Color.Gainsboro ) 'SGSTPercentage 
		'        txtIGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, Color.White, Color.Gainsboro) 'IGSTPercentage 
		'    Next
		'End If

		btnPrint.Enabled = (Not (mOrder.StatusID = 3) And Not (mOrder.IsNew))
	End Sub
	Private Function GridViewGSTColoumnsVisibility()
		'---------------------------------------------------------------------------
		If mOrder.Visibility = 1 Then
			dgOrderItems.Columns(18).Visible = True  'HSNACSCode 
			dgOrderItems.Columns(19).Visible = True 'CGSTPercentage 
			dgOrderItems.Columns(20).Visible = True 'CGSTCAmount 
			dgOrderItems.Columns(21).Visible = True 'SGSTPercentage 
			dgOrderItems.Columns(22).Visible = True 'SGSTCAmount 
			dgOrderItems.Columns(23).Visible = False 'IGSTPercentage 
			dgOrderItems.Columns(24).Visible = False 'IGSTCAmount 


			lblTotalCGST.Visible = True
			txtTotalCGST.Visible = True
			lblTotalSGST.Visible = True
			txtTotalSGST.Visible = True

			lblTotalIGST.Visible = False
			txtTotalIGST.Visible = False
		ElseIf mOrder.Visibility = 2 Then
			dgOrderItems.Columns(18).Visible = True  'HSNACSCode 
			dgOrderItems.Columns(19).Visible = False 'CGSTPercentage 
			dgOrderItems.Columns(20).Visible = False 'CGSTCAmount 
			dgOrderItems.Columns(21).Visible = False 'SGSTPercentage 
			dgOrderItems.Columns(22).Visible = False 'SGSTCAmount 
			dgOrderItems.Columns(23).Visible = True  'IGSTPercentage 
			dgOrderItems.Columns(24).Visible = True 'IGSTCAmount 

			lblTotalCGST.Visible = False
			txtTotalCGST.Visible = False
			lblTotalSGST.Visible = False
			txtTotalSGST.Visible = False

			lblTotalIGST.Visible = True
			txtTotalIGST.Visible = True
		ElseIf mOrder.Visibility = 3 Then
			If AppSettings("HSNACSCodeVisibleInPartMaster") = "True" Then
				dgOrderItems.Columns(18).Visible = True 'HSNACSCode 
			Else
				dgOrderItems.Columns(18).Visible = False 'HSNACSCode  
			End If
			dgOrderItems.Columns(19).Visible = False 'CGSTPercentage 
			dgOrderItems.Columns(20).Visible = False 'CGSTCAmount 
			dgOrderItems.Columns(21).Visible = False 'SGSTPercentage 
			dgOrderItems.Columns(22).Visible = False 'SGSTCAmount 
			dgOrderItems.Columns(23).Visible = False  'IGSTPercentage 
			dgOrderItems.Columns(24).Visible = False 'IGSTCAmount 
			lblTotalCGST.Visible = False
			txtTotalCGST.Visible = False
			lblTotalSGST.Visible = False
			txtTotalSGST.Visible = False
			lblTotalIGST.Visible = False
			txtTotalIGST.Visible = False
		End If
		'---------------------------------------------------------------------------
		'If mOrder.Visibility = 1 Or mOrder.Visibility = 2 Then
		'    Dim txtCGSTPercentage As TextBox
		'    Dim txtSGSTPercentage As TextBox
		'    Dim txtIGSTPercentage As TextBox

		'    For i As Integer = 0 To dgOrderItems.Rows.Count - 1
		'        txtCGSTPercentage = CType(Me.dgOrderItems.Rows(i).FindControl("txtWCGST"), TextBox)
		'        txtSGSTPercentage = CType(Me.dgOrderItems.Rows(i).FindControl("txtWSGST"), TextBox)
		'        txtIGSTPercentage = CType(Me.dgOrderItems.Rows(i).FindControl("txtWIGST"), TextBox)

		'        txtCGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, False, True) 'CGSTPercentage 
		'        'txtSGSTPercentage.Enabled = IIf(AppSettings("ChangeGSTPercentage") = "True" And mQuotation.StatusID <> 2 And mQuotation.StatusID <> 4, False,True ) 'SGSTPercentage 
		'        txtIGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, False, True) 'IGSTPercentage 

		'        txtCGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, Color.White, Color.Gainsboro) 'CGSTPercentage 
		'        'txtSGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") = "True" And mQuotation.StatusID <> 2 And mQuotation.StatusID <> 4, Color.White,Color.Gainsboro ) 'SGSTPercentage 
		'        txtIGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") = "True" And mOrder.StatusID <> 2 And mOrder.StatusID <> 4, Color.White, Color.Gainsboro) 'IGSTPercentage 
		'    Next
		'End If
	End Function
	Private Function SaveAmendOrder() As Boolean
		If mOrder.StatusID = 3 And CType(Session("Amend"), String) = "Yes" Then
			Dim mAmendOrder As Order

			mOrder.StatusID = 1
			mOrder.AmendedStatus = True
			mOrder.AmendCount = mOrder.AmendCount + 1

			mAmendOrder = Order.GetAmendedOrder(mOrder)
			mOrder = CType(mOrder.Save(), Order)

			mAmendOrder.IsAttachmentAdded = False
			mAmendOrder = CType(mAmendOrder.Save(), Order)
			'To make StatusID  as MarkDirty  again set it to 2 and then 1 
			mOrder.StatusID = 2
			mOrder.StatusID = 1
			mOrder.AmendedStatus = False
			mOrder = CType(mOrder.Save(), Order)
			mOrder = Order.GetOrder(mOrder.ID) 'We are not geting ERo Qty first time so to get it fetch order again
			SetControlStatusAfterAmendOrder(mOrder.StatusID)
			Dim OrderDetail As String = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mVendorList(mOrder.VendorID).Name & " Created By : " & mOrder.UserName 'Added by Saylee on 19-July-2011 
			MarkLog(Util.Action.Amend, mModuleName, OrderDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
			Session("Amend") = ""
		End If
		Session("mOrder") = mOrder
		SetPage()
		Return True
	End Function
	'Added By Vikrant On 23-Dec-2014 For All23122014-2
	'Commented by Sankalp 25-08-25
	'Private Sub ControlVisibilityForAttachment()
	'    If mOrder.IsAttachmentAdded Then
	'        ImageButton1.Visible = True
	'        'btnDelAttach.Enabled = IIf(mOrder.StatusID >= 2, False, True)
	'        btnDelAttach.Enabled = CType(IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
	'    Else
	'        ImageButton1.Visible = False
	'        btnDelAttach.Enabled = False
	'    End If
	'End Sub
	Private Sub SaveAttachment() '
		If mFileAttach IsNot Nothing Then
			If mFileAttach.Size > 0 Then
				Try
					mFileAttach.Save()
				Catch ex As Exception
					ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
				End Try
			Else
				If (Not mOrder.IsNew) And IsAttachmentDeleted Then
					FileAttach.DeleteAttachment(mFileAttach.ID, mOrder.ID)
				End If
				IsAttachmentDeleted = False
				Session("IsAttachmentDeleted") = IsAttachmentDeleted
			End If
		End If
	End Sub
	Private Sub ViewImage()
		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString

		If mOrder.IsAttachmentAdded And mFileAttach Is Nothing Then
			mFileAttach = FileAttach.GetAttachment(mOrder.ID)
		End If
		If mFileAttach.Size > 0 Then
			Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
			Dim fs As FileStream
			If File.Exists(AppSettings("DOCPath")) = False Then
				'Delete File if exist
				System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
				' Create the file.
				fs = File.Create(path)
				'' Add some information to the file.
				fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
			End If
		End If
	End Sub
	'End
	Private Function Save() As Boolean
		'Authentication
		If mOrder.OrderDate IsNot System.DBNull.Value Then
			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
			If mCheck.WebAuthentication = True Then
				Dim mDays As Integer = 0
				mDays = mCheck.Number("Days")
				Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
				If DateDiff(DateInterval.Day, CDate(mOrder.OrderDate), maxAllowableDate) < 0 Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Your subscription has been expired. can not save Order." + "\n" + "Order Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), False), True)
					Exit Function
				End If
			End If
		End If
		'Authentication
		Dim OrderClone As Order
		OrderClone = mOrder.Clone
		Try
			If Not mOrder.OrderItems.Count = 0 Then
				setObject()
				setVendorDetails()
				If CurrencyRightsBeforeSave() = False Then Exit Function 'Added by Prashant
				If mOrder.VendorID.Equals(mOrder.CustomerID) Then
					'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Supplier & Customer are same. <br><br> Select another Customer from list.", MsgBoxStyle.OkOnly, "")
					'Exit Function
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Supplier & Customer are same." + "\n" + "Select another Customer from list.", False), True)
					Exit Function
				End If
				If mVendorList(mOrder.VendorID).NotInUse = True Then
					If CDate(mVendorList(mOrder.VendorID).NotInUseDate) <= CDate(mOrder.OrderDate) Then
						'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Supplier is not applicable since " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
						'Exit Function
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Supplier is not applicable since " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + "\n" + "Select another Supplier from list or select date before " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " & try again", False), True)
						Exit Function
					End If
				End If
				If mOrder.IsCustomer = True Then
					If mVendorList(mOrder.CustomerID).NotInUse = True Then
						If CDate(mVendorList(mOrder.CustomerID).NotInUseDate) <= CDate(mOrder.OrderDate) Then
							'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Customer is not applicable since " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + " <br><br> Select another Customer from list or select date before " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
							'Exit Function
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Customer is not applicable since " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + "\n" + "Select another Customer from list or select date before " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + " & try again", False), True)
							Exit Function
						End If
					End If
				End If
				Session("mOrder") = mOrder
				Dim mOrderCharge As OrderCharge
				For Each mOrderCharge In mOrder.OrderCharges
					If (mOrderCharge.Sign <> 1 And mOrderCharge.CChargeAmount <= 0) Or (Not (mOrderCharge.IsValid)) Then
						'MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Order Charge(s) are not allowed if Order Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Percentage Order Charge(s) are not allowed if Order Amount Is Zero. ", False), True)
						mOrder.CancelEdit()
						Exit Function
					End If
				Next
				mOrder.ApplyEdit()
				If mOrder.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012
					mOrder.RoundCGrandTotal()
				End If
				'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
				'Check if OrderText is blank then call TransTextSeries UI
				If (mOrder.IsNew) And (mOrder.Text = "") Then

					Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mOrder.TransTypeID, mOrder.OrderDateFormatted)

					If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mOrder.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mOrder.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mOrder.TransTypeID).TransText = "")) Then

						Dim str = "<script language='javascript'>openledgersame('wfPurchaseOrder_Ajax.aspx');</script>"

						Session("BackPagestr_ForTransSeries") = str

						Session("TransName_ForTransSeries") = "Order"
						Session("TransTypeID_ForTransSeries") = mOrder.TransTypeID
						Session("TransDate_ForTransSeries") = mOrder.OrderDateFormatted
						Session("AddTransTextSeries") = "True"

						Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

					Else
						Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

						If mAutoRenewTransTextSeries.IsRenewed Then
							With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mOrder.TransTypeID)
								mOrder.Text = .TransText
								mOrder.No = .StartingTransNo
							End With
						Else
							Dim str = "<script language='javascript'>openledgersame('wfPurchaseOrder_Ajax.aspx');</script>"

							Session("BackPagestr_ForTransSeries") = str

							Session("TransName_ForTransSeries") = "Order"
							Session("TransTypeID_ForTransSeries") = mOrder.TransTypeID
							Session("TransDate_ForTransSeries") = mOrder.OrderDateFormatted
							Session("AddTransTextSeries") = "True"

							Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
						End If
					End If

				End If
				'End
				mOrder.Save()
				SaveAttachment() 'Added By Vikrant On 23-Dec-2014 For All23122014-2
				'Changed by Utkarsh ON 09-Aug-2012  
				'Dim OrderDetail As String = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mVendorList(mOrder.VendorID).Name & " Created By : " & mOrder.UserName 'Added by Saylee on 19-July-2011 
				Dim OrderDetail As String = String.Empty
				If Session("RateChangeEventLog") = "RateChangeEventLog" Then
					Session("RateChangeEventLog") = ""

					'===================================================
					If AppSettings("AddChargesInRCI") = "True" Then 'Added By Prashant
						If mOrder.TransTypeID = 31 Then 'Exchange Order
							If (mOrder.OrderItems.Count = 1 And mOrder.OrderItems.CurrentItem.Qty = 1 And mOrder.OrderItems.CurrentItem.ReceiptBalanceQty = 0) Then

								mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(OrderID:=mOrder.ID, IsByOrderID:=True)

								If mOrder.OrderCharges.Count > 0 Then 'Order has charges

									' --- Add / Update ---
									For Each oCharge As OrderCharge In mOrder.OrderCharges
										Dim rCharge As InvoiceCharge = Nothing

										' Find existing charge
										For Each c As InvoiceCharge In mReceiptCumInvoice.Invoice.InvoiceCharges
											If c.ChargeID = oCharge.ChargeID Then
												rCharge = c
												Exit For
											End If
										Next

										If rCharge Is Nothing Then
											' --- Add new charge ---
											mReceiptCumInvoice.Invoice.InvoiceCharges.Add(mReceiptCumInvoice.Invoice.ID)

											With mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem
												.SrNo = mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentIndex + 1
												.ChargeID = oCharge.ChargeID
												.ConversionFactor = mReceiptCumInvoice.ConversionFactor
												.Percentage = oCharge.Percentage
												.CChargeAmount = oCharge.CChargeAmount

												If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then
													.BasicAmount = mReceiptCumInvoice.ReceiptCumInvoiceItems.CGrandTotalAmountItem
												End If
											End With
										Else
											' --- Update if amount differs ---
											If rCharge.CChargeAmount <> oCharge.CChargeAmount Then
												rCharge.CChargeAmount = oCharge.CChargeAmount
											End If
										End If
									Next

									' --- Delete charges that are not in Order anymore ---
									For i As Integer = mReceiptCumInvoice.Invoice.InvoiceCharges.Count - 1 To 0 Step -1
										Dim rCharge As InvoiceCharge = mReceiptCumInvoice.Invoice.InvoiceCharges(i)
										Dim existsInOrder As Boolean = False

										For Each oCharge As OrderCharge In mOrder.OrderCharges
											If oCharge.ChargeID = rCharge.ChargeID Then
												existsInOrder = True
												Exit For
											End If
										Next

										If Not existsInOrder Then
											mReceiptCumInvoice.Invoice.InvoiceCharges.RemoveAt(i)
										End If
									Next
									' --- Recalculate totals ---
									If mReceiptCumInvoice.Invoice.InvoiceCharges.IsDirty Then
										mReceiptCumInvoice.Invoice.CalculateTotal()
										mReceiptCumInvoice.Save()
										Session("InvoiceChargesAddedUdateFromOrder") = mReceiptCumInvoice
									End If
								End If
							End If
						End If
					End If
					'===================================================

					OrderDetail = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mVendorList(mOrder.VendorID).Name & " Created By : " & mOrder.UserName & ChangeInfoDetails.ToString + " From Change Info. Button"  'Added by Saylee on 19-July-2011 
					ChangeInfoDetails.Clear()
				Else
					OrderDetail = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mVendorList(mOrder.VendorID).Name & " Created By : " & mOrder.UserName   'Added by Saylee on 19-July-2011 
				End If
				If mOrder.StatusID = 2 Then
					SendPUSHNotification(mOrder) 'Added by Prashant on 18-Oct-2021
					MarkLog(Util.Action.Authorize, mModuleName, OrderDetail & " Authorized By : " & mOrder.AuthorizedBy, Util.ErrorType.NoError, mOrder.ID, EventLogID)
				ElseIf mOrder.StatusID = 3 Then
					MarkLog(Util.Action.Amend, mModuleName, OrderDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
				ElseIf mOrder.StatusID = 4 Then
					MarkLog(Util.Action.Cancel, mModuleName, OrderDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
				Else
					MarkLog(Util.Action.Save, mModuleName, OrderDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
				End If
				mOrder.MarkClean()
				lblTitle.Text = "Purchase Order ( Saved ...)"
				Session("mOrder") = mOrder
				SetPage()
				Return True
			Else
				'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Purchase Order can not be saved without Item.", MsgBoxStyle.OkOnly, "")
				'Exit Function
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Purchase Order can not be saved without Item.", False), True)
				Exit Function
			End If
		Catch ex As SqlClient.SqlException
			Session("OrderClone") = OrderClone
			If ex.Number = 8114 Or ex.Number = 8115 Then
				MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				Exit Function
			ElseIf ex.Number = 8145 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				Exit Function
			ElseIf ex.Number = 2627 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				Exit Function
			ElseIf ex.Number = 547 Then
				If InStr(ex.Message, "CCtabQuotationItemPurchaseBalQty", CompareMethod.Text) Then
					'MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Order Qty can not be greater than Quotation Qty.", MsgBoxStyle.OkOnly, "")
					'Exit Function
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Order Qty can not be greater than Quotation Qty.", False), True)
					Exit Function
				ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
					MSGBoxCtrl.Show("Alert!", "Cannot Save / Authorized !!!" + " <BR> Order Qty can not be less than Received Qty.", "", MsgBoxStyle.OkOnly, "Status")
					Exit Function
					'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Order Qty can not be less than Received Qty.", False), True)
					'Exit Function
				ElseIf InStr(ex.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
					MSGBoxCtrl.Show("Alert!", "Cannot Save / Authorized !!!" + " <BR> Order Qty can not be less than Issued Qty.", "", MsgBoxStyle.OkOnly, "Status")
					Exit Function
				ElseIf InStr(ex.Message, "FK_tabOrderCharge_tabCharge", CompareMethod.Text) Then
					'MSGBoxCtrl.show("Alert!", "Other Charge Deleted! " + "<BR> Other Charge Not available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", "", MsgBoxStyle.OkOnly, "")
					'Exit Function
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Other Charge is not available" + "\n" + "Selected Charge is no longer exist in the Database" + "\n" + "Remove the Charge and try again", False), True)
					Exit Function
				ElseIf InStr(ex.Message, "FKtabOrderTermtabTerm", CompareMethod.Text) Then
					MSGBoxCtrl.Show("Alert!", "Term Deleted! " + "<BR>Term is not available<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove the Term and try again", "", MsgBoxStyle.OkOnly, "")
					Exit Function
				Else
					'MSGBoxCtrl.show("Alert!", "Can Not Be Saved !\n\n" + ex.Message, "", MsgBoxStyle.OkOnly, "")
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Can Not Be Saved !", False), True)
					Exit Function
				End If
			ElseIf ex.Number = 50000 Then
				If ex.State = 2 Then
					MSGBoxCtrl.show("Alert!", "Can Not Save ! " + "</br>" + ex.Message, "", MsgBoxStyle.OkOnly, "Status")
					Exit Function
					'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Can Not Be Saved !\n\n" + ex.Message, False), True)
					'Exit Function
				Else
					MSGBoxCtrl.show("Alert!", "Can Not Save ! " + "</br>" + ex.Message, "", MsgBoxStyle.OkOnly, "Status")
					Exit Function
					'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Can Not Be Saved !\n\n" + ex.Message, False), True)
					'Exit Function
				End If
			End If
			mOrder = OrderClone
			Session("mOrder") = mOrder
		Catch ex As Exception
			MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
			Exit Function
		Finally
			OrderClone = Nothing
		End Try
	End Function
	Private Function getVendorStatus(ByVal TransTypeID As Integer, ByVal Type As RequstFor) As Boolean
		If Type = RequstFor.Supplier Then                                  'Purchase Order / PurchaseOrderForExchangeRepair
			Select Case CType(TransTypeID, Trans)
				Case Util.Trans.PurchaseOrder
					Return True
				Case Util.Trans.PurchaseOrderForExchangeRepair
					Return True
				Case Else
					Return False
			End Select
		ElseIf Type = RequstFor.Customer Then                              '--------      
			Return False
		End If
	End Function
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
			Case Rights.Authorized          'Added By Prashant 17-Aug-2011
				Return User.IsInRole(IsInRoleString + "Authorized")
		End Select
	End Function
	Private Function IsInRoleForIssue(ByVal CheckFor As Rights, ByVal mIssue As Issue) As Boolean
		Dim IsInRoleString As String = ""

		If mIssue.TransTypeID = Util.Trans.ExchangeRepairIssueToVendor Then
			IsInRoleString = "IssueToVendorForExchange"
		End If

		Select Case CheckFor
			Case Rights.View
				Return User.IsInRole(IsInRoleString + "View")
			Case Rights.[New]
				Return User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return User.IsInRole(IsInRoleString + "Delete")
			Case Rights.Print
				Return User.IsInRole(IsInRoleString + "Print")
			Case Rights.Authorized
				Return User.IsInRole(IsInRoleString + "Authorized")
				'-----------------------------
		End Select
	End Function
	'Private Sub AddPart() 'Added by vikrant For New Requisition
	'    Dim mRequisitionItemNew As RequisitionItemNew
	'    Dim mRequisitionItemsNew As RequisitionItemsNew = Session("mRequisitionItemsNew")
	'    If mRequisitionItemsNew Is Nothing Then Exit Sub
	'    For Each mRequisitionItemNew In mRequisitionItemsNew
	'        If mRequisitionItemNew.IsSelect Then
	'            With mOrder.OrderItems.CurrentItem
	'                'Check is Requisition Part is present ?
	'                If Not .RequisitionItemOrderItems.Contains(mRequisitionItemNew.ID) Then
	'                    'if NOT then add
	'                    .RequisitionItemOrderItems.Add(.ID, mRequisitionItemNew.ID, mRequisitionItemNew.OrderBalQty, mRequisitionItemNew.RequisitionNo)
	'                Else
	'                    'if YES fire Message
	'                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ValidationAlert, SIMsgBox.Message_text.ValidationAlert, "Requisition item already taken for Order", MsgBoxStyle.OkOnly)
	'                    'msg1.ReplacePage = "wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
	'                    'Session("sender") = "Close"
	'                    'msg1.Show()
	'                    'MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for Order", MsgBoxStyle.OkOnly, "")
	'                    'Exit Sub
	'                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Requisition item already taken for Order", False), True)
	'                    Exit Sub
	'                End If
	'            End With
	'        End If
	'    Next
	'End Sub 'End
	Private Sub SetChargeGrid()
		For j As Integer = 0 To dgOrderCharges.Rows.Count - 1
			If (Me.dgOrderCharges.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or Me.dgOrderCharges.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then
				'dgOrderCharges.Rows.Item(j).Cells(4).Enabled = False
				'dgOrderCharges.Rows.Item(j).Cells(5).Enabled = False
				dgOrderCharges.Rows.Item(j).Cells(4).Enabled = False
				'dgOrderCharges.Rows.Item(j).Cells(5).Enabled = False
			End If
		Next
	End Sub
	Private Sub GetCanceledQtyBeforeZero()
		Dim txtValue As TextBox
		Dim mOrderItem As OrderItem
		Dim i As Integer = 0
		For Each mOrderItem In mOrder.OrderItems
			With mOrderItem
				Try
					txtValue = CType(Me.dgOrderItems.Rows(i).FindControl("txtQty"), TextBox)
					If (CDec(Val(txtValue.Text)) = 0 And mOrder.AmendCount > 0 And mOrderItem.CanceledQty = 0) Then
						.CanceledQty = mOrderItem.Qty
					End If
				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			i = i + 1
		Next
		Session("mOrder") = mOrder
	End Sub
	Private Sub UpdatePanel()
		ControlsDataBind()
		upnlStatusName.Update()
		upnlOrderDetails.Update()
		upnlSupplierDetails.Update()
		upnlTotalAmount.Update()
		upnlGrandTotal.Update()
		upnlButtons.Update()
		SetControlStatus(mOrder.StatusID)
		ControlVisibility()
		SetControlStatusAfterAmendOrder(mOrder.StatusID)
	End Sub
	Public Function GetPeriodUnitID(PeriodID As Integer) As Integer
		Select Case PeriodID
			Case 1
				Return 1
			Case 2
				Return 0
			Case 3
				Return 6
			Case 4
				Return 7
			Case 5
				Return 8
			Case 6
				Return 9
			Case 7
				Return 10
			Case 8
				Return 11
			Case 9
				Return 12
			Case 10
				Return 13
			Case 11
				Return 14
			Case 12
				Return 15
			Case 13
				Return 16
			Case 14
				Return 17
			Case 15
				Return 18
		End Select
	End Function
	Public Sub SendPUSHNotification(ByVal tmpOrder As Order)
		Dim PreviousStepStatus As Boolean = False

		'Step # 1: Get User Devices

		Dim mUserDeviceList As APP_UserDeviceList = APP_UserDeviceList.GetUserDeviceList(2) '2:Order

		If mUserDeviceList.Count = 0 Then
			PreviousStepStatus = False
		Else
			PreviousStepStatus = True
		End If

		If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------


		'Step # 2: Record PUSH Notification in the table

		Dim UserIDs(50) As Guid
		UserIDs = (From c As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList
				   Select (c.UserID)).Distinct().ToArray()

		Dim Notifications(UserIDs.Count - 1) As APP_UserNotification

		For i As Integer = 0 To UserIDs.Count - 1

			If UserIDs(i).Equals(Guid.Empty) Then Exit For

			Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)


			Try
				With mAPP_UserNotification
					.UserID = UserIDs(i)
					.SentOn = Now
					.Message = "Order:- " + tmpOrder.OrderNo + " Dated:- " + tmpOrder.OrderDateFormatted + " Authorized By:- " + tmpOrder.AuthorizedBy
					.ModuleType = 2 'Order
					.ModuleID = tmpOrder.ID
				End With

				mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

				Notifications(i) = mAPP_UserNotification

				PreviousStepStatus = True
			Catch ex As Exception
				PreviousStepStatus = False
			End Try
		Next

		'Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)

		If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------

		For Each Notification As APP_UserNotification In Notifications

			Dim errorcount As Integer = 0

StartStep3:

			'Step # 3: Trigger PUSH Notification

			errorcount = errorcount + 1

			System.Net.ServicePointManager.Expect100Continue = True
			System.Net.ServicePointManager.SecurityProtocol = 3072 'System.Net.SecurityProtocolType.Tls

			Dim request = TryCast(System.Net.WebRequest.Create("https://onesignal.com/api/v1/notifications"), System.Net.HttpWebRequest)

			request.KeepAlive = True
			request.Method = "POST"
			request.ContentType = "application/json; charset=utf-8"

			request.Headers.Add("authorization", "Basic YmE0YTUwZDgtMmJkYS00MjMzLWI5NjgtZTkxZmE5MzQ0NzMw")

			Dim serializer = New JavaScriptSerializer()

			'Forming Notification Detail URL
			'
			'
			Dim index As Integer = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("wfPurchaseOrder_Ajax.aspx")
			Dim urlNotificationDetail As String = ""
			urlNotificationDetail = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, index) + "APP/Launcher.aspx?NotificationID=" + Notification.ID.ToString + "&ModuleID=" + tmpOrder.ID.ToString + "&username=" + Notification.UserName + "&EventLogSessionID=" + Guid.NewGuid.ToString + "&ModuleTypeID=2"


			Dim filterObject As Object()
			ReDim filterObject(((mUserDeviceList.Count - 1) * 2) + 1)

			Dim idx As Integer = 0
			Dim Ridx As Integer = 0
			For Each info As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList

				If Notification.UserID.Equals(info.UserID) Then


					If idx = 0 Then
						filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(0).DeviceID.ToString}
						idx = idx + 1
					Else
						Ridx = Ridx + 1

						filterObject(idx) = New With {Key .[operator] = "OR"}
						idx = idx + 1

						filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(Ridx).DeviceID.ToString}
						idx = idx + 1
					End If

				End If

			Next

			Dim obj = New With {Key .app_id = "f877b4d2-b6e5-4595-a381-87165f6e46a0", Key .contents = New With {Key .en = Notification.Message}, Key .headings = New With {Key .en = "FlyPal"}, Key .filters = filterObject, Key .data = New With {Key .url = urlNotificationDetail.ToString}}

			'---------------------

			Dim param = serializer.Serialize(obj)
			Dim byteArray As Byte() = Encoding.UTF8.GetBytes(param)

			Dim responseContent As String = Nothing

			Try

				Using writer = request.GetRequestStream()
					writer.Write(byteArray, 0, byteArray.Length)
				End Using

				Using response As System.Net.HttpWebResponse = request.GetResponse()

					Using reader = New System.IO.StreamReader(response.GetResponseStream())

						responseContent = reader.ReadToEnd()

					End Using

				End Using

			Catch ex As System.Net.WebException
				System.Diagnostics.Debug.WriteLine(ex.Message)
				System.Diagnostics.Debug.WriteLine(New System.IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd())

				If errorcount <= 3 Then GoTo StartStep3

			End Try

			System.Diagnostics.Debug.WriteLine(responseContent)
		Next

	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mCurrencyList = CurrencyList.GetCurrencyList(, , True)
		cmbCurrencyList.DataSource = mCurrencyList
		Session("mCurrencyList") = mCurrencyList

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Indamer" Or AppSettings("ClientCode") = "7AR") Then     ' '7AR Clientcode Added By Saylee 14-Oct-2024            ''Added By Prashant 28-Jun-2010
			mVendorList = VendorList.GetVendortList(0, , , , , , True, , True, True)
		Else
			mVendorList = VendorList.GetVendortList(0, , , , , , True, , True)
		End If
		cmbVendorList.DataSource = mVendorList
		Session("mVendorList") = mVendorList

		mCustomerList = VendorList.GetVendortList(0, , , , , , True, True)
		Session("mCustomerList") = mCustomerList
		mPriorityList = PriorityList.GetPriorityList(, , "")
		Session("mPriorityList") = mPriorityList

		dgOrderItems.DataSource = mOrder.OrderItems
		dgOrderTerms.DataSource = mOrder.OrderTerms
		calOrderDate.Text = mOrder.OrderDateFormatted
		If txtQuotationDate.Text = "" Then
			txtQuotationDate.Text = ""
		Else
			txtQuotationDate.Text = mOrder.QuotationDateFormatted
		End If
		txtQuotationDate.DataBind()
		txtOpeningLine.DataBind()
		dgOrderCharges.DataSource = mOrder.OrderCharges

		mPOTowards = POTowards.GetPOTowards("(SELECT)")
		cmbPOTowards.DataSource = mPOTowards
		dgItemAttachment.DataSource = mOrder.FileAttachments     'Sankalp 25-08-25
		ControlsDataBind()
	End Sub
	Private Sub ControlsDataBind()
		dgOrderItems.DataBind()
		dgOrderTerms.DataBind()
		dgOrderCharges.DataBind()

		upnlStatusName.DataBind()
		upnlOrderDetails.DataBind()
		upnlSupplierDetails.DataBind()
		upnlTotalAmount.DataBind()
		upnlGrandTotal.DataBind()
		upnlButtons.DataBind()
		upnldgItemAttachment.DataBind() 'Sankalp 25-08-25
		AddAttributesForGridControls()
	End Sub
	Private Sub OrderItemDataGrid()
		mPriorityList = PriorityList.GetPriorityList(, , "")
		Session("mPriorityList") = mPriorityList
		dgOrderItems.DataSource = mOrder.OrderItems
		dgOrderItems.DataBind()
		upnlOrderItems.Update()
		upnlTotalAmount.Update()
		upnlGrandTotal.Update()
		upnlTotalAmount.DataBind()
		upnlGrandTotal.DataBind()
		'upnldgItemAttachment.DataBind() 'Sankalp  25-08-25
		'DataBind()
		AddAttributesForGridControls()
	End Sub
	Private Sub OrderChargeDataGrid()
		dgOrderCharges.DataSource = mOrder.OrderCharges
		dgOrderCharges.DataBind()
		upnlOrderCharges.Update()
		upnlTotalAmount.Update()
		upnlGrandTotal.Update()
		upnlTotalAmount.DataBind()
		'upnldgItemAttachment.DataBind() 'Sankalp  25-08-25
		upnlGrandTotal.DataBind()
	End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "calOrderDate" Then
			If calOrderDate.Text.ToString = "" Then
				custValidator.ErrorMessage = "Select Order Date."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtQuotationDate" Then
			If txtQuotationDate.Text.ToString <> "" Then
				If Not IsDate(txtQuotationDate.Text) Then
					custValidator.ErrorMessage = "Select Quotation Date."
					e.IsValid = False
				End If
			End If
		ElseIf custValidator.ControlToValidate = "cmbVendorList" Then
			If cmbVendorList.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Select Supplier from the list."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "cmbCurrencyList" Then
			If cmbCurrencyList.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Select Currency from the List."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtOrderRemark" Then
			If txtOrderRemark.Text.Length > 500 Then
				custValidator.ErrorMessage = "Remark is too long."
				e.IsValid = False
			End If
		End If
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		getSession()
		addAttributes()
		SetControlStatus(mOrder.StatusID)

		EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 19-July-2011
		mOpenFrom = Request.QueryString("Type") 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
		If CType(Session("PendingQuotationItems"), String) = "True" Then
			AddPurchaseQuotationParts()
			Session("PendingQuotationItems") = "False"
		Else
			Session("PendingQuotationItems") = "False"
		End If

		If CType(Session("SalesOrderItems"), String) = "True" Then
			AddSalesOrderPartsForPurchaseOrder()
			Session("SalesOrderItems") = "False"
		Else
			Session("SalesOrderItems") = "False"
		End If

		'Added by vikrant For New Requisition
		'If CType(Session("AddPart"), String) = "True" Then
		'    AddPart()
		'    Session("AddPart") = "False"
		'    Session("AddRequisitionParts") = "False"
		'Else
		'    Session("AddPart") = "False"
		'    Session("AddRequisitionParts") = "False"
		'End If
		'End

		If Not IsPostBack And Session("sender") = "" Then
			'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
			If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Session("TransText_ForTransSeries") IsNot Nothing) Then
				If Session("sender") = "IssueCreate" Then
					'
				Else
					If mOrder.IsNew Then

						mOrder.Text = Session("TransText_ForTransSeries")
						txtText.Text = mOrder.Text

						'mOrder.No = Session("TransNo_ForTransSeries")

						Session("mOrder") = mOrder

						Session("AddTransTextSeries") = "False"

						Session.Remove("TransName_ForTransSeries")
						Session.Remove("TransText_ForTransSeries")
						Session.Remove("TransNo_ForTransSeries")
					End If
				End If
			End If
			'End

			DataFieldBind()
			'New Addition By Deven Sir to Solve Bug No:-PO_O_16
			If mOrder.StatusID = 1 And mOrder.IsNew = False Then
				lblStatus.Text = "OPENED"
			End If
		End If
		SetPage()
		ControlVisibility()
		If mOrder.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012
			SetChargeGrid()
		End If
		SetControlStatusAfterAmendOrder(mOrder.StatusID)
		AddAttributesForGridControls()
		If Session("RedirectFromTransSeries") = "RedirectFromTransSeries" Then
			Session.Remove("RedirectFromTransSeries")
			AutoIssueCreation()
		End If
		If AppSettings("ClientCode") = "7AR" Then 'You can enable or disable the validator based on some condition in your code-behind:
			rfvAircraft.Enabled = True    'RequiredFieldValidator
		Else
			rfvAircraft.Enabled = False    'RequiredFieldValidator
		End If
	End Sub
	Private Sub SetModelID()
		If (mOrder.OrderItems.Count <> 0) Then
			mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
		End If
	End Sub
	Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		If IsValid = False Then upnlValidationsummary.Update() : Exit Sub
		If cmbCurrencyList.SelectedIndex > 0 Then
			mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
			If (mUser.IsCurrencywisePOLimit = True And mUser.UserCurrencywisePOLimits.Count > 0) Then
				If mUser.UserCurrencywisePOLimits.Contains(mIsApplicable:=True, mCurrencyID:=New Guid(cmbCurrencyList.SelectedValue)) = False Then
					MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="You are not authorized user to create order in this currency. Select another currency.", ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
					Exit Sub
				End If
			End If
		End If
		setObject()
		setVendorDetails()
		Session("mOrder") = mOrder
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 1 Then  'New Purchase and Part(Direct) 
			mOrder.OrderItems.Add(mOrder.ID)
			SetModelID()
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		'New Purchase and Purchase Quotation OR         mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 2
		'Repair/Overhul Purchase against Quotation OR   mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 2
		'Rental/Lease Purchase and Quotation            mOrder.TransTypeID = 39 And mOrder.AgainstTypeID = 2
		If (mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 2) Or (mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 2) Or (mOrder.TransTypeID = 39 And mOrder.AgainstTypeID = 2) Then
			If (mOrder.OrderItems.Count <> 0) Then
				mOrder.OrderItems.CurrentItem.ModelID = Guid.Empty
			End If
			If (mOrder.OrderItems.Count = 0) Then
				mPrevTransID = Guid.Empty
			Else
				mPrevTransID = mOrder.OrderItems(mOrder.OrderItems.Count - 1).OrderItemQuotationItems(0).QuotationID '
			End If
			Session("mPrevTransID") = mPrevTransID
			Response.Redirect("wfPendingPurchaseQuotations_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 3 Then 'New Purchase and Approval Quots.
			If AppSettings("NewRequisition") = "True" Then 'Added By Prashant 23-Jul-2012
				Response.Redirect("wfApprovedQuotationItems_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
			Else
				Response.Redirect("wfMgtApprovedQuotationItems.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
			End If
		End If
		If (mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.AgainstTypeID = 5 Then    ' (Exchange, Overhaul, Repair) and From Stock.
			mOrder.OrderItems.Add(mOrder.ID)
			SetModelID()

			' Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
			'Added by Shital on 18-Oct-2019
			Dim str As String
			If mOrder.ExchangeOrderTypeID = 2 Then
				str = "openledgersame('wfRequisitionPartListForPurchaseOrder_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
			Else
				Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
			End If

		End If
		If mOrder.TransTypeID = 39 And mOrder.AgainstTypeID = 1 Then  'Purchase for Rentail / Lease and Part(Direct) 
			mOrder.OrderItems.Add(mOrder.ID)
			SetModelID()
			Response.Redirect("wfPartStockStatusList_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 4 Then  'Sales order for Purchase order added By Prashant 4-Feb-2010
			Response.Redirect("wfSalesOrderForPurchaseOrder_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		'Added by vikrant For New Requisition
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 6 Then  'New Purchase and Requistion Parts 
			mOrder.OrderItems.Add(mOrder.ID)
			SetModelID()
			Response.Redirect("wfRequisitionPartListForPurchaseOrder_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
		'End
		'Added By Vikrant On 04-Jan-2017 For ALL04012017
		If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 7 Then
			mOrder.OrderItems.Add(mOrder.ID)
			Response.Redirect("wfPendingEnquiryItemsForOrder_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
		End If
	End Sub
	Private Sub btnAddTerm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTerm.Click
		setObject()
		setVendorDetails()
		Session("mOrder") = mOrder
		Response.Redirect("wfOrderTerm_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&Type=1")
	End Sub
	Private Sub btnAddSupplierSpecificTerms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSupplierSpecificTerms.Click
		mVendorTerms = VendorTerms.GetVendorTerms(New Guid(cmbVendorList.SelectedValue), mOrder.TransTypeID, mOrder.ID.ToString, 1)
		Dim i As Integer = 0
		While i < mVendorTerms.Count
			If mOrder.OrderTerms.Contains(mVendorTerms.Item(i).TermID) = False Then
				mOrder.OrderTerms.Add(mOrder.ID)
				mOrder.OrderTerms.CurrentItem.Terms = mVendorTerms.Item(i).Terms
				mOrder.OrderTerms.CurrentItem.TermID = mVendorTerms.Item(i).TermID
			End If
			i = i + 1
		End While
		dgOrderTerms.DataSource = mVendorTerms
		dgOrderTerms.DataBind()
	End Sub
	Private Sub dgOrderItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgOrderItems.RowCommand
		Select Case e.CommandName
			Case "EditView"
				Dim Index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgOrderItems.PageIndex * dgOrderItems.PageSize
				Session("Edit") = True
				setObject()
				setVendorDetails()
				mOrder.OrderItems.CurrentIndex = Index
				If mOrder.OrderItems.Item(Index:=Index).IsScheduleExpenses = True Then
					mOrder.OrderItems.Item(Index:=Index).IsScheduleExpensesYes = True
					mOrder.OrderItems.Item(Index:=Index).IsScheduleExpensesNo = False
				Else
					mOrder.OrderItems.Item(Index:=Index).IsScheduleExpensesYes = False
					mOrder.OrderItems.Item(Index:=Index).IsScheduleExpensesNo = True
				End If
				Session("mOrder") = mOrder
				If mOrder.VendorID.Equals(Guid.Empty) Then
					Session("VendorName") = ""
				Else
					Session("VendorName") = mVendorList.Item(cmbVendorList.SelectedIndex).Name
				End If
				Response.Redirect("wfOrderItem_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx")
			Case "DeleteRecord"
				Dim Index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgOrderItems.PageIndex * dgOrderItems.PageSize
				DeleteRecord(Index)
			Case "ViewTechDirection" 'Added By Prashant 3-Dec-2018 YATA03122018
				Dim Index As Integer = CInt(e.CommandArgument)
				If mOrder.OrderItems.Item(Index:=Index).TechDirectionCount = 0 Then
					'Do nothing 
				Else
					mCompStatus = CompStatus.GetCompStatus(mOrder.OrderItems.Item(Index:=Index).CompStatusID, Guid.Empty, Today.ToString)
					Dim mrptTechDirection As rptTechDirection = rptTechDirection.GetTechDirection(mOrder.OrderItems.Item(Index:=Index).CompStatusID, 2, mCompStatus.RemovedOn.ToString) '2 for compoenent
					Dim mAssemblyList As AssemblyList
					mrptTechDirection.RemovalReason = mCompStatus.RemovalReasonName
					mrptTechDirection.RemovalDate = mCompStatus.RemovedOn

					mAssemblyList = AssemblyList.GetAssemblyList(1, mOrder.OrderItems.Item(Index:=Index).MachineID.ToString, mCompStatus.RemovedOn.ToString)
					mrptTechDirection.ATA = mCompStatus.ATAChapter
					mrptTechDirection.PartNo = mCompStatus.PartName
					mrptTechDirection.Description = mCompStatus.Description
					mrptTechDirection.SerialNo = mCompStatus.SerialNo
					mrptTechDirection.ModelName = mAssemblyList(0).ModelName
					mrptTechDirection.AircaftName = mAssemblyList(0).RegNo
					mrptTechDirection.AircaftSrNo = mAssemblyList(0).SerialNo
					mrptTechDirection.IsRemUnschedule = mCompStatus.IsRemUnschedule
					mrptTechDirection.TimeSinceNew = String.Join(", ", From c As CompStatusPeriod In mCompStatus.CompStatusPeriods Select New Period(c.PeriodID, c.CompRemovalValue, GetPeriodUnitID(c.PeriodID), CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)

					ReportDetail(Index:=Index)
					Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
					Dim da As New CSLA.Data.ObjectAdapter
					Dim ds As New dsTechDirection
					Dim mCompanyDetail As New CompanyDetail
					myReport = New crptTechDirection
					Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
						  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
						  mCompanyDetail.WebSite, "", mrptTechDirection.RemovalDateFormatted.ToString, "", "", "", "", AppSettings("Product Version"), _
						  AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
					ds.Clear()
					ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
					Dim mrptImage As rptImage = rptImage.GetImage(ds)
					da.Fill(ds, mrptImage)
					da.Fill(ds, mrptTechDirection)
					da.Fill(ds, Report)
					If ReportMaintenanceDetails.Count <= 0 Then 'If mrptTechDirection.TypeID = 1 or  Then
						ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(Guid.Empty, DoneOnDate:=mOrder.OrderItems.Item(Index:=Index).TechDirectionDate.ToString))
					End If
					da.Fill(ds, ReportMaintenanceDetails)

					myReport.SetDataSource(ds)
					Session("CrystalReport") = myReport
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
					If DirectCast(ReportMaintenanceDetails(0), Flypal.ReportMaintenanceDetail).MonitorType = "" Then
						ReportMaintenanceDetails = New ReportMaintenanceDetailList
						Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
					End If
				End If
			Case "ShowPartStatus"  'Added By Prashant on 19-Feb-2021 Heligo19022021
				Dim Index As Integer = CInt(e.CommandArgument)
				Dim mItemStatus As Item = Item.GetItem(mOrder.OrderItems.Item(Index:=Index).ItemID)
				Dim LinkID As Guid = mItemStatus.LinkID
				Dim Unit As String = mItemStatus.UnitName

				Dim mStockPartStatus As rptStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
				Dim mOnOrderPartStatus As rptOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
				Dim mReturnablePartStatus As rptReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
				Dim mTransitPartList As rptTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
				Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode"))

				Session("PartNo") = mItemStatus.Name
				Session("Description") = mItemStatus.Description
				Session("Unit") = Unit

				Session("mStockPartStatus") = mStockPartStatus
				Session("mOnOrderPartStatus") = mOnOrderPartStatus
				Session("mReturnablePartStatus") = mReturnablePartStatus
				Session("mTransitPartList") = mTransitPartList
				Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
				Session("LinkID") = LinkID
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenShowPartNoStatusWindow", "OpenShowPartNoStatusWindow();", True)
			Case "PartStatus"  'Added By Prashant on 9-Mar-2021 Heligo09032021
				Dim Index As Integer = CInt(e.CommandArgument)
				Dim mNameOfItem As String = mOrder.OrderItems.Item(Index:=Index).ItemName
				Session("FromPOItemName") = mNameOfItem
				Session("mOrderForPartStatus") = mOrder
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenPartStatusWindow", "OpenPartStatusWindow();", True)
		End Select
	End Sub
	Private Sub dgOrderTerms_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgOrderTerms.RowCommand
		Select Case e.CommandName
			Case "DeleteTerm"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgOrderItems.PageIndex * dgOrderItems.PageSize
				DeleteTerm(Index)
		End Select
	End Sub
	Private Sub dgOrderCharges_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgOrderCharges.RowCommand
		Select Case e.CommandName
			Case "EditCharge"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgOrderCharges.PageIndex * dgOrderCharges.PageSize
				Session("Edit") = True
				setObject()
				setVendorDetails()
				mOrder.OrderCharges.CurrentIndex = Index
				Session("mOrder") = mOrder
				Response.Redirect("wfOrderCharge_Ajax.aspx")
			Case "DeleteCharge"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgOrderCharges.PageIndex * dgOrderCharges.PageSize
				If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then 'Added By Prashant 28-jan-2014
					setObject()
					setVendorDetails()
				End If
				DeleteCharge(Index)
		End Select
	End Sub
	Private Sub cmbVendorList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVendorList.SelectedIndexChanged
		If cmbVendorList.Enabled = True Then
			setFocus(cmbVendorList)
		End If
		mOrder.Attention = mVendorList(cmbVendorList.SelectedIndex).ContactPerson
		'Added By Prashant 3-Jun-2011
		If AppSettings("LastOrderCurrency") = "True" Then
			'Added By Prashant 3-Jun-2011
			Dim mRecordOfLastOrder As RecordOfLastOrder = RecordOfLastOrder.GetRecordOfLastOrder(mOrder.TransTypeID, New Guid(cmbVendorList.SelectedValue).ToString)
			mOrder.CurrencyID = mRecordOfLastOrder(0).CurrencyID
			cmbCurrencyList.DataBind()
			txtConversionFactor.DataBind()
			mRecordOfLastOrder = Nothing
			'----------------------------
		End If
		If cmbVendorList.SelectedIndex > 0 Then
			Dim mVendorApprovalListForDue As VendorApprovalListForDue

			''Added By Saylee on 7-Jun-2022
			If AppSettings("ClientCode") = "BA" Then
				mVendorApprovals = VendorApprovals.GetVendorApprovalList(New Guid(cmbVendorList.SelectedValue))
				If mVendorApprovals.Count = 0 Then
					MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "No Approval Document(s) present for selected Vendor.", MsgBoxStyle.OkOnly, "")
					mVendorApprovals = Nothing
					Exit Sub
				End If
			End If
			'**********************************

			mVendorApprovalListForDue = VendorApprovalListForDue.GetVendorApprovalListForDue(mOrder.OrderDate.ToString, cmbVendorList.SelectedValue.ToString)
			For i As Integer = 0 To mVendorApprovalListForDue.Count - 1
				If (mVendorApprovalListForDue(i).RemainingDays < 0) Then
					Dim str As String = mVendorApprovalListForDue(i).Name
					MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Approval Document(s) " & str & " of selected vendor not valid for the Order Date.", MsgBoxStyle.OkOnly, "vendornotvalid")
					mVendorApprovalListForDue = Nothing
					Exit Sub
				End If
			Next
		End If
		setVendorDetails()
		Dim mOrderItem As OrderItem
		Dim k As Integer = 0
		For Each mOrderItem In mOrder.OrderItems
			With mOrderItem
				'GST Changes
				If AppSettings("IsGSTApplicable") = "True" And Not mOrder.VendorID.Equals(Guid.Empty) Then
					mVendor = Vendor.GetVendor(mOrder.VendorID)
					If mVendor.CountryName.ToUpper = "INDIA" And CDate(mOrder.OrderDateFormatted.ToString) >= CDate("01-Jul-2017") And mVendor.ClientCountryName.ToUpper.Equals("INDIA") Then
						mGSTPercentage = GSTPercentage.GetPercentage(mOrder.OrderDateFormatted.ToString, 1, .ItemID.ToString)
						If mGSTPercentage IsNot Nothing Then
							Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
							If Len(mVendor.StateCode) > 0 Then
								If mVendor.StateCode = mVendor.ClientStateCode Then
									.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
									.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
									.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
									.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
									.IGSTPercentage = 0
									.IGSTCAmount = 0
									.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
									mOrder.StateCode = mVendor.StateCode
									mOrder.ClientStateCode = mVendor.ClientStateCode
									mOrder.VendorCountry = mVendor.CountryName
									mOrder.Visibility = 1
								Else
									.IGSTPercentage = (mGSTPercentage.GSTPercentage)
									.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
									.CGSTPercentage = 0
									.SGSTPercentage = 0
									.CGSTCAmount = 0
									.SGSTCAmount = 0
									.TotalCAmount = .CAmount + .IGSTCAmount
									mOrder.StateCode = mVendor.StateCode
									mOrder.ClientStateCode = mVendor.ClientStateCode
									mOrder.VendorCountry = mVendor.CountryName
									mOrder.Visibility = 2
								End If
								.HSNACSCode = mtmpItem.HSNACSCode
							Else
								.CGSTPercentage = 0
								.SGSTPercentage = 0
								.CGSTCAmount = 0
								.SGSTCAmount = 0
								.IGSTPercentage = 0
								.IGSTCAmount = 0
								.HSNACSCode = ""
								mOrder.StateCode = mVendor.StateCode
								mOrder.ClientStateCode = mVendor.ClientStateCode
								mOrder.VendorCountry = mVendor.CountryName
								mOrder.Visibility = 3
							End If
						End If
					Else
						.CGSTPercentage = 0
						.SGSTPercentage = 0
						.CGSTCAmount = 0
						.SGSTCAmount = 0
						.IGSTPercentage = 0
						.IGSTCAmount = 0
						.HSNACSCode = ""
						mOrder.StateCode = mVendor.StateCode
						mOrder.ClientStateCode = mVendor.ClientStateCode
						mOrder.VendorCountry = mVendor.CountryName
						mOrder.Visibility = 3
					End If
				Else
					.CGSTPercentage = 0
					.SGSTPercentage = 0
					.CGSTCAmount = 0
					.SGSTCAmount = 0
					.IGSTPercentage = 0
					.IGSTCAmount = 0
					If AppSettings("HSNACSCodeVisibleInPartMaster") = "True" Then
						'Do nothing 
					Else
						.HSNACSCode = ""
					End If
					mOrder.Visibility = 3
				End If
				'End
			End With
			k = k + 1
		Next
		Session("mOrder") = mOrder
		ControlVisibility()
		'Commented by Prashant on 11-Jun-2020 ALL11062020
		'dgOrderItems.DataSource = mOrder.OrderItems
		'dgOrderItems.DataBind()
		'ControlVisibility()
		'End of Commented by Prashant on 11-Jun-2020 ALL11062020
		AddAttributesForGridControls()
		upnlOrderItems.Update()
	End Sub
	Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
		txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
		If cmbCurrencyList.Enabled = True Then
			setFocus(cmbCurrencyList)
		End If
		upnlValidationsummary.Update()
		If cmbCurrencyList.SelectedIndex > 0 Then
			mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
			If (mUser.IsCurrencywisePOLimit = True And mUser.UserCurrencywisePOLimits.Count > 0) Then
				If mUser.UserCurrencywisePOLimits.Contains(mIsApplicable:=True, mCurrencyID:=New Guid(cmbCurrencyList.SelectedValue)) = False Then
					MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="You are not authorized user to create order in this currency. Select another currency.", ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
					Exit Sub
				End If
			End If
		End If
	End Sub
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		setVendorDetails()
		setObject()
		setSession()
		If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
			Exit Sub
		End If
		If IsValid = True Then
			If CurrencyRightsBeforeSave() = False Then Exit Sub 'Added by Prashant
			If Save() Then
				If mOrder.TransTypeID = 5 And (mOrder.AgainstTypeID = 6 Or mOrder.AgainstTypeID = 3) Then  '6 For Requistion and 3 For Approved Quotation
					If (mOrder.AgainstTypeID = 6 Or mOrder.AgainstTypeID = 3) Then
						upnlStatusName.DataBind()
						upnlOrderDetails.DataBind()
						upnlSupplierDetails.DataBind()
						upnlTotalAmount.DataBind()
						upnlGrandTotal.DataBind()
						upnlButtons.DataBind()
						If mOrder.AgainstTypeID = 6 Then
							mPriorityList = PriorityList.GetPriorityList(, , "")
							Session("mPriorityList") = mPriorityList
							GridViewGSTColoumnsVisibility()
							dgOrderItems.DataSource = mOrder.OrderItems
							dgOrderItems.DataBind()
							upnlOrderItems.Update()
						End If
					Else
						ControlsDataBind()
					End If
					upnlStatusName.Update()
					upnlOrderDetails.Update()
					upnlSupplierDetails.Update()
					upnlTotalAmount.Update()
					upnlGrandTotal.Update()
					upnlButtons.Update()
					ControlVisibility()
				Else
					dgOrderTerms.DataSource = mOrder.OrderTerms
					dgOrderTerms.DataBind()
					dgOrderItems.DataSource = mOrder.OrderItems
					UpdatePanel()
					OrderItemDataGrid()
					OrderChargeDataGrid()
					ControlVisibility()
				End If
				'Sankalp 25-08-25
				dgItemAttachment.DataSource = mOrder.FileAttachments
				dgItemAttachment.DataBind()
				SetChargeGrid()
				AddAttributesForGridControls()
				MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
			End If
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Session("ToOpenOrderForRateChange") = ""
		Session("ToMakeAuthorizeButtonVisibleFalse") = ""
		Session("NotEqualsQty") = ""

		MarkLog(Util.Action.Close, mModuleName, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		If mOpenFrom IsNot Nothing AndAlso mOpenFrom = "FromReqItemStatusReport" Then 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
			RemoveSession()
			Response.Redirect("Index.aspx")
		End If
		setObject()
		setVendorDetails()
		If mOrder.IsDirty Then
			Session("IsValid") = "True"
			MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
		Else
			If mOrder.IsNew Then
				Session.Remove("mOrder")
			End If
			RemoveSession()
			Response.Redirect("Index.aspx")
		End If
	End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If


		If mOrder.StatusID < 2 Then 'Added by Saylee on 14-Nov-2022 to show watermark
			''Session("ShowWatermark") = "True"
			Session("ShowWatermark") = AppSettings("ShowWatermark")
		End If

		SetReport() 'Added By Prashant 16-Sep-2013 ALL16092013
		Dim Str1 As String
		Str1 = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
		Dim OrderPrintDetail As String = String.Empty 'Added By Prashant 15-Feb-2021 ALL15022021
		OrderPrintDetail = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mVendorList(mOrder.VendorID).Name & " Created By : " & mOrder.UserName
		MarkLog(Util.Action.Print, mModuleName, OrderPrintDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
	End Sub
	Private Sub btnAddCharges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharges.Click '=====================Added By Saylee on 29-Aug-2007============================
		setObject()
		setVendorDetails()
		mOrder.OrderCharges.Add(mOrder.ID)
		Session("mOrder") = mOrder
		If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
			MarkLog(Util.Action.New, mModuleName, "After Change Info. Adding Charges", Util.ErrorType.NoError, mOrder.ID, EventLogID)
		Else
			MarkLog(Util.Action.New, mModuleName, "Adding Charges", Util.ErrorType.NoError, mOrder.ID, EventLogID)
		End If
		Response.Redirect("wfOrderCharge_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx")
	End Sub
	Private Sub chkIsRoundOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsRoundOff.CheckedChanged
		Dim Child As OrderCharge
		For i As Integer = mOrder.OrderCharges.Count - 1 To 0 Step -1
			Child = mOrder.OrderCharges(i)
			If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
				mOrder.OrderCharges.Remove(Child)
			End If
		Next
		mOrder.IsRoundOff = chkIsRoundOff.Checked
		dgOrderCharges.DataSource = mOrder.OrderCharges
		dgOrderCharges.DataBind()
	End Sub

	Public Sub SetReport(Optional ByMail As Boolean = False,
						 Optional IsForDS As Boolean = False,
						 Optional IsPROCUREMENTANDPAYMENTFORM As Boolean = False) 'Added By Prashant 16-Sep-2013 ALL16092013

		Dim da As New CSLA.Data.ObjectAdapter
		Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

		If IsPROCUREMENTANDPAYMENTFORM = True Then 'Added By Prashant 28-Jan-2025
			rpt = New crptPROCUREMENTFORM
		Else
			If AppSettings("ClientCode") = "ASH" Then
				rpt = New crptOrderAshleyAviation
			Else
				If CDate(calOrderDate.Text) <= CDate("30-Jun-2017") Or mOrder.Visibility = 3 Then
					'Added By Vikrant on 2-July-2011 For FlyGer02072012
					If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
						rpt = New crptOrderDetailPortraitForFlyGeorgia
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "JA" Then
						rpt = New crptOrderDetailPortraitForJA
					ElseIf AppSettings("ClientCode") = "PTW" Then 'Added By Prashant on 1-Jul-2024
						rpt = New crptOrderDetailPortraitForPattaya

					Else
						If mOrder.TransTypeID = 5 Then
							'rpt = New crptOrder
							If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
								rpt = New crptOrderDetailPortraitForInd
							ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
								rpt = New crptOrderDetailPortraitForHeligo
							ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "HL" Then
								rpt = New crptOrderDetailPortraitForHL
								'Added By Shweta On 5th Feb-2013 for YA04022013-1
							ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
								rpt = New crptOrderDetailPortraitForYA
							ElseIf (AppSettings("ClientCode") = "CGA") Then
								rpt = New crptOrderDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
							ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
								rpt = New crptOrderDetailPortraitBA 'Added By Prashant On 30-Oct-2014  BA30102014
							ElseIf (AppSettings("ClientCode") = "MID") Then
								rpt = New crptOrderDetailPortraitForMidex
							ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
								rpt = New crptOrderDetailPortraitForGEP
							ElseIf (AppSettings("ClientCode") = "LAMA") Then
								rpt = New crptOrderDetailPortraitLAMA
							ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
								rpt = New crptOrderDetailPortraitForHeliStar
							ElseIf AppSettings("ClientCode") = "ARA" Then
								rpt = New crptOrderDetailPortraitForARAirWays
							ElseIf AppSettings("ClientCode") = "KAS" Then 'Added By Prashant on 27-Jan-2025
								rpt = New crptOrderDetailPortraitKasas
							Else
								rpt = New crptOrderDetailPortrait
							End If
						ElseIf mOrder.TransTypeID = 31 Then
							'rpt = New crptOrderExchOH
							If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
								rpt = New crptOrderExchOHDetailPortraitForInd
							ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
								rpt = New crptOrderExchOHDetailPortraitForHeligo
							ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
								rpt = New crptOrderExchOHDetailPortraitForDeccan
								'Added By Shweta On 5th Feb-2013 for YA04022013-1
							ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
								rpt = New crptOrderExchOHDetailPortraitForYA
							ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then  'Added By Prashant On 31-Jul-2014  BA31072014
								rpt = New crptOrderExchOHDetailPortraitBA
							ElseIf (AppSettings("ClientCode") = "CGA") Then
								rpt = New crptOrderExchOHDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
							ElseIf (AppSettings("ClientCode") = "MID") Then
								rpt = New crptOrderExchOHDetailPortraitForMidex
							ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
								rpt = New crptOrderExchOHDetailPortraitForGEP
							ElseIf (AppSettings("ClientCode") = "LAMA") Then
								rpt = New crptOrderExchOHDetailPortraitLAMA
							ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
								rpt = New crptOrderExchOHDetailPortraitForHeliStar
							ElseIf AppSettings("ClientCode") = "ARA" Then
								rpt = New crptOrderDetailPortraitForARAirWays
							Else
								rpt = New crptOrderExchOHDetailPortrait
							End If
						ElseIf mOrder.TransTypeID = 38 Then
							If mOrder.IsOverhaul = True Then
								'rpt = New crptOrderExchOH
								If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
									rpt = New crptOrderExchOHDetailPortraitForInd
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
									rpt = New crptOrderExchOHDetailPortraitForHeligo
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
									rpt = New crptOrderExchOHDetailPortraitForDeccan
									'Added By Shweta On 5th Feb-2013 for YA04022013-1
								ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
									rpt = New crptOrderExchOHDetailPortraitForYA
								ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then 'Added By Prashant On 31-Jul-2014  BA31072014
									rpt = New crptOrderExchOHDetailPortraitBA
								ElseIf (AppSettings("ClientCode") = "CGA") Then
									rpt = New crptOrderExchOHDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
								ElseIf (AppSettings("ClientCode") = "MID") Then
									rpt = New crptOrderExchOHDetailPortraitForMidex
								ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
									rpt = New crptOrderExchOHDetailPortraitForGEP
								ElseIf (AppSettings("ClientCode") = "LAMA") Then
									rpt = New crptOrderExchOHDetailPortraitLAMA
								ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
									rpt = New crptOrderExchOHDetailPortraitForHeliStar
								ElseIf AppSettings("ClientCode") = "ARA" Then
									rpt = New crptOrderDetailPortraitForARAirWays
								Else
									rpt = New crptOrderExchOHDetailPortrait
								End If
							Else
								If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
									rpt = New crptOrderWOForInd
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
									rpt = New crptOrderWOForHeligo
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
									rpt = New crptOrderWOForDeccan
									'Added By Shweta On 5th Feb-2013 for YA04022013-1
								ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
									rpt = New crptOrderWOForYA
								ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then 'Added By Prashant On 31-Jul-2014  BA31072014
									rpt = New crptOrderWOBA
								ElseIf (AppSettings("ClientCode") = "CGA") Then
									rpt = New crptOrderWOForChhattisgarh    'Added By Prashant On 26-Aug-2014  CGA26082014
								ElseIf (AppSettings("ClientCode") = "MID") Then
									rpt = New crptOrderExchOHDetailPortraitForMidex
								ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
									rpt = New crptOrderWOForGEP
								ElseIf (AppSettings("ClientCode") = "LAMA") Then
									rpt = New crptOrderWOLAMA
								ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
									rpt = New crptOrderWOForHeliStar
								ElseIf AppSettings("ClientCode") = "ARA" Then
									rpt = New crptOrderDetailPortraitForARAirWays
								Else
									rpt = New crptOrderWO
								End If
							End If
						ElseIf mOrder.TransTypeID = 39 Then
							'rpt = New crptOrder
							If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
								rpt = New crptOrderDetailPortraitForInd
							ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
								rpt = New crptOrderDetailPortraitForHeligo
							ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "HL" Then
								rpt = New crptOrderDetailPortraitForHL
								'Added By Shweta On 5th Feb-2013 for YA04022013-1
							ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
								rpt = New crptOrderDetailPortraitForYA
							ElseIf (AppSettings("ClientCode") = "CGA") Then
								rpt = New crptOrderDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
							ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
								rpt = New crptOrderDetailPortraitBA 'Added By Prashant On 30-Oct-2014  BA30102014
							ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
								rpt = New crptOrderDetailPortraitForGEP
							ElseIf (AppSettings("ClientCode") = "LAMA") Then
								rpt = New crptOrderDetailPortraitLAMA
							ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
								rpt = New crptOrderDetailPortraitForHeliStar
							Else
								rpt = New crptOrderDetailPortrait
							End If
						End If
					End If
				Else
					rpt = New crptOrderGSTDetail
				End If
			End If
		End If

		Dim obj As rptOrders
		Dim objChilds As rptOrderChields
		Dim letter As rptLetterHead
		Dim ds As New dsOrder
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		obj = rptOrders.GetOrders(mOrder.ID)
		objChilds = rptOrderChields.GetOrderChields(mOrder.ID)
		'Added By Utkarsh(SearchStr1 Parameter Value) ON 15-May-2013 FOR All13052013-1

		If mOrder.AuthorizedBy = "" Then 'Added By Prashant 30-Aug-2019 For Heli Star
			EmployeeName = ""
		Else
			EmployeeName = SI.UTILITY.User.GetUser(mOrder.AuthorizedBy).EmpNoName
		End If 'End of Added By Prashant 30-Aug-2019 For Heli Star

		Dim mEmployeeInfoFromUser As User

		If obj.Count > 0 Then
			If obj(0).CreatedBy = "" Then 'Created By
				'Do nothing
			Else
				mEmployeeInfoFromUser = SI.UTILITY.User.GetUser(obj(0).CreatedBy) 'Created By
			End If
		End If

		If CBool(AppSettings("ShowKitItems")) Then
			mListOfKitItemsForOrderItem = ListOfKitItemsForOrderItem.GetListOfKitItemsForOrderItems(mOrder.ID)
			da.Fill(ds, mListOfKitItemsForOrderItem)
			ListOfKitItemsForOrderItemCount = mListOfKitItemsForOrderItem.Count.ToString
		End If

		letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), EmployeeName,
													 ListOfKitItemsForOrderItemCount, AppSettings("Logo"),
													 SearchString3:=AppSettings("AdvancePayment"),
													 ClientCode:=AppSettings("ClientCode"),
													 SearchString4:=AppSettings("HSNACSCodeVisibleInPartMaster"),
													 SearchString5:=mOrder.OrderItems.Count.ToString,
													 SearchString6:=txtOrderConfirmationNo.Text.Trim,
													 SearchString7:=mEmployeeInfoFromUser.EmployeeName,
													 SearchString8:=mEmployeeInfoFromUser.EmployeeEmail,
													 SearchString9:=mEmployeeInfoFromUser.EmployeePhoneNo)

		If letter.Count > 0 Then
			BaseCurrencysymbol = letter(0).BaseCurrencysymbol
			Session("BaseCurrencysymbol") = BaseCurrencysymbol
		End If

		da.Fill(ds, obj)
		da.Fill(ds, objChilds)
		da.Fill(ds, letter)
		da.Fill(ds, mrptImage)
		rpt.SetDataSource(ds)

		Session("CrystalReport") = rpt


		If ByMail Then

			Dim OrderNo As String = $"{mOrder.Text} - {mOrder.No} {IIf(mOrder.Amend = "", "", "-" + mOrder.Amend)}"
			Dim Subject As String = $"{letter(0).Name} Order No:- {OrderNo}"
			Dim Details As New Dictionary(Of String, String) From {
				{"Order No", $"{OrderNo}"},
				{"Order Date", mOrder.OrderDateFormatted}
			}
			Dim MailBody As String = ReportHelper.GenerateEmailBody(Details:=Details,
																	ModuleName:="Purchase Order",
																	AuthorizedBy:=Thread.CurrentPrincipal.Identity.Name,
																	AuthorizationDate:=New SmartDate(Today.Date).FormattedText)

			SendMailFile.SendMailFile(rpt:=Session("CrystalReport"),
									  UserName:=User.Identity.Name,
									  Subject:=$"{Subject}",
									  Text:=$"{OrderNo}",
									  Info:=MailBody,
									  VendorEmailID:="",
									  ToMailID:=Session("ToSendMailIDs"),
									  CCMailID:=Session("CcSendMailIDs"),
									  ReportPath:="",
									  ReportByMail:=True,
									  Remark:=Session("SendMailRemark"),
									  ReportGeneratedBy:=Session("ReportGenratedBy"),
									  SmtpHost:=Session("SmtpHost"),
									  SmtpPort:=Session("SmtpPort"),
									  SmtpUser:=Session("SmtpUser"),
									  SmtpPassword:=Session("SmtpPassword"))

		End If


		'Added By Prashant on 4-Jun-2024
		If IsForDS = True Then
			Dim myExportOption As CrystalDecisions.Shared.ExportOptions
			Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions
			Dim myFile As String
			Dim n As New Random
			Dim fs As FileStream
			Dim br As BinaryReader
			Dim imgbyte As Byte()
			myFile = "C:\Temp\Rep" + "Order Report " + Now.ToString.Replace(":", " ") + ".PDF"

			Session("myFile") = myFile

			myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
			myDiskOption.DiskFileName = myFile
			myExportOption = rpt.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = .ExportDestinationType.DiskFile
				.ExportFormatType = .ExportFormatType.PortableDocFormat
			End With
			rpt.Export()
			rpt.Close()
			rpt.Dispose()

			br = Nothing
			imgbyte = Nothing

			If IsForDS = False Then

				Response.ClearContent()
				Response.ClearHeaders()
				Response.ContentType = "application/pdf"
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + myFile)
				Response.WriteFile(myFile)
				Response.Flush()

			End If

			If fs IsNot Nothing Then
				fs.Dispose()
			End If
			Try
				' EventLogUtil.EventLogSave(mEventLogSession.ID, mEventLogSession.UserName, mEventLogSession.Password, mEventLogSession.IPAddress, mEventLogSession.MachineName, Now.Date.ToString, "Export To PDF", "Crew Profile Report", "Export To PDF Crew Profile Report", "", "")
			Catch ex As Exception
			End Try
		End If
		'End of Added By Prashant on 4-Jun-2024
	End Sub
	'Public Sub SendMail()
	'    If AppSettings("MailsRequire") = "True" Then
	'        SetReport()
	'        Dim str As String
	'        If mOrder.StatusID = 2 Then
	'            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">New Order No. <b> " & mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend) & "</b> Dated : <b>" + mOrder.OrderDateFormatted + "</b> To Supplier : <b>" + mOrder.VendorName + "</b> of Value : <b>" + "(" + Session("BaseCurrencysymbol") + ") " + mOrder.CGrandTotal.ToString + "</b> has been Authorized By User : <b>" + User.Identity.Name + " </b> on : <b>" + New SmartDate(Today.Date).FormattedText + "</b>,</font></P> ")
	'        ElseIf mOrder.StatusID = 4 Then
	'            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Order No. <b> " & mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend) & "</b> Dated : <b>" + mOrder.OrderDateFormatted + "</b> To Supplier : <b>" + mOrder.VendorName + "</b> of Value : <b>" + "(" + Session("BaseCurrencysymbol") + ") " + mOrder.CGrandTotal.ToString + "</b> has been Canceled By User : <b>" + User.Identity.Name + " </b> on : <b>" + New SmartDate(Today.Date).FormattedText + "</b>,</font></P> ")
	'        End If
	'        str = str + ("</body></html>")
	'        Session.Remove("BaseCurrencysymbol")
	'        SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Order Details", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), Info:=str, Remark:=Session("SendMailRemark"), ReportGenratedBy:=Session("ReportGenratedBy"))
	'    End If
	'End Sub
	Private Sub calOrderDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calOrderDate.TextChanged 'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
		mOrder = Session("mOrder")
		mOrder.OrderDate = calOrderDate.Text
		txtText.Text = mOrder.Text
		txtText.DataBind()
		Session("mOrder") = mOrder
	End Sub 'End
	Protected Sub btnChangeRate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChangeRate.Click
		If IsValid Then
			MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Change Info. And Authorize The Order.", MsgBoxStyle.OkOnly, "")
			Session("ToMakeAuthorizeButtonVisibleFalse") = ""
			Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange"
			Session("RateChangeEventLog") = "RateChangeEventLog"
			SetControlStatus(mOrder.StatusID)
			ControlVisibility()

			SetControlStatusAfterAmendOrder(mOrder.StatusID)
			MarkLog(Action.Authorize, mModuleName, "User : " & User.Identity.Name & " changed rate", ErrorType.NoError, mOrder.ID, EventLogID)
			upnlOrderItems.Update()
			Exit Sub
		End If
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub
	'Added By Vikrant On 20-Oct-2014 For BA20102014
	Private Sub btnShopWorkOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShopWorkOrder.Click
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		Dim da As New CSLA.Data.ObjectAdapter
		Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		rpt = New crptShopWorkOrder

		Dim ds As New dsOrder
		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		Dim mShopWorkOrderList As ShopWorkOrderList
		mShopWorkOrderList = ShopWorkOrderList.GetShopWorkOrderList(mOrder.ID.ToString)

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
		mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
		mCompanyDetail.WebSite, "Shop Work Order", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

		da.Fill(ds, mShopWorkOrderList)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)

		rpt.SetDataSource(ds)
		Session("CrystalReport") = rpt
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
	End Sub
	'End
	'Added By Vikrant On 23-Dec-2014 For All23122014-2
	'Commented by Sankalp 25-08-25
	'Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
	'    mOrder.IsAttachmentAdded = True
	'    ControlVisibilityForAttachment()
	'    upnlFileupload.Update()
	'End Sub
	'Commented by Sankalp 25-08-25
	'Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
	'    Dim fileSize1 As Integer = 0
	'    Dim file1(fileSize1) As Byte

	'    If mOrder.IsAttachmentAdded And mFileAttach Is Nothing Then
	'        mFileAttach = FileAttach.GetAttachment(mOrder.ID)
	'    End If

	'    mFileAttach.ImageFile = file1
	'    mFileAttach.Size = 0

	'    ImageButton1.Visible = False
	'    btnDelAttach.Enabled = False
	'    IsAttachmentDeleted = True
	'    mOrder.IsAttachmentAdded = False
	'    Session("IsAttachmentDeleted") = IsAttachmentDeleted
	'End Sub
	'Commented by Sankalp 25-08-25
	'Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
	'    ViewImage()
	'End Sub
	'Commented by Sankalp 25-08-25
	'Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
	'    If mOrder.IsAttachmentAdded Then
	'        mFileAttach = FileAttach.GetAttachment(mOrder.ID)
	'    Else
	'        mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mOrder.ID)
	'    End If
	'    Session("mFileAttach") = mFileAttach
	'End Sub
	'End
	Private Sub chkIsPBHPurchase_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsPBHPurchase.CheckedChanged
		If chkIsPBHPurchase.Checked = True Then
			setObject()
			setVendorDetails()
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMSPAssemblySelectionWindow", "OpenMSPAssemblySelectionWindow();", True)
		ElseIf chkIsPBHPurchase.Checked = False Then
			mOrder.MSPID = Guid.Empty
			mOrder.MSPAssemblyID = Guid.Empty
			mOrder.AssemblyName = ""
			mOrder.PlanName = ""
			mOrder.ContractNo = ""
			mOrder.MSPPORemark = ""
			Session("mOrder") = mOrder
			lblContractNo.DataBind()
			upnlSupplierDetails.Update()
		End If
	End Sub
	Private Sub hdnBtnMSPAssemblySelection_Click(sender As Object, e As EventArgs) Handles hdnBtnMSPAssemblySelection.Click
		If mOrder.MSPID.Equals(Guid.Empty) And chkIsPBHPurchase.Checked = True Then
			chkIsPBHPurchase.Checked = False
		End If
		lblContractNo.DataBind()
		upnlSupplierDetails.Update()
	End Sub
	Private Sub btnPrintPROCUREMENTANDPAYMENTFORM_Click(sender As Object, e As EventArgs) Handles btnPrintPROCUREMENTANDPAYMENTFORM.Click
		SetReport(IsPROCUREMENTANDPAYMENTFORM:=True)
		Dim Str1 As String
		Str1 = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
		Dim OrderPrintDetail As String = String.Empty
		OrderPrintDetail = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mVendorList(mOrder.VendorID).Name & " Created By : " & mOrder.UserName
		MarkLog(Util.Action.Print, mModuleName, OrderPrintDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
	End Sub

	Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
		Dim mUser As User
		mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
		' Session("UserEmailID") = mUser.UserEmail + IIf(mUser.ManagerEmail <> "", "," + mUser.ManagerEmail, "")
		Session("UserEmailID") = mTransactionList.Item(mOrder.TransTypeID).SendToMailID
		Session("MailsRequire") = mTransactionList.Item(mOrder.TransTypeID).MailsRequire
		Session("SmtpHost") = mTransactionList.Item(mOrder.TransTypeID).SmtpHost
		Session("SmtpPort") = mTransactionList.Item(mOrder.TransTypeID).SmtpPort
		Session("SmtpUser") = mTransactionList.Item(mOrder.TransTypeID).SmtpUser
		Session("SmtpPassword") = mTransactionList.Item(mOrder.TransTypeID).SmtpPassword
		Session("FormRevisionNo") = mTransactionList.Item(mOrder.TransTypeID).FormRevisionNo
		Session("FormRevisionDate") = mTransactionList.Item(mOrder.TransTypeID).FormRevisionDate

		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", "OpenByMaiWindow();", True)
	End Sub
	Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
		Try
			SetReport(True)
			'SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Order Details", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), _
			'                          "", "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
			'                          ReportGenratedBy:=Session("ReportGenratedBy"))
			'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)
		Catch ex As Exception
			Dim Day, Month, Year As String
			Day = Format(Today.Date.Day, "0#")
			Month = Format(Today.Date.Month, "0#")
			Year = Format(Today.Date.Year, "0#")
			Dim todaydate As String = Day & Month & Year
			Dim Path As String = AppSettings("DOCPath") & todaydate
			FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
			FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
			FileClose(1)
		End Try
	End Sub
#End Region

#Region " Currency And Limits Rights"
	Private Function CurrencyRightsBeforeSave() As Boolean
		setObject()
		setVendorDetails()
		mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
		If (mUser.IsCurrencywisePOLimit = True And mUser.UserCurrencywisePOLimits.Count > 0) Then
			'If mOrder.CGrandTotal > mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit Then
			If mUser.UserCurrencywisePOLimits.Contains(mIsApplicable:=True, mCurrencyID:=mOrder.CurrencyID) = False Then
				MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="You are not authorized user to create order in this currency.", ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
				Return False
			End If
			''End If
			'If mOrder.StatusID = 2 Then
			'    If (mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit > 0 And mOrder.CGrandTotal > mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit) Then
			'        MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="Order amount exceeded limit set for your user.<BR>You are not authorized user to create order greater than " + cmbCurrencyList.SelectedItem.Text + " " + mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit.ToString, ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
			'        Return False
			'    End If
			'End If
		End If
		Return True
	End Function
	Private Function CurrencyRightsAndLimitBeforeAutorized() As Boolean
		setObject()
		setVendorDetails()
		mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
		If (mUser.IsCurrencywisePOLimit = True And mUser.UserCurrencywisePOLimits.Count > 0) Then
			'If mOrder.CGrandTotal > mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit Then
			If mUser.UserCurrencywisePOLimits.Contains(mIsApplicable:=True, mCurrencyID:=mOrder.CurrencyID) = False Then
				MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="You are not authorized user to create order in this currency.", ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
				Return False
			End If
			'End If
			'If mOrder.StatusID = 2 Then
			If (mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit > 0 And mOrder.CGrandTotal > mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit) Then
				MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="Order amount exceeded limit set for your user.<BR>You are not authorized user to create order greater than " + cmbCurrencyList.SelectedItem.Text + " " + mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit.ToString, ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
				Return False
			End If
			'End If
		End If
		Return True
	End Function
	Private Function LimitAfterAutorized() As Boolean
		setObject()
		setVendorDetails()
		mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
		If (mUser.IsCurrencywisePOLimit = True And mUser.UserCurrencywisePOLimits.Count > 0) Then
			If mOrder.StatusID = 2 Then
				If (mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit > 0 And mOrder.CGrandTotal > mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit) Then
					MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="Order amount exceeded limit set for your user.<BR>You are not authorized user to create order greater than " + cmbCurrencyList.SelectedItem.Text + " " + mUser.UserCurrencywisePOLimits.Item(mOrder.CurrencyID).Limit.ToString, ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
					Return False
				End If
			End If
		End If
		Return True
	End Function
#End Region

#Region " Status "
	Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
		If IsValid Then
			'If Not CustomValidate2() Then Exit Sub 'Added by Saylee on 28-Nov-2012 for ALL22112012
			If CurrencyRightsAndLimitBeforeAutorized() = False Then Exit Sub 'Added by Prashant
			setVendorDetails()
			'Added by Saylee on 24-Jul-2012
			If mVendorList(mOrder.VendorID).NotInUse = True Then
				If CDate(mVendorList(mOrder.VendorID).NotInUseDate) <= CDate(mOrder.OrderDate) Then
					'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Record can not be saved. <br><br> Supplier is not applicable since " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
					'Exit Sub
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Supplier is not applicable since " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + "\n" + "Select another Supplier from list or select date before " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " & try again", False), True)
					Exit Sub
				End If
			End If
			If mOrder.IsCustomer = True Then
				If mVendorList(mOrder.CustomerID).NotInUse = True Then
					If CDate(mVendorList(mOrder.CustomerID).NotInUseDate) <= CDate(mOrder.OrderDate) Then
						' MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Record can not be saved. <br><br> Customer is not applicable since " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + " <br><br> Select another Customer from list or select date before " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
						'Exit Sub
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Customer is not applicable since " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + "\n" + "Select another Customer from list or select date before " + mVendorList(mOrder.CustomerID).NotInUseDateFormatted + " & try again", False), True)
						Exit Sub
					End If
				End If
			End If
			'********************************
			If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
				''If Method() = False Then
				''    Session("ToMakeAuthorizeButtonVisibleFalse") = ""
				''    Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange"
				''    SetControlStatus(mOrder.StatusID)
				''    ControlVisibility()
				''    SetControlStatusAfterAmendOrder(mOrder.StatusID)
				''    upnlOrderItems.Update()
				''    Exit Sub
				''Else
				'Save()
				'MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Order Authorized With New Changes.", MsgBoxStyle.OkOnly, "OrderAuthorizedWithNewRate")
				'Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse"
				'Exit Sub
				''End If
				If Save() = True Then
					mReceiptCumInvoice = Session("InvoiceChargesAddedUdateFromOrder")
					If mReceiptCumInvoice Is Nothing Then
						'Do nothing
					ElseIf (mReceiptCumInvoice.Invoice.InvoiceCharges.Count > 0 And mOrder.TransTypeID = 31 And AppSettings("AddChargesInRCI") = "True") Then
						Session.Remove("InvoiceChargesAddedUdateFromOrder")
						MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, "Order Authorized With New Changes." & "<br>Receipt: <b>" + mReceiptCumInvoice.ReceiptNo.ToString & vbCrLf & "</b><br><b>Date: " + mReceiptCumInvoice.RecCumInvDateFormatted.ToString + "</b><br>also add or update with charges", MsgBoxStyle.OkOnly, "OrderAuthorizedWithNewRate")
						Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse"
						Exit Sub
					End If
					MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, "Order Authorized With New Changes.", MsgBoxStyle.OkOnly, "OrderAuthorizedWithNewRate")
					Session("ToMakeAuthorizeButtonVisibleFalse") = "ToMakeAuthorizeButtonVisibleFalse"
					Exit Sub
				Else
					Exit Sub
				End If
			End If
			MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> Purchase Order </Strong>", MsgBoxStyle.YesNo, "Status")
			Session("IsValid") = IsValid
			'mOrder.StatusID = 2
			GetCanceledQtyBeforeZero()
			Session("mOrder") = mOrder
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub btnAmend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAmend.Click
		Session("ToOpenOrderForRateChange") = "" 'Added By Prashant 28-Jan-2014
		If IsValid Then
			MSGBoxCtrl.show(MSGBox.Message_title.StatusAmended, MSGBox.Message_text.StatusAmended, "<Strong> Purchase Order </Strong>", MsgBoxStyle.YesNo, "AmendStatus")
			Session("IsValid") = IsValid
			Session("Amend") = "Yes"
			mOrder.StatusID = 3
			Session("mOrder") = mOrder
		End If
	End Sub
	Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
		Session("ToOpenOrderForRateChange") = "" 'Added By Prashant 28-Jan-2014
		If IsValid Then
			Dim IsInUse As IsInUse = IsInUse.GetIsInUseOrderINReceipt(mOrder.ID)
			If IsInUse.IsInUse Then
				'MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, "<Strong> Purchase Order, It is used in Receipt or Receipt-Cum-Invoice or Issue .</Strong>", MsgBoxStyle.OkOnly, "StatusCancel")
				'Session("mOrder") = mOrder
				'Exit Sub
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Purchase Order, It is used in Receipt or Goods Receipt or Issue.", False), True)
				Exit Sub
			End If
			MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Purchase Order </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
			Session("IsValid") = IsValid
			Session("mOrder") = mOrder
		End If
	End Sub
	Private Sub hdnBtnFileAttachmentAndOtherInfo_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileAttachmentAndOtherInfo.Click 'Added By Prashant On 3-Feb-2021 For BA03022021
		If Session("BackOrSaveFromwfFileAttachmentAndOtherInfo_Ajax") = "Back" Then
			'Do nothing
		Else 'From Save
			txtOrderRemark.Text = mOrder.Remark
			upnlOrderDetails.Update()
		End If
		Session.Remove("BackOrSaveFromwfFileAttachmentAndOtherInfo_Ajax")
	End Sub
#End Region

#Region " Show BrokenRules "
	Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		If Flag = 1 Then Exit Sub
		Dim CustValidator As CustomValidator
		CustValidator = CType(s, CustomValidator)
		Dim strMsg As String = ""
		GetCanceledQtyBeforeZero()
		setObject()
		setVendorDetails()
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
		If strMsg.Trim <> "" Then
			CustValidator.ErrorMessage = strMsg
			e.IsValid = False
		End If
		Flag = 1
	End Sub
	Public Function CustomValidate2() As Boolean 'Added by Saylee on 28-Nov-2012 for ALL22112012
		Dim strMsg As String = ""
		setObject()
		setVendorDetails()
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
		If strMsg <> "" Then
			CustValidator.ErrorMessage = strMsg
			CustValidator.IsValid = False
			Return False
		End If
		Return True
	End Function
#End Region

#Region " Add Multiple Parts "
	Private Sub AddPurchaseQuotationParts()
		Dim mPendingPurchaseQuotationItems As PendingPurchaseQuotationItems = Session("mPendingPurchaseQuotationItems")
		Dim PendingQuotationInfo As PendingPurchaseQuotationItem 'PendingPurchaseQuotationItems.PendingQuotationInfo
		If mPendingPurchaseQuotationItems IsNot Nothing Then

			For Each PendingQuotationInfo In mPendingPurchaseQuotationItems
				If PendingQuotationInfo.IsSelected Then
					If Not mOrder.OrderItems.Contains(PendingQuotationInfo.ItemID) Then

						mOrder.OrderItems.Add(mOrder.ID)

						With mOrder.OrderItems.CurrentItem
							mOrder.OrderItems.CurrentItem.ItemID = PendingQuotationInfo.ItemID
							mOrder.OrderItems.CurrentItem.UnitID = PendingQuotationInfo.UnitID
							mOrder.OrderItems.CurrentItem.UnitName = PendingQuotationInfo.UnitName 'Added By Vikrant On 22-Nov-2019 For ALL22112019
							mOrder.OrderItems.CurrentItem.ConversionFactor = mOrder.ConversionFactor 'Added by Prashant 20-Sep-2012 'ALL20092012-1
							mOrder.OrderItems.CurrentItem.CRate = PendingQuotationInfo.CRate
							mOrder.OrderItems.CurrentItem.RequisitionTextNo = PendingQuotationInfo.RequisitionTextNo
							mOrder.OrderItems.CurrentItem.HSNACSCode = PendingQuotationInfo.HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
							.OrderItemQuotationItems.Add(.ID, PendingQuotationInfo.QuotationItemID, PendingQuotationInfo.QuotationQty, PendingQuotationInfo.QuotationTextNo, PendingQuotationInfo.QuotationDate.ToString, PendingQuotationInfo.QuotationID)

							Dim mVendor As Vendor
							Dim mGSTPercentage As GSTPercentage
							If AppSettings("IsGSTApplicable") = "True" And Not mOrder.VendorID.Equals(Guid.Empty) Then
								mVendor = Vendor.GetVendor(mOrder.VendorID)
								If mVendor.CountryName.ToUpper = "INDIA" And CDate(mOrder.OrderDateFormatted.ToString) >= CDate("01-Jul-2017") And mVendor.ClientCountryName.ToUpper.Equals("INDIA") Then
									mGSTPercentage = GSTPercentage.GetPercentage(mOrder.OrderDateFormatted.ToString, 1, .ItemID.ToString)
									If mGSTPercentage IsNot Nothing Then
										Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
										If Len(mVendor.StateCode) > 0 Then
											If mVendor.StateCode = mVendor.ClientStateCode Then
												.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
												.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
												.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
												.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
												.IGSTPercentage = 0
												.IGSTCAmount = 0
												.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
												mOrder.StateCode = mVendor.StateCode
												mOrder.ClientStateCode = mVendor.ClientStateCode
												mOrder.VendorCountry = mVendor.CountryName
												mOrder.Visibility = 1
											Else
												.IGSTPercentage = (mGSTPercentage.GSTPercentage)
												.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
												.CGSTPercentage = 0
												.SGSTPercentage = 0
												.CGSTCAmount = 0
												.SGSTCAmount = 0
												.TotalCAmount = .CAmount + .IGSTCAmount
												mOrder.StateCode = mVendor.StateCode
												mOrder.ClientStateCode = mVendor.ClientStateCode
												mOrder.VendorCountry = mVendor.CountryName
												mOrder.Visibility = 2
											End If
											.HSNACSCode = mtmpItem.HSNACSCode
										Else
											.CGSTPercentage = 0
											.SGSTPercentage = 0
											.CGSTCAmount = 0
											.SGSTCAmount = 0
											.IGSTPercentage = 0
											.IGSTCAmount = 0
											.HSNACSCode = ""
											mOrder.StateCode = mVendor.StateCode
											mOrder.ClientStateCode = mVendor.ClientStateCode
											mOrder.VendorCountry = mVendor.CountryName
											mOrder.Visibility = 3
										End If
									End If
								Else
									.CGSTPercentage = 0
									.SGSTPercentage = 0
									.CGSTCAmount = 0
									.SGSTCAmount = 0
									.IGSTPercentage = 0
									.IGSTCAmount = 0
									.HSNACSCode = ""
									mOrder.StateCode = mVendor.StateCode
									mOrder.ClientStateCode = mVendor.ClientStateCode
									mOrder.VendorCountry = mVendor.CountryName
									mOrder.Visibility = 3
								End If
							Else
								.CGSTPercentage = 0
								.SGSTPercentage = 0
								.CGSTCAmount = 0
								.SGSTCAmount = 0
								.IGSTPercentage = 0
								.IGSTCAmount = 0
								If AppSettings("HSNACSCodeVisibleInPartMaster") = "True" Then
									'Do nothing 
								Else
									.HSNACSCode = ""
								End If
								mOrder.Visibility = 3
							End If
						End With
					Else
						'MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Purchase Order Part already taken for Purchase Order", MsgBoxStyle.OkOnly, "")
						'DataFieldBind()
						'Exit Sub
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Quotation Part already taken for Purchase Order", False), True)
						Exit Sub
					End If
				End If
			Next
		End If
	End Sub
	Private Sub AddSalesOrderPartsForPurchaseOrder() 'Added By Prashant 4-Feb-2010
		Dim mSalesOrderItemsForPurchaseOrder As SalesOrderItemsForPurchaseOrder = Session("mSalesOrderItemsForPurchaseOrder")

		Dim SalesOrderItemsForPurchaseOrderInfo As SalesOrderItemForPurchaseOrder

		If mSalesOrderItemsForPurchaseOrder IsNot Nothing Then

			For Each SalesOrderItemsForPurchaseOrderInfo In mSalesOrderItemsForPurchaseOrder
				If SalesOrderItemsForPurchaseOrderInfo.IsSelected Then
					If Not mOrder.OrderItems.Contains(SalesOrderItemsForPurchaseOrderInfo.ItemID) Then

						mOrder.OrderItems.Add(mOrder.ID)
						With mOrder.OrderItems.CurrentItem
							mOrder.OrderItems.CurrentItem.ItemID = SalesOrderItemsForPurchaseOrderInfo.ItemID
							mOrder.OrderItems.CurrentItem.UnitID = SalesOrderItemsForPurchaseOrderInfo.UnitID
							mOrder.OrderItems.CurrentItem.CRate = SalesOrderItemsForPurchaseOrderInfo.SalesOrderItemCRate
							mOrder.OrderItems.CurrentItem.ConversionFactor = mOrder.ConversionFactor 'Added by Prashant 20-Sep-2012 'ALL20092012-1
							mOrder.OrderItems.CurrentItem.FromNo = SalesOrderItemsForPurchaseOrderInfo.SalesOrderTextNo
							mOrder.OrderItems.CurrentItem.FromDate = SalesOrderItemsForPurchaseOrderInfo.SalesOrderDate
							mOrder.OrderItems.CurrentItem.HSNACSCode = SalesOrderItemsForPurchaseOrderInfo.HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
							mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.SalesOrder

							.OrderItemSalesOrderItems.Add(.ID, SalesOrderItemsForPurchaseOrderInfo.SalesOrderItemID, SalesOrderItemsForPurchaseOrderInfo.SalesOrderItemQty, SalesOrderItemsForPurchaseOrderInfo.SalesOrderTextNo)
						End With
					Else
						'MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Sales Order Part already taken for Purchase Order", MsgBoxStyle.OkOnly, "")
						'DataFieldBind()
						'Exit Sub
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Sales Order Part already taken for Purchase Order", False), True)
						Exit Sub
					End If
				End If
			Next
		End If
	End Sub
#End Region

#Region " Vendor Address "
	Private Sub VendorApprovalsGridBind()
		mVendorApprovals = VendorApprovals.GetVendorApprovalList(New Guid(cmbVendorList.SelectedValue))
		dgApprovalList.DataSource = mVendorApprovals
		dgApprovalList.DataBind()
	End Sub
	Private Sub dgApprovalList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgApprovalList.RowCommand
		Select Case e.CommandName
			Case "ViewRec"
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				Dim ID As Guid = New Guid(e.CommandArgument.ToString)
				mFileAttach = FileAttach.GetAttachment(ID)
				Session("mFileAttach") = mFileAttach
				If mFileAttach.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						Dim Str As String
						Str = "openFile();"
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
					End If
				End If
		End Select
	End Sub
	Private Sub btnAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddress.Click
		setObject()
		setVendorDetails()
		txtAddress.DataBind()
		txtAttention.DataBind()
		VendorApprovalsGridBind()
		upnlVendorDetails.Update()
		mdeVendorDetails.Show()
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		mOrder.Attention = txtAttention.Text
		mdeVendorDetails.Hide()
	End Sub
	Protected Sub btnVendorDetailsBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVendorDetailsBack.Click
		mdeVendorDetails.Hide()
	End Sub
#End Region

#Region "Ship Bill Details"
	Private Sub GetSessionOfShipBillDetails()
		mCustomerList = Session("mCustomerList")
		mBillToShipToTypeList = Session("mBillToShipToTypeList")
		mLocationList = Session("mLocationList")
		mShipToTypeList = Session("mShipToTypeList")
	End Sub
	Private Sub RemoveSessionOfShipBillDetails()
		Session.Remove("mCustomerList")
		Session.Remove("mBillToShipToTypeList")
		Session.Remove("mLocationList")
		Session.Remove("mShipToTypeList")
	End Sub
	Private Sub DataFieldBindOfShipBillDetails()
		mCustomerList = VendorList.GetVendortList(0, , , , , , True, True)
		cmbCustomerList.DataSource = mCustomerList
		Session("mCustomerList") = mCustomerList
		cmbCustomerList.DataBind()
		mBillToShipToTypeList = BillToShipToTypeList.GetBillToShipToTypeList()
		Session("mBillToShipToTypeList") = mBillToShipToTypeList
		cmbBillType.DataSource = mBillToShipToTypeList
		cmbBillType.DataBind()
		mShipToTypeList = BillToShipToTypeList.GetBillToShipToTypeList()
		Session("mShipToTypeList") = mShipToTypeList
		cmbShipType.DataSource = mBillToShipToTypeList
		cmbShipType.DataBind()
		mLocationList = LocationList.GetLocationList(0, , , , , , True)
		Session("mLocationList") = mLocationList
		cmbLocation.DataSource = mLocationList
		cmbLocation.DataBind()
		txtBillingAddress.DataBind()
		txtShippingAddress.DataBind()
	End Sub
	Private Sub SetObjectOfShipBillDetails()
		If mOrder.StatusID = 1 Then
			mOrder.BillToTypeID = CInt(cmbBillType.SelectedValue)
			mOrder.ShipToTypeID = CInt(cmbShipType.SelectedValue)
			mOrder.LocationID = New Guid(cmbLocation.SelectedValue)
			mOrder.CustomerID = New Guid(cmbCustomerList.SelectedValue)
			mOrder.BillingAddress = txtBillingAddress.Text
			mOrder.ShippingAddress = txtShippingAddress.Text
		End If
		If mOrder.StatusID = 2 Then  'Added By Prashant 12-Aug-2014  ALL05082014
			''mOrder.Save()  ''Commneted and added by Prashant on 5-Apr-2022
			mOrder.UpdateOrderShipBillDetails(mOrder.ID, CInt(cmbBillType.SelectedValue), CInt(cmbShipType.SelectedValue), New Guid(cmbLocation.SelectedValue), _
											  New Guid(cmbCustomerList.SelectedValue), txtBillingAddress.Text.Trim, txtShippingAddress.Text.Trim)
		End If
		Session("mOrder") = mOrder
	End Sub
	Private Sub btnShipBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShipBill.Click
		setObject()
		setVendorDetails()
		setSession()
		'If AppSettings("BillShipFromPrevOrder") = "True" Then
		'    If mOrder.IsNew Then
		'        Dim mRecordOfLastOrder As RecordOfLastOrder = RecordOfLastOrder.GetRecordOfLastOrder(mOrder.TransTypeID)
		'        mOrder.BillingAddress = mRecordOfLastOrder(0).BillingAddress
		'        mOrder.ShippingAddress = mRecordOfLastOrder(0).ShippingAddress
		'    End If
		'End If
		DataFieldBindOfShipBillDetails()
		upnlShipBillDetails.Update()
		mdeShipBillDetails.Show()
	End Sub
	Private Sub cmbLocation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLocation.SelectedIndexChanged
		GetSessionOfShipBillDetails()
		If (cmbBillType.SelectedIndex = 1) Or (cmbShipType.SelectedIndex = 1) Then
			cmbCustomerList.Enabled = True
		End If
		If cmbShipType.SelectedIndex = 2 Then
			txtShippingAddress.Text = mLocationList(cmbLocation.SelectedIndex).Address
		End If
		If cmbBillType.SelectedIndex = 2 Then
			txtBillingAddress.Text = mLocationList(cmbLocation.SelectedIndex).Address
		End If
		If (cmbShipType.SelectedIndex = 2) And (cmbBillType.SelectedIndex = 2) Then
			txtShippingAddress.Text = mLocationList(cmbLocation.SelectedIndex).Address
			txtBillingAddress.Text = mLocationList(cmbLocation.SelectedIndex).Address
		End If
		If cmbLocation.Enabled = True Then
			setFocus(cmbLocation)
		End If
	End Sub
	Private Sub cmbBillType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillType.SelectedIndexChanged
		GetSessionOfShipBillDetails()
		If (cmbBillType.SelectedIndex = 0) And (cmbShipType.SelectedIndex = 1 Or mOrder.IsCustomer = True) Then
			txtBillingAddress.Text = mOrder.BillingAddress
			cmbCustomerList.Enabled = True
			cmbLocation.SelectedIndex = 0
		ElseIf (cmbBillType.SelectedIndex = 0) And (cmbShipType.SelectedIndex = 2) Then
			txtBillingAddress.Text = mOrder.BillingAddress
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = True
			cmbCustomerList.SelectedIndex = 0
		ElseIf (cmbBillType.SelectedIndex = 0) And (cmbShipType.SelectedIndex = 0) Then
			txtBillingAddress.Text = mOrder.BillingAddress
			txtShippingAddress.Text = mOrder.ShippingAddress
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = False
			cmbCustomerList.SelectedIndex = 0
			cmbLocation.SelectedIndex = 0
		ElseIf cmbBillType.SelectedIndex = 0 Then
			txtBillingAddress.Text = mOrder.BillingAddress
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = False
		End If
		If (cmbBillType.SelectedIndex = 1 Or mOrder.IsCustomer = True) And (cmbShipType.SelectedIndex = 1 Or mOrder.IsCustomer = True) Then
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = False
			cmbLocation.SelectedIndex = 0
		ElseIf ((cmbBillType.SelectedIndex = 1 Or mOrder.IsCustomer = True) And (cmbShipType.SelectedIndex = 0)) Then
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = False
			cmbLocation.SelectedIndex = 0
		ElseIf (cmbBillType.SelectedIndex = 1 Or mOrder.IsCustomer = True) And (cmbShipType.SelectedIndex = 2) Then
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = True
		End If
		If (cmbBillType.SelectedIndex = 2) And (cmbShipType.SelectedIndex = 2) Then
			txtBillingAddress.Text = ""
			cmbLocation.Enabled = True
			cmbCustomerList.Enabled = False
			cmbCustomerList.SelectedIndex = 0
		ElseIf (cmbBillType.SelectedIndex = 2) And (cmbShipType.SelectedIndex = 0) Then
			txtBillingAddress.Text = ""
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = True
			cmbCustomerList.SelectedIndex = 0
		ElseIf (cmbBillType.SelectedIndex = 2) And (cmbShipType.SelectedIndex = 1 Or mOrder.IsCustomer = True) Then
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = True
		End If
		If cmbBillType.Enabled = True Then
			setFocus(cmbBillType)
		End If
	End Sub
	Private Sub cmbShipType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbShipType.SelectedIndexChanged
		GetSessionOfShipBillDetails()
		If (cmbShipType.SelectedIndex = 0) And (cmbBillType.SelectedIndex = 1 Or mOrder.IsCustomer = True) Then
			txtShippingAddress.Text = mOrder.ShippingAddress
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = False
			cmbLocation.SelectedIndex = 0
		ElseIf (cmbShipType.SelectedIndex = 0) And (cmbBillType.SelectedIndex = 2) Then
			txtShippingAddress.Text = mOrder.ShippingAddress
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = True
			cmbCustomerList.SelectedIndex = 0
		ElseIf (cmbShipType.SelectedIndex = 0) And (cmbBillType.SelectedIndex = 0) Then
			txtBillingAddress.Text = mOrder.BillingAddress
			txtShippingAddress.Text = mOrder.ShippingAddress
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = False
			cmbCustomerList.SelectedIndex = 0
			cmbLocation.SelectedIndex = 0
		ElseIf cmbShipType.SelectedIndex = 0 Then
			txtShippingAddress.Text = mOrder.ShippingAddress
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = False
		End If
		If (cmbShipType.SelectedIndex = 1 Or mOrder.IsCustomer = True) And (cmbBillType.SelectedIndex = 0) Then
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = False
			cmbLocation.SelectedIndex = 0
		ElseIf (cmbShipType.SelectedIndex = 1 Or mOrder.IsCustomer = True) And (cmbBillType.SelectedIndex = 1 Or mOrder.IsCustomer = True) Then
			txtShippingAddress.Text = mCustomerList(cmbCustomerList.SelectedIndex).Address
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = False
			cmbLocation.SelectedIndex = 0
		ElseIf (cmbShipType.SelectedIndex = 1 Or mOrder.IsCustomer = True) And (cmbBillType.SelectedIndex = 2) Then
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = True
			cmbCustomerList.SelectedIndex = 0
		End If
		If (cmbShipType.SelectedIndex = 2) And (cmbBillType.SelectedIndex = 2) Then
			txtShippingAddress.Text = ""
			cmbLocation.Enabled = False
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = True
			cmbCustomerList.SelectedIndex = 0
		ElseIf (cmbShipType.SelectedIndex = 2) And (cmbBillType.SelectedIndex = 0) Then
			txtShippingAddress.Text = ""
			cmbLocation.Enabled = False
			cmbCustomerList.Enabled = False
			cmbLocation.Enabled = True
			cmbCustomerList.SelectedIndex = 0
		ElseIf (cmbShipType.SelectedIndex = 2) And (cmbBillType.SelectedIndex = 1 Or mOrder.IsCustomer = True) Then
			cmbLocation.Enabled = True
			cmbCustomerList.Enabled = True
			cmbLocation.Enabled = True
		End If
		If cmbShipType.Enabled = True Then
			setFocus(cmbShipType)
		End If
	End Sub
	Private Sub cmbCustomerList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustomerList.SelectedIndexChanged
		GetSessionOfShipBillDetails()
		cmbCustomerList.Enabled = True
		If cmbBillType.SelectedIndex = 1 Then
			txtBillingAddress.Text = mCustomerList(cmbCustomerList.SelectedIndex).Address
		End If
		If cmbShipType.SelectedIndex = 1 Then
			txtShippingAddress.Text = mCustomerList(cmbCustomerList.SelectedIndex).Address
		End If
		If (mOrder.IsCustomer = True) And (cmbBillType.SelectedIndex = 1) And (cmbShipType.SelectedIndex = 1) Then
			txtBillingAddress.Text = mCustomerList(cmbCustomerList.SelectedIndex).Address
			txtShippingAddress.Text = mCustomerList(cmbCustomerList.SelectedIndex).Address
		End If
		If cmbCustomerList.Enabled = True Then
			setFocus(cmbCustomerList)
		End If
	End Sub
	Private Sub btnOkShipBillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOkShipBillDetails.Click
		SetObjectOfShipBillDetails()
		mdeShipBillDetails.Hide()
		RemoveSessionOfShipBillDetails()
	End Sub
	Private Sub btnBackShipBillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackShipBillDetails.Click
		mdeShipBillDetails.Hide()
		RemoveSessionOfShipBillDetails()
	End Sub
#End Region

#Region "Service Methods"

	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
		Dim str As String() = contextKey.Split("¿")
		Dim mTransTypeID As Integer = CInt(str(0).Substring(str(0).IndexOf("=") + 1))
		Dim mOrderDate As String = str(1).Substring(str(1).IndexOf("=") + 1)
		mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, , True, mTransTypeID, mOrderDate)
		If count = 0 Then
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		Else
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		End If
	End Function

#End Region

#Region "Report Maintenance Details"

	Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
	Dim mMachineList As MachineList
	Dim MaintenanceActivityType As String
	Dim DoneOnValue As String
	Dim AssemblyID As Guid
	Dim PeriodUnitName, FrequencyValue, DueOnValue, FrequencyValueFormatted, DoneOnValueFormatted, DueOnValueFormatted, CurrentValueFormatted, ElapsedValueFormatted, ExtensionValueFormatted, RemainingValueFormatted, MonitorInfo, DoneOn As String
	Dim mCompStatus As CompStatus

	Public Function ReportDetail(Optional ByVal Index As Integer = -1) As ReportMaintenanceDetailList
		Dim ObjMachine As MachineInfo
		Dim ObjAssemblyStatus As AssemblyStatusInfo
		Dim ObjCompStatus As CompStatusInfo
		Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
		Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
		Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
		Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo
		Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
		Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo

		Dim LogID As String = MachineMaintenance.GetMachineMaintenance(mOrder.OrderItems(Index).CompStatusID, 4).LogID.ToString
		mMachineList = MachineList.GetMachineListComplianceComponentsMonitoringStatusForRemovedComp(mOrder.OrderItems(Index).TechDirectionDate.ToString, , mOrder.OrderItems(Index).ItemName, , , , _
																									mOrder.OrderItems(Index).SerialNo, , , , , , , , , , , True, True, True, , , _
																									mOrder.OrderItems(Index).TechDirectionRegNo, LogID, SkipIsForInventoryAircarft:=True)

		For Each ObjMachine In mMachineList
			For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
				For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
					If ObjCompStatus.CompMonitorServiceStatusList.Count > 0 Then
						For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
							PeriodUnitName = ""
							FrequencyValue = ""
							DueOnValue = ""
							FrequencyValueFormatted = ""
							DoneOnValueFormatted = ""
							DueOnValueFormatted = ""
							MonitorInfo = ""
							DoneOn = ""
							CurrentValueFormatted = ""
							ElapsedValueFormatted = ""
							ExtensionValueFormatted = ""
							RemainingValueFormatted = ""

							MaintenanceActivityType = ObjCompMonitorServiceStatus.Type
							MonitorInfo = ObjCompMonitorServiceStatus.Code
							DoneOn = ObjCompMonitorServiceStatus.DoneOnFormatted

							For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
								If PeriodUnitName = "" Then
									PeriodUnitName = ObjCompMonitorServiceStatusPeriod.PeriodUnitName
									FrequencyValue = ObjCompMonitorServiceStatusPeriod.FrequencyValue
									DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue
									DueOnValue = ObjCompMonitorServiceStatusPeriod.DueOnValue
									FrequencyValueFormatted = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
									DoneOnValueFormatted = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
									DueOnValueFormatted = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
									CurrentValueFormatted = ObjCompMonitorServiceStatusPeriod.CurrentValueFormatted
									ElapsedValueFormatted = ObjCompMonitorServiceStatusPeriod.ElapsedAtRemovalFormattedForOCComponents
									ExtensionValueFormatted = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
									RemainingValueFormatted = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
								Else
									PeriodUnitName = PeriodUnitName & "</BR>" & ObjCompMonitorServiceStatusPeriod.PeriodUnitName
									FrequencyValue = FrequencyValue & "</BR>" & ObjCompMonitorServiceStatusPeriod.FrequencyValue
									DoneOnValue = DoneOnValue & "</BR>" & ObjCompMonitorServiceStatusPeriod.DoneOnValue
									DueOnValueFormatted = DueOnValue & "</BR>" & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
									FrequencyValueFormatted = FrequencyValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
									DoneOnValueFormatted = DoneOnValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
									CurrentValueFormatted = CurrentValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.CurrentValueFormatted
									ElapsedValueFormatted = ElapsedValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.ElapsedAtRemovalFormattedForOCComponents
									ExtensionValueFormatted = ExtensionValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
									RemainingValueFormatted = RemainingValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
								End If
							Next
							ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , , , , , , MaintenanceActivityType, MonitorInfo, , , , _
						   , , , , FrequencyValueFormatted, , , ElapsedValueFormatted, , , RemainingValueFormatted, , , DueOnValueFormatted, , , , PeriodUnitName, , , , , _
						   , , , , , , , , CurrentValueFormatted, , , , , , , , , , , DoneOnValueFormatted, DoneOn, , , , , , , ExtensionValueFormatted))
						Next
					End If

					If ObjCompStatus.CompMonitorInspStatusList.Count > 0 Then
						For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
							PeriodUnitName = ""
							FrequencyValue = ""
							DueOnValue = ""
							FrequencyValueFormatted = ""
							DoneOnValueFormatted = ""
							DueOnValueFormatted = ""
							MonitorInfo = ""
							DoneOn = ""
							CurrentValueFormatted = ""
							ElapsedValueFormatted = ""
							ExtensionValueFormatted = ""
							RemainingValueFormatted = ""

							MaintenanceActivityType = ObjCompMonitorInspStatus.Type
							MonitorInfo = ObjCompMonitorInspStatus.Code
							DoneOn = ObjCompMonitorInspStatus.DoneOnFormatted

							For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
								If PeriodUnitName = "" Then
									PeriodUnitName = ObjCompMonitorInspStatusPeriod.PeriodUnitName
									FrequencyValue = ObjCompMonitorInspStatusPeriod.FrequencyValue
									DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue
									DueOnValue = ObjCompMonitorInspStatusPeriod.DueOnValue
									FrequencyValueFormatted = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
									DoneOnValueFormatted = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
									DueOnValueFormatted = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
									CurrentValueFormatted = ObjCompMonitorInspStatusPeriod.CurrentValueFormatted
									ElapsedValueFormatted = ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
									ExtensionValueFormatted = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
									RemainingValueFormatted = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
								Else
									PeriodUnitName = PeriodUnitName & "</BR>" & ObjCompMonitorInspStatusPeriod.PeriodUnitName
									FrequencyValue = FrequencyValue & "</BR>" & ObjCompMonitorInspStatusPeriod.FrequencyValue
									DoneOnValue = DoneOnValue & "</BR>" & ObjCompMonitorInspStatusPeriod.DoneOnValue
									DueOnValue = DueOnValue & "</BR>" & ObjCompMonitorInspStatusPeriod.DueOnValue
									FrequencyValueFormatted = FrequencyValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
									DoneOnValueFormatted = DoneOnValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
									DueOnValueFormatted = DueOnValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
									CurrentValueFormatted = CurrentValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.CurrentValueFormatted
									ElapsedValueFormatted = ElapsedValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
									ExtensionValueFormatted = ExtensionValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
									RemainingValueFormatted = RemainingValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
								End If
							Next
							ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , , , , , , MaintenanceActivityType, MonitorInfo, , , , _
						  , , , , FrequencyValueFormatted, , , ElapsedValueFormatted, , , RemainingValueFormatted, , , DueOnValueFormatted, , , , PeriodUnitName, , , , , _
						  , , , , , , , , CurrentValueFormatted, , , , , , , , , , , DoneOnValueFormatted, DoneOn, , , , , , , ExtensionValueFormatted))
						Next
					End If

					If ObjCompStatus.CompMonitorModStatusList.Count > 0 Then
						For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
							PeriodUnitName = ""
							FrequencyValue = ""
							DueOnValue = ""
							FrequencyValueFormatted = ""
							DoneOnValueFormatted = ""
							DueOnValueFormatted = ""
							MonitorInfo = ""
							DoneOn = ""
							CurrentValueFormatted = ""
							ElapsedValueFormatted = ""
							ExtensionValueFormatted = ""
							RemainingValueFormatted = ""

							MaintenanceActivityType = ObjCompMonitorModStatus.Type
							MonitorInfo = ObjCompMonitorModStatus.Code
							DoneOn = ObjCompMonitorModStatus.DoneOnFormatted

							For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
								If PeriodUnitName = "" Then
									PeriodUnitName = ObjCompMonitorModStatusPeriod.PeriodUnitName
									FrequencyValue = ObjCompMonitorModStatusPeriod.FrequencyValue
									DoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue
									DueOnValue = ObjCompMonitorModStatusPeriod.DueOnValue
									FrequencyValueFormatted = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
									DoneOnValueFormatted = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
									DueOnValueFormatted = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
									CurrentValueFormatted = ObjCompMonitorModStatusPeriod.CurrentValueFormatted
									ElapsedValueFormatted = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
									ExtensionValueFormatted = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
									RemainingValueFormatted = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
								Else
									PeriodUnitName = PeriodUnitName & "</BR>" & ObjCompMonitorModStatusPeriod.PeriodUnitName
									FrequencyValue = FrequencyValue & "</BR>" & ObjCompMonitorModStatusPeriod.FrequencyValue
									DoneOnValue = DoneOnValue & "</BR>" & ObjCompMonitorModStatusPeriod.DoneOnValue
									DueOnValue = DueOnValue & "</BR>" & ObjCompMonitorModStatusPeriod.DueOnValue
									FrequencyValueFormatted = FrequencyValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
									DoneOnValueFormatted = DoneOnValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
									DueOnValueFormatted = DueOnValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
									CurrentValueFormatted = CurrentValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.CurrentValueFormatted
									ElapsedValueFormatted = ElapsedValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
									ExtensionValueFormatted = ExtensionValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
									RemainingValueFormatted = RemainingValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.RemainingValueFormatted
								End If
							Next
							ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , , , , , , MaintenanceActivityType, MonitorInfo, , , , _
						, , , , FrequencyValueFormatted, , , ElapsedValueFormatted, , , RemainingValueFormatted, , , DueOnValueFormatted, , , , PeriodUnitName, , , , , _
						, , , , , , , , CurrentValueFormatted, , , , , , , , , , , DoneOnValueFormatted, DoneOn, , , , , , , ExtensionValueFormatted))
						Next
					End If
				Next
			Next
		Next
		Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
		Return ReportMaintenanceDetails
	End Function

#End Region

#Region " Digital Signature "
	Private Sub btnlRequestForDigitalSignature_Click(sender As Object, e As EventArgs) Handles btnlRequestForDigitalSignature.Click
		Try
			Dim mDS_Queue As DS_Queue = DS_Queue.NewDS_Queue()
			Dim mDS_ModuleList As DS_ModuleList = DS_ModuleList.GetDS_ModuleList()

			With mDS_Queue

				.ModuleID = 1
				.ModuleName = mDS_ModuleList.Item(.ModuleID, "").Name
				.TransactionID = mOrder.ID

				SetReport(IsForDS:=True)

				Dim myFile As String = ""
				myFile = Session("myFile")

				Dim bytes As Byte() = System.IO.File.ReadAllBytes(myFile)
				Dim fileName As String = myFile
				Dim fi As FileInfo = New FileInfo(fileName)

				.ImageSize = fi.Length
				.Extension = fi.Extension
				.FileName = fi.Name

				Dim b1(fi.Length - 1) As Byte
				Dim txt As Byte() = New UTF8Encoding(True).GetBytes(myFile)
				.Imagefile = bytes 'txt
				Dim mUser As SI.UTILITY.User = SI.UTILITY.User.GetUser(User.Identity.Name)
				.RequestedUserID = mUser.UserID
			End With

			Session("mDS_Queue") = mDS_Queue
			Session("myFile") = Nothing
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDigitalSignatureRequestWindow", "OpenDigitalSignatureRequestWindow();", True)
		Catch ex As Exception
			'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop311", "opennotificationpopup('" + ex.Message + "','error');", True)
		End Try
	End Sub
	Private Sub btnViewDSFile_Click(sender As Object, e As EventArgs) Handles btnViewDSFile.Click


		Dim DS_Queue As DS_Queue = DS_Queue.GetDS_QueueAfterSigned(mOrder.ID, True)


		If DS_Queue.DS_ImageSize > 0 Then

			Dim NO As New Random
			Dim mFile As String = "PurchaseOrderDS" & NO.Next.ToString
			Dim fileName As String = mFile & DS_Queue.DS_Extension
			Dim path As String = Server.MapPath("~/Temp") & "\" & fileName
			Dim fs As System.IO.FileStream

			If System.IO.File.Exists(path) = False Then
				System.IO.File.Delete(path)
				fs = System.IO.File.Create(path)
				fs.Write(DS_Queue.DS_ImageFile, 0, DS_Queue.DS_ImageFile.Length)
				fs.Close()
				Dim str As String
				str = "openFile()"
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScript", str, True)
				Session("DocPath") = path

			End If
		Else
			' ShowAlertBox("", "warning")
			'ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, "opennotificationpopup('" + "Digital Signature Is Pending" + "','warning');", True)
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Digital Signature Is Pending", False), True)
		End If

	End Sub
#End Region

#Region " Multiple Attachments "

	Private Sub btnSelectFiles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
		setObject()
		Session("mOrder") = mOrder
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
	End Sub

	Private Sub Attachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgItemAttachment.RowCommand

		Try

			Dim Index As Integer = CInt(e.CommandArgument) + dgItemAttachment.PageSize * dgItemAttachment.PageIndex

			Select Case e.CommandName
				Case "View"

					mFileAttachments = mOrder.FileAttachments

					AttachmentHelper.DownloadAttachmentWithName(Index:=Index,
													   ModuleName:="Multiple Attachments",
													   AttachmentObject:=mFileAttachments)

					ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

					dgItemAttachment.DataSource = mOrder.FileAttachments
					dgItemAttachment.DataBind()
					upnlItemAttachment.Update()
					upnldgItemAttachment.Update()

				Case "Remove"

					mFileAttachments = mOrder.FileAttachments

					If mFileAttachments.Count = 1 Then

						DeleteAttachment(0)
						mOrder.IsAttachmentAdded = False
						Session("IsAttachmentDeleted") = IsAttachmentDeleted

					Else
						DeleteAttachment(Index - 1)
					End If

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
		'setObject()
		'Session("mItem") = mItem
		AttachMyFile()
		mOrder.IsAttachmentAdded = True
		ControlVisibilityForAttachment()
		upnlItemAttachment.Update()

	End Sub

	Private Sub AttachMyFile()

		Try

			If Not mOrder.FileAttachments.Contains(ReferenceID:=mOrder.ID, FileName:=CType(Session("FileUpload.FileName"), String)) Then

				mOrder.FileAttachments.Add(mOrder.ID, CType(Session("FileUpload.FileName"), String))
				mOrder.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
				mOrder.FileAttachments.CurrentItem.Size = Session("Size")
				mOrder.FileAttachments.CurrentItem.Extension = Session("Extension")
				mOrder.FileAttachments.CurrentItem.FileName = CType(Session("FileUpload.FileName"), String)

				Session("mOrder") = mOrder
				Session("AttachmentName") = CType(Session("FileUpload.FileName"), String)

				dgItemAttachment.DataSource = mOrder.FileAttachments
				dgItemAttachment.DataBind()

				For i As Integer = 0 To mOrder.FileAttachments.Count - 1
					Dim txtValue As TextBox
					txtValue = CType(Me.dgItemAttachment.Rows(i).FindControl("txtFileName"), TextBox)
					txtValue.Text = mOrder.FileAttachments(i).FileName
				Next

				Session.Remove("Size")
				Session.Remove("ImageFile")
				Session.Remove("Extension")
				Session.Remove("FileUpload.FileName")
				upnlItemAttachment.Update()
				upnldgItemAttachment.Update()

			Else

				Session("mOrder") = mOrder
				MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate,
								MSGBox.Message_Text.Duplicate,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteAttachment(ByVal Index As Int32)
		MSGBoxCtrl.Show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
		mOrder.FileAttachments.CurrentIndex = Index
		Session("mOrder") = mOrder
	End Sub

	'Changes by Sankalp 25-08-25
	Private Sub ControlVisibilityForAttachment()

		btnSelectFiles.Visible = CType(IIf(mOrder.StatusID = 2 Or
										   mOrder.StatusID = 4 Or
										   mOrder.ReceiptCount > 0 Or
										   Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
		upnldgItemAttachment.Update()
		upnlItemAttachment.Update()

	End Sub

#End Region

End Class