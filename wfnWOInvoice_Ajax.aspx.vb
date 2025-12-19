'Created by : Saylee
'Dated      : 2-Aug-2022

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Public Class wfnWOInvoice_Ajax
	Inherits System.Web.UI.Page


#Region " Enumaration "
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

#Region "Variables and Declarations"
	Public mWOInvoice As WOInvoice
	Dim mCurrencyList As CurrencyList
	Public mCapabilityTaskList As CapabilityTaskList

	Dim mWOInvoiceJob As WOInvoiceJob
	Dim mWOInvoiceSpare As WOInvoiceSpare
	Dim mWOInvoiceJobCharge As WOInvoiceJobCharge
	Dim mWOInvoiceSparesCharge As WOInvoiceSparesCharge

	Dim mUser As User

	Dim mChargeList As ChargeList
	Dim mTerms As Terms
	Public SparePartNo As String = ""
	Public SpareDescription As String = ""
	Dim mFetchItemByName As FetchItemByName

	'GST
	Dim mCompanyDetail As CompanyDetail
	Dim mGSTPercentage As GSTPercentage
	'End
	Dim MaxGSTPercentage As Decimal = 0
	Dim mFetchTermByName As TermList
	Dim mFetchChargeByName As ChargeList
	Public mVendor As Vendor

#End Region

