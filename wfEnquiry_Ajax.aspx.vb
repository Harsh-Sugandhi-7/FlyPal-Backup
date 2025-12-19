'***********************************
'AJAX Conversion By Vikrant On 30-Jun-2014
'***********************************


Imports System.Linq


Public Class wfEnquiry_Ajax
	Inherits Page


#Region " Enumaration "
	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		FindNow = 7
		Authorized = 8 'Added By Prashant 17-Aug-2011
	End Enum
	Private Enum RequestFor
		Supplier = 0
		Customer = 1
	End Enum

#End Region

#Region " Variable Declaration "

	Public Enquiry As Enquiry
	Public mVendorList As VendorList
	Public mStatusList As StatusList
	Public mEnquirySourceList As EnquirySourceList
	Public mCustomerList As VendorList    'Added Code By Girish on July,18,2007
	Public mVendorTerms As VendorTerms
	Public mVendors As Vendors
	Public mPriorityList As PriorityList
	Public mFAScsReportList As FAScsReportList
	Public mTransactionList As TransactionList  'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
	Private _reportHelper As New ReportHelper

	Dim Flag As Integer 'Kalpesh - 03-05-2007 ------
	Dim mModuleName As String
	Dim mTransTypeID As Integer
	Dim EventLogID As Guid 'Added By Utkarsh On 20-Jul-2011 For All19072011
	Dim mEnquiryDetail As String 'Added By Utkarsh On 20-Jul-2011 For All19072011
	Dim email As Thread


#End Region

