'***********************************
'Modified by Harsh Sugandhi on 22nd April 2025 for FLYPAL 2334 => Facility to attach a file to Service Module. 
'***********************************

Public Class wfLineMaintenanceOrder_Ajax
	Inherits Page

#Region " Enum "

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

#Region " Variable Declaration "

	Public mLineMaintenanceOrder As LineMaintenanceOrder
	Public mMachineNameValueList As MachineNameValueList
	Public mVendorList As VendorList
	Public mCurrencyList As CurrencyList
	Public Flag As Integer
	Public mLocationList As LocationList
	Public mFileAttach As FileAttach
	Public mPrevTransID As Guid = Guid.Empty

	Dim EventLogID As Guid
	Dim IsAttachmentDeleted As Boolean = False

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mLineMaintenanceOrder = Session("mLineMaintenanceOrder")
		mMachineNameValueList = Session("mMachineNameValueList")
		mVendorList = Session("mVendorList")
		mCurrencyList = Session("mCurrencyList")
		mLocationList = Session("mLocationList")
		mFileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
	End Sub

	Private Sub SetSession()
		Session("mMachineNameValueList") = mMachineNameValueList
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
		Session("mVendorList") = mVendorList
		Session("mCurrencyList") = mCurrencyList
		Session("mLocationList") = mLocationList
		Session("mFileAttach") = mFileAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mMachineNameValueList")
		Session.Remove("mVendorList")
		Session.Remove("mCurrencyList")
		Session.Remove("mLocationList")
		Session.Remove("Address")
		Session.Remove("Attention")
		Session.Remove("mFileAttach")
		Session.Remove("IsAttachmentDeleted")
	End Sub

	Private Sub SetObject()

		Try

			If calOrderDate.Text = "" Then
				mLineMaintenanceOrder.OrderDate = Today.Date
			Else
				mLineMaintenanceOrder.OrderDate = CDate(calOrderDate.Text)
			End If

			mLineMaintenanceOrder.UserName = User.Identity.Name
			mLineMaintenanceOrder.Text = txtText.Text
			mLineMaintenanceOrder.No = Val(txtNo.Text)
			mLineMaintenanceOrder.OpeningLine = Trim(txtOpeningLine.Text)
			mLineMaintenanceOrder.IsRoundOff = chkIsRoundOff.Checked
			mLineMaintenanceOrder.LocationID = New Guid(cmbLocation.SelectedValue)
			mLineMaintenanceOrder.BillingAddress = Trim(txtBillingAddress.Text)
			mLineMaintenanceOrder.Attention = Trim(txtAttention.Text)
			mLineMaintenanceOrder.MachineID = New Guid(cmbMachine.SelectedValue)
			mLineMaintenanceOrder.IsMSP = chkMaintenanceSupportPlan.Checked

			If mFileAttach IsNot Nothing Then

				If mFileAttach.Size > 0 Then
					mLineMaintenanceOrder.IsAttachmentAdded = True
				Else
					mLineMaintenanceOrder.IsAttachmentAdded = False
				End If

			End If

			mLineMaintenanceOrder.CalculateTotal()

			Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetVendorDetails()

		mLineMaintenanceOrder.VendorID = New Guid(cmbVendorList.SelectedValue)
		mLineMaintenanceOrder.QuotationNo = Trim(txtQuotationNo.Text)

		If txtQuotationDate.Text <> "" Then
			mLineMaintenanceOrder.QuotationDate = CDate(txtQuotationDate.Text)
		Else
			mLineMaintenanceOrder.QuotationDate = DBNull.Value
		End If

		mLineMaintenanceOrder.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
		mLineMaintenanceOrder.ConversionFactor = Val(txtConversionFactor.Text)
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

	End Sub

	Private Sub DeleteRecord(Index As Int32)

		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
						MSGBox.Message_text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"Delete")

		mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentIndex = Index
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

	End Sub

	Private Sub DeleteCharge(index As Int32)

		MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge,
						MSGBox.Message_text.RemoveCharge,
						"",
						MsgBoxStyle.YesNo,
						"DeleteCharge")

		mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentIndex = index
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

	End Sub

	Private Overloads Sub SetFocus(webControl As WebControl)
		If webControl.Enabled = False Or webControl.Visible = False Then Exit Sub
		webControl.Focus()
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
							mLineMaintenanceOrder = CType(Session("mLineMaintenanceOrder"), LineMaintenanceOrder)
							mLineMaintenanceOrder.LineMaintenanceOrderItems.Remove(mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem)
							dgOrderItems.DataSource = mLineMaintenanceOrder.LineMaintenanceOrderItems
							dgOrderItems.DataBind()
							upnlLineMaintenanceOrderItems.Update()
							mLineMaintenanceOrder.CalculateTotal()
							If mLineMaintenanceOrder.IsRoundOff = True Then
								SetChargeGrid()
								mLineMaintenanceOrder.RoundCGrandTotal()
							End If
							upnlGrandTotal.Update()
							upnlTotal.Update()
							Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

						Catch ex As SqlException

							MSGBoxCtrl.show(MSGBox.Message_title.Alert,
											MSGBox.Message_text.Alert,
											ex.Message,
											MsgBoxStyle.OkOnly,
											"")

							Exit Sub

						End Try

					End If

					If MSGBoxCtrl.Sender = "Close" Then

						Session("sender") = ""

						If mLineMaintenanceOrder.IsValid = True Then

							Session.Remove("IsValid")
							DataFieldBind()

							If (Not IsInRole(Rights.New) And Not IsInRole(Rights.Edit)) Then
								ClientScript.RegisterStartupScript([GetType], "OpenScript", MessageBox.Show("You are not authorized user"))
								Exit Sub
							End If

							If Save() Then
								RemoveSession()
								Response.Redirect("Index.aspx")
							End If

						Else

							If CustomValidate1() = False Then
								upnlValidationsummary.Update()
								Exit Sub
							End If

						End If

					End If

					If MSGBoxCtrl.Sender = "DeleteCharge" Then

						Try

							Session("Sender") = ""
							mLineMaintenanceOrder = CType(Session("mLineMaintenanceOrder"), LineMaintenanceOrder)
							mLineMaintenanceOrder.LineMaintenanceOrderCharges.Remove(mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem)
							dgOrderCharges.DataSource = mLineMaintenanceOrder.LineMaintenanceOrderCharges
							dgOrderCharges.DataBind()
							upnlLineMaintenanceOrderCharges.Update()
							mLineMaintenanceOrder.CalculateTotal()

							If mLineMaintenanceOrder.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012
								SetChargeGrid()
								mLineMaintenanceOrder.RoundCGrandTotal()
							End If

							upnlGrandTotal.Update()
							upnlTotal.Update()
							Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

						Catch ex As SqlException

							MSGBoxCtrl.show(MSGBox.Message_title.Alert,
											MSGBox.Message_text.Alert,
											ex.Message,
											MsgBoxStyle.OkOnly,
											"")

							Exit Sub

						End Try

					End If

					If MSGBoxCtrl.Sender = "Status" Then

						Session("sender") = ""

						If mLineMaintenanceOrder.IsValid = True Then
							mLineMaintenanceOrder.StatusID = 2
							DataFieldBind()
							Save()
						End If

					End If

					If MSGBoxCtrl.Sender = "StatusCancel" Then

						Session("sender") = ""
						mLineMaintenanceOrder.StatusID = 4
						DataFieldBind()
						Save()

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
						Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
						DataFieldBind()
						upnlLineMaintenanceOrderItems.Update()
						upnlLineMaintenanceOrderCharges.Update()
						upnlOrderTerms.Update()

					End If

			End Select

		End If

	End Sub

	Private Sub AddAttributes()
		txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub SetControlStatus(StatusId As Int16)

		btnAddOrderItems.Enabled = IIf(StatusId > 1, False, True)
		btnAddTerms.Enabled = IIf(StatusId > 1, False, True)
		btnAddCharges.Enabled = IIf(StatusId > 1, False, True)
		dgOrderTerms.Columns(2).Visible = IIf(StatusId > 1, False, True)
		btnSave.Visible = IIf(StatusId > 1, False, True)
		dgOrderCharges.Columns(4).Visible = IIf(StatusId > 1, False, True)
		dgOrderCharges.Columns(5).Visible = IIf(StatusId > 1, False, True)

	End Sub

	Private Sub ControlVisibility(StatusID As Integer)

		txtText.Enabled = CType(IIf(mLineMaintenanceOrder.StatusID >= 2, False, True), Boolean)
		txtNo.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		txtQuotationNo.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		cmbCurrencyList.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		calOrderDate.Enabled = (mLineMaintenanceOrder.IsNew) Or mLineMaintenanceOrder.LineMaintenanceOrderItems.Count = 0
		txtConversionFactor.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		txtQuotationDate.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		txtOpeningLine.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		txtAttention.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		cmbLocation.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		calOrderDate.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		cmbMachine.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		cmbVendorList.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)
		txtBillingAddress.Enabled = CType(IIf(StatusID >= 2, False, True), Boolean)

		chkIsRoundOff.Enabled = (StatusID = 1)
		'Authorized Status
		btnAuthorized.Visible = (Not mLineMaintenanceOrder.LineMaintenanceOrderItems.Count = 0) And (Not mLineMaintenanceOrder.IsNew) And (StatusID = 1)

		If (Not mLineMaintenanceOrder.TransTypeID = 31 And Not mLineMaintenanceOrder.TransTypeID = 38) Then 'Added by Saylee on 23-Oct-2012
			'Canceled Status
			btnCancel.Visible = (Not mLineMaintenanceOrder.IsNew) And (StatusID = 2) And (Not mLineMaintenanceOrder.TransTypeID = 31 Or Not mLineMaintenanceOrder.TransTypeID = 38)
		Else
			btnCancel.Visible = False
		End If

		If Not IsInRole(Rights.Authorized) Then
			btnAuthorized.Enabled = False
			btnAuthorized.ToolTip = "You are not authorized user "
			btnCancel.Enabled = False
			btnCancel.ToolTip = "You are not authorized user "
		End If

		ControlAttachmentIconVisibility(StatusID)

	End Sub

	Private Function Save() As Boolean

		Try

			'Authentication
			If mLineMaintenanceOrder.OrderDate IsNot DBNull.Value Then

				Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

				If mCheck.WebAuthentication = True Then

					Dim mDays As Integer = 0
					mDays = mCheck.Number("Days")
					Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)

					If DateDiff(DateInterval.Day, CDate(mLineMaintenanceOrder.OrderDate), maxAllowableDate) < 0 Then

						MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
										MSGBox.Message_text.saveAlert,
										" Your subscription has been expired. can not save LineMaintenanceOrder. 
                                        <br> LineMaintenanceOrder Date can not be greater than " &
										maxAllowableDate.ToString(WebDateFormat),
										MsgBoxStyle.OkOnly,
										"")

						Exit Function

					End If

				End If

			End If

			'Authentication
			Dim OrderClone As LineMaintenanceOrder
			OrderClone = mLineMaintenanceOrder.Clone

			Try

				If Not mLineMaintenanceOrder.LineMaintenanceOrderItems.Count = 0 Then

					SetObject()
					SetVendorDetails()

					If mVendorList(mLineMaintenanceOrder.VendorID).NotInUse = True Then

						If CDate(mVendorList(mLineMaintenanceOrder.VendorID).NotInUseDate) <= CDate(mLineMaintenanceOrder.OrderDate) Then

							MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
											MSGBox.Message_text.saveAlert,
											"Supplier is not applicable since " + mVendorList(mLineMaintenanceOrder.VendorID).NotInUseDateFormatted +
											" <br><br> Select another Supplier from list or select date before " +
											mVendorList(mLineMaintenanceOrder.VendorID).NotInUseDateFormatted + " & try again",
											MsgBoxStyle.OkOnly,
											"")

							Exit Function

						End If

					End If

					Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
					Dim LineMaintenanceOrderCharge As LineMaintenanceOrderCharge

					For Each LineMaintenanceOrderCharge In mLineMaintenanceOrder.LineMaintenanceOrderCharges

						If (LineMaintenanceOrderCharge.Sign <> 1 And LineMaintenanceOrderCharge.CChargeAmount <= 0) Or (Not (LineMaintenanceOrderCharge.IsValid)) Then

							MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert,
											MSGBox.Message_text.ValidationAlert,
											"Percentage LineMaintenanceOrder Charge(s) are not allowed if LineMaintenanceOrder Amount Is Zero. ",
											MsgBoxStyle.OkOnly,
											"")

							mLineMaintenanceOrder.CancelEdit()

							Exit Function

						End If

					Next

					If mLineMaintenanceOrder.IsRoundOff = True Then
						mLineMaintenanceOrder.RoundCGrandTotal()
					End If

					If (mLineMaintenanceOrder.IsNew) And (mLineMaintenanceOrder.Text = "") Then

						Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mLineMaintenanceOrder.TransTypeID,
																													 mLineMaintenanceOrder.OrderDateFormatted)

						If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mLineMaintenanceOrder.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mLineMaintenanceOrder.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mLineMaintenanceOrder.TransTypeID).TransText = "")) Then

							Dim str = "<script language='javascript'>openledgersame('wfLineMaintenanceOrder_Ajax.aspx?BackPage=" &
								Request.QueryString("BackPage") & "');</script>"

							Session("BackPagestr_ForTransSeries") = str
							Session("TransName_ForTransSeries") = "LineMaintOrder"
							Session("TransTypeID_ForTransSeries") = mLineMaintenanceOrder.TransTypeID
							Session("TransDate_ForTransSeries") = mLineMaintenanceOrder.OrderDateFormatted
							Session("sender") = "LineMaintOrderTransTextSeriesAlert"
							Session("AddTransTextSeries") = "True"

							Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

						Else

							Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

							If mAutoRenewTransTextSeries.IsRenewed Then

								With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mLineMaintenanceOrder.TransTypeID)
									mLineMaintenanceOrder.Text = .TransText
									mLineMaintenanceOrder.No = .StartingTransNo
								End With

							Else

								Dim str = "<script language='javascript'>openledgersame('wfLineMaintenanceOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

								Session("BackPagestr_ForTransSeries") = str
								Session("TransName_ForTransSeries") = "LineMaintOrder"
								Session("TransTypeID_ForTransSeries") = mLineMaintenanceOrder.TransTypeID
								Session("TransDate_ForTransSeries") = mLineMaintenanceOrder.OrderDateFormatted
								Session("AddTransTextSeries") = "True"

								Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

							End If

						End If

					End If
					'End

					mLineMaintenanceOrder.Save()
					SaveAttachment()

					Dim OrderDetail As String = mLineMaintenanceOrder.OrderNo +
												" Dated : " + mLineMaintenanceOrder.OrderDateFormatted +
												" To " + mVendorList(mLineMaintenanceOrder.VendorID).Name &
												" Created By : " & mLineMaintenanceOrder.UserName 'Added by Saylee on 19-July-2011 

					If mLineMaintenanceOrder.StatusID = 2 Then

						MarkLog(Action.Authorize,
								"LineMaintenanceOrder",
								OrderDetail & " Authorized By : " & mLineMaintenanceOrder.AuthorizedBy,
								ErrorType.NoError,
								mLineMaintenanceOrder.ID,
								EventLogID)

					ElseIf mLineMaintenanceOrder.StatusID = 3 Then

						MarkLog(Action.Amend, "LineMaintenanceOrder",
								OrderDetail,
								ErrorType.NoError,
								mLineMaintenanceOrder.ID,
								EventLogID)

					ElseIf mLineMaintenanceOrder.StatusID = 4 Then

						MarkLog(Action.Cancel,
								"LineMaintenanceOrder",
								OrderDetail,
								ErrorType.NoError,
								mLineMaintenanceOrder.ID,
								EventLogID)

					Else

						MarkLog(Action.Save,
								"LineMaintenanceOrder",
								OrderDetail,
								ErrorType.NoError,
								mLineMaintenanceOrder.ID,
								EventLogID)

					End If

					mLineMaintenanceOrder.MarkClean()
					lblTitle.Text = "Service Order ( Saved ...)"

					If mLineMaintenanceOrder.StatusID = 3 Then

						Dim mAmendOrder As LineMaintenanceOrder
						mAmendOrder = LineMaintenanceOrder.GetAmendedOrder(mLineMaintenanceOrder)
						mLineMaintenanceOrder = mAmendOrder
						mLineMaintenanceOrder = CType(mLineMaintenanceOrder.Save(), LineMaintenanceOrder)

					End If

					DataFieldBind()
					SetPage()
					SetChargeGrid()
					ControlVisibility(mLineMaintenanceOrder.StatusID)
					SetControlStatus(mLineMaintenanceOrder.StatusID)

					upnlStatusName.Update()
					upnlOrderDetails.Update()
					upnlSupplierDetails.Update()
					upnlLineMaintenanceOrderItems.Update()
					upnlLineMaintenanceOrderCharges.Update()
					upnlOrderTerms.Update()
					upnlGrandTotal.Update()
					upnlTotal.Update()
					upnlButtons.Update()
					upnlFileAttachmentButtons.Update()

					MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
									MSGBox.Message_text.SavedSuccessFully,
									"",
									MsgBoxStyle.OkOnly,
									"")

					Return True

				Else

					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
									MSGBox.Message_text.saveAlert,
									"Service Order can not be saved without Item.",
									MsgBoxStyle.OkOnly,
									"")
					Exit Function

				End If

			Catch ex As SqlException

				If ex.Number = 8114 Or ex.Number = 8115 Then

					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow,
									MSGBox.Message_text.NumericOverFlow,
									" Rate or Qty or Conversion Factor. ",
									MsgBoxStyle.OkOnly,
									"")

					Exit Function

				ElseIf ex.Number = 8145 Then

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
									MSGBox.Message_text.ProcedureError,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")

					Exit Function

				ElseIf ex.Number = 2627 Then

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
									MSGBox.Message_text.Duplicate,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")

					Exit Function

				ElseIf ex.Number = 547 Then

					MSGBoxCtrl.Show("Save",
									"LineMaintenanceOrder Can Not Be Saved !",
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Function

				End If

				mLineMaintenanceOrder = OrderClone
				Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

			Catch ex As Exception

				MSGBoxCtrl.show(MSGBox.Message_title.ErrorMessage,
								MSGBox.Message_text.ErrorMessage,
								ex.GetBaseException.ToString(),
								MsgBoxStyle.OkOnly,
								"")

				Exit Function

			Finally
				OrderClone = Nothing
			End Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function IsInRole(CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		'Deciding IsInRole String to check Rights
		Select Case mLineMaintenanceOrder.TransTypeID
			Case Trans.LineMaintenanceOrder
				IsInRoleString = "LineMaintenanceOrder"
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

	Private Sub SetChargeGrid()
		For j As Integer = 0 To dgOrderCharges.Rows.Count - 1
			If (Me.dgOrderCharges.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or Me.dgOrderCharges.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then
				dgOrderCharges.Rows.Item(j).Cells(4).Enabled = False
				dgOrderCharges.Rows.Item(j).Cells(5).Enabled = False
			End If
		Next
	End Sub

	Private Sub ControlAttachmentIconVisibility(StatusID As Integer)

		Try

			btnSelectFile.Disabled = IIf(StatusID = 2, True, False)
			btnRemoveAttach.Enabled = IIf(mLineMaintenanceOrder.IsAttachmentAdded AndAlso StatusID <> 2, True, False)
			AttachmentIcon.Visible = IIf(mLineMaintenanceOrder.IsAttachmentAdded, True, False)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveAttachment()

		Try

			If mFileAttach IsNot Nothing Then

				If mFileAttach.Size > 0 Then

					Try

						mFileAttach.Save()

					Catch ex As Exception

						ScriptManager.RegisterClientScriptBlock(Me,
																[GetType],
																"",
																MessageBox.Show(ex.InnerException.ToString, False),
																True)
					End Try

				Else

					If (Not mLineMaintenanceOrder.IsNew) And IsAttachmentDeleted Then

						FileAttach.DeleteAttachment(ID:=mFileAttach.ID,
													ReferenceID:=mLineMaintenanceOrder.ID)

					End If

					IsAttachmentDeleted = False
					Session("IsAttachmentDeleted") = IsAttachmentDeleted

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", ForInventory:=True, SkipIsForInventoryAircarft:=True)
		cmbMachine.DataSource = mMachineNameValueList
		Session("mMachineNameValueList") = mMachineNameValueList

		mLocationList = LocationList.GetLocationsList(0, , , , , , True, SelectTag:="(SELECT)")
		Session("mLocationList") = mLocationList
		cmbLocation.DataSource = mLocationList

		mCurrencyList = CurrencyList.GetCurrencyList(, , True)
		cmbCurrencyList.DataSource = mCurrencyList
		Session("mCurrencyList") = mCurrencyList

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
			mVendorList = VendorList.GetVendortList(0, , , , , , True, , True, True)
		Else
			mVendorList = VendorList.GetVendortList(0, , , , , , True, , True)
		End If

		cmbVendorList.DataSource = mVendorList
		Session("mVendorList") = mVendorList

		If mLineMaintenanceOrder.IsNew Then

			Dim mRecordOfLastOrder As RecordOfLastOrder = RecordOfLastOrder.GetRecordOfLastOrder(mLineMaintenanceOrder.TransTypeID)
			txtBillingAddress.Text = mRecordOfLastOrder(0).BillingAddress

			If cmbVendorList.SelectedIndex > 0 Then
				txtAttention.Text = mVendorList(cmbVendorList.SelectedIndex).ContactPerson
			Else
				txtAttention.Text = ""
			End If

		End If

		dgOrderItems.DataSource = mLineMaintenanceOrder.LineMaintenanceOrderItems

		dgOrderTerms.DataSource = mLineMaintenanceOrder.LineMaintenanceOrderTerms

		calOrderDate.Text = mLineMaintenanceOrder.OrderDateFormatted.ToString

		txtQuotationDate.Text = mLineMaintenanceOrder.QuotationDateFormatted.ToString

		dgOrderCharges.DataSource = mLineMaintenanceOrder.LineMaintenanceOrderCharges

		DataBind()

	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "calOrderDate" Then
			If calOrderDate.Text = "" Then
				custValidator.ErrorMessage = "Select LineMaintenanceOrder Date."
				e.IsValid = False
			End If
		End If
		If custValidator.ControlToValidate = "txtConversionFactor" Then
			If Val(txtConversionFactor.Text) = 0 Then
				custValidator.ErrorMessage = "Conversion factor Required."
				e.IsValid = False
			ElseIf Not IsNumeric(Val(txtConversionFactor.Text)) And Val(txtConversionFactor.Text) <> 0 Then
				custValidator.ErrorMessage = "Conversion factor must be numeric."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
		If custValidator.ControlToValidate = "cmbCurrencyList" Then
			If cmbCurrencyList.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Please select Currency."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub

	Private Sub SetPage()
		If mLineMaintenanceOrder.No > 0 Then
			lblTitle.Text = "Service Order [ " + mLineMaintenanceOrder.OrderNo + " ]"
		Else
			lblTitle.Text = "Service Order [New]"
		End If
		upnlTitle.Update()
	End Sub

#End Region

#Region " Events "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		GetSession()
		AddAttributes()
		SetControlStatus(mLineMaintenanceOrder.StatusID)

		EventLogID = CType(Session("EventLogID"), Guid)

		If Not IsPostBack And Session("sender") = "" Then

			'Added by Utkarsh on 22-Nov-2013 for Trans Text Series
			If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Session("TransText_ForTransSeries") IsNot Nothing) Then

				If mLineMaintenanceOrder.IsNew Then

					mLineMaintenanceOrder.Text = Session("TransText_ForTransSeries")
					txtText.Text = mLineMaintenanceOrder.Text
					Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
					Session("AddTransTextSeries") = "False"
					Session.Remove("TransName_ForTransSeries")
					Session.Remove("TransText_ForTransSeries")
					Session.Remove("TransNo_ForTransSeries")

				End If

			End If
			'End

			DataFieldBind()

			If mLineMaintenanceOrder.StatusID = 1 And mLineMaintenanceOrder.IsNew = False Then
				lblStatus.Text = "OPENED"
			End If

		End If

		SetPage()
		ControlVisibility(mLineMaintenanceOrder.StatusID)

		If mLineMaintenanceOrder.IsRoundOff = True Then
			SetChargeGrid()
		End If

	End Sub

	Private Sub Add_Click(sender As Object, e As EventArgs) Handles btnAddOrderItems.Click

		If IsValid = False Then upnlValidationsummary.Update() : Exit Sub
		SetObject()
		SetVendorDetails()
		mLineMaintenanceOrder.LineMaintenanceOrderItems.Add(mLineMaintenanceOrder.ID)
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
		Response.Redirect("wfLineMaintenanceOrderItem_Ajax.aspx?BackPage=wfLineMaintenanceOrder_Ajax.aspx")

	End Sub

	Private Sub AddTerm_Click(sender As Object, e As EventArgs) Handles btnAddTerms.Click

		SetObject()
		SetVendorDetails()
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
		Response.Redirect("wfLineMaintenanceOrderTerm_Ajax.aspx?BackPage=wfLineMaintenanceOrder_Ajax.aspx&Type=7")

	End Sub

	Private Sub GV_OrderItems_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgOrderItems.RowCommand

		Select Case e.CommandName
			Case "EditView"

				Dim Index As Integer = CInt(e.CommandArgument) + dgOrderItems.PageIndex * dgOrderItems.PageSize
				Session("Edit") = True
				SetObject()
				SetVendorDetails()
				mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentIndex = Index
				Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

				If mLineMaintenanceOrder.VendorID.Equals(Guid.Empty) Then
					Session("VendorName") = ""
				Else
					Session("VendorName") = mVendorList.Item(cmbVendorList.SelectedIndex).Name
				End If

				Response.Redirect("wfLineMaintenanceOrderItem_Ajax.aspx?BackPage=wfLineMaintenanceOrder_Ajax.aspx")

			Case "DeleteRecord"
				Dim Index As Integer = CInt(e.CommandArgument) + dgOrderItems.PageIndex * dgOrderItems.PageSize
				DeleteRecord(Index)
		End Select

	End Sub

	Private Sub GV_OrderTerms_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgOrderTerms.RowCommand

		Select Case e.CommandName
			Case "DeleteTerm"

				Dim Index As Integer = CInt(e.CommandArgument) + dgOrderTerms.PageIndex * dgOrderTerms.PageSize
				mLineMaintenanceOrder.LineMaintenanceOrderTerms.CurrentIndex = Index
				mLineMaintenanceOrder.LineMaintenanceOrderTerms.Remove(mLineMaintenanceOrder.LineMaintenanceOrderTerms.CurrentItem)
				Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
				dgOrderTerms.DataSource = mLineMaintenanceOrder.LineMaintenanceOrderTerms
				dgOrderTerms.DataBind()
				upnlOrderTerms.Update()

		End Select

	End Sub

	Private Sub GV_OrderCharges_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgOrderCharges.RowCommand
		Select Case e.CommandName

			Case "EditCharge"

				Dim Index As Integer = CInt(e.CommandArgument) + dgOrderCharges.PageIndex * dgOrderCharges.PageSize
				Session("Edit") = True
				SetObject()
				SetVendorDetails()
				mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentIndex = Index
				Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
				Response.Redirect("wfLineMaintenanceOrderCharge_Ajax.aspx")

			Case "DeleteCharge"
				Dim Index As Integer = CInt(e.CommandArgument) + dgOrderCharges.PageIndex * dgOrderCharges.PageSize
				DeleteCharge(Index)
		End Select

	End Sub

	Private Sub VendorList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbVendorList.SelectedIndexChanged

		If cmbVendorList.Enabled = True Then
			SetFocus(cmbVendorList)
		End If

		If cmbVendorList.SelectedIndex > 0 Then
			txtAttention.Text = mVendorList(cmbVendorList.SelectedIndex).ContactPerson
		Else
			txtAttention.Text = ""
		End If

		If AppSettings("LastOrderCurrency") = "True" Then

			Dim mRecordOfLastOrder As RecordOfLastOrder = RecordOfLastOrder.GetRecordOfLastOrder(mLineMaintenanceOrder.TransTypeID, New Guid(cmbVendorList.SelectedValue).ToString)
			mLineMaintenanceOrder.CurrencyID = mRecordOfLastOrder(0).CurrencyID
			cmbCurrencyList.DataBind()
			txtConversionFactor.DataBind()
			mRecordOfLastOrder = Nothing

		End If

		upnlOrderDetails.Update()

	End Sub

	Private Sub cmbCurrencyList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
		txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
		If cmbCurrencyList.Enabled = True Then
			SetFocus(cmbCurrencyList)
		End If
	End Sub

	Private Sub SaveDetails(sender As Object, e As EventArgs) Handles btnSave.Click

		If (Not IsInRole(Rights.New) And Not IsInRole(Rights.Edit)) Then

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"OpenScript",
												MessageBox.Show("You are not authorized user", False),
												True)
			Exit Sub

		End If

		SetVendorDetails()
		SetObject()
		SetSession()

		If IsValid = True Then
			Save()
		Else
			upnlValidationsummary.Update()
		End If

	End Sub

	Private Sub Back_Click(sender As Object, e As EventArgs) Handles btnBack.Click

		MarkLog(Action.Close,
				"LineMaintenanceOrder",
				"",
				ErrorType.NoError,
				Guid.Empty,
				EventLogID)

		SetObject()
		SetVendorDetails()
		If mLineMaintenanceOrder.IsDirty Then

			MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm,
							MSGBox.Message_text.CloseConfirm,
							"",
							MsgBoxStyle.YesNo,
							"Close")
		Else

			If mLineMaintenanceOrder.IsNew Then
				Session.Remove("mLineMaintenanceOrder")
			End If
			RemoveSession()
			Response.Redirect("Index.aspx")

		End If

	End Sub

	Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		If Not IsInRole(Rights.Print) Then

			ClientScript.RegisterStartupScript([GetType],
											   "OpenScript",
											   MessageBox.Show("You are not authorized user"))

			Exit Sub

		End If

		Dim myReport As Engine.ReportClass
		Dim rpt As rptLineMaintenanceOrder
		Dim letter As rptLetterHead
		Dim da As New ObjectAdapter
		Dim dsLineMaintOrder As New dsLineMaintenanceOrder

		If (AppSettings("ClientCode") = "CGA") Then 'Added By Vikrant On 13-Nov-2014 For CGA13112014
			myReport = New crptLineMaintenanceOrderDetailsCGA
		ElseIf AppSettings("ClientCode") = "PTW" Then 'Added By Prashant on 22-Apr-2025
			myReport = New crptLineMaintenanceOrderDetailsForPattaya
		Else 'End
			myReport = New crptLineMaintenanceOrderDetails
		End If


		rpt = rptLineMaintenanceOrder.GetLineMaintenanceOrder(LineMaintOrderID:=mLineMaintenanceOrder.ID, ClientCode:=AppSettings("ClientCode"))

		Dim mEmployeeInfoFromUser As User
		mEmployeeInfoFromUser = SI.UTILITY.User.GetUser(rpt(0).UserName) 'Created By
		letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
												 "",
												 "",
												 AppSettings("Logo"),
												 SearchString3:="",
												 ClientCode:=AppSettings("ClientCode"),
												 SearchString4:="",
												 SearchString5:="",
												 SearchString6:="",
												 SearchString7:=mEmployeeInfoFromUser.EmployeeName,
												 SearchString8:=mEmployeeInfoFromUser.EmployeeEmail,
												 SearchString9:=mEmployeeInfoFromUser.EmployeePhoneNo)

		dsLineMaintOrder.Clear()

		Dim mrptImage As rptImage = rptImage.GetImage(dsLineMaintOrder)
		da.Fill(dsLineMaintOrder, mrptImage)

		da.Fill(dsLineMaintOrder, rpt)
		da.Fill(dsLineMaintOrder, letter)
		myReport.SetDataSource(dsLineMaintOrder)

		Session("CrystalReport") = myReport
		Dim Str1 As String
		Str1 = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me,
											[GetType],
											"openTranDetail",
											Str1,
											True)

	End Sub

	Private Sub AddCharge_Click(sender As Object, e As EventArgs) Handles btnAddCharges.Click

		SetObject()
		SetVendorDetails()
		mLineMaintenanceOrder.LineMaintenanceOrderCharges.Add(mLineMaintenanceOrder.ID)
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
		Response.Redirect("wfLineMaintenanceOrderCharge_Ajax.aspx?BackPage=wfLineMaintenanceOrder_Ajax.aspx")

	End Sub

	Private Sub IsRoundOff_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsRoundOff.CheckedChanged

		Dim Child As LineMaintenanceOrderCharge

		For i As Integer = mLineMaintenanceOrder.LineMaintenanceOrderCharges.Count - 1 To 0 Step -1

			Child = mLineMaintenanceOrder.LineMaintenanceOrderCharges(i)
			If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
				mLineMaintenanceOrder.LineMaintenanceOrderCharges.Remove(Child)
			End If

		Next
		dgOrderCharges.DataSource = mLineMaintenanceOrder.LineMaintenanceOrderCharges
		dgOrderCharges.DataBind()

	End Sub

	'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
	Private Sub OrderDate_TextChanged(sender As Object, e As EventArgs) Handles calOrderDate.TextChanged

		mLineMaintenanceOrder = Session("mLineMaintenanceOrder")
		mLineMaintenanceOrder.OrderDate = calOrderDate.Text
		txtText.Text = mLineMaintenanceOrder.Text
		txtText.DataBind()
		upnlOrderDetails.Update()
		Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

	End Sub
	'End

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	Private Sub MaintenanceSupportPlan_CheckedChanged() Handles chkMaintenanceSupportPlan.CheckedChanged

		Try

			If chkMaintenanceSupportPlan.Checked = True Then

				SetObject()
				SetVendorDetails()
				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"OpenMSPAssemblySelectionWindow",
													"OpenMSPAssemblySelectionWindow();",
													True)

			ElseIf chkMaintenanceSupportPlan.Checked = False Then

				mLineMaintenanceOrder.MSPID = Guid.Empty
				mLineMaintenanceOrder.ContractNO = String.Empty

				lblContractNO.DataBind()

				Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
				upnlSupplierDetails.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MSPAssemblySelection_Clicked(sender As Object, e As EventArgs) Handles MSPAssemblySelection.Click

		Try

			If mLineMaintenanceOrder.MSPID.Equals(Guid.Empty) And chkMaintenanceSupportPlan.Checked = True Then
				chkMaintenanceSupportPlan.Checked = False
			End If

			lblContractNO.DataBind()
			upnlSupplierDetails.Update()

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Private Sub HdnBtnFileAttachment(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

		mLineMaintenanceOrder.IsAttachmentAdded = True
		ControlAttachmentIconVisibility(mLineMaintenanceOrder.StatusID)
		upnlFileAttachmentButtons.Update()

	End Sub

	Private Sub RemoveAttachment(sender As Object, e As EventArgs) Handles btnRemoveAttach.Click

		Dim fileSize As Integer = 0
		Dim file(fileSize) As Byte

		Try

			If mLineMaintenanceOrder.IsAttachmentAdded And mFileAttach Is Nothing Then
				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintenanceOrder.ID)
			End If

			mFileAttach.ImageFile = file
			mFileAttach.Size = 0

			AttachmentIcon.Visible = False
			btnRemoveAttach.Enabled = False
			IsAttachmentDeleted = True
			mLineMaintenanceOrder.IsAttachmentAdded = False

			Session("IsAttachmentDeleted") = IsAttachmentDeleted

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ViewAttachment(sender As Object, e As ImageClickEventArgs) Handles AttachmentIcon.Click

		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString

		Try

			If mLineMaintenanceOrder.IsAttachmentAdded And mFileAttach Is Nothing Then

				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintenanceOrder.ID)

			End If

			If mFileAttach.Size > 0 Then

				Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
				Dim fs As FileStream

				If File.Exists(AppSettings("DOCPath")) = False Then

					'Delete File if exist
					File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
					' Create the file.
					fs = File.Create(path)
					'' Add some information to the file.
					fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
					fs.Close()

					Session("DOCPath") = path

					ScriptManager.RegisterStartupScript(Me,
															[GetType],
															"View Attachment",
															"viewAttachment();",
															True)
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AttachFile(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick

		Try

			If mLineMaintenanceOrder.IsAttachmentAdded Then

				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintenanceOrder.ID)

			Else

				mFileAttach = FileAttach.NewAttachment(ID:=Guid.NewGuid,
													   ReferenceID:=mLineMaintenanceOrder.ID)
			End If

			Session("mFileAttach") = mFileAttach

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Status "

	Private Sub Authorized_Click(sender As Object, e As EventArgs) Handles btnAuthorized.Click

		If IsValid Then

			SetVendorDetails()

			If mVendorList(mLineMaintenanceOrder.VendorID).NotInUse = True Then

				If CDate(mVendorList(mLineMaintenanceOrder.VendorID).NotInUseDate) <= CDate(mLineMaintenanceOrder.OrderDate) Then

					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
									MSGBox.Message_text.saveAlert,
									"Record can not be saved. <br><br> Supplier is not applicable since " +
									mVendorList(mLineMaintenanceOrder.VendorID).NotInUseDateFormatted +
									" <br><br> Select another Supplier from list or select date before " +
									mVendorList(mLineMaintenanceOrder.VendorID).NotInUseDateFormatted + " & try again",
									MsgBoxStyle.OkOnly,
									"")
					Exit Sub

				End If

			End If

			MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized,
							MSGBox.Message_text.StatusAuthorized,
							"<Strong> Service Order </Strong>",
							MsgBoxStyle.YesNo,
							"Status")

			Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

		End If

	End Sub

	Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

		If IsValid Then

			Dim IsInUse As IsInUse = IsInUse.GetIsInUseLineOrderInLineInvoice(mLineMaintenanceOrder.ID)

			If IsInUse.IsInUse Then

				MSGBoxCtrl.show(MSGBox.Message_title.Cancel,
								MSGBox.Message_text.Cancel,
								"<Strong> Service Order, It is used in Service Invoice .</Strong>",
								MsgBoxStyle.OkOnly,
								"StatusCancel")

				Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
				Exit Sub

			End If

			MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled,
							MSGBox.Message_text.StatusCanceled,
							"<Strong> Service Order </Strong>",
							MsgBoxStyle.YesNo,
							"StatusCancel")

			Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

		End If

	End Sub

#End Region

#Region " Show BrokenRules "

	Public Function CustomValidate1() As Boolean

		Dim strMsg As String = ""
		Dim mLineMaintenanceOrderItem As LineMaintenanceOrderItem

		SetObject()

		If mLineMaintenanceOrder.IsValid = False Then

			For i As Integer = 0 To mLineMaintenanceOrder.GetBrokenRulesCollection.Count - 1
				strMsg = strMsg + mLineMaintenanceOrder.GetBrokenRulesCollection(i).Description + "<Br>"
			Next

		End If

		If mLineMaintenanceOrder.LineMaintenanceOrderItems.IsValid = False Then

			For Each mLineMaintenanceOrderItem In mLineMaintenanceOrder.LineMaintenanceOrderItems

				For i As Integer = 0 To mLineMaintenanceOrderItem.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mLineMaintenanceOrderItem.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

			Next

		End If

		If strMsg.Trim <> "" Then

			cvVendor.ErrorMessage = strMsg
			cvVendor.IsValid = False
			Return False

		End If

		Return True

	End Function

#End Region

End Class