#Region "Helper Methods"
	Private Sub addAttributes()
		txtWOInvoiceText.Attributes.Add("onblur", "WaterMark(this, event);")
		txtWOInvoiceText.Attributes.Add("onfocus", "WaterMark(this, event);")
	End Sub
	Private Sub GetSession()
		mWOInvoice = Session("mWOInvoice")
		mCurrencyList = Session("mCurrencyList")
		mChargeList = Session("mChargeList")
		mTerms = Session("mTerms")
		mCompanyDetail = Session("mCompanyDetail") 'GST
		mCapabilityTaskList = Session("mCapabilityTaskList")
	End Sub

	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		IsInRoleString = "WOInvoice"
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
		End Select
	End Function
	Private Sub ResetGSTRates(Optional ByVal mWOInvoiceSpare As WOInvoiceSpare = Nothing, Optional ByVal mWOInvoiceJob As WOInvoiceJob = Nothing)
		If mWOInvoiceJob IsNot Nothing Then
			mWOInvoiceJob.CGSTPercentage = 0
			mWOInvoiceJob.SGSTPercentage = 0
			mWOInvoiceJob.CGSTCAmount = 0
			mWOInvoiceJob.SGSTCAmount = 0
			mWOInvoiceJob.IGSTPercentage = 0
			mWOInvoiceJob.IGSTCAmount = 0
			mWOInvoiceJob.TotalCAmount = mWOInvoiceJob.CAmount
		End If
		If mWOInvoiceSpare IsNot Nothing Then
			mWOInvoiceSpare.CGSTPercentage = 0
			mWOInvoiceSpare.SGSTPercentage = 0
			mWOInvoiceSpare.CGSTCAmount = 0
			mWOInvoiceSpare.SGSTCAmount = 0
			mWOInvoiceSpare.IGSTPercentage = 0
			mWOInvoiceSpare.IGSTCAmount = 0
			mWOInvoiceSpare.TotalCAmount = mWOInvoiceSpare.CAmount
		End If
	End Sub
	Private Sub SetGSTRates(Optional ByVal mnWOInvoiceSpare As WOInvoiceSpare = Nothing, Optional ByVal mWOInvoiceJob As WOInvoiceJob = Nothing, Optional ByVal HSNACSID As String = "00000000-0000-0000-0000-000000000000", Optional currentRow As GridViewRow = Nothing)
		Dim txtValue As TextBox
		Dim i As Integer = 0
		Dim Gridcurrentrow As GridViewRow
		Try
			'------------------------------------------------------------------
			If AppSettings("IsGSTApplicableWOInvoice") = "True" Then
				''mWOInvoiceJob

				If mWOInvoiceJob IsNot Nothing Then


					For Each mWOInvoiceJob In mWOInvoice.WOInvoiceJobs
						With mWOInvoiceJob

							If currentRow IsNot Nothing Then
								Gridcurrentrow = currentRow
							Else
								Gridcurrentrow = Me.dgWOInvoiceJobs.Rows(i)
							End If
							'Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
							mVendor = Vendor.GetVendor(mWOInvoice.CustomerID)
							If Gridcurrentrow.ClientID = dgWOInvoiceJobs.Rows(i).ClientID Then
								If mVendor.ClientCountryName.ToUpper = "INDIA" Then
									If mVendor.CountryName.ToUpper = "INDIA" And mWOInvoice.Date >= CDate("01-Jul-2017") Then
										'mGSTPercentage = GSTPercentage.GetPercentage(mWOInvoice.Date, 1, .ItemID.ToString)
										'If Not mGSTPercentage Is Nothing Then
										If Len(mVendor.StateCode) > 0 Then
											Dim mHSNACSlist As HSNACSList
											If Not New Guid(HSNACSID).Equals(Guid.Empty) Then
												mHSNACSlist = HSNACSList.GetConditionCheckReceiptItemList(New Guid(HSNACSID))
											End If

											If mVendor.StateCode = mVendor.ClientStateCode Then
												If Not New Guid(HSNACSID).Equals(Guid.Empty) Then
													txtValue = CType(Gridcurrentrow.FindControl("txtCGSTPer"), TextBox)
													txtValue.Text = mHSNACSlist(0).GSTPercent / 2
													.CGSTPercentage = CDec(Val(txtValue.Text))

													txtValue = CType(Gridcurrentrow.FindControl("txtSGSTPer"), TextBox)
													txtValue.Text = mHSNACSlist(0).GSTPercent / 2
													.SGSTPercentage = CDec(Val(txtValue.Text))

													.HSNACSCode = mHSNACSlist(0).Code
												Else
													txtValue = CType(Gridcurrentrow.FindControl("txtCGSTPer"), TextBox)
													.CGSTPercentage = CDec(Val(txtValue.Text))

													txtValue = CType(Gridcurrentrow.FindControl("txtSGSTPer"), TextBox)
													.SGSTPercentage = CDec(Val(txtValue.Text))
												End If

												.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
												.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)


												.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount


												.IGSTPercentage = 0
												.IGSTCAmount = 0

												mWOInvoice.StateCode = mVendor.StateCode
												mWOInvoice.ClientStateCode = mVendor.ClientStateCode
												mWOInvoice.CountryName = mVendor.CountryName
												mWOInvoice.Visibility = 1
											Else
												txtValue = CType(Gridcurrentrow.FindControl("txtIGSTPer"), TextBox)
												If Not New Guid(HSNACSID).Equals(Guid.Empty) Then
													txtValue.Text = mHSNACSlist(0).GSTPercent
													.HSNACSCode = mHSNACSlist(0).Code
												End If

												.IGSTPercentage = CDec(Val(txtValue.Text))
												.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

												.CGSTPercentage = 0
												.SGSTPercentage = 0
												.CGSTCAmount = 0
												.SGSTCAmount = 0

												.TotalCAmount = .CAmount + .IGSTCAmount

												'  .TotalCAmount = .CAmount

												'  .HSNACSCode = mtmpItem.HSNACSCode
												mWOInvoice.StateCode = mVendor.StateCode
												mWOInvoice.ClientStateCode = mVendor.ClientStateCode
												mWOInvoice.CountryName = mVendor.CountryName
												mWOInvoice.Visibility = 2
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
											.TotalCAmount = .CAmount
											mWOInvoice.StateCode = mVendor.StateCode
											mWOInvoice.ClientStateCode = mVendor.ClientStateCode
											mWOInvoice.VendorCountry = mVendor.CountryName
											mWOInvoice.Visibility = 3
										End If
										'End If
									Else
										.CGSTPercentage = 0
										.SGSTPercentage = 0
										.CGSTCAmount = 0
										.SGSTCAmount = 0
										.IGSTPercentage = 0
										.IGSTCAmount = 0
										.TotalCAmount = 0
										.HSNACSCode = ""
										.TotalCAmount = .CAmount
										mWOInvoice.StateCode = mVendor.StateCode
										mWOInvoice.ClientStateCode = mVendor.ClientStateCode
										mWOInvoice.VendorCountry = mVendor.CountryName
										mWOInvoice.Visibility = 3
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
									.TotalCAmount = .CAmount
									mWOInvoice.StateCode = mVendor.StateCode
									mWOInvoice.ClientStateCode = mVendor.ClientStateCode
									mWOInvoice.VendorCountry = mVendor.CountryName
									mWOInvoice.Visibility = 3
								End If
							End If
						End With
						i = i + 1

					Next
				End If



				''WOInvoiceSpare
				If mnWOInvoiceSpare IsNot Nothing Then
					For Each mWOInvoiceSpare In mWOInvoice.WOInvoiceSpares
						With mWOInvoiceSpare
							Dim mtmpItem As Item = Item.GetItem(.PartID)
							mVendor = Vendor.GetVendor(mWOInvoice.CustomerID)
							If mVendor.ClientCountryName.ToUpper = "INDIA" Then
								If mVendor.CountryName.ToUpper = "INDIA" And mWOInvoice.Date >= CDate("01-Jul-2017") Then
									'mGSTPercentage = GSTPercentage.GetPercentage(mWOInvoice.Date, 1, .ItemID.ToString)
									'If Not mGSTPercentage Is Nothing Then
									If Len(mVendor.StateCode) > 0 Then
										Dim mHSNACSlist As HSNACSList
										If Not mtmpItem.HSNACSID.Equals(Guid.Empty) Then
											mHSNACSlist = HSNACSList.GetConditionCheckReceiptItemList(mtmpItem.HSNACSID)
										End If

										If mVendor.StateCode = mVendor.ClientStateCode Then
											If Not mtmpItem.HSNACSID.Equals(Guid.Empty) Then
												txtValue = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareCGSTPer"), TextBox)
												txtValue.Text = mHSNACSlist(0).GSTPercent / 2
												.CGSTPercentage = CDec(Val(txtValue.Text))

												txtValue = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareSGSTPer"), TextBox)
												txtValue.Text = mHSNACSlist(0).GSTPercent / 2
												.SGSTPercentage = CDec(Val(txtValue.Text))

												.HSNACSCode = mHSNACSlist(0).Code
											Else
												txtValue = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareCGSTPer"), TextBox)
												.CGSTPercentage = CDec(Val(txtValue.Text))

												txtValue = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareSGSTPer"), TextBox)
												.SGSTPercentage = CDec(Val(txtValue.Text))
											End If

											.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
											.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)

											.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

											.IGSTPercentage = 0
											.IGSTCAmount = 0

											mWOInvoice.StateCode = mVendor.StateCode
											mWOInvoice.ClientStateCode = mVendor.ClientStateCode
											mWOInvoice.CountryName = mVendor.CountryName
											mWOInvoice.Visibility = 1
										Else
											txtValue = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareIGSTPer"), TextBox)
											If Not mtmpItem.HSNACSID.Equals(Guid.Empty) Then
												txtValue.Text = mHSNACSlist(0).GSTPercent
												.HSNACSCode = mHSNACSlist(0).Code
											End If

											.IGSTPercentage = CDec(Val(txtValue.Text))
											.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

											.CGSTPercentage = 0
											.SGSTPercentage = 0
											.CGSTCAmount = 0
											.SGSTCAmount = 0

											.TotalCAmount = .CAmount + .IGSTCAmount

											'  .HSNACSCode = mtmpItem.HSNACSCode
											mWOInvoice.StateCode = mVendor.StateCode
											mWOInvoice.ClientStateCode = mVendor.ClientStateCode
											mWOInvoice.CountryName = mVendor.CountryName
											mWOInvoice.Visibility = 2
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
										.TotalCAmount = .CAmount
										mWOInvoice.StateCode = mVendor.StateCode
										mWOInvoice.ClientStateCode = mVendor.ClientStateCode
										mWOInvoice.VendorCountry = mVendor.CountryName
										mWOInvoice.Visibility = 3
									End If
									'End If
								Else
									.CGSTPercentage = 0
									.SGSTPercentage = 0
									.CGSTCAmount = 0
									.SGSTCAmount = 0
									.IGSTPercentage = 0
									.IGSTCAmount = 0
									.TotalCAmount = 0
									.HSNACSCode = ""
									.TotalCAmount = .CAmount
									mWOInvoice.StateCode = mVendor.StateCode
									mWOInvoice.ClientStateCode = mVendor.ClientStateCode
									mWOInvoice.VendorCountry = mVendor.CountryName
									mWOInvoice.Visibility = 3
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
								.TotalCAmount = .CAmount
								mWOInvoice.StateCode = mVendor.StateCode
								mWOInvoice.ClientStateCode = mVendor.ClientStateCode
								mWOInvoice.VendorCountry = mVendor.CountryName
								mWOInvoice.Visibility = 3
							End If
						End With
						i = i + 1
					Next
				End If
			Else
				mWOInvoice.Visibility = 3
			End If
			'------------------------------------------------------------------
		Catch ex As Exception
			Dim a As Integer = 0
		End Try
		'--------------------------------------------
	End Sub

	Private Sub SetAttributes()
		Dim txtValue As TextBox

		For i As Integer = 0 To dgWOInvoiceJobs.Rows.Count - 1

			Try
				txtValue = CType(Me.dgWOInvoiceJobs.Rows(i).FindControl("txtManHrs"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")


				txtValue = CType(Me.dgWOInvoiceJobs.Rows(i).FindControl("txtRate"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgWOInvoiceJobs.Rows(i).FindControl("txtTax"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

			Catch ex As Exception
				Dim a As Integer = 0
			End Try

		Next

		For i As Integer = 0 To dgWOInvoiceSpares.Rows.Count - 1
			Try

				txtValue = dgWOInvoiceSpares.Rows(i).FindControl("txtSpareQty")
				txtValue.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtValue.ClientID + "').value,event)")


				txtValue = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareRate"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

				txtValue = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtTax"), TextBox)
				txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

			Catch ex As Exception
				Dim a As Integer = 0
			End Try

		Next

	End Sub


	Private Sub setObject(Optional currentRow As GridViewRow = Nothing)

		If txtWOInvoiceDate.Text.ToString <> "" Then
			mWOInvoice.Date = CDate(txtWOInvoiceDate.Text)
		Else
			mWOInvoice.Date = System.DBNull.Value
		End If

		'  mWOInvoice.Text = Trim(txtWOInvoiceText.Text.Trim)
		If txtWOInvoiceText.Text = "Select your prefix" Then
			mWOInvoice.Text = ""
		Else
			mWOInvoice.Text = Trim(txtWOInvoiceText.Text.Trim)
		End If
		mWOInvoice.No = Val(txtWOInvoiceNo.Text)

		mWOInvoice.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
		mWOInvoice.ConversionFactor = Val(txtConversionFactor.Text)

		mWOInvoice.UserName = User.Identity.Name

		'mWOInvoice.BillingLocationID = New Guid(cmbBillingLocation.SelectedValue) 'Billing Location Change
		'Bind child
		Dim txtValue As TextBox

		mWOInvoice.Remark = txtRemark.Text   'Added on 10-May-2018 for Aman requirement

		'Invoice Job
		Dim mWOInvoiceJob As WOInvoiceJob
		Dim i As Integer = 0

		Dim cmbValue As DropDownList

		Dim Gridcurrentrow As GridViewRow
		Dim ID As Guid
		For Each mWOInvoiceJob In mWOInvoice.WOInvoiceJobs

			With mWOInvoiceJob
				If currentRow IsNot Nothing Then
					Gridcurrentrow = currentRow
				Else
					Gridcurrentrow = Me.dgWOInvoiceJobs.Rows(i)
				End If
				ID = New Guid(dgWOInvoiceJobs.DataKeys(i).Values("ID").ToString)
				.ConversionFactor = Val(txtConversionFactor.Text)
				Try
					'Dim str1 As String = Gridcurrentrow.ID
					'Dim str2 As String = dgWOInvoiceJobs.Rows(i).ID

					'Dim str11 As String = Gridcurrentrow.ClientID
					'Dim str22 As String = dgWOInvoiceJobs.Rows(i).ClientID

					If Gridcurrentrow.ClientID = dgWOInvoiceJobs.Rows(i).ClientID Then
						txtValue = CType(Gridcurrentrow.FindControl("txtManHrs"), TextBox)
						.ManHour = txtValue.Text

						cmbValue = CType(Gridcurrentrow.FindControl("cmbCapability"), DropDownList)
						.CapabilityTaskID = New Guid(cmbValue.SelectedValue.ToString)

						txtValue = CType(Gridcurrentrow.FindControl("txtRate"), TextBox)
						.CRate = CDec(Val(txtValue.Text))

						txtValue = CType(Gridcurrentrow.FindControl("txtTax"), TextBox)
						.TaxPercentage = CDec(Val(txtValue.Text))

						'.TaxCAmount = (.TaxPercentage * .CAmount) / 100
						.TaxCAmount = (.TaxPercentage * .CRate) / 100

						SetGSTRates(mWOInvoiceJob:=mWOInvoiceJob, HSNACSID:=mCapabilityTaskList.Item(New Guid(cmbValue.SelectedValue.ToString)).HSNACSID.ToString, currentRow:=currentRow)

						'If ChkIsMixedCombinedGSTRateApplicable.Checked Then
						'    SetMAXGSTPercentageofJobItem()
						'Else
						'    SetGSTRates(mWOInvoiceJob:=mWOInvoiceJob)
						'End If
						'End
					End If
				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			i = i + 1


		Next



		'Invoice Spare
		Dim mWOInvoiceSpare As WOInvoiceSpare
		Dim j As Integer = 0
		For Each mWOInvoiceSpare In mWOInvoice.WOInvoiceSpares

			With mWOInvoiceSpare
				.ConversionFactor = Val(txtConversionFactor.Text)
				Try
					Dim txt As TextBox
					txt = dgWOInvoiceSpares.Rows(j).FindControl("txtSparesPartNo")
					If (txt.Text.Trim.IndexOf("[") > 0 And txt.Text.Trim.IndexOf("]") > 0) Then
						SparePartNo = txt.Text.Substring(0, txt.Text.Trim.IndexOf("[")).Trim
						SpareDescription = Mid(txt.Text.Trim, txt.Text.Trim.IndexOf("[") + 2, txt.Text.Trim.IndexOf("]") - txt.Text.Trim.IndexOf("[") - 1).Trim
					Else
						SparePartNo = Trim(txt.Text)
						SpareDescription = Trim(txt.Text)
					End If
					.PartNo = SparePartNo
					.Description = SpareDescription
					mFetchItemByName = FetchItemByName.GetItemByName(SparePartNo)
					.PartID = mFetchItemByName(0).ID

					txt = dgWOInvoiceSpares.Rows(j).FindControl("txtSpareQty")
					.Qty = Val(txt.Text)

					txtValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtSpareRate"), TextBox)

					.CRate = CDec(Val(txtValue.Text))

					txtValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtTax"), TextBox)
					.TaxPercentage = CDec(Val(txtValue.Text))
					'''' .TaxCAmount = (.TaxPercentage * .CAmount) / 100
					.TaxCAmount = (mWOInvoiceSpare.TaxPercentage * (mWOInvoiceSpare.CRate * mWOInvoiceSpare.Qty)) / 100
					''GST

					SetGSTRates(mnWOInvoiceSpare:=mWOInvoiceSpare)

					'If ChkIsMixedCombinedGSTRateApplicable.Checked Then
					'    SetMAXGSTPercentageofJobItem()
					'Else
					'    SetGSTRates(mWOInvoiceSpare:=mWOInvoiceSpare)
					'End If
					''End
				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			j = j + 1
		Next
		SetOtherChargeobject()
		mWOInvoice = Session("mWOInvoice")
		mWOInvoice.CalculateTotal(ConsiderJobChargesForCalculation:=True)
		Session("mWOInvoice") = mWOInvoice
	End Sub
	Private Function SetSpareOtherChargeobject() As Boolean
		Dim mWOInvoiceSparesCharge As WOInvoiceSparesCharge
		Dim j As Integer = 0
		For Each mWOInvoiceSparesCharge In mWOInvoice.WOInvoiceSparesCharges

			With mWOInvoiceSparesCharge
				.ConversionFactor = Val(txtConversionFactor.Text)
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtSpareCharge")

					mWOInvoice.WOInvoiceSparesCharges(j).ChargeName = Trim(txt.Text)

					mFetchChargeByName = ChargeList.GetChargeList(Trim(txt.Text))
					Dim ID As Guid = mFetchChargeByName(0).ID
					mWOInvoice.WOInvoiceSparesCharges(j).ChargeID = mFetchChargeByName(0).ID
					mWOInvoice.WOInvoiceSparesCharges(j).ChargeName = mFetchChargeByName(ID).Name


					Dim txtPercentage, txtChargeAmount As TextBox
					txtPercentage = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtPercentage")
					txtChargeAmount = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtSpareChargeAmount")

					mWOInvoice.WOInvoiceSparesCharges(j).ConversionFactor = mWOInvoice.ConversionFactor
					mWOInvoice.WOInvoiceSparesCharges(j).Percentage = Val(txtPercentage.Text)
					mWOInvoice.WOInvoiceSparesCharges(j).CChargeAmount = Val(txtChargeAmount.Text)


					'Color
					txtPercentage.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 3)
					txtChargeAmount.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 1)

					txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
					txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
					'txtChargeAmount.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, 0, txtChargeAmount.Text)
					mWOInvoice.WOInvoiceSparesCharges(j).Percentage = Val(txtPercentage.Text)
					mWOInvoice.WOInvoiceSparesCharges(j).CChargeAmount = Val(txtChargeAmount.Text)


				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			j = j + 1
		Next
		Session("mWOInvoice") = mWOInvoice
	End Function
	Private Function SetOtherChargeobject() As Boolean
		Dim mWOInvoiceJobCharge As WOInvoiceJobCharge
		Dim j As Integer = 0
		For Each mWOInvoiceJobCharge In mWOInvoice.WOInvoiceJobCharges

			With mWOInvoiceJobCharge
				.ConversionFactor = Val(txtConversionFactor.Text)
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtCharge")

					mWOInvoice.WOInvoiceJobCharges(j).ChargeName = Trim(txt.Text)

					mFetchChargeByName = ChargeList.GetChargeList(Trim(txt.Text))
					Dim ID As Guid = mFetchChargeByName(0).ID
					mWOInvoice.WOInvoiceJobCharges(j).ChargeID = mFetchChargeByName(0).ID
					mWOInvoice.WOInvoiceJobCharges(j).ChargeName = mFetchChargeByName(ID).Name


					Dim txtPercentage, txtChargeAmount As TextBox
					txtPercentage = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtPercentage")
					txtChargeAmount = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtChargeAmount")

					mWOInvoice.WOInvoiceJobCharges(j).ConversionFactor = mWOInvoice.ConversionFactor
					mWOInvoice.WOInvoiceJobCharges(j).Percentage = Val(txtPercentage.Text)
					mWOInvoice.WOInvoiceJobCharges(j).CChargeAmount = Val(txtChargeAmount.Text)


					'Color
					txtPercentage.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 3)
					txtChargeAmount.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 1)

					txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
					txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
					'txtChargeAmount.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, 0, txtChargeAmount.Text)
					mWOInvoice.WOInvoiceJobCharges(j).Percentage = Val(txtPercentage.Text)
					mWOInvoice.WOInvoiceJobCharges(j).CChargeAmount = Val(txtChargeAmount.Text)


				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			j = j + 1
		Next
		Session("mWOInvoice") = mWOInvoice
	End Function
	Public Function FetchItemByNameCount(Optional ByVal PartNo As String = "", Optional ByVal IsForID As Integer = 0) As Object
		If (PartNo.Trim.IndexOf("[") > 0 And PartNo.Trim.IndexOf("]") > 0) Then
			SparePartNo = PartNo.Substring(0, PartNo.Trim.IndexOf("[")).Trim
			SpareDescription = Mid(PartNo.Trim, PartNo.Trim.IndexOf("[") + 2, PartNo.Trim.IndexOf("]") - PartNo.Trim.IndexOf("[") - 1).Trim
		Else
			SparePartNo = Trim(PartNo)
			SpareDescription = Trim(PartNo)
		End If
		mFetchItemByName = FetchItemByName.GetItemByName(SparePartNo)
		If IsForID = 0 Then
			Return mFetchItemByName.Count
		ElseIf IsForID = 1 Then
			Return mFetchItemByName(0).ID
		ElseIf IsForID = 2 Then
			Return SparePartNo
		ElseIf IsForID = 3 Then
			Return SpareDescription
		End If
		Return ""
	End Function
	Private Function Save() As Boolean

		Try
			setObject()
			mWOInvoice.ApplyEdit()


			If mWOInvoice.IsValid Then
				mWOInvoice.Save()
			Else
				upnlValidationsummary.Update()
			End If

			DataFieldBind()
			ControlVisibility()
			SetPage()
			SetJobChargeGrid()
			SetSpareChargeGrid()
			'MarkLog(Util.Action.Save, "WOInvoice", "", Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)
			Dim InvoiceDetail = mWOInvoice.InvoiceText + " Dated : " + mWOInvoice.WOInvoiceDateFormatted + " for " + mWOInvoice.WOTextNo
			MarkLog(Util.Action.Save, "WOInvoice", User.Identity.Name + " Saved Quotation : " + InvoiceDetail + " SuccessFully.", Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)

			Return True
		Catch ex As SqlException
			If ex.Number = 8145 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
			ElseIf ex.Number = 2627 Or ex.Number = 2601 Then

				Dim DuplicateEntryMessage As String = String.Empty
				If InStr(ex.Message, "UK_tabnWOInvoiceSpare", CompareMethod.Text) Then
					MSGBoxCtrl.Show("Duplicate Alert!", "You are trying to save the duplicate entry for Spare", ex.Message, MsgBoxStyle.OkOnly, "")
				ElseIf InStr(ex.Message, "UK_tabnWOInvoiceTerm", CompareMethod.Text) Then
					MSGBoxCtrl.Show("Duplicate Alert!", "You are trying to save the duplicate entry for Term", ex.Message, MsgBoxStyle.OkOnly, "")
				Else
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
				End If


			ElseIf ex.Number = 547 Then
				MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
			Else
				MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
			End If
			Return False
		Catch ex1 As Exception


			Return False
		End Try

	End Function
	Private Sub SetPage()

		If mWOInvoice.IsNew = True Then
			lblTitle.InnerText = "Invoice For " + mWOInvoice.WOTextNo.ToString + " [ NEW ]"
		Else
			lblTitle.InnerText = "Invoice For " + mWOInvoice.WOTextNo.ToString + " [" + mWOInvoice.InvoiceText.ToString + "]"
		End If
		upnlTitle.Update()
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
					If MSGBoxCtrl.Sender = "DeleteJobCharge" Then
						Try
							Session("Sender") = ""
							Dim mWOInvoice As WOInvoice
							mWOInvoice = CType(Session("mWOInvoice"), WOInvoice)
							mWOInvoice.WOInvoiceJobCharges.Remove(mWOInvoice.WOInvoiceJobCharges.CurrentItem)
							mWOInvoice.CalculateTotal(IsChargeItemDeleted:=True)
							'If mWOInvoice.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012 'BHushan
							'    mWOInvoice.RoundCGrandTotal()
							'End If
							Session("mWOInvoice") = mWOInvoice
							WOInvoiceJobChargesGrid()
							ControlVisibility()
							SetJobChargeGrid()
						Catch ex As SqlException
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
							Exit Sub
						End Try
					End If
					If MSGBoxCtrl.Sender = "DeleteSpareCharge" Then
						Try
							Session("Sender") = ""
							Dim mWOInvoice As WOInvoice
							mWOInvoice = CType(Session("mWOInvoice"), WOInvoice)
							mWOInvoice.WOInvoiceSparesCharges.Remove(mWOInvoice.WOInvoiceSparesCharges.CurrentItem)
							mWOInvoice.CalculateTotal()
							'If mWOInvoice.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012 'BHushan
							'    mWOInvoice.RoundCGrandTotal()
							'End If
							Session("mWOInvoice") = mWOInvoice
							WOInvoiceSpareChargesGrid()
							SetSpareChargeGrid()
						Catch ex As SqlException
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
							Exit Sub
						End Try
					End If

					If MSGBoxCtrl.Sender = "Status" Then
						Session("sender") = ""
						If mWOInvoice.IsValid = True Then
							mWOInvoice.StatusID = 2
							DataFieldBind()
							Save()
							SetJobChargeGrid()
							SetSpareChargeGrid()
							upnlWOInvoiceTerms.Update()
							Dim InvoiceDetail = mWOInvoice.InvoiceText + " Dated : " + mWOInvoice.WOInvoiceDateFormatted + " for " + mWOInvoice.WOTextNo
							MarkLog(Util.Action.Authorize, "WOInvoice", User.Identity.Name + " Authorized Invoice : " + InvoiceDetail, Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)
							MSGBoxCtrl.Show("Authorized!", "Authorized SuccessFully", "", MsgBoxStyle.OkOnly, "")
						Else
							If CustomValidate1() = False Then
								upnlValidationsummary.Update()
								Exit Sub
							End If
						End If
					End If
					If MSGBoxCtrl.Sender = "StatusCancel" Then
						Session("sender") = ""
						If mWOInvoice.IsValid = True Then
							mWOInvoice.StatusID = 4
							DataFieldBind()
							Save()
							SetJobChargeGrid()
							SetSpareChargeGrid()
							upnlWOInvoiceTerms.Update()
							Dim InvoiceDetail = mWOInvoice.InvoiceText + " Dated : " + mWOInvoice.WOInvoiceDateFormatted + " from " + mWOInvoice.WOTextNo
							MarkLog(Util.Action.Cancel, "WOInvoice", User.Identity.Name + " Canceled Invoice : " + InvoiceDetail, Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)
							MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")

						Else
							If CustomValidate1() = False Then
								upnlValidationsummary.Update()
								Exit Sub
							End If
						End If
					End If

					If MSGBoxCtrl.Sender = "DeleteInvoiceTerms" Then
						mWOInvoice = Session("mWOInvoice")
						mWOInvoice.WOInvoiceTerms.Remove(mWOInvoice.WOInvoiceTerms.CurrentItem)
						dgWOInvoiceTerms.DataSource = mWOInvoice.WOInvoiceTerms
						dgWOInvoiceTerms.DataBind()
						Session("mWOInvoice") = mWOInvoice
						upnlWOInvoiceTerms.Update()
					End If
					If MSGBoxCtrl.Sender = "Close" Then
						If Not CustomValidate1() Then
							upnlValidationsummary.Update()
							Exit Sub
						End If

						If Save() Then
							SetPage()
							MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
							Dim InvoiceDetail = mWOInvoice.InvoiceText + " Dated : " + mWOInvoice.WOInvoiceDateFormatted + " for " + mWOInvoice.WOTextNo
							MarkLog(Util.Action.Save, "WOInvoice", User.Identity.Name + " Saved Invoice : " + InvoiceDetail + " SuccessFully.", Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)
							Response.Redirect("Index.aspx")
						End If
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						Session.Remove("IsValid")
						Session("Sender") = ""
						Response.Redirect("Index.aspx")
					End If
					If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then
						Session("Sender") = ""
						Session.Remove("IsValid")
						Session("mWOInvoice") = mWOInvoice
						DataFieldBind()
						'upnlWOInvoiceJobsOtherChargesAmount.Update()
						upnlWOInvoiceJobsAndSpares.Update()
						upnlWOInvoiceTerms.Update()
					End If
					If MSGBoxCtrl.Sender = "AmendStatus" Then
						Session("Sender") = ""
						Session.Remove("IsValid")
						If mWOInvoice.StatusID = 2 Then
							mWOInvoice.StatusID = 1
						ElseIf mWOInvoice.StatusID = 3 Or mWOInvoice.StatusID = 4 Then
							mWOInvoice.StatusID = 2
						End If
						Session("mWOInvoice") = mWOInvoice
						DataFieldBind()
						SetJobChargeGrid()
						SetSpareChargeGrid()
						UpdatePanel()
					End If

				Case MsgBoxResult.Ok

			End Select
		End If
	End Sub
#End Region


#Region "Data Binding"
	Private Sub DataFieldBind()

		txtWOInvoiceDate.Text = mWOInvoice.WOInvoiceDateFormatted
		txtWO.Text = mWOInvoice.WOTextNo

		mCurrencyList = CurrencyList.GetCurrencyList(, , True)
		cmbCurrencyList.DataSource = mCurrencyList
		Session("mCurrencyList") = mCurrencyList

		mCapabilityTaskList = CapabilityTaskList.GetCapabilityTaskList(AddTopItem:="(SELECT)")
		Session("mCapabilityTaskList") = mCapabilityTaskList


		dgWOInvoiceJobs.DataSource = mWOInvoice.WOInvoiceJobs
		dgWOInvoiceSpares.DataSource = mWOInvoice.WOInvoiceSpares

		dgWOInvoiceJobOtherCharges.DataSource = mWOInvoice.WOInvoiceJobCharges
		dgWOInvoiceSpareOtherCharges.DataSource = mWOInvoice.WOInvoiceSparesCharges


		dgWOInvoiceTerms.DataSource = mWOInvoice.WOInvoiceTerms
		'Total 

		txtWOInvoiceJobsTotalAmount.Text = mWOInvoice.CTotalJobAmount
		txtWOInvoiceSparesTotalAmount.Text = mWOInvoice.CTotalSpareAmount
		SetSpareAndJobTotalCost() 'GST
		txtWOInvoiceJobOtherChargesTotalAmount.Text = mWOInvoice.CTotalJobCharges
		txtWOInvoiceSparesOtherChargesTotalAmount.Text = mWOInvoice.CTotalSpareCharges


		'End
		DataBind()

		'If mWOInvoice.IsNew Then
		'    cmbCurrencyList.SelectedIndex = 3
		'    txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
		'End If

		upnlWOInvoiceDetails.Update()
		upnlWOInvoiceJobsAndSpares.Update()

		upnlWOInvoiceJobsTotalAmount.Update()
		upnlWOInvoiceSparesTotalAmount.Update()

		upnlWOInvoiceJobOtherChargesAmount.Update()
		upnlWOInvoiceSparesOtherChargesAmount.Update()

		upnlWOInvoiceTotalJobEstimationAmount.Update()
		upnlWOInvoiceTotalspareEstimationAmount.Update()

		upnlGrandTotal.Update()


		upnlRemark.Update()
	End Sub
	Private Sub WOInvoiceJobChargesGrid()
		dgWOInvoiceJobOtherCharges.DataSource = mWOInvoice.WOInvoiceJobCharges
		dgWOInvoiceJobOtherCharges.DataBind()

		upnlWOInvoiceJobOtherCharges.Update()
		txtWOInvoiceJobsTotalAmount.Text = mWOInvoice.CTotalJobAmount
		txtWOInvoiceSparesTotalAmount.Text = mWOInvoice.CTotalSpareAmount
		SetSpareAndJobTotalCost() 'GST
		txtWOInvoiceJobOtherChargesTotalAmount.Text = mWOInvoice.CTotalJobCharges
		txtWOInvoiceSparesOtherChargesTotalAmount.Text = mWOInvoice.CTotalSpareCharges
		UpdatePanel()
	End Sub
	Private Sub WOInvoiceSpareChargesGrid()
		dgWOInvoiceSpareOtherCharges.DataSource = mWOInvoice.WOInvoiceSparesCharges
		dgWOInvoiceSpareOtherCharges.DataBind()

		upnlWOInvoiceJobOtherCharges.Update()
		upnlWOInvoiceSpareOtherCharges.Update()
		txtWOInvoiceJobsTotalAmount.Text = mWOInvoice.CTotalJobAmount
		txtWOInvoiceSparesTotalAmount.Text = mWOInvoice.CTotalSpareAmount
		SetSpareAndJobTotalCost() 'GST
		txtWOInvoiceJobOtherChargesTotalAmount.Text = mWOInvoice.CTotalJobCharges
		txtWOInvoiceSparesOtherChargesTotalAmount.Text = mWOInvoice.CTotalSpareCharges
		UpdatePanel()
	End Sub
	Public Function CustomValidate1() As Boolean
		Dim strMsg As String = ""
		setObject()
		If mWOInvoice.IsValid = False Then
			For i As Integer = 0 To mWOInvoice.GetBrokenRulesCollection.Count - 1
				strMsg = strMsg + mWOInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If
		Dim mWOInvoiceJob As WOInvoiceJob
		If mWOInvoice.WOInvoiceJobs.IsValid = False Then
			For Each mWOInvoiceJob In mWOInvoice.WOInvoiceJobs
				For i As Integer = 0 To mWOInvoiceJob.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mWOInvoiceJob.CustomerDescription + " : " + mWOInvoiceJob.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			Next
		End If

		Dim mWOInvoiceJobCharge As WOInvoiceJobCharge
		If mWOInvoice.WOInvoiceJobCharges.IsValid = False Then
			For Each mWOInvoiceJobCharge In mWOInvoice.WOInvoiceJobCharges
				For i As Integer = 0 To mWOInvoiceJobCharge.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mWOInvoiceJobCharge.ChargeName + " : " + mWOInvoiceJobCharge.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			Next
		End If

		Dim mWOInvoiceSpare As WOInvoiceSpare
		If mWOInvoice.WOInvoiceSpares.IsValid = False Then
			For Each mWOInvoiceSpare In mWOInvoice.WOInvoiceSpares
				For i As Integer = 0 To mWOInvoiceSpare.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mWOInvoiceSpare.PartNo + " : " + mWOInvoiceSpare.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			Next
		End If

		Dim mWOInvoiceSparesCharge As WOInvoiceSparesCharge
		If mWOInvoice.WOInvoiceSparesCharges.IsValid = False Then
			For Each mWOInvoiceSparesCharge In mWOInvoice.WOInvoiceSparesCharges
				For i As Integer = 0 To mWOInvoiceSparesCharge.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + "Spare Charges : " + mWOInvoiceSparesCharge.ChargeName + " : " + mWOInvoiceSparesCharge.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			Next
		End If

		If strMsg.Trim <> "" Then
			CustValidator.ErrorMessage = strMsg
			CustValidator.IsValid = False
			Return False
		End If
		Return True
	End Function
	Private Function CustomValidateJob() As Boolean

		'Dim strError As String = String.Empty

		'Dim txtAccHeadDescription As TextBox
		'Dim rfvAccHeadDescription As RequiredFieldValidator
		'Dim upnlAccHeadDescriptionValidate As UpdatePanel

		'For j As Integer = 0 To dgWOInvoiceJobs.Rows.Count - 1

		'    rfvAccHeadDescription = CType(Me.dgWOInvoiceJobs.Rows(j).FindControl("rfvAccHeadDescription"), RequiredFieldValidator)
		'    upnlAccHeadDescriptionValidate = CType(Me.dgWOInvoiceJobs.Rows(j).FindControl("upnlAccHeaderValidate"), UpdatePanel)
		'    txtAccHeadDescription = CType(Me.dgWOInvoiceJobs.Rows(j).FindControl("txtAccHeadHeader"), TextBox)

		'    If txtAccHeadDescription.Text = "" Then
		'        rfvAccHeadDescription.IsValid = False
		'        rfvAccHeadDescription.Text = "* Account Head Required"
		'        strError = "* Account Head Required"
		'        upnlAccHeadDescriptionValidate.Update()
		'    End If


		'Next
		'If strError <> "" Then
		'    Return False
		'End If
		Return True
	End Function
	Private Function CustomValidateSpare() As Boolean
		Dim strError As String = String.Empty
		Dim builder = New StringBuilder()


		Dim txtSparesPartNo As TextBox
		Dim rfvSpare As RequiredFieldValidator
		Dim upnlSparesPartNoValidate As UpdatePanel
		Dim cvValidator As CustomValidator




		For j As Integer = 0 To dgWOInvoiceSpares.Rows.Count - 1
			rfvSpare = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("rfvSpare"), RequiredFieldValidator)
			upnlSparesPartNoValidate = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("upnlSparesPartNoValidate"), UpdatePanel)
			txtSparesPartNo = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtSparesPartNo"), TextBox)
			cvValidator = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("cvSpare"), CustomValidator)

			If txtSparesPartNo.Text = "" Then
				rfvSpare.IsValid = False
				rfvSpare.Text = "* Part Required"
				strError = "* Part Required"
				upnlSparesPartNoValidate.Update()
			ElseIf FetchItemByNameCount(PartNo:=txtSparesPartNo.Text.Trim, IsForID:=0) = 0 Then
				'cvValidator.IsValid = False
				'cvValidator.Text = "* Enter whole part no. and description"
				'strError = "* Enter whole part no. and description"
				rfvSpare.IsValid = False
				rfvSpare.Text = "* Enter whole part no. and description"
				strError = "* Enter whole part no. and description"
				upnlSparesPartNoValidate.Update()
				'Else
				'    cvValidator.Visible = False
			End If

		Next
		If strError <> "" Then
			Return False
		End If
		Return True
	End Function
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtInvoiceDate" Then
			If txtWOInvoiceDate.Text = "" Then
				custValidator.ErrorMessage = "Select Invoice Date."
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
		ElseIf custValidator.ControlToValidate = "txtRemark" Then
			If Len(txtRemark.Text) > 200 Then
				custValidator.ErrorMessage = "Lenght of Remark should not greater than 200 Characters."
				e.IsValid = False
			End If
		End If
	End Sub
#End Region

#Region "Buissness Methods"
	Private Sub SetJobChargeGrid()
		Dim mWOInvoiceJobCharge As WOInvoiceJobCharge
		Dim j As Integer = 0
		For Each mWOInvoiceJobCharge In mWOInvoice.WOInvoiceJobCharges

			With mWOInvoiceJobCharge
				.ConversionFactor = Val(txtConversionFactor.Text)
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtCharge")

					mWOInvoice.WOInvoiceJobCharges(j).ChargeName = Trim(txt.Text)

					mFetchChargeByName = ChargeList.GetChargeList(Trim(txt.Text))
					Dim ID As Guid = mFetchChargeByName(0).ID

					Dim txtPercentage, txtChargeAmount As TextBox
					txtPercentage = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtPercentage")
					txtChargeAmount = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtChargeAmount")

					'Color
					txtPercentage.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 3)
					txtChargeAmount.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 1)

					txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
					txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)



				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			j = j + 1
		Next
		Session("mWOInvoice") = mWOInvoice
	End Sub 'End

	Private Sub SetSpareChargeGrid()
		Dim mWOInvoiceSparesCharge As WOInvoiceSparesCharge
		Dim j As Integer = 0
		For Each mWOInvoiceSparesCharge In mWOInvoice.WOInvoiceSparesCharges

			With mWOInvoiceSparesCharge
				.ConversionFactor = Val(txtConversionFactor.Text)
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtSpareCharge")

					mWOInvoice.WOInvoiceSparesCharges(j).ChargeName = Trim(txt.Text)

					mFetchChargeByName = ChargeList.GetChargeList(Trim(txt.Text))
					Dim ID As Guid = mFetchChargeByName(0).ID

					Dim txtPercentage, txtChargeAmount As TextBox
					txtPercentage = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtPercentage")
					txtChargeAmount = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtSpareChargeAmount")

					'Color
					txtPercentage.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 3)
					txtChargeAmount.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 1)

					txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
					txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)



				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End With
			j = j + 1
		Next
		Session("mWOInvoice") = mWOInvoice
	End Sub 'End
	Private Sub SetSpareAndJobTotalCost()
		txtTotalJobEstimation.Text = mWOInvoice.CTotalJobAmount + mWOInvoice.CTotalJobCharges '+ mWOInvoice.CTotalJobCGST + mWOInvoice.CTotalJobSGST + mWOInvoice.CTotalJobIGST '+ mWOInvoice.CTotalJobTax
		txtTotalSparesEstimation.Text = mWOInvoice.CTotalSpareAmount + mWOInvoice.CTotalSpareCharges '+ mWOInvoice.CTotalSpareCGST + mWOInvoice.CTotalSpareSGST + mWOInvoice.CTotalSpareIGST '+ mWOInvoice.CTotalSpareTax
		txtGrandTotal.Text = mWOInvoice.CGrandTotal

		upnlWOInvoiceSparesTotalAmount.Update()
		upnlWOInvoiceJobsTotalAmount.Update()
		upnlWOInvoiceTotalspareEstimationAmount.Update()
		upnlWOInvoiceTotalJobEstimationAmount.Update()
		upnlGrandTotal.Update()
	End Sub
	Private Sub ControlVisibility()

		txtWOInvoiceDate.Enabled = IIf(Not mWOInvoice.IsNew, False, True)
		txtWOInvoiceText.Enabled = IIf(Not mWOInvoice.IsNew, False, True)
		txtWOInvoiceNo.Enabled = IIf(Not mWOInvoice.IsNew, False, True)

		cmbCurrencyList.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)
		txtConversionFactor.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)

		''''''txtWOInvoiceJobsTotalAmount.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)
		''''''txtWOInvoiceSparesTotalAmount.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)

		txtWOInvoiceJobOtherChargesTotalAmount.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)
		txtWOInvoiceSparesOtherChargesTotalAmount.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)

		txtTotalJobEstimation.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)
		txtTotalSparesEstimation.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)

		txtGrandTotal.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)


		ImgAddWOInvoiceSpare.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)

		ImgAddWOInvoiceJobOtherCharges.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True) And mWOInvoice.WOInvoiceJobs.Count > 0
		ImgAddWOInvoiceSpareOtherCharges.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True) And mWOInvoice.WOInvoiceSpares.Count > 0

		ImgWOInvoiceTerms.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)

		dgWOInvoiceJobs.Enabled = IIf(mWOInvoice.StatusID >= 2 Or mWOInvoice.WOInvoiceJobCharges.Count > 0, False, True)
		dgWOInvoiceSpares.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)
		dgWOInvoiceJobOtherCharges.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)
		dgWOInvoiceSpareOtherCharges.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)
		dgWOInvoiceTerms.Enabled = IIf(mWOInvoice.StatusID >= 2, False, True)


		btnCancel.Visible = (Not mWOInvoice.IsNew) And (mWOInvoice.StatusID = 2)
		btnAuthorized.Visible = (Not mWOInvoice.IsNew) And (mWOInvoice.StatusID = 1)
		btnSave.Visible = (Not mWOInvoice.StatusID >= 2)
		btnPrint.Visible = (Not mWOInvoice.IsNew)

		Dim txtCGSTPer As TextBox
		Dim txtSGSTPer As TextBox
		Dim txtIGSTPer As TextBox
		For i As Integer = 0 To dgWOInvoiceJobs.Rows.Count - 1
			txtCGSTPer = CType(Me.dgWOInvoiceJobs.Rows(i).FindControl("txtCGSTPer"), TextBox)
			'txtCGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mWOInvoice.Invoicejobs(i).HSNACSCode = "", False, True)
			txtCGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False", False, True)


			txtSGSTPer = CType(Me.dgWOInvoiceJobs.Rows(i).FindControl("txtSGSTPer"), TextBox)
			txtSGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False", False, True)



			txtIGSTPer = CType(Me.dgWOInvoiceJobs.Rows(i).FindControl("txtIGSTPer"), TextBox)
			'txtIGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mWOInvoice.Invoicejobs(i).HSNACSCode = "", False, True)
			txtIGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False", False, True)
		Next

		Dim txtSpareCGSTPer As TextBox
		Dim txtSpareSGSTPer As TextBox
		Dim txtSpareIGSTPer As TextBox
		For i As Integer = 0 To dgWOInvoiceSpares.Rows.Count - 1
			txtSpareCGSTPer = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareCGSTPer"), TextBox)
			'txtCGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mWOInvoice.Invoicejobs(i).HSNACSCode = "", False, True)
			txtSpareCGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False", False, True)


			txtSpareSGSTPer = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareSGSTPer"), TextBox)
			txtSpareSGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False", False, True)



			txtSpareIGSTPer = CType(Me.dgWOInvoiceSpares.Rows(i).FindControl("txtSpareIGSTPer"), TextBox)
			'txtIGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mWOInvoice.Invoicejobs(i).HSNACSCode = "", False, True)
			txtSpareIGSTPer.Enabled = IIf(mWOInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False", False, True)
		Next


		If mWOInvoice.Visibility = 1 And ((cmbCurrencyList.SelectedItem.Text).ToUpper = "INR" Or (cmbCurrencyList.SelectedItem.Text).ToUpper.ToString.Contains("RUPEE")) Then
			'dgWOInvoiceJobs.Columns(18).Visible = True  'HSNACSCode 
			dgWOInvoiceJobs.Columns(8).Visible = True           'CGSTPercentage 
			dgWOInvoiceJobs.Columns(9).Visible = True           'CGSTCAmount 
			dgWOInvoiceJobs.Columns(10).Visible = True          'SGSTPercentage 
			dgWOInvoiceJobs.Columns(11).Visible = True          'SGSTCAmount 
			dgWOInvoiceJobs.Columns(12).Visible = False         'IGSTPercentage 
			dgWOInvoiceJobs.Columns(13).Visible = False         'IGSTCAmount 
			dgWOInvoiceJobs.Columns(14).Visible = True          'TotalCAmount 

			'lblTotalCGST.Visible = True
			'txtTotalCGST.Visible = True
			'lblTotalSGST.Visible = True
			'txtTotalSGST.Visible = True

			'lblTotalIGST.Visible = False
			'txtTotalIGST.Visible = False

			dgWOInvoiceSpares.Columns(8).Visible = True         'CGSTPercentage 
			dgWOInvoiceSpares.Columns(9).Visible = True         'CGSTCAmount 
			dgWOInvoiceSpares.Columns(10).Visible = True        'SGSTPercentage 
			dgWOInvoiceSpares.Columns(11).Visible = True        'SGSTCAmount 
			dgWOInvoiceSpares.Columns(12).Visible = False       'IGSTPercentage 
			dgWOInvoiceSpares.Columns(13).Visible = False       'IGSTCAmount 
			dgWOInvoiceSpares.Columns(14).Visible = True        'TotalCAmount 

		ElseIf mWOInvoice.Visibility = 2 And ((cmbCurrencyList.SelectedItem.Text).ToUpper = "INR" Or (cmbCurrencyList.SelectedItem.Text).ToUpper.ToString.Contains("RUPEE")) Then
			'dgWOInvoiceJobs.Columns(18).Visible = True  'HSNACSCode 
			dgWOInvoiceJobs.Columns(8).Visible = False 'CGSTPercentage 
			dgWOInvoiceJobs.Columns(9).Visible = False 'CGSTCAmount 
			dgWOInvoiceJobs.Columns(10).Visible = False 'SGSTPercentage 
			dgWOInvoiceJobs.Columns(11).Visible = False 'SGSTCAmount 
			dgWOInvoiceJobs.Columns(12).Visible = True  'IGSTPercentage 
			dgWOInvoiceJobs.Columns(13).Visible = True 'IGSTCAmount 
			dgWOInvoiceJobs.Columns(14).Visible = True 'TotalCAmount 

			'lblTotalCGST.Visible = False
			'txtTotalCGST.Visible = False
			'lblTotalSGST.Visible = False
			'txtTotalSGST.Visible = False

			'lblTotalIGST.Visible = True
			'txtTotalIGST.Visible = True

			dgWOInvoiceSpares.Columns(8).Visible = False 'CGSTPercentage 
			dgWOInvoiceSpares.Columns(9).Visible = False 'CGSTCAmount 
			dgWOInvoiceSpares.Columns(10).Visible = False 'SGSTPercentage 
			dgWOInvoiceSpares.Columns(11).Visible = False 'SGSTCAmount 
			dgWOInvoiceSpares.Columns(12).Visible = True  'IGSTPercentage 
			dgWOInvoiceSpares.Columns(13).Visible = True 'IGSTCAmount 
			dgWOInvoiceSpares.Columns(14).Visible = True 'TotalCAmount 

		ElseIf mWOInvoice.Visibility = 3 Or ((cmbCurrencyList.SelectedItem.Text).ToUpper <> "INR" And Not (cmbCurrencyList.SelectedItem.Text).ToUpper.ToString.Contains("RUPEE")) Then
			'If AppSettings("HSNACSCodeVisibleInPartMaster") = "True" Then
			'    dgWOInvoiceJobs.Columns(18).Visible = True 'HSNACSCode 
			'Else
			'    dgWOInvoiceJobs.Columns(18).Visible = False 'HSNACSCode  
			'End If
			dgWOInvoiceJobs.Columns(8).Visible = False 'CGSTPercentage 
			dgWOInvoiceJobs.Columns(9).Visible = False 'CGSTCAmount 
			dgWOInvoiceJobs.Columns(10).Visible = False 'SGSTPercentage 
			dgWOInvoiceJobs.Columns(11).Visible = False 'SGSTCAmount 
			dgWOInvoiceJobs.Columns(12).Visible = False  'IGSTPercentage 
			dgWOInvoiceJobs.Columns(13).Visible = False 'IGSTCAmount 
			dgWOInvoiceJobs.Columns(14).Visible = False 'TotalCAmount 
			'lblTotalCGST.Visible = False
			'txtTotalCGST.Visible = False
			'lblTotalSGST.Visible = False
			'txtTotalSGST.Visible = False
			'lblTotalIGST.Visible = False
			'txtTotalIGST.Visible = False

			dgWOInvoiceSpares.Columns(8).Visible = False 'CGSTPercentage 
			dgWOInvoiceSpares.Columns(9).Visible = False 'CGSTCAmount 
			dgWOInvoiceSpares.Columns(10).Visible = False 'SGSTPercentage 
			dgWOInvoiceSpares.Columns(11).Visible = False 'SGSTCAmount 
			dgWOInvoiceSpares.Columns(12).Visible = False  'IGSTPercentage 
			dgWOInvoiceSpares.Columns(13).Visible = False 'IGSTCAmount 
			dgWOInvoiceSpares.Columns(14).Visible = False 'TotalCAmount 
		End If

		UpdatePanel()

		'End
		'''btnSendMail.Visible = IIf(mWOInvoice.StatusID = 2, True, False)
	End Sub
	Private Sub UpdatePanel()

		upnlWOInvoiceDetails.Update()
		upnlWOInvoiceJobsAndSpares.Update()
		upnlWOInvoiceJobsTotalAmount.Update()
		upnlWOInvoiceSparesTotalAmount.Update()
		'upnlWOInvoiceJobOtherCharges.Update()
		'upnlWOInvoiceSpareOtherCharges.Update()
		'upnlWOInvoiceJobsOtherChargesAmount.Update()
		'upnlWOInvoiceSparesOtherChargesAmount.Update()
		upnlWOInvoiceTotalJobEstimationAmount.Update()
		upnlWOInvoiceTotalspareEstimationAmount.Update()
		upnlGrandTotal.Update()
		upnlWOInvoiceTerms.Update()
		upnlStatusName.Update()
		upnlButtons.Update()
		upnlWOInvoiceJobs.Update()

	End Sub
#End Region

#Region "Events"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		addAttributes()
		If Not IsPostBack Then
			DataFieldBind()
			SetPage()
			ControlVisibility()
			SetJobChargeGrid()
			SetSpareChargeGrid()
		End If
		SetAttributes()
	End Sub
	Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If Not CustomValidateSpare() Or CustomValidateJob() = False Then
			upnlValidationsummary.Update()
			Exit Sub
		End If

		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateSpares", "CheckDuplicateSpares();", True)
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateTerms", "CheckDuplicateTerms();", True)

		If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

		If CustomValidate1() Then

			If (Not IsInRole(Rights.[New]) And mWOInvoice.IsNew) Or (Not IsInRole(Rights.Edit) And Not mWOInvoice.IsNew) Then
				MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			If Save() Then
				SetPage()
				MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
				Dim InvoiceDetail = mWOInvoice.InvoiceText + " Dated : " + mWOInvoice.WOInvoiceDateFormatted + " for " + mWOInvoice.WOTextNo
				MarkLog(Util.Action.Save, "WOInvoice", User.Identity.Name + " Saved Invoice : " + InvoiceDetail + " SuccessFully.", Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)
				'Added on 21-May-2018 by Shital
				mWOInvoice = WOInvoice.GetWOInvoice(mWOInvoice.ID)
				Session("mWOInvoice") = mWOInvoice
				'----------
			End If
		Else
			upnlValidationsummary.Update()
		End If

	End Sub
	Private Sub dgWOInvoiceSpares_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOInvoiceSpares.RowCommand
		Select Case e.CommandName
			Case "DeleteRec"
				mWOInvoice.WOInvoiceSpares.Remove(CInt(e.CommandArgument) - 1)
				dgWOInvoiceSpares.DataSource = mWOInvoice.WOInvoiceSpares
				dgWOInvoiceSpares.DataBind()
				Session("mWOInvoice") = mWOInvoice
				'GST
				mWOInvoice.CalculateTotal(IsChargeItemDeleted:=True)
				Session("mWOInvoice") = mWOInvoice
				ControlVisibility()
				'''''''txtWOInvoiceSparesTotalAmount.Text = mWOInvoice.CTotalSpareAmount
				txtTotalSparesEstimation.Text = mWOInvoice.CTotalSpareAmount + mWOInvoice.CTotalSpareCharges
				txtGrandTotal.Text = mWOInvoice.CGrandTotal

				'GST
				SetSpareAndJobTotalCost()

				upnlWOInvoiceSparesTotalAmount.Update()
				upnlWOInvoiceTotalspareEstimationAmount.Update()
				upnlGrandTotal.Update()
				'End
		End Select
	End Sub
	Private Sub btnAuthorized_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click

		If (Not IsInRole(Rights.Authorized)) Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
			Exit Sub
		End If

		If IsValid Then
			Session("mWOInvoice") = mWOInvoice
			MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>Invoice</strong>", MsgBoxStyle.YesNo, "Status")
		End If
	End Sub
	Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim ParentGridViewID As String
		Dim txtSpareValue As TextBox
		Dim mWOInvoiceJob As WOInvoiceJob
		Dim currentRow As GridViewRow
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateSpares", "CheckDuplicateSpares();", True)
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateTerms", "CheckDuplicateTerms();", True)


		mWOInvoice = Session("mWOInvoice")


		If sender.id <> "cmbCapability" Then
			ParentGridViewID = CType(CType(CType(sender, TextBox).NamingContainer, GridViewRow).NamingContainer, GridView).ID.ToString
			currentRow = CType(sender, TextBox).Parent.Parent
		Else
			ParentGridViewID = CType(CType(CType(sender, DropDownList).NamingContainer, GridViewRow).NamingContainer, GridView).ID.ToString
			currentRow = CType(sender, DropDownList).Parent.Parent
		End If

		setObject(currentRow)
		Dim txtSpareCGSTPer As TextBox
		Dim j As Integer = 0
		For Each mWOInvoiceSpare In mWOInvoice.WOInvoiceSpares
			With mWOInvoiceSpare
				Try

					'txtValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtQty"), TextBox)
					'txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value)")
					'mWOInvoiceSpare.Qty = CDec(Val(txtValue.Text))

					txtSpareValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtSpareRate"), TextBox)
					txtSpareValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtSpareValue.ClientID + "').value)")
					mWOInvoiceSpare.CRate = CDec(Val(txtSpareValue.Text))

					txtSpareValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtTax"), TextBox)
					txtSpareValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtSpareValue.ClientID + "').value)")

					mWOInvoiceSpare.TaxPercentage = CDec(Val(txtSpareValue.Text))
					''  mWOInvoiceSpare.TaxCAmount = (mWOInvoiceSpare.TaxPercentage * mWOInvoiceSpare.CAmount) / 100
					mWOInvoiceSpare.TaxCAmount = (mWOInvoiceSpare.TaxPercentage * (mWOInvoiceSpare.CRate * mWOInvoiceSpare.Qty)) / 100

					txtSpareCGSTPer = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtSpareCGSTPer"), TextBox)
					txtSpareCGSTPer.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtSpareCGSTPer.ClientID + "').value,event)")

					txtSpareValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtSpareSGSTPer"), TextBox)
					txtSpareValue.Text = Val(txtSpareCGSTPer.Text)

					txtSpareValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtSpareIGSTPer"), TextBox)
					txtSpareValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtSpareValue.ClientID + "').value,event)")


					''GST
					'If ChkIsMixedCombinedGSTRateApplicable.Checked Then
					'    SetMAXGSTPercentageofJobItem()
					'Else
					'    SetGSTRates(mWOInvoiceSpare:=mWOInvoiceSpare)
					'End If
					''End
				Catch ex As Exception
				End Try
			End With
			j = j + 1
		Next

		dgWOInvoiceJobs.DataSource = mWOInvoice.WOInvoiceJobs
		dgWOInvoiceJobs.DataBind()

		dgWOInvoiceSpares.DataSource = mWOInvoice.WOInvoiceSpares
		dgWOInvoiceSpares.DataBind()
		mWOInvoice.CalculateTotal(ConsiderJobChargesForCalculation:=IIf(ParentGridViewID = "dgWOInvoiceJobs", True, False))
		mWOInvoice = Session("mWOInvoice")
		upnlWOInvoiceJobsAndSpares.Update()
		dgWOInvoiceJobs.DataSource = mWOInvoice.WOInvoiceJobs

		'dgWOInvoiceJobsOtherCharges.DataSource = mWOInvoice.WOInvoiceJobCharges
		'dgWOInvoiceSpareOtherCharges.DataSource = mWOInvoice.WOInvoiceSparesCharges


		dgWOInvoiceTerms.DataSource = mWOInvoice.WOInvoiceTerms

		'GST

		UpdatePanel()

		'Total 
		'''''''txtWOInvoiceJobsTaxTotalAmount.Text = mWOInvoice.CTotalJobTax
		'''''''txtWOInvoiceSparesTaxTotalAmount.Text = mWOInvoice.CTotalSpareTax

		txtWOInvoiceJobsTotalAmount.Text = mWOInvoice.CTotalJobAmount
		txtWOInvoiceSparesTotalAmount.Text = mWOInvoice.CTotalSpareAmount

		SetSpareAndJobTotalCost()
		'txtWOInvoiceJobOtherChargesTotalAmount.Text = mWOInvoice.CTotalJobCharges
		'txtWOInvoiceSparesOtherChargesTotalAmount.Text = mWOInvoice.CTotalSpareCharges

		ControlVisibility()
		SetPage()
		SetJobChargeGrid()
		SetSpareChargeGrid()
	End Sub
	'Public Sub cmbCapability_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
	'    Dim ParentGridViewID As String

	'    Dim currentRow As GridViewRow
	'    Dim txtValue As TextBox

	'    mWOInvoice = Session("mWOInvoice")

	'    ParentGridViewID = CType(CType(CType(sender, DropDownList).NamingContainer, GridViewRow).NamingContainer, GridView).ID.ToString
	'    currentRow = CType(sender, DropDownList).Parent.Parent


	'    Dim txtManHrs As TextBox = CType(currentRow.FindControl("txtManHrs"), TextBox)
	'    Dim cmbCapability As DropDownList = CType(currentRow.FindControl("cmbCapability"), DropDownList)
	'    Dim txtCRate As TextBox = CType(currentRow.FindControl("txtRate"), TextBox)


	'    SetGSTRates(HSNACSID:=mCapabilityTaskList.Item(New Guid(cmbCapability.SelectedValue.ToString)).HSNACSID.ToString, currentRow:=currentRow)


	'    'dgWOInvoiceJobs.DataSource = mWOInvoice.WOInvoiceJobs
	'    'dgWOInvoiceJobs.DataBind()

	'    'dgWOInvoiceSpares.DataSource = mWOInvoice.WOInvoiceSpares
	'    'dgWOInvoiceSpares.DataBind()
	'    mWOInvoice.CalculateTotal(ConsiderJobChargesForCalculation:=IIf(ParentGridViewID = "dgWOInvoiceJobs", True, False))
	'    mWOInvoice = Session("mWOInvoice")
	'    upnlWOInvoiceJobsAndSpares.Update()

	'    'GST

	'    UpdatePanel()


	'    txtWOInvoiceJobsTotalAmount.Text = mWOInvoice.CTotalJobAmount
	'    txtWOInvoiceSparesTotalAmount.Text = mWOInvoice.CTotalSpareAmount

	'    SetSpareAndJobTotalCost()
	'    'txtWOInvoiceJobOtherChargesTotalAmount.Text = mWOInvoice.CTotalJobCharges
	'    'txtWOInvoiceSparesOtherChargesTotalAmount.Text = mWOInvoice.CTotalSpareCharges

	'    ControlVisibility()
	'    SetPage()
	'    SetJobChargeGrid()
	'    SetSpareChargeGrid()
	'End Sub
	Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click ''===============================WO - 2006-2007-1-19

		If (Not IsInRole(Rights.Authorized)) Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user to cancel this Quoation", False), True)
			Exit Sub
		End If

		If IsValid Then
			'   Dim IsInUse As IsInUse = IsInUse.GetIsInUseInvoiceINSalesOrder(mWOInvoice.ID)
			'If IsInUse.IsInUse Then
			'    MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, "<Strong> Invoice, It is used in  .</Strong>", MsgBoxStyle.OkOnly, "StatusCancel")
			'    Session("mWOInvoice") = mWOInvoice
			'    Exit Sub
			'End If

			MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Invoice </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
			Session("mWOInvoice") = mWOInvoice
		End If
	End Sub
	Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click

		Dim InvoiceDetail As String

		InvoiceDetail = mWOInvoice.InvoiceText + " Dated : " + mWOInvoice.WOInvoiceDateFormatted + " for " + mWOInvoice.WOTextNo

		MarkLog(Util.Action.Close, "WOInvoice", InvoiceDetail, Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)


		If mWOInvoice.IsDirty Then
			Session("IsValid") = "True"
			MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
		Else
			Response.Redirect("index.aspx")
		End If




	End Sub
	Private Sub ImgAddWOInvoiceSpare_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgAddWOInvoiceSpare.Click
		If CustomValidateSpare() = False Then upnlValidationsummary.Update() : Exit Sub
		setObject()
		mWOInvoice.WOInvoiceSpares.Add(mWOInvoice.ID)
		dgWOInvoiceSpares.DataSource = mWOInvoice.WOInvoiceSpares
		dgWOInvoiceSpares.DataBind()

		Dim lblSparesPartNoStar, lblSparesQtyStar As Label

		lblSparesPartNoStar = dgWOInvoiceSpares.HeaderRow().FindControl("lblSparesPartNoStar")
		lblSparesQtyStar = dgWOInvoiceSpares.HeaderRow().FindControl("lblSparesQtyStar")

		lblSparesPartNoStar.Visible = True
		lblSparesQtyStar.Visible = True

		Session("mWOInvoice") = mWOInvoice
		upnlValidationsummary.Update()
	End Sub

	Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
		If cmbCurrencyList.SelectedIndex > 0 Then
			mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
			If (mUser.IsCurrencywisePOLimit = True And mUser.UserCurrencywisePOLimits.Count > 0) Then
				If mUser.UserCurrencywisePOLimits.Contains(mIsApplicable:=True, mCurrencyID:=New Guid(cmbCurrencyList.SelectedValue)) = False Then
					'MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Alert, MessageText:=MSGBox.Message_text.Alert, ExtraMessage:="You are not authorized user to create Invoice in this currency. Select another currency.", ButtonToShow:=MsgBoxStyle.OkOnly, Sender:="")
					MSGBoxCtrl.Show("Not Authorized !", "You are not authorized user to create Invoice in this currency. Select another currency.", "", MsgBoxStyle.OkOnly, "")

					Exit Sub
				End If
			End If
		End If
		txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
		If cmbCurrencyList.Enabled = True Then
			setFocus(cmbCurrencyList)
		End If


		mWOInvoice.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
		dgWOInvoiceSpares.DataSource = mWOInvoice.WOInvoiceSpares

		ControlVisibility()

		upnlWOInvoiceSpares.DataBind()
		upnlWOInvoiceSpares.Update()
		'------------

		upnlWOInvoiceDetails.Update()

	End Sub
	'Private Sub ImgWOInvoiceTerms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImgWOInvoiceTerms.Click

	'    mdlTerms.Show()
	'    Session("mWOInvoice") = mWOInvoice
	'    TermsDataFieldBind()

	'End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub
	'Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
	'    If Not IsInRole(Rights.Print) Then
	'        MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
	'        Exit Sub
	'    End If

	'    SetReport()


	'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
	'End Sub
	Protected Sub txtSparesPartNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		' Dim txtQty As TextBox = TryCast(currentRow.FindControl("txtQty"), TextBox)

		Dim j As Integer = currentRow.DataItemIndex

		For i As Integer = 0 To mWOInvoice.WOInvoiceSpares.Count - 1
			If i = j Then
				mWOInvoice.WOInvoiceSpares(j).ConversionFactor = Val(txtConversionFactor.Text)
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceSpares.Rows(j).FindControl("txtSparesPartNo")
					If (txt.Text.Trim.IndexOf("[") > 0 And txt.Text.Trim.IndexOf("]") > 0) Then
						SparePartNo = txt.Text.Substring(0, txt.Text.Trim.IndexOf("[")).Trim
						SpareDescription = Mid(txt.Text.Trim, txt.Text.Trim.IndexOf("[") + 2, txt.Text.Trim.IndexOf("]") - txt.Text.Trim.IndexOf("[") - 1).Trim
					Else
						SparePartNo = Trim(txt.Text)
						SpareDescription = Trim(txt.Text)
					End If
					mWOInvoice.WOInvoiceSpares(j).PartNo = SparePartNo
					mWOInvoice.WOInvoiceSpares(j).Description = SpareDescription
					mFetchItemByName = FetchItemByName.GetItemByName(SparePartNo)
					mWOInvoice.WOInvoiceSpares(j).PartID = mFetchItemByName(0).ID

					txt = dgWOInvoiceSpares.Rows(j).FindControl("txtSpareQty")
					mWOInvoice.WOInvoiceSpares(j).Qty = Val(txt.Text)

					txtValue = CType(Me.dgWOInvoiceSpares.Rows(j).FindControl("txtSpareRate"), TextBox)
					mWOInvoice.WOInvoiceSpares(j).CRate = CDec(Val(txtValue.Text))
					''GST
					'If ChkIsMixedCombinedGSTRateApplicable.Checked Then
					'    SetMAXGSTPercentageofJobItem()
					'Else
					'    SetGSTRates(mWOInvoiceSpare:=mWOInvoice.WOInvoiceSpares(j))
					'End If
					''End
					SetGSTRates(mnWOInvoiceSpare:=mWOInvoice.WOInvoiceSpares(j))

				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End If
		Next
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateSpares", "CheckDuplicateSpares();", True)
		mWOInvoice.CalculateTotal(ConsiderJobChargesForCalculation:=False)
		dgWOInvoiceSpares.DataSource = mWOInvoice.WOInvoiceSpares
		dgWOInvoiceSpares.DataBind()
		Session("mWOInvoice") = mWOInvoice

		SetSpareAndJobTotalCost()
		upnlWOInvoiceSparesTotalAmount.Update()
		upnlGrandTotal.Update()
	End Sub
	Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

		Dim da As New CSLA.Data.ObjectAdapter
		Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail

		Dim ds As New dsWOInvoice


		mWOInvoice = WOInvoice.GetWOInvoice(mWOInvoice.ID)

		If mWOInvoice.Visibility = 3 Then
			myReport = New crWOInvoiceDetail
		Else
			myReport = New crWOInvoiceGSTDetail
		End If

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			mCompanyDetail.WebSite, "", "", "",
			"", "", "", AppSettings("Product Version"), SearchStr8:=mWOInvoice.WOInvoiceJobCharges.Count.ToString, SearchStr9:=mWOInvoice.WOInvoiceSparesCharges.Count.ToString, SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("MROISONo"),
			SearchStr12:="TELEFAX:" & mCompanyDetail.Fax & " " & mCompanyDetail.Email, SINote:="", SearchStr13:=mWOInvoice.WOInvoiceSpares.Count.ToString)


		ds.Clear()
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mrptImage)


		da.Fill(ds, mWOInvoice)
		da.Fill(ds, "WOInvoiceWork", mWOInvoice.WOInvoiceJobs)
		da.Fill(ds, "WOInvoiceSpare", mWOInvoice.WOInvoiceSpares)

		da.Fill(ds, "WOInvoiceWorkCharge", mWOInvoice.WOInvoiceJobCharges)
		da.Fill(ds, "WOInvoiceSparesCharge", mWOInvoice.WOInvoiceSparesCharges)
		da.Fill(ds, "WOInvoiceTerm", mWOInvoice.WOInvoiceTerms)

		da.Fill(ds, Report)
		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport
		Dim Str1 As String
		Str1 = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)






	End Sub
#End Region

#Region "Job Other Charges"

	Private Function CustomValidateJobOtherCharges() As Boolean
		Dim strError As String = String.Empty
		Dim builder = New StringBuilder()


		Dim txtCharge As TextBox
		Dim rfvCharge As RequiredFieldValidator
		Dim upnlChargeValidate As UpdatePanel
		Dim cvValidator As CustomValidator

		For j As Integer = 0 To dgWOInvoiceJobOtherCharges.Rows.Count - 1
			rfvCharge = CType(Me.dgWOInvoiceJobOtherCharges.Rows(j).FindControl("rfvCharge"), RequiredFieldValidator)
			upnlChargeValidate = CType(Me.dgWOInvoiceJobOtherCharges.Rows(j).FindControl("upnlChargeValidate"), UpdatePanel)
			txtCharge = CType(Me.dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtCharge"), TextBox)
			cvValidator = CType(Me.dgWOInvoiceJobOtherCharges.Rows(j).FindControl("cvCharge"), CustomValidator)

			If txtCharge.Text = "" Then
				rfvCharge.IsValid = False
				rfvCharge.Text = "* Charge Required"
				strError = "* Charge Required"
				upnlChargeValidate.Update()
			Else
				mFetchChargeByName = ChargeList.GetChargeList(Trim(txtCharge.Text))
				If mFetchChargeByName.Count <= 0 Then
					rfvCharge.IsValid = False
					rfvCharge.Text = "* Select proper Charge"
					strError = "* Select proper Charge"
					upnlChargeValidate.Update()
				End If

				'Else
				'    cvValidator.Visible = False
			End If

		Next
		If strError <> "" Then
			Return False
		End If
		Return True
	End Function

#Region "Other Charges Events"
	Private Sub ImgAddWOInvoiceJobOtherCharges_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgAddWOInvoiceJobOtherCharges.Click
		If CustomValidateJobOtherCharges() = False Then upnlValidationsummary.Update() : Exit Sub
		setObject()
		mWOInvoice.WOInvoiceJobCharges.Add(mWOInvoice.ID)
		dgWOInvoiceJobOtherCharges.DataSource = mWOInvoice.WOInvoiceJobCharges
		dgWOInvoiceJobOtherCharges.DataBind()
		''upnlWOInvoiceJobOtherCharges.Update()
		SetJobChargeGrid()
		upnlWOInvoiceWorkOtherCharges.Update()
	End Sub
	Private Sub dgWOInvoiceJobOtherCharges_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOInvoiceJobOtherCharges.PageIndexChanging
		dgWOInvoiceJobOtherCharges.PageIndex = e.NewPageIndex
		dgWOInvoiceJobOtherCharges.DataSource = mWOInvoice.WOInvoiceJobCharges
		Session("mWOInvoice") = mWOInvoice
		dgWOInvoiceJobOtherCharges.DataBind()
	End Sub
	Protected Sub txtCharge_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		' Dim txtQty As TextBox = TryCast(currentRow.FindControl("txtQty"), TextBox)

		Dim j As Integer = currentRow.DataItemIndex

		For i As Integer = 0 To mWOInvoice.WOInvoiceJobCharges.Count - 1
			If i = j Then
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtCharge")



					mFetchChargeByName = ChargeList.GetChargeList(Trim(txt.Text))

					If mFetchChargeByName.Count > 0 Then
						Dim txtPercentage, txtChargeAmount As TextBox
						txtPercentage = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtPercentage")
						txtChargeAmount = dgWOInvoiceJobOtherCharges.Rows(j).FindControl("txtChargeAmount")
						Dim ID As Guid = mFetchChargeByName(0).ID

						If Not mWOInvoice.WOInvoiceJobCharges(j).ChargeName = Trim(txt.Text) Then
							txtChargeAmount.Text = "0"
							txtPercentage.Text = "0"
						End If

						mWOInvoice.WOInvoiceJobCharges(j).ChargeName = Trim(txt.Text)
						mWOInvoice.WOInvoiceJobCharges(j).ChargeID = mFetchChargeByName(0).ID
						mWOInvoice.WOInvoiceJobCharges(j).ChargeName = mFetchChargeByName(ID).Name




						txtPercentage.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 3)
						txtChargeAmount.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 1)

						txtPercentage.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, 0, mFetchChargeByName(ID).Percentage)
						' txtChargeAmount.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, txtChargeAmount.Text, 0)

						txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
						txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
						'txtChargeAmount.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, 0, txtChargeAmount.Text)
						mWOInvoice.WOInvoiceJobCharges(j).Percentage = Val(txtPercentage.Text)
						mWOInvoice.WOInvoiceJobCharges(j).CChargeAmount = Val(txtChargeAmount.Text)
					End If
				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End If
		Next
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateCharges", "CheckDuplicateCharges();", True)


		Session("mWOInvoice") = mWOInvoice
		mWOInvoice.ApplyEdit()
		mWOInvoice.CalculateTotal(ConsiderJobChargesForCalculation:=True)
		dgWOInvoiceJobOtherCharges.DataSource = mWOInvoice.WOInvoiceJobCharges
		dgWOInvoiceJobOtherCharges.DataBind()
		SetOtherChargeobject()
		SetSpareAndJobTotalCost()
		txtWOInvoiceJobOtherChargesTotalAmount.Text = mWOInvoice.CTotalJobCharges
		upnlWOInvoiceJobOtherChargesAmount.Update()
	End Sub
	Private Sub dgWOInvoiceJobOtherCharges_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOInvoiceJobOtherCharges.RowCommand

		Select Case e.CommandName
			Case "DeleteCharge"
				Dim Index As Integer = CInt(e.CommandArgument)
				mWOInvoice.WOInvoiceJobCharges.CurrentIndex = Index
				Session("mWOInvoice") = mWOInvoice
				MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteJobCharge")


		End Select

	End Sub
#End Region




#End Region

#Region "Spare Other Charges"
	Private Function CustomValidateSpareOtherCharges() As Boolean
		Dim strError As String = String.Empty
		Dim builder = New StringBuilder()


		Dim txtCharge As TextBox
		Dim rfvCharge As RequiredFieldValidator
		Dim upnlChargeValidate As UpdatePanel
		Dim cvValidator As CustomValidator

		For j As Integer = 0 To dgWOInvoiceSpareOtherCharges.Rows.Count - 1
			rfvCharge = CType(Me.dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("rfvSpareCharge"), RequiredFieldValidator)
			upnlChargeValidate = CType(Me.dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("upnlSpareChargeValidate"), UpdatePanel)
			txtCharge = CType(Me.dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtSpareCharge"), TextBox)
			cvValidator = CType(Me.dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("cvSpareCharge"), CustomValidator)

			If txtCharge.Text = "" Then
				rfvCharge.IsValid = False
				rfvCharge.Text = "* Charge Required"
				strError = "* Charge Required"
				upnlChargeValidate.Update()
			Else
				mFetchChargeByName = ChargeList.GetChargeList(Trim(txtCharge.Text))
				If mFetchChargeByName.Count <= 0 Then
					rfvCharge.IsValid = False
					rfvCharge.Text = "* Select proper Charge"
					strError = "* Select proper Charge"
					upnlChargeValidate.Update()
				End If

				'Else
				'    cvValidator.Visible = False
			End If

		Next
		If strError <> "" Then
			Return False
		End If
		Return True
	End Function

#Region "Spare Other Charges Events"
	Private Sub ImgAddWOInvoiceSpareOtherCharges_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgAddWOInvoiceSpareOtherCharges.Click
		If CustomValidateSpareOtherCharges() = False Then upnlValidationsummary.Update() : Exit Sub
		setObject()
		mWOInvoice.WOInvoiceSparesCharges.Add(mWOInvoice.ID)
		dgWOInvoiceSpareOtherCharges.DataSource = mWOInvoice.WOInvoiceSparesCharges
		SetSpareChargeGrid()
		dgWOInvoiceSpareOtherCharges.DataBind()
	End Sub
	Private Sub dgWOInvoiceSparesOtherCharges_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOInvoiceSpareOtherCharges.PageIndexChanging
		dgWOInvoiceSpareOtherCharges.PageIndex = e.NewPageIndex
		dgWOInvoiceSpareOtherCharges.DataSource = mWOInvoice.WOInvoiceSparesCharges
		Session("mWOInvoice") = mWOInvoice
		dgWOInvoiceSpareOtherCharges.DataBind()
	End Sub
	Protected Sub txtSpareCharge_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		' Dim txtQty As TextBox = TryCast(currentRow.FindControl("txtQty"), TextBox)

		Dim j As Integer = currentRow.DataItemIndex

		For i As Integer = 0 To mWOInvoice.WOInvoiceSparesCharges.Count - 1
			If i = j Then
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtSpareCharge")


					mFetchChargeByName = ChargeList.GetChargeList(Trim(txt.Text))

					If mFetchChargeByName.Count > 0 Then

						Dim ID As Guid = mFetchChargeByName(0).ID
						Dim txtPercentage, txtChargeAmount As TextBox
						txtPercentage = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtPercentage")
						txtChargeAmount = dgWOInvoiceSpareOtherCharges.Rows(j).FindControl("txtSpareChargeAmount")

						If Not mWOInvoice.WOInvoiceSparesCharges(j).ChargeName = Trim(txt.Text) Then
							txtChargeAmount.Text = "0"
							txtPercentage.Text = "0"
						End If

						mWOInvoice.WOInvoiceSparesCharges(j).ChargeName = Trim(txt.Text)

						mWOInvoice.WOInvoiceSparesCharges(j).ChargeID = mFetchChargeByName(0).ID
						mWOInvoice.WOInvoiceSparesCharges(j).ChargeName = mFetchChargeByName(ID).Name




						txtPercentage.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 3)
						txtChargeAmount.ReadOnly = Not (mFetchChargeByName(ID).PercentageTypeID = 1)

						txtPercentage.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, 0, mFetchChargeByName(ID).Percentage)
						' txtChargeAmount.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, txtChargeAmount.Text, 0)

						txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
						txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
						'txtChargeAmount.Text = IIf(mFetchChargeByName(ID).PercentageTypeID = 1, 0, txtChargeAmount.Text)
						mWOInvoice.WOInvoiceSparesCharges(j).Percentage = Val(txtPercentage.Text)
						mWOInvoice.WOInvoiceSparesCharges(j).CChargeAmount = Val(txtChargeAmount.Text)
					End If
				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End If
		Next
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateSpareCharges", "CheckDuplicateSpareCharges();", True)


		Session("mWOInvoice") = mWOInvoice
		mWOInvoice.ApplyEdit()
		mWOInvoice.CalculateTotal()
		dgWOInvoiceSpareOtherCharges.DataSource = mWOInvoice.WOInvoiceSparesCharges
		dgWOInvoiceSpareOtherCharges.DataBind()
		SetSpareOtherChargeobject()
		txtWOInvoiceSparesOtherChargesTotalAmount.Text = mWOInvoice.CTotalSpareCharges
		upnlWOInvoiceSparesOtherChargesAmount.Update()
		SetSpareAndJobTotalCost()
	End Sub
	Private Sub dgWOInvoiceSpareOtherCharges_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOInvoiceSpareOtherCharges.RowCommand

		Select Case e.CommandName
			Case "DeleteCharge"
				Dim Index As Integer = CInt(e.CommandArgument)
				mWOInvoice.WOInvoiceSparesCharges.CurrentIndex = Index
				Session("mWOInvoice") = mWOInvoice
				MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteSpareCharge")


		End Select

	End Sub
