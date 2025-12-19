Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.PeriodList


Public Class wfReceiptcumInvoiceItem_Ajax
	Inherits Page

#Region " Enumeration "

	Private Enum Rights

		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		Authorized = 7 'Added By Prashant 17-Aug-2011

	End Enum

#End Region

#Region " Variable Declaration "

	Public mReceiptCumInvoice As ReceiptCumInvoice
	Public mStoreList As StoreList
	Public mTotalPendingItemQty As Decimal = 0
	Public TotalCount As Decimal = 0
	Public mPartTypeList As PartTypeList
	Public mSelectPeriods As SelectPeriods = SelectPeriods.NewSelectPeriods
	Private Flag As Int16 'Added By Prashant 11-Feb-2010
	Public mUnitConverterList As UnitConverterList
	Public mItemTypeList As PartTypeList 'Added By Vikrant On 31-Oct-2012 For ALL31102012
	Dim mFileAttach As FileAttach 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
	Dim IsAttachmentDeleted As Boolean = False 'End
	Dim mLastWarrantyInformation As LastWarrantyInformation
	Dim mDateForPreMatureFailure As DateForPreMatureFailure  'Added By Prashant 07-Jul-2016 
	Public isPOPShown As Boolean = False
	Public mStore As Store
	Dim mIsOwnedByCustomer As Boolean
	Public mGSTPercentage As GSTPercentage
	Public mVendor As Vendor
	Public mWarrantyStatusList As WarrantyStatusList
	Public mLastServicedInspectedDoneOnDate As LastServicedInspectedDoneOnDate
	Public mUserHasNoStoreRights As UserHasNoStoreRights

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
		mStoreList = CType(Session("mStoreList"), StoreList)
		mTotalPendingItemQty = Session("mTotalPendingItemQty")
		TotalCount = Session("TotalCount")
		mPartTypeList = Session("mPartTypeList")
		mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods) 'Added By Prashant 10-Feb-2010
		mUnitConverterList = CType(Session("mUnitConverterList"), UnitConverterList) '-----------------------------
		mItemTypeList = CType(Session("mItemTypeList"), PartTypeList) 'Added By Vikrant On 31-Oct-2012 For ALL31102012
		mFileAttach = Session("mFileAttach") 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
		IsAttachmentDeleted = Session("IsAttachmentDeleted") 'End
		mDateForPreMatureFailure = Session("mDateForPreMatureFailure")
	End Sub

	Private Sub SetSession()
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		Session("mStoreList") = mStoreList
		Session("mPartTypeList") = mPartTypeList
		Session("mSelectPeriods") = mSelectPeriods
		Session("mUnitConverterList") = mUnitConverterList
		Session("mFileAttach") = mFileAttach 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
		Session("IsAttachmentDeleted") = IsAttachmentDeleted 'End
		Session("mDateForPreMatureFailure") = mDateForPreMatureFailure
	End Sub

	Private Sub RemoveSessions()
		Session("mTotalPendingItemQty") = 0
		Session("TotalCount") = 0
		Session.Remove("mItemTypeList") 'Added By Vikrant On 31-Oct-2012 For ALL31102012
		Session.Remove("mFileAttach") 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
		Session.Remove("IsAttachmentDeleted") 'End
		Session.Remove("mDateForPreMatureFailure")
	End Sub

	Private Sub SetPage()
		If Session("Edit") Then
			lblTitle.Text = "Receiving Part [" & mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName & "]"
			imgPartNo.BackColor = Color.Silver
			txtPartNo.BackColor = Color.Silver
		End If
		lblSerializedStatus.Visible = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized And Not Session("Edit")
		If mTotalPendingItemQty - TotalCount + 1 > mTotalPendingItemQty Then
			lblSerializedStatus.Text = " Extra Part : You are trying to add more Items."
		Else
			lblSerializedStatus.Text = "Receiving Serialized Part: " + CType(mTotalPendingItemQty - TotalCount + 1, String) + "/" + CType(mTotalPendingItemQty, String)
		End If
	End Sub

	Private Overloads Sub setFocus(cntrl As WebControl)
		If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub

	Private Sub AddAttributes()

		Try

			txtQuantity.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQuantity').value,event)")

			If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
				txtCAmount.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
				txtDisplayCAmount.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
			Else
				txtCRate.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
				txtDisplayCRate.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
			End If

			txtCOtherCharges.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCOtherCharges').value,event)")
			txtCureQtrs.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCureQtrs').value,event)")
			txtCureYear.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCureYear').value,event)")
			txtExpQrts.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExpQrts').value,event)")
			txtExpYear.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExpYear').value,event)")
			txtCommercialRate.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
			txtGROCRate.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
			'Added By Vikrant On 11-Aug-2016 For ALL11082016
			txtExcessQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExcessQty').value,event)")
			txtShortQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtShortQty').value,event)")
			txtRejectedQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRejectedQty').value,event)")
			'End
			txtDisplayCommercialRate.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtDisplayCommercialRate').value,event)")

			'Ajay 10-03-2023
			txtCureQtrs.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
			txtCureYear.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
			txtExpQrts.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
			txtExpYear.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
			txtWarrantyInDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
			'--------------------

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub NewReceiptCumInvoiceItem(mReceiptCumInvoiceItem As ReceiptCumInvoiceItem)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoiceItem.ID)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SrNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex + 1
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mReceiptCumInvoiceItem.ItemID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mReceiptCumInvoiceItem.Part
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mReceiptCumInvoiceItem.PartDescription
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mReceiptCumInvoiceItem.FromItemTypeID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StockBalanceQty = mReceiptCumInvoiceItem.StockBalanceQty
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID = mReceiptCumInvoiceItem.OrderItemID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mReceiptCumInvoiceItem.IssueItemID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = mReceiptCumInvoiceItem.FromPartList
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteNo = mReceiptCumInvoiceItem.ReleaseNoteNo
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDate = mReceiptCumInvoiceItem.ReleaseNoteDate
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = ""
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoiceItem.StoreID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Location = mReceiptCumInvoiceItem.Location
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoiceItem.StartDate
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = mReceiptCumInvoiceItem.ExpiryDate
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mReceiptCumInvoiceItem.DisplayUnitID          'Added By Prashant 11-May-2010
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitName = cmbUnitConverterList.SelectedItem.Text      'Added By Prashant 11-May-2010
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = 1                                                'Added By Prashant 11-May-2010
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoiceItem.CRate
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoiceItem.DisplayCRate            'Added By Prashant 5-Feb-2019 ALL04022019
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Factor = mReceiptCumInvoiceItem.Factor                        'Added By Prashant 5-Feb-2019 ALL04022019
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = mReceiptCumInvoiceItem.COtherCharges
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Remark = mReceiptCumInvoiceItem.Remark
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Note = mReceiptCumInvoiceItem.Note
		If Not (mReceiptCumInvoice.CurrencyID.Equals(Guid.Empty)) Then
			Dim mCurrency As Currency = Currency.GetCurrency(mReceiptCumInvoice.CurrencyID)
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Currency = mCurrency.Name
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Currency = Trim(txtRateCurrency.Text)
		End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = mReceiptCumInvoiceItem.IsWarranty                        'Added By Prashant 12/11/2007
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = mReceiptCumInvoiceItem.WarrantyInDays
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = mReceiptCumInvoiceItem.WarrantyStartDate
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = mReceiptCumInvoiceItem.WarrantyExpiryDate
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpQtrs = mReceiptCumInvoiceItem.ExpQtrs
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpYear = mReceiptCumInvoiceItem.ExpYear
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs = mReceiptCumInvoiceItem.CureQtrs
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear = mReceiptCumInvoiceItem.CureYear                            '----------------------------
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BatchNo = mReceiptCumInvoiceItem.BatchNo                              'Added By Prashant 19/Aug/2008
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = mReceiptCumInvoiceItem.CalibrationDoneOnDate  'Added By Prashant 19/Aug/2008
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BarcodeNo = mReceiptCumInvoiceItem.BarcodeNo                          'Added by Vikrant on 30-AUG-2011
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryNA = mReceiptCumInvoiceItem.IsExpiryNA '----Added by Vikrant FOR ALL10052012-10--------------
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryUnlimited = mReceiptCumInvoiceItem.IsExpiryUnlimited '-----------------------------------------------------
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = mReceiptCumInvoiceItem.GROCRate 'Added By Prashant 28-Oct-2013 --ALL25102013-1
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsTransitDamage = chkIsTransitDamage.Checked
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = mReceiptCumInvoiceItem.PrimaryCategoryID 'Added By Prashant On 07-Oct-2015 For ALL06102015
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = ""   'Added By Prashant On 07-Oct-2015 For ALL06102015
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss = mReceiptCumInvoiceItem.IsAirworthinss
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConditionCheckDoneOnDate = mReceiptCumInvoiceItem.ConditionCheckDoneOnDate
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ServiedInspectedCheckDoneOnDate = mReceiptCumInvoiceItem.ServiedInspectedCheckDoneOnDate 'Added by SHital on 13-Sep-2019 For ALL13092019
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyApplicableStatus = Val(cmbWarrantyStatus.SelectedValue)   '1 Accepted 2 Rejected 0 None
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FaultFound = Val(cmbFaultFound.SelectedValue)
		'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeEmailIDs = mReceiptCumInvoiceItem.ReqEmployeeEmailIDs
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqNo = mReceiptCumInvoiceItem.ReqNo
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeName = mReceiptCumInvoiceItem.ReqEmployeeName
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqQty = mReceiptCumInvoiceItem.ReqQty
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqDate = mReceiptCumInvoiceItem.ReqDate.ToString
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeID = mReceiptCumInvoiceItem.ReqEmployeeID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqItemID = mReceiptCumInvoiceItem.ReqItemID
		'End
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartCategory = mReceiptCumInvoiceItem.PartCategory 'Added by Vikrant on 14-Apr-2021 for ALL14042021
		AddReceiptItemServiceInspections() 'Added By Prashant 30-Sep-2019
		If mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections.Count > 0 Then 'Added By Prashant 30-Sep-2019
			For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Count - 1
				For j As Integer = 0 To mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections.Count - 1
					If mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections(j).ItemServiceInspectionsID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ItemServiceInspectionsID Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ServiedInspectedCheckDoneOnDate = mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections(j).ServiedInspectedCheckDoneOnDate
					End If
				Next
			Next
		End If 'End of Added By Prashant 30-Sep-2019 
		Dim mtmpItem As Item
		mtmpItem = Item.GetItem(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
		If AppSettings("IsGSTApplicable") = "True" Then
			If (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 27 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or
				mReceiptCumInvoice.TransTypeID = 67 Or mReceiptCumInvoice.TransTypeID = 28 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 57) Then
				mVendor = Vendor.GetVendor(mReceiptCumInvoice.VendorID)
				If mVendor.ClientCountryName.ToUpper = "INDIA" Then
					If mVendor.CountryName.ToUpper = "INDIA" And mReceiptCumInvoice.RecCumInvDate >= CDate("01-Jul-2017") Then

						'mGSTPercentage = GSTPercentage.GetPercentage(mReceiptCumInvoice.RecCumInvDate, 1, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString)
						If Not mGSTPercentage Is Nothing Then
							If Len(mVendor.StateCode) > 0 Then
								If mVendor.StateCode = mVendor.ClientStateCode Then
									If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage = 0 Then
										mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage = mReceiptCumInvoiceItem.CGSTPercentage
										mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTPercentage = mReceiptCumInvoiceItem.SGSTPercentage
									Else
										'Do nothing  Already GST percentage set from Order Item
									End If
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTCAmount = ((mReceiptCumInvoiceItem.CGSTPercentage * mReceiptCumInvoiceItem.CAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTCAmount = ((mReceiptCumInvoiceItem.SGSTPercentage * mReceiptCumInvoiceItem.CAmount) / 100)

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.TotalCAmount = mReceiptCumInvoiceItem.CAmount + mReceiptCumInvoiceItem.CGSTCAmount + mReceiptCumInvoiceItem.SGSTCAmount
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCGSTCAmount = ((mReceiptCumInvoiceItem.CGSTPercentage * mReceiptCumInvoiceItem.DisplayCAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplaySGSTCAmount = ((mReceiptCumInvoiceItem.SGSTPercentage * mReceiptCumInvoiceItem.DisplayCAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayTotalCAmount = mReceiptCumInvoiceItem.DisplayCAmount + mReceiptCumInvoiceItem.DisplayCGSTCAmount + mReceiptCumInvoiceItem.DisplaySGSTCAmount
								Else
									If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage = 0 Then
										mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage = (mReceiptCumInvoiceItem.IGSTPercentage)
									Else
										'Do nothing  Already GST percentage set from Order Item
									End If
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTCAmount = ((mReceiptCumInvoiceItem.IGSTPercentage * mReceiptCumInvoiceItem.CAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.TotalCAmount = mReceiptCumInvoiceItem.CAmount + mReceiptCumInvoiceItem.IGSTCAmount
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayIGSTCAmount = ((mReceiptCumInvoiceItem.IGSTPercentage * mReceiptCumInvoiceItem.DisplayCAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayTotalCAmount = mReceiptCumInvoiceItem.DisplayCAmount + mReceiptCumInvoiceItem.DisplayIGSTCAmount
								End If
							Else
							End If
						End If
						mtmpItem = Nothing
					Else
					End If
				Else
					mReceiptCumInvoice.StateCode = mVendor.StateCode
					mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
					mReceiptCumInvoice.VendorCountry = mVendor.CountryName
					mReceiptCumInvoice.Visibility = 3
				End If
			End If
		Else
			mReceiptCumInvoice.Visibility = 3
		End If
		mFileAttach = FileAttach.NewAttachmentChild(Guid.NewGuid, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID)
		Session("mFileAttach") = mFileAttach
	End Sub

	Private Function SetObject() As Boolean
		mReceiptCumInvoice.BeginEdit()
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SrNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex + 1
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteNo = Trim(txtReleaseNote.Text)
		If (txtReleaseNoteDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDate = txtReleaseNoteDate.Text
		End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = Trim(txtSerialNo.Text)
		If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID.Equals(New Guid(cmbStore.SelectedValue)) = True Then
			'Do nothing
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID = Item.GetItem(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID).ItemTagID
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName = Item.GetItem(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID).ItemTagName
			upnlAttentionInfo.DataBind()
			upnlAttentionInfo.Update()
		End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = New Guid(cmbStore.SelectedValue)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreName = Trim(cmbStore.SelectedItem.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Location = Trim(txtLocation.Text)
		If (txtStartDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = txtStartDate.Text
		End If
		If (txtExpiryDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = txtExpiryDate.Text
		End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = Val(cmbPartType.SelectedValue)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = New Guid(cmbUnitConverterList.SelectedValue)   'Added By Prashant 11-May-2010
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitName = cmbUnitConverterList.SelectedItem.Text     'Added By Prashant 11-May-2010
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(Val(txtQuantity.Text))

		'Added By Vikrant on 02-Aug-2012 For BA01082012
		If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
			If (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
				'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = CDec(Val(txtGROCAmount.Text)) 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = CDec(Val(txtGROCRate.Text))
			Else
				'If txtCAmount.Visible = True Then
				'    txtCAmount.DataBind()   'Added By Prashant 5-Feb-2019 ALL04022019  on Qty change CAmount was not changing so we did this
				'    upnlRateValues.Update() 'Added By Prashant 5-Feb-2019 ALL04022019
				'End If
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = CDec(Val(txtCAmount.Text))
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount = CDec(Val(txtDisplayCAmount.Text))
			End If
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(Val(txtQuantity.Text))
			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty > 0 Then
				If (mReceiptCumInvoice.TransTypeID = 10) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
					'Do Nothing
				ElseIf (mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = CDec(Val(txtCommercialRate.Text))
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = CDec(Val(txtCommercialRate.Text))
				Else
					'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / CDec(Val(txtQuantity.Text))
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = 0 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
				End If
			End If
		Else 'Old Condition
			If (mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = CDec(Val(txtCommercialRate.Text))
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = CDec(Val(txtCommercialRate.Text))
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = CDec(Val(txtCOtherCharges.Text)) 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = CDec(Val(txtCRate.Text))
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = CDec(Val(txtDisplayCRate.Text))
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = CDec(Val(txtCOtherCharges.Text)) 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			End If
		End If
		'End
		'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = CDec(Val(txtCOtherCharges.Text)) 'Commented By Prashant 28-Oct-2013 --ALL25102013-1	
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(Val(txtQuantity.Text))
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Remark = Trim(txtRemark.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Note = Trim(txtNote.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = chkIsInWarranty.Checked
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = Val(txtWarrantyInDays.Text)
		If (txtWarrantyStartDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = txtWarrantyStartDate.Text
		End If

		If (txtWarrantyExpiryDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = txtWarrantyExpiryDate.Text
		End If
		If Not (mReceiptCumInvoice.CurrencyID.Equals(Guid.Empty)) Then
			Dim mCurrency As Currency = Currency.GetCurrency(mReceiptCumInvoice.CurrencyID)
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Currency = mCurrency.Name
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Currency = Trim(txtRateCurrency.Text)
		End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs = Val(txtCureQtrs.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear = Val(txtCureYear.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpQtrs = Val(txtExpQrts.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpYear = Val(txtExpYear.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BatchNo = Trim(txtBatchNo.Text)
		If (txtCalibrationDoneOnDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = txtCalibrationDoneOnDate.Text
		End If
		'If (txtConditionCheckDoneOnDate.Text = "") Then
		'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConditionCheckDoneOnDate = System.DBNull.Value
		'Else
		'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConditionCheckDoneOnDate = txtConditionCheckDoneOnDate.Text
		'End If
		'Added by Shital on 13-Sep-2019
		'If (txtServicedInspectedDoneOnDate.Text = "") Then
		'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ServiedInspectedCheckDoneOnDate = System.DBNull.Value
		'Else
		'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ServiedInspectedCheckDoneOnDate = txtServicedInspectedDoneOnDate.Text
		'End If

		If (mReceiptCumInvoice.Receipt.ReceiptItems.Contains(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem) = True) Then
			MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add Duplicate entry in Goods Receipt. <BR><BR> Goods Receipt can not contains non serialized part with same Release Note No.", MsgBoxStyle.OkOnly, "")
			mReceiptCumInvoice.CancelEdit()
			Exit Function
			'ElseIf (AppSettings("ClientCode") = "BA"  Or AppSettings("ClientCode") = "Novo"  And ChkIsConsiderAsAsset.Checked = False And ((mReceiptCumInvoice.TransTypeID = 67 Or mReceiptCumInvoice.TransTypeID = 10) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 1 Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2))) Then
			'    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Select Consider As Asset", MsgBoxStyle.OkOnly, "")
			'    mReceiptCumInvoice.CancelEdit()
			'    Exit Function
		ElseIf ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") And chkRemovedasReturnableFromAircraft.Checked = False And ChkIsConsiderAsAsset.Checked = False And mReceiptCumInvoice.TransTypeID = 9 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 1) Then
			MSGBoxCtrl.Show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Select Removed As Returnable From Aircraft", MsgBoxStyle.OkOnly, "")
			mReceiptCumInvoice.CancelEdit()
			Exit Function
		Else
			mReceiptCumInvoice.ApplyEdit()
		End If
		'If (AppSettings("CodeNo") = "True" And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2 And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartFromListisSerialized = True)) Then
		'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = txtCodeNo.Text.Trim   'Added By Prashant On 07-Oct-2015 For ALL06102015
		'    If (mReceiptCumInvoice.Receipt.ReceiptItems.ContainsCodeNo(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem) = True) Then 'Added By Prashant On 07-Oct-2015 For ALL06102015    
		'        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You can not add duplicate Code No.", MsgBoxStyle.OkOnly, "")
		'        mReceiptCumInvoice.CancelEdit()
		'        Exit Function
		'    Else
		'        mReceiptCumInvoice.ApplyEdit()
		'    End If
		'End If

		If (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = CDec(Val(txtGROCRate.Text))
		End If
		If mReceiptCumInvoice.TransTypeID = 10 Then
			mReceiptCumInvoice.Invoice.InvoiceItems.CurrentItem.TransTypeID = 10
		ElseIf mReceiptCumInvoice.TransTypeID = 48 Then
			mReceiptCumInvoice.Invoice.InvoiceItems.CurrentItem.TransTypeID = 48
		ElseIf mReceiptCumInvoice.TransTypeID = 54 Then
			mReceiptCumInvoice.Invoice.InvoiceItems.CurrentItem.TransTypeID = 54
		ElseIf (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True) Then
			mReceiptCumInvoice.Invoice.InvoiceItems.CurrentItem.TransTypeID = 67
			mReceiptCumInvoice.Invoice.InvoiceItems.CurrentItem.IsReturnFromOHRepair = True
		End If
		mReceiptCumInvoice.Invoice.CalculateTotal()
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BarcodeNo = Trim(txtBarcodeNo.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = CDec(Val(txtCommercialRate.Text))
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCCommercialRate = CDec(Val(txtDisplayCommercialRate.Text))
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryNA = chkIsExpiryNA.Checked
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryUnlimited = chkIsExpiryUnlimited.Checked
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsConsiderAsAsset = ChkIsConsiderAsAsset.Checked
		If mReceiptCumInvoice.TransTypeID = 9 Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovedAsReturnableFromAircraft = chkRemovedasReturnableFromAircraft.Checked
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.AircraftRemovedQty = IIf(chkRemovedasReturnableFromAircraft.Checked, Val(txtQuantity.Text), 0)
		End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsTransitDamage = chkIsTransitDamage.Checked
		'Added By Vikrant On 11-Aug-2016 For ALL11082016
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExcessQty = CDec(Val(txtExcessQty.Text))
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ShortQty = CDec(Val(txtShortQty.Text))
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RejectedQty = CDec(Val(txtRejectedQty.Text))
		'End
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyApplicableStatus = Val(cmbWarrantyStatus.SelectedValue)   '1 Accepted 2 Rejected 0 None
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PreviousWorkScope = Trim(txtPreviousWorkScope.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FaultFound = Val(cmbFaultFound.SelectedValue)
		'----GST--------------------------------------------------------------

		''Added by Saylee on 9-Mar-2021 for Heligo10032021
		If (txtManufacturingDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ManufacturingDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ManufacturingDate = txtManufacturingDate.Text
		End If
		'****************************
		'-----ReceiptItem-------------------------------------------
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompID = Trim(txtCompID.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HazmatID = Trim(txtHazmatID.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CertificateNo = Trim(txtCertificateNo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RevisionNo = Trim(txtRevisionNo.Text)


		If (txtRevisionDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RevisionDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RevisionDate = txtRevisionDate.Text
		End If

		'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RevisionDate = Trim(txtRevisionDate.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CertifyingRemarks = Trim(txtCertifyingRemarks.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WorkOrderRONo = Trim(txtWORONo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WorkCardNoRepVendor = Trim(txtWCRepVendor.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CertificateType = Trim(txtCertificateType.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ApprovalNo = Trim(txtApprovalNo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Warehouse = Trim(txtWarehouseNo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ManfLot = Trim(txtManfLot.Text)



		If (txtInspectedDate.Text = "") Then
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InspectedDate = System.DBNull.Value
		Else
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InspectedDate = txtInspectedDate.Text
		End If

		'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InspectedDate = Trim(txtInspectedDate.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InspectedBy = Trim(txtInspectedBy.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.LastRemovalPosition = Trim(txtLastRemovalPosition.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovalReason = Trim(txtRemovalReason.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.NHAPartNo = Trim(txtNHAPartNo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.NHASerialNo = Trim(txtNHASerialNo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PackageWONo = Trim(txtPackageWONo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CR = Trim(txtCR.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StationWC = Trim(txtStationWC.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovalType = Trim(txtRemovalType.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovedBy = Trim(txtRemovedBy.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InstallPart = Trim(txtInstallPart.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InstallSerial = Trim(txtInstallSerial.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InstallBy = Trim(txtInstallBy.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DiscrepancyNo = Trim(txtDiscrepancyNo.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RepeatDiscrepancy = Trim(txtRepeatDiscrepancy.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Incident = Trim(txtIncident.Text)

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CausedDelay = Trim(txtCausedDelay.Text)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DiscrepancyDescription = Trim(txtDiscrepancyDescription.Text)



		'-----------------------------------------------------------
		Dim mtmpItem As Item
		mtmpItem = Item.GetItem(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
		If AppSettings("IsGSTApplicable") = "True" Then
			If (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 27 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or
				mReceiptCumInvoice.TransTypeID = 67 Or mReceiptCumInvoice.TransTypeID = 28 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 57) Then
				mVendor = Vendor.GetVendor(mReceiptCumInvoice.VendorID)
				If mVendor.ClientCountryName.ToUpper = "INDIA" Then
					If mVendor.CountryName.ToUpper = "INDIA" And mReceiptCumInvoice.RecCumInvDate >= CDate("01-Jul-2017") Then
						mGSTPercentage = GSTPercentage.GetPercentage(mReceiptCumInvoice.RecCumInvDate, 1, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString)
						If Not mGSTPercentage Is Nothing Then
							If Len(mVendor.StateCode) > 0 Then
								If mVendor.StateCode = mVendor.ClientStateCode Then
									If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage = 0 Then
										mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
										mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
									Else
										'Do nothing  Already GST percentage set from Order Item
									End If
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTCAmount = ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTCAmount = ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTPercentage * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount) / 100)

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.TotalCAmount = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTCAmount
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCGSTCAmount = ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplaySGSTCAmount = ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTPercentage * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayTotalCAmount = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplaySGSTCAmount

									mReceiptCumInvoice.StateCode = mVendor.StateCode
									mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
									mReceiptCumInvoice.VendorCountry = mVendor.CountryName
									mReceiptCumInvoice.Visibility = 1
								Else
									If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage = 0 Then
										mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage = (mGSTPercentage.GSTPercentage)
									Else
										'Do nothing  Already GST percentage set from Order Item
									End If
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTCAmount = ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.TotalCAmount = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTCAmount
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayIGSTCAmount = ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount) / 100)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayTotalCAmount = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayIGSTCAmount

									mReceiptCumInvoice.StateCode = mVendor.StateCode
									mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
									mReceiptCumInvoice.VendorCountry = mVendor.CountryName
									mReceiptCumInvoice.Visibility = 2
								End If
							Else
								mReceiptCumInvoice.StateCode = mVendor.StateCode
								mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
								mReceiptCumInvoice.VendorCountry = mVendor.CountryName
								mReceiptCumInvoice.Visibility = 3
							End If
						End If

					Else
						mReceiptCumInvoice.StateCode = mVendor.StateCode
						mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
						mReceiptCumInvoice.VendorCountry = mVendor.CountryName
						mReceiptCumInvoice.Visibility = 3
					End If
				Else
					mReceiptCumInvoice.StateCode = mVendor.StateCode
					mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
					mReceiptCumInvoice.VendorCountry = mVendor.CountryName
					mReceiptCumInvoice.Visibility = 3
				End If
			End If
		Else
			mReceiptCumInvoice.Visibility = 3
		End If
		mtmpItem = Nothing
		'----END GST--------------------------------------------------------------
		Return True
	End Function

	Private Sub ControlVisibility()

		Try

			If mReceiptCumInvoice.TransTypeID = Trans.ReceiptAgainstLoanIssuedToStore Then
				cmbStore.Enabled = False
			ElseIf mReceiptCumInvoice.TransTypeID = Trans.LoanTakenFromStore Then
				cmbStore.Enabled = False
			ElseIf mReceiptCumInvoice.TransTypeID = Trans.ReceiptAgainstLoanIssuedToAircraft Then
				cmbStore.Enabled = True
			ElseIf mReceiptCumInvoice.TransTypeID = Trans.ReceivedFromOtherStore Then
				cmbStore.Enabled = False
			ElseIf mReceiptCumInvoice.TransTypeID = Trans.ReceiptAgainstLoanIssuedToWorkShop Then
				cmbStore.Enabled = True
			ElseIf (mReceiptCumInvoice.TransTypeID = Trans.ReceiptcumInvoiceAgainstPuchaseOrder) And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = False Then
			End If

			txtOrderDate.Visible = Not (mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 57) '57 Added By Prashant 21-May-2010
			txtQuantity.Enabled = IIf(mReceiptCumInvoice.TransTypeID = 61, False, True)  'Added By Prashant 28-Jan-2011 

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "TAAL" Then 'Added by Archana on 4-Nov-2009
				lblBatchNo.Text = "RNN No."
			End If

			If (AppSettings("Barcode") IsNot Nothing) AndAlso AppSettings("Barcode") = "True" Then 'Added by vikrant on 26-aug-2011
				txtBarcodeNo.Visible = True
			End If

			If (AppSettings("CodeNo") = "True" And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2) Then

				lblCodeNo.Visible = True
				txtCodeNo.Visible = True
				'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
				lblCodeNo.Text = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "GSE No.", "Code No.")
				txtCodeNo.ToolTip = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "Enter GSE No.", "Enter Code No.")

			End If

			If mReceiptCumInvoice.TransTypeID = Trans.ReceivedFromAircraft Then 'Added by Vikrant on 7.3.12 FORALL03052012
				btnAlternatePart.Visible = False
			End If 'End

			'Added by Vikrant FOR ALL10052012-10
			If (chkIsExpiryNA.Checked Or chkIsExpiryUnlimited.Checked) And (AppSettings("ClientCode") <> "IND") Then 'IND'Commneted and Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020

				If chkIsExpiryNA.Checked Then
					chkIsExpiryUnlimited.Checked = False
					chkIsExpiryUnlimited.Enabled = False
				ElseIf chkIsExpiryUnlimited.Checked Then
					chkIsExpiryNA.Checked = False
					chkIsExpiryNA.Enabled = False
				End If

				txtStartDate.Enabled = False
				txtExpiryDate.Enabled = False
				txtCureQtrs.Enabled = False
				txtCureYear.Enabled = False
				txtExpQrts.Enabled = False
				txtExpYear.Enabled = False
				txtStartDate.Text = ""
				txtExpiryDate.Text = ""
				txtCureQtrs.Text = "0"
				txtCureYear.Text = "0"
				txtExpQrts.Text = "0"
				txtExpYear.Text = "0"

				If mReceiptCumInvoice.StatusID = 2 Then
					chkIsExpiryNA.Enabled = False
					chkIsExpiryUnlimited.Enabled = False
				End If

			Else

				txtStartDate.Enabled = (mReceiptCumInvoice.StatusID = 1)
				txtExpiryDate.Enabled = (mReceiptCumInvoice.StatusID = 1)
				txtCureQtrs.Enabled = (mReceiptCumInvoice.StatusID = 1)
				txtCureYear.Enabled = (mReceiptCumInvoice.StatusID = 1)
				txtExpQrts.Enabled = (mReceiptCumInvoice.StatusID = 1)
				txtExpYear.Enabled = (mReceiptCumInvoice.StatusID = 1)
				chkIsExpiryNA.Enabled = (mReceiptCumInvoice.StatusID = 1)
				chkIsExpiryUnlimited.Enabled = (mReceiptCumInvoice.StatusID = 1)

			End If
			'End

			'Added By Vikrant on 01-Aug-2012 For BA01082012
			If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then

				txtCRate.ReadOnly = True
				txtCRate.BackColor = Color.Gainsboro
				txtCOtherCharges.Visible = False
				lblOtherCharges.Visible = False
				txtDisplayCRate.ReadOnly = True
				txtDisplayCRate.BackColor = Color.Gainsboro
				txtDisplayCAmount.ReadOnly = False 'Added By Prashant 5-Feb-2019 ALL04022019 

			Else

				txtCRate.ReadOnly = False

				If (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
					txtCOtherCharges.Visible = False
					lblOtherCharges.Visible = False
				Else
					txtCOtherCharges.Visible = True
					lblOtherCharges.Visible = True
				End If

				txtDisplayCRate.ReadOnly = False
				txtDisplayCAmount.ReadOnly = True   'Added By Prashant 5-Feb-2019 ALL04022019 
				txtDisplayCAmount.BackColor = Color.Gainsboro   'Added By Prashant 5-Feb-2019 ALL04022019 

			End If

			If (mReceiptCumInvoice.TransTypeID = Trans.ExchangeRepairReceivedFromVendor Or
				mReceiptCumInvoice.TransTypeID = Trans.ReceiptasLoanFromSupplier Or
				mReceiptCumInvoice.TransTypeID = Trans.ReceivedfromSupplierRentalLease Or
			   (mReceiptCumInvoice.TransTypeID = Trans.RCIFromSupplierAsNone And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then

				txtCRate.ReadOnly = True
				txtCRate.BackColor = Color.Gainsboro
				lblAmount.Visible = False
				lblValues.Text = "Original Values"
				txtDisplayCRate.ReadOnly = True
				txtDisplayCRate.BackColor = Color.Gainsboro
				txtDisplayCAmount.Visible = False 'Added By Prashant 5-Feb-2019 ALL04022019 

			Else
				lblAmount.Visible = True
				lblValues.Text = "Values"
				txtDisplayCAmount.Visible = True 'Added By Prashant 5-Feb-2019 ALL04022019 
			End If

			imgPartNo.Enabled = (mReceiptCumInvoice.StatusID = 1)
			btnAlternatePart.Enabled = (mReceiptCumInvoice.StatusID = 1)
			chkRemovedasReturnableFromAircraft.Enabled = (mReceiptCumInvoice.StatusID = 1)
			ChkIsConsiderAsAsset.Enabled = (mReceiptCumInvoice.StatusID = 1)
			cmbPartType.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtQuantity.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtReleaseNote.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtReleaseNoteDate.Enabled = (mReceiptCumInvoice.StatusID = 1)
			cmbStore.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtSerialNo.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtLocation.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtCRate.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtDisplayCRate.Enabled = (mReceiptCumInvoice.StatusID = 1)   'Added By Prashant 5-Feb-2019 ALL04022019 
			txtCOtherCharges.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtCAmount.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtDisplayCAmount.Enabled = (mReceiptCumInvoice.StatusID = 1) 'Added By Prashant 5-Feb-2019 ALL04022019 
			txtCEffectiveRate.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtCommercialRate.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtGROCRate.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtGROCAmount.Enabled = (mReceiptCumInvoice.StatusID = 1)
			chkIsInWarranty.Enabled = (mReceiptCumInvoice.StatusID = 1)
			chkIsTransitDamage.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtCalibrationDoneOnDate.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtRemark.Enabled = (mReceiptCumInvoice.StatusID = 1 Or mReceiptCumInvoice.StatusID = 2)  ''APFT :ALL18012018 added Or mReceiptCumInvoice.StatusID = 2 by Saylee  on 18-Jan-2019 to open button after authorization ,to save rematk and note
			txtNote.Enabled = (mReceiptCumInvoice.StatusID = 1 Or mReceiptCumInvoice.StatusID = 2)  ''APFT :ALL18012018 added Or mReceiptCumInvoice.StatusID = 2 by Saylee  on 18-Jan-2019 to open button after authorization ,to save rematk and note
			txtPreviousWorkScope.Enabled = (mReceiptCumInvoice.StatusID = 1)
			ImgAddPeroid.Enabled = (mReceiptCumInvoice.StatusID = 1)
			txtWarrantyInDays.Enabled = (mReceiptCumInvoice.StatusID = 1)
			ImgPartType.Enabled = (mReceiptCumInvoice.StatusID = 1)
			btnOK.Enabled = (mReceiptCumInvoice.StatusID = 1 Or mReceiptCumInvoice.StatusID = 2)  ''APFT :ALL18012018 added Or mReceiptCumInvoice.StatusID = 2 by Saylee  on 18-Jan-2019 to open button after authorization ,to save rematk and note
			txtCodeNo.Enabled = (mReceiptCumInvoice.StatusID = 1)

			If (chkIsInWarranty.Checked = True And mReceiptCumInvoice.StatusID = 1) Then

				txtWarrantyInDays.Enabled = True

				If Val(txtWarrantyInDays.Text) > 0 Then
					txtWarrantyStartDate.Enabled = True
				Else
					txtWarrantyStartDate.Enabled = False
				End If

			Else
				txtWarrantyInDays.Enabled = False
				txtWarrantyStartDate.Enabled = False
			End If

			If (
					AppSettings("ClientCode") = "CE" And
					(
						mReceiptCumInvoice.TransTypeID = 6 Or
						mReceiptCumInvoice.TransTypeID = 7 Or
						mReceiptCumInvoice.TransTypeID = 10 Or
						mReceiptCumInvoice.TransTypeID = 27 Or
						mReceiptCumInvoice.TransTypeID = 48 Or
						mReceiptCumInvoice.TransTypeID = 54 Or
						mReceiptCumInvoice.TransTypeID = 67 Or
						mReceiptCumInvoice.FromTypeID = 16
					)
				) Then

				mIsOwnedByCustomer = IIf(cmbStore.SelectedIndex > 0, Store.GetStore(New Guid(cmbStore.SelectedValue)).IsOwnedByCustomer, False)

				If mIsOwnedByCustomer = False Then
					txtBatchNo.Enabled = False
				Else

					If mReceiptCumInvoice.StatusID = 2 Then
						txtBatchNo.Enabled = False
					Else
						txtBatchNo.Enabled = True
					End If

				End If

			Else
				txtBatchNo.Enabled = (mReceiptCumInvoice.StatusID = 1)
			End If

			'Added by Shital on 07-Sep-2016
			chkAirworthiness.Visible = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss
			cmbWarrantyStatus.Enabled = (mReceiptCumInvoice.StatusID = 1)
			cmbFaultFound.Enabled = (mReceiptCumInvoice.StatusID = 1)
			dgRCIAttachment.Columns(6).Visible = (mReceiptCumInvoice.StatusID = 1) 'Attachment Delete
			dgPeriods.Columns(7).Visible = (mReceiptCumInvoice.StatusID = 1) 'Remove

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibilityForExcessQty()
		If Val(txtExcessQty.Text) > 0 Then
			txtShortQty.Enabled = False
		Else
			txtShortQty.Enabled = (mReceiptCumInvoice.StatusID = 1)
		End If
		If Val(txtShortQty.Text) > 0 Then
			txtExcessQty.Enabled = False
		Else
			txtExcessQty.Enabled = (mReceiptCumInvoice.StatusID = 1)
		End If
	End Sub

	Private Sub SetGridObject() 'Added By Prashant 10-Feb-2010-----------------------------------------
		Dim i As Integer
		Dim txtTSNValue1 As TextBox
		Dim txtTSOHValue1 As TextBox
		Dim txtTSIValue1 As TextBox

		Dim txtCSNValue1 As TextBox
		Dim txtCSOValue1 As TextBox
		Dim txtCSIValue1 As TextBox
		For i = 0 To dgPeriods.Rows.Count - 1
			txtTSNValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtTSNValue"), TextBox)
			txtTSOHValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtTSOHValue"), TextBox)
			txtTSIValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtTSIValue"), TextBox)

			txtCSNValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtCSNValue"), TextBox)
			txtCSOValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtCSOValue"), TextBox)
			txtCSIValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtCSIValue"), TextBox)

			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtTSNValue1.Text) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSNValueFormatted = ""
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSNValueFormatted = Trim(txtTSNValue1.Text)
				End If
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSNValue = Trim(txtTSNValue1.Text)
			End If

			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtTSOHValue1.Text) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSOValueFormatted = ""
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSOValueFormatted = Trim(txtTSOHValue1.Text)
				End If
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSOValue = Trim(txtTSOHValue1.Text)
			End If


			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtTSIValue1.Text) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSIValueFormatted = ""
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSIValueFormatted = Trim(txtTSIValue1.Text)
				End If
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).TSIValue = Trim(txtTSIValue1.Text)
			End If




			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtCSNValue1.Text) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSNValueFormatted = ""
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSNValueFormatted = Trim(txtCSNValue1.Text)
				End If
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSNValue = Trim(txtCSNValue1.Text)
			End If


			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtCSOValue1.Text) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSOValueFormatted = ""
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSOValueFormatted = Trim(txtCSOValue1.Text)
				End If
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSOValue = Trim(txtCSOValue1.Text)
			End If



			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtCSIValue1.Text) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSIValueFormatted = ""
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSIValueFormatted = Trim(txtCSIValue1.Text)
				End If
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).CSIValue = Trim(txtCSIValue1.Text)
			End If


		Next i
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
	End Sub

	Private Sub SetReceiptItemKitItemsGridObject() 'Added By Prashant 8-Nov-2016
		Dim i As Integer
		Dim txtSerialNoForItemIDOfKitItem As TextBox
		Dim txtRemark As TextBox
		For i = 0 To dgReceiptItemKitItems.Rows.Count - 1
			txtSerialNoForItemIDOfKitItem = CType(Me.dgReceiptItemKitItems.Rows(i).FindControl("txtSerialNoForItemIDOfKitItem"), TextBox)
			txtRemark = CType(Me.dgReceiptItemKitItems.Rows(i).FindControl("txtRemark"), TextBox)
			'If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew And Not Session("Edit") Then
			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew Then
				If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.Contains(New Guid(Me.dgReceiptItemKitItems.Rows(i).Cells(5).Text), "") Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.Add(ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID, ItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID, SerialNo:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
				End If
			End If
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(i).SerialNoForItemIDOfKitItem = Trim(txtSerialNoForItemIDOfKitItem.Text)
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(i).Remark = Trim(txtRemark.Text) '4 Remark
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(i).ItemIDFromKitItem = New Guid(Me.dgReceiptItemKitItems.Rows(i).Cells(5).Text) '5 ItemIDFromKitItem
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(i).ItemName = Me.dgReceiptItemKitItems.Rows(i).Cells(0).Text '0 ItemName
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(i).ItemDescription = Me.dgReceiptItemKitItems.Rows(i).Cells(1).Text '1 ItemDescription
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(i).KitItemQty = CType(Me.dgReceiptItemKitItems.Rows(i).Cells(2).Text, Decimal) '1 ItemDescription
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(i).KitItemID = New Guid(Me.dgReceiptItemKitItems.Rows(i).Cells(6).Text) '6 KitItemID
		Next i
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
	End Sub

	Private Sub SetPeroids()
		Dim mPeriodlist As PeriodList
		Dim mListOfPeriod As List(Of PeriodInfo) = New List(Of PeriodInfo)
		mSelectPeriods = SelectPeriods.NewSelectPeriods
		mPeriodlist = PeriodList.GetPeriodList
		mListOfPeriod = (From c As PeriodInfo In mPeriodlist
						 Where c.ID = 1 Or c.ID = 3
						 Select c).ToList
		For i As Integer = 0 To mListOfPeriod.Count - 1
			If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.Contains(mListOfPeriod(i).ID) Then
				mSelectPeriods.Add(mListOfPeriod(i).ID, mListOfPeriod(i).PeriodName)
			End If
		Next
		Session("mSelectPeriods") = mSelectPeriods
	End Sub

	Private Sub AddSelectedPeroids()
		Dim mSelectPeriod As SelectPeriod
		If IsNothing(mSelectPeriods) Then
			mSelectPeriods = SelectPeriods.NewSelectPeriods
		End If
		For Each mSelectPeriod In mSelectPeriods
			If mSelectPeriod.IsSelected Then
				'mReceiptCumInvoice.Receipt.ReceiptItems.CurrentItem.ReceiptItemPeriods.Add(ReceiptItemPeriod.NewReceiptItemPeriod(mReceiptCumInvoice.Receipt.ReceiptItems.CurrentItem.ID, mReceiptCumInvoice.Receipt.TransTypeID, mSelectPeriod.PeriodID))
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.Add(ReceiptItemPeriod.NewReceiptItemPeriod(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID, mReceiptCumInvoice.Receipt.TransTypeID, mSelectPeriod.PeriodID))
			End If
		Next
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		Session.Remove("mSelectPeriods")
		mSelectPeriods = Nothing
	End Sub

	Private Sub AddReceiptItemServiceInspections() 'Added By Prashant 30-Sep-2019
		'Dim mSelectPeriod As SelectPeriod
		'If IsNothing(mSelectPeriods) Then
		'    mSelectPeriods = SelectPeriods.NewSelectPeriods
		'End If
		Dim mItemForServiInspec As ItemServiceInspectionsList = Nothing
		mItemForServiInspec = ItemServiceInspectionsList.GetItemServiceInspectionsList(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
		If mItemForServiInspec.Count >= 1 Then
			For i As Integer = 0 To mItemForServiInspec.Count - 1
				If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections Is Nothing Then
					If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Contains(mItemForServiInspec(i).ID) = True Then
						'Do nothing
					Else
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Add(ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																															ItemServiceInspectionsID:=mItemForServiInspec(i).ID,
																															ItemServiceInspectionDescription:=mItemForServiInspec(i).Description,
																															ItemServiceInspectionFrequency:=mItemForServiInspec(i).Frequency,
																															ItemServiceInspectionFrequencyPeriod:=mItemForServiInspec(i).FrequencyPeriod,
																															ItemID:=mItemForServiInspec(i).ItemID.ToString)
					End If
				End If
			Next
		End If
		If (mReceiptCumInvoice.TransTypeID = 61 Or mReceiptCumInvoice.TransTypeID = 62) Then
			mLastServicedInspectedDoneOnDate = LastServicedInspectedDoneOnDate.GetLastServicedInspectedDoneOnDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString,
																											  mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
			If mLastServicedInspectedDoneOnDate.Count > 0 Then
				For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Count - 1
					For j As Integer = 0 To mLastServicedInspectedDoneOnDate.Count - 1
						If mLastServicedInspectedDoneOnDate(j).ItemServiceInspectionsID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ItemServiceInspectionsID Then
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ServiedInspectedCheckDoneOnDate = mLastServicedInspectedDoneOnDate(j).DoneOnDateForServiceInspectedCheck
						End If
					Next
				Next
				upnlConditionCheckInfo.Update()
				mLastServicedInspectedDoneOnDate = Nothing
			End If
		End If
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		mItemForServiInspec = Nothing
	End Sub

	Private Sub ControlVisibilityForExpCalibration()
		If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAttachmentAdded Then
			'ImageButton1.Visible = True
			'btnDelAttach1.Enabled = (mReceiptCumInvoice.StatusID = 1)
		Else
			'ImageButton1.Visible = False
			'btnDelAttach1.Enabled = False
		End If
		upnlAttachment.Update()
	End Sub

	''Added by Shital on 25-Jun-2020
	Private Sub AttachMyFile()
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAttachmentAdded = True
		'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Add(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension)
		'Session("mReceiptCumInvoice") = mReceiptCumInvoice
		Dim BackupPath As String = ""
		BackupPath = AppSettings("DOCPath") & "New.PDF"
		mReceiptCumInvoice = Session("mReceiptCumInvoice")
		Try
			If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Contains(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then

				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Add(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID, CType(Session("FileUpload.FileName"), String))
				' mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Add(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension)
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.CurrentItem.ImageFile = mFileAttach.ImageFile 'CType(Session("ImageFile"), Byte())
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.CurrentItem.Size = mFileAttach.Size 'Session("Size")
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.CurrentItem.Extension = mFileAttach.Extension 'Session("Extension")

				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				dgRCIAttachment.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments
				dgRCIAttachment.DataBind()

				For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Count - 1
					Dim txtValue As TextBox
					txtValue = CType(Me.dgRCIAttachment.Rows(i).FindControl("txtFileName"), TextBox)
					txtValue.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments(i).FileName
				Next

				Session.Remove("Size")
				Session.Remove("ImageFile")
				Session.Remove("Extension")
				Session.Remove("FileUpload.FileName")
				upnlRCIAttachment.Update()
				upnldgRCIAttachment.Update()
			Else
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		Catch ex As Exception
		End Try
	End Sub

	Private Sub DeleteAttachment(Index As Int32)
		MSGBoxCtrl.Show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.CurrentIndex = Index
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
	End Sub
	'------------------

	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							Dim mReceiptCumInvoice As ReceiptCumInvoice
							mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.RemoveAt(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentIndex)
							Session("mReceiptCumInvoice") = mReceiptCumInvoice
							DataFieldBind()
							upnlTSNTSOValues.Update()
						Catch ex As SqlException
							If ex.Number = 547 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If
						Finally
						End Try
					End If
					If MSGBoxCtrl.Sender = "StoreTag" Then
						'Added by Shital on 11-May-2021
						If mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Then
							Dim OrderCRate As Decimal = 0.0
							Dim ExtraMessage As String = ""
							OrderCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate + (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate * (10 / 100))
							ExtraMessage = "order rate " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate.ToString + " is not matching with receiving rate do you want to continue"
							If (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate > OrderCRate And mReceiptCumInvoice.TransTypeID = 7) Or (mReceiptCumInvoice.TransTypeID = 10 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate > OrderCRate) Then
								MSGBoxCtrl.Show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "DifferRCIRate")
								Exit Sub
							Else
								ReceiptCumInvoiceItems()
							End If
						Else
							ReceiptCumInvoiceItems()
						End If
						'-------
						' 'Commented by Shital on 11-May-2021
						'ReceiptCumInvoiceItems()

					End If
					If MSGBoxCtrl.Sender = "RemoveAttachment" Then


						Try
							Session("Sender") = ""
							Dim mnWO As nWO
							mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.CurrentItem)
							If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Count = 0 Then
								mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAttachmentAdded = False
							End If
							dgRCIAttachment.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments
							dgRCIAttachment.DataBind()
							upnldgRCIAttachment.Update()
							upnlRCIAttachment.Update()
							Session("mReceiptCumInvoice") = mReceiptCumInvoice

						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							End If
						End Try
					End If
					'Addedby Shital on 11-May-2021
					If MSGBoxCtrl.Sender = "DifferRCIRate" Then
						ReceiptCumInvoiceItems()
						MarkLog(Action.Save, "RCI", "RCI Rate. Changed by " + User.Identity.Name + " on " + Today.Date.ToString, ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
					End If
					'----
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						DataFieldBind()
					End If
					If MSGBoxCtrl.Sender = "Delete" Then
						DataFieldBind()
					End If
					If MSGBoxCtrl.Sender = "StoreTag" Then
						DataFieldBind()
					End If
					'Addedby Shital on 11-May-2021
					If MSGBoxCtrl.Sender = "DifferRCIRate" Then
						DataFieldBind()
					End If
					'---
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "ReductionFromValuation" Then
						RemoveSessions()
						Session.Remove("tmpReceiptCumInvoice")
						Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
					End If
					If MSGBoxCtrl.Sender = "WarrantyInfo" Then
						RemoveSessions()
						Session.Remove("tmpReceiptCumInvoice")
						Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
					End If
					If MSGBoxCtrl.Sender = "PreMatureFailure" Then
						RemoveSessions()
						Session.Remove("tmpReceiptCumInvoice")
						Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
					End If
					If MSGBoxCtrl.Sender = "ResetStore" Then  ''Added By Prashant 13-May-2020
						cmbStore.ClearSelection()
						upnlStore.Update()
					End If
			End Select
		End If
	End Sub

	Private Sub ControlVisibilityForExpiryInfo()

		Try

			If (
				(txtStartDate.Text <> "" Or txtExpiryDate.Text <> "") Or (txtCureQtrs.Text <> "0" And txtCureQtrs.Text <> "") Or
				(txtCureYear.Text <> "0" And txtCureYear.Text <> "") Or (txtExpQrts.Text <> "0" And txtExpQrts.Text <> "") Or
				(txtExpYear.Text <> "0" And txtExpYear.Text <> "")
			   ) And (AppSettings("ClientCode") <> "IND") Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020

				chkIsExpiryNA.Enabled = False
				chkIsExpiryUnlimited.Enabled = False

			ElseIf Session("EditForExpiryInfo") = "True" Then

				Session("EditForExpiryInfo") = "False"
				chkIsExpiryNA.Enabled = IIf(chkIsExpiryNA.Checked, True, False)
				chkIsExpiryUnlimited.Enabled = IIf(chkIsExpiryUnlimited.Checked, True, False)

			Else
				chkIsExpiryNA.Enabled = True
				chkIsExpiryUnlimited.Enabled = True
			End If

			'If AppSettings("ClientCode") = "Heligo" AndAlso
			'   mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth = 0 AndAlso
			'   mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter = 0 Then

			'	pnlExpiryDetails.Enabled = False
			'	pnlExpiryDetails.CssClass &= " disabled-panel"
			'	pnlExpiryDetails.Attributes("data-message") = "Control as disabled as Expiry Details are not mentioned in Part Master."

			'End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetControl() 'Added By Prashant On 07-Oct-2015 For ALL06102015
		mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, txtSerialNo.Text)

		If Not mLastWarrantyInformation Is Nothing Then
			If mLastWarrantyInformation.Count > 0 Then
				txtManufacturingDate.Text = mLastWarrantyInformation(0).ManufacturingDateFormatted.ToString  'Added by Saylee on 9-Mar-2021 for Heligo10032021

				txtStartDate.Text = mLastWarrantyInformation(0).StartDate.ToString
				txtExpiryDate.Text = mLastWarrantyInformation(0).ExpiryDate.ToString
				txtExpQrts.Text = mLastWarrantyInformation(0).ExpQtrs
				txtExpYear.Text = mLastWarrantyInformation(0).ExpYear
				txtCureQtrs.Text = mLastWarrantyInformation(0).CureQtrs
				txtCureYear.Text = mLastWarrantyInformation(0).CureYear
				chkIsExpiryNA.Checked = mLastWarrantyInformation(0).IsExpiryNA
				chkIsExpiryUnlimited.Checked = mLastWarrantyInformation(0).IsExpiryUnlimited
				upnlExpiryInformation.Update()

			Else
				txtManufacturingDate.Text = ""
			End If
		End If

		If (mReceiptCumInvoice.TransTypeID <> 7) Then
			mLastServicedInspectedDoneOnDate = LastServicedInspectedDoneOnDate.GetLastServicedInspectedDoneOnDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, txtSerialNo.Text)
			If mLastWarrantyInformation.Count > 0 Then
				txtCodeNo.Text = mLastWarrantyInformation(0).CodeNo
				'txtCodeNo.Enabled = False
				If (mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 61 Or mReceiptCumInvoice.TransTypeID = 62) Then
					txtCalibrationDoneOnDate.Text = mLastWarrantyInformation(0).LastCalibrationDoneOnDateFormatted.ToString
					upnlCalibrationInfo.Update()
				End If

				mLastWarrantyInformation = Nothing
			Else
				txtCodeNo.Text = ""
				txtCodeNo.Enabled = True
				txtCalibrationDoneOnDate.Text = ""

			End If
			If mLastServicedInspectedDoneOnDate.Count > 0 Then
				If (mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 61 Or mReceiptCumInvoice.TransTypeID = 62) Then
					For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Count - 1
						For j As Integer = 0 To mLastServicedInspectedDoneOnDate.Count - 1
							If mLastServicedInspectedDoneOnDate(j).ItemServiceInspectionsID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ItemServiceInspectionsID Then
								mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ServiedInspectedCheckDoneOnDate = mLastServicedInspectedDoneOnDate(j).DoneOnDateForServiceInspectedCheck
								'Exit For
							End If
						Next
					Next
					upnlConditionCheckInfo.Update()
				End If
				mLastServicedInspectedDoneOnDate = Nothing
			Else
				upnlConditionCheckInfo.Update()
			End If
			upnlReceivingInformation1.Update()
		End If
		'Added By Prashant 8-Nov-2016
		dgReceiptItemKitItems.DataSource = ReceiptItemKitItems.GetReceiptItemKitItemsList(ItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, SerialNo:=txtSerialNo.Text.Trim)
		dgReceiptItemKitItems.DataBind()
		upnlReceiptItemKitItems.Update()
		'End
	End Sub

	Private Sub ReceiptCumInvoiceItems()
		If TotalCount <= 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized Then
			Session.Remove("Edit")
			Response.Redirect(Request.QueryString("BackPage"))
		End If
		If Not CustomValidate2() Then Exit Sub
		If AppSettings("ClientCode") = "Taj" Then  'Added By Prashant 0n 8-Jan-2021 As Per Taj Client requiremet 
			'Do nothing
		Else
			mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, txtSerialNo.Text)
			If Not mLastWarrantyInformation Is Nothing Then
				''Added by Saylee on 9-Mar-2021 for Heligo10032021
				If mLastWarrantyInformation.Count > 0 Then
					If txtManufacturingDate.Text = "" Then
						txtManufacturingDate.Text = mLastWarrantyInformation(0).ManufacturingDateFormatted.ToString
					End If
				End If
			End If
			'*****************************
			If ((mReceiptCumInvoice.TransTypeID <> 7 Or mReceiptCumInvoice.TransTypeID <> 10 Or mReceiptCumInvoice.TransTypeID <> 8 _
				 Or mReceiptCumInvoice.TransTypeID <> 11 Or mReceiptCumInvoice.TransTypeID <> 12 Or mReceiptCumInvoice.TransTypeID <> 13 _
				 Or mReceiptCumInvoice.TransTypeID <> 27 Or mReceiptCumInvoice.TransTypeID <> 28 Or mReceiptCumInvoice.TransTypeID <> 47 _
				 Or mReceiptCumInvoice.TransTypeID <> 54) And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True) Then

				If mReceiptCumInvoice.TransTypeID <> 53 Then  'Received from customer as new
					If mLastWarrantyInformation.Count > 0 Then
						If chkIsInWarranty.Checked = False Then
							chkIsInWarranty.Checked = mLastWarrantyInformation(0).IsWarranty
							txtWarrantyInDays.Text = mLastWarrantyInformation(0).WarrantyInDays
							txtWarrantyStartDate.Text = mLastWarrantyInformation(0).WarrantyStartDateFormatted.ToString
							txtWarrantyExpiryDate.Text = mLastWarrantyInformation(0).WarrantyExpiryDateFormatted.ToString
							mLastWarrantyInformation = Nothing
						End If
					End If
				ElseIf mReceiptCumInvoice.TransTypeID = 53 Then 'Received from customer as new
					If chkIsInWarranty.Checked = False Then
						chkIsInWarranty.Checked = True
						txtWarrantyInDays.Text = Val(AppSettings("WarrantyForNewOH"))
						txtWarrantyStartDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted.ToString
						txtWarrantyExpiryDate.Text = CDate(DateAdd(DateInterval.Day, Val(txtWarrantyInDays.Text), CDate(txtWarrantyStartDate.Text))).ToString(AppSettings("DateFormat").ToString)
					End If
				End If
			End If
		End If
		'SetControl()
		If SetObject() Then

			'***********************************
			If mReceiptCumInvoice.TransTypeID = 9 Then
				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartMatchingInMaint(mReceiptCumInvoice.AircraftID) Then
					'do nothing : already CompstatusID set in this method
				Else

					If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartialPartSerialNoMatch(mReceiptCumInvoice.AircraftID) Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GetCompStatusDetails(mReceiptCumInvoice.AircraftID)

						txtCompStatusPart.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompPartNo
						txtCompSerialNo.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompSerialNo
						txtCompDesc.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompDescription
						txtCompRegNo.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompRegNo
						txtCompRemoveDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompRemovedOnDate
						upnlCompstatus.DataBind()
						upnlCompstatus.Update()
						lnkCompStatus_ModalPopupExtender.Show()
						isPOPShown = True
					Else
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompStatusID = Guid.Empty
					End If
				End If
			End If
			'***********************************


			If (AppSettings("CodeNo") = "True" And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2 And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartFromListisSerialized = True)) Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = txtCodeNo.Text.Trim   'Added By Prashant On 07-Oct-2015 For ALL06102015
				If (mReceiptCumInvoice.Receipt.ReceiptItems.ContainsCodeNo(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem) = True) Then 'Added By Prashant On 07-Oct-2015 For ALL06102015    
					MSGBoxCtrl.Show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You can not add duplicate Code No.", MsgBoxStyle.OkOnly, "")
					mReceiptCumInvoice.CancelEdit()
					Exit Sub
				Else
					mReceiptCumInvoice.ApplyEdit()
				End If
			End If
			SetGridObject()
			SetReceiptItemKitItemsGridObject()
			TotalCount -= 1
			Session("TotalCount") = TotalCount
			If TotalCount > 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized Then
				NewReceiptCumInvoiceItem(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
				Session.Remove("Edit")
				Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
			End If
			Session("mReceiptCumInvoice") = mReceiptCumInvoice
			Session.Remove("Edit")
			'Added by Prashant  16-Jul-2013 'ALL15072013
			If (Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.AlternateItemID.Equals(Guid.Empty) And (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 54)) Then
				Session("Note") = "Order Part is amended as alternate part is received."
			End If
			'-------------------------------------------
			'---Added By Prashant 12-Nov-2014------------------------------
			If (Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.AlternateItemID.Equals(Guid.Empty) And (mReceiptCumInvoice.TransTypeID = 66) And mReceiptCumInvoice.IsNew = True) Then 'Received From Aircraft As Core Unit return
				Dim ItemNameOfIssue As String = ""
				ItemNameOfIssue = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID).IssueItemDetailForReceipt.ItemName
				If ItemNameOfIssue.Equals(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName) Then
					'Do Nothing
				Else
					Dim Str As String = ""
					'Str = "Item was amended to " + ItemNameOfIssue + ", as receiving item is " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName + ", Which is alternate part of " + ItemNameOfIssue
					Str = "As Receiving Part Is " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName + " , Which Is Alternate Of " + ItemNameOfIssue + ". " + vbCrLf + ItemNameOfIssue + " Has Been Reduced From Asset And " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName + " Will Get Considered In Asset With Same Value Of " + ItemNameOfIssue
					MSGBoxCtrl.Show("Alert!", Str, "", MsgBoxStyle.OkOnly, "ReductionFromValuation")
					Exit Sub
				End If
			End If
			'---End OF Added By Prashant 12-Nov-2014------------------------------
			'---Added By Prashant 20-Feb-2015------------------------------
			If ((mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 61 Or mReceiptCumInvoice.TransTypeID = 66) And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew = True And lblPartStatus.Text = "Unserviceable" And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True) Then 'Received From Aircraft As Core Unit return
				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate.ToString <> "" Then
					If DateDiff(DateInterval.Day, mReceiptCumInvoice.RecCumInvDate, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate) > 0 Then
						Dim Str As String = ""
						Str = "Receiving part : " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName + " (" + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo + ") " + "is under warranty. " + "</br>Warranty start date : " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDateFormatted.ToString + "</br>Expiry date : " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDateFormatted.ToString
						MSGBoxCtrl.Show("Alert!", Str, "", MsgBoxStyle.OkOnly, "WarrantyInfo")
						Exit Sub
					End If
				End If
			End If
			If (mReceiptCumInvoice.TransTypeID = 9 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew = True) Then
				mDateForPreMatureFailure = DateForPreMatureFailure.GetDateForPreMatureFailure(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, txtSerialNo.Text.Trim, mReceiptCumInvoice.RecCumInvDate, mReceiptCumInvoice.AircraftID.ToString)
				If mDateForPreMatureFailure.Count > 0 Then
					Dim Str As String = ""
					If (mDateForPreMatureFailure(0).NoOfDays < 365 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeName = "Overhaul") Then
						MSGBoxCtrl.Show("Alert Pre-Mature Failure!", "The received part is removed from aircraft before 1 year for Overhaul" + "</br>And will be consider as Pre-Mature Failure", "", MsgBoxStyle.OkOnly, "PreMatureFailure")
						Exit Sub
					ElseIf (mDateForPreMatureFailure(0).NoOfDays < 180 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeName = "Repaired") Then
						MSGBoxCtrl.Show("Alert Pre-Mature Failure!", "The received part is removed from aircraft before 6 months for Repaire" + "</br>And will be consider as Pre-Mature Failure", "", MsgBoxStyle.OkOnly, "PreMatureFailure")
						Exit Sub
					End If
				End If
			End If
			'---End OF Added By Prashant 20-Feb-2015------------------------------
			If isPOPShown = False Then
				RemoveSessions()
				Session.Remove("tmpReceiptCumInvoice")
				Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			End If
		End If
	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		'Deciding IsInRole String to check Rights
		'        Select Case mTransTypeID                             'Commented By Prashant 17-Aug-2011
		Select Case mReceiptCumInvoice.TransTypeID                    'Added By Prashant 17-Aug-2011
			Case Trans.ReceiptcumInvoiceAgainstPuchaseOrder
				IsInRoleString = "RCIFromPO"
			Case Trans.ReceiptAgainstLoanIssueToVendor
				IsInRoleString = "RCIFromVendorForLoanReturn"
			Case Trans.ExchangeRepairReceivedFromVendor
				IsInRoleString = "RCIFromVendor"
			Case Trans.ReceivedFromAircraft
				IsInRoleString = "RCIFromAircraft"
			Case Trans.ReceiptAgainstLoanIssuedToAircraft
				IsInRoleString = "RCIFromAircraftForLoanReturn"
			Case Trans.ReceivedFromOtherStore
				IsInRoleString = "RCIFromStore"
			Case Trans.LoanTakenFromStore
				IsInRoleString = "RCIFromStoreForLoan"
			Case Trans.ReceiptAgainstLoanIssuedToStore
				IsInRoleString = "RCIFromStoreForLoanReturn"
			Case Trans.ReceiptAgainstLoanIssueToCustomer
				IsInRoleString = "RCIFromCustomerForLoanReturn"
			Case Trans.AssembledFromWorkShop
				IsInRoleString = "AssembledFromWorkShop"
			Case Trans.ReceiptAgainstLoanIssuedToWorkShop
				IsInRoleString = "RCIFromWorkShopForLoanReturn"
			Case Trans.RCIFromWorkOrderAsReturn
				IsInRoleString = "RCIFromWorkOrderReturn"
			Case Trans.RCIFromAircraftAsCoreUnitReturn
				IsInRoleString = "RCIFromAircraftAsCoreUnitReturn"
			Case Trans.RCIFromSupplierAsNone
				IsInRoleString = "RCIFromSupplierAsNone"
			Case Trans.DisassembledFromWorkShop
				IsInRoleString = "DisassembledFromWorkShop"
			Case Trans.ReceivedfromSupplierRentalLease
				IsInRoleString = "ReceivedfromSupplierRentalLease"
			Case Trans.ReceiptasLoanFromSupplier
				IsInRoleString = "ReceiptasLoanFromSupplier"
			Case Trans.ReceiptasLoanFromCustomer
				IsInRoleString = "ReceiptasLoanFromCustomer"
			Case Trans.ReceiptFromCustomer
				IsInRoleString = "RCIFromCustomer"
			Case Trans.ReceivedFromCustomerAsForRepair
				IsInRoleString = "ReceivedFromCustomerAsForRepair"
			Case Trans.RCIFromWorkOrder
				IsInRoleString = "RCIFromWorkOrder"
			Case Trans.ReceivedFromWorkShopAsServiceableReturned        'Added By Prashant 10-Sep-2014 'ALL10092014
				IsInRoleString = "ReceivedFromWorkShopAsServiceablReturned"
		End Select
		'IsInRoleString = "ReceiptCumInvoice"
		'Depending upon decided IsInRole String; checkign Rights of the User
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
			Case Rights.Authorized                              'Added By Prashant 17-Aug-2011
				Return User.IsInRole(IsInRoleString + "Authorized")
		End Select
	End Function

#End Region

#Region " Custom Validation(s) "

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
		Dim mItem As Item
		mItem = Item.GetItem(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "cmbStore" Then
			If cmbStore.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Please Select the Store."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtExpiryDate" Then
			If (Not txtExpiryDate.Text = "" And txtStartDate.Text = "") And
				((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And
				((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And
				((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth <> 0 Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter <> 0)) Then
				custValidator.ErrorMessage = "Select Cure Date "
				e.IsValid = False
			ElseIf IsDate(txtExpiryDate.Text) And IsDate(txtStartDate.Text) Then
				If CDate(txtExpiryDate.Text) < CDate(txtStartDate.Text) Then
					custValidator.ErrorMessage = "Expiry date should be Later to Cure Date."
					e.IsValid = False
				Else
					e.IsValid = True
				End If
				'ElseIf (Not txtExpiryDate.Text = "" And txtStartDate.Text = "") And _
				'    ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And _
				'    ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And _
				'    (mItem.IsExpiryItem = True) Then  'Added by Prashant On 10-Aug-2020 All10082020
				'    custValidator.ErrorMessage = "Enter Expiry Information"
				'    e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtStartDate" Then
			If (txtExpiryDate.Text = "" And Not txtStartDate.Text = "") And
			  ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And
			  ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And
			  ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth <> 0 Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter <> 0)) _
			  And Not (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "IND") Then

				custValidator.ErrorMessage = "Select Expiry Date "
				e.IsValid = False

			ElseIf (txtExpiryDate.Text = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) _
				And (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
				custValidator.ErrorMessage = "Select Expiry Date "
				e.IsValid = False
				'ElseIf (txtExpiryDate.Text = "" And Not txtStartDate.Text = "") And _
				'       ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And _
				'       ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And _
				'        (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo") And (mItem.IsExpiryItem = True) Then 'Added by Prashant On 10-Aug-2020 All10082020

				'    custValidator.ErrorMessage = "Select Expiry Date "
				'    e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtQuantity" Then
			Dim mQtyBalReceived As Decimal = 0
			If Session("Edit") = True Then
				mQtyBalReceived = Session("mQtyBalReceived")
			Else
				mQtyBalReceived = CDec(Session("mTotalPendingItemQty"))
				Session("mQtyBalReceived") = mQtyBalReceived
			End If
			If Val(txtQuantity.Text) <= 0 Then
				custValidator.ErrorMessage = "Quantity shoud be non-zero Positive integer."
				e.IsValid = False
			ElseIf Val(txtQuantity.Text) > mQtyBalReceived And Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized And Session("Pending") = True Then
				custValidator.ErrorMessage = "Goods Receipt Part Qty can not be greater than Pending Qty"
				e.IsValid = False
			ElseIf Val(txtQuantity.Text) <> 1 And Len(txtSerialNo.Text.Trim) = 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized Then
				custValidator.ErrorMessage = "Serialized Part should be a Single Part. <BR> Serial No. Required Since Part is Serialized."
				e.IsValid = False
			ElseIf Val(txtQuantity.Text) <> 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized Then
				custValidator.ErrorMessage = "Serialized Part should be a Single Part."
				e.IsValid = False
			ElseIf Len(txtSerialNo.Text.Trim) = 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized Then
				custValidator.ErrorMessage = "Serial No. Required Since Part is Serialized."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtCOtherCharges" Then
			If CDec(Val(txtCOtherCharges.Text)) < 0 Then
				custValidator.ErrorMessage = "Other charges shoud be Positive integer."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtSerialNo" Then
			If Len(txtSerialNo.Text) > 50 Then
				custValidator.ErrorMessage = "Maximun Length of SerialNo. should be 50."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtLocation" Then
			If Len(txtLocation.Text) > 50 Then
				custValidator.ErrorMessage = "Maximun Length of Location should be 50."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
			If Len(txtLocation.Text) = 0 And AppSettings("ClientCode") = "Novo" Then
				custValidator.ErrorMessage = "Bin Location Require."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtRemark" Then
			If Len(txtRemark.Text) > 500 Then
				custValidator.ErrorMessage = "Maximun Length of Remark should be 500."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtNote" Then
			If Len(txtNote.Text) > 500 Then
				custValidator.ErrorMessage = "Maximun Length of Note should be 500."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtPreviousWorkScope" Then
			If Len(txtPreviousWorkScope.Text) > 500 Then
				custValidator.ErrorMessage = "Maximun Length of Previous Work Scope should be 500."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "cmbPartType" Then
			If cmbPartType.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Select Part Type From List"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtCureQtrs" Then
			If (Not txtExpiryDate.Text = "" Or Not txtStartDate.Text = "") And
				(Val(txtCureQtrs.Text) <> 0 Or Val(txtCureYear.Text) <> 0 Or Val(txtExpQrts.Text) <> 0 Or Val(txtExpYear.Text) <> 0) Then
				custValidator.ErrorMessage = "Enter either Cure/Expiry Date or Cure/Expiry Quarters."
				e.IsValid = False
			ElseIf Val(txtCureQtrs.Text) < 0 Or Val(txtCureQtrs.Text) > 4 Then
				custValidator.ErrorMessage = "Cure Quarters should be between 1 to 4"
				e.IsValid = False
			ElseIf (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0") Then
				custValidator.ErrorMessage = "Cure Year also required with Cure Qtrs."
				e.IsValid = False
			ElseIf (mItem.StatusEquipment = True And mItem.BenchmarkMonths > 0 And mItem.CalibrationPeriodInID > 0) Then
				If txtCalibrationDoneOnDate.Text.ToString = "" Then
					custValidator.ErrorMessage = "Part is Calibrated so Calibration Start Date required"
					e.IsValid = False
				ElseIf txtManufacturingDate.Text.ToString = "" And AppSettings("ClientCode") = "Heligo" Then
					custValidator.ErrorMessage = "Part is Calibrated so Manufacturing Date required"
					e.IsValid = False
				ElseIf txtManufacturingDate.Text.ToString <> "" Then
					If CDate(txtCalibrationDoneOnDate.Text) < CDate(txtManufacturingDate.Text) Then
						custValidator.ErrorMessage = "Manufacturing Date should be less than or equal to Calibration Date"
						e.IsValid = False
					End If
				ElseIf CDate(txtCalibrationDoneOnDate.Text) > Today.Date Then
					custValidator.ErrorMessage = "Calibration date should not be greater than today's date"
					e.IsValid = False
				Else 'Added By Vikrant On 17-Jul-2018 For ALL17072018-1
					Dim mCalibrationItemChildList As CalibrationItemChildList
					Dim moldCalibrationItemChild As CalibrationItemChild

					mCalibrationItemChildList = CalibrationItemChildList.GetCalibrationChildList(FromDate:="1/1/1900", ToDate:="1/1/3300", ItemName:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName, Description:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemDescription, SerialNo:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo, ReceiptItemIDToBeSkipped:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID.ToString)
					If mCalibrationItemChildList.Count > 0 Then
						moldCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(mCalibrationItemChildList(0).ID)
						If CDate(txtCalibrationDoneOnDate.Text) < CDate(moldCalibrationItemChild.DoneOnDate) Then
							custValidator.ErrorMessage = "Calibration Date should be greater than or equal to Last Calibration date (" + moldCalibrationItemChild.DoneOnDateFormatted.ToString + ")"
							e.IsValid = False
						End If
					End If
				End If
			ElseIf ((txtExpiryDate.Text = "" And txtStartDate.Text = "")) And
				((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
				((txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0")) And
				((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth <> 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter <> 0)) Then
				custValidator.ErrorMessage = "Expiry Year and Expiry Quarters required."
				e.IsValid = False
				'ElseIf ((txtExpiryDate.Text = "" And txtStartDate.Text = "")) And _
				'    ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And _
				'    ((txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0")) And _
				'    (mItem.IsExpiryItem = True) Then 'Added by Prashant On 10-Aug-2020 All10082020

				'    custValidator.ErrorMessage = "Enter Expiry Information"
				'    e.IsValid = False

			ElseIf ((
					 (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth <> 0 _
					  Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter <> 0) And
					 Not (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "IND")
					) And
					((txtStartDate.Text = "" And txtExpiryDate.Text = "") And
					((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
					((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")))) Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				custValidator.ErrorMessage = "As " & mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryPeriod & ". Enter Expiry Information"
				e.IsValid = False
				'ElseIf (((mItem.IsExpiryItem = True) And (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo")) And _
				'        ((txtStartDate.Text = "" And txtExpiryDate.Text = "") And _
				'        ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And _
				'        ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")))) Then  'Added by Prashant On 10-Aug-2020 All10082020
				'    custValidator.ErrorMessage = "Enter Expiry Information"
				'    e.IsValid = False
			ElseIf ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or
				   ((AppSettings("ClientCode") = "IND" Or Appsettings("ClientCode") = "Heligo") And mItem.IsExpiryItem = True)) And
				   ((txtStartDate.Text = "" And txtExpiryDate.Text = "") And
				   ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
				   ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")) And
				   (chkIsExpiryNA.Checked = False) And (chkIsExpiryUnlimited.Checked = False))) Then  'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				custValidator.ErrorMessage = "Enter Expiry Information"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
			'ElseIf custValidator.ControlToValidate = "txtConditionCheckDoneOnDate" Then

			'    If (txtConditionCheckDoneOnDate.Text = "" And ((mItem.IsConditionCheck = True And mItem.ConditionCheckInterval > 0 And mItem.ConditionCheckIntervalIn > 0))) Then
			'        If mItem.IsConditionCheck = True Then
			'            custValidator.ErrorMessage = "Part is Condition Checked so Condition Check Start Date required"
			'        End If
			'        e.IsValid = False
			'    End If
		ElseIf custValidator.ControlToValidate = "txtServicedInspectedDoneOnDate" Then
			'If (mItem.IsServicedInspected = True And mItem.ServicedInspectedInterval > 0 And mItem.ServicedInspectedIntervalIn > 0) Then
			If (mItem.IsServicedInspected = True Or mItem.IsConditionCheck = True) Then
				For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Count - 1
					If IsDBNull(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ServiedInspectedCheckDoneOnDate) = True Then
						e.IsValid = False
						Exit For
					End If
				Next
				'custValidator.ErrorMessage = "Part is Serviced/Inspected so Serviced Inspected Start Date required"
				custValidator.ErrorMessage = "Equipment Maintenance Part Start Date required"
				'Equipment Maintenance
			End If
		ElseIf custValidator.ControlToValidate = "txtExpQrts" Then
			If Val(txtExpQrts.Text) < 0 Or Val(txtExpQrts.Text) > 4 Then
				custValidator.ErrorMessage = "Expiry Quarters should be between 1 to 4"
				e.IsValid = False
			ElseIf (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0") Then
				custValidator.ErrorMessage = "Expiry Year also required with Expiry Qtrs."
				e.IsValid = False
			ElseIf ((txtExpiryDate.Text = "" And txtStartDate.Text = "")) And ((txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0")) And ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")) And ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth <> 0 Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter <> 0)) Then
				custValidator.ErrorMessage = "Cure Year and Cure Quarters required."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
			'------------------------------
		ElseIf custValidator.ControlToValidate = "txtExpYear" Then
			If (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0") Then
				custValidator.ErrorMessage = "Expiry Qtrs also required with Expiry Year."
				e.IsValid = False
			ElseIf txtExpYear.Text <> "0" And txtExpYear.Text <> "" And Len(txtExpYear.Text) < 4 Then
				custValidator.ErrorMessage = "Expiry Year should be not be less than 4 digits"
				e.IsValid = False
			ElseIf txtExpYear.Text <> "0" And txtExpYear.Text <> "" And Val(txtExpYear.Text) < 1753 Or Val(txtExpYear.Text) > 3030 Then
				custValidator.ErrorMessage = "Enter valid Expiry Year"
				e.IsValid = False
			ElseIf (txtCureYear.Text <> "0" And txtExpYear.Text <> "0") And (Val(txtCureYear.Text) > Val(txtExpYear.Text)) Then
				custValidator.ErrorMessage = "Expiry Year should be Later to Cure Year."
				e.IsValid = False
			ElseIf (txtCureYear.Text <> "0" And txtExpYear.Text <> "0") And (Val(txtCureYear.Text) > Val(txtExpYear.Text)) Then
				custValidator.ErrorMessage = "Expiry Year should be Later to Cure Year."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtCureYear" Then
			If (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") Then
				custValidator.ErrorMessage = "Cure Qtrs also required with Cure Year."
				e.IsValid = False
			ElseIf txtCureYear.Text <> "0" And Len(txtCureYear.Text) < 4 Then
				custValidator.ErrorMessage = "Cure Year should be not be less than 4 digits"
				e.IsValid = False
			ElseIf txtCureYear.Text <> "0" And Val(txtCureYear.Text) < 1753 Or Val(txtCureYear.Text) > 3030 Then
				custValidator.ErrorMessage = "Enter valid Cure Year"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtCodeNo" Then
			If (AppSettings("CodeNo") = "True" And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2 And mItem.SerialisedStatus = True) Then
				If (txtCodeNo.Text.Length = 0 Or txtCodeNo.Text.Trim = "") Then
					custValidator.ErrorMessage = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "GSE No. Required", "Code No. Required")
					e.IsValid = False
				Else
					e.IsValid = True
				End If
			End If
		ElseIf custValidator.ControlToValidate = "cmbWarrantyStatus" Then
			If (cmbWarrantyStatus.SelectedIndex = 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarrantyApplicableCheckedInOrderItem = True) Then
				custValidator.ErrorMessage = "Please Select Warranty Status As Accepted Or Rejected"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtReleaseNote" Then
			If (txtReleaseNote.Text.Trim = "" Or txtReleaseNote.Text.Trim = String.Empty) And AppSettings("ReleaseNoteNoRequire").ToUpper = "True".ToUpper Then
				custValidator.ErrorMessage = "Release Note No. Require."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "cmbFaultFound" And AppSettings("ClientCode") = "BA" And mReceiptCumInvoice.TransTypeID = 10 Then
			If (cmbFaultFound.SelectedIndex = 0) Then
				custValidator.ErrorMessage = "Fault found or not Please Select Yes/No"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub

	'Added By Prashant 11-Feb-2010---------------------------------------------------
	Public Sub CustomValidate1(s As Object, e As ServerValidateEventArgs)
		If Flag = 1 Then Exit Sub
		Dim CustValidator As CustomValidator
		CustValidator = CType(s, CustomValidator)
		Dim strMsg As String = ""
		SetObject()
		For j As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.GetBrokenRulesCollection.Count - 1
			strMsg = strMsg + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.GetBrokenRulesCollection(j).Description + "<Br>"
		Next
		If strMsg.Trim <> "" Then
			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.GetBrokenRulesCollection.Count = 1 And
				Val(cmbPartType.SelectedValue) > 0 And strMsg.Trim.Contains("Part Type is required") Then
				'do nothing
			Else
				CustValidator.ErrorMessage = strMsg
				e.IsValid = False
			End If
		End If
		Flag = 1
	End Sub

	Public Function CustomValidate2() As Boolean
		Dim strMsg As String = ""
		For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.Count - 1
			If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).IsValid Then
				For j As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(i).GetBrokenRulesCollection(j).Description + "<Br>"
				Next
			End If
		Next i
		If strMsg.Trim <> "" Then
			cvExpiryDate.ErrorMessage = strMsg
			cvExpiryDate.IsValid = False
			Return False
		End If
		Return True
	End Function
	'-----------------------------

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()
		mStoreList = StoreList.GetStoreList(0, "", True)
		cmbStore.DataSource = mStoreList
		Session("mStoreList") = mStoreList
		mPartTypeList = PartTypeList.GetPartTypeList(True)
		cmbPartType.DataSource = mPartTypeList
		Session("mPartTypeList") = mPartTypeList

		mUnitConverterList = UnitConverterList.GetUnitConverterList(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID, "(SELECT)")
		cmbUnitConverterList.DataSource = mUnitConverterList
		Session("mUnitConverterList") = mUnitConverterList


		If Session("RCIItem") = True Then
			'Added Trans.RCIFromSupplierAsNone By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1
			If mReceiptCumInvoice.TransTypeID = Trans.ReceiptFromCustomer Or mReceiptCumInvoice.TransTypeID = Trans.RCIFromSupplierAsNone Then

			ElseIf mReceiptCumInvoice.TransTypeID = Trans.RCIFromWorkOrder Then   'Added By Utkarsh 9-Dec-2010
				txtOrderDate.Text = "" 'DBNull.Value"
			Else
				txtOrderDate.Text = New SmartDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IODate.ToString).FormattedText
			End If
		Else
		End If
		txtReleaseNoteDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDate.ToString
		txtStartDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate.ToString
		txtExpiryDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate.ToString
		txtWarrantyInDays.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays
		txtWarrantyStartDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate.ToString
		txtWarrantyExpiryDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate.ToString
		txtCalibrationDoneOnDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate.ToString

		'txtConditionCheckDoneOnDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConditionCheckDoneOnDate.ToString
		'txtServicedInspectedDoneOnDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ServiedInspectedCheckDoneOnDate.ToString '       'Added by Shital on 13-Sep-2019
		'Added By Prashant 10-Feb-2010
		dgPeriods.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods 'mReceiptCumInvoice.Receipt.ReceiptItems.CurrentItem.ReceiptItemPeriods
		'-----------------------------
		dgReceiptItemKitItems.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems 'Added By Prashant 8-Nov-2016

		If (mReceiptCumInvoice.TransTypeID = 27) Or (mReceiptCumInvoice.TransTypeID = 28) Then
			cmbStore.SelectedValue = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID.ToString
		End If

		'Added By Vikrant On 31-Oct-2012 For ALL31102012
		mItemTypeList = PartTypeList.GetPartTypeList(True)
		Session("mItemTypeList") = mItemTypeList
		'End
		mWarrantyStatusList = WarrantyStatusList.GetWarrantyStatusList(True, "(SELECT)")
		cmbWarrantyStatus.DataSource = mWarrantyStatusList

		dgRCIAttachment.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments 'Added by Shital on 25-jun-2020

		DataBind()
	End Sub


#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			AddAttributes()
			txtOrderDate.Enabled = False

			If Not IsPostBack And Session("Sender") = "" Then

				If txtPartNo.Enabled = True Then
					setFocus(txtPartNo)
				End If

				AddSelectedPeroids() 'Added By Prashant 10-Feb-2010-----------------
				AddReceiptItemServiceInspections() 'Added By Prashant 30-Sep-2019
				DataFieldBind()

				Call cmbPartType_SelectedIndexChanged(Nothing, Nothing)  'Added by Utkarsh on 07-Nov-2011 For ALL07112011

			End If

			ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
			Controlvisibility()
			SetPage()
			ControlVisibilityForExpCalibration()
			ControlVisibilityForExcessQty()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOK.Click
		If IsValid Then
			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID > 0 Then
				mStore = Store.GetStore(New Guid(cmbStore.SelectedValue))
				If mStore.StoreTags.Contains(New Guid(cmbStore.SelectedValue), mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID) = False Then 'And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew Then
					'MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Store is not capable to hold this Part. As Part tag is " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName + "</br>Do you what to continue?", MsgBoxStyle.YesNo, "StoreTag")
					MSGBoxCtrl.Show("Alert!", mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName + " Part!", "Selected store does not facilitate to store this part " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName + " as it is tagged as " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName + ".</br>Do you want to continue?", MsgBoxStyle.YesNo, "StoreTag")
					Exit Sub
				End If
			End If
			'Added by Shital on 11-May-2021
			If mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Then
				Dim OrderCRate As Decimal = 0.0
				Dim ExtraMessage As String = ""
				OrderCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate + (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate * (10 / 100))
				ExtraMessage = "order rate " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate.ToString + " is not matching with receiving rate do you want to continue"
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate > OrderCRate And mReceiptCumInvoice.TransTypeID = 7) Or (mReceiptCumInvoice.TransTypeID = 10 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate > OrderCRate) Then
					MSGBoxCtrl.Show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "DifferRCIRate")
				Else
					ReceiptCumInvoiceItems()
				End If
			Else
				ReceiptCumInvoiceItems()
			End If
			'-------
			'Commentedby Shital on 11-May-2021 
			'ReceiptCumInvoiceItems()

		Else
			upnlValidationSummary.Update()
			Exit Sub
		End If
	End Sub

	Private Sub imgbtnPartNo_Click(sender As Object, e As EventArgs) Handles imgPartNo.Click  ''imgbtnPartNo.Click
		SetObject()
		SetGridObject()
		SetReceiptItemKitItemsGridObject()
		SetSession()
		Session("mFromToTypeID") = CInt(IIf(mReceiptCumInvoice.FromTypeID = 14, 1, mReceiptCumInvoice.FromTypeID)) '8  'Store
		Select Case mReceiptCumInvoice.TransTypeID
			Case 8    'ReceivedFromOtherStore
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
					Session("mPrevTransID") = Guid.Empty
				Else
					Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
				End If
				Session("OpenFrom") = "1"
				Session("mPrimaryOrderType") = 4
				Session("mTransaction") = 4
				Session("mFromPartList") = False
				Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType= 2")
			Case 9
				Session("ItemNo") = txtPartNo.Text
				Response.Redirect("wfSearchPartListForRCI_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")
			Case 10
				Session("mPrimaryOrderType") = 4
				Session("mTransaction") = 3
				Session("mFromPartList") = False
				Session("OpenFrom") = "1"
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
					Session("mPrevTransID") = Guid.Empty
				Else
					Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).OrderItemDetailForReceipt.OrderID
				End If
				Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType= 2")
			Case 11
				Response.Redirect("wfPendingLoanToRecover_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")
			Case 12    'LoanTaken
				Session("mPrimaryOrderType") = 4
				Session("mTransaction") = 4
				Session("mFromPartList") = False
				Session("OpenFrom") = "1"
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
					Session("mPrevTransID") = Guid.Empty
				Else
					Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
				End If
				Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType= 2")
			Case 13
				Session("mPrimaryOrderType") = 4
				Session("mTransaction") = 4
				Session("mFromPartList") = False
				Session("OpenFrom") = "1"
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
					Session("mPrevTransID") = Guid.Empty
				Else
					Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
				End If
				Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType= 2")
			Case 27, 28
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
					Session("mPrevTransID") = Guid.Empty
				Else
					Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
				End If
				' Session("mFromToTypeID") = 1  'Customer
				Session("mPrimaryOrderType") = 4 'TransListOf.Order_LoanRecovery
				Session("mTransaction") = 4 'Transaction.Issue
				Session("mFromPartList") = False
				Session("OpenFrom") = "1"
				Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType= 2")
			Case 46, 56
				Session("ItemNo") = txtPartNo.Text
				Response.Redirect("wfSearchPartListForRCI_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")
			Case 47
				Session("mPrimaryOrderType") = 4
				Session("mTransaction") = 4
				Session("mFromPartList") = False
				Session("OpenFrom") = "1"
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
					Session("mPrevTransID") = Guid.Empty
				Else
					Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
				End If
				Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType= 2")
			Case 48, 50, 57 '57 Added By Prashant 21-May-2010
				Session("ItemNo") = txtPartNo.Text
				Response.Redirect("wfSearchPartListForRCI_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")
			Case 53
				Session("ItemNo") = txtPartNo.Text
				Response.Redirect("wfSearchPartListForRCI_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")

			Case 61                   'Added By Utkarsh 09-Dec-2010
				Session("ItemNo") = txtPartNo.Text
				Dim mPrevTransID As Guid = Guid.Empty
				Dim mPrimaryOrderType As Integer
				Dim mTransaction As Integer
				Dim mFromPartList As Boolean
				mPrevTransID = Guid.Empty
				'If CType(mTransTypeID, Trans) = Trans.ReceiptcumInvoiceAgainstPuchaseOrder Then
				mPrimaryOrderType = 3
				mTransaction = 3 'Transaction.Order
				mFromPartList = False
				Session("OpenFrom") = 1
				Session("mPrevTransID") = mPrevTransID
				Session("mPrimaryOrderType") = mPrimaryOrderType
				Session("mTransaction") = mTransaction
				Session("mFromPartList") = mFromPartList
				Response.Redirect("wfnPendingWOListForRemoveComp_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType=2")
			Case 62 'Added by Saylee
				Session("ItemNo") = txtPartNo.Text
				'Session("mFromToTypeID") = 1   'Vendor
				Session("mPrimaryOrderType") = 3 'TransListOf.Order_Replaced
				Session("mTransaction") = 2 'Transaction.Issue
				Session("mFromPartList") = False 'True
				Session("OpenFrom") = 1
				If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
					Session("mPrevTransID") = Guid.Empty
				Else
					Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
				End If
				'Str = "<script language='javascript'>openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2'); </script>"
				Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx&mType= 2")
				''Added By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1
			Case 67
				Session("ItemNo") = txtPartNo.Text.Trim
				Response.Redirect("wfSearchPartListForRCI_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")
				'End
		End Select
	End Sub

	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
		Session.Remove("Enable")
		TotalCount = 0
		RemoveSessions()
		If Request.QueryString("ChildPage1") = "wfReceiptPendingOrderList_Ajax.aspx" Then
			Response.Redirect(Request.QueryString("ChildPage1") & "?mType=2" & "&BackPage=wfReceiptcumInvoiceItem_Ajax.aspx" & "&ChildPage=" & Request.QueryString("ChildPage"))
		ElseIf (Request.QueryString("BackPage") = "wfReceiptCumInvoice_Ajax.aspx") Or (Request.QueryString("BackPage") = Nothing) Then

			'******************************
			'Added by Saylee on 7-Jun-2011
			If Not Session("tmpReceiptCumInvoice") Is Nothing And Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsDirty = True Then
				mReceiptCumInvoice = CType(Session("tmpReceiptCumInvoice"), ReceiptCumInvoice)
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex = CType(Session("ItemIndex"), Integer)
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				Session.Remove("tmpReceiptCumInvoice")
			End If
			'******************************

			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew And Not Session("Edit") Then mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
			Session.Remove("Edit")
			Response.Redirect("wfReceiptCumInvoice_Ajax.aspx")
		Else
			Response.Redirect(Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub txtStartDate_TextChanged(sender As Object, e As EventArgs) Handles txtStartDate.TextChanged
		If IsDate(txtStartDate.Text) Or (txtStartDate.Text = "") Then
			If Not txtStartDate.Text Is mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate Then
				'' mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = CDate(txtStartDate.Text).ToShortDateString
				'*********************************
				'Added by Saylee on 7-Jun-2011
				If Not Session("tmpReceiptCumInvoice") Is Nothing Then
					Dim tmpReceiptCumInvoice As ReceiptCumInvoice = mReceiptCumInvoice.Clone
					Session("tmpReceiptCumInvoice") = tmpReceiptCumInvoice
					Session("ItemIndex") = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex
				End If
				'*********************************
				If (txtStartDate.Text.Trim = String.Empty) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = System.DBNull.Value
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = txtStartDate.Text
				End If
				txtStartDate.Text = New SmartDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate.ToString).FormattedText
				txtExpiryDate.Text = New SmartDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate.ToString).FormattedText
			End If
		Else
			txtStartDate.Text = ""
		End If
		ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
	End Sub

	Private Sub btnAlternatePart_Click(sender As Object, e As EventArgs) Handles btnAlternatePart.Click
		SetObject()
		Session("mItem") = Item.GetItem(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
		'Old'Response.Redirect("wfAlternatePOPartList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfReceiptcumInvoiceItem_Ajax.aspx" & "&mType=1&OpenFrom=2") ' OpenFrom=1 for receiptcuminvoice
		'My'Response.Redirect("wfAlternatePartListForRCI.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfReceiptcumInvoiceItem_Ajax.aspx")
		Response.Redirect("wfAlternatePartListForRCI_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")
	End Sub

	Private Sub txtCureQtrs_TextChanged(sender As Object, e As EventArgs) Handles txtCureQtrs.TextChanged
		If (Val(txtCureQtrs.Text) >= 0 Or Val(txtCureQtrs.Text) <= 4) Then

			'*********************************
			'Added by Saylee on 7-Jun-2011
			If Val(txtCureYear.Text) = 0 And Session("tmpReceiptCumInvoice") Is Nothing Then
				Dim tmpReceiptCumInvoice As ReceiptCumInvoice = mReceiptCumInvoice.Clone
				Session("tmpReceiptCumInvoice") = tmpReceiptCumInvoice
				Session("ItemIndex") = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex
			End If
			'*********************************

			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs = Val(txtCureQtrs.Text)

			txtExpQrts.DataBind()
			txtExpYear.DataBind()

			If Val(txtCureQtrs.Text) = 0 Then txtCureQtrs.Text = "0"
			ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
		End If
	End Sub

	Private Sub txtCureYear_TextChanged(sender As Object, e As EventArgs) Handles txtCureYear.TextChanged
		If (Val(txtCureQtrs.Text) >= 0 And Val(txtCureQtrs.Text) <= 4) Then

			'*********************************
			'Added by Saylee on 7-Jun-2011
			If Val(txtCureQtrs.Text) = 0 And Session("tmpReceiptCumInvoice") Is Nothing Then
				Dim tmpReceiptCumInvoice As ReceiptCumInvoice = mReceiptCumInvoice.Clone
				Session("tmpReceiptCumInvoice") = tmpReceiptCumInvoice
				Session("ItemIndex") = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex
			End If
			'*********************************

			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear = Val(txtCureYear.Text)
			txtExpQrts.DataBind()
			txtExpYear.DataBind()

			Session("ReceiptCumInvoice") = mReceiptCumInvoice
			ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
		End If
	End Sub

	Private Sub btnSelectFiles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
		If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
			Exit Sub
		End If
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
	End Sub

	Private Sub hdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click
		AttachMyFile()
		upnlRCIAttachment.Update()
		'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAttachmentAdded = True
		'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.Add(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension)
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		'ControlVisibilityForExpCalibration()
	End Sub

	Private Sub dgRCIAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRCIAttachment.RowCommand
		Dim mFileAttachments As FileAttachments
		Select Case e.CommandName
			Case "View"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex

				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				mFileAttachments = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments

				If mFileAttachments.Count = 1 Then
					mFileAttachments.CurrentIndex = 0
				Else
					mFileAttachments.CurrentIndex = Index - 1
				End If

				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments.CurrentItem.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
					End If
				End If
				dgRCIAttachment.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments
				dgRCIAttachment.DataBind()
				Controlvisibility()
				upnlRCIAttachment.Update()
				upnldgRCIAttachment.Update()
			Case "Remove"
				'Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex
				Dim Index As Integer = CInt(e.CommandArgument) + dgRCIAttachment.PageSize * dgRCIAttachment.PageIndex
				' DeleteAttachment(Index)
				mFileAttachments = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.FileAttachments
				If mFileAttachments.Count = 1 Then
					DeleteAttachment(0)
				Else
					DeleteAttachment(Index - 1)
				End If
		End Select

	End Sub

	Private Sub chkIsInWarranty_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsInWarranty.CheckedChanged
		If chkIsInWarranty.Checked = True Then
			txtWarrantyInDays.Enabled = True
			'txtWarrantyStartDate.Enabled = True
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyInDays.Text = "0"
			txtWarrantyStartDate.Enabled = False
			txtWarrantyStartDate.Text = ""
			txtWarrantyExpiryDate.Text = ""
		End If
	End Sub

	Private Sub txtWarrantyStartDate_TextChanged(sender As Object, e As EventArgs) Handles txtWarrantyStartDate.TextChanged
		'txtWarrantyInDays.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays
		'txtWarrantyInDays.DataBind()
		If IsDate(txtWarrantyStartDate.Text) Then
			If Val(txtWarrantyInDays.Text) <> 0 And IsDate(txtWarrantyStartDate.Text) Then
				txtWarrantyExpiryDate.Text = CDate(DateAdd(DateInterval.Day, Val(txtWarrantyInDays.Text), CDate(txtWarrantyStartDate.Text))).ToString(AppSettings("DateFormat").ToString)
			Else
				txtWarrantyExpiryDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate.ToString(AppSettings("DateFormat").ToString)
			End If
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyInDays.Text = "0"
			txtWarrantyStartDate.Enabled = False
			txtWarrantyStartDate.Text = ""
			txtWarrantyExpiryDate.Text = ""
			chkIsInWarranty.Checked = False
		End If
	End Sub

	Private Sub txtWarrantyInDays_TextChanged(sender As Object, e As EventArgs) Handles txtWarrantyInDays.TextChanged
		'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = Val(txtWarrantyInDays.Text)
		If Val(txtWarrantyInDays.Text) <> 0 Then
			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDateFormatted.ToString = "" Then
				txtWarrantyStartDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted.ToString
			Else
				txtWarrantyStartDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDateFormatted
			End If

			If Val(txtWarrantyInDays.Text) <> 0 And IsDate(txtWarrantyStartDate.Text) Then
				txtWarrantyExpiryDate.Text = CDate(DateAdd(DateInterval.Day, Val(txtWarrantyInDays.Text), CDate(txtWarrantyStartDate.Text))).ToString(AppSettings("DateFormat").ToString)
			Else
				txtWarrantyExpiryDate.Text = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate.ToString(AppSettings("DateFormat").ToString)
			End If
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyInDays.Text = "0"
			txtWarrantyStartDate.Enabled = False
			txtWarrantyStartDate.Text = ""
			txtWarrantyExpiryDate.Text = ""
			chkIsInWarranty.Checked = False
		End If
	End Sub

	Private Sub btnAddPeroid_Click(sender As Object, e As EventArgs) Handles ImgAddPeroid.Click  ''btnAddPeroid.Click
		SetPeroids()
		SetObject()
		SetGridObject()
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		'' Response.Redirect("wfSelectPeriod_Ajax.aspx?BackPage2=wfReceiptcumInvoiceItem_Ajax.aspx&BackPage=" & Request.QueryString("BackPage"))
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
	End Sub

	Private Sub hdnAddPeriod_Click(sender As Object, e As EventArgs) Handles hdnAddPeriod.Click
		AddSelectedPeroids()
		mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
		dgPeriods.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods
		dgPeriods.DataBind()
		upnlTSNTSOValues.Update()
		upnlTabDetails.Update()
	End Sub

	'Added By Prashant 10-Feb-2010----------------------------------------
	Private Sub dgPeriods_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPeriods.RowCommand
		Select Case e.CommandName
			Case "ForDelete"
				Dim index As Integer = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentIndex = index
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				MSGBoxCtrl.Show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "Remove Part TSN / TSOH Values.", MsgBoxStyle.YesNo, "Delete")
		End Select
	End Sub

	'Added By Utkarsh On 21-Sep-2011 For ALL21092011-2
	Private Sub imgbtnPartType_Click(sender As Object, e As EventArgs) Handles ImgPartType.Click ''ImgbtnPartType.Click
		SetObject()
		Response.Redirect("wfItemType_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfReceiptcumInvoiceItem_Ajax.aspx" & "&mType=1&OpenFrom=2") ' OpenFrom=1 for receiptcuminvoice
	End Sub
	'End

	'Added by Utkarsh on 07-Nov-2011 For ALL07112011
	Private Sub cmbPartType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPartType.SelectedIndexChanged
		If cmbPartType.SelectedIndex > 0 Then
			lblColor.BackColor = ColorTranslator.FromHtml("#" & mPartTypeList(cmbPartType.SelectedIndex).Color)
		Else
			lblColor.BackColor = Color.WhiteSmoke
		End If
		upnlReceivingInformation1.Update()
		lblPartStatus.Text = IIf(cmbPartType.SelectedIndex > 0, mItemTypeList(cmbPartType.SelectedIndex).PartStatusName, "") 'Added By Vikrant On 31-Oct-2012 For ALL31102012
	End Sub
	'End

	'----Added by Vikrant FOR ALL10052012-10--------------
	Private Sub txtExpQrts_TextChanged(sender As Object, e As EventArgs) Handles txtExpQrts.TextChanged
		ControlVisibilityForExpiryInfo()
	End Sub

	Private Sub txtExpYear_TextChanged(sender As Object, e As EventArgs) Handles txtExpYear.TextChanged
		ControlVisibilityForExpiryInfo()
	End Sub

	Private Sub txtExpiryDate_TextChanged(sender As Object, e As EventArgs) Handles txtExpiryDate.TextChanged
		If IsDate(txtExpiryDate.Text) Then
			ControlVisibilityForExpiryInfo()
		Else
			txtExpiryDate.Text = ""
		End If
	End Sub

	Private Sub chkIsExpiryNA_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsExpiryNA.CheckedChanged
		If chkIsExpiryNA.Checked Then
			If AppSettings("ClientCode") = "IND" Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				'Do nothing 
			Else
				chkIsExpiryUnlimited.Enabled = False
			End If
		End If
	End Sub

	Private Sub chkIsExpiryUnlimited_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsExpiryUnlimited.CheckedChanged
		If chkIsExpiryUnlimited.Checked Then
			If AppSettings("ClientCode") = "IND" Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				'Do nothing 
			Else
				chkIsExpiryNA.Enabled = False
			End If
		End If
	End Sub
	'-----------------------------------------------------

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	Private Sub txtReleaseNoteDate_TextChanged(sender As Object, e As EventArgs) Handles txtReleaseNoteDate.TextChanged
		If Not IsDate(txtReleaseNoteDate.Text) Then
			txtReleaseNoteDate.Text = ""
		End If
	End Sub

	Private Sub txtCalibrationDoneOnDate_TextChanged(sender As Object, e As EventArgs) Handles txtCalibrationDoneOnDate.TextChanged
		If Not IsDate(txtCalibrationDoneOnDate.Text) Then
			txtCalibrationDoneOnDate.Text = ""
		End If
	End Sub

	Private Sub txtServicedInspectedDoneOnDate_TextChanged(sender As Object, e As EventArgs) Handles txtServicedInspectedDoneOnDate.TextChanged
		If Not IsDate(txtServicedInspectedDoneOnDate.Text) Then
			txtServicedInspectedDoneOnDate.Text = ""
		End If
	End Sub

	Private Sub txtSerialNo_TextChanged(sender As Object, e As EventArgs) Handles txtSerialNo.TextChanged
		SetControl()
	End Sub

	Private Sub btnSelectCompStatus_Click(sender As Object, e As EventArgs) Handles btnSelectCompStatus.Click
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompStatusID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.tmpCompStatusID
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		isPOPShown = False
		RemoveSessions()
		Session.Remove("tmpReceiptCumInvoice")
		Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
	End Sub

	Private Sub btnNoCompStatus_Click(sender As Object, e As EventArgs) Handles btnNoCompStatus.Click
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompStatusID = Guid.Empty
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		lnkCompStatus_ModalPopupExtender.Hide()
		isPOPShown = False
		RemoveSessions()
		Session.Remove("tmpReceiptCumInvoice")
		Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
	End Sub

	Private Sub btnCloseCompStatus_Click(sender As Object, e As EventArgs) Handles btnCloseCompStatus.Click
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompStatusID = Guid.Empty
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		lnkCompStatus_ModalPopupExtender.Hide()
		TotalCount = 1
		Session("TotalCount") = TotalCount
		isPOPShown = False
	End Sub

	'Added By Vikrant On 11-Aug-2016 For ALL11082016
	Private Sub txtExcessQty_TextChanged(sender As Object, e As EventArgs) Handles txtExcessQty.TextChanged
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExcessQty = CDec(Val(txtExcessQty.Text))
		ControlVisibilityForExcessQty()
		txtShortQty.DataBind()
	End Sub

	Private Sub txtShortQty_TextChanged(sender As Object, e As EventArgs) Handles txtShortQty.TextChanged
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ShortQty = CDec(Val(txtShortQty.Text))
		ControlVisibilityForExcessQty()
		txtExcessQty.DataBind()
	End Sub
	'End

	Private Sub cmbStore_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStore.SelectedIndexChanged
		mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, cmbStore.SelectedValue.ToString) ''Added By Prashant 13-May-2020
		If mUserHasNoStoreRights.Count > 0 Then
			MSGBoxCtrl.Show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "ResetStore")
			Exit Sub
		End If
		If mStoreList(New Guid(cmbStore.SelectedValue)).NotInUse = True Then
			If CDate(mStoreList(New Guid(cmbStore.SelectedValue)).NotInUseDate) <= CDate(mReceiptCumInvoice.RecCumInvDate) Then
				MSGBoxCtrl.Show("Alert!", "Store is not applicable since " + mStoreList(New Guid(cmbStore.SelectedValue)).NotInUseDateFormatted, "Select another Store from list or select date before " + mStoreList(New Guid(cmbStore.SelectedValue)).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		End If ''End of Added By Prashant 13-May-2020

		'If (AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "LAMA") Then
		If AppSettings("ClientCode") = "CE" Then

			mIsOwnedByCustomer = IIf(cmbStore.SelectedIndex > 0, Store.GetStore(New Guid(cmbStore.SelectedValue)).IsOwnedByCustomer, False)

			If (
					mReceiptCumInvoice.TransTypeID = 6 Or
					mReceiptCumInvoice.TransTypeID = 7 Or
					mReceiptCumInvoice.TransTypeID = 10 Or
					mReceiptCumInvoice.TransTypeID = 27 Or
					mReceiptCumInvoice.TransTypeID = 48 Or
					mReceiptCumInvoice.TransTypeID = 54 Or
					mReceiptCumInvoice.TransTypeID = 67 Or
					mReceiptCumInvoice.FromTypeID = 16
				) Then

				txtBatchNo.Enabled = mIsOwnedByCustomer

			Else
				txtBatchNo.Enabled = True
			End If

			upnlReceivingInformation1.Update()

		End If

	End Sub

	Private Sub txtDisplayCAmount_TextChanged(sender As Object, e As EventArgs) Handles txtDisplayCAmount.TextChanged 'Added By Prashant 5-Feb-2019 ALL04022019
		If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
			Dim Factor As Decimal
			Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
			If Not mUnitConverterList Is Nothing Then
				Factor = mUnitConverterList.UnitConverterFactor(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BaseUnitID, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID)
			End If
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount = CDec(Val(txtDisplayCAmount.Text))
			'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = CDec(Val(txtDisplayCAmount.Text)) / Factor
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = CDec(Val(txtDisplayCAmount.Text))
			txtCAmount.DataBind()
			If CDec(Val(txtQuantity.Text)) > 0 Then
				If (mReceiptCumInvoice.TransTypeID = 10) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
					'Do Nothing
				ElseIf (mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = CDec(Val(txtCommercialRate.Text))
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = CDec(Val(txtCommercialRate.Text))
				Else
					'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / (CDec(Val(txtQuantity.Text)) / Factor)
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount / (CDec(Val(txtQuantity.Text)))
					'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / (CDec(Val(txtQuantity.Text)) / Factor)
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / CDec(Val(txtQuantity.Text)) '(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / Factor) / (CDec(Val(txtQuantity.Text)) / Factor)
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = 0 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
				End If
				txtDisplayCRate.DataBind()
				txtCRate.DataBind()
			End If
		End If
	End Sub

	Private Sub txtDisplayCRate_TextChanged(sender As Object, e As EventArgs) Handles txtDisplayCRate.TextChanged
		If (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo") Then
			Dim Factor As Decimal
			Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
			If Not mUnitConverterList Is Nothing Then
				Factor = mUnitConverterList.UnitConverterFactor(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BaseUnitID, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID)
			End If
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = CDec(Val(txtDisplayCRate.Text)) * Factor
			txtCRate.DataBind()
			upnlRateValues.Update()
		End If
	End Sub

	Private Sub txtDisplayCommercialRate_TextChanged(sender As Object, e As EventArgs) Handles txtDisplayCommercialRate.TextChanged
		Dim Factor As Decimal
		Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
		If Not mUnitConverterList Is Nothing Then
			Factor = mUnitConverterList.UnitConverterFactor(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BaseUnitID, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID)
		End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = CDec(Val(txtDisplayCommercialRate.Text)) * Factor
		txtCommercialRate.DataBind()
		upnlEffectiveRate.Update()
	End Sub

	Private Sub txtQuantity_TextChanged(sender As Object, e As EventArgs) Handles txtQuantity.TextChanged
		If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
			'Dim Factor As Decimal
			'Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID)
			'If Not mUnitConverterList Is Nothing Then
			'    Factor = mUnitConverterList.UnitConverterFactor(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BaseUnitID, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID)
			'End If
			'If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty > 0 Then
			'    If (mReceiptCumInvoice.TransTypeID = 10) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			'        'Do Nothing
			'    ElseIf (mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = CDec(Val(txtCommercialRate.Text))
			'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = CDec(Val(txtCommercialRate.Text))
			'    Else
			'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty
			'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = 0 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			'    End If
			'    txtCRate.DataBind()
			'End If
			'If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty > 0 Then
			'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = New Guid(cmbUnitConverterList.SelectedValue)    'Added By Prashant 11-May-2010
			'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitName = cmbUnitConverterList.SelectedItem.Text     'Added By Prashant 11-May-2010
			' mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(Val(txtQuantity.Text))

			'If (mReceiptCumInvoice.TransTypeID = 10) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			'    'Do Nothing
			'ElseIf (mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = CDec(Val(txtCommercialRate.Text))
			'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = CDec(Val(txtCommercialRate.Text))
			'Else
			'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty
			'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = 0 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
			'End If
			'txtCRate.DataBind()
			'End If
		End If
	End Sub

	Private Sub imgbtnReceiptItemServiceInspection_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnReceiptItemServiceInspection.Click
		'If IsValid Then
		'SetObject()
		'Session("mMaintenanceID") = mAssemblyStatus.ID
		'mMaintenanceDoneByEmployees = mAssemblyStatus.MaintenanceDoneByEmployees
		'Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddReceiptItemServiceInspection", "AddReceiptItemServiceInspection();", True)
		'Else
		'upnlValidationSummary.Update()
		'End If
	End Sub

#End Region

End Class