#Region " Properties "

	Shared mTransID As Integer
	Shared mEnqDate As String

	Public Shared ReadOnly Property TransID As Integer
		Get
			Return mTransID
		End Get
	End Property

	Public Shared ReadOnly Property EnqDate As String
		Get
			Return mEnqDate
		End Get
	End Property

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		Enquiry = Session("mEnquiry")
		mVendorList = Session("mVendorList")
		mStatusList = Session("mStatusList")
		mEnquirySourceList = Session("mEnquirySourceList")
		mVendors = Session("mVendors")
		mCustomerList = Session("mCustomerList")   'Added Code By Girish on July,18,2007
		mTransTypeID = Session("mTransTypeID")
		mVendorTerms = Session("mVendorTerms")
		mModuleName = Session("mModuleName")
		mTransactionList = Session("TransactionList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

	End Sub

	Private Sub SetSession()

		Session("mEnquiry") = Enquiry
		Session("mVendorList") = mVendorList
		Session("mStatusList") = mStatusList
		Session("mEnquirySourceList") = mEnquirySourceList
		Session("mVendors") = mVendors
		Session("mCustomerList") = mCustomerList     'Added Code By Girish on July,18,2007
		Session("mVendorTerms") = mVendorTerms

	End Sub

	Private Sub SetObject()

		Try

			If Not IsDate(txtEnquiryDate.Text) Then
				Enquiry.Date = Today.Date
			Else
				Enquiry.Date = CDate(txtEnquiryDate.Text)
			End If

			''=============================WO - 2006-2007-1-22.doc
			Enquiry.Text = txtText.Text
			Enquiry.No = Val(txtNo.Text)
			Enquiry.UserName = User.Identity.Name
			Enquiry.VendorEnqNo = txtCustomerEnqNo.Text
			Enquiry.CustomerID = New Guid(cmbCustomer.SelectedValue)   'Added Code By Girish on July,18,2007

			If Not IsDate(txtCustomerEnqDate.Text) Then
				Enquiry.VendorEnqDate = DBNull.Value
			Else
				Enquiry.VendorEnqDate = CDate(txtCustomerEnqDate.Text)           'Added Code By Prashant on 09-Nov-2009
			End If

			Enquiry.OpeningLine = txtOpeningLine.Text
			Enquiry.IsCustomer = chkIsCustomer.Checked

			Dim txtValue As TextBox
			Dim mEnquiryItem As EnquiryItem
			Dim cmbValue As DropDownList
			Dim i As Integer = 0

			For Each mEnquiryItem In Enquiry.EnquiryItems

				With mEnquiryItem

					txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtQty"), TextBox)
					.Qty = CDec(Val(txtValue.Text))

					txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtRemark"), TextBox)
					.Remark = txtValue.Text

					txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtNote"), TextBox)
					.Note = txtValue.Text

					'Added Code By Girish on July 19,2007
					txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtReqinDays"), TextBox)
					If Not IsNumeric(Val(txtValue.Text)) Then
						.RequiredInDays = 0
					Else
						.RequiredInDays = Val(txtValue.Text)
					End If

					txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtIPCReference"), TextBox)
					.IPCReference = txtValue.Text
					'End of Added Code

					'=======Added by Saylee on 10-Oct-2007=============
					cmbValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("cmbPriority"), DropDownList)
					.PriorityID = CInt(cmbValue.SelectedValue)
					'==================================================

				End With

				i = i + 1

			Next

			Session("mEnquiry") = Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetComboDetails()

		Try

			Enquiry.VendorID = New Guid(cmbVendorList.SelectedValue.ToString)
			Enquiry.EnquirySourceID = Val(cmbSource.SelectedValue)
			Enquiry.CustomerID = New Guid(cmbCustomer.SelectedValue.ToString)   'Added Code By Girish on July,18,2007
			Session("mEnquiry") = Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteRecord(Index As Int32)

		Try

			MSGBoxCtrl.Show(MSGBox.Message_title.RemoveItem,
							MSGBox.Message_text.RemoveItem,
							"",
							MsgBoxStyle.YesNo,
							"Delete")

			Enquiry.EnquiryItems.CurrentIndex = Index
			Session("mEnquiry") = Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		Try

			If control.Enabled = False Or control.Visible = False Then Exit Sub
			control.Focus()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		MsgBoxResult = MSGBoxCtrl.Result

		Try

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Try

								Session("Sender") = ""
								Dim Enquiry As Enquiry
								Enquiry = CType(Session("mEnquiry"), Enquiry)
								Enquiry.EnquiryItems.Remove(Enquiry.EnquiryItems.CurrentItem)
								Session("mEnquiry") = Enquiry
								dgEnquiryItems.DataSource = Enquiry.EnquiryItems
								mPriorityList = PriorityList.GetPriorityList(, , "")

								dgEnquiryItems.DataBind()
								upnlEnquiryItem.Update()

							Catch ex As SqlException

								If ex.Number = 8145 Then

									MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
													MSGBox.Message_text.ProcedureError,
													ex.Procedure,
													MsgBoxStyle.OkOnly,
													"")

								ElseIf ex.Number = 2627 Then

									MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
													MSGBox.Message_text.Duplicate,
													ex.Procedure,
													MsgBoxStyle.OkOnly,
													"")

								ElseIf ex.Number = 547 Then

									MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete,
													MSGBox.Message_text.ReferenceDelete,
													ex.Procedure,
													MsgBoxStyle.OkOnly,
													"")

								End If

							End Try

						ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation

							Session("sender") = ""
							If Session("IsValid") Then
								Session.Remove("IsValid")
								DataFieldBind() ''Added New RAJNISH------After Clicking on YES or NO button of message box, date and other fields become refresh
								Save()
							Else
								Session.Remove("IsValid")
							End If
							'========================================WO - 2006-2007-1-17.doc

						ElseIf MSGBoxCtrl.Sender = "Status" Then

							Session("sender") = ""
							If Session("IsValid") Then
								Session.Remove("IsValid")
								DataFieldBind()
								Save()
							Else
								Session.Remove("IsValid")
							End If
							''==========================================

						End If

					Case MsgBoxResult.No

						If MSGBoxCtrl.Sender = "Close" Then

							Session.Remove("IsValid")
							Session("Sender") = ""
							Response.Redirect("Index.aspx")

						ElseIf MSGBoxCtrl.Sender = "Status" Then

							Session("Sender") = ""
							Session.Remove("IsValid")
							If Enquiry.StatusID = 2 Then
								Enquiry.StatusID = 1
							ElseIf Enquiry.StatusID = 4 Then
								Enquiry.StatusID = 2
							End If

							Session("mEnquiry") = Enquiry

						Else
							Session("Sender") = ""
						End If

					Case MsgBoxResult.Ok

						If MSGBoxCtrl.Sender = "Status" Then

							Session("sender") = ""
							''==========================================WO - 2006-2007-1-17.doc
							If Enquiry.StatusID = 2 Then
								Enquiry.StatusID = 1
							ElseIf Enquiry.StatusID = 4 Then
								Enquiry.StatusID = 2
							End If
							Session("mEnquiry") = Enquiry

						ElseIf MSGBoxCtrl.Sender = "BlankParts" Then

							Session("sender") = ""
							Session("mEnquiry") = Enquiry

							'Added by Utkarsh On 19-Nov-2013 For TransTextSeries
						ElseIf MSGBoxCtrl.Sender = "EnquiryTransTextSeriesAlert" Then
							Session("sender") = ""
							Session("AddTransTextSeries") = "True"
							Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
							'ENd
						Else
							Session("sender") = ""
							DataFieldBind()
						End If

				End Select

			ElseIf MsgBoxResult = -1 Then

				''====================================WO - 2006-2007-1-17.doc
				If Enquiry.StatusID = 2 And Session("sender") <> "Close" Then
					Enquiry.StatusID = 1
				ElseIf Enquiry.StatusID = 4 Then
					Enquiry.StatusID = 2
				End If
				Session("sender") = ""
				Session("mEnquiry") = Enquiry
				''======================================

			ElseIf MsgBoxResult = 0 And Session("sender") = "Authorization" Then
				Session("sender") = ""
				DataFieldBind()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetControlStatus(StatusId As Int16)

		Try

			btnAdd.Enabled = IIf(StatusId > 1, False, True)
			cmbAdd.Enabled = IIf(StatusId > 1, False, True)
			btnAddTerm.Enabled = IIf(StatusId > 1, False, True)
			btnAddSupplierSpecificTerms.Enabled = IIf(StatusId > 1, False, True)

			'Added By Utkarsh On 26-Jul-2011 For All19072011
			btnSave.Visible = IIf(StatusId > 1, False, True)
			dgEnquiryItems.Columns(13).Visible = IIf(StatusId > 1, False, True)
			dgEnquiryItems.Columns(7).Visible = IIf(Enquiry.TransTypeID = 32, True, False)
			dgEnquiryTerms.Columns(2).Visible = IIf(StatusId > 1, False, True)
			btnSuppliers.Enabled = IIf(StatusId > 1, False, True)
			dgEnqSupplierList.Columns(4).Visible = IIf(StatusId > 1, False, True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetTitle()   ' Rajnish On 18-12-2007 'Modified By Saylee on 26th-Dec-2007

		Try

			Dim mTransTypeList As TransactionList
			mTransTypeList = TransactionList.GetTransactionList()

			If Enquiry.IsNew Then
				lblTitle.Text = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " [ NEW ]"
			Else

				If Enquiry.No > 0 And Enquiry.TransTypeID = 1 Then
					lblTitle.Text = mTransTypeList.GetTransactionTypeName(Enquiry.TransTypeID).ToString + " [" & Enquiry.Text + "-" + CType(Enquiry.No, String) + "]"
				ElseIf (Enquiry.TransTypeID = 32) Or (Enquiry.TransTypeID = 34) Or (Enquiry.TransTypeID = 35) Then
					lblTitle.Text = mTransTypeList.GetTransactionTypeName(Enquiry.TransTypeID).ToString + " [" & Enquiry.Text + "-" + CType(Enquiry.No, String) + "]"
				End If

			End If

			mModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
			Session("mModuleName") = mModuleName

			If Enquiry.TransTypeID = 1 Then

				lblVendorDetail.InnerText = "Customer Details"
				btnAddSupplierSpecificTerms.Text = "Add Customer Specific Terms"
				btnAddSupplierSpecificTerms.ToolTip = "Click To Add Customer Specific Terms"

				If btnName.Enabled Then
					btnName.ToolTip = "Click to Add Customer "
				End If

			ElseIf (Enquiry.TransTypeID = 32) Or (Enquiry.TransTypeID = 34) Or (Enquiry.TransTypeID = 35) Then

				lblVendorDetail.InnerText = "Supplier Details"

				If btnName.Enabled Then
					btnName.ToolTip = "Click to add Supplier"
				End If

			End If

			btnSave.ToolTip = "Click to Save the " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
			btnAuthorized.ToolTip = "Click to Authorize the " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
			btnCancel.ToolTip = "Click to Cancel the " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString

			If btnPrint.Enabled Then
				btnPrint.ToolTip = "Click to Print the " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibility()

		Try

			''===========================WO - 2006-2007-1-17.doc and WO - 2006-2007-1-31.doc
			txtText.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
			txtNo.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
			cmbVendorList.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean) And Enquiry.EnquiryItems.Count = 0) Or (Enquiry.EnquiryItems.Count = 0)
			'End If
			cmbSource.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))

			If AppSettings("ClientCode") = "IND" Then  'Added By Prashant 12-Aug-2019 As per Points in mail
				txtEnquiryDate.Enabled = False
			Else
				txtEnquiryDate.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean) And Enquiry.EnquiryItems.Count = 0) Or (Enquiry.EnquiryItems.Count = 0) And (CType(Enquiry.TransTypeID, Trans) <> Trans.RequestingForQuotation)
			End If
			btnAuthorized.Visible = (Not Enquiry.EnquiryItems.Count = 0) And (Not Enquiry.IsNew) And (Enquiry.StatusID = 1)
			txtCustomerEnqNo.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
			txtCustomerEnqDate.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
			txtOpeningLine.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
			chkIsCustomer.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
			btnName.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
			cmbCustomer.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean) And (chkIsCustomer.Checked = True))

			Dim txtValue As TextBox
			Dim cmbValue As DropDownList

			For i As Integer = 0 To dgEnquiryItems.Rows.Count - 1

				txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtQty"), TextBox)

				If AppSettings("NewRequisition") = "True" Then  'Added by Vikrant For New Requisition
					txtValue.Enabled = (Enquiry.StatusID = 1 And Enquiry.EnquiryItems(i).RequisitionItemEnquiryItems.Count = 0)
				Else 'End
					txtValue.Enabled = (Enquiry.StatusID = 1 And Enquiry.EnquiryItems(i).EnquiryItemRequisitionItems.Count = 0)
				End If

				txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtRemark"), TextBox)
				txtValue.Enabled = CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean)
				txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtNote"), TextBox)
				txtValue.Enabled = CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean)

				'Code Added By Girish on July,19,2007
				txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtReqinDays"), TextBox)
				txtValue.Enabled = CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean)
				txtValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("txtIPCReference"), TextBox)
				txtValue.Enabled = CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean)

				'============Added By Saylee on 10-Oct-2007============
				cmbValue = CType(Me.dgEnquiryItems.Rows(i).FindControl("cmbPriority"), DropDownList)

				If (Enquiry IsNot Nothing AndAlso Enquiry.EnquiryItems.Item(i).EnquiryItemRequisitionItems.Count > 0) Then
					cmbValue.Enabled = False
				Else
					cmbValue.Enabled = (CType(IIf(Enquiry.StatusID >= 2, False, True), Boolean))
				End If

				'======================================================

			Next

			'Canceled Status
			btnCancel.Visible = (Not Enquiry.IsNew) And (Enquiry.StatusID = 2)
			'===========Added By Saylee on 10th-Sep-2007================
			lblCustomerEnqNo.Visible = IIf((CType(Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation) Or (CType(Enquiry.TransTypeID, Trans) = Trans.OverHaulRepairEnquiry) Or (CType(Enquiry.TransTypeID, Trans) = Trans.RentialLeaseEnquiry), False, True)
			lblCustomerEnqDate.Visible = IIf((CType(Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation) Or (CType(Enquiry.TransTypeID, Trans) = Trans.OverHaulRepairEnquiry) Or (CType(Enquiry.TransTypeID, Trans) = Trans.RentialLeaseEnquiry), False, True)
			txtCustomerEnqNo.Visible = IIf((CType(Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation) Or (CType(Enquiry.TransTypeID, Trans) = Trans.OverHaulRepairEnquiry) Or (CType(Enquiry.TransTypeID, Trans) = Trans.RentialLeaseEnquiry), False, True)
			txtCustomerEnqDate.Visible = IIf((CType(Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation) Or (CType(Enquiry.TransTypeID, Trans) = Trans.OverHaulRepairEnquiry) Or (CType(Enquiry.TransTypeID, Trans) = Trans.RentialLeaseEnquiry), False, True)
			'===========Added By Prashant on 24th-Sep-2007================
			chkIsCustomer.Visible = IIf((CType(Enquiry.TransTypeID, Trans) = Trans.Enquiry), False, True)
			lblCustomer.Visible = IIf((CType(Enquiry.TransTypeID, Trans) = Trans.Enquiry), False, True)
			cmbCustomer.Visible = IIf((CType(Enquiry.TransTypeID, Trans) = Trans.Enquiry), False, True)
			'==========================================================

			'Added By Prashant 17-Aug-2011
			If Not IsInRole(Rights.Authorized) Then

				btnAuthorized.Enabled = False
				btnAuthorized.ToolTip = "You are not authorized user "
				btnCancel.Enabled = False
				btnCancel.ToolTip = "You are not authorized user "

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetProperties()
		mTransID = Enquiry.TransTypeID
		mEnqDate = Enquiry.Date.ToString
	End Sub

	''Added On 4 April - RAJNISH (Save subroutine for minimizing the code)
	Private Sub Save()

		Try

			'Authentication
			If Enquiry.Date IsNot DBNull.Value Then

				Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
				If mCheck.WebAuthentication = True Then

					Dim mDays As Integer = 0
					mDays = mCheck.Number("Days")

					Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
					'---------------------------------
					If DateDiff(DateInterval.Day, CDate(Enquiry.Date), maxAllowableDate) < 0 Then

						MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Enquiry. <br> Enquiry Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
						DataFieldBind()
						Exit Sub

					End If

				End If

			End If

			'Authentication
			Dim EnquiryClone As Enquiry
			EnquiryClone = Enquiry.Clone

			Try

				If Not Enquiry.EnquiryItems.Count = 0 Then

					SetObject()
					SetComboDetails()

					'Added By Rajnish On 20-02-2008
					For i As Integer = 0 To Enquiry.EnquirySuppliers.Count - 1

						If Enquiry.EnquirySuppliers(i).VendorID.Equals(Enquiry.CustomerID) Then
							MSGBoxCtrl.Show("Save Alert!", "Record can not be saved. <br><br> Supplier & Customer are same. <br><br> Select another Customer from list.", "", MsgBoxStyle.OkOnly, "")
							Exit Sub
						End If

					Next

					For i As Integer = 0 To Enquiry.EnquirySuppliers.Count - 1

						'Added by Saylee on 24-Jul-2012
						If mVendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUse = True Then

							If CDate(mVendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUseDate) <= CDate(Enquiry.Date) Then
								MSGBoxCtrl.Show("Save Alert!", "Record can not be saved. <br><br> Supplier " + mVendorList(Enquiry.EnquirySuppliers(i).VendorID).Name + " is not applicable since " + mVendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUseDateFormatted + " & try again", "", MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If

						End If

					Next

					If Enquiry.IsCustomer = True Then

						If mVendorList(Enquiry.CustomerID).NotInUse = True Then

							If CDate(mVendorList(Enquiry.CustomerID).NotInUseDate) <= CDate(Enquiry.Date) Then
								MSGBoxCtrl.Show("Save Alert!", "Record can not be saved. <br><br> Customer is not applicable since " + mVendorList(Enquiry.CustomerID).NotInUseDateFormatted + " <br><br> Select another Customer from list or select date before " + mVendorList(Enquiry.VendorID).NotInUseDateFormatted + " & try again", "", MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If

						End If

					End If

					'*************************
					Session("mEnquiry") = Enquiry
					'=========
					Dim VendorNames As String = String.Empty

					If Session("Vendors") = "True" And Enquiry.IsNew Then 'And Enquiry.IsNew

						'*************************End of  'Added by Saylee on 24-Jul-2012
						Dim clnEnquiry1 As Enquiry = Enquiry.GetNextEnquiry(Enquiry)

						'Setting next Supplier ID
						'clnEnquiry1.VendorID = mVendor.ID Commented By Vikrant 
						'Added by Utkarsh ON 21-Nov-2013 FOr TransTextSeries
						'Check if Enquiry is blank then call TransTextSeries UI

						If (clnEnquiry1.IsNew) And (clnEnquiry1.Text = "") Then

							Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(clnEnquiry1.TransTypeID, clnEnquiry1.DateFormatted)

							If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Enquiry.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(clnEnquiry1.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(clnEnquiry1.TransTypeID).TransText = "")) Then

								Dim str = "<script language='javascript'>openModal('wfEnquiry_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

								Session("BackPagestr_ForTransSeries") = str
								Session("TransName_ForTransSeries") = "Enquiry"
								Session("TransTypeID_ForTransSeries") = clnEnquiry1.TransTypeID
								Session("TransDate_ForTransSeries") = clnEnquiry1.DateFormatted

								MSGBoxCtrl.Show("Enquiry Transaction Series", "System does not find Transaction Series for this Transaction. Click Ok to enter Transaction Series.", "", MsgBoxStyle.OkOnly, "EnquiryTransTextSeriesAlert")

								Exit Sub

							Else

								Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

								If mAutoRenewTransTextSeries.IsRenewed Then

									With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(clnEnquiry1.TransTypeID)
										clnEnquiry1.Text = .TransText
										clnEnquiry1.No = .StartingTransNo
									End With

								Else

									Dim str = "<script language='javascript'>openModal('wfEnquiry_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

									Session("BackPagestr_ForTransSeries") = str
									Session("TransName_ForTransSeries") = "Enquiry"
									Session("TransTypeID_ForTransSeries") = clnEnquiry1.TransTypeID
									Session("TransDate_ForTransSeries") = clnEnquiry1.DateFormatted

									MSGBoxCtrl.Show("Enquiry Transaction Series", "System does not find Transaction Series for this Transaction. Click Ok to enter Transaction Series.", "", MsgBoxStyle.OkOnly, "EnquiryTransTextSeriesAlert")

									Exit Sub

								End If

							End If

						End If

						'End

						'Saving Enquiry for multiple suppliers
						Session("Vendors") = "False"
						clnEnquiry1 = CType(clnEnquiry1.Save(), Enquiry)

						For i As Integer = 0 To clnEnquiry1.EnquirySuppliers.Count - 1

							If VendorNames = "" Then
								VendorNames = clnEnquiry1.EnquirySuppliers(i).VendorName
							Else
								VendorNames = VendorNames + "," + clnEnquiry1.EnquirySuppliers(i).VendorName
							End If

						Next

						'Changed By Utkarsh On 20-Jul-2011 For All19072011
						mEnquiryDetail = clnEnquiry1.EnquiryNo + ";" + " Dated : " + clnEnquiry1.DateFormatted + ";" + " from : " + VendorNames
						MarkLog(Action.Save, mModuleName, mEnquiryDetail, ErrorType.NoError, clnEnquiry1.ID, EventLogID)
						'End
						Enquiry = clnEnquiry1
						Session("mEnquiry") = Enquiry

					Else

						'Added by Utkarsh ON 21-Nov-2013 FOr TransTextSeries
						'Check if Enquiry is blank then call TransTextSeries UI

						If (Enquiry.IsNew) And (Enquiry.Text = "") Then

							Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(Enquiry.TransTypeID, Enquiry.DateFormatted)

							If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Enquiry.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Enquiry.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Enquiry.TransTypeID).TransText = "")) Then

								Dim str = "<script language='javascript'>openModal('wfEnquiry_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

								Session("BackPagestr_ForTransSeries") = str
								Session("TransName_ForTransSeries") = "Enquiry"
								Session("TransTypeID_ForTransSeries") = Enquiry.TransTypeID
								Session("TransDate_ForTransSeries") = Enquiry.DateFormatted

								MSGBoxCtrl.Show("Enquiry Transaction Series", "System does not find Transaction Series for this Transaction. Click Ok to enter Transaction Series.", "", MsgBoxStyle.OkOnly, "EnquiryTransTextSeriesAlert")

								Exit Sub

							Else

								Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

								If mAutoRenewTransTextSeries.IsRenewed Then

									With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Enquiry.TransTypeID)
										Enquiry.Text = .TransText
										Enquiry.No = .StartingTransNo
									End With

								Else

									Dim str = "<script language='javascript'>openModal('wfEnquiry_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

									Session("BackPagestr_ForTransSeries") = str
									Session("TransName_ForTransSeries") = "Enquiry"
									Session("TransTypeID_ForTransSeries") = Enquiry.TransTypeID
									Session("TransDate_ForTransSeries") = Enquiry.DateFormatted

									MSGBoxCtrl.Show("Enquiry Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "EnquiryTransTextSeriesAlert")

									Exit Sub

								End If

							End If

						End If

						Enquiry.Save()

						For i As Integer = 0 To Enquiry.EnquirySuppliers.Count - 1

							If VendorNames = "" Then
								VendorNames = Enquiry.EnquirySuppliers(i).VendorName
							Else
								VendorNames = VendorNames + "," + Enquiry.EnquirySuppliers(i).VendorName
							End If

						Next

						'Added By Utkarsh On 20-Jul-2011 For All19072011
						mEnquiryDetail = Enquiry.EnquiryNo + ";" + " Dated : " + Enquiry.DateFormatted + ";" + " from : " + VendorNames

						Select Case Enquiry.StatusID
							Case 1
								MarkLog(Action.Save, mModuleName, mEnquiryDetail, ErrorType.NoError, Enquiry.ID, EventLogID)
							Case 2
								MarkLog(Action.Authorize, mModuleName, mEnquiryDetail, ErrorType.NoError, Enquiry.ID, EventLogID)
							Case 3
								MarkLog(Action.Amend, mModuleName, mEnquiryDetail, ErrorType.NoError, Enquiry.ID, EventLogID)
							Case 4
								MarkLog(Action.Cancel, mModuleName, mEnquiryDetail, ErrorType.NoError, Enquiry.ID, EventLogID)
						End Select

					End If

					Enquiry.MarkClean()
					lblTitle.Text = "Enquiry ( Saved ...)"
					Session("mEnquiry") = Enquiry
					upnlEnquiryDetails.DataBind()
					upnlVendorDetails.DataBind()
					DataFieldBind()
					SetTitle()
					ControlVisibility()
					upnlTitle.Update()
					upnlActionBtn.Update()
					upnlEnquiryDetails.Update()
					upnlVendorDetails.Update()
					upnlStatus.Update()
					upnlEnquiryItem.Update()
					upnlEnquiryTerm.Update()

					MSGBoxCtrl.Show(MSGBox.Message_title.SavedSuccessFully,
									MSGBox.Message_text.SavedSuccessFully,
									"",
									MsgBoxStyle.OkOnly,
									"")


				Else

					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert,
									MSGBox.Message_text.saveAlert,
									"Enquiry Can not save without Item.",
									MsgBoxStyle.OkOnly,
									"")

				End If

			Catch ex As SqlException

				Session("EnquiryClone") = EnquiryClone

				If ex.Number = 8114 Or ex.Number = 8115 Then

					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow,
									MSGBox.Message_text.NumericOverFlow,
									" Rate or Qty or Conversion Factor. ",
									MsgBoxStyle.OkOnly,
									"")

				ElseIf ex.Number = 8145 Then

					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
									MSGBox.Message_text.ProcedureError,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")

				ElseIf ex.Number = 2627 Then

					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
									MSGBox.Message_text.Duplicate,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")

				ElseIf ex.Number = 547 Then

					If InStr(ex.Message, "CCtabRequisitionItemEnquiryBalQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabRequisitionItemEnquiryBalQty", CompareMethod.Text) Then

						MSGBoxCtrl.Show(MSGBox.Message_title.PendingQty,
										MSGBox.Message_text.PendingQty,
										"Enquiry Qty can not be greater than Requisition Qty.",
										MsgBoxStyle.OkOnly,
										"")

					ElseIf InStr(ex.Message, "FKtabEnquiryTermtabTerm", CompareMethod.Text) Then

						MSGBoxCtrl.Show("Term Deleted! ",
										"Term Not Available<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again",
										" ",
										MsgBoxStyle.OkOnly,
										"")
					Else

						MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete,
										MSGBox.Message_text.ReferenceDelete,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					End If

				End If

			Finally
				EnquiryClone = Nothing
			End Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	''====================================================

	Private Sub AddAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean

		Dim IsInRoleString As String = ""

		Try

			'Deciding IsInRole String to check Rights
			Select Case Enquiry.TransTypeID
				Case Trans.Enquiry
					IsInRoleString = "Enquiry"
				Case Trans.RequestingForQuotation
					IsInRoleString = "RequestingForQuotation"
				Case Trans.OverHaulRepairEnquiry
					IsInRoleString = "PurchaseEnquiryRepairOverHaul"
				Case Trans.RentialLeaseEnquiry
					IsInRoleString = "PurchaseEnquiryRentalLease"
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
				Case Rights.Authorized 'Authorized = 8 'Added By Prashant 17-Aug-2011
					Return User.IsInRole(IsInRoleString + "Authorized")
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function GetVendorStatus(TransTypeID As Integer, Type As RequestFor) As Boolean

		Try

			If Type = RequestFor.Supplier Then                                  ''Purchase Enquiry 

				Select Case CType(TransTypeID, Trans)
					Case Trans.RequestingForQuotation
						Return True
					Case Trans.OverHaulRepairEnquiry
						Return True
					Case Trans.RentialLeaseEnquiry
						Return True
					Case Else
						Return False
				End Select

			ElseIf Type = RequestFor.Customer Then                              'Sales Enquiry

				Select Case CType(TransTypeID, Trans)
					Case Trans.Enquiry
						Return True
					Case Else
						Return False
				End Select

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Sub SetReport(Optional ByMail As Boolean = False)

		Dim crystalReport As Engine.ReportClass
		Dim SuppliersCount As Integer = dgEnqSupplierList.Rows.Count
		Dim IsVendorDetailsRequired(SuppliersCount - 1) As Boolean
		Dim MailInfo As String = String.Empty

		Try

			For i As Integer = 0 To SuppliersCount - 1

				Dim chk As CheckBox
				chk = CType(dgEnqSupplierList.Rows(i).FindControl("chkSelect"), CheckBox)

				If chk.Checked Then
					IsVendorDetailsRequired(i) = True
				End If

			Next

			Dim Result As ReturnMessage = _reportHelper.GetRequestForQuotationDetailedReport(ByMail:=ByMail,
																							 EnquiryObject:=Enquiry,
																							 RequestFromAPI:=False,
																							 SuppliersCount:=SuppliersCount,
																							 IsVendorDetailsRequired:=IsVendorDetailsRequired)

			crystalReport = CType(Result.Result, Engine.ReportClass)

			Session("CrystalReport") = crystalReport

			If ByMail Then

				MailInfo = If(Split(Result.Message, " ")(0), "")

				SendMailFile.SendMailFile(rpt:=Session("CrystalReport"),
										  UserName:=Thread.CurrentPrincipal.Identity.Name,
										  Subject:=$"{If(Split(Result.Message, " ")(1), "")} Enquiry No:- {Enquiry.EnquiryNo}",
										  Text:=Enquiry.EnquiryNo,
										  Info:=MailInfo,
										  VendorEmailID:="",
										  ToMailID:=Session("ToSendMailIDs"),
										  CCMailID:=Session("CcSendMailIDs"),
										  ReportPath:="",
										  ReportByMail:=False,
										  Remark:=Session("SendMailRemark"),
										  ReportGeneratedBy:=Session("ReportGenratedBy"),
										  SmtpHost:=mTransactionList.Item(Trans.RequestingForQuotation).SmtpHost,
										  SmtpPort:=mTransactionList.Item(Trans.RequestingForQuotation).SmtpPort,
										  SmtpUser:=mTransactionList.Item(Trans.RequestingForQuotation).SmtpUser,
										  SmtpPassword:=mTransactionList.Item(Trans.RequestingForQuotation).SmtpPassword)
			Else

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"Display Report",
													"displayReportInPDF();",
													True)

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			mVendorList = VendorList.GetVendortList(0, , , , , ,
													True,
													GetVendorStatus(Enquiry.TransTypeID, RequestFor.Customer),
													GetVendorStatus(Enquiry.TransTypeID, RequestFor.Supplier))
			Session("mVendorList") = mVendorList
			cmbVendorList.DataSource = mVendorList

			mStatusList = StatusList.GetStatusList(Enquiry.StatusID, 0, True)
			Session("mStatusList") = mStatusList

			mEnquirySourceList = EnquirySourceList.GetEnquirySourceList()
			Session("mEnquirySourceList") = mEnquirySourceList
			cmbSource.DataSource = mEnquirySourceList

			mCustomerList = VendorList.GetVendortList(0, , , , , , True, True) 'Code Added By Girish on July,18,2007
			Session("mCustomerList") = mCustomerList
			cmbCustomer.DataSource = mCustomerList                               'Code Added By Girish on July,18,2007 

			'Code Added By Girish on July,18,2007 
			For Each mEnquirySupplier As EnquirySupplier In Enquiry.EnquirySuppliers

				Enquiry.EnquirySuppliers(mEnquirySupplier.ID).VendorName = mVendorList(mEnquirySupplier.VendorID).Name
				Enquiry.EnquirySuppliers(mEnquirySupplier.ID).ContactPerson = mVendorList(mEnquirySupplier.VendorID).ContactPerson

			Next

			dgEnquiryItems.DataSource = Enquiry.EnquiryItems
			dgEnquiryTerms.DataSource = Enquiry.EnquiryTerms
			dgEnqSupplierList.DataSource = Enquiry.EnquirySuppliers
			txtEnquiryDate.Text = CDate(Enquiry.Date).ToString(AppSettings("DateFormat"))
			txtCustomerEnqDate.Text = Enquiry.VendorEnqDateFormatted.ToString
			mPriorityList = PriorityList.GetPriorityList(, , "")

			DataBind()

			If Enquiry.TransTypeID <> Trans.RequestingForQuotation And
			   Enquiry.TransTypeID <> Trans.OverHaulRepairEnquiry And
			   Enquiry.TransTypeID <> Trans.RentialLeaseEnquiry Then

				If mVendorList.Contains(Enquiry.VendorID) Then
					cmbVendorList.SelectedValue = Enquiry.VendorID.ToString
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		Try

			Dim CustomValidator As CustomValidator
			CustomValidator = CType(s, CustomValidator)

			If CustomValidator.ControlToValidate = "cmbVendorList" Then

				If cmbVendorList.SelectedIndex <= 0 Then

					If Enquiry.TransTypeID = 1 Then
						CustomValidator.ErrorMessage = "Select Customer from the list"
					Else
						CustomValidator.ErrorMessage = "Select Supplier from the list"
					End If

					e.IsValid = False

				End If

			ElseIf CustomValidator.ControlToValidate = "cmbCustomer" Then

				If (cmbCustomer.SelectedIndex <= 0) And (chkIsCustomer.Checked = True) Then
					CustomValidator.ErrorMessage = "Select Customer from the list"
					e.IsValid = False
				End If

			End If

			If Flag = 1 Then Exit Sub

			Dim strMsg As String = ""
			SetObject()
			SetComboDetails()

			If Not Enquiry.IsValid Then

				For i As Integer = 0 To Enquiry.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + Enquiry.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

			End If

			Dim mEnquiryItem As EnquiryItem

			If Not Enquiry.EnquiryItems.IsValid Then

				For Each mEnquiryItem In Enquiry.EnquiryItems
					For i As Integer = 0 To mEnquiryItem.GetBrokenRulesCollection.Count - 1
						strMsg = strMsg + mEnquiryItem.ItemName + " : " + mEnquiryItem.GetBrokenRulesCollection(i).Description + "<Br>"
					Next
				Next

			End If

			If strMsg.Trim <> "" Then
				CustomValidator.ErrorMessage = strMsg
				e.IsValid = False
			End If

			Flag = 1

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 20-Jul-2011 For All19072011
			AddAttributes()
			SetControlStatus(Enquiry.StatusID)
			'Kalpesh - 03-05-2007 --------------------------------

			If CType(Session("AddParts"), String) = "True" Then
				'Add selected part(s) to Enquiry Items
				AddMultipleParts()
				Session("AddParts") = "False"
			Else
				Session("AddParts") = "False"
			End If

			If CType(Session("SelectVendors"), String) = "True" Then
				'Add selected part(s) to Enquiry Items
				SetFirstVendor()
				Session("SelectVendors") = "False"
			Else
				Session("SelectVendors") = "False"
			End If

			'added by Prashant 07/08/07
			If CType(Session("AddRequisitionParts"), String) = "True" Then
				'Add selected part(s) to Enquiry Items
				AddRequisitionParts()
				Session("AddRequisitionParts") = "False"
				Session("AddPart") = "False"
			Else
				Session("AddRequisitionParts") = "False"
				Session("AddPart") = "False"
			End If
			'-----------------------------------------------------

			If Not IsPostBack And Session("sender") = "" Then

				If AppSettings("AutoCompleteTransText") <> "True" Then 'Added by VIkrant For ALL23052012

					If txtText.Enabled = True Then
						SetFocus(txtText)
					End If

				End If

				'Added by Utkarsh on 21-Nov-2013 for Trans Text Series
				If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Session("TransText_ForTransSeries") IsNot Nothing) Then

					If Enquiry.IsNew Then

						Enquiry.Text = Session("TransText_ForTransSeries")
						txtText.Text = Enquiry.Text
						Session("mEnquiry") = Enquiry
						Session("AddTransTextSeries") = "False"
						Session.Remove("TransName_ForTransSeries")
						Session.Remove("TransText_ForTransSeries")
						Session.Remove("TransNo_ForTransSeries")

					End If

				End If
				'End

				DataFieldBind()
				If (AppSettings("NewRequisition") = "True" And ((CType(Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation) Or (CType(Enquiry.TransTypeID, Trans) = Trans.OverHaulRepairEnquiry))) Then  'Added by Vikrant For New Requisition
					cmbAdd.Items.Add("Add Requisition Items")
				Else 'End
					'======================Added By Saylee on 17-Sep-2007==================
					If ((CType(Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation)) Then '"Comment by RAJNISH" Or (CType(mEnquiry.TransTypeID, Trans) = Trans.RentialLeaseEnquiry) Or (CType(mEnquiry.TransTypeID, Trans) = Trans.OverHaulRepairEnquiry)) Then
						cmbAdd.Items.Add("Store Approved part List")
					End If
					'======================================================================
				End If

				SetTitle() ' Rajnish On 18-12-2007
				ControlVisibility()
				SetProperties()

				If cmbVendorList.Visible = True Then
					txtAddress.Text = mVendorList(cmbVendorList.SelectedIndex).Address
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Event Added By Girish on July 18,2007
	Private Sub AddCustomer(sender As Object, e As EventArgs) Handles btnName.Click

		Try

			Session.Remove("SearchIndex")
			Response.Redirect("wfVendorList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&BackPage1=wfEnquiry_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub IsCustomer_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsCustomer.CheckedChanged

		Try

			If chkIsCustomer.Checked = True Then
				cmbCustomer.Enabled = True
			Else
				cmbCustomer.Enabled = False
				cmbCustomer.SelectedIndex = 0
			End If

			If chkIsCustomer.Enabled = True Then
				SetFocus(chkIsCustomer)
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddEnquiryItems(sender As Object, e As EventArgs) Handles btnAdd.Click

		Try

			If cmbAdd.SelectedIndex = 0 Then

				If IsValid Then

					SetObject()
					SetComboDetails()
					Enquiry.EnquiryItems.Add(Enquiry.ID)
					Session("mEnquiry") = Enquiry
					Session("mVendors") = mVendors
					Response.Redirect("wfEnquiryItem_Ajax.aspx?BackPage=wfEnquiry_Ajax.aspx")

				Else
					upnlValidationSAummary.Update()
				End If

			End If

			If cmbAdd.SelectedIndex = 1 Then

				If IsValid Then

					SetComboDetails()
					SetObject()
					SetSession()
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"OpenWindow",
														"OpenPartsWindow('" + Enquiry.EnquiryItems.Count.ToString + "', '" + Enquiry.DateFormatted.ToString + "');", True)

				Else
					upnlValidationSAummary.Update()
				End If

			End If

			If cmbAdd.SelectedIndex = 2 Then

				If IsValid Then

					SetComboDetails()
					SetObject()
					SetSession()
					Dim str As String
					Session("TransDate") = Enquiry.Date.ToString
					Session("EnquiryItem") = Guid.Empty
					Session("ListFor") = 0

					If AppSettings("NewRequisition") = "True" Then  'Added by Vikrant For New Requisition

						ScriptManager.RegisterStartupScript(Me,
															[GetType],
															"OpenWindow",
															"OpenReqPartsWindow('" + Enquiry.EnquiryItems.Count.ToString + "', '" + Enquiry.DateFormatted.ToString + "');", True)
					Else 'End

						str = "openModal('wfStoreApprovalList.aspx?BackPage=wfEnquiry_Ajax.aspx&LookinTypeID=1 &Name=');"
						ScriptManager.RegisterStartupScript(Me,
															[GetType],
															"OpenScript",
															str,
															True)

					End If

				Else
					upnlValidationSAummary.Update()
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddTerms(sender As Object, e As EventArgs) Handles btnAddTerm.Click

		Try

			If IsValid Then

				SetObject()
				SetComboDetails()

				Session("mEnquiry") = Enquiry
				Session("mVendors") = mVendors

			Else
				upnlValidationSAummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_EnquiryItems_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgEnquiryItems.RowCommand

		Dim Index As Int32
		Try

			Select Case e.CommandName
				Case "EditRec"

					Index = CInt(e.CommandArgument)
					Session("Edit") = True
					SetObject()
					SetComboDetails()
					Enquiry.EnquiryItems.CurrentIndex = Index
					Session("mEnquiry") = Enquiry
					Session("mVendors") = mVendors
					Response.Redirect("wfEnquiryItem_Ajax.aspx?BackPage=wfEnquiry_Ajax.aspx")

				Case "DeleteRec"
					Index = CInt(e.CommandArgument)
					DeleteRecord(Index)
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_EnquiryTerms_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgEnquiryTerms.RowCommand

		Try

			Select Case e.CommandName
				Case "DeleteRec"

					Dim Index As Int32 = CInt(e.CommandArgument)
					Enquiry.EnquiryTerms.CurrentIndex = Index
					Enquiry.EnquiryTerms.Remove(Enquiry.EnquiryTerms.CurrentItem)
					Session("mEnquiry") = Enquiry
					Session("mVendors") = mVendors
					dgEnquiryTerms.DataSource = Enquiry.EnquiryTerms
					dgEnquiryTerms.DataBind()

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveEnquiry(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If (Not IsInRole(Rights.New) And Not IsInRole(Rights.Edit)) Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			If IsValid Then
				Save()
			Else
				upnlValidationSAummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnBack.Click

		Try

			'Added By Utkarsh On 26-Jul-2011 For All19072011
			MarkLog(Action.Close,
					mModuleName,
					"",
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)
			'End

			Session("IsValid") = IsValid

			If Enquiry.IsDirty Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm,
								MSGBox.Message_Text.Save,
								"",
								MsgBoxStyle.YesNo,
								"Close")

				If IsValid Then
					SetObject()
					SetComboDetails()
				Else
					upnlValidationSAummary.Update()
				End If

			Else
				Enquiry = Nothing
				Session.Remove("mVendors")
				Response.Redirect("Index.aspx")
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		Try

			If Not IsInRole(Rights.Print) Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			SetReport(False)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	''Kalpesh - 03-05-2007
	Private Sub AddSuppliers(sender As Object, e As EventArgs) Handles btnSuppliers.Click

		Try

			SetComboDetails()
			SetObject()
			SetSession()
			Session("mtmpVendors") = mVendors
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Open Script",
												"openModal('wfCommonVendorList_Ajax.aspx?BackPage=wfEnquiry_Ajax.aspx');",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_EnqSupplierList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgEnqSupplierList.RowCommand

		Try

			Select Case e.CommandName
				Case "DeleteRec"

					Dim Index As Int32 = CInt(e.CommandArgument)
					Enquiry.EnquirySuppliers.CurrentIndex = Index
					Enquiry.EnquirySuppliers.Remove(Enquiry.EnquirySuppliers.CurrentItem)
					Session("mEnquiry") = Enquiry
					Session("mVendors") = mVendors
					dgEnqSupplierList.DataSource = Enquiry.EnquirySuppliers
					dgEnqSupplierList.DataBind()

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Private Sub HdnBtnCommonPartList_Click(sender As Object, e As EventArgs) Handles hdnimgBtnCommonPartList.Click

		Try

			DataFieldBind()
			SetTitle() ' Rajnish On 18-12-2007
			ControlVisibility()
			SetProperties()

			upnlEnquiryItem.Update()
			upnlVendorDetails.Update()
			upnlEnquiryDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SendEmail(sender As Object, e As EventArgs) Handles btnSendMail.Click

		Try

			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
			Session("UserEmailID") = mTransactionList.Item(Trans.Enquiry).SendToMailID
			'---------------

			'Added by shital on 03-Dec-2019 for sending individual mail to suppliers
			Dim SupplierCnt As Integer = 0
			If AppSettings("ClientCode") = "Novo" Then

				For i As Integer = 0 To dgEnqSupplierList.Rows.Count - 1

					Dim chk As CheckBox

					chk = CType(dgEnqSupplierList.Rows(i).FindControl("chkSelect"), CheckBox)

					If chk.Checked Then

						SupplierCnt += 1
						Session("UserEmailID") = Enquiry.EnquirySuppliers(i).VendorMail

						If SupplierCnt >= 2 Then Exit For

					End If

				Next

			End If

			If SupplierCnt > 1 Then

				MSGBoxCtrl.Show("SendMail Alert!",
								"Mail can not be send for Multiple Suppliers",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If
			'---------------------------------

			Dim Str As String
			Str = "OpenMailWindow();"
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"OpenMailWindow",
												Str,
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnBtnSendMail(sender As Object, e As EventArgs) Handles hdnimgBtnSendMail.Click

		Try

			email = New Thread(Sub() SetReport(True)) With {
				.IsBackground = True
			}
			email.Start()

		Catch ex As Exception

			Dim Day, Month, Year As String
			Day = Format(Today.Date.Day, "0#")
			Month = Format(Today.Date.Month, "0#")
			Year = Format(Today.Date.Year, "0#")
			Dim TodayDate As String = Day & Month & Year
			Dim Path As String = AppSettings("DOCPath") & TodayDate
			FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
			WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
			FileClose(1)

		End Try

	End Sub

#End Region

#Region " Status "

	''================================ WO - 2006-2007-1-17.doc 
	Private Sub AuthorizeEnquiry(sender As Object, e As EventArgs) Handles btnAuthorized.Click

		Try

			If (Not IsInRole(Rights.Authorized)) Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			If IsValid Then

				Dim Child As EnquiryItem
				Dim strParts As String = ""
				Dim BlankPartsCount As Integer = 0

				If Enquiry.IsNew = False Then
					Enquiry = Enquiry.GetEnquiry(Enquiry.ID) 'To get Item ID
				End If

				For Each Child In Enquiry.EnquiryItems

					If Child.ItemID.Equals(Guid.Empty) Then
						BlankPartsCount = BlankPartsCount + 1
						strParts = strParts + BlankPartsCount.ToString & ")" & Child.ItemName & " (" & Child.ItemDescription & ")<Br>"
					End If

				Next

				If BlankPartsCount > 0 And strParts <> "" Then

					MSGBoxCtrl.Show("Following Part(s) needs to be added to Part Master -  <Br>", strParts, "", MsgBoxStyle.OkOnly, "BlankParts")
					Session("IsValid") = IsValid
					Session("mEnquiry") = Enquiry
					Exit Sub

				End If

				'Added by Saylee on 24-Jul-2012
				SetComboDetails()

				If mVendorList(Enquiry.VendorID).NotInUse = True Then

					If CDate(mVendorList(Enquiry.VendorID).NotInUseDate) <= CDate(Enquiry.Date) Then

						MSGBoxCtrl.Show("Save Alert!", "Record can not be saved. <br><br> Supplier " + mVendorList(Enquiry.VendorID).Name + " is not applicable since " + mVendorList(Enquiry.VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(Enquiry.VendorID).NotInUseDateFormatted + " & try again", "", MsgBoxStyle.OkOnly, "")
						Exit Sub

					End If

				End If

				If Enquiry.IsCustomer = True Then

					If mVendorList(Enquiry.CustomerID).NotInUse = True Then

						If CDate(mVendorList(Enquiry.CustomerID).NotInUseDate) <= CDate(Enquiry.Date) Then

							MSGBoxCtrl.Show("Save Alert!", "Record can not be saved. <br><br> Customer is not applicable since " + mVendorList(Enquiry.CustomerID).NotInUseDateFormatted + " <br><br> Select another Customer from list or select date before " + mVendorList(Enquiry.VendorID).NotInUseDateFormatted + " & try again", "", MsgBoxStyle.OkOnly, "")
							Exit Sub

						End If

					End If

				End If

				'*************************
				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusAuthorized,
								MSGBox.Message_Text.StatusAuthorized,
								"<Strong>Enquiry</Strong>",
								MsgBoxStyle.YesNo,
								"Status")

				Session("IsValid") = IsValid
				Enquiry.StatusID = 2
				Session("mEnquiry") = Enquiry

			Else
				upnlValidationSAummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	''================================ WO - 2006-2007-1-17.doc
	Private Sub CancelEnquiry(sender As Object, e As EventArgs) Handles btnCancel.Click

		Try

			If (Not IsInRole(Rights.Authorized)) Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			If IsValid Then

				Dim IsInUse As IsInUse = IsInUse.GetIsInUseEnquiryINQuotation(Enquiry.ID)

				If IsInUse.IsInUse Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.Cancel,
									MSGBox.Message_Text.Cancel,
									"<Strong>Enquiry, It is used in Quotation</Strong>",
									MsgBoxStyle.OkOnly,
									"Status")
					Enquiry.StatusID = 4
					Session("mEnquiry") = Enquiry
					Exit Sub

				End If

				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusCanceled,
								MSGBox.Message_Text.StatusCanceled,
								"<Strong>Enquiry</Strong>",
								MsgBoxStyle.YesNo, "Status")
				Session("IsValid") = IsValid
				Enquiry.StatusID = 4
				Session("mEnquiry") = Enquiry

			Else
				upnlValidationSAummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Add Multiple Parts "

	Private Sub AddMultipleParts()

		Dim mItem As Item
		Dim mItems As Items = Session("mItems")

		Try

			For Each mItem In mItems

				If mItem.IsSelected Then

					If Not Enquiry.EnquiryItems.Contains(mItem.ID) Then

						Enquiry.EnquiryItems.Add(Enquiry.ID)

						With Enquiry.EnquiryItems.CurrentItem
							.ItemID = mItem.ID
						End With

					Else
						' Added By Rajnish On 31-12-2007
						MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate,
										MSGBox.Message_Text.Duplicate,
										"Enquiry,Part already taken for Enquiry",
										MsgBoxStyle.OkOnly,
										"Close")

						DataFieldBind()

						Exit Sub

					End If

				End If

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetFirstVendor()

		Dim mVendor As Vendor
		mVendors = Session("mVendors")
		Try

			For Each mVendor In mVendors

				If mVendor.IsSelect Then

					Enquiry.VendorID = mVendor.ID
					Enquiry.KindAttention = mVendor.ContactPerson  'added by Prashant 27/10/07
					Exit Sub

				End If

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddRequisitionParts()

		Try

			If AppSettings("NewRequisition") = "True" Then 'Added by Vikrant for New Requisition

				Dim RequisitionItem As RequisitionItemNew
				Dim RequisitionItems As RequisitionItemsNew = Session("mRequisitionItemsNew")

				If RequisitionItems Is Nothing Then Exit Sub

				For Each RequisitionItem In RequisitionItems

					If RequisitionItem.IsSelect Then

						If Not Enquiry.EnquiryItems.Contains(RequisitionItemID:=RequisitionItem.ID, "") Then

							'Check is Part is present ?
							'If YES
							If Enquiry.EnquiryItems.Contains(RequisitionItem.ItemID) Then

								MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate,
												MSGBox.Message_Text.Duplicate,
												"Enquiry, Part " + RequisitionItem.PartNo + " already taken for Enquiry",
												MsgBoxStyle.OkOnly,
												"")

							Else

								Enquiry.EnquiryItems.Add(EnquiryID:=Enquiry.ID)

								With Enquiry.EnquiryItems.CurrentItem

									.ItemID = RequisitionItem.ItemID
									.ItemName = RequisitionItem.PartNo
									.ItemDescription = RequisitionItem.Description
									.IPCReference = RequisitionItem.IPCReference
									.PriorityID = RequisitionItem.PriorityID
									.RequisitionNumber = RequisitionItem.RequisitionNo
									.ModelID = RequisitionItem.ModelID
									.ModelName = RequisitionItem.ModelName
									.ReqItemUnitID = RequisitionItem.UnitID
									.ReqItemUnitName = RequisitionItem.Unit
									.Qty = RequisitionItem.EnquiryBalQty

									If Not .RequisitionItemEnquiryItems.Contains(RequisitionItemID:=RequisitionItem.ID) Then

										'if NOT then add
										.RequisitionItemEnquiryItems.Add(EnquiryItemID:= .ID,
																		 RequisitionItemID:=RequisitionItem.ID,
																		 Qty:=RequisitionItem.EnquiryBalQty,
																		 RequisitionNo:=RequisitionItem.RequisitionNo)

									Else

										'if YES fire Message
										MSGBoxCtrl.Show(MSGBox.Message_Title.ValidationAlert,
														MSGBox.Message_Text.ValidationAlert,
														"Requisition Item is already taken for Enquiry",
														MsgBoxStyle.OkOnly,
														"")
										Exit Sub

									End If

								End With

							End If

						End If

					End If

				Next

			Else

				Dim RequisitionItem As RequisitionItem
				Dim RequisitionItems As RequisitionItems = Session("mRequisitionItems")

				If RequisitionItems Is Nothing Then Exit Sub

				For Each RequisitionItem In RequisitionItems

					If RequisitionItem.IsSelect Then

						If Not Enquiry.EnquiryItems.Contains(RequisitionItemID:=RequisitionItem.ID, "") Then

							'Check is Part is present ?
							'If YES
							If Enquiry.EnquiryItems.Contains(RequisitionItem.ItemID) Then

								With Enquiry.EnquiryItems.Item(RequisitionItem.ItemID, "")

									'Check is Requisition Part is present ?
									If Not .EnquiryItemRequisitionItems.Contains(RequisitionItemID:=RequisitionItem.ID) Then

										'if NOT then add
										.EnquiryItemRequisitionItems.Add(EnquiryItemID:= .ID,
																		 RequisitionItemID:=RequisitionItem.ID,
																		 Qty:=RequisitionItem.EnquiryBalQty,
																		 RequisitionNo:=RequisitionItem.RequisitionNo)

									Else

										'if YES fire Message
										MSGBoxCtrl.Show(MSGBox.Message_Title.ValidationAlert,
														MSGBox.Message_Text.ValidationAlert,
														"Requisition item already taken for Enquiry",
														MsgBoxStyle.OkOnly,
														"")
										Exit Sub

									End If

								End With

							Else

								'If NOT
								Enquiry.EnquiryItems.Add(EnquiryID:=Enquiry.ID)

								With Enquiry.EnquiryItems.CurrentItem

									.ItemID = RequisitionItem.ItemID
									.ItemName = RequisitionItem.ItemName
									.ItemDescription = RequisitionItem.ItemDescription
									.IPCReference = RequisitionItem.IPCReference
									.PriorityID = RequisitionItem.PriorityID
									.Qty = RequisitionItem.EnquiryBalQty

									'Check is Requisition Part is present ?
									If Not .EnquiryItemRequisitionItems.Contains(RequisitionItemID:=RequisitionItem.ID) Then

										'if NOT then add
										.EnquiryItemRequisitionItems.Add(EnquiryItemID:= .ID,
																		 RequisitionItemID:=RequisitionItem.ID,
																		 Qty:=RequisitionItem.EnquiryBalQty,
																		 RequisitionNo:=RequisitionItem.RequisitionNo)

									Else

										'if YES fire Message
										MSGBoxCtrl.Show(MSGBox.Message_Title.ValidationAlert,
														MSGBox.Message_Text.ValidationAlert,
														"Requisition Item already taken for Enquiry",
														MsgBoxStyle.OkOnly,
														"")

										Exit Sub

									End If

								End With

							End If

						End If

					End If

				Next

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnBtnSupplier(sender As Object, e As EventArgs) Handles hdnimgBtnSupplier.Click

		Try

			dgEnqSupplierList.DataSource = Enquiry.EnquirySuppliers
			dgEnqSupplierList.DataBind()
			upnlVendorDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnBtnEnquiryTerm(sender As Object, e As EventArgs) Handles hdnimgBtnEnquiryTerm.Click

		Try

			DataFieldBind()
			SetTitle()
			ControlVisibility()
			SetProperties()

			upnlEnquiryTerm.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnBtnReqPartList(sender As Object, e As EventArgs) Handles hdnimgBtnReqPartList.Click

		Try

			DataFieldBind()
			SetTitle()
			ControlVisibility()
			SetProperties()

			upnlEnquiryItem.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub VendorListIndexChanged(sender As Object, e As EventArgs) Handles cmbVendorList.SelectedIndexChanged

		Try

			txtAddress.Text = mVendorList(cmbVendorList.SelectedIndex).Address
			If cmbVendorList.Enabled = True Then
				SetFocus(cmbVendorList)
			End If

			If cmbVendorList.SelectedIndex > 0 Then

				If Not Enquiry.EnquirySuppliers.Contains(New Guid(cmbVendorList.SelectedValue)) Then

					If Enquiry.EnquirySuppliers.Count > 0 Then
						Enquiry.EnquirySuppliers.RemoveAt(0)
					End If

					Dim mCustomerID As New Guid(cmbVendorList.SelectedValue)

					Enquiry.VendorID = mCustomerID
					Enquiry.EnquirySuppliers.Add(EnquiryID:=Enquiry.ID)
					Enquiry.EnquirySuppliers.CurrentItem.VendorID = mVendorList(mCustomerID).ID
					Enquiry.EnquirySuppliers.CurrentItem.VendorName = mVendorList(mCustomerID).Name
					Enquiry.EnquirySuppliers.CurrentItem.ContactPerson = mVendorList(mCustomerID).ContactPerson
					Enquiry.EnquirySuppliers.CurrentItem.VendorAddress = mVendorList(mCustomerID).Address
					Enquiry.EnquirySuppliers.CurrentItem.Phone = ""
					Enquiry.EnquirySuppliers.CurrentItem.VendorMail = ""

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub EnquiryDateChanged(sender As Object, e As EventArgs) Handles txtEnquiryDate.TextChanged

		Try

			SetComboDetails()

			If Not IsDate(txtEnquiryDate.Text) Then
				Enquiry.Date = Today.Date
			Else
				Enquiry.Date = CDate(txtEnquiryDate.Text)
			End If

			txtText.DataBind()
			txtNo.DataBind()
			SetObject()
			SetSession()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Show BrokenRules "

	Public Sub CustomValidation(s As Object, e As ServerValidateEventArgs)

		If Flag = 1 Then Exit Sub
		Try

			Dim CustValidator As CustomValidator
			CustValidator = CType(s, CustomValidator)
			Dim strMsg As String = ""

			SetObject()

			If Not Enquiry.IsValid Then
				For i As Integer = 0 To Enquiry.GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + Enquiry.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			End If

			Dim EnquiryItem As EnquiryItem

			If Not Enquiry.EnquiryItems.IsValid Then

				For Each EnquiryItem In Enquiry.EnquiryItems

					For i As Integer = 0 To EnquiryItem.GetBrokenRulesCollection.Count - 1
						strMsg = strMsg + EnquiryItem.ItemName + " : " + EnquiryItem.GetBrokenRulesCollection(i).Description + "<Br>"
					Next

				Next

			End If

			If strMsg.Trim <> "" Then
				CustValidator.ErrorMessage = strMsg
				e.IsValid = False
			End If

			Flag = 1

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Service Methods "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetTextList(prefixText As String, count As Integer, contextKey As String) As String()

		Dim DistinctTextList As DistinctTextListAutoComplete
		Try

			DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, , True, mTransID, mEnqDate)
			If count = 0 Then
				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
			Else
				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class