#End Region
#End Region

#Region "Terms"

#Region "Terms Helper Method"

	'    Private Sub TermsDataFieldBind()

	'        mTerms = Terms.GetTerms(mWOInvoice.ID, 1)
	'        Session("mTerms") = mTerms
	'        setTerms()
	'        dgTerm.DataSource = mTerms
	'        If AppSettings("ClientCode") = "Deccan" Then
	'            dgTerm.AllowPaging = False
	'        End If
	'        dgTerm.DataBind()
	'        upnlTerm.Update()
	'    End Sub
	'    Private Sub setTerms()
	'        Dim i As Integer
	'        While i < mTerms.Count
	'            If mWOInvoice.WOInvoiceTerms.Contains(mTerms.Item(i).ID) = True Then
	'                mTerms.Item(i).IsSelected = True
	'            Else
	'                mTerms.Item(i).IsSelected = False
	'            End If
	'            i = i + 1
	'        End While
	'    End Sub
	'    Private Sub SetSelectedTerms()
	'        Dim chkBox As CheckBox
	'        Dim Recordno, PageItems As Integer
	'        Dim i As Integer
	'        PageItems = dgTerm.Rows.Count - 1
	'        For i = 0 To PageItems
	'            Recordno = i + dgTerm.PageSize * dgTerm.PageIndex
	'            chkBox = CType(dgTerm.Rows(i).FindControl("chkSelect"), CheckBox)
	'            mTerms(Recordno).IsSelected = chkBox.Checked
	'        Next
	'        Session("mTerms") = mTerms
	'    End Sub
	'    Private Sub setTermObject()
	'        Dim i As Integer = 0
	'        While i < mTerms.Count
	'            If mTerms.Item(i).IsDirty = True Then
	'                If mTerms.Item(i).IsSelected = True Then
	'                    If mWOInvoice.WOInvoiceTerms.Contains(mTerms.Item(i).ID) = False Then
	'                        mWOInvoice.WOInvoiceTerms.Add(mWOInvoice.ID)

	'                        mWOInvoice.WOInvoiceTerms.CurrentItem.WOTerm = mTerms.Item(i).Terms
	'                        mWOInvoice.WOInvoiceTerms.CurrentItem.WOTermID = mTerms.Item(i).ID

	'                    End If
	'                Else
	'                    mWOInvoice.WOInvoiceTerms.Remove(mTerms.Item(i).ID, "")
	'                End If
	'            End If
	'            i = i + 1
	'        End While
	'    End Sub


	Private Function CustomValidateTerm() As Boolean
		Dim strError As String = String.Empty
		Dim builder = New StringBuilder()


		Dim txtTerm As TextBox
		Dim rfvTerm As RequiredFieldValidator
		Dim upnlTermValidate As UpdatePanel
		Dim cvValidator As CustomValidator

		For j As Integer = 0 To dgWOInvoiceTerms.Rows.Count - 1
			rfvTerm = CType(Me.dgWOInvoiceTerms.Rows(j).FindControl("rfvTerm"), RequiredFieldValidator)
			upnlTermValidate = CType(Me.dgWOInvoiceTerms.Rows(j).FindControl("upnlTermValidate"), UpdatePanel)
			txtTerm = CType(Me.dgWOInvoiceTerms.Rows(j).FindControl("txtTerm"), TextBox)
			cvValidator = CType(Me.dgWOInvoiceTerms.Rows(j).FindControl("cvTerm"), CustomValidator)

			If txtTerm.Text = "" Then
				rfvTerm.IsValid = False
				rfvTerm.Text = "* Term Required"
				strError = "* Term Required"
				upnlTermValidate.Update()
			Else
				mFetchTermByName = TermList.GetTermList(Trim(txtTerm.Text))
				If mFetchTermByName.Count <= 0 Then
					rfvTerm.IsValid = False
					rfvTerm.Text = "* Select proper Term"
					strError = "* Select proper Term"
					upnlTermValidate.Update()
				End If

				'Else
				'    cvValidator.Visible = False
			End If

		Next
		If strError <> "" Then
			Return False
		End If
		Return True
	End Function
#End Region

#Region "Terms Events"
	Private Sub ImgWOInvoiceTerms_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgWOInvoiceTerms.Click
		If CustomValidateTerm() = False Then upnlValidationsummary.Update() : Exit Sub
		setObject()
		mWOInvoice.WOInvoiceTerms.Add(mWOInvoice.ID)
		dgWOInvoiceTerms.DataSource = mWOInvoice.WOInvoiceTerms
		dgWOInvoiceTerms.DataBind()
	End Sub
	Private Sub dgTerm_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOInvoiceTerms.PageIndexChanging
		dgWOInvoiceTerms.PageIndex = e.NewPageIndex
		dgWOInvoiceTerms.DataSource = mWOInvoice.WOInvoiceTerms
		Session("mWOInvoice") = mWOInvoice
		dgWOInvoiceTerms.DataBind()
	End Sub
	Protected Sub txtTerm_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		' Dim txtQty As TextBox = TryCast(currentRow.FindControl("txtQty"), TextBox)

		Dim j As Integer = currentRow.DataItemIndex

		For i As Integer = 0 To mWOInvoice.WOInvoiceTerms.Count - 1
			If i = j Then
				Try
					Dim txt, txtValue As TextBox
					txt = dgWOInvoiceTerms.Rows(j).FindControl("txtTerm")

					mWOInvoice.WOInvoiceTerms(j).WOTerm = Trim(txt.Text)

					mFetchTermByName = TermList.GetTermList(Trim(txt.Text))
					mWOInvoice.WOInvoiceTerms(j).WOTermID = mFetchTermByName(0).ID

				Catch ex As Exception
					Dim a As Integer = 0
				End Try
			End If
		Next
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateTerms", "CheckDuplicateTerms();", True)


		dgWOInvoiceTerms.DataSource = mWOInvoice.WOInvoiceTerms
		dgWOInvoiceTerms.DataBind()
		Session("mWOInvoice") = mWOInvoice

	End Sub
	'    Private Sub btnOKTerms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOKTerms.Click
	'        SetSelectedTerms()
	'        setTermObject()
	'        Session("mWOInvoice") = mWOInvoice


	'        dgWOInvoiceTerms.DataSource = mWOInvoice.WOInvoiceTerms
	'        dgWOInvoiceTerms.DataBind()

	'        upnlWOInvoiceTerms.Update()
	'        Dim InvoiceDetail = mWOInvoice.InvoiceText + " Dated : " + mWOInvoice.WOInvoiceDateFormatted + " from " + mWOInvoice.CustomerName
	'        MarkLog(Util.Action.Save, "WOInvoice", User.Identity.Name + " added Terms for Invoice : " + InvoiceDetail, Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)

	'        mdlTerms.Hide()
	'    End Sub

	Private Sub dgWOInvoiceTerms_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOInvoiceTerms.RowCommand

		Select Case e.CommandName
			Case "DeleteTerm"
				Dim Index As Integer = CInt(e.CommandArgument)
				mWOInvoice.WOInvoiceTerms.CurrentIndex = Index
				Session("mWOInvoice") = mWOInvoice
				MSGBoxCtrl.show(MSGBox.Message_title.RemoveTerm, MSGBox.Message_text.RemoveTerm, "", MsgBoxStyle.YesNo, "DeleteInvoiceTerms")


		End Select

	End Sub
	'    Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
	'        TermsDataFieldBind()
	'        Session("mTerms") = mTerms
	'    End Sub
	'    Private Sub imgbtnTerm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnTerm.Click
	'        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTermWindow", "OpenTermWindow();", True)

	'    End Sub
#End Region



#End Region



#Region "Service Methods"

	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

		'Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
		'Dim str As String() = contextKey.Split("¿")
		'Dim mTransTypeID As Integer = CInt(str(0).Substring(str(0).IndexOf("=") + 1))

		'Dim mDate As String = str(1).Substring(str(1).IndexOf("=") + 1)
		'mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, , True, mTransTypeID, mDate)

		'If count = 0 Then
		'    Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
		'       Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		'Else
		'    Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
		'       Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		'End If
		Dim DistinctTextList As DistinctTextListForWOInvoice
		DistinctTextList = DistinctTextListForWOInvoice.GetDistinctTextList(prefixText:=prefixText)

		If count = 0 Then
			Return (From c As DistinctTextListForWOInvoice.TextInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		Else
			Return (From c As DistinctTextListForWOInvoice.TextInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		End If
	End Function
	<System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
	Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
		Dim itemlist As ItemListAutoComplete
		itemlist = ItemListAutoComplete.GetItemList(prefixText, , , 0)
		If contextKey = "Tools" Then
			Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
					Where c.PrimaryCategoryID = 2
					Select c.Item).Take(count).ToList
		Else
			Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
					Where c.PrimaryCategoryID <> 2
					Select c.Item).Take(count).ToList
		End If
	End Function
	<System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
	Public Shared Function GetTermList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
		Dim mTermList As TermList
		mTermList = TermList.GetTermList(prefixText)

		Return (From c As TermList.TermInfo In mTermList
				Select c.Terms).Take(count).ToList

	End Function
	<System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
	Public Shared Function GetChargeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
		Dim mChargeList As ChargeList
		mChargeList = ChargeList.GetChargeList(Trim(prefixText), -1)

		Return (From c As ChargeList.ChargeInfo In mChargeList
				Select c.Name).Take(count).ToList

	End Function
#End Region




End